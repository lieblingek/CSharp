// Decompiled with JetBrains decompiler
// Type: pwnagebot.LotroInterface.a
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using pwnagebot.GameInterface.Frameworks;
using System;
using System.Collections;

namespace pwnagebot.LotroInterface
{
  internal class a
  {
    private MemoryReader a;
    private int b;
    private int c;

    public a(MemoryReader A_0, int A_1, int A_2)
    {
      if (A_2 < 1 || A_2 > 2)
        throw new ArgumentException("Invalid keysize");
      this.a = A_0;
      this.c = A_2;
      this.b = A_1;
    }

    public Hashtable b()
    {
      Hashtable A_0 = this.a();
      for (Hashtable A_1 = this.a(); !this.a(A_0, A_1); A_1 = this.a())
        A_0 = this.a();
      return A_0;
    }

    public int a(int A_0)
    {
      int num1 = 0;
      if (this.c != 1)
        throw new ArgumentException("Invalid keysize");
      int num2 = this.a.ReadDWORD(this.b);
      int num3 = this.a.ReadDWORD(this.b + 8);
      if (num2 == 0 || num3 > 8191 || num3 == 0)
        return 0;
      int num4 = A_0 % num3;
      int address = this.a.ReadDWORD(num2 + num4 * 4);
      int num5 = this.a.ReadDWORD(address);
      while (num5 != A_0)
      {
        address = this.a.ReadDWORD(address + 4);
        if (address == 0)
          return 0;
        num5 = this.a.ReadDWORD(address);
        if (num1++ > 999)
          return 0;
      }
      return address;
    }

    private Hashtable a()
    {
      Hashtable hashtable = new Hashtable();
      int num1 = this.a.ReadDWORD(this.b);
      int num2 = this.a.ReadDWORD(this.b + 8);
      if (num1 == 0 || num2 > 8191)
        return hashtable;
      for (int index = 0; index < num2; ++index)
      {
        int num3 = 0;
        int address = this.a.ReadDWORD(num1 + index * 4);
        while (address != 0)
        {
          if (this.c == 1)
          {
            int num4 = this.a.ReadDWORD(address);
            if (!hashtable.Contains((object) num4))
              hashtable.Add((object) num4, (object) address);
          }
          else if (this.c == 2)
          {
            ulong num4 = this.a.ReadLong(address);
            if (!hashtable.Contains((object) num4))
              hashtable.Add((object) num4, (object) address);
          }
          address = this.a.ReadDWORD(address + 4 * this.c);
          if (num3++ > 999)
            return hashtable;
        }
      }
      return hashtable;
    }

    private bool a(ICollection A_0, ICollection A_1)
    {
      IEnumerator enumerator1 = A_0.GetEnumerator();
      IEnumerator enumerator2 = A_1.GetEnumerator();
      if (A_0.Count != A_1.Count)
        return false;
      while (enumerator1.MoveNext() && enumerator2.MoveNext())
      {
        if (enumerator1.Current.GetType().ToString() == "System.UInt64" && enumerator2.Current.GetType().ToString() == "System.UInt64")
        {
          if ((long) (ulong) enumerator1.Current != (long) (ulong) enumerator2.Current)
            return false;
        }
        else if (!(enumerator1.Current.GetType().ToString() == "System.Int32") || !(enumerator2.Current.GetType().ToString() == "System.Int32") || (int) enumerator1.Current != (int) enumerator2.Current)
          return false;
      }
      return true;
    }

    private bool a(Hashtable A_0, Hashtable A_1)
    {
      return A_0.Count == A_1.Count && this.a(A_0.Keys, A_1.Keys) && this.a(A_0.Values, A_1.Values);
    }
  }
}
