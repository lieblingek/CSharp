// Decompiled with JetBrains decompiler
// Type: pwnagebot.LotroInterface.LotroInteract
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using pwnagebot.GameInterface;
using pwnagebot.GameInterface.Frameworks;

namespace pwnagebot.LotroInterface
{
  public class LotroInteract : IInteractable
  {
    private pwnagebot.LotroInterface.LotroInterface a;

    public LotroInteract(pwnagebot.LotroInterface.LotroInterface interface_)
    {
      this.a = interface_;
    }

    public void SetClientId(uint processId_)
    {
      b.SetStateIndex(processId_);
    }

    public bool SelectTarget(Entity entity_)
    {
      LotroEntity lotroEntity = entity_ as LotroEntity;
      if (lotroEntity == null)
        return false;
      b.c(lotroEntity.Guid);
      return true;
    }

    public bool EnterCombatMode()
    {
      LotroEntity target = this.a.EntityManager.Target as LotroEntity;
      if (target == null)
        return false;
      b.b(target.Guid);
      return true;
    }

    public void PressHotkey(int index_)
    {
      b.a(index_);
    }

    public bool Loot(Entity entity_)
    {
      b.b(((LotroEntity) entity_).Guid);
      return true;
    }
  }
}
