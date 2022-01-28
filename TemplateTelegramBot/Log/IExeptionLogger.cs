﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.Log
{
    public interface IExeptionLogger
    {
        public delegate void ExceptionPusherCallback(ExceptionData exceptionData);
        public void PushException(ExceptionData exceptionData);
    }
}