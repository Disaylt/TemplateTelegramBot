using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.Log
{
    public class ExceptionData
    {
        public DateTime DateTime { get; set; }
        public string? Message { get; set; }
        public string? CurrentMethod { get; set; }
        public string? StackTrace { get; set; }
    }
}
