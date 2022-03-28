string token = System.IO.File.ReadAllText($@"{GlobalVariables.ProjectDirectory}token.txt");

MyLog myLog = new();
TelegramBotInstaller botInstaller = new(token, myLog);
ImplementedCommands implementedCommands = new ImplementedCommands();
ImplementedActions implementedActions = new ImplementedActions();
StandardAction standardAction = new StandardAction();
await botInstaller.Start(implementedCommands, implementedActions, standardAction, false);