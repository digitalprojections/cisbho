using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //output text to the screen
            int myInteger;
            string myString;
            myInteger = 17;
            myString = "\"myInteger\"is";
            Console.WriteLine("{0} actually {1}.",myString, myInteger);
            
            Console.ReadKey();
        }
    }
}
