using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot
{
    internal class MyLog : IExeptionLogger
    {

        public void PushException(Exception exceptionData)
        {
            Console.WriteLine("------------------");
            Console.WriteLine($"DateTime: {DateTime.Now}");
            Console.WriteLine($"Method: {exceptionData.TargetSite}");
            Console.WriteLine($"ErrorMessage: {exceptionData.Message}");
            Console.WriteLine($"StackTrace: {exceptionData.StackTrace}");
        }
    }
}
