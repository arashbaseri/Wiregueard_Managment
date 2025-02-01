using System;
using Microsoft.Extensions.Hosting;
using Renci.SshNet;
namespace WSM.Application.Service
{
    public class MikrotikSsh
    {
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        //public string Command { get; set; }
        public string Result { get; set; }
        public string Error { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsError { get; set; }
        public MikrotikSsh(string host, string user, string password, int port)
        {
            Host = host;
            User = user;
            Password = password;
            Port = port;

        }
        public string CreatePeer(string pblickey, string allowedIps)
        {
            using (var sshClient = new SshClient(Host, Port, User, Password))
            {
                try
                {
                    // Connect to the server
                    sshClient.Connect();

                    if (sshClient.IsConnected)
                    {
                        Console.WriteLine("Connected to the server.");
                        
                        // Execute the batch file
                        var command = sshClient.CreateCommand($"wg set wg0 peer {pblickey} allowed-ips {allowedIps}");
                       
                        // Execute the command
                        command.CommandTimeout = TimeSpan.FromSeconds(5);
                        var result = command.Execute();
                        Console.WriteLine($"Command output:\n{result}");
                        return result;
                    }
                    else
                    {
                        Console.WriteLine("Failed to connect to the linux server.");
                        return "Failed to connect to the linux server.";
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return ex.Message;
                }
                finally
                {
                    // Disconnect the SSH client
                    if (sshClient.IsConnected)
                    {
                        sshClient.Disconnect();
                    }

                }
            }
        }
    }
}
