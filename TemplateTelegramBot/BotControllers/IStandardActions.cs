using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.BotControllers
{
    public interface IStandardActions
    {
        public Task UserNotFound(Update update, TelegramBotClient client);
        public Task CommandNotFoundd(Update update, TelegramBotClient client);
    }
}
