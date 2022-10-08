// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.NetCarbonSequestrationBySpecies
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Util;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  internal class NetCarbonSequestrationBySpecies : DatabaseReport
  {
    public NetCarbonSequestrationBySpecies() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTtileAnnualNetCarbonSequestrationOfTreesBySpecies;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Insert(0, (RenderObject) renderTable);
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      int count = 2;
      renderTable.Width = (Unit) "70%";
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontSize = 14f;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.NetCarbonSequestration;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.CO2Equivalent;
      renderTable.Cells[1, 1].Text = renderTable.Cells[1, 2].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.TonneUnits());
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      DataTable carbonSequestration = this.GetCarbonSequestration();
      int num = count;
      foreach (DataRow row in (InternalDataCollectionBase) carbonSequestration.Rows)
      {
        Tuple<string, string> tuple = this.estUtil.ClassValues[Classifiers.Species][(short) ReportUtil.ConvertFromDBVal<int>(row[classifierName])];
        renderTable.Cells[num, 0].Text = ReportBase.ScientificName ? tuple.Item2 : tuple.Item1;
        double english = EstimateUtil.ConvertToEnglish(ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]), Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None), ReportBase.EnglishUnits);
        renderTable.Cells[num, 1].Text = english.ToString("N2");
        renderTable.Cells[num, 2].Text = (english * 3.667).ToString("N2");
        ++num;
      }
      renderTable.Cells[num, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      double english1 = EstimateUtil.ConvertToEnglish(ReportUtil.ConvertFromDBVal<double>((object) carbonSequestration.AsEnumerable().Sum<DataRow>((Func<DataRow, double>) (x => x.Field<double>("EstimateValue")))), Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None), ReportBase.EnglishUnits);
      renderTable.Cells[num, 1].Text = english1.ToString("N2");
      renderTable.Cells[num, 2].Text = (english1 * 3.667).ToString("N2");
      renderTable.Rows[num].Style.Borders.Top = LineDef.Default;
      renderTable.Rows[num].Style.FontBold = true;
      ReportUtil.FormatRenderTable(renderTable);
      if (!this.ProjectIsUsingTropicalEquations())
        return;
      this.Note(C1doc);
    }

    protected override string ReportMessage() => i_Tree_Eco_v6.Resources.Strings.MsgTropicalEquations;

    private DataTable GetCarbonSequestration() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NetCarbonSequestration]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatesQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesWithSEAndTotals(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Species]);
  }
}
