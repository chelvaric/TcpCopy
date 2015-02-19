using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.Net;

namespace commandLineParameterHelp
{
    class Program
    {
       static List<string> _sendIpAdresses = new List<string>();
        static List<string> _sendSockets = new List<string>();
        static int _listenSocket;
        static IPAddress _listenIpAdress;
        static bool ServerOrClient;

        static void Main(string[] args)
        {
            Options options = new Options();
            var result = Parser.Default.ParseArguments(args,options);
            if(Parser.Default.ParseArguments(args,options))
            {
               
                if(options.WayOfSend)
                {
                    ServerOrClient = true;
                    Console.WriteLine("ListenSocket: {0}", options.ListenSocket);
                  Console.WriteLine("ListenIP: {0}", options.ListenIpAdress);
                    Console.WriteLine("SendSocket or sockets:{0}", options.sendSocket);
                    _listenSocket = options.ListenSocket;
                    _listenIpAdress = IPAddress.Parse(options.ListenIpAdress);
                    if(options.sendSocket.IndexOf(';') != -1)
                    {

                        String[] sepSock = options.sendSocket.Split(';');
                        _sendIpAdresses.AddRange(sepSock);
                    }
                    else
                    {
                        _sendIpAdresses.Add(options.sendSocket);

                    }


                   
                }
                else
                {
                    ServerOrClient = false;
                    Console.WriteLine("ListenSocket: {0}", options.ListenSocket);
                    Console.WriteLine("ListenIP: {0}", options.ListenIpAdress);
                    Console.WriteLine("SendSocket or sockets:{0}", options.sendSocket);
                    Console.WriteLine("sendIps: {0}", options.SendIp);
                    _listenIpAdress = IPAddress.Parse(options.ListenIpAdress);
                    if (options.sendSocket.IndexOf(';') != -1)
                    {

                        String[] sepSock = options.sendSocket.Split(';');
                        _sendSockets.AddRange(sepSock);
                    }
                    else
                    {
                        _sendSockets.Add(options.sendSocket);

                    }

                    if(options.SendIp.IndexOf(';') != -1)
                    {
                        String[] sepIp = options.SendIp.Split(';');
                        _sendIpAdresses.AddRange(sepIp);
                    }
                    else
                    {
                        _sendIpAdresses.Add(options.SendIp);

                    }
                }
            }
            Console.ReadKey();
            
        }
    }
}
