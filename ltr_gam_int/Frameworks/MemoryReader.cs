// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Frameworks.MemoryReader
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.Runtime.InteropServices;

namespace pwnagebot.GameInterface.Frameworks
{
  public class MemoryReader
  {
    private object a = new object();
    private IntPtr c = IntPtr.Zero;
    private IntPtr b;
    private byte[] d;

    public MemoryReader(IntPtr hClient)
    {
      this.b = hClient;
      this.c = Marshal.AllocHGlobal(8);
      this.d = new byte[4];
    }

    ~MemoryReader()
    {
      Marshal.FreeHGlobal(this.c);
      this.d = (byte[]) null;
    }

    public int ReadStruct<T>(int address, ref T structRef) where T : struct
    {
      int num1 = Marshal.SizeOf((object) default (T));
      IntPtr num2 = Marshal.AllocHGlobal(num1);
      int A_4;
      pwnagebot.GameInterface.Frameworks.a.ReadProcessMemory(this.b, new IntPtr(address), num2, num1, out A_4);
      structRef = (T) Marshal.PtrToStructure(num2, typeof (T));
      Marshal.FreeHGlobal(num2);
      return A_4;
    }

    public byte ReadBYTE(int address)
    {
      int A_4 = 0;
      lock (this.a)
      {
        pwnagebot.GameInterface.Frameworks.a.ReadProcessMemory(this.b, new IntPtr(address), this.c, 1, out A_4);
        return Marshal.ReadByte(this.c);
      }
    }

    public ulong ReadLong(int address)
    {
      int A_4 = 0;
      lock (this.a)
      {
        pwnagebot.GameInterface.Frameworks.a.ReadProcessMemory(this.b, new IntPtr(address), this.c, 8, out A_4);
        return (ulong) Marshal.ReadInt64(this.c);
      }
    }

    public int ReadDWORD(int address)
    {
      int A_4 = 0;
      lock (this.a)
      {
        pwnagebot.GameInterface.Frameworks.a.ReadProcessMemory(this.b, new IntPtr(address), this.c, 4, out A_4);
        return Marshal.ReadInt32(this.c);
      }
    }

    public float ReadFloat(int address)
    {
      int A_4 = 0;
      lock (this.a)
      {
        pwnagebot.GameInterface.Frameworks.a.ReadProcessMemory(this.b, new IntPtr(address), this.c, 4, out A_4);
        Marshal.Copy(this.c, this.d, 0, 4);
        return BitConverter.ToSingle(this.d, 0);
      }
    }

    public void WriteFloat(int Address, float value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      IntPtr zero = IntPtr.Zero;
      lock (this.a)
      {
        Marshal.Copy(bytes, 0, this.c, 4);
        pwnagebot.GameInterface.Frameworks.a.WriteProcessMemory(this.b, new IntPtr(Address), this.c, 4, ref zero);
      }
    }

    public void WriteInteger(int Address, int value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      IntPtr zero = IntPtr.Zero;
      lock (this.a)
      {
        Marshal.Copy(bytes, 0, this.c, 4);
        pwnagebot.GameInterface.Frameworks.a.WriteProcessMemory(this.b, new IntPtr(Address), this.c, 4, ref zero);
      }
    }

    public void WriteLong(int Address, ulong value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      IntPtr zero = IntPtr.Zero;
      lock (this.a)
      {
        Marshal.Copy(bytes, 0, this.c, 8);
        pwnagebot.GameInterface.Frameworks.a.WriteProcessMemory(this.b, new IntPtr(Address), this.c, 8, ref zero);
      }
    }

    public string ReadAnsiString(int iAddress, int iLength)
    {
      IntPtr A_1 = new IntPtr(iAddress);
      string str = "";
      if (iAddress == 0)
        return str;
      IntPtr num = Marshal.AllocHGlobal(iLength);
      int A_4;
      if (!pwnagebot.GameInterface.Frameworks.a.ReadProcessMemory(this.b, A_1, num, iLength, out A_4) || A_4 != iLength)
        return str;
      string stringAnsi = Marshal.PtrToStringAnsi(num);
      Marshal.FreeHGlobal(num);
      return stringAnsi;
    }

    public string ReadUnicodeString(int iAddress, int iLength)
    {
      IntPtr A_1 = new IntPtr(iAddress);
      string str = "";
      if (iAddress == 0)
        return str;
      IntPtr num = Marshal.AllocHGlobal(iLength);
      int A_4;
      if (!pwnagebot.GameInterface.Frameworks.a.ReadProcessMemory(this.b, A_1, num, iLength, out A_4) || A_4 != iLength)
        return str;
      string stringUni = Marshal.PtrToStringUni(num);
      Marshal.FreeHGlobal(num);
      return stringUni;
    }
  }
}
