// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Navigation.Region
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace pwnagebot.GameInterface.Navigation
{
  [Serializable]
  public abstract class Region
  {
    private List<Region> m_children = new List<Region>();
    private List<Connection> m_connections = new List<Connection>();
    private Hashtable m_attributes = new Hashtable();
    private string m_name;
    private Region m_parent;

    public string Name
    {
      get
      {
        return this.m_name;
      }
      set
      {
        this.m_name = value;
      }
    }

    public string FullName
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder(this.m_name);
        for (Region parent = this.Parent; parent != null; parent = parent.Parent)
          stringBuilder.Insert(0, parent.Name + ".");
        return stringBuilder.ToString();
      }
    }

    public ReadOnlyCollection<Region> Children
    {
      get
      {
        List<Region> regionList = new List<Region>();
        regionList.Capacity = this.m_children.Count;
        regionList.AddRange((IEnumerable<Region>) this.m_children);
        return new ReadOnlyCollection<Region>((IList<Region>) regionList);
      }
    }

    public ReadOnlyCollection<Connection> Connections
    {
      get
      {
        return new ReadOnlyCollection<Connection>((IList<Connection>) new List<Connection>((IEnumerable<Connection>) this.m_connections));
      }
    }

    public Hashtable Attributes
    {
      get
      {
        return this.m_attributes;
      }
    }

    public Region Parent
    {
      get
      {
        return this.m_parent;
      }
    }

    public Region(Region parent_)
    {
      this.m_parent = parent_;
    }

    public abstract Region.Point3f CenterPoint();

    public abstract bool Contains(float x_, float y_, float z_);

    public abstract Region.Point3f NearestPoint(float x_, float y_, float z_);

    public object GetAttribute(string name_)
    {
      return this.m_attributes[(object) name_];
    }

    public void SetAttribute(string name_, object value_)
    {
      if (this.m_attributes.Contains((object) name_))
        this.m_attributes[(object) name_] = value_;
      else
        this.m_attributes.Add((object) name_, value_);
    }

    public void AddChild(Region child_)
    {
      if (!this.Contains(child_))
        throw new ArgumentException("Invalid child region: region is not contained by it's parent.");
      if (this.m_children.Contains(child_))
        return;
      this.m_children.Add(child_);
      child_.m_parent = this;
    }

    public void RemoveChild(Region child_)
    {
      if (!this.m_children.Contains(child_))
        throw new ArgumentException("parent does not own child region.");
      this.m_children.Remove(child_);
    }

    public void Connect(Region region_)
    {
      foreach (Connection connection in this.m_connections)
      {
        if (connection.To == region_)
          return;
      }
      this.m_connections.Add(new Connection(this, region_, this.Distance(region_)));
    }

    public virtual bool Contains(Region region_)
    {
      Region.Point3f point3f = this.CenterPoint();
      return this.Contains(point3f.X, point3f.Y, point3f.Z);
    }

    public float Distance(Region to_)
    {
      return this.CenterPoint().Distance(to_.CenterPoint());
    }

    public float Distance(float x_, float y_, float z_)
    {
      return this.CenterPoint().Distance(new Region.Point3f(x_, y_, z_));
    }

    public Region BestContainer(float x_, float y_, float z_)
    {
      return this.BestContainer(true, x_, y_, z_);
    }

    private Region BestContainer(bool traverseUp_, float x_, float y_, float z_)
    {
      if (this.Contains(x_, y_, z_))
      {
        foreach (Region child in this.Children)
        {
          Region region = child.BestContainer(false, x_, y_, z_);
          if (region != null)
            return region;
        }
        return this;
      }
      if (traverseUp_)
        return this.Parent.BestContainer(true, x_, y_, z_);
      return (Region) null;
    }

    public ReadOnlyCollection<Region> ChildrenWithin(float x_, float y_, float z_, float maxDist_)
    {
      List<Region> regionList = new List<Region>();
      foreach (Region child in this.m_children)
      {
        if ((double) child.Distance(x_, y_, z_) < (double) maxDist_)
          regionList.Add(child);
      }
      return new ReadOnlyCollection<Region>((IList<Region>) regionList);
    }

    public ReadOnlyCollection<Region> ChildrenWithin(Region.Point3f point_, float maxDist_)
    {
      return this.ChildrenWithin(point_.X, point_.Y, point_.Z, maxDist_);
    }

    public override string ToString()
    {
      Region.Point3f point3f = this.CenterPoint();
      return string.Format("{0:0.0}, {1:0.0}, {2:0.0}", (object) point3f.X, (object) point3f.Y, (object) point3f.Z);
    }

    [Serializable]
    public struct Point3f
    {
      public float X;
      public float Y;
      public float Z;

      public Point3f(float x_, float y_, float z_)
      {
        this.X = x_;
        this.Y = y_;
        this.Z = z_;
      }

      public float Distance(Region.Point3f other_)
      {
        return (float) Math.Sqrt(Math.Pow((double) other_.X - (double) this.X, 2.0) + Math.Pow((double) other_.Y - (double) this.Y, 2.0) + Math.Pow((double) other_.Z - (double) this.Z, 2.0));
      }
    }
  }
}
