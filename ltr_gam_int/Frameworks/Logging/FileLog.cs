// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Frameworks.Logging.FileLog
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.IO;
using System.Threading;

namespace pwnagebot.GameInterface.Frameworks.Logging
{
  public class FileLog : Logger
  {
    private static object b = new object();
    private string a = "";
    private string d = "";

    public string FileName
    {
      get
      {
        return this.a;
      }
      set
      {
        this.a = value;
      }
    }

    public string FileLocation
    {
      get
      {
        return this.d;
      }
      set
      {
        this.d = value;
        if (this.d.LastIndexOf("\\") == this.d.Length - 1)
          return;
        this.d += "\\";
      }
    }

    public event Logger.LogHandler OnLog;

    public FileLog()
    {
      string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "pwnagebot\\");
      Directory.CreateDirectory(path);
      this.FileLocation = path;
      this.FileName = "log.txt";
    }

    public override void Log(Logger.MessageType severity_, Exception message_)
    {
      this.Log(severity_, message_.Message);
    }

    public void Info(string message_)
    {
      this.Log(Logger.MessageType.Info, message_);
    }

    public void Debug(string message_)
    {
      this.Log(Logger.MessageType.Debug, message_);
    }

    public void Trace(string message_)
    {
      this.Log(Logger.MessageType.Trace, message_);
    }

    public override void Log(Logger.MessageType severity_, string message_)
    {
      if (severity_ < Logger.MessageType.Debug)
        return;
      FileStream fileStream = (FileStream) null;
      StreamWriter streamWriter = (StreamWriter) null;
      string message_1 = DateTime.Now.ToString() + ": " + message_;
      lock (FileLog.b)
      {
        while (fileStream == null)
        {
          try
          {
            fileStream = new FileStream(this.d + this.a, FileMode.OpenOrCreate, FileAccess.Write);
          }
          catch (IOException exception_0)
          {
            fileStream = (FileStream) null;
            Thread.Sleep(0);
          }
        }
        try
        {
          streamWriter = new StreamWriter((Stream) fileStream);
          streamWriter.BaseStream.Seek(0L, SeekOrigin.End);
          streamWriter.WriteLine(message_1);
          streamWriter.Flush();
        }
        finally
        {
          if (streamWriter != null)
            streamWriter.Close();
        }
      }
      if (this.c == null)
        return;
      this.c(message_1);
    }
  }
}
