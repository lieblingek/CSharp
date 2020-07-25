// Decompiled with JetBrains decompiler
// Type: pwnagebot.LotroInterface.WStringID
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using System;
using System.Runtime.InteropServices;

namespace pwnagebot.LotroInterface
{
  [Serializable]
  [StructLayout(LayoutKind.Explicit)]
  internal struct WStringID
  {
    [FieldOffset(0)]
    public ulong a;
    [FieldOffset(0)]
    public int b;
    [FieldOffset(4)]
    public int c;
  }
}
