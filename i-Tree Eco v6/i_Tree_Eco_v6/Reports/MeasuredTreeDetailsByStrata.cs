// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.MeasuredTreeDetailsByStrata
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Domain.Properties;
using Eco.Util;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  internal class MeasuredTreeDetailsByStrata : DatabaseReport
  {
    public MeasuredTreeDetailsByStrata() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleMeasuredTreeDetailsByStratum;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontSize = 12f;
      ReportUtil.FormatRenderTableHeader(renderTable);
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 1].SpanCols = renderTable.Cells[0, 3].SpanCols = renderTable.Cells[0, 5].SpanCols = renderTable.Cells[0, 7].SpanCols = renderTable.Cells[0, 9].SpanCols = 2;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.TreeCount;
      renderTable.Cells[0, 3].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.CanopyCover, this.SquareMeterUnits());
      renderTable.Cells[0, 5].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafArea, this.SquareMeterUnits());
      renderTable.Cells[0, 7].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafBiomass, ReportBase.KgUnits());
      renderTable.Cells[0, 9].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.BasalArea, this.SquareMeterUnits());
      renderTable.Cells[1, 1].Text = renderTable.Cells[1, 3].Text = renderTable.Cells[1, 5].Text = renderTable.Cells[1, 7].Text = renderTable.Cells[1, 9].Text = i_Tree_Eco_v6.Resources.Strings.ValueAbbr;
      renderTable.Cells[1, 2].Text = renderTable.Cells[1, 4].Text = renderTable.Cells[1, 6].Text = renderTable.Cells[1, 8].Text = renderTable.Cells[1, 10].Text = "%";
      List<MeasuredTreeDetailsByStrata.TreeDetailsStrata> data = (List<MeasuredTreeDetailsByStrata.TreeDetailsStrata>) this.GetData();
      double num1 = (double) data.Sum<MeasuredTreeDetailsByStrata.TreeDetailsStrata>((Func<MeasuredTreeDetailsByStrata.TreeDetailsStrata, int>) (item => item.NumTrees));
      double val = data.Sum<MeasuredTreeDetailsByStrata.TreeDetailsStrata>((Func<MeasuredTreeDetailsByStrata.TreeDetailsStrata, double>) (item => item.LeafArea));
      double d1 = data.Sum<MeasuredTreeDetailsByStrata.TreeDetailsStrata>((Func<MeasuredTreeDetailsByStrata.TreeDetailsStrata, double>) (item => item.LeafBiomass));
      double d2 = data.Sum<MeasuredTreeDetailsByStrata.TreeDetailsStrata>((Func<MeasuredTreeDetailsByStrata.TreeDetailsStrata, double>) (item => item.CanopyCover));
      double d3 = data.Sum<MeasuredTreeDetailsByStrata.TreeDetailsStrata>((Func<MeasuredTreeDetailsByStrata.TreeDetailsStrata, double>) (item => item.BasalArea));
      int num2 = 2;
      for (int index = 0; index < data.Count; ++index)
      {
        string strataDescription = data[index].StrataDescription;
        renderTable.Cells[num2, 0].Text = data.Count != 1 || this.curYear.RecordStrata ? strataDescription : i_Tree_Eco_v6.Resources.Strings.StudyArea;
        renderTable.Cells[num2, 1].Text = data[index].NumTrees.ToString("N0");
        renderTable.Cells[num2, 2].Text = ((double) data[index].NumTrees / num1 * 100.0).ToString("N1");
        TableCell cell1 = renderTable.Cells[num2, 3];
        double num3 = EstimateUtil.ConvertToEnglish(data[index].CanopyCover, Units.Squaremeter, ReportBase.EnglishUnits);
        string str1 = num3.ToString("N1");
        cell1.Text = str1;
        TableCell cell2 = renderTable.Cells[num2, 4];
        num3 = data[index].CanopyCover / d2 * 100.0;
        string str2 = num3.ToString("N1");
        cell2.Text = str2;
        TableCell cell3 = renderTable.Cells[num2, 5];
        num3 = this.GetUnitsAdjustedLeafArea(data[index].LeafArea);
        string str3 = num3.ToString("N1");
        cell3.Text = str3;
        renderTable.Cells[num2, 6].Text = (data[index].LeafArea / val * 100.0).ToString("N1");
        renderTable.Cells[num2, 7].Text = EstimateUtil.ConvertToEnglish(data[index].LeafBiomass, Units.Kilograms, ReportBase.EnglishUnits).ToString("N1");
        renderTable.Cells[num2, 8].Text = (data[index].LeafBiomass / d1 * 100.0).ToString("N1");
        renderTable.Cells[num2, 9].Text = EstimateUtil.ConvertToEnglish(data[index].BasalArea, Units.Squaremeter, ReportBase.EnglishUnits).ToString("N1");
        renderTable.Cells[num2, 10].Text = (data[index].BasalArea / d3 * 100.0).ToString("N1");
        ++num2;
      }
      if (data.Count != 1)
        renderTable.Rows[num2].Style.FontBold = true;
      if (data.Count != 1)
      {
        renderTable.Rows[num2].Style.Borders.Top = LineDef.Default;
        renderTable.Cells[num2, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
        renderTable.Cells[num2, 1].Text = num1.ToString("N0");
        renderTable.Cells[num2, 3].Text = EstimateUtil.ConvertToEnglish(d2, Units.Squaremeter, ReportBase.EnglishUnits).ToString("N1");
        TableCell cell4 = renderTable.Cells[num2, 5];
        double num4 = this.GetUnitsAdjustedLeafArea(val);
        string str4 = num4.ToString("N1");
        cell4.Text = str4;
        TableCell cell5 = renderTable.Cells[num2, 7];
        num4 = EstimateUtil.ConvertToEnglish(d1, Units.Kilograms, ReportBase.EnglishUnits);
        string str5 = num4.ToString("N1");
        cell5.Text = str5;
        TableCell cell6 = renderTable.Cells[num2, 9];
        num4 = EstimateUtil.ConvertToEnglish(d3, Units.Squaremeter, ReportBase.EnglishUnits);
        string str6 = num4.ToString("N1");
        cell6.Text = str6;
        renderTable.Cells[num2, 2].Text = renderTable.Cells[num2, 4].Text = renderTable.Cells[num2, 6].Text = renderTable.Cells[num2, 8].Text = renderTable.Cells[num2, 10].Text = "100";
      }
      ReportUtil.FormatRenderTable(renderTable);
    }

    public override object GetData()
    {
      DataTable dataTable = this.curInputISession.GetNamedQuery(nameof (MeasuredTreeDetailsByStrata)).SetParameter<Guid>("y", this.YearGuid).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
      List<MeasuredTreeDetailsByStrata.TreeDetailsStrata> data = new List<MeasuredTreeDetailsByStrata.TreeDetailsStrata>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str = ReportUtil.ConvertFromDBVal<string>(row["Strata"]);
        data.Add(new MeasuredTreeDetailsByStrata.TreeDetailsStrata()
        {
          StrataDescription = str,
          NumTrees = ReportUtil.ConvertFromDBVal<int>(row["TreeCount"]),
          CanopyCover = ReportUtil.ConvertFromDBVal<double>(row["CanopyM2"]),
          LeafArea = ReportUtil.ConvertFromDBVal<double>(row["LeafAreaM2"]),
          LeafBiomass = ReportUtil.ConvertFromDBVal<double>(row["LeafBiomassKg"]),
          BasalArea = ReportUtil.ConvertFromDBVal<double>(row["BasalAreaM2"])
        });
      }
      return (object) data;
    }

    private double GetUnitsAdjustedLeafArea(double val) => EstimateUtil.ConvertToEnglish(val, Units.Squaremeter, ReportBase.EnglishUnits);

    private class TreeDetailsStrata
    {
      public string StrataDescription { get; set; }

      public int NumTrees { get; set; }

      public double CanopyCover { get; set; }

      public double LeafArea { get; set; }

      public double LeafBiomass { get; set; }

      public double BasalArea { get; set; }
    }
  }
}
