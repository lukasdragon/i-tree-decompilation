// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.Summary
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Deposition;

namespace UFORE.BioEmission
{
  public class Summary
  {
    public const string YEAR_LANDUSE_TABLE = "01_YearlySummaryByLanduse";
    public const string YEAR_GENERA_TABLE = "02_YearlySummaryByGenera";

    [Obsolete]
    public static void CalculateSummary(string sBioEDB)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Summary.CalculateSummary(cnBioEDB);
      }
    }

    public static void CalculateSummary(OleDbConnection cnBioEDB)
    {
      YearlySummary.CalcSummaryByLanduse(cnBioEDB);
      YearlySummary.CalcSummaryByGenera(cnBioEDB);
      HourlySummary.CalcSummaryByLanduse(cnBioEDB);
      HourlySummary.CalcSummaryByGenera(cnBioEDB);
      HourlySummary.CalcDomainSummary(cnBioEDB);
    }

    public static void CalculateSummaryForLessMdbStorage(OleDbConnection cnBioEDB)
    {
      YearlySummary.CalcSummaryByLanduse(cnBioEDB);
      YearlySummary.CalcSummaryByGenera(cnBioEDB);
      HourlySummary.CalcSummaryByLanduseForLessMdbStorage(cnBioEDB);
      HourlySummary.CalcSummaryByGeneraForLessMdbStorage(cnBioEDB);
      HourlySummary.CalcSummaryBySpeciesForLessMdbStorage(cnBioEDB);
      HourlySummary.CalcDomainSummaryForLessMdbStorage(cnBioEDB);
    }

    [Obsolete]
    public static void CalculateSummary(
      City ctData,
      ref List<string> landuse,
      ref Dictionary<string, Landuse> luDict,
      ref List<string> genera,
      ref Dictionary<string, LeafBiomass> genDict,
      DryDeposition.VEG_TYPE veg,
      bool inv,
      string sDryDepDB,
      string sBioEDB)
    {
      using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDryDepDB))
      {
        using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDryDepDB))
        {
          cnDryDepDB.Open();
          cnBioEDB.Open();
          Summary.CalculateSummary(ctData, ref landuse, ref luDict, ref genera, ref genDict, veg, inv, cnDryDepDB, cnBioEDB);
        }
      }
    }

    public static void CalculateSummary(
      City ctData,
      ref List<string> landuse,
      ref Dictionary<string, Landuse> luDict,
      ref List<string> genera,
      ref Dictionary<string, LeafBiomass> genDict,
      DryDeposition.VEG_TYPE veg,
      bool inv,
      OleDbConnection cnDryDepDB,
      OleDbConnection cnBioEDB)
    {
      List<DryDeposition> dryDepositionList1 = new List<DryDeposition>();
      List<DryDeposition> dryDepositionList2 = new List<DryDeposition>();
      DryDeposition.ReadDryDeposition(cnDryDepDB, "CO", ref dryDepositionList1);
      DryDeposition.ReadDryDeposition(cnDryDepDB, "O3", ref dryDepositionList2);
      YearlySummary.CalcSummaryByLanduse(cnBioEDB);
      YearlyCOLanduseSummary.CalcSummaryByLanduse(ref landuse, ref luDict, ref dryDepositionList1, veg, inv, cnBioEDB);
      YearlyO3LanduseSummary.CalcSummaryByLanduse(ref landuse, ref luDict, ref dryDepositionList2, veg, inv, cnBioEDB);
      YearlySummary.CalcSummaryByGenera(cnBioEDB);
      YearlyCOGeneraSummary.CalcSummaryByGenera(ref genera, ref genDict, ref dryDepositionList1, inv, cnBioEDB);
      YearlyO3GeneraSummary.CalcSummaryByGenera(ref genera, ref genDict, ref dryDepositionList2, inv, cnBioEDB);
      LeafOnSummary.CalcSummaryByLanduse(cnBioEDB);
      LeafOnCOLanduseSummary.CalcSummaryByLanduse(ref landuse, ref luDict, ref dryDepositionList1, veg, inv, cnBioEDB);
      LeafOnO3LanduseSummary.CalcSummaryByLanduse(ref landuse, ref luDict, ref dryDepositionList2, veg, inv, cnBioEDB);
      LeafOnSummary.CalcSummaryByGenera(cnBioEDB);
      LeafOnCOGeneraSummary.CalcSummaryByGenera(ref genera, ref genDict, ref dryDepositionList1, inv, cnBioEDB);
      LeafOnO3GeneraSummary.CalcSummaryByGenera(ref genera, ref genDict, ref dryDepositionList2, inv, cnBioEDB);
      bool flag1 = false;
      MonthlySummary.CalcSummary(cnBioEDB, flag1);
      MonthlyCOSummary.CalcSummary(ctData, ref dryDepositionList1, veg, inv, cnBioEDB, flag1);
      MonthlyO3Summary.CalcSummary(ctData, ref dryDepositionList2, veg, inv, cnBioEDB, flag1);
      bool flag2 = true;
      MonthlySummary.CalcSummary(cnBioEDB, flag2);
      MonthlyCOSummary.CalcSummary(ctData, ref dryDepositionList1, veg, inv, cnBioEDB, flag2);
      MonthlyO3Summary.CalcSummary(ctData, ref dryDepositionList2, veg, inv, cnBioEDB, flag2);
      HourlySummary.CalcSummary(cnBioEDB);
      HourlyCOSummary.CalcSummary(ctData, ref dryDepositionList1, veg, inv, cnBioEDB);
      HourlyO3Summary.CalcSummary(ctData, ref dryDepositionList2, veg, inv, cnBioEDB);
    }
  }
}
