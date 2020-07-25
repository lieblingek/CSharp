// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Navigation.Mapper
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using pwnagebot.GameInterface.Frameworks;
using pwnagebot.GameInterface.Frameworks.Logging;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;

namespace pwnagebot.GameInterface.Navigation
{
  [Serializable]
  public class Mapper : ISerializable
  {
    private static FileLog s_log = Singleton<FileLog>.Instance;
    private Nav m_nav;
    private Interface m_interface;
    private double m_defaultSize;
    private int m_count;
    private Region m_prevRegion;
    private Region m_lastBestContainer;

    public double DefaultSize
    {
      get
      {
        return this.m_defaultSize;
      }
      set
      {
        this.m_defaultSize = value;
      }
    }

    public Interface Interface
    {
      set
      {
        this.m_interface = value;
      }
    }

    public Nav NavRef
    {
      get
      {
        return this.m_nav;
      }
    }

    public Region CurrentRegion
    {
      get
      {
        Entity me = this.m_interface.EntityManager.Me;
        if (me == null)
          return (Region) null;
        if (this.m_lastBestContainer == null)
          this.m_lastBestContainer = (Region) this.m_nav.Tree;
        Region container_ = this.m_lastBestContainer.BestContainer(me.X, me.Y, me.Z);
        if (this.IsMapped(container_))
          return container_;
        return (Region) null;
      }
    }

    public Mapper(Interface interface_)
    {
      this.m_nav = new Nav();
      this.m_interface = interface_;
      this.m_lastBestContainer = (Region) this.m_nav.Tree;
      this.m_defaultSize = 200.0;
    }

    protected Mapper(SerializationInfo info_, StreamingContext context_)
    {
      if (info_ == null)
        throw new ArgumentNullException("info_");
      this.m_count = info_.GetInt32("count");
      this.m_defaultSize = (double) info_.GetInt32("defaultsize");
      this.m_nav = (Nav) info_.GetValue("nav", typeof (Nav));
      this.m_lastBestContainer = (Region) this.m_nav.Tree;
    }

    public void ShowDebug()
    {
      Form form = new Form();
      MapperDebug mapperDebug = new MapperDebug(this.m_nav, this.m_interface);
      mapperDebug.Dock = DockStyle.Fill;
      form.Controls.Add((Control) mapperDebug);
      form.Show();
    }

    public void StartMapping()
    {
      Mapper.s_log.Info("Starting mapping...");
      this.m_interface.OnUpdate += new EventHandler(this.Mapper_OnUpdate);
    }

    public void StopMapping()
    {
      Mapper.s_log.Info("Stopping mapping...");
      this.m_interface.OnUpdate -= new EventHandler(this.Mapper_OnUpdate);
      this.m_prevRegion = (Region) null;
    }

    public Region FindRegion(float x_, float y_, float z_)
    {
      if (this.m_lastBestContainer == null)
        this.m_lastBestContainer = (Region) this.m_nav.Tree;
      Region container_ = this.m_lastBestContainer.BestContainer(x_, y_, z_);
      if (this.IsMapped(container_))
        return container_;
      return (Region) null;
    }

    private bool IsMapped(Region container_)
    {
      return container_ != this.m_nav.Tree;
    }

    private bool ShouldConnect(Region a_, Region b_)
    {
      return a_ != null && b_ != null && (a_ != b_ && (double) a_.Distance(b_) <= this.m_defaultSize * 3.0);
    }

    private Region MapLocation(float x_, float y_, float z_)
    {
      float num = (float) this.m_defaultSize / 2f;
      Region region1 = (Region) new Rect(this.m_lastBestContainer, x_ - num, x_ + num, y_ - num, y_ + num);
      region1.Name = this.m_count.ToString();
      if (this.ShouldConnect(this.m_prevRegion, region1))
      {
        region1.Connect(this.m_prevRegion);
        this.m_prevRegion.Connect(region1);
      }
      foreach (Region region2 in region1.Parent.ChildrenWithin(region1.CenterPoint(), (float) this.m_defaultSize))
      {
        if (this.ShouldConnect(region2, region1))
        {
          region1.Connect(region2);
          region2.Connect(region1);
        }
      }
      this.m_lastBestContainer.AddChild(region1);
      ++this.m_count;
      return region1;
    }

    private void Mapper_OnUpdate(object sender_, EventArgs args_)
    {
      Entity me = this.m_interface.EntityManager.Me;
      if (me == null)
      {
        Mapper.s_log.Debug("Can not map because player location not found.");
      }
      else
      {
        if (this.m_lastBestContainer == null)
          this.m_lastBestContainer = (Region) this.m_nav.Tree;
        Region region1 = this.m_lastBestContainer.BestContainer(me.X, me.Y, me.Z);
        if (this.IsMapped(region1))
        {
          if (this.m_prevRegion == region1)
            return;
          if (this.ShouldConnect(this.m_prevRegion, region1))
          {
            this.m_prevRegion.Connect(region1);
            float num = (me.Z - this.m_prevRegion.CenterPoint().Z) / this.m_prevRegion.Distance(me.X, me.Y, me.Z);
            if ((double) Math.Abs(num) < 0.7)
              region1.Connect(this.m_prevRegion);
            else
              Mapper.s_log.Debug("Not connecting to previous region due to high slope: " + (object) num);
          }
          this.m_prevRegion = region1;
        }
        else
        {
          this.m_lastBestContainer = region1;
          Region region2 = this.MapLocation(me.X, me.Y, me.Z);
          this.m_prevRegion = region2;
          Mapper.s_log.Info(string.Format("New region at ({0:0.0f}, {1:0.0f}, {2:0.0f}) [Connections {3}]", (object) region2.CenterPoint().X, (object) region2.CenterPoint().Y, (object) region2.CenterPoint().Z, (object) region2.Connections.Count));
        }
      }
    }

    protected virtual void GetObjectData(SerializationInfo info_, StreamingContext context_)
    {
      info_.AddValue("count", this.m_count);
      info_.AddValue("defaultsize", this.m_defaultSize);
      info_.AddValue("nav", (object) this.m_nav);
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info_, StreamingContext context_)
    {
      if (info_ == null)
        throw new ArgumentNullException("info");
      this.GetObjectData(info_, context_);
    }
  }
}
