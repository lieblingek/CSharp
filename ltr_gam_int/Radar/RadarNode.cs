// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Radar.RadarNode
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.Drawing;

namespace pwnagebot.GameInterface.Radar
{
  public class RadarNode : RadarBase
  {
    public RadarNode(Entity entity_, Entity me_)
      : base(entity_, me_)
    {
      this.m_type = RadarBase.ObjectType.Unknown;
    }

    public override void Draw(Graphics g_)
    {
      Brush brush = this.m_owner.GetBrush(this.m_type);
      g_.FillEllipse(brush, this.m_origin.X, this.m_origin.Y, 9, 9);
      g_.DrawString(this.m_entity.FullName, this.m_owner.Font, brush, (float) (this.m_origin.X + 4), (float) this.m_origin.Y);
    }

    public override void Refresh()
    {
      this.m_origin = this.GetPointCartesian((int) Math.Round((double) this.m_entity.X - (double) this.m_me.X), (int) Math.Round((double) this.m_me.Y - (double) this.m_entity.Y));
    }
  }
}
