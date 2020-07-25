// Decompiled with JetBrains decompiler
// Type: pwnagebot.LotroInterface.LotroEntity
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using pwnagebot.GameInterface;
using pwnagebot.GameInterface.Frameworks;
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace pwnagebot.LotroInterface
{
  public class LotroEntity : Entity
  {
    protected int m_entityPtr;
    private LotroEntity.b a;
    private LotroEntity.c b;
    private LotroEntity.c c;
    private bool d;
    private bool e;
    private bool f;
    private bool g;
    private DateTime h;
    private LotroEntity.WGUID i;
    private int j;
    private int k;
    private string l;
    private int? m;
    private int? n;
    private float? o;
    private int? p;
    private int? q;
    private int? r;
    private int? s;
    private int? t;
    private float? u;
    private float? v;
    private int? w;
    private int? x;
    private int? y;
    private int? z;

    public int BasePtr
    {
      get
      {
        return this.m_entityPtr;
      }
    }

    public string Name
    {
      get
      {
        return this.l;
      }
    }

    public bool ValidName
    {
      get
      {
        return this.e;
      }
    }

    public override string FullName
    {
      get
      {
        string str1 = string.Empty;
        if (this.o.HasValue && (double) this.o.Value == 0.0)
          str1 += (string) (object) 'D';
        if (this.CombatActive)
          str1 += (string) (object) 'X';
        if (this.Lootable)
          str1 += (string) (object) 'L';
        if (this.n.HasValue)
          str1 = str1 + "[" + (object) this.n + "] ";
        string str2 = str1 + this.l;
        switch (this.Difficulty)
        {
          case LotroEntity.LotroEntityDifficulty.SWARM:
            str2 += "[sw]";
            goto case LotroEntity.LotroEntityDifficulty.NORMAL;
          case LotroEntity.LotroEntityDifficulty.ELITE_MASTER:
            str2 += "[em]";
            goto case LotroEntity.LotroEntityDifficulty.NORMAL;
          case LotroEntity.LotroEntityDifficulty.NEMESIS:
            str2 += "[ne]";
            goto case LotroEntity.LotroEntityDifficulty.NORMAL;
          case LotroEntity.LotroEntityDifficulty.SIGNATURE:
            str2 += "[s]";
            goto case LotroEntity.LotroEntityDifficulty.NORMAL;
          case LotroEntity.LotroEntityDifficulty.NORMAL:
            float valueOrDefault = this.o.GetValueOrDefault(0.0f);
            if ((double) valueOrDefault > 0.0)
              str2 += string.Format(" ({0:.}%)", (object) (float) ((double) valueOrDefault * 100.0));
            return str2;
          case LotroEntity.LotroEntityDifficulty.ARCH_NEMESIS:
            str2 += "[an]";
            goto case LotroEntity.LotroEntityDifficulty.NORMAL;
          case LotroEntity.LotroEntityDifficulty.ELITE:
            str2 += "[e]";
            goto case LotroEntity.LotroEntityDifficulty.NORMAL;
          case LotroEntity.LotroEntityDifficulty.DEFENDER:
            str2 += "[d]";
            goto case LotroEntity.LotroEntityDifficulty.NORMAL;
          case LotroEntity.LotroEntityDifficulty.UNKNOWN:
            if (this.m.HasValue)
            {
              str2 = str2 + "[" + this.m.Value.ToString() + "]";
              goto case LotroEntity.LotroEntityDifficulty.NORMAL;
            }
            else
              goto case LotroEntity.LotroEntityDifficulty.NORMAL;
          default:
            str2 += "???";
            goto case LotroEntity.LotroEntityDifficulty.NORMAL;
        }
      }
    }

    public LotroEntity.LotroEntityType Type
    {
      get
      {
        switch ((int) byte.MaxValue & this.i.High >> 24)
        {
          case 2:
            return LotroEntity.LotroEntityType.PLAYER;
          case 3:
            return !this.u.HasValue ? LotroEntity.LotroEntityType.NODE : LotroEntity.LotroEntityType.ENTITY;
          case 4:
            return LotroEntity.LotroEntityType.WORLDOBJ;
          case 128:
            return LotroEntity.LotroEntityType.ITEM;
          default:
            return LotroEntity.LotroEntityType.UNKNOWN;
        }
      }
    }

    public ulong Guid
    {
      get
      {
        return this.i.GUID;
      }
    }

    public LotroEntity.LotroEntityDifficulty Difficulty
    {
      get
      {
        if (!this.m.HasValue)
          return LotroEntity.LotroEntityDifficulty.UNKNOWN;
        return (LotroEntity.LotroEntityDifficulty) this.m.Value;
      }
    }

    public int Level
    {
      get
      {
        return this.n.GetValueOrDefault(-1);
      }
    }

    public override float Health
    {
      get
      {
        if (this.o.HasValue)
          return this.o.Value * 100f;
        return float.NaN;
      }
    }

    public override bool Dead
    {
      get
      {
        return (double) this.Health <= 0.0;
      }
    }

    public bool Lootable
    {
      get
      {
        return this.p.GetValueOrDefault(0) == 1;
      }
    }

    public int? CombatMode
    {
      get
      {
        return this.q;
      }
    }

    public bool IsRangedAttacker
    {
      get
      {
        if (this.q.GetValueOrDefault(0) != 2)
          return this.q.GetValueOrDefault(0) == 3;
        return true;
      }
    }

    public bool IsMeleeAttacker
    {
      get
      {
        return this.q.GetValueOrDefault(0) == 2;
      }
    }

    public bool CombatActive
    {
      get
      {
        return this.r.GetValueOrDefault(0) != 0;
      }
    }

    public int Fervour
    {
      get
      {
        return this.s.GetValueOrDefault(0);
      }
    }

    public int Focus
    {
      get
      {
        return this.t.GetValueOrDefault(0);
      }
    }

    public float MaxSpeed
    {
      get
      {
        return this.u.GetValueOrDefault(0.0f);
      }
    }

    public float CurrentSpeed
    {
      get
      {
        return this.v.GetValueOrDefault(0.0f);
      }
    }

    public bool FervourBuff
    {
      get
      {
        if (this.w.HasValue)
          return this.w.Value == 1;
        return false;
      }
    }

    public int Durability
    {
      get
      {
        return this.y.GetValueOrDefault(-1);
      }
    }

    public int Slot
    {
      get
      {
        return this.z.GetValueOrDefault(-1);
      }
    }

    public override float X
    {
      get
      {
        return (float) ((int) this.b.b * 160) + this.b.f;
      }
    }

    public override float Y
    {
      get
      {
        return (float) ((int) this.b.c * 160) + this.b.g;
      }
    }

    public override float Z
    {
      get
      {
        return this.b.h;
      }
    }

    public override float Heading
    {
      get
      {
        return 360f - (float) (Math.Asin((double) this.b.i) * 360.0 / Math.PI + 180.0);
      }
    }

    public bool EnableUpdates
    {
      get
      {
        return this.g;
      }
      set
      {
        this.g = value;
      }
    }

    public bool IsInvalid
    {
      get
      {
        return this.d;
      }
    }

    public LotroEntity(MemoryReader reader_, int entityPtr_)
      : base(reader_, entityPtr_)
    {
      this.m_entityPtr = entityPtr_;
      this.l = string.Format("0x{0:x8}", (object) this.m_entityPtr);
      this.f = true;
      this.g = true;
    }

    public override bool Update()
    {
      if (!this.g)
        return true;
      this.UpdateBasic();
      if (this.d)
        return false;
      if (this.f)
      {
        this.UpdateOneTime();
        this.f = false;
      }
      if (this.Type == LotroEntity.LotroEntityType.ITEM)
        this.g = false;
      return true;
    }

    public void UpdateBasic()
    {
      this.m_reader.ReadStruct<LotroEntity.b>(this.m_entityPtr, ref this.a);
      if (this.f)
      {
        this.i.GUID = this.a.c.GUID;
        this.l = string.Format("0x{0:x16}", (object) this.i.GUID);
      }
      else if ((long) this.i.GUID != (long) this.a.c.GUID)
      {
        this.d = true;
        return;
      }
      this.m_reader.ReadStruct<LotroEntity.c>(this.a.a + 80, ref this.b);
      this.e();
      if (this.j == 0)
        this.d();
      this.a();
    }

    public void UpdateOneTime()
    {
      this.m_reader.ReadStruct<LotroEntity.c>(this.a.b, ref this.c);
    }

    private int? b(pwnagebot.LotroInterface.a A_0, int A_1)
    {
      int num = A_0.a(A_1);
      if (num == 0)
        return new int?();
      return new int?(this.m_reader.ReadDWORD(num + 12));
    }

    private float? a(pwnagebot.LotroInterface.a A_0, int A_1)
    {
      int num = A_0.a(A_1);
      if (num == 0)
        return new float?();
      return new float?(this.m_reader.ReadFloat(num + 12));
    }

    private void e()
    {
      int num = this.m_reader.ReadDWORD(this.m_entityPtr + 96);
      if (num == 0)
        return;
      pwnagebot.LotroInterface.a A_0 = new pwnagebot.LotroInterface.a(this.m_reader, num + 32, 1);
      this.m = this.b(A_0, 268438084);
      this.n = this.b(A_0, 268439569);
      this.o = this.a(A_0, 268439560);
      this.p = this.b(A_0, 268438067);
      this.q = this.b(A_0, 268437988);
      this.r = this.b(A_0, 268438638);
      this.s = this.b(A_0, 268437184);
      this.t = this.b(A_0, 268435744);
      this.u = this.a(A_0, 577);
      this.v = this.a(A_0, 669);
      this.w = this.b(A_0, 268441400);
      this.y = this.b(A_0, 268440364);
      this.z = this.b(A_0, 846);
    }

    public override string ToString()
    {
      return this.l;
    }

    private void d()
    {
      int num1 = 0;
      if (this.m_entityPtr == 0)
        return;
      int num2 = this.m_reader.ReadDWORD(this.m_entityPtr + 96);
      if (num2 == 0)
        return;
      int num3 = this.m_reader.ReadDWORD(num2 + 52);
      if (num3 == 0)
        return;
      int num4 = this.m_reader.ReadDWORD(num3 + 28);
      int num5 = this.m_reader.ReadDWORD(num3 + 36);
      if (num4 == 0 || num5 == 0)
        return;
      int num6 = 852 % num5;
      for (int address = this.m_reader.ReadDWORD(num4 + num6 * 4); address != 0 && num1++ < 2000; address = this.m_reader.ReadDWORD(address + 4))
      {
        if (this.m_reader.ReadDWORD(address) == 852)
        {
          int num7 = this.m_reader.ReadDWORD(address + 12);
          if (num7 != 0)
          {
            this.j = this.m_reader.ReadDWORD(num7 + 12);
            this.k = this.m_reader.ReadDWORD(num7 + 16);
          }
        }
      }
    }

    private bool c()
    {
      int num1 = 0;
      if (this.m_entityPtr == 0)
        return false;
      int num2 = this.m_reader.ReadDWORD(this.m_entityPtr + 96);
      if (num2 == 0)
        return false;
      int num3 = this.m_reader.ReadDWORD(num2 + 32);
      int num4 = this.m_reader.ReadDWORD(num2 + 40);
      if (num3 == 0 || num4 == 0)
        return false;
      int num5 = 852 % num4;
      int address = this.m_reader.ReadDWORD(num3 + num5 * 4);
      while (address != 0 && num1++ < 2000)
      {
        int num6 = this.m_reader.ReadDWORD(address);
        int num7 = this.m_reader.ReadDWORD(address + 12);
        if (num6 != 852)
        {
          address = this.m_reader.ReadDWORD(address + 4);
        }
        else
        {
          int iAddress = this.m_reader.ReadDWORD(num7 + 44);
          if (iAddress == 0)
          {
            address = this.m_reader.ReadDWORD(address + 4);
          }
          else
          {
            this.l = this.m_reader.ReadUnicodeString(iAddress, 256);
            Match match = Regex.Match(this.l, "(.*)\\[([A-Za-z]*)\\]");
            if (match.Success)
              this.l = match.Groups[1].Value;
            return true;
          }
        }
      }
      return false;
    }

    private void b()
    {
      if (this.j == 0)
        return;
      string str = pwnagebot.LotroInterface.e.b(this.m_reader, this.k, this.j);
      if (str == string.Empty)
      {
        this.l = string.Format("{0:x8}-{1:x8}", (object) this.k, (object) this.j);
      }
      else
      {
        this.l = str;
        this.e = true;
      }
    }

    private void a()
    {
      if (this.e || (DateTime.Now - this.h).TotalMilliseconds < (double) (2000 + new Random((int) DateTime.Now.Ticks).Next(0, 500)))
        return;
      this.h = DateTime.Now;
      if (this.c() && this.l != string.Empty)
      {
        this.e = true;
      }
      else
      {
        this.b();
        Match match = Regex.Match(this.l, "(.*)\\[([A-Za-z]*)\\]");
        if (!match.Success)
          return;
        this.l = match.Groups[1].Value;
      }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct WGUID
    {
      [FieldOffset(0)]
      public ulong GUID;
      [FieldOffset(0)]
      public int Low;
      [FieldOffset(4)]
      public int High;
    }

    public enum LotroEntityType
    {
      PLAYER,
      ENTITY,
      NODE,
      ITEM,
      WORLDOBJ,
      UNKNOWN,
    }

    public enum LotroEntityDifficulty
    {
      SWARM = 1,
      ELITE_MASTER = 2,
      NEMESIS = 3,
      SIGNATURE = 4,
      NORMAL = 5,
      ARCH_NEMESIS = 6,
      ELITE = 7,
      DEFENDER = 8,
      UNKNOWN = 9,
    }

    private enum a
    {
      i = 577,
      j = 669,
      m = 846,
      h = 268435744,
      g = 268437184,
      e = 268437988,
      d = 268438067,
      a = 268438084,
      f = 268438638,
      c = 268439560,
      b = 268439569,
      l = 268440364,
      k = 268441400,
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct b
    {
      [FieldOffset(4)]
      public int a;
      [FieldOffset(28)]
      public int b;
      [FieldOffset(144)]
      public LotroEntity.WGUID c;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct c
    {
      [FieldOffset(36)]
      public int a;
      [FieldOffset(40)]
      public byte b;
      [FieldOffset(41)]
      public byte c;
      [FieldOffset(42)]
      public byte d;
      [FieldOffset(44)]
      public byte e;
      [FieldOffset(48)]
      public float f;
      [FieldOffset(52)]
      public float g;
      [FieldOffset(56)]
      public float h;
      [FieldOffset(60)]
      public float i;
      [FieldOffset(72)]
      public float j;
    }
  }
}
