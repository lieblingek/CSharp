// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Offset.Offsets
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System.Diagnostics;
using System.Reflection;

namespace pwnagebot.GameInterface.Offset
{
  public class Offsets
  {
    private string a;
    private string b;
    private pwnagebot.GameInterface.Offset.a c;

    public Offsets(string username_, string password_)
    {
      this.a = username_;
      this.b = password_;
      this.c = new pwnagebot.GameInterface.Offset.a(this.a, this.b);
    }

    public bool VerifyAccount(int accessLevel_)
    {
      return true;
    }

    public bool VerifyAppVersion(string application_, out string latestVersion_)
    {
      latestVersion_ = string.Empty;
      FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
      return this.c.a(application_, versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart, out latestVersion_);
    }

    public bool VerifyGameVersion(string game_)
    {
      return true;
    }
  }
}
