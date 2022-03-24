using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQliteCommandExecuter;
using Microsoft.Data.Sqlite;

namespace TemplateTelegramBot.UserStorage
{
    public class UserStorage
    {
        private const string _dbName = "UserStorage";
        private const string _usersTableName = "Users";
        private readonly SqlTemplateCommandExecuter _commandExecuter;
        private readonly UserStorageContext _userStorageContext;

        public UserStorage()
        {
            _commandExecuter = new SqlTemplateCommandExecuter(_dbName);
            _userStorageContext = new UserStorageContext(_dbName, _usersTableName);
            CreateDatabaseIfNotExcists();
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
            var parameter = new SqliteParameter("ChatId", chatId);
            var parameters = new List<SqliteParameter> { parameter };
            _commandExecuter.Delete(_usersTableName, parameters);
        }

        public virtual void AddUser(RootUser rootUser)
        {
            List<SqliteParameter> parameters = new List<SqliteParameter>
            {
                new SqliteParameter("UserName", rootUser.Name),
                new SqliteParameter("ChatId", rootUser.Id),
                new SqliteParameter("UserTypeId", rootUser.UserType)
            };
            _commandExecuter.Insert(_usersTableName, parameters);
        }

        public virtual RootUser GetUser(long chatId)
        {
            var rootUsers = _userStorageContext.RootUsers.
        }

    }
}
