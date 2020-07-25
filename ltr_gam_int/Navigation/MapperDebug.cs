// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Navigation.MapperDebug
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace pwnagebot.GameInterface.Navigation
{
  public class MapperDebug : UserControl
  {
    private IContainer components;
    private Timer m_timer;
    private Timer timer1;
    private Interface m_interfaceRef;
    private Nav m_nav;
    private ReadOnlyCollection<Region> m_regions;
    private Region m_lastRegion;

    public MapperDebug(Nav nav_, Interface interfaceRef_)
    {
      this.InitializeComponent();
      this.BackColor = Color.Black;
      this.m_nav = nav_;
      this.m_interfaceRef = interfaceRef_;
      this.m_timer.Start();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.m_timer = new Timer(this.components);
      this.timer1 = new Timer(this.components);
      this.SuspendLayout();
      this.m_timer.Tick += new EventHandler(this.m_timer_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.DoubleBuffered = true;
      this.Name = "MapperDebug";
      this.Paint += new PaintEventHandler(this.MapperDebug_Paint);
      this.ResumeLayout(false);
    }

    private void AddConnectedRegions(Region start_, List<Region> regions_, ref int count_)
    {
      if (start_ == null || count_ > 5000)
        return;
      foreach (Connection connection in start_.Connections)
      {
        if (connection.To != start_)
        {
          count_ = count_ + 1;
          if (!regions_.Contains(connection.To))
          {
            regions_.Add(connection.To);
            this.AddConnectedRegions(connection.To, regions_, ref count_);
          }
        }
      }
    }

    private ReadOnlyCollection<Region> GetRegions()
    {
      List<Region> regions_ = new List<Region>();
      Region currentRegion = this.m_interfaceRef.NavMapper.CurrentRegion;
      int count_ = 0;
      this.AddConnectedRegions(currentRegion, regions_, ref count_);
      return new ReadOnlyCollection<Region>((IList<Region>) regions_);
    }

    private void MapperDebug_Paint(object sender, PaintEventArgs e)
    {
      Entity me = this.m_interfaceRef.EntityManager.Me;
      Graphics graphics = e.Graphics;
      if (this.m_lastRegion != this.m_interfaceRef.NavMapper.CurrentRegion || this.m_regions == null)
        this.m_regions = this.m_nav.GetAllRegionsByDistance(me.X, me.Y, 0.0f);
      this.m_lastRegion = this.m_interfaceRef.NavMapper.CurrentRegion;
      int count = this.m_regions.Count;
      for (int index = 0; index < count; ++index)
      {
        if ((double) this.m_regions[index].Distance(me.X, me.Y, 0.0f) <= 400.0)
          this.DrawRegion(graphics, this.m_regions[index], me);
      }
      if (this.m_nav.CurrentPath != null)
      {
        foreach (Connection connection in this.m_nav.CurrentPath)
        {
          float x1 = connection.From.CenterPoint().X;
          float y1 = connection.From.CenterPoint().Y;
          System.Drawing.Point pointCartesian1 = this.GetPointCartesian((int) Math.Round((double) x1 - (double) me.X), (int) Math.Round((double) me.Y - (double) y1));
          float x2 = connection.To.CenterPoint().X;
          float y2 = connection.To.CenterPoint().Y;
          System.Drawing.Point pointCartesian2 = this.GetPointCartesian((int) Math.Round((double) x2 - (double) me.X), (int) Math.Round((double) me.Y - (double) y2));
          graphics.DrawLine(new Pen(Color.Green, 1f), pointCartesian1, pointCartesian2);
        }
      }
      if (this.m_nav.GrindSpots != null)
      {
        foreach (Region grindSpot in this.m_nav.GrindSpots)
        {
          float x = grindSpot.CenterPoint().X;
          float y = grindSpot.CenterPoint().Y;
          System.Drawing.Point pointCartesian = this.GetPointCartesian((int) Math.Round((double) x - (double) me.X), (int) Math.Round((double) me.Y - (double) y));
          graphics.FillEllipse((Brush) new SolidBrush(Color.RoyalBlue), pointCartesian.X, pointCartesian.Y, 8, 8);
        }
      }
      Region currentGrindSpot = this.m_nav.CurrentGrindSpot;
      if (currentGrindSpot != null)
      {
        float x = currentGrindSpot.CenterPoint().X;
        float y = currentGrindSpot.CenterPoint().Y;
        System.Drawing.Point pointCartesian = this.GetPointCartesian((int) Math.Round((double) x - (double) me.X), (int) Math.Round((double) me.Y - (double) y));
        graphics.FillEllipse((Brush) new SolidBrush(Color.Violet), pointCartesian.X, pointCartesian.Y, 8, 8);
      }
      System.Drawing.Point pointCartesian3 = this.GetPointCartesian(0, 0);
      graphics.FillEllipse((Brush) new SolidBrush(Color.White), pointCartesian3.X - 4, pointCartesian3.Y - 4, 8, 8);
    }

    private void DrawRegion(Graphics g_, Region region_, Entity me_)
    {
      float x1 = region_.CenterPoint().X;
      float y1 = region_.CenterPoint().Y;
      System.Drawing.Point pointCartesian1 = this.GetPointCartesian((int) Math.Round((double) x1 - (double) me_.X), (int) Math.Round((double) me_.Y - (double) y1));
      g_.FillEllipse((Brush) new SolidBrush(Color.Yellow), pointCartesian1.X, pointCartesian1.Y, 3, 3);
      foreach (Connection connection in region_.Connections)
      {
        float x2 = connection.To.CenterPoint().X;
        float y2 = connection.To.CenterPoint().Y;
        System.Drawing.Point pointCartesian2 = this.GetPointCartesian((int) Math.Round((double) x2 - (double) me_.X), (int) Math.Round((double) me_.Y - (double) y2));
        g_.DrawLine(new Pen(Color.Red, 1f), pointCartesian1, pointCartesian2);
      }
    }

    public System.Drawing.Point GetPointCartesian(int X, int Y)
    {
      int num1 = this.ClientSize.Width / 2;
      int num2 = this.ClientSize.Height / 2;
      float num3 = 0.5f;
      int num4 = (int) ((double) X / (double) num3);
      int num5 = (int) ((double) Y / (double) num3);
      return new System.Drawing.Point(num1 + num4, num2 + num5);
    }

    private void m_timer_Tick(object sender, EventArgs e)
    {
      this.Invalidate();
    }
  }
}
