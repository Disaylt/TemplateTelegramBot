using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot
{
    public static class LastUsersActions
    {
        private static Dictionary<string, string>? _lastUsersAction;
        public static Dictionary<string, string> LastUsersAction
        {
            get
            {
                if (_lastUsersAction == null)
                {
                    _lastUsersAction = new Dictionary<string, string>();
                }
                return _lastUsersAction;
            }
        }

        public static void UpdateLastAction(string id, string action)
        {
            if(LastUsersAction.ContainsKey(id))
            {
                LastUsersAction[id] = action;
            }
            else
            {
                LastUsersAction.Add(id, action);
            }
        }

    }
}
