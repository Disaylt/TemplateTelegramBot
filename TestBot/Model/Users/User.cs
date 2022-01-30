using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot.Model.Users
{
    internal class User : RootUser, IUserProperties
    {
        private static BotCommand[] _botCommands = new BotCommand[]
        {
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
