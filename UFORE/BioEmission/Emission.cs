// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.Emission
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace UFORE.BioEmission
{
  public class Emission
  {
    public const double CTOISO = 1.135;
    public const double CTOMONO = 1.135;
    public const double CTOOVOC = 1.189;
    public const string YEAR_EMIS_TABLE = "YearlyEmission";
    public const string LFON_EMIS_TABLE = "LeafOnEmission";
    public const string MONTH_EMIS_TABLE = "MonthlyEmission";
    public const string MONTH_DAY_EMIS_TABLE = "MonthlyDaytimeEmission";
    public const string HOUR_EMIS_TABLE = "HourlyEmission";
    public const string HOUR_EMIS_LANDUSE_TABLE = "HourlyEmissionByLanduse";
    public const string HOUR_EMIS_SPECIES_TABLE = "HourlyEmissionBySpecies";
    public const string BASE_EMIS_RATE_TABLE = "BASEEMISSION";
    public const string ACE_LBIOMASS_TABLE = "LBIOMASS";
    private const double KG_TO_G = 1000.0;
    private const double UG_TO_KG = 1E-09;
    private const double UG_TO_G = 1E-06;
    private string Landuse;
    private string LanduseDesc;
    private string Genera;
    private DateTime TimeStamp;
    private int Month;
    private int Hour;
    private bool Tree;
    private bool Deciduous;
    private double EmitIso;
    private double EmitMono;
    private double EmitOvoc;

    [Obsolete]
    public static void ProcessBioEmission(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      string sBioEDB)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Emission.ProcessBioEmission(ref lbList, ref beDict, cnBioEDB);
      }
    }

    public static void ProcessBioEmission(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      OleDbConnection cnBioEDB)
    {
      Emission.ProcessYearlyEmission(ref lbList, ref beDict, cnBioEDB);
      Emission.ProcessHourlyEmissionForLessMDBStorage(ref lbList, ref beDict, cnBioEDB);
    }

    private static void ProcessYearlyEmission(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      OleDbConnection cnBioEDB)
    {
      List<Emission> list = new List<Emission>();
      for (int index = 0; index < lbList.Count; ++index)
      {
        Emission emission = new Emission();
        BaseEmission baseEmission = (BaseEmission) null;
        beDict.TryGetValue(lbList[index].Genera, out baseEmission);
        if (baseEmission == null)
          throw new Exception("Genera '" + lbList[index].Genera + "' seems not having base emission. It is surposed to use its parent's or class base emission. Double check the code");
        emission.CalcYearlyEmission(lbList[index], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, cnBioEDB);
        list.Add(emission);
      }
      Emission.CreateYearlyEmissionTable(cnBioEDB);
      Emission.WriteYearlyEmissionRecords(cnBioEDB, ref list);
    }

    private void CalcYearlyEmission(
      LeafBiomass lb,
      double beIso,
      double beMono,
      double beOvoc,
      OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        this.Landuse = lb.Landuse;
        this.Genera = lb.Genera;
        this.Tree = lb.Tree;
        this.Deciduous = lb.Deciduous;
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant FROM YearlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Isoprene\";";
        double num;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LanduseDesc = oleDbDataReader.GetString(1);
          num = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitIso = beIso * lb.TotalLeafBiomass * 1000.0 * num * 1.135 * 1E-06;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant FROM YearlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Monoterpene\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitMono = beMono * lb.TotalLeafBiomass * 1000.0 * num * 1.135 * 1E-06;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant FROM YearlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"OVOC\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitOvoc = beOvoc * lb.TotalLeafBiomass * 1000.0 * num * 1.189 * 1E-06;
      }
    }

    private static void CreateYearlyEmissionTable(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE YearlyEmission ([ID] INT,[Landuse] TEXT (10),[LanduseDesc] TEXT (30),[Genera] TEXT (30),[Tree] BIT,[Deciduous] BIT,[Isoprene emitted] DOUBLE,[Monoterpene emitted] DOUBLE,[Other VOCs emitted] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void WriteYearlyEmissionRecords(
      OleDbConnection cnBioEDB,
      ref List<Emission> list)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        for (int index = 0; index < list.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO YearlyEmission ([ID],[Landuse],[LanduseDesc],[Genera],[Tree],[Deciduous],[Isoprene emitted],[Monoterpene emitted],[Other VOCs emitted]) Values (" + index.ToString() + ",\"" + list[index].Landuse + "\",\"" + list[index].LanduseDesc + "\",\"" + list[index].Genera + "\"," + list[index].Tree.ToString() + "," + list[index].Deciduous.ToString() + "," + list[index].EmitIso.ToString() + "," + list[index].EmitMono.ToString() + "," + list[index].EmitOvoc.ToString() + ")";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    private static void ProcessLeafOnEmission(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Emission> list = new List<Emission>();
      for (int index = 0; index < lbList.Count; ++index)
      {
        Emission emission = new Emission();
        BaseEmission baseEmission = (BaseEmission) null;
        beDict.TryGetValue(lbList[index].Genera, out baseEmission);
        if (baseEmission == null)
          throw new Exception("Genera '" + lbList[index].Genera + "' seems not having base emission. It is surposed to use class base emission. Double check the code");
        emission.CalcLeafOnEmission(lbList[index], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, inv, cnBioEDB);
        list.Add(emission);
      }
      Emission.CreateLeafOnEmissionTable(cnBioEDB);
      Emission.WriteLeafOnEmissionRecords(cnBioEDB, ref list);
    }

    private void CalcLeafOnEmission(
      LeafBiomass lb,
      double beIso,
      double beMono,
      double beOvoc,
      bool bInv,
      OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        double num1 = !bInv ? 1E-09 : 1E-06;
        this.Landuse = lb.Landuse;
        this.Genera = lb.Genera;
        this.Tree = lb.Tree;
        this.Deciduous = lb.Deciduous;
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor,  Pollutant FROM LeafOnCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Pollutant=\"Isoprene\";";
        double num2;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LanduseDesc = oleDbDataReader.GetString(1);
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitIso = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Pollutant FROM LeafOnCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Pollutant=\"Monoterpene\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitMono = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Pollutant FROM LeafOnCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Pollutant=\"OVOC\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitOvoc = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 1.189 * num1;
      }
    }

    private static void CreateLeafOnEmissionTable(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE LeafOnEmission ([ID] INT,[Landuse] TEXT (10),[LanduseDesc] TEXT (30),[Genera] TEXT (30),[Tree] BIT,[Deciduous] BIT,[Isoprene emitted] DOUBLE,[Monoterpene emitted] DOUBLE,[Other VOCs emitted] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void WriteLeafOnEmissionRecords(
      OleDbConnection cnBioEDB,
      ref List<Emission> list)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        for (int index = 0; index < list.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO LeafOnEmission ([ID],[Landuse],[LanduseDesc],[Genera],[Tree],[Deciduous],[Isoprene emitted],[Monoterpene emitted],[Other VOCs emitted]) Values (" + index.ToString() + ",\"" + list[index].Landuse + "\",\"" + list[index].LanduseDesc + "\",\"" + list[index].Genera + "\"," + list[index].Tree.ToString() + "," + list[index].Deciduous.ToString() + "," + list[index].EmitIso.ToString() + "," + list[index].EmitMono.ToString() + "," + list[index].EmitOvoc.ToString() + ")";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    private static void ProcessMonthlyEmission(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Emission> list = new List<Emission>();
      bool daytime = false;
      for (int index1 = 0; index1 < lbList.Count; ++index1)
      {
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index1].Genera, out baseEmission);
        for (int index2 = 0; index2 < 12; ++index2)
        {
          Emission emission = new Emission();
          baseEmission = (BaseEmission) null;
          if (baseEmission == null)
            throw new Exception("Genera '" + lbList[index1].Genera + "' seems not having base emission. It is surposed to use class base emission. Double check the code");
          emission.CalcMonthlyEmission(lbList[index1], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, index2 + 1, inv, cnBioEDB);
          list.Add(emission);
        }
      }
      Emission.CreateMonthlyEmissionTable(cnBioEDB, daytime);
      Emission.WriteMonthlyEmissionRecords(cnBioEDB, ref list, daytime);
    }

    private static void ProcessMonthlyDaytimeEmission(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Emission> list = new List<Emission>();
      bool daytime = true;
      for (int index1 = 0; index1 < lbList.Count; ++index1)
      {
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index1].Genera, out baseEmission);
        for (int index2 = 0; index2 < 12; ++index2)
        {
          Emission emission = new Emission();
          baseEmission = (BaseEmission) null;
          if (baseEmission == null)
            throw new Exception("Genera '" + lbList[index1].Genera + "' seems not having base emission. It is surposed to use class base emission. Double check the code");
          emission.CalcMonthlyDaytimeEmission(lbList[index1], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, index2 + 1, inv, cnBioEDB);
          list.Add(emission);
        }
      }
      Emission.CreateMonthlyEmissionTable(cnBioEDB, daytime);
      Emission.WriteMonthlyEmissionRecords(cnBioEDB, ref list, daytime);
    }

    private void CalcMonthlyEmission(
      LeafBiomass lb,
      double beIso,
      double beMono,
      double beOvoc,
      int mon,
      bool bInv,
      OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        double num1 = !bInv ? 1E-09 : 1E-06;
        this.Landuse = lb.Landuse;
        this.Genera = lb.Genera;
        this.Month = mon;
        this.Tree = lb.Tree;
        this.Deciduous = lb.Deciduous;
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Month FROM MonthlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Isoprene\" AND Month=" + this.Month.ToString() + ";";
        double num2;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LanduseDesc = oleDbDataReader.GetString(1);
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitIso = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Month FROM MonthlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Monoterpene\" AND Month=" + this.Month.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitMono = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Month FROM MonthlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"OVOC\" AND Month=" + this.Month.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitOvoc = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 1.189 * num1;
      }
    }

    private void CalcMonthlyDaytimeEmission(
      LeafBiomass lb,
      double beIso,
      double beMono,
      double beOvoc,
      int mon,
      bool bInv,
      OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        double num1 = !bInv ? 1E-09 : 1E-06;
        this.Landuse = lb.Landuse;
        this.Genera = lb.Genera;
        this.Month = mon;
        this.Tree = lb.Tree;
        this.Deciduous = lb.Deciduous;
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Month FROM MonthlyDaytimeCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Isoprene\" AND Month=" + this.Month.ToString() + ";";
        double num2;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LanduseDesc = oleDbDataReader.GetString(1);
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitIso = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Month FROM MonthlyDaytimeCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Monoterpene\" AND Month=" + this.Month.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitMono = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Month FROM MonthlyDaytimeCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"OVOC\" AND Month=" + this.Month.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitOvoc = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 1.189 * num1;
      }
    }

    private static void CreateMonthlyEmissionTable(OleDbConnection cnBioEDB, bool daytime)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = daytime ? "MonthlyDaytimeEmission" : "MonthlyEmission";
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE " + str + " ([ID] INT,[Landuse] TEXT (10),[LanduseDesc] TEXT (30),[Genera] TEXT (30),[Month] INT,[Tree] BIT,[Deciduous] BIT,[Isoprene emitted] DOUBLE,[Monoterpene emitted] DOUBLE,[Other VOCs emitted] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void WriteMonthlyEmissionRecords(
      OleDbConnection cnBioEDB,
      ref List<Emission> list,
      bool daytime)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = daytime ? "MonthlyDaytimeEmission" : "MonthlyEmission";
        oleDbCommand.Connection = cnBioEDB;
        for (int index = 0; index < list.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO " + str + " ([ID],[Landuse],[LanduseDesc],[Genera],[Month],[Tree],[Deciduous],[Isoprene emitted],[Monoterpene emitted],[Other VOCs emitted]) Values (" + index.ToString() + ",\"" + list[index].Landuse + "\",\"" + list[index].LanduseDesc + "\",\"" + list[index].Genera + "\"," + list[index].Month.ToString() + "," + list[index].Tree.ToString() + "," + list[index].Deciduous.ToString() + "," + list[index].EmitIso.ToString() + "," + list[index].EmitMono.ToString() + "," + list[index].EmitOvoc.ToString() + ")";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    private static void GetTimeStamp(OleDbConnection cn, ref List<DateTime> dtList)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW TimeStamp FROM CorrectionFactor GROUP BY TimeStamp ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            DateTime dateTime = oleDbDataReader.GetDateTime(0);
            dtList.Add(dateTime);
          }
          oleDbDataReader.Close();
        }
      }
    }

    private static void ProcessHourlyEmissionForLessMDBStorage(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      OleDbConnection cnBioEDB)
    {
      Emission.CreateHourlyEmissionTableForLessMDBStorage(cnBioEDB);
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "INSERT INTO [BASEEMISSION] (Genera,BaseIsoprene,BaseMonoterpene,BaseOtherVOC)  VALUES (@genera, @iso, @mono, @ovoc)";
        oleDbCommand.Parameters.Clear();
        oleDbCommand.Parameters.Add("@genera", OleDbType.VarWChar);
        oleDbCommand.Parameters.Add("@iso", OleDbType.Double);
        oleDbCommand.Parameters.Add("@mono", OleDbType.Double);
        oleDbCommand.Parameters.Add("@ovoc", OleDbType.Double);
        Dictionary<string, int> dictionary = new Dictionary<string, int>();
        for (int index = 0; index < lbList.Count; ++index)
        {
          if (!dictionary.ContainsKey(lbList[index].Genera))
          {
            BaseEmission baseEmission = beDict[lbList[index].Genera];
            oleDbCommand.Parameters["@genera"].Value = (object) lbList[index].Genera;
            oleDbCommand.Parameters["@iso"].Value = (object) baseEmission.BaseIsoprene;
            oleDbCommand.Parameters["@mono"].Value = (object) baseEmission.BaseMonoterpene;
            oleDbCommand.Parameters["@ovoc"].Value = (object) baseEmission.BaseOtherVOC;
            oleDbCommand.ExecuteNonQuery();
            dictionary.Add(lbList[index].Genera, 1);
          }
        }
        oleDbCommand.CommandText = "CREATE INDEX [landuseDescTimeStamp] ON CorrectionFactor([LANDUSE],[LANDUSEDESC],[TimeStamp])";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "CREATE INDEX [landuseTimeStamp] ON CorrectionFactor([LANDUSE],[TimeStamp])";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyEmissionByLanduse ( Landuse, LanduseDesc, [TimeStamp], Isoprene, Monoterpene )  SELECT [CorrectionFactor].[Landuse], [CorrectionFactor].[LanduseDesc], [CorrectionFactor].[TimeStamp],  SUM(iif([CorrectionFactor].GrowSeason=false AND [LBIOMASS].[DECIDUOUS]=True,0,[BASEEMISSION].[BaseIsoprene]*[LBIOMASS].[TotalLeafBiomass]* @kg_to_g *[CorrectionFactor].[LtCrFctIspr]*[CorrectionFactor].[TmpCrFctIspr]* @ctoiso * @unit)) AS Isoprene,  SUM(iif([CorrectionFactor].GrowSeason=false AND [LBIOMASS].[DECIDUOUS]=True,0,[BASEEMISSION].BaseMonoterpene*[LBIOMASS].[TotalLeafBiomass]* @kg_to_g *[CorrectionFactor].[TmpCrFctMono]* @ctomono * @unit) ) AS Monoterpene  FROM ([CorrectionFactor] INNER JOIN [LBIOMASS] ON [CorrectionFactor].[LANDUSE] = [LBIOMASS].[Landuse]) INNER JOIN [BASEEMISSION] ON [LBIOMASS].[GENUS] = [BASEEMISSION].[Genera]  GROUP BY [CorrectionFactor].[Landuse], [CorrectionFactor].[LanduseDesc], [CorrectionFactor].[TimeStamp] ";
        oleDbCommand.Parameters.Clear();
        oleDbCommand.Parameters.Add("@kg_to_g", OleDbType.Double);
        oleDbCommand.Parameters.Add("@ctoiso", OleDbType.Double);
        oleDbCommand.Parameters.Add("@unit", OleDbType.Double);
        oleDbCommand.Parameters.Add("@ctomono", OleDbType.Double);
        oleDbCommand.Parameters["@kg_to_g"].Value = (object) 1000.0;
        oleDbCommand.Parameters["@ctoiso"].Value = (object) 1.135;
        oleDbCommand.Parameters["@unit"].Value = (object) 1E-06;
        oleDbCommand.Parameters["@ctomono"].Value = (object) 1.135;
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyEmissionBySpecies ( Code, Genera, [TimeStamp], Isoprene, Monoterpene )  SELECT [LBIOMASS].[CODE], [LBIOMASS].[GENUS], [CorrectionFactor].[TimeStamp],  SUM(iif([CorrectionFactor].GrowSeason=false AND [LBIOMASS].[DECIDUOUS]=True,0,[BASEEMISSION].[BaseIsoprene]*[LBIOMASS].[TotalLeafBiomass]* @kg_to_g *[CorrectionFactor].[LtCrFctIspr]*[CorrectionFactor].[TmpCrFctIspr]* @ctoiso * @unit)) AS Isoprene,  SUM(iif([CorrectionFactor].GrowSeason=false AND [LBIOMASS].[DECIDUOUS]=True,0,[BASEEMISSION].BaseMonoterpene*[LBIOMASS].[TotalLeafBiomass]* @kg_to_g *[CorrectionFactor].[TmpCrFctMono]* @ctomono * @unit )) AS Monoterpene  FROM ([LBIOMASS] INNER JOIN [CorrectionFactor] ON [LBIOMASS].[LANDUSE] = [CorrectionFactor].[Landuse]) INNER JOIN [BASEEMISSION] ON [LBIOMASS].[GENUS] = [BASEEMISSION].[Genera]  GROUP BY [LBIOMASS].[CODE], [LBIOMASS].[GENUS], [CorrectionFactor].[TimeStamp] ";
        oleDbCommand.Parameters.Clear();
        oleDbCommand.Parameters.Add("@kg_to_g", OleDbType.Double);
        oleDbCommand.Parameters.Add("@ctoiso", OleDbType.Double);
        oleDbCommand.Parameters.Add("@unit", OleDbType.Double);
        oleDbCommand.Parameters.Add("@ctomono", OleDbType.Double);
        oleDbCommand.Parameters["@kg_to_g"].Value = (object) 1000.0;
        oleDbCommand.Parameters["@ctoiso"].Value = (object) 1.135;
        oleDbCommand.Parameters["@unit"].Value = (object) 1E-06;
        oleDbCommand.Parameters["@ctomono"].Value = (object) 1.135;
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void ProcessHourlyEmission(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      OleDbConnection cnBioEDB)
    {
      List<Emission> emissionList = new List<Emission>();
      Emission.CreateHourlyEmissionTable(cnBioEDB);
      for (int index = 0; index < lbList.Count; ++index)
      {
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index].Genera, out baseEmission);
        new Emission().CalcHourlyEmission(lbList[index], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, cnBioEDB);
      }
    }

    private void CalcHourlyEmission(
      LeafBiomass lb,
      double beIso,
      double beMono,
      double beOvoc,
      OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand1 = new OleDbCommand())
      {
        double num1 = 1E-06;
        this.Landuse = lb.Landuse;
        this.Genera = lb.Genera;
        this.Tree = lb.Tree;
        this.Deciduous = lb.Deciduous;
        oleDbCommand1.Connection = cnBioEDB;
        if (this.Deciduous)
        {
          OleDbCommand oleDbCommand2 = oleDbCommand1;
          string[] strArray = new string[29];
          strArray[0] = "INSERT INTO HourlyEmission ( Landuse, LanduseDesc, [TimeStamp], [Genera], [Tree], [Deciduous],[Isoprene],[Monoterpene]) SELECT DISTINCTROW Landuse, LanduseDesc, TimeStamp,\"";
          strArray[1] = this.Genera;
          strArray[2] = "\",";
          strArray[3] = this.Tree.ToString();
          strArray[4] = ",";
          strArray[5] = this.Deciduous.ToString();
          strArray[6] = ",";
          strArray[7] = beIso.ToString();
          strArray[8] = "*";
          strArray[9] = lb.TotalLeafBiomass.ToString();
          strArray[10] = "*";
          strArray[11] = 1000.0.ToString();
          strArray[12] = "*[LtCrFctIspr]*[TmpCrFctIspr] * ";
          double num2 = 1.135;
          strArray[13] = num2.ToString();
          strArray[14] = "*";
          strArray[15] = num1.ToString();
          strArray[16] = ",";
          strArray[17] = beMono.ToString();
          strArray[18] = "*";
          strArray[19] = lb.TotalLeafBiomass.ToString();
          strArray[20] = "*";
          num2 = 1000.0;
          strArray[21] = num2.ToString();
          strArray[22] = "*[TmpCrFctMono] * ";
          num2 = 1.135;
          strArray[23] = num2.ToString();
          strArray[24] = "*";
          strArray[25] = num1.ToString();
          strArray[26] = " FROM CorrectionFactor WHERE Landuse=\"";
          strArray[27] = this.Landuse;
          strArray[28] = "\" AND GrowSeason=True;";
          string str = string.Concat(strArray);
          oleDbCommand2.CommandText = str;
          oleDbCommand1.ExecuteNonQuery();
          oleDbCommand1.CommandText = "INSERT INTO HourlyEmission ( Landuse, LanduseDesc, [TimeStamp], [Genera], [Tree], [Deciduous],[Isoprene],[Monoterpene]) SELECT DISTINCTROW Landuse, LanduseDesc, TimeStamp, \"" + this.Genera + "\"," + this.Tree.ToString() + "," + this.Deciduous.ToString() + ", 0 , 0  FROM CorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND GrowSeason=False;";
          oleDbCommand1.ExecuteNonQuery();
        }
        else
        {
          OleDbCommand oleDbCommand3 = oleDbCommand1;
          string[] strArray = new string[29];
          strArray[0] = "INSERT INTO HourlyEmission ( Landuse, LanduseDesc, [TimeStamp], [Genera], [Tree], [Deciduous],[Isoprene],[Monoterpene]) SELECT DISTINCTROW Landuse, LanduseDesc, TimeStamp, \"";
          strArray[1] = this.Genera;
          strArray[2] = "\",";
          strArray[3] = this.Tree.ToString();
          strArray[4] = ",";
          strArray[5] = this.Deciduous.ToString();
          strArray[6] = ",";
          strArray[7] = beIso.ToString();
          strArray[8] = "*";
          strArray[9] = lb.TotalLeafBiomass.ToString();
          strArray[10] = "*";
          strArray[11] = 1000.0.ToString();
          strArray[12] = "*[LtCrFctIspr]*[TmpCrFctIspr] * ";
          double num3 = 1.135;
          strArray[13] = num3.ToString();
          strArray[14] = "*";
          strArray[15] = num1.ToString();
          strArray[16] = ",";
          strArray[17] = beMono.ToString();
          strArray[18] = "*";
          strArray[19] = lb.TotalLeafBiomass.ToString();
          strArray[20] = "*";
          num3 = 1000.0;
          strArray[21] = num3.ToString();
          strArray[22] = "*[TmpCrFctMono] * ";
          num3 = 1.135;
          strArray[23] = num3.ToString();
          strArray[24] = "*";
          strArray[25] = num1.ToString();
          strArray[26] = " FROM CorrectionFactor WHERE Landuse=\"";
          strArray[27] = this.Landuse;
          strArray[28] = "\";";
          string str = string.Concat(strArray);
          oleDbCommand3.CommandText = str;
          oleDbCommand1.ExecuteNonQuery();
        }
      }
    }

    private static void CreateHourlyEmissionTable(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE HourlyEmission ([Landuse] TEXT (10),[LanduseDesc] TEXT (30),[Genera] TEXT (30),[TimeStamp] DateTime,[Tree] BIT,[Deciduous] BIT,[Isoprene] DOUBLE,[Monoterpene] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CreateHourlyEmissionTableForLessMDBStorage(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE HourlyEmissionByLanduse ([Landuse] TEXT (10),[LanduseDesc] TEXT (50),[TimeStamp] DateTime,[Isoprene] DOUBLE,[Monoterpene] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "CREATE TABLE HourlyEmissionBySpecies ([CODE] TEXT (15), [Genera] TEXT (30),[TimeStamp] DateTime,[Isoprene] DOUBLE,[Monoterpene] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "CREATE TABLE BASEEMISSION ([Genera] TEXT (30),[BaseIsoprene] DOUBLE,[BaseMonoterpene] DOUBLE,[BaseOtherVOC] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "CREATE UNIQUE INDEX [GeneraInd] ON BASEEMISSION ([Genera]) WITH DISALLOW NULL";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    public static void WriteHourlyEmissionRecords(OleDbConnection cnBioEDB, ref List<Emission> list)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        for (int index = 0; index < list.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO HourlyEmission ([ID],[Landuse],[LanduseDesc],[Genera],[DateTime],[Tree],[Deciduous],[Isoprene emitted],[Monoterpene emitted],[Other VOCs emitted]) Values (" + index.ToString() + ",\"" + list[index].Landuse + "\",\"" + list[index].LanduseDesc + "\",\"" + list[index].Genera + "\"," + list[index].TimeStamp.ToString() + "," + list[index].Tree.ToString() + "," + list[index].Deciduous.ToString() + "," + list[index].EmitIso.ToString() + "," + list[index].EmitMono.ToString() + "," + list[index].EmitOvoc.ToString() + ")";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    private static void ProcessHourlyAverageEmission(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      OleDbConnection cnBioEDB)
    {
      List<Emission> list = new List<Emission>();
      for (int index = 0; index < lbList.Count; ++index)
      {
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index].Genera, out baseEmission);
        for (int hr = 0; hr < 24; ++hr)
        {
          Emission emission = new Emission();
          if (baseEmission != null)
            emission.CalcHourlyAverageEmission(lbList[index], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, hr, cnBioEDB);
          else
            emission.CalcHourlyAverageEmission(lbList[index], 0.0, 0.0, 0.0, hr, cnBioEDB);
          list.Add(emission);
        }
      }
      Emission.CreateHourlyAvgEmissionTable(cnBioEDB);
      Emission.WriteHourlyAvgEmissionRecords(cnBioEDB, ref list);
    }

    private void CalcHourlyAverageEmission(
      LeafBiomass lb,
      double beIso,
      double beMono,
      double beOvoc,
      int hr,
      OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        double num1 = 1E-06;
        this.Landuse = lb.Landuse;
        this.Genera = lb.Genera;
        this.Hour = hr;
        this.Tree = lb.Tree;
        this.Deciduous = lb.Deciduous;
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Hour FROM HourlyAverageCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Isoprene\" AND Hour=" + this.Hour.ToString() + ";";
        double num2;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LanduseDesc = oleDbDataReader.GetString(1);
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitIso = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Hour FROM HourlyAverageCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Monoterpene\" AND Hour=" + this.Hour.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitMono = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Hour FROM HourlyAverageCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"OVOC\" AND Hour=" + this.Hour.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        this.EmitOvoc = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 1.189 * num1;
      }
    }

    private static void CreateHourlyAvgEmissionTable(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE HourlyEmission ([ID] INT,[Landuse] TEXT (10),[LanduseDesc] TEXT (30),[Genera] TEXT (30),[Hour] INT,[Tree] BIT,[Deciduous] BIT,[Isoprene emitted] DOUBLE,[Monoterpene emitted] DOUBLE,[Other VOCs emitted] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    public static void WriteHourlyAvgEmissionRecords(
      OleDbConnection cnBioEDB,
      ref List<Emission> list)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        for (int index = 0; index < list.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO HourlyEmission ([ID],[Landuse],[LanduseDesc],[Genera],[Hour],[Tree],[Deciduous],[Isoprene emitted],[Monoterpene emitted],[Other VOCs emitted]) Values (" + index.ToString() + ",\"" + list[index].Landuse + "\",\"" + list[index].LanduseDesc + "\",\"" + list[index].Genera + "\"," + list[index].Hour.ToString() + "," + list[index].Tree.ToString() + "," + list[index].Deciduous.ToString() + "," + list[index].EmitIso.ToString() + "," + list[index].EmitMono.ToString() + "," + list[index].EmitOvoc.ToString() + ")";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }
  }
}
