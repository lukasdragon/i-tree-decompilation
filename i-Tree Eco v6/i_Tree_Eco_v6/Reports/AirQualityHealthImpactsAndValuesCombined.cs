// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.AirQualityHealthImpactsAndValuesCombined
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Core;
using DaveyTree.NHibernate;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Resources;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;

namespace i_Tree_Eco_v6.Reports
{
  public class AirQualityHealthImpactsAndValuesCombined : AirQualityHealthImpactsAndValuesBase
  {
    public AirQualityHealthImpactsAndValuesCombined()
    {
      this.TreeShrub = "'T', 'S', 'G'";
      this.ReportTitle = this.CreateHeader(this.TreeShrub);
    }

    protected string CreateHeader(string filter)
    {
      IList<string> stringList = this.estUtil.queryProvider.GetEstimateUtilProvider().GetAvailableBenefitTypes(filter).SetParameter<Guid>("y", this.YearGuid).List<string>();
      List<object> objectList = new List<object>();
      foreach (string str in (IEnumerable<string>) stringList)
        objectList.Add(Enum.Parse(typeof (AbbreviationsEnum), str[0].ToString()));
      List<string> values = new List<string>();
      foreach (KeyValuePair<AbbreviationsEnum, string> keyValuePair in (IEnumerable<KeyValuePair<AbbreviationsEnum, string>>) EnumHelper.ConvertToDictionary<AbbreviationsEnum>())
      {
        if (objectList.Contains((object) keyValuePair.Key))
          values.Add(keyValuePair.Value);
      }
      return string.Format("{0}: {1}", (object) Strings.ReportTitlebyAirQualityHealthImpactsAndValuesBySummary, (object) string.Join(", ", (IEnumerable<string>) values));
    }

    protected override DataTable GetDBData() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetAirQualityHealthImpactsAndValuesCombined(this.TreeShrub).SetParameter<Guid>("y", this.YearGuid).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
  }
}
