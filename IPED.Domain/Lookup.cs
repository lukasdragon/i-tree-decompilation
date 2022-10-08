// Decompiled with JetBrains decompiler
// Type: IPED.Domain.Lookup
// Assembly: IPED.Domain, Version=1.1.6145.0, Culture=neutral, PublicKeyToken=null
// MVID: A1138CF7-F031-4F0B-8D41-1DE13D446B52
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\IPED.Domain.dll

using IPED.Domain.Attributes;
using IPED.Domain.Properties;
using System.ComponentModel;

namespace IPED.Domain
{
  public abstract class Lookup
  {
    [Browsable(false)]
    [LocalizedDescription(typeof (Resources), "Lookup_Id")]
    public virtual int Id { get; protected set; }

    [LocalizedDescription(typeof (Resources), "Lookup_Code")]
    public virtual int Code { get; set; }

    public virtual int? Sequence { get; set; }

    [LocalizedDescription(typeof (Resources), "Lookup_Description")]
    public virtual string Description { get; set; }

    public virtual string IPEDDescription { get; set; }
  }
}
