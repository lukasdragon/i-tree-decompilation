// Decompiled with JetBrains decompiler
// Type: <CrtImplementationDetails>.Exception
// Assembly: UFOREWrapper, Version=1.0.5750.15158, Culture=neutral, PublicKeyToken=null
// MVID: A0E9D70D-C711-40C2-8444-67229D4F47C1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFOREWrapper.dll

using System;
using System.Runtime.Serialization;

namespace \u003CCrtImplementationDetails\u003E
{
  [Serializable]
  internal class Exception : System.Exception
  {
    protected Exception(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public Exception(string message, System.Exception innerException)
      : base(message, innerException)
    {
    }

    public Exception(string message)
      : base(message)
    {
    }
  }
}
