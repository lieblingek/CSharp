// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Interface
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using pwnagebot.GameInterface.Frameworks;
using pwnagebot.GameInterface.Frameworks.Logging;
using pwnagebot.GameInterface.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading;

namespace pwnagebot.GameInterface
{
  public abstract class Interface
  {
    private static FileLog b = Singleton<FileLog>.Instance;
    private const uint a = 8192;
    private Thread c;
    private int d;
    private string e;
    private Interface.WindowInfo f;
    private IntPtr g;
    private string h;
    private bool i;
    private string j;
    private object k;
    private uint l;
    protected MemoryReader m_reader;
    protected Pointers m_pointers;
    protected EntityManager m_entityManager;
    protected Movement m_movement;
    protected Fishing m_fishing;
    protected Mapper m_navMapper;
    private pwnagebot.GameInterface.Frameworks.c m;
    private List<Interface.a> n;
    private IInteractable o;

    public IInteractable Interact
    {
      get
      {
        return this.o;
      }
      set
      {
        this.o = value;
      }
    }

    public string DllToInject
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

    public int UpdateInterval
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

    protected string ProcessTitle
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

    public bool Injected
    {
      get
      {
        return this.j != string.Empty;
      }
    }

    public EntityManager EntityManager
    {
      get
      {
        return this.m_entityManager;
      }
    }

    public Fishing Fishing
    {
      get
      {
        return this.m_fishing;
      }
    }

    public Mapper NavMapper
    {
      get
      {
        return this.m_navMapper;
      }
      set
      {
        this.m_navMapper = value;
      }
    }

    public Movement Movement
    {
      get
      {
        return this.m_movement;
      }
      set
      {
        this.m_movement = value;
      }
    }

    public string MapperDataPath
    {
      get
      {
        return this.h;
      }
      set
      {
        this.h = value;
      }
    }

    public bool PipeServer
    {
      get
      {
        return this.i;
      }
      set
      {
        this.i = value;
      }
    }

    public event EventHandler OnUpdate;

    public event Interface.MessageDelegate OnMessage;

    public event EventHandler<EventArgs<bool>> OnCastComplete;

    public event EventHandler<EventArgs> OnDeath;

    public event EventHandler<EventArgs<int>> OnLevelChanged;

    public event EventHandler<EventArgs> OnLineOfSight;

    public event EventHandler<EventArgs> OnMustFaceTarget;

    public event EventHandler<EventArgs> OnInvalidTarget;

    public Interface()
    {
      this.d = 100;
      this.m_pointers = new Pointers();
      this.m_navMapper = new Mapper(this);
      this.k = new object();
      this.n = new List<Interface.a>();
    }

    public Interface.InterfaceError RetrievePointers(string user_, string password_)
    {
      pwnagebot.GameInterface.Offset.a a = new pwnagebot.GameInterface.Offset.a(user_, password_);
      Hashtable hashtable = new Hashtable();
      try
      {
        FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
        string str = "pointers";
        Interface.b.Debug("Requesting table " + str + " " + this.f.Version.FileVersion + " from server.");
        a.a(str + " " + this.f.Version.FileVersion, hashtable);
      }
      catch (Exception ex)
      {
        Interface.b.Log(Logger.MessageType.Failure, ex.Message);
        this.Shutdown();
        if (ex.Message == "Out of date version")
          return Interface.InterfaceError.OUT_OF_DATE_VERSION;
        throw ex;
      }
      if (hashtable.Count == 0)
      {
        Interface.b.Log(Logger.MessageType.Failure, "Failed to retrieve offsets from server!");
        return Interface.InterfaceError.FAILED_TO_RETRIEVE_OFFSETS;
      }
      this.m_pointers.Set(hashtable);
      return Interface.InterfaceError.SUCCESS;
    }

    public Interface.InterfaceError Initialize(string user, string password)
    {
      Interface.b.Trace("Initializing game interface...");
      List<Interface.WindowInfo> windowList = Interface.GetWindowList(this.e);
      if (windowList.Count <= 0)
        return Interface.InterfaceError.CLIENT_NOT_FOUND;
      this.f = windowList[0];
      Interface.InterfaceError interfaceError1 = this.e();
      if (interfaceError1 != Interface.InterfaceError.SUCCESS)
        return interfaceError1;
      Interface.InterfaceError interfaceError2 = this.RetrievePointers(user, password);
      if (interfaceError2 != Interface.InterfaceError.SUCCESS)
      {
        this.Shutdown();
        return interfaceError2;
      }
      this.LoadPointers();
      if (this.i && !this.f())
        return Interface.InterfaceError.FAILED_TO_SETUP_NAMED_PIPE;
      if (this.j != string.Empty && !this.g())
        return Interface.InterfaceError.UNKNOWN;
      this.Interact.SetClientId(this.l);
      this.d();
      this.RegisterEvents();
      return Interface.InterfaceError.SUCCESS;
    }

    [DllImport("DLLInject.dll")]
    internal static extern void InjectDLLByWindow(IntPtr A_0, string A_1);

    private bool g()
    {
      Interface.InjectDLLByWindow(this.f.Handle, this.j);
      return true;
    }

    private bool f()
    {
      this.m = new pwnagebot.GameInterface.Frameworks.c(8192U);
      if (!this.m.a(this.f.Handle))
        return false;
      this.m.a(new pwnagebot.GameInterface.Frameworks.c.a(this.a));
      this.m.b();
      return true;
    }

    private Interface.InterfaceError e()
    {
      this.l = 0U;
      pwnagebot.GameInterface.Frameworks.a.a();
      int windowThreadProcessId = (int) pwnagebot.GameInterface.Frameworks.a.GetWindowThreadProcessId((uint) this.f.Handle.ToInt32(), out this.l);
      if ((int) this.l == 0)
      {
        Interface.b.Log(Logger.MessageType.Failure, "OpenClient: Couldn't get process ID!");
        return Interface.InterfaceError.UNKNOWN;
      }
      this.g = pwnagebot.GameInterface.Frameworks.a.OpenProcess(pwnagebot.GameInterface.Frameworks.a.c.l, false, this.l);
      if (this.g == IntPtr.Zero)
      {
        Interface.b.Log(Logger.MessageType.Failure, "OpenClient: Failed to open client process! (LastError = " + pwnagebot.GameInterface.Frameworks.a.GetLastError().ToString() + ")");
        return Interface.InterfaceError.UNKNOWN;
      }
      this.m_reader = new MemoryReader(this.g);
      return Interface.InterfaceError.SUCCESS;
    }

    public abstract bool SaveMapData();

    public bool SaveMapData(string filename_)
    {
      if (!Directory.Exists(this.MapperDataPath))
        Directory.CreateDirectory(this.MapperDataPath);
      FileStream fileStream = new FileStream(this.MapperDataPath + filename_, FileMode.Create);
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      try
      {
        binaryFormatter.Serialize((Stream) fileStream, (object) this.m_navMapper);
      }
      catch (SerializationException ex)
      {
        Interface.b.Log(Logger.MessageType.Failure, "Failed to serialize. Reason: " + ex.Message);
        throw;
      }
      finally
      {
        fileStream.Close();
      }
      return true;
    }

    public bool LoadMapData(string filename_)
    {
      this.m_navMapper = (Mapper) null;
      if (!Directory.Exists(this.MapperDataPath) || !File.Exists(this.MapperDataPath + filename_))
        return false;
      FileStream fileStream = new FileStream(this.MapperDataPath + filename_, FileMode.Open);
      try
      {
        this.m_navMapper = (Mapper) new BinaryFormatter().Deserialize((Stream) fileStream);
        this.m_navMapper.Interface = this;
        this.m_navMapper.DefaultSize = 20.0;
        ReadOnlyCollection<Region> regionsByDistance = this.m_navMapper.NavRef.GetAllRegionsByDistance(0.0f, 0.0f, 0.0f);
        if (regionsByDistance != null)
        {
          foreach (Region child_ in regionsByDistance)
          {
            Region.Point3f point3f = child_.CenterPoint();
            if (double.IsNaN((double) point3f.X) || double.IsNaN((double) point3f.Y) || double.IsNaN((double) point3f.Z))
              child_.Parent.RemoveChild(child_);
          }
        }
      }
      catch (SerializationException ex)
      {
        Interface.b.Info("Failed to deserialize. Reason: " + ex.Message);
        throw;
      }
      finally
      {
        fileStream.Close();
      }
      return true;
    }

    private void d()
    {
      Interface.b.Trace("Starting interface update loop.");
      this.c = new Thread(new ThreadStart(this.b));
      this.c.IsBackground = true;
      this.c.Name = "Interface Updates";
      this.c.Start();
    }

    private void c()
    {
      Interface.b.Trace("Aborting interface update loop.");
      try
      {
        if (this.c == null)
          return;
        this.c.Abort();
      }
      catch
      {
      }
    }

    public virtual void Shutdown()
    {
      Interface.b.Trace("Shutting down game interface.");
      this.DeregisterEvents();
      this.c();
      this.m_reader = (MemoryReader) null;
      if (this.g != IntPtr.Zero)
      {
        pwnagebot.GameInterface.Frameworks.a.CloseHandle(this.g);
        this.g = IntPtr.Zero;
      }
      if (this.m != null)
      {
        this.m.d();
        this.m = (pwnagebot.GameInterface.Frameworks.c) null;
      }
      this.r = (EventHandler<EventArgs<bool>>) null;
      this.s = (EventHandler<EventArgs>) null;
      this.t = (EventHandler<EventArgs<int>>) null;
      this.u = (EventHandler<EventArgs>) null;
      this.v = (EventHandler<EventArgs>) null;
      this.w = (EventHandler<EventArgs>) null;
    }

    public static List<Interface.WindowInfo> GetWindowList(string processName_)
    {
      Process[] processesByName = Process.GetProcessesByName(processName_);
      List<Interface.WindowInfo> windowInfoList = new List<Interface.WindowInfo>();
      Interface.WindowInfo windowInfo = new Interface.WindowInfo();
      foreach (Process process in processesByName)
      {
        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(process.MainModule.FileName);
        windowInfo.Title = process.MainWindowTitle;
        windowInfo.Handle = process.MainWindowHandle;
        windowInfo.ProcessHandle = process.Id;
        windowInfo.Version = versionInfo;
        windowInfoList.Add(windowInfo);
      }
      return windowInfoList;
    }

    private void b()
    {
      try
      {
        while (true)
        {
          if (this.p != null)
            this.p((object) null, EventArgs.Empty);
          this.a();
          Thread.Sleep(this.d);
        }
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (ThreadStateException ex)
      {
      }
    }

    private void a()
    {
      Interface.a a;
      do
      {
        lock (this.k)
        {
          if (this.n.Count > 0)
          {
            a = this.n[0];
            if ((uint) (DateTime.Now - a.a).TotalMilliseconds < 100U)
              break;
            this.n.RemoveAt(0);
          }
          else
            a = (Interface.a) null;
        }
        if (a != null && this.q != null)
          this.q(a.b);
      }
      while (a != null);
    }

    private static string c(string A_0)
    {
      return Regex.Replace(Regex.Replace(A_0, "\r", string.Empty), "\\s+\\n", "\n");
    }

    private static bool b(string A_0)
    {
      return Regex.Match(A_0, "^-?[0-9-, /]+\\n?$").Success || Regex.Match(A_0, "^W+$").Success;
    }

    private void a(string A_0)
    {
      A_0 = Interface.c(A_0);
      if (Interface.b(A_0))
        return;
      lock (this.k)
      {
        foreach (Interface.a item_0 in this.n)
        {
          if (item_0.b == A_0)
            return;
        }
        this.n.Add(new Interface.a(DateTime.Now, A_0));
      }
    }

    protected virtual void CastCompleted(object sender_, EventArgs<bool> eventArgs_)
    {
      Interface.b.Info("Cast completed: " + (eventArgs_.Value ? "success" : "failure"));
      if (this.r == null)
        return;
      this.r(sender_, eventArgs_);
    }

    protected virtual void LevelChanged(object sender_, EventArgs<int> eventArgs_)
    {
      Interface.b.Info("Level changed: " + (object) eventArgs_.Value);
      if (this.t == null)
        return;
      this.t(sender_, eventArgs_);
    }

    protected virtual void LineOfSight(object sender_, EventArgs eventArgs_)
    {
      Interface.b.Info("Line of sight warning.");
      if (this.u == null)
        return;
      this.u(sender_, eventArgs_);
    }

    protected virtual void MustFaceTarget(object sender_, EventArgs eventArgs_)
    {
      Interface.b.Info("Must face target warning.");
      if (this.v == null)
        return;
      this.v(sender_, eventArgs_);
    }

    protected virtual void PlayerDied(object sender_, EventArgs eventArgs_)
    {
      Interface.b.Info("Player died!");
      if (this.s == null)
        return;
      this.s(sender_, eventArgs_);
    }

    protected virtual void InvalidTarget(object sender_, EventArgs eventArgs_)
    {
      Interface.b.Info("Invalid target, ignoring.");
      if (this.w == null)
        return;
      this.w(sender_, eventArgs_);
    }

    protected virtual void LoadPointers()
    {
    }

    protected abstract void RegisterEvents();

    protected virtual void DeregisterEvents()
    {
      this.p = (EventHandler) null;
      this.q = (Interface.MessageDelegate) null;
    }

    public enum InterfaceError
    {
      SUCCESS,
      CLIENT_NOT_FOUND,
      FAILED_TO_RETRIEVE_OFFSETS,
      MISSING_OFFSETS,
      FAILED_TO_SETUP_NAMED_PIPE,
      FAILED_TO_OPEN_CLIENT,
      TIMED_OUT_GAME_UPDATE,
      TIME_OUT_PIPE_SERVER,
      OUT_OF_DATE_VERSION,
      UNKNOWN,
    }

    public delegate void MessageDelegate(string s_);

    public class WindowInfo
    {
      public string Title;
      public IntPtr Handle;
      public int ProcessHandle;
      public FileVersionInfo Version;

      public override string ToString()
      {
        return this.ProcessHandle.ToString() + " - " + this.Title;
      }
    }

    private class a
    {
      public DateTime a;
      public string b;

      public a(DateTime A_0, string A_1)
      {
        this.a = A_0;
        this.b = A_1;
      }
    }
  }
}
