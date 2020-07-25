// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Pointers
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System.Collections;
using System.Collections.Generic;

namespace pwnagebot.GameInterface
{
  public class Pointers
  {
    private Dictionary<string, int> a;

    public Pointers()
    {
      this.a = new Dictionary<string, int>();
    }

    public bool Exists(string name_)
    {
      return this.a.ContainsKey(name_);
    }

    public int Get(string name_)
    {
      return this.a[name_];
    }

    public void Set(string name_, int value_)
    {
      this.a[name_] = value_;
    }

    public void Set(Hashtable table_)
    {
      this.a.Clear();
      foreach (DictionaryEntry dictionaryEntry in table_)
        this.a.Add((string) dictionaryEntry.Key, (int) dictionaryEntry.Value);
    }
  }
}
