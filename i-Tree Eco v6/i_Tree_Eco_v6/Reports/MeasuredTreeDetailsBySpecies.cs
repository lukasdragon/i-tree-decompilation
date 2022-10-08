// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.MeasuredTreeDetailsBySpecies
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Util;
using NHibernate.Transform;
using System;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class MeasuredTreeDetailsBySpecies : DatabaseReport
  {
    public MeasuredTreeDetailsBySpecies() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleMeasuredTreeDetailsBySpecies;

    public override object GetData()
    {
      DataTable dataTable = this.curInputISession.GetNamedQuery(nameof (MeasuredTreeDetailsBySpecies)).SetParameter<Guid>("y", this.YearGuid).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
      dataTable.DefaultView.Sort = ReportBase.ScientificName ? "SppScientificName" : "SppCommonName";
      return (object) dataTable.DefaultView.ToTable();
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      DataTable data = (DataTable) this.GetData();
      int num1 = data.AsEnumerable().Sum<DataRow>((Func<DataRow, int>) (row => row.Field<int>("CountOfSppScientificName")));
      double english1 = EstimateUtil.ConvertToEnglish(data.AsEnumerable().Sum<DataRow>((Func<DataRow, double>) (row => row.Field<double>("SumOfGroundArea"))), Units.Squaremeter, ReportBase.EnglishUnits);
      double adjustedLeafArea1 = this.GetUnitsAdjustedLeafArea(data.AsEnumerable().Sum<DataRow>((Func<DataRow, double>) (row => row.Field<double>("SumOfLeafArea"))));
      double english2 = EstimateUtil.ConvertToEnglish(data.AsEnumerable().Sum<DataRow>((Func<DataRow, double>) (row => row.Field<double>("SumOfLeafBioMass"))), Units.Kilograms, ReportBase.EnglishUnits);
      double english3 = EstimateUtil.ConvertToEnglish(data.AsEnumerable().Sum<DataRow>((Func<DataRow, double>) (row => row.Field<double>("SumOfBasalArea"))), Units.Squaremeter, ReportBase.EnglishUnits);
      C1doc.ClipPage = true;
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[1, 1].Text = renderTable.Cells[1, 3].Text = renderTable.Cells[1, 5].Text = renderTable.Cells[1, 7].Text = renderTable.Cells[1, 9].Text = i_Tree_Eco_v6.Resources.Strings.Value;
      renderTable.Cells[1, 2].Text = renderTable.Cells[1, 4].Text = renderTable.Cells[1, 6].Text = renderTable.Cells[1, 8].Text = renderTable.Cells[1, 10].Text = "%";
      renderTable.Cols[3].Style.Borders.Left = this.tableBorderLineGray;
      renderTable.Cells[0, 1].SpanCols = renderTable.Cells[0, 3].SpanCols = renderTable.Cells[0, 5].SpanCols = renderTable.Cells[0, 7].SpanCols = renderTable.Cells[0, 9].SpanCols = 2;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.SpeciesName;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.TreeCount;
      renderTable.Cells[0, 3].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.CanopyCover, this.SquareMeterUnits());
      renderTable.Cells[0, 5].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafArea, this.SquareMeterUnits());
      renderTable.Cells[0, 7].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafBiomass, ReportBase.KgUnits());
      renderTable.Cells[0, 9].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.BasalArea, this.SquareMeterUnits());
      renderTable.Cols[5].Style.Borders.Left = renderTable.Cols[7].Style.Borders.Left = renderTable.Cols[9].Style.Borders.Left = renderTable.Cols[10].Style.Borders.Right = this.tableBorderLineGray;
      int num2 = count;
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        renderTable.Cells[num2, 0].Text = ReportBase.ScientificName ? ReportUtil.ConvertFromDBVal<string>(row["SppScientificName"]) : ReportUtil.ConvertFromDBVal<string>(row["SppCommonName"]);
        int num3 = ReportUtil.ConvertFromDBVal<int>(row["CountOfSppScientificName"]);
        renderTable.Cells[num2, 1].Text = num3.ToString("N0");
        renderTable.Cells[num2, 2].Text = ((double) num3 / (double) num1 * 100.0).ToString("N1");
        double english4 = EstimateUtil.ConvertToEnglish(ReportUtil.ConvertFromDBVal<double>(row["SumOfGroundArea"]), Units.Squaremeter, ReportBase.EnglishUnits);
        renderTable.Cells[num2, 3].Text = english4.ToString("N1");
        renderTable.Cells[num2, 4].Text = (english4 / english1 * 100.0).ToString("N1");
        double adjustedLeafArea2 = this.GetUnitsAdjustedLeafArea(ReportUtil.ConvertFromDBVal<double>(row["SumOfLeafArea"]));
        renderTable.Cells[num2, 5].Text = adjustedLeafArea2.ToString("N1");
        renderTable.Cells[num2, 6].Text = (adjustedLeafArea2 / adjustedLeafArea1 * 100.0).ToString("N1");
        double english5 = EstimateUtil.ConvertToEnglish(ReportUtil.ConvertFromDBVal<double>(row["SumOfLeafBioMass"]), Units.Kilograms, ReportBase.EnglishUnits);
        renderTable.Cells[num2, 7].Text = english5.ToString("N1");
        TableCell cell1 = renderTable.Cells[num2, 8];
        double num4 = english5 / english2 * 100.0;
        string str1 = num4.ToString("N1");
        cell1.Text = str1;
        double english6 = EstimateUtil.ConvertToEnglish(ReportUtil.ConvertFromDBVal<double>(row["SumOfBasalArea"]), Units.Squaremeter, ReportBase.EnglishUnits);
        renderTable.Cells[num2, 9].Text = english6.ToString("N1");
        TableCell cell2 = renderTable.Cells[num2, 10];
        num4 = english6 / english3 * 100.0;
        string str2 = num4.ToString("N1");
        cell2.Text = str2;
        ++num2;
      }
      renderTable.Rows[num2].Style.Borders.Top = LineDef.Default;
      renderTable.Rows[num2].Style.FontBold = true;
      renderTable.Cells[num2, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      renderTable.Cells[num2, 1].Text = num1.ToString("N0");
      renderTable.Cells[num2, 3].Text = english1.ToString("N1");
      renderTable.Cells[num2, 5].Text = adjustedLeafArea1.ToString("N1");
      renderTable.Cells[num2, 7].Text = english2.ToString("N1");
      renderTable.Cells[num2, 9].Text = english3.ToString("N1");
      renderTable.Cells[num2, 2].Text = renderTable.Cells[num2, 4].Text = renderTable.Cells[num2, 6].Text = renderTable.Cells[num2, 8].Text = renderTable.Cells[num2, 10].Text = "100";
      ReportUtil.FormatRenderTable(renderTable);
      for (int row = 1; row < renderTable.Rows.Count; ++row)
      {
        for (int col = 1; col < renderTable.Cols.Count; ++col)
        {
          if (renderTable.Cells[row, col].Text == "0.0")
            renderTable.Cells[row, col].Text = "<0.1";
        }
      }
    }

    private double GetUnitsAdjustedLeafArea(double val) => EstimateUtil.ConvertToEnglish(val, Units.Squaremeter, ReportBase.EnglishUnits);
  }
}
