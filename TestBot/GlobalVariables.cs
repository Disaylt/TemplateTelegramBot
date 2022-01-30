using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot
{
    internal class GlobalVariables
    {
        private static readonly string? _projectDirecotory = $@"{Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent}\";
        public static string ProjectDirectory
        {
            get
            {
                if (_projectDirecotory == null)
                {
                    return string.Empty;
                }
                else
                {
                    return _projectDirecotory;
                }
            }
        }
        public static string UserStorageDirectory = $@"{ProjectDirectory}UsersStorage\";
    }
}
