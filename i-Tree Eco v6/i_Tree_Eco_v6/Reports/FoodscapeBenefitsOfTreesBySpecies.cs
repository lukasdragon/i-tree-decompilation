// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.FoodscapeBenefitsOfTreesBySpecies
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.Core;
using DaveyTree.NHibernate;
using Eco.Domain.v6;
using Eco.Util;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class FoodscapeBenefitsOfTreesBySpecies : DatabaseReport
  {
    private FoodscapeBenefitsOfTreesBySpecies.TupleList<string, Dictionary<string, string>> _columnsMap;
    private DataTable _dt;

    public FoodscapeBenefitsOfTreesBySpecies()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleFoodscapeBenefitsOfTreesBySpecies;
      TypeHelper<EdibleValues> typeHelper = new TypeHelper<EdibleValues>();
      FoodscapeBenefitsOfTreesBySpecies.TupleList<string, Dictionary<string, string>> tupleList1 = new FoodscapeBenefitsOfTreesBySpecies.TupleList<string, Dictionary<string, string>>();
      FoodscapeBenefitsOfTreesBySpecies.TupleList<string, Dictionary<string, string>> tupleList2 = tupleList1;
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      dictionary1.Add("Pollinators", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.Pollinators)));
      dictionary1.Add("Productivity", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.Productivity)));
      dictionary1.Add("Usefull parts", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.UsefulParts)));
      dictionary1.Add("Harvesting frequency", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.HarvestingFrequency)));
      Dictionary<string, string> dictionary2 = dictionary1;
      tupleList2.Add("General", dictionary2);
      FoodscapeBenefitsOfTreesBySpecies.TupleList<string, Dictionary<string, string>> tupleList3 = tupleList1;
      Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
      dictionary3.Add("Fruit present", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.FruitSeason)));
      dictionary3.Add("Edible parts", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.EdibleParts)));
      dictionary3.Add("Fruit type", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.FruitType)));
      dictionary3.Add("Fruit/nut size", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.FruitNutSize)));
      Dictionary<string, string> dictionary4 = dictionary3;
      tupleList3.Add("Fruit", dictionary4);
      FoodscapeBenefitsOfTreesBySpecies.TupleList<string, Dictionary<string, string>> tupleList4 = tupleList1;
      Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
      dictionary5.Add("Flowering type", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.FloweringType)));
      dictionary5.Add("Bloom time", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.BloomTime)));
      dictionary5.Add("Flower color", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.FlowerColor)));
      dictionary5.Add("Fragrance", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.Fragrance)));
      Dictionary<string, string> dictionary6 = dictionary5;
      tupleList4.Add("Flowers", dictionary6);
      FoodscapeBenefitsOfTreesBySpecies.TupleList<string, Dictionary<string, string>> tupleList5 = tupleList1;
      Dictionary<string, string> dictionary7 = new Dictionary<string, string>();
      dictionary7.Add("Pharmaceuticals", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.Pharmaceuticals)));
      dictionary7.Add("Fiber", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.Fiber)));
      dictionary7.Add("Food", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.Food)));
      dictionary7.Add("Fuel", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.Fuel)));
      dictionary7.Add("Value added", typeHelper.NameOf((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (ev => ev.ValueAdded)));
      Dictionary<string, string> dictionary8 = dictionary7;
      tupleList5.Add("Yield", dictionary8);
      this._columnsMap = tupleList1;
    }

    protected override void GetDBData()
    {
      if (this._dt != null)
        return;
      IList<string> unique_species_codes;
      using (ISession session = ProgramSession.GetInstance().InputSession.CreateSession())
        unique_species_codes = session.CreateCriteria<Tree>().SetProjection(Projections.Distinct((IProjection) Projections.Property("Species"))).List<string>();
      if (unique_species_codes.Count <= 0)
        return;
      this._dt = RetryExecutionHandler.Execute<DataTable>((Func<DataTable>) (() =>
      {
        using (ISession session = ReportBase.m_ps.LocSp.OpenSession())
        {
          using (session.BeginTransaction())
          {
            Species s = (Species) null;
            EdibleValues ev = (EdibleValues) null;
            return session.QueryOver<EdibleValues>((System.Linq.Expressions.Expression<Func<EdibleValues>>) (() => ev)).JoinAlias((System.Linq.Expressions.Expression<Func<EdibleValues, object>>) (evs => evs.Species), (System.Linq.Expressions.Expression<Func<object>>) (() => s)).JoinQueryOver<Species>((System.Linq.Expressions.Expression<Func<EdibleValues, Species>>) (evs => evs.Species)).WhereRestrictionOn((System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.Code)).IsIn((object[]) unique_species_codes.ToArray<string>()).Select(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => s.Code)).As("SpCode"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.BloomTime)).As("BloomTime"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.EdibleParts)).As("EdibleParts"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.Fiber)).As("Fiber"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.FlowerColor)).As("FlowerColor"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.FloweringType)).As("FloweringType"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.Food)).As("Food"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.Fragrance)).As("Fragrance"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.FruitNutSize)).As("FruitNutSize"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.FruitSeason)).As("FruitSeason"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.FruitType)).As("FruitType"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.Fuel)).As("Fuel"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.HarvestingFrequency)).As("HarvestingFrequency"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.Pharmaceuticals)).As("Pharmaceuticals"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.Pollinators)).As("Pollinators"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.Productivity)).As("Productivity"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.UsefulParts)).As("UsefulParts"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => ev.ValueAdded)).As("ValueAdded")).TransformUsing((IResultTransformer) new DataTableResultTransformer()).SingleOrDefault<DataTable>();
          }
        }
      }));
    }

    private List<string> getDBColumnNames()
    {
      List<string> dbColumnNames = new List<string>();
      foreach (Tuple<string, Dictionary<string, string>> columns in (List<Tuple<string, Dictionary<string, string>>>) this._columnsMap)
        dbColumnNames.AddRange((IEnumerable<string>) new List<string>((IEnumerable<string>) columns.Item2.Values));
      return dbColumnNames;
    }

    public string CreateSelectString(string prefix)
    {
      string selectString = string.Empty;
      List<string> dbColumnNames = this.getDBColumnNames();
      string str1 = dbColumnNames.Last<string>();
      foreach (string str2 in dbColumnNames)
      {
        if (!str2.Equals(str1))
          selectString = selectString + prefix + "." + str2 + ", ";
      }
      if (dbColumnNames.Count > 0)
        selectString = selectString + prefix + "." + str1;
      return selectString;
    }

    public override object GetData()
    {
      this.GetDBData();
      return (object) this._dt;
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
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
        {
          RenderTable renderTable = new RenderTable();
          C1doc.Body.Children.Add((RenderObject) renderTable);
          renderTable.Style.Spacing.Bottom = (Unit) "1ls";
          C1doc.PageLayout.PageSettings.Landscape = false;
          renderTable.Style.FontSize = 11f;
          renderTable.Width = (Unit) "72%";
          renderTable.Cols[0].Width = (Unit) "1%";
          renderTable.Cols[1].Width = (Unit) "1%";
          renderTable.Cols[2].Width = (Unit) "20%";
          Style style = ReportUtil.SetDefaultReportFormatting(C1doc, renderTable, this.curYear, 1);
          renderTable.Style.TextAlignHorz = AlignHorzEnum.Left;
          int num = 0;
          renderTable.Cells[0, 0].SpanCols = 4;
          renderTable.Cells[num, 0].Text = string.Format("{0}: {1}", (object) i_Tree_Eco_v6.Resources.Strings.SpeciesName, (object) this.GetSpecies((string) row["SpCode"]));
          renderTable.Rows[num].Style.Borders.Bottom = LineDef.Default;
          renderTable.Rows[num].Style.GridLines.Bottom = LineDef.Default;
          foreach (Tuple<string, Dictionary<string, string>> columns in (List<Tuple<string, Dictionary<string, string>>>) this._columnsMap)
          {
            ++num;
            renderTable.Cells[num, 1].Text = columns.Item1;
            renderTable.Cells[num, 1].SpanCols = 3;
            renderTable.Rows[num].Style.Parent = style;
            renderTable.Rows[num].Style.FontBold = true;
            foreach (string key in columns.Item2.Keys)
            {
              ++num;
              renderTable.Cells[num, 2].Style.Borders.Right = LineDef.Default;
              renderTable.Cells[num, 2].Text = key;
              renderTable.Cells[num, 3].Text = row[columns.Item2[key]].ToString();
            }
          }
        }
      }
    }

    public override void SetAlignment(C1PrintDocument C1doc)
    {
      C1doc.Style.FlowAlign = FlowAlignEnum.Center;
      C1doc.Style.FlowAlignChildren = FlowAlignEnum.Center;
      C1doc.Style.TextAlignHorz = AlignHorzEnum.Left;
    }

    public class TupleList<T1, T2> : List<Tuple<T1, T2>>
    {
      public void Add(T1 item, T2 item2) => this.Add(new Tuple<T1, T2>(item, item2));
    }
  }
}
