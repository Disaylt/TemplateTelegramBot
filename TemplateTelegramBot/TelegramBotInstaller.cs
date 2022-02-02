using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot
{
    public class TelegramBotInstaller
    {
        private readonly string _token;
        private readonly IExeptionLogger _exceptionPusher;
        private event IExeptionLogger.ExceptionPusherCallback pushException;

        public TelegramBotInstaller(string token, IExeptionLogger exeptionLogger, UsersStorageSettings? usersStorageSettings = null)
        {
            _token = token;
            _exceptionPusher = exeptionLogger;
            pushException = _exceptionPusher.PushException;
            ConnectToUsersStorage(exeptionLogger);
            if(usersStorageSettings != null)
            {
                UsersStorage.TypeMap = usersStorageSettings.UserTypeMap;
                UsersStorage.PathToDirectoryUsersStorage = usersStorageSettings.PathToDirectoryUsersStorage;
            }
        }

        private void ConnectToUsersStorage(IExeptionLogger exeptionLogger)
        {
            UsersStorage.ExceptionPusher = exeptionLogger;
            UsersStorage.PushException += UsersStorage.ExceptionPusher.PushException;
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
                    ControllerTelegramBot controllerTelegramBot = new(implementedCommand, implementedActions, standardActions, answerAll, client, _exceptionPusher);
                    await controllerTelegramBot.Start();

                }
                catch (Exception ex)
                {
                    ExceptionData exceptionData = new()
                    {
                        CurrentMethod = ex.TargetSite?.Name,
                        DateTime = DateTime.Now,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    };
                    pushException.Invoke(exceptionData);
                }
                finally
                {
                    Thread.Sleep(errorTimeout * 1000);
                }
            }
        }
    }
}
