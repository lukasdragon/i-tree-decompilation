// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.SpeciesAllergyIndexByStratum
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using C1.Win.C1Chart;
using DaveyTree.Core;
using DaveyTree.NHibernate;
using Eco.Domain.Properties;
using Eco.Util;
using Eco.Util.Views;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class SpeciesAllergyIndexByStratum : DatabaseReport
  {
    private IDictionary<Impact, Tuple<short, short>> AllergyClasses;

    public SpeciesAllergyIndexByStratum()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleAllergyIndexOfTreesByStratum;
      this.AllergyClasses = this.GetAllergyClasses();
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      C1doc.PageLayout.PageSettings.Landscape = false;
      DataTable data = (DataTable) this.GetData();
      if (data == null)
      {
        this.DisplayStandardMessage(C1doc, i_Tree_Eco_v6.Resources.Strings.MsgNoData);
      }
      else
      {
        List<ColumnFormat> columnFormatList = this.ColumnsFormat(data);
        RenderTable renderTable = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) renderTable);
        renderTable.Style.Font = new Font("Calibri", 12f);
        renderTable.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
        renderTable.BreakAfter = BreakEnum.None;
        renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
        renderTable.Width = Unit.Auto;
        renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
        int count = 2;
        renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
        ReportUtil.FormatRenderTableHeader(renderTable);
        C1.C1Preview.Style style = ReportUtil.AddAlternateStyle(renderTable);
        renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
        renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
        renderTable.Cells[0, 1].Text = Impact.Low.ToString();
        renderTable.Cells[0, 2].Text = Impact.Medium.ToString();
        renderTable.Cells[0, 3].Text = Impact.High.ToString();
        renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.Unknown;
        renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.AllergyIndex;
        renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.AllergyClass;
        renderTable.Cells[1, 1].Text = renderTable.Cells[1, 2].Text = renderTable.Cells[1, 3].Text = renderTable.Cells[1, 4].Text = ReportUtil.FormatHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.PercentSymbol);
        renderTable.Cols[0].Style.Borders.Right = renderTable.Cols[4].Style.Borders.Right = renderTable.Cols[5].Style.Borders.Right = ReportUtil.GetTableLine();
        int num = count;
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
        {
          foreach (ColumnFormat columnFormat in columnFormatList)
            renderTable.Cells[num, columnFormat.ColNum].Text = columnFormat.Format(row[columnFormat.ColName]);
          if ((num - count) % 2 == 0)
            renderTable.Rows[num].Style.Parent = style;
          ++num;
        }
        ReportUtil.FormatRenderTable(renderTable);
        if (data.Rows.Count <= 1)
          return;
        DataView defaultView = data.DefaultView;
        defaultView.Sort = this.GetColumnNameContainingMax(data);
        DataTable table = defaultView.ToTable();
        renderTable.BreakAfter = BreakEnum.Page;
        this.GenerateCharts(C1doc, g, table);
      }
    }

    private string GetColumnNameContainingMax(DataTable data)
    {
      double? nullable1 = new double?();
      string nameContainingMax = string.Empty;
      foreach (string col1 in this.GetCols())
      {
        string col = col1;
        double num1 = Enumerable.Max(data.AsEnumerable().Select<DataRow, double>((Func<DataRow, double>) (al => al.Field<double>(col))).Distinct<double>().ToList<double>());
        if (nullable1.HasValue)
        {
          double num2 = num1;
          double? nullable2 = nullable1;
          double valueOrDefault = nullable2.GetValueOrDefault();
          if (!(num2 >= valueOrDefault & nullable2.HasValue))
            continue;
        }
        nameContainingMax = col;
        nullable1 = new double?(num1);
      }
      return nameContainingMax;
    }

    private List<string> GetCols()
    {
      List<string> list = this.AllergyClasses.Keys.Select<Impact, string>((Func<Impact, string>) (k => k.ToString())).ToList<string>();
      list.Add(i_Tree_Eco_v6.Resources.Strings.Unknown);
      return list;
    }

    public void GenerateCharts(C1PrintDocument C1doc, Graphics g, DataTable data)
    {
      short num1 = 50;
      bool flag = data.Rows.Count > (int) num1;
      int count = flag ? (int) num1 : data.Rows.Count;
      DataTable dataTable = data.AsEnumerable().Take<DataRow>(count).CopyToDataTable<DataRow>();
      C1.Win.C1Chart.C1Chart c1Chart = new C1.Win.C1Chart.C1Chart();
      c1Chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      ReportUtil.SetChartOptions(c1Chart, true);
      ReportUtil.SetBarChartStyle(c1Chart);
      c1Chart.ChartGroups[0].Stacked = true;
      c1Chart.ChartArea.Inverted = true;
      c1Chart.Legend.Visible = true;
      c1Chart.Legend.Compass = CompassEnum.North;
      c1Chart.ChartArea.AxisY.Min = 0.0;
      c1Chart.ChartArea.AxisY.Max = 100.0;
      c1Chart.Header.Visible = true;
      c1Chart.Header.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.AllergyClassDistributionbyStratum, flag ? (object) string.Format(i_Tree_Eco_v6.Resources.Strings.TopNFormat, (object) count) : (object) string.Empty);
      c1Chart.ChartArea.AxisY.Text = i_Tree_Eco_v6.Resources.Strings.Percent;
      c1Chart.ChartArea.AxisX.AnnotationRotation = 0;
      int num2 = 0;
      foreach (string col in this.GetCols())
      {
        ChartDataSeries chartDataSeries = c1Chart.ChartGroups[0].ChartData.SeriesList.AddNewSeries();
        chartDataSeries.Label = col;
        chartDataSeries.LineStyle.Color = ReportUtil.GetColor(num2++);
        List<double> doubleList = new List<double>();
        List<string> stringList = new List<string>();
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          doubleList.Add((double) row[dataTable.Columns.IndexOf(col)]);
          stringList.Add(row[0].ToString());
        }
        chartDataSeries.Y.CopyDataIn((object) doubleList.ToArray());
        chartDataSeries.X.CopyDataIn((object) stringList.ToArray());
      }
      RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart, g, C1doc, 0.9, 0.9);
      C1doc.Body.Children.Add(chartRenderObject);
    }

    public override object GetData()
    {
      Dictionary<string, SpeciesView> species = ReportBase.m_ps.Species;
      IDictionary<int, double> speciesAlleryIndexes = this.GetSpeciesAlleryIndexes();
      Dictionary<short, Tuple<string, string, string>> speciesValues = this.estUtil.SpeciesValues;
      Dictionary<short, Dictionary<short, double>> landuseLeafAreas = this.GetLanduseLeafAreas();
      DataTable dataTable = new DataTable();
      string columnName1 = "Stratum";
      dataTable.Columns.Add(columnName1, typeof (string));
      foreach (KeyValuePair<Impact, Tuple<short, short>> allergyClass in (IEnumerable<KeyValuePair<Impact, Tuple<short, short>>>) this.AllergyClasses)
        dataTable.Columns.Add(allergyClass.Key.ToString(), typeof (double));
      string unknown = i_Tree_Eco_v6.Resources.Strings.Unknown;
      dataTable.Columns.Add(unknown, typeof (double));
      string columnName2 = "AllergyIndex";
      dataTable.Columns.Add(columnName2, typeof (double));
      string columnName3 = "AllergyClass";
      dataTable.Columns.Add(columnName3, typeof (string));
      foreach (KeyValuePair<short, Dictionary<short, double>> keyValuePair1 in landuseLeafAreas)
      {
        DataRow dataRow = dataTable.Rows.Add();
        foreach (KeyValuePair<Impact, Tuple<short, short>> allergyClass in (IEnumerable<KeyValuePair<Impact, Tuple<short, short>>>) this.AllergyClasses)
          dataRow[allergyClass.Key.ToString()] = (object) 0;
        dataRow[unknown] = (object) 0;
        double num1 = 0.0;
        double num2 = 0.0;
        foreach (KeyValuePair<short, double> keyValuePair2 in keyValuePair1.Value)
        {
          string key = speciesValues[keyValuePair2.Key].Item3;
          if (species.ContainsKey(key))
          {
            double num3 = keyValuePair2.Value;
            num2 += num3;
            if (speciesAlleryIndexes.ContainsKey(species[key].Id))
            {
              int id = species[key].Id;
              double num4 = speciesAlleryIndexes[id];
              num1 += num4 * num3;
              string allergyClassByValue = this.GetAllergyClassByValue(Convert.ToInt16(num4));
              double num5 = (double) dataRow[allergyClassByValue];
              dataRow[allergyClassByValue] = (object) (num5 + num3);
            }
            else
            {
              double num6 = (double) dataRow[unknown];
              dataRow[unknown] = (object) (num6 + num3);
            }
          }
        }
        dataRow[columnName1] = this.curYear.RecordStrata || landuseLeafAreas.Count != 1 ? (object) this.estUtil.ClassValues[Classifiers.Strata][keyValuePair1.Key].Item1 : (object) i_Tree_Eco_v6.Resources.Strings.StudyArea;
        dataRow[columnName2] = (object) (num1 / num2);
        foreach (KeyValuePair<Impact, Tuple<short, short>> allergyClass in (IEnumerable<KeyValuePair<Impact, Tuple<short, short>>>) this.AllergyClasses)
        {
          string columnName4 = allergyClass.Key.ToString();
          dataRow[columnName4] = (object) UnitsHelper.ToPercent((double) dataRow[columnName4] / num2);
        }
        dataRow[unknown] = (object) UnitsHelper.ToPercent((double) dataRow[unknown] / num2);
        dataRow[columnName3] = Convert.ToInt16(dataRow[columnName2]) != (short) 0 ? (object) EnumHelper.GetDescription<Impact>((Impact) Enum.Parse(typeof (Impact), this.GetAllergyClassByValue(Convert.ToInt16(dataRow[columnName2])))) : (object) "N/A";
      }
      DataView defaultView = dataTable.DefaultView;
      defaultView.Sort = string.Format("{0} ASC", (object) columnName2);
      return (object) defaultView.ToTable();
    }

    private Dictionary<short, Dictionary<short, double>> GetLanduseLeafAreas() => this.CreatePivotTable((IDataReader) new DataTableReader(this.estUtil.queryProvider.GetEstimateUtilProvider().GetLanduseLeafAreas(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Strata], this.estUtil.ClassifierNames[Classifiers.Species]).SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.LeafArea]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squaremeter, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("strataStudyArea", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetParameter<short>("speciesTotal", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>()));

    private Dictionary<short, Dictionary<short, double>> CreatePivotTable(
      IDataReader reader)
    {
      int ordinal1 = reader.GetOrdinal(this.estUtil.ClassifierNames[Classifiers.Strata]);
      int ordinal2 = reader.GetOrdinal(this.estUtil.ClassifierNames[Classifiers.Species]);
      int ordinal3 = reader.GetOrdinal("EstimateValue");
      Dictionary<short, Dictionary<short, double>> pivotTable = new Dictionary<short, Dictionary<short, double>>();
      while (reader.Read())
      {
        short int16_1 = Convert.ToInt16(ReportUtil.ConvertFromDBVal<int>(reader[ordinal1]));
        short int16_2 = Convert.ToInt16(ReportUtil.ConvertFromDBVal<int>(reader[ordinal2]));
        if (!pivotTable.ContainsKey(int16_1))
          pivotTable.Add(int16_1, new Dictionary<short, double>());
        pivotTable[int16_1].Add(int16_2, ReportUtil.ConvertFromDBVal<double>(reader[ordinal3]));
      }
      return pivotTable;
    }

    public IDictionary<Impact, Tuple<short, short>> GetAllergyClasses() => (IDictionary<Impact, Tuple<short, short>>) RetryExecutionHandler.Execute<Dictionary<Impact, Tuple<short, short>>>((Func<Dictionary<Impact, Tuple<short, short>>>) (() =>
    {
      using (ISession session = ReportBase.m_ps.LocSp.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          List<AllergyClass> list = session.Query<AllergyClass>().WithOptions<AllergyClass>((System.Action<NhQueryableOptions>) (o => o.SetCacheable(true))).Distinct<AllergyClass>().ToList<AllergyClass>();
          transaction.Commit();
          return list.ToDictionary<AllergyClass, Impact, Tuple<short, short>>((Func<AllergyClass, Impact>) (d => (Impact) Enum.Parse(typeof (Impact), d.SeverityClass)), (Func<AllergyClass, Tuple<short, short>>) (d => new Tuple<short, short>(d.AlleryPotentialMin, d.AllergyPotentialMax)));
        }
      }
    }));

    public string GetAllergyClassByValue(short val) => this.AllergyClasses.Where<KeyValuePair<Impact, Tuple<short, short>>>((Func<KeyValuePair<Impact, Tuple<short, short>>, bool>) (d => (int) d.Value.Item1 <= (int) val && (int) d.Value.Item2 >= (int) val)).Select<KeyValuePair<Impact, Tuple<short, short>>, string>((Func<KeyValuePair<Impact, Tuple<short, short>>, string>) (d => d.Key.ToString())).FirstOrDefault<string>();

    public IDictionary<int, double> GetSpeciesAlleryIndexes() => (IDictionary<int, double>) RetryExecutionHandler.Execute<Dictionary<int, double>>((Func<Dictionary<int, double>>) (() =>
    {
      using (ISession session = ReportBase.m_ps.LocSp.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          List<SpeciesAllergyIndex> list = session.Query<SpeciesAllergyIndex>().WithOptions<SpeciesAllergyIndex>((System.Action<NhQueryableOptions>) (o => o.SetCacheable(true))).Distinct<SpeciesAllergyIndex>().ToList<SpeciesAllergyIndex>();
          transaction.Commit();
          return list.ToDictionary<SpeciesAllergyIndex, int, double>((Func<SpeciesAllergyIndex, int>) (d => Convert.ToInt32(d.Id)), (Func<SpeciesAllergyIndex, double>) (d => Convert.ToDouble(d.AllergyPotential)));
        }
      }
    }));

    public override List<ColumnFormat> ColumnsFormat(DataTable data)
    {
      List<ColumnFormat> columnFormatList = new List<ColumnFormat>();
      int index = 0;
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = v6Strings.Strata_SingularName,
        ColName = data.Columns[index].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = index
      });
      foreach (Impact key in (IEnumerable<Impact>) this.AllergyClasses.Keys)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = ReportUtil.FormatInlineCSVHeaderStr(new string[2]
          {
            key.ToString(),
            i_Tree_Eco_v6.Resources.Strings.PercentSymbol
          }),
          ColName = data.Columns[++index].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
          ColNum = index
        });
      int num1;
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderStr(new string[2]
        {
          i_Tree_Eco_v6.Resources.Strings.Unknown,
          i_Tree_Eco_v6.Resources.Strings.PercentSymbol
        }),
        ColName = data.Columns[num1 = index + 1].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = num1
      });
      int num2;
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.AllergyIndex,
        ColName = data.Columns[num2 = num1 + 1].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = num2
      });
      int num3;
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.AllergyClass,
        ColName = data.Columns[num3 = num2 + 1].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = num3
      });
      return columnFormatList;
    }
  }
}
