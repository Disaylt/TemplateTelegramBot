using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot.Model.Users
{
    internal static class UserTypes
    {
        private static Dictionary<int, Type> _typeMap = new Dictionary<int, Type>
        {
            {(int)Types.Admin, typeof(Admin) },
            {(int)Types.User, typeof(User) }
        };
        internal enum Types
        {
            User,
            Admin
        }
        internal static Dictionary<int, Type> TypeMap
        {
            get
            {
                return _typeMap;
            }
        }
    }
}
