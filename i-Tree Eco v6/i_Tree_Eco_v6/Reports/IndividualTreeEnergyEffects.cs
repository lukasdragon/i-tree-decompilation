// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeEnergyEffects
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
  public class IndividualTreeEnergyEffects : IndividualTreeEnergyBase
  {
    public IndividualTreeEnergyEffects() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleEnergyEffectsOfMeasuredTrees;

    public override DataTable ReportSpecificColumnsData(DataTable results)
    {
      if (results.Rows.Count <= 0)
        return results;
      SortedDictionary<Tuple<int, int>, IndividualTreeEnergyEffect> energyEffects = new SortedDictionary<Tuple<int, int>, IndividualTreeEnergyEffect>();
      this.CalculateEnergy<IndividualTreeEnergyEffect>(energyEffects);
      IList<Expression<Func<IndividualTreeEnergyEffect, object>>> expressionList = (IList<Expression<Func<IndividualTreeEnergyEffect, object>>>) new Expression<Func<IndividualTreeEnergyEffect, object>>[10]
      {
        (Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => ee.Name),
        (Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.CarbonAvoidedKgPerYear),
        (Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.CarbonAvoidedValuePerYear),
        (Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.FuelsAvoidedHeating),
        (Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.HeatingMBTUDollars),
        (Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.ElectricityAvoidedHeating),
        (Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.HeatingKwhDollars),
        (Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.ElectricityAvoidedCooling),
        (Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.CoolingKwhDollars),
        (Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.Total)
      };
      using (TypeHelper<IndividualTreeEnergyEffect> typeHelper = new TypeHelper<IndividualTreeEnergyEffect>())
      {
        DataColumnCollection columns1 = results.Columns;
        foreach (Expression<Func<IndividualTreeEnergyEffect, object>> expression in (IEnumerable<Expression<Func<IndividualTreeEnergyEffect, object>>>) expressionList)
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
            IndividualTreeEnergyEffect treeEnergyEffect = energyEffects[key];
            foreach (Expression<Func<IndividualTreeEnergyEffect, object>> expression in (IEnumerable<Expression<Func<IndividualTreeEnergyEffect, object>>>) expressionList)
            {
              string str = typeHelper.NameOf(expression);
              row[str] = treeEnergyEffect[str];
            }
          }
        }
      }
      return results;
    }

    public override int ReportSpecificColumns(ref RenderTable rTable)
    {
      rTable.Cols[14].Style.TextAlignHorz = rTable.Cols[15].Style.TextAlignHorz = AlignHorzEnum.Left;
      rTable.Cols[3].Style.Borders.Left = rTable.Cols[5].Style.Borders.Left = rTable.Cols[9].Style.Borders.Left = rTable.Cols[11].Style.Borders.Left = rTable.Cols[12].Style.Borders.Left = rTable.Cols[14].Style.Borders.Left = rTable.Cols[15].Style.Borders.Left = this.tableBorderLineGray;
      rTable.Cells[0, 3].Text = string.Format("{0}*", (object) i_Tree_Eco_v6.Resources.Strings.CarbonAvoided);
      rTable.Cells[0, 3].SpanCols = 2;
      rTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.Heating;
      rTable.Cells[0, 5].SpanCols = 4;
      rTable.Cells[0, 3].Style.TextAlignHorz = rTable.Cells[0, 5].Style.TextAlignHorz = rTable.Cells[0, 9].Style.TextAlignHorz = AlignHorzEnum.Center;
      rTable.Cells[0, 9].Text = i_Tree_Eco_v6.Resources.Strings.Cooling;
      rTable.Cells[0, 9].SpanCols = 2;
      int col = 11;
      rTable.Cells[0, col].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      rTable.Cells[1, 3].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.KgUnits());
      rTable.Cells[1, 4].Text = rTable.Cells[1, 6].Text = rTable.Cells[1, 8].Text = rTable.Cells[1, 10].Text = rTable.Cells[1, col].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
      rTable.Cells[1, 5].Text = ReportUtil.GetFormattedValuePerYrStr(i_Tree_Eco_v6.Resources.Strings.UnitMBTU);
      rTable.Cells[1, 7].Text = rTable.Cells[1, 9].Text = ReportUtil.GetFormattedValuePerYrStr(i_Tree_Eco_v6.Resources.Strings.KWH);
      return col;
    }

    protected override string ReportMessage() => string.Format(i_Tree_Eco_v6.Resources.Strings.NoteElectricityAndMBTUValues, (object) this.CurrencySymbol, (object) ReportBase.FormatDoubleValue2((object) this.customizedElectricityDollarsPerKwh), (object) ReportBase.FormatDoubleValue2((object) (this.customizedHeatingDollarsPerTherm * 10.0023877))) + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.NoteTreeBuildingEnergyBenefit1 + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.NoteTreesWithNoBuildingInformation1 + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.NoteTreesWithNoBuildingInformation2 + Environment.NewLine + string.Format("* {0}", (object) i_Tree_Eco_v6.Resources.Strings.NoteAvoidedCarbonMonetaryValuesNotIncludedInTotals);

    public override List<ColumnFormat> ColumnsFormat(DataTable data)
    {
      List<ColumnFormat> columnFormatList1 = new List<ColumnFormat>();
      using (TypeHelper<IndividualTreeEnergyEffect> typeHelper = new TypeHelper<IndividualTreeEnergyEffect>())
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
        columnFormat1.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => ee.Name));
        columnFormat1.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr);
        columnFormat1.ColNum = 2;
        ColumnFormat columnFormat2 = columnFormat1;
        columnFormatList2.Add(columnFormat2);
        List<ColumnFormat> columnFormatList3 = columnFormatList1;
        ColumnFormat columnFormat3 = new ColumnFormat();
        columnFormat3.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.CarbonAvoided, ReportUtil.GetValuePerYrStr(ReportBase.KgUnits()));
        columnFormat3.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.CarbonAvoidedKgPerYear));
        columnFormat3.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeight3);
        columnFormat3.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeight0);
        columnFormat3.ColNum = 3;
        ColumnFormat columnFormat4 = columnFormat3;
        columnFormatList3.Add(columnFormat4);
        List<ColumnFormat> columnFormatList4 = columnFormatList1;
        ColumnFormat columnFormat5 = new ColumnFormat();
        columnFormat5.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.CarbonAvoided, ReportUtil.GetValuePerYrStr(this.CurrencySymbol));
        columnFormat5.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.CarbonAvoidedValuePerYear));
        columnFormat5.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2);
        columnFormat5.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat5.ColNum = 4;
        ColumnFormat columnFormat6 = columnFormat5;
        columnFormatList4.Add(columnFormat6);
        List<ColumnFormat> columnFormatList5 = columnFormatList1;
        ColumnFormat columnFormat7 = new ColumnFormat();
        columnFormat7.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Heating, ReportUtil.GetValuePerYrStr(i_Tree_Eco_v6.Resources.Strings.UnitMBTU));
        columnFormat7.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.FuelsAvoidedHeating));
        columnFormat7.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue3);
        columnFormat7.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat7.ColNum = 5;
        ColumnFormat columnFormat8 = columnFormat7;
        columnFormatList5.Add(columnFormat8);
        List<ColumnFormat> columnFormatList6 = columnFormatList1;
        ColumnFormat columnFormat9 = new ColumnFormat();
        columnFormat9.HeaderText = ReportUtil.FormatInlineCSVHeaderStr(new string[3]
        {
          i_Tree_Eco_v6.Resources.Strings.Heating,
          ReportUtil.FormatHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.UnitMBTU),
          ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol)
        });
        columnFormat9.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.HeatingMBTUDollars));
        columnFormat9.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2);
        columnFormat9.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat9.ColNum = 6;
        ColumnFormat columnFormat10 = columnFormat9;
        columnFormatList6.Add(columnFormat10);
        List<ColumnFormat> columnFormatList7 = columnFormatList1;
        ColumnFormat columnFormat11 = new ColumnFormat();
        columnFormat11.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Heating, ReportUtil.GetValuePerYrStr(i_Tree_Eco_v6.Resources.Strings.KWH));
        columnFormat11.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.ElectricityAvoidedHeating));
        columnFormat11.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue3);
        columnFormat11.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat11.ColNum = 7;
        ColumnFormat columnFormat12 = columnFormat11;
        columnFormatList7.Add(columnFormat12);
        List<ColumnFormat> columnFormatList8 = columnFormatList1;
        ColumnFormat columnFormat13 = new ColumnFormat();
        columnFormat13.HeaderText = ReportUtil.FormatInlineCSVHeaderStr(new string[3]
        {
          i_Tree_Eco_v6.Resources.Strings.Heating,
          ReportUtil.FormatHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.KWH),
          ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol)
        });
        columnFormat13.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.HeatingKwhDollars));
        columnFormat13.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2);
        columnFormat13.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat13.ColNum = 8;
        ColumnFormat columnFormat14 = columnFormat13;
        columnFormatList8.Add(columnFormat14);
        List<ColumnFormat> columnFormatList9 = columnFormatList1;
        ColumnFormat columnFormat15 = new ColumnFormat();
        columnFormat15.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Cooling, ReportUtil.GetValuePerYrStr(i_Tree_Eco_v6.Resources.Strings.KWH));
        columnFormat15.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.ElectricityAvoidedCooling));
        columnFormat15.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue3);
        columnFormat15.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat15.ColNum = 9;
        ColumnFormat columnFormat16 = columnFormat15;
        columnFormatList9.Add(columnFormat16);
        List<ColumnFormat> columnFormatList10 = columnFormatList1;
        ColumnFormat columnFormat17 = new ColumnFormat();
        columnFormat17.HeaderText = ReportUtil.FormatInlineCSVHeaderStr(new string[3]
        {
          i_Tree_Eco_v6.Resources.Strings.Cooling,
          ReportUtil.FormatHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.KWH),
          ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol)
        });
        columnFormat17.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.CoolingKwhDollars));
        columnFormat17.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2);
        columnFormat17.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat17.ColNum = 10;
        ColumnFormat columnFormat18 = columnFormat17;
        columnFormatList10.Add(columnFormat18);
        List<ColumnFormat> columnFormatList11 = columnFormatList1;
        ColumnFormat columnFormat19 = new ColumnFormat();
        columnFormat19.HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Total, ReportUtil.GetValuePerYrStr(this.CurrencySymbol));
        columnFormat19.ColName = typeHelper.NameOf((Expression<Func<IndividualTreeEnergyEffect, object>>) (ee => (object) ee.Total));
        columnFormat19.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2);
        columnFormat19.FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0);
        columnFormat19.ColNum = 11;
        ColumnFormat columnFormat20 = columnFormat19;
        columnFormatList11.Add(columnFormat20);
      }
      if (this.hasCoordinates && ReportBase.m_ps.ShowGPS)
      {
        columnFormatList1.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.xCoordinate,
          ColName = data.Columns[2].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 12
        });
        columnFormatList1.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.yCoordinate,
          ColName = data.Columns[3].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 13
        });
      }
      if (ReportBase.m_ps.ShowComments)
        columnFormatList1.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.Comments,
          ColName = data.Columns[4].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 14
        });
      if (this.hasUID && ReportBase.m_ps.ShowUID)
        columnFormatList1.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.UserID,
          ColName = data.Columns[5].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 15
        });
      return columnFormatList1;
    }
  }
}
