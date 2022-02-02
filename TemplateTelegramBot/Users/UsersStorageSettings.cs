using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.Users
{
    public class UsersStorageSettings
    {
        internal readonly string PathToDirectoryUsersStorage;
        internal readonly Dictionary<int, Type> UserTypeMap;

        public UsersStorageSettings(string pathToDirectoryUsersStorage, Dictionary<int, Type> userTypeMap)
        {
            PathToDirectoryUsersStorage = pathToDirectoryUsersStorage;
            UserTypeMap = userTypeMap;
        }
    }
}
