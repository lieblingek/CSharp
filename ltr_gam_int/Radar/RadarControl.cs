// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Radar.RadarControl
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using pwnagebot.GameInterface.Frameworks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace pwnagebot.GameInterface.Radar
{
  public class RadarControl : UserControl
  {
    private object b = new object();
    private float d = 1f;
    private float e = 100f;
    private Interface a;
    private RadarControl.RenderMode c;
    private bool f;
    private Bitmap g;
    private List<RadarBase> h;
    private Dictionary<RadarBase.ObjectType, Brush> i;
    private bool j;
    private IContainer k;
    private Button l;
    private Label m;
    private Label n;
    private Button o;
    private Button p;
    private Button q;
    private Button r;
    private Label s;
    private TableLayoutPanel t;
    private TableLayoutPanel u;

    public bool InvertYAxis
    {
      get
      {
        return this.j;
      }
      set
      {
        this.j = value;
      }
    }

    public float Zoom
    {
      get
      {
        return this.e;
      }
      set
      {
        this.e = value;
      }
    }

    public float RadarScale
    {
      get
      {
        return this.d;
      }
      set
      {
        this.d = value;
      }
    }

    public Interface Interface
    {
      set
      {
        this.a = value;
      }
    }

    public RadarControl()
    {
      this.b();
      this.h = new List<RadarBase>();
      this.e();
      this.n.Text = ((double) this.e).ToString() + "%";
      this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
    }

    public void SetRenderMode(RadarControl.RenderMode mode_)
    {
      if (mode_ == this.c)
        return;
      if (mode_ == RadarControl.RenderMode.Window)
      {
        Form parentForm = this.ParentForm;
        parentForm.ShowInTaskbar = true;
        parentForm.TopMost = true;
        parentForm.FormBorderStyle = FormBorderStyle.Sizable;
      }
      else
      {
        Form parentForm = this.ParentForm;
        parentForm.ShowInTaskbar = false;
        parentForm.TopMost = true;
        parentForm.FormBorderStyle = FormBorderStyle.None;
      }
      this.c = mode_;
    }

    private void e()
    {
      this.i = new Dictionary<RadarBase.ObjectType, Brush>(Enum.GetValues(typeof (RadarBase.ObjectType)).Length);
      this.i[RadarBase.ObjectType.Npc] = (Brush) new SolidBrush(Color.Yellow);
      this.i[RadarBase.ObjectType.Player] = (Brush) new SolidBrush(Color.Red);
      this.i[RadarBase.ObjectType.Unknown] = (Brush) new SolidBrush(Color.DarkGray);
    }

    public Brush GetBrush(RadarBase.ObjectType type_)
    {
      return this.i[type_];
    }

    public void SetBrush(RadarBase.ObjectType type_, Brush brush_)
    {
      this.i[type_] = brush_;
    }

    private void d()
    {
      if (this.g != null)
        this.g.Dispose();
      foreach (Brush brush in this.i.Values)
        brush.Dispose();
      if (this.h == null)
        return;
      foreach (RadarBase radarBase in this.h)
        radarBase.OnDispose();
    }

    private void a(object A_0, PaintEventArgs A_1)
    {
      if (!this.c() || this.g == null)
        return;
      A_1.Graphics.DrawImageUnscaled((Image) this.g, 0, 0);
    }

    private bool c()
    {
      if (this.c == RadarControl.RenderMode.Window)
      {
        if (this.ClientSize.IsEmpty)
          return false;
      }
      else if (this.f)
        return false;
      return true;
    }

    public void RefreshObjects()
    {
      if (!this.c())
        return;
      Bitmap bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
      Graphics g_ = Graphics.FromImage((Image) bitmap);
      g_.SmoothingMode = SmoothingMode.AntiAlias;
      if (this.c == RadarControl.RenderMode.Overlay)
        g_.Clear(Color.White);
      else
        g_.Clear(Color.Black);
      lock (this.b)
      {
        foreach (RadarBase item_0 in this.h)
        {
          item_0.Refresh();
          item_0.Draw(g_);
        }
        this.BeginInvoke((Delegate) (() =>
        {
          Entity me = this.a.EntityManager.Me;
          this.m.Text = this.h.Count.ToString() + " objs";
          if (me == null)
            this.s.Text = "n/a";
          else
            this.s.Text = string.Format("({0:0.0}, {1:0.0}, {2:0.0})", (object) me.X, (object) me.Y, (object) me.Z);
        }));
      }
      Bitmap g = this.g;
      this.g = bitmap;
      if (g != null)
        g.Dispose();
      this.Invalidate();
    }

    public void Reload()
    {
      this.a.EntityManager.EntityAdded -= new EventHandler<EventArgs<Entity>>(this.b);
      this.a.EntityManager.EntityRemoved -= new EventHandler<EventArgs<Entity>>(this.a);
      lock (this.b)
      {
        this.a.EntityManager.EntityAdded += new EventHandler<EventArgs<Entity>>(this.b);
        this.a.EntityManager.EntityRemoved += new EventHandler<EventArgs<Entity>>(this.a);
        ReadOnlyCollection<Entity> local_1 = this.a.EntityManager.Entities;
        this.h.Clear();
        this.h.Capacity = local_1.Count;
        foreach (Entity item_0 in local_1)
        {
          RadarBase local_0 = this.a.EntityManager.ConvertEntityToRadar(item_0);
          if (local_0 != null)
          {
            local_0.Owner = this;
            local_0.Refresh();
            this.h.Add(local_0);
          }
        }
        RadarBase local_0_1 = this.a.EntityManager.ConvertEntityToRadar(this.a.EntityManager.Me);
        local_0_1.Owner = this;
        local_0_1.Refresh();
        this.h.Add(local_0_1);
      }
      this.RefreshObjects();
    }

    private void b(object A_0, EventArgs<Entity> A_1)
    {
      lock (this.b)
      {
        RadarBase local_0 = this.a.EntityManager.ConvertEntityToRadar(A_1.Value);
        if (local_0 == null)
          return;
        local_0.Owner = this;
        local_0.Refresh();
        this.h.Add(local_0);
      }
    }

    private void a(object A_0, EventArgs<Entity> A_1)
    {
      lock (this.b)
      {
        foreach (RadarBase item_0 in this.h)
        {
          if (item_0.MyId == A_1.Value.MyId)
          {
            this.h.Remove(item_0);
            break;
          }
        }
      }
    }

    private void e(object A_0, EventArgs A_1)
    {
      if ((double) this.e < 200.0)
        this.e += 10f;
      this.n.Text = (200.0 - (double) this.e).ToString() + "%";
    }

    private void d(object A_0, EventArgs A_1)
    {
      if ((double) this.e > 10.0)
        this.e -= 10f;
      this.n.Text = (200.0 - (double) this.e).ToString() + "%";
    }

    private void c(object A_0, EventArgs A_1)
    {
      if (this.c == RadarControl.RenderMode.Window)
        this.ParentForm.WindowState = FormWindowState.Minimized;
      else
        this.f = !this.f;
    }

    private void b(object A_0, EventArgs A_1)
    {
      if (this.c == RadarControl.RenderMode.Overlay)
        this.SetRenderMode(RadarControl.RenderMode.Window);
      else
        this.SetRenderMode(RadarControl.RenderMode.Overlay);
    }

    private void a(object A_0, EventArgs A_1)
    {
    }

    protected override void Dispose(bool disposing)
    {
      this.d();
      if (disposing && this.k != null)
        this.k.Dispose();
      base.Dispose(disposing);
    }

    private void b()
    {
      this.l = new Button();
      this.m = new Label();
      this.n = new Label();
      this.o = new Button();
      this.p = new Button();
      this.q = new Button();
      this.r = new Button();
      this.s = new Label();
      this.t = new TableLayoutPanel();
      this.u = new TableLayoutPanel();
      this.t.SuspendLayout();
      this.u.SuspendLayout();
      this.SuspendLayout();
      this.l.FlatAppearance.BorderSize = 0;
      this.l.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.l.Location = new Point(84, 20);
      this.l.Margin = new Padding(0);
      this.l.Name = "m_btnToggle";
      this.l.Size = new Size(22, 23);
      this.l.TabIndex = 10;
      this.l.Text = "T";
      this.l.UseVisualStyleBackColor = true;
      this.l.Click += new EventHandler(this.c);
      this.m.Anchor = AnchorStyles.Left;
      this.m.AutoSize = true;
      this.m.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.m.ForeColor = Color.DimGray;
      this.m.Location = new Point(183, 23);
      this.m.Name = "m_lblCount";
      this.m.Size = new Size(27, 16);
      this.m.TabIndex = 9;
      this.m.Text = "n/a";
      this.n.Anchor = AnchorStyles.Left;
      this.n.AutoSize = true;
      this.n.ForeColor = Color.DarkGray;
      this.n.Location = new Point(153, 25);
      this.n.Name = "m_lblZoom";
      this.n.Size = new Size(24, 13);
      this.n.TabIndex = 8;
      this.n.Text = "n/a";
      this.o.FlatAppearance.BorderSize = 0;
      this.o.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.o.Location = new Point(20, 20);
      this.o.Margin = new Padding(0);
      this.o.Name = "m_btnZoomOut";
      this.o.Size = new Size(22, 23);
      this.o.TabIndex = 7;
      this.o.Text = "-";
      this.o.UseVisualStyleBackColor = true;
      this.o.Click += new EventHandler(this.e);
      this.p.FlatAppearance.BorderSize = 0;
      this.p.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.p.Location = new Point(42, 20);
      this.p.Margin = new Padding(0);
      this.p.Name = "m_btnZoomIn";
      this.p.Size = new Size(22, 23);
      this.p.TabIndex = 6;
      this.p.Text = "+";
      this.p.UseVisualStyleBackColor = true;
      this.p.Click += new EventHandler(this.d);
      this.q.FlatAppearance.BorderSize = 0;
      this.q.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.q.Location = new Point(106, 20);
      this.q.Margin = new Padding(0);
      this.q.Name = "m_btnMode";
      this.q.Size = new Size(22, 23);
      this.q.TabIndex = 11;
      this.q.Text = "M";
      this.q.UseVisualStyleBackColor = true;
      this.q.Click += new EventHandler(this.b);
      this.r.FlatAppearance.BorderSize = 0;
      this.r.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.r.Location = new Point(128, 20);
      this.r.Margin = new Padding(0);
      this.r.Name = "m_btnOptions";
      this.r.Size = new Size(22, 23);
      this.r.TabIndex = 12;
      this.r.Text = "O";
      this.r.UseVisualStyleBackColor = true;
      this.r.Click += new EventHandler(this.a);
      this.s.Anchor = AnchorStyles.Left;
      this.s.AutoSize = true;
      this.s.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.s.ForeColor = Color.DimGray;
      this.s.Location = new Point(3, 0);
      this.s.Name = "m_lblCoords";
      this.s.Size = new Size(27, 16);
      this.s.TabIndex = 13;
      this.s.Text = "n/a";
      this.t.AutoSize = true;
      this.t.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.t.ColumnCount = 10;
      this.t.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
      this.t.ColumnStyles.Add(new ColumnStyle());
      this.t.ColumnStyles.Add(new ColumnStyle());
      this.t.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
      this.t.ColumnStyles.Add(new ColumnStyle());
      this.t.ColumnStyles.Add(new ColumnStyle());
      this.t.ColumnStyles.Add(new ColumnStyle());
      this.t.ColumnStyles.Add(new ColumnStyle());
      this.t.ColumnStyles.Add(new ColumnStyle());
      this.t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.t.Controls.Add((Control) this.o, 1, 1);
      this.t.Controls.Add((Control) this.p, 2, 1);
      this.t.Controls.Add((Control) this.n, 8, 1);
      this.t.Controls.Add((Control) this.m, 9, 1);
      this.t.Controls.Add((Control) this.r, 6, 1);
      this.t.Controls.Add((Control) this.l, 4, 1);
      this.t.Controls.Add((Control) this.q, 5, 1);
      this.t.Location = new Point(0, 0);
      this.t.Name = "tableLayoutPanel1";
      this.t.RowCount = 2;
      this.t.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.t.RowStyles.Add(new RowStyle());
      this.t.Size = new Size(213, 43);
      this.t.TabIndex = 14;
      this.u.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.u.AutoSize = true;
      this.u.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.u.ColumnCount = 2;
      this.u.ColumnStyles.Add(new ColumnStyle());
      this.u.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
      this.u.Controls.Add((Control) this.s, 0, 0);
      this.u.Location = new Point(289, 293);
      this.u.Name = "tableLayoutPanel2";
      this.u.RowCount = 2;
      this.u.RowStyles.Add(new RowStyle());
      this.u.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.u.Size = new Size(53, 36);
      this.u.TabIndex = 15;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Black;
      this.Controls.Add((Control) this.u);
      this.Controls.Add((Control) this.t);
      this.DoubleBuffered = true;
      this.Name = "RadarControl";
      this.Size = new Size(342, 329);
      this.Paint += new PaintEventHandler(this.a);
      this.t.ResumeLayout(false);
      this.t.PerformLayout();
      this.u.ResumeLayout(false);
      this.u.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public enum RenderMode
    {
      Overlay,
      Window,
    }
  }
}
