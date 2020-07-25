// Decompiled with JetBrains decompiler
// Type: pwnagebot.LotroInterface.LotroEntityManager
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using pwnagebot.GameInterface;
using pwnagebot.GameInterface.Frameworks;
using pwnagebot.GameInterface.Frameworks.Logging;
using pwnagebot.GameInterface.Radar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace pwnagebot.LotroInterface
{
  public class LotroEntityManager : EntityManager
  {
    private int a;
    private int b;
    private int c;
    private int d;
    private int e;
    private int f;

    public LotroEntityManager(MemoryReader reader_, int entityManagerPtr_, int meEntityPtr_, int vitalsThisPtr_, int groupPtr_, int groupLeaderPtr_, int groupLeaderMarker_, bool injected_)
      : base(reader_, injected_)
    {
      this.a = entityManagerPtr_;
      this.e = meEntityPtr_;
      this.f = vitalsThisPtr_;
      this.b = groupPtr_;
      this.c = groupLeaderPtr_;
      this.d = groupLeaderMarker_;
    }

    public override RadarBase ConvertEntityToRadar(Entity entity_)
    {
      LotroEntity lotroEntity = entity_ as LotroEntity;
      if (lotroEntity == null)
        throw new ArgumentException("Unknown entity type", "entity_");
      RadarBase radarBase = (RadarBase) null;
      switch (lotroEntity.Type)
      {
        case LotroEntity.LotroEntityType.PLAYER:
          radarBase = (RadarBase) new RadarPlayer(entity_, this.m_me);
          break;
        case LotroEntity.LotroEntityType.ENTITY:
          radarBase = (RadarBase) new RadarNpc(entity_, this.m_me);
          break;
        case LotroEntity.LotroEntityType.NODE:
          radarBase = (RadarBase) new RadarNode(entity_, this.m_me);
          break;
        case LotroEntity.LotroEntityType.WORLDOBJ:
          radarBase = (RadarBase) new RadarNode(entity_, this.m_me);
          break;
      }
      return radarBase;
    }

    public void ReloadFishing()
    {
    }

    public void ReloadFellowship()
    {
      FileLog instance = Singleton<FileLog>.Instance;
      int num1 = this.m_reader.ReadDWORD(this.b);
      if (this.m_reader.ReadDWORD(num1 + 240) <= 0)
        return;
      int address1 = this.m_reader.ReadDWORD(this.m_reader.ReadDWORD(this.m_reader.ReadDWORD(this.c) + 120) + 24);
      if (this.m_reader.ReadDWORD(address1) == 6)
      {
        int num2 = this.m_reader.ReadDWORD(address1 + 8);
        int num3 = this.m_reader.ReadDWORD(address1 + 16);
        for (int index = 0; index < num3; ++index)
        {
          int address2 = this.m_reader.ReadDWORD(num2 + index * 8);
          if (this.m_reader.ReadDWORD(address2) == this.d)
          {
            long num4 = (long) this.m_reader.ReadLong(address2 + 96);
            break;
          }
        }
      }
      Hashtable hashtable = new pwnagebot.LotroInterface.a(this.m_reader, num1 + 228, 2).b();
      ReadOnlyCollection<Entity> entities = this.Entities;
      this.m_groupList = new List<Entity>(hashtable.Count);
      LotroMe me = (LotroMe) this.Me;
      foreach (DictionaryEntry dictionaryEntry in hashtable)
      {
        LotroFellowshipEntity fellowshipEntity = new LotroFellowshipEntity(this.m_reader, this.m_reader.ReadDWORD((int) dictionaryEntry.Value + 16));
        fellowshipEntity.Update();
        if ((long) fellowshipEntity.Guid != (long) me.Guid)
        {
          foreach (Entity entity in entities)
          {
            LotroEntity lotroEntity = entity as LotroEntity;
            if (lotroEntity != null && (long) lotroEntity.Guid == (long) fellowshipEntity.Guid)
            {
              fellowshipEntity.EntityRef = lotroEntity;
              break;
            }
          }
          this.m_groupList.Add((Entity) fellowshipEntity);
        }
      }
    }

    public override void RemoveAttacker(Entity entity_)
    {
      lock (this.m_attackerLock)
      {
        if (!this.m_attackersList.Contains(entity_))
          return;
        this.m_attackersList.Remove(entity_);
      }
    }

    public void AddAttacker(Entity entity_)
    {
      lock (this.m_attackerLock)
      {
        if (this.m_attackersList.Contains(entity_))
          return;
        EntityManager.s_log.Info("A '" + entity_.ToString() + "' has aggro'd us.");
        this.m_attackersList.Add(entity_);
      }
    }

    private void b()
    {
      lock (this.m_attackerLock)
      {
        if (this.m_attackersList.Count == 0)
          return;
        LotroMe local_0 = this.m_me as LotroMe;
        if (local_0 == null)
          return;
        if (!local_0.CombatActive)
        {
          this.m_attackersList.Clear();
        }
        else
        {
          for (int local_1 = this.m_attackersList.Count - 1; local_1 >= 0; --local_1)
          {
            LotroEntity local_2 = this.m_attackersList[local_1] as LotroEntity;
            if (local_2 != null && (!local_2.CombatActive || local_2.IsInvalid || (local_2.Dead || (double) local_2.DistanceTo((Entity) local_0) > 60.0)))
              this.m_attackersList.RemoveAt(local_1);
          }
        }
      }
    }

    private void a()
    {
      Hashtable hashtable = new pwnagebot.LotroInterface.a(this.m_reader, this.m_reader.ReadDWORD(this.f) + 20, 2).b();
      LotroMe me = (LotroMe) this.m_me;
      foreach (DictionaryEntry dictionaryEntry in hashtable)
      {
        if ((long) (ulong) dictionaryEntry.Key == (long) me.Guid)
        {
          IDictionaryEnumerator enumerator = new pwnagebot.LotroInterface.a(this.m_reader, this.m_reader.ReadDWORD((int) dictionaryEntry.Value + 16) + 32, 1).b().GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
            {
              DictionaryEntry current = (DictionaryEntry) enumerator.Current;
              this.m_reader.ReadDWORD((int) current.Value);
              if ((int) current.Key == 1)
                me.MoralePtr = (int) current.Value;
              else if ((int) current.Key == 2)
                me.PowerPtr = (int) current.Value;
            }
            break;
          }
          finally
          {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
              disposable.Dispose();
          }
        }
      }
    }

    public void Reload()
    {
      ulong num1 = this.m_reader.ReadLong(this.e);
      ulong num2 = 0;
      if (this.m_injected)
        num2 = pwnagebot.LotroInterface.b.d();
      Hashtable hashtable = new pwnagebot.LotroInterface.a(this.m_reader, this.m_reader.ReadDWORD(this.a) + 12, 2).b();
      List<Entity> entities_ = new List<Entity>(hashtable.Count);
      foreach (DictionaryEntry dictionaryEntry in hashtable)
      {
        LotroEntity lotroEntity = new LotroEntity(this.m_reader, this.m_reader.ReadDWORD((int) dictionaryEntry.Value + 16));
        if (this.m_me == null || lotroEntity.MyId != this.m_me.MyId)
          entities_.Add((Entity) lotroEntity);
      }
      Dictionary<int, Entity> entites = this.ParseEntites(entities_);
      Dictionary<int, Entity>.ValueCollection values = entites.Values;
      if (this.m_me != null)
      {
        this.m_me.Update();
        this.b();
        int num3 = 0;
        int num4 = 999;
        int num5 = 0;
        foreach (LotroEntity lotroEntity in values)
        {
          if (lotroEntity.Durability != -1 && lotroEntity.ValidName && lotroEntity.Type == LotroEntity.LotroEntityType.NODE)
          {
            num3 += lotroEntity.Durability;
            if (lotroEntity.Durability < num4)
              num4 = lotroEntity.Durability;
            ++num5;
          }
        }
        int num6;
        if (num5 == 0)
        {
          num6 = 999;
          num4 = 999;
        }
        else
          num6 = num3 / num5;
        LotroMe me = (LotroMe) this.m_me;
        me.AvgDurability = num6;
        me.MinDurability = num4;
      }
      else
      {
        foreach (LotroEntity lotroEntity in values)
        {
          if ((long) lotroEntity.Guid == (long) num1)
          {
            entites.Remove(lotroEntity.MyId);
            this.m_me = (Entity) new LotroMe(this.m_reader, lotroEntity.MyId);
            this.m_me.Update();
            this.a();
            break;
          }
        }
      }
      Entity entity = (Entity) null;
      if ((long) num2 != 0L)
      {
        foreach (LotroEntity lotroEntity in values)
        {
          if ((long) lotroEntity.Guid == (long) num2)
          {
            entity = (Entity) lotroEntity;
            break;
          }
        }
      }
      this.m_target = entity;
      this.m_entityDict = entites;
    }
  }
}
