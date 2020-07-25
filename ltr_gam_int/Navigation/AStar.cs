// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Navigation.AStar
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using pwnagebot.GameInterface.Frameworks;
using pwnagebot.GameInterface.Frameworks.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wintellect.PowerCollections;

namespace pwnagebot.GameInterface.Navigation
{
  public class AStar : Pathfinder
  {
    private static FileLog s_log = Singleton<FileLog>.Instance;

    public override ReadOnlyCollection<Connection> FindPath(Region source_, Region dest_)
    {
      OrderedBag<Pathfinder.PathNode> orderedBag1 = new OrderedBag<Pathfinder.PathNode>((IComparer<Pathfinder.PathNode>) new AStar.NodeComparer());
      OrderedBag<Pathfinder.PathNode> orderedBag2 = new OrderedBag<Pathfinder.PathNode>((IComparer<Pathfinder.PathNode>) new AStar.NodeComparer());
      Pathfinder.PathNode pathNode1 = new Pathfinder.PathNode();
      DateTime now = DateTime.Now;
      pathNode1.ConnectCost = 0.0f;
      pathNode1.EstimatedCost = 0.0f;
      pathNode1.TotalCost = 0.0f;
      pathNode1.RegionRef = source_;
      pathNode1.Parent = (Pathfinder.PathNode) null;
      pathNode1.CurrentLink = (Connection) null;
      orderedBag1.Add(pathNode1);
      while (orderedBag1.Count != 0)
      {
        Pathfinder.PathNode pathNode2 = orderedBag1.RemoveFirst();
        if (pathNode2.RegionRef == dest_)
        {
          Pathfinder.PathNode pathNode3 = pathNode2;
          List<Connection> connectionList = new List<Connection>();
          for (; pathNode3.Parent != null; pathNode3 = pathNode3.Parent)
            connectionList.Add(pathNode3.CurrentLink);
          connectionList.Reverse();
          TimeSpan timeSpan = DateTime.Now - now;
          AStar.s_log.Debug("PathFind finishTime: " + (object) timeSpan.TotalMilliseconds);
          return new ReadOnlyCollection<Connection>((IList<Connection>) connectionList);
        }
        int count1 = pathNode2.RegionRef.Connections.Count;
        for (int index1 = 0; index1 < count1; ++index1)
        {
          Connection connection = pathNode2.RegionRef.Connections[index1];
          bool flag = false;
          int count2 = orderedBag1.Count;
          for (int index2 = 0; index2 < count2; ++index2)
          {
            Pathfinder.PathNode pathNode3 = orderedBag1[index2];
            if (pathNode3.RegionRef == connection.To)
            {
              if ((double) pathNode2.ConnectCost + (double) connection.Distance < (double) pathNode3.ConnectCost)
              {
                pathNode3.ConnectCost = pathNode2.ConnectCost + connection.Distance;
                pathNode3.TotalCost = pathNode3.ConnectCost + pathNode3.EstimatedCost;
                pathNode3.Parent = pathNode2;
                pathNode3.CurrentLink = connection;
                orderedBag1.Remove(pathNode3);
                orderedBag1.Add(pathNode3);
              }
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            int count3 = orderedBag2.Count;
            for (int index2 = 0; index2 < count3; ++index2)
            {
              if (orderedBag2[index2].RegionRef == connection.To)
              {
                flag = true;
                break;
              }
            }
          }
          if (!flag)
          {
            Pathfinder.PathNode pathNode3 = new Pathfinder.PathNode();
            pathNode3.Parent = pathNode2;
            pathNode3.RegionRef = connection.To;
            pathNode3.ConnectCost = pathNode2.ConnectCost + connection.Distance;
            pathNode3.EstimatedCost = this.Heuristic(pathNode2.RegionRef, dest_);
            pathNode3.TotalCost = pathNode3.ConnectCost + pathNode3.EstimatedCost;
            pathNode3.CurrentLink = connection;
            orderedBag1.Add(pathNode3);
          }
        }
        orderedBag2.Add(pathNode2);
      }
      TimeSpan timeSpan1 = DateTime.Now - now;
      AStar.s_log.Debug("PathFind failed finishTime: " + (object) timeSpan1.TotalMilliseconds);
      return (ReadOnlyCollection<Connection>) null;
    }

    private float Heuristic(Region m_src, Region m_dest)
    {
      return m_dest.Distance(m_src);
    }

    public void TestMyNavThing()
    {
      Universe universe = new Universe();
      Point point1 = (Point) null;
      Point point2 = (Point) null;
      for (int index1 = 0; index1 < 20; ++index1)
      {
        for (int index2 = 0; index2 < 20; ++index2)
        {
          Point point3 = new Point((Region) universe, (float) index1, (float) index2, 0.0f);
          if (index1 == 19 && index2 == 19)
            point1 = point3;
          if (index1 == 0 && index2 == 0)
            point2 = point3;
          if (index1 < 5 || index1 > 15 || (index2 < 5 || index2 > 15))
          {
            foreach (Region child in universe.Children)
            {
              if ((double) child.Distance((Region) point3) < 2.0)
              {
                point3.Connect(child);
                child.Connect((Region) point3);
              }
            }
            universe.AddChild((Region) point3);
          }
        }
      }
      this.FindPath((Region) point1, (Region) point2);
    }

    private class NodeComparer : IComparer<Pathfinder.PathNode>
    {
      public int Compare(Pathfinder.PathNode x_, Pathfinder.PathNode y_)
      {
        return x_.TotalCost.CompareTo(y_.TotalCost);
      }
    }
  }
}
