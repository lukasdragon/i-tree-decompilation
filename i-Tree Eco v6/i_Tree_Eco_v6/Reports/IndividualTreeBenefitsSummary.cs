// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeBenefitsSummary
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Domain.v6;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

namespace i_Tree_Eco_v6.Reports
{
  public class IndividualTreeBenefitsSummary : DatabaseReport
  {
    private int _PM10RemovedOrd = -1;
    private double _customizedCarbonDollarsPerKg = -1.0;
    private double _customizedCoDollarsPerG = -1.0;
    private double _customizedO3DollarsPerG = -1.0;
    private double _customizedNO2DollarsPerG = -1.0;
    private double _customizedSO2DollarsPerG = -1.0;
    private double _customizedPM10DollarsPerG = -1.0;
    private double _customizedPM25DollarsPerG = -1.0;
    private SortedDictionary<Tuple<int, int>, IndividualTreeEnergyEffect> _energyEffects;
    private double _t_replacementValue;
    private double _t_carbonStorage;
    private double _t_carbonStorageValue;
    private double _t_carbonSequestration;
    private double _t_carbonSequestrationValue;
    private double _t_avoidedRunoff;
    private double _t_avoidedRunoffValue;
    private double _t_carbonAvoided;
    private double _t_carbonAvoidedValue;
    private double _t_polutionRemoved;
    private double _t_polutionRemovedValue;
    private double _t_combinedEnergyValue;
    private double _t_totalAnualBenefitsValue;
    private int _currPlotID;
    private int _currTreeID;
    private string _speciesName;
    private double _structualValue;
    private double _DBH;
    private double _carbonStorage;
    private double _carbonStorageValue;
    private double _grossCarbonSequestration;
    private double _grossCarbonSequestrationValue;
    private double _avoidedRunoff;
    private double _avoidedRunoffValue;
    private double? _carbonAvoided;
    private double? _carbonAvoidedValue;
    private double _pollutionRemoval;
    private double _pollutionRemovalValue;
    private double? _totalEnergyValue;
    private double _benefitsTotalValue;
    private object _xCoordinate;
    private object _yCoordinate;
    private string _comments;
    private string _UID;

    public IndividualTreeBenefitsSummary()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleIndividualTreeBenefitsSummary;
      this.hasComments = true;
      this.hasCoordinates = this.curYear.RecordTreeGPS;
      this.hasUID = this.curYear.RecordTreeUserId;
    }

    private void set_customizedCarbonDollarsPerKg() => this._customizedCarbonDollarsPerKg = this.customizedCarbonDollarsPerTon / 1000.0;

    private void set_customizedPulutionDollarsPerG()
    {
      this._customizedCoDollarsPerG = this.customizedCoDollarsPerTon / 1000000.0;
      this._customizedO3DollarsPerG = this.customizedO3DollarsPerTon / 1000000.0;
      this._customizedNO2DollarsPerG = this.customizedNO2DollarsPerTon / 1000000.0;
      this._customizedSO2DollarsPerG = this.customizedSO2DollarsPerTon / 1000000.0;
      this._customizedPM10DollarsPerG = this.customizedPM10DollarsPerTon / 1000000.0;
      this._customizedPM25DollarsPerG = this.customizedPM25DollarsPerTon / 1000000.0;
    }

    private Tuple<double, double> GetAmountAndMonetaryValue(
      double amount,
      double monetaryValue)
    {
      return new Tuple<double, double>(amount, amount * monetaryValue);
    }

    private double getEnergyValues(IndividualTreeEnergyEffect ee)
    {
      IndividualTreeEnergyEffect treeEnergyEffect = ee;
      return treeEnergyEffect.FuelsAvoidedHeating * this.customizedHeatingDollarsPerTherm * 10.0023877 + treeEnergyEffect.ElectricityAvoidedHeating * this.customizedElectricityDollarsPerKwh + treeEnergyEffect.ElectricityAvoidedCooling * this.customizedElectricityDollarsPerKwh;
    }

    private Tuple<double, double> getCarbonAvoided(IndividualTreeEnergyEffect ee)
    {
      IndividualTreeEnergyEffect treeEnergyEffect = ee;
      return new Tuple<double, double>(treeEnergyEffect.CarbonAvoidedKgPerYear, treeEnergyEffect.CarbonAvoidedValuePerYear);
    }

    private Tuple<double, double> GetPollutionRemoval(DataRow r)
    {
      double num1 = ReportUtil.ConvertFromDBVal<double>(r["CORemoved"]);
      double num2 = ReportUtil.ConvertFromDBVal<double>(r["O3Removed"]);
      double num3 = ReportUtil.ConvertFromDBVal<double>(r["NO2Removed"]);
      double num4 = ReportUtil.ConvertFromDBVal<double>(r["SO2Removed"]);
      double num5 = ReportUtil.ConvertFromDBVal<double>(r["PM25Removed"]);
      double num6 = ReportUtil.ConvertFromDBVal<double>(r["PM10Removed"]);
      return new Tuple<double, double>(num1 + num2 + num3 + num4 + num5 + num6, num1 * this._customizedCoDollarsPerG + num2 * this._customizedO3DollarsPerG + num3 * this._customizedNO2DollarsPerG + num4 * this._customizedSO2DollarsPerG + num5 * this._customizedPM25DollarsPerG + num6 * this._customizedPM10DollarsPerG);
    }

    private void SetBenefitsDataRow(DataRow r)
    {
      this._currPlotID = ReportUtil.ConvertFromDBVal<int>(r["PlotId"]);
      this._currTreeID = ReportUtil.ConvertFromDBVal<int>(r["TreeId"]);
      this._speciesName = ReportBase.ScientificName ? ReportUtil.ConvertFromDBVal<string>(r["SppScientificName"]) : ReportUtil.ConvertFromDBVal<string>(r["SppCommonName"]);
      this._DBH = ReportUtil.ConvertFromDBVal<double>(r["DBH"]);
      this._structualValue = ReportUtil.ConvertFromDBVal<double>(r["TreeValue"]);
      Tuple<double, double> andMonetaryValue1 = this.GetAmountAndMonetaryValue(ReportUtil.ConvertFromDBVal<double>(r["CarbonStorage"]), this._customizedCarbonDollarsPerKg);
      this._carbonStorage = andMonetaryValue1.Item1;
      this._carbonStorageValue = andMonetaryValue1.Item2;
      Tuple<double, double> andMonetaryValue2 = this.GetAmountAndMonetaryValue(ReportUtil.ConvertFromDBVal<double>(r["GrossCarbonSeq"]), this._customizedCarbonDollarsPerKg);
      this._grossCarbonSequestration = andMonetaryValue2.Item1;
      this._grossCarbonSequestrationValue = andMonetaryValue2.Item2;
      Tuple<double, double> andMonetaryValue3 = this.GetAmountAndMonetaryValue(ReportUtil.ConvertFromDBVal<double>(r["AvoidedRunoff"]), this.customizedWaterDollarsPerM3);
      this._avoidedRunoff = andMonetaryValue3.Item1;
      this._avoidedRunoffValue = andMonetaryValue3.Item2;
      Tuple<double, double> pollutionRemoval = this.GetPollutionRemoval(r);
      this._pollutionRemoval = pollutionRemoval.Item1;
      this._pollutionRemovalValue = pollutionRemoval.Item2;
      this._benefitsTotalValue = andMonetaryValue2.Item2 + andMonetaryValue3.Item2 + pollutionRemoval.Item2;
      if (ReportBase.energyEffectsAvailable)
      {
        Tuple<int, int> key = new Tuple<int, int>(this._currPlotID, this._currTreeID);
        if (!this._energyEffects.ContainsKey(key))
        {
          this._totalEnergyValue = new double?();
          this._carbonAvoided = new double?();
          this._carbonAvoidedValue = new double?();
        }
        else
        {
          IndividualTreeEnergyEffect energyEffect = this._energyEffects[key];
          double energyValues = this.getEnergyValues(energyEffect);
          this._t_combinedEnergyValue += energyValues;
          this._totalEnergyValue = new double?(energyValues);
          this._benefitsTotalValue += energyValues;
          Tuple<double, double> carbonAvoided = this.getCarbonAvoided(energyEffect);
          double num1 = carbonAvoided.Item1;
          double num2 = carbonAvoided.Item2;
          this._carbonAvoided = new double?(num1);
          this._carbonAvoidedValue = new double?(num2);
          this._t_carbonAvoided += num1;
          this._t_carbonAvoidedValue += num2;
          this._benefitsTotalValue += num2;
        }
      }
      this._xCoordinate = r["xCoordinate"];
      this._yCoordinate = r["yCoordinate"];
      this._comments = ReportUtil.ConvertFromDBVal<string>(r["comments"]);
      this._UID = ReportUtil.ConvertFromDBVal<string>(r["UserId"]);
      this.IncrementTotals();
    }

    private void AllowNullColumn(DataTable data, string colName, Type type) => data.Columns.Add(new DataColumn(colName, type)
    {
      AllowDBNull = true
    });

    private void InitTotals()
    {
      this._t_replacementValue = 0.0;
      this._t_carbonStorage = 0.0;
      this._t_carbonStorageValue = 0.0;
      this._t_carbonSequestration = 0.0;
      this._t_carbonSequestrationValue = 0.0;
      this._t_avoidedRunoff = 0.0;
      this._t_avoidedRunoffValue = 0.0;
      this._t_carbonAvoided = 0.0;
      this._t_carbonAvoidedValue = 0.0;
      this._t_polutionRemoved = 0.0;
      this._t_polutionRemovedValue = 0.0;
      this._t_combinedEnergyValue = 0.0;
      this._t_totalAnualBenefitsValue = 0.0;
      this._energyEffects = new SortedDictionary<Tuple<int, int>, IndividualTreeEnergyEffect>();
    }

    private void IncrementTotals()
    {
      this._t_replacementValue += this._structualValue;
      this._t_carbonStorage += this._carbonStorage;
      this._t_carbonStorageValue += this._carbonStorageValue;
      this._t_carbonSequestration += this._grossCarbonSequestration;
      this._t_carbonSequestrationValue += this._grossCarbonSequestrationValue;
      this._t_avoidedRunoff += this._avoidedRunoff;
      this._t_avoidedRunoffValue += this._avoidedRunoffValue;
      this._t_polutionRemoved += this._pollutionRemoval;
      this._t_polutionRemovedValue += this._pollutionRemovalValue;
      this._t_totalAnualBenefitsValue += this._benefitsTotalValue;
    }

    public override object GetData()
    {
      this.InitTotals();
      this.set_customizedCarbonDollarsPerKg();
      this.set_customizedPulutionDollarsPerG();
      if (this.curYear.RecordEnergy && this.CheckNations())
      {
        this.CalculateEnergy<IndividualTreeEnergyEffect>(this._energyEffects);
        ReportBase.energyEffectsAvailable = true;
      }
      else
        ReportBase.energyEffectsAvailable = false;
      DataTable dataTable = this.curInputISession.GetNamedQuery(nameof (IndividualTreeBenefitsSummary)).SetParameter<Guid>("y", this.YearGuid).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
      DataTable data = this.InitBenefitsObject();
      foreach (DataRow row1 in (InternalDataCollectionBase) dataTable.Rows)
      {
        DataRow row2 = data.NewRow();
        this.SetBenefitsDataRow(row1);
        row2["PlotID"] = (object) this._currPlotID;
        row2["TreeID"] = (object) this._currTreeID;
        row2["speciesName"] = (object) this._speciesName;
        row2["DBH"] = (object) this._DBH;
        row2["structualValue"] = (object) this._structualValue;
        row2["carbonStorage"] = (object) this._carbonStorage;
        row2["carbonStorageValue"] = (object) this._carbonStorageValue;
        row2["grossCarbonSequestration"] = (object) this._grossCarbonSequestration;
        row2["grossCarbonSequestrationValue"] = (object) this._grossCarbonSequestrationValue;
        row2["avoidedRunoff"] = (object) this._avoidedRunoff;
        row2["avoidedRunoffValue"] = (object) this._avoidedRunoffValue;
        DataRow dataRow1 = row2;
        double? nullable = this._carbonAvoided;
        object obj1 = (object) nullable ?? Convert.DBNull;
        dataRow1["carbonAvoided"] = obj1;
        DataRow dataRow2 = row2;
        nullable = this._carbonAvoidedValue;
        object obj2 = (object) nullable ?? Convert.DBNull;
        dataRow2["carbonAvoidedValue"] = obj2;
        row2["pollutionRemoval"] = (object) this._pollutionRemoval;
        row2["pollutionRemovalValue"] = (object) this._pollutionRemovalValue;
        DataRow dataRow3 = row2;
        nullable = this._totalEnergyValue;
        object obj3 = (object) nullable ?? Convert.DBNull;
        dataRow3["totalEnergyValue"] = obj3;
        row2["benefitsTotalValue"] = (object) this._benefitsTotalValue;
        row2["xCoordinate"] = this._xCoordinate;
        row2["yCoordinate"] = this._yCoordinate;
        row2["comments"] = (object) this._comments;
        row2["UID"] = (object) this._UID;
        data.Rows.Add(row2);
      }
      return (object) data;
    }

    private DataTable InitBenefitsObject()
    {
      DataTable data = new DataTable();
      data.Columns.Add("PlotID", typeof (int));
      data.Columns.Add("TreeID", typeof (int));
      data.Columns.Add("speciesName", typeof (string));
      data.Columns.Add("DBH", typeof (double));
      data.Columns.Add("structualValue", typeof (double));
      data.Columns.Add("carbonStorage", typeof (double));
      data.Columns.Add("carbonStorageValue", typeof (double));
      data.Columns.Add("grossCarbonSequestration", typeof (double));
      data.Columns.Add("grossCarbonSequestrationValue", typeof (double));
      data.Columns.Add("avoidedRunoff", typeof (double));
      data.Columns.Add("avoidedRunoffValue", typeof (double));
      data.Columns.Add("carbonAvoided", typeof (double));
      data.Columns.Add("carbonAvoidedValue", typeof (double));
      data.Columns.Add("pollutionRemoval", typeof (double));
      data.Columns.Add("pollutionRemovalValue", typeof (double));
      this.AllowNullColumn(data, "totalEnergyValue", typeof (double));
      data.Columns.Add("benefitsTotalValue", typeof (double));
      this.AllowNullColumn(data, "xCoordinate", typeof (double));
      this.AllowNullColumn(data, "yCoordinate", typeof (double));
      this.AllowNullColumn(data, "comments", typeof (string));
      this.AllowNullColumn(data, "UID", typeof (string));
      return data;
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      DataTable data = (DataTable) this.GetData();
      List<ColumnFormat> columnFormatList = this.ColumnsFormat(data);
      C1doc.ClipPage = true;
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Width = Unit.Auto;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      renderTable.Style.Font = new Font("Calibri", 9f);
      if (this.curYear.Series.SampleType == SampleType.Inventory)
        renderTable.Cols[0].Visible = false;
      renderTable.Cols[13].Visible = renderTable.Cols[14].Visible = this.pollutionIsAvailable;
      renderTable.Cols[17].Visible = renderTable.Cols[18].Visible = this.hasCoordinates && ReportBase.m_ps.ShowGPS;
      renderTable.Cols[19].Visible = ReportBase.m_ps.ShowComments;
      renderTable.Cols[20].Visible = this.hasUID && ReportBase.m_ps.ShowUID;
      int count = 3;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      ReportUtil.FormatRenderTableHeader(renderTable);
      Style style = ReportUtil.AddAlternateStyle(renderTable);
      renderTable.Rows[0].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontBold = true;
      renderTable.Cells[0, 0].SpanCols = 7;
      renderTable.Cells[0, 7].SpanCols = 10;
      renderTable.Cells[0, 16].SpanCols = 3;
      renderTable.ColGroups[0, 7].SplitBehavior = SplitBehaviorEnum.Never;
      renderTable.ColGroups[7, 10].SplitBehavior = SplitBehaviorEnum.Never;
      renderTable.ColGroups[17, 3].SplitBehavior = SplitBehaviorEnum.Never;
      renderTable.Cells[0, 0].Text = string.Empty;
      renderTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.AnnualBenefits;
      renderTable.Cells[0, 7].Style.Borders.Bottom = this.tableBorderLineGray;
      renderTable.Cols[7].Style.Borders.Left = renderTable.Cols[15].Style.Borders.Right = renderTable.Cols[16].Style.Borders.Right = this.tableBorderLineGray;
      renderTable.Cells[1, 0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[1, 0].Text = i_Tree_Eco_v6.Resources.Strings.PlotID;
      renderTable.Cells[1, 1].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.TreeID;
      renderTable.Cells[1, 2].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[1, 2].Text = i_Tree_Eco_v6.Resources.Strings.SpeciesName;
      renderTable.Cells[1, 3].Text = i_Tree_Eco_v6.Resources.Strings.DBH;
      renderTable.Cells[1, 4].Text = i_Tree_Eco_v6.Resources.Strings.ReplacementValue;
      renderTable.Cells[1, 5].SpanCols = 2;
      renderTable.Cells[1, 5].Text = i_Tree_Eco_v6.Resources.Strings.CarbonStorage;
      renderTable.Cells[1, 7].SpanCols = 2;
      renderTable.Cells[1, 7].Text = i_Tree_Eco_v6.Resources.Strings.GrossCarbonNewLineSequestration;
      renderTable.Cells[1, 9].SpanCols = 2;
      renderTable.Cells[1, 9].Text = i_Tree_Eco_v6.Resources.Strings.AvoidedRunoff;
      renderTable.Cells[1, 11].SpanCols = 2;
      renderTable.Cells[1, 11].Text = i_Tree_Eco_v6.Resources.Strings.CarbonAvoided;
      renderTable.Cells[1, 13].SpanCols = 2;
      renderTable.Cells[1, 13].Text = i_Tree_Eco_v6.Resources.Strings.PollutionRemoval;
      renderTable.Cells[1, 15].Text = i_Tree_Eco_v6.Resources.Strings.EnergyNewLineSavings;
      renderTable.Cells[1, 16].Text = i_Tree_Eco_v6.Resources.Strings.TotalAnnualNewLineBenefits;
      renderTable.Cells[1, 17].Text = i_Tree_Eco_v6.Resources.Strings.xCoordinate;
      renderTable.Cells[1, 18].Text = i_Tree_Eco_v6.Resources.Strings.yCoordinate;
      renderTable.Cells[1, 19].Text = i_Tree_Eco_v6.Resources.Strings.Comments;
      renderTable.Cells[1, 20].Text = i_Tree_Eco_v6.Resources.Strings.UserID;
      renderTable.Cols[17].Style.Borders.Left = renderTable.Cols[19].Style.Borders.Left = renderTable.Cols[20].Style.Borders.Left = this.tableBorderLineGray;
      renderTable.Cols[19].Style.TextAlignHorz = renderTable.Cols[20].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[1, 19].Style.TextAlignHorz = renderTable.Cells[1, 20].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.Cells[2, 4].Text = ReportUtil.FormatHeaderUnitsStr(this.CurrencySymbol);
      renderTable.Cells[2, 3].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.CmUnits());
      renderTable.Cells[2, 5].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.KgUnits());
      renderTable.Cells[2, 6].Text = ReportUtil.FormatHeaderUnitsStr(this.CurrencySymbol);
      renderTable.Cells[2, 7].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.KgUnits());
      renderTable.Cells[2, 8].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
      renderTable.Cells[2, 9].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.CubicMeterUnits());
      renderTable.Cells[2, 10].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
      renderTable.Cells[2, 11].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.KgUnits());
      renderTable.Cells[2, 12].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
      renderTable.Cells[2, 13].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.GUnits());
      renderTable.Cells[2, 14].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
      renderTable.Cells[2, 15].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
      renderTable.Cells[2, 16].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
      int num1 = count;
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        renderTable.Rows[num1].Style.TextAlignHorz = AlignHorzEnum.Right;
        renderTable.Cells[num1, 0].Style.TextAlignHorz = renderTable.Cells[num1, 1].Style.TextAlignHorz = renderTable.Cells[num1, 2].Style.TextAlignHorz = AlignHorzEnum.Left;
        foreach (ColumnFormat columnFormat in columnFormatList)
          renderTable.Cells[num1, columnFormat.ColNum].Text = columnFormat.Format(row[columnFormat.ColName]);
        if ((num1 - count) % 2 == 0)
          renderTable.Rows[num1].Style.Parent = style;
        ++num1;
      }
      renderTable.Rows[num1 + 2].Style.FontBold = true;
      renderTable.Rows[num1 + 2].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Rows[num1 + 2].Style.Borders.Top = LineDef.Default;
      renderTable.Cells[num1 + 2, 2].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[num1 + 2, 2].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      renderTable.Cells[num1 + 2, 4].Text = this._t_replacementValue.ToString("N0");
      renderTable.Cells[num1 + 2, 5].Text = Decimal.Parse(ReportBase.FormatDoubleWeight((object) this._t_carbonStorage)).ToString("N0");
      renderTable.Cells[num1 + 2, 6].Text = this._t_carbonStorageValue.ToString("N0");
      TableCell cell1 = renderTable.Cells[num1 + 2, 7];
      Decimal num2 = Decimal.Parse(ReportBase.FormatDoubleWeight((object) this._t_carbonSequestration));
      string str1 = num2.ToString("N0");
      cell1.Text = str1;
      renderTable.Cells[num1 + 2, 8].Text = this._t_carbonSequestrationValue.ToString("N0");
      TableCell cell2 = renderTable.Cells[num1 + 2, 9];
      num2 = Decimal.Parse(ReportBase.FormatDoubleVolumeCubicMeter1((object) this._t_avoidedRunoff));
      string str2 = num2.ToString("N0");
      cell2.Text = str2;
      renderTable.Cells[num1 + 2, 10].Text = this._t_avoidedRunoffValue.ToString("N0");
      string s1 = ReportBase.FormatEnergyEffectsSingle((object) this._t_carbonAvoided);
      TableCell cell3 = renderTable.Cells[num1 + 2, 11];
      string str3;
      if (!(s1 != i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr))
      {
        str3 = s1;
      }
      else
      {
        num2 = Decimal.Parse(s1);
        str3 = num2.ToString("N0");
      }
      cell3.Text = str3;
      string s2 = ReportBase.FormatEnergyEffectsValue((object) this._t_carbonAvoidedValue);
      TableCell cell4 = renderTable.Cells[num1 + 2, 12];
      string str4;
      if (!(s2 != i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr))
      {
        str4 = s2;
      }
      else
      {
        num2 = Decimal.Parse(s2);
        str4 = num2.ToString("N0");
      }
      cell4.Text = str4;
      TableCell cell5 = renderTable.Cells[num1 + 2, 13];
      num2 = Decimal.Parse(ReportBase.FormatDoubleWeightGrams((object) this._t_polutionRemoved));
      string str5 = num2.ToString("N0");
      cell5.Text = str5;
      renderTable.Cells[num1 + 2, 14].Text = this._t_polutionRemovedValue.ToString("N0");
      string s3 = ReportBase.FormatEnergyEffectsValue((object) this._t_combinedEnergyValue);
      TableCell cell6 = renderTable.Cells[num1 + 2, 15];
      string str6;
      if (!(s3 != i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr))
      {
        str6 = s3;
      }
      else
      {
        num2 = Decimal.Parse(s3);
        str6 = num2.ToString("N0");
      }
      cell6.Text = str6;
      renderTable.Cells[num1 + 2, 16].Text = this._t_totalAnualBenefitsValue.ToString("N0");
      this.Note(C1doc);
    }

    protected override string ReportMessage()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.Carbon_kg_lb_Footer());
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.NoteCarbonStorageLimit);
      if (this.ProjectIsUsingTropicalEquations())
      {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.MsgTropicalEquations);
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(this.AvoidedRunoff_m3_ft3_Footer());
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(this.Energy_MKW_MBTU_Footer());
      if (this.pollutionIsAvailable)
      {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(this.PollutionRemoval_kg_lb_Footer());
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(this.ReplacementValue_Footer());
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.NoteZeroValuesMeaning);
      return stringBuilder.ToString();
    }

    public override List<ColumnFormat> ColumnsFormat(DataTable data)
    {
      List<ColumnFormat> columnFormatList = new List<ColumnFormat>();
      if (ReportBase.plotInventory)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.PlotID,
          ColName = data.Columns[0].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatInt),
          ColNum = 0
        });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.TreeID,
        ColName = data.Columns[1].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatInt),
        ColNum = 1
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.SpeciesName,
        ColName = data.Columns[2].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 2
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.DBH, ReportBase.CmUnits()),
        ColName = data.Columns[3].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleCentimeters),
        ColNum = 3
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.ReplacementValue, this.CurrencySymbol),
        ColName = data.Columns[4].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 4
      });
      string carbonStorage = i_Tree_Eco_v6.Resources.Strings.CarbonStorage;
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(carbonStorage, ReportBase.KgUnits()),
        ColName = data.Columns[5].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeight),
        ColNum = 5
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(carbonStorage, this.CurrencySymbol),
        ColName = data.Columns[6].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 6
      });
      string carbonSequestration = i_Tree_Eco_v6.Resources.Strings.GrossCarbonSequestration;
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(carbonSequestration, ReportUtil.GetValuePerYrStr(ReportBase.KgUnits())),
        ColName = data.Columns[7].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeight),
        ColNum = 7
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(carbonSequestration, ReportUtil.GetValuePerYrStr(this.CurrencySymbol)),
        ColName = data.Columns[8].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 8
      });
      string avoidedRunoff = i_Tree_Eco_v6.Resources.Strings.AvoidedRunoff;
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(avoidedRunoff, ReportUtil.GetValuePerYrStr(ReportBase.CubicMeterUnits())),
        ColName = data.Columns[9].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleVolumeCubicMeter1),
        ColNum = 9
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(avoidedRunoff, ReportUtil.GetValuePerYrStr(this.CurrencySymbol)),
        ColName = data.Columns[10].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 10
      });
      string carbonAvoided = i_Tree_Eco_v6.Resources.Strings.CarbonAvoided;
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(carbonAvoided, ReportUtil.GetValuePerYrStr(ReportBase.KgUnits())),
        ColName = data.Columns[11].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatEnergyEffectsSingle),
        ColNum = 11
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(carbonAvoided, ReportUtil.GetValuePerYrStr(this.CurrencySymbol)),
        ColName = data.Columns[12].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatEnergyEffectsValue),
        ColNum = 12
      });
      string pollutionRemoval = i_Tree_Eco_v6.Resources.Strings.PollutionRemoval;
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(pollutionRemoval, ReportUtil.GetValuePerYrStr(ReportBase.GUnits())),
        ColName = data.Columns[13].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        ColNum = 13
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(pollutionRemoval, ReportUtil.GetValuePerYrStr(this.CurrencySymbol)),
        ColName = data.Columns[14].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 14
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.EnergySavings, ReportUtil.GetValuePerYrStr(this.CurrencySymbol)),
        ColName = data.Columns[15].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatEnergyEffectsValue),
        ColNum = 15
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.TotalAnnualBenefits, ReportUtil.GetValuePerYrStr(this.CurrencySymbol)),
        ColName = data.Columns[16].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 16
      });
      if (this.hasCoordinates && ReportBase.m_ps.ShowGPS)
      {
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.xCoordinate,
          ColName = data.Columns[17].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 17
        });
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.yCoordinate,
          ColName = data.Columns[18].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 18
        });
      }
      if (ReportBase.m_ps.ShowComments)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.Comments,
          ColName = data.Columns[19].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 19
        });
      if (this.hasUID && ReportBase.m_ps.ShowUID)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.UserID,
          ColName = data.Columns[20].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 20
        });
      return columnFormatList;
    }
  }
}
