// Decompiled with JetBrains decompiler
// Type: pwnagebot.LotroInterface.LotroInterface
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using pwnagebot.GameInterface;
using pwnagebot.GameInterface.Frameworks;
using pwnagebot.GameInterface.Frameworks.Logging;
using pwnagebot.GameInterface.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace pwnagebot.LotroInterface
{
  public class LotroInterface : Interface
  {
    private static FileLog a = Singleton<FileLog>.Instance;
    private bool b;

    public LotroEntityManager EntityManager
    {
      get
      {
        return (LotroEntityManager) this.m_entityManager;
      }
    }

    public LotroInterface()
    {
      pwnagebot.LotroInterface.LotroInterface.a.FileName = "lotro_log.txt";
      this.MapperDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "pwnagebot" + (object) Path.DirectorySeparatorChar + "Lotro" + (object) Path.DirectorySeparatorChar + "mapdata" + (object) Path.DirectorySeparatorChar);
      this.ProcessTitle = "lotroclient";
      this.DllToInject = "LOTROInjected.DLL";
      this.PipeServer = true;
      this.m_navMapper.DefaultSize = 20.0;
      this.Interact = (IInteractable) new LotroInteract(this);
      this.b = true;
    }

    protected override void RegisterEvents()
    {
      this.OnUpdate += new EventHandler(this.a);
      this.OnMessage += new Interface.MessageDelegate(this.a);
    }

    private void a(string A_0)
    {
      if (Regex.Match(A_0, "Success!").Success)
        this.CastCompleted((object) null, new EventArgs<bool>(true));
      if (Regex.Match(A_0, "(Target not in line of sight.)|(The skill is being barred from use.)|(Your action was interrupted.)|(Interrupted!)|(You must face target.)|(You cannot do that while moving.)|(You cannot do that right now.)|(You need a valid target.)|(The target is dead.)").Success)
        this.CastCompleted((object) null, new EventArgs<bool>(false));
      Match match1 = Regex.Match(A_0, "Your level has changed to (\\d+).");
      if (match1.Success)
        this.LevelChanged((object) null, new EventArgs<int>(int.Parse(match1.Groups[1].Value)));
      if (Regex.Match(A_0, "Target not in line of sight.").Success)
        this.LineOfSight((object) null, EventArgs.Empty);
      if (Regex.Match(A_0, "You must face target.").Success)
        this.MustFaceTarget((object) null, EventArgs.Empty);
      if (Regex.Match(A_0, "(Cannot harm target.)|(Invalid target.)").Success)
        this.InvalidTarget((object) null, EventArgs.Empty);
      Match match2 = Regex.Match(A_0, "ATTACKERS 0x([A-Fa-f0-9]+) 0x([A-Fa-f0-9]+)");
      if (!match2.Success)
        return;
      this.a(int.Parse(match2.Groups[1].Value, NumberStyles.HexNumber), int.Parse(match2.Groups[2].Value, NumberStyles.HexNumber));
    }

    private void a(int A_0, int A_1)
    {
      if (A_0 == 0 || A_1 == 0)
        return;
      LotroMe me = this.EntityManager.Me as LotroMe;
      if (me == null || me.BasePtr != A_1)
        return;
      LotroEntity lotroEntity1 = (LotroEntity) null;
      ReadOnlyCollection<Entity> entities = this.EntityManager.Entities;
      int count = entities.Count;
      for (int index = 0; index < count; ++index)
      {
        LotroEntity lotroEntity2 = entities[index] as LotroEntity;
        if (lotroEntity2 != null && lotroEntity2.BasePtr == A_0)
        {
          lotroEntity1 = lotroEntity2;
          break;
        }
      }
      if (lotroEntity1 == null || lotroEntity1.Type == LotroEntity.LotroEntityType.PLAYER || (double) lotroEntity1.DistanceTo((Entity) me) > 40.0)
        return;
      Console.WriteLine(lotroEntity1.FullName + " is attacking us (" + (object) me.MyId + ")");
      this.EntityManager.AddAttacker((Entity) lotroEntity1);
    }

    protected override void LoadPointers()
    {
      this.m_entityManager = (pwnagebot.GameInterface.EntityManager) new LotroEntityManager(this.m_reader, this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pBaseMobManager.ToString()), this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pMeGUID.ToString()), this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pVitalsThis.ToString()), this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pGroup.ToString()), this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pGroupLeader.ToString()), this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_cGroupLeaderMarker.ToString()), this.Injected);
      this.m_movement = (Movement) new f((Interface) this, this.m_reader, this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pSetMovement.ToString()));
      this.m_fishing = (Fishing) new LotroFishing(this, this.m_reader, this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pFishingBase.ToString()));
      e.a(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pStringBase.ToString()));
      pwnagebot.LotroInterface.b.SetGetMePtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pGetMe.ToString()));
      pwnagebot.LotroInterface.b.SetTargetPtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pDoTarget.ToString()));
      pwnagebot.LotroInterface.b.SetVitalsMangerPtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pVitalsThis.ToString()));
      pwnagebot.LotroInterface.b.SetGetVitalsPtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pGetVitalProperty.ToString()));
      pwnagebot.LotroInterface.b.SetHotkeyPtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pDoHotkey.ToString()));
      pwnagebot.LotroInterface.b.SetLootPtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pLoot.ToString()));
      pwnagebot.LotroInterface.b.SetUsePtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pUse.ToString()));
      pwnagebot.LotroInterface.b.SetTimerPtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pTimer.ToString()));
      pwnagebot.LotroInterface.b.SetThreadHookPtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pThreadHook.ToString()));
      pwnagebot.LotroInterface.b.SetTargetHookPtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pTargetHook.ToString()));
      pwnagebot.LotroInterface.b.SetStringWatcherHookPtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pStringWatcherHook.ToString()));
      pwnagebot.LotroInterface.b.SetSkillbarIndexHookPtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pSkillbarIndexHook.ToString()));
      pwnagebot.LotroInterface.b.SetAttackersHookPtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pAttackersHook.ToString()));
      pwnagebot.LotroInterface.b.SetClickUpPtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pClickUp.ToString()));
      d.a(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pGuiBase.ToString()));
      if (this.m_pointers.Exists(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pLastInput.ToString()))
        pwnagebot.LotroInterface.b.SetLastInputPtr(this.m_pointers.Get(pwnagebot.LotroInterface.LotroInterface.LotroPointers.g_pLastInput.ToString()));
      else
        pwnagebot.LotroInterface.b.SetLastInputPtr(0);
      bool flag = false;
      try
      {
        flag = this.LoadMapData("lotro_map.dat");
      }
      finally
      {
        if (!flag)
        {
          pwnagebot.LotroInterface.LotroInterface.a.Info("Failed to load map data, using empty map.");
          this.NavMapper = new Mapper((Interface) this);
          this.MapperDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "pwnagebot" + (object) Path.DirectorySeparatorChar + "Lotro" + (object) Path.DirectorySeparatorChar + "mapdata" + (object) Path.DirectorySeparatorChar);
          this.m_navMapper.DefaultSize = 20.0;
        }
      }
      try
      {
        e.a();
      }
      catch
      {
      }
    }

    public override void Shutdown()
    {
      base.Shutdown();
      this.SaveMapData();
      try
      {
        e.b();
      }
      catch
      {
        pwnagebot.LotroInterface.LotroInterface.a.Log(Logger.MessageType.Failure, "Unknown error saving string cache data.");
      }
    }

    public override bool SaveMapData()
    {
      return this.SaveMapData("lotro_map.dat");
    }

    public int GetSlotBySkillId(int skillId_)
    {
      int num = this.m_reader.ReadDWORD(pwnagebot.LotroInterface.b.c() + 24);
      for (int index = 0; index < 72; ++index)
      {
        if (this.m_reader.ReadDWORD(num + index * 32 + 24) == skillId_)
          return index;
      }
      return -1;
    }

    public bool PressReleaseCorpseButton()
    {
      return d.h(this.m_reader);
    }

    private void a(object A_0, EventArgs A_1)
    {
      LotroEntityManager entityManager = this.EntityManager;
      if (entityManager == null)
        return;
      entityManager.Reload();
      entityManager.ReloadFellowship();
      if (entityManager.Me != null && (double) entityManager.Me.Health <= 0.0 && this.b)
      {
        this.PlayerDied((object) null, EventArgs.Empty);
        this.b = false;
      }
      if (entityManager.Me != null && (double) entityManager.Me.Health > 0.0 && !this.b)
        this.b = true;
      LotroFishing fishing = (LotroFishing) this.Fishing;
      if (fishing.Initialized)
        return;
      foreach (LotroEntity entity in entityManager.Entities)
      {
        if (entity.Name.Contains("Fishing Rod") && entity.Type != LotroEntity.LotroEntityType.UNKNOWN)
        {
          fishing.Initialize(entity.Guid);
          break;
        }
      }
    }

    internal enum LotroPointers
    {
      g_pBaseMobManager,
      g_pMeGUID,
      g_pStringBase,
      g_pGuiBase,
      g_pSetMovement,
      g_pFishingBase,
      g_pGroup,
      g_cGroupLeaderMarker,
      g_pGroupLeader,
      g_pGetMe,
      g_pDoTarget,
      g_pVitalsThis,
      g_pGetVitalProperty,
      g_pDoHotkey,
      g_pLoot,
      g_pUse,
      g_pTimer,
      g_pThreadHook,
      g_pTargetHook,
      g_pStringWatcherHook,
      g_pSkillbarIndexHook,
      g_pAttackersHook,
      g_pClickUp,
      g_pLastInput,
    }
  }
}
