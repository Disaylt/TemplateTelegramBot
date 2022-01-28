using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.BotControllers
{
    public interface IImplementedCommands
    {
        public delegate Task CommandUse(Update update, TelegramBotClient client);
        public Dictionary<string, CommandUse>? Commands { get; }
    }
}