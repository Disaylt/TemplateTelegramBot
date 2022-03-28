using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQliteCommandExecuter;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TemplateTelegramBot.UserStorage
{
    public class UserStorageHandler
    {
        private const string _dbName = "UserStorage";
        private const string _usersTableName = "Users";
        private readonly SqlTemplateCommandExecuter _commandExecuter;
        private readonly UserStorageContext _userStorageContext;
        internal event ExceptionPusherCallback? PushException;

        public UserStorageHandler()
        {
            _commandExecuter = new SqlTemplateCommandExecuter(_dbName);
            _userStorageContext = new UserStorageContext(_dbName, _usersTableName);
            CreateDatabaseIfNotExcists();
        }

        public UserStorageHandler(ExceptionPusherCallback exceptionPusher) : this()
        {
            PushException = exceptionPusher;
        }

        private void CreateUsersTable()
        {
            string[] standardUsersTableColumns =
                {
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE",
                    "ChatId INTEGER NOT NULL",
                    "UserName TEXT",
                    "UserTypeId INTEGER"
                };
            _commandExecuter.CrateTable(_usersTableName, standardUsersTableColumns);
        }

        private void CreateDatabaseIfNotExcists()
        {
            string daFileName = $"{_dbName}.db";
            if(!SysFile.Exists(daFileName))
            {
                CreateUsersTable();
            }
        }

        public virtual void Delete(long chatId)
        {
            try
            {
                var parameter = new SqliteParameter("ChatId", chatId);
                var parameters = new List<SqliteParameter> { parameter };
                _commandExecuter.Delete(_usersTableName, parameters);
            }
            catch (Exception ex)
            {
                PushException?.Invoke(ex);
            }
        }

        public virtual void AddUser(RootUser rootUser)
        {
            try
            {
                List<SqliteParameter> parameters = new()
                {
                    new SqliteParameter("UserName", rootUser.UserName),
                    new SqliteParameter("ChatId", rootUser.ChatId),
                    new SqliteParameter("UserTypeId", rootUser.UserTypeId)
                };
                _commandExecuter.Insert(_usersTableName, parameters);
            }
            catch (Exception ex)
            {
                PushException?.Invoke(ex);
            }
        }

        public virtual RootUser? GetUser(long chatId)
        {
            try
            {
                string[] whereParameters = { "ChatId" };
                string command = SqlCommandTextCreator.GetSelectCommand(_usersTableName, whereParameters);
                SqliteParameter sqliteParameter = new("@ChatId", chatId);
                var rootUsers = _userStorageContext.RootUsers?.FromSqlRaw(command, sqliteParameter).FirstOrDefault();
                return rootUsers;
            }
            catch (Exception ex)
            {
                PushException?.Invoke(ex);
                return null;
            }
        }

        public virtual List<RootUser>? GetUsers()
        {
            try
            {
                string command = SqlCommandTextCreator.GetSelectCommand(_usersTableName);
                var rootUsers = _userStorageContext.RootUsers?.FromSqlRaw(command).ToList();
                return rootUsers;
            }
            catch (Exception ex)
            {
                PushException?.Invoke(ex);
                return null;
            }
        }
    }
}
