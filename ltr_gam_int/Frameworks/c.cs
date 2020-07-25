// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Frameworks.c
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using pwnagebot.GameInterface.Frameworks.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace pwnagebot.GameInterface.Frameworks
{
  //internal class c : IDisposable
  public class c : IDisposable
    {
        private static FileLog b = Singleton<FileLog>.Instance;
    private const string a = "\\\\.\\pipe\\lotroinj";
    private Thread cc;
    private IntPtr d;
    private uint e;
    private bool f;
    private pwnagebot.GameInterface.Frameworks.c.a g;

    public c(uint A_0)
    {
      this.e = A_0;
      this.d = new IntPtr(-1);
    }

    [SpecialName]
    public bool ccc()
    {
      return this.f;
    }

    [SpecialName]
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void a(pwnagebot.GameInterface.Frameworks.c.a A_0)
    {
      this.g = this.g + A_0;
    }

    [SpecialName]
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void b(pwnagebot.GameInterface.Frameworks.c.a A_0)
    {
      this.g = this.g - A_0;
    }

    public void b()
    {
      this.cc = new Thread(new ThreadStart(this.a));
      this.cc.IsBackground = true;
      this.cc.Name = "Pipe Server Listener";
      this.cc.Start();
    }

    public bool a(IntPtr A_0)
    {
      uint A_1 = 0;
      string A_0_1 = "\\\\.\\pipe\\lotroinj";
      int windowThreadProcessId = (int) pwnagebot.GameInterface.Frameworks.a.GetWindowThreadProcessId((uint) A_0.ToInt32(), out A_1);
      if ((int) A_1 == 0)
      {
        pwnagebot.GameInterface.Frameworks.c.b.Log(Logger.MessageType.Failure, "Failed to find process ID for window '" + A_0.ToString() + "'");
        return false;
      }
      if (A_0 != IntPtr.Zero)
        A_0_1 = "\\\\.\\pipe\\lotroinj" + A_1.ToString();
      this.d = pwnagebot.GameInterface.Frameworks.b.CreateNamedPipe(A_0_1, 1U, 6U, (uint) byte.MaxValue, this.e, 0U, uint.MaxValue, IntPtr.Zero);
      if (this.d.ToInt32() == -1)
      {
        pwnagebot.GameInterface.Frameworks.c.b.Log(Logger.MessageType.Failure, "Failed to create pipe - " + pwnagebot.GameInterface.Frameworks.b.GetLastError().ToString() + ".\n");
        return false;
      }
      pwnagebot.GameInterface.Frameworks.c.b.Debug("Opened named pipe at \"" + A_0_1 + "\"");
      return true;
    }

    private void a()
    {
      byte[] A_1 = new byte[4];
      byte[] A_3 = new byte[4];
      try
      {
        pwnagebot.GameInterface.Frameworks.c.b.Debug("PipeListener has started.");
        while (true)
        {
          while (this.f)
          {
            if (pwnagebot.GameInterface.Frameworks.b.ReadFile(this.d, A_1, 4U, A_3, 0U))
            {
              int int32 = BitConverter.ToInt32(A_1, 0);
              if ((long) int32 > (long) this.e)
              {
                pwnagebot.GameInterface.Frameworks.c.b.Log(Logger.MessageType.Failure, "Pipe received too large of a message = " + int32.ToString());
              }
              else
              {
                byte[] numArray = new byte[int32];
                pwnagebot.GameInterface.Frameworks.b.ReadFile(this.d, numArray, (uint) int32, A_3, 0U);
                string A_0 = Encoding.Unicode.GetString(numArray);
                if (this.g != null)
                  this.g(A_0);
              }
            }
            else
              this.f = false;
          }
          this.f = pwnagebot.GameInterface.Frameworks.b.ConnectNamedPipe(this.d, (pwnagebot.GameInterface.Frameworks.b.a) null);
          if (this.f)
          {
            pwnagebot.GameInterface.Frameworks.c.b.Debug("Pipe Client connected.");
          }
          else
          {
            uint lastError = pwnagebot.GameInterface.Frameworks.b.GetLastError();
            switch (lastError)
            {
              case 232:
                Thread.Sleep(100);
                continue;
              case 535:
                pwnagebot.GameInterface.Frameworks.c.b.Debug("Pipe Client connected.");
                this.f = true;
                goto case 232;
              default:
                pwnagebot.GameInterface.Frameworks.c.b.Debug("Pipe client failed to connect, GetLastError = " + (object) lastError);
                goto case 232;
            }
          }
        }
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (ThreadStateException ex)
      {
      }
    }

    public void Dispose()
    {
      this.a(true);
      GC.SuppressFinalize((object) this);
    }

    private void a(bool A_0)
    {
      if (A_0 && this.cc != null && this.cc.IsAlive)
        this.cc.Abort();
      this.g = (pwnagebot.GameInterface.Frameworks.c.a) null;
      if (this.d.ToInt32() == -1)
        return;
      if (this.f)
        pwnagebot.GameInterface.Frameworks.b.DisconnectNamedPipe(this.d);
      pwnagebot.GameInterface.Frameworks.b.CloseHandle(this.d);
    }

    void object.e()
    {
      try
      {
        this.a(false);
      }
      finally
      {
        // ISSUE: explicit finalizer call
        this.Finalize();
      }
    }

    public delegate void a(string A_0);
  }
}
