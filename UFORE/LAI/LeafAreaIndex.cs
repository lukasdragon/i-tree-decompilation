// Decompiled with JetBrains decompiler
// Type: UFORE.LAI.LeafAreaIndex
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Deposition;
using UFORE.Location;

namespace UFORE.LAI
{
  public class LeafAreaIndex
  {
    public DateTime TimeStamp;
    public double Lai;
    private const string LAI_TABLE = "LAI";
    private const int TR_DAYS = 14;
    private const double GRASS_LAI = 1.2;

    public static void ProcessLAI(
      double maxLai,
      double pctEvGrn,
      LocationData locData,
      int startYear,
      int endYear,
      string laiDB)
    {
      bool flag1 = locData.LeafOffDOY > locData.LeafOnDOY;
      bool flag2 = (locData.LeafOnDOY == 0 || locData.LeafOnDOY == 1) && locData.LeafOffDOY == 365;
      AccessFunc.CreateDB(laiDB);
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + laiDB))
      {
        conn.Open();
        LeafAreaIndex.CreateLAITable(conn);
        double min = maxLai * (pctEvGrn / 100.0);
        for (int yr = startYear; yr <= endYear; ++yr)
        {
          List<LeafAreaIndex> leafAreaIndexList1 = new List<LeafAreaIndex>();
          List<LeafAreaIndex> leafAreaIndexList2 = new List<LeafAreaIndex>();
          if (flag2)
            LeafAreaIndex.CreateAnnualDailyLAINoFrost(yr, maxLai, ref leafAreaIndexList1);
          else if (flag1)
            LeafAreaIndex.CreateAnnualDailyLAINorth(yr, min, maxLai, locData.LeafOnDOY, locData.LeafOffDOY, ref leafAreaIndexList1);
          else
            LeafAreaIndex.CreateAnnualDailyLAISouth(yr, min, maxLai, locData.LeafOnDOY, locData.LeafOffDOY, ref leafAreaIndexList1);
          LeafAreaIndex.CreateAnnualHourlyLAI(ref leafAreaIndexList1, ref leafAreaIndexList2);
          LeafAreaIndex.WriteLAIRecords(conn, ref leafAreaIndexList2);
        }
      }
    }

    public static void ProcessLAI(
      double maxLai,
      double pctEvGrn,
      LocationData locData,
      int startYear,
      int endYear,
      string laiDB,
      UFORE_D uforeDObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      bool flag1 = locData.LeafOffDOY > locData.LeafOnDOY;
      bool flag2 = (locData.LeafOnDOY == 0 || locData.LeafOnDOY == 1) && locData.LeafOffDOY == 365;
      bool flag3 = locData.LeafOnDOY == 0 && locData.LeafOffDOY == 0;
      double num = (double) (PercentRangeTo - PercentRangeFrom) / (double) (endYear - startYear + 1);
      AccessFunc.CreateDB(laiDB);
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + laiDB))
      {
        conn.Open();
        LeafAreaIndex.CreateLAITable(conn);
        double min = maxLai * (pctEvGrn / 100.0);
        for (int yr = startYear; yr <= endYear; ++yr)
        {
          List<LeafAreaIndex> leafAreaIndexList1 = new List<LeafAreaIndex>();
          List<LeafAreaIndex> leafAreaIndexList2 = new List<LeafAreaIndex>();
          if (flag2)
            LeafAreaIndex.CreateAnnualDailyLAINoFrost(yr, maxLai, ref leafAreaIndexList1);
          else if (flag3)
            LeafAreaIndex.CreateAnnualDailyLAIAllFrost(yr, min, ref leafAreaIndexList1);
          else if (flag1)
            LeafAreaIndex.CreateAnnualDailyLAINorth(yr, min, maxLai, locData.LeafOnDOY, locData.LeafOffDOY, ref leafAreaIndexList1);
          else
            LeafAreaIndex.CreateAnnualDailyLAISouth(yr, min, maxLai, locData.LeafOnDOY, locData.LeafOffDOY, ref leafAreaIndexList1);
          LeafAreaIndex.CreateAnnualHourlyLAI(ref leafAreaIndexList1, ref leafAreaIndexList2);
          uforeDObj.reportProgress(PercentRangeFrom + (int) (num * ((double) (yr - startYear) + 0.5)));
          LeafAreaIndex.WriteLAIRecords(conn, ref leafAreaIndexList2, uforeDObj, PercentRangeFrom + (int) (num * ((double) (yr - startYear) + 0.5)), PercentRangeFrom + (int) (num * (double) (yr - startYear + 1)));
          uforeDObj.reportProgress(PercentRangeFrom + (int) (num * (double) (yr - startYear + 1)));
        }
      }
    }

    private static void CreateAnnualDailyLAIAllFrost(
      int yr,
      double min,
      ref List<LeafAreaIndex> list)
    {
      int num = DateTime.IsLeapYear(yr) ? 366 : 365;
      DateTime dateTime = new DateTime(yr, 1, 1);
      for (int index = 0; index < num; ++index)
      {
        list.Add(new LeafAreaIndex()
        {
          TimeStamp = dateTime,
          Lai = min
        });
        dateTime = dateTime.AddDays(1.0);
      }
    }

    private static void CreateAnnualDailyLAINoFrost(
      int yr,
      double max,
      ref List<LeafAreaIndex> list)
    {
      int num = DateTime.IsLeapYear(yr) ? 366 : 365;
      DateTime dateTime = new DateTime(yr, 1, 1);
      for (int index = 0; index < num; ++index)
      {
        list.Add(new LeafAreaIndex()
        {
          TimeStamp = dateTime,
          Lai = max
        });
        dateTime = dateTime.AddDays(1.0);
      }
    }

    private static void CreateAnnualDailyLAINorth(
      int yr,
      double min,
      double max,
      int LeafOnDOY,
      int LeafOffDOY,
      ref List<LeafAreaIndex> list)
    {
      int num1 = DateTime.IsLeapYear(yr) ? 366 : 365;
      int num2 = LeafOffDOY - LeafOnDOY;
      for (int index = 0; index < num1; ++index)
        list.Add(new LeafAreaIndex() { Lai = min });
      for (int index = 15; index < num2; ++index)
        list[index].Lai = max;
      for (int index = 0; index < 29; ++index)
        list[index].Lai = min + (max - min) / (1.0 + Math.Exp(-0.37 * (double) (index - 14)));
      int num3 = num2;
      for (int index = 0; index < 29; ++index)
        list[num3++].Lai = min + (max - min) / (1.0 + Math.Exp(-0.37 * (double) (14 - index)));
      DateTime dateTime = new DateTime(yr - 1, 12, 31);
      list[0].TimeStamp = dateTime.AddDays((double) (LeafOnDOY - 14));
      if (list[0].TimeStamp.Year != yr)
      {
        dateTime = new DateTime(yr, list[0].TimeStamp.Month, list[0].TimeStamp.Day);
        list[0].TimeStamp = dateTime;
      }
      for (int index = 1; index < list.Count; ++index)
      {
        list[index].TimeStamp = list[index - 1].TimeStamp.AddDays(1.0);
        if (list[index].TimeStamp.Year > yr)
        {
          dateTime = new DateTime(yr, list[index].TimeStamp.Month, list[index].TimeStamp.Day);
          list[index].TimeStamp = dateTime;
        }
      }
    }

    private static void CreateAnnualDailyLAISouth(
      int yr,
      double min,
      double max,
      int LeafOnDOY,
      int LeafOffDOY,
      ref List<LeafAreaIndex> list)
    {
      int num1 = DateTime.IsLeapYear(yr) ? 366 : 365;
      int num2 = LeafOnDOY - LeafOffDOY;
      for (int index = 0; index < num1; ++index)
        list.Add(new LeafAreaIndex() { Lai = max });
      for (int index = 15; index < num2; ++index)
        list[index].Lai = min;
      for (int index = 0; index < 29; ++index)
        list[index].Lai = min + (max - min) / (1.0 + Math.Exp(-0.37 * (double) (14 - index)));
      int num3 = num2;
      for (int index = 0; index < 29; ++index)
        list[num3++].Lai = min + (max - min) / (1.0 + Math.Exp(-0.37 * (double) (index - 14)));
      DateTime dateTime = new DateTime(yr - 1, 12, 31);
      list[0].TimeStamp = dateTime.AddDays((double) (LeafOffDOY - 14));
      if (list[0].TimeStamp.Year != yr)
      {
        dateTime = new DateTime(yr, list[0].TimeStamp.Month, list[0].TimeStamp.Day);
        list[0].TimeStamp = dateTime;
      }
      for (int index = 1; index < list.Count; ++index)
      {
        list[index].TimeStamp = list[index - 1].TimeStamp.AddDays(1.0);
        if (list[index].TimeStamp.Year > yr)
        {
          dateTime = new DateTime(yr, list[index].TimeStamp.Month, list[index].TimeStamp.Day);
          list[index].TimeStamp = dateTime;
        }
      }
    }

    private static void CreateAnnualHourlyLAI(
      ref List<LeafAreaIndex> annDailyLAI,
      ref List<LeafAreaIndex> annHourlyLAI)
    {
      for (int index1 = 0; index1 < annDailyLAI.Count; ++index1)
      {
        DateTime timeStamp = annDailyLAI[index1].TimeStamp;
        for (int index2 = 0; index2 < 24; ++index2)
          annHourlyLAI.Add(new LeafAreaIndex()
          {
            Lai = annDailyLAI[index1].Lai,
            TimeStamp = timeStamp.AddHours((double) index2)
          });
      }
    }

    public static void CreateLAITable(OleDbConnection conn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "CREATE TABLE LAI ([TimeStamp] DateTime,[LAI] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void CreateLAITable(string sDB)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        LeafAreaIndex.CreateLAITable(conn);
      }
    }

    public static void WriteLAIRecords(OleDbConnection conn, ref List<LeafAreaIndex> laiList)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand("INSERT INTO LAI ([TimeStamp],[LAI]) Values (@TimeStamp,@LAI)", conn))
      {
        oleDbCommand.Parameters.Add("@TimeStamp", OleDbType.Date);
        oleDbCommand.Parameters.Add("@LAI", OleDbType.Double);
        for (int index = 0; index < laiList.Count; ++index)
        {
          oleDbCommand.Parameters["@TimeStamp"].Value = (object) laiList[index].TimeStamp;
          oleDbCommand.Parameters["@LAI"].Value = (object) laiList[index].Lai;
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    public static void WriteLAIRecords(string sDB, ref List<LeafAreaIndex> laiList)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        LeafAreaIndex.WriteLAIRecords(conn, ref laiList);
      }
    }

    public static void WriteLAIRecords(
      OleDbConnection conn,
      ref List<LeafAreaIndex> laiList,
      UFORE_D uforeDObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      double num = (double) (PercentRangeTo - PercentRangeFrom) / (double) laiList.Count;
      using (OleDbCommand oleDbCommand = new OleDbCommand("INSERT INTO LAI ([TimeStamp],[LAI]) Values (@TimeStamp,@LAI)", conn))
      {
        oleDbCommand.Parameters.Add("@TimeStamp", OleDbType.Date);
        oleDbCommand.Parameters.Add("@LAI", OleDbType.Double);
        oleDbCommand.Parameters["@TimeStamp"].Value = (object) laiList[0].TimeStamp;
        oleDbCommand.Parameters["@LAI"].Value = (object) laiList[0].Lai;
        oleDbCommand.ExecuteNonQuery();
        uforeDObj.reportProgress(PercentRangeFrom + (int) (num * 1.0));
        for (int index = 1; index < laiList.Count; ++index)
        {
          oleDbCommand.Parameters["@TimeStamp"].Value = (object) laiList[index].TimeStamp;
          oleDbCommand.Parameters["@LAI"].Value = (object) laiList[index].Lai;
          oleDbCommand.ExecuteNonQuery();
          uforeDObj.reportProgress(PercentRangeFrom + (int) (num * (double) (index + 1)));
        }
      }
    }

    [Obsolete]
    public static void WriteLAIRecords(
      string sDB,
      ref List<LeafAreaIndex> laiList,
      UFORE_D uforeDObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        LeafAreaIndex.WriteLAIRecords(conn, ref laiList, uforeDObj, PercentRangeFrom, PercentRangeTo);
      }
    }

    public static int ReadLAIAllRecords(OleDbConnection conn, ref List<LeafAreaIndex> laiList)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT * FROM LAI ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
            laiList.Add(new LeafAreaIndex()
            {
              TimeStamp = oleDbDataReader.GetDateTime(0),
              Lai = oleDbDataReader.GetDouble(1)
            });
        }
      }
      return laiList.Count;
    }

    [Obsolete]
    public static int ReadLAIAllRecords(string sDB, ref List<LeafAreaIndex> laiList)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        return LeafAreaIndex.ReadLAIAllRecords(conn, ref laiList);
      }
    }

    public static int ReadLAIPartialRecords(
      OleDbConnection cnLaiDB,
      ref List<LeafAreaIndex> laiList,
      DateTime start,
      DateTime end)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnLaiDB;
        oleDbCommand.CommandText = "SELECT * FROM LAI WHERE TimeStamp >= #" + start.ToString() + "# AND TimeStamp<= #" + end.ToString() + "# ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
            laiList.Add(new LeafAreaIndex()
            {
              TimeStamp = oleDbDataReader.GetDateTime(0),
              Lai = oleDbDataReader.GetDouble(1)
            });
        }
      }
      return laiList.Count;
    }

    [Obsolete]
    public static int ReadLAIPartialRecords(
      string sLaiDB,
      ref List<LeafAreaIndex> laiList,
      DateTime start,
      DateTime end)
    {
      using (OleDbConnection cnLaiDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sLaiDB))
      {
        cnLaiDB.Open();
        return LeafAreaIndex.ReadLAIPartialRecords(cnLaiDB, ref laiList, start, end);
      }
    }

    public static double ReadMaxLAI(OleDbConnection conn, DateTime start, DateTime end)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Max(LAI) AS [Max Of LAI] FROM LAI WHERE TimeStamp >= #" + start.ToString() + "# AND TimeStamp <= #" + end.ToString() + "#;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          return oleDbDataReader.GetDouble(0);
        }
      }
    }

    [Obsolete]
    public static double ReadMaxLAI(string sDB, DateTime start, DateTime end)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        return LeafAreaIndex.ReadMaxLAI(conn, start, end);
      }
    }

    public static void ProcessGrassLAI(
      int modelYr,
      string st,
      string srcDB,
      double grsCov,
      double wdGrsCov,
      double hrbCov,
      string dstDB)
    {
      List<LeafAreaIndex> leafAreaIndexList1 = new List<LeafAreaIndex>();
      List<LeafAreaIndex> leafAreaIndexList2 = new List<LeafAreaIndex>();
      DateTime dateTime = new DateTime(modelYr, 1, 1);
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      double laiRatio = 1.2 / LeafAreaIndex.ReadDailyMaxLAI(srcDB, st);
      LeafAreaIndex.ReadDailyLAIRecords(srcDB, st, modelYr, ref leafAreaIndexList1);
      LeafAreaIndex.CreateHourlyLAI(ref leafAreaIndexList1, laiRatio, grsCov, wdGrsCov, hrbCov, ref leafAreaIndexList2);
      AccessFunc.CreateDB(dstDB);
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + dstDB))
      {
        conn.Open();
        LeafAreaIndex.CreateLAITable(conn);
        LeafAreaIndex.WriteLAIRecords(conn, ref leafAreaIndexList2);
      }
    }

    private static double ReadDailyMaxLAI(string DB, string state)
    {
      double num = 0.0;
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + DB))
      {
        try
        {
          oleDbConnection.Open();
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "SELECT TOP 1 [LAI (m2/m2) - Smoothed data] FROM " + state + " ORDER BY [LAI (m2/m2) - Smoothed data] DESC;";
          OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
          while (oleDbDataReader.Read())
            num = (double) oleDbDataReader["LAI (m2/m2) - Smoothed data"];
          return num;
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    private static void ReadDailyLAIRecords(
      string DB,
      string state,
      int yr,
      ref List<LeafAreaIndex> listLai)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + DB))
      {
        try
        {
          oleDbConnection.Open();
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "SELECT * FROM " + state + " ORDER BY Date;";
          OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
          while (oleDbDataReader.Read())
          {
            LeafAreaIndex leafAreaIndex1 = new LeafAreaIndex();
            DateTime dateTime = (DateTime) oleDbDataReader["Date"];
            int month = dateTime.Month;
            dateTime = (DateTime) oleDbDataReader["Date"];
            int num1 = dateTime.Day;
            leafAreaIndex1.TimeStamp = DateTime.Parse(yr.ToString() + "/" + month.ToString() + "/" + num1.ToString());
            leafAreaIndex1.Lai = (double) oleDbDataReader["LAI (m2/m2) - Smoothed data"];
            listLai.Add(leafAreaIndex1);
            if (DateTime.IsLeapYear(yr) && month == 2 && num1 == 28)
            {
              LeafAreaIndex leafAreaIndex2 = new LeafAreaIndex();
              int num2 = 2;
              num1 = 29;
              leafAreaIndex2.TimeStamp = DateTime.Parse(yr.ToString() + "/" + num2.ToString() + "/" + num1.ToString());
              leafAreaIndex2.Lai = (double) oleDbDataReader["LAI (m2/m2) - Smoothed data"];
              listLai.Add(leafAreaIndex2);
            }
          }
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    private static void CreateHourlyLAI(
      ref List<LeafAreaIndex> dailyLAI,
      double laiRatio,
      double grsCov,
      double wdGrsCov,
      double hrbCov,
      ref List<LeafAreaIndex> hourlyLAI)
    {
      for (int index1 = 0; index1 < dailyLAI.Count; ++index1)
      {
        for (int index2 = 0; index2 < 24; ++index2)
          hourlyLAI.Add(new LeafAreaIndex()
          {
            TimeStamp = dailyLAI[index1].TimeStamp.AddHours((double) index2),
            Lai = grsCov != 0.0 || wdGrsCov != 0.0 || hrbCov != 0.0 ? dailyLAI[index1].Lai * laiRatio * (grsCov / (grsCov + wdGrsCov + hrbCov)) + dailyLAI[index1].Lai * ((wdGrsCov + hrbCov) / (grsCov + wdGrsCov + hrbCov)) : 0.0
          });
      }
    }
  }
}
