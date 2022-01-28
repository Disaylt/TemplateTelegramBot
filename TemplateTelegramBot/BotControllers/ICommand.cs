using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.BotControllers
{
    public interface ICommand
    {
        public delegate Task UseCommand(Update update, TelegramBotClient client);
        public Dictionary<string, UseCommand> UseComands { get; }
    }
}
