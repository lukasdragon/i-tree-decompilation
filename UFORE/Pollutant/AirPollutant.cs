// Decompiled with JetBrains decompiler
// Type: UFORE.Pollutant.AirPollutant
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Deposition;
using UFORE.Location;
using UFORE.Weather;

namespace UFORE.Pollutant
{
  public class AirPollutant
  {
    private const string POLL_TABLE = "AirPollutant";
    private DateTime TimeStamp;
    private string Pollutant;
    public string State;
    public string County;
    public string SiteID;
    private int Unit;
    private double SampleValue;
    public double PPM;
    public double uGm3;

    [Obsolete]
    public static void ProcessAirPollutant(
      string sCODB,
      string sNO2DB,
      string sO3DB,
      string sPM10DB,
      string sPM25DB,
      string sSO2DB,
      string sSiteDB,
      LocationData locData,
      string sWeatherDB)
    {
      AirPollutant.ProcessAirPollutant(sCODB, sNO2DB, sO3DB, sPM10DB, sPM25DB, sSO2DB, sSiteDB, locData, sWeatherDB, (UFORE_D) null, 0, 0);
    }

    [Obsolete]
    public static void ProcessAirPollutant(
      string sCODB,
      string sNO2DB,
      string sO3DB,
      string sPM10DB,
      string sPM25DB,
      string sSO2DB,
      string sSiteDB,
      LocationData locData,
      string sWeatherDB,
      UFORE_D uforeDObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      AccessFunc.CreateDB(sSiteDB);
      using (OleDbConnection cnWeatherDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sWeatherDB))
      {
        using (OleDbConnection cnSiteDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sSiteDB))
        {
          cnWeatherDB.Open();
          cnSiteDB.Open();
          AirPollutant.ProcessAirPollutant(sCODB, sNO2DB, sO3DB, sPM10DB, sPM25DB, sSO2DB, cnSiteDB, locData, cnWeatherDB, uforeDObj, PercentRangeFrom, PercentRangeTo);
        }
      }
    }

    public static void ProcessAirPollutant(
      string sCODB,
      string sNO2DB,
      string sO3DB,
      string sPM10DB,
      string sPM25DB,
      string sSO2DB,
      OleDbConnection cnSiteDB,
      LocationData locData,
      OleDbConnection cnWeatherDB,
      UFORE_D uforeDObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      List<SurfaceWeather> sfcList = new List<SurfaceWeather>();
      SurfaceWeather.ReadSurfaceAllRecords(cnWeatherDB, ref sfcList);
      List<Action> actionList = new List<Action>()
      {
        (Action) (() => AirPollutant.CreateAirPollutantTable(cnSiteDB)),
        (Action) (() => AirPollutant.ExtractSiteRecords(sCODB, cnSiteDB, "CO", locData.PollMonDict, ref sfcList)),
        (Action) (() => AirPollutant.ExtractSiteRecords(sNO2DB, cnSiteDB, "NO2", locData.PollMonDict, ref sfcList)),
        (Action) (() => AirPollutant.ExtractSiteRecords(sO3DB, cnSiteDB, "O3", locData.PollMonDict, ref sfcList)),
        (Action) (() => AirPollutant.ExtractSiteRecords(sPM10DB, cnSiteDB, "PM10", locData.PollMonDict, ref sfcList)),
        (Action) (() => AirPollutant.ExtractSiteRecords(sPM25DB, cnSiteDB, "PM2.5", locData.PollMonDict, ref sfcList)),
        (Action) (() => AirPollutant.ExtractSiteRecords(sSO2DB, cnSiteDB, "SO2", locData.PollMonDict, ref sfcList)),
        (Action) (() => AirPollutant.ConvertPM10toPM10Star(cnSiteDB, locData.PollMonDict))
      };
      double num1 = (double) (PercentRangeTo - PercentRangeFrom) / (double) actionList.Count;
      int num2 = 0;
      while (num2 < actionList.Count)
      {
        actionList[num2++]();
        uforeDObj?.reportProgress((int) ((double) PercentRangeFrom + (double) num2 * num1));
      }
    }

    public static void CreateAirPollutantTable(OleDbConnection conn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.CommandText = "CREATE TABLE AirPollutant([TimeStamp] DateTime,[Pollutant] TEXT (5),[State] TEXT (10),[County] TEXT (10),[SiteID] TEXT (10),[PPM] DOUBLE,[uGm3] DOUBLE);";
        oleDbCommand.Connection = conn;
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void CreateAirPollutantTable(string sDB)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        AirPollutant.CreateAirPollutantTable(conn);
      }
    }

    public static void ExtractSiteRecords(
      string sMasterDB,
      OleDbConnection cnSiteDB,
      string sPoll,
      Dictionary<string, List<PollutantMonitor>> dict,
      ref List<SurfaceWeather> sfcList)
    {
      List<PollutantMonitor> pollutantMonitorList = new List<PollutantMonitor>();
      dict.TryGetValue(sPoll, out pollutantMonitorList);
      if (pollutantMonitorList.Count <= 0 || string.IsNullOrEmpty(sMasterDB))
        return;
      using (OleDbConnection cnMasterDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sMasterDB))
      {
        cnMasterDB.Open();
        for (int index = 0; index < pollutantMonitorList.Count; ++index)
        {
          if (pollutantMonitorList[index].MonState != "Null")
          {
            string sTable = pollutantMonitorList[index].MonState + "_" + pollutantMonitorList[index].MonCounty + "_" + pollutantMonitorList[index].MonSiteID;
            List<AirPollutant> airPollutantList1 = new List<AirPollutant>();
            int records = AirPollutant.ExtractRecords(cnMasterDB, sTable, sPoll, ref airPollutantList1);
            List<AirPollutant> airPollutantList2 = new List<AirPollutant>();
            int recordsWithinPeriod = AirPollutant.ExtractRecordsWithinPeriod(ref airPollutantList1, records, ref airPollutantList2, ref sfcList);
            AirPollutant.ConvertSampleUnit(sPoll, ref airPollutantList2, recordsWithinPeriod, ref sfcList);
            AirPollutant.WriteAirPollutantRecords(cnSiteDB, ref airPollutantList2, recordsWithinPeriod);
          }
        }
      }
    }

    [Obsolete]
    public static void ExtractSiteRecords(
      string sMasterDB,
      string sSiteDB,
      string sPoll,
      Dictionary<string, List<PollutantMonitor>> dict,
      ref List<SurfaceWeather> sfcList)
    {
      using (OleDbConnection cnSiteDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sSiteDB))
      {
        cnSiteDB.Open();
        AirPollutant.ExtractSiteRecords(sMasterDB, cnSiteDB, sPoll, dict, ref sfcList);
      }
    }

    [Obsolete]
    private static int ExtractRecords(
      string sDB,
      string sTable,
      string sPoll,
      ref List<AirPollutant> list)
    {
      using (OleDbConnection cnMasterDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sDB))
      {
        cnMasterDB.Open();
        return AirPollutant.ExtractRecords(cnMasterDB, sTable, sPoll, ref list);
      }
    }

    private static int ExtractRecords(
      OleDbConnection cnMasterDB,
      string sTable,
      string sPoll,
      ref List<AirPollutant> list)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnMasterDB;
        oleDbCommand.CommandText = "SELECT * FROM " + sTable + " ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            AirPollutant airPollutant = new AirPollutant()
            {
              TimeStamp = (DateTime) oleDbDataReader["TimeStamp"],
              Pollutant = sPoll,
              State = (string) oleDbDataReader["State"],
              County = (string) oleDbDataReader["County"],
              SiteID = (string) oleDbDataReader["SiteID"],
              Unit = (int) oleDbDataReader["Unit"],
              SampleValue = (double) oleDbDataReader["SampleValue"]
            };
            if (airPollutant.SampleValue < 0.0)
              airPollutant.SampleValue = 0.0;
            list.Add(airPollutant);
          }
        }
      }
      return list.Count;
    }

    private static int ExtractRecordsWithinPeriod(
      ref List<AirPollutant> polIntList,
      int polCnt,
      ref List<AirPollutant> polFinList,
      ref List<SurfaceWeather> sfcList)
    {
      int num1 = 0;
      int num2 = 0;
      try
      {
        TimeSpan timeSpan;
        for (int index = 0; index < polCnt; ++index)
        {
          timeSpan = polIntList[index].TimeStamp - sfcList[0].TimeStamp;
          if (timeSpan.TotalHours == 0.0)
          {
            num1 = index;
            break;
          }
        }
        for (int index = polCnt - 1; index >= 0; --index)
        {
          timeSpan = polIntList[index].TimeStamp - sfcList[sfcList.Count - 1].TimeStamp;
          if (timeSpan.TotalHours == 0.0)
          {
            num2 = index;
            break;
          }
        }
        int recordsWithinPeriod = num2 - num1 + 1;
        for (int index = num1; index <= num2; ++index)
          polFinList.Add(new AirPollutant()
          {
            TimeStamp = polIntList[index].TimeStamp,
            Pollutant = polIntList[index].Pollutant,
            State = polIntList[index].State,
            County = polIntList[index].County,
            SiteID = polIntList[index].SiteID,
            Unit = polIntList[index].Unit,
            SampleValue = polIntList[index].SampleValue
          });
        return recordsWithinPeriod;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private static void ConvertSampleUnit(
      string sPoll,
      ref List<AirPollutant> polList,
      int polCnt,
      ref List<SurfaceWeather> sfcList)
    {
      for (int index = 0; index < polCnt; ++index)
      {
        if (!(sPoll == "CO"))
        {
          if (!(sPoll == "NO2"))
          {
            if (!(sPoll == "O3"))
            {
              if (!(sPoll == "PM10"))
              {
                if (!(sPoll == "PM2.5"))
                {
                  if (sPoll == "SO2")
                  {
                    switch (polList[index].Unit)
                    {
                      case 1:
                        polList[index].uGm3 = polList[index].SampleValue;
                        polList[index].PPM = polList[index].uGm3 * (0.08314 * sfcList[index].TempK) / (sfcList[index].PrsMbar * 64.0588);
                        continue;
                      case 3:
                        polList[index].uGm3 = polList[index].SampleValue * Math.Pow(10.0, -3.0);
                        polList[index].PPM = polList[index].uGm3 * (0.08314 * sfcList[index].TempK) / (sfcList[index].PrsMbar * 64.0588);
                        continue;
                      case 5:
                        polList[index].uGm3 = polList[index].SampleValue * Math.Pow(10.0, 3.0);
                        polList[index].PPM = polList[index].uGm3 * (0.08314 * sfcList[index].TempK) / (sfcList[index].PrsMbar * 64.0588);
                        continue;
                      case 7:
                        polList[index].PPM = polList[index].SampleValue;
                        polList[index].uGm3 = polList[index].PPM * sfcList[index].PrsMbar * 64.0588 / (0.08314 * sfcList[index].TempK);
                        continue;
                      case 8:
                        polList[index].PPM = polList[index].SampleValue * Math.Pow(10.0, -3.0);
                        polList[index].uGm3 = polList[index].PPM * sfcList[index].PrsMbar * 64.0588 / (0.08314 * sfcList[index].TempK);
                        continue;
                      case 87:
                        polList[index].PPM = polList[index].SampleValue * Math.Pow(10.0, -1.0);
                        polList[index].uGm3 = polList[index].PPM * sfcList[index].PrsMbar * 64.0588 / (0.08314 * sfcList[index].TempK);
                        continue;
                      default:
                        continue;
                    }
                  }
                }
                else
                {
                  switch (polList[index].Unit)
                  {
                    case 1:
                      polList[index].PPM = 0.0;
                      polList[index].uGm3 = polList[index].SampleValue;
                      continue;
                    case 3:
                      polList[index].PPM = 0.0;
                      polList[index].uGm3 = polList[index].SampleValue * Math.Pow(10.0, -3.0);
                      continue;
                    case 5:
                      polList[index].PPM = 0.0;
                      polList[index].uGm3 = polList[index].SampleValue * Math.Pow(10.0, 3.0);
                      continue;
                    case 7:
                      polList[index].PPM = polList[index].SampleValue;
                      polList[index].uGm3 = 0.0;
                      continue;
                    case 8:
                      polList[index].PPM = polList[index].SampleValue * Math.Pow(10.0, -3.0);
                      polList[index].uGm3 = 0.0;
                      continue;
                    case 87:
                      polList[index].PPM = polList[index].SampleValue * Math.Pow(10.0, -1.0);
                      polList[index].uGm3 = 0.0;
                      continue;
                    case 105:
                      polList[index].PPM = 0.0;
                      polList[index].uGm3 = polList[index].SampleValue;
                      continue;
                    default:
                      continue;
                  }
                }
              }
              else
              {
                switch (polList[index].Unit)
                {
                  case 1:
                    polList[index].PPM = 0.0;
                    polList[index].uGm3 = polList[index].SampleValue;
                    continue;
                  case 3:
                    polList[index].PPM = 0.0;
                    polList[index].uGm3 = polList[index].SampleValue * Math.Pow(10.0, -3.0);
                    continue;
                  case 5:
                    polList[index].PPM = 0.0;
                    polList[index].uGm3 = polList[index].SampleValue * Math.Pow(10.0, 3.0);
                    continue;
                  case 7:
                    polList[index].PPM = polList[index].SampleValue;
                    polList[index].uGm3 = 0.0;
                    continue;
                  case 8:
                    polList[index].PPM = polList[index].SampleValue * Math.Pow(10.0, -3.0);
                    polList[index].uGm3 = 0.0;
                    continue;
                  case 87:
                    polList[index].PPM = polList[index].SampleValue * Math.Pow(10.0, -1.0);
                    polList[index].uGm3 = 0.0;
                    continue;
                  case 105:
                    polList[index].PPM = 0.0;
                    polList[index].uGm3 = polList[index].SampleValue;
                    continue;
                  default:
                    continue;
                }
              }
            }
            else
            {
              switch (polList[index].Unit)
              {
                case 1:
                  polList[index].uGm3 = polList[index].SampleValue;
                  polList[index].PPM = polList[index].uGm3 * (0.08314 * sfcList[index].TempK) / (sfcList[index].PrsMbar * 47.9982);
                  continue;
                case 3:
                  polList[index].uGm3 = polList[index].SampleValue * Math.Pow(10.0, -3.0);
                  polList[index].PPM = polList[index].uGm3 * (0.08314 * sfcList[index].TempK) / (sfcList[index].PrsMbar * 47.9982);
                  continue;
                case 5:
                  polList[index].uGm3 = polList[index].SampleValue * Math.Pow(10.0, 3.0);
                  polList[index].PPM = polList[index].uGm3 * (0.08314 * sfcList[index].TempK) / (sfcList[index].PrsMbar * 47.9982);
                  continue;
                case 7:
                  polList[index].PPM = polList[index].SampleValue;
                  polList[index].uGm3 = polList[index].PPM * sfcList[index].PrsMbar * 47.9982 / (0.08314 * sfcList[index].TempK);
                  continue;
                case 8:
                  polList[index].PPM = polList[index].SampleValue * Math.Pow(10.0, -3.0);
                  polList[index].uGm3 = polList[index].PPM * sfcList[index].PrsMbar * 47.9982 / (0.08314 * sfcList[index].TempK);
                  continue;
                case 87:
                  polList[index].PPM = polList[index].SampleValue * Math.Pow(10.0, -1.0);
                  polList[index].uGm3 = polList[index].PPM * sfcList[index].PrsMbar * 47.9982 / (0.08314 * sfcList[index].TempK);
                  continue;
                default:
                  continue;
              }
            }
          }
          else
          {
            switch (polList[index].Unit)
            {
              case 1:
                polList[index].uGm3 = polList[index].SampleValue;
                polList[index].PPM = polList[index].uGm3 * (0.08314 * sfcList[index].TempK) / (sfcList[index].PrsMbar * 46.0055);
                continue;
              case 3:
                polList[index].uGm3 = polList[index].SampleValue * Math.Pow(10.0, -3.0);
                polList[index].PPM = polList[index].uGm3 * (0.08314 * sfcList[index].TempK) / (sfcList[index].PrsMbar * 46.0055);
                continue;
              case 5:
                polList[index].uGm3 = polList[index].SampleValue * Math.Pow(10.0, 3.0);
                polList[index].PPM = polList[index].uGm3 * (0.08314 * sfcList[index].TempK) / (sfcList[index].PrsMbar * 46.0055);
                continue;
              case 7:
                polList[index].PPM = polList[index].SampleValue;
                polList[index].uGm3 = polList[index].PPM * sfcList[index].PrsMbar * 46.0055 / (0.08314 * sfcList[index].TempK);
                continue;
              case 8:
                polList[index].PPM = polList[index].SampleValue * Math.Pow(10.0, -3.0);
                polList[index].uGm3 = polList[index].PPM * sfcList[index].PrsMbar * 46.0055 / (0.08314 * sfcList[index].TempK);
                continue;
              case 87:
                polList[index].PPM = polList[index].SampleValue * Math.Pow(10.0, -1.0);
                polList[index].uGm3 = polList[index].PPM * sfcList[index].PrsMbar * 46.0055 / (0.08314 * sfcList[index].TempK);
                continue;
              default:
                continue;
            }
          }
        }
        else
        {
          switch (polList[index].Unit)
          {
            case 1:
              polList[index].uGm3 = polList[index].SampleValue;
              polList[index].PPM = polList[index].uGm3 * (0.08314 * sfcList[index].TempK) / (sfcList[index].PrsMbar * 28.0104);
              continue;
            case 3:
              polList[index].uGm3 = polList[index].SampleValue * Math.Pow(10.0, -3.0);
              polList[index].PPM = polList[index].uGm3 * (0.08314 * sfcList[index].TempK) / (sfcList[index].PrsMbar * 28.0104);
              continue;
            case 5:
              polList[index].uGm3 = polList[index].SampleValue * Math.Pow(10.0, 3.0);
              polList[index].PPM = polList[index].uGm3 * (0.08314 * sfcList[index].TempK) / (sfcList[index].PrsMbar * 28.0104);
              continue;
            case 7:
              polList[index].PPM = polList[index].SampleValue;
              polList[index].uGm3 = polList[index].PPM * sfcList[index].PrsMbar * 28.0104 / (0.08314 * sfcList[index].TempK);
              continue;
            case 8:
              polList[index].PPM = polList[index].SampleValue * Math.Pow(10.0, -3.0);
              polList[index].uGm3 = polList[index].PPM * sfcList[index].PrsMbar * 28.0104 / (0.08314 * sfcList[index].TempK);
              continue;
            case 87:
              polList[index].PPM = polList[index].SampleValue * Math.Pow(10.0, -1.0);
              polList[index].uGm3 = polList[index].PPM * sfcList[index].PrsMbar * 28.0104 / (0.08314 * sfcList[index].TempK);
              continue;
            default:
              continue;
          }
        }
      }
    }

    private static void WriteAirPollutantRecords(
      OleDbConnection cnSiteDB,
      ref List<AirPollutant> polList,
      int recCnt)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnSiteDB;
        oleDbCommand.CommandText = "INSERT INTO AirPollutant([TimeStamp],[Pollutant],[State],[County],[SiteID],[PPM],[uGm3]) Values (@TimeStamp,@Pollutant,@State,@County,@SiteID,@PPM,@uGm3)";
        oleDbCommand.Parameters.Add("@TimeStamp", OleDbType.Date);
        oleDbCommand.Parameters.Add("@Pollutant", OleDbType.VarChar);
        oleDbCommand.Parameters.Add("@State", OleDbType.VarChar);
        oleDbCommand.Parameters.Add("@County", OleDbType.VarChar);
        oleDbCommand.Parameters.Add("@SiteID", OleDbType.VarChar);
        oleDbCommand.Parameters.Add("@PPM", OleDbType.Double);
        oleDbCommand.Parameters.Add("@uGm3", OleDbType.Double);
        for (int index = 0; index < recCnt; ++index)
        {
          oleDbCommand.Parameters["@TimeStamp"].Value = (object) polList[index].TimeStamp;
          oleDbCommand.Parameters["@Pollutant"].Value = (object) polList[index].Pollutant;
          oleDbCommand.Parameters["@State"].Value = (object) polList[index].State;
          oleDbCommand.Parameters["@County"].Value = (object) polList[index].County;
          oleDbCommand.Parameters["@SiteID"].Value = (object) polList[index].SiteID;
          oleDbCommand.Parameters["@PPM"].Value = (object) polList[index].PPM;
          oleDbCommand.Parameters["@uGm3"].Value = (object) polList[index].uGm3;
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    private static void ConvertPM10toPM10Star(
      OleDbConnection cnSiteDB,
      Dictionary<string, List<PollutantMonitor>> dict)
    {
      int pm10idx = 0;
      int pm25idx = 0;
      List<PollutantMonitor> pm10mon = new List<PollutantMonitor>();
      List<PollutantMonitor> pm25mon = new List<PollutantMonitor>();
      dict.TryGetValue("PM10", out pm10mon);
      dict.TryGetValue("PM2.5", out pm25mon);
      if (pm25mon.Count <= 0)
        return;
      for (int index1 = 0; index1 < pm10mon.Count; ++index1)
      {
        double num1 = 1000000.0;
        for (int index2 = 0; index2 < pm25mon.Count; ++index2)
        {
          double num2 = Math.Sqrt(Math.Pow(pm10mon[index1].MonLat - pm25mon[index2].MonLat, 2.0) + Math.Pow(pm10mon[index1].MonLong - pm25mon[index2].MonLong, 2.0));
          if (num2 < num1)
          {
            num1 = num2;
            pm10idx = index1;
            pm25idx = index2;
          }
        }
        AirPollutant.UpdatePM10toPM10Star(cnSiteDB, pm10mon, pm10idx, pm25mon, pm25idx);
      }
    }

    public static void UpdatePM10toPM10Star(
      OleDbConnection conn,
      List<PollutantMonitor> pm10mon,
      int pm10idx,
      List<PollutantMonitor> pm25mon,
      int pm25idx)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT * INTO PM10 FROM AirPollutant WHERE Pollutant=\"PM10\"  AND State=\"" + pm10mon[pm10idx].MonState + "\" AND County=\"" + pm10mon[pm10idx].MonCounty + "\" AND SiteID=\"" + pm10mon[pm10idx].MonSiteID + "\" ORDER BY TimeStamp;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT * INTO PM25 FROM AirPollutant WHERE Pollutant=\"PM2.5\"  AND State=\"" + pm25mon[pm25idx].MonState + "\" AND County=\"" + pm25mon[pm25idx].MonCounty + "\" AND SiteID=\"" + pm25mon[pm25idx].MonSiteID + "\" ORDER BY TimeStamp;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "UPDATE (AirPollutant AS P INNER JOIN PM10 ON (P.SiteID = PM10.SiteID) AND (P.County = PM10.County) AND (P.State = PM10.State) AND (P.Pollutant = PM10.Pollutant) AND (P.TimeStamp = PM10.TimeStamp)) INNER JOIN PM25 ON PM10.TimeStamp = PM25.TimeStamp SET P.[uGm3] = [PM10].[uGm3]-[PM25].[uGm3];";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "UPDATE AirPollutant SET [uGm3] = 0 WHERE Pollutant=\"PM10\" AND State=\"" + pm10mon[pm10idx].MonState + "\" AND County=\"" + pm10mon[pm10idx].MonCounty + "\" AND SiteID=\"" + pm10mon[pm10idx].MonSiteID + "\" AND [uGm3]<0;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "UPDATE AirPollutant SET Pollutant = \"PM10*\" WHERE Pollutant=\"PM10\" AND State=\"" + pm10mon[pm10idx].MonState + "\" AND County=\"" + pm10mon[pm10idx].MonCounty + "\" AND SiteID=\"" + pm10mon[pm10idx].MonSiteID + "\";";
        oleDbCommand.ExecuteNonQuery();
        AccessFunc.RemoveTable(conn, "PM25");
        AccessFunc.RemoveTable(conn, "PM10");
      }
    }

    [Obsolete]
    public static void UpdatePM10toPM10Star(
      string DB,
      List<PollutantMonitor> pm10mon,
      int pm10idx,
      List<PollutantMonitor> pm25mon,
      int pm25idx)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + DB))
      {
        conn.Open();
        AirPollutant.UpdatePM10toPM10Star(conn, pm10mon, pm10idx, pm25mon, pm25idx);
      }
    }

    public static int ReadPollAllRecords(
      OleDbConnection cnPolDB,
      string sPoll,
      ref List<AirPollutant> pollList)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnPolDB;
        oleDbCommand.CommandText = "SELECT * FROM AirPollutant WHERE Pollutant=\"" + sPoll + "\" ORDER BY State, County, SiteID, TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            AirPollutant airPollutant = new AirPollutant()
            {
              TimeStamp = (DateTime) oleDbDataReader["TimeStamp"],
              Pollutant = (string) oleDbDataReader["Pollutant"],
              State = (string) oleDbDataReader["State"],
              County = (string) oleDbDataReader["County"],
              SiteID = (string) oleDbDataReader["SiteID"],
              PPM = (double) oleDbDataReader["PPM"],
              uGm3 = (double) oleDbDataReader["uGm3"]
            };
            pollList.Add(airPollutant);
          }
        }
      }
      return pollList.Count;
    }

    [Obsolete]
    public static int ReadPollAllRecords(
      string sPolDB,
      string sPoll,
      ref List<AirPollutant> pollList)
    {
      using (OleDbConnection cnPolDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sPolDB))
      {
        cnPolDB.Open();
        return AirPollutant.ReadPollAllRecords(cnPolDB, sPoll, ref pollList);
      }
    }

    public static int ReadPollPartialRecords(
      OleDbConnection cnPollDB,
      string sPoll,
      ref List<AirPollutant> pollList,
      DateTime start,
      DateTime end)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnPollDB;
        oleDbCommand.CommandText = "SELECT * FROM AirPollutant WHERE TimeStamp >= #" + start.ToString() + "# AND TimeStamp<= #" + end.ToString() + "# AND Pollutant=\"" + sPoll + "\" ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
            pollList.Add(new AirPollutant()
            {
              TimeStamp = (DateTime) oleDbDataReader["TimeStamp"],
              Pollutant = (string) oleDbDataReader["Pollutant"],
              State = (string) oleDbDataReader["State)"],
              County = (string) oleDbDataReader["County"],
              SiteID = (string) oleDbDataReader["SiteID'"],
              PPM = (double) oleDbDataReader["PPM)"],
              uGm3 = (double) oleDbDataReader["uGm3"]
            });
        }
      }
      return pollList.Count;
    }

    [Obsolete]
    public static int ReadPollPartialRecords(
      string sPollDB,
      string sPoll,
      ref List<AirPollutant> pollList,
      DateTime start,
      DateTime end)
    {
      using (OleDbConnection cnPollDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sPollDB))
      {
        cnPollDB.Open();
        return AirPollutant.ReadPollPartialRecords(cnPollDB, sPoll, ref pollList, start, end);
      }
    }

    public static int SetNullPollData(int recCnt, ref List<AirPollutant> pollList)
    {
      for (int index = 0; index < recCnt; ++index)
      {
        AirPollutant airPollutant = new AirPollutant()
        {
          State = "Null",
          County = "Null",
          SiteID = "Null",
          PPM = 0.0,
          uGm3 = 0.0
        };
        pollList.Add(airPollutant);
      }
      return recCnt;
    }
  }
}
