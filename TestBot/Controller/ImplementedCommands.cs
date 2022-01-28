using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot.Controller
{
    internal class ImplementedCommands : IImplementedCommands
    {
        public Dictionary<string, IImplementedCommands.CommandUse>? Commands { get; }

        internal ImplementedCommands()
        {
            Commands = new Dictionary<string, IImplementedCommands.CommandUse>();
            Commands.Add("/start", StartBot);
        }

        private async Task StartBot(Update update, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(update.Message.Chat.Id, "Запущен");
        }
    }
}
