using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot
{
    internal class MyLog : IExeptionLogger
    {
        public void PushException(ExceptionData exceptionData)
        {
            Console.WriteLine("------------------");
            Console.WriteLine($"DateTime: {exceptionData.DateTime}");
            Console.WriteLine($"Method: {exceptionData.CurrentMethod}");
            Console.WriteLine($"ErrorMessage: {exceptionData.Message}");
            Console.WriteLine($"StackTrace: {exceptionData.StackTrace}");
        }
    }
}
