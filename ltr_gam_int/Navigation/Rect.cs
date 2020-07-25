// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Navigation.Rect
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;

namespace pwnagebot.GameInterface.Navigation
{
  [Serializable]
  internal class Rect : Region
  {
    private float m_x1;
    private float m_x2;
    private float m_y1;
    private float m_y2;

    public Rect(Region parent_, float x1_, float x2_, float y1_, float y2_)
      : base(parent_)
    {
      if ((double) x1_ >= (double) x2_ || (double) y1_ >= (double) y2_)
        throw new ArgumentOutOfRangeException("x1_ must be < x2_ and y1_ must be < y2_");
      this.m_x1 = x1_;
      this.m_x2 = x2_;
      this.m_y1 = y1_;
      this.m_y2 = y2_;
    }

    public override bool Contains(float x_, float y_, float z_)
    {
      if ((double) x_ > (double) this.m_x1 && (double) x_ < (double) this.m_x2 && (double) y_ > (double) this.m_y1)
        return (double) y_ < (double) this.m_y2;
      return false;
    }

    public override Region.Point3f CenterPoint()
    {
      return new Region.Point3f((float) (((double) this.m_x2 + (double) this.m_x1) / 2.0), (float) (((double) this.m_y2 + (double) this.m_y1) / 2.0), 0.0f);
    }

    public override Region.Point3f NearestPoint(float x_, float y_, float z_)
    {
      return new Region.Point3f(Math.Max(this.m_x1, Math.Min(this.m_x2, x_)), Math.Max(this.m_y1, Math.Min(this.m_y2, y_)), z_);
    }
  }
}
