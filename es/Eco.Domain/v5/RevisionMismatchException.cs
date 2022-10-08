// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.RevisionMismatchException
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;
using System.Runtime.Serialization;

namespace Eco.Domain.v5
{
  public class RevisionMismatchException : Exception
  {
    public RevisionMismatchException()
    {
    }

    public RevisionMismatchException(string message)
      : base(message)
    {
    }

    public RevisionMismatchException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public RevisionMismatchException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public virtual long Revision1 { get; set; }

    public virtual long Revision2 { get; set; }
  }
}
