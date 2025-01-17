using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using WSM.Application.Interfaces;
using WSM.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

using WSM.Domain.Repositories;
using System.Reflection;
using System.Collections.Concurrent;
using static System.Net.Mime.MediaTypeNames;
using WSM.Application.Service;
using WSM.Application.DTOs;
using System.Xml.Linq;
using System;
using System.IO;



namespace WSM.Application.Services
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly TelegramBotClient _botClient;
        private readonly TelegramBotConfig _config;
        private readonly ILogger<TelegramBotService> _logger;
        private readonly IServiceProvider _serviceProvider;
        
        private static readonly ConcurrentDictionary<long, string> UserStates = new();//in-memory Dictionary for user-state
        public TelegramBotService(IOptions<TelegramBotConfig> config, ILogger<TelegramBotService> logger, IServiceProvider serviceProvider)
        {
            _config = config.Value;
            _botClient = new TelegramBotClient(_config.Token);
            _logger = logger;
            _serviceProvider = serviceProvider;
            

        }

        private async Task<string> GenerateQrcode(Update update)
        {
            UserStates[update.CallbackQuery.Message.Chat.Id] = update.CallbackQuery.Data;
            try
            {
                var endpointId = Guid.Parse(update.CallbackQuery.Data.Split('@')[1]);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var mikrotikEndpointServic = scope.ServiceProvider.GetRequiredService<IMikrotikEndpointService>();
                    var res = await mikrotikEndpointServic.GetMikrotikEndpointQrcode(endpointId);
                    if (res.Success)
                    {

                        var message = await _botClient.SendDocument(update.CallbackQuery.Message.Chat.Id,
                                document: InputFile.FromStream(res.Data, "qrcode.png"),
                                caption: "کد را اسکن کنید");
      
                    }
                    else
                    {
                        await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "QRCode could not be generated.⛔️");
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {
                await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "QRCode could not be generated.⛔️" + "\n" + ex.Message);
                return "";
            }
        }

        private async Task<string> CretaeNewMikrotikEndPoint(Update update)
        {
            try
            {
                if (!update.Message.Text.Contains("@"))
                {
                    return "عملیات با شکست روبرو شد⛔️." + "\n" + "لطفا فرمت نام را بصورت صحیح وارد کنید.";
                }
       
                var newEndpoint = new MikrotikEndpointCreateDto
                {
                    TelegramId = update.Message.Chat.Id,
                    MikrotikInterface = update.Message.Text.Split('@')[1],
                    Comment = update.Message.Text.Split('@')[0],
                    DaysToRenew = 30
                };
                using (var scope = _serviceProvider.CreateScope())
                {
                    var mikrotikEndpointServic = scope.ServiceProvider.GetRequiredService<IMikrotikEndpointService>();

                    string res;
                    var result = await mikrotikEndpointServic.CreateMikrotikEndpoint(newEndpoint);
                    if (result.Success)
                    {
                        res = "\u2705 اکانت با موفقیت ساخته شد" + "\n"+result.Data.AllowedAddress;
                    }
                    else
                    {
                        res = "\u26d4\ufe0f"+result.ErrorMessage;
                    }

                    return res;
                }
            }
            catch (Exception ex)
            {
                return "\u26d4\ufe0f" + ex.Message;
            }
        }
        private async Task  ShowEndpointDetail(long tgId, Guid endpointId)
        {
            _logger.LogInformation("enter ShowEndpoints");

            using (var scope = _serviceProvider.CreateScope())
            {
                var mikRepo = scope.ServiceProvider.GetRequiredService<IMikrotikEndpointRepository>();
                var myEndpoint = await mikRepo.GetMikrotikEndpointById(tgId, endpointId);
                if (myEndpoint == null)
                {
                    await _botClient.SendTextMessageAsync(tgId, "آیتمی برای شما وجود ندارد");
                }
                else
                {
                    string replyText = $"اکانت مشخصات : \n Interface: {myEndpoint.MikrotikInterface}"
                        + $"\n Ip:{myEndpoint.AllowedAddress}"
                        + $"\n Valid Date:{myEndpoint.EndDate}"
                        + $"\n name:{myEndpoint.Comment}"
                        + $"\n upload(GB) :0"
                        + $"\n download(GB) :0"

                        ;
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                                   {
                                           new []
                                    {
                                        InlineKeyboardButton.WithCallbackData("فعال", "option_111113"),
                                        InlineKeyboardButton.WithCallbackData("غیر فعال  \u274c", "option_2111"),
                                        InlineKeyboardButton.WithCallbackData("تمدید", "option_211")

                                    },
                                    new []
                                    {
                                        InlineKeyboardButton.WithCallbackData("QR Code  🔲", "QRCODE@"+myEndpoint.Id.ToString()),
                                        InlineKeyboardButton.WithCallbackData("دریافت فایل  \ud83d\udce9", "option_2")

                                    }
                                });
                    await _botClient.SendTextMessageAsync(tgId, replyText, replyMarkup: inlineKeyboard);

                }

            }
        }

        public async Task SetWebhookAsync(string url)
        {
            _logger.LogInformation("Setting webhook to {Url}", url);
            UpdateType[] ap = { UpdateType.Message, UpdateType.InlineQuery, UpdateType.ChosenInlineResult, UpdateType.CallbackQuery };
            await _botClient.SetWebhookAsync(url, allowedUpdates: ap);

        }

        public async Task HandleCallbackQueryAsync(Update update)
        {
            _logger.LogInformation("enter HandleCallbackQueryAsync");
            await _botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: $"You selected: {update.CallbackQuery.Data}");
            switch (update.CallbackQuery.Data)
            {
                case "NewAccount":
                    UserStates[update.CallbackQuery.Message.Chat.Id] = "AwaitingName";
                    await _botClient.SendTextMessageAsync(chatId: update.CallbackQuery.Message.Chat.Id, text: "Please send me the name for new account.");

                    break;
             


                default:
                    if (update.CallbackQuery.Data.StartsWith("QRCODE@"))
                    {
                        
                        GenerateQrcode(update);
                    }

                    else
                    {
                        await _botClient.SendTextMessageAsync(
                            chatId: update.CallbackQuery.Message.Chat.Id,
                            text: "Unknown option selected.");
                    }
                    break;
            }



        }

        public async Task HandleUpdateAsync(Update update)
        {
            _logger.LogInformation("enter HandleUpdateAsync");
            if (update.Message.Type==MessageType.Sticker)
            {
                _logger.LogInformation("sticker is recieved ");
            }
            if (update.Type == UpdateType.Message)
            {
                if (UserStates.TryGetValue(update.Message.Chat.Id, out var currentState) && currentState == "AwaitingName")
                {


                    var msg= await CretaeNewMikrotikEndPoint(update);

                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, msg);// or may is not created and say the error
                    //if could succesfully create new account then
                    UserStates.TryRemove(update.Message.Chat.Id, out _); // Reset state

                }
                if (update.Message.Text != null && update.Message.Text.StartsWith("WG.") && update.Message.ViaBot != null)
                {
                    ShowEndpointDetail(update.Message.Chat.Id, Guid.Parse(update.Message.Text.Split('.')[1]));
                }

                if (update.Message.Text == "/home")
                {
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                                 {
                                // First row
                                new []
                                {
                                    //InlineKeyboardButton.WithUrl("Visit Google", "https://www.google.com"),
                                    InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("نمایش اکانتهای وایرگارد من",""),
                                },
                                // Second row
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData(" درخواست اکانت جدید", "NewAccount"),
                                }
                         });
                    // await _botClient.SendTextMessageAsync(update.Message.Chat.Id, " لطقا گزینه ");
                    var str = $"✨ به shcanbot خوش آمدید! ✨" +
                        $"\r\n\nاز من میتوانید در موارد زیر استفاده کنی:" +
                        $"\r\n\n➡️ نمایش لیست اکانت های خود\r\n\n➡️ گرفتن استعلام مانده اعتبار هر اکانت\r\n\n➡️ دریافت QRCode هر اکانت" +
                        $"\r\n\nدر صورت بروز مشکل با  پشتیبان در تماس باشید" +
                        $"\r\n\n*@shcan1402*";
                    ;
                    //"*لطفا گزینه مورد نظر خورد انتخاب کنید*" + "\n توسط این بات شما امکان مدیریت اکانت های خود را دارید." + "\n در صورت بروز مشکل با آی دی پشتبان در تماس باشید" + "\n@shcan1402";
                    await _botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, text: str, replyMarkup: inlineKeyboard);
                    //string message = "Items:\n" + $"1. all.\n `/do_something`";


                    //await _botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, text: message);
                }
            }
        }
        public async Task SendMessage(long id, string message)
        {
            _logger.LogInformation("Handling update: {0} to {1}", id, message);
            if (id != null)
            {
                await _botClient.SendStickerAsync(id, sticker: "CAACAgIAAxkBAAIBAAFned0xTyVQHbEVyIXx5mURyTsCxgACpkMAAlvDKEoAAdLsOhPT_mM2BA");
                await _botClient.SendTextMessageAsync(id, message + "😊" + "\U0001F605");

            }
        }

        public async Task HandleInlineQueryAsync(InlineQuery inlineQuery)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mikRepo = scope.ServiceProvider.GetRequiredService<IMikrotikEndpointRepository>();

                //string query = inlineQuery.Query;
                List<InlineQueryResult> results = new List<InlineQueryResult>();

                var myEndpoints = await mikRepo.GetMikrotikEndpointByTelegramId(inlineQuery.From.Id, inlineQuery.Query);
                if (myEndpoints == null || !myEndpoints.Any())
                {
                    results.Add(new InlineQueryResultArticle(
                        id: "notexists",
                        title: "پیدا نشد!!",
                        inputMessageContent: new InputTextMessageContent("Not Found!!."))
                    {
                        Description = ""
                    });
                }
                else
                {
                    foreach (var endpoint in myEndpoints.Take(10))
                    {
                        var properties = endpoint.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        var serializedProperties = properties.Select(prop => $"{prop.Name}: {prop.GetValue(endpoint)?.ToString() ?? "null"}");
                        var serializedString = string.Join(", ", serializedProperties);


                        results.Add(new InlineQueryResultArticle(
                            id: endpoint.Id.ToString(),
                            title: endpoint.AllowedAddress ?? "No AllowedAddress",
                            inputMessageContent: new InputTextMessageContent($"WG.{endpoint.Id}"))// text which return in telegram chat
                                                                                                  //inputMessageContent: new InputTextMessageContent($"WG: {endpoint.MikrotikInterface ?? "No Name"}.{endpoint.AllowedAddress}"))// text which return in telegram chat
                        {
                            Description = (endpoint.Comment ?? "No Description") + "\n" + endpoint.MikrotikInterface
                            //,ThumbnailUrl = "https://example.com/endpoint.jpg" // Optional
                        });
                    }
                }

                InlineQueryResultsButton btn = new InlineQueryResultsButton { Text = "برگشت به بات", StartParameter = "start" };
                await _botClient.AnswerInlineQueryAsync(inlineQuery.Id, results, button: btn, isPersonal: true);
            }
        }

    }
}
