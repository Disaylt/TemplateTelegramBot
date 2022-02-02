using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;

namespace TestBot.Controller
{
    internal class ImplementedCommands : IImplementedCommands
    {
        private string _accessDeniedMessage = "Отказанно в доступе";

        public event ExceptionPusherCallback? PushException;

        public Dictionary<string, IImplementedCommands.CommandUse> Commands { get; }

        internal ImplementedCommands()
        {
            Commands = new Dictionary<string, IImplementedCommands.CommandUse>();
            Commands.Add("/start", StartBot);
            Commands.Add("OZON", PressingButtonMarketplace);
            Commands.Add("WB", PressingButtonMarketplace);
            Commands.Add("/add_user", AddOrDeleteUser);
            Commands.Add("/del_user", AddOrDeleteUser);
            Commands.Add("/get_excel", GetExcel);
        }

        private async Task StartBot(Update update, TelegramBotClient client)
        {
            long userId = update?.Message?.Chat.Id ?? -1;
            Chat? chat = update?.Message?.Chat;
            RootUser? rootUser = UsersStorage.GetUserData(userId);
            if(rootUser != null)
            {
                var keyboard = Keyboard.StartKeyboard;
                switch (rootUser.UserType)
                {
                    case (int)UserTypes.Types.User:
                        await client.SetMyCommandsAsync(Model.Users.User.BotCommands, BotCommandScope.Chat(chat));
                        break;
                    case (int)UserTypes.Types.Admin:
                        await client.SetMyCommandsAsync(Admin.BotCommands, BotCommandScope.Chat(chat));
                        break;
                }
                await client.SendTextMessageAsync(chat.Id, "Привет, для просмотра доступных команд введите '/'", replyMarkup: keyboard);
            }
            else
            {
                await client.SendTextMessageAsync(chat.Id, _accessDeniedMessage);
            }
        }

        private async Task PressingButtonMarketplace(Update update, TelegramBotClient client)
        {
            long userId = update?.Message?.Chat.Id ?? -1;
            IUserProperties? userData = UsersStorage.GetUserData(userId) as IUserProperties;
            string messageText = update?.Message?.Text ?? string.Empty;
            int marketplace = -1;
            switch(messageText)
            {
                case "OZON":
                    marketplace = (int)MarketplaceController.MarketplaceTypes.Ozon;
                    break;
                case "WB":
                    marketplace = (int)MarketplaceController.MarketplaceTypes.WB;
                    break;
            }
            if(marketplace != -1 && userData != null)
            {
                userData.SelectMarketplace = marketplace;
                RootUser rootUser = userData as RootUser;
                UsersStorage.ReplaceUserDontSave(rootUser);
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите артикул");
            }
            else
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Ошибка при выборе магазина");
            }
        }

        private async Task AddOrDeleteUser(Update update, TelegramBotClient client)
        {
            long userId = update?.Message?.Chat.Id ?? -1;
            Admin? admin = UsersStorage.GetUserData(userId) as Admin;
            if(admin != null)
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите Id пользователя.");
                string command = update?.Message?.Text ?? "/start";
                LastUsersActions.UpdateLastCommand(userId, command);
            }
            else
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "У вас нет прав на данную команду.");
                LastUsersActions.UpdateLastCommand(userId, "/start");
            }
        }

        private async Task GetExcel(Update update, TelegramBotClient client)
        {
            try
            {
                string directoryExcel = @"C:\Users\Administrator\Desktop\bots\ItemIndexation\Keys\";
                string[] files = Directory.GetFiles(directoryExcel, "*.xlsx");
                foreach(string filePath in files)
                {
                    Stream stream = System.IO.File.OpenRead(filePath);
                    InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, Path.GetFileName(filePath));
                    await client.SendDocumentAsync(update.Message.Chat.Id, inputOnlineFile);
                }
            }
            catch 
            {
                Console.WriteLine("Неудалось отправить Excel");
            }
        }

    }
}
