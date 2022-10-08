// Decompiled with JetBrains decompiler
// Type: UFORE.Ultraviolet.UVReduction
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Weather;

namespace UFORE.Ultraviolet
{
  public class UVReduction
  {
    private DateTime Date;
    private string Landuse;
    private double Latitude;
    private double Longitude;
    private double CldCov;
    private double TreeCov;
    private double UVI;
    private double AdjUVI;
    private double SolZenRad;
    private double ShadedTs;
    private double ShadedUvi;
    private double ShadedPf;
    private double ShadedUVRed;
    private double ShadedUVRedPct;
    private double AllTs;
    private double AllUvi;
    private double AllPf;
    private double AllUVRed;
    private double AllUVRedPct;
    private const int NODATA = -1;
    private const string UVR_TABLE = "UVReduction";
    private const string TOTAL_UVR_TABLE = "TotalUVReduction";
    public const string MONTHLY_TABLE = "MonthlyMeanUVReduction";
    public const string YEARLY_TABLE = "YearlyMeanUVReduction";

    private static void CreateShadeDict(ref Dictionary<UVReduction.SKYCOV, double[]> dict)
    {
      double[] numArray1 = new double[5]
      {
        0.261,
        0.407,
        1.5,
        0.969,
        7.24
      };
      double[] numArray2 = new double[5]
      {
        0.315,
        0.527,
        2.97,
        1.79,
        5.72
      };
      double[] numArray3 = new double[5]
      {
        0.259,
        0.37,
        2.63,
        1.82,
        7.78
      };
      double[] numArray4 = new double[5]
      {
        0.06,
        0.316,
        0.97,
        1.86,
        13.63
      };
      dict.Add(UVReduction.SKYCOV.CLR, numArray1);
      dict.Add(UVReduction.SKYCOV.SCT, numArray2);
      dict.Add(UVReduction.SKYCOV.BKN, numArray3);
      dict.Add(UVReduction.SKYCOV.OVC, numArray4);
    }

    private static void CreateAllDict(ref Dictionary<UVReduction.SKYCOV, double[]> dict)
    {
      double[] numArray1 = new double[3]{ 1.0, 0.598, 4.77 };
      double[] numArray2 = new double[3]
      {
        0.931,
        0.447,
        5.02
      };
      double[] numArray3 = new double[3]
      {
        0.683,
        0.252,
        7.0
      };
      double[] numArray4 = new double[3]
      {
        0.368,
        0.0,
        14.87
      };
      dict.Add(UVReduction.SKYCOV.CLR, numArray1);
      dict.Add(UVReduction.SKYCOV.SCT, numArray2);
      dict.Add(UVReduction.SKYCOV.BKN, numArray3);
      dict.Add(UVReduction.SKYCOV.OVC, numArray4);
    }

    public static void ProcessUVReduction(
      ref List<SurfaceWeather> metList,
      ref List<UFORE.Ultraviolet.UVI> uviList,
      double tc,
      double lat,
      double lon,
      string outDB)
    {
      List<UVReduction> uvrList = new List<UVReduction>();
      Dictionary<UVReduction.SKYCOV, double[]> dict1 = new Dictionary<UVReduction.SKYCOV, double[]>();
      Dictionary<UVReduction.SKYCOV, double[]> dict2 = new Dictionary<UVReduction.SKYCOV, double[]>();
      UVReduction.CreateShadeDict(ref dict1);
      UVReduction.CreateAllDict(ref dict2);
      for (int index = 0; index < metList.Count; ++index)
      {
        UVReduction uvReduction = new UVReduction();
        uvReduction.UVI = uviList[index].UVindex;
        if (uvReduction.UVI >= 0.0)
        {
          uvReduction.Latitude = lat;
          uvReduction.Longitude = lon;
          uvReduction.Date = metList[index].TimeStamp;
          uvReduction.TreeCov = tc / 100.0;
          uvReduction.CldCov = metList[index].ToCldCov;
          uvReduction.AdjUVI = uvReduction.UVI * UVReduction.getTrRate(uvReduction.CldCov);
          uvReduction.SolZenRad = metList[index].SolZenAgl * Math.PI / 180.0;
          UVReduction.SKYCOV skyCov = UVReduction.getSkyCov(uvReduction.CldCov);
          uvReduction.ShadedTs = (dict1[skyCov][0] + dict1[skyCov][1] * Math.Pow(uvReduction.SolZenRad, dict1[skyCov][2])) * (1.0 - uvReduction.TreeCov) - Math.Pow(uvReduction.SolZenRad, dict1[skyCov][3]) / dict1[skyCov][4] * Math.Sin(Math.PI * uvReduction.TreeCov);
          uvReduction.ShadedTs = uvReduction.TreeCov == 1.0 ? 0.0 : uvReduction.ShadedTs;
          uvReduction.ShadedUvi = uvReduction.UVI * uvReduction.ShadedTs;
          if (uvReduction.UVI == 0.0)
          {
            uvReduction.ShadedPf = 0.0;
            uvReduction.ShadedUVRedPct = 0.0;
          }
          else if (uvReduction.TreeCov == 1.0)
          {
            uvReduction.ShadedPf = -1.0;
            uvReduction.ShadedUVRedPct = (uvReduction.AdjUVI - uvReduction.ShadedUvi) / uvReduction.AdjUVI * 100.0;
          }
          else
          {
            uvReduction.ShadedPf = uvReduction.AdjUVI / uvReduction.ShadedUvi;
            uvReduction.ShadedUVRedPct = (uvReduction.AdjUVI - uvReduction.ShadedUvi) / uvReduction.AdjUVI * 100.0;
          }
          uvReduction.ShadedUVRed = uvReduction.AdjUVI - uvReduction.ShadedUvi;
          uvReduction.ShadedUVRed = uvReduction.ShadedUVRed < 0.0 ? 0.0 : uvReduction.ShadedUVRed;
          uvReduction.ShadedUVRedPct = uvReduction.ShadedUVRedPct < 0.0 ? 0.0 : uvReduction.ShadedUVRedPct;
          uvReduction.AllTs = dict2[skyCov][0] * (1.0 - uvReduction.TreeCov) - Math.Pow(uvReduction.SolZenRad, dict2[skyCov][1]) / dict2[skyCov][2] * Math.Sin(Math.PI * uvReduction.TreeCov);
          uvReduction.AllTs = uvReduction.TreeCov == 1.0 ? 0.0 : uvReduction.AllTs;
          uvReduction.AllUvi = uvReduction.UVI * uvReduction.AllTs;
          if (uvReduction.UVI == 0.0)
          {
            uvReduction.AllPf = 0.0;
            uvReduction.AllUVRedPct = 0.0;
          }
          else if (uvReduction.TreeCov == 1.0)
          {
            uvReduction.AllPf = -1.0;
            uvReduction.AllUVRedPct = (uvReduction.AdjUVI - uvReduction.AllUvi) / uvReduction.AdjUVI * 100.0;
          }
          else
          {
            uvReduction.AllPf = uvReduction.AdjUVI / uvReduction.AllUvi;
            uvReduction.AllUVRedPct = (uvReduction.AdjUVI - uvReduction.AllUvi) / uvReduction.AdjUVI * 100.0;
          }
          uvReduction.AllUVRed = uvReduction.AdjUVI - uvReduction.AllUvi;
          uvReduction.AllUVRed = uvReduction.AllUVRed < 0.0 ? 0.0 : uvReduction.AllUVRed;
          uvReduction.AllUVRedPct = uvReduction.AllUVRedPct < 0.0 ? 0.0 : uvReduction.AllUVRedPct;
          uvrList.Add(uvReduction);
        }
      }
      UVReduction.CreateCityUVReductionTable(outDB);
      UVReduction.WriteCityUVReductionRecords(outDB, uvrList);
    }

    public static void ProcessUVReduction(
      ref List<SurfaceWeather> metList,
      ref List<UFORE.Ultraviolet.UVI> uviList,
      ref List<string> landuse,
      ref Dictionary<string, UFORE.BioEmission.Landuse> luDict,
      double lat,
      double lon,
      string outDB)
    {
      List<UVReduction> uvrList = new List<UVReduction>();
      Dictionary<UVReduction.SKYCOV, double[]> dict1 = new Dictionary<UVReduction.SKYCOV, double[]>();
      Dictionary<UVReduction.SKYCOV, double[]> dict2 = new Dictionary<UVReduction.SKYCOV, double[]>();
      UVReduction.CreateShadeDict(ref dict1);
      UVReduction.CreateAllDict(ref dict2);
      for (int index1 = 0; index1 < landuse.Count; ++index1)
      {
        for (int index2 = 0; index2 < metList.Count; ++index2)
        {
          UVReduction uvReduction = new UVReduction();
          uvReduction.UVI = uviList[index2].UVindex;
          if (uvReduction.UVI >= 0.0)
          {
            UFORE.BioEmission.Landuse landuse1;
            luDict.TryGetValue(landuse[index1], out landuse1);
            uvReduction.Latitude = lat;
            uvReduction.Longitude = lon;
            uvReduction.Landuse = landuse1.LanduseDesc;
            uvReduction.Date = metList[index2].TimeStamp;
            uvReduction.TreeCov = landuse1.TreePctCov / 100.0;
            uvReduction.CldCov = metList[index2].ToCldCov;
            uvReduction.AdjUVI = uvReduction.UVI * UVReduction.getTrRate(uvReduction.CldCov);
            uvReduction.SolZenRad = metList[index2].SolZenAgl * Math.PI / 180.0;
            UVReduction.SKYCOV skyCov = UVReduction.getSkyCov(uvReduction.CldCov);
            uvReduction.ShadedTs = (dict1[skyCov][0] + dict1[skyCov][1] * Math.Pow(uvReduction.SolZenRad, dict1[skyCov][2])) * (1.0 - uvReduction.TreeCov) - Math.Pow(uvReduction.SolZenRad, dict1[skyCov][3]) / dict1[skyCov][4] * Math.Sin(Math.PI * uvReduction.TreeCov);
            uvReduction.ShadedTs = uvReduction.TreeCov == 1.0 ? 0.0 : uvReduction.ShadedTs;
            uvReduction.ShadedUvi = uvReduction.UVI * uvReduction.ShadedTs;
            if (uvReduction.UVI == 0.0)
            {
              uvReduction.ShadedPf = 0.0;
              uvReduction.ShadedUVRedPct = 0.0;
            }
            else if (uvReduction.TreeCov == 1.0)
            {
              uvReduction.ShadedPf = -1.0;
              uvReduction.ShadedUVRedPct = (uvReduction.AdjUVI - uvReduction.ShadedUvi) / uvReduction.AdjUVI * 100.0;
            }
            else
            {
              uvReduction.ShadedPf = uvReduction.AdjUVI / uvReduction.ShadedUvi;
              uvReduction.ShadedUVRedPct = (uvReduction.AdjUVI - uvReduction.ShadedUvi) / uvReduction.AdjUVI * 100.0;
            }
            uvReduction.ShadedUVRed = uvReduction.AdjUVI - uvReduction.ShadedUvi;
            uvReduction.ShadedUVRed = uvReduction.ShadedUVRed < 0.0 ? 0.0 : uvReduction.ShadedUVRed;
            uvReduction.ShadedUVRedPct = uvReduction.ShadedUVRedPct < 0.0 ? 0.0 : uvReduction.ShadedUVRedPct;
            uvReduction.AllTs = dict2[skyCov][0] * (1.0 - uvReduction.TreeCov) - Math.Pow(uvReduction.SolZenRad, dict2[skyCov][1]) / dict2[skyCov][2] * Math.Sin(Math.PI * uvReduction.TreeCov);
            uvReduction.AllTs = uvReduction.TreeCov == 1.0 ? 0.0 : uvReduction.AllTs;
            uvReduction.AllUvi = uvReduction.UVI * uvReduction.AllTs;
            if (uvReduction.UVI == 0.0)
            {
              uvReduction.AllPf = 0.0;
              uvReduction.AllUVRedPct = 0.0;
            }
            else if (uvReduction.TreeCov == 1.0)
            {
              uvReduction.AllPf = -1.0;
              uvReduction.AllUVRedPct = (uvReduction.AdjUVI - uvReduction.AllUvi) / uvReduction.AdjUVI * 100.0;
            }
            else
            {
              uvReduction.AllPf = uvReduction.AdjUVI / uvReduction.AllUvi;
              uvReduction.AllUVRedPct = (uvReduction.AdjUVI - uvReduction.AllUvi) / uvReduction.AdjUVI * 100.0;
            }
            uvReduction.AllUVRed = uvReduction.AdjUVI - uvReduction.AllUvi;
            uvReduction.AllUVRed = uvReduction.AllUVRed < 0.0 ? 0.0 : uvReduction.AllUVRed;
            uvReduction.AllUVRedPct = uvReduction.AllUVRedPct < 0.0 ? 0.0 : uvReduction.AllUVRedPct;
            uvrList.Add(uvReduction);
          }
        }
      }
      UVReduction.CreateUVReductionTable(outDB);
      UVReduction.WriteUVReductionRecords(outDB, uvrList);
    }

    private static double getTrRate(double skyCov)
    {
      double trRate = 0.0;
      if (skyCov == 0.0)
        trRate = 0.999;
      else if (skyCov == 3.75)
        trRate = 0.896;
      else if (skyCov == 7.5)
        trRate = 0.726;
      else if (skyCov == 10.0)
        trRate = 0.316;
      return trRate;
    }

    private static UVReduction.SKYCOV getSkyCov(double skyCov)
    {
      UVReduction.SKYCOV skyCov1 = UVReduction.SKYCOV.CLR;
      if (skyCov == 0.0)
        skyCov1 = UVReduction.SKYCOV.CLR;
      else if (skyCov == 3.75)
        skyCov1 = UVReduction.SKYCOV.SCT;
      else if (skyCov == 7.5)
        skyCov1 = UVReduction.SKYCOV.BKN;
      else if (skyCov == 10.0)
        skyCov1 = UVReduction.SKYCOV.OVC;
      return skyCov1;
    }

    public static void CreateCityUVReductionTable(string sDB)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        try
        {
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "CREATE TABLE TotalUVReduction([Date] DateTime,[Latitude] double,[Longitude] double,[CldCov] DOUBLE,[TreeCov] DOUBLE,[UVI] DOUBLE,[AdjUVI] DOUBLE,[SolZenRad] DOUBLE,[ShadedTs] DOUBLE,[ShadedUvi] DOUBLE,[ShadedUVPF] DOUBLE,[ShadedUVRed] DOUBLE,[ShadedUVRedPct] DOUBLE,[AllTs] DOUBLE,[AllUvi] DOUBLE,[AllUVPF] DOUBLE,[AllUVRed] DOUBLE,[AllUVRedPct] DOUBLE);";
          oleDbCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          throw;
        }
        finally
        {
          oleDbConnection.Close();
        }
      }
    }

    public static void WriteCityUVReductionRecords(string sDB, List<UVReduction> uvrList)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        try
        {
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          for (int index = 0; index < uvrList.Count; ++index)
          {
            oleDbCommand.CommandText = "INSERT INTO TotalUVReduction([Date], [Latitude], [Longitude], [CldCov], [TreeCov], [UVI], [AdjUVI], [SolZenRad],[ShadedTs], [ShadedUvi], [ShadedUVPF], [ShadedUVRed], [ShadedUVRedPct], [AllTs], [AllUvi], [AllUVPF], [AllUVRed], [AllUVRedPct]) Values (#" + uvrList[index].Date.ToString() + "#," + uvrList[index].Latitude.ToString() + "," + uvrList[index].Longitude.ToString() + "," + uvrList[index].CldCov.ToString() + "," + uvrList[index].TreeCov.ToString() + "," + uvrList[index].UVI.ToString() + "," + uvrList[index].AdjUVI.ToString() + "," + uvrList[index].SolZenRad.ToString() + "," + uvrList[index].ShadedTs.ToString() + "," + uvrList[index].ShadedUvi.ToString() + "," + uvrList[index].ShadedPf.ToString() + "," + uvrList[index].ShadedUVRed.ToString() + "," + uvrList[index].ShadedUVRedPct.ToString() + "," + uvrList[index].AllTs.ToString() + "," + uvrList[index].AllUvi.ToString() + "," + uvrList[index].AllPf.ToString() + "," + uvrList[index].AllUVRed.ToString() + "," + uvrList[index].AllUVRedPct.ToString() + ");";
            oleDbCommand.ExecuteNonQuery();
          }
        }
        catch (Exception ex)
        {
          throw;
        }
        finally
        {
          oleDbConnection.Close();
        }
      }
    }

    public static void CreateUVReductionTable(string sDB)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        try
        {
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "CREATE TABLE UVReduction([Landuse] TEXT(255),[Date] DateTime,[Latitude] double,[Longitude] double,[CldCov] DOUBLE,[TreeCov] DOUBLE,[UVI] DOUBLE,[AdjUVI] DOUBLE,[SolZenRad] DOUBLE,[ShadedTs] DOUBLE,[ShadedUvi] DOUBLE,[ShadedUVPF] DOUBLE,[ShadedUVRed] DOUBLE,[ShadedUVRedPct] DOUBLE,[AllTs] DOUBLE,[AllUvi] DOUBLE,[AllUVPF] DOUBLE,[AllUVRed] DOUBLE,[AllUVRedPct] DOUBLE);";
          oleDbCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          throw;
        }
        finally
        {
          oleDbConnection.Close();
        }
      }
    }

    public static void WriteUVReductionRecords(string sDB, List<UVReduction> uvrList)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        try
        {
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          for (int index = 0; index < uvrList.Count; ++index)
          {
            oleDbCommand.CommandText = "INSERT INTO UVReduction([Landuse], [Date], [Latitude], [Longitude], [CldCov], [TreeCov], [UVI], [AdjUVI], [SolZenRad],[ShadedTs], [ShadedUvi], [ShadedUVPF], [ShadedUVRed], [ShadedUVRedPct], [AllTs], [AllUvi], [AllUVPF], [AllUVRed], [AllUVRedPct]) Values (\"" + uvrList[index].Landuse + "\",#" + uvrList[index].Date.ToString() + "#," + uvrList[index].Latitude.ToString() + "," + uvrList[index].Longitude.ToString() + "," + uvrList[index].CldCov.ToString() + "," + uvrList[index].TreeCov.ToString() + "," + uvrList[index].UVI.ToString() + "," + uvrList[index].AdjUVI.ToString() + "," + uvrList[index].SolZenRad.ToString() + "," + uvrList[index].ShadedTs.ToString() + "," + uvrList[index].ShadedUvi.ToString() + "," + uvrList[index].ShadedPf.ToString() + "," + uvrList[index].ShadedUVRed.ToString() + "," + uvrList[index].ShadedUVRedPct.ToString() + "," + uvrList[index].AllTs.ToString() + "," + uvrList[index].AllUvi.ToString() + "," + uvrList[index].AllPf.ToString() + "," + uvrList[index].AllUVRed.ToString() + "," + uvrList[index].AllUVRedPct.ToString() + ");";
            oleDbCommand.ExecuteNonQuery();
          }
        }
        catch (Exception ex)
        {
          throw;
        }
        finally
        {
          oleDbConnection.Close();
        }
      }
    }

    public static void Summarize(string sDB)
    {
      UVReduction.MonthlyMeans(sDB);
      UVReduction.YearlyMeans(sDB);
    }

    private static void MonthlyMeans(string sDB)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        OleDbCommand oleDbCommand = new OleDbCommand();
        try
        {
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, Month([Date]) AS [Month], Avg(UVI) AS MeanUVI, Avg(AdjUVI) AS MeanAdjUVI, Avg(ShadedTs) AS MeanShadedTs, Avg(ShadedUvi) AS MeanShadedUvi, Avg(ShadedUVPF) AS MeanShadedUVPF, Avg(ShadedUVRed) AS MeanShadedUVRed, Avg(ShadedUVRedPct) AS MeanShadedUVRedPct, Avg(AllTs) AS MeanAllTs, Avg(AllUvi) AS MeanAllUvi, Avg(AllUVPF) AS MeanAllUVPF, Avg(AllUVRed) AS MeanAllUVRed, Avg(AllUVRedPct) AS MeanAllUVRedPct INTO MonthlyMeanUVReduction From UVReduction GROUP BY Landuse, Month([Date]) ORDER BY Month([Date]);";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "INSERT INTO MonthlyMeanUVReduction( [Month], Landuse, MeanUVI, MeanAdjUVI,MeanShadedTs, MeanShadedUvi, MeanShadedUVPF, MeanShadedUVRed, MeanShadedUVRedPct,MeanAllTs, MeanAllUvi, MeanAllUVPF, MeanAllUVRed, MeanAllUVRedPct ) SELECT DISTINCTROW Month([Date]) AS [Month], \"Total\", Avg(UVI), Avg(AdjUVI),Avg(ShadedTs), Avg(ShadedUvi), Avg(ShadedUVPF), Avg(ShadedUVRed), Avg(ShadedUVRedPct),Avg(AllTs), Avg(AllUvi), Avg(AllUVPF), Avg(AllUVRed), Avg(AllUVRedPct) FROM TotalUVReduction GROUP BY Month([Date]);";
          oleDbCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          throw;
        }
        finally
        {
          oleDbConnection.Close();
        }
      }
    }

    private static void YearlyMeans(string sDB)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        OleDbCommand oleDbCommand = new OleDbCommand();
        try
        {
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "SELECT DISTINCTROW Landuse, Avg(UVI) AS MeanUVI, Avg(AdjUVI) AS MeanAdjUVI, Avg(ShadedTs) AS MeanShadedTs, Avg(ShadedUvi) AS MeanShadedUvi, Avg(ShadedUVPF) AS MeanShadedUVPF, Avg(ShadedUVRed) AS MeanShadedUVRed, Avg(ShadedUVRedPct) AS MeanShadedUVRedPct, Avg(AllTs) AS MeanAllTs, Avg(AllUvi) AS MeanAllUvi, Avg(AllUVPF) AS MeanAllUVPF, Avg(AllUVRed) AS MeanAllUVRed, Avg(AllUVRedPct) AS MeanAllUVRedPct INTO YearlyMeanUVReduction From UVReduction GROUP BY Landuse;";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "INSERT INTO YearlyMeanUVReduction( Landuse, MeanUVI, MeanAdjUVI,MeanShadedTs, MeanShadedUvi, MeanShadedUVPF, MeanShadedUVRed, MeanShadedUVRedPct,MeanAllTs, MeanAllUvi, MeanAllUVPF, MeanAllUVRed, MeanAllUVRedPct ) SELECT DISTINCTROW  \"Total\", Avg(UVI), Avg(AdjUVI),Avg(ShadedTs), Avg(ShadedUvi), Avg(ShadedUVPF), Avg(ShadedUVRed), Avg(ShadedUVRedPct),Avg(AllTs), Avg(AllUvi), Avg(AllUVPF), Avg(AllUVRed), Avg(AllUVRedPct) FROM TotalUVReduction;";
          oleDbCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          throw;
        }
        finally
        {
          oleDbConnection.Close();
        }
      }
    }

    private enum SKYCOV
    {
      CLR,
      SCT,
      BKN,
      OVC,
    }
  }
}
