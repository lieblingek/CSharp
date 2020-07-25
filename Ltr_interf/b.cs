// Decompiled with JetBrains decompiler
// Type: pwnagebot.LotroInterface.b
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using System;
using System.Runtime.InteropServices;

namespace pwnagebot.LotroInterface
{
  internal static class b
  {
    private static object a = new object();
    private static uint b;
    private static uint c;

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetStateIndex(uint A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetGetMePtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetTargetPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetVitalsMangerPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetGetVitalsPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetHotkeyPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetLootPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetUsePtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetLastInputPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetTimerPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetThreadHookPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetTargetHookPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetLootActiveHookPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetLootDeactiveHookPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetStringWatcherHookPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetSkillbarIndexHookPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetAttackersHookPtr(int A_0);

    [DllImport("LOTROInjected.dll")]
    internal static extern void SetClickUpPtr(int A_0);

    [DllImport("DLLInject.dll")]
    internal static extern void InjectDLL(string A_0, string A_1);

    [DllImport("DLLInject.dll")]
    internal static extern void InjectDLLByWindow(IntPtr A_0, string A_1);

    [DllImport("DLLInject.dll")]
    internal static extern void RemoveDLL();

    private static void a(ulong A_0, out uint A_1, out uint A_2)
    {
      A_1 = (uint) (A_0 & (ulong) uint.MaxValue);
      A_2 = (uint) ((A_0 & 18446744069414584320UL) >> 32);
    }

    public static bool d(ulong A_0)
    {
      lock (pwnagebot.LotroInterface.b.a)
      {
        pwnagebot.LotroInterface.b.a(A_0, out pwnagebot.LotroInterface.b.b, out pwnagebot.LotroInterface.b.c);
        return pwnagebot.LotroInterface.b.a.RetrieveVitals(pwnagebot.LotroInterface.b.b, pwnagebot.LotroInterface.b.c);
      }
    }

    public static void c(ulong A_0)
    {
      lock (pwnagebot.LotroInterface.b.a)
      {
        pwnagebot.LotroInterface.b.a(A_0, out pwnagebot.LotroInterface.b.b, out pwnagebot.LotroInterface.b.c);
        pwnagebot.LotroInterface.b.a.DoTarget(pwnagebot.LotroInterface.b.b, pwnagebot.LotroInterface.b.c);
      }
    }

    public static ulong d()
    {
      lock (pwnagebot.LotroInterface.b.a)
        return (ulong) pwnagebot.LotroInterface.b.a.GetTargetGUIDHigh() << 32 | (ulong) (uint) pwnagebot.LotroInterface.b.a.GetTargetGUIDLow();
    }

    public static void a(int A_0)
    {
      lock (pwnagebot.LotroInterface.b.a)
        pwnagebot.LotroInterface.b.a.DoHotkey(A_0);
    }

    public static void b(ulong A_0)
    {
      lock (pwnagebot.LotroInterface.b.a)
      {
        pwnagebot.LotroInterface.b.a(A_0, out pwnagebot.LotroInterface.b.b, out pwnagebot.LotroInterface.b.c);
        pwnagebot.LotroInterface.b.a.Use(pwnagebot.LotroInterface.b.b, pwnagebot.LotroInterface.b.c);
      }
    }

    public static void a(ulong A_0)
    {
      lock (pwnagebot.LotroInterface.b.a)
      {
        pwnagebot.LotroInterface.b.a(A_0, out pwnagebot.LotroInterface.b.b, out pwnagebot.LotroInterface.b.c);
        pwnagebot.LotroInterface.b.a.Loot(pwnagebot.LotroInterface.b.b, pwnagebot.LotroInterface.b.c);
      }
    }

    public static int c()
    {
      lock (pwnagebot.LotroInterface.b.a)
        return pwnagebot.LotroInterface.b.a.GetSkillbarOffset();
    }

    public static void a(int A_0, int A_1, int A_2)
    {
      lock (pwnagebot.LotroInterface.b.a)
        pwnagebot.LotroInterface.b.a.ClickGuiElement(A_0, A_1, A_2);
    }

    public static void b()
    {
      lock (pwnagebot.LotroInterface.b.a)
        ;
    }

    public static void a()
    {
      lock (pwnagebot.LotroInterface.b.a)
        pwnagebot.LotroInterface.b.a.ResetPipe();
    }

    private class a
    {
      [DllImport("LOTROInjected")]
      internal static extern bool RetrieveVitals(uint A_0, uint A_1);

      [DllImport("LOTROInjected.dll")]
      internal static extern void DoTarget(uint A_0, uint A_1);

      [DllImport("LOTROInjected.dll")]
      internal static extern int GetTargetGUIDHigh();

      [DllImport("LOTROInjected.dll")]
      internal static extern int GetTargetGUIDLow();

      [DllImport("LOTROInjected.dll")]
      internal static extern void DoHotkey(int A_0);

      [DllImport("LOTROInjected.dll")]
      internal static extern void Use(uint A_0, uint A_1);

      [DllImport("LOTROInjected.dll")]
      internal static extern void Loot(uint A_0, uint A_1);

      [DllImport("LOTROInjected.dll")]
      internal static extern void ClickGuiElement(int A_0, int A_1, int A_2);

      [DllImport("LOTROInjected.dll")]
      internal static extern void ResetAFKTimer();

      [DllImport("LOTROInjected.dll")]
      internal static extern void ResetPipe();

      [DllImport("LOTROInjected")]
      internal static extern int GetSkillbarOffset();
    }
  }
}
