﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.Users
{
    public static class UsersStorage
    {
        private static string _fileName = "UsersStorage.json";
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
                LoadUsersData(value);
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
            else
            {
                ExceptionData exceptionData = new()
                {
                    CurrentMethod = nameof(LoadUsersData),
                    DateTime = DateTime.Now,
                    Message = $"Не найден файл по пути {pathToFile}, либо данные с пользователями пусты.",
                    StackTrace = string.Empty
                };
                if (PushException != null)
                {
                    PushException.Invoke(exceptionData);
                }
            }
        }

        private static void LoadUsersData(string pathToDirectoryUsersStorage)
        {
            string pathToFile = $@"{pathToDirectoryUsersStorage}\{_fileName}";
            if(SysFile.Exists(pathToFile) && TypeMap != null)
            {
                string content = SysFile.ReadAllText(pathToFile);
                try
                {
                    List<RootUser> usersData = new List<RootUser>();
                    var usersToken = JToken.Parse(content);
                    foreach(var userToken in usersToken)
                    {
                        int numUserType = userToken.Value<int>("UserType");
                        if(TypeMap.ContainsKey(numUserType))
                        {
                            RootUser? rootUser = userToken.ToObject(TypeMap[numUserType]) as RootUser;
                            if(rootUser != null)
                            {
                                usersData.Add(rootUser);
                            }
                        }
                    }
                    _usersData = usersData;
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
                    if(PushException != null)
                    {
                        PushException.Invoke(exceptionData);
                    }
                }
            }
            else
            {
                ExceptionData exceptionData = new()
                {
                    CurrentMethod = nameof(LoadUsersData),
                    DateTime = DateTime.Now,
                    Message = $"Не найден файл по пути {pathToFile}",
                    StackTrace = string.Empty
                };
                if (PushException != null)
                {
                    PushException.Invoke(exceptionData);
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
            RootUser? userData = UsersData
                .Where(x => x.Id == userId)
                .FirstOrDefault();
            return userData;
        }
    }
}
