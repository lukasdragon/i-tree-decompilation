// Decompiled with JetBrains decompiler
// Type: <CrtImplementationDetails>.OpenMPWithMultipleAppdomainsException
// Assembly: UFOREWrapper, Version=1.0.5750.15158, Culture=neutral, PublicKeyToken=null
// MVID: A0E9D70D-C711-40C2-8444-67229D4F47C1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFOREWrapper.dll

using System;
using System.Runtime.Serialization;

namespace \u003CCrtImplementationDetails\u003E
{
  [Serializable]
  internal class OpenMPWithMultipleAppdomainsException : System.Exception
  {
    protected OpenMPWithMultipleAppdomainsException(
      SerializationInfo info,
      StreamingContext context)
      : base(info, context)
    {
    }

    public OpenMPWithMultipleAppdomainsException()
    {
    }
  }
}
