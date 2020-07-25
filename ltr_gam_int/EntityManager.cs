// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.EntityManager
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using pwnagebot.GameInterface.Frameworks;
using pwnagebot.GameInterface.Frameworks.Logging;
using pwnagebot.GameInterface.Radar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace pwnagebot.GameInterface
{
  public class EntityManager
  {
    protected static FileLog s_log = Singleton<FileLog>.Instance;
    protected Dictionary<int, Entity> m_entityDict;
    protected Entity m_me;
    protected Entity m_target;
    protected MemoryReader m_reader;
    protected bool m_injected;
    protected List<Entity> m_attackersList;
    protected List<Entity> m_groupList;
    protected object m_attackerLock;

    public ReadOnlyCollection<Entity> Attackers
    {
      get
      {
        lock (this.m_attackerLock)
          return new ReadOnlyCollection<Entity>((IList<Entity>) new List<Entity>((IEnumerable<Entity>) this.m_attackersList));
      }
    }

    public ReadOnlyCollection<Entity> Group
    {
      get
      {
        lock (this.m_attackerLock)
          return new ReadOnlyCollection<Entity>((IList<Entity>) new List<Entity>((IEnumerable<Entity>) this.m_groupList));
      }
    }

    public ReadOnlyCollection<Entity> Entities
    {
      get
      {
        return new ReadOnlyCollection<Entity>((IList<Entity>) new List<Entity>((IEnumerable<Entity>) this.m_entityDict.Values));
      }
    }

    public Entity Me
    {
      get
      {
        return this.m_me;
      }
    }

    public Entity Target
    {
      get
      {
        return this.m_target;
      }
    }

    public event EventHandler<EventArgs<Entity>> EntityAdded;

    public event EventHandler<EventArgs<Entity>> EntityRemoved;

    public EntityManager(MemoryReader reader_, bool injected_)
    {
      this.m_reader = reader_;
      this.m_injected = injected_;
      this.m_entityDict = new Dictionary<int, Entity>();
      this.m_attackersList = new List<Entity>();
      this.m_groupList = new List<Entity>();
      this.m_attackerLock = new object();
    }

    protected Dictionary<int, Entity> ParseEntites(List<Entity> entities_)
    {
      Dictionary<int, Entity> dictionary = new Dictionary<int, Entity>(entities_.Count);
      foreach (Entity entity1 in entities_)
      {
        if (entity1.MyId != 0 && !dictionary.ContainsKey(entity1.MyId))
        {
          Entity entity2 = (Entity) null;
          if (this.m_entityDict.ContainsKey(entity1.MyId))
          {
            entity2 = this.m_entityDict[entity1.MyId];
            if (!entity2.Update())
            {
              entity2 = (Entity) null;
              this.DoEntityRemoved(this.m_entityDict[entity1.MyId]);
            }
            else
              dictionary[entity2.MyId] = entity2;
          }
          if (entity2 == null)
          {
            entity1.Update();
            dictionary.Add(entity1.MyId, entity1);
            this.DoEntityAdded(entity1);
          }
        }
      }
      foreach (int key in this.m_entityDict.Keys)
      {
        if (!dictionary.ContainsKey(key))
          this.DoEntityRemoved(this.m_entityDict[key]);
      }
      return dictionary;
    }

    protected void DoEntityAdded(Entity entity_)
    {
      if (this.a == null)
        return;
      this.a((object) null, new EventArgs<Entity>(entity_));
    }

    protected void DoEntityRemoved(Entity entity_)
    {
      if (this.b == null)
        return;
      this.b((object) null, new EventArgs<Entity>(entity_));
    }

    public virtual RadarBase ConvertEntityToRadar(Entity entity_)
    {
      return (RadarBase) null;
    }

    public virtual void RemoveAttacker(Entity entity_)
    {
    }
  }
}
