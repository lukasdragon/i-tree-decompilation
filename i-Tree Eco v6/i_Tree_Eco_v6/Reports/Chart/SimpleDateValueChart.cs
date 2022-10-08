// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.Chart.SimpleDateValueChart
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
  public class SimpleDateValueChart
  {
    protected string item1 = string.Empty;
    protected string _chartAxisYTitle = string.Empty;
    protected string _chartAxisXTitle = string.Empty;
    protected string _tableXTitle = string.Empty;
    protected string _tableYTitle = string.Empty;
    protected string _xAxisTitle = string.Empty;
    protected string _yAxisTitle = string.Empty;
    protected string _mainTitle = string.Empty;
    protected DataTable _dt;
    private EstimateUtil m_estUtil;
    private ProgramSession m_ps = ProgramSession.GetInstance();

    public virtual DataTable dt
    {
      get => this._dt ?? (this._dt = this.GetData(this.item1));
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
      get => this._yAxisTitle;
      protected set => this._yAxisTitle = value;
    }

    public virtual string TableXTitle
    {
      get => this._tableXTitle;
      protected set => this._tableXTitle = value;
    }

    public virtual string TableYTitle
    {
      get => this._tableYTitle;
      protected set => this._tableYTitle = value;
    }

    public SimpleDateValueChart() => this.m_estUtil = new EstimateUtil(this.m_ps.InputSession, this.m_ps.LocSp);

    protected DataTable GetData(string column1) => this.AddDataTableColumnDataTypes(this.m_estUtil.queryProvider.GetEstimateUtilProvider().GetUVIndexReduction(column1).SetParameter<Guid?>("y", this.m_ps.InputSession.YearKey).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>());

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
