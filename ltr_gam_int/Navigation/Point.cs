// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Navigation.Point
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;

namespace pwnagebot.GameInterface.Navigation
{
  [Serializable]
  internal class Point : Region
  {
    private Region.Point3f m_p;

    public Point(Region parent_, float x_, float y_, float z_)
      : base(parent_)
    {
      this.m_p = new Region.Point3f(x_, y_, z_);
    }

    public override bool Contains(float x_, float y_, float z_)
    {
      if ((double) this.m_p.X == (double) x_ && (double) this.m_p.Y == (double) y_)
        return (double) this.m_p.Z == (double) z_;
      return false;
    }

    public override Region.Point3f NearestPoint(float x_, float y_, float z_)
    {
      return this.m_p;
    }

    public override Region.Point3f CenterPoint()
    {
      return this.m_p;
    }
  }
}
