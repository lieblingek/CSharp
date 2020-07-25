// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Radar.RadarPlayer
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace pwnagebot.GameInterface.Radar
{
  public class RadarPlayer : RadarBase
  {
    private Point[] a;

    public RadarPlayer(Entity entity_, Entity me_)
      : base(entity_, me_)
    {
      this.m_type = RadarBase.ObjectType.Player;
    }

    public override void Draw(Graphics g_)
    {
      Brush brush = this.m_owner.GetBrush(this.m_type);
      g_.FillPolygon(brush, this.a);
      g_.DrawString(this.m_entity.FullName, this.m_owner.Font, brush, (float) (this.m_origin.X + 4), (float) this.m_origin.Y);
    }

    public override void Refresh()
    {
      this.m_origin = this.GetPointCartesian((int) Math.Round((double) this.m_entity.X - (double) this.m_me.X), (int) Math.Round((double) this.m_me.Y - (double) this.m_entity.Y));
      this.a = new Point[4];
      int num = 12;
      this.a[0].X = this.m_origin.X - num / 2;
      this.a[0].Y = this.m_origin.Y + num / 2;
      this.a[1].X = this.m_origin.X;
      this.a[1].Y = this.m_origin.Y + num / 3;
      this.a[2].X = this.m_origin.X + num / 2;
      this.a[2].Y = this.m_origin.Y + num / 2;
      this.a[3].X = this.m_origin.X;
      this.a[3].Y = this.m_origin.Y - num / 2;
      Matrix matrix = new Matrix();
      PointF point = new PointF((float) this.m_origin.X, (float) this.m_origin.Y);
      matrix.RotateAt(this.m_entity.Heading, point);
      matrix.TransformPoints(this.a);
    }
  }
}
