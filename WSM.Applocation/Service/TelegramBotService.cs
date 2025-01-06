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



namespace WSM.Application.Services
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly TelegramBotClient _botClient;
        private readonly TelegramBotConfig _config;
        private readonly ILogger<TelegramBotService> _logger;

        public TelegramBotService(IOptions<TelegramBotConfig> config, ILogger<TelegramBotService> logger)
        {
            _config = config.Value;
            _botClient = new TelegramBotClient(_config.Token);
            _logger = logger;


        }

        public async Task SetWebhookAsync(string url)
        {
            _logger.LogInformation("Setting webhook to {Url}", url);
            UpdateType[] ap = { UpdateType.Message, UpdateType.InlineQuery, UpdateType.ChosenInlineResult , UpdateType.CallbackQuery};
            await _botClient.SetWebhookAsync(url, allowedUpdates: ap);

        }

        public async Task HandleCallbackQueryAsync(Update update)
        {
            _logger.LogInformation("enter HandleCallbackQueryAsync");
            await _botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: $"You selected: {update.CallbackQuery.Data}");
            switch (update.CallbackQuery.Data)
            {
                case "option_1":
                    await _botClient.SendTextMessageAsync(
                        chatId: update.CallbackQuery.Message.Chat.Id,
                        text: "You chose Option 1!"
                    );
                    break;



                default:
                    await _botClient.SendTextMessageAsync(
                        chatId: update.CallbackQuery.Message.Chat.Id,
                        text: "Unknown option selected."
                    );
                    break;
            }



        }

        public async Task HandleUpdateAsync(Update update)
        {
            _logger.LogInformation("enter HandleUpdateAsync");
            if (update.Type == UpdateType.Message)
            {
                if (update.Message.Text == "This is an inline query result.")
                {
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                                {
                                    new []
                                    {
                                        InlineKeyboardButton.WithCallbackData("Option 1", "option_1"),
                                        InlineKeyboardButton.WithCallbackData("Option 2", "option_2")
                                    }
                                });
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "فعالیت مورد نظر شما چیست؟؟", replyMarkup: inlineKeyboard);
                }

                if (update.Message.Text == "/home")
                {
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                 {
                // First row
                new []
                {
                    //InlineKeyboardButton.WithUrl("Visit Google", "https://www.google.com"),
                    InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("نمایش اکانتهای وایرگارد من","fruit"),
                },
                // Second row
                new []
                {
                    InlineKeyboardButton.WithCallbackData("نمایش اکانتهای openvpn من", "button2_data"),
                },
                new []
                {
                 InlineKeyboardButton.WithSwitchInlineQuery("نمایش اکانتهای v2tay  من"),


                }
            });
                    // await _botClient.SendTextMessageAsync(update.Message.Chat.Id, " لطقا گزینه ");
                    var str = "*لطفا گزینه مورد نظر خورد انتخاب کنید*" + "\n توسط این بات شما امکان مدیریت اکانت های خود را دارید." + "\n در صورت بروز مشکل با آی دی پشتبان در تماس باشید" + "\n@shcan1402";
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
                await _botClient.SendTextMessageAsync(id, message+ "😊"+ "\U0001F605");
                
            }
        }

        public async Task HandleInlineQueryAsync(InlineQuery inlineQuery)
        {

            //var inlineQuery = update.InlineQuery;
            string query = inlineQuery.Query;

            List<InlineQueryResult> results = new List<InlineQueryResult>();

            if (query.StartsWith("fruit"))
            {
                results.Add(new InlineQueryResultArticle(
                    id: "apple",
                    title: "saaed2",
                       inputMessageContent: new InputTextMessageContent("This is an inline query result."))
                {
                    Description = "10.10.10.125/32",
                    ThumbnailUrl = "https://example.com/apple.jpg" //Optional
                    ,
                });

                results.Add(new InlineQueryResultArticle(
                    id: "banana",
                    title: "kian2",
                    inputMessageContent: new InputTextMessageContent("You chose a banana!"))
                {
                    Description = "10.10.10.125/32",
                    ThumbnailUrl = "https://example.com/banana.jpg" //Optional
                });
            }
            else if (query.StartsWith("color"))
            {
                results.Add(new InlineQueryResultArticle(
                    id: "red",
                    title: "Red",
                    inputMessageContent: new InputTextMessageContent("You chose red!"))
                {
                    Description = "A colordfgsdggfdgfdgfdgdfgdfgdfgdfgdfgdg\ndsfsdfsdfsdf\ndqwqwqw",
                    Url = "www.google.com",
                    HideUrl = false,
                    ThumbnailUrl = "https://example.com/red.jpg" //Optional
                });
            }
            if (results != null)
            {
                InlineQueryResultsButton btn = new InlineQueryResultsButton { Text = "برگشت به بات", StartParameter = "start" };
                await _botClient.AnswerInlineQueryAsync(inlineQuery.Id, results,
  button: btn
      );
            }

        }
    }
}
