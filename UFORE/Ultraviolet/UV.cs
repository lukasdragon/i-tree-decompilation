// Decompiled with JetBrains decompiler
// Type: UFORE.Ultraviolet.UV
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.BioEmission;
using UFORE.Location;
using UFORE.Weather;

namespace UFORE.Ultraviolet
{
  public class UV
  {
    private const string UVI_TABLE = "UVI";
    public string LocDB;
    public string ModelDomain;
    public string NationID;
    public string PriPartID;
    public string SecPartID;
    public string TerPartID;
    public double MeasureHt;
    public string WeatherDB;
    public string UviDB;
    public string AceDB;
    public string LanduseCoverTable;
    public string UVRedDB;

    public UV(
      string sLocDB,
      string sDomain,
      string sNationID,
      string sPriPartID,
      string sSecPartID,
      string sTerPartID,
      string sWeatherDB,
      string sUVIDB,
      string sTCDB,
      string sLanduseTbl,
      string sOutDB)
    {
      this.LocDB = sLocDB;
      this.ModelDomain = sDomain;
      this.NationID = sNationID;
      this.PriPartID = sPriPartID;
      this.SecPartID = sSecPartID;
      this.TerPartID = sTerPartID;
      this.WeatherDB = sWeatherDB;
      this.AceDB = sTCDB;
      this.LanduseCoverTable = sLanduseTbl;
      this.UviDB = sUVIDB;
      this.UVRedDB = sOutDB;
    }

    public void Run()
    {
      List<SurfaceWeather> surfaceWeatherList = new List<SurfaceWeather>();
      List<UVI> uviList1 = new List<UVI>();
      List<UVI> uviList2 = new List<UVI>();
      List<string> landuse = new List<string>();
      Dictionary<string, Landuse> luDict = new Dictionary<string, Landuse>();
      LocationData locationData = new LocationData();
      locationData.NationID = this.NationID;
      locationData.PriPartID = this.PriPartID;
      locationData.SecPartID = this.SecPartID;
      locationData.TerPartID = this.TerPartID;
      string modelDomain = this.ModelDomain;
      if (!(modelDomain == "PriPart"))
      {
        if (!(modelDomain == "SecPart"))
        {
          if (modelDomain == "TerPart")
            locationData.ReadTertiaryPartitionCoord(this.LocDB);
        }
        else
          locationData.ReadSecondaryPartitionCoord(this.LocDB);
      }
      else
        locationData.ReadPrimaryPartitionCoord(this.LocDB);
      try
      {
        using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.WeatherDB))
        {
          conn.Open();
          SurfaceWeather.ReadSurfaceAllRecordsHr(conn, 12, ref surfaceWeatherList);
        }
        double adjCoord1;
        this.RoundCoordinates(locationData.Latitude, out adjCoord1);
        double adjCoord2;
        this.RoundCoordinates(locationData.Longitude, out adjCoord2);
        if (!string.IsNullOrEmpty(this.UviDB))
          UVI.ReadUVIRecords(this.UviDB, "UVI", adjCoord1, adjCoord2, ref uviList1);
        if (uviList1.Count == 0)
          UVI.SetNullUVIData(surfaceWeatherList[0].TimeStamp.Year, ref uviList1);
        UV.AdjustDates(ref surfaceWeatherList, ref uviList1, ref uviList2);
        Landuse.ReadLanduseData(this.AceDB, this.LanduseCoverTable, ref landuse, ref luDict);
        double tc;
        Landuse.ReadCityTreeCover(this.AceDB, this.LanduseCoverTable, out tc);
        AccessFunc.CreateDB(this.UVRedDB);
        UVReduction.ProcessUVReduction(ref surfaceWeatherList, ref uviList2, ref landuse, ref luDict, adjCoord1, adjCoord2, this.UVRedDB);
        UVReduction.ProcessUVReduction(ref surfaceWeatherList, ref uviList2, tc, adjCoord1, adjCoord2, this.UVRedDB);
        UVReduction.Summarize(this.UVRedDB);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void RoundCoordinates(double orgCoord, out double adjCoord)
    {
      bool flag = orgCoord < 0.0;
      double d = Math.Abs(orgCoord);
      double num1 = Math.Truncate(d);
      double num2 = d - num1;
      adjCoord = 0.0;
      if (num2 >= 0.0 && num2 < 0.5)
        adjCoord = num1 + 0.25;
      else if (num2 >= 0.5 && num2 < 1.0)
        adjCoord = num1 + 0.75;
      adjCoord *= flag ? -1.0 : 1.0;
    }

    private static void AdjustDates(
      ref List<SurfaceWeather> sfc,
      ref List<UVI> uviOrg,
      ref List<UVI> uvi)
    {
      int num = 0;
      for (int index = 0; index < uviOrg.Count; ++index)
      {
        if (sfc[0].TimeStamp == uviOrg[index].TimeStamp)
        {
          num = index;
          break;
        }
      }
      for (int index = 0; index < sfc.Count; ++index)
        uvi.Add(uviOrg[num + index]);
    }
  }
}
