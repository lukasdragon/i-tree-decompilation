// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.LeafAreaByStrataBase
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.NHibernate;
using Eco.Util;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;

namespace i_Tree_Eco_v6.Reports
{
  public class LeafAreaByStrataBase : DatabaseReport
  {
    protected int _estUnits;

    public override object GetData()
    {
      DataTable stratumValuesData = this.GetEstimatedStratumValuesData();
      SortedList<int, double> data = new SortedList<int, double>();
      foreach (DataRow row in (InternalDataCollectionBase) stratumValuesData.Rows)
        data.Add(ReportUtil.ConvertFromDBVal<int>(row[this.estUtil.ClassifierNames[Classifiers.Strata]]), ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]));
      if (data.Count == 2)
        data.Remove((int) this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area"));
      return (object) data;
    }

    private DataTable GetEstimatedStratumValuesData() => this.GetEstimatedStratumValues().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.LeafArea]).SetParameter<int>("estUnits", this._estUnits).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatedStratumValues() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedStratumValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);
  }
}
