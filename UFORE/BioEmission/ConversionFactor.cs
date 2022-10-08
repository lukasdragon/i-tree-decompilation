// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.ConversionFactor
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Data.OleDb;

namespace UFORE.BioEmission
{
  internal class ConversionFactor
  {
    public const string YR_CV_TABLE = "YearlyConversionFactor";
    public const string LFON_CV_TABLE = "LeafOnConversionFactor";
    public const string MN_CV_TABLE = "MonthlyConversionFactor";
    public const string MNDY_CV_TABLE = "MonthlyDaytimeConversionFactor";
    public const string HR_CV_TABLE = "HourlyAverageConversionFactor";
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

    [Obsolete]
    public static void ProcessConversionFactor(string sBioEDB)
    {
      using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sBioEDB))
      {
        cnBioEDB.Open();
        ConversionFactor.ProcessConversionFactor(cnBioEDB);
      }
    }

    public static void ProcessConversionFactor(OleDbConnection cnBioEDB)
    {
      ConversionFactor.CalcYearlyConversionFactor(cnBioEDB);
      ConversionFactor.CalcLeafOnConversionFactor(cnBioEDB);
      ConversionFactor.CalcMonthlyConversionFactor(cnBioEDB);
      ConversionFactor.CalcMonthlyDaytimeConversionFactor(cnBioEDB);
      ConversionFactor.CalcHourlyAverageConversionFactor(cnBioEDB);
    }

    private static void CalcYearlyConversionFactor(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono) AS ConvFactor, false AS Deciduous,\"Monoterpene\" AS Pollutant INTO YearlyConversionFactor FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, false, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO YearlyConversionFactor ( Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([LtCrFctIspr]*[TmpCrFctIspr]), false, \"Isoprene\" FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, false, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO YearlyConversionFactor ( Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([TmpCrFctMono]), false, \"OVOC\" FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, false, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO YearlyConversionFactor ( Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono), true, \"Monoterpene\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, true, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO YearlyConversionFactor ( Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([LtCrFctIspr]*[TmpCrFctIspr]), true, \"Isoprene\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, true, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO YearlyConversionFactor ( Landuse, LanduseDesc, ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono), true, \"OVOC\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, true, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcLeafOnConversionFactor(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Sum(TmpCrFctMono) AS ConvFactor, \"Monoterpene\" AS Pollutant INTO LeafOnConversionFactor FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO LeafOnConversionFactor ( Landuse, LanduseDesc, ConvFactor, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([LtCrFctIspr]*[TmpCrFctIspr]), \"Isoprene\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO LeafOnConversionFactor ( Landuse, LanduseDesc, ConvFactor, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Sum([TmpCrFctMono]), \"OVOC\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcMonthlyConversionFactor(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], Sum(TmpCrFctMono) AS ConvFactor,false AS Deciduous, \"Monoterpene\" AS Pollutant INTO MonthlyConversionFactor FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), false, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum([LtCrFctIspr]*[TmpCrFctIspr]), false, \"Isoprene\" FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), false, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum([TmpCrFctMono]), false, \"OVOC\" FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), false, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum(TmpCrFctMono), true, \"Monoterpene\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), \"Monoterpene\", true;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], 0 AS ConvFactor, true AS Deciduous, \"Monoterpene\" AS Pollutant INTO win1 FROM CorrectionFactor WHERE GrowSeason=False GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT win1.Landuse, win1.LanduseDesc, win1.Month, win1.ConvFactor, win1.Pollutant, win1.Deciduous INTO win2 FROM win1 LEFT JOIN  MonthlyConversionFactor AS MON ON (win1.Deciduous = MON.Deciduous) AND (win1.Pollutant = MON.Pollutant) AND (win1.LanduseDesc = MON.LanduseDesc) AND (win1.Landuse = MON.Landuse) AND (win1.[Month] = MON.[Month]) WHERE MON.Month Is Null;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Month, ConvFactor, Deciduous, Pollutant FROM win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum([LtCrFctIspr]*[TmpCrFctIspr]), true, \"Isoprene\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], 0 AS ConvFactor, true AS Deciduous, \"Isoprene\" AS Pollutant INTO win1 FROM CorrectionFactor WHERE GrowSeason=False GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT win1.Landuse, win1.LanduseDesc, win1.Month, win1.ConvFactor, win1.Pollutant, win1.Deciduous INTO win2 FROM win1 LEFT JOIN  MonthlyConversionFactor AS MON ON (win1.Deciduous = MON.Deciduous) AND (win1.Pollutant = MON.Pollutant) AND (win1.LanduseDesc = MON.LanduseDesc) AND (win1.Landuse = MON.Landuse) AND (win1.[Month] = MON.[Month]) WHERE MON.Month Is Null;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Month, ConvFactor, Deciduous, Pollutant FROM win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum(TmpCrFctMono), true, \"OVOC\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], 0 AS ConvFactor, true AS Deciduous, \"OVOC\" AS Pollutant INTO win1 FROM CorrectionFactor WHERE GrowSeason=False GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT win1.Landuse, win1.LanduseDesc, win1.Month, win1.ConvFactor, win1.Pollutant, win1.Deciduous INTO win2 FROM win1 LEFT JOIN  MonthlyConversionFactor AS MON ON (win1.Deciduous = MON.Deciduous) AND (win1.Pollutant = MON.Pollutant) AND (win1.LanduseDesc = MON.LanduseDesc) AND (win1.Landuse = MON.Landuse) AND (win1.[Month] = MON.[Month]) WHERE MON.Month Is Null;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Month, ConvFactor, Deciduous, Pollutant FROM win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win2;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcMonthlyDaytimeConversionFactor(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], Sum(TmpCrFctMono) AS ConvFactor,false AS Deciduous, \"Monoterpene\" AS Pollutant INTO MonthlyDaytimeConversionFactor FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), \"Monoterpene\", false;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum([LtCrFctIspr]*[TmpCrFctIspr]), false, \"Isoprene\" FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), false, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum([TmpCrFctMono]), false, \"OVOC\" FROM CorrectionFactor WHERE LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), false, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum(TmpCrFctMono), true, \"Monoterpene\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], 0 AS ConvFactor, true AS Deciduous, \"Monoterpene\" AS Pollutant INTO win1 FROM CorrectionFactor WHERE GrowSeason=False GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT win1.Landuse, win1.LanduseDesc, win1.Month, win1.ConvFactor, win1.Pollutant, win1.Deciduous INTO win2 FROM win1 LEFT JOIN  MonthlyDaytimeConversionFactor AS MON ON (win1.Deciduous = MON.Deciduous) AND (win1.Pollutant = MON.Pollutant) AND (win1.LanduseDesc = MON.LanduseDesc) AND (win1.Landuse = MON.Landuse) AND (win1.[Month] = MON.[Month]) WHERE MON.Month Is Null;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Month, ConvFactor, Deciduous, Pollutant FROM win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum([LtCrFctIspr]*[TmpCrFctIspr]), true, \"Isoprene\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], 0 AS ConvFactor, true AS Deciduous, \"Isoprene\" AS Pollutant INTO win1 FROM CorrectionFactor WHERE GrowSeason=False GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT win1.Landuse, win1.LanduseDesc, win1.Month, win1.ConvFactor, win1.Pollutant, win1.Deciduous INTO win2 FROM win1 LEFT JOIN  MonthlyDaytimeConversionFactor AS MON ON (win1.Deciduous = MON.Deciduous) AND (win1.Pollutant = MON.Pollutant) AND (win1.LanduseDesc = MON.LanduseDesc) AND (win1.Landuse = MON.Landuse) AND (win1.[Month] = MON.[Month]) WHERE MON.Month Is Null;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Month, ConvFactor, Deciduous, Pollutant FROM win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]), Sum(TmpCrFctMono), true, \"OVOC\" FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Month([TimeStamp]) AS [Month], 0 AS ConvFactor, true AS Deciduous, \"OVOC\" AS Pollutant INTO win1 FROM CorrectionFactor WHERE GrowSeason=False GROUP BY Landuse, LanduseDesc, Month([TimeStamp]), true, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT win1.Landuse, win1.LanduseDesc, win1.Month, win1.ConvFactor, win1.Pollutant, win1.Deciduous INTO win2 FROM win1 LEFT JOIN  MonthlyDaytimeConversionFactor AS MON ON (win1.Deciduous = MON.Deciduous) AND (win1.Pollutant = MON.Pollutant) AND (win1.LanduseDesc = MON.LanduseDesc) AND (win1.Landuse = MON.Landuse) AND (win1.[Month] = MON.[Month]) WHERE MON.Month Is Null;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO  MonthlyDaytimeConversionFactor ( Landuse, LanduseDesc, [Month], ConvFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Month, ConvFactor, Deciduous, Pollutant FROM win2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE win2;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcHourlyAverageConversionFactor(OleDbConnection cnBioEDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBioEDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour], Avg(TmpCrFctMono)  AS ConvFactor, False AS Deciduous, \"Monoterpene\" AS Pollutant INTO HourlyAverageConversionFactor FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), False, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyAverageConversionFactor ( Landuse, LanduseDesc, [Hour], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour], Avg([LtCrFctIspr]*[TmpCrFctIspr]) AS ConvFactor, False AS Deciduous, \"Isoprene\" AS Pollutant FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), False, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyAverageConversionFactor ( Landuse, LanduseDesc, [Hour], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour], Avg(TmpCrFctMono)  AS ConvFactor, False AS Deciduous, \"OVOC\" AS Pollutant FROM CorrectionFactor GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), False, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyAverageConversionFactor ( Landuse, LanduseDesc, [Hour], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour], Avg(TmpCrFctMono)  AS ConvFactor, True AS Deciduous, \"Monoterpene\" AS Pollutant FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), True, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour], 0 AS ConvFactor,True AS Deciduous, \"Monoterpene\" AS Pollutant INTO night1 FROM CorrectionFactor WHERE (((LtCrFctIspr)<=0)) GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), True, \"Monoterpene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT NT.Landuse, NT.LanduseDesc, NT.Hour, NT.ConvFactor, NT.Deciduous, NT.Pollutant INTO night2 FROM night1 AS NT LEFT JOIN HourlyAverageConversionFactor AS HR ON (NT.Deciduous = HR.Deciduous) AND (NT.Pollutant = HR.Pollutant) AND (NT.Hour = HR.Hour) AND (NT.LanduseDesc = HR.LanduseDesc) AND (NT.Landuse = HR.Landuse) WHERE (((HR.Hour) Is Null));";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyAverageConversionFactor ( Landuse, LanduseDesc, [Hour], ConvFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Hour, ConvFactor, Deciduous, Pollutant FROM night2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE night1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE night2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyAverageConversionFactor ( Landuse, LanduseDesc, [Hour], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour], Avg([LtCrFctIspr]*[TmpCrFctIspr])  AS ConvFactor, True AS Deciduous, \"Isoprene\" AS Pollutant FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), True, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour], 0 AS ConvFactor,True AS Deciduous, \"Isoprene\" AS Pollutant INTO night1 FROM CorrectionFactor WHERE (((LtCrFctIspr)<=0)) GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), True, \"Isoprene\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT NT.Landuse, NT.LanduseDesc, NT.Hour, NT.ConvFactor, NT.Deciduous, NT.Pollutant INTO night2 FROM night1 AS NT LEFT JOIN HourlyAverageConversionFactor AS HR ON (NT.Deciduous = HR.Deciduous) AND (NT.Pollutant = HR.Pollutant) AND (NT.Hour = HR.Hour) AND (NT.LanduseDesc = HR.LanduseDesc) AND (NT.Landuse = HR.Landuse) WHERE (((HR.Hour) Is Null));";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyAverageConversionFactor ( Landuse, LanduseDesc, [Hour], ConvFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Hour, ConvFactor, Deciduous, Pollutant FROM night2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE night1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE night2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyAverageConversionFactor ( Landuse, LanduseDesc, [Hour], ConvFactor, Deciduous, Pollutant ) SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour],  Avg(TmpCrFctMono)*1.189 AS ConvFactor, True AS Deciduous, \"OVOC\" AS Pollutant FROM CorrectionFactor WHERE GrowSeason=True AND LtCrFctIspr>0 GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), True, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, LanduseDesc, Hour([TimeStamp]) AS [Hour], 0 AS ConvFactor,True AS Deciduous, \"OVOC\" AS Pollutant INTO night1 FROM CorrectionFactor WHERE (((LtCrFctIspr)<=0)) GROUP BY Landuse, LanduseDesc, Hour([TimeStamp]), True, \"OVOC\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT NT.Landuse, NT.LanduseDesc, NT.Hour, NT.ConvFactor, NT.Deciduous, NT.Pollutant INTO night2 FROM night1 AS NT LEFT JOIN HourlyAverageConversionFactor AS HR ON (NT.Deciduous = HR.Deciduous) AND (NT.Pollutant = HR.Pollutant) AND (NT.Hour = HR.Hour) AND (NT.LanduseDesc = HR.LanduseDesc) AND (NT.Landuse = HR.Landuse) WHERE (((HR.Hour) Is Null));";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO HourlyAverageConversionFactor ( Landuse, LanduseDesc, [Hour], ConvFactor, Deciduous, Pollutant ) SELECT Landuse, LanduseDesc, Hour, ConvFactor, Deciduous, Pollutant FROM night2;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE night1;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE night2;";
        oleDbCommand.ExecuteNonQuery();
      }
    }
  }
}
