﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.BotControllers
{
    public interface IImplementedActions
    {
        public event ExceptionPusherCallback? PushException;
        public delegate Task<string> ReturnNextActionAndUseAction(Update update, TelegramBotClient client);
        public Dictionary<string, ReturnNextActionAndUseAction>? Actions { get; }
    }
}
