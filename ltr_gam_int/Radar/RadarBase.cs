// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Radar.RadarBase
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System.Drawing;

namespace pwnagebot.GameInterface.Radar
{
  public abstract class RadarBase
  {
    protected Entity m_entity;
    protected Entity m_me;
    protected Point m_origin;
    protected RadarControl m_owner;
    protected RadarBase.ObjectType m_type;

    public RadarControl Owner
    {
      get
      {
        return this.m_owner;
      }
      set
      {
        this.m_owner = value;
      }
    }

    public RadarBase.ObjectType Type
    {
      get
      {
        return this.m_type;
      }
      set
      {
        this.m_type = value;
      }
    }

    public int MyId
    {
      get
      {
        return this.m_entity.MyId;
      }
    }

    public RadarBase(Entity entity_, Entity me_)
    {
      this.m_entity = entity_;
      this.m_me = me_;
    }

    public abstract void Refresh();

    public abstract void Draw(Graphics g_);

    public virtual void OnDispose()
    {
    }

    public Point GetPointCartesian(int X, int Y)
    {
      return RadarBase.GetPointCartesian(X, Y, this.m_owner);
    }

    public static Point GetPointCartesian(int X, int Y, RadarControl radarControl_)
    {
      if ((double) radarControl_.Zoom == 0.0)
        return new Point();
      int num1 = radarControl_.ClientSize.Width / 2;
      int num2 = radarControl_.ClientSize.Height / 2;
      int num3 = (int) ((double) X * (100.0 / (double) radarControl_.Zoom) * (double) radarControl_.RadarScale);
      int num4 = (int) ((double) Y * (100.0 / (double) radarControl_.Zoom) * (double) radarControl_.RadarScale);
      return !radarControl_.InvertYAxis ? new Point(num1 + num3, num2 - num4) : new Point(num1 + num3, num2 + num4);
    }

    public enum ObjectType
    {
      Npc,
      Player,
      Unknown,
    }
  }
}
