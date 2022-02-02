﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot.Controller
{
    internal class StandardAction : IStandardActions
    {
        public event ExceptionPusherCallback? PushException;

        public async Task CommandNotFound(Update update, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(update.Message.Chat.Id, "Неизвестная команда");
        }

        public async Task UserNotFound(Update update, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(update.Message.Chat.Id, "Необходима регистрация");
        }
    }
}
