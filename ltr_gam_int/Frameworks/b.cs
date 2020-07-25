// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Frameworks.b
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.Runtime.InteropServices;

namespace pwnagebot.GameInterface.Frameworks
{
  internal sealed class b
  {
    public const uint a = 2;
    public const uint b = 3;
    public const uint c = 1;
    public const uint d = 0;
    public const uint e = 1;
    public const uint f = 0;
    public const uint g = 2;
    public const uint h = 0;
    public const uint i = 4;
    public const uint j = 0;
    public const uint k = 1;
    public const uint l = 255;
    public const uint m = 4294967295;
    public const uint n = 1;
    public const uint o = 0;
    public const uint p = 2147483648;
    public const uint q = 1073741824;
    public const uint r = 536870912;
    public const uint s = 268435456;
    public const uint t = 1;
    public const uint u = 2;
    public const uint v = 3;
    public const uint w = 4;
    public const uint x = 5;
    public const int y = -1;
    public const uint z = 0;
    public const uint aa = 2;
    public const uint ab = 231;
    public const uint ac = 232;
    public const uint ad = 233;
    public const uint ae = 234;
    public const uint af = 535;
    public const uint ag = 536;

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr CreateNamedPipe(string A_0, uint A_1, uint A_2, uint A_3, uint A_4, uint A_5, uint A_6, IntPtr A_7);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern int DisconnectNamedPipe(IntPtr A_0);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadFile(IntPtr A_0, byte[] A_1, uint A_2, byte[] A_3, uint A_4);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(IntPtr A_0);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CancelIo(IntPtr A_0);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint GetLastError();

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ConnectNamedPipe(IntPtr A_0, pwnagebot.GameInterface.Frameworks.b.a A_1);

    [StructLayout(LayoutKind.Sequential)]
    internal class a
    {
    }
  }
}
