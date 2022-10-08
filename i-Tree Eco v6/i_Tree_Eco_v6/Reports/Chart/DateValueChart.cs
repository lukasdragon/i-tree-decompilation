// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.Chart.DateValueChart
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.NHibernate;
using Eco.Util;
using NHibernate.Transform;
using System;
using System.Data;

namespace i_Tree_Eco_v6.Reports.Chart
{
  public class DateValueChart
  {
    protected string item1 = string.Empty;
    protected string item2 = string.Empty;
    protected double valueMultiplier1 = 1.0;
    protected double valueMultiplier2 = 1.0;
    protected string category = string.Empty;
    protected string _chartAxisYTitle = string.Empty;
    protected string _chartAxisXTitle = string.Empty;
    protected string _tableXTitle = string.Empty;
    protected string _tableYTitle = string.Empty;
    protected string _tableY2Title = string.Empty;
    protected string _xAxisTitle = string.Empty;
    protected string _yAxisTitle = string.Empty;
    protected string _yAxisTitle2 = string.Empty;
    protected string _mainTitle = string.Empty;
    protected DataTable _dt;
    protected DataTable _dtConverted;
    protected Units curUnit1 = Units.Centimeters;
    protected Units curUnit2 = Units.CubicMeter;
    protected string metricUnits1 = string.Empty;
    protected string metricUnits2 = string.Empty;
    protected string englishUnits1 = string.Empty;
    protected string englishUnits2 = string.Empty;
    private EstimateUtil m_estUtil;
    private ProgramSession m_ps = ProgramSession.GetInstance();

    public virtual DataTable dt
    {
      get
      {
        if (this._dt == null)
        {
          this._dt = this.GetData(this.category, this.item1, this.valueMultiplier1, this.item2, this.valueMultiplier2);
          this._dtConverted = this.ConvertUnits(this._dt);
        }
        return this.m_ps.UseEnglishUnits ? this._dtConverted : this._dt;
      }
      private set => this._dt = value;
    }

    public virtual string XAxisTitle
    {
      get => this._xAxisTitle;
      protected set => this._xAxisTitle = value;
    }

    public virtual string MainTitle
    {
      get => this._mainTitle;
      protected set => this._mainTitle = value;
    }

    public virtual string YAxisTitle
    {
      get => string.Format(this._yAxisTitle + " ({0})", this.m_ps.UseEnglishUnits ? (object) this.englishUnits1 : (object) this.metricUnits1);
      protected set => this._yAxisTitle = value;
    }

    public virtual string YAxisTitle2
    {
      get => string.Format(this._yAxisTitle2 + " ({0})", this.m_ps.UseEnglishUnits ? (object) this.englishUnits2 : (object) this.metricUnits2);
      protected set => this._yAxisTitle2 = value;
    }

    public virtual string TableXTitle
    {
      get => this._tableXTitle;
      protected set => this._tableXTitle = value;
    }

    public virtual string TableYTitle
    {
      get => string.Format(this._tableYTitle + " ({0})", this.m_ps.UseEnglishUnits ? (object) this.englishUnits1 : (object) this.metricUnits1);
      protected set => this._tableYTitle = value;
    }

    public virtual string TableY2Title
    {
      get => string.Format(this._tableY2Title + " ({0})", this.m_ps.UseEnglishUnits ? (object) this.englishUnits2 : (object) this.metricUnits2);
      protected set => this._tableY2Title = value;
    }

    public DateValueChart() => this.m_estUtil = new EstimateUtil(this.m_ps.InputSession, this.m_ps.LocSp);

    private DataTable ConvertUnits(DataTable dt)
    {
      DataTable dataTable = dt.Copy();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        row[1] = (object) EstimateUtil.ConvertToEnglish(ReportUtil.ConvertFromDBVal<double>(row[1]), this.curUnit1, true);
        row[2] = (object) EstimateUtil.ConvertToEnglish(ReportUtil.ConvertFromDBVal<double>(row[2]), this.curUnit2, true);
      }
      return dataTable;
    }

    protected DataTable GetData(
      string plantCategory,
      string column1,
      double multiplier1,
      string column2,
      double multiplier2)
    {
      return this.AddDataTableColumnDataTypes(this.m_estUtil.queryProvider.GetEstimateUtilProvider().GetPollutantByPlantCategory(plantCategory, column1, multiplier1, column2, multiplier2).SetParameter<Guid?>("y", this.m_ps.InputSession.YearKey).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>());
    }

    protected DataTable AddDataTableColumnDataTypes(DataTable dt)
    {
      DataTable dataTable = dt.Clone();
      dataTable.Columns[0].DataType = typeof (DateTime);
      dataTable.Columns[1].DataType = typeof (double);
      dataTable.Columns[2].DataType = typeof (double);
      foreach (DataRow row in (InternalDataCollectionBase) dt.Rows)
        dataTable.ImportRow(row);
      return dataTable;
    }
  }
}
