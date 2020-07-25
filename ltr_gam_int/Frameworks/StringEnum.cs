// Decompiled with JetBrains decompiler
// Type: pwnagebot.GameInterface.Frameworks.StringEnum
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.Collections;
using System.Reflection;

namespace pwnagebot.GameInterface.Frameworks
{
  public class StringEnum
  {
    private static Hashtable b = new Hashtable();
    private static object c = new object();
    private Type a;

    public Type EnumType
    {
      get
      {
        return this.a;
      }
    }

    public StringEnum(Type enum_)
    {
      if (enum_ == null)
        throw new ArgumentNullException("enum_", "Supplied parameter is null");
      if (!enum_.IsEnum)
        throw new ArgumentException(string.Format("Supplied parameter must be an Enum.  Type was {0}", (object) enum_.ToString()), "enum_");
      this.a = enum_;
    }

    public string GetStringValue(string value_)
    {
      string str = (string) null;
      try
      {
        str = StringEnum.GetStringValue((Enum) Enum.Parse(this.a, value_));
      }
      catch (ArgumentNullException ex)
      {
      }
      catch (ArgumentException ex)
      {
      }
      return str;
    }

    public Array GetStringValues()
    {
      ArrayList arrayList = new ArrayList();
      foreach (MemberInfo field in this.a.GetFields())
      {
        StringEnum.StringValueAttribute[] customAttributes = field.GetCustomAttributes(typeof (StringEnum.StringValueAttribute), false) as StringEnum.StringValueAttribute[];
        if (customAttributes.Length > 0)
          arrayList.Add((object) customAttributes[0].Value);
      }
      return (Array) arrayList.ToArray();
    }

    public IList GetListValues()
    {
      Type underlyingType = Enum.GetUnderlyingType(this.a);
      ArrayList arrayList = new ArrayList();
      foreach (FieldInfo field in this.a.GetFields())
      {
        StringEnum.StringValueAttribute[] customAttributes = field.GetCustomAttributes(typeof (StringEnum.StringValueAttribute), false) as StringEnum.StringValueAttribute[];
        if (customAttributes.Length > 0)
          arrayList.Add((object) new DictionaryEntry(Convert.ChangeType(Enum.Parse(this.a, field.Name), underlyingType), (object) customAttributes[0].Value));
      }
      return (IList) arrayList;
    }

    public bool IsStringDefined(string value_)
    {
      return StringEnum.Parse(this.a, value_) != null;
    }

    public bool IsStringDefined(string value_, bool ignoreCase_)
    {
      return StringEnum.Parse(this.a, value_, ignoreCase_) != null;
    }

    public static string GetStringValue(Enum value_)
    {
      if (value_ == null)
        throw new ArgumentNullException(string.Format("Supplied type must be an Enum not null"));
      string str = (string) null;
      Type type = value_.GetType();
      lock (StringEnum.c)
      {
        if (StringEnum.b.ContainsKey((object) value_))
        {
          str = (StringEnum.b[(object) value_] as StringEnum.StringValueAttribute).Value;
        }
        else
        {
          StringEnum.StringValueAttribute[] local_3 = type.GetField(value_.ToString()).GetCustomAttributes(typeof (StringEnum.StringValueAttribute), false) as StringEnum.StringValueAttribute[];
          if (local_3.Length > 0)
          {
            StringEnum.b.Add((object) value_, (object) local_3[0]);
            str = local_3[0].Value;
          }
          else
          {
            str = Enum.GetName(value_.GetType(), (object) value_);
            StringEnum.b.Add((object) value_, (object) new StringEnum.StringValueAttribute(new string[1]{ str }));
          }
        }
      }
      return str;
    }

    public static object Parse(Type enum_, string value_)
    {
      return StringEnum.Parse(enum_, value_, false);
    }

    public static object Parse(Type enum_, string value_, bool ignoreCase_)
    {
      if (enum_ == null)
        throw new ArgumentNullException(string.Format("Supplied type must be an Enum not null"));
      if (!enum_.IsEnum)
        throw new ArgumentException(string.Format("Supplied type must be an Enum.  Type was {0}", (object) enum_.ToString()), "enum_");
      object obj = (object) null;
      foreach (FieldInfo field in enum_.GetFields())
      {
        StringEnum.StringValueAttribute[] customAttributes = field.GetCustomAttributes(typeof (StringEnum.StringValueAttribute), false) as StringEnum.StringValueAttribute[];
        if (customAttributes.Length > 0)
        {
          foreach (StringEnum.StringValueAttribute stringValueAttribute in customAttributes)
          {
            foreach (string strA in stringValueAttribute.Values)
            {
              if (string.Compare(strA, value_, ignoreCase_) == 0)
              {
                obj = Enum.Parse(enum_, field.Name);
                break;
              }
            }
          }
          if (obj != null)
            break;
        }
      }
      if (obj == null)
      {
        try
        {
          obj = Enum.Parse(enum_, value_, ignoreCase_);
        }
        catch (ArgumentException ex)
        {
          return obj;
        }
      }
      return obj;
    }

    public static bool IsStringDefined(Type enum_, string value_)
    {
      return StringEnum.Parse(enum_, value_) != null;
    }

    public static bool IsStringDefined(Type enum_, string value_, bool ignoreCase_)
    {
      return StringEnum.Parse(enum_, value_, ignoreCase_) != null;
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class StringValueAttribute : Attribute
    {
      private readonly string[] a;

      public string Value
      {
        get
        {
          return this.a[0];
        }
      }

      public string[] Values
      {
        get
        {
          return this.a;
        }
      }

      public StringValueAttribute(string[] values_)
      {
        if (values_ == null)
          throw new ArgumentNullException("values_", "StringValueAttribute received a null parameter");
        if (values_.Length == 0)
          throw new ArgumentException("StringValueAttribute received an empty array", "values_");
        this.a = values_;
      }
    }
  }
}
