using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library.Messager
{
    internal class ConsoleMessanger : IMessanger
    {
        public void Message(object s)
        {
            Console.WriteLine("------");
            Console.WriteLine(s.ToString());
            Console.WriteLine("------");
        }
    }
}
