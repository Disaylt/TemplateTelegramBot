using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TemplateTelegramBot.UserStorage
{
    [Keyless]
    public class RootUser
    {
        public string? UserName { get; set; }
        public long ChatId { get; set; }
        public int UserTypeId { get; set; }
        public int IsActive { get; set; }
    }
}
