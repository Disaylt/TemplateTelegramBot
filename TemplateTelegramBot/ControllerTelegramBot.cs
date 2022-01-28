using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot
{
    internal class ControllerTelegramBot
    {
        private readonly TelegramBotClient _client;
        private readonly Dictionary<string, IImplementedCommands.CommandUse> _commands;
        private readonly int _timeout;
        private readonly bool _answerAll;
        private int _offset;
        private event IExeptionLogger.ExceptionPusherCallback pushException;

        internal ControllerTelegramBot(IImplementedCommands commandUse, bool answerAll, TelegramBotClient client, IExeptionLogger exeptionLogger)
        {
            _client = client;
            _timeout = 1000;
            _answerAll = answerAll;
            pushException = exeptionLogger.PushException;
            _commands = commandUse.Commands ?? new Dictionary<string, IImplementedCommands.CommandUse>();
        }

        private void UpdateHandling(Update update, long userId)
        {
            string messageText = update.Message?.Text ?? string.Empty;
            long userId = update.Message?.Chat.Id ?? default;
            if(_commands.ContainsKey(messageText))
            {
                LastUsersCommands.UpdateLastCommand(userId, messageText);
                _commands[messageText].Invoke(update, _client);
            }
            else
            {

            }
        }

        private void ReadUpdates(Update[] updates)
        {
            foreach (var update in updates)
            {
                if(_answerAll || )
                UpdateHandling(update);
                _offset = update.Id + 1;
            }
        }

        internal async Task Start()
        {
            try
            {
                while (true)
                {
                    var updates = await _client.GetUpdatesAsync(_offset, timeout: _timeout);
                    ReadUpdates(updates);
                }
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
        }
    }
}
