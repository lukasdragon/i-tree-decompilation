// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeWoodProducts
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Domain.v6;
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
using System.Linq.Expressions;

namespace i_Tree_Eco_v6.Reports
{
  public class IndividualTreeWoodProducts : DatabaseReport
  {
    private const double minSoftwoodDBH = 23.0;
    private const double minHardwoodDBH = 28.0;
    private const double biomassToBoardFeetConstant = 176.0;
    private const double gymnosperms = 0.48;
    private const double angiosperms = 0.56;
    private const double boardFeetValuePerBF = 0.105;
    private const double firewoodValuePerCoord = 25.0;
    private const double palletsValuePerBF = 0.075;
    private const double woodChipeValuePerFreshWeightBiomassTonne = 1.79;

    public IndividualTreeWoodProducts()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleWoodProductsOfIndividualTrees;
      this.hasComments = true;
      this.hasCoordinates = this.curYear.RecordTreeGPS;
      this.hasUID = this.curYear.RecordTreeUserId;
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      DataTable data = (DataTable) this.GetData();
      if (data == null)
      {
        this.DisplayStandardMessage(C1doc, i_Tree_Eco_v6.Resources.Strings.MsgNoData);
      }
      else
      {
        RenderTable renderTable = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) renderTable);
        renderTable.Style.Font = new Font("Calibri", 12f);
        renderTable.Style.TextAlignHorz = AlignHorzEnum.Right;
        renderTable.BreakAfter = BreakEnum.None;
        renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
        renderTable.Width = Unit.Auto;
        renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
        int count = 2;
        renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
        ReportUtil.FormatRenderTableHeader(renderTable);
        Style style = ReportUtil.AddAlternateStyle(renderTable);
        renderTable.Cols[0].Style.TextAlignHorz = renderTable.Cols[1].Style.TextAlignHorz = renderTable.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Left;
        renderTable.Cols[4].Style.Borders.Left = renderTable.Cols[5].Style.Borders.Left = renderTable.Cols[6].Style.Borders.Left = renderTable.Cols[7].Style.Borders.Left = this.tableBorderLineGray;
        renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.PlotID;
        renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.TreeID;
        renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.Species;
        renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.LumberWood;
        renderTable.Cells[1, 3].Text = ReportUtil.FormatHeaderUnitsStr(this.CurrencySymbol);
        renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.Firewood;
        renderTable.Cells[1, 4].Text = ReportUtil.FormatHeaderUnitsStr(this.CurrencySymbol);
        renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.Pallets;
        renderTable.Cells[1, 5].Text = ReportUtil.FormatHeaderUnitsStr(this.CurrencySymbol);
        renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.WoodChips;
        renderTable.Cells[1, 6].Text = ReportUtil.FormatHeaderUnitsStr(this.CurrencySymbol);
        renderTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.xCoordinate;
        renderTable.Cells[0, 8].Text = i_Tree_Eco_v6.Resources.Strings.yCoordinate;
        renderTable.Cells[0, 9].Text = i_Tree_Eco_v6.Resources.Strings.Comments;
        renderTable.Cells[0, 10].Text = i_Tree_Eco_v6.Resources.Strings.UserID;
        renderTable.Cols[0].Visible = ReportBase.plotInventory;
        renderTable.Cols[7].Visible = renderTable.Cols[8].Visible = this.curYear.RecordTreeGPS && ReportBase.m_ps.ShowGPS;
        renderTable.Cols[9].Visible = ReportBase.m_ps.ShowComments;
        renderTable.Cols[10].Visible = this.hasUID && ReportBase.m_ps.ShowUID;
        List<ColumnFormat> columnFormatList = this.ColumnsFormat(data);
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
      }
    }

    public override object GetData() => (object) this.CalculateAddtionalData(this.curInputISession.GetNamedQuery("IndividualTreeLumberEstimates").SetParameter<Guid>("y", this.YearGuid).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>());

    private DataTable CalculateAddtionalData(DataTable data)
    {
      IList<Tree> treeList = this.GetTreeList();
      Dictionary<string, SpeciesView> species1 = ReportBase.m_ps.Species;
      IDictionary<int, Species> treeSpecies = this.GetTreeSpecies();
      string columnName1 = "MerchantableWoodValue";
      data.Columns.Add(columnName1, typeof (double));
      string columnName2 = "FirewoodValue";
      data.Columns.Add(columnName2, typeof (double));
      string columnName3 = "PalletsValue";
      data.Columns.Add(columnName3, typeof (double));
      string columnName4 = "WoodChipsValue";
      data.Columns.Add(columnName4, typeof (double));
      IList<DBHMortalityClass> mortalityClasses = this.GetDBHMortalityClasses();
      foreach (Tree tree in (IEnumerable<Tree>) treeList)
      {
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
        {
          if (tree.Plot.Id == row.Field<int>("PlotId") && row.Field<int>("TreeId") == tree.Id)
          {
            SpeciesView speciesView = (SpeciesView) null;
            if (species1.TryGetValue(tree.Species, out speciesView))
            {
              Species species2 = (Species) null;
              if (treeSpecies.TryGetValue(speciesView.Id, out species2))
              {
                double freshWeightBiomassTonne = this.AboveGroundBiomassToFreshWeightBiomass(UnitsHelper.BiomassToAboveGroundBiomass(UnitsHelper.CarbonStorageToBiomass(ReportUtil.ConvertFromDBVal<double>(row["CarbonStorage"]))), species2) / 1000.0;
                double dbh = ReportUtil.ConvertFromDBVal<double>(row["DBH"]);
                PercentLeafType? percentLeafType1 = species2.PercentLeafType;
                PercentLeafType percentLeafType2 = PercentLeafType.Hardwood;
                bool isHardwood = percentLeafType1.GetValueOrDefault() == percentLeafType2 & percentLeafType1.HasValue;
                double num = freshWeightBiomassTonne - this.GetFreshWeightBiomassRemoved(mortalityClasses, freshWeightBiomassTonne, dbh);
                double softwoodHardwood = this.GetMerchantableSoftwoodHardwood(species2, dbh, isHardwood);
                double boardFeet = num * softwoodHardwood * 176.0;
                row[columnName1] = (object) this.AdjustCurrency(this.GetMerchantableLumber(boardFeet, species2, dbh, isHardwood) * 0.105);
                row[columnName2] = (object) this.AdjustCurrency(this.BoardFeetToFirewoodCoord(boardFeet) * 25.0);
                row[columnName3] = (object) this.AdjustCurrency(boardFeet * 0.075);
                row[columnName4] = (object) this.AdjustCurrency(num * 1.79);
              }
            }
          }
        }
      }
      return data;
    }

    private double GetFreshWeightBiomassRemoved(
      IList<DBHMortalityClass> dbhMortalityClasses,
      double freshWeightBiomassTonne,
      double dbh)
    {
      return freshWeightBiomassTonne * this.GetDBHMortality(dbhMortalityClasses, dbh) / 100.0;
    }

    private double GetDBHMortality(IList<DBHMortalityClass> dbhMortalityClasses, double dbh)
    {
      double num = Math.Round(dbh, 1);
      foreach (DBHMortalityClass dbhMortalityClass in (IEnumerable<DBHMortalityClass>) dbhMortalityClasses)
      {
        if (dbhMortalityClass.DBHMin <= num && num <= dbhMortalityClass.DBHMax)
          return dbhMortalityClass.Mortality;
      }
      return dbhMortalityClasses.OrderByDescending<DBHMortalityClass, double>((Func<DBHMortalityClass, double>) (i => i.DBHMax)).First<DBHMortalityClass>().Mortality;
    }

    public IList<DBHMortalityClass> GetDBHMortalityClasses() => (IList<DBHMortalityClass>) RetryExecutionHandler.Execute<List<DBHMortalityClass>>((Func<List<DBHMortalityClass>>) (() =>
    {
      using (ISession session = ReportBase.m_ps.LocSp.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          List<DBHMortalityClass> list = session.Query<DBHMortalityClass>().WithOptions<DBHMortalityClass>((System.Action<NhQueryableOptions>) (o => o.SetCacheable(true))).OrderBy<DBHMortalityClass, double>((Expression<Func<DBHMortalityClass, double>>) (wd => wd.DBHMin)).Distinct<DBHMortalityClass>().ToList<DBHMortalityClass>();
          transaction.Commit();
          return list;
        }
      }
    }));

    private double AboveGroundBiomassToFreshWeightBiomass(double biomass, Species species) => !this.IsAngiosperm(species) ? biomass / 0.48 : biomass / 0.56;

    private bool IsAngiosperm(Species species)
    {
      PercentLeafType? percentLeafType1 = species.PercentLeafType;
      PercentLeafType percentLeafType2 = PercentLeafType.Hardwood;
      if (!(percentLeafType1.GetValueOrDefault() == percentLeafType2 & percentLeafType1.HasValue))
      {
        PercentLeafType? percentLeafType3 = species.PercentLeafType;
        PercentLeafType percentLeafType4 = PercentLeafType.Herbaceous;
        if (!(percentLeafType3.GetValueOrDefault() == percentLeafType4 & percentLeafType3.HasValue))
        {
          PercentLeafType? percentLeafType5 = species.PercentLeafType;
          PercentLeafType percentLeafType6 = PercentLeafType.Palm;
          return percentLeafType5.GetValueOrDefault() == percentLeafType6 & percentLeafType5.HasValue;
        }
      }
      return true;
    }

    private double GetMerchantableSoftwoodHardwood(Species species, double dbh, bool isHardwood)
    {
      if (dbh < 13.0 || species.GetSpeciesClassId() == 1)
        return 0.0;
      return species.GetSpeciesClassId() != 6 ? Math.Exp(-5.424 / dbh - 0.3065) : Math.Exp(-1.8055 / dbh - 0.3737);
    }

    private double GetMerchantableLumber(
      double boardFeet,
      Species species,
      double dbh,
      bool isHardwood)
    {
      if (species.GetSpeciesClassId() == 6 && dbh >= 23.0)
        return boardFeet;
      return ((IEnumerable<int>) new int[4]
      {
        2,
        3,
        4,
        5
      }).Contains<int>(species.GetSpeciesClassId()) && dbh >= 28.0 ? boardFeet : 0.0;
    }

    private double BoardFeetToFirewoodCoord(double boardFeet) => boardFeet / 500.0;

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
        ColName = ReportBase.ScientificName ? data.Columns[2].ColumnName : data.Columns[3].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 2
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.LumberWood,
        ColName = data.Columns[10].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 3
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Firewood, this.CurrencySymbol),
        ColName = data.Columns[11].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 4
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Pallets, this.CurrencySymbol),
        ColName = data.Columns[12].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 5
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.WoodChips, this.CurrencySymbol),
        ColName = data.Columns[13].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 6
      });
      if (this.hasCoordinates && ReportBase.m_ps.ShowGPS)
      {
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.xCoordinate,
          ColName = data.Columns[6].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 7
        });
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.yCoordinate,
          ColName = data.Columns[7].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 8
        });
      }
      if (ReportBase.m_ps.ShowComments)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.Comments,
          ColName = data.Columns[8].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 9
        });
      if (this.hasUID && ReportBase.m_ps.ShowUID)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.UserID,
          ColName = data.Columns[9].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 10
        });
      return columnFormatList;
    }
  }
}
