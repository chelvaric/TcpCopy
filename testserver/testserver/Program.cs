using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace testserver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("file inlezen");
            leesFileIn();
            Int32 port = 1300;
            IPAddress localAdress = IPAddress.Parse("127.0.0.1");
            TcpListener server = new TcpListener(localAdress, port);
            server.Start();

            byte[] Bytes = new byte[256];
            string data = null;
            while (true)
            { 
              Console.WriteLine("waiting for a connection..");
              TcpClient client = server.AcceptTcpClient();
              Console.WriteLine("connected!");
              
              NetworkStream stream = client.GetStream();
              data = "test";

              try
              {



                  for (int i = 0; i < arrayOfData.Length; i++)
                  {
                      byte[] msg = System.Text.Encoding.ASCII.GetBytes(arrayOfData);
                      // Send data
                      stream.Write(msg, 0, msg.Length);
                      Console.WriteLine(String.Format("Sent: {0}", arrayOfData[i]));

                  }

              }
              catch(System.IO.IOException e)
              {

              }
              
            
            }
  
            
 

        }
        static String  arrayOfData;
        public static void leesFileIn()
        {
            arrayOfData = System.IO.File.ReadAllText(@".\binarydata.txt");
           

        }
    }
}
