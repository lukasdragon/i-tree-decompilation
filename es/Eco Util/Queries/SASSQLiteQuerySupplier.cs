// Decompiled with JetBrains decompiler
// Type: Eco.Util.Queries.SASSQLiteQuerySupplier
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Util.Queries.Interfaces;
using Eco.Util.Queries.SQLite;
using NHibernate;

namespace Eco.Util.Queries
{
  internal class SASSQLiteQuerySupplier : SASIQuerySupplier
  {
    public ISession Session { get; private set; }

    public SASSQLiteQuerySupplier(ISession session) => this.Session = session;

    public SASIUtilProvider GetSASUtilProvider() => (SASIUtilProvider) new SASUtilProvider((SASIQuerySupplier) this);
  }
}
