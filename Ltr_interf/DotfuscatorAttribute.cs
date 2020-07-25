// Decompiled with JetBrains decompiler
// Type: DotfuscatorAttribute
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using System;
/*
[AttributeUsage(AttributeTargets.Assembly)]
/*
public sealed class DotfuscatorAttribute : Attribute
{
  private string a;
  private int c;

  public string A
  {
    get
    {
      return this.a;
    }
  }

  public int C
  {
    get
    {
      return this.c;
    }
  }

  public DotfuscatorAttribute(string a, int c)
  {
    DotfuscatorAttribute dotfuscatorAttribute = this;
    // ISSUE: explicit constructor call
    // #### dotfuscatorAttribute.\u002Ector();

    // #### string str = a;
    // #### dotfuscatorAttribute.a = str;
    this.a = a;
    this.c = c;
  }

  public string a()
  {
    return this.a;
  }

  public int c()
  {
    return this.c;
  }
}
*/