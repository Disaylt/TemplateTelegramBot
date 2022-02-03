using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.Log
{
    internal static class GeneralExceptionsPusher
    {
        internal static IExeptionLogger? ExceptionPusher { get; set; }
    }
}
