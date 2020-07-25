// Decompiled with JetBrains decompiler
// Type: pwnagebot.LotroInterface.LotroFellowshipEntity
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using pwnagebot.GameInterface;
using pwnagebot.GameInterface.Frameworks;
using System;
using System.Runtime.InteropServices;

namespace pwnagebot.LotroInterface
{
  public class LotroFellowshipEntity : Entity
  {
    private int a;
    private LotroEntity b;
    private LotroFellowshipEntity.a c;
    private bool d;
    private bool e;
    private bool f;
    private ulong g;
    private string h;

    public bool IsLeader
    {
      get
      {
        return this.d;
      }
      set
      {
        this.d = value;
      }
    }

    public LotroEntity EntityRef
    {
      get
      {
        return this.b;
      }
      set
      {
        this.b = value;
      }
    }

    public override string FullName
    {
      get
      {
        string str = "Fellow-";
        return this.b == null ? str + this.h : str + this.b.FullName;
      }
    }

    public override bool Dead
    {
      get
      {
        if (this.b != null)
          return this.b.Dead;
        return false;
      }
    }

    public override float Health
    {
      get
      {
        if (this.b != null)
          return this.b.Health;
        return float.NaN;
      }
    }

    public override float Heading
    {
      get
      {
        return 360f - (float) (Math.Asin((double) this.c.j) * 360.0 / Math.PI + 180.0);
      }
    }

    public override float X
    {
      get
      {
        return (float) ((int) this.c.c * 160) + this.c.g;
      }
    }

    public override float Y
    {
      get
      {
        return (float) ((int) this.c.d * 160) + this.c.h;
      }
    }

    public override float Z
    {
      get
      {
        return this.c.i;
      }
    }

    public ulong Guid
    {
      get
      {
        return this.g;
      }
    }

    public LotroFellowshipEntity(MemoryReader reader_, int entityPtr_)
      : base(reader_, entityPtr_)
    {
      this.a = entityPtr_;
      this.f = true;
    }

    public override bool Update()
    {
      if (this.e)
        return false;
      this.m_reader.ReadStruct<LotroFellowshipEntity.a>(this.a, ref this.c);
      if (this.f)
      {
        this.g = this.c.a;
        this.h = string.Format("0x{0:x16}", (object) this.g);
      }
      else if ((long) this.g != (long) this.c.a || this.b != null && this.b.IsInvalid)
      {
        this.e = true;
        return false;
      }
      return true;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct a
    {
      [FieldOffset(0)]
      public ulong a;
      [FieldOffset(76)]
      public int b;
      [FieldOffset(80)]
      public byte c;
      [FieldOffset(81)]
      public byte d;
      [FieldOffset(82)]
      public byte e;
      [FieldOffset(84)]
      public byte f;
      [FieldOffset(88)]
      public float g;
      [FieldOffset(92)]
      public float h;
      [FieldOffset(96)]
      public float i;
      [FieldOffset(100)]
      public float j;
      [FieldOffset(112)]
      public float k;
    }
  }
}
