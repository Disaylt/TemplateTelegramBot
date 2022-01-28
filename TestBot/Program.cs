MyLog myLog = new();
TelegramBotInstaller botInstaller = new("5206803137:AAHqsMS72JsaDmxnJa3cIPMHsXgLjIR3cPo", myLog);
ImplementedCommands implementedCommands = new ImplementedCommands();
await botInstaller.Start(implementedCommands);