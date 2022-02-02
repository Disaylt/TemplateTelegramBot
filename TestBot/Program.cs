string token = System.IO.File.ReadAllText($@"{GlobalVariables.ProjectDirectory}token.txt");

MyLog myLog = new();
UsersStorageSettings storageSettings = new UsersStorageSettings(GlobalVariables.UserStorageDirectory, UserTypes.TypeMap);
TelegramBotInstaller botInstaller = new(token, myLog, storageSettings);
ImplementedCommands implementedCommands = new ImplementedCommands();
ImplementedActions implementedActions = new ImplementedActions();
StandardAction standardAction = new StandardAction();
await botInstaller.Start(implementedCommands, implementedActions, standardAction, true);