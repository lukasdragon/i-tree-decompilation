// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.StructureSummaryBase
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.NHibernate;
using Eco.Util;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace i_Tree_Eco_v6.Reports
{
  [DesignerCategory("Code")]
  internal class StructureSummaryBase : DatabaseReport
  {
    protected readonly string _species;
    protected readonly string _estimateValue = "EstimateValue";
    protected readonly string _estimateStandardError = "EstimateStandardError";
    protected readonly int _totalSpecies;

    public StructureSummaryBase()
    {
      this._species = this.estUtil.ClassifierNames[Classifiers.Species];
      this._totalSpecies = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total");
    }

    public DataTable GetMultipleFieldsData(
      List<Classifiers> fields,
      EstimateTypeEnum estType,
      Units primaryUnit,
      Units secondaryUnit,
      Units tertiaryUnit)
    {
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetMultipleFieldsData(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, fields)], string.Join<Classifiers>(", ", (IEnumerable<Classifiers>) fields)).SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("equType", 1).SetParameter<int>(nameof (estType), this.estUtil.EstTypes[estType]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(primaryUnit, secondaryUnit, tertiaryUnit)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
    }

    public DataTable GetAverageCondition(List<Classifiers> fields)
    {
      List<Classifiers> classifiersList = new List<Classifiers>((IEnumerable<Classifiers>) fields)
      {
        Classifiers.Dieback
      };
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetAverageCondition(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, classifiersList)], string.Join<Classifiers>(", ", (IEnumerable<Classifiers>) fields), this.estUtil.ClassifierNames[Classifiers.Dieback]).SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<int>("eqType", 1).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
    }

    protected class StructureSummarySpecies
    {
      public double NumTrees { get; set; }

      public double NumTreesSE { get; set; }

      public double LeafArea { get; set; }

      public double LeafAreaSE { get; set; }

      public double LeafBiomass { get; set; }

      public double LeafBiomassSE { get; set; }

      public double CarbonStorage { get; set; }

      public double CarbonStorageSE { get; set; }

      public double AverageCondition { get; set; }
    }
  }
}
