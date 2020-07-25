// Decompiled with JetBrains decompiler
// Type: pwnagebot.LotroInterface.LotroMe
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using pwnagebot.GameInterface.Frameworks;
using System.Runtime.InteropServices;

namespace pwnagebot.LotroInterface
{
  public class LotroMe : LotroEntity
  {
    private LotroMe.a a;
    private LotroMe.a b;
    private int c;
    private int d;
    private int e;
    private int f;

    public int MoralePtr
    {
      set
      {
        this.c = value;
      }
    }

    public int PowerPtr
    {
      set
      {
        this.d = value;
      }
    }

    public float Power
    {
      get
      {
        if ((double) this.b.b == 0.0)
          return 0.0f;
        return (float) ((double) this.b.a / (double) this.b.b * 100.0);
      }
    }

    public int AvgDurability
    {
      get
      {
        return this.f;
      }
      set
      {
        this.f = value;
      }
    }

    public int MinDurability
    {
      get
      {
        return this.e;
      }
      set
      {
        this.e = value;
      }
    }

    public LotroMe(MemoryReader reader_, int entityPtr_)
      : base(reader_, entityPtr_)
    {
    }

    public override bool Update()
    {
      if (!base.Update())
        return false;
      this.UpdateVitals();
      return true;
    }

    public void UpdateVitals()
    {
      if (this.c == 0 || this.d == 0)
        return;
      this.m_reader.ReadStruct<LotroMe.a>(this.c, ref this.a);
      this.m_reader.ReadStruct<LotroMe.a>(this.d, ref this.b);
    }

    public override string ToString()
    {
      return string.Format("0x{0:x8}", (object) this.m_entityPtr);
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct a
    {
      [FieldOffset(8)]
      public float a;
      [FieldOffset(12)]
      public float b;
      [FieldOffset(16)]
      public float c;
    }
  }
}
