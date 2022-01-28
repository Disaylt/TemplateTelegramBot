using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot
{
    public static class LastUsersCommands
    {
        private static Dictionary<string, string>? _lastUsersCommand;
        public static Dictionary<string, string> LastUsersCommand
        {
            get
            {
                if (_lastUsersCommand == null)
                {
                    _lastUsersCommand = new Dictionary<string, string>();
                }
                return _lastUsersCommand;
            }
        }

        public static void UpdateLastCommand(string id, string action)
        {
            if(LastUsersCommand.ContainsKey(id))
            {
                LastUsersCommand[id] = action;
            }
            else
            {
                LastUsersCommand.Add(id, action);
            }
        }

    }
}
