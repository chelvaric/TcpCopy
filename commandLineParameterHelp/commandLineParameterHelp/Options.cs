using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commandLineParameterHelp
{
    class Options
    {
        [Option("ListenSocket", HelpText = "geef de socket op van de data tcp")]
        public int ListenSocket { get; set; }
       [Option("ListenIp", HelpText = "hier heeft u de ip in waar dat hij moet op luisteren")]
        public string ListenIpAdress { get; set; }

        [Option("sendSocket", HelpText = "socket waarop de data weer word uitgestuurd. shijd met ; voor meerdere")]
        public string sendSocket { get; set; }

        [Option('w', "wayofsending", HelpText = "true als u wilt sturen vergeet dan niet de SendIp en SendSocket in te stellen, false als u wilt laten conecteren u moet dan de sendsocket instellen naar waar u de data uitbrengt")]
        public bool WayOfSend { get; set; }

        [Option("SendIpAdress", HelpText = "ipadresses to send to, use ; to seperate them ")]
        public string SendIp { get; set; }
     
    }
}
