// Decompiled with JetBrains decompiler
// Type: pwnagebot.LotroInterface.f
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using pwnagebot.GameInterface;
using pwnagebot.GameInterface.Frameworks;
using System;
using System.Threading;

namespace pwnagebot.LotroInterface
{
  internal class f : Movement
  {
    private const float a = 1f;
    private const float b = -1f;
    private const float c = 0.0f;
    private MemoryReader d;
    private int e;

    public f(Interface A_0, MemoryReader A_1, int A_2)
      : base(A_0)
    {
      this.d = A_1;
      this.e = A_2;
    }

    public virtual void b()
    {
      this.a(1f);
    }

    public virtual void c()
    {
      this.a(-1f);
    }

    public virtual void a()
    {
      this.a(0.0f);
    }

    private void a(float A_0)
    {
      this.d.WriteFloat(this.d.ReadDWORD(this.d.ReadDWORD(this.d.ReadDWORD(this.d.ReadDWORD(this.e) + 100) + 32) + 8) + 4, A_0);
      pwnagebot.LotroInterface.b.b();
    }

    public virtual void b(float A_0)
    {
      int num1 = this.d.ReadDWORD(((LotroEntity) this.InterfaceRef.EntityManager.Me).BasePtr + 4);
      double num2 = Math.Atan2(0.0, 1.0) - ((double) A_0 + Math.PI / 4.0) * (Math.PI / 180.0);
      if (num2 < 0.0)
        num2 += 2.0 * Math.PI;
      double num3 = (Math.PI - num2) / 2.0;
      double num4 = -Math.Sin(num3);
      double num5 = -Math.Cos(num3);
      this.d.WriteFloat(num1 + 140, (float) num4);
      this.d.WriteFloat(num1 + 152, (float) num5);
      pwnagebot.LotroInterface.b.b();
    }

    public virtual void a(float A_0, float A_1)
    {
      Entity me = this.InterfaceRef.EntityManager.Me;
      double num1 = me.HeadingTo(A_0, A_1);
      double num2 = Math.Abs(num1 - (double) me.Heading);
      ((Movement) this).Face((float) num1);
      Thread.Sleep(Math.Max((int) num2 * 3, 200));
    }
  }
}
