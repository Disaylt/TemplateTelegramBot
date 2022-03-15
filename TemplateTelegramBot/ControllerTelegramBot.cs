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
        private readonly Dictionary<string, CommandUse> _commands;
        private readonly Dictionary<string, ReturnNextActionAndUseAction> _actions;
        private readonly IStandardActions _standardActions;
        private readonly int _timeout;
        private readonly bool _answerAll;
        private int _offset;
        private event ExceptionPusherCallback? pushException;

        internal ControllerTelegramBot(IImplementedCommands implementedCommand, IImplementedActions implementedActions, IStandardActions standardActions, bool answerAll, TelegramBotClient client)
        {
            _client = client;
            _timeout = 1000;
            _answerAll = answerAll;
            _commands = implementedCommand.Commands ?? new Dictionary<string, CommandUse>();
            _actions = implementedActions.Actions ?? new Dictionary<string, ReturnNextActionAndUseAction>();
            _standardActions = standardActions;
            if(GeneralExceptionsPusher.ExceptionPusher != null)
            {
                pushException = GeneralExceptionsPusher.ExceptionPusher.PushException;
            }
        }

        private async Task UpdateHandling(Update update, long userId)
        {
            string messageText = update.Message?.Text ?? string.Empty;
            if(_commands.ContainsKey(messageText))
            {
                LastUsersActions.UpdateLastAction(userId, messageText);
                await _commands[messageText].Invoke(update, _client);
            }
            else
            {
                string lastUserAction = LastUsersActions.GetLastAction(userId);
                if(_actions.ContainsKey(lastUserAction))
                {
                    string nextAction = await _actions[lastUserAction].Invoke(update, _client);
                    LastUsersActions.UpdateLastAction(userId, nextAction);
                }
                else
                {
                    await _standardActions.CommandNotFound(update, _client);
                }
            }
        }

        private async Task ReadUpdates(Update[] updates)
        {
            foreach (var update in updates)
            {
                long userId = update.Message?.Chat.Id ?? default;
                if (_answerAll || UsersStorage.UsersData?.Find(x=> x.Id == userId) != null)
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
                pushException?.Invoke(ex);
            }
        }
    }
}
