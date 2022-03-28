using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TemplateTelegramBot
{
    public delegate void ExceptionPusherCallback(Exception exceptionData);
    public class TelegramBotInstaller
    {
        private readonly string _token;
        private event ExceptionPusherCallback pushException;

        public TelegramBotInstaller(string token, IExeptionLogger exeptionLogger)
        {
            _token = token;
            GeneralExceptionsPusher.ExceptionPusher = exeptionLogger;
            pushException = exeptionLogger.PushException;
        }

        public async Task Start(IImplementedCommands implementedCommand, IImplementedActions implementedActions, IStandardActions standardActions, bool answerAll = true, string? webhook = default, int errorTimeout = 120)
        {
            TelegramBotClient client;
            while (true)
            {
                try
                {
                    client = new TelegramBotClient(_token);
                    await client.SetWebhookAsync(webhook ?? string.Empty);
                    ControllerTelegramBot controllerTelegramBot = new(implementedCommand, implementedActions, standardActions, answerAll, client);
                    await controllerTelegramBot.Start();

                }
                catch (Exception ex)
                {
                    pushException.Invoke(ex);
                }
                finally
                {
                    Thread.Sleep(errorTimeout * 1000);
                }
            }
        }
    }
}
