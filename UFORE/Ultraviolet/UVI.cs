// Decompiled with JetBrains decompiler
// Type: UFORE.Ultraviolet.UVI
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace UFORE.Ultraviolet
{
  public class UVI
  {
    public DateTime TimeStamp;
    public double UVindex;
    private const int NODATA = -1;

    public static void ReadUVIRecords(
      string sDB,
      string tbl,
      double lat,
      double lon,
      ref List<UVI> uviData)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sDB))
      {
        try
        {
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "SELECT * FROM [" + tbl + "] WHERE Latitude = " + lat.ToString() + " AND Longitude = " + lon.ToString() + " ORDER BY TimeStamp;";
          OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
          while (oleDbDataReader.Read())
            uviData.Add(new UVI()
            {
              TimeStamp = (DateTime) oleDbDataReader["TimeStamp"],
              UVindex = (double) oleDbDataReader[nameof (UVI)]
            });
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    public static void SetNullUVIData(int year, ref List<UVI> uviData)
    {
      DateTime dateTime = DateTime.Parse("1/1/" + year.ToString() + " 12:00");
      int num = DateTime.IsLeapYear(year) ? 366 : 365;
      for (int index = 0; index < num; ++index)
        uviData.Add(new UVI()
        {
          TimeStamp = dateTime.AddDays((double) index),
          UVindex = 0.0
        });
    }

    public static void FillExtrapolate(ref List<UVI> list)
    {
      double num = 0.0;
      if (list[0].UVindex == -1.0)
      {
        int index1 = 1;
        while (list[index1].UVindex == -1.0)
        {
          ++index1;
          if (index1 == list.Count)
            goto label_5;
        }
        num = list[index1].UVindex;
label_5:
        if (index1 != list.Count)
        {
          for (int index2 = index1 - 1; index2 >= 0; --index2)
            list[index2].UVindex = num;
        }
      }
      if (list[list.Count - 1].UVindex != -1.0)
        return;
      int index3 = list.Count - 1;
      while (list[index3].UVindex == -1.0)
      {
        --index3;
        if (index3 == 0)
          goto label_14;
      }
      num = list[index3].UVindex;
label_14:
      if (index3 == 0)
        return;
      for (int index4 = index3 + 1; index4 < list.Count; ++index4)
        list[index4].UVindex = num;
    }

    public static void FillInterpolate(ref List<UVI> list)
    {
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        if (list[index1].UVindex == -1.0)
        {
          int num = index1 - 1;
          int index2 = index1 + 1;
          while (list[index2].UVindex == -1.0)
            ++index2;
          int index3 = index2;
          for (int index4 = num + 1; index4 < index3; ++index4)
            list[index4].UVindex = list[index3].UVindex;
          index1 = index2;
        }
      }
    }
  }
}
