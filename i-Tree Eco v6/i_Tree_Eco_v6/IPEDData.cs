// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.IPEDData
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Core;
using i_Tree_Eco_v6.Resources;
using IPED.Domain;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace i_Tree_Eco_v6
{
  public class IPEDData
  {
    public IPEDData(ISessionFactory sf)
    {
      if (sf == null)
        throw new ArgumentNullException(nameof (sf));
      using (ISession s = sf.OpenSession())
      {
        using (ITransaction transaction = s.BeginTransaction())
        {
          this.Pests = this.GetIPEDPests(s);
          this.BBAbnGrowth = this.GetIPEDLookup<IPED.Domain.BBAbnGrowth>(s);
          this.BBDiseaseSigns = this.GetIPEDLookup<IPED.Domain.BBDiseaseSigns>(s);
          this.BBInsectPres = this.GetIPEDLookup<IPED.Domain.BBInsectPres>(s);
          this.BBInsectSigns = this.GetIPEDLookup<IPED.Domain.BBInsectSigns>(s);
          this.BBProbLoc = this.GetIPEDLookup<IPED.Domain.BBProbLoc>(s);
          this.FTAbnFoli = this.GetIPEDLookup<IPED.Domain.FTAbnFoli>(s);
          this.FTChewFoli = this.GetIPEDLookup<IPED.Domain.FTChewFoli>(s);
          this.FTDiscFoli = this.GetIPEDLookup<IPED.Domain.FTDiscFoli>(s);
          this.FTFoliAffect = this.GetIPEDLookup<IPED.Domain.FTFoliAffect>(s);
          this.FTInsectSigns = this.GetIPEDLookup<IPED.Domain.FTInsectSigns>(s);
          this.TSDieback = this.GetIPEDLookup<IPED.Domain.TSDieback>(s);
          this.TSEnvStress = this.GetIPEDLookup<IPED.Domain.TSEnvStress>(s);
          this.TSEpiSprout = this.GetIPEDLookup<IPED.Domain.TSEpiSprout>(s);
          this.TSHumStress = this.GetIPEDLookup<IPED.Domain.TSHumStress>(s);
          this.TSWiltFoli = this.GetIPEDLookup<IPED.Domain.TSWiltFoli>(s);
          transaction.Commit();
        }
      }
    }

    public IList<Pest> Pests { get; private set; }

    public IList<IPED.Domain.BBAbnGrowth> BBAbnGrowth { get; private set; }

    public IList<IPED.Domain.BBDiseaseSigns> BBDiseaseSigns { get; private set; }

    public IList<IPED.Domain.BBInsectPres> BBInsectPres { get; private set; }

    public IList<IPED.Domain.BBInsectSigns> BBInsectSigns { get; private set; }

    public IList<IPED.Domain.BBProbLoc> BBProbLoc { get; private set; }

    public IList<IPED.Domain.FTAbnFoli> FTAbnFoli { get; private set; }

    public IList<IPED.Domain.FTChewFoli> FTChewFoli { get; private set; }

    public IList<IPED.Domain.FTDiscFoli> FTDiscFoli { get; private set; }

    public IList<IPED.Domain.FTFoliAffect> FTFoliAffect { get; private set; }

    public IList<IPED.Domain.FTInsectSigns> FTInsectSigns { get; private set; }

    public IList<IPED.Domain.TSDieback> TSDieback { get; private set; }

    public IList<IPED.Domain.TSEnvStress> TSEnvStress { get; private set; }

    public IList<IPED.Domain.TSEpiSprout> TSEpiSprout { get; private set; }

    public IList<IPED.Domain.TSHumStress> TSHumStress { get; private set; }

    public IList<IPED.Domain.TSWiltFoli> TSWiltFoli { get; private set; }

    private IList<Pest> GetIPEDPests(ISession s)
    {
      List<Pest> list = new List<Pest>();
      list.Add(new Pest()
      {
        Id = 0,
        CommonName = Strings.EntryNotEntered,
        ScientificName = Strings.EntryNotEntered
      });
      list.Add(new Pest()
      {
        Id = -1,
        CommonName = Strings.EntryUnknownPest,
        ScientificName = Strings.EntryUnknownPest
      });
      list.AddRange((IEnumerable<Pest>) s.CreateCriteria<Pest>().AddOrder(Order.Asc(TypeHelper.NameOf<Pest>((System.Linq.Expressions.Expression<Func<Pest, object>>) (p => p.CommonName)))).List<Pest>());
      return (IList<Pest>) new ReadOnlyCollection<Pest>((IList<Pest>) list);
    }

    private IList<T> GetIPEDLookup<T>(ISession s) where T : Lookup => (IList<T>) new ReadOnlyCollection<T>(s.CreateCriteria<T>().Add((ICriterion) Restrictions.IsNotNull(TypeHelper.NameOf<T>((System.Linq.Expressions.Expression<Func<T, object>>) (l => (object) l.Sequence)))).AddOrder(Order.Asc(TypeHelper.NameOf<T>((System.Linq.Expressions.Expression<Func<T, object>>) (l => (object) l.Sequence)))).List<T>());
  }
}
