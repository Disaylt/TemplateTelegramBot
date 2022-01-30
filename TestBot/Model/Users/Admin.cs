using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot.Model.Users
{
    internal class Admin : RootUser, IUserProperties
    {
        private static BotCommand[] _botCommands = new BotCommand[]
        {
            new BotCommand() { Command = "/add_user", Description = "Добавить пользователя"},
            new BotCommand() { Command = "/del_user", Description = "Удалить пользователя"},
            new BotCommand() { Command = "/get_excel", Description = "Получить Excel"}
        };

        public override long Id { get; set; }
        public override int UserType { get; set; }
        public int SelectMarketplace { get; set; }
        public string? SelectId { get; set; }
        public static BotCommand[] BotCommands
        {
            get { return _botCommands; }
        }
    }
}
