using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot
{
    internal class ControllerTelegramBot
    {
        private readonly TelegramBotClient _client;
        private readonly int _timeout;
        private int offset { get; set; }
        private event IExeptionLogger.ExceptionPusherCallback pushException;

        internal ControllerTelegramBot(TelegramBotClient client, IExeptionLogger exeptionLogger)
        {
            _client = client;
            _timeout = 1000;
            pushException = exeptionLogger.PushException;
        }

        private void UpdateHandling(Update update)
        {

        }

        private void ReadUpdates(Update[] updates)
        {
            foreach (var update in updates)
            {
                UpdateHandling(update);
                offset = update.Id + 1;
            }
        }

        internal async Task Start()
        {
            try
            {
                while (true)
                {
                    var updates = await _client.GetUpdatesAsync(offset, timeout: _timeout);
                    ReadUpdates(updates);
                }
            }
            catch (Exception ex)
            {
                ExceptionData exceptionData = new ExceptionData
                {
                    CurrentMethod = ex.TargetSite?.Name,
                    DateTime = DateTime.Now,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };
                pushException.Invoke(exceptionData);
            }
        }
    }
}
