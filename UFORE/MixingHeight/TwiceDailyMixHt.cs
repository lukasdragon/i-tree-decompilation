// Decompiled with JetBrains decompiler
// Type: UFORE.MixingHeight.TwiceDailyMixHt
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Weather;

namespace UFORE.MixingHeight
{
  public class TwiceDailyMixHt
  {
    private const int MIXHT_NODATA = 99999;
    public const double C = 57.29578;
    public const int PRE = 0;
    public const int CUR = 1;
    public const int NXT = 2;
    public const string TDMIX_TABLE = "TwiceDailyMixHt";
    private DateTime TimeStamp;
    private double MixHtAm;
    private double MixHtPm;

    public static void CalcTwiceDailyMixHt(
      OleDbConnection connWeather,
      DateTime dtStart,
      DateTime dtEnd)
    {
      int num = 0;
      int numDays = (dtEnd - dtStart).Days + 1;
      TwiceDailyMixHt[] tdArr = new TwiceDailyMixHt[numDays];
      for (DateTime dt0hr = dtStart; dt0hr <= dtEnd; dt0hr = dt0hr.AddDays(1.0))
      {
        List<UpperAir> uprList = new List<UpperAir>();
        int recCnt = UpperAir.ReadUpperAirRecords(connWeather, dt0hr, ref uprList);
        double mixht1;
        double mixht2;
        if (recCnt > 0)
        {
          double[] numArray = new double[recCnt];
          double dTempAm;
          double dTempPm;
          SurfaceWeather.ReadSurfaceAmPmTemp(connWeather, dt0hr, out dTempAm, out dTempPm);
          double sfcPtAm;
          double sfcPtPm;
          TwiceDailyMixHt.CalcPotentialTemp(uprList, numArray, dTempAm, out sfcPtAm, dTempPm, out sfcPtPm);
          if (sfcPtAm < numArray[0])
            mixht1 = 99999.0;
          else
            TwiceDailyMixHt.CalcMixingHt(recCnt, uprList, numArray, sfcPtAm, out mixht1);
          if (sfcPtPm < numArray[0])
            mixht2 = 99999.0;
          else
            TwiceDailyMixHt.CalcMixingHt(recCnt, uprList, numArray, sfcPtPm, out mixht2);
        }
        else
        {
          mixht1 = 99999.0;
          mixht2 = 99999.0;
        }
        tdArr[num++] = new TwiceDailyMixHt()
        {
          TimeStamp = dt0hr,
          MixHtAm = mixht1,
          MixHtPm = mixht2
        };
      }
      TwiceDailyMixHt.FillMissingMixHt(tdArr, numDays);
      TwiceDailyMixHt.WriteTwiceDailyMixHt(connWeather, tdArr);
    }

    [Obsolete]
    public static void CalcTwiceDailyMixHt(string sWeatherDB, DateTime dtStart, DateTime dtEnd)
    {
      using (OleDbConnection connWeather = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sWeatherDB))
      {
        connWeather.Open();
        TwiceDailyMixHt.CalcTwiceDailyMixHt(connWeather, dtStart, dtEnd);
      }
    }

    private static void CalcPotentialTemp(
      List<UpperAir> uprList,
      double[] pTemp,
      double sfcTempAm,
      out double sfcPtAm,
      double sfcTempPm,
      out double sfcPtPm)
    {
      for (int index = 0; index < uprList.Count; ++index)
        pTemp[index] = uprList[index].TempK * Math.Pow(1000.0 / uprList[index].PressMBar, 0.286);
      sfcPtAm = (sfcTempAm + 5.0) * Math.Pow(1000.0 / uprList[0].PressMBar, 0.286);
      sfcPtPm = sfcTempPm * Math.Pow(1000.0 / uprList[0].PressMBar, 0.286);
    }

    private static void CalcMixingHt(
      int recCnt,
      List<UpperAir> uprList,
      double[] potTemp,
      double sfcTemp,
      out double mixht)
    {
      try
      {
        mixht = 0.0;
        for (int index1 = 0; index1 < recCnt; ++index1)
        {
          if (potTemp[index1] > sfcTemp)
          {
            int index2 = index1 - 1;
            int index3 = index1;
            if (uprList[index2].HeightM == 99999.0 || uprList[index3].HeightM == 99999.0)
            {
              double num1 = potTemp[index3] - potTemp[index2];
              double num2 = sfcTemp - potTemp[index2];
              double num3 = uprList[index3].PressMBar - uprList[index2].PressMBar;
              double num4 = uprList[index2].PressMBar + num2 / num1 * num3;
              for (int index4 = index1 - 1; index4 >= 0; --index4)
              {
                if (uprList[index4].HeightM != 99999.0)
                {
                  index2 = index4;
                  break;
                }
                mixht = -9999.0;
              }
              for (int index5 = 0; index5 < recCnt; ++index5)
              {
                if (uprList[index5].HeightM != 99999.0)
                {
                  index3 = index5;
                  break;
                }
                mixht = 99999.0;
              }
              if (mixht == 99999.0)
                break;
              double num5 = uprList[index3].PressMBar - uprList[index2].PressMBar;
              double num6 = num4 - uprList[index2].PressMBar;
              double num7 = num6 / num5;
              double num8 = uprList[index3].HeightM - uprList[index2].HeightM;
              mixht = uprList[index2].HeightM + num6 / num5 * num8;
              break;
            }
            double num9 = potTemp[index3] - potTemp[index2];
            double num10 = sfcTemp - potTemp[index2];
            double num11 = uprList[index3].HeightM - uprList[index2].HeightM;
            mixht = uprList[index2].HeightM + num10 / num9 * num11;
            break;
          }
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private static void FillMissingMixHt(TwiceDailyMixHt[] tdArr, int numDays)
    {
      double[] mixht = new double[numDays * 2];
      int num1 = 0;
      for (int index1 = 0; index1 < numDays; ++index1)
      {
        double[] numArray1 = mixht;
        int index2 = num1;
        int num2 = index2 + 1;
        double mixHtAm = tdArr[index1].MixHtAm;
        numArray1[index2] = mixHtAm;
        double[] numArray2 = mixht;
        int index3 = num2;
        num1 = index3 + 1;
        double mixHtPm = tdArr[index1].MixHtPm;
        numArray2[index3] = mixHtPm;
      }
      TwiceDailyMixHt.ExtrapolateHead(mixht);
      TwiceDailyMixHt.ExtrapolateTail(mixht);
      TwiceDailyMixHt.Interpolate(mixht);
      int num3 = 0;
      for (int index4 = 0; index4 < numDays; ++index4)
      {
        TwiceDailyMixHt twiceDailyMixHt1 = tdArr[index4];
        double[] numArray3 = mixht;
        int index5 = num3;
        int num4 = index5 + 1;
        double num5 = numArray3[index5];
        twiceDailyMixHt1.MixHtAm = num5;
        TwiceDailyMixHt twiceDailyMixHt2 = tdArr[index4];
        double[] numArray4 = mixht;
        int index6 = num4;
        num3 = index6 + 1;
        double num6 = numArray4[index6];
        twiceDailyMixHt2.MixHtPm = num6;
      }
    }

    private static void ExtrapolateHead(double[] mixht)
    {
      double num = 0.0;
      if (mixht[0] != 99999.0)
        return;
      int index1;
      for (index1 = 1; index1 < mixht.Length; ++index1)
      {
        if (mixht[index1] != 99999.0)
        {
          num = mixht[index1];
          break;
        }
      }
      for (int index2 = index1 - 1; index2 >= 0; --index2)
        mixht[index2] = num;
    }

    private static void ExtrapolateTail(double[] mixht)
    {
      double num = 0.0;
      if (mixht[mixht.Length - 1] != 99999.0)
        return;
      int index1;
      for (index1 = mixht.Length - 2; index1 >= 0; --index1)
      {
        if (mixht[index1] != 99999.0)
        {
          num = mixht[index1];
          break;
        }
      }
      for (int index2 = index1 + 1; index2 < mixht.Length; ++index2)
        mixht[index2] = num;
    }

    private static void Interpolate(double[] mixht)
    {
      double num1 = 0.0;
      for (int index1 = 0; index1 < mixht.Length; ++index1)
      {
        if (mixht[index1] == 99999.0 && index1 != 0)
        {
          double num2 = mixht[index1 - 1];
          int index2;
          for (index2 = index1 + 1; index2 < mixht.Length; ++index2)
          {
            if (mixht[index2] != 99999.0)
            {
              num1 = mixht[index2];
              break;
            }
          }
          double num3 = (num1 - num2) / (double) (index2 - (index1 - 1));
          double num4 = num2;
          for (int index3 = index1; index3 < index2; ++index3)
          {
            num4 += num3;
            mixht[index3] = num4;
          }
          index1 = index2;
        }
      }
    }

    public static void WriteTwiceDailyMixHt(OleDbConnection conn, TwiceDailyMixHt[] tdArr)
    {
      TwiceDailyMixHt.CreateTwiceDailyMixHtTable(conn);
      TwiceDailyMixHt.WriteTwiceDailyMixHtData(conn, tdArr);
    }

    [Obsolete]
    public static void WriteTwiceDailyMixHt(string sDB, TwiceDailyMixHt[] tdArr)
    {
      TwiceDailyMixHt.CreateTwiceDailyMixHtTable(sDB);
      TwiceDailyMixHt.WriteTwiceDailyMixHtData(sDB, tdArr);
    }

    public static void CreateTwiceDailyMixHtTable(OleDbConnection conn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "CREATE TABLE TwiceDailyMixHt([TimeStamp] DateTime,[MixHtAM] DOUBLE,[MixHtPM] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void CreateTwiceDailyMixHtTable(string sDB)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        TwiceDailyMixHt.CreateTwiceDailyMixHtTable(conn);
      }
    }

    private static void WriteTwiceDailyMixHtData(OleDbConnection conn, TwiceDailyMixHt[] tdArr)
    {
      OleDbCommand oleDbCommand = new OleDbCommand();
      oleDbCommand.Connection = conn;
      DateTime dateTime1 = tdArr[0].TimeStamp.AddDays(-1.0);
      oleDbCommand.CommandText = "INSERT INTO TwiceDailyMixHt([TimeStamp],[MixHtAM],[MixHtPM]) Values (#" + dateTime1.ToString() + "#," + tdArr[0].MixHtAm.ToString() + "," + tdArr[0].MixHtPm.ToString() + ");";
      oleDbCommand.ExecuteNonQuery();
      int index;
      for (index = 0; index < tdArr.Length; ++index)
      {
        oleDbCommand.CommandText = "INSERT INTO TwiceDailyMixHt([TimeStamp],[MixHtAM],[MixHtPM]) Values (#" + tdArr[index].TimeStamp.ToString() + "#," + tdArr[index].MixHtAm.ToString() + "," + tdArr[index].MixHtPm.ToString() + ");";
        oleDbCommand.ExecuteNonQuery();
      }
      DateTime dateTime2 = tdArr[index - 1].TimeStamp.AddDays(1.0);
      oleDbCommand.CommandText = "INSERT INTO TwiceDailyMixHt([TimeStamp],[MixHtAM],[MixHtPM]) Values (#" + dateTime2.ToString() + "#," + tdArr[index - 1].MixHtAm.ToString() + "," + tdArr[index - 1].MixHtPm.ToString() + ");";
      oleDbCommand.ExecuteNonQuery();
    }

    [Obsolete]
    private static void WriteTwiceDailyMixHtData(string sDB, TwiceDailyMixHt[] tdArr)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        TwiceDailyMixHt.WriteTwiceDailyMixHtData(conn, tdArr);
      }
    }

    public static void Read3DaysMixHt(
      OleDbConnection conn,
      DateTime curDate,
      ref double[] amMixHt,
      ref double[] pmMixHt)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT * FROM TwiceDailyMixHt Where TimeStamp=#" + curDate.ToString() + "#;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          amMixHt[1] = (double) oleDbDataReader["MixHtAM"];
          pmMixHt[1] = (double) oleDbDataReader["MixHtPM"];
        }
        DateTime dateTime1 = curDate.AddDays(-1.0);
        oleDbCommand.CommandText = "SELECT * FROM TwiceDailyMixHt Where TimeStamp=#" + dateTime1.ToString() + "#;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          amMixHt[0] = (double) oleDbDataReader["MixHtAM"];
          pmMixHt[0] = (double) oleDbDataReader["MixHtPM"];
        }
        DateTime dateTime2 = curDate.AddDays(1.0);
        oleDbCommand.CommandText = "SELECT * FROM TwiceDailyMixHt Where TimeStamp=#" + dateTime2.ToString() + "#;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          amMixHt[2] = (double) oleDbDataReader["MixHtAM"];
          pmMixHt[2] = (double) oleDbDataReader["MixHtPM"];
        }
      }
    }

    [Obsolete]
    public static void Read3DaysMixHt(
      string sDB,
      DateTime curDate,
      ref double[] amMixHt,
      ref double[] pmMixHt)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        TwiceDailyMixHt.Read3DaysMixHt(conn, curDate, ref amMixHt, ref pmMixHt);
      }
    }
  }
}
