using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace ThreadExperimenting
{

    

    class Program
    {

        static long _total;
        static ConcurrentQueue<string> _qued;


        static void Main(string[] args)
        {
            
            _qued = new ConcurrentQueue<string>();
            _total = 0;

            while (true)
            {
                Task task2 = Task.Run(() => PutInQue());
                Task.WaitAll(task2);
                Console.WriteLine("ontvangen en verzonden");
                Thread.Sleep(50000);
            }

         
            Console.ReadKey();



        }
       public  static void ProssesQue()
        {
            Int32 port = 1400;
            IPAddress ipadress = IPAddress.Parse("127.0.0.1");
            TcpClient client = new TcpClient();
            client.Connect(ipadress, port);
            Boolean stillSending = true;
            try
            {
                string value;

                while (stillSending)
                {
                    

                    NetworkStream stream = client.GetStream();
                    string data = "waiting";
                    if (_qued.TryDequeue(out value))
                    {
                        data = value;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine(String.Format("Sent: {0}", data));
                    }
                    else
                    {
                        client.Close();
                        stream.Close();
                        stillSending = false;
                    }
                   
                }

            }
            catch (InvalidOperationException e)
            { 
            
            }

        }

        public static void PutInQue()
       {
           try
           {
               
               int socket = 1300;
               TcpClient client = new TcpClient();
               client.Connect("127.0.0.1", 1300);
               NetworkStream sm = client.GetStream();
               StringBuilder sb = new StringBuilder();
               byte[] buffer = new byte[256];
               bool stillreading = true;
               while(stillreading)
               {
                   sm.Read(buffer, 0, 256);
                   if (buffer.Length > 0)
                   {
                       string s = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                       int i = s.IndexOf("\0");
                       if (i > 0)
                       {
                           _qued.Enqueue(s.Remove(i));
                       }
                       Task task1 = Task.Run(() => ProssesQue());
                       Console.WriteLine("recieved:" + Encoding.ASCII.GetString(buffer, 0, buffer.Length));
                       sb.Clear();
                       Array.Clear(buffer, 0, buffer.Length);
                       Task.WaitAll(task1);
                       
                       client.Close();
                       sm.Close();
                       stillreading = false;
                   }
               }


               
                   
                  
               

           }
            catch(InvalidOperationException e)
           {


           }


       }
    }
}
