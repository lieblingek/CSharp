// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Frameworks.EventArgs`1
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;

namespace pwnagebot.GameInterface.Frameworks
{
  public class EventArgs<T> : EventArgs
  {
    private T a;

    public T Value
    {
      get
      {
        return this.a;
      }
    }

    public EventArgs(T value)
    {
      this.a = value;
    }
  }
}
