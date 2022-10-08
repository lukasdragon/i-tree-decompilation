// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PollutantRemovalMonthlyBase
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Util;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  internal class PollutantRemovalMonthlyBase : DatabaseReport
  {
    public string TreeShrub = string.Empty;
    public bool isGrassReport;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      string notApplicableAbbr = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
      LineDef lineDef1 = new LineDef((Unit) "1pt", Color.Black);
      LineDef lineDef2 = new LineDef((Unit) "1pt", Color.Gray);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Cells[0, 0].SpanCols = 2;
      renderTable.Cells[0, 2].SpanCols = 3;
      renderTable.Cells[0, 5].SpanCols = 3;
      renderTable.Rows[0].Style.FontBold = true;
      renderTable.Rows[1].Style.FontBold = true;
      renderTable.Cols[5].Style.Borders.Left = lineDef2;
      renderTable.Cols[2].Style.Borders.Left = lineDef2;
      renderTable.Rows[0].Style.FontSize = 16f;
      renderTable.Rows[1].Style.FontSize = 14f;
      renderTable.Cols[1].Width = (Unit) "8%";
      for (int index = 2; index <= 7; ++index)
        renderTable.Cols[index].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.RowGroups[0, 2].PageHeader = true;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 2].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.Cells[0, 5].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.Cells[0, 2].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Removal, ReportBase.KilogramsUnits());
      renderTable.Cells[0, 5].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Value, this.CurrencySymbol);
      renderTable.Cells[1, 0].Text = i_Tree_Eco_v6.Resources.Strings.Pollutant;
      renderTable.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Month;
      renderTable.Cells[1, 2].Text = i_Tree_Eco_v6.Resources.Strings.Mean;
      renderTable.Cells[1, 3].Text = i_Tree_Eco_v6.Resources.Strings.Max;
      renderTable.Cells[1, 4].Text = i_Tree_Eco_v6.Resources.Strings.Min;
      renderTable.Cells[1, 5].Text = i_Tree_Eco_v6.Resources.Strings.Mean;
      renderTable.Cells[1, 6].Text = i_Tree_Eco_v6.Resources.Strings.Max;
      renderTable.Cells[1, 7].Text = i_Tree_Eco_v6.Resources.Strings.Min;
      IList<string> polutantsList = this.GetPolutantsList();
      if (this.isGrassReport && !this.DataExists())
      {
        DatabaseReport.NewReportMessage(C1doc, i_Tree_Eco_v6.Resources.Strings.MsgGrassReport);
        ReportUtil.FormatRenderTable(renderTable);
      }
      else
      {
        int num1 = 2;
        foreach (string pollutant in (IEnumerable<string>) polutantsList)
        {
          double d1 = 0.0;
          double d2 = 0.0;
          double d3 = 0.0;
          double num2 = 0.0;
          double num3 = 0.0;
          double num4 = 0.0;
          renderTable.Cells[num1, 0].Text = pollutant;
          double english;
          foreach (DataRow row in (InternalDataCollectionBase) this.GetData(pollutant).Rows)
          {
            int num5 = ReportUtil.ConvertFromDBVal<int>(row["Month"]);
            double d4 = ReportUtil.ConvertFromDBVal<double>(row["myAmount"]) / 1000.0;
            double d5 = ReportUtil.ConvertFromDBVal<double>(row["myAmountMax"]) / 1000.0;
            double d6 = ReportUtil.ConvertFromDBVal<double>(row["myAmountMin"]) / 1000.0;
            double num6 = 0.0;
            switch (pollutant)
            {
              case "CO":
                num6 = this.customizedCoDollarsPerTon / 1000.0;
                break;
              case "NO2":
                num6 = this.customizedNO2DollarsPerTon / 1000.0;
                break;
              case "O3":
                num6 = this.customizedO3DollarsPerTon / 1000.0;
                break;
              case "PM10*":
                num6 = this.customizedPM10DollarsPerTon / 1000.0;
                break;
              case "PM2.5":
              case "PM25":
                num6 = this.customizedPM25DollarsPerTon / 1000.0;
                break;
              case "SO2":
                num6 = this.customizedSO2DollarsPerTon / 1000.0;
                break;
            }
            renderTable.Cells[num1, 1].Text = num5.ToString();
            TableCell cell = renderTable.Cells[num1, 2];
            english = EstimateUtil.ConvertToEnglish(d4, Units.Kilograms, ReportBase.EnglishUnits);
            string str1 = english.ToString("N3");
            cell.Text = str1;
            double num7 = d4 * num6;
            double num8 = d5 * num6;
            double num9 = d6 * num6;
            string str2;
            string str3;
            string str4;
            string str5;
            if (pollutant == "CO")
            {
              string str6;
              str2 = str6 = notApplicableAbbr;
              str3 = str6;
              str4 = str6;
              str5 = str6;
            }
            else
            {
              english = EstimateUtil.ConvertToEnglish(d5, Units.Kilograms, ReportBase.EnglishUnits);
              str5 = english.ToString("N3");
              english = EstimateUtil.ConvertToEnglish(d6, Units.Kilograms, ReportBase.EnglishUnits);
              str4 = english.ToString("N3");
              str3 = num8.ToString("N2");
              str2 = num9.ToString("N2");
              d2 += d5;
              d3 += d6;
              num3 += num8;
              num4 += num9;
            }
            renderTable.Cells[num1, 3].Text = str5;
            renderTable.Cells[num1, 4].Text = str4;
            renderTable.Cells[num1, 5].Text = num7.ToString("N2");
            renderTable.Cells[num1, 6].Text = str3;
            renderTable.Cells[num1, 7].Text = str2;
            d1 += d4;
            num2 += num7;
            ++num1;
          }
          renderTable.Rows[num1].Style.FontBold = true;
          renderTable.Rows[num1].Style.Borders.Top = lineDef1;
          renderTable.Rows[num1].Style.Borders.Bottom = lineDef1;
          renderTable.Cells[num1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Annual;
          TableCell cell1 = renderTable.Cells[num1, 2];
          english = EstimateUtil.ConvertToEnglish(d1, Units.Kilograms, ReportBase.EnglishUnits);
          string str7 = english.ToString("N3");
          cell1.Text = str7;
          string str8;
          string str9;
          string str10;
          string str11;
          if (pollutant == "CO")
          {
            string str12;
            str8 = str12 = notApplicableAbbr;
            str9 = str12;
            str10 = str12;
            str11 = str12;
          }
          else
          {
            english = EstimateUtil.ConvertToEnglish(d2, Units.Kilograms, ReportBase.EnglishUnits);
            str11 = english.ToString("N3");
            english = EstimateUtil.ConvertToEnglish(d3, Units.Kilograms, ReportBase.EnglishUnits);
            str10 = english.ToString("N3");
            str9 = num3.ToString("N2");
            str8 = num4.ToString("N2");
          }
          renderTable.Cells[num1, 3].Text = str11;
          renderTable.Cells[num1, 4].Text = str10;
          renderTable.Cells[num1, 5].Text = num2.ToString("N2");
          renderTable.Cells[num1, 6].Text = str9;
          renderTable.Cells[num1, 7].Text = str8;
          ++num1;
        }
        ReportUtil.FormatRenderTable(renderTable);
        this.Note(C1doc);
      }
    }

    protected override string ReportMessage() => string.Format("{0} {1}", (object) this.PollutionRemoval_kg_lb_Footer(), (object) i_Tree_Eco_v6.Resources.Strings.NoteMinAndMaxValuesForCOAreNotCalculated) + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.NoteZeroValuesMeaning + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.WR_PM25Pm10;

    public DataTable GetData(string pollutant) => this.estUtil.queryProvider.GetEstimateUtilProvider().GetPollutantRemovalMonthlyValues(this.TreeShrub).SetParameter<Guid>("y", this.YearGuid).SetParameter<string>(nameof (pollutant), pollutant).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    protected bool DataExists() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetPollutionDataCount(this.TreeShrub).SetParameter<Guid>("y", this.YearGuid).UniqueResult<int>() > 0;

    protected IList<string> GetPolutantsList() => this.curInputISession.GetNamedQuery("GetPollutantsList").SetParameter<Guid>("y", this.YearGuid).List<string>();
  }
}
