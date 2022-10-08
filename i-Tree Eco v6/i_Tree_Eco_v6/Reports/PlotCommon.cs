// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PlotCommon
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.Properties;
using i_Tree_Eco_v6.Resources;
using System.Collections.Generic;
using System.Data;

namespace i_Tree_Eco_v6.Reports
{
  public class PlotCommon : CommentsBase
  {
    public PlotCommon()
    {
      this.hasCoordinates = this.curYear.RecordGPS;
      this.hasComments = true;
    }

    public override List<ColumnFormat> ColumnsFormat(DataTable data)
    {
      List<ColumnFormat> columnFormatList = new List<ColumnFormat>();
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = Strings.PlotID,
        ColName = data.Columns[0].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatInt),
        ColNum = 0
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = v6Strings.Strata_SingularName,
        ColName = data.Columns[1].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 1
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = Strings.Address,
        ColName = data.Columns[2].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 2
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = Strings.Date,
        ColName = data.Columns[3].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDate),
        ColNum = 3
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = Strings.Crew,
        ColName = data.Columns[4].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 4
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = Strings.Complete,
        ColName = data.Columns[5].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatBool),
        ColNum = 5
      });
      if (this.hasCoordinates && ReportBase.m_ps.ShowGPS)
      {
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = Strings.xCoordinate,
          ColName = data.Columns[6].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 6
        });
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = Strings.yCoordinate,
          ColName = data.Columns[7].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 7
        });
      }
      if (this.hasComments && this.ShowComments)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = Strings.Comments,
          ColName = data.Columns[8].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 8
        });
      return columnFormatList;
    }
  }
}
