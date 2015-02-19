// Decompiled with JetBrains decompiler
// Type: TCPDumper.Program
// Assembly: TCPDumper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 80394E41-EAA0-4587-9915-7D844B49EF71
// Assembly location: C:\Users\wouter\Desktop\TcpDumper\TCPDumper.exe

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPDumper
{
  internal class Program
  {
    private static readonly ConcurrentDictionary<string, ulong> ClientStats = new ConcurrentDictionary<string, ulong>();
    private static TcpListener _tcpL;
    private static LogWriter _writer;
    private static bool _debug;

    private static void Main(string[] args)
    {
      try
      {
#if DEBUG
          args = new[] { "1400" };
#endif
        if (args == null || args.Length == 0)
        {
          Console.WriteLine("Please supply the port paramater");
        }
        else
        {
          int port = int.Parse(args[0]);
          Program._debug = args.Length > 1 && !string.IsNullOrEmpty(args[1]) && args[1].ToLower().Equals("/d");
          Program._tcpL = new TcpListener(IPAddress.Any, port);
          Program._tcpL.Start();
          Program._writer = new LogWriter(port);
          Program._writer.WriteLogLine("Server started on port " + port.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          Program._tcpL.BeginAcceptTcpClient(new AsyncCallback(Program.ConnectionHandler), (object) null);
          Console.Title = "TCPDumper started on " + (object) port;
          Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
          Console.Clear();
          while (true)
          {
            Console.Clear();
            Console.WriteLine("\tTCP Dumper listening on " + (object) port);
            Console.WriteLine("\t-------------------------------");
            Console.WriteLine(string.Format("Number of clients conntected: {0} \t\t\t {1}", (object) Program.ClientStats.Count, (object) DateTime.Now));
           
            if (!Program.ClientStats.IsEmpty)
            {
              foreach (KeyValuePair<string, ulong> keyValuePair in Program.ClientStats)
                Console.WriteLine(string.Format("{0}\t: {1}", (object) keyValuePair.Key, (object) keyValuePair.Value));
            }
            Thread.Sleep(1000);
          }
        }
      }
      finally
      {
        if (Program._tcpL != null)
          Program._tcpL.Stop();
      }
    }

    private static void ConnectionHandler(IAsyncResult result)
    {
      TcpClient client;
      try
      {
        client = Program._tcpL.EndAcceptTcpClient(result);
      }
      catch (ObjectDisposedException ex)
      {
        return;
      }
      string key = client.Client.RemoteEndPoint.ToString();
      Program._tcpL.BeginAcceptTcpClient(new AsyncCallback(Program.ConnectionHandler), (object) null);
      Program.ClientStats.TryAdd(key, 0UL);
      Program.ProcessClient(client);
    }

    private static void ProcessClient(TcpClient client)
    {
      client.ReceiveTimeout = 500000;
      NetworkStream stream = client.GetStream();
      byte[] buffer = new byte[4096];
      stream.ReadTimeout = 500000;
      try
      {
        while (true)
        {
          int num1;
          
          
            num1 = stream.Read(buffer, 0, 4096);
            if (num1 != 0)
            {
                Console.WriteLine("test:" + num1);
              long num2 = (long) Program.ClientStats.AddOrUpdate(client.Client.RemoteEndPoint.ToString(), 1UL, (Func<string, ulong, ulong>) ((key, value) => value + 1UL));
            }
            else
              goto label_8;
          
          
          Program._writer.WriteLogLine(client.Client.RemoteEndPoint.ToString());
          for (int index = 0; index < num1; ++index)
          {
          //    string s = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
          //    int i = s.IndexOf("\0");
          //    s.Trim();
          //    s += "\n \n";
            string s = BitConverter.ToString(new byte[1]
            {
              buffer[index]
           });
            s = Encoding.ASCII.GetString(new byte[1]
            {
              buffer[index]
           });
            Program._writer.WriteLog(s);
          }
          Program._writer.WriteLog(Environment.NewLine);
        }
label_8:;
      }
      catch (Exception ex)
      {
        Program._writer.WriteLogLine("2" + ex.Message + ex.StackTrace);
      }
    }
  }
}
