// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.HourlySummary
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System.Data.OleDb;

namespace UFORE.BioEmission
{
  internal class HourlySummary
  {
    public const string HOUR_TABLE = "07_HourlySummary";
    public const string HOUR_LANDUSE_TABLE = "08_HourlySummaryByLanduse";
    public const string HOUR_GENERA_TABLE = "09_HourlySummaryByGenera";
    public const string HOUR_DOMAIN_TABLE = "10_HourlySummaryDomain";
    public const string HOUR_SPECIES_TABLE = "11_HourlySummaryBySpecies";

    public static void CalcSummaryByLanduse(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = "HourlyEmission";
        string str2 = "08_HourlySummaryByLanduse";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW TimeStamp, Landuse, LanduseDesc, Sum([Isoprene]) AS [Isoprene emitted], Sum([Monoterpene]) AS [Monoterpene emitted] INTO [" + str2 + "] FROM " + str1 + " GROUP BY TimeStamp, Landuse, LanduseDesc;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    public static void CalcSummaryByLanduseForLessMdbStorage(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = "HourlyEmissionByLanduse";
        string str2 = "08_HourlySummaryByLanduse";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, TimeStamp, Sum([Isoprene]) AS [Isoprene emitted], Sum([Monoterpene]) AS [Monoterpene emitted] INTO [" + str2 + "] FROM " + str1 + " GROUP BY Landuse, LanduseDesc, TimeStamp;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    public static void CalcSummaryByGenera(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = "HourlyEmission";
        string str2 = "09_HourlySummaryByGenera";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Genera, TimeStamp, Sum([Isoprene]) AS [Isoprene emitted], Sum([Monoterpene]) AS [Monoterpene emitted] INTO [" + str2 + "] FROM " + str1 + " GROUP BY TimeStamp, TimeStamp;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    public static void CalcSummaryByGeneraForLessMdbStorage(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = "HourlyEmissionBySpecies";
        string str2 = "09_HourlySummaryByGenera";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW TimeStamp, Genera, Sum([Isoprene]) AS [Isoprene emitted], Sum([Monoterpene]) AS [Monoterpene emitted] INTO [" + str2 + "] FROM " + str1 + " GROUP BY TimeStamp, Genera;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    public static void CalcSummaryBySpeciesForLessMdbStorage(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = "HourlyEmissionBySpecies";
        string str2 = "11_HourlySummaryBySpecies";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Code as Code, Genera, TimeStamp, Sum([Isoprene]) AS [Isoprene emitted], Sum([Monoterpene]) AS [Monoterpene emitted] INTO [" + str2 + "] FROM " + str1 + " GROUP BY Code, Genera, TimeStamp;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    public static void CalcDomainSummary(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = "HourlyEmission";
        string str2 = "10_HourlySummaryDomain";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW TimeStamp,Sum([Isoprene]) AS [Isoprene emitted], Sum([Monoterpene]) AS [Monoterpene emitted] INTO [" + str2 + "] FROM " + str1 + " GROUP BY TimeStamp;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    public static void CalcDomainSummaryForLessMdbStorage(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = "HourlyEmissionByLanduse";
        string str2 = "10_HourlySummaryDomain";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW TimeStamp,Sum([Isoprene]) AS [Isoprene emitted], Sum([Monoterpene]) AS [Monoterpene emitted] INTO [" + str2 + "] FROM " + str1 + " GROUP BY TimeStamp;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    public static void CalcSummary(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = "HourlyEmission";
        string str2 = "07_HourlySummary";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW EM.Hour, Sum(EM.[Isoprene emitted]) AS [Isoprene emitted], Sum(EM.[Monoterpene emitted]) AS [Monoterpene emitted],Sum(EM.[Other VOCs emitted]) AS [Other VOCs emitted] INTO [" + str2 + "] FROM " + str1 + " AS EM GROUP BY EM.Hour;";
        oleDbCommand.ExecuteNonQuery();
      }
    }
  }
}
