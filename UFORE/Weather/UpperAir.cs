// Decompiled with JetBrains decompiler
// Type: UFORE.Weather.UpperAir
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using UFORE.Deposition;

namespace UFORE.Weather
{
  public class UpperAir
  {
    public const string UPR_TABLE = "UpperAir";
    public DateTime TimeStamp;
    public int Year;
    public int Month;
    public int Day;
    public int Hour;
    public double PressMBar;
    public double HeightM;
    public double TempK;

    [Obsolete]
    public static void ProcessUpperAirData(string sUprFile, string sDB)
    {
      using (OleDbConnection cnDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        cnDB.Open();
        UpperAir.ProcessUpperAirData(sUprFile, cnDB);
      }
    }

    public static void ProcessUpperAirData(string sUprFile, OleDbConnection cnDB)
    {
      List<UpperAir> upperAirList = new List<UpperAir>();
      int recCnt = UpperAir.ReadUpperAirData(sUprFile, ref upperAirList);
      UpperAir.ConvertUnits(ref upperAirList);
      UpperAir.CreateUpperAirTable(cnDB);
      UpperAir.WriteUpperAirRecords(cnDB, ref upperAirList, recCnt);
    }

    [Obsolete]
    public static void ProcessUpperAirData(
      string sUprFile,
      string sDB,
      UFORE_D uforeDObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      using (OleDbConnection cnDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        cnDB.Open();
        UpperAir.ProcessUpperAirData(sUprFile, cnDB, uforeDObj, PercentRangeFrom, PercentRangeTo);
      }
    }

    public static void ProcessUpperAirData(
      string sUprFile,
      OleDbConnection cnDB,
      UFORE_D uforeDObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      List<UpperAir> upperAirList = new List<UpperAir>();
      int recCnt = UpperAir.ReadUpperAirData(sUprFile, ref upperAirList);
      UpperAir.ConvertUnits(ref upperAirList);
      UpperAir.CreateUpperAirTable(cnDB);
      uforeDObj.reportProgress(PercentRangeFrom++);
      UpperAir.WriteUpperAirRecords(cnDB, ref upperAirList, recCnt, uforeDObj, PercentRangeFrom, PercentRangeTo);
      uforeDObj.reportProgress(PercentRangeTo);
    }

    private static void ConvertUnits(ref List<UpperAir> list)
    {
      for (int index = 0; index < list.Count; ++index)
      {
        if (list[index].PressMBar != 99999.0)
        {
          if (list[index].PressMBar == (double) short.MaxValue)
            list[index].PressMBar = 99999.0;
          else
            list[index].PressMBar /= 10.0;
        }
        if (list[index].TempK != 99999.0)
          list[index].TempK = list[index].TempK != (double) short.MaxValue ? list[index].TempK / 10.0 + 273.16 : 99999.0;
      }
    }

    public static int ReadUpperAirData(string sFile, ref List<UpperAir> uprList)
    {
      string[] mon = new string[12]
      {
        "JAN",
        "FEB",
        "MAR",
        "APR",
        "MAY",
        "JUN",
        "JUL",
        "AUG",
        "SEP",
        "OCT",
        "NOV",
        "DEC"
      };
      StreamReader streamReader;
      using (streamReader = new StreamReader(sFile))
      {
        try
        {
          string str1;
          while ((str1 = streamReader.ReadLine()) != null)
          {
            int num1 = int.Parse(str1.Substring(7, 7));
            int num2 = int.Parse(str1.Substring(14, 7));
            string month = str1.Substring(27, 3);
            int num3 = int.Parse(str1.Substring(34, 4));
            string str2 = streamReader.ReadLine();
            int num4 = int.Parse(streamReader.ReadLine().Substring(28, 7));
            str2 = streamReader.ReadLine();
            for (int index = 0; index < num4 - 4; ++index)
            {
              UpperAir upperAir = new UpperAir();
              string str3 = streamReader.ReadLine();
              upperAir.Hour = num1;
              upperAir.Day = num2;
              upperAir.Month = UpperAir.MonthConv(month, mon);
              upperAir.Year = num3;
              upperAir.TimeStamp = DateTime.Parse(upperAir.Month.ToString() + "/" + num2.ToString() + "/" + num3.ToString() + " " + num1.ToString() + ":00");
              upperAir.PressMBar = (double) int.Parse(str3.Substring(7, 7));
              upperAir.HeightM = double.Parse(str3.Substring(14, 7));
              upperAir.TempK = (double) int.Parse(str3.Substring(21, 7));
              uprList.Add(upperAir);
            }
          }
          streamReader.Close();
          return uprList.Count;
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    public static int ReadUpperAirDataTimeStamp(string sFile, ref List<UpperAir> uprList)
    {
      string[] mon = new string[12]
      {
        "JAN",
        "FEB",
        "MAR",
        "APR",
        "MAY",
        "JUN",
        "JUL",
        "AUG",
        "SEP",
        "OCT",
        "NOV",
        "DEC"
      };
      StreamReader streamReader;
      using (streamReader = new StreamReader(sFile))
      {
        try
        {
          string str1;
          while ((str1 = streamReader.ReadLine()) != null)
          {
            int num1 = int.Parse(str1.Substring(7, 7));
            int num2 = int.Parse(str1.Substring(14, 7));
            string month = str1.Substring(27, 3);
            int num3 = int.Parse(str1.Substring(34, 4));
            string str2 = streamReader.ReadLine();
            int num4 = int.Parse(streamReader.ReadLine().Substring(28, 7));
            str2 = streamReader.ReadLine();
            UpperAir upperAir = new UpperAir()
            {
              Hour = num1,
              Day = num2,
              Month = UpperAir.MonthConv(month, mon),
              Year = num3
            };
            upperAir.TimeStamp = DateTime.Parse(upperAir.Month.ToString() + "/" + num2.ToString() + "/" + num3.ToString() + " " + num1.ToString() + ":00");
            uprList.Add(upperAir);
            for (int index = 0; index < num4 - 4; ++index)
              str2 = streamReader.ReadLine();
          }
          streamReader.Close();
          return uprList.Count;
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    private static int MonthConv(string month, string[] mon)
    {
      int index = 0;
      while (index < 12 && !(month == mon[index]))
        ++index;
      return index + 1;
    }

    private static int GetLineCount(string sFile)
    {
      int lineCount = 0;
      int num1 = 0;
      try
      {
        StreamReader streamReader = new StreamReader(sFile);
        while (streamReader.ReadLine() != null)
        {
          int num2 = num1 + 1;
          streamReader.ReadLine();
          string str = streamReader.ReadLine();
          streamReader.ReadLine();
          num1 = num2 + 3;
          int num3 = int.Parse(str.Substring(28, 7));
          lineCount = lineCount + num3 - 4;
          for (int index = 0; index < num3 - 4; ++index)
          {
            streamReader.ReadLine();
            ++num1;
          }
        }
        streamReader.Close();
        return lineCount;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static int ReadUpperAirRecords(
      OleDbConnection conn,
      DateTime dt0hr,
      ref List<UpperAir> uprList)
    {
      double num1;
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        int num2 = 12;
        DateTime dateTime1 = dt0hr.AddHours((double) num2);
        oleDbCommand.CommandText = "SELECT DISTINCTROW TimeStamp, Count(*) AS [Count] FROM UpperAir WHERE PressMBar <> 99999 AND TempK <> 99999 GROUP BY TimeStamp HAVING TimeStamp=#" + dateTime1.ToString() + "#;";
        bool flag;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          flag = oleDbDataReader.Read();
          if (flag)
          {
            int num3 = (int) oleDbDataReader["Count"];
          }
        }
        if (!flag)
        {
          DateTime dateTime2 = dt0hr.AddHours(10.0);
          DateTime dateTime3 = dt0hr.AddHours(15.0);
          oleDbCommand.CommandText = "SELECT DISTINCTROW TimeStamp, Count(*) AS [Count] FROM UpperAir WHERE PressMBar <>99999 AND TempK <> 99999 GROUP BY TimeStamp HAVING TimeStamp >= #" + dateTime2.ToString() + "# And TimeStamp <= #" + dateTime3.ToString() + "# And TimeStamp <> #" + dateTime1.ToString() + "#;";
          using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
          {
            if (!oleDbDataReader.Read())
              return 0;
            int num4 = 0;
            do
            {
              if (num4 < (int) oleDbDataReader["Count"])
              {
                num2 = ((DateTime) oleDbDataReader["TimeStamp"]).Hour;
                num4 = (int) oleDbDataReader["Count"];
              }
            }
            while (oleDbDataReader.Read());
          }
        }
        DateTime dateTime4 = dt0hr.AddHours((double) num2);
        oleDbCommand.CommandText = "SELECT TimeStamp, PressMBar, HeightM, TempK FROM UpperAir WHERE TimeStamp =#" + dateTime4.ToString() + "# AND PressMBar <> 99999 AND TempK <> 99999 ORDER BY PressMBar DESC;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
            uprList.Add(new UpperAir()
            {
              TimeStamp = (DateTime) oleDbDataReader["TimeStamp"],
              PressMBar = (double) oleDbDataReader["PressMBar"],
              HeightM = (double) oleDbDataReader["HeightM"],
              TempK = (double) oleDbDataReader["TempK"]
            });
        }
        oleDbCommand.CommandText = "SELECT DISTINCTROW Min(UpperAir.HeightM) AS [Min Of HeightM] FROM UpperAir WHERE TimeStamp=#" + dateTime4.ToString() + "# AND PressMBar <> 99999 AND TempK <> 99999;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          num1 = oleDbDataReader.GetDouble(0);
        }
      }
      if (num1 == 99999.0)
        return 0;
      for (int index = 0; index < uprList.Count; ++index)
        uprList[index].HeightM -= num1;
      return uprList.Count;
    }

    [Obsolete]
    public static int ReadUpperAirRecords(string sDB, DateTime dt0hr, ref List<UpperAir> uprList)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        return UpperAir.ReadUpperAirRecords(conn, dt0hr, ref uprList);
      }
    }

    private static void CreateUpperAirTable(OleDbConnection conn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "CREATE TABLE UpperAir([TimeStamp] DateTime,[PressMBar] DOUBLE,[HeightM] DOUBLE,[TempK] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    private static void CreateUpperAirTable(string sDB)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        UpperAir.CreateUpperAirTable(conn);
      }
    }

    private static void WriteUpperAirRecords(
      OleDbConnection conn,
      ref List<UpperAir> uprList,
      int recCnt)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        for (int index = 0; index < recCnt; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO UpperAir([TimeStamp],[PressMBar],[HeightM],[TempK]) Values (#" + uprList[index].TimeStamp.ToString() + "#," + uprList[index].PressMBar.ToString() + "," + uprList[index].HeightM.ToString() + "," + uprList[index].TempK.ToString() + ");";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    private static void WriteUpperAirRecords(string sDB, ref List<UpperAir> uprList, int recCnt)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        UpperAir.WriteUpperAirRecords(conn, ref uprList, recCnt);
      }
    }

    private static void WriteUpperAirRecords(
      OleDbConnection conn,
      ref List<UpperAir> uprList,
      int recCnt,
      UFORE_D uforeDObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      double num = (double) (PercentRangeTo - PercentRangeFrom) / (double) recCnt;
      using (OleDbCommand oleDbCommand = new OleDbCommand("INSERT INTO UpperAir ([TimeStamp],[PressMBar],[HeightM],[TempK]) Values (@TimeStamp,@PressMBar,@HeightM,@TempK)", conn))
      {
        oleDbCommand.Parameters.Add("@TimeStamp", OleDbType.Date);
        oleDbCommand.Parameters.Add("@PressMBar", OleDbType.Double);
        oleDbCommand.Parameters.Add("@HeightM", OleDbType.Double);
        oleDbCommand.Parameters.Add("@TempK", OleDbType.Double);
        oleDbCommand.Parameters["@TimeStamp"].Value = (object) uprList[0].TimeStamp;
        oleDbCommand.Parameters["@PressMBar"].Value = (object) uprList[0].PressMBar;
        oleDbCommand.Parameters["@HeightM"].Value = (object) uprList[0].HeightM;
        oleDbCommand.Parameters["@TempK"].Value = (object) uprList[0].TempK;
        oleDbCommand.Prepare();
        oleDbCommand.ExecuteNonQuery();
        uforeDObj.reportProgress(PercentRangeFrom + (int) (1.0 * num));
        for (int index = 1; index < recCnt; ++index)
        {
          oleDbCommand.Parameters["@TimeStamp"].Value = (object) uprList[index].TimeStamp;
          oleDbCommand.Parameters["@PressMBar"].Value = (object) uprList[index].PressMBar;
          oleDbCommand.Parameters["@HeightM"].Value = (object) uprList[index].HeightM;
          oleDbCommand.Parameters["@TempK"].Value = (object) uprList[index].TempK;
          oleDbCommand.ExecuteNonQuery();
          uforeDObj.reportProgress(PercentRangeFrom + (int) ((double) (index + 1) * num));
        }
      }
    }

    [Obsolete]
    private static void WriteUpperAirRecords(
      string sDB,
      ref List<UpperAir> uprList,
      int recCnt,
      UFORE_D uforeDObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        conn.Open();
        UpperAir.WriteUpperAirRecords(conn, ref uprList, recCnt, uforeDObj, PercentRangeFrom, PercentRangeTo);
      }
    }

    public static void CopyUpperAirData(string inFile, string outFile, bool blAppend)
    {
      using (StreamReader streamReader = new StreamReader(inFile))
      {
        using (StreamWriter streamWriter = new StreamWriter(outFile, blAppend))
        {
          for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
            streamWriter.WriteLine(str);
        }
      }
    }
  }
}
