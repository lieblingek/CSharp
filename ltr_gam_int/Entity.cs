// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Entity
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using pwnagebot.GameInterface.Frameworks;
using System;
using System.Collections.Generic;

namespace pwnagebot.GameInterface
{
  public abstract class Entity
  {
    protected MemoryReader m_reader;
    private int a;
    protected DateTime m_firstSeen;

    public abstract float X { get; }

    public abstract float Y { get; }

    public abstract float Z { get; }

    public abstract float Heading { get; }

    public abstract float Health { get; }

    public abstract bool Dead { get; }

    public abstract string FullName { get; }

    public int MyId
    {
      get
      {
        return this.a;
      }
    }

    public TimeSpan ExistedFor
    {
      get
      {
        return DateTime.Now - this.m_firstSeen;
      }
    }

    public Entity(MemoryReader reader_, int id_)
    {
      this.m_reader = reader_;
      this.a = id_;
      this.m_firstSeen = DateTime.Now;
    }

    public virtual bool Update()
    {
      return true;
    }

    public float DistanceTo(float x_, float y_, float z_)
    {
      float num1 = this.X - x_;
      float num2 = this.Y - y_;
      float num3 = this.Z - z_;
      return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
    }

    public float DistanceTo(Entity entity_)
    {
      return this.DistanceTo(entity_.X, entity_.Y, entity_.Z);
    }

    private double a(double A_0)
    {
      double num = -(A_0 * 180.0 / Math.PI - 90.0);
      if (num < 0.0)
        num += 360.0;
      return num;
    }

    public double HeadingTo(float x_, float y_)
    {
      return this.a(this.GetRadianTo(x_, y_));
    }

    public double GetRadianTo(float x_, float y_)
    {
      double x = (double) x_ - (double) this.X;
      double num = Math.Atan2((double) y_ - (double) this.Y, x);
      if (num < 0.0)
        num += 2.0 * Math.PI;
      return num;
    }

    public class DistanceSorter : IComparer<Entity>
    {
      private Entity a;

      public DistanceSorter(Entity target_)
      {
        this.a = target_;
      }

      public int Compare(Entity obj1, Entity obj2)
      {
        return ((double) obj1.DistanceTo(this.a.X, this.a.Y, this.a.Z)).CompareTo((double) obj2.DistanceTo(this.a.X, this.a.Y, this.a.Z));
      }
    }
  }
}
