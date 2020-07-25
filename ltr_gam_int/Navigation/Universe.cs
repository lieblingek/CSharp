// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Navigation.Universe
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;

namespace pwnagebot.GameInterface.Navigation
{
  [Serializable]
  public class Universe : Region
  {
    public Universe()
      : base((Region) null)
    {
      this.Name = "Universe";
    }

    public override bool Contains(float x_, float y_, float z_)
    {
      return true;
    }

    public override Region.Point3f NearestPoint(float x_, float y_, float z_)
    {
      return new Region.Point3f(x_, y_, z_);
    }

    public override Region.Point3f CenterPoint()
    {
      return new Region.Point3f(0.0f, 0.0f, 0.0f);
    }
  }
}
