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
        private readonly Dictionary<string, IImplementedActions.ReturnNextActionAndUseAction> _actions;
        private readonly IStandardActions _standardActions;
        private readonly int _timeout;
        private readonly bool _answerAll;
        private int _offset;
        private event IExeptionLogger.ExceptionPusherCallback pushException;

        internal ControllerTelegramBot(IImplementedCommands implementedCommand, IImplementedActions implementedActions, IStandardActions standardActions, bool answerAll, TelegramBotClient client, IExeptionLogger exeptionLogger)
        {
            _client = client;
            _timeout = 1000;
            _answerAll = answerAll;
            pushException = exeptionLogger.PushException;
            _commands = implementedCommand.Commands ?? new Dictionary<string, IImplementedCommands.CommandUse>();
            _actions = implementedActions.Actions ?? new Dictionary<string, IImplementedActions.ReturnNextActionAndUseAction>();
            _standardActions = standardActions;
        }

        private async Task UpdateHandling(Update update, long userId)
        {
            string messageText = update.Message?.Text ?? string.Empty;
            if(_commands.ContainsKey(messageText))
            {
                LastUsersActions.UpdateLastCommand(userId, messageText);
                await _commands[messageText].Invoke(update, _client);
            }
            else
            {
                string lastUserAction = LastUsersActions.GetLastCommand(userId);
                if(_actions.ContainsKey(lastUserAction))
                {
                    string nextAction = await _actions[lastUserAction].Invoke(update, _client);
                    LastUsersActions.UpdateLastCommand(userId, nextAction);
                }
                else
                {
                    await _standardActions.CommandNotFoundd(update, _client);
                }
            }
        }

        private async Task ReadUpdates(Update[] updates)
        {
            foreach (var update in updates)
            {
                long userId = update.Message?.Chat.Id ?? default;
                if (_answerAll || UsersStorage.UsersData.Find(x=> x.Id == userId) != null)
                {
                    await UpdateHandling(update, userId);
                }
                else
                {
                    await _standardActions.UserNotFound(update, _client);
                }
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
                    await ReadUpdates(updates);
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
