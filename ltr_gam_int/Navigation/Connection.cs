// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Navigation.Connection
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;

namespace pwnagebot.GameInterface.Navigation
{
  [Serializable]
  public class Connection
  {
    private float m_distance;
    private float m_actualDistance;
    private Region m_sourceRegion;
    private Region m_destRegion;

    public float Distance
    {
      get
      {
        return this.m_distance;
      }
      set
      {
        this.m_distance = value;
      }
    }

    public float ActualDistance
    {
      get
      {
        return this.m_actualDistance;
      }
    }

    public Region To
    {
      get
      {
        return this.m_destRegion;
      }
    }

    public Region From
    {
      get
      {
        return this.m_sourceRegion;
      }
    }

    public Connection(Region source_, Region dest_, float dist_)
    {
      this.m_sourceRegion = source_;
      this.m_destRegion = dest_;
      this.m_distance = dist_;
      this.m_actualDistance = source_.Distance(dest_);
    }

    public override bool Equals(object obj)
    {
      return base.Equals(obj);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
