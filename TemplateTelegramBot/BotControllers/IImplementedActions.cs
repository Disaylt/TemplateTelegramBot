using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.BotControllers
{
    public delegate Task<string> ReturnNextActionAndUseAction(Update update, TelegramBotClient client);
    public interface IImplementedActions
    {
        public Dictionary<string, ReturnNextActionAndUseAction>? Actions { get; }
    }
}
