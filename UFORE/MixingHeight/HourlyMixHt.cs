// Decompiled with JetBrains decompiler
// Type: UFORE.MixingHeight.HourlyMixHt
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using UFORE.Location;
using UFORE.Weather;

namespace UFORE.MixingHeight
{
  public class HourlyMixHt
  {
    private const string HRMIX_TABLE = "HourlyMixHt";
    private const int WEAK = 0;
    private const int SLIGHT = 1;
    private const int MODERATE = 2;
    private const int STRONG = 3;
    private const int OVERCAST = 4;
    private const int NIGHTOVERCAST = 5;
    private const int NIGHTCLEAR = 6;
    private DateTime TimeStamp;
    private double FlowVec;
    private double WdSpdMs;
    private double TempK;
    private int Stability;
    public double RuralMixHt;
    public double UrbanMixHt;

    public static void CalcHourlyMixHt(
      OleDbConnection connWeather,
      LocationData locData,
      DateTime dtStart,
      DateTime dtEnd)
    {
      List<HourlyMixHt> mhList = new List<HourlyMixHt>();
      bool[] calm = new bool[24];
      double[] amMixHt = new double[3];
      double[] pmMixHt = new double[3];
      int[,] stab = new int[13, 7];
      int lastStab = 0;
      for (int index = 0; index < 24; ++index)
      {
        HourlyMixHt hourlyMixHt = new HourlyMixHt();
        mhList.Add(hourlyMixHt);
      }
      HourlyMixHt.CreateStabTable(ref stab);
      DateTime dateTime = dtStart;
      HourlyMixHt.CreateHourlyMixHtTable(connWeather);
      for (; dateTime <= dtEnd; dateTime = dateTime.AddDays(1.0))
      {
        HourlyMixHt.SetDate(ref mhList, dateTime);
        int sunrise;
        int sunset;
        double[] solAng;
        HourlyMixHt.Sun(mhList[0].TimeStamp, locData.Latitude, locData.Longitude, locData.GMTOffset, out sunrise, out sunset, out solAng);
        List<SurfaceWeather> surfaceWeatherList = new List<SurfaceWeather>();
        if (SurfaceWeather.ReadSurfaceDailyRecords(connWeather, mhList[0].TimeStamp, ref surfaceWeatherList) == 24)
        {
          HourlyMixHt.SetWeatherData(ref mhList, ref surfaceWeatherList);
          HourlyMixHt.CheckCalm(ref surfaceWeatherList, ref calm);
          HourlyMixHt.DetermineStability(ref surfaceWeatherList, stab, sunrise, sunset, solAng, ref mhList, lastStab);
          lastStab = mhList[23].Stability;
          TwiceDailyMixHt.Read3DaysMixHt(connWeather, dateTime, ref amMixHt, ref pmMixHt);
          if (sunrise == 0 && (sunset == 0 || sunset == 23))
            HourlyMixHt.InterpolateHourlyMixHt(amMixHt, pmMixHt, ref mhList);
          else
            HourlyMixHt.InterpolateHourlyMixHt(amMixHt, pmMixHt, sunrise, sunset, ref mhList);
          HourlyMixHt.WriteHourlyMixHt(connWeather, ref mhList, calm);
        }
      }
    }

    [Obsolete]
    public static void CalcHourlyMixHt(
      string sWeatherDB,
      LocationData locData,
      DateTime dtStart,
      DateTime dtEnd)
    {
      using (OleDbConnection connWeather = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sWeatherDB))
      {
        connWeather.Open();
        HourlyMixHt.CalcHourlyMixHt(connWeather, locData, dtStart, dtEnd);
      }
    }

    private static void SetDate(ref List<HourlyMixHt> mhList, DateTime d)
    {
      for (int index = 0; index < 24; ++index)
        mhList[index].TimeStamp = d.AddHours((double) index);
    }

    private static void Sun(
      DateTime dt,
      double lat,
      double lon,
      double timeZone,
      out int sunrise,
      out int sunset,
      out double[] solAng)
    {
      int num1 = 0;
      DateTime dateTime = DateTime.Parse("12/30/1899 0:00:00");
      sunrise = sunset = 0;
      solAng = new double[24];
      for (int index1 = 0; index1 < 24; ++index1)
      {
        ++num1;
        double num2 = ((double) (dt - dateTime).Days + 2415018.5 + (double) index1 / 24.0 - timeZone / 24.0 - 2451545.0) / 36525.0;
        double num3 = 280.46646 + num2 * (36000.76983 + num2 * 0.0003032);
        Math.Truncate(num3 / 360.0);
        double angle1 = num3 % 360.0;
        double angle2 = 357.52911 + num2 * (35999.05029 - 0.0001537 * num2);
        double x1 = 0.016708634 - num2 * (4.2037E-05 + 1.267E-07 * num2);
        double num4 = Math.Sin(HourlyMixHt.Deg2Rad(angle2)) * (1.914602 - num2 * (0.004817 + 1.4E-05 * num2)) + Math.Sin(HourlyMixHt.Deg2Rad(2.0 * angle2)) * (0.019993 - 0.000101 * num2) + Math.Sin(HourlyMixHt.Deg2Rad(3.0 * angle2)) * 0.000289;
        double num5 = angle1 + num4;
        double angle3 = angle2 + num4;
        double num6 = 1.000001018 * (1.0 - Math.Pow(x1, 2.0)) / (1.0 + x1 * Math.Cos(HourlyMixHt.Deg2Rad(angle3)));
        double angle4 = num5 - 0.00569 - 0.00478 * Math.Sin(HourlyMixHt.Deg2Rad(125.04 - 1934.136 * num2));
        double angle5 = 23.0 + (26.0 + (21.448 - num2 * (46.815 + num2 * (0.00059 - num2 * 0.001813))) / 60.0) / 60.0 + 0.00256 * Math.Cos(HourlyMixHt.Deg2Rad(125.04 - 1934.136 * num2));
        double angle6 = HourlyMixHt.Rad2Deg(Math.Asin(Math.Sin(HourlyMixHt.Deg2Rad(angle5)) * Math.Sin(HourlyMixHt.Deg2Rad(angle4))));
        double x2 = Math.Tan(HourlyMixHt.Deg2Rad(angle5 / 2.0)) * Math.Tan(HourlyMixHt.Deg2Rad(angle5 / 2.0));
        double num7 = 4.0 * HourlyMixHt.Rad2Deg(x2 * Math.Sin(2.0 * HourlyMixHt.Deg2Rad(angle1)) - 2.0 * x1 * Math.Sin(HourlyMixHt.Deg2Rad(angle2)) + 4.0 * x1 * x2 * Math.Sin(HourlyMixHt.Deg2Rad(angle2)) * Math.Cos(2.0 * HourlyMixHt.Deg2Rad(angle1)) - 0.5 * Math.Pow(x2, 2.0) * Math.Sin(4.0 * HourlyMixHt.Deg2Rad(angle1)) - 1.25 * Math.Pow(x1, 2.0) * Math.Sin(2.0 * HourlyMixHt.Deg2Rad(angle2)));
        double num8 = HourlyMixHt.Rad2Deg(Math.Acos(Math.Cos(HourlyMixHt.Deg2Rad(90.833)) / (Math.Cos(HourlyMixHt.Deg2Rad(lat)) * Math.Cos(HourlyMixHt.Deg2Rad(angle6))) - Math.Tan(HourlyMixHt.Deg2Rad(lat)) * Math.Tan(HourlyMixHt.Deg2Rad(angle6))));
        double num9 = (720.0 - 4.0 * lon - num7 + timeZone * 60.0) / 1440.0;
        double num10 = num9 - num8 * 4.0 / 1440.0;
        double num11 = num9 + num8 * 4.0 / 1440.0;
        if (index1 == 12)
        {
          for (int index2 = 0; index2 < 24; ++index2)
          {
            if (num10 <= 1.0 / 24.0 * (double) index2)
            {
              double num12 = 1.0 / 24.0 * (double) index2 - num10;
              double num13 = num10 - 1.0 / 24.0 * (double) (index2 - 1);
              sunrise = num12 < num13 ? index2 : index2 - 1;
              break;
            }
          }
          for (int index3 = 0; index3 < 24; ++index3)
          {
            if (num11 <= 1.0 / 24.0 * (double) index3)
            {
              double num14 = 1.0 / 24.0 * (double) index3 - num11;
              double num15 = num11 - 1.0 / 24.0 * (double) (index3 - 1);
              sunset = num14 < num15 ? index3 : index3 - 1;
              break;
            }
          }
        }
        double num16 = ((double) index1 / 24.0 * 1440.0 + num7 + 4.0 * lon - 60.0 * timeZone) % 1440.0;
        double angle7 = num16 / 4.0 >= 0.0 ? num16 / 4.0 - 180.0 : num16 / 4.0 + 180.0;
        double num17 = HourlyMixHt.Rad2Deg(Math.Acos(Math.Sin(HourlyMixHt.Deg2Rad(lat)) * Math.Sin(HourlyMixHt.Deg2Rad(angle6)) + Math.Cos(HourlyMixHt.Deg2Rad(lat)) * Math.Cos(HourlyMixHt.Deg2Rad(angle6)) * Math.Cos(HourlyMixHt.Deg2Rad(angle7))));
        solAng[index1] = 90.0 - num17;
        if (solAng[index1] < 0.0)
          solAng[index1] = 0.0;
      }
    }

    private static double Deg2Rad(double angle) => Math.PI * angle / 180.0;

    private static double Rad2Deg(double angle) => 180.0 * angle / Math.PI;

    private static DateTime Serial2Time(DateTime dt, double serialVal)
    {
      int num1 = (int) (serialVal * 24.0);
      serialVal = serialVal * 24.0 - (double) num1;
      int num2 = (int) (serialVal * 60.0);
      serialVal = serialVal * 60.0 - (double) num2;
      int num3 = (int) (serialVal * 60.0);
      DateTime dateTime = dt.AddHours((double) num1);
      dateTime = dateTime.AddMinutes((double) num2);
      dateTime = dateTime.AddSeconds((double) num3);
      return dateTime;
    }

    private static void SetWeatherData(
      ref List<HourlyMixHt> mhList,
      ref List<SurfaceWeather> surface)
    {
      for (int index = 0; index < 24; ++index)
      {
        mhList[index].WdSpdMs = surface[index].WdSpdMs;
        mhList[index].TempK = surface[index].TempK;
      }
    }

    private static void CheckCalm(ref List<SurfaceWeather> sfcList, ref bool[] calm)
    {
      for (int index = 0; index < 24; ++index)
        calm[index] = sfcList[index].WdSpdMs == 0.0;
    }

    private static void DetermineStability(
      ref List<SurfaceWeather> sfcList,
      int[,] stab,
      int sunrise,
      int sunset,
      double[] solAng,
      ref List<HourlyMixHt> mhList,
      int lastStab)
    {
      for (int index1 = 0; index1 < 24; ++index1)
      {
        int num = index1 + 1;
        int index2 = (int) sfcList[index1].WdSpdKnt;
        if (index2 < 1)
          index2 = 1;
        if (index2 > 12)
          index2 = 12;
        int index3;
        if (sfcList[index1].ToCldCov == 10.0 && sfcList[index1].Ceiling < 70.0)
          index3 = 4;
        else if (num <= sunrise || sunset <= num)
        {
          index3 = 5.0 > sfcList[index1].ToCldCov ? 6 : 5;
        }
        else
        {
          index3 = 60.0 >= solAng[index1] ? (35.0 >= solAng[index1] ? (15.0 >= solAng[index1] ? 0 : 1) : 2) : 3;
          if (sfcList[index1].ToCldCov == 10.0)
          {
            if (sfcList[index1].Ceiling <= 160.0)
              index3 -= 2;
            else
              --index3;
          }
          else if (5.0 < sfcList[index1].ToCldCov)
          {
            if (sfcList[index1].Ceiling < 70.0)
              index3 -= 2;
            else if (sfcList[index1].Ceiling <= 160.0)
              --index3;
            else
              --index3;
          }
          if (index3 < 0)
            index3 = 0;
        }
        mhList[index1].Stability = stab[index2, index3];
        if (lastStab == 0)
          lastStab = mhList[index1].Stability;
        else if (mhList[index1].Stability - lastStab > 1)
          mhList[index1].Stability = lastStab + 1;
        else if (lastStab - mhList[index1].Stability > 1)
          mhList[index1].Stability = lastStab - 1;
        lastStab = mhList[index1].Stability;
      }
    }

    private static void CreateStabTable(ref int[,] stab)
    {
      stab[0, 0] = 3;
      stab[1, 3] = 1;
      stab[2, 3] = 1;
      stab[3, 3] = 1;
      stab[4, 3] = 1;
      stab[5, 3] = 1;
      stab[6, 3] = 2;
      stab[7, 3] = 2;
      stab[8, 3] = 2;
      stab[9, 3] = 2;
      stab[10, 3] = 3;
      stab[11, 3] = 3;
      stab[12, 3] = 3;
      stab[1, 2] = 1;
      stab[2, 2] = 2;
      stab[3, 2] = 2;
      stab[4, 2] = 2;
      stab[5, 2] = 2;
      stab[6, 2] = 2;
      stab[7, 2] = 2;
      stab[8, 2] = 3;
      stab[9, 2] = 3;
      stab[10, 2] = 3;
      stab[11, 2] = 3;
      stab[12, 2] = 4;
      stab[1, 1] = 2;
      stab[2, 1] = 2;
      stab[3, 1] = 2;
      stab[4, 1] = 3;
      stab[5, 1] = 3;
      stab[6, 1] = 3;
      stab[7, 1] = 3;
      stab[8, 1] = 3;
      stab[9, 1] = 3;
      stab[10, 1] = 4;
      stab[11, 1] = 4;
      stab[12, 1] = 4;
      stab[1, 0] = 3;
      stab[2, 0] = 3;
      stab[3, 0] = 3;
      stab[4, 0] = 4;
      stab[5, 0] = 4;
      stab[6, 0] = 4;
      stab[7, 0] = 4;
      stab[8, 0] = 4;
      stab[9, 0] = 4;
      stab[10, 0] = 4;
      stab[11, 0] = 4;
      stab[12, 0] = 4;
      stab[1, 4] = 4;
      stab[2, 4] = 4;
      stab[3, 4] = 4;
      stab[4, 4] = 4;
      stab[5, 4] = 4;
      stab[6, 4] = 4;
      stab[7, 4] = 4;
      stab[8, 4] = 4;
      stab[9, 4] = 4;
      stab[10, 4] = 4;
      stab[11, 4] = 4;
      stab[12, 4] = 4;
      stab[1, 5] = 6;
      stab[2, 5] = 6;
      stab[3, 5] = 6;
      stab[4, 5] = 5;
      stab[5, 5] = 5;
      stab[6, 5] = 5;
      stab[7, 5] = 4;
      stab[8, 5] = 4;
      stab[9, 5] = 4;
      stab[10, 5] = 4;
      stab[11, 5] = 4;
      stab[12, 5] = 4;
      stab[1, 6] = 7;
      stab[2, 6] = 7;
      stab[3, 6] = 7;
      stab[4, 6] = 6;
      stab[5, 6] = 6;
      stab[6, 6] = 6;
      stab[7, 6] = 5;
      stab[8, 6] = 5;
      stab[9, 6] = 5;
      stab[10, 6] = 5;
      stab[11, 6] = 4;
      stab[12, 6] = 4;
    }

    private static void InterpolateHourlyMixHt(
      double[] amMixHt,
      double[] pmMixHt,
      ref List<HourlyMixHt> mhList)
    {
      int num1 = 24;
      int num2 = 0;
      for (int index = 0; index < num1; ++index)
      {
        int num3 = index + 1;
        if (mhList[index].Stability == 4)
        {
          mhList[index].UrbanMixHt = pmMixHt[0] + (pmMixHt[1] - pmMixHt[0]) * ((double) (24 - num2 + num3) / (double) (24 - num2 + 14));
          mhList[index].RuralMixHt = pmMixHt[0] + (pmMixHt[1] - pmMixHt[0]) * ((double) (24 - num2 + num3) / (double) (24 - num2 + 14));
        }
        else
        {
          mhList[index].UrbanMixHt = amMixHt[1];
          mhList[index].RuralMixHt = pmMixHt[0] + (pmMixHt[1] - pmMixHt[0]) * ((double) (24 - num2 + num3) / (double) (24 - num2 + 14));
        }
      }
    }

    private static void InterpolateHourlyMixHt(
      double[] amMixHt,
      double[] pmMixHt,
      int sunrise,
      int sunset,
      ref List<HourlyMixHt> mhList)
    {
      for (int index = 0; index < sunrise; ++index)
      {
        int num = index + 1;
        if (mhList[index].Stability == 4)
        {
          mhList[index].UrbanMixHt = pmMixHt[0] + (pmMixHt[1] - pmMixHt[0]) * ((double) (24 - sunset + num) / (double) (24 - sunset + 14));
          mhList[index].RuralMixHt = pmMixHt[0] + (pmMixHt[1] - pmMixHt[0]) * ((double) (24 - sunset + num) / (double) (24 - sunset + 14));
        }
        else
        {
          mhList[index].UrbanMixHt = amMixHt[1];
          mhList[index].RuralMixHt = pmMixHt[0] + (pmMixHt[1] - pmMixHt[0]) * ((double) (24 - sunset + num) / (double) (24 - sunset + 14));
        }
      }
      for (int index = sunrise; index < 13; ++index)
      {
        int num = index + 1;
        if (mhList[sunrise - 1].Stability == 4)
        {
          mhList[index].UrbanMixHt = pmMixHt[0] + (pmMixHt[1] - pmMixHt[0]) * ((double) (24 - sunset + num) / (double) (24 - sunset + 14));
          mhList[index].RuralMixHt = pmMixHt[0] + (pmMixHt[1] - pmMixHt[0]) * ((double) (24 - sunset + num) / (double) (24 - sunset + 14));
        }
        else
        {
          mhList[index].UrbanMixHt = amMixHt[1] + (pmMixHt[1] - amMixHt[1]) * ((double) (num - sunrise) / (double) (14 - sunrise));
          mhList[index].RuralMixHt = pmMixHt[1] * ((double) (num - sunrise) / (double) (14 - sunrise));
        }
      }
      for (int index = 13; index < sunset; ++index)
      {
        int num = index + 1;
        mhList[index].UrbanMixHt = pmMixHt[1];
        mhList[index].RuralMixHt = pmMixHt[1];
      }
      for (int index = sunset; index < 24; ++index)
      {
        int num = index + 1;
        if (mhList[index].Stability == 4)
        {
          mhList[index].UrbanMixHt = pmMixHt[1] + (pmMixHt[2] - pmMixHt[1]) * ((double) (num - sunset) / (double) (38 - sunset));
          mhList[index].RuralMixHt = pmMixHt[1] + (pmMixHt[2] - pmMixHt[1]) * ((double) (num - sunset) / (double) (38 - sunset));
        }
        else
        {
          mhList[index].UrbanMixHt = pmMixHt[1] + (amMixHt[2] - pmMixHt[1]) * ((double) (num - sunset) / (double) (24 - sunset));
          mhList[index].RuralMixHt = pmMixHt[1] + (pmMixHt[2] - pmMixHt[1]) * ((double) (num - sunset) / (double) (38 - sunset));
        }
      }
    }

    public static void CreateHourlyMixHtTable(OleDbConnection conn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "CREATE TABLE HourlyMixHt([TimeStamp] DateTime,[FlowVec] DOUBLE,[WdSpdMs] DOUBLE,[TempK] DOUBLE,[Stability] INT,[RuralMixHt] DOUBLE,[UrbanMixHt] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void CreateHourlyMixHtTable(string sDB)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        HourlyMixHt.CreateHourlyMixHtTable(conn);
      }
    }

    public static void WriteHourlyMixHt(
      OleDbConnection conn,
      ref List<HourlyMixHt> mhList,
      bool[] calm)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        for (int index = 0; index < 24; ++index)
        {
          if (calm[index])
            mhList[index].WdSpdMs = 0.0;
          oleDbCommand.CommandText = "INSERT INTO HourlyMixHt([TimeStamp],[FlowVec],[WdSpdMs],[TempK],[Stability],[RuralMixHt],[UrbanMixHt]) Values (#" + mhList[index].TimeStamp.ToString() + "#," + mhList[index].FlowVec.ToString() + "," + mhList[index].WdSpdMs.ToString() + "," + mhList[index].TempK.ToString() + "," + mhList[index].Stability.ToString() + "," + mhList[index].RuralMixHt.ToString() + "," + mhList[index].UrbanMixHt.ToString() + ");";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    public static void WriteHourlyMixHt(string sDB, ref List<HourlyMixHt> mhList, bool[] calm)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        HourlyMixHt.WriteHourlyMixHt(conn, ref mhList, calm);
      }
    }

    [Obsolete]
    public static int ReadMixHtAllRecords(string sWeatherDB, ref List<HourlyMixHt> mhList)
    {
      using (OleDbConnection cnWeatherDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sWeatherDB))
      {
        cnWeatherDB.Open();
        return HourlyMixHt.ReadMixHtAllRecords(cnWeatherDB, ref mhList);
      }
    }

    public static int ReadMixHtAllRecords(OleDbConnection cnWeatherDB, ref List<HourlyMixHt> mhList)
    {
      bool flag = false;
      DataTable oleDbSchemaTable = cnWeatherDB.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[4]
      {
        null,
        null,
        null,
        (object) "TABLE"
      });
      for (int index = 0; index < oleDbSchemaTable.Rows.Count; ++index)
      {
        if (oleDbSchemaTable.Rows[index].ItemArray[2].ToString() == nameof (HourlyMixHt))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return 0;
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnWeatherDB;
        oleDbCommand.CommandText = "SELECT * FROM HourlyMixHt ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            HourlyMixHt hourlyMixHt = new HourlyMixHt()
            {
              TimeStamp = (DateTime) oleDbDataReader["TimeStamp"],
              FlowVec = (double) oleDbDataReader["FlowVec"],
              Stability = (int) oleDbDataReader["Stability"],
              RuralMixHt = (double) oleDbDataReader["RuralMixHt"],
              UrbanMixHt = (double) oleDbDataReader["UrbanMixHt"],
              TempK = (double) oleDbDataReader["TempK"],
              WdSpdMs = (double) oleDbDataReader["WdSpdMs"]
            };
            mhList.Add(hourlyMixHt);
          }
          oleDbDataReader.Close();
        }
      }
      return mhList.Count;
    }

    public static void SetNullMixHtData(int recCnt, ref List<HourlyMixHt> mhList)
    {
      HourlyMixHt hourlyMixHt = new HourlyMixHt();
      for (int index = 0; index < recCnt; ++index)
      {
        hourlyMixHt.UrbanMixHt = -999.0;
        hourlyMixHt.RuralMixHt = -999.0;
        mhList.Add(hourlyMixHt);
      }
    }
  }
}
