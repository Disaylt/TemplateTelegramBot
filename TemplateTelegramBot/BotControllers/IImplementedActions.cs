using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.BotControllers
{
    internal interface IImplementedActions
    {
        public delegate Task<string> ReturnNextActionAndUseAction(Update update, TelegramBotClient client);
        public Dictionary<string, ReturnNextActionAndUseAction>? Commands { get; }
    }
}
