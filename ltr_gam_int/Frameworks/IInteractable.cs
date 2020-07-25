// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Frameworks.IInteractable
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

namespace pwnagebot.GameInterface.Frameworks
{
  public interface IInteractable
  {
    void SetClientId(uint processId_);

    bool SelectTarget(Entity entity_);

    bool EnterCombatMode();

    bool Loot(Entity entity_);
  }
}
