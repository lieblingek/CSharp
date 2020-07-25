// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Frameworks.a
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.Runtime.InteropServices;

namespace pwnagebot.GameInterface.Frameworks
{
  internal sealed class a
  {
    public const ushort a = 2;
    private const int b = 32;
    private const int c = 8;
    private const int d = 2;
    private const string e = "SeDebugPrivilege";

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint GetLastError();

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(uint A_0, out uint A_1);

    [DllImport("User32")]
    public static extern uint FindWindow(string A_0, string A_1);

    [DllImport("User32")]
    public static extern bool SetForegroundWindow(IntPtr A_0);

    [DllImport("User32")]
    public static extern uint GetForegroundWindow();

    [DllImport("Kernel32", SetLastError = true)]
    public static extern IntPtr OpenProcess(pwnagebot.GameInterface.Frameworks.a.c A_0, bool A_1, uint A_2);

    [DllImport("Kernel32")]
    public static extern bool CloseHandle(IntPtr A_0);

    [DllImport("Kernel32")]
    public static extern bool ReadProcessMemory(IntPtr A_0, IntPtr A_1, IntPtr A_2, int A_3, out int A_4);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern int VirtualProtectEx(IntPtr A_0, IntPtr A_1, int A_2, int A_3, out int A_4);

    [DllImport("kernel32.dll")]
    public static extern int WriteProcessMemory(IntPtr A_0, IntPtr A_1, IntPtr A_2, int A_3, ref IntPtr A_4);

    [DllImport("User32")]
    public static extern int PostMessage(IntPtr A_0, int A_1, int A_2, int A_3);

    [DllImport("User32")]
    public static extern int SendMessage(IntPtr A_0, int A_1, int A_2, int A_3);

    [DllImport("User32", EntryPoint = "MapVirtualKey")]
    public static extern int MapVirtualKeyA(int A_0, int A_1);

    [DllImport("User32")]
    public static extern void keybd_event(char A_0, char A_1, int A_2, int A_3);

    [DllImport("user32.dll")]
    public static extern void mouse_event(uint A_0, uint A_1, uint A_2, uint A_3, IntPtr A_4);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
    private static extern int OpenProcessToken(int A_0, int A_1, ref int A_2);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    private static extern int GetCurrentProcess();

    [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
    private static extern int LookupPrivilegeValue(string A_0, string A_1, [MarshalAs(UnmanagedType.Struct)] ref pwnagebot.GameInterface.Frameworks.a.b A_2);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
    private static extern int AdjustTokenPrivileges(int A_0, int A_1, [MarshalAs(UnmanagedType.Struct)] ref pwnagebot.GameInterface.Frameworks.a.a A_2, int A_3, int A_4, int A_5);

    public static void a()
    {
      pwnagebot.GameInterface.Frameworks.a.a A_2_1 = new pwnagebot.GameInterface.Frameworks.a.a();
      pwnagebot.GameInterface.Frameworks.a.b A_2_2 = new pwnagebot.GameInterface.Frameworks.a.b();
      int A_2_3 = 0;
      pwnagebot.GameInterface.Frameworks.a.OpenProcessToken(pwnagebot.GameInterface.Frameworks.a.GetCurrentProcess(), 40, ref A_2_3);
      pwnagebot.GameInterface.Frameworks.a.LookupPrivilegeValue((string) null, "SeDebugPrivilege", ref A_2_2);
      A_2_1.c = 1;
      A_2_1.b = 2;
      A_2_1.a = A_2_2;
      pwnagebot.GameInterface.Frameworks.a.AdjustTokenPrivileges(A_2_3, 0, ref A_2_1, 1024, 0, 0);
    }

    [Flags]
    public enum c : uint
    {
      a = 1,
      b = 2,
      c = 8,
      d = 16,
      e = 32,
      f = 64,
      g = 128,
      h = 256,
      i = 512,
      j = 1024,
      k = 1048576,
      l = 2035711,
    }

    private struct b
    {
      public int a;
      public int b;
    }

    private struct a
    {
      public pwnagebot.GameInterface.Frameworks.a.b a;
      public int b;
      public int c;
    }
  }
}
