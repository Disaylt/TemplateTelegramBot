using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TemplateTelegramBot
{
    public delegate void ExceptionPusherCallback(ExceptionData exceptionData);
    public class TelegramBotInstaller
    {
        private readonly string _token;
        private event ExceptionPusherCallback pushException;

        public TelegramBotInstaller(string token, IExeptionLogger exeptionLogger, UsersStorageSettings? usersStorageSettings = null)
        {
            _token = token;
            GeneralExceptionsPusher.ExceptionPusher = exeptionLogger;
            UsersStorage.PushException += GeneralExceptionsPusher.ExceptionPusher.PushException;
            pushException = GeneralExceptionsPusher.ExceptionPusher.PushException;
            if(usersStorageSettings != null)
            {
                UsersStorage.TypeMap = usersStorageSettings.UserTypeMap;
                UsersStorage.PathToDirectoryUsersStorage = usersStorageSettings.PathToDirectoryUsersStorage;
            }
        }

        public async Task Start(IImplementedCommands implementedCommand, IImplementedActions implementedActions, IStandardActions standardActions, bool answerAll = true, string? webhook = default, int errorTimeout = 120)
        {
            TelegramBotClient client;
            if(GeneralExceptionsPusher.ExceptionPusher != null)
            {
                implementedCommand.PushException += GeneralExceptionsPusher.ExceptionPusher.PushException;
                implementedActions.PushException += GeneralExceptionsPusher.ExceptionPusher.PushException;
                standardActions.PushException += GeneralExceptionsPusher.ExceptionPusher.PushException;
            }
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
