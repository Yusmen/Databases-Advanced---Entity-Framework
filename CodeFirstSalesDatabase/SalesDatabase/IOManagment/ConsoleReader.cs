using SalesDatabase.IOManagment.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesDatabase.IOManagment
{
    public class ConsoleReader : IReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
