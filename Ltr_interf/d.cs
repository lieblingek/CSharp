// Decompiled with JetBrains decompiler
// Type: pwnagebot.LotroInterface.d
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using pwnagebot.GameInterface.Frameworks;
using System;
using System.Collections;

namespace pwnagebot.LotroInterface
{
  internal class d
  {
    private const int a = 528;
    private const int b = 532;
    private const int c = 156;
    private const int d = 160;
    private const int e = 612;
    private const int f = 620;
    private static int g;

    public static void a(int A_0)
    {
      pwnagebot.LotroInterface.d.g = A_0;
    }

    internal static int n(MemoryReader A_0)
    {
      int num = A_0.ReadDWORD(pwnagebot.LotroInterface.d.g);
      if (num == 0)
        return 0;
      return A_0.ReadDWORD(num + 380);
    }

    internal static int m(MemoryReader A_0)
    {
      int num = A_0.ReadDWORD(pwnagebot.LotroInterface.d.g);
      if (num == 0)
        return 0;
      return A_0.ReadDWORD(num + 376);
    }

    private static pwnagebot.LotroInterface.c b(MemoryReader A_0, int A_1)
    {
      pwnagebot.LotroInterface.c c = new pwnagebot.LotroInterface.c();
      if (A_1 == 0)
        return c;
      Console.WriteLine(string.Format("Getting gui element at 0x{0:x8}", (object) A_1));
      c.a = A_1;
      c.b = A_0.ReadDWORD(A_1 + 528);
      c.c = A_0.ReadDWORD(A_1 + 532);
      c.d = A_0.ReadDWORD(A_1 + 156);
      c.e = A_0.ReadDWORD(A_1 + 160);
      return c;
    }

    private static void b(MemoryReader A_0, pwnagebot.LotroInterface.c A_1)
    {
      int a = A_1.a;
      if (a == 0)
        return;
      int num1 = A_0.ReadDWORD(a + 612);
      int num2 = A_0.ReadDWORD(a + 620);
      if (num1 == 0 || num2 <= 0 || num2 >= 200)
        return;
      for (int index1 = 0; index1 < num2; ++index1)
      {
        int A_1_1 = A_0.ReadDWORD(num1 + index1 * 4) - 32;
        if (A_1_1 != 0 && A_1_1 % 4 == 0 && A_1_1 > 16777216)
        {
          Console.WriteLine(string.Format("Child at 0x{0:x8}", (object) A_1_1));
          A_1.f.Add((object) pwnagebot.LotroInterface.d.b(A_0, A_1_1));
          for (int address = A_0.ReadDWORD(A_1_1 + 40); address != 0; address = A_0.ReadDWORD(address))
          {
            Console.WriteLine(string.Format("Backptr at 0x{0:x8}", (object) (address - 40)));
            A_1.f.Add((object) pwnagebot.LotroInterface.d.b(A_0, address - 40));
          }
          for (int index2 = A_0.ReadDWORD(A_1_1 + 44); index2 != 0; index2 = A_0.ReadDWORD(index2 + 4))
          {
            Console.WriteLine(string.Format("Fwdptr at 0x{0:x8}", (object) (index2 - 40)));
            A_1.f.Add((object) pwnagebot.LotroInterface.d.b(A_0, index2 - 40));
          }
          break;
        }
      }
    }

    private static pwnagebot.LotroInterface.c a(MemoryReader A_0, int A_1)
    {
      pwnagebot.LotroInterface.c c = new pwnagebot.LotroInterface.c();
      if (A_1 == 0)
        return c;
      Console.WriteLine(string.Format("Getting gui element at 0x{0:x8}", (object) A_1));
      c.a = A_1;
      c.b = A_0.ReadDWORD(A_1 + 528);
      c.c = A_0.ReadDWORD(A_1 + 532);
      c.d = A_0.ReadDWORD(A_1 + 156);
      c.e = A_0.ReadDWORD(A_1 + 160);
      int num1 = A_0.ReadDWORD(A_1 + 612);
      int num2 = A_0.ReadDWORD(A_1 + 620);
      if (num1 != 0 && num2 > 0 && num2 < 200)
      {
        for (int index1 = 0; index1 < num2; ++index1)
        {
          int A_1_1 = A_0.ReadDWORD(num1 + index1 * 4) - 32;
          if (A_1_1 != 0 && A_1_1 % 4 == 0 && A_1_1 > 16777216)
          {
            Console.WriteLine(string.Format("Child at 0x{0:x8}", (object) A_1_1));
            c.f.Add((object) pwnagebot.LotroInterface.d.a(A_0, A_1_1));
            for (int address = A_0.ReadDWORD(A_1_1 + 40); address != 0; address = A_0.ReadDWORD(address))
            {
              Console.WriteLine(string.Format("Backptr at 0x{0:x8}", (object) (address - 40)));
              c.f.Add((object) pwnagebot.LotroInterface.d.a(A_0, address - 40));
            }
            for (int index2 = A_0.ReadDWORD(A_1_1 + 44); index2 != 0; index2 = A_0.ReadDWORD(index2 + 4))
            {
              Console.WriteLine(string.Format("Fwdptr at 0x{0:x8}", (object) (index2 - 40)));
              c.f.Add((object) pwnagebot.LotroInterface.d.a(A_0, index2 - 40));
            }
            break;
          }
        }
      }
      return c;
    }

    internal static pwnagebot.LotroInterface.c l(MemoryReader A_0)
    {
      return pwnagebot.LotroInterface.d.a(A_0, pwnagebot.LotroInterface.d.n(A_0));
    }

    internal static pwnagebot.LotroInterface.c k(MemoryReader A_0)
    {
      return pwnagebot.LotroInterface.d.a(A_0, pwnagebot.LotroInterface.d.m(A_0));
    }

    internal static ArrayList j(MemoryReader A_0)
    {
      ArrayList arrayList = new ArrayList();
      int num1 = A_0.ReadDWORD(pwnagebot.LotroInterface.d.g);
      if (num1 == 0)
        return arrayList;
      int num2 = A_0.ReadDWORD(num1 + 384);
      if (num2 == 0)
        return arrayList;
      for (int index = 0; index < 64; ++index)
      {
        int A_1 = A_0.ReadDWORD(num2 + index * 4);
        if (A_1 != 0)
          arrayList.Add((object) pwnagebot.LotroInterface.d.b(A_0, A_1));
      }
      return arrayList;
    }

    internal static pwnagebot.LotroInterface.c i(MemoryReader A_0)
    {
      foreach (pwnagebot.LotroInterface.c c in pwnagebot.LotroInterface.d.j(A_0))
      {
        if (c.d == 512 && c.e == 200)
        {
          pwnagebot.LotroInterface.d.b(A_0, c);
          ArrayList A_0_1 = pwnagebot.LotroInterface.d.a(c, 0, 0, 512, 200);
          if (A_0_1.Count != 0)
          {
            foreach (pwnagebot.LotroInterface.c A_1 in A_0_1)
              pwnagebot.LotroInterface.d.b(A_0, A_1);
            ArrayList arrayList = pwnagebot.LotroInterface.d.a(A_0_1, 310, 150, 106, 20);
            if (arrayList.Count == 1)
              return (pwnagebot.LotroInterface.c) arrayList[0];
          }
        }
      }
      return (pwnagebot.LotroInterface.c) null;
    }

    internal static bool h(MemoryReader A_0)
    {
      pwnagebot.LotroInterface.c c = pwnagebot.LotroInterface.d.i(A_0);
      if (c == null || c.a == 0)
        return false;
      pwnagebot.LotroInterface.d.a(c.a, 0, 0);
      return true;
    }

    private static ArrayList a(pwnagebot.LotroInterface.c A_0, int A_1, int A_2, int A_3, int A_4)
    {
      ArrayList arrayList = new ArrayList();
      foreach (pwnagebot.LotroInterface.c c in A_0.f)
      {
        if (c.b == A_1 && c.c == A_2 && (c.d == A_3 && c.e == A_4))
          arrayList.Add((object) c);
      }
      return arrayList;
    }

    private static ArrayList a(ArrayList A_0, int A_1, int A_2, int A_3, int A_4)
    {
      ArrayList arrayList = new ArrayList();
      foreach (pwnagebot.LotroInterface.c c1 in A_0)
      {
        foreach (pwnagebot.LotroInterface.c c2 in c1.f)
        {
          if (c2.b == A_1 && c2.c == A_2 && (c2.d == A_3 && c2.e == A_4))
            arrayList.Add((object) c2);
        }
      }
      return arrayList;
    }

    private static pwnagebot.LotroInterface.c g(MemoryReader A_0)
    {
      foreach (pwnagebot.LotroInterface.c c in pwnagebot.LotroInterface.d.j(A_0))
      {
        if (c.d == 412 && c.e == 488)
        {
          pwnagebot.LotroInterface.d.b(A_0, c);
          ArrayList A_0_1 = pwnagebot.LotroInterface.d.a(c, 4, 53, 404, 422);
          if (A_0_1.Count != 0)
          {
            foreach (pwnagebot.LotroInterface.c A_1 in A_0_1)
              pwnagebot.LotroInterface.d.b(A_0, A_1);
            ArrayList A_0_2 = pwnagebot.LotroInterface.d.a(A_0_1, 8, 32, 390, 383);
            if (A_0_2.Count != 0)
            {
              foreach (pwnagebot.LotroInterface.c A_1 in A_0_2)
                pwnagebot.LotroInterface.d.b(A_0, A_1);
              ArrayList A_0_3 = pwnagebot.LotroInterface.d.a(A_0_2, 0, 0, 391, 384);
              if (A_0_3.Count != 0)
              {
                foreach (pwnagebot.LotroInterface.c A_1 in A_0_3)
                  pwnagebot.LotroInterface.d.b(A_0, A_1);
                ArrayList arrayList = pwnagebot.LotroInterface.d.a(A_0_3, 222, 364, 80, 20);
                if (arrayList.Count == 1)
                  return (pwnagebot.LotroInterface.c) arrayList[0];
              }
            }
          }
        }
      }
      return (pwnagebot.LotroInterface.c) null;
    }

    internal static bool f(MemoryReader A_0)
    {
      pwnagebot.LotroInterface.c c = pwnagebot.LotroInterface.d.g(A_0);
      if (c == null || c.a == 0)
        return false;
      pwnagebot.LotroInterface.d.a(c.a, 0, 0);
      return true;
    }

    private static pwnagebot.LotroInterface.c e(MemoryReader A_0)
    {
      foreach (pwnagebot.LotroInterface.c c in pwnagebot.LotroInterface.d.j(A_0))
      {
        if (c.d == 412 && c.e == 488)
        {
          pwnagebot.LotroInterface.d.b(A_0, c);
          ArrayList A_0_1 = pwnagebot.LotroInterface.d.a(c, 4, 53, 404, 422);
          if (A_0_1.Count != 0)
          {
            foreach (pwnagebot.LotroInterface.c A_1 in A_0_1)
              pwnagebot.LotroInterface.d.b(A_0, A_1);
            ArrayList A_0_2 = pwnagebot.LotroInterface.d.a(A_0_1, 8, 32, 390, 383);
            if (A_0_2.Count != 0)
            {
              foreach (pwnagebot.LotroInterface.c A_1 in A_0_2)
                pwnagebot.LotroInterface.d.b(A_0, A_1);
              ArrayList arrayList = pwnagebot.LotroInterface.d.a(A_0_2, 300, 364, 80, 20);
              if (arrayList.Count == 1)
                return (pwnagebot.LotroInterface.c) arrayList[0];
            }
          }
        }
      }
      return (pwnagebot.LotroInterface.c) null;
    }

    internal static bool d(MemoryReader A_0)
    {
      pwnagebot.LotroInterface.c c = pwnagebot.LotroInterface.d.e(A_0);
      if (c == null || c.a == 0)
        return false;
      pwnagebot.LotroInterface.d.a(c.a, 0, 0);
      return true;
    }

    private static pwnagebot.LotroInterface.c c(MemoryReader A_0)
    {
      foreach (pwnagebot.LotroInterface.c c in pwnagebot.LotroInterface.d.j(A_0))
      {
        if (c.d == 512 && c.e == 173)
        {
          pwnagebot.LotroInterface.d.b(A_0, c);
          ArrayList A_0_1 = pwnagebot.LotroInterface.d.a(c, 0, 0, 512, 173);
          if (A_0_1.Count != 0)
          {
            foreach (pwnagebot.LotroInterface.c A_1 in A_0_1)
              pwnagebot.LotroInterface.d.b(A_0, A_1);
            ArrayList A_0_2 = pwnagebot.LotroInterface.d.a(A_0_1, 2, 141, 508, 20);
            if (A_0_2.Count != 0)
            {
              foreach (pwnagebot.LotroInterface.c A_1 in A_0_2)
                pwnagebot.LotroInterface.d.b(A_0, A_1);
              ArrayList arrayList = pwnagebot.LotroInterface.d.a(A_0_2, 142, 0, 106, 20);
              if (arrayList.Count == 1)
                return (pwnagebot.LotroInterface.c) arrayList[0];
            }
          }
        }
      }
      return (pwnagebot.LotroInterface.c) null;
    }

    internal static bool b(MemoryReader A_0)
    {
      pwnagebot.LotroInterface.c c = pwnagebot.LotroInterface.d.c(A_0);
      if (c == null || c.a == 0)
        return false;
      pwnagebot.LotroInterface.d.a(c.a, 0, 0);
      return true;
    }

    internal static ArrayList a(MemoryReader A_0)
    {
      foreach (pwnagebot.LotroInterface.c c in pwnagebot.LotroInterface.d.j(A_0))
      {
        if (c.d == 412 && c.e == 488)
        {
          pwnagebot.LotroInterface.d.b(A_0, c);
          ArrayList A_0_1 = pwnagebot.LotroInterface.d.a(c, 4, 53, 404, 422);
          if (A_0_1.Count != 0)
          {
            foreach (pwnagebot.LotroInterface.c A_1 in A_0_1)
              pwnagebot.LotroInterface.d.b(A_0, A_1);
            ArrayList A_0_2 = pwnagebot.LotroInterface.d.a(A_0_1, 8, 32, 390, 383);
            if (A_0_2.Count != 0)
            {
              foreach (pwnagebot.LotroInterface.c A_1 in A_0_2)
                pwnagebot.LotroInterface.d.b(A_0, A_1);
              ArrayList A_0_3 = pwnagebot.LotroInterface.d.a(A_0_2, 0, 30, 384, 329);
              if (A_0_3.Count != 0)
              {
                foreach (pwnagebot.LotroInterface.c A_1 in A_0_3)
                  pwnagebot.LotroInterface.d.b(A_0, A_1);
                ArrayList arrayList = pwnagebot.LotroInterface.d.a(A_0_3, 4, 4, 368, 321);
                if (arrayList.Count == 1)
                {
                  foreach (pwnagebot.LotroInterface.c A_1 in arrayList)
                    pwnagebot.LotroInterface.d.b(A_0, A_1);
                  return ((pwnagebot.LotroInterface.c) arrayList[0]).f;
                }
              }
            }
          }
        }
      }
      return (ArrayList) null;
    }

    internal static ulong a(MemoryReader A_0, pwnagebot.LotroInterface.c A_1)
    {
      return A_0.ReadLong(A_1.a + 1168);
    }

    private static void a(int A_0, int A_1, int A_2)
    {
      pwnagebot.LotroInterface.b.a(A_0, A_1, A_2);
    }
  }
}
