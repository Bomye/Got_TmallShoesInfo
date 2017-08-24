using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nike_Tmall
{
    class DNSCleaner
    {
        public static void DnsClean()
        {
            //CC.Utility.ProcessHelper process = new CC.Utility.ProcessHelper("cmd.exe", Show, Show);
            //process.Start();
            //process.Input("ipconfig/flushdns");
        }
        static void Show(string str)
        {
            Console.WriteLine(str);
        }
    }
}
