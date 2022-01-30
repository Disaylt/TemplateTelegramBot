using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot.Model.Users
{
    public interface IUserProperties
    {
        public int SelectMarketplace { get; set; }
        public string? SelectId { get; set; }
    }
}
