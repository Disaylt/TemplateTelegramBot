using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot.Controller
{
    public class ImplementedActions : IImplementedActions
    {
        public Dictionary<string, IImplementedActions.ReturnNextActionAndUseAction> Actions { get; }

        public ImplementedActions()
        {
            Actions = new Dictionary<string, IImplementedActions.ReturnNextActionAndUseAction>();
            Actions.Add("/start", GetName);
        }

        private async Task<string> GetName(Update update, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(update.Message.Chat.Id, $"Привет {update.Message.Text}");
            return "/next";
        }
    }
}
