// Decompiled with JetBrains decompiler
// Type: TCPDumper.LogWriter
// Assembly: TCPDumper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 80394E41-EAA0-4587-9915-7D844B49EF71
// Assembly location: C:\Users\wouter\Desktop\TcpDumper\TCPDumper.exe

using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace TCPDumper
{
  public class LogWriter
  {
    private static readonly object SyncObject = new object();
    private static StreamWriter _sw;
    private readonly int _port;

    protected LogWriter()
    {
    }

    public LogWriter(int port)
    {
      this._port = port;
      LogWriter.SetupWriter(port);
    }

    private static void SetupWriter(int port)
    {
      LogWriter._sw = File.CreateText(string.Format("c:\\be-mobile\\logs\\tcpdump.{0}.txt", (object) port.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      LogWriter._sw.AutoFlush = true;
    }

    public void Cycle()
    {
      Monitor.Enter(LogWriter.SyncObject);
      LogWriter._sw.Close();
      LogWriter.SetupWriter(this._port);
      Monitor.Exit(LogWriter.SyncObject);
    }

    public void Dispose()
    {
      if (LogWriter._sw == null)
        return;
      LogWriter._sw.Dispose();
    }

    public void WriteLogLine(string s)
    {
      Monitor.Enter(LogWriter.SyncObject);
      LogWriter._sw.WriteLine(s);
      Monitor.Exit(LogWriter.SyncObject);
    }

    public void WriteLog(string s)
    {
      Monitor.Enter(LogWriter.SyncObject);
      LogWriter._sw.Write(s);
      Monitor.Exit(LogWriter.SyncObject);
    }
  }
}
