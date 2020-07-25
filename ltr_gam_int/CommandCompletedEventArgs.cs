// Decompiled with JetBrains decompiler
// Type: CommandCompletedEventArgs
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

[GeneratedCode("wsdl", "2.0.50727.42")]
[DebuggerStepThrough]
[DesignerCategory("code")]
internal class CommandCompletedEventArgs : AsyncCompletedEventArgs
{
  private object[] results;

  public string Result
  {
    get
    {
      this.RaiseExceptionIfNecessary();
      return (string) this.results[0];
    }
  }

  internal CommandCompletedEventArgs(object[] A_0, Exception A_1, bool A_2, object A_3)
    : base(A_1, A_2, A_3)
  {
    this.results = A_0;
  }
}
