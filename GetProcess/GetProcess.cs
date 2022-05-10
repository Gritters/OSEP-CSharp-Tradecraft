using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetProcess
{
    internal class GetProcess
    {
        static void Main(string[] args)
        {
            Process[] explorer = Process.GetProcessesByName("explorer");
            Console.Write(explorer[0]);
            Console.Write(explorer[0].Id);

            
        }
    }
}
