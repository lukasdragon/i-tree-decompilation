// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.LeafAreaAndBiomassBase
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Domain.Properties;
using Eco.Util;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  internal class LeafAreaAndBiomassBase : DatabaseReport
  {
    protected EstimateDataTypes _estimateType;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> strataDic = new SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>>();
      SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> strataDict1 = this.AddLeafAreaToStrataDict(this.GetLandUseCompositionByStrataLeafArea(), strataDic);
      SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> strataDict2 = this.AddLeafBiomassToStrataDict(this.GetLandUseCompositionByStrataLeafBiomass(), strataDict1);
      if (this.curYear.RecordStrata)
      {
        SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> strataDict3 = this.AddLeafAreaDensityToStrataDict(this.GetLandUseCompositionByStrataLeafAreaDensity(), strataDict2);
        strataDict2 = this.AddLeafBiomassDensityToStrataDict(this.GetLandUseCompositionByStrataLeafBiomassDensity(), strataDict3);
      }
      C1doc.ClipPage = true;
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Width = (Unit) "100%";
      renderTable.Cols[0].Width = (Unit) "12%";
      renderTable.Cols[1].Width = (Unit) "18%";
      int count = 3;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.Rows[0].Style.FontSize = 14f;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 2].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.Cells[0, 6].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.Cols[0].Style.FontBold = true;
      if (!this.curYear.RecordStrata)
        renderTable.Cols[2].Visible = renderTable.Cols[3].Visible = renderTable.Cols[4].Visible = renderTable.Cols[5].Visible = false;
      renderTable.Cells[0, 0].SpanCols = 2;
      renderTable.Cells[1, 0].SpanCols = 2;
      renderTable.Cells[0, 2].SpanCols = 4;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.Density;
      renderTable.Cells[0, 6].SpanCols = 4;
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      renderTable.Cells[1, 2].SpanCols = 2;
      renderTable.Cells[1, 2].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafArea, ReportUtil.GetValuePerValueStr(this.SquareMeterUnits(), ReportBase.HaUnits()));
      renderTable.Cells[1, 4].SpanCols = 2;
      renderTable.Cells[1, 4].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafBiomass, ReportUtil.GetValuePerValueStr(ReportBase.KgUnits(), ReportBase.HaUnits()));
      renderTable.Cells[1, 6].SpanCols = 2;
      renderTable.Cells[1, 6].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafArea, ReportBase.HaUnits());
      renderTable.Cells[1, 8].SpanCols = 2;
      renderTable.Cells[1, 8].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafBiomass, ReportBase.TonneUnits());
      renderTable.Cells[2, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[2, 1].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[2, 2].Text = renderTable.Cells[2, 4].Text = renderTable.Cells[2, 6].Text = renderTable.Cells[2, 8].Text = i_Tree_Eco_v6.Resources.Strings.ValueAbbr;
      renderTable.Cells[2, 3].Text = renderTable.Cells[2, 5].Text = renderTable.Cells[2, 7].Text = renderTable.Cells[2, 9].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
      int num1 = count;
      short valueOrderFromName1 = this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area");
      short valueOrderFromName2 = this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total");
      for (int index1 = 0; index1 < strataDict2.Count; ++index1)
      {
        int key1 = strataDict2.Keys[index1];
        if (this.curYear.RecordStrata || key1 != (int) valueOrderFromName1)
        {
          SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass> sortedList = strataDict2[key1];
          renderTable.Cells[num1, 0].Text = !this.curYear.RecordStrata ? i_Tree_Eco_v6.Resources.Strings.StudyArea : this.estUtil.ClassValues[Classifiers.Strata][(short) key1].Item1;
          for (int index2 = 0; index2 < sortedList.Count; ++index2)
          {
            short key2 = (short) sortedList.Keys[index2];
            LeafAreaAndBiomassBase.LeafAreaBiomass leafAreaBiomass = sortedList.Values[index2];
            renderTable.Cells[num1, 1].Text = !ReportBase.ScientificName ? this.estUtil.ClassValues[Classifiers.Species][key2].Item1 : this.estUtil.ClassValues[Classifiers.Species][key2].Item2;
            if ((int) key2 == (int) valueOrderFromName2)
            {
              renderTable.Cells[num1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Total;
              renderTable.Rows[num1].Style.Borders.Top = LineDef.Default;
              renderTable.Rows[num1].Style.Borders.Bottom = LineDef.Default;
              renderTable.Rows[num1].Style.FontBold = true;
            }
            if (key1 == (int) valueOrderFromName1 && (int) key2 == (int) valueOrderFromName2)
            {
              renderTable.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
              renderTable.Rows[num1].Style.FontBold = true;
              renderTable.Cells[num1, 1].Text = string.Empty;
            }
            if (this.curYear.RecordStrata)
            {
              renderTable.Cells[num1, 2].Text = (leafAreaBiomass.LeafAreaDensity * (ReportBase.EnglishUnits ? 4356000.0 : 1000000.0)).ToString("N1");
              renderTable.Cells[num1, 3].Text = (leafAreaBiomass.LeafAreaDensitySE * (ReportBase.EnglishUnits ? 4356000.0 : 1000000.0)).ToString("N1");
              renderTable.Cells[num1, 4].Text = EstimateUtil.ConvertToEnglish(leafAreaBiomass.BiomassDensity, Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.Hectare, Units.None), ReportBase.EnglishUnits).ToString("N1");
              renderTable.Cells[num1, 5].Text = EstimateUtil.ConvertToEnglish(leafAreaBiomass.BiomassDensitySE, Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.Hectare, Units.None), ReportBase.EnglishUnits).ToString("N1");
            }
            renderTable.Cells[num1, 6].Text = (leafAreaBiomass.LeafArea * (ReportBase.EnglishUnits ? 247.105 : 100.0)).ToString("N1");
            renderTable.Cells[num1, 7].Text = (leafAreaBiomass.LeafAreaSE * (ReportBase.EnglishUnits ? 247.105 : 100.0)).ToString("N1");
            renderTable.Cells[num1, 8].Text = EstimateUtil.ConvertToEnglish(leafAreaBiomass.Biomass, Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None), ReportBase.EnglishUnits).ToString("N1");
            renderTable.Cells[num1, 9].Text = EstimateUtil.ConvertToEnglish(leafAreaBiomass.BiomassSE, Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None), ReportBase.EnglishUnits).ToString("N1");
            ++num1;
          }
        }
      }
      int num2 = this.curYear.RecordStrata ? 2 : 6;
      for (int row = 3; row < renderTable.Rows.Count; ++row)
      {
        for (int col = num2; col < renderTable.Cols.Count; ++col)
        {
          if (renderTable.Cells[row, col].Text == "0.0")
            renderTable.Cells[row, col].Text = "<0.1";
        }
      }
      ReportUtil.FormatRenderTable(renderTable);
    }

    private SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> AddLeafAreaToStrataDict(
      DataTable data,
      SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> strataDic)
    {
      SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> strataDict = strataDic;
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"]);
        if (!strataDic.ContainsKey(key1))
          strataDic.Add(key1, new SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>());
        if (!strataDic[key1].ContainsKey(key2))
          strataDic[key1].Add(key2, new LeafAreaAndBiomassBase.LeafAreaBiomass()
          {
            LeafArea = num1,
            LeafAreaSE = num2
          });
      }
      return strataDict;
    }

    private SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> AddLeafBiomassToStrataDict(
      DataTable data,
      SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> strataDic)
    {
      SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> strataDict = strataDic;
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"]);
        if (!strataDic.ContainsKey(key1))
          strataDic.Add(key1, new SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>());
        if (!strataDic[key1].ContainsKey(key2))
        {
          strataDic[key1].Add(key2, new LeafAreaAndBiomassBase.LeafAreaBiomass()
          {
            Biomass = num1,
            BiomassSE = num2
          });
        }
        else
        {
          strataDic[key1][key2].Biomass = num1;
          strataDic[key1][key2].BiomassSE = num2;
        }
      }
      return strataDict;
    }

    private SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> AddLeafAreaDensityToStrataDict(
      DataTable data,
      SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> strataDic)
    {
      SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> strataDict = strataDic;
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"]);
        if (!strataDic.ContainsKey(key1))
          strataDic.Add(key1, new SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>());
        if (!strataDic[key1].ContainsKey(key2))
        {
          strataDic[key1].Add(key2, new LeafAreaAndBiomassBase.LeafAreaBiomass()
          {
            LeafAreaDensity = num1,
            LeafAreaDensitySE = num2
          });
        }
        else
        {
          strataDic[key1][key2].LeafAreaDensity = num1;
          strataDic[key1][key2].LeafAreaDensitySE = num2;
        }
      }
      return strataDict;
    }

    private SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> AddLeafBiomassDensityToStrataDict(
      DataTable data,
      SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> strataDic)
    {
      SortedList<int, SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>> strataDict = strataDic;
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]) * 1000.0;
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"]) * 1000.0;
        if (!strataDic.ContainsKey(key1))
          strataDic.Add(key1, new SortedList<int, LeafAreaAndBiomassBase.LeafAreaBiomass>());
        if (!strataDic[key1].ContainsKey(key2))
        {
          strataDic[key1].Add(key2, new LeafAreaAndBiomassBase.LeafAreaBiomass()
          {
            BiomassDensity = num1,
            BiomassDensitySE = num2
          });
        }
        else
        {
          strataDic[key1][key2].BiomassDensity = num1;
          strataDic[key1][key2].BiomassDensitySE = num2;
        }
      }
      return strataDict;
    }

    private DataTable GetLandUseCompositionByStrataLeafArea() => this.GetMultipleFieldsData2().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.LeafArea]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.None, Units.None)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetLandUseCompositionByStrataLeafBiomass() => this.GetMultipleFieldsData2().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.LeafBiomass]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetLandUseCompositionByStrataLeafAreaDensity() => this.GetMultipleFieldsData2().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.LeafArea]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.Hectare, Units.None)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetLandUseCompositionByStrataLeafBiomassDensity() => this.GetMultipleFieldsData2().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.LeafBiomass]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Hectare, Units.None)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetMultipleFieldsData2()
    {
      List<Classifiers> source = new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.Species
      };
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetMultipleFieldsData2(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(this._estimateType, source)], source.Select<Classifiers, string>((Func<Classifiers, string>) (c => this.estUtil.ClassifierNames[c])).ToArray<string>());
    }

    private class LeafAreaBiomass
    {
      public double LeafAreaDensity { get; set; }

      public double LeafAreaDensitySE { get; set; }

      public double BiomassDensity { get; set; }

      public double BiomassDensitySE { get; set; }

      public double LeafArea { get; set; }

      public double LeafAreaSE { get; set; }

      public double Biomass { get; set; }

      public double BiomassSE { get; set; }
    }
  }
}
