﻿// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Fishing
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

namespace pwnagebot.GameInterface
{
  public abstract class Fishing
  {
    public abstract bool Cast();

    public abstract bool Reel();

    public abstract bool HasBite();

    public abstract bool IsFishing();
  }
}
