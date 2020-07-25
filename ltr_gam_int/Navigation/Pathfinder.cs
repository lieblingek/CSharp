// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Navigation.Pathfinder
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System.Collections.ObjectModel;

namespace pwnagebot.GameInterface.Navigation
{
  public abstract class Pathfinder
  {
    public abstract ReadOnlyCollection<Connection> FindPath(Region src_, Region dest_);

    protected class PathNode
    {
      public Region RegionRef;
      public Pathfinder.PathNode Parent;
      public float ConnectCost;
      public float EstimatedCost;
      public float TotalCost;
      public Connection CurrentLink;
    }
  }
}
