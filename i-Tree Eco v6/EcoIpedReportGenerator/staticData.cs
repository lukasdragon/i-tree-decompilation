// Decompiled with JetBrains decompiler
// Type: EcoIpedReportGenerator.staticData
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.NHibernate;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Queries.Interfaces;
using Eco.Util.Views;
using i_Tree_Eco_v6;
using IPED.Domain;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;

namespace EcoIpedReportGenerator
{
  public static class staticData
  {
    private static bool _useScientificName;
    private static bool _isSample;
    private static DataSet _dsData = new DataSet();
    private static Guid _yearGuid;
    private static ProgramSession _m_ps;
    public static int PestPestAffectedClassValueOrder;
    public static int PestPestNoneClassValueOrder;
    public static int PestPestUnknownClassValueOrder;
    public static IQuerySupplier queryProvider;
    public static Dictionary<Classifiers, int> dctClassifierNoneValues = new Dictionary<Classifiers, int>();
    public static EstimateUtil estUtil;

    public static bool UseScientificName
    {
      get => staticData._useScientificName;
      set => staticData._useScientificName = value;
    }

    public static bool IsSample
    {
      get => staticData._isSample;
      set => staticData._isSample = value;
    }

    public static DataSet DsData
    {
      get => staticData._dsData;
      set => staticData._dsData = value;
    }

    public static Guid YearGuid
    {
      get => staticData._yearGuid;
      set => staticData._yearGuid = value;
    }

    public static ProgramSession ProgramSession
    {
      get => staticData._m_ps;
      set => staticData._m_ps = value;
    }

    public static void ClearData()
    {
      staticData._yearGuid = Guid.Empty;
      staticData._isSample = false;
      staticData.dctClassifierNoneValues = new Dictionary<Classifiers, int>();
      staticData.PestPestAffectedClassValueOrder = -1;
      staticData.PestPestNoneClassValueOrder = -1;
      staticData.PestPestUnknownClassValueOrder = -1;
      if (staticData._dsData != null)
        staticData._dsData.Dispose();
      staticData._dsData = new DataSet();
    }

    public static string GetGenusFromSppCode(string SpCode)
    {
      ProgramSession instance = ProgramSession.GetInstance();
      SpeciesView speciesView;
      if (instance.Species.TryGetValue(SpCode, out speciesView))
      {
        if (speciesView.Rank < SpeciesRank.Genus)
          return (string) null;
        while (speciesView != null && speciesView.Rank > SpeciesRank.Genus)
          instance.Species.TryGetValue(speciesView.Species.Parent.Code, out speciesView);
        if (speciesView != null)
          return instance.SpeciesDisplayName != SpeciesDisplayEnum.CommonName ? speciesView.ScientificName : speciesView.CommonName;
      }
      return (string) null;
    }

    public static string GetIPEDFieldName(string sField)
    {
      string ipedFieldName = string.Empty;
      switch (sField.ToLower())
      {
        case "bbabngrowth":
          ipedFieldName = "Loose Bark";
          break;
        case "bbdiseasesigns":
          ipedFieldName = "Disease Signs";
          break;
        case "bbinsectpres":
          ipedFieldName = "Insect Presence";
          break;
        case "bbinsectsigns":
          ipedFieldName = "Insect Signs";
          break;
        case "bbprobloc":
          ipedFieldName = "Problem Location";
          break;
        case "ftabnfoli":
          ipedFieldName = "Abnormal Foliage";
          break;
        case "ftchewfoli":
          ipedFieldName = "Defoliation";
          break;
        case "ftdiscfoli":
          ipedFieldName = "Discolored Foliage";
          break;
        case "ftfoliaffect":
          ipedFieldName = "% Foliage Affected";
          break;
        case "ftinsectsigns":
          ipedFieldName = "Insect Signs";
          break;
        case "tsdieback":
          ipedFieldName = "Dieback";
          break;
        case "tsenvstress":
          ipedFieldName = "Environmental Stress";
          break;
        case "tsepisprout":
          ipedFieldName = "Epicormic Sprouts";
          break;
        case "tshumstress":
          ipedFieldName = "Human caused Stress";
          break;
        case "tswiltfoli":
          ipedFieldName = "Wilted Foliage";
          break;
      }
      return ipedFieldName;
    }

    public static Classifiers GetEstIPEDClassifier(string TableName)
    {
      Classifiers estIpedClassifier = Classifiers.Species;
      switch (TableName.ToLower())
      {
        case "bbabngrowth":
          estIpedClassifier = Classifiers.BBAbnGrowth;
          break;
        case "bbdiseasesigns":
          estIpedClassifier = Classifiers.BBDiseaseSigns;
          break;
        case "bbinsectpres":
          estIpedClassifier = Classifiers.BBInsectPres;
          break;
        case "bbinsectsigns":
          estIpedClassifier = Classifiers.BBInsectSigns;
          break;
        case "bbprobloc":
          estIpedClassifier = Classifiers.BBProbLoc;
          break;
        case "ftabnfoli":
          estIpedClassifier = Classifiers.FTAbnFoli;
          break;
        case "ftchewfoli":
          estIpedClassifier = Classifiers.FTChewFoli;
          break;
        case "ftdiscfoli":
          estIpedClassifier = Classifiers.FTDiscFoli;
          break;
        case "ftfoliaffect":
          estIpedClassifier = Classifiers.FTFoliAffect;
          break;
        case "ftinsectsigns":
          estIpedClassifier = Classifiers.FTInsectSigns;
          break;
        case "tsdieback":
          estIpedClassifier = Classifiers.TSDieback;
          break;
        case "tsenvstress":
          estIpedClassifier = Classifiers.TSEnvStress;
          break;
        case "tsepisprout":
          estIpedClassifier = Classifiers.TSEpiSprout;
          break;
        case "tshumstress":
          estIpedClassifier = Classifiers.TSHumStress;
          break;
        case "tswiltfoli":
          estIpedClassifier = Classifiers.TSWiltFoli;
          break;
      }
      return estIpedClassifier;
    }

    public static string GetIPEDSignSymptomTypeLocationForField(string sField)
    {
      string locationForField = string.Empty;
      switch (sField.ToLower())
      {
        case "bbabngrowth":
          locationForField = "Branches/Bole";
          break;
        case "bbdiseasesigns":
          locationForField = "Branches/Bole";
          break;
        case "bbinsectpres":
          locationForField = "Branches/Bole";
          break;
        case "bbinsectsigns":
          locationForField = "Branches/Bole";
          break;
        case "bbprobloc":
          locationForField = "Branches/Bole";
          break;
        case "ftabnfoli":
          locationForField = "Foliage/Twigs";
          break;
        case "ftchewfoli":
          locationForField = "Foliage/Twigs";
          break;
        case "ftdiscfoli":
          locationForField = "Foliage/Twigs";
          break;
        case "ftfoliaffect":
          locationForField = "Foliage/Twigs";
          break;
        case "ftinsectsigns":
          locationForField = "Foliage/Twigs";
          break;
        case "tsdieback":
          locationForField = "Tree Stress";
          break;
        case "tsenvstress":
          locationForField = "Tree Stress";
          break;
        case "tsepisprout":
          locationForField = "Tree Stress";
          break;
        case "tshumstress":
          locationForField = "Tree Stress";
          break;
        case "tswiltfoli":
          locationForField = "Tree Stress";
          break;
      }
      return locationForField;
    }

    public static void LoadData()
    {
      ISession session = staticData._m_ps.InputSession.CreateSession();
      Year y = session.Get<Year>((object) staticData._m_ps.InputSession.YearKey);
      staticData.AddPests();
      staticData.AddLucid(staticData._m_ps.IPEDSessionFactory.OpenSession());
      staticData.AddStrataTable(session, y);
      staticData.AddSpeciesPresent(staticData.AddTreeTable(session, y));
    }

    public static void AddStrataTable(ISession s, Year y)
    {
      DataTable strata = staticData.GetStrata(s, y);
      strata.TableName = "Strata";
      staticData._dsData.Tables.Add(strata);
    }

    public static DataTable AddTreeTable(ISession s, Year y)
    {
      DataTable trees = staticData.GetTrees(s, y);
      trees.TableName = "Trees";
      staticData._dsData.Tables.Add(trees);
      return trees;
    }

    public static DataTable GetStrata(ISession s, Year y)
    {
      DataTable dataTable = new DataTable();
      using (s.BeginTransaction())
      {
        Strata strata = (Strata) null;
        return s.QueryOver<Strata>((System.Linq.Expressions.Expression<Func<Strata>>) (() => strata)).Where((System.Linq.Expressions.Expression<Func<Strata, bool>>) (st => st.Year == y)).Select(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) strata.Id)).As("Id"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => strata.Description)).As("Description")).TransformUsing((IResultTransformer) new DataTableResultTransformer()).SingleOrDefault<DataTable>();
      }
    }

    public static DataTable GetTrees(ISession s, Year y)
    {
      using (s.BeginTransaction())
      {
        Tree tree = (Tree) null;
        Plot plot = (Plot) null;
        PlotLandUse plotLandUse = (PlotLandUse) null;
        LandUse landUse = (LandUse) null;
        Strata strata = (Strata) null;
        return s.QueryOver<Tree>((System.Linq.Expressions.Expression<Func<Tree>>) (() => tree)).JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => tr.Plot), (System.Linq.Expressions.Expression<Func<object>>) (() => plot)).Where((System.Linq.Expressions.Expression<Func<bool>>) (() => plot.Year == y)).JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => tr.PlotLandUse), (System.Linq.Expressions.Expression<Func<object>>) (() => plotLandUse), JoinType.LeftOuterJoin).JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => plotLandUse.LandUse), (System.Linq.Expressions.Expression<Func<object>>) (() => landUse), JoinType.LeftOuterJoin).JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => plot.Strata), (System.Linq.Expressions.Expression<Func<object>>) (() => strata)).Select(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plot.Id)).As("PlotID"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => plot.Address)).As("Address"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => landUse.Description)).As("FieldLandUseDescription"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => strata.Description)).As("MapLandUseDescription"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.Id)).As("TreeID"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => tree.Species)).As("SpCode"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.StreetTree)).As("TreeSite"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.TSDieback)).As("PestTSDieback"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.TSEpiSprout)).As("PestTSEpiSprout"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.TSWiltFoli)).As("PestTSWiltFoli"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.TSEnvStress)).As("PestTSEnvStress"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.TSHumStress)).As("PestTSHumStress"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => tree.IPED.TSNotes)).As("PestTSNotes"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.FTChewFoli)).As("PestFTChewFoli"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.FTDiscFoli)).As("PestFTDiscFoli"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.FTAbnFoli)).As("PestFTAbnFoli"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.FTInsectSigns)).As("PestFTInsectSigns"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.FTFoliAffect)).As("PestFTFoliAffect"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => tree.IPED.FTNotes)).As("PestFTNotes"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.BBInsectSigns)).As("PestBBInsectSigns"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.BBInsectPres)).As("PestBBInsectPres"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.BBDiseaseSigns)).As("PestBBDiseaseSigns"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.BBProbLoc)).As("PestBBProbLoc"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.BBAbnGrowth)).As("PestBBAbnGrowth"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => tree.IPED.BBNotes)).As("PestBBNotes"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.IPED.Pest)).As("PestPest")).TransformUsing((IResultTransformer) new DataTableResultTransformer()).SingleOrDefault<DataTable>();
      }
    }

    public static void AddSpeciesPresent(DataTable trees)
    {
      DataTable table = new DataView(trees, string.Empty, string.Empty, DataViewRowState.CurrentRows).ToTable(true, "SpCode");
      table.Columns.Add("ClassValueName", typeof (string));
      table.Columns.Add("ClassValueName1", typeof (string));
      int speciesDisplayName = (int) staticData._m_ps.SpeciesDisplayName;
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        SpeciesView speciesView;
        if (staticData._m_ps.Species.TryGetValue((string) row["SpCode"], out speciesView))
        {
          row["ClassValueName"] = (object) speciesView.CommonName;
          row["ClassValueName1"] = (object) speciesView.ScientificName;
        }
      }
      table.TableName = "SpeciesPresent";
      staticData._dsData.Tables.Add(table);
    }

    private static void AddLucid(ISession s)
    {
      using (s.BeginTransaction())
      {
        Lucid p = (Lucid) null;
        DataTable table = s.QueryOver<Lucid>((System.Linq.Expressions.Expression<Func<Lucid>>) (() => p)).Select(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.Id)).As("PestID"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HC_Abies)).As("HC_Abies"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HC_Juniper)).As("HC_Juniper"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HC_Picea)).As("HC_Picea"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HC_Pinus)).As("HC_Pinus"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HC_Tamrisk)).As("HC_Tamrisk"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HC_Taxus)).As("HC_Taxus"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HC_Tsuga)).As("HC_Tsuga"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HC_Other)).As("HC_Other"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Acer)).As("HH_Acer"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Aesculus)).As("HH_Aesculus"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Betula)).As("HH_Betula"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Carya)).As("HH_Carya"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Citrus)).As("HH_Citrus"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Fagus)).As("HH_Fagus"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Fraxinus)).As("HH_Fraxinus"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Juglans)).As("HH_Juglans"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Platanus)).As("HH_Platanus"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Populus)).As("HH_Populus"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Prunus)).As("HH_Prunus"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Quercus)).As("HH_Quercus"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Salix)).As("HH_Salix"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Tilia)).As("HH_Tilia"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Ulmus)).As("HH_Ulmus"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.HH_Other)).As("HH_Other"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBAbnGrowth_05)).As("BBAbnGrowth_05"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBAbnGrowth_07)).As("BBAbnGrowth_07"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBAbnGrowth_08)).As("BBAbnGrowth_08"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBAbnGrowth_09)).As("BBAbnGrowth_09"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBDiseaseSigns_01)).As("BBDiseaseSigns_01"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBDiseaseSigns_02)).As("BBDiseaseSigns_02"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBDiseaseSigns_03)).As("BBDiseaseSigns_03"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBDiseaseSigns_04)).As("BBDiseaseSigns_04"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBDiseaseSigns_07)).As("BBDiseaseSigns_07"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBDiseaseSigns_09)).As("BBDiseaseSigns_09"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBDiseaseSigns_10)).As("BBDiseaseSigns_10"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBInsectPres_01)).As("BBInsectPres_01"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBInsectPres_02)).As("BBInsectPres_02"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBInsectPres_03)).As("BBInsectPres_03"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBInsectPres_04)).As("BBInsectPres_04"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBInsectPres_05)).As("BBInsectPres_05"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBInsectSigns_01)).As("BBInsectSigns_01"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBInsectSigns_02)).As("BBInsectSigns_02"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBInsectSigns_03)).As("BBInsectSigns_03"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBInsectSigns_04)).As("BBInsectSigns_04"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBInsectSigns_05)).As("BBInsectSigns_05"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBInsectSigns_06)).As("BBInsectSigns_06"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBInsectSigns_07)).As("BBInsectSigns_07"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBProbLoc_01)).As("BBProbLoc_01"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBProbLoc_02)).As("BBProbLoc_02"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.BBProbLoc_03)).As("BBProbLoc_03"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTAbnFoli_02)).As("FTAbnFoli_02"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTAbnFoli_06)).As("FTAbnFoli_06"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTChewFoli_01)).As("FTChewFoli_01"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTChewFoli_02)).As("FTChewFoli_02"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTChewFoli_03)).As("FTChewFoli_03"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTChewFoli_04)).As("FTChewFoli_04"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTChewFoli_05)).As("FTChewFoli_05"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTDiscFoli_01)).As("FTDiscFoli_01"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTDiscFoli_02)).As("FTDiscFoli_02"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTDiscFoli_03)).As("FTDiscFoli_03"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTDiscFoli_04)).As("FTDiscFoli_04"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTDiscFoli_05)).As("FTDiscFoli_05"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTDiscFoli_06)).As("FTDiscFoli_06"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTDiscFoli_07)).As("FTDiscFoli_07"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTDiscFoli_08)).As("FTDiscFoli_08"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTDiscFoli_10)).As("FTDiscFoli_10"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTFoliAffect_02)).As("FTFoliAffect_02"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTFoliAffect_03)).As("FTFoliAffect_03"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTFoliAffect_04)).As("FTFoliAffect_04"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTInsectSigns_01)).As("FTInsectSigns_01"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTInsectSigns_03)).As("FTInsectSigns_03"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTInsectSigns_04)).As("FTInsectSigns_04"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTInsectSigns_05)).As("FTInsectSigns_05"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTInsectSigns_06)).As("FTInsectSigns_06"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.FTInsectSigns_07)).As("FTInsectSigns_07"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSDieback_02)).As("TSDieback_02"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSDieback_03)).As("TSDieback_03"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSEnvStress_01)).As("TSEnvStress_01"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSEnvStress_02)).As("TSEnvStress_02"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSEnvStress_03)).As("TSEnvStress_03"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSEnvStress_04)).As("TSEnvStress_04"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSEnvStress_05)).As("TSEnvStress_05"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSEnvStress_06)).As("TSEnvStress_06"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSEnvStress_07)).As("TSEnvStress_07"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSEnvStress_08)).As("TSEnvStress_08"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSEpiSprout_01)).As("TSEpiSprout_01"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSHumStress_01)).As("TSHumStress_01"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSHumStress_02)).As("TSHumStress_02"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSHumStress_03)).As("TSHumStress_03"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSHumStress_04)).As("TSHumStress_04"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSHumStress_05)).As("TSHumStress_05"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSWiltFoli_01)).As("TSWiltFoli_01"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.TSWiltFoli_02)).As("TSWiltFoli_02")).TransformUsing((IResultTransformer) new DataTableResultTransformer()).SingleOrDefault<DataTable>();
        table.TableName = "Lucid";
        DataColumn[] dataColumnArray = new DataColumn[1]
        {
          table.Columns["PestID"]
        };
        table.PrimaryKey = dataColumnArray;
        staticData._dsData.Tables.Add(table);
      }
    }

    private static void AddPests()
    {
      DataTable table = new DataTable("Pests");
      table.Columns.Add("Id");
      table.Columns.Add("CommonName");
      table.Columns.Add("ScientificName");
      DataColumn[] dataColumnArray = new DataColumn[1]
      {
        table.Columns["Id"]
      };
      table.PrimaryKey = dataColumnArray;
      foreach (IPED.Domain.Pest pest in (IEnumerable<IPED.Domain.Pest>) staticData.ProgramSession.IPEDData.Pests)
        table.Rows.Add((object) pest.Id, (object) pest.CommonName, (object) pest.ScientificName);
      table.Rows.RemoveAt(0);
      table.Rows.RemoveAt(0);
      staticData._dsData.Tables.Add(table);
    }

    public static bool LoadEstimateDatabase()
    {
      staticData.estUtil = new EstimateUtil(staticData.ProgramSession.InputSession, staticData.ProgramSession.LocSp);
      staticData.queryProvider = staticData.estUtil.queryProvider;
      staticData.PestPestAffectedClassValueOrder = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.PestPest, "Affected");
      staticData.PestPestNoneClassValueOrder = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.PestPest, "None");
      staticData.PestPestUnknownClassValueOrder = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.PestPest, "Unknown");
      staticData.dctClassifierNoneValues[Classifiers.TSDieback] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.TSDieback, "None");
      staticData.dctClassifierNoneValues[Classifiers.TSEpiSprout] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.TSEpiSprout, "No");
      staticData.dctClassifierNoneValues[Classifiers.TSWiltFoli] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.TSWiltFoli, "No wilt");
      staticData.dctClassifierNoneValues[Classifiers.TSEnvStress] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.TSEnvStress, "None");
      staticData.dctClassifierNoneValues[Classifiers.TSHumStress] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.TSHumStress, "None");
      staticData.dctClassifierNoneValues[Classifiers.FTChewFoli] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.FTChewFoli, "None");
      staticData.dctClassifierNoneValues[Classifiers.FTDiscFoli] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.FTDiscFoli, "None");
      staticData.dctClassifierNoneValues[Classifiers.FTAbnFoli] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.FTAbnFoli, "None");
      staticData.dctClassifierNoneValues[Classifiers.FTInsectSigns] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.FTInsectSigns, "None");
      staticData.dctClassifierNoneValues[Classifiers.FTFoliAffect] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.FTFoliAffect, "None");
      staticData.dctClassifierNoneValues[Classifiers.BBInsectSigns] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.BBInsectSigns, "None");
      staticData.dctClassifierNoneValues[Classifiers.BBInsectPres] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.BBInsectPres, "None");
      staticData.dctClassifierNoneValues[Classifiers.BBDiseaseSigns] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.BBDiseaseSigns, "None");
      staticData.dctClassifierNoneValues[Classifiers.BBAbnGrowth] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.BBAbnGrowth, "None");
      staticData.dctClassifierNoneValues[Classifiers.BBProbLoc] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.BBProbLoc, "None");
      staticData.dctClassifierNoneValues[Classifiers.PestPest] = (int) staticData.estUtil.GetClassValueOrderFromName(Classifiers.PestPest, "None");
      if (staticData.PestsTableMissing())
        return false;
      staticData.AddPestsData();
      staticData.AddSpeciesData();
      staticData.AddStrataData();
      return true;
    }

    public static bool PestsTableMissing() => staticData.GetTreeStratumPestTableName(Classifiers.PestPest).Equals(string.Empty);

    public static void AddPestsData()
    {
      List<Classifiers> classifiersList = new List<Classifiers>()
      {
        Classifiers.PestPest,
        Classifiers.TSDieback,
        Classifiers.TSEpiSprout,
        Classifiers.TSWiltFoli,
        Classifiers.TSEnvStress,
        Classifiers.TSHumStress,
        Classifiers.FTChewFoli,
        Classifiers.FTDiscFoli,
        Classifiers.FTAbnFoli,
        Classifiers.FTInsectSigns,
        Classifiers.FTFoliAffect,
        Classifiers.BBInsectSigns,
        Classifiers.BBInsectPres,
        Classifiers.BBDiseaseSigns,
        Classifiers.BBAbnGrowth,
        Classifiers.BBProbLoc
      };
      foreach (Classifiers Classifier in classifiersList)
      {
        string name = Enum.GetName(typeof (Classifiers), (object) Classifier);
        staticData.AddPestDataTable(staticData.GetStudyAreaTreePestData(staticData.GetTreeStratumPestTableName(Classifier), name), string.Format("Landuse_{0}", (object) name));
      }
      foreach (Classifiers Classifier in classifiersList)
      {
        string name = Enum.GetName(typeof (Classifiers), (object) Classifier);
        staticData.AddPestDataTable(staticData.GetTreePestData(staticData.GetTreePestTableName(Classifier), name), string.Format("Species_{0}", (object) name));
      }
    }

    public static void AddSpeciesData()
    {
      DataTable table = new DataTable("EstSpecies");
      table.Columns.Add("SpeciesId", typeof (int));
      table.Columns.Add("SpeciesName", typeof (string));
      foreach (KeyValuePair<short, Tuple<string, string>> keyValuePair in staticData.estUtil.ClassValues[Classifiers.Species])
      {
        if (keyValuePair.Value.Item1.ToLower() != "total")
        {
          DataRow row = table.NewRow();
          row["SpeciesId"] = (object) keyValuePair.Key;
          row["SpeciesName"] = staticData._useScientificName ? (object) keyValuePair.Value.Item2 : (object) keyValuePair.Value.Item1;
          table.Rows.Add(row);
        }
      }
      staticData._dsData.Tables.Add(table);
    }

    public static void AddStrataData()
    {
      DataTable table = new DataTable("EstMapLanduse");
      table.Columns.Add("LanduseId", typeof (int));
      table.Columns.Add("LanduseName", typeof (string));
      foreach (KeyValuePair<short, Tuple<string, string>> keyValuePair in staticData.estUtil.ClassValues[Classifiers.Strata])
      {
        if (keyValuePair.Value.Item1.ToLower() != "study area")
        {
          DataRow row = table.NewRow();
          row["LanduseId"] = (object) keyValuePair.Key;
          row["LanduseName"] = (object) keyValuePair.Value.Item1;
          table.Rows.Add(row);
        }
      }
      staticData._dsData.Tables.Add(table);
    }

    public static string GetTreePestTableName(Classifiers Classifier)
    {
      Tuple<EstimateDataTypes, List<Classifiers>> key = Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Species,
        Classifier
      });
      return staticData.estUtil.EstTableDictionary.ContainsKey(key) ? staticData.estUtil.EstTableDictionary[key] : string.Empty;
    }

    private static DataTable GetStudyAreaTreePestData(string estTable, string c1) => staticData.queryProvider.GetEstimateUtilProvider().GetStratumTreePestEstimateValues(estTable, c1).SetParameter<Guid>("y", staticData.YearGuid).SetParameter<short>("strata", staticData.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private static DataTable GetTreePestData(string estTable, string c1) => staticData.queryProvider.GetEstimateUtilProvider().GetPestEstimateValues(estTable, c1).SetParameter<Guid>("y", staticData.YearGuid).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    public static string GetTreeStratumPestTableName(Classifiers Classifier)
    {
      Tuple<EstimateDataTypes, List<Classifiers>> key = Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifier
      });
      return staticData.estUtil.EstTableDictionary.ContainsKey(key) ? staticData.estUtil.EstTableDictionary[key] : string.Empty;
    }

    private static void AddPestDataTable(DataTable data, string tableName)
    {
      data.TableName = tableName;
      staticData._dsData.Tables.Add(data);
    }

    public static string GetClassValueName(Classifiers classifier, short ClassValueOrder) => staticData.estUtil.ClassValues[classifier][ClassValueOrder].Item1;

    public static string GetClassValueName1(Classifiers classifier, short ClassValueOrder) => staticData.estUtil.ClassValues[classifier][ClassValueOrder].Item2;
  }
}
