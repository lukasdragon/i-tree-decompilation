// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.EnergyEffectsOfTrees
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Util;
using System;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class EnergyEffectsOfTrees : DatabaseReport
  {
    public EnergyEffectsOfTrees() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleEnergyEffectsOfTrees;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      if (this.CountTreesWithBuildings() == 0)
      {
        this.DisplayStandardMessage(C1doc, i_Tree_Eco_v6.Resources.Strings.MsgNoData);
      }
      else
      {
        int count = 2;
        string str = "80%";
        RenderTable renderTable1 = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) renderTable1);
        renderTable1.Width = (Unit) str;
        renderTable1.Style.FontSize = 11f;
        renderTable1.Style.Spacing.Bottom = (Unit) "1cm";
        renderTable1.RowGroups[0, count].Header = TableHeaderEnum.Page;
        renderTable1.Rows[0].Style.FontSize = 13f;
        renderTable1.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
        renderTable1.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Amounts;
        renderTable1.Cells[1, 0].Text = i_Tree_Eco_v6.Resources.Strings.Type;
        renderTable1.Cells[2, 0].Text = i_Tree_Eco_v6.Resources.Strings.UnitMBTU;
        renderTable1.Cells[3, 0].Text = i_Tree_Eco_v6.Resources.Strings.UnitMWH;
        renderTable1.Cells[4, 0].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.CarbonAvoided, ReportBase.TonneUnits());
        renderTable1.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Heating;
        renderTable1.Cells[1, 2].Text = i_Tree_Eco_v6.Resources.Strings.Cooling;
        renderTable1.Cells[1, 3].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        renderTable1.Cells[2, 1].Text = string.Format("{0:N3}", (object) this.estUtil.GetEnergyValues(1, 2, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon));
        renderTable1.Cells[2, 2].Text = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        renderTable1.Cells[2, 3].Text = renderTable1.Cells[2, 1].Text;
        renderTable1.Cells[3, 1].Text = string.Format("{0:N3}", (object) this.estUtil.GetEnergyValues(2, 2, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon));
        renderTable1.Cells[3, 2].Text = string.Format("{0:N3}", (object) this.estUtil.GetEnergyValues(2, 1, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon));
        renderTable1.Cells[3, 3].Text = string.Format("{0:N3}", (object) (this.estUtil.GetEnergyValues(2, 2, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon) + this.estUtil.GetEnergyValues(2, 1, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon)));
        renderTable1.Cells[4, 1].Text = string.Format("{0:N3}", (object) EstimateUtil.ConvertToEnglish(this.estUtil.GetEnergyValues(3, 2, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon), Units.MetricTons, ReportBase.EnglishUnits));
        renderTable1.Cells[4, 2].Text = string.Format("{0:N3}", (object) EstimateUtil.ConvertToEnglish(this.estUtil.GetEnergyValues(3, 1, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon), Units.MetricTons, ReportBase.EnglishUnits));
        renderTable1.Cells[4, 3].Text = string.Format("{0:N3}", (object) EstimateUtil.ConvertToEnglish(this.estUtil.GetEnergyValues(3, 2, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon) + this.estUtil.GetEnergyValues(3, 1, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon), Units.MetricTons, ReportBase.EnglishUnits));
        ReportUtil.FormatRenderTable(renderTable1);
        RenderTable renderTable2 = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) renderTable2);
        renderTable2.Width = (Unit) str;
        renderTable2.Style.FontSize = 11f;
        renderTable2.Rows[0].Style.FontSize = 13f;
        renderTable2.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
        renderTable2.RowGroups[0, count].Header = TableHeaderEnum.Page;
        renderTable2.Cells[0, 0].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.EnergyValues, this.CurrencySymbol);
        renderTable2.Cells[1, 0].Text = i_Tree_Eco_v6.Resources.Strings.Type;
        renderTable2.Cells[2, 0].Text = i_Tree_Eco_v6.Resources.Strings.UnitMBTU;
        renderTable2.Cells[3, 0].Text = i_Tree_Eco_v6.Resources.Strings.UnitMWH;
        renderTable2.Cells[4, 0].Text = i_Tree_Eco_v6.Resources.Strings.CarbonAvoided;
        renderTable2.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Heating;
        renderTable2.Cells[1, 2].Text = i_Tree_Eco_v6.Resources.Strings.Cooling;
        renderTable2.Cells[1, 3].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        renderTable2.Cells[2, 1].Text = string.Format("{0:N0}", (object) this.estUtil.GetEnergyValues(1, 2, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon));
        renderTable2.Cells[2, 2].Text = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        renderTable2.Cells[2, 3].Text = renderTable2.Cells[2, 1].Text;
        renderTable2.Cells[3, 1].Text = string.Format("{0:N0}", (object) this.estUtil.GetEnergyValues(2, 2, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon));
        renderTable2.Cells[3, 2].Text = string.Format("{0:N0}", (object) this.estUtil.GetEnergyValues(2, 1, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon));
        renderTable2.Cells[3, 3].Text = string.Format("{0:N0}", (object) (this.estUtil.GetEnergyValues(2, 2, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon) + this.estUtil.GetEnergyValues(2, 1, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon)));
        renderTable2.Cells[4, 1].Text = string.Format("{0:N0}", (object) this.estUtil.GetEnergyValues(3, 2, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon));
        renderTable2.Cells[4, 2].Text = string.Format("{0:N0}", (object) this.estUtil.GetEnergyValues(3, 1, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon));
        renderTable2.Cells[4, 3].Text = string.Format("{0:N0}", (object) (this.estUtil.GetEnergyValues(3, 2, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon) + this.estUtil.GetEnergyValues(3, 1, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon)));
        ReportUtil.FormatRenderTable(renderTable2);
        this.Note(C1doc);
      }
    }

    protected override string ReportMessage() => this.Carbon_MetricTon_ShortTon_Footer(i_Tree_Eco_v6.Resources.Strings.NoteCarbon) + Environment.NewLine + this.Energy_MKW_MBTU_Footer();
  }
}
