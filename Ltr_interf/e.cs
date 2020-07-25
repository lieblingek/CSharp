// Decompiled with JetBrains decompiler
// Type: pwnagebot.LotroInterface.e
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using pwnagebot.GameInterface.Frameworks;
using pwnagebot.GameInterface.Frameworks.Logging;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace pwnagebot.LotroInterface
{
  internal class e
  {
    private static Hashtable a = new Hashtable();
    private static Hashtable b = new Hashtable();
    private static int c = 0;

    public static void a(int A_0)
    {
      e.c = A_0;
    }

    public static void b()
    {
      FileLog instance = Singleton<FileLog>.Instance;
      string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "pwnagebot" + (object) Path.DirectorySeparatorChar + "Lotro");
      instance.Info("Saving strings...");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      FileStream fileStream = new FileStream(path + (object) Path.DirectorySeparatorChar + "strings.dat", FileMode.OpenOrCreate, FileAccess.Write);
      try
      {
        new BinaryFormatter().Serialize((Stream) fileStream, (object) e.a);
      }
      finally
      {
        fileStream.Close();
      }
    }

    public static void a()
    {
      FileLog instance = Singleton<FileLog>.Instance;
      string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "pwnagebot" + (object) Path.DirectorySeparatorChar + "Lotro" + (object) Path.DirectorySeparatorChar + "strings.dat");
      instance.Info("Loading strings...");
      if (!File.Exists(path))
        return;
      FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
      try
      {
        e.a = (Hashtable) new BinaryFormatter().Deserialize((Stream) fileStream);
      }
      finally
      {
        fileStream.Close();
      }
    }

    public static string b(MemoryReader A_0, int A_1, int A_2)
    {
      WStringID wstringId = new WStringID();
      wstringId.b = A_1;
      wstringId.c = A_2;
      string str;
      if (e.a.Contains((object) wstringId))
      {
        str = (string) e.a[(object) wstringId];
      }
      else
      {
        if (e.b.Contains((object) wstringId))
        {
          if ((uint) (DateTime.Now - (DateTime) e.b[(object) wstringId]).TotalMilliseconds < 1000U)
            return "";
        }
        else
          e.b.Add((object) wstringId, (object) DateTime.Now);
        str = e.a(A_0, wstringId.b, wstringId.c);
        e.b[(object) wstringId] = (object) DateTime.Now;
        if (str != string.Empty)
          e.a.Add((object) wstringId, (object) str);
      }
      return str;
    }

    private static string a(MemoryReader A_0, int A_1, int A_2)
    {
      Hashtable hashtable1 = new pwnagebot.LotroInterface.a(A_0, e.c, 1).b();
      if (!hashtable1.ContainsKey((object) 32))
        throw new StringTableException();
      int num1 = (int) hashtable1[(object) 32];
      int num2 = A_0.ReadDWORD(num1 + 8);
      if (num2 == 0)
        throw new StringTableException();
      Hashtable hashtable2 = new pwnagebot.LotroInterface.a(A_0, num2 + 16, 1).b();
      if (!hashtable2.ContainsKey((object) A_1))
        return "";
      int num3 = (int) hashtable2[(object) A_1];
      int num4 = A_0.ReadDWORD(num3 + 8);
      if (num4 == 0)
        throw new StringTableException();
      Hashtable hashtable3 = new pwnagebot.LotroInterface.a(A_0, num4 + 76, 1).b();
      if (!hashtable3.ContainsKey((object) A_2))
        return string.Empty;
      int num5 = (int) hashtable3[(object) A_2];
      int num6 = A_0.ReadDWORD(num5 + 8);
      if (num6 == 0)
        throw new StringTableException();
      int address = A_0.ReadDWORD(num6 + 20);
      if (address == 0)
        throw new StringTableException();
      int iAddress = A_0.ReadDWORD(address);
      if (address == 0)
        throw new StringTableException();
      return A_0.ReadUnicodeString(iAddress, 256);
    }
  }
}
