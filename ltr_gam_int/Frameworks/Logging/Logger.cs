// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Frameworks.Logging.Logger
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;

namespace pwnagebot.GameInterface.Frameworks.Logging
{
  public abstract class Logger
  {
    public abstract void Log(Logger.MessageType Severity, Exception Message);

    public abstract void Log(Logger.MessageType Severity, string Message);

    public enum MessageType
    {
      Trace = 1,
      Debug = 2,
      Info = 3,
      Failure = 4,
      Warning = 5,
      Error = 6,
    }

    public delegate void LogHandler(string message_);
  }
}
