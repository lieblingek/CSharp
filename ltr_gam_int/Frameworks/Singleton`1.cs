// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Frameworks.Singleton`1
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

namespace pwnagebot.GameInterface.Frameworks
{
  public static class Singleton<T> where T : new()
  {
    private static object a = new object();
    private static T b;

    public static T Instance
    {
      get
      {
        lock (Singleton<T>.a)
        {
          if ((object) Singleton<T>.b == null)
            Singleton<T>.b = new T();
          return Singleton<T>.b;
        }
      }
    }
  }
}
