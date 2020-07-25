// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Offset.a
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;

namespace pwnagebot.GameInterface.Offset
{
  internal class a
  {
    private string a;
    private string b;
    private OffsetServer c;

    public a(string A_0, string A_1)
    {
      this.a = A_0;
      this.b = A_1;
      this.c = new OffsetServer();
      ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(pwnagebot.GameInterface.Offset.a.a);
    }

    internal bool a(string A_0, int A_1, int A_2, int A_3, int A_4, out string A_5)
    {
      A_5 = string.Empty;
      string str = this.c.Command(string.Format("{0}|{1}|version|{2}|{3}|{4}|{5}|{6}", (object) this.a, (object) this.b, (object) A_0, (object) A_1, (object) A_2, (object) A_3, (object) A_4));
      if (str.StartsWith("OK"))
        return true;
      if (!str.StartsWith("ERROR|"))
        throw new Exception("There was an unknown problem contacting the server. Please try again later.");
      A_5 = str.Split('|')[1];
      return false;
    }

    internal void a(string A_0, Hashtable A_1)
    {
      string str = this.c.Command(string.Format("{1}|{2}|retrieve|{0}", (object) A_0, (object) this.a, (object) this.b));
      if (!str.StartsWith("MORE|"))
      {
        if (str.StartsWith("ERROR|"))
          throw new Exception(str.Split('|')[1]);
        throw new Exception("There was an unknown problem contacting the server. Please try again later.");
      }
      int num = int.Parse(str.Split('|')[1].Replace("\r\n", ""));
      if (num == 0)
        throw new Exception("There was an unknown problem contacting the server. Please try again later.");
      try
      {
        string[] strArray = str.Split('|');
        int index = 2;
        while (index < 2 * num + 2)
        {
          A_1.Add((object) strArray[index], (object) int.Parse(strArray[index + 1], NumberStyles.HexNumber));
          index += 2;
        }
      }
      catch
      {
        throw new Exception("There was an unknown problem contacting the server. Please try again later.");
      }
    }

    public static bool a(object A_0, X509Certificate A_1, X509Chain A_2, SslPolicyErrors A_3)
    {
      if (A_1.GetCertHashString() != "5C1B6A6CB411F88004ADE3A77B0CA8E116314ADA")
        return false;
      if ((A_3 & SslPolicyErrors.RemoteCertificateChainErrors) == SslPolicyErrors.RemoteCertificateChainErrors)
        A_3 &= ~SslPolicyErrors.RemoteCertificateChainErrors;
      if ((A_3 & SslPolicyErrors.RemoteCertificateNameMismatch) == SslPolicyErrors.RemoteCertificateNameMismatch)
      {
        Zone fromUrl = Zone.CreateFromUrl(((WebRequest) A_0).RequestUri.ToString());
        return fromUrl.SecurityZone == SecurityZone.Intranet || fromUrl.SecurityZone == SecurityZone.MyComputer;
      }
      return A_3 == SslPolicyErrors.None;
    }
  }
}
