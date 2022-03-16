using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.Users
{
    public static class UsersStorage
    {
        private const string _fileName = "UsersStorage.json";
        private static string? _pathToDirectoryUsersStorage;
        private static List<RootUser>? _usersData;

        internal static event ExceptionPusherCallback? PushException;
        internal static Dictionary<int, Type>? TypeMap { get; set; }
        internal static string PathToDirectoryUsersStorage { 
            get 
            { 
                if (_pathToDirectoryUsersStorage == null)
                {
                    _pathToDirectoryUsersStorage = string.Empty;
                }
                return _pathToDirectoryUsersStorage; 
            }
            set 
            {
                LoadUsersStorage(value);
                _pathToDirectoryUsersStorage = value;
            } 
        }

        public static List<RootUser>? UsersData
        {
            get
            {
                return _usersData;
            }
        }

        private static void SaveFile()
        {
            string pathToFile = $@"{_pathToDirectoryUsersStorage}\{_fileName}";
            if(SysFile.Exists(pathToFile))
            {
                string content = JsonConvert.SerializeObject(UsersData);
                SysFile.WriteAllText(pathToFile, content);
            }
        }

        private static List<RootUser> LoadUsers(JToken usersToken)
        {
            List<RootUser> usersData = new();
            foreach (var userToken in usersToken)
            {
                int numUserType = userToken.Value<int>("UserType");
                if (TypeMap != null && TypeMap.ContainsKey(numUserType))
                {
                    if (userToken.ToObject(TypeMap[numUserType]) is RootUser rootUser)
                    {
                        usersData.Add(rootUser);
                    }
                }
            }
            return usersData;
        }

        private static void LoadUsersStorage(string pathToDirectoryUsersStorage)
        {
            string pathToFile = $@"{pathToDirectoryUsersStorage}\{_fileName}";
            if(SysFile.Exists(pathToFile))
            {
                string content = SysFile.ReadAllText(pathToFile);
                try
                {
                    var usersToken = JToken.Parse(content);
                    var userStorage = LoadUsers(usersToken);
                    _usersData = userStorage;
                }
                catch (Exception ex)
                {
                    if(PushException != null)
                    {
                        PushException.Invoke(ex);
                    }
                }
            }
        }

        public static void DeleteUser(long userId)
        {
            if(UsersData != null)
            {
                UsersData.RemoveAll(x => x.Id == userId);
                SaveFile();
            }
        }

        public static void AddUser(RootUser user)
        {
            if (UsersData != null)
            {
                UsersData.Add(user);
                SaveFile();
            }
        }

        public static void ReplaceUserDontSave(RootUser user)
        {
            if (UsersData != null)
            {
                UsersData.RemoveAll(x => x.Id == user.Id);
                UsersData.Add(user);
            }
        }

        public static void ReplaceUser(RootUser user)
        {
            if (UsersData != null)
            {
                UsersData.RemoveAll(x => x.Id == user.Id);
                UsersData.Add(user);
                SaveFile();
            }
        }

        public static RootUser? GetUserData(long userId)
        {
            RootUser? userData = UsersData?
                .Where(x => x.Id == userId)
                .FirstOrDefault();
            return userData;
        }
    }
}
