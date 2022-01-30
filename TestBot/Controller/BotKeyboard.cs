using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot.Controller
{
    internal static class BotKeyboard
    {
        private static KeyboardButton[] _startKeyboadrButtons = new KeyboardButton[]
        {
            new KeyboardButton("WB"),
            new KeyboardButton("OZON")
        };
        private static ReplyKeyboardMarkup _startKeyboard = new ReplyKeyboardMarkup(_startKeyboadrButtons);

        public static ReplyKeyboardMarkup StartKeyboard
        {
            get { return _startKeyboard; }
        }
    }
}
