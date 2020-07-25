// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Movement
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using pwnagebot.GameInterface.Frameworks;
using pwnagebot.GameInterface.Frameworks.Logging;
using System;
using System.Threading;

namespace pwnagebot.GameInterface
{
  public abstract class Movement
  {
    private static FileLog a = Singleton<FileLog>.Instance;
    private Interface b;

    protected Interface InterfaceRef
    {
      get
      {
        return this.b;
      }
    }

    public Movement(Interface interface_)
    {
      this.b = interface_;
    }

    public abstract void Forward();

    public abstract void Backward();

    public abstract void Stop();

    public void Face(Entity entity_)
    {
      this.Face(entity_.X, entity_.Y);
    }

    public abstract void Face(float x_, float y_);

    public abstract void Face(float heading_);

    public bool MoveTo(Entity entity_, float minDist_, bool stopRunning_, bool stopOnDamage_, int waitFor_)
    {
      return this.MoveTo(entity_, 0.0f, 0.0f, 0.0f, minDist_, stopRunning_, stopOnDamage_, waitFor_);
    }

    public bool MoveTo(float x_, float y_, float z_, float minDist_, bool stopRunning_, bool stopOnDamage_, int waitFor_)
    {
      return this.MoveTo((Entity) null, x_, y_, z_, minDist_, stopRunning_, stopOnDamage_, waitFor_);
    }

    protected bool MoveTo(Entity entity_, float x_, float y_, float z_, float minDist_, bool stopRunning_, bool stopOnDamage_, int waitFor_)
    {
      Entity me = this.b.EntityManager.Me;
      int num1 = 0;
      int num2 = 0;
      DateTime now = DateTime.Now;
      if (me == null)
        return false;
      float x_1;
      float y_1;
      float z_1;
      if (entity_ != null)
      {
        entity_.Update();
        x_1 = entity_.X;
        y_1 = entity_.Y;
        z_1 = entity_.Z;
      }
      else
      {
        x_1 = x_;
        y_1 = y_;
        z_1 = z_;
      }
      me.Update();
      if ((double) z_ == 0.0)
        z_1 = me.Z;
      Random random = new Random();
      float num3 = me.DistanceTo(x_1, y_1, z_1);
      if ((double) num3 <= (double) minDist_)
      {
        if (stopRunning_)
          this.Stop();
        this.Face(x_, y_);
        return true;
      }
      double num4 = (me.HeadingTo(x_1, y_1) - (double) me.Heading) % 180.0;
      while (num4 < 0.0)
        num4 += 180.0;
      if (num4 > 90.0)
      {
        this.Face(x_1, y_1);
        me.Update();
        num4 = (me.HeadingTo(x_1, y_1) - (double) me.Heading) % 180.0;
        while (num4 < 0.0)
          num4 += 180.0;
      }
      this.Forward();
      float health = me.Health;
      while ((double) num3 > (double) minDist_ && (double) num3 < 200.0 && !me.Dead)
      {
        float x = me.X;
        float y = me.Y;
        float z = me.Z;
        if ((double) num3 < (double) minDist_ * 2.0 && num4 > 175.0 && (num4 < 185.0 && !stopRunning_))
          return true;
        if ((double) num3 - (double) minDist_ < 1.5 && num4 > 45.0)
        {
          Movement.a.Trace("DEBUG: heading diff: " + (object) num4 + ", distToWalk: " + (object) num3);
          this.Stop();
          this.Face(x_1, y_1);
          this.Forward();
          Thread.Sleep(250);
        }
        else
        {
          this.Face(x_1, y_1);
          Thread.Sleep(100);
          me.Update();
          if ((double) z_ == 0.0)
            z_1 = me.Z;
          if ((double) me.DistanceTo(x, y, z) < 0.3)
          {
            this.Forward();
            num2 = 0;
            ++num1;
            if (num1 == 10 || num1 == 15 || (num1 == 20 || num1 == 25))
              Movement.a.Trace("stuckCounter: " + (object) num1);
            if (num1 == 10)
            {
              this.Face((float) (((double) me.Heading + 90.0) % 360.0));
              Thread.Sleep(1000 + random.Next(0, num1 * 10));
              this.Face(x_, y_);
            }
            if (num1 == 15)
            {
              this.Face((float) (((double) me.Heading + 270.0) % 360.0));
              Thread.Sleep(1000 + random.Next(0, num1 * 10));
              this.Face(x_, y_);
            }
            if (num1 == 20)
            {
              this.Face((float) random.Next(0, 360));
              Thread.Sleep(2000 + random.Next(0, num1 * 10));
              this.Face(x_, y_);
            }
            if (num1 == 25)
            {
              this.Stop();
              return false;
            }
          }
          else
          {
            ++num2;
            if (num2 > 5)
              num1 = 0;
          }
        }
        if ((DateTime.Now - now).TotalSeconds > (double) waitFor_)
        {
          Movement.a.Info(string.Format("Movement timed out ({0:0.0} away)! ({1:0.0}, {2:0.0}, {3:0.0})", (object) num3, (object) x_1, (object) y_1, (object) z_1));
          return false;
        }
        me.Update();
        if ((double) z_ == 0.0)
          z_1 = me.Z;
        if (stopOnDamage_ && (double) health > (double) me.Health)
        {
          this.Stop();
          return false;
        }
        if (entity_ != null)
        {
          entity_.Update();
          x_1 = entity_.X;
          y_1 = entity_.Y;
          z_1 = entity_.Z;
        }
        num3 = me.DistanceTo(x_1, y_1, z_1);
        num4 = (me.HeadingTo(x_1, y_1) - (double) me.Heading) % 180.0;
        while (num4 < 0.0)
          num4 += 180.0;
      }
      if ((double) num3 > 200.0)
      {
        Movement.a.Info(string.Format("Location is too far to walk ({0:0.0} away)! ({1:0.0}, {2:0.0}, {3:0.0})", (object) num3, (object) x_1, (object) y_1, (object) z_1));
        return false;
      }
      if (stopRunning_)
        this.Stop();
      return true;
    }

    public bool Forward(float distance_, int waitFor_)
    {
      this.Forward();
      bool flag = this.a(distance_, waitFor_);
      this.Stop();
      return flag;
    }

    public bool Backward(float distance_, int waitFor_)
    {
      this.Backward();
      bool flag = this.a(distance_, waitFor_);
      this.Stop();
      return flag;
    }

    private bool a(float A_0, int A_1)
    {
      Entity me = this.b.EntityManager.Me;
      float x = me.X;
      float y = me.Y;
      float z = me.Z;
      DateTime now = DateTime.Now;
      while ((double) me.DistanceTo(x, y, z) < (double) A_0)
      {
        if ((DateTime.Now - now).TotalSeconds > (double) A_1)
          return false;
        Thread.Sleep(10);
        me.Update();
      }
      return true;
    }
  }
}
