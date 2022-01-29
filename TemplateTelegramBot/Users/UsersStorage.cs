using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.Users
{
    public static class UsersStorage
    {
        public static List<RootUser> UsersData { get; set; } = new List<RootUser>();
    }
}
