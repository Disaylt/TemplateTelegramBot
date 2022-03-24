using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateTelegramBot.UserStorage
{
    public abstract class RootUser
    {
        public abstract string Name { get; }
        public abstract long Id { get; set; }
        public abstract int UserType { get; set; }
        
    }
}
