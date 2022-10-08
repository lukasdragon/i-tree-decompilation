// Decompiled with JetBrains decompiler
// Type: UFORE.Weather.SurfaceWeather
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using UFORE.Deposition;
using UFORE.LAI;
using UFORE.Location;

namespace UFORE.Weather
{
  public class SurfaceWeather
  {
    private const double TEMP_CK = 273.15;
    private const double XLIM = 1E-09;
    private const double ABS_PRS = 1013.25;
    private const double ABS_TEMP = 288.15;
    private const int SOL_CONST = 1367;
    private const double ES_CONS = 0.6108;
    private const double ATRN = 0.957;
    private const double K1 = 0.1;
    private const double BA = 0.84;
    private const double PA_CONS = 3.486;
    private const double CHI_CONS = 2.501;
    private const double GAMMA_C = 0.0016286;
    private const double theta = 5.67E-08;
    private const double ES = 0.97;
    private const double ES_RN = 0.98;
    private const double CP = 1013.0;
    private const int ZCONS = 2;
    private const double rd = 0.00137;
    private const double rDS = 0.005;
    private const double rDT = 0.95;
    private const double rTreeHeight = 5.0;
    private const double rZom = 0.123;
    private const double rZov = 0.0123;
    private const double rZu = 2.0;
    private const double rZe = 2.0;
    private const double rZuT = 7.0;
    private const double rZuG = 0.10137;
    public const double LEAF_STORAGE_M = 0.0002;
    private const double IMPERV_STORAGE_M = 0.0015;
    private const double PERV_STORAGE_M = 0.001;
    public const double NODATA = -999.0;
    private const int NOWBAN = 99999;
    public const string SFC_TABLE = "SurfaceWeather";
    private const string HRMIX_TABLE = "HourlyMixHt";
    private const string NEW_INTERNATIONAL = "  USAF  WBAN YR--MODAHRMN DIR SPD GUS CLG SKC L M H  VSB WW WW WW ZZ ZZ ZZ W TEMP DEWP    SLP   ALT    STP MAX MIN PCP01 PCP06 PCP24 PCPXX SD";
    private const string NEW_US_CANADA = "  USAF  WBAN YR--MODAHRMN DIR SPD GUS CLG SKC L M H  VSB MW MW MW MW AW AW AW AW W TEMP DEWP    SLP   ALT    STP MAX MIN PCP01 PCP06 PCP24 PCPXX SD";
    private const string OLD = "  USAF  WBAN YR--MODAHRMN DIR SPD GUS CLG SKC L M H  VSB WW WW WW W TEMP DEWP    SLP   ALT    STP MAX MIN PCP01 PCP06 PCP24 PCPXX SD";
    private const string NARCCAP = "  USAF  WBAN YR--MODAHRMN DIR SPD GUS CLG SKC L M H  VSB WW WW WW W TEMP DEWP    SLP   ALT    STP MAX MIN PCP01      NETRAD PCPXX SD";
    private const int USAF = 0;
    private const int WBAN = 1;
    private const int DATETIME = 2;
    private const int DIR = 3;
    private const int SPD = 4;
    private const int GUS = 5;
    private const int CLG = 6;
    private const int SKC = 7;
    private const int TEMP = 16;
    private const int DEWP = 17;
    private const int SLP = 18;
    private const int ALT = 19;
    private const int STP = 20;
    private const int MAX = 21;
    private const int MIN = 22;
    private const int PCP01 = 23;
    private const int NETRAD = 24;
    private const int SD = 27;
    public DateTime TimeStamp;
    public int Jday;
    public int Year;
    public int Month;
    public int Day;
    public int Hour;
    public int Minute;
    public double AirDens;
    public double AirMass;
    public double Altimeter;
    public double Ceiling;
    public double DewTempC;
    public double DewTempF;
    public double DifRadWm2;
    public double DirRadWm2;
    public double GlbRadWm2;
    public double NetRadWm2;
    public double OpCldCov;
    public double PrsIn;
    public double PrsKpa;
    public double PrsMbar;
    public double PeGrMh;
    public double PeTrMh;
    public double PeSnGrMh;
    public double PeSnTrMh;
    public double PtTrMh;
    public double VegEvMh;
    public double VegStMh;
    public double VegIntcptMh;
    public double UnderCanThrufallMh;
    public double UnderCanPervEvMh;
    public double UnderCanPervStMh;
    public double UnderCanPervInfilMh;
    public double UnderCanImpervEvMh;
    public double UnderCanImpervStMh;
    public double UnderCanImpervRunoffMh;
    public double NoCanPervEvMh;
    public double NoCanPervStMh;
    public double NoCanPervInfilMh;
    public double NoCanImpervEvMh;
    public double NoCanImpervStMh;
    public double NoCanImpervRunoffMh;
    public double PARuEm2s;
    public double PARWm2;
    public double RainInH;
    public double RainIn6H;
    public double RainMh;
    public double RelHum;
    public double SatVPrsKpa;
    public double SnowIn;
    public double SnowM;
    public double SolZenAgl;
    public double TempC;
    public double TempF;
    public double TempK;
    public double ToCldCov;
    public double TrCldCov;
    public double VapPrsKpa;
    public double WdDir;
    public double WdSpdKnt;
    public double WdSpdMh;
    public double WdSpdMs;
    public static SurfaceWeather.WeatherDataFormat weaFormat;

    [Obsolete]
    public static void ProcessSurfaceWeatherData(
      string sSfcFile,
      LocationData locData,
      string laiDB,
      DryDeposition.VEG_TYPE VegType,
      int startYear,
      int endYear)
    {
      using (OleDbConnection cnLaiDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + laiDB))
      {
        cnLaiDB.Open();
        SurfaceWeather.ProcessSurfaceWeatherData(sSfcFile, locData, cnLaiDB, VegType, startYear, endYear);
      }
    }

    public static void ProcessSurfaceWeatherData(
      string sSfcFile,
      LocationData locData,
      OleDbConnection cnLaiDB,
      DryDeposition.VEG_TYPE VegType,
      int startYear,
      int endYear)
    {
      List<SurfaceWeather> weaList = new List<SurfaceWeather>();
      List<SurfaceWeather> surfaceWeatherList = new List<SurfaceWeather>();
      List<SurfaceWeather> finList = new List<SurfaceWeather>();
      List<LeafAreaIndex> laiList = new List<LeafAreaIndex>();
      SurfaceWeather.ReadSurfaceWeatherData(sSfcFile, ref weaList);
      SurfaceWeather.CheckPressure(weaList, startYear, endYear);
      SurfaceWeather.AdjustTimeStamp(weaList);
      int hourlyRecords = SurfaceWeather.CreateHourlyRecords(weaList, ref surfaceWeatherList, startYear, endYear);
      if (SurfaceWeather.No1HRain(ref surfaceWeatherList))
        SurfaceWeather.Disaggregate6HRain(surfaceWeatherList);
      SurfaceWeather.FillExtrapolate(surfaceWeatherList);
      SurfaceWeather.FillInterpolate(surfaceWeatherList);
      SurfaceWeather.ConvertData(surfaceWeatherList, locData.GMTOffset, hourlyRecords);
      int validData = SurfaceWeather.ExtractValidData(surfaceWeatherList, hourlyRecords, ref finList);
      SurfaceWeather.CalcAirDensity(finList, validData);
      SurfaceWeather.CalcSolarZenithAngle(finList, locData, validData);
      SurfaceWeather.CalcSolarRadiation(finList, locData, validData);
      LeafAreaIndex.ReadLAIPartialRecords(cnLaiDB, ref laiList, finList[0].TimeStamp, finList[validData - 1].TimeStamp);
      SurfaceWeather.CalcET(finList, laiList, validData);
      SurfaceWeather.CalcPrecipInterceptByCanopy(finList, laiList, validData, VegType);
      SurfaceWeather.CalcPrecipInterceptByUnderCanopyCover(finList, validData);
      SurfaceWeather.CalcPrecipInterceptByNoCanopyCover(finList, validData);
    }

    public static void ProcessSurfaceWeatherData(
      string sSfcFile,
      LocationData locData,
      string laiDB,
      int startYear,
      int endYear,
      string sWeatherDB)
    {
      AccessFunc.CreateDB(sWeatherDB);
      using (OleDbConnection cnWeatherDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sWeatherDB))
      {
        using (OleDbConnection cnLaiDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + laiDB))
        {
          cnWeatherDB.Open();
          cnLaiDB.Open();
          SurfaceWeather.ProcessSurfaceWeatherData(sSfcFile, locData, cnLaiDB, startYear, endYear, cnWeatherDB);
        }
      }
    }

    public static void ProcessSurfaceWeatherData(
      string sSfcFile,
      LocationData locData,
      OleDbConnection cnLaiDB,
      int startYear,
      int endYear,
      OleDbConnection cnWeatherDB)
    {
      List<SurfaceWeather> weaList = new List<SurfaceWeather>();
      List<SurfaceWeather> surfaceWeatherList = new List<SurfaceWeather>();
      List<SurfaceWeather> finList = new List<SurfaceWeather>();
      List<LeafAreaIndex> laiList = new List<LeafAreaIndex>();
      SurfaceWeather.ReadSurfaceWeatherData(sSfcFile, ref weaList);
      SurfaceWeather.CheckPressure(weaList, startYear, endYear);
      SurfaceWeather.AdjustTimeStamp(weaList);
      int hourlyRecords = SurfaceWeather.CreateHourlyRecords(weaList, ref surfaceWeatherList, startYear, endYear);
      if (SurfaceWeather.No1HRain(ref surfaceWeatherList))
        SurfaceWeather.Disaggregate6HRain(surfaceWeatherList);
      SurfaceWeather.FillExtrapolate(surfaceWeatherList);
      SurfaceWeather.FillInterpolate(surfaceWeatherList);
      SurfaceWeather.ConvertData(surfaceWeatherList, locData.GMTOffset, hourlyRecords);
      int validData = SurfaceWeather.ExtractValidData(surfaceWeatherList, hourlyRecords, ref finList);
      SurfaceWeather.CalcAirDensity(finList, validData);
      SurfaceWeather.CalcSolarZenithAngle(finList, locData, validData);
      SurfaceWeather.CalcSolarRadiation(finList, locData, validData);
      LeafAreaIndex.ReadLAIPartialRecords(cnLaiDB, ref laiList, finList[0].TimeStamp, finList[validData - 1].TimeStamp);
      SurfaceWeather.CalcET(finList, laiList, validData);
      SurfaceWeather.CreateSurfaceWeatherTable(cnWeatherDB);
      SurfaceWeather.WriteSurfaceWeatherRecords(cnWeatherDB, finList, validData);
    }

    [Obsolete]
    public static void ProcessSurfaceWeatherData(
      string sSfcFile,
      LocationData locData,
      string laiDB,
      DryDeposition.VEG_TYPE VegType,
      int startYear,
      int endYear,
      string sWeatherDB)
    {
      AccessFunc.CreateDB(sWeatherDB);
      using (OleDbConnection cnWeatherDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sWeatherDB))
      {
        using (OleDbConnection cnLaiDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + laiDB))
        {
          cnWeatherDB.Open();
          cnLaiDB.Open();
          SurfaceWeather.ProcessSurfaceWeatherData(sSfcFile, locData, cnLaiDB, VegType, startYear, endYear, cnWeatherDB);
        }
      }
    }

    public static void ProcessSurfaceWeatherData(
      string sSfcFile,
      LocationData locData,
      OleDbConnection cnLaiDB,
      DryDeposition.VEG_TYPE VegType,
      int startYear,
      int endYear,
      OleDbConnection cnWeatherDB)
    {
      List<SurfaceWeather> weaList = new List<SurfaceWeather>();
      List<SurfaceWeather> surfaceWeatherList = new List<SurfaceWeather>();
      List<SurfaceWeather> finList = new List<SurfaceWeather>();
      List<LeafAreaIndex> laiList = new List<LeafAreaIndex>();
      SurfaceWeather.ReadSurfaceWeatherData(sSfcFile, ref weaList);
      SurfaceWeather.CheckPressure(weaList, startYear, endYear);
      SurfaceWeather.AdjustTimeStamp(weaList);
      int hourlyRecords = SurfaceWeather.CreateHourlyRecords(weaList, ref surfaceWeatherList, startYear, endYear);
      double p06;
      double p01 = p06 = 0.0;
      SurfaceWeather.CalcAnnualPrecip(ref surfaceWeatherList, ref p01, ref p06);
      if (p01 < p06)
      {
        SurfaceWeather.Remove1HRain(surfaceWeatherList);
        SurfaceWeather.Disaggregate6HRain(surfaceWeatherList);
      }
      SurfaceWeather.FillExtrapolate(surfaceWeatherList);
      SurfaceWeather.FillInterpolate(surfaceWeatherList);
      SurfaceWeather.ConvertData(surfaceWeatherList, locData.GMTOffset, hourlyRecords);
      int validData = SurfaceWeather.ExtractValidData(surfaceWeatherList, hourlyRecords, ref finList);
      SurfaceWeather.CalcAirDensity(finList, validData);
      SurfaceWeather.CalcSolarZenithAngle(finList, locData, validData);
      SurfaceWeather.CalcSolarRadiation(finList, locData, validData);
      LeafAreaIndex.ReadLAIPartialRecords(cnLaiDB, ref laiList, finList[0].TimeStamp, finList[validData - 1].TimeStamp);
      SurfaceWeather.CalcET(finList, laiList, validData);
      SurfaceWeather.CalcPrecipInterceptByCanopy(finList, laiList, validData, VegType);
      SurfaceWeather.CalcPrecipInterceptByUnderCanopyCover(finList, validData);
      SurfaceWeather.CalcPrecipInterceptByNoCanopyCover(finList, validData);
      SurfaceWeather.CreateSurfaceWeatherTable(cnWeatherDB);
      SurfaceWeather.WriteSurfaceWeatherRecords(cnWeatherDB, finList, validData);
    }

    [Obsolete]
    public static void ProcessSurfaceWeatherData(
      string sSfcFile,
      LocationData locData,
      string laiDB,
      DryDeposition.VEG_TYPE VegType,
      int modelYear,
      string sWeatherDB)
    {
      AccessFunc.CreateDB(sWeatherDB);
      using (OleDbConnection cnWeatherDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sWeatherDB))
      {
        using (OleDbConnection cnLaiDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + laiDB))
        {
          cnWeatherDB.Open();
          cnLaiDB.Open();
          SurfaceWeather.ProcessSurfaceWeatherData(sSfcFile, locData, cnLaiDB, VegType, modelYear, cnWeatherDB);
        }
      }
    }

    public static void ProcessSurfaceWeatherData(
      string sSfcFile,
      LocationData locData,
      OleDbConnection cnLaiDB,
      DryDeposition.VEG_TYPE VegType,
      int modelYear,
      OleDbConnection cnWeatherDB)
    {
      List<SurfaceWeather> weaList = new List<SurfaceWeather>();
      List<SurfaceWeather> surfaceWeatherList = new List<SurfaceWeather>();
      List<SurfaceWeather> finList = new List<SurfaceWeather>();
      List<LeafAreaIndex> laiList = new List<LeafAreaIndex>();
      SurfaceWeather.ReadSurfaceWeatherData(sSfcFile, ref weaList);
      SurfaceWeather.CheckPressure(weaList, modelYear, modelYear);
      SurfaceWeather.AdjustTimeStamp(weaList);
      int hourlyRecords = SurfaceWeather.CreateHourlyRecords(weaList, ref surfaceWeatherList, modelYear, modelYear);
      if (SurfaceWeather.No1HRain(ref surfaceWeatherList))
        SurfaceWeather.Disaggregate6HRain(surfaceWeatherList);
      SurfaceWeather.FillExtrapolate(surfaceWeatherList);
      SurfaceWeather.FillInterpolate(surfaceWeatherList);
      SurfaceWeather.ConvertData(surfaceWeatherList, locData.GMTOffset, hourlyRecords);
      int validData = SurfaceWeather.ExtractValidData(surfaceWeatherList, hourlyRecords, ref finList);
      SurfaceWeather.CalcAirDensity(finList, validData);
      SurfaceWeather.CalcSolarZenithAngle(finList, locData, validData);
      SurfaceWeather.CalcSolarRadiation(finList, locData, validData);
      LeafAreaIndex.ReadLAIPartialRecords(cnLaiDB, ref laiList, finList[0].TimeStamp, finList[validData - 1].TimeStamp);
      SurfaceWeather.CalcET(finList, laiList, validData);
      SurfaceWeather.CalcPrecipInterceptByCanopy(finList, laiList, validData, VegType);
      SurfaceWeather.CalcPrecipInterceptByUnderCanopyCover(finList, validData);
      SurfaceWeather.CalcPrecipInterceptByNoCanopyCover(finList, validData);
      SurfaceWeather.CreateSurfaceWeatherTable(cnWeatherDB);
      SurfaceWeather.WriteSurfaceWeatherRecords(cnWeatherDB, finList, validData);
    }

    [Obsolete]
    public static void ProcessSurfaceWeatherData(
      ref List<SurfaceWeather> finList,
      string laiDB,
      DryDeposition.VEG_TYPE VegType,
      string sWeatherDB)
    {
      using (OleDbConnection cnWeatherDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sWeatherDB))
      {
        using (OleDbConnection cnLaiDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + laiDB))
        {
          cnWeatherDB.Open();
          cnLaiDB.Open();
          SurfaceWeather.ProcessSurfaceWeatherData(ref finList, cnLaiDB, VegType, cnWeatherDB);
        }
      }
    }

    public static void ProcessSurfaceWeatherData(
      ref List<SurfaceWeather> finList,
      OleDbConnection cnLaiDB,
      DryDeposition.VEG_TYPE VegType,
      OleDbConnection cnWeatherDB)
    {
      int count = finList.Count;
      List<LeafAreaIndex> laiList = new List<LeafAreaIndex>();
      LeafAreaIndex.ReadLAIPartialRecords(cnLaiDB, ref laiList, finList[0].TimeStamp, finList[count - 1].TimeStamp);
      SurfaceWeather.CalcET(finList, laiList, count);
      SurfaceWeather.CalcPrecipInterceptByCanopy(finList, laiList, count, VegType);
      SurfaceWeather.CalcPrecipInterceptByUnderCanopyCover(finList, count);
      SurfaceWeather.CalcPrecipInterceptByNoCanopyCover(finList, count);
      AccessFunc.RemoveTable(cnWeatherDB, nameof (SurfaceWeather));
      SurfaceWeather.CreateSurfaceWeatherTable(cnWeatherDB);
      SurfaceWeather.WriteSurfaceWeatherRecords(cnWeatherDB, finList, count);
    }

    public static void ProcessSurfaceWeatherData(
      string sSfcFile,
      LocationData locData,
      OleDbConnection cnLaiDB,
      DryDeposition.VEG_TYPE VegType,
      string rainFile,
      int startYear,
      int endYear,
      OleDbConnection cnWeatherDB)
    {
      List<SurfaceWeather> weaList = new List<SurfaceWeather>();
      List<SurfaceWeather> surfaceWeatherList1 = new List<SurfaceWeather>();
      List<SurfaceWeather> surfaceWeatherList2 = new List<SurfaceWeather>();
      List<LeafAreaIndex> laiList = new List<LeafAreaIndex>();
      SurfaceWeather.ReadSurfaceWeatherData(sSfcFile, ref weaList);
      SurfaceWeather.CheckPressure(weaList, startYear, endYear);
      SurfaceWeather.AdjustTimeStamp(weaList);
      int hourlyRecords = SurfaceWeather.CreateHourlyRecords(weaList, ref surfaceWeatherList1, startYear, endYear);
      double p06;
      double p01 = p06 = 0.0;
      SurfaceWeather.CalcAnnualPrecip(ref surfaceWeatherList1, ref p01, ref p06);
      if (string.IsNullOrEmpty(rainFile) && p01 < p06)
      {
        SurfaceWeather.Remove1HRain(surfaceWeatherList1);
        SurfaceWeather.Disaggregate6HRain(surfaceWeatherList1);
      }
      SurfaceWeather.FillExtrapolate(surfaceWeatherList1);
      SurfaceWeather.FillInterpolate(surfaceWeatherList1);
      SurfaceWeather.ConvertData(surfaceWeatherList1, locData.GMTOffset, hourlyRecords);
      int validData = SurfaceWeather.ExtractValidData(surfaceWeatherList1, hourlyRecords, ref surfaceWeatherList2);
      if (!string.IsNullOrEmpty(rainFile))
      {
        if (Path.GetExtension(rainFile) == ".csv")
          SurfaceWeather.ReplaceCsvRainData(rainFile, locData.GMTOffset, ref surfaceWeatherList2);
        else
          SurfaceWeather.ReplaceRainData(rainFile, startYear, ref surfaceWeatherList2);
      }
      SurfaceWeather.CalcAirDensity(surfaceWeatherList2, validData);
      SurfaceWeather.CalcSolarZenithAngle(surfaceWeatherList2, locData, validData);
      SurfaceWeather.CalcSolarRadiation(surfaceWeatherList2, locData, validData);
      LeafAreaIndex.ReadLAIPartialRecords(cnLaiDB, ref laiList, surfaceWeatherList2[0].TimeStamp, surfaceWeatherList2[validData - 1].TimeStamp);
      SurfaceWeather.CalcET(surfaceWeatherList2, laiList, validData);
      SurfaceWeather.CalcPrecipInterceptByCanopy(surfaceWeatherList2, laiList, validData, VegType);
      SurfaceWeather.CalcPrecipInterceptByUnderCanopyCover(surfaceWeatherList2, validData);
      SurfaceWeather.CalcPrecipInterceptByNoCanopyCover(surfaceWeatherList2, validData);
      SurfaceWeather.CreateSurfaceWeatherTable(cnWeatherDB);
      SurfaceWeather.WriteSurfaceWeatherRecords(cnWeatherDB, surfaceWeatherList2, validData);
    }

    private static void CalcAnnualPrecip(
      ref List<SurfaceWeather> list,
      ref double p01,
      ref double p06)
    {
      for (int index = 0; index < list.Count; ++index)
      {
        p01 += list[index].RainInH * 25.4;
        p06 += list[index].RainIn6H * 25.4;
      }
    }

    private static void ReplaceRainData(string rainFile, int yr, ref List<SurfaceWeather> list)
    {
      OleDbCommand oleDbCommand = new OleDbCommand();
      List<SurfaceWeather> surfaceWeatherList = new List<SurfaceWeather>();
      int num1 = 0;
      using (OleDbConnection oleDbConnection = new OleDbConnection())
      {
        if (Path.GetExtension(rainFile) == ".xls")
          oleDbConnection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + rainFile + ";Extended Properties=\"Excel 8.0;HDR=YES\"";
        else if (Path.GetExtension(rainFile) == ".xlsx")
          oleDbConnection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + rainFile + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
        try
        {
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "SELECT [TimeStamp], [RainMh] FROM [" + yr.ToString() + "$] ORDER BY [TimeStamp];";
          OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
          while (oleDbDataReader.Read())
          {
            if (oleDbDataReader[0] != DBNull.Value && oleDbDataReader[1] != DBNull.Value)
            {
              SurfaceWeather surfaceWeather = new SurfaceWeather();
              DateTime dateTime = oleDbDataReader.GetDateTime(0).AddSeconds(1.0);
              surfaceWeather.TimeStamp = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
              surfaceWeather.RainMh = oleDbDataReader.GetDouble(1);
              surfaceWeatherList.Add(surfaceWeather);
            }
          }
          oleDbDataReader.Close();
          for (int index = 0; index < surfaceWeatherList.Count; ++index)
          {
            if (list[0].TimeStamp.Year == surfaceWeatherList[index].TimeStamp.Year && list[0].TimeStamp.Month == surfaceWeatherList[index].TimeStamp.Month && list[0].TimeStamp.Day == surfaceWeatherList[index].TimeStamp.Day && list[0].TimeStamp.Hour == surfaceWeatherList[index].TimeStamp.Hour)
            {
              num1 = index;
              break;
            }
          }
          int num2 = num1;
          for (int index = 0; index < list.Count; ++index)
            list[index].RainMh = surfaceWeatherList[num2++].RainMh;
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    private static void ReadPrecip(string sFile, ref List<SurfaceWeather> rList)
    {
      Regex regex = new Regex("^(?<dt>.{10})\\s+(?<hr>\\d{2}):(?<mn>\\d{2}),\\s*(?<p>.+)\\s*$");
      using (StreamReader streamReader = new StreamReader(sFile))
      {
        try
        {
          streamReader.ReadLine();
          string input;
          while ((input = streamReader.ReadLine()) != null)
          {
            Match match = regex.Match(input);
            SurfaceWeather surfaceWeather = new SurfaceWeather()
            {
              Year = int.Parse(match.Groups["dt"].ToString().Substring(0, 4)),
              Month = int.Parse(match.Groups["dt"].ToString().Substring(5, 2)),
              Day = int.Parse(match.Groups["dt"].ToString().Substring(8, 2)),
              Hour = int.Parse(match.Groups["hr"].ToString()),
              Minute = int.Parse(match.Groups["mn"].ToString())
            };
            surfaceWeather.TimeStamp = DateTime.Parse(surfaceWeather.Month.ToString() + "/" + surfaceWeather.Day.ToString() + "/" + surfaceWeather.Year.ToString() + " " + surfaceWeather.Hour.ToString() + ":" + surfaceWeather.Minute.ToString());
            surfaceWeather.RainMh = double.Parse(match.Groups["p"].ToString());
            rList.Add(surfaceWeather);
          }
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    private static void AdjustTimeZone(ref List<SurfaceWeather> intList, double timeZone)
    {
      for (int index = 0; index < intList.Count; ++index)
      {
        DateTime dateTime = intList[index].TimeStamp.AddHours((double) (int) timeZone);
        intList[index].TimeStamp = dateTime;
      }
    }

    private static void ReplaceCsvRainData(
      string rainFile,
      double gmtOffset,
      ref List<SurfaceWeather> list)
    {
      List<SurfaceWeather> surfaceWeatherList = new List<SurfaceWeather>();
      SurfaceWeather.ReadPrecip(rainFile, ref surfaceWeatherList);
      SurfaceWeather.AdjustTimeZone(ref surfaceWeatherList, gmtOffset);
      int index1 = 0;
      while (index1 < surfaceWeatherList.Count && !(list[0].TimeStamp == surfaceWeatherList[index1].TimeStamp))
        ++index1;
      for (int index2 = 0; index2 < list.Count; ++index2)
      {
        if (list[index2].TimeStamp == surfaceWeatherList[index1].TimeStamp)
          list[index2].RainMh = surfaceWeatherList[index1++].RainMh;
      }
    }

    public static void CopySurfaceWeatherData(string inFile, string outFile, bool blAppend)
    {
      string str1 = "";
      string str2 = "";
      StreamReader streamReader;
      using (streamReader = new StreamReader(inFile))
      {
        SurfaceWeather.WeatherDataFormat weatherDataFormat = SurfaceWeather.CheckWeatherDataFormat(inFile);
        try
        {
          if (!blAppend)
          {
            StreamWriter streamWriter;
            using (streamWriter = new StreamWriter(outFile, blAppend))
            {
              string str3;
              while ((str3 = streamReader.ReadLine()) != null)
              {
                switch (weatherDataFormat)
                {
                  case SurfaceWeather.WeatherDataFormat.OLD:
                    str2 = str3;
                    break;
                  case SurfaceWeather.WeatherDataFormat.NEWINTL:
                    str2 = str3.Substring(0, 66) + str3.Substring(75, 66);
                    break;
                  case SurfaceWeather.WeatherDataFormat.NEWUSCAN:
                    str2 = str3.Substring(0, 66) + str3.Substring(81, 66);
                    break;
                  case SurfaceWeather.WeatherDataFormat.NARCCAP:
                    str2 = str3;
                    break;
                }
                streamWriter.WriteLine(str2);
              }
            }
          }
          else
          {
            StreamWriter streamWriter;
            using (streamWriter = new StreamWriter(outFile, blAppend))
            {
              str1 = streamReader.ReadLine();
              string str4;
              while ((str4 = streamReader.ReadLine()) != null)
              {
                switch (weatherDataFormat)
                {
                  case SurfaceWeather.WeatherDataFormat.OLD:
                    str2 = str4;
                    break;
                  case SurfaceWeather.WeatherDataFormat.NEWINTL:
                    str2 = str4.Substring(0, 66) + str4.Substring(75, 66);
                    break;
                  case SurfaceWeather.WeatherDataFormat.NEWUSCAN:
                    str2 = str4.Substring(0, 66) + str4.Substring(81, 66);
                    break;
                }
                streamWriter.WriteLine(str2);
              }
            }
          }
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    public static SurfaceWeather.WeatherDataFormat CheckWeatherDataFormat(string sFile)
    {
      SurfaceWeather.WeatherDataFormat weatherDataFormat = SurfaceWeather.WeatherDataFormat.OLD;
      using (StreamReader streamReader = new StreamReader(sFile))
      {
        try
        {
          string input = streamReader.ReadLine();
          if (Regex.IsMatch(input, "  USAF  WBAN YR--MODAHRMN DIR SPD GUS CLG SKC L M H  VSB WW WW WW ZZ ZZ ZZ W TEMP DEWP    SLP   ALT    STP MAX MIN PCP01 PCP06 PCP24 PCPXX SD"))
            weatherDataFormat = SurfaceWeather.WeatherDataFormat.NEWINTL;
          else if (Regex.IsMatch(input, "  USAF  WBAN YR--MODAHRMN DIR SPD GUS CLG SKC L M H  VSB MW MW MW MW AW AW AW AW W TEMP DEWP    SLP   ALT    STP MAX MIN PCP01 PCP06 PCP24 PCPXX SD"))
            weatherDataFormat = SurfaceWeather.WeatherDataFormat.NEWUSCAN;
          else if (Regex.IsMatch(input, "  USAF  WBAN YR--MODAHRMN DIR SPD GUS CLG SKC L M H  VSB WW WW WW W TEMP DEWP    SLP   ALT    STP MAX MIN PCP01 PCP06 PCP24 PCPXX SD"))
            weatherDataFormat = SurfaceWeather.WeatherDataFormat.OLD;
          else if (Regex.IsMatch(input, "  USAF  WBAN YR--MODAHRMN DIR SPD GUS CLG SKC L M H  VSB WW WW WW W TEMP DEWP    SLP   ALT    STP MAX MIN PCP01      NETRAD PCPXX SD"))
            weatherDataFormat = SurfaceWeather.WeatherDataFormat.NARCCAP;
          return weatherDataFormat;
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    public static void ReadSurfaceWeatherData(string sFile, ref List<SurfaceWeather> weaList)
    {
      SurfaceWeather.weaFormat = SurfaceWeather.CheckWeatherDataFormat(sFile);
      using (StreamReader streamReader = new StreamReader(sFile))
      {
        try
        {
          Regex regex;
          switch (SurfaceWeather.weaFormat)
          {
            case SurfaceWeather.WeatherDataFormat.OLD:
              regex = new Regex("^(?<usaf>.{6}) (?<wban>.{5}) (?<dt>.{12}) (?<dir>.{3}) (?<spd>.{3}) .{3} (?<clg>.{3}) (?<skc>.{3}) .{21} (?<temp>.{4}) (?<dewp>.{4}) .{6} (?<alt>.{5}) (?<stp>.{6}) .{7} (?<pcp01>.{5}) (?<pcp06>.{5}) .{11} (?<sd>.{2})\\s*$");
              break;
            case SurfaceWeather.WeatherDataFormat.NEWINTL:
              regex = new Regex("^(?<usaf>.{6}) (?<wban>.{5}) (?<dt>.{12}) (?<dir>.{3}) (?<spd>.{3}) .{3} (?<clg>.{3}) (?<skc>.{3}) .{30} (?<temp>.{4}) (?<dewp>.{4}) .{6} (?<alt>.{5}) (?<stp>.{6}) .{7} (?<pcp01>.{5}) (?<pcp06>.{5}) .{11} (?<sd>.{2})\\s*$");
              break;
            case SurfaceWeather.WeatherDataFormat.NEWUSCAN:
              regex = new Regex("^(?<usaf>.{6}) (?<wban>.{5}) (?<dt>.{12}) (?<dir>.{3}) (?<spd>.{3}) .{3} (?<clg>.{3}) (?<skc>.{3}) .{36} (?<temp>.{4}) (?<dewp>.{4}) .{6} (?<alt>.{5}) (?<stp>.{6}) .{7} (?<pcp01>.{5}) (?<pcp06>.{5}) .{11} (?<sd>.{2})\\s*$");
              break;
            case SurfaceWeather.WeatherDataFormat.NARCCAP:
              regex = new Regex("^(?<usaf>.{6}) (?<wban>.{5}) (?<dt>.{12}) (?<dir>.{3}) (?<spd>.{3}) .{3} (?<clg>.{3}) (?<skc>.{3}) .{21} (?<temp>.{4}) (?<dewp>.{4}) .{6} (?<alt>.{5}) (?<stp>.{6}) .{7} (?<pcp>.{5}) (?<netrad>.{11}) .{5} (?<sd>.{2})\\s*$");
              break;
            default:
              regex = new Regex("^(?<usaf>.{6}) (?<wban>.{5}) (?<dt>.{12}) (?<dir>.{3}) (?<spd>.{3}) .{3} (?<clg>.{3}) (?<skc>.{3}) .{21} (?<temp>.{4}) (?<dewp>.{4}) .{6} (?<alt>.{5}) (?<stp>.{6}) .{7} (?<pcp01>.{5}) (?<pcp06>.{5}) .{11}  (?<sd>.{2})\\s*$");
              break;
          }
          streamReader.ReadLine();
          string input1;
          while ((input1 = streamReader.ReadLine()) != null)
          {
            string input2 = Regex.Replace(input1, "([0-9.]+)T", "$1 ");
            Match match = regex.Match(input2);
            SurfaceWeather surfaceWeather = new SurfaceWeather()
            {
              Year = match.Groups["dt"].ToString().Substring(0, 4) == "****" || match.Groups["dt"].ToString().Substring(0, 4) == "    " ? -999 : int.Parse(match.Groups["dt"].ToString().Substring(0, 4)),
              Month = match.Groups["dt"].ToString().Substring(4, 2) == "**" || match.Groups["dt"].ToString().Substring(4, 2) == "  " ? -999 : int.Parse(match.Groups["dt"].ToString().Substring(4, 2)),
              Day = match.Groups["dt"].ToString().Substring(6, 2) == "**" || match.Groups["dt"].ToString().Substring(6, 2) == "  " ? -999 : int.Parse(match.Groups["dt"].ToString().Substring(6, 2)),
              Hour = match.Groups["dt"].ToString().Substring(8, 2) == "**" || match.Groups["dt"].ToString().Substring(8, 2) == "  " ? -999 : int.Parse(match.Groups["dt"].ToString().Substring(8, 2)),
              Minute = match.Groups["dt"].ToString().Substring(10, 2) == "**" || match.Groups["dt"].ToString().Substring(10, 2) == "  " ? -999 : int.Parse(match.Groups["dt"].ToString().Substring(10, 2))
            };
            surfaceWeather.TimeStamp = DateTime.Parse(surfaceWeather.Month.ToString() + "/" + surfaceWeather.Day.ToString() + "/" + surfaceWeather.Year.ToString() + " " + surfaceWeather.Hour.ToString() + ":" + surfaceWeather.Minute.ToString());
            surfaceWeather.WdDir = match.Groups["dir"].ToString() == "***" || match.Groups["dir"].ToString() == "   " ? -999.0 : double.Parse(match.Groups["dir"].ToString());
            surfaceWeather.WdSpdMh = match.Groups["spd"].ToString() == "***" || match.Groups["spd"].ToString() == "   " ? -999.0 : double.Parse(match.Groups["spd"].ToString());
            surfaceWeather.Ceiling = match.Groups["clg"].ToString() == "***" || match.Groups["clg"].ToString() == "   " ? -999.0 : double.Parse(match.Groups["clg"].ToString());
            string str = match.Groups["skc"].ToString();
            surfaceWeather.ToCldCov = str == "***" || str == "   " ? -999.0 : (str == "CLR" ? 0.0 : (str == "SCT" ? 3.75 : (str == "BKN" ? 7.5 : (str == "OVC" ? 10.0 : 10.0))));
            surfaceWeather.TempF = match.Groups["temp"].ToString() == "****" || match.Groups["temp"].ToString() == "    " ? -999.0 : double.Parse(match.Groups["temp"].ToString());
            surfaceWeather.DewTempF = match.Groups["dewp"].ToString() == "****" || match.Groups["dewp"].ToString() == "    " ? -999.0 : double.Parse(match.Groups["dewp"].ToString());
            surfaceWeather.Altimeter = match.Groups["alt"].ToString() == "*****" || match.Groups["alt"].ToString() == "     " ? -999.0 : double.Parse(match.Groups["alt"].ToString());
            surfaceWeather.PrsMbar = match.Groups["stp"].ToString() == "******" || match.Groups["stp"].ToString() == "      " ? -999.0 : double.Parse(match.Groups["stp"].ToString());
            surfaceWeather.RainInH = match.Groups["pcp01"].ToString() == "*****" || match.Groups["pcp01"].ToString() == "     " ? 0.0 : double.Parse(match.Groups["pcp01"].ToString());
            if (SurfaceWeather.weaFormat == SurfaceWeather.WeatherDataFormat.NARCCAP)
              surfaceWeather.NetRadWm2 = match.Groups["netrad"].ToString() == "***********" || match.Groups["netrad"].ToString() == "           " ? -999.0 : double.Parse(match.Groups["netrad"].ToString());
            else
              surfaceWeather.RainIn6H = match.Groups["pcp06"].ToString() == "*****" || match.Groups["pcp06"].ToString() == "     " ? 0.0 : double.Parse(match.Groups["pcp06"].ToString());
            surfaceWeather.SnowIn = match.Groups["sd"].ToString() == "**" || match.Groups["sd"].ToString() == "  " ? 0.0 : double.Parse(match.Groups["sd"].ToString());
            weaList.Add(surfaceWeather);
          }
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    public static void CheckPressure(List<SurfaceWeather> weaList, int startYear, int endYear)
    {
      if (SurfaceWeather.CheckPrsMbar(weaList, startYear, endYear) || !SurfaceWeather.CheckAltimeter(weaList, startYear, endYear))
        return;
      SurfaceWeather.Alt2Prs(weaList);
    }

    private static bool CheckPrsMbar(List<SurfaceWeather> list, int startYear, int endYear)
    {
      bool flag = false;
      for (int index = 0; index < list.Count; ++index)
      {
        if (list[index].PrsMbar != -999.0)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private static bool CheckAltimeter(List<SurfaceWeather> list, int startYear, int endYear)
    {
      bool flag = false;
      for (int index = 0; index < list.Count; ++index)
      {
        if (list[index].Altimeter != -999.0)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private static void Alt2Prs(List<SurfaceWeather> list)
    {
      for (int index = 0; index < list.Count; ++index)
        list[index].PrsMbar = list[index].Altimeter != -999.0 ? list[index].Altimeter * 100.0 * 0.338639 : -999.0;
    }

    public static void AdjustTimeStamp(List<SurfaceWeather> rawList)
    {
      for (int index = 0; index < rawList.Count; ++index)
      {
        if (rawList[index].TimeStamp.Minute != 0)
          rawList[index].TimeStamp = rawList[index].TimeStamp.AddMinutes((double) (60 - rawList[index].TimeStamp.Minute));
      }
    }

    public static int CreateHourlyRecords(
      List<SurfaceWeather> rawList,
      ref List<SurfaceWeather> hrList,
      int startYear,
      int endYear)
    {
      int hourlyRecords = 0;
      for (int year = startYear; year <= endYear; ++year)
      {
        if (DateTime.IsLeapYear(year))
          hourlyRecords += 8784;
        else
          hourlyRecords += 8760;
      }
      DateTime dateTime = DateTime.Parse("1/1/" + startYear.ToString() + " 0:00");
      hrList = new List<SurfaceWeather>();
      for (int index = 0; index < hourlyRecords; ++index)
      {
        SurfaceWeather surfaceWeather = new SurfaceWeather()
        {
          TimeStamp = dateTime.AddHours((double) index)
        };
        surfaceWeather.WdDir = surfaceWeather.WdSpdMh = surfaceWeather.Ceiling = surfaceWeather.ToCldCov = surfaceWeather.TempF = surfaceWeather.DewTempF = surfaceWeather.PrsMbar = surfaceWeather.NetRadWm2 = -999.0;
        surfaceWeather.RainInH = surfaceWeather.RainIn6H = 0.0;
        hrList.Add(surfaceWeather);
      }
      try
      {
        int index1 = 0;
        for (int index2 = 0; index2 < hourlyRecords; ++index2)
        {
          if (hrList[index2].TimeStamp == rawList[index1].TimeStamp)
          {
            do
            {
              if (rawList[index1].RainInH >= hrList[index2].RainInH)
                hrList[index2].RainInH = rawList[index1].RainInH;
              if (rawList[index1].RainIn6H >= hrList[index2].RainIn6H)
                hrList[index2].RainIn6H = rawList[index1].RainIn6H;
              if (rawList[index1].WdDir != -999.0)
                hrList[index2].WdDir = rawList[index1].WdDir;
              if (rawList[index1].WdSpdMh != -999.0)
                hrList[index2].WdSpdMh = rawList[index1].WdSpdMh;
              if (rawList[index1].Ceiling != -999.0)
                hrList[index2].Ceiling = rawList[index1].Ceiling;
              if (rawList[index1].ToCldCov != -999.0)
                hrList[index2].ToCldCov = rawList[index1].ToCldCov;
              if (rawList[index1].TempF != -999.0)
                hrList[index2].TempF = rawList[index1].TempF;
              if (rawList[index1].DewTempF != -999.0)
                hrList[index2].DewTempF = rawList[index1].DewTempF;
              if (rawList[index1].PrsMbar != -999.0)
                hrList[index2].PrsMbar = rawList[index1].PrsMbar;
              if (SurfaceWeather.weaFormat == SurfaceWeather.WeatherDataFormat.NARCCAP && rawList[index1].NetRadWm2 != -999.0)
                hrList[index2].NetRadWm2 = rawList[index1].NetRadWm2;
              ++index1;
              if (index1 == rawList.Count)
                return hourlyRecords;
            }
            while (hrList[index2].TimeStamp == rawList[index1].TimeStamp);
          }
        }
        return hourlyRecords;
      }
      catch (Exception ex)
      {
        return hourlyRecords;
      }
    }

    private static bool No1HRain(ref List<SurfaceWeather> intList)
    {
      bool flag = true;
      for (int index = 0; index < intList.Count; ++index)
      {
        if (intList[index].RainInH > 0.0)
        {
          flag = false;
          break;
        }
      }
      return flag;
    }

    private static void Remove1HRain(List<SurfaceWeather> intList)
    {
      try
      {
        for (int index = 0; index < intList.Count; ++index)
          intList[index].RainInH = intList[index].RainMh = 0.0;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private static void Disaggregate6HRain(List<SurfaceWeather> intList)
    {
      try
      {
        for (int index1 = intList.Count - 1; index1 >= 0; --index1)
        {
          if (intList[index1].RainIn6H > 0.0)
          {
            double num1 = intList[index1].RainIn6H / 6.0;
            int num2 = index1;
            for (int index2 = 0; index2 < 6; ++index2)
            {
              intList[num2--].RainInH = num1;
              if (num2 == -1)
                break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static void ConvertData(List<SurfaceWeather> intList, double timeZone, int recCnt)
    {
      try
      {
        for (int index = 0; index < recCnt; ++index)
        {
          DateTime dateTime = intList[index].TimeStamp.AddHours((double) (int) timeZone);
          intList[index].TimeStamp = dateTime;
          intList[index].Year = dateTime.Year;
          intList[index].Month = dateTime.Month;
          intList[index].Day = dateTime.Day;
          intList[index].Hour = dateTime.Hour;
          intList[index].Jday = dateTime.DayOfYear;
          intList[index].PrsIn = intList[index].PrsMbar / 33.8639 * 100.0;
          intList[index].PrsKpa = intList[index].PrsMbar / 10.0;
          intList[index].RainMh = intList[index].RainInH * 0.0254;
          intList[index].SnowM = intList[index].SnowIn * 0.0254;
          intList[index].TempC = (intList[index].TempF - 32.0) * 5.0 / 9.0;
          intList[index].TempK = intList[index].TempC + 273.15;
          intList[index].DewTempC = (intList[index].DewTempF - 32.0) * 5.0 / 9.0;
          intList[index].WdSpdKnt = Math.Round(intList[index].WdSpdMh / 1.151);
          intList[index].WdSpdMs = intList[index].WdSpdMh / 2.237;
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static int ExtractValidData(
      List<SurfaceWeather> intList,
      int recCnt,
      ref List<SurfaceWeather> finList)
    {
      int num1 = 0;
      int num2 = 0;
      if (intList[0].Hour != 0 || intList[recCnt - 1].Hour != 23)
      {
        for (int index = 0; index < recCnt; ++index)
        {
          if (intList[index].Hour == 0)
          {
            num1 = index;
            break;
          }
        }
        for (int index = recCnt - 1; index >= 0; --index)
        {
          if (intList[index].Hour == 23)
          {
            num2 = index;
            break;
          }
        }
        recCnt = num2 - num1 + 1;
        finList = new List<SurfaceWeather>();
        for (int index = 0; index < recCnt; ++index)
        {
          SurfaceWeather surfaceWeather = new SurfaceWeather();
          finList.Add(surfaceWeather);
        }
        int index1 = 0;
        for (int index2 = num1; index2 <= num2; ++index2)
        {
          finList[index1].TimeStamp = intList[index2].TimeStamp;
          finList[index1].Jday = intList[index2].Jday;
          finList[index1].Year = intList[index2].Year;
          finList[index1].Month = intList[index2].Month;
          finList[index1].Day = intList[index2].Day;
          finList[index1].Hour = intList[index2].Hour;
          finList[index1].Ceiling = intList[index2].Ceiling;
          finList[index1].DewTempF = intList[index2].DewTempF;
          finList[index1].RainInH = intList[index2].RainInH;
          finList[index1].PrsMbar = intList[index2].PrsMbar;
          finList[index1].SnowIn = intList[index2].SnowIn;
          finList[index1].TempF = intList[index2].TempF;
          finList[index1].ToCldCov = intList[index2].ToCldCov;
          finList[index1].WdDir = intList[index2].WdDir;
          finList[index1].WdSpdMh = intList[index2].WdSpdMh;
          finList[index1].PrsIn = intList[index2].PrsIn;
          finList[index1].PrsKpa = intList[index2].PrsKpa;
          finList[index1].RainMh = intList[index2].RainMh;
          finList[index1].SnowM = intList[index2].SnowM;
          finList[index1].TempC = intList[index2].TempC;
          finList[index1].TempK = intList[index2].TempK;
          finList[index1].DewTempC = intList[index2].DewTempC;
          finList[index1].WdSpdKnt = intList[index2].WdSpdKnt;
          finList[index1].WdSpdMs = intList[index2].WdSpdMs;
          if (SurfaceWeather.weaFormat == SurfaceWeather.WeatherDataFormat.NARCCAP)
            finList[index1].NetRadWm2 = intList[index2].NetRadWm2;
          ++index1;
        }
      }
      else
        finList = intList;
      return recCnt;
    }

    public static void FillExtrapolate(List<SurfaceWeather> list)
    {
      SurfaceWeather.ExtrapoloteCeiling(list);
      SurfaceWeather.ExtrapolatePrsMbar(list);
      SurfaceWeather.ExtrapolateToCldCov(list);
      SurfaceWeather.ExtrapolateTempF(list);
      SurfaceWeather.ExtrapolateDewTempF(list);
      SurfaceWeather.ExtrapolateWdSpdMh(list);
      if (SurfaceWeather.weaFormat != SurfaceWeather.WeatherDataFormat.NARCCAP)
        return;
      SurfaceWeather.ExtrapolateNetRadWm2(list);
    }

    private static void ExtrapoloteCeiling(List<SurfaceWeather> list)
    {
      double num = 0.0;
      if (list[0].Ceiling == -999.0)
      {
        int index1 = 1;
        while (list[index1].Ceiling == -999.0)
        {
          ++index1;
          if (index1 == list.Count)
            goto label_5;
        }
        num = list[index1].Ceiling;
label_5:
        if (index1 != list.Count)
        {
          for (int index2 = index1 - 1; index2 >= 0; --index2)
            list[index2].Ceiling = num;
        }
      }
      if (list[list.Count - 1].Ceiling != -999.0)
        return;
      int index3 = list.Count - 1;
      while (list[index3].Ceiling == -999.0)
      {
        --index3;
        if (index3 == 0)
          goto label_14;
      }
      num = list[index3].Ceiling;
label_14:
      if (index3 == 0)
        return;
      for (int index4 = index3 + 1; index4 < list.Count; ++index4)
        list[index4].Ceiling = num;
    }

    private static void ExtrapolatePrsMbar(List<SurfaceWeather> list)
    {
      double num = 0.0;
      if (list[0].PrsMbar == -999.0)
      {
        int index1 = 1;
        while (list[index1].PrsMbar == -999.0)
        {
          ++index1;
          if (index1 == list.Count)
            goto label_5;
        }
        num = list[index1].PrsMbar;
label_5:
        if (index1 != list.Count)
        {
          for (int index2 = index1 - 1; index2 >= 0; --index2)
            list[index2].PrsMbar = num;
        }
      }
      if (list[list.Count - 1].PrsMbar != -999.0)
        return;
      int index3 = list.Count - 1;
      while (list[index3].PrsMbar == -999.0)
      {
        --index3;
        if (index3 == 0)
          goto label_14;
      }
      num = list[index3].PrsMbar;
label_14:
      if (index3 == 0)
        return;
      for (int index4 = index3 + 1; index4 < list.Count; ++index4)
        list[index4].PrsMbar = num;
    }

    private static void ExtrapolateToCldCov(List<SurfaceWeather> list)
    {
      double num = 0.0;
      if (list[0].ToCldCov == -999.0)
      {
        int index1 = 1;
        while (list[index1].ToCldCov == -999.0)
        {
          ++index1;
          if (index1 == list.Count)
            goto label_5;
        }
        num = list[index1].ToCldCov;
label_5:
        if (index1 != list.Count)
        {
          for (int index2 = index1 - 1; index2 >= 0; --index2)
            list[index2].ToCldCov = num;
        }
      }
      if (list[list.Count - 1].ToCldCov != -999.0)
        return;
      int index3 = list.Count - 1;
      while (list[index3].ToCldCov == -999.0)
      {
        --index3;
        if (index3 == 0)
          goto label_14;
      }
      num = list[index3].ToCldCov;
label_14:
      if (index3 == 0)
        return;
      for (int index4 = index3 + 1; index4 < list.Count; ++index4)
        list[index4].ToCldCov = num;
    }

    private static void ExtrapolateTempF(List<SurfaceWeather> list)
    {
      double num = 0.0;
      if (list[0].TempF == -999.0)
      {
        int index1 = 1;
        while (list[index1].TempF == -999.0)
        {
          ++index1;
          if (index1 == list.Count)
            goto label_5;
        }
        num = list[index1].TempF;
label_5:
        if (index1 != list.Count)
        {
          for (int index2 = index1 - 1; index2 >= 0; --index2)
            list[index2].TempF = num;
        }
      }
      if (list[list.Count - 1].TempF != -999.0)
        return;
      int index3 = list.Count - 1;
      while (list[index3].TempF == -999.0)
      {
        --index3;
        if (index3 == 0)
          goto label_14;
      }
      num = list[index3].TempF;
label_14:
      if (index3 == 0)
        return;
      for (int index4 = index3 + 1; index4 < list.Count; ++index4)
        list[index4].TempF = num;
    }

    private static void ExtrapolateDewTempF(List<SurfaceWeather> list)
    {
      double num = 0.0;
      if (list[0].DewTempF == -999.0)
      {
        int index1 = 1;
        while (list[index1].DewTempF == -999.0)
        {
          ++index1;
          if (index1 == list.Count)
            goto label_5;
        }
        num = list[index1].DewTempF;
label_5:
        if (index1 != list.Count)
        {
          for (int index2 = index1 - 1; index2 >= 0; --index2)
            list[index2].DewTempF = num;
        }
      }
      if (list[list.Count - 1].DewTempF != -999.0)
        return;
      int index3 = list.Count - 1;
      while (list[index3].DewTempF == -999.0)
      {
        --index3;
        if (index3 == 0)
          goto label_14;
      }
      num = list[index3].DewTempF;
label_14:
      if (index3 == 0)
        return;
      for (int index4 = index3 + 1; index4 < list.Count; ++index4)
        list[index4].DewTempF = num;
    }

    private static void ExtrapolateWdDir(List<SurfaceWeather> list)
    {
      double num = 0.0;
      if (list[0].WdDir == -999.0)
      {
        int index1 = 1;
        while (list[index1].WdDir == -999.0)
        {
          ++index1;
          if (index1 == list.Count)
            goto label_5;
        }
        num = list[index1].WdDir;
label_5:
        if (index1 != list.Count)
        {
          for (int index2 = index1 - 1; index2 >= 0; --index2)
            list[index2].WdDir = num;
        }
      }
      if (list[list.Count - 1].WdDir != -999.0)
        return;
      int index3 = list.Count - 1;
      while (list[index3].WdDir == -999.0)
      {
        --index3;
        if (index3 == 0)
          goto label_14;
      }
      num = list[index3].WdDir;
label_14:
      if (index3 == 0)
        return;
      for (int index4 = index3 + 1; index4 < list.Count; ++index4)
        list[index4].WdDir = num;
    }

    private static void ExtrapolateWdSpdMh(List<SurfaceWeather> list)
    {
      double num = 0.0;
      if (list[0].WdSpdMh == -999.0)
      {
        int index1 = 1;
        while (list[index1].WdSpdMh == -999.0)
        {
          ++index1;
          if (index1 == list.Count)
            goto label_5;
        }
        num = list[index1].WdSpdMh;
label_5:
        if (index1 != list.Count)
        {
          for (int index2 = index1 - 1; index2 >= 0; --index2)
            list[index2].WdSpdMh = num;
        }
      }
      if (list[list.Count - 1].WdSpdMh != -999.0)
        return;
      int index3 = list.Count - 1;
      while (list[index3].WdSpdMh == -999.0)
      {
        --index3;
        if (index3 == 0)
          goto label_14;
      }
      num = list[index3].WdSpdMh;
label_14:
      if (index3 == 0)
        return;
      for (int index4 = index3 + 1; index4 < list.Count; ++index4)
        list[index4].WdSpdMh = num;
    }

    private static void ExtrapolateNetRadWm2(List<SurfaceWeather> list)
    {
      double num = 0.0;
      if (list[0].NetRadWm2 == -999.0)
      {
        int index1 = 1;
        while (list[index1].NetRadWm2 == -999.0)
        {
          ++index1;
          if (index1 == list.Count)
            goto label_5;
        }
        num = list[index1].NetRadWm2;
label_5:
        if (index1 != list.Count)
        {
          for (int index2 = index1 - 1; index2 >= 0; --index2)
            list[index2].NetRadWm2 = num;
        }
      }
      if (list[list.Count - 1].NetRadWm2 != -999.0)
        return;
      int index3 = list.Count - 1;
      while (list[index3].NetRadWm2 == -999.0)
      {
        --index3;
        if (index3 == 0)
          goto label_14;
      }
      num = list[index3].NetRadWm2;
label_14:
      if (index3 == 0)
        return;
      for (int index4 = index3 + 1; index4 < list.Count; ++index4)
        list[index4].NetRadWm2 = num;
    }

    public static void FillInterpolate(List<SurfaceWeather> list)
    {
      SurfaceWeather.InterpolatePrsMbar(list);
      SurfaceWeather.InterpolateTempF(list);
      SurfaceWeather.InterpolateDewTempF(list);
      SurfaceWeather.InterpolateWdSpdMh(list);
      SurfaceWeather.InterpolateCeiling(list);
      SurfaceWeather.InterpolateToCldCov(list);
      if (SurfaceWeather.weaFormat != SurfaceWeather.WeatherDataFormat.NARCCAP)
        return;
      SurfaceWeather.InterpolateNetRadWm2(list);
    }

    private static void InterpolatePrsMbar(List<SurfaceWeather> list)
    {
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        if (list[index1].PrsMbar == -999.0)
        {
          int index2 = index1 - 1;
          int index3 = index1 + 1;
          while (list[index3].PrsMbar == -999.0)
            ++index3;
          int index4 = index3;
          int totalMinutes = (int) (list[index4].TimeStamp - list[index2].TimeStamp).TotalMinutes;
          double num = (list[index4].PrsMbar - list[index2].PrsMbar) / (double) totalMinutes;
          for (int index5 = index2 + 1; index5 < index4; ++index5)
          {
            TimeSpan timeSpan = list[index5].TimeStamp - list[index2].TimeStamp;
            list[index5].PrsMbar = list[index2].PrsMbar + num * timeSpan.TotalMinutes;
          }
          index1 = index3;
        }
      }
    }

    private static void InterpolateTempF(List<SurfaceWeather> list)
    {
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        if (list[index1].TempF == -999.0)
        {
          int index2 = index1 - 1;
          int index3 = index1 + 1;
          while (list[index3].TempF == -999.0)
            ++index3;
          int index4 = index3;
          int totalMinutes = (int) (list[index4].TimeStamp - list[index2].TimeStamp).TotalMinutes;
          double num = (list[index4].TempF - list[index2].TempF) / (double) totalMinutes;
          for (int index5 = index2 + 1; index5 < index4; ++index5)
          {
            TimeSpan timeSpan = list[index5].TimeStamp - list[index2].TimeStamp;
            list[index5].TempF = list[index2].TempF + num * timeSpan.TotalMinutes;
          }
          index1 = index3;
        }
      }
    }

    private static void InterpolateDewTempF(List<SurfaceWeather> list)
    {
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        if (list[index1].DewTempF == -999.0)
        {
          int index2 = index1 - 1;
          int index3 = index1 + 1;
          while (list[index3].DewTempF == -999.0)
            ++index3;
          int index4 = index3;
          int totalMinutes = (int) (list[index4].TimeStamp - list[index2].TimeStamp).TotalMinutes;
          double num = (list[index4].DewTempF - list[index2].DewTempF) / (double) totalMinutes;
          for (int index5 = index2 + 1; index5 < index4; ++index5)
          {
            TimeSpan timeSpan = list[index5].TimeStamp - list[index2].TimeStamp;
            list[index5].DewTempF = list[index2].DewTempF + num * timeSpan.TotalMinutes;
          }
          index1 = index3;
        }
      }
    }

    private static void InterpolateWdDir(List<SurfaceWeather> list)
    {
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        if (list[index1].WdDir == -999.0)
        {
          int index2 = index1 - 1;
          int index3 = index1 + 1;
          while (list[index3].WdDir == -999.0)
            ++index3;
          int index4 = index3;
          int totalMinutes = (int) (list[index4].TimeStamp - list[index2].TimeStamp).TotalMinutes;
          double num = (list[index4].WdDir - list[index2].WdDir) / (double) totalMinutes;
          for (int index5 = index2 + 1; index5 < index4; ++index5)
          {
            TimeSpan timeSpan = list[index5].TimeStamp - list[index2].TimeStamp;
            list[index5].WdDir = list[index2].WdDir + num * timeSpan.TotalMinutes;
          }
          index1 = index3;
        }
      }
    }

    private static void InterpolateWdSpdMh(List<SurfaceWeather> list)
    {
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        if (list[index1].WdSpdMh == -999.0)
        {
          int index2 = index1 - 1;
          int index3 = index1 + 1;
          while (list[index3].WdSpdMh == -999.0)
            ++index3;
          int index4 = index3;
          int totalMinutes = (int) (list[index4].TimeStamp - list[index2].TimeStamp).TotalMinutes;
          double num = (list[index4].WdSpdMh - list[index2].WdSpdMh) / (double) totalMinutes;
          for (int index5 = index2 + 1; index5 < index4; ++index5)
          {
            TimeSpan timeSpan = list[index5].TimeStamp - list[index2].TimeStamp;
            list[index5].WdSpdMh = list[index2].WdSpdMh + num * timeSpan.TotalMinutes;
          }
          index1 = index3;
        }
      }
    }

    private static void InterpolateCeiling(List<SurfaceWeather> list)
    {
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        if (list[index1].Ceiling == -999.0)
        {
          int num = index1 - 1;
          int index2 = index1 + 1;
          while (list[index2].Ceiling == -999.0)
            ++index2;
          int index3 = index2;
          for (int index4 = num + 1; index4 < index3; ++index4)
            list[index4].Ceiling = list[index3].Ceiling;
          index1 = index2;
        }
      }
    }

    private static void InterpolateToCldCov(List<SurfaceWeather> list)
    {
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        if (list[index1].ToCldCov == -999.0)
        {
          int num = index1 - 1;
          int index2 = index1 + 1;
          while (list[index2].ToCldCov == -999.0)
            ++index2;
          int index3 = index2;
          for (int index4 = num + 1; index4 < index3; ++index4)
            list[index4].ToCldCov = list[index3].ToCldCov;
          index1 = index2;
        }
      }
    }

    private static void InterpolateNetRadWm2(List<SurfaceWeather> list)
    {
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        if (list[index1].NetRadWm2 == -999.0)
        {
          int index2 = index1 - 1;
          int index3 = index1 + 1;
          while (list[index3].NetRadWm2 == -999.0)
            ++index3;
          int index4 = index3;
          int totalMinutes = (int) (list[index4].TimeStamp - list[index2].TimeStamp).TotalMinutes;
          double num = (list[index4].NetRadWm2 - list[index2].NetRadWm2) / (double) totalMinutes;
          for (int index5 = index2 + 1; index5 < index4; ++index5)
          {
            TimeSpan timeSpan = list[index5].TimeStamp - list[index2].TimeStamp;
            list[index5].NetRadWm2 = list[index2].NetRadWm2 + num * timeSpan.TotalMinutes;
          }
          index1 = index3;
        }
      }
    }

    public static void CalcAirDensity(List<SurfaceWeather> weaList, int recCnt)
    {
      for (int index = 0; index < recCnt; ++index)
        weaList[index].AirDens = weaList[index].PrsMbar * 100.0 / (287.05 * weaList[index].TempK);
    }

    public static void CalcSolarZenithAngle(
      List<SurfaceWeather> weaList,
      LocationData locData,
      int recCnt)
    {
      for (int index = 0; index < recCnt; ++index)
      {
        double num1 = SurfaceWeather.Deg2Rad((double) (360 * (weaList[index].Jday - 1)) / 365.0);
        double num2 = (7.5E-05 + 0.001868 * Math.Cos(num1) - 0.032077 * Math.Sin(num1) - 0.014615 * Math.Cos(2.0 * num1) - 0.040849 * Math.Sin(2.0 * num1)) * 229.18;
        double num3 = SurfaceWeather.Deg2Rad((0.006918 - 0.399912 * Math.Cos(num1) + 0.070257 * Math.Sin(num1) - 0.006758 * Math.Cos(2.0 * num1) + 0.000907 * Math.Sin(2.0 * num1) - 0.002697 * Math.Cos(3.0 * num1) + 0.00148 * Math.Sin(3.0 * num1)) * (180.0 / Math.PI));
        double num4 = SurfaceWeather.Deg2Rad(locData.Latitude);
        double num5 = 15.0 * locData.GMTOffset;
        double d1 = SurfaceWeather.Deg2Rad(15.0 * ((double) weaList[index].Hour + (4.0 * (Math.Abs(num5) - Math.Abs(locData.Longitude)) + num2) / 60.0) - 180.0);
        double d2 = Math.Sin(num3) * Math.Sin(num4) + Math.Cos(num3) * Math.Cos(num4) * Math.Cos(d1);
        if (d2 > 1.0)
          d2 = 1.0;
        else if (d2 < -1.0)
          d2 = -1.0;
        double num6 = SurfaceWeather.Rad2Deg(Math.Acos(d2));
        double num7 = 90.0 - num6;
        double num8 = 0.0;
        if (-1.0 <= num7 && num7 < 15.0)
          num8 = 3.5163977095262888 * (0.1594 + 0.0196 * num7 + 2E-05 * Math.Pow(num7, 2.0)) / (1.0 + 0.505 * num7 + 0.0845 * Math.Pow(num7, 2.0));
        else if (15.0 <= num7 && num7 < 90.0)
          num8 = 0.015894117647058822 / Math.Tan(SurfaceWeather.Deg2Rad(num7));
        weaList[index].SolZenAgl = num6 - num8;
      }
    }

    private static double Deg2Rad(double angle) => Math.PI * angle / 180.0;

    private static double Rad2Deg(double angle) => 180.0 * angle / Math.PI;

    public static void CalcSolarRadiation(
      List<SurfaceWeather> weaList,
      LocationData locData,
      int recCnt)
    {
      for (int index = 0; index < recCnt; ++index)
      {
        double ETR;
        double ETRN;
        SurfaceWeather.CalcExoatmRad(weaList[index].Jday, weaList[index].SolZenAgl, out ETR, out ETRN);
        double pcAirMass;
        SurfaceWeather.CalcAirMass(weaList[index].SolZenAgl, ref weaList[index].AirMass, weaList[index].PrsKpa, out pcAirMass);
        double opqn;
        double opqd;
        SurfaceWeather.CalcCloudCover(weaList[index].AirMass, weaList[index].Ceiling, weaList[index].ToCldCov, ref weaList[index].OpCldCov, ref weaList[index].TrCldCov, out opqn, out opqd);
        double Tr;
        double Toz;
        double Tum;
        double Ta;
        double Kn;
        SurfaceWeather.CalcTransmissionValue(weaList[index].Day, weaList[index].AirMass, weaList[index].TempC, weaList[index].DewTempC, weaList[index].PrsMbar, ref weaList[index].SatVPrsKpa, ref weaList[index].VapPrsKpa, ref weaList[index].RelHum, locData.ozone[weaList[index].Month - 1], locData.CoeffA, locData.CoeffPHI, locData.CoeffC, locData.Elevation, pcAirMass, opqn, out Tr, out Toz, out Tum, out Ta, out Kn);
        double Kd;
        SurfaceWeather.CalcDiffuseValue(weaList[index].AirMass, weaList[index].OpCldCov, weaList[index].TrCldCov, weaList[index].RainMh, locData.AlbedoVal[weaList[index].Month - 1], Tr, Toz, Tum, Ta, opqd, Kn, out Kd);
        weaList[index].DirRadWm2 = Kn * ETRN;
        weaList[index].DifRadWm2 = Kd * ETR;
        weaList[index].GlbRadWm2 = (Kn + Kd) * ETR;
        weaList[index].PARWm2 = weaList[index].GlbRadWm2 * 0.46;
        weaList[index].PARuEm2s = weaList[index].PARWm2 * 4.6;
        if (SurfaceWeather.weaFormat != SurfaceWeather.WeatherDataFormat.NARCCAP)
          SurfaceWeather.CalcNetRad(weaList[index].TempK, weaList[index].DewTempC, weaList[index].ToCldCov, weaList[index].GlbRadWm2, ref weaList[index].NetRadWm2, locData.AlbedoVal[weaList[index].Month - 1]);
      }
    }

    private static void CalcExoatmRad(int jDay, double zenAgl, out double ETR, out double ETRN)
    {
      double num1 = SurfaceWeather.Deg2Rad((double) (360 * (jDay - 1)) / 365.0);
      double num2 = 1.00011 + 0.034221 * Math.Cos(num1) + 0.00128 * Math.Sin(num1) + 0.000719 * Math.Cos(2.0 * num1) + 7.7E-05 * Math.Sin(2.0 * num1);
      ETR = num2 * 1367.0 * Math.Cos(SurfaceWeather.Deg2Rad(zenAgl));
      ETRN = num2 * 1367.0;
      if (ETR >= 0.0)
        return;
      ETR = 0.0;
      ETRN = 0.0;
    }

    private static void CalcAirMass(
      double zenAgl,
      ref double airMass,
      double prsKpa,
      out double pcAirMass)
    {
      double angle = 90.0 - zenAgl;
      airMass = zenAgl > 90.0 ? 99.0 : 1.0 / (Math.Sin(SurfaceWeather.Deg2Rad(angle)) + 0.50572 * Math.Pow(angle + 6.07995, -1.6364));
      if (airMass >= 30.0)
        airMass = 30.0;
      pcAirMass = airMass * (prsKpa / 10.0) / 1013.0;
    }

    private static void CalcCloudCover(
      double airMass,
      double Ceiling,
      double total,
      ref double opaque,
      ref double trans,
      out double opqn,
      out double opqd)
    {
      opaque = Ceiling <= 700.0 || total > 5.0 ? (Ceiling <= 700.0 || total <= 5.0 ? (Ceiling > 70.0 || total < 0.0 ? 0.0 : total) : 5.0) : total;
      trans = total - opaque;
      double num1;
      double num2;
      if (1.0 <= opaque && opaque <= 9.0)
      {
        double num3 = 4.955 * (1.0 - Math.Exp(-0.454 * airMass)) - 3.4;
        double num4 = num3 > 0.0 ? 0.1 * num3 : -0.2 * num3;
        num1 = num3 * Math.Sin(Math.PI / 10.0 * total) + num4 * Math.Sin(Math.PI / 5.0 * total);
        num2 = num3 * Math.Sin(Math.PI / 10.0 * total) * 0.5;
      }
      else
      {
        num1 = 0.0;
        num2 = 0.0;
      }
      opqn = opaque + num1;
      opqd = opaque + num2;
    }

    private static void CalcTransmissionValue(
      int day,
      double airMass,
      double tempC,
      double dewC,
      double prsMb,
      ref double SatVPrsKpa,
      ref double VapPrsKpa,
      ref double RelHum,
      double ozone,
      double a,
      double phi,
      double C,
      double elev,
      double pcAirMass,
      double opqn,
      out double Tr,
      out double Toz,
      out double Tum,
      out double Ta,
      out double Kn)
    {
      Tr = Math.Exp(-0.0903 * Math.Pow(pcAirMass, 0.84) * (1.0 + pcAirMass - Math.Pow(pcAirMass, 1.01)));
      double x = ozone * airMass;
      Toz = 1.0 - 0.1611 * x * Math.Pow(1.0 + 139.45 * x, -0.3035) - 0.002715 * x / (1.0 + 0.044 * x + 0.0003 * Math.Pow(x, 2.0));
      Tum = Math.Exp(-0.0127 * Math.Pow(pcAirMass, 0.26));
      double num1 = a * Math.Sin(360.0 * (double) day / 365.0 - phi) + C;
      Ta = Math.Exp(-num1 * airMass);
      SatVPrsKpa = 0.6108 * Math.Exp(17.27 * tempC / (237.3 + tempC));
      VapPrsKpa = 0.6108 * Math.Exp(17.27 * dewC / (237.3 + dewC));
      RelHum = VapPrsKpa / SatVPrsKpa;
      double num2 = (VapPrsKpa * 10.0 * prsMb / 1013.25 * (0.0004 * elev + 1.1) + 1.0) / 10.0 * airMass;
      double num3 = 1.0 - 1.668 * num2 / (Math.Pow(1.0 + 54.6 * num2, 0.637) + 4.042 * num2);
      double num4 = 1.0;
      double num5 = (10.0 - opqn) / 10.0;
      Kn = 0.9751 * Tr * Toz * Tum * num3 * Ta * num5 * num4;
    }

    private static void CalcDiffuseValue(
      double airMass,
      double opaque,
      double Trans,
      double rain,
      double albedo,
      double Tr,
      double Toz,
      double Tum,
      double Ta,
      double opqd,
      double Kn,
      out double Kd)
    {
      double num1 = 1.0 - 0.1 * (1.0 - airMass + Math.Pow(airMass, 1.06)) * (1.0 - Ta);
      double num2 = 0.5 * (1.0 - Tr) * Toz * Tum * num1;
      double num3 = 0.84 * (1.0 - Ta) * Toz * Tum * num1;
      double num4 = 0.38 + 0.925 * Math.Exp(-0.851 * airMass);
      double num5 = 0.0;
      double num6 = 0.0;
      if (opqd >= 5.0)
      {
        double num7 = 0.0953 + 0.137 * opqd - 0.0409 * Math.Pow(opqd, 2.0) + 0.00579 * Math.Pow(opqd, 3.0) - 0.000328 * Math.Pow(opqd, 4.0);
        double num8 = -0.109 - 0.02 * opqd + 0.011 * Math.Pow(opqd, 2.0) - 0.00156 * Math.Pow(opqd, 3.0) + 0.000121 * Math.Pow(opqd, 4.0);
        num6 = num7 * Ta - 0.06 + num8 * Math.Pow(Ta, 2.0);
        if (num6 <= 0.0)
          num6 = 0.0;
      }
      double num9 = opaque < 8.0 ? 1.0 : (rain == 0.0 ? 1.0 : 0.6);
      double num10 = num2 + num3;
      double num11 = (num4 * num10 + num6 + num5) * num9;
      double num12 = 0.06 * opaque + 0.02 * Trans;
      double num13 = (Kn + num11) * num12 * (albedo - 0.2);
      if (num13 <= 0.0)
        num13 = 0.01;
      double num14 = (0.0685 + 0.16 * (1.0 - Ta / num1)) * (10.0 - opaque) / 10.0;
      double num15 = (Kn + num11) * num14 * albedo;
      double num16 = num13 + num15;
      Kd = num11 + num16;
    }

    private static void CalcNetRad(
      double TempK,
      double dewC,
      double ToCldCov,
      double glbRad,
      ref double netRad,
      double albedo)
    {
      double num1 = 0.741 + 0.0062 * dewC;
      double num2 = 5.67E-08 * Math.Pow(TempK, 4.0);
      double num3 = ToCldCov / 10.0 * 5.67E-08 * Math.Pow(TempK, 4.0);
      double num4 = 1.0 - ToCldCov / 10.0;
      double num5 = num1 * num4 * 5.67E-08 * Math.Pow(TempK, 4.0);
      double num6 = (1.0 - albedo) * glbRad;
      double num7 = 0.97 * (num5 + num3 - num2);
      netRad = num6 + num7 < 0.0 ? 0.0 : num6 + num7;
    }

    public static void CalcET(
      List<SurfaceWeather> weatherData,
      List<LeafAreaIndex> laiList,
      int recCnt)
    {
      try
      {
        for (int index = 0; index < recCnt; ++index)
        {
          double num1 = 0.6108 * Math.Exp(17.27 * weatherData[index].DewTempC / (237.3 + weatherData[index].DewTempC));
          double num2 = weatherData[index].SatVPrsKpa - num1;
          double num3 = weatherData[index].WdSpdMs;
          if (num3 == 0.0)
            num3 = 0.0001;
          double num4 = num3 * (Math.Log(5109.4890510948908) / Math.Log(1459.8540145985403));
          double num5 = num3 * (Math.Log(73.992700729927009) / Math.Log(1459.8540145985403));
          double num6 = -0.0051 * Math.Pow(weatherData[index].TempC, 2.0) + 0.018 * weatherData[index].TempC + 999.88;
          double num7 = 3.486 * (weatherData[index].PrsKpa / (275.0 + weatherData[index].TempC));
          double num8 = 2.501 - 0.002361 * weatherData[index].TempC;
          double num9 = 0.0016286 * (weatherData[index].PrsKpa / num8);
          double num10 = 4098.0 * weatherData[index].SatVPrsKpa / Math.Pow(237.3 + weatherData[index].TempC, 2.0);
          double num11 = weatherData[index].NetRadWm2;
          if (num11 <= 0.0)
            num11 = 0.0;
          double num12 = laiList[index].Lai != 0.0 ? (laiList[index].Lai >= 1.0 ? 200.0 / laiList[index].Lai : 200.0) : 0.0;
          double num13 = Math.Log(17.115960633290541) * Math.Log(3500.0 / 41.0) / (Math.Pow(0.41, 2.0) * num3);
          double num14 = 208.0 / num3;
          double num15 = 208.0 / num4;
          double num16 = 208.0 / num5;
          weatherData[index].PtTrMh = num10 * num11 + num7 * 1013.0 * num2 / num15;
          weatherData[index].PtTrMh /= num10 + num9 * (1.0 + num12 / num15);
          weatherData[index].PtTrMh /= num8 * num6;
          weatherData[index].PtTrMh /= 1000.0;
          if (weatherData[index].PtTrMh < 0.0)
            weatherData[index].PtTrMh = 0.0;
          double num17 = 4.72 * Math.Pow(Math.Log(1459.8540145985403), 2.0) / (1.0 + 0.536 * num3);
          double num18 = 4.72 * Math.Pow(Math.Log(599.058622165169), 2.0) / (1.0 + 0.536 * num4);
          double num19 = 4.72 * Math.Pow(Math.Log(118687.31826004393), 2.0) / (1.0 + 0.536 * num5);
          weatherData[index].PeTrMh = num10 * num11 + num7 * 1013.0 * num2 / num18;
          weatherData[index].PeTrMh /= num10 + num9;
          weatherData[index].PeTrMh /= num8 * num6;
          weatherData[index].PeTrMh /= 1000.0;
          if (weatherData[index].PeTrMh < 0.0)
            weatherData[index].PeTrMh = 0.0;
          weatherData[index].PeGrMh = num10 * num11 + num7 * 1013.0 * num2 / num19;
          weatherData[index].PeGrMh /= num10 + num9;
          weatherData[index].PeGrMh /= num8 * num6;
          weatherData[index].PeGrMh /= 1000.0;
          if (weatherData[index].PeGrMh < 0.0)
            weatherData[index].PeGrMh = 0.0;
          weatherData[index].PeSnTrMh = 0.1 / weatherData[index].PrsKpa * num4 / Math.Pow(Math.Log(5109.4890510948908), 2.0) * (611.2 - num1);
          weatherData[index].PeSnTrMh /= 1000.0;
          if (weatherData[index].PeSnTrMh < 0.0)
            weatherData[index].PeSnTrMh = 0.0;
          weatherData[index].PeSnGrMh = 0.1 / weatherData[index].PrsKpa * num5 / Math.Pow(Math.Log(1459.8540145985403), 2.0) * (611.2 - num1);
          weatherData[index].PeSnGrMh /= 1000.0;
          if (weatherData[index].PeSnGrMh < 0.0)
            weatherData[index].PeSnGrMh = 0.0;
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static void CalcPrecipInterceptByCanopy(
      List<SurfaceWeather> wList,
      List<LeafAreaIndex> laiList,
      int recCnt,
      DryDeposition.VEG_TYPE vegType)
    {
      for (int index = 0; index < recCnt; ++index)
      {
        double num1 = index == 0 ? 0.0 : wList[index - 1].VegStMh;
        double num2 = index == 0 ? 0.0 : wList[index - 1].VegEvMh;
        double num3 = vegType == DryDeposition.VEG_TYPE.TREE ? wList[index].PeTrMh : wList[index].PeGrMh;
        double num4 = vegType == DryDeposition.VEG_TYPE.TREE ? 1.0 - Math.Exp(-0.7 * laiList[index].Lai) : 1.0 - Math.Exp(-0.3 * laiList[index].Lai);
        double num5 = 0.0002 * laiList[index].Lai;
        double num6 = wList[index].RainMh * (1.0 - num4);
        double num7 = wList[index].RainMh - num6;
        wList[index].VegStMh = num1 + num7 - num2;
        if (wList[index].VegStMh > num5)
          wList[index].VegStMh = num5;
        else if (wList[index].VegStMh < 0.0)
          wList[index].VegStMh = 0.0;
        if (num5 == 0.0)
        {
          wList[index].VegEvMh = 0.0;
        }
        else
        {
          wList[index].VegEvMh = Math.Pow(wList[index].VegStMh / num5, 2.0 / 3.0) * num3;
          if (wList[index].VegEvMh > wList[index].VegStMh)
            wList[index].VegEvMh = wList[index].VegStMh;
        }
        if (wList[index].VegStMh < num5)
        {
          wList[index].UnderCanThrufallMh = num6;
        }
        else
        {
          double num8;
          if (num1 < num5)
          {
            num8 = num7 - (num5 - num1) - num2;
            if (num8 < 0.0)
              num8 = 0.0;
          }
          else
          {
            num8 = num7 - num2;
            if (num8 < 0.0)
              num8 = 0.0;
          }
          wList[index].UnderCanThrufallMh = num8 + num6;
        }
        wList[index].VegIntcptMh = wList[index].RainMh <= 0.0 ? 0.0 : wList[index].RainMh - wList[index].UnderCanThrufallMh;
      }
    }

    public static void CalcPrecipInterceptByUnderCanopyCover(List<SurfaceWeather> wList, int recCnt)
    {
      for (int index = 0; index < recCnt; ++index)
      {
        double num1 = index == 0 ? 0.0 : wList[index - 1].UnderCanPervStMh;
        double num2 = index == 0 ? 0.0 : wList[index - 1].UnderCanImpervStMh;
        double num3 = index == 0 ? 0.0 : wList[index - 1].UnderCanPervEvMh;
        double num4 = index == 0 ? 0.0 : wList[index - 1].UnderCanImpervEvMh;
        double peGrMh = wList[index].PeGrMh;
        double num5 = 0.001;
        double num6 = 0.0015;
        wList[index].UnderCanPervStMh = num1 + wList[index].UnderCanThrufallMh - num3;
        if (wList[index].UnderCanPervStMh > num5)
          wList[index].UnderCanPervStMh = num5;
        else if (wList[index].UnderCanPervStMh < 0.0)
          wList[index].UnderCanPervStMh = 0.0;
        wList[index].UnderCanImpervStMh = num2 + wList[index].UnderCanThrufallMh - num4;
        if (wList[index].UnderCanImpervStMh > num6)
          wList[index].UnderCanImpervStMh = num6;
        else if (wList[index].UnderCanImpervStMh < 0.0)
          wList[index].UnderCanImpervStMh = 0.0;
        wList[index].UnderCanPervEvMh = wList[index].UnderCanPervStMh / num5 * peGrMh;
        if (wList[index].UnderCanPervEvMh > wList[index].UnderCanPervStMh)
          wList[index].UnderCanPervEvMh = wList[index].UnderCanPervStMh;
        wList[index].UnderCanImpervEvMh = wList[index].UnderCanImpervStMh / num6 * peGrMh;
        if (wList[index].UnderCanImpervEvMh > wList[index].UnderCanImpervStMh)
          wList[index].UnderCanImpervEvMh = wList[index].UnderCanImpervStMh;
        wList[index].UnderCanImpervRunoffMh = 0.0;
        if (num2 < num6)
          wList[index].UnderCanImpervRunoffMh = wList[index].UnderCanThrufallMh - (num6 - num2) - wList[index].UnderCanImpervEvMh;
        else if (num2 == num6)
          wList[index].UnderCanImpervRunoffMh = wList[index].UnderCanThrufallMh - wList[index].UnderCanImpervEvMh;
        if (wList[index].UnderCanImpervRunoffMh < 0.0)
          wList[index].UnderCanImpervRunoffMh = 0.0;
        wList[index].UnderCanPervInfilMh = 0.0;
        if (num1 < num5)
          wList[index].UnderCanPervInfilMh = wList[index].UnderCanThrufallMh - (num5 - num1) - wList[index].UnderCanPervEvMh;
        else if (num1 == num5)
          wList[index].UnderCanPervInfilMh = wList[index].UnderCanThrufallMh - wList[index].UnderCanPervEvMh;
        if (wList[index].UnderCanPervInfilMh < 0.0)
          wList[index].UnderCanPervInfilMh = 0.0;
      }
    }

    public static void CalcPrecipInterceptByNoCanopyCover(List<SurfaceWeather> wList, int recCnt)
    {
      for (int index = 0; index < recCnt; ++index)
      {
        double num1 = index == 0 ? 0.0 : wList[index - 1].NoCanPervStMh;
        double num2 = index == 0 ? 0.0 : wList[index - 1].NoCanImpervStMh;
        double num3 = index == 0 ? 0.0 : wList[index - 1].NoCanPervEvMh;
        double num4 = index == 0 ? 0.0 : wList[index - 1].NoCanImpervEvMh;
        double peGrMh = wList[index].PeGrMh;
        double num5 = 0.001;
        double num6 = 0.0015;
        wList[index].NoCanPervStMh = num1 + wList[index].RainMh - num3;
        if (wList[index].NoCanPervStMh > num5)
          wList[index].NoCanPervStMh = num5;
        else if (wList[index].NoCanPervStMh < 0.0)
          wList[index].NoCanPervStMh = 0.0;
        wList[index].NoCanImpervStMh = num2 + wList[index].RainMh - num4;
        if (wList[index].NoCanImpervStMh > num6)
          wList[index].NoCanImpervStMh = num6;
        else if (wList[index].NoCanImpervStMh < 0.0)
          wList[index].NoCanImpervStMh = 0.0;
        wList[index].NoCanPervEvMh = wList[index].NoCanPervStMh / num5 * peGrMh;
        if (wList[index].NoCanPervEvMh > wList[index].NoCanPervStMh)
          wList[index].NoCanPervEvMh = wList[index].NoCanPervStMh;
        wList[index].NoCanImpervEvMh = wList[index].NoCanImpervStMh / num6 * peGrMh;
        if (wList[index].NoCanImpervEvMh > wList[index].NoCanImpervStMh)
          wList[index].NoCanImpervEvMh = wList[index].NoCanImpervStMh;
        wList[index].NoCanImpervRunoffMh = 0.0;
        if (num2 < num6)
          wList[index].NoCanImpervRunoffMh = wList[index].RainMh - (num6 - num2) - wList[index].NoCanImpervEvMh;
        else if (num2 == num6)
          wList[index].NoCanImpervRunoffMh = wList[index].RainMh - wList[index].NoCanImpervEvMh;
        if (wList[index].NoCanImpervRunoffMh < 0.0)
          wList[index].NoCanImpervRunoffMh = 0.0;
        wList[index].NoCanPervInfilMh = 0.0;
        if (num1 < num5)
          wList[index].NoCanPervInfilMh = wList[index].RainMh - (num5 - num1) - wList[index].NoCanPervEvMh;
        else if (num1 == num5)
          wList[index].NoCanPervInfilMh = wList[index].RainMh - wList[index].NoCanPervEvMh;
        if (wList[index].NoCanPervInfilMh < 0.0)
          wList[index].NoCanPervInfilMh = 0.0;
      }
    }

    public static void CreateSurfaceWeatherTable(OleDbConnection conn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "CREATE TABLE SurfaceWeather([TimeStamp] DateTime,[AirDens] DOUBLE,[AirMass] DOUBLE,[Ceiling] DOUBLE,[ToCldCov] DOUBLE,[OpCldCov] DOUBLE,[TrCldCov] DOUBLE,[PrsIn] DOUBLE,[PrsKPa] DOUBLE,[PrsMBar] DOUBLE,[PeTrMh] DOUBLE,[PeGrMh] DOUBLE,[PeSnTrMh] DOUBLE,[PeSnGrMh] DOUBLE,[PtTrMh] DOUBLE,[VegEvMh] DOUBLE,[VegStMh] DOUBLE,[VegIntcptMh] DOUBLE,[UnderCanThrufallMh] DOUBLE,[UnderCanPervEvMh] DOUBLE,[UnderCanPervStMh] DOUBLE,[UnderCanPervInfilMh] DOUBLE,[UnderCanImpervEvMh] DOUBLE,[UnderCanImpervStMh] DOUBLE,[UnderCanImpervRunoffMh] DOUBLE,[NoCanPervEvMh] DOUBLE,[NoCanPervStMh] DOUBLE,[NoCanPervInfilMh] DOUBLE,[NoCanImpervEvMh] DOUBLE,[NoCanImpervStMh] DOUBLE,[NoCanImpervRunoffMh] DOUBLE,[DirRadWm2] DOUBLE,[DifRadWm2] DOUBLE,[GlbRadWm2] DOUBLE,[PARWm2] DOUBLE,[PARuEm2s] DOUBLE,[NetRadWm2] DOUBLE,[RainInH] DOUBLE,[RainMh] DOUBLE,[RelHum] DOUBLE,[SatVPrsKPa] DOUBLE,[SnowIn] DOUBLE,[SnowM] DOUBLE,[SolZenAgl] DOUBLE,[TempC] DOUBLE,[TempF] DOUBLE,[TempK] DOUBLE,[DewTempC] DOUBLE,[DewTempF] DOUBLE,[VapPrsKPa] DOUBLE,[WdDir] DOUBLE,[WdSpdMh] DOUBLE,[WdSpdMs] DOUBLE,[WdSpdKnt] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void CreateSurfaceWeatherTable(string sDB)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        SurfaceWeather.CreateSurfaceWeatherTable(conn);
      }
    }

    public static void WriteSurfaceWeatherRecords(
      OleDbConnection conn,
      List<SurfaceWeather> weaList,
      int recCnt)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand("INSERT INTO SurfaceWeather([TimeStamp],[AirDens],[AirMass],[Ceiling],[ToCldCov],[OpCldCov],[TrCldCov],[PrsIn],[PrsKPa],[PrsMBar],[PeTrMh],[PeGrMh],[PeSnTrMh],[PeSnGrMh],[PtTrMh],[VegEvMh],[VegStMh],[VegIntcptMh],[UnderCanThrufallMh],[UnderCanPervEvMh],[UnderCanPervStMh],[UnderCanPervInfilMh],[UnderCanImpervEvMh],[UnderCanImpervStMh],[UnderCanImpervRunoffMh],[NoCanPervEvMh],[NoCanPervStMh],[NoCanPervInfilMh],[NoCanImpervEvMh],[NoCanImpervStMh],[NoCanImpervRunoffMh],[DirRadWm2],[DifRadWm2],[GlbRadWm2],[PARWm2],[PARuEm2s],[NetRadWm2],[RainInH],[RainMh],[RelHum],[SatVPrsKPa],[SnowIn],[SnowM],[SolZenAgl],[TempC],[TempF],[TempK],[DewTempC],[DewTempF],[VapPrsKPa],[WdDir],[WdSpdMh],[WdSpdMs],[WdSpdKnt]) Values (@TimeStamp,@AirDens,@AirMass,@Ceiling,@ToCldCov,@OpCldCov,@TrCldCov,@PrsIn,@PrsKpa,@PrsMbar,@PeTrMh,@PeGrMh,@PeSnTrMh,@PeSnGrMh,@PtTrMh,@VegEvMh,@VegStMh,@VegIntcptMh,@UnderCanThrufallMh,@UnderCanPervEvMh,@UnderCanPervStMh,@UnderCanPervInfilMh,@UnderCanImpervEvMh,@UnderCanImpervStMh,@UnderCanImpervRunoffMh,@NoCanPervEvMh,@NoCanPervStMh,@NoCanPervInfilMh,@NoCanImpervEvMh,@NoCanImpervStMh,@NoCanImpervRunoffMh,@DirRadWm2,@DifRadWm2,@GlbRadWm2,@PARWm2,@PARuEm2s,@NetRadWm2,@RainInH,@RainMh,@RelHum,@SatVPrsKpa,@SnowIn,@SnowM,@SolZenAgl,@TempC,@TempF,@TempK,@DewTempC,@DewTempF,@VapPrsKpa,@WdDir,@WdSpdMh,@WdSpdMs,@WdSpdKnt)", conn))
      {
        oleDbCommand.Parameters.Add("@TimeStamp", OleDbType.Date);
        oleDbCommand.Parameters.Add("@AirDens", OleDbType.Double);
        oleDbCommand.Parameters.Add("@AirMass", OleDbType.Double);
        oleDbCommand.Parameters.Add("@Ceiling", OleDbType.Double);
        oleDbCommand.Parameters.Add("@ToCldCov", OleDbType.Double);
        oleDbCommand.Parameters.Add("@OpCldCov", OleDbType.Double);
        oleDbCommand.Parameters.Add("@TrCldCov", OleDbType.Double);
        oleDbCommand.Parameters.Add("@PrsIn", OleDbType.Double);
        oleDbCommand.Parameters.Add("@PrsKpa", OleDbType.Double);
        oleDbCommand.Parameters.Add("@PrsMbar", OleDbType.Double);
        oleDbCommand.Parameters.Add("@PeTrMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@PeGrMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@PeSnTrMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@PeSnGrMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@PtTrMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@VegEvMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@VegStMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@VegIntcptMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@UnderCanThrufallMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@UnderCanPervEvMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@UnderCanPervStMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@UnderCanPervInfilMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@UnderCanImpervEvMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@UnderCanImpervStMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@UnderCanImpervRunoffMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@NoCanPervEvMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@NoCanPervStMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@NoCanPervInfilMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@NoCanImpervEvMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@NoCanImpervStMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@NoCanImpervRunoffMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@DirRadWm2", OleDbType.Double);
        oleDbCommand.Parameters.Add("@DifRadWm2", OleDbType.Double);
        oleDbCommand.Parameters.Add("@GlbRadWm2", OleDbType.Double);
        oleDbCommand.Parameters.Add("@PARWm2", OleDbType.Double);
        oleDbCommand.Parameters.Add("@PARuEm2s", OleDbType.Double);
        oleDbCommand.Parameters.Add("@NetRadWm2", OleDbType.Double);
        oleDbCommand.Parameters.Add("@RainInH", OleDbType.Double);
        oleDbCommand.Parameters.Add("@RainMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@RelHum", OleDbType.Double);
        oleDbCommand.Parameters.Add("@SatVPrsKpa", OleDbType.Double);
        oleDbCommand.Parameters.Add("@SnowIn", OleDbType.Double);
        oleDbCommand.Parameters.Add("@SnowM", OleDbType.Double);
        oleDbCommand.Parameters.Add("@SolZenAgl", OleDbType.Double);
        oleDbCommand.Parameters.Add("@TempC", OleDbType.Double);
        oleDbCommand.Parameters.Add("@TempF", OleDbType.Double);
        oleDbCommand.Parameters.Add("@TempK", OleDbType.Double);
        oleDbCommand.Parameters.Add("@DewTempC", OleDbType.Double);
        oleDbCommand.Parameters.Add("@DewTempF", OleDbType.Double);
        oleDbCommand.Parameters.Add("@VapPrsKpa", OleDbType.Double);
        oleDbCommand.Parameters.Add("@WdDir", OleDbType.Double);
        oleDbCommand.Parameters.Add("@WdSpdMh", OleDbType.Double);
        oleDbCommand.Parameters.Add("@WdSpdMs", OleDbType.Double);
        oleDbCommand.Parameters.Add("@WdSpdKnt", OleDbType.Double);
        oleDbCommand.Parameters["@TimeStamp"].Value = (object) weaList[0].TimeStamp;
        oleDbCommand.Parameters["@AirDens"].Value = (object) weaList[0].AirDens;
        oleDbCommand.Parameters["@AirMass"].Value = (object) weaList[0].AirMass;
        oleDbCommand.Parameters["@Ceiling"].Value = (object) weaList[0].Ceiling;
        oleDbCommand.Parameters["@ToCldCov"].Value = (object) weaList[0].ToCldCov;
        oleDbCommand.Parameters["@OpCldCov"].Value = (object) weaList[0].OpCldCov;
        oleDbCommand.Parameters["@TrCldCov"].Value = (object) weaList[0].TrCldCov;
        oleDbCommand.Parameters["@PrsIn"].Value = (object) weaList[0].PrsIn;
        oleDbCommand.Parameters["@PrsKpa"].Value = (object) weaList[0].PrsKpa;
        oleDbCommand.Parameters["@PrsMbar"].Value = (object) weaList[0].PrsMbar;
        oleDbCommand.Parameters["@PeTrMh"].Value = (object) weaList[0].PeTrMh;
        oleDbCommand.Parameters["@PeGrMh"].Value = (object) weaList[0].PeGrMh;
        oleDbCommand.Parameters["@PeSnTrMh"].Value = (object) weaList[0].PeSnTrMh;
        oleDbCommand.Parameters["@PeSnGrMh"].Value = (object) weaList[0].PeSnGrMh;
        oleDbCommand.Parameters["@PtTrMh"].Value = (object) weaList[0].PtTrMh;
        oleDbCommand.Parameters["@VegEvMh"].Value = (object) weaList[0].VegEvMh;
        oleDbCommand.Parameters["@VegStMh"].Value = (object) weaList[0].VegStMh;
        oleDbCommand.Parameters["@VegIntcptMh"].Value = (object) weaList[0].VegIntcptMh;
        oleDbCommand.Parameters["@UnderCanThrufallMh"].Value = (object) weaList[0].UnderCanThrufallMh;
        oleDbCommand.Parameters["@UnderCanPervEvMh"].Value = (object) weaList[0].UnderCanPervEvMh;
        oleDbCommand.Parameters["@UnderCanPervStMh"].Value = (object) weaList[0].UnderCanPervStMh;
        oleDbCommand.Parameters["@UnderCanPervInfilMh"].Value = (object) weaList[0].UnderCanPervInfilMh;
        oleDbCommand.Parameters["@UnderCanImpervEvMh"].Value = (object) weaList[0].UnderCanImpervEvMh;
        oleDbCommand.Parameters["@UnderCanImpervStMh"].Value = (object) weaList[0].UnderCanImpervStMh;
        oleDbCommand.Parameters["@UnderCanImpervRunoffMh"].Value = (object) weaList[0].UnderCanImpervRunoffMh;
        oleDbCommand.Parameters["@NoCanPervEvMh"].Value = (object) weaList[0].NoCanPervEvMh;
        oleDbCommand.Parameters["@NoCanPervStMh"].Value = (object) weaList[0].NoCanPervStMh;
        oleDbCommand.Parameters["@NoCanPervInfilMh"].Value = (object) weaList[0].NoCanPervInfilMh;
        oleDbCommand.Parameters["@NoCanImpervEvMh"].Value = (object) weaList[0].NoCanImpervEvMh;
        oleDbCommand.Parameters["@NoCanImpervStMh"].Value = (object) weaList[0].NoCanImpervStMh;
        oleDbCommand.Parameters["@NoCanImpervRunoffMh"].Value = (object) weaList[0].NoCanImpervRunoffMh;
        oleDbCommand.Parameters["@DirRadWm2"].Value = (object) weaList[0].DirRadWm2;
        oleDbCommand.Parameters["@DifRadWm2"].Value = (object) weaList[0].DifRadWm2;
        oleDbCommand.Parameters["@GlbRadWm2"].Value = (object) weaList[0].GlbRadWm2;
        oleDbCommand.Parameters["@PARWm2"].Value = (object) weaList[0].PARWm2;
        oleDbCommand.Parameters["@PARuEm2s"].Value = (object) weaList[0].PARuEm2s;
        oleDbCommand.Parameters["@NetRadWm2"].Value = (object) weaList[0].NetRadWm2;
        oleDbCommand.Parameters["@RainInH"].Value = (object) weaList[0].RainInH;
        oleDbCommand.Parameters["@RainMh"].Value = (object) weaList[0].RainMh;
        oleDbCommand.Parameters["@RelHum"].Value = (object) weaList[0].RelHum;
        oleDbCommand.Parameters["@SatVPrsKpa"].Value = (object) weaList[0].SatVPrsKpa;
        oleDbCommand.Parameters["@SnowIn"].Value = (object) weaList[0].SnowIn;
        oleDbCommand.Parameters["@SnowM"].Value = (object) weaList[0].SnowM;
        oleDbCommand.Parameters["@SolZenAgl"].Value = (object) weaList[0].SolZenAgl;
        oleDbCommand.Parameters["@TempC"].Value = (object) weaList[0].TempC;
        oleDbCommand.Parameters["@TempF"].Value = (object) weaList[0].TempF;
        oleDbCommand.Parameters["@TempK"].Value = (object) weaList[0].TempK;
        oleDbCommand.Parameters["@DewTempC"].Value = (object) weaList[0].DewTempC;
        oleDbCommand.Parameters["@DewTempF"].Value = (object) weaList[0].DewTempF;
        oleDbCommand.Parameters["@VapPrsKpa"].Value = (object) weaList[0].VapPrsKpa;
        oleDbCommand.Parameters["@WdDir"].Value = (object) weaList[0].WdDir;
        oleDbCommand.Parameters["@WdSpdMh"].Value = (object) weaList[0].WdSpdMh;
        oleDbCommand.Parameters["@WdSpdMs"].Value = (object) weaList[0].WdSpdMs;
        oleDbCommand.Parameters["@WdSpdKnt"].Value = (object) weaList[0].WdSpdKnt;
        oleDbCommand.Prepare();
        oleDbCommand.ExecuteNonQuery();
        for (int index = 1; index < recCnt; ++index)
        {
          oleDbCommand.Parameters["@TimeStamp"].Value = (object) weaList[index].TimeStamp;
          oleDbCommand.Parameters["@AirDens"].Value = (object) weaList[index].AirDens;
          oleDbCommand.Parameters["@AirMass"].Value = (object) weaList[index].AirMass;
          oleDbCommand.Parameters["@Ceiling"].Value = (object) weaList[index].Ceiling;
          oleDbCommand.Parameters["@ToCldCov"].Value = (object) weaList[index].ToCldCov;
          oleDbCommand.Parameters["@OpCldCov"].Value = (object) weaList[index].OpCldCov;
          oleDbCommand.Parameters["@TrCldCov"].Value = (object) weaList[index].TrCldCov;
          oleDbCommand.Parameters["@PrsIn"].Value = (object) weaList[index].PrsIn;
          oleDbCommand.Parameters["@PrsKpa"].Value = (object) weaList[index].PrsKpa;
          oleDbCommand.Parameters["@PrsMbar"].Value = (object) weaList[index].PrsMbar;
          oleDbCommand.Parameters["@PeTrMh"].Value = (object) weaList[index].PeTrMh;
          oleDbCommand.Parameters["@PeGrMh"].Value = (object) weaList[index].PeGrMh;
          oleDbCommand.Parameters["@PeSnTrMh"].Value = (object) weaList[index].PeSnTrMh;
          oleDbCommand.Parameters["@PeSnGrMh"].Value = (object) weaList[index].PeSnGrMh;
          oleDbCommand.Parameters["@PtTrMh"].Value = (object) weaList[index].PtTrMh;
          oleDbCommand.Parameters["@VegEvMh"].Value = (object) weaList[index].VegEvMh;
          oleDbCommand.Parameters["@VegStMh"].Value = (object) weaList[index].VegStMh;
          oleDbCommand.Parameters["@VegIntcptMh"].Value = (object) weaList[index].VegIntcptMh;
          oleDbCommand.Parameters["@UnderCanThrufallMh"].Value = (object) weaList[index].UnderCanThrufallMh;
          oleDbCommand.Parameters["@UnderCanPervEvMh"].Value = (object) weaList[index].UnderCanPervEvMh;
          oleDbCommand.Parameters["@UnderCanPervStMh"].Value = (object) weaList[index].UnderCanPervStMh;
          oleDbCommand.Parameters["@UnderCanPervInfilMh"].Value = (object) weaList[index].UnderCanPervInfilMh;
          oleDbCommand.Parameters["@UnderCanImpervEvMh"].Value = (object) weaList[index].UnderCanImpervEvMh;
          oleDbCommand.Parameters["@UnderCanImpervStMh"].Value = (object) weaList[index].UnderCanImpervStMh;
          oleDbCommand.Parameters["@UnderCanImpervRunoffMh"].Value = (object) weaList[index].UnderCanImpervRunoffMh;
          oleDbCommand.Parameters["@NoCanPervEvMh"].Value = (object) weaList[index].NoCanPervEvMh;
          oleDbCommand.Parameters["@NoCanPervStMh"].Value = (object) weaList[index].NoCanPervStMh;
          oleDbCommand.Parameters["@NoCanPervInfilMh"].Value = (object) weaList[index].NoCanPervInfilMh;
          oleDbCommand.Parameters["@NoCanImpervEvMh"].Value = (object) weaList[index].NoCanImpervEvMh;
          oleDbCommand.Parameters["@NoCanImpervStMh"].Value = (object) weaList[index].NoCanImpervStMh;
          oleDbCommand.Parameters["@NoCanImpervRunoffMh"].Value = (object) weaList[index].NoCanImpervRunoffMh;
          oleDbCommand.Parameters["@DirRadWm2"].Value = (object) weaList[index].DirRadWm2;
          oleDbCommand.Parameters["@DifRadWm2"].Value = (object) weaList[index].DifRadWm2;
          oleDbCommand.Parameters["@GlbRadWm2"].Value = (object) weaList[index].GlbRadWm2;
          oleDbCommand.Parameters["@PARWm2"].Value = (object) weaList[index].PARWm2;
          oleDbCommand.Parameters["@PARuEm2s"].Value = (object) weaList[index].PARuEm2s;
          oleDbCommand.Parameters["@NetRadWm2"].Value = (object) weaList[index].NetRadWm2;
          oleDbCommand.Parameters["@RainInH"].Value = (object) weaList[index].RainInH;
          oleDbCommand.Parameters["@RainMh"].Value = (object) weaList[index].RainMh;
          oleDbCommand.Parameters["@RelHum"].Value = (object) weaList[index].RelHum;
          oleDbCommand.Parameters["@SatVPrsKpa"].Value = (object) weaList[index].SatVPrsKpa;
          oleDbCommand.Parameters["@SnowIn"].Value = (object) weaList[index].SnowIn;
          oleDbCommand.Parameters["@SnowM"].Value = (object) weaList[index].SnowM;
          oleDbCommand.Parameters["@SolZenAgl"].Value = (object) weaList[index].SolZenAgl;
          oleDbCommand.Parameters["@TempC"].Value = (object) weaList[index].TempC;
          oleDbCommand.Parameters["@TempF"].Value = (object) weaList[index].TempF;
          oleDbCommand.Parameters["@TempK"].Value = (object) weaList[index].TempK;
          oleDbCommand.Parameters["@DewTempC"].Value = (object) weaList[index].DewTempC;
          oleDbCommand.Parameters["@DewTempF"].Value = (object) weaList[index].DewTempF;
          oleDbCommand.Parameters["@VapPrsKpa"].Value = (object) weaList[index].VapPrsKpa;
          oleDbCommand.Parameters["@WdDir"].Value = (object) weaList[index].WdDir;
          oleDbCommand.Parameters["@WdSpdMh"].Value = (object) weaList[index].WdSpdMh;
          oleDbCommand.Parameters["@WdSpdMs"].Value = (object) weaList[index].WdSpdMs;
          oleDbCommand.Parameters["@WdSpdKnt"].Value = (object) weaList[index].WdSpdKnt;
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    public static void WriteSurfaceWeatherRecords(
      string sDB,
      List<SurfaceWeather> weaList,
      int recCnt)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        SurfaceWeather.WriteSurfaceWeatherRecords(conn, weaList, recCnt);
      }
    }

    public static int ReadSurfaceAllRecords(
      OleDbConnection cnWeatherDB,
      ref List<SurfaceWeather> sfcData)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnWeatherDB;
        oleDbCommand.CommandText = "SELECT * FROM SurfaceWeather ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
            sfcData.Add(new SurfaceWeather()
            {
              TimeStamp = (DateTime) oleDbDataReader["TimeStamp"],
              AirDens = (double) oleDbDataReader["AirDens"],
              AirMass = (double) oleDbDataReader["AirMass"],
              Ceiling = (double) oleDbDataReader["Ceiling"],
              DewTempC = (double) oleDbDataReader["DewTempC"],
              DewTempF = (double) oleDbDataReader["DewTempF"],
              DifRadWm2 = (double) oleDbDataReader["DifRadWm2"],
              DirRadWm2 = (double) oleDbDataReader["DirRadWm2"],
              GlbRadWm2 = (double) oleDbDataReader["GlbRadWm2"],
              NetRadWm2 = (double) oleDbDataReader["NetRadWm2"],
              OpCldCov = (double) oleDbDataReader["OpCldCov"],
              PrsIn = (double) oleDbDataReader["PrsIn"],
              PrsKpa = (double) oleDbDataReader["PrsKpa"],
              PrsMbar = (double) oleDbDataReader["PrsMbar"],
              PeGrMh = (double) oleDbDataReader["PeGrMh"],
              PeTrMh = (double) oleDbDataReader["PeTrMh"],
              PeSnGrMh = (double) oleDbDataReader["PeSnGrMh"],
              PeSnTrMh = (double) oleDbDataReader["PeSnTrMh"],
              PtTrMh = (double) oleDbDataReader["PtTrMh"],
              PARuEm2s = (double) oleDbDataReader["PARuEm2s"],
              PARWm2 = (double) oleDbDataReader["PARWm2"],
              RainInH = (double) oleDbDataReader["RainInH"],
              RainMh = (double) oleDbDataReader["RainMh"],
              RelHum = (double) oleDbDataReader["RelHum"],
              SatVPrsKpa = (double) oleDbDataReader["SatVPrsKpa"],
              SnowIn = (double) oleDbDataReader["SnowIn"],
              SnowM = (double) oleDbDataReader["SnowM"],
              SolZenAgl = (double) oleDbDataReader["SolZenAgl"],
              TempC = (double) oleDbDataReader["TempC"],
              TempF = (double) oleDbDataReader["TempF"],
              TempK = (double) oleDbDataReader["TempK"],
              ToCldCov = (double) oleDbDataReader["ToCldCov"],
              TrCldCov = (double) oleDbDataReader["TrCldCov"],
              VapPrsKpa = (double) oleDbDataReader["VapPrsKpa"],
              VegStMh = (double) oleDbDataReader["VegStMh"],
              WdDir = (double) oleDbDataReader["WdDir"],
              WdSpdKnt = (double) oleDbDataReader["WdSpdKnt"],
              WdSpdMh = (double) oleDbDataReader["WdSpdMh"],
              WdSpdMs = (double) oleDbDataReader["WdSpdMs"]
            });
        }
        return sfcData.Count;
      }
    }

    [Obsolete]
    public static int ReadSurfaceAllRecords(string sWeatherDB, ref List<SurfaceWeather> sfcData)
    {
      using (OleDbConnection cnWeatherDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sWeatherDB))
      {
        cnWeatherDB.Open();
        return SurfaceWeather.ReadSurfaceAllRecords(cnWeatherDB, ref sfcData);
      }
    }

    public static int ReadSurfaceAllRecordsHr(
      OleDbConnection conn,
      int hr,
      ref List<SurfaceWeather> sfcData)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT * FROM SurfaceWeather WHERE Hour(TimeStamp)=" + hr.ToString() + " ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
            sfcData.Add(new SurfaceWeather()
            {
              TimeStamp = (DateTime) oleDbDataReader["TimeStamp"],
              AirDens = (double) oleDbDataReader["AirDens"],
              AirMass = (double) oleDbDataReader["AirMass"],
              Ceiling = (double) oleDbDataReader["Ceiling"],
              DewTempC = (double) oleDbDataReader["DewTempC"],
              DewTempF = (double) oleDbDataReader["DewTempF"],
              DifRadWm2 = (double) oleDbDataReader["DifRadWm2"],
              DirRadWm2 = (double) oleDbDataReader["DirRadWm2"],
              GlbRadWm2 = (double) oleDbDataReader["GlbRadWm2"],
              NetRadWm2 = (double) oleDbDataReader["NetRadWm2"],
              OpCldCov = (double) oleDbDataReader["OpCldCov"],
              PrsIn = (double) oleDbDataReader["PrsIn"],
              PrsKpa = (double) oleDbDataReader["PrsKpa"],
              PrsMbar = (double) oleDbDataReader["PrsMbar"],
              PeGrMh = (double) oleDbDataReader["PeGrMh"],
              PeTrMh = (double) oleDbDataReader["PeTrMh"],
              PeSnGrMh = (double) oleDbDataReader["PeSnGrMh"],
              PeSnTrMh = (double) oleDbDataReader["PeSnTrMh"],
              PtTrMh = (double) oleDbDataReader["PtTrMh"],
              PARuEm2s = (double) oleDbDataReader["PARuEm2s"],
              PARWm2 = (double) oleDbDataReader["PARWm2"],
              RainInH = (double) oleDbDataReader["RainInH"],
              RainMh = (double) oleDbDataReader["RainMh"],
              RelHum = (double) oleDbDataReader["RelHum"],
              SatVPrsKpa = (double) oleDbDataReader["SatVPrsKpa"],
              SnowIn = (double) oleDbDataReader["SnowIn"],
              SnowM = (double) oleDbDataReader["SnowM"],
              SolZenAgl = (double) oleDbDataReader["SolZenAgl"],
              TempC = (double) oleDbDataReader["TempC"],
              TempF = (double) oleDbDataReader["TempF"],
              TempK = (double) oleDbDataReader["TempK"],
              ToCldCov = (double) oleDbDataReader["ToCldCov"],
              TrCldCov = (double) oleDbDataReader["TrCldCov"],
              VapPrsKpa = (double) oleDbDataReader["VapPrsKpa"],
              VegStMh = (double) oleDbDataReader["VegStMh"],
              WdDir = (double) oleDbDataReader["WdDir"],
              WdSpdKnt = (double) oleDbDataReader["WdSpdKnt"],
              WdSpdMh = (double) oleDbDataReader["WdSpdMh"],
              WdSpdMs = (double) oleDbDataReader["WdSpdMs"]
            });
        }
        return sfcData.Count;
      }
    }

    public static int ReadSurfaceDailyRecords(
      OleDbConnection conn,
      DateTime dt0hr,
      ref List<SurfaceWeather> sfcData)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        DateTime dateTime = dt0hr.AddHours(23.0);
        oleDbCommand.CommandText = "SELECT * FROM SurfaceWeather Where TimeStamp>=#" + dt0hr.ToString() + "# And TimeStamp<=#" + dateTime.ToString() + "# ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
            sfcData.Add(new SurfaceWeather()
            {
              TimeStamp = (DateTime) oleDbDataReader["TimeStamp"],
              AirDens = (double) oleDbDataReader["AirDens"],
              AirMass = (double) oleDbDataReader["AirMass"],
              Ceiling = (double) oleDbDataReader["Ceiling"],
              DewTempC = (double) oleDbDataReader["DewTempC"],
              DewTempF = (double) oleDbDataReader["DewTempF"],
              DifRadWm2 = (double) oleDbDataReader["DifRadWm2"],
              DirRadWm2 = (double) oleDbDataReader["DirRadWm2"],
              GlbRadWm2 = (double) oleDbDataReader["GlbRadWm2"],
              NetRadWm2 = (double) oleDbDataReader["NetRadWm2"],
              OpCldCov = (double) oleDbDataReader["OpCldCov"],
              PrsIn = (double) oleDbDataReader["PrsIn"],
              PrsKpa = (double) oleDbDataReader["PrsKpa"],
              PrsMbar = (double) oleDbDataReader["PrsMbar"],
              PeGrMh = (double) oleDbDataReader["PeGrMh"],
              PeTrMh = (double) oleDbDataReader["PeTrMh"],
              PeSnGrMh = (double) oleDbDataReader["PeSnGrMh"],
              PeSnTrMh = (double) oleDbDataReader["PeSnTrMh"],
              PtTrMh = (double) oleDbDataReader["PtTrMh"],
              PARuEm2s = (double) oleDbDataReader["PARuEm2s"],
              PARWm2 = (double) oleDbDataReader["PARWm2"],
              RainInH = (double) oleDbDataReader["RainInH"],
              RainMh = (double) oleDbDataReader["RainMh"],
              RelHum = (double) oleDbDataReader["RelHum"],
              SatVPrsKpa = (double) oleDbDataReader["SatVPrsKpa"],
              SnowIn = (double) oleDbDataReader["SnowIn"],
              SnowM = (double) oleDbDataReader["SnowM"],
              SolZenAgl = (double) oleDbDataReader["SolZenAgl"],
              TempC = (double) oleDbDataReader["TempC"],
              TempF = (double) oleDbDataReader["TempF"],
              TempK = (double) oleDbDataReader["TempK"],
              ToCldCov = (double) oleDbDataReader["ToCldCov"],
              TrCldCov = (double) oleDbDataReader["TrCldCov"],
              VapPrsKpa = (double) oleDbDataReader["VapPrsKpa"],
              WdDir = (double) oleDbDataReader["WdDir"],
              WdSpdKnt = (double) oleDbDataReader["WdSpdKnt"],
              WdSpdMh = (double) oleDbDataReader["WdSpdMh"],
              WdSpdMs = (double) oleDbDataReader["WdSpdMs"]
            });
        }
      }
      return sfcData.Count;
    }

    [Obsolete]
    public static int ReadSurfaceDailyRecords(
      string sDB,
      DateTime dt0hr,
      ref List<SurfaceWeather> sfcData)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        return SurfaceWeather.ReadSurfaceDailyRecords(conn, dt0hr, ref sfcData);
      }
    }

    public static void ReadSurfaceAmPmTemp(
      OleDbConnection conn,
      DateTime dt0hr,
      out double dTempAm,
      out double dTempPm)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        DateTime dateTime1 = dt0hr.AddHours(2.0);
        DateTime dateTime2 = dt0hr.AddHours(6.0);
        oleDbCommand.CommandText = "SELECT * FROM SurfaceWeather Where TimeStamp>=#" + dateTime1.ToString() + "# And TimeStamp<=#" + dateTime2.ToString() + "# ORDER BY TempK;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          dTempAm = (double) oleDbDataReader["TempK"];
        }
        DateTime dateTime3 = dt0hr.AddHours(12.0);
        DateTime dateTime4 = dt0hr.AddHours(16.0);
        oleDbCommand.CommandText = "SELECT * FROM SurfaceWeather Where TimeStamp>=#" + dateTime3.ToString() + "# And TimeStamp<=#" + dateTime4.ToString() + "# ORDER BY TempK DESC;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          dTempPm = (double) oleDbDataReader["TempK"];
        }
      }
    }

    [Obsolete]
    public static void ReadSurfaceAmPmTemp(
      string sDB,
      DateTime dt0hr,
      out double dTempAm,
      out double dTempPm)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        SurfaceWeather.ReadSurfaceAmPmTemp(conn, dt0hr, out dTempAm, out dTempPm);
      }
    }

    public static int ReadDateTime(OleDbConnection conn, ref List<DateTime> dt)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT *  FROM SurfaceWeather ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
            dt.Add((DateTime) oleDbDataReader["TimeStamp"]);
        }
      }
      return dt.Count;
    }

    [Obsolete]
    public static int ReadDateTime(string sDB, ref List<DateTime> dt)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        return SurfaceWeather.ReadDateTime(conn, ref dt);
      }
    }

    public enum WeatherDataFormat
    {
      OLD,
      NEWINTL,
      NEWUSCAN,
      NARCCAP,
    }
  }
}
