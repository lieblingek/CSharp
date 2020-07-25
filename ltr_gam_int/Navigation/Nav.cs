// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Navigation.Nav
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace pwnagebot.GameInterface.Navigation
{
  [Serializable]
  public class Nav
  {
    private Region m_root;
    private Dictionary<string, List<Region>> m_groups;
    private ReadOnlyCollection<Connection> m_currentPath;
    private ReadOnlyCollection<Region> m_grindSpots;
    private Region m_currentGrindSpot;

    public Universe Tree
    {
      get
      {
        return (Universe) this.m_root;
      }
      set
      {
        this.m_root = (Region) value;
      }
    }

    public ReadOnlyCollection<Connection> CurrentPath
    {
      get
      {
        return this.m_currentPath;
      }
      set
      {
        this.m_currentPath = value;
      }
    }

    public ReadOnlyCollection<Region> GrindSpots
    {
      get
      {
        return this.m_grindSpots;
      }
      set
      {
        this.m_grindSpots = value;
      }
    }

    public Region CurrentGrindSpot
    {
      get
      {
        return this.m_currentGrindSpot;
      }
      set
      {
        this.m_currentGrindSpot = value;
      }
    }

    public Nav()
    {
      this.m_groups = new Dictionary<string, List<Region>>();
      this.m_root = (Region) new Universe();
    }

    public Region GetClosestRegion(float x_, float y_, float z_)
    {
      ReadOnlyCollection<Region> regionsByDistance = this.GetAllRegionsByDistance(x_, y_, z_);
      if (regionsByDistance.Count == 0)
        return (Region) null;
      return regionsByDistance[0];
    }

    private void GetAllChildren(Region region_, List<Region> children_)
    {
      children_.AddRange((IEnumerable<Region>) region_.Children);
      foreach (Region child in region_.Children)
        this.GetAllChildren(child, children_);
    }

    public ReadOnlyCollection<Region> GetAllRegionsByDistance(float x_, float y_, float z_)
    {
      List<Region> children_ = new List<Region>();
      this.GetAllChildren(this.m_root, children_);
      if (children_.Count == 0)
        return (ReadOnlyCollection<Region>) null;
      Nav.DistanceSorter distanceSorter = new Nav.DistanceSorter(x_, y_, z_);
      children_.Sort((IComparer<Region>) distanceSorter);
      return new ReadOnlyCollection<Region>((IList<Region>) children_);
    }

    public void AddRegionToGroup(string name_, Region region_)
    {
      if (this.m_groups.ContainsKey(name_))
      {
        List<Region> group = this.m_groups[name_];
        if (group.Contains(region_))
          return;
        group.Add(region_);
      }
      else
        this.m_groups.Add(name_, new List<Region>() { region_ });
    }

    public ReadOnlyCollection<Region> GetRegionGroup(string name_)
    {
      return !this.m_groups.ContainsKey(name_) ? new ReadOnlyCollection<Region>((IList<Region>) new List<Region>()) : new ReadOnlyCollection<Region>((IList<Region>) this.m_groups[name_]);
    }

    public void RemoveRegionFromGroup(string name_, Region region_)
    {
      if (!this.m_groups.ContainsKey(name_))
        return;
      List<Region> group = this.m_groups[name_];
      if (!group.Contains(region_))
        return;
      group.Remove(region_);
    }

    private class DistanceSorter : IComparer<Region>
    {
      private float m_x;
      private float m_y;
      private float m_z;

      public DistanceSorter(float x_, float y_, float z_)
      {
        this.m_x = x_;
        this.m_y = y_;
        this.m_z = z_;
      }

      public int Compare(Region obj1, Region obj2)
      {
        return ((double) obj1.Distance(this.m_x, this.m_y, this.m_z)).CompareTo((double) obj2.Distance(this.m_x, this.m_y, this.m_z));
      }
    }
  }
}
