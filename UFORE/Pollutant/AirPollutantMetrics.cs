// Decompiled with JetBrains decompiler
// Type: UFORE.Pollutant.AirPollutantMetrics
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Deposition;
using UFORE.Location;

namespace UFORE.Pollutant
{
  public class AirPollutantMetrics
  {
    public const string POLL_METRIC_TABLE = "AirPollutantMetrics";
    public const string POLL_METRIC_ANNUAL_TABLE = "10_AirPollutantMetricsAnnualMean";
    public const string POLL_METRIC_QUARTERLY_TABLE = "11_AirPollutantMetricsQuarterlyMean";
    public const string CONC_CHG_ANNUAL_TABLE = "12_ConcentrationChangeAnnualMean";
    public const string CONC_CHG_LEAFON_TABLE = "13_ConcentrationChangeLeafOnMean";
    private DateTime TimeStamp;
    private string Pollutant;
    private string MonState;
    private string MonCounty;
    private string MonSiteID;
    private double D8HourMax;
    private double D8HourMaxChg;
    private double D8HourMaxChgMin;
    private double D8HourMaxChgMax;

    [Obsolete]
    public static void CalcAirPollutantMetrics(LocationData locData, string sDB)
    {
      using (OleDbConnection cn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        cn.Open();
        AirPollutantMetrics.CalcAirPollutantMetrics(locData, cn);
      }
    }

    public static void CalcAirPollutantMetrics(LocationData locData, OleDbConnection cn)
    {
      AirPollutantMetrics.CreatePollutantMetricsTable(cn);
      AirPollutantMetrics.CalcD1HourMax(cn, "CO");
      AirPollutantMetrics.CalcD3HourMean(cn, "CO");
      AirPollutantMetrics.CalcD4HourMean(cn, "CO");
      AirPollutantMetrics.CalcD8HourMean(cn, "CO");
      AirPollutantMetrics.CalcD24HourMean(cn, "CO");
      AirPollutantMetrics.CalcD8HourMax(cn, "CO", locData.PollMonDict, false);
      AirPollutantMetrics.CalcD1HourMax(cn, "NO2");
      AirPollutantMetrics.CalcD3HourMean(cn, "NO2");
      AirPollutantMetrics.CalcD4HourMean(cn, "NO2");
      AirPollutantMetrics.CalcD8HourMean(cn, "NO2");
      AirPollutantMetrics.CalcD24HourMean(cn, "NO2");
      AirPollutantMetrics.CalcD8HourMax(cn, "NO2", locData.PollMonDict, false);
      AirPollutantMetrics.CalcD1HourMax(cn, "O3");
      AirPollutantMetrics.CalcD3HourMean(cn, "O3");
      AirPollutantMetrics.CalcD4HourMean(cn, "O3");
      AirPollutantMetrics.CalcD8HourMean(cn, "O3");
      AirPollutantMetrics.CalcD24HourMean(cn, "O3");
      AirPollutantMetrics.CalcD8HourMax(cn, "O3", locData.PollMonDict, false);
      AirPollutantMetrics.CalcD1HourMax(cn, "PM10*");
      AirPollutantMetrics.CalcD3HourMean(cn, "PM10*");
      AirPollutantMetrics.CalcD4HourMean(cn, "PM10*");
      AirPollutantMetrics.CalcD8HourMean(cn, "PM10*");
      AirPollutantMetrics.CalcD24HourMean(cn, "PM10*");
      AirPollutantMetrics.CalcD8HourMax(cn, "PM10*", locData.PollMonDict, true);
      AirPollutantMetrics.CalcD1HourMax(cn, "PM2.5");
      AirPollutantMetrics.CalcD3HourMean(cn, "PM2.5");
      AirPollutantMetrics.CalcD4HourMean(cn, "PM2.5");
      AirPollutantMetrics.CalcD8HourMean(cn, "PM2.5");
      AirPollutantMetrics.CalcD24HourMean(cn, "PM2.5");
      AirPollutantMetrics.CalcD8HourMax(cn, "PM2.5", locData.PollMonDict, true);
      AirPollutantMetrics.CalcD1HourMax(cn, "SO2");
      AirPollutantMetrics.CalcD3HourMean(cn, "SO2");
      AirPollutantMetrics.CalcD4HourMean(cn, "SO2");
      AirPollutantMetrics.CalcD8HourMean(cn, "SO2");
      AirPollutantMetrics.CalcD24HourMean(cn, "SO2");
      AirPollutantMetrics.CalcD8HourMax(cn, "SO2", locData.PollMonDict, false);
      AirPollutantMetrics.CalcMetricsAnnualMean(cn);
      AirPollutantMetrics.CalcMetricsQuarterlyMean(cn);
      AirPollutantMetrics.CalcConcChgAnnualMean(cn);
      AirPollutantMetrics.CalcConcChgLeafOnMean(cn);
    }

    private static void CreatePollutantMetricsTable(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "CREATE TABLE AirPollutantMetrics([TimeStamp] DateTime,[Pollutant] TEXT (5),[MonitorState] TEXT (10),[MonitorCounty] TEXT (10),[MonitorSiteID] TEXT (10),[D1HourMax] DOUBLE,[D3HourMean] DOUBLE,[D4HourMean] DOUBLE,[D8HourMax] DOUBLE,[D8HourMean] DOUBLE,[D24HourMean] DOUBLE,[D1HourMaxChg] DOUBLE,[D1HourMaxChgMin] DOUBLE,[D1HourMaxChgMax] DOUBLE,[D3HourMeanChg] DOUBLE,[D3HourMeanChgMin] DOUBLE,[D3HourMeanChgMax] DOUBLE,[D4HourMeanChg] DOUBLE,[D4HourMeanChgMin] DOUBLE,[D4HourMeanChgMax] DOUBLE,[D8HourMaxChg] DOUBLE,[D8HourMaxChgMin] DOUBLE,[D8HourMaxChgMax] DOUBLE,[D8HourMeanChg] DOUBLE,[D8HourMeanChgMin] DOUBLE,[D8HourMeanChgMax] DOUBLE,[D24HourMeanChg] DOUBLE,[D24HourMeanChgMin] DOUBLE,[D24HourMeanChgMax] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcD1HourMax(OleDbConnection cn, string sPoll)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[uGm3]" : "[PPM]*1000";
        string str2 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChg]" : "[ConcChg]*1000";
        string str3 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChgMin]" : "[ConcChgMin]*1000";
        string str4 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChgMax]" : "[ConcChgMax]*1000";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "INSERT INTO AirPollutantMetrics ([TimeStamp],[Pollutant],[MonitorState],[MonitorCounty],[MonitorSiteID],[D1HourMax],[D1HourMaxChg],[D1HourMaxChgMin],[D1HourMaxChgMax]) SELECT DISTINCTROW CDate(Format$([TimeStamp],\"Short Date\")),  Pollutant, MonitorState, MonitorCounty, MonitorSiteID, Max(" + str1 + "), Max(" + str2 + "), Max(" + str3 + "), Max(" + str4 + ") FROM DryDeposition GROUP BY CDate(Format$([TimeStamp],\"Short Date\")), Pollutant, MonitorState, MonitorCounty, MonitorSiteID Having Pollutant=\"" + sPoll + "\" ORDER BY CDate(Format$([TimeStamp],\"Short Date\"));";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcD3HourMean(OleDbConnection cn, string sPoll)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[uGm3]" : "[PPM]*1000";
        string str2 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChg]" : "[ConcChg]*1000";
        string str3 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChgMin]" : "[ConcChgMin]*1000";
        string str4 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChgMax]" : "[ConcChgMax]*1000";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW CDate(Format$([TimeStamp],\"Short Date\")) AS [TimeStamp], [Pollutant],[MonitorState],[MonitorCounty],[MonitorSiteID], Avg(" + str1 + ") AS D3HourMean, Avg(" + str2 + ") AS D3HourMeanChg, Avg(" + str3 + ") AS D3HourMeanChgMin, Avg(" + str4 + ") AS D3HourMeanChgMax INTO aaa FROM DryDeposition WHERE Hour(TimeStamp)>=8 And Hour(TimeStamp)<=11 GROUP BY CDate(Format$([TimeStamp],\"Short Date\")),[Pollutant],[MonitorState],[MonitorCounty],[MonitorSiteID] Having Pollutant=\"" + sPoll + "\" ORDER BY CDate(Format$([TimeStamp],\"Short Date\"));";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "UPDATE AirPollutantMetrics INNER JOIN aaa ON AirPollutantMetrics.[TimeStamp] = aaa.[TimeStamp] AND AirPollutantMetrics.Pollutant = [aaa].[Pollutant] AND AirPollutantMetrics.MonitorState = [aaa].[MonitorState] AND AirPollutantMetrics.MonitorCounty = [aaa].[MonitorCounty] AND AirPollutantMetrics.MonitorSiteID = [aaa].[MonitorSiteID] SET AirPollutantMetrics.D3HourMean = [aaa].[D3HourMean],AirPollutantMetrics.D3HourMeanChg = [aaa].[D3HourMeanChg],AirPollutantMetrics.D3HourMeanChgMin = [aaa].[D3HourMeanChgMin],AirPollutantMetrics.D3HourMeanChgMax = [aaa].[D3HourMeanChgMax];";
        oleDbCommand.ExecuteNonQuery();
      }
      AccessFunc.RemoveTable(cn, "aaa");
    }

    private static void CalcD4HourMean(OleDbConnection cn, string sPoll)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[uGm3]" : "[PPM]*1000";
        string str2 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChg]" : "[ConcChg]*1000";
        string str3 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChgMin]" : "[ConcChgMin]*1000";
        string str4 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChgMax]" : "[ConcChgMax]*1000";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW CDate(Format$([DryDeposition].[TimeStamp],\"Short Date\")) AS [TimeStamp], [Pollutant],[MonitorState],[MonitorCounty],[MonitorSiteID], Avg(" + str1 + ") AS D4HourMean, Avg(" + str2 + ") AS D4HourMeanChg, Avg(" + str3 + ") AS D4HourMeanChgMin, Avg(" + str4 + ") AS D4HourMeanChgMax INTO aaa FROM DryDeposition WHERE Hour(TimeStamp)>=6 And Hour(TimeStamp)<=10 GROUP BY CDate(Format$([TimeStamp],\"Short Date\")),[Pollutant],[MonitorState],[MonitorCounty],[MonitorSiteID] Having Pollutant=\"" + sPoll + "\" ORDER BY CDate(Format$([TimeStamp],\"Short Date\"));";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "UPDATE AirPollutantMetrics INNER JOIN aaa ON AirPollutantMetrics.[TimeStamp] = aaa.[TimeStamp] AND AirPollutantMetrics.Pollutant = [aaa].[Pollutant] AND AirPollutantMetrics.MonitorState = [aaa].[MonitorState] AND AirPollutantMetrics.MonitorCounty = [aaa].[MonitorCounty] AND AirPollutantMetrics.MonitorSiteID = [aaa].[MonitorSiteID] SET AirPollutantMetrics.D4HourMean = [aaa].[D4HourMean],AirPollutantMetrics.D4HourMeanChg = [aaa].[D4HourMeanChg],AirPollutantMetrics.D4HourMeanChgMin = [aaa].[D4HourMeanChgMin],AirPollutantMetrics.D4HourMeanChgMax = [aaa].[D4HourMeanChgMax];";
        oleDbCommand.ExecuteNonQuery();
      }
      AccessFunc.RemoveTable(cn, "aaa");
    }

    private static void CalcD8HourMean(OleDbConnection cn, string sPoll)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[uGm3]" : "[PPM]*1000";
        string str2 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChg]" : "[ConcChg]*1000";
        string str3 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChgMin]" : "[ConcChgMin]*1000";
        string str4 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChgMax]" : "[ConcChgMax]*1000";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW CDate(Format$([TimeStamp],\"Short Date\")) AS [TimeStamp], [Pollutant],[MonitorState],[MonitorCounty],[MonitorSiteID], Avg(" + str1 + ") AS D8HourMean, Avg(" + str2 + ") AS D8HourMeanChg, Avg(" + str3 + ") AS D8HourMeanChgMin, Avg(" + str4 + ") AS D8HourMeanChgMax INTO aaa FROM DryDeposition WHERE Hour(TimeStamp)>=9 And Hour(TimeStamp)<=16 GROUP BY CDate(Format$([TimeStamp],\"Short Date\")),[Pollutant],[MonitorState],[MonitorCounty],[MonitorSiteID] Having Pollutant=\"" + sPoll + "\" ORDER BY CDate(Format$([TimeStamp],\"Short Date\"));";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "UPDATE AirPollutantMetrics INNER JOIN aaa ON AirPollutantMetrics.[TimeStamp] = aaa.[TimeStamp] AND AirPollutantMetrics.Pollutant = [aaa].[Pollutant] AND AirPollutantMetrics.MonitorState = [aaa].[MonitorState] AND AirPollutantMetrics.MonitorCounty = [aaa].[MonitorCounty] AND AirPollutantMetrics.MonitorSiteID = [aaa].[MonitorSiteID] SET AirPollutantMetrics.D8HourMean = [aaa].[D8HourMean],AirPollutantMetrics.D8HourMeanChg = [aaa].[D8HourMeanChg],AirPollutantMetrics.D8HourMeanChgMin = [aaa].[D8HourMeanChgMin],AirPollutantMetrics.D8HourMeanChgMax = [aaa].[D8HourMeanChgMax];";
        oleDbCommand.ExecuteNonQuery();
      }
      AccessFunc.RemoveTable(cn, "aaa");
    }

    private static void CalcD24HourMean(OleDbConnection cn, string sPoll)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[uGm3]" : "[PPM]*1000";
        string str2 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChg]" : "[ConcChg]*1000";
        string str3 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChgMin]" : "[ConcChgMin]*1000";
        string str4 = sPoll == "PM10*" || sPoll == "PM2.5" ? "[ConcChgMax]" : "[ConcChgMax]*1000";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW CDate(Format$([TimeStamp],\"Short Date\")) AS [TimeStamp], [Pollutant],[MonitorState],[MonitorCounty],[MonitorSiteID], Avg(" + str1 + ") AS D24HourMean, Avg(" + str2 + ") AS D24HourMeanChg, Avg(" + str3 + ") AS D24HourMeanChgMin, Avg(" + str4 + ") AS D24HourMeanChgMax INTO aaa FROM DryDeposition GROUP BY CDate(Format$([TimeStamp],\"Short Date\")),[Pollutant],[MonitorState],[MonitorCounty],[MonitorSiteID] Having Pollutant=\"" + sPoll + "\" ORDER BY CDate(Format$([TimeStamp],\"Short Date\"));";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "UPDATE AirPollutantMetrics INNER JOIN aaa ON AirPollutantMetrics.[TimeStamp] = aaa.[TimeStamp] AND AirPollutantMetrics.Pollutant = [aaa].[Pollutant] AND AirPollutantMetrics.MonitorState = [aaa].[MonitorState] AND AirPollutantMetrics.MonitorCounty = [aaa].[MonitorCounty] AND AirPollutantMetrics.MonitorSiteID = [aaa].[MonitorSiteID] SET AirPollutantMetrics.D24HourMean = [aaa].[D24HourMean],AirPollutantMetrics.D24HourMeanChg = [aaa].[D24HourMeanChg],AirPollutantMetrics.D24HourMeanChgMin = [aaa].[D24HourMeanChgMin],AirPollutantMetrics.D24HourMeanChgMax = [aaa].[D24HourMeanChgMax];";
        oleDbCommand.ExecuteNonQuery();
      }
      AccessFunc.RemoveTable(cn, "aaa");
    }

    private static void CalcD8HourMax(
      OleDbConnection cnDryDepDB,
      string sPoll,
      Dictionary<string, List<PollutantMonitor>> dict,
      bool pm)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        DryDeposition[] ddArr = new DryDeposition[24];
        List<PollutantMonitor> pollutantMonitorList = new List<PollutantMonitor>();
        if (sPoll == "PM10*")
          dict.TryGetValue("PM10", out pollutantMonitorList);
        else
          dict.TryGetValue(sPoll, out pollutantMonitorList);
        oleDbCommand.Connection = cnDryDepDB;
        if (pollutantMonitorList.Count == 0)
          pollutantMonitorList.Add(new PollutantMonitor()
          {
            MonState = "Null",
            MonCounty = "Null",
            MonSiteID = "Null"
          });
        for (int index1 = 0; index1 < pollutantMonitorList.Count; ++index1)
        {
          List<DryDeposition> list = new List<DryDeposition>();
          List<AirPollutantMetrics> pollutantMetricsList = new List<AirPollutantMetrics>();
          DryDeposition.ReadPollutant(cnDryDepDB, sPoll, pollutantMonitorList[index1], list);
          int num = list.Count / 24;
          int index2 = 0;
          while (index2 < list.Count)
          {
            for (int index3 = 0; index3 < 24; ++index3)
            {
              ddArr[index3] = list[index2];
              ++index2;
            }
            AirPollutantMetrics pollutantMetrics = new AirPollutantMetrics()
            {
              TimeStamp = ddArr[0].TimeStamp.Date,
              Pollutant = ddArr[0].Pollutant,
              MonState = pollutantMonitorList[index1].MonState,
              MonCounty = pollutantMonitorList[index1].MonCounty,
              MonSiteID = pollutantMonitorList[index1].MonSiteID
            };
            if (pm)
              AirPollutantMetrics.CalcMax8HrMovAvgPM(ddArr, ref pollutantMetrics.D8HourMax, ref pollutantMetrics.D8HourMaxChg, ref pollutantMetrics.D8HourMaxChgMin, ref pollutantMetrics.D8HourMaxChgMax);
            else
              AirPollutantMetrics.CalcMax8HrMovAvg(ddArr, ref pollutantMetrics.D8HourMax, ref pollutantMetrics.D8HourMaxChg, ref pollutantMetrics.D8HourMaxChgMin, ref pollutantMetrics.D8HourMaxChgMax);
            pollutantMetricsList.Add(pollutantMetrics);
          }
          oleDbCommand.CommandText = "CREATE TABLE aaa ([TimeStamp] DateTime,[Pollutant] TEXT (5),[MonitorState] TEXT (10),[MonitorCounty] TEXT (10),[MonitorSiteID] TEXT (10),[D8HourMax] DOUBLE,[D8HourMaxChg] DOUBLE,[D8HourMaxChgMin] DOUBLE,[D8HourMaxChgMax] DOUBLE);";
          oleDbCommand.ExecuteNonQuery();
          for (int index4 = 0; index4 < num; ++index4)
          {
            oleDbCommand.CommandText = "INSERT INTO aaa ([TimeStamp],[Pollutant],[MonitorState],[MonitorCounty],[MonitorSiteID],[D8HourMax],[D8HourMaxChg],[D8HourMaxChgMin],[D8HourMaxChgMax]) Values (#" + pollutantMetricsList[index4].TimeStamp.ToString() + "#,\"" + pollutantMetricsList[index4].Pollutant + "\",\"" + pollutantMetricsList[index4].MonState + "\",\"" + pollutantMetricsList[index4].MonCounty + "\",\"" + pollutantMetricsList[index4].MonSiteID + "\"," + pollutantMetricsList[index4].D8HourMax.ToString() + "," + pollutantMetricsList[index4].D8HourMaxChg.ToString() + "," + pollutantMetricsList[index4].D8HourMaxChgMin.ToString() + "," + pollutantMetricsList[index4].D8HourMaxChgMax.ToString() + ");";
            oleDbCommand.ExecuteNonQuery();
          }
          oleDbCommand.CommandText = "UPDATE AirPollutantMetrics INNER JOIN aaa ON AirPollutantMetrics.[TimeStamp] = aaa.[TimeStamp] AND AirPollutantMetrics.Pollutant = [aaa].[Pollutant] AND AirPollutantMetrics.MonitorState = [aaa].[MonitorState] AND AirPollutantMetrics.MonitorCounty = [aaa].[MonitorCounty] AND AirPollutantMetrics.MonitorSiteID = [aaa].[MonitorSiteID] SET AirPollutantMetrics.D8HourMax = [aaa].[D8HourMax],AirPollutantMetrics.D8HourMaxChg = [aaa].[D8HourMaxChg],AirPollutantMetrics.D8HourMaxChgMin = [aaa].[D8HourMaxChgMin],AirPollutantMetrics.D8HourMaxChgMax = [aaa].[D8HourMaxChgMax];";
          oleDbCommand.ExecuteNonQuery();
          AccessFunc.RemoveTable(cnDryDepDB, "aaa");
        }
      }
    }

    private static void CalcMax8HrMovAvg(
      DryDeposition[] ddArr,
      ref double max,
      ref double maxChg,
      ref double maxChgMin,
      ref double maxChgMax)
    {
      double[] numArray1 = new double[17];
      double[] numArray2 = new double[17];
      double[] numArray3 = new double[17];
      double[] numArray4 = new double[17];
      for (int index1 = 0; index1 < 17; ++index1)
      {
        for (int index2 = 0; index2 < 8; ++index2)
        {
          numArray1[index1] += ddArr[index1 + index2].PPM * 1000.0;
          numArray2[index1] += ddArr[index1 + index2].ConcChg * 1000.0;
          numArray3[index1] += ddArr[index1 + index2].ConcChgMin * 1000.0;
          numArray4[index1] += ddArr[index1 + index2].ConcChgMax * 1000.0;
        }
        numArray1[index1] /= 8.0;
        numArray2[index1] /= 8.0;
        numArray3[index1] /= 8.0;
        numArray4[index1] /= 8.0;
      }
      max = maxChg = maxChgMin = maxChgMax = 0.0;
      for (int index = 0; index < 17; ++index)
      {
        if (max < numArray1[index])
          max = numArray1[index];
        if (maxChg < numArray2[index])
          maxChg = numArray2[index];
        if (maxChgMin < numArray3[index])
          maxChgMin = numArray3[index];
        if (maxChgMax < numArray4[index])
          maxChgMax = numArray4[index];
      }
    }

    private static void CalcMax8HrMovAvgPM(
      DryDeposition[] ddArr,
      ref double max,
      ref double maxChg,
      ref double maxChgMin,
      ref double maxChgMax)
    {
      double[] numArray1 = new double[17];
      double[] numArray2 = new double[17];
      double[] numArray3 = new double[17];
      double[] numArray4 = new double[17];
      for (int index1 = 0; index1 < 17; ++index1)
      {
        for (int index2 = 0; index2 < 8; ++index2)
        {
          numArray1[index1] += ddArr[index1 + index2].uGm3;
          numArray2[index1] += ddArr[index1 + index2].ConcChg;
          numArray3[index1] += ddArr[index1 + index2].ConcChgMin;
          numArray4[index1] += ddArr[index1 + index2].ConcChgMax;
        }
        numArray1[index1] /= 8.0;
        numArray2[index1] /= 8.0;
        numArray3[index1] /= 8.0;
        numArray4[index1] /= 8.0;
      }
      max = maxChg = maxChgMin = maxChgMax = 0.0;
      for (int index = 0; index < 17; ++index)
      {
        if (max < numArray1[index])
          max = numArray1[index];
        if (maxChg < numArray2[index])
          maxChg = numArray2[index];
        if (maxChgMin < numArray3[index])
          maxChgMin = numArray3[index];
        if (maxChgMax < numArray4[index])
          maxChgMax = numArray4[index];
      }
    }

    private static void CalcMetricsAnnualMean(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Year([TimeStamp]) AS [Year], Pollutant, MonitorState, MonitorCounty, MonitorSiteID, Avg(D1HourMax) AS [D1HourMax],  Avg(D3HourMean) AS [D3HourMean], Avg(D4HourMean) AS [D4HourMean], Avg(D8HourMax) AS [D8HourMax], Avg(D8HourMean) AS [D8HourMean], Avg(D24HourMean) AS [D24HourMean], Avg(D1HourMaxChg) AS [D1HourMaxChg], Avg(D1HourMaxChgMin) AS [D1HourMaxChgMin], Avg(D1HourMaxChgMax) AS [D1HourMaxChgMax], Avg(D3HourMeanChg) AS [D3HourMeanChg], Avg(D3HourMeanChgMin) AS [D3HourMeanChgMin], Avg(D3HourMeanChgMax) AS [D3HourMeanChgMax],  Avg(D4HourMeanChg) AS [D4HourMeanChg], Avg(D4HourMeanChgMin) AS [D4HourMeanChgMin], Avg(D4HourMeanChgMax) AS [D4HourMeanChgMax],  Avg(D8HourMaxChg) AS [D8HourMaxChg], Avg(D8HourMaxChgMin) AS [D8HourMaxChgMin], Avg(D8HourMaxChgMax) AS [D8HourMaxChgMax],  Avg(D8HourMeanChg) AS [D8HourMeanChg], Avg(D8HourMeanChgMin) AS [D8HourMeanChgMin], Avg(D8HourMeanChgMax) AS [D8HourMeanChgMax],  Avg(D24HourMeanChg) AS [D24HourMeanChg], Avg(D24HourMeanChgMin) AS [D24HourMeanChgMin], Avg(D24HourMeanChgMax) AS [D24HourMeanChgMax] INTO 10_AirPollutantMetricsAnnualMean FROM AirPollutantMetrics GROUP BY Year([TimeStamp]), Pollutant, MonitorState, MonitorCounty, MonitorSiteID ORDER BY Pollutant, MonitorState, MonitorCounty, MonitorSiteID;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcMetricsQuarterlyMean(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Format$([TimeStamp],\"\\Qq yyyy\") AS [Quarter], Pollutant, MonitorState, MonitorCounty, MonitorSiteID, Avg(D24HourMean) AS [D24HourMean],  Avg(D24HourMeanChg) AS [D24HourMeanChg], Avg(D24HourMeanChgMin) AS [D24HourMeanChgMin], Avg(D24HourMeanChgMax) AS [D24HourMeanChgMax] INTO 11_AirPollutantMetricsQuarterlyMean FROM AirPollutantMetrics GROUP BY Format$([TimeStamp],\"\\Qq yyyy\"), Pollutant, MonitorState, MonitorCounty, MonitorSiteID ORDER BY Pollutant, MonitorState, MonitorCounty, MonitorSiteID, Format$([TimeStamp],\"\\Qq yyyy\");";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcConcChgAnnualMean(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Pollutant, MonitorState, MonitorCounty, MonitorSiteID, Avg([ConcChg]*1000) AS AvgConcChg,Avg([ConcChgMin]*1000) AS AvgConcChgMin, Avg([ConcChgMax]*1000) AS AvgConcChgMax INTO 12_ConcentrationChangeAnnualMean FROM DryDeposition GROUP BY Pollutant, MonitorState, MonitorCounty, MonitorSiteID HAVING Pollutant=\"CO\" OR Pollutant=\"NO2\" OR Pollutant=\"O3\" OR Pollutant=\"SO2\";";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO 12_ConcentrationChangeAnnualMean ( Pollutant, MonitorState, MonitorCounty, MonitorSiteID, AvgConcChg, AvgConcChgMin, AvgConcChgMax) SELECT DISTINCTROW Pollutant, MonitorState, MonitorCounty, MonitorSiteID, Avg(ConcChg) AS AvgConcChg, Avg(ConcChgMin) AS AvgConcChgMin, Avg(ConcChgMax) AS AvgConcChgMax FROM DryDeposition GROUP BY Pollutant, MonitorState, MonitorCounty, MonitorSiteID HAVING Pollutant=\"PM10*\" Or Pollutant=\"PM2.5\";";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void CalcConcChgLeafOnMean(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Pollutant, MonitorState, MonitorCounty, MonitorSiteID,  Avg([ConcChg]*1000) AS AvgConcChg, Avg([ConcChgMin]*1000) AS AvgConcChgMin, Avg([ConcChgMax]*1000) AS AvgConcChgMax INTO 13_ConcentrationChangeLeafOnMean FROM DryDeposition GROUP BY Pollutant, MonitorState, MonitorCounty, MonitorSiteID, GrowSeason HAVING Pollutant=\"CO\" AND GrowSeason=True OR Pollutant=\"NO2\" AND GrowSeason=True OR Pollutant=\"O3\" AND GrowSeason=True OR Pollutant=\"SO2\" AND GrowSeason=True;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO 13_ConcentrationChangeLeafOnMean ( Pollutant, MonitorState, MonitorCounty, MonitorSiteID, AvgConcChg, AvgConcChgMin, AvgConcChgMax ) SELECT DISTINCTROW Pollutant, MonitorState, MonitorCounty, MonitorSiteID, Avg(ConcChg) AS AvgConcChg, Avg(ConcChgMin) AS AvgConcChgMin, Avg(ConcChgMax) AS AvgConcChgMax FROM DryDeposition GROUP BY Pollutant, MonitorState, MonitorCounty, MonitorSiteID, GrowSeason HAVING Pollutant=\"PM10*\" AND GrowSeason=True Or Pollutant=\"PM2.5\" AND GrowSeason=True;";
        oleDbCommand.ExecuteNonQuery();
      }
    }
  }
}
