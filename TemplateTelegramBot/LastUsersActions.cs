using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot
{
    public static class LastUsersActions
    {
        private static Dictionary<long, string>? _lastUsersCommand;
        private static Dictionary<long, string> lastUsersCommand
        {
            get
            {
                if (_lastUsersCommand == null)
                {
                    _lastUsersCommand = new Dictionary<long, string>();
                }
                return _lastUsersCommand;
            }
        }

        public static void UpdateLastCommand(long id, string action)
        {
            if(lastUsersCommand.ContainsKey(id))
            {
                lastUsersCommand[id] = action;
            }
            else
            {
                lastUsersCommand.Add(id, action);
            }
        }

        public static string GetLastCommand(long id)
        {
            if (lastUsersCommand.ContainsKey(id))
            {
                return lastUsersCommand[id];
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
