// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.CorrectionFactor
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Deposition;
using UFORE.LAI;

namespace UFORE.BioEmission
{
  public class CorrectionFactor
  {
    public const string CF_TABLE = "CorrectionFactor";
    public const string YR_CF_TABLE = "YearlyCorrectionFactor";
    public const string LFON_CF_TABLE = "LeafOnCorrectionFactor";
    public const string MN_CF_TABLE = "MonthlyCorrectionFactor";
    public const string MNDY_CF_TABLE = "MonthlyDaytimeCorrectionFactor";
    public const string HR_CF_TABLE = "HourlyCorrectionFactor";
    public const string HRAV_CF_TABLE = "HourlyAverageCorrectionFactor";
    private const double MONOBETA = 0.09;
    private const double M_TEMP = 314.0;
    private const double STD_TEMP = 303.0;
    private const double RUGC = 8.314;
    private const double CT_ONE = 95000.0;
    private const double CT_TWO = 230000.0;
    private const double GEUNTCON = 0.963649783;
    private const double STD_TRAN = 80.0;
    private const double STD_LAI = 6.0;
    private const double CONV_FAC = 0.036;
    private string Landuse;
    private string LanduseDesc;
    private double LanduseArea;
    private double ShrubPctCov;
    private double ShrubLAI;
    private double TreePctCov;
    private double TreeLAI;
    private DateTime TimeStamp;
    private bool GrowSeason;
    private double LAI;
    private double Trans;
    private double TempK;
    private double TempEfc;
    private double NewTemp;
    private double LtCrFctIspr;
    private double TmpCrFctIspr;
    private double TmpCrFctMono;

    [Obsolete]
    public static void ProcessCorrectionFactor(
      ref List<string> landuse,
      ref Dictionary<string, UFORE.BioEmission.Landuse> luDict,
      ref List<DryDeposition> ltCfList,
      double maxLai,
      ref List<LeafAreaIndex> laiList,
      DryDeposition.VEG_TYPE vegType,
      bool inv,
      string sBioEDB,
      UFORE_B uforeBObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        CorrectionFactor.ProcessCorrectionFactor(ref landuse, ref luDict, ref ltCfList, maxLai, ref laiList, vegType, inv, cnBioEDB, uforeBObj, PercentRangeFrom, PercentRangeTo);
      }
    }

    public static void ProcessCorrectionFactor(
      ref List<string> landuse,
      ref Dictionary<string, UFORE.BioEmission.Landuse> luDict,
      ref List<DryDeposition> ltCfList,
      double maxLai,
      ref List<LeafAreaIndex> laiList,
      DryDeposition.VEG_TYPE vegType,
      bool inv,
      OleDbConnection cnBioEDB,
      UFORE_B uforeBObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      CorrectionFactor.CreateCorrFactorTable(cnBioEDB);
      double num = (double) (PercentRangeTo - 1 - PercentRangeFrom) / (double) landuse.Count;
      for (int index1 = 0; index1 < landuse.Count; ++index1)
      {
        List<CorrectionFactor> cfList = new List<CorrectionFactor>();
        for (int index2 = 0; index2 < ltCfList.Count; ++index2)
        {
          UFORE.BioEmission.Landuse luData;
          luDict.TryGetValue(landuse[index1], out luData);
          CorrectionFactor correctionFactor = new CorrectionFactor();
          correctionFactor.CalcCorrectionFactor(luData, ltCfList[index2], maxLai, laiList[index2].Lai, vegType, inv);
          cfList.Add(correctionFactor);
        }
        CorrectionFactor.WriteCorrFactorRecords(cnBioEDB, ref cfList, uforeBObj, PercentRangeFrom + (int) ((double) index1 * num), PercentRangeFrom + (int) ((double) (index1 + 1) * num));
        uforeBObj.reportProgress(PercentRangeFrom + (int) ((double) (index1 + 1) * num));
      }
      CorrectionFactor.CalcYearlyCorrectionFactor(cnBioEDB, vegType, maxLai);
      uforeBObj.reportProgress(PercentRangeTo);
    }

    [Obsolete]
    public static void ProcessCorrectionFactor(
      ref List<DryDeposition> ltCfList,
      ref List<LeafAreaIndex> laiList,
      DryDeposition.VEG_TYPE veg,
      double trCovPct,
      double maxLai,
      bool inv,
      string sBioEDB)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        CorrectionFactor.ProcessCorrectionFactor(ref ltCfList, ref laiList, veg, trCovPct, maxLai, inv, cnBioEDB);
      }
    }

    public static void ProcessCorrectionFactor(
      ref List<DryDeposition> ltCfList,
      ref List<LeafAreaIndex> laiList,
      DryDeposition.VEG_TYPE veg,
      double trCovPct,
      double maxLai,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<CorrectionFactor> cfList = new List<CorrectionFactor>();
      Dictionary<string, CorrectionFactor> dictionary = new Dictionary<string, CorrectionFactor>();
      CorrectionFactor.CreateCorrFactorTable(cnBioEDB);
      for (int index = 0; index < ltCfList.Count; ++index)
      {
        CorrectionFactor correctionFactor = new CorrectionFactor();
        correctionFactor.CalcCorrectionFactor(ltCfList[index], laiList[index].Lai, trCovPct, inv);
        cfList.Add(correctionFactor);
      }
      CorrectionFactor.WriteCorrFactorRecords(cnBioEDB, ref cfList, (UFORE_B) null, 1, 100);
      CorrectionFactor.CalcYearlyCorrectionFactor(cnBioEDB, veg, maxLai);
    }

    private void CalcCorrectionFactor(
      UFORE.BioEmission.Landuse luData,
      DryDeposition dd,
      double maxLai,
      double varLai,
      DryDeposition.VEG_TYPE form,
      bool bInv)
    {
      this.TimeStamp = dd.TimeStamp;
      this.GrowSeason = dd.GrowSeason;
      this.Trans = dd.Trans;
      this.TempK = dd.TempK;
      this.LtCrFctIspr = dd.IsprLtCor;
      this.Landuse = luData.LanduseAbbr;
      this.LanduseDesc = luData.LanduseDesc;
      this.LanduseArea = luData.LanduseArea;
      this.ShrubPctCov = luData.ShrubPctCov;
      this.ShrubLAI = luData.ShrubLAI;
      this.TreePctCov = luData.TreePctCov;
      this.TreeLAI = luData.TreeLAI;
      double num1 = form == DryDeposition.VEG_TYPE.TREE ? luData.TreeLAI : luData.ShrubLAI;
      this.LAI = maxLai == 0.0 ? 0.0 : varLai * num1 / maxLai;
      double num2 = form == DryDeposition.VEG_TYPE.TREE ? luData.TreePctCov : luData.ShrubPctCov;
      this.TempEfc = bInv ? 0.0 : this.Trans / 80.0 * this.LAI / 6.0 * 0.036 * num2;
      this.NewTemp = this.TempK - this.TempEfc;
      this.TmpCrFctIspr = maxLai == 0.0 ? 0.0 : Math.Exp(95000.0 * (this.NewTemp - 303.0) / (2519.142 * this.NewTemp)) / (0.963649783 + Math.Exp(230000.0 * (this.NewTemp - 314.0) / (2519.142 * this.NewTemp)));
      this.TmpCrFctMono = maxLai == 0.0 ? 0.0 : Math.Exp(0.09 * (this.NewTemp - 303.0));
    }

    private void CalcCorrectionFactor(DryDeposition dd, double lai, double pctCov, bool bInv)
    {
      this.TimeStamp = dd.TimeStamp;
      this.GrowSeason = dd.GrowSeason;
      this.Trans = dd.Trans;
      this.TempK = dd.TempK;
      this.LtCrFctIspr = dd.IsprLtCor;
      this.LAI = lai;
      this.TempEfc = bInv ? 0.0 : this.Trans / 80.0 * lai / 6.0 * 0.036 * pctCov;
      this.NewTemp = this.TempK - this.TempEfc;
      this.TmpCrFctIspr = Math.Exp(95000.0 * (this.NewTemp - 303.0) / (2519.142 * this.NewTemp)) / (0.963649783 + Math.Exp(230000.0 * (this.NewTemp - 314.0) / (2519.142 * this.NewTemp)));
      this.TmpCrFctMono = Math.Exp(0.09 * (this.NewTemp - 303.0));
    }

    private static void CreateCorrFactorTable(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE CorrectionFactor([Landuse] TEXT (10),[LanduseDesc] TEXT (50),[LanduseArea] DOUBLE,[ShrubPctCov] DOUBLE,[ShrubLAI] DOUBLE,[TreePctCov] DOUBLE,[TreeLAI] DOUBLE,[TimeStamp] DateTime,[GrowSeason] BIT,[LAI] DOUBLE,[Trans] DOUBLE,[TempK] DOUBLE,[TempEfc] DOUBLE,[NewTemp] DOUBLE,[LtCrFctIspr] DOUBLE,[TmpCrFctIspr] DOUBLE,[TmpCrFctMono] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void WriteCorrFactorRecords(
      OleDbConnection cnBioEDB,
      ref List<CorrectionFactor> cfList,
      UFORE_B uforeBObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        double num = (double) (PercentRangeTo - PercentRangeFrom) / (double) cfList.Count;
        for (int index = 0; index < cfList.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO CorrectionFactor([Landuse],[LanduseDesc],[LanduseArea],[ShrubPctCov],[ShrubLAI],[TreePctCov],[TreeLAI],[TimeStamp],[GrowSeason],[LAI],[Trans],[TempK],[TempEfc],[NewTemp],[LtCrFctIspr],[TmpCrFctIspr],[TmpCrFctMono]) Values (\"" + cfList[index].Landuse + "\",\"" + cfList[index].LanduseDesc + "\"," + cfList[index].LanduseArea.ToString() + "," + cfList[index].ShrubPctCov.ToString() + "," + cfList[index].ShrubLAI.ToString() + "," + cfList[index].TreePctCov.ToString() + "," + cfList[index].TreeLAI.ToString() + ",#" + cfList[index].TimeStamp.ToString() + "#," + cfList[index].GrowSeason.ToString() + "," + cfList[index].LAI.ToString() + "," + cfList[index].Trans.ToString() + "," + cfList[index].TempK.ToString() + "," + cfList[index].TempEfc.ToString() + "," + cfList[index].NewTemp.ToString() + "," + cfList[index].LtCrFctIspr.ToString() + "," + cfList[index].TmpCrFctIspr.ToString() + "," + cfList[index].TmpCrFctMono.ToString() + ");";
          oleDbCommand.ExecuteNonQuery();
          uforeBObj?.reportProgress(PercentRangeFrom + (int) ((double) (index + 1) * num));
        }
      }
    }

    private static void CalcYearlyCorrectionFactor(
      OleDbConnection cnBioEDB,
      DryDeposition.VEG_TYPE veg,
      double maxLai)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        if (veg == DryDeposition.VEG_TYPE.TREE)
        {
          if (maxLai == 0.0)
          {
            oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono) AS CorrFactor, false AS Deciduous,\"Monoterpene\" AS Pollutant, TreeLAI AS LAI INTO YearlyCorrectionFactor FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, false, \"Monoterpene\", TreeLAI;";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([LtCrFctIspr]*[TmpCrFctIspr]), false, \"Isoprene\", TreeLAI FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, false, \"Isoprene\", TreeLAI;";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([TmpCrFctMono]), false, \"OVOC\", TreeLAI FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, false, \"OVOC\", TreeLAI;";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono), true, \"Monoterpene\", TreeLAI FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, true, \"Monoterpene\", TreeLAI;";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([LtCrFctIspr]*[TmpCrFctIspr]), true, \"Isoprene\", TreeLAI FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, true, \"Isoprene\", TreeLAI;";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono), true, \"OVOC\", TreeLAI FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, true, \"OVOC\", TreeLAI;";
            oleDbCommand.ExecuteNonQuery();
          }
          else
          {
            oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono) AS CorrFactor, false AS Deciduous,\"Monoterpene\" AS Pollutant, TreeLAI AS LAI INTO YearlyCorrectionFactor FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, false, \"Monoterpene\", TreeLAI;";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([LtCrFctIspr]*[TmpCrFctIspr]), false, \"Isoprene\", TreeLAI FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, false, \"Isoprene\", TreeLAI;";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([TmpCrFctMono]), false, \"OVOC\", TreeLAI FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, false, \"OVOC\", TreeLAI;";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono), true, \"Monoterpene\", TreeLAI FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, true, \"Monoterpene\", TreeLAI;";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([LtCrFctIspr]*[TmpCrFctIspr]), true, \"Isoprene\", TreeLAI FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, true, \"Isoprene\", TreeLAI;";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono), true, \"OVOC\", TreeLAI FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, true, \"OVOC\", TreeLAI;";
            oleDbCommand.ExecuteNonQuery();
          }
        }
        else if (maxLai == 0.0)
        {
          oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono) AS CorrFactor, false AS Deciduous,\"Monoterpene\" AS Pollutant, ShrubLAI AS LAI INTO YearlyCorrectionFactor FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, false, \"Monoterpene\", ShrubLAI;";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([LtCrFctIspr]*[TmpCrFctIspr]), false, \"Isoprene\", ShrubLAI FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, false, \"Isoprene\", ShrubLAI;";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([TmpCrFctMono]), false, \"OVOC\", ShrubLAI FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, false, \"OVOC\", ShrubLAI;";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono), true, \"Monoterpene\", ShrubLAI FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, true, \"Monoterpene\", ShrubLAI;";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([LtCrFctIspr]*[TmpCrFctIspr]), true, \"Isoprene\", ShrubLAI FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, true, \"Isoprene\", ShrubLAI;";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono), true, \"OVOC\", ShrubLAI FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, true, \"OVOC\", ShrubLAI;";
          oleDbCommand.ExecuteNonQuery();
        }
        else
        {
          oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono) AS CorrFactor, false AS Deciduous,\"Monoterpene\" AS Pollutant, ShrubLAI AS LAI INTO YearlyCorrectionFactor FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, false, \"Monoterpene\", ShrubLAI;";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([LtCrFctIspr]*[TmpCrFctIspr]), false, \"Isoprene\", ShrubLAI FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, false, \"Isoprene\", ShrubLAI;";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([TmpCrFctMono]), false, \"OVOC\", ShrubLAI FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, false, \"OVOC\", ShrubLAI;";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono), true, \"Monoterpene\", ShrubLAI FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, true, \"Monoterpene\", ShrubLAI;";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([LtCrFctIspr]*[TmpCrFctIspr]), true, \"Isoprene\", ShrubLAI FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, true, \"Isoprene\", ShrubLAI;";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, LAI ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono), true, \"OVOC\", ShrubLAI FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, true, \"OVOC\", ShrubLAI;";
          oleDbCommand.ExecuteNonQuery();
        }
        oleDbCommand.CommandText = "INSERT INTO YearlyCorrectionFactor (Landuse,LanduseDesc,CorrFactor,Deciduous,Pollutant,LAI)  SELECT YearlyCorrectionFactor.Landuse, YearlyCorrectionFactor.LanduseDesc, 0, YearlyCorrectionFactor.Deciduous, \"Isoprene\", YearlyCorrectionFactor.LAI  FROM YearlyCorrectionFactor LEFT JOIN YearlyCorrectionFactor AS YearlyCorrectionFactor_1 ON  (YearlyCorrectionFactor.Deciduous = YearlyCorrectionFactor_1.Deciduous) AND  (YearlyCorrectionFactor.LanduseDesc = YearlyCorrectionFactor_1.LanduseDesc) AND  (YearlyCorrectionFactor.Landuse = YearlyCorrectionFactor_1.Landuse)  AND ((YearlyCorrectionFactor.Pollutant=\"Monoterpene\") AND (YearlyCorrectionFactor_1.Pollutant=\"Isoprene\"))  WHERE isnull(YearlyCorrectionFactor_1.Landuse)=true";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcLeafOnCorrectionFactor(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono) AS CorrFactor, \"Monoterpene\" AS Pollutant INTO LeafOnCorrectionFactor FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO LeafOnCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([LtCrFctIspr]*[TmpCrFctIspr]), \"Isoprene\" FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO LeafOnCorrectionFactor ( Landuse, LanduseDesc, CorrFactor, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([TmpCrFctMono]), \"OVOC\" FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcMonthlyCorrectionFactor(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], Sum(TmpCrFctMono) AS CorrFactor,false AS Deciduous, \"Monoterpene\" AS Pollutant INTO MonthlyCorrectionFactor FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), false, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Pollutant, Deciduous ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum([LtCrFctIspr]*[TmpCrFctIspr]), \"Isoprene\", false FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), false, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Pollutant, Deciduous ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum([TmpCrFctMono]), \"OVOC\", false FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), false, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Pollutant, Deciduous ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum(TmpCrFctMono), \"Monoterpene\", true FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], 0 AS CorrFactor, \"Monoterpene\" AS Pollutant, true AS Deciduous INTO win1 FROM CorrectionFactor WHERE GrowSeason=False GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Monoterpene\"";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT win1.Landuse, win1.LanduseDesc, win1.Month, win1.CorrFactor, win1.Pollutant, win1.Deciduous INTO win2 FROM win1 LEFT JOIN  MonthlyCorrectionFactor AS MON ON (win1.Deciduous = MON.Deciduous) AND (win1.Pollutant = MON.Pollutant) AND (win1.LanduseDesc = MON.LanduseDesc) AND (win1.Landuse = MON.Landuse) AND (win1.[Month] = MON.[Month]) WHERE MON.Month Is Null;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Month, CorrFactor, Deciduous, Pollutant FROM win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum([LtCrFctIspr]*[TmpCrFctIspr]), true, \"Isoprene\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], 0 AS CorrFactor, \"Isoprene\" AS Pollutant, true AS Deciduous INTO win1 FROM CorrectionFactor WHERE GrowSeason=False GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT win1.Landuse, win1.LanduseDesc, win1.Month, win1.CorrFactor, win1.Pollutant, win1.Deciduous INTO win2 FROM win1 LEFT JOIN  MonthlyCorrectionFactor AS MON ON (win1.Deciduous = MON.Deciduous) AND (win1.Pollutant = MON.Pollutant) AND (win1.LanduseDesc = MON.LanduseDesc) AND (win1.Landuse = MON.Landuse) AND (win1.[Month] = MON.[Month]) WHERE MON.Month Is Null;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Month, CorrFactor, Deciduous, Pollutant FROM win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum(TmpCrFctMono), true, \"OVOC\" FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], 0 AS CorrFactor, true AS Deciduous, \"OVOC\" AS Pollutant INTO win1 FROM CorrectionFactor WHERE GrowSeason=False GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT win1.Landuse, win1.LanduseDesc, win1.Month, win1.CorrFactor, win1.Pollutant, win1.Deciduous INTO win2 FROM win1 LEFT JOIN  MonthlyCorrectionFactor AS MON ON (win1.Deciduous = MON.Deciduous) AND (win1.Pollutant = MON.Pollutant) AND (win1.LanduseDesc = MON.LanduseDesc) AND (win1.Landuse = MON.Landuse) AND (win1.[Month] = MON.[Month]) WHERE MON.Month Is Null;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Month, CorrFactor, Deciduous, Pollutant FROM win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win2;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcMonthlyDaytimeCorrectionFactor(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], Sum(TmpCrFctMono) AS CorrFactor,false AS Deciduous, \"Monoterpene\" AS Pollutant INTO MonthlyDaytimeCorrectionFactor FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), false, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum([LtCrFctIspr]*[TmpCrFctIspr]), false, \"Isoprene\" FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), false, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum([TmpCrFctMono]), false, \"OVOC\" FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), false, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum(TmpCrFctMono), true, \"Monoterpene\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], 0 AS CorrFactor, true AS Deciduous, \"Monoterpene\" AS Pollutant INTO win1 FROM CorrectionFactor WHERE GrowSeason=False GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Monoterpene\"";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT win1.Landuse, win1.LanduseDesc, win1.Month, win1.CorrFactor, win1.Pollutant, win1.Deciduous INTO win2 FROM win1 LEFT JOIN  MonthlyDaytimeCorrectionFactor AS MON ON (win1.Deciduous = MON.Deciduous) AND (win1.Pollutant = MON.Pollutant) AND (win1.LanduseDesc = MON.LanduseDesc) AND (win1.Landuse = MON.Landuse) AND (win1.[Month] = MON.[Month]) WHERE MON.Month Is Null;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Month, CorrFactor, Deciduous, Pollutant FROM win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum([LtCrFctIspr]*[TmpCrFctIspr]), true, \"Isoprene\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], 0 AS CorrFactor, true AS Deciduous, \"Isoprene\" AS Pollutant INTO win1 FROM CorrectionFactor WHERE GrowSeason=False GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT win1.Landuse, win1.LanduseDesc, win1.Month, win1.CorrFactor, win1.Pollutant, win1.Deciduous INTO win2 FROM win1 LEFT JOIN  MonthlyDaytimeCorrectionFactor AS MON ON (win1.Deciduous = MON.Deciduous) AND (win1.Pollutant = MON.Pollutant) AND (win1.LanduseDesc = MON.LanduseDesc) AND (win1.Landuse = MON.Landuse) AND (win1.[Month] = MON.[Month]) WHERE MON.Month Is Null;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Month, CorrFactor, Deciduous, Pollutant FROM win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum(TmpCrFctMono), true, \"OVOC\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], 0 AS CorrFactor, true AS Deciduous, \"OVOC\" AS Pollutant INTO win1 FROM CorrectionFactor WHERE GrowSeason=False GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT win1.Landuse, win1.LanduseDesc, win1.Month, win1.CorrFactor, win1.Pollutant, win1.Deciduous INTO win2 FROM win1 LEFT JOIN  MonthlyDaytimeCorrectionFactor AS MON ON (win1.Deciduous = MON.Deciduous) AND (win1.Pollutant = MON.Pollutant) AND (win1.LanduseDesc = MON.LanduseDesc) AND (win1.Landuse = MON.Landuse) AND (win1.[Month] = MON.[Month]) WHERE MON.Month Is Null;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeCorrectionFactor ( Landuse, LanduseDesc, [Month], CorrFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Month, CorrFactor, Deciduous, Pollutant FROM win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win2;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcHourlyAverageCorrectionFactor(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour], Avg(TmpCrFctMono)  AS CorrFactor, False AS Deciduous, \"Monoterpene\" AS Pollutant INTO HourlyAverageCorrectionFactor FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), False, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyAverageCorrectionFactor ( Landuse, LanduseDesc, [Hour], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour], Avg([LtCrFctIspr]*[TmpCrFctIspr]) AS CorrFactor, False AS Deciduous, \"Isoprene\" AS Pollutant FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), False, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyAverageCorrectionFactor ( Landuse, LanduseDesc, [Hour], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour], Avg(TmpCrFctMono)  AS CorrFactor, False AS Deciduous, \"OVOC\" AS Pollutant FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), False, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyAverageCorrectionFactor ( Landuse, LanduseDesc, [Hour], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour], Avg(TmpCrFctMono)  AS CorrFactor, True AS Deciduous, \"Monoterpene\" AS Pollutant FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), True, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyAverageCorrectionFactor ( Landuse, LanduseDesc, [Hour], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour], Avg([LtCrFctIspr]*[TmpCrFctIspr])  AS CorrFactor, True AS Deciduous, \"Isoprene\" AS Pollutant FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), True, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyAverageCorrectionFactor ( Landuse, LanduseDesc, [Hour], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour],  Avg(TmpCrFctMono)*1.189 AS CorrFactor, True AS Deciduous, \"OVOC\" AS Pollutant FROM CorrectionFactor WHERE GrowSeason=True GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), True, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcHourlyCorrectionFactor(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, [TimeStamp], TmpCrFctMono AS CorrFactor, False AS Deciduous, \"Monoterpene\" AS Pollutant INTO HourlyCorrectionFactor FROM CorrectionFactor;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyCorrectionFactor ( Landuse, LanduseDesc, [TimeStamp], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, [TimeStamp], [LtCrFctIspr]*[TmpCrFctIspr] AS CorrFactor, False AS Deciduous, \"Isoprene\" AS Pollutant FROM CorrectionFactor;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyCorrectionFactor ( Landuse, LanduseDesc, [TimeStamp], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, TimeStamp, TmpCrFctMono  AS CorrFactor, False AS Deciduous, \"OVOC\" AS Pollutant FROM CorrectionFactor;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyCorrectionFactor ( Landuse, LanduseDesc, [TimeStamp], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, TimeStamp, TmpCrFctMono AS CorrFactor, True AS Deciduous, \"Monoterpene\" AS Pollutant FROM CorrectionFactor WHERE GrowSeason=True;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyCorrectionFactor ( Landuse, LanduseDesc, [TimeStamp], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, TimeStamp, [LtCrFctIspr]*[TmpCrFctIspr] AS CorrFactor, True AS Deciduous, \"Isoprene\" AS Pollutant FROM CorrectionFactor WHERE GrowSeason=True;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyCorrectionFactor ( Landuse, LanduseDesc, [TimeStamp], CorrFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, TimeStamp,  TmpCrFctMono*1.189 AS CorrFactor, True AS Deciduous, \"OVOC\" AS Pollutant FROM CorrectionFactor WHERE GrowSeason=True;";
        oleDbCommand.ExecuteNonQuery();
      }
    }
  }
}
