// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.Chart.VOCDateValueChart
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
  public class VOCDateValueChart
  {
    private EstimateUtil m_estUtil;
    private ProgramSession m_ps;
    protected string _tableYTitle = string.Empty;
    protected string _yAxisTitle = string.Empty;
    protected DataTable _dt;
    protected DataTable _dtConverted;
    protected string metricUnits = string.Empty;
    protected string englishUnits = string.Empty;

    protected string Item { get; private set; }

    protected string Category { get; private set; }

    protected Units Units { get; private set; }

    public virtual string MainTitle { get; protected set; }

    public virtual string XAxisTitle { get; protected set; }

    public virtual string TableXTitle { get; protected set; }

    public string HelpTopic { get; protected set; }

    public VOCDateValueChart(string item, string category, Units units)
    {
      this.Item = item;
      this.Category = category;
      this.Units = units;
      this.m_ps = ProgramSession.GetInstance();
      this.m_estUtil = new EstimateUtil(this.m_ps.InputSession, this.m_ps.LocSp);
      this.HelpTopic = this.GetType().Name;
    }

    public virtual DataTable dt
    {
      get
      {
        if (this._dt == null)
        {
          this._dt = this.GetData(this.Category, this.Item);
          this._dtConverted = this.ConvertUnits(this._dt);
        }
        return !this.m_ps.UseEnglishUnits ? this._dt : this._dtConverted;
      }
      private set => this._dt = value;
    }

    public virtual string YAxisTitle
    {
      get => string.Format(this._yAxisTitle + " ({0})", this.m_ps.UseEnglishUnits ? (object) this.englishUnits : (object) this.metricUnits);
      protected set => this._yAxisTitle = value;
    }

    public virtual string TableYTitle
    {
      get => string.Format(this._tableYTitle + " ({0})", this.m_ps.UseEnglishUnits ? (object) this.englishUnits : (object) this.metricUnits);
      protected set => this._tableYTitle = value;
    }

    private DataTable ConvertUnits(DataTable dt)
    {
      DataTable dataTable = dt.Copy();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        row[1] = (object) EstimateUtil.ConvertToEnglish(ReportUtil.ConvertFromDBVal<double>(row[1]), this.Units, true);
      return dataTable;
    }

    protected DataTable GetData(string plantCategory, string column) => this.AddDataTableColumnDataTypes(this.m_estUtil.queryProvider.GetEstimateUtilProvider().GetHourlyUFOREBResults(column).SetParameter<Guid?>("y", this.m_ps.InputSession.YearKey).SetParameter<string>("category", plantCategory).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>());

    protected DataTable AddDataTableColumnDataTypes(DataTable dt)
    {
      DataTable dataTable = dt.Clone();
      dataTable.Columns[0].DataType = typeof (DateTime);
      dataTable.Columns[1].DataType = typeof (double);
      foreach (DataRow row in (InternalDataCollectionBase) dt.Rows)
        dataTable.ImportRow(row);
      return dataTable;
    }
  }
}
