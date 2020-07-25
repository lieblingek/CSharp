// Decompiled with JetBrains decompiler
// Type: pwnagebot.LotroInterface.LotroFishing
// Assembly: LotroInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F17673E-01E0-44D0-A3C2-FEC018E33C2F
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\LotroInterface.dll

using pwnagebot.GameInterface;
using pwnagebot.GameInterface.Frameworks;
using pwnagebot.GameInterface.Frameworks.Logging;
using System;
using System.Threading;

namespace pwnagebot.LotroInterface
{
  public class LotroFishing : Fishing
  {
    private static FileLog c = Singleton<FileLog>.Instance;
    private const int a = 56;
    private const int b = 1879109150;
    private MemoryReader d;
    private pwnagebot.LotroInterface.LotroInterface e;
    private LotroInteract f;
    private int g;
    private int h;
    private int i;

    public bool Initialized
    {
      get
      {
        return this.i != -1;
      }
    }

    public LotroFishing(pwnagebot.LotroInterface.LotroInterface interface_, MemoryReader reader_, int fishingPtr_)
    {
      this.e = interface_;
      this.f = (LotroInteract) interface_.Interact;
      this.d = reader_;
      this.g = fishingPtr_;
      this.i = -1;
    }

    public bool Initialize(ulong guid_)
    {
      int num1 = this.d.ReadDWORD(this.g + 4);
      int num2 = this.d.ReadDWORD(this.d.ReadDWORD(this.g + 8));
      this.i = -1;
      this.h = -1;
      for (int index = 0; index < num1; ++index)
      {
        if ((long) this.d.ReadLong(num2 + index * 216 + 24) == (long) guid_)
        {
          this.i = this.d.ReadDWORD(num2 + index * 216 + 44);
          int num3 = this.d.ReadDWORD(this.i + 56);
          if (num3 == 3)
          {
            this.h = this.e.GetSlotBySkillId(1879109150);
            break;
          }
          LotroFishing.c.Debug("Found unknown fishing status of: " + (object) num3);
          this.i = -1;
        }
      }
      return this.i != -1;
    }

    private int a()
    {
      if (this.i == -1)
        throw new InvalidOperationException("Can not get fishing status before being initialized succesfully.");
      return this.d.ReadDWORD(this.i + 56);
    }

    public override bool Cast()
    {
      if (this.h == -1)
        return false;
      this.f.PressHotkey(this.h);
      Thread.Sleep(6000);
      return true;
    }

    public override bool Reel()
    {
      if (this.h == -1)
        return false;
      this.f.PressHotkey(this.h);
      Thread.Sleep(6000);
      return true;
    }

    public override bool HasBite()
    {
      return this.a() == 5;
    }

    public override bool IsFishing()
    {
      return this.a() != 3;
    }

    private enum a
    {
      a = 3,
      b = 4,
      c = 5,
      d = 6,
      e = 7,
    }
  }
}
