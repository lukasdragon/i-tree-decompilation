// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.Formation
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Location;

namespace UFORE.BioEmission
{
  internal class Formation
  {
    public const string YEAR_CO_FORM_TABLE = "YearlyCOFormation";
    public const string LFON_CO_FORM_TABLE = "LeafOnCOFormation";
    public const string MONTH_CO_FORM_TABLE = "MonthlyCOFormation";
    public const string MONTH_DAY_CO_FORM_TABLE = "MonthlyDaytimeCOFormation";
    public const string HOUR_CO_FORM_TABLE = "HourlyCOFormation";
    public const string YEAR_O3_FORM_TABLE = "YearlyO3Formation";
    public const string LFON_O3_FORM_TABLE = "LeafOnO3Formation";
    public const string MONTH_O3_FORM_TABLE = "MonthlyO3Formation";
    public const string MONTH_DAY_O3_FORM_TABLE = "MonthlyDaytimeO3Formation";
    public const string HOUR_O3_FORM_TABLE = "HourlyO3Formation";
    public const double MIRISO = 11.47;
    public const double EBIRISO = 2.2;
    public const double MIRMON = 4.07;
    public const double EBIRMON = 0.95;
    public const double MIRVOC = 3.2;
    public const double EBIRVOC = 0.65;
    public const double MIRCO = 0.07;
    public const double EBIRCO = 0.03;
    public const double VOCTOCO = 0.1;
    public const double CTOCO = 2.333;
    private const double KG_TO_G = 1000.0;
    private const double UG_TO_KG = 1E-09;
    private const double UG_TO_G = 1E-06;
    public int ID;
    public string Landuse;
    public string LanduseDesc;
    public string Genera;
    public bool Deciduous;
    public bool Tree;
    public int Month;
    public int Hour;
    public double COFormed;
    public double O3Formed;
    public double O3FormedMin;
    public double O3FormedMax;

    [Obsolete]
    public static void ProcessCOFormation(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      string sBioEDB)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.ProcessCOFormation(ref lbList, ref beDict, inv, cnBioEDB);
      }
    }

    public static void ProcessCOFormation(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      Formation.ProcessYearlyCOFormation(ref lbList, ref beDict, inv, cnBioEDB);
      Formation.ProcessLeafOnCOFormation(ref lbList, ref beDict, inv, cnBioEDB);
      Formation.ProcessMonthlyCOFormation(ref lbList, ref beDict, inv, cnBioEDB);
      Formation.ProcessMonthlyDaytimeCOFormation(ref lbList, ref beDict, inv, cnBioEDB);
      Formation.ProcessHourlyAverageCOFormation(ref lbList, ref beDict, inv, cnBioEDB);
    }

    private static void ProcessYearlyCOFormation(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Formation> list = new List<Formation>();
      for (int index = 0; index < lbList.Count; ++index)
      {
        Formation formation = new Formation();
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index].Genera, out baseEmission);
        if (baseEmission != null)
          formation.CalcYearlyCOFormation(lbList[index], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, inv, cnBioEDB);
        else
          formation.CalcYearlyCOFormation(lbList[index], 0.0, 0.0, 0.0, inv, cnBioEDB);
        list.Add(formation);
      }
      Formation.CreateYearlyCOFormationTable(cnBioEDB);
      Formation.WriteYearlyCOFormationRecords(cnBioEDB, ref list);
    }

    private void CalcYearlyCOFormation(
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
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant FROM YearlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Isoprene\";";
        double num2;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LanduseDesc = oleDbDataReader.GetString(1);
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num3 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant FROM YearlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Monoterpene\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num4 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant FROM YearlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"OVOC\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num5 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        this.COFormed = num3 + num4 + num5;
      }
    }

    [Obsolete]
    public static void CreateYearlyCOFormationTable(string sBioEDB)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.CreateYearlyCOFormationTable(cnBioEDB);
      }
    }

    public static void CreateYearlyCOFormationTable(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE YearlyCOFormation ([ID] INT,[Landuse] TEXT (10),[LanduseDesc] TEXT (30),[Genera] TEXT (30),[Tree] BIT,[Deciduous] BIT,[CO formed] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void WriteYearlyCOFormationRecords(string sBioEDB, ref List<Formation> list)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.WriteYearlyCOFormationRecords(cnBioEDB, ref list);
      }
    }

    public static void WriteYearlyCOFormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> list)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        for (int index = 0; index < list.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO YearlyCOFormation ([ID],[Landuse],[LanduseDesc],[Genera],[Tree],[Deciduous],[CO formed]) Values (" + index.ToString() + ",\"" + list[index].Landuse + "\",\"" + list[index].LanduseDesc + "\",\"" + list[index].Genera + "\"," + list[index].Tree.ToString() + "," + list[index].Deciduous.ToString() + "," + list[index].COFormed.ToString() + ")";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    public static void ReadYearlyCOFormationRecords(string sBioEDB, ref List<Formation> fmList)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.ReadYearlyCOFormationRecords(cnBioEDB, ref fmList);
      }
    }

    public static void ReadYearlyCOFormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> fmList)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT [ID],[Landuse],[LanduseDesc],[Genera],[CO formed] FROM YearlyCOFormation ORDER BY [ID];";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            Formation formation = new Formation()
            {
              ID = oleDbDataReader.GetInt32(0),
              Landuse = oleDbDataReader.GetString(1),
              LanduseDesc = oleDbDataReader.GetString(2),
              Genera = oleDbDataReader.GetString(3),
              COFormed = oleDbDataReader.GetDouble(4)
            };
            fmList.Add(formation);
          }
          oleDbDataReader.Close();
        }
      }
    }

    private static void ProcessLeafOnCOFormation(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Formation> list = new List<Formation>();
      for (int index = 0; index < lbList.Count; ++index)
      {
        Formation formation = new Formation();
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index].Genera, out baseEmission);
        if (baseEmission != null)
          formation.CalcLeafOnCOFormation(lbList[index], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, inv, cnBioEDB);
        else
          formation.CalcLeafOnCOFormation(lbList[index], 0.0, 0.0, 0.0, inv, cnBioEDB);
        list.Add(formation);
      }
      Formation.CreateLeafOnCOFormationTable(cnBioEDB);
      Formation.WriteLeafOnCOFormationRecords(cnBioEDB, ref list);
    }

    private void CalcLeafOnCOFormation(
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
        double num3 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Pollutant FROM LeafOnCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Pollutant=\"Monoterpene\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num4 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Pollutant FROM LeafOnCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Pollutant=\"OVOC\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num5 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        this.COFormed = num3 + num4 + num5;
      }
    }

    private static void CreateLeafOnCOFormationTable(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE LeafOnCOFormation ([ID] INT,[Landuse] TEXT (10),[LanduseDesc] TEXT (30),[Genera] TEXT (30),[Tree] BIT,[Deciduous] BIT,[CO formed] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void WriteLeafOnCOFormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> list)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        for (int index = 0; index < list.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO LeafOnCOFormation ([ID],[Landuse],[LanduseDesc],[Genera],[Tree],[Deciduous],[CO formed]) Values (" + index.ToString() + ",\"" + list[index].Landuse + "\",\"" + list[index].LanduseDesc + "\",\"" + list[index].Genera + "\"," + list[index].Tree.ToString() + "," + list[index].Deciduous.ToString() + "," + list[index].COFormed.ToString() + ")";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    public static void ReadLeafOnCOFormationRecords(string sBioEDB, ref List<Formation> fmList)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.ReadLeafOnCOFormationRecords(cnBioEDB, ref fmList);
      }
    }

    public static void ReadLeafOnCOFormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> fmList)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT [ID],[Landuse],[LanduseDesc],[Genera],[CO formed] FROM LeafOnCOFormation ORDER BY [ID];";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            Formation formation = new Formation()
            {
              ID = oleDbDataReader.GetInt32(0),
              Landuse = oleDbDataReader.GetString(1),
              LanduseDesc = oleDbDataReader.GetString(2),
              Genera = oleDbDataReader.GetString(3),
              COFormed = oleDbDataReader.GetDouble(4)
            };
            fmList.Add(formation);
          }
          oleDbDataReader.Close();
        }
      }
    }

    private static void ProcessMonthlyCOFormation(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Formation> list = new List<Formation>();
      bool daytime = false;
      for (int index1 = 0; index1 < lbList.Count; ++index1)
      {
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index1].Genera, out baseEmission);
        for (int index2 = 0; index2 < 12; ++index2)
        {
          Formation formation = new Formation();
          if (baseEmission != null)
            formation.CalcMonthlyCOFormation(lbList[index1], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, index2 + 1, inv, cnBioEDB);
          else
            formation.CalcMonthlyCOFormation(lbList[index1], 0.0, 0.0, 0.0, index2 + 1, inv, cnBioEDB);
          list.Add(formation);
        }
      }
      Formation.CreateMonthlyCOFormationTable(cnBioEDB, daytime);
      Formation.WriteMonthlyCOFormationRecords(cnBioEDB, ref list, daytime);
    }

    private static void ProcessMonthlyDaytimeCOFormation(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Formation> list = new List<Formation>();
      bool daytime = true;
      for (int index1 = 0; index1 < lbList.Count; ++index1)
      {
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index1].Genera, out baseEmission);
        for (int index2 = 0; index2 < 12; ++index2)
        {
          Formation formation = new Formation();
          if (baseEmission != null)
            formation.CalcMonthlyDaytimeCOFormation(lbList[index1], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, index2 + 1, inv, cnBioEDB);
          else
            formation.CalcMonthlyDaytimeCOFormation(lbList[index1], 0.0, 0.0, 0.0, index2 + 1, inv, cnBioEDB);
          list.Add(formation);
        }
      }
      Formation.CreateMonthlyCOFormationTable(cnBioEDB, daytime);
      Formation.WriteMonthlyCOFormationRecords(cnBioEDB, ref list, daytime);
    }

    private void CalcMonthlyCOFormation(
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
        double num3 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Month FROM MonthlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Monoterpene\" AND Month=" + this.Month.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num4 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Month FROM MonthlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"OVOC\" AND Month=" + this.Month.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num5 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        this.COFormed = num3 + num4 + num5;
      }
    }

    private void CalcMonthlyDaytimeCOFormation(
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
        double num3 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Month FROM MonthlyDaytimeCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Monoterpene\" AND Month=" + this.Month.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num4 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Month FROM MonthlyDaytimeCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"OVOC\" AND Month=" + this.Month.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num5 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        this.COFormed = num3 + num4 + num5;
      }
    }

    private static void CreateMonthlyCOFormationTable(OleDbConnection cnBioEDB, bool daytime)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = daytime ? "MonthlyDaytimeCOFormation" : "MonthlyCOFormation";
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE " + str + " ([ID] INT,[Landuse] TEXT (10),[LanduseDesc] TEXT (30),[Genera] TEXT (30),[Month] INT,[Tree] BIT,[Deciduous] BIT,[CO formed] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void WriteMonthlyCOFormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> list,
      bool daytime)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = daytime ? "MonthlyDaytimeCOFormation" : "MonthlyCOFormation";
        oleDbCommand.Connection = cnBioEDB;
        for (int index = 0; index < list.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO " + str + " ([ID],[Landuse],[LanduseDesc],[Genera],[Month],[Tree],[Deciduous],[CO formed]) Values (" + index.ToString() + ",\"" + list[index].Landuse + "\",\"" + list[index].LanduseDesc + "\",\"" + list[index].Genera + "\"," + list[index].Month.ToString() + "," + list[index].Tree.ToString() + "," + list[index].Deciduous.ToString() + "," + list[index].COFormed.ToString() + ")";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    public static void ReadMonthlyCOFormationRecords(
      string sBioEDB,
      ref List<Formation> fmList,
      bool daytime)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.ReadMonthlyCOFormationRecords(cnBioEDB, ref fmList, daytime);
      }
    }

    public static void ReadMonthlyCOFormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> fmList,
      bool daytime)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = daytime ? "MonthlyDaytimeCOFormation" : "MonthlyCOFormation";
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT [ID],[Landuse],[LanduseDesc],[Genera],[Month],[CO formed] FROM " + str + " ORDER BY [ID];";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            Formation formation = new Formation()
            {
              ID = oleDbDataReader.GetInt32(0),
              Landuse = oleDbDataReader.GetString(1),
              LanduseDesc = oleDbDataReader.GetString(2),
              Genera = oleDbDataReader.GetString(3),
              Month = oleDbDataReader.GetInt32(4),
              COFormed = oleDbDataReader.GetDouble(5)
            };
            fmList.Add(formation);
          }
          oleDbDataReader.Close();
        }
      }
    }

    private static void ProcessHourlyAverageCOFormation(
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Formation> list = new List<Formation>();
      for (int index = 0; index < lbList.Count; ++index)
      {
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index].Genera, out baseEmission);
        for (int hr = 0; hr < 24; ++hr)
        {
          Formation formation = new Formation();
          if (baseEmission != null)
            formation.CalcHourlyAverageCOFormation(lbList[index], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, hr, inv, cnBioEDB);
          else
            formation.CalcHourlyAverageCOFormation(lbList[index], 0.0, 0.0, 0.0, hr, inv, cnBioEDB);
          list.Add(formation);
        }
      }
      Formation.CreateHourlyCOFormationTable(cnBioEDB);
      Formation.WriteHourlyCOFormationRecords(cnBioEDB, ref list);
    }

    private void CalcHourlyAverageCOFormation(
      LeafBiomass lb,
      double beIso,
      double beMono,
      double beOvoc,
      int hr,
      bool bInv,
      OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        double num1 = !bInv ? 1E-09 : 1E-06;
        this.Landuse = lb.Landuse;
        this.Genera = lb.Genera;
        this.Hour = hr;
        this.Tree = lb.Tree;
        this.Deciduous = lb.Deciduous;
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Hour FROM HourlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Isoprene\" AND Hour=" + this.Hour.ToString() + ";";
        double num2;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LanduseDesc = oleDbDataReader.GetString(1);
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num3 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Hour FROM HourlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Monoterpene\" AND Hour=" + this.Hour.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num4 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, CorrFactor, Deciduous, Pollutant, Hour FROM HourlyCorrectionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"OVOC\" AND Hour=" + this.Hour.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num5 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        this.COFormed = num3 + num4 + num5;
      }
    }

    private static void CreateHourlyCOFormationTable(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE HourlyCOFormation ([ID] INT,[Landuse] TEXT (10),[LanduseDesc] TEXT (30),[Genera] TEXT (30),[Hour] INT,[Tree] BIT,[Deciduous] BIT,[CO formed] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void WriteHourlyCOFormationRecords(string sBioEDB, ref List<Formation> list)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.WriteHourlyCOFormationRecords(cnBioEDB, ref list);
      }
    }

    public static void WriteHourlyCOFormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> list)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        for (int index = 0; index < list.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO HourlyCOFormation ([ID],[Landuse],[LanduseDesc],[Genera],[Hour],[Tree],[Deciduous],[CO formed]) Values (" + index.ToString() + ",\"" + list[index].Landuse + "\",\"" + list[index].LanduseDesc + "\",\"" + list[index].Genera + "\"," + list[index].Hour.ToString() + "," + list[index].Tree.ToString() + "," + list[index].Deciduous.ToString() + "," + list[index].COFormed.ToString() + ")";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    public static void ReadHourlyCOFormationRecords(string sBioEDB, ref List<Formation> fmList)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.ReadHourlyCOFormationRecords(cnBioEDB, ref fmList);
      }
    }

    public static void ReadHourlyCOFormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> fmList)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT [ID],[Landuse],[LanduseDesc],[Genera],[Hour],[CO formed] FROM HourlyCOFormation ORDER BY [ID];";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            Formation formation = new Formation()
            {
              ID = oleDbDataReader.GetInt32(0),
              Landuse = oleDbDataReader.GetString(1),
              LanduseDesc = oleDbDataReader.GetString(2),
              Genera = oleDbDataReader.GetString(3),
              Hour = oleDbDataReader.GetInt32(4),
              COFormed = oleDbDataReader.GetDouble(5)
            };
            fmList.Add(formation);
          }
          oleDbDataReader.Close();
        }
      }
    }

    [Obsolete]
    public static void ProcessO3Formation(
      LocationDataForB locData,
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      string sBioEDB)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.ProcessO3Formation(locData, ref lbList, ref beDict, inv, cnBioEDB);
      }
    }

    public static void ProcessO3Formation(
      LocationDataForB locData,
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      Formation.ProcessYearlyO3Formation(locData, ref lbList, ref beDict, inv, cnBioEDB);
      Formation.ProcessLeafOnO3Formation(locData, ref lbList, ref beDict, inv, cnBioEDB);
      Formation.ProcessMonthlyO3Formation(locData, ref lbList, ref beDict, inv, cnBioEDB);
      Formation.ProcessMonthlyDaytimeO3Formation(locData, ref lbList, ref beDict, inv, cnBioEDB);
      Formation.ProcessHourlyAverageO3Formation(locData, ref lbList, ref beDict, inv, cnBioEDB);
    }

    private static void ProcessYearlyO3Formation(
      LocationDataForB locData,
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Formation> list = new List<Formation>();
      for (int index = 0; index < lbList.Count; ++index)
      {
        Formation formation = new Formation();
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index].Genera, out baseEmission);
        if (baseEmission != null)
          formation.CalcYearlyO3Formation(locData, lbList[index], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, inv, cnBioEDB);
        else
          formation.CalcYearlyO3Formation(locData, lbList[index], 0.0, 0.0, 0.0, inv, cnBioEDB);
        list.Add(formation);
      }
      Formation.CreateYearlyO3FormationTable(cnBioEDB);
      Formation.WriteYearlyO3FormationRecords(cnBioEDB, ref list);
    }

    private void CalcYearlyO3Formation(
      LocationDataForB locData,
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
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant FROM YearlyConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Isoprene\";";
        double num2;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LanduseDesc = oleDbDataReader.GetString(1);
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num3 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        double num4 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num5 = num3 * locData.MoirIsoprene + num4 * locData.MoirCO;
        double num6 = num3 * 2.2 + num4 * 0.03;
        double num7 = num3 * 11.47 + num4 * 0.07;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant FROM YearlyConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Monoterpene\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num8 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        double num9 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num10 = num8 * locData.MoirMonoterpene + num9 * locData.MoirCO;
        double num11 = num8 * 0.95 + num9 * 0.03;
        double num12 = num8 * 4.07 + num9 * 0.07;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant FROM YearlyConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"OVOC\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num13 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 1.189 * num1;
        double num14 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num15 = num13 * locData.MoirVoc + num14 * locData.MoirCO;
        double num16 = num13 * 0.65 + num14 * 0.03;
        double num17 = num13 * 3.2 + num14 * 0.07;
        this.O3Formed = num5 + num10 + num15;
        this.O3FormedMin = num6 + num11 + num16;
        this.O3FormedMax = num7 + num12 + num17;
      }
    }

    [Obsolete]
    public static void CreateYearlyO3FormationTable(string sBioEDB)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.CreateYearlyO3FormationTable(cnBioEDB);
      }
    }

    public static void CreateYearlyO3FormationTable(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE YearlyO3Formation ([ID] INT,[Landuse] TEXT (10),[LanduseDesc] TEXT (30),[Genera] TEXT (30),[Tree] BIT,[Deciduous] BIT,[O3 formed minimum] DOUBLE,[O3 formed] DOUBLE,[O3 formed maximum] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void WriteYearlyO3FormationRecords(string sBioEDB, ref List<Formation> list)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.WriteYearlyO3FormationRecords(cnBioEDB, ref list);
      }
    }

    public static void WriteYearlyO3FormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> list)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        for (int index = 0; index < list.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO YearlyO3Formation ([ID],[Landuse],[LanduseDesc],[Genera],[Tree],[Deciduous],[O3 formed],[O3 formed minimum],[O3 formed maximum]) Values (" + index.ToString() + ",\"" + list[index].Landuse + "\",\"" + list[index].LanduseDesc + "\",\"" + list[index].Genera + "\"," + list[index].Tree.ToString() + "," + list[index].Deciduous.ToString() + "," + list[index].O3Formed.ToString() + "," + list[index].O3FormedMin.ToString() + "," + list[index].O3FormedMax.ToString() + ")";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    public static void ReadYearlyO3FormationRecords(string sBioEDB, ref List<Formation> fmList)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.ReadYearlyO3FormationRecords(cnBioEDB, ref fmList);
      }
    }

    public static void ReadYearlyO3FormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> fmList)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT [ID],[Landuse],[LanduseDesc],[Genera],[O3 formed minimum],[O3 formed],[O3 formed maximum] FROM YearlyO3Formation ORDER BY [ID];";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            Formation formation = new Formation()
            {
              ID = oleDbDataReader.GetInt32(0),
              Landuse = oleDbDataReader.GetString(1),
              LanduseDesc = oleDbDataReader.GetString(2),
              Genera = oleDbDataReader.GetString(3),
              O3FormedMin = oleDbDataReader.GetDouble(4),
              O3Formed = oleDbDataReader.GetDouble(5),
              O3FormedMax = oleDbDataReader.GetDouble(6)
            };
            fmList.Add(formation);
          }
          oleDbDataReader.Close();
        }
      }
    }

    private static void ProcessLeafOnO3Formation(
      LocationDataForB locData,
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Formation> list = new List<Formation>();
      for (int index = 0; index < lbList.Count; ++index)
      {
        Formation formation = new Formation();
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index].Genera, out baseEmission);
        if (baseEmission != null)
          formation.CalcLeafOnO3Formation(locData, lbList[index], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, inv, cnBioEDB);
        else
          formation.CalcLeafOnO3Formation(locData, lbList[index], 0.0, 0.0, 0.0, inv, cnBioEDB);
        list.Add(formation);
      }
      Formation.CreateLeafOnO3FormationTable(cnBioEDB);
      Formation.WriteLeafOnO3FormationRecords(cnBioEDB, ref list);
    }

    private void CalcLeafOnO3Formation(
      LocationDataForB locData,
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
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor,  Pollutant FROM LeafOnConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Pollutant=\"Isoprene\";";
        double num2;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LanduseDesc = oleDbDataReader.GetString(1);
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num3 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        double num4 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num5 = num3 * locData.MoirIsoprene + num4 * locData.MoirCO;
        double num6 = num3 * 2.2 + num4 * 0.03;
        double num7 = num3 * 11.47 + num4 * 0.07;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Pollutant FROM LeafOnConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Pollutant=\"Monoterpene\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num8 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        double num9 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num10 = num8 * locData.MoirMonoterpene + num9 * locData.MoirCO;
        double num11 = num8 * 0.95 + num9 * 0.03;
        double num12 = num8 * 4.07 + num9 * 0.07;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Pollutant FROM LeafOnConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Pollutant=\"OVOC\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num13 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 1.189 * num1;
        double num14 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num15 = num13 * locData.MoirVoc + num14 * locData.MoirCO;
        double num16 = num13 * 0.65 + num14 * 0.03;
        double num17 = num13 * 3.2 + num14 * 0.07;
        this.O3Formed = num5 + num10 + num15;
        this.O3FormedMin = num6 + num11 + num16;
        this.O3FormedMax = num7 + num12 + num17;
      }
    }

    private static void CreateLeafOnO3FormationTable(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE LeafOnO3Formation ([ID] INT,[Landuse] TEXT (10),[LanduseDesc] TEXT (30),[Genera] TEXT (30),[Tree] BIT,[Deciduous] BIT,[O3 formed minimum] DOUBLE,[O3 formed] DOUBLE,[O3 formed maximum] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void WriteLeafOnO3FormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> list)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        for (int index = 0; index < list.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO LeafOnO3Formation ([ID],[Landuse],[LanduseDesc],[Genera],[Tree],[Deciduous],[O3 formed],[O3 formed minimum],[O3 formed maximum]) Values (" + index.ToString() + ",\"" + list[index].Landuse + "\",\"" + list[index].LanduseDesc + "\",\"" + list[index].Genera + "\"," + list[index].Tree.ToString() + "," + list[index].Deciduous.ToString() + "," + list[index].O3Formed.ToString() + "," + list[index].O3FormedMin.ToString() + "," + list[index].O3FormedMax.ToString() + ")";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    public static void ReadLeafOnO3FormationRecords(string sBioEDB, ref List<Formation> fmList)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.ReadLeafOnO3FormationRecords(cnBioEDB, ref fmList);
      }
    }

    public static void ReadLeafOnO3FormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> fmList)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT [ID],[Landuse],[LanduseDesc],[Genera],[O3 formed minimum],[O3 formed],[O3 formed maximum] FROM LeafOnO3Formation ORDER BY [ID];";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            Formation formation = new Formation()
            {
              ID = oleDbDataReader.GetInt32(0),
              Landuse = oleDbDataReader.GetString(1),
              LanduseDesc = oleDbDataReader.GetString(2),
              Genera = oleDbDataReader.GetString(3),
              O3FormedMin = oleDbDataReader.GetDouble(4),
              O3Formed = oleDbDataReader.GetDouble(5),
              O3FormedMax = oleDbDataReader.GetDouble(6)
            };
            fmList.Add(formation);
          }
          oleDbDataReader.Close();
        }
      }
    }

    private static void ProcessMonthlyO3Formation(
      LocationDataForB locData,
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Formation> list = new List<Formation>();
      bool daytime = false;
      for (int index1 = 0; index1 < lbList.Count; ++index1)
      {
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index1].Genera, out baseEmission);
        for (int index2 = 0; index2 < 12; ++index2)
        {
          Formation formation = new Formation();
          if (baseEmission != null)
            formation.CalcMonthlyO3Formation(locData, lbList[index1], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, index2 + 1, inv, cnBioEDB);
          else
            formation.CalcMonthlyO3Formation(locData, lbList[index1], 0.0, 0.0, 0.0, index2 + 1, inv, cnBioEDB);
          list.Add(formation);
        }
      }
      Formation.CreateMonthlyO3FormationTable(cnBioEDB, daytime);
      Formation.WriteMonthlyO3FormationRecords(cnBioEDB, ref list, daytime);
    }

    private static void ProcessMonthlyDaytimeO3Formation(
      LocationDataForB locData,
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Formation> list = new List<Formation>();
      bool daytime = true;
      for (int index1 = 0; index1 < lbList.Count; ++index1)
      {
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index1].Genera, out baseEmission);
        for (int index2 = 0; index2 < 12; ++index2)
        {
          Formation formation = new Formation();
          if (baseEmission != null)
            formation.CalcMonthlyDaytimeO3Formation(locData, lbList[index1], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, index2 + 1, inv, cnBioEDB);
          else
            formation.CalcMonthlyDaytimeO3Formation(locData, lbList[index1], 0.0, 0.0, 0.0, index2 + 1, inv, cnBioEDB);
          list.Add(formation);
        }
      }
      Formation.CreateMonthlyO3FormationTable(cnBioEDB, daytime);
      Formation.WriteMonthlyO3FormationRecords(cnBioEDB, ref list, daytime);
    }

    private void CalcMonthlyO3Formation(
      LocationDataForB locData,
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
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant, Month FROM MonthlyConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Isoprene\" AND Month=" + this.Month.ToString() + ";";
        double num2;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LanduseDesc = oleDbDataReader.GetString(1);
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num3 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        double num4 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num5 = num3 * locData.MoirIsoprene + num4 * locData.MoirCO;
        double num6 = num3 * 2.2 + num4 * 0.03;
        double num7 = num3 * 11.47 + num4 * 0.07;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant, Month FROM MonthlyConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Monoterpene\" AND Month=" + this.Month.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num8 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        double num9 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num10 = num8 * locData.MoirMonoterpene + num9 * locData.MoirCO;
        double num11 = num8 * 0.95 + num9 * 0.03;
        double num12 = num8 * 4.07 + num9 * 0.07;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant, Month FROM MonthlyConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"OVOC\" AND Month=" + this.Month.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num13 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 1.189 * num1;
        double num14 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num15 = num13 * locData.MoirVoc + num14 * locData.MoirCO;
        double num16 = num13 * 0.65 + num14 * 0.03;
        double num17 = num13 * 3.2 + num14 * 0.07;
        this.O3Formed = num5 + num10 + num15;
        this.O3FormedMin = num6 + num11 + num16;
        this.O3FormedMax = num7 + num12 + num17;
      }
    }

    private void CalcMonthlyDaytimeO3Formation(
      LocationDataForB locData,
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
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant, Month FROM MonthlyDaytimeConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Isoprene\" AND Month=" + this.Month.ToString() + ";";
        double num2;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LanduseDesc = oleDbDataReader.GetString(1);
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num3 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        double num4 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num5 = num3 * locData.MoirIsoprene + num4 * locData.MoirCO;
        double num6 = num3 * 2.2 + num4 * 0.03;
        double num7 = num3 * 11.47 + num4 * 0.07;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant, Month FROM MonthlyDaytimeConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Monoterpene\" AND Month=" + this.Month.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num8 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        double num9 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num10 = num8 * locData.MoirMonoterpene + num9 * locData.MoirCO;
        double num11 = num8 * 0.95 + num9 * 0.03;
        double num12 = num8 * 4.07 + num9 * 0.07;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant, Month FROM MonthlyDaytimeConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"OVOC\" AND Month=" + this.Month.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num13 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 1.189 * num1;
        double num14 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num15 = num13 * locData.MoirVoc + num14 * locData.MoirCO;
        double num16 = num13 * 0.65 + num14 * 0.03;
        double num17 = num13 * 3.2 + num14 * 0.07;
        this.O3Formed = num5 + num10 + num15;
        this.O3FormedMin = num6 + num11 + num16;
        this.O3FormedMax = num7 + num12 + num17;
      }
    }

    private static void CreateMonthlyO3FormationTable(OleDbConnection cnBioEDB, bool daytime)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = daytime ? "MonthlyDaytimeO3Formation" : "MonthlyO3Formation";
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE " + str + " ([ID] INT,[Landuse] TEXT (10),[LanduseDesc] TEXT (30),[Genera] TEXT (30),[Month] INT,[Tree] BIT,[Deciduous] BIT,[O3 formed minimum] DOUBLE,[O3 formed] DOUBLE,[O3 formed maximum] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void WriteMonthlyO3FormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> list,
      bool daytime)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = daytime ? "MonthlyDaytimeO3Formation" : "MonthlyO3Formation";
        oleDbCommand.Connection = cnBioEDB;
        for (int index = 0; index < list.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO " + str + " ([ID],[Landuse],[LanduseDesc],[Genera],[Month],[Tree],[Deciduous],[O3 formed],[O3 formed minimum],[O3 formed maximum]) Values (" + index.ToString() + ",\"" + list[index].Landuse + "\",\"" + list[index].LanduseDesc + "\",\"" + list[index].Genera + "\"," + list[index].Month.ToString() + "," + list[index].Tree.ToString() + "," + list[index].Deciduous.ToString() + "," + list[index].O3Formed.ToString() + "," + list[index].O3FormedMin.ToString() + "," + list[index].O3FormedMax.ToString() + ")";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    public static void ReadMonthlyO3FormationRecords(
      string sBioEDB,
      ref List<Formation> fmList,
      bool daytime)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.ReadMonthlyO3FormationRecords(cnBioEDB, ref fmList, daytime);
      }
    }

    public static void ReadMonthlyO3FormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> fmList,
      bool daytime)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = daytime ? "MonthlyDaytimeO3Formation" : "MonthlyO3Formation";
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT [ID],[Landuse],[LanduseDesc],[Genera],[Month],[O3 formed minimum],[O3 formed],[O3 formed maximum] FROM " + str + " ORDER BY [ID];";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            Formation formation = new Formation()
            {
              ID = oleDbDataReader.GetInt32(0),
              Landuse = oleDbDataReader.GetString(1),
              LanduseDesc = oleDbDataReader.GetString(2),
              Genera = oleDbDataReader.GetString(3),
              Month = oleDbDataReader.GetInt32(4),
              O3FormedMin = oleDbDataReader.GetDouble(5),
              O3Formed = oleDbDataReader.GetDouble(6),
              O3FormedMax = oleDbDataReader.GetDouble(7)
            };
            fmList.Add(formation);
          }
          oleDbDataReader.Close();
        }
      }
    }

    private static void ProcessHourlyAverageO3Formation(
      LocationDataForB locData,
      ref List<LeafBiomass> lbList,
      ref Dictionary<string, BaseEmission> beDict,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Formation> list = new List<Formation>();
      for (int index = 0; index < lbList.Count; ++index)
      {
        BaseEmission baseEmission;
        beDict.TryGetValue(lbList[index].Genera, out baseEmission);
        for (int hr = 0; hr < 24; ++hr)
        {
          Formation formation = new Formation();
          if (baseEmission != null)
            formation.CalcHourlyAverageO3Formation(locData, lbList[index], baseEmission.BaseIsoprene, baseEmission.BaseMonoterpene, baseEmission.BaseOtherVOC, hr, inv, cnBioEDB);
          else
            formation.CalcHourlyAverageO3Formation(locData, lbList[index], 0.0, 0.0, 0.0, hr, inv, cnBioEDB);
          list.Add(formation);
        }
      }
      Formation.CreateHourlyO3FormationTable(cnBioEDB);
      Formation.WriteHourlyO3FormationRecords(cnBioEDB, ref list);
    }

    private void CalcHourlyAverageO3Formation(
      LocationDataForB locData,
      LeafBiomass lb,
      double beIso,
      double beMono,
      double beOvoc,
      int hr,
      bool bInv,
      OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        double num1 = !bInv ? 1E-09 : 1E-06;
        this.Landuse = lb.Landuse;
        this.Genera = lb.Genera;
        this.Hour = hr;
        this.Tree = lb.Tree;
        this.Deciduous = lb.Deciduous;
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant, Hour FROM HourlyAverageConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Isoprene\" AND Hour=" + this.Hour.ToString() + ";";
        double num2;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LanduseDesc = oleDbDataReader.GetString(1);
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num3 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        double num4 = beIso * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num5 = num3 * locData.MoirIsoprene + num4 * locData.MoirCO;
        double num6 = num3 * 2.2 + num4 * 0.03;
        double num7 = num3 * 11.47 + num4 * 0.07;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant, Hour FROM HourlyAverageConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"Monoterpene\" AND Hour=" + this.Hour.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num8 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 1.135 * num1;
        double num9 = beMono * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num10 = num8 * locData.MoirMonoterpene + num9 * locData.MoirCO;
        double num11 = num8 * 0.95 + num9 * 0.03;
        double num12 = num8 * 4.07 + num9 * 0.07;
        oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant, Hour FROM HourlyAverageConversionFactor WHERE Landuse=\"" + this.Landuse + "\" AND Deciduous=" + this.Deciduous.ToString() + " AND Pollutant=\"OVOC\" AND Hour=" + this.Hour.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num2 = oleDbDataReader.GetDouble(2);
          oleDbDataReader.Close();
        }
        double num13 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 1.189 * num1;
        double num14 = beOvoc * lb.TotalLeafBiomass * 1000.0 * num2 * 0.1 * 2.333 * num1;
        double num15 = num13 * locData.MoirVoc + num14 * locData.MoirCO;
        double num16 = num13 * 0.65 + num14 * 0.03;
        double num17 = num13 * 3.2 + num14 * 0.07;
        this.O3Formed = num5 + num10 + num15;
        this.O3FormedMin = num6 + num11 + num16;
        this.O3FormedMax = num7 + num12 + num17;
      }
    }

    private static void CreateHourlyO3FormationTable(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "CREATE TABLE HourlyO3Formation ([ID] INT,[Landuse] TEXT (10),[LanduseDesc] TEXT (30),[Genera] TEXT (30),[Hour] INT,[Tree] BIT,[Deciduous] BIT,[O3 formed minimum] DOUBLE,[O3 formed] DOUBLE,[O3 formed maximum] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void WriteHourlyO3FormationRecords(string sBioEDB, ref List<Formation> list)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.WriteHourlyO3FormationRecords(cnBioEDB, ref list);
      }
    }

    public static void WriteHourlyO3FormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> list)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        for (int index = 0; index < list.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO HourlyO3Formation ([ID],[Landuse],[LanduseDesc],[Genera],[Hour],[Tree],[Deciduous],[O3 formed],[O3 formed minimum],[O3 formed maximum]) Values (" + index.ToString() + ",\"" + list[index].Landuse + "\",\"" + list[index].LanduseDesc + "\",\"" + list[index].Genera + "\"," + list[index].Hour.ToString() + "," + list[index].Tree.ToString() + "," + list[index].Deciduous.ToString() + "," + list[index].O3Formed.ToString() + "," + list[index].O3FormedMin.ToString() + "," + list[index].O3FormedMax.ToString() + ")";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    public static void ReadHourlyO3FormationRecords(string sBioEDB, ref List<Formation> fmList)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        Formation.ReadHourlyO3FormationRecords(cnBioEDB, ref fmList);
      }
    }

    public static void ReadHourlyO3FormationRecords(
      OleDbConnection cnBioEDB,
      ref List<Formation> fmList)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT [ID],[Landuse],[LanduseDesc],[Genera],[Hour],[O3 formed minimum],[O3 formed],[O3 formed maximum] FROM HourlyO3Formation ORDER BY [ID];";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            Formation formation = new Formation()
            {
              ID = oleDbDataReader.GetInt32(0),
              Landuse = oleDbDataReader.GetString(1),
              LanduseDesc = oleDbDataReader.GetString(2),
              Genera = oleDbDataReader.GetString(3),
              Hour = oleDbDataReader.GetInt32(4),
              O3FormedMin = oleDbDataReader.GetDouble(5),
              O3Formed = oleDbDataReader.GetDouble(6),
              O3FormedMax = oleDbDataReader.GetDouble(7)
            };
            fmList.Add(formation);
          }
          oleDbDataReader.Close();
        }
      }
    }
  }
}
