// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeEnergyAvoidedPollutants
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace i_Tree_Eco_v6.Reports
{
  public class IndividualTreeEnergyAvoidedPollutants : IndividualTreeEnergyBase
  {
    public IndividualTreeEnergyAvoidedPollutants() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleEnergyAvoidedPollutants;

    public override DataTable ReportSpecificColumnsData(DataTable results)
    {
      if (results.Rows.Count <= 0)
        return results;
      SortedDictionary<Tuple<int, int>, IndividualTreeEnergyAvoidedPollutant> energyEffects = new SortedDictionary<Tuple<int, int>, IndividualTreeEnergyAvoidedPollutant>();
      this.CalculateEnergy<IndividualTreeEnergyAvoidedPollutant>(energyEffects);
      foreach (KeyValuePair<Tuple<int, int>, IndividualTreeEnergyAvoidedPollutant> keyValuePair in energyEffects)
      {
        keyValuePair.Value.CarbonMonoxideDollarValue = keyValuePair.Value.CarbonMonoxideLbs / 2204.62 * this.customizedCoDollarsPerTon;
        keyValuePair.Value.NitrogenDioxideDollarValue = keyValuePair.Value.NitrogenDioxideLbs / 2204.62 * this.customizedNO2DollarsPerTon;
        keyValuePair.Value.PM10DollarValue = keyValuePair.Value.PM10Lbs / 2204.62 * this.customizedPM10DollarsPerTon;
        keyValuePair.Value.PM25DollarValue = keyValuePair.Value.PM25Lbs / 2204.62 * this.customizedPM25DollarsPerTon;
        keyValuePair.Value.SulphurDioxideDollarValue = keyValuePair.Value.SulphurDioxideLbs / 2204.62 * this.customizedSO2DollarsPerTon;
        keyValuePair.Value.Total = keyValuePair.Value.CarbonMonoxideDollarValue + keyValuePair.Value.NitrogenDioxideDollarValue + keyValuePair.Value.PM10DollarValue + keyValuePair.Value.PM25DollarValue + keyValuePair.Value.SulphurDioxideDollarValue + keyValuePair.Value.VOCDollarValue;
      }
      IList<Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>> expressionList = (IList<Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>>) new Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>[14]
      {
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => ee.Name),
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.CarbonMonoxideLbs),
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.CarbonMonoxideDollarValue),
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.NitrogenDioxideLbs),
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.NitrogenDioxideDollarValue),
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.SulphurDioxideLbs),
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.SulphurDioxideDollarValue),
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.PM10Lbs),
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.PM10DollarValue),
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.PM25Lbs),
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.PM25DollarValue),
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.VOCLbs),
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.VOCDollarValue),
        (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.Total)
      };
      using (TypeHelper<IndividualTreeEnergyAvoidedPollutant> typeHelper = new TypeHelper<IndividualTreeEnergyAvoidedPollutant>())
      {
        DataColumnCollection columns1 = results.Columns;
        foreach (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>> expression in (IEnumerable<Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>>) expressionList)
        {
          Type nullableType = typeHelper.TypeOf(expression);
          string name = typeHelper.NameOf(expression);
          if (!columns1.Contains(name))
          {
            DataColumnCollection columns2 = results.Columns;
            string columnName = name;
            Type dataType = Nullable.GetUnderlyingType(nullableType);
            if ((object) dataType == null)
              dataType = nullableType;
            DataColumn column = new DataColumn(columnName, dataType);
            columns2.Add(column);
          }
        }
        foreach (DataRow row in (InternalDataCollectionBase) results.Rows)
        {
          Tuple<int, int> key = new Tuple<int, int>((int) row[this.plotId_col], (int) row[this.treeId_col]);
          if (energyEffects.ContainsKey(key))
          {
            IndividualTreeEnergyAvoidedPollutant avoidedPollutant = energyEffects[key];
            foreach (Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>> expression in (IEnumerable<Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>>) expressionList)
            {
              string str = typeHelper.NameOf(expression);
              row[str] = avoidedPollutant[str];
            }
          }
        }
      }
      return results;
    }

    public override int ReportSpecificColumns(ref RenderTable rTable)
    {
      rTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.CarbonMonoxide;
      rTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.NitrogenDioxide;
      rTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.SulfurDioxide;
      rTable.Cells[0, 9].Text = i_Tree_Eco_v6.Resources.Strings.PM10Removed;
      rTable.Cells[0, 11].Text = i_Tree_Eco_v6.Resources.Strings.PM25Removed;
      rTable.Cells[0, 13].Text = i_Tree_Eco_v6.Resources.Strings.VOCSEmitted;
      int num1 = 15;
      rTable.Cells[0, num1].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      int[] numArray = new int[6]{ 3, 5, 7, 9, 11, 13 };
      foreach (int num2 in numArray)
      {
        rTable.Cells[0, num2].Style.TextAlignHorz = AlignHorzEnum.Center;
        rTable.Cols[num2].Style.Borders.Left = this.tableBorderLineGray;
        rTable.Cells[0, num2].SpanCols = 2;
        rTable.Cells[1, num2].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.GUnits());
        rTable.Cells[1, num2 + 1].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
      }
      rTable.Cols[num1].Style.Borders.Left = this.tableBorderLineGray;
      rTable.Cells[1, num1].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
      return num1;
    }

    protected override string ReportMessage() => i_Tree_Eco_v6.Resources.Strings.NoteTreeBuildingEnergyBenefit1 + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.NoteTreesWithNoBuildingInformation1 + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.NoteTreesWithNoBuildingInformation2;

    public override List<ColumnFormat> ColumnsFormat(DataTable data)
    {
      List<ColumnFormat> columnFormatList1 = new List<ColumnFormat>();
      using (TypeHelper<IndividualTreeEnergyAvoidedPollutant> typeHelper = new TypeHelper<IndividualTreeEnergyAvoidedPollutant>())
      {
        if (ReportBase.plotInventory)
          columnFormatList1.Add(new ColumnFormat()
          {
            HeaderText = i_Tree_Eco_v6.Resources.Strings.PlotID,
            ColName = data.Columns[0].ColumnName,
            Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatInt),
            ColNum = 0
          });
        columnFormatList1.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.TreeID,
          ColName = data.Columns[1].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatInt),
          ColNum = 1
        });
        List<ColumnFormat> columnFormatList2 = columnFormatList1;
        ColumnFormat columnFormat1 = new ColumnFormat();
        columnFormat1.HeaderText = i_Tree_Eco_v6.Resources.Strings.SpeciesName;
        columnFormat1.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => ee.Name));
        columnFormat1.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr);
        columnFormat1.ColNum = 2;
        ColumnFormat columnFormat2 = columnFormat1;
        columnFormatList2.Add(columnFormat2);
        List<ColumnFormat> columnFormatList3 = columnFormatList1;
        ColumnFormat columnFormat3 = new ColumnFormat();
        columnFormat3.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.CarbonMonoxide, ReportUtil.GetValuePerYrStr(ReportBase.GUnits()));
        columnFormat3.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.CarbonMonoxideLbs));
        columnFormat3.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleEnglishWeightScaleDownToGrams);
        columnFormat3.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleEnglishWeightScaleDownToGrams);
        columnFormat3.ColNum = 3;
        ColumnFormat columnFormat4 = columnFormat3;
        columnFormatList3.Add(columnFormat4);
        List<ColumnFormat> columnFormatList4 = columnFormatList1;
        ColumnFormat columnFormat5 = new ColumnFormat();
        columnFormat5.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.CarbonMonoxide, ReportUtil.GetValuePerYrStr(this.CurrencySymbol));
        columnFormat5.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.CarbonMonoxideDollarValue));
        columnFormat5.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2);
        columnFormat5.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat5.ColNum = 4;
        ColumnFormat columnFormat6 = columnFormat5;
        columnFormatList4.Add(columnFormat6);
        List<ColumnFormat> columnFormatList5 = columnFormatList1;
        ColumnFormat columnFormat7 = new ColumnFormat();
        columnFormat7.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.NitrogenDioxide, ReportUtil.GetValuePerYrStr(ReportBase.GUnits()));
        columnFormat7.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.NitrogenDioxideLbs));
        columnFormat7.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleEnglishWeightScaleDownToGrams);
        columnFormat7.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleEnglishWeightScaleDownToGrams);
        columnFormat7.ColNum = 5;
        ColumnFormat columnFormat8 = columnFormat7;
        columnFormatList5.Add(columnFormat8);
        List<ColumnFormat> columnFormatList6 = columnFormatList1;
        ColumnFormat columnFormat9 = new ColumnFormat();
        columnFormat9.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.NitrogenDioxide, ReportUtil.GetValuePerYrStr(this.CurrencySymbol));
        columnFormat9.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.NitrogenDioxideDollarValue));
        columnFormat9.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2);
        columnFormat9.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat9.ColNum = 6;
        ColumnFormat columnFormat10 = columnFormat9;
        columnFormatList6.Add(columnFormat10);
        List<ColumnFormat> columnFormatList7 = columnFormatList1;
        ColumnFormat columnFormat11 = new ColumnFormat();
        columnFormat11.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.SulfurDioxide, ReportUtil.GetValuePerYrStr(ReportBase.GUnits()));
        columnFormat11.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.SulphurDioxideLbs));
        columnFormat11.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleEnglishWeightScaleDownToGrams);
        columnFormat11.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleEnglishWeightScaleDownToGrams);
        columnFormat11.ColNum = 7;
        ColumnFormat columnFormat12 = columnFormat11;
        columnFormatList7.Add(columnFormat12);
        List<ColumnFormat> columnFormatList8 = columnFormatList1;
        ColumnFormat columnFormat13 = new ColumnFormat();
        columnFormat13.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.SulfurDioxide, ReportUtil.GetValuePerYrStr(this.CurrencySymbol));
        columnFormat13.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.SulphurDioxideDollarValue));
        columnFormat13.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2);
        columnFormat13.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat13.ColNum = 8;
        ColumnFormat columnFormat14 = columnFormat13;
        columnFormatList8.Add(columnFormat14);
        List<ColumnFormat> columnFormatList9 = columnFormatList1;
        ColumnFormat columnFormat15 = new ColumnFormat();
        columnFormat15.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.PM10Removed, ReportUtil.GetValuePerYrStr(ReportBase.GUnits()));
        columnFormat15.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.PM10Lbs));
        columnFormat15.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleEnglishWeightScaleDownToGrams);
        columnFormat15.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleEnglishWeightScaleDownToGrams);
        columnFormat15.ColNum = 9;
        ColumnFormat columnFormat16 = columnFormat15;
        columnFormatList9.Add(columnFormat16);
        List<ColumnFormat> columnFormatList10 = columnFormatList1;
        ColumnFormat columnFormat17 = new ColumnFormat();
        columnFormat17.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.PM10Removed, ReportUtil.GetValuePerYrStr(this.CurrencySymbol));
        columnFormat17.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.PM10DollarValue));
        columnFormat17.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2);
        columnFormat17.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat17.ColNum = 10;
        ColumnFormat columnFormat18 = columnFormat17;
        columnFormatList10.Add(columnFormat18);
        List<ColumnFormat> columnFormatList11 = columnFormatList1;
        ColumnFormat columnFormat19 = new ColumnFormat();
        columnFormat19.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.PM25Removed, ReportUtil.GetValuePerYrStr(ReportBase.GUnits()));
        columnFormat19.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.PM25Lbs));
        columnFormat19.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleEnglishWeightScaleDownToGrams);
        columnFormat19.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleEnglishWeightScaleDownToGrams);
        columnFormat19.ColNum = 11;
        ColumnFormat columnFormat20 = columnFormat19;
        columnFormatList11.Add(columnFormat20);
        List<ColumnFormat> columnFormatList12 = columnFormatList1;
        ColumnFormat columnFormat21 = new ColumnFormat();
        columnFormat21.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.PM25Removed, ReportUtil.GetValuePerYrStr(this.CurrencySymbol));
        columnFormat21.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.PM25DollarValue));
        columnFormat21.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2);
        columnFormat21.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat21.ColNum = 12;
        ColumnFormat columnFormat22 = columnFormat21;
        columnFormatList12.Add(columnFormat22);
        List<ColumnFormat> columnFormatList13 = columnFormatList1;
        ColumnFormat columnFormat23 = new ColumnFormat();
        columnFormat23.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.VOCSEmitted, ReportUtil.GetValuePerYrStr(ReportBase.GUnits()));
        columnFormat23.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.VOCLbs));
        columnFormat23.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleEnglishWeightScaleDownToGrams);
        columnFormat23.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleEnglishWeightScaleDownToGrams);
        columnFormat23.ColNum = 13;
        ColumnFormat columnFormat24 = columnFormat23;
        columnFormatList13.Add(columnFormat24);
        List<ColumnFormat> columnFormatList14 = columnFormatList1;
        ColumnFormat columnFormat25 = new ColumnFormat();
        columnFormat25.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.VOCSEmitted, ReportUtil.GetValuePerYrStr(this.CurrencySymbol));
        columnFormat25.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.VOCDollarValue));
        columnFormat25.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2);
        columnFormat25.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat25.ColNum = 14;
        ColumnFormat columnFormat26 = columnFormat25;
        columnFormatList14.Add(columnFormat26);
        List<ColumnFormat> columnFormatList15 = columnFormatList1;
        ColumnFormat columnFormat27 = new ColumnFormat();
        columnFormat27.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Total, ReportUtil.GetValuePerYrStr(this.CurrencySymbol));
        columnFormat27.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyAvoidedPollutant, object>>) (ee => (object) ee.Total));
        columnFormat27.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2);
        columnFormat27.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat27.ColNum = 15;
        ColumnFormat columnFormat28 = columnFormat27;
        columnFormatList15.Add(columnFormat28);
      }
      if (this.hasCoordinates && ReportBase.m_ps.ShowGPS)
      {
        columnFormatList1.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.xCoordinate,
          ColName = data.Columns[2].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 16
        });
        columnFormatList1.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.yCoordinate,
          ColName = data.Columns[3].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 17
        });
      }
      if (ReportBase.m_ps.ShowComments)
        columnFormatList1.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.Comments,
          ColName = data.Columns[4].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 18
        });
      if (this.hasUID && ReportBase.m_ps.ShowUID)
        columnFormatList1.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.UserID,
          ColName = data.Columns[5].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 20
        });
      return columnFormatList1;
    }
  }
}
