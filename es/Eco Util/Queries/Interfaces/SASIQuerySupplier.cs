// Decompiled with JetBrains decompiler
// Type: Eco.Util.Queries.Interfaces.SASIQuerySupplier
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using NHibernate;

namespace Eco.Util.Queries.Interfaces
{
  public interface SASIQuerySupplier
  {
    ISession Session { get; }

    SASIUtilProvider GetSASUtilProvider();
  }
}
