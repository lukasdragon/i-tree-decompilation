// Decompiled with JetBrains decompiler
// Type: UFORE.MixingHeight.MixHt
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Deposition;
using UFORE.Location;
using UFORE.Weather;

namespace UFORE.MixingHeight
{
  public class MixHt
  {
    [Obsolete]
    public static void CalcMixHts(string sWeatherDB, LocationData locData)
    {
      using (OleDbConnection cnWeatherDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sWeatherDB))
      {
        cnWeatherDB.Open();
        MixHt.CalcMixHts(cnWeatherDB, locData, (UFORE_D) null, 0, 0);
      }
    }

    [Obsolete]
    public static void CalcMixHts(
      string sWeatherDB,
      LocationData locData,
      UFORE_D uforeDObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      using (OleDbConnection cnWeatherDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sWeatherDB))
      {
        cnWeatherDB.Open();
        MixHt.CalcMixHts(cnWeatherDB, locData, uforeDObj, PercentRangeFrom, PercentRangeTo);
      }
    }

    public static void CalcMixHts(
      OleDbConnection cnWeatherDB,
      LocationData locData,
      UFORE_D uforeDObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      List<DateTime> dt = new List<DateTime>();
      DateTime startDt = new DateTime();
      DateTime endDt = new DateTime();
      int cnt = 0;
      List<Action> actionList = new List<Action>()
      {
        (Action) (() => cnt = SurfaceWeather.ReadDateTime(cnWeatherDB, ref dt)),
        (Action) (() => MixHt.SearchStartEndDateTime(dt, cnt, out startDt, out endDt)),
        (Action) (() => TwiceDailyMixHt.CalcTwiceDailyMixHt(cnWeatherDB, startDt, endDt)),
        (Action) (() => HourlyMixHt.CalcHourlyMixHt(cnWeatherDB, locData, startDt, endDt))
      };
      double num1 = (double) (PercentRangeTo - PercentRangeFrom) / (double) actionList.Count;
      int num2 = 0;
      while (num2 < actionList.Count)
      {
        actionList[num2++]();
        uforeDObj?.reportProgress((int) ((double) PercentRangeFrom + (double) num2 * num1));
      }
    }

    private static void SearchStartEndDateTime(
      List<DateTime> dt,
      int recCnt,
      out DateTime startDt,
      out DateTime endDt)
    {
      startDt = endDt = DateTime.Parse("1/1/2000");
      for (int index = 0; index < recCnt; ++index)
      {
        if (dt[index].Month == 1)
        {
          startDt = dt[index];
          break;
        }
      }
      for (int index = recCnt - 1; index >= 0; --index)
      {
        if (dt[index].Month == 12)
        {
          endDt = dt[index];
          break;
        }
      }
    }
  }
}
