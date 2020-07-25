// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Frameworks.SerializationTester
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace pwnagebot.GameInterface.Frameworks
{
  public static class SerializationTester
  {
    public static T RoundTrip<T>(T value)
    {
      if ((object) value == null)
        throw new ArgumentNullException("value");
      using (MemoryStream memoryStream = new MemoryStream())
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize((Stream) memoryStream, (object) value);
        memoryStream.Seek(0L, SeekOrigin.Begin);
        return (T) binaryFormatter.Deserialize((Stream) memoryStream);
      }
    }
  }
}
