using System;
using System.Collections.Generic;
using System.Globalization;
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
            Actions.Add("OZON", SetArticle);
            Actions.Add("WB", SetArticle);
            Actions.Add("/get_roi", SendROI);
            Actions.Add("/del_user", DelUser);
            Actions.Add("/add_user", AddUser);
        }

        public event ExceptionPusherCallback? PushException;

        private async Task<string> SetArticle(Update update, TelegramBotClient client)
        {
            try
            {
                long userId = update?.Message?.Chat.Id ?? -1;
                IUserProperties? userData = UsersStorage.GetUserData(userId) as IUserProperties;
                string? messageText = update?.Message?.Text;
                userData.SelectId = messageText;
                RootUser rootUser = userData as RootUser;
                UsersStorage.ReplaceUserDontSave(rootUser);
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите цену");
                return "/get_roi";
            }
            catch
            {
                return "/start";
            }
        }

        private async Task<string> SendROI(Update update, TelegramBotClient client)
        {
            try
            {
                long userId = update?.Message?.Chat.Id ?? -1;
                string? price = update?.Message?.Text?.Replace('.', ',');
                IUserProperties? userData = UsersStorage.GetUserData(userId) as IUserProperties;
                string? store = MarketplaceController.GetMarketplace(userData.SelectMarketplace);
                string? article = userData.SelectId.ToString();
                if (userId != -1 && double.TryParse(price, out double resultPrice))
                {
                    double? roi = ExcelBuilder.GetROI(article, resultPrice, store);
                    if(roi != null)
                    {
                        await client.SendTextMessageAsync(update.Message.Chat.Id, $"ROI: {Math.Round(roi.Value, 2)}%");
                    }
                    else
                    {
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Не смог произвести расчеты.");

                    }
                }
                else
                {
                    await client.SendTextMessageAsync(update.Message.Chat.Id, "Ошибка");
                }
                return "/get_roi";
            }
            catch
            {
                return "/start";
            }
        }

        private async Task<string> DelUser(Update update, TelegramBotClient client)
        {
            string userId = update?.Message?.Text ?? string.Empty;
            if (long.TryParse(userId, out long resultId))
            {
                UsersStorage.DeleteUser(resultId);
                await client.SendTextMessageAsync(update.Message.Chat.Id, $"Пользователь {resultId} удаален.");

            }
            else
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Ошибка");
            }
            return "/start";
        }

        private async Task<string> AddUser(Update update, TelegramBotClient client)
        {
            string userId = update?.Message?.Text ?? string.Empty;
            if(long.TryParse(userId, out long resultId))
            {
                Model.Users.User user = new()
                {
                    Id = resultId,
                    UserType = (int)UserTypes.Types.User
                };
                UsersStorage.AddUser(user);
                await client.SendTextMessageAsync(update.Message.Chat.Id, $"Пользователь {resultId} добавлен.");

            }
            else
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Ошибка");
            }
            return "/start";
        }

    }
}
