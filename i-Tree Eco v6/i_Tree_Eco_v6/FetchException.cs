// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.FetchException
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System;
using System.Runtime.Serialization;

namespace i_Tree_Eco_v6
{
  public class FetchException : Exception
  {
    public FetchException()
    {
    }

    public FetchException(string message)
      : base(message)
    {
    }

    public FetchException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public FetchException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
