// Decompiled with JetBrains decompiler
// Type: UFORE.Location.LocationData
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Pollutant;

namespace UFORE.Location
{
  public class LocationData
  {
    public LocationData.PlaceInfo USA = new LocationData.PlaceInfo("United States of America", "001", 219);
    private LocationData.PlaceInfo[] US_NonContinentalStates = new LocationData.PlaceInfo[5]
    {
      new LocationData.PlaceInfo("Alaska", "02", 236),
      new LocationData.PlaceInfo("Hawaii", "15", 256),
      new LocationData.PlaceInfo("Puerto Rico", "72", 303),
      new LocationData.PlaceInfo("Guam", "66", 90),
      new LocationData.PlaceInfo("Virgin Islands", "78", 226)
    };
    private LocationData.PlaceInfo[] FourNations = new LocationData.PlaceInfo[7]
    {
      new LocationData.PlaceInfo("Canada", "002", 45),
      new LocationData.PlaceInfo("Australia", "230", 21),
      new LocationData.PlaceInfo("United Kingdom", "021", 218),
      new LocationData.PlaceInfo("Mexico", "107", 138),
      new LocationData.PlaceInfo("Columbia", "199", 52),
      new LocationData.PlaceInfo("Korea, Republic of", "132", 114),
      new LocationData.PlaceInfo("Japan", "138", 108)
    };
    private LocationData.PlaceInfo[] EuropeanNations = new LocationData.PlaceInfo[33]
    {
      new LocationData.PlaceInfo("Austria", "229", 22),
      new LocationData.PlaceInfo("Belgium", "222", 29),
      new LocationData.PlaceInfo("Bulgaria", "212", 40),
      new LocationData.PlaceInfo("Croatia", "192", 59),
      new LocationData.PlaceInfo("Cyprus", "190", 61),
      new LocationData.PlaceInfo("Czech Republic", "189", 62),
      new LocationData.PlaceInfo("Denmark", "188", 63),
      new LocationData.PlaceInfo("Estonia", "178", 73),
      new LocationData.PlaceInfo("Finland", "173", 77),
      new LocationData.PlaceInfo("France", "172", 78),
      new LocationData.PlaceInfo("Germany", "165", 84),
      new LocationData.PlaceInfo("Greece", "162", 86),
      new LocationData.PlaceInfo("Hungary", "148", 98),
      new LocationData.PlaceInfo("Iceland", "147", 99),
      new LocationData.PlaceInfo("Ireland", "142", 104),
      new LocationData.PlaceInfo("Italy", "140", 106),
      new LocationData.PlaceInfo("Latvia", "128", 118),
      new LocationData.PlaceInfo("Lithuania", "122", 124),
      new LocationData.PlaceInfo("Luxembourg", "121", 125),
      new LocationData.PlaceInfo("Macedonia", "119", (int) sbyte.MaxValue),
      new LocationData.PlaceInfo("Malta", "113", 133),
      new LocationData.PlaceInfo("Netherlands", "095", 150),
      new LocationData.PlaceInfo("Norway", "085", 160),
      new LocationData.PlaceInfo("Poland", "074", 171),
      new LocationData.PlaceInfo("Portugal", "073", 172),
      new LocationData.PlaceInfo("Republic of Montenegro", "265", 65435),
      new LocationData.PlaceInfo("Romania", "069", 175),
      new LocationData.PlaceInfo("Slovakia", "053", 189),
      new LocationData.PlaceInfo("Slovenia", "052", 190),
      new LocationData.PlaceInfo("Spain", "047", 194),
      new LocationData.PlaceInfo("Sweden", "040", 199),
      new LocationData.PlaceInfo("Switzerland", "039", 200),
      new LocationData.PlaceInfo("Turkey", "028", 211)
    };
    private LocationData.PlaceInfo Liechtenstein = new LocationData.PlaceInfo(nameof (Liechtenstein), "123", 123);
    private const string CO2CONC_TABLE = "_CO2Concentration";
    public const string PRI_NULL = "00";
    public const string SEC_NULL = "000";
    public const string TER_NULL = "00000";
    public const int NO_LEAF_ON = -1;
    public const int NO_LEAF_OFF = -1;
    public const int NO_ELEV = -99999;
    public const int NO_GMT_OFFSET = -99;
    public const string NO_STLAT = "";
    public const int DEF_LEAF_ON = 1;
    public const int DEF_LEAF_OFF = 365;
    public const int DEF_ELEV = 0;
    public const int DEF_GMT_OFFSET = 0;
    public const string DEF_STLAT = "0000";
    public string NationID;
    public string Nation;
    public string PriPartID;
    public string PriPart;
    public string SecPartID;
    public string SecPart;
    public string TerPartID;
    public string TerPart;
    public double Latitude;
    public double Longitude;
    public int LeafOffDOY;
    public int LeafOnDOY;
    public double Elevation;
    public double GMTOffset;
    public string StLatID;
    public List<double> ozone;
    public int AlbedoStID;
    public int albedo;
    public List<double> AlbedoVal;
    public double CoeffA;
    public double CoeffC;
    public double CoeffPHI;
    public double TerrainFactor;
    public Dictionary<string, List<PollutantMonitor>> PollMonDict;
    public Dictionary<string, PollutantCost> PollCostDict;
    public double ModelYearPPI;
    public int Population;
    public double AreaKm2;
    public double CO2Conc;

    public LocationData()
    {
      this.ozone = new List<double>();
      this.AlbedoVal = new List<double>();
      this.PollMonDict = new Dictionary<string, List<PollutantMonitor>>();
      this.PollCostDict = new Dictionary<string, PollutantCost>();
    }

    public bool isUsingBenMAPRegression()
    {
      if (this.NationID == this.USA.Code)
      {
        for (int index = 0; index < this.US_NonContinentalStates.Length; ++index)
        {
          if (this.PriPartID == this.US_NonContinentalStates[index].Code)
            return true;
        }
        return false;
      }
      for (int index = 0; index < this.FourNations.Length; ++index)
      {
        if (this.FourNations[index].Code == this.NationID)
          return true;
      }
      for (int index = 0; index < this.EuropeanNations.Length; ++index)
      {
        if (this.EuropeanNations[index].Code == this.NationID)
          return true;
      }
      return false;
    }

    public bool isUsingBenMAPresults()
    {
      if (this.NationID == this.USA.Code)
        return true;
      for (int index = 0; index < this.FourNations.Length; ++index)
      {
        if (this.NationID == this.FourNations[index].Code)
          return true;
      }
      for (int index = 0; index < this.EuropeanNations.Length; ++index)
      {
        if (this.NationID == this.EuropeanNations[index].Code)
          return true;
      }
      return false;
    }

    public bool isUsingActualBenMAP()
    {
      if (!(this.NationID == this.USA.Code))
        return false;
      for (int index = 0; index < this.US_NonContinentalStates.Length; ++index)
      {
        if (this.PriPartID == this.US_NonContinentalStates[index].Code)
          return false;
      }
      return true;
    }

    public bool isUSA_ContinentalState()
    {
      if (!(this.NationID == this.USA.Code))
        return false;
      for (int index = 0; index < this.US_NonContinentalStates.Length; ++index)
      {
        if (this.PriPartID == this.US_NonContinentalStates[index].Code)
          return false;
      }
      return true;
    }

    public bool IsUS() => this.NationID == this.USA.Code;

    public bool IsUSlikeNation()
    {
      for (int index = 0; index < this.FourNations.Length; ++index)
      {
        if (this.FourNations[index].Code == this.NationID)
          return true;
      }
      for (int index = 0; index < this.EuropeanNations.Length; ++index)
      {
        if (this.EuropeanNations[index].Code == this.NationID)
          return true;
      }
      return false;
    }

    public bool IsUSorUSlikeNation()
    {
      if (this.NationID == this.USA.Code)
        return true;
      for (int index = 0; index < this.FourNations.Length; ++index)
      {
        if (this.FourNations[index].Code == this.NationID)
          return true;
      }
      for (int index = 0; index < this.EuropeanNations.Length; ++index)
      {
        if (this.EuropeanNations[index].Code == this.NationID)
          return true;
      }
      return false;
    }

    [Obsolete]
    public void ReadPrimaryPartitionCoord(string sDB)
    {
      using (OleDbConnection cnLocDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sDB))
      {
        cnLocDB.Open();
        this.ReadPrimaryPartitionCoord(cnLocDB);
      }
    }

    public void ReadPrimaryPartitionCoord(OleDbConnection cnLocDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnLocDB;
        oleDbCommand.CommandText = "SELECT * FROM NationTable WHERE NationID=\"" + this.NationID + "\"";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.Nation = (string) oleDbDataReader["NationName"];
        }
        oleDbCommand.CommandText = "SELECT * FROM PrimaryPartitionTable WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\"";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.PriPart = (string) oleDbDataReader["PrimaryPartitionName"];
          this.Latitude = (double) oleDbDataReader["Latitude"];
          this.Longitude = (double) oleDbDataReader["Longitude"];
        }
      }
    }

    [Obsolete]
    public void ReadSecondaryPartitionCoord(string sDB)
    {
      using (OleDbConnection cnLocDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sDB))
      {
        cnLocDB.Open();
        this.ReadSecondaryPartitionCoord(cnLocDB);
      }
    }

    public void ReadSecondaryPartitionCoord(OleDbConnection cnLocDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnLocDB;
        oleDbCommand.CommandText = "SELECT * FROM NationTable WHERE NationID=\"" + this.NationID + "\"";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.Nation = (string) oleDbDataReader["NationName"];
        }
        oleDbCommand.CommandText = "SELECT * FROM PrimaryPartitionTable WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\"";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.PriPart = (string) oleDbDataReader["PrimaryPartitionName"];
        }
        oleDbCommand.CommandText = "SELECT * FROM SecondaryPartitionTable WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\" AND SecondaryPartitionID=\"" + this.SecPartID + "\"";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.SecPart = (string) oleDbDataReader["SecondaryPartitionName"];
          this.Latitude = (double) oleDbDataReader["Latitude"];
          this.Longitude = (double) oleDbDataReader["Longitude"];
        }
      }
    }

    [Obsolete]
    public void ReadTertiaryPartitionCoord(string sDB)
    {
      using (OleDbConnection cnLocDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sDB))
      {
        cnLocDB.Open();
        this.ReadTertiaryPartitionCoord(cnLocDB);
      }
    }

    public void ReadTertiaryPartitionCoord(OleDbConnection cnLocDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnLocDB;
        oleDbCommand.CommandText = "SELECT * FROM NationTable WHERE NationID=\"" + this.NationID + "\"";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.Nation = (string) oleDbDataReader["NationName"];
        }
        oleDbCommand.CommandText = "SELECT * FROM PrimaryPartitionTable WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\"";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.PriPart = (string) oleDbDataReader["PrimaryPartitionName"];
        }
        oleDbCommand.CommandText = "SELECT * FROM SecondaryPartitionTable WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\" AND SecondaryPartitionID=\"" + this.SecPartID + "\"";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.SecPart = (string) oleDbDataReader["SecondaryPartitionName"];
        }
        oleDbCommand.CommandText = "SELECT * FROM TertiaryPartitionTable WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\" AND SecondaryPartitionID=\"" + this.SecPartID + "\" AND TertiaryPartitionID=\"" + this.TerPartID + "\" ";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.TerPart = (string) oleDbDataReader["TertiaryPartitionName"];
          this.Latitude = (double) oleDbDataReader["Latitude"];
          this.Longitude = (double) oleDbDataReader["Longitude"];
        }
      }
    }

    [Obsolete]
    public void ReadPrimaryPartitionData(string sDB, int modelYear)
    {
      using (OleDbConnection cnLocDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sDB))
      {
        cnLocDB.Open();
        this.ReadPrimaryPartitionData(cnLocDB, modelYear);
      }
    }

    public void ReadPrimaryPartitionData(OleDbConnection cnLocDB, int modelYear)
    {
      this.ReadPrimaryPartitionCoord(cnLocDB);
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnLocDB;
        oleDbCommand.CommandText = "SELECT * FROM PrimaryPartitionAssociatedParamsTable WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\"";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LeafOffDOY = (int) (short) oleDbDataReader["LeafOffDOY"];
          this.LeafOnDOY = (int) (short) oleDbDataReader["LeafOnDOY"];
          this.Elevation = (double) oleDbDataReader["Elevation"];
          this.GMTOffset = (double) oleDbDataReader["GMTOffset"];
          this.StLatID = (string) oleDbDataReader["StateLatID"];
        }
      }
      this.ReadUpperPartitionData(cnLocDB, LocationData.PART.PRIMARY);
      this.ReadOzoneData(cnLocDB);
      this.ReadAlbedoData(cnLocDB);
      this.ReadPollMonData(cnLocDB, modelYear, "CO");
      this.ReadPollMonData(cnLocDB, modelYear, "NO2");
      this.ReadPollMonData(cnLocDB, modelYear, "O3");
      this.ReadPollMonData(cnLocDB, modelYear, "PM10");
      this.ReadPollMonData(cnLocDB, modelYear, "PM2.5");
      this.ReadPollMonData(cnLocDB, modelYear, "SO2");
      this.ReadPollCostData(cnLocDB, modelYear, "CO");
      this.ReadPollCostData(cnLocDB, modelYear, "NO2");
      this.ReadPollCostData(cnLocDB, modelYear, "O3");
      this.ReadPollCostData(cnLocDB, modelYear, "PM10");
      this.ReadPollCostData(cnLocDB, modelYear, "PM2.5");
      this.ReadPollCostData(cnLocDB, modelYear, "SO2");
      this.ReadPopulation(cnLocDB);
      this.ReadCO2Conc(cnLocDB, modelYear);
    }

    [Obsolete]
    public void ReadSecondaryPartitionData(string sDB, int modelYear)
    {
      using (OleDbConnection cnLocDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sDB))
      {
        cnLocDB.Open();
        this.ReadSecondaryPartitionData(cnLocDB, modelYear);
      }
    }

    public void ReadSecondaryPartitionData(OleDbConnection cnLocDB, int modelYear)
    {
      this.ReadSecondaryPartitionCoord(cnLocDB);
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnLocDB;
        oleDbCommand.CommandText = "SELECT * FROM SecondaryPartitionAssociatedParamsTable WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\" AND SecondaryPartitionID=\"" + this.SecPartID + "\"";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LeafOffDOY = (int) (short) oleDbDataReader["LeafOffDOY"];
          this.LeafOnDOY = (int) (short) oleDbDataReader["LeafOnDOY"];
          this.Elevation = (double) oleDbDataReader["Elevation"];
          this.GMTOffset = (double) oleDbDataReader["GMTOffset"];
          this.StLatID = (string) oleDbDataReader["StateLatID"];
          int ordinal = oleDbDataReader.GetOrdinal("Area");
          if (oleDbDataReader.GetValue(ordinal) != DBNull.Value)
            this.AreaKm2 = (double) oleDbDataReader["Area"] * Math.Pow(10.0, -6.0);
        }
      }
      this.ReadUpperPartitionData(cnLocDB, LocationData.PART.SECONDARY);
      this.ReadOzoneData(cnLocDB);
      this.ReadAlbedoData(cnLocDB);
      this.ReadPollMonData(cnLocDB, modelYear, "CO");
      this.ReadPollMonData(cnLocDB, modelYear, "NO2");
      this.ReadPollMonData(cnLocDB, modelYear, "O3");
      this.ReadPollMonData(cnLocDB, modelYear, "PM10");
      this.ReadPollMonData(cnLocDB, modelYear, "PM2.5");
      this.ReadPollMonData(cnLocDB, modelYear, "SO2");
      this.ReadPollCostData(cnLocDB, modelYear, "CO");
      this.ReadPollCostData(cnLocDB, modelYear, "NO2");
      this.ReadPollCostData(cnLocDB, modelYear, "O3");
      this.ReadPollCostData(cnLocDB, modelYear, "PM10");
      this.ReadPollCostData(cnLocDB, modelYear, "PM2.5");
      this.ReadPollCostData(cnLocDB, modelYear, "SO2");
      this.ReadPopulation(cnLocDB);
      this.ReadCO2Conc(cnLocDB, modelYear);
    }

    [Obsolete]
    public void ReadTertiaryPartitionData(string sDB, int modelYear)
    {
      using (OleDbConnection cnLocDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sDB))
      {
        cnLocDB.Open();
        this.ReadTertiaryPartitionData(cnLocDB, modelYear);
      }
    }

    public void ReadTertiaryPartitionData(OleDbConnection cnLocDB, int modelYear)
    {
      this.ReadTertiaryPartitionData(cnLocDB);
      this.ReadPollMonData(cnLocDB, modelYear, "CO");
      this.ReadPollMonData(cnLocDB, modelYear, "NO2");
      this.ReadPollMonData(cnLocDB, modelYear, "O3");
      this.ReadPollMonData(cnLocDB, modelYear, "PM10");
      this.ReadPollMonData(cnLocDB, modelYear, "PM2.5");
      this.ReadPollMonData(cnLocDB, modelYear, "SO2");
      this.ReadPollCostData(cnLocDB, modelYear, "CO");
      this.ReadPollCostData(cnLocDB, modelYear, "NO2");
      this.ReadPollCostData(cnLocDB, modelYear, "O3");
      this.ReadPollCostData(cnLocDB, modelYear, "PM10");
      this.ReadPollCostData(cnLocDB, modelYear, "PM2.5");
      this.ReadPollCostData(cnLocDB, modelYear, "SO2");
      this.ReadPopulation(cnLocDB);
      this.ReadCO2Conc(cnLocDB, modelYear);
    }

    public void ReadTertiaryPartitionData(OleDbConnection conn)
    {
      this.ReadTertiaryPartitionCoord(conn);
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT * FROM TertiaryPartitionAssociatedParamsTable WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\" AND SecondaryPartitionID=\"" + this.SecPartID + "\" AND TertiaryPartitionID=\"" + this.TerPartID + "\" ";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.LeafOffDOY = (int) (short) oleDbDataReader["LeafOffDOY"];
          this.LeafOnDOY = (int) (short) oleDbDataReader["LeafOnDOY"];
          this.Elevation = (double) oleDbDataReader["Elevation"];
          this.GMTOffset = (double) oleDbDataReader["GMTOffset"];
          this.StLatID = (string) oleDbDataReader["StateLatID"];
          int ordinal = oleDbDataReader.GetOrdinal("Area");
          if (oleDbDataReader.GetValue(ordinal) != DBNull.Value)
            this.AreaKm2 = (double) oleDbDataReader["Area"] * Math.Pow(10.0, -6.0);
        }
      }
      this.ReadUpperPartitionData(conn, LocationData.PART.TERTIARY);
      this.ReadOzoneData(conn);
      this.ReadAlbedoData(conn);
    }

    [Obsolete]
    public void ReadTertiaryPartitionData(string sDB)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sDB))
      {
        conn.Open();
        this.ReadTertiaryPartitionData(conn);
      }
    }

    public void ReadUpperPartitionData(OleDbConnection conn, LocationData.PART part)
    {
      if (this.LeafOffDOY == -1 && part == LocationData.PART.TERTIARY)
        this.ReadSecondaryPartitionData(conn, LocationData.PARAM.LFOFF);
      if (this.LeafOffDOY == -1 && (part == LocationData.PART.TERTIARY || part == LocationData.PART.SECONDARY))
        this.ReadPrimaryPartitionData(conn, LocationData.PARAM.LFOFF);
      if (this.LeafOffDOY == -1)
        this.LeafOffDOY = 365;
      if (this.LeafOnDOY == -1 && part == LocationData.PART.TERTIARY)
        this.ReadSecondaryPartitionData(conn, LocationData.PARAM.LFON);
      if (this.LeafOnDOY == -1 && (part == LocationData.PART.TERTIARY || part == LocationData.PART.SECONDARY))
        this.ReadPrimaryPartitionData(conn, LocationData.PARAM.LFON);
      if (this.LeafOnDOY == -1)
        this.LeafOnDOY = 1;
      if (this.Elevation == -99999.0 && part == LocationData.PART.TERTIARY)
        this.ReadSecondaryPartitionData(conn, LocationData.PARAM.ELEV);
      if (this.Elevation == -99999.0 && (part == LocationData.PART.TERTIARY || part == LocationData.PART.SECONDARY))
        this.ReadPrimaryPartitionData(conn, LocationData.PARAM.ELEV);
      if (this.Elevation == -99999.0)
        this.Elevation = 0.0;
      if (this.GMTOffset == -99.0 && part == LocationData.PART.TERTIARY)
        this.ReadSecondaryPartitionData(conn, LocationData.PARAM.GMT);
      if (this.GMTOffset == -99.0 && (part == LocationData.PART.TERTIARY || part == LocationData.PART.SECONDARY))
        this.ReadPrimaryPartitionData(conn, LocationData.PARAM.GMT);
      if (this.GMTOffset == -99.0)
        this.GMTOffset = 0.0;
      if (this.StLatID == "" && part == LocationData.PART.TERTIARY)
        this.ReadSecondaryPartitionData(conn, LocationData.PARAM.STLAT);
      if (this.StLatID == "" && (part == LocationData.PART.TERTIARY || part == LocationData.PART.SECONDARY))
        this.ReadPrimaryPartitionData(conn, LocationData.PARAM.STLAT);
      if (!(this.StLatID == ""))
        return;
      this.StLatID = "0000";
    }

    [Obsolete]
    public void ReadUpperPartitionData(string sDB, LocationData.PART part)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sDB))
      {
        conn.Open();
        this.ReadUpperPartitionData(conn, part);
      }
    }

    private void ReadPrimaryPartitionData(OleDbConnection conn, LocationData.PARAM prm)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT * FROM PrimaryPartitionAssociatedParamsTable WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\"";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          switch (prm)
          {
            case LocationData.PARAM.LFON:
              this.LeafOnDOY = (int) (short) oleDbDataReader["LeafOnDOY"];
              break;
            case LocationData.PARAM.LFOFF:
              this.LeafOffDOY = (int) (short) oleDbDataReader["LeafOffDOY"];
              break;
            case LocationData.PARAM.ELEV:
              this.Elevation = (double) oleDbDataReader["Elevation"];
              break;
            case LocationData.PARAM.GMT:
              this.GMTOffset = (double) oleDbDataReader["GMTOffset"];
              break;
            default:
              this.StLatID = (string) oleDbDataReader["StateLatID"];
              break;
          }
        }
      }
    }

    [Obsolete]
    private void ReadPrimaryPartitionData(string sDB, LocationData.PARAM prm)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sDB))
      {
        conn.Open();
        this.ReadPrimaryPartitionData(conn, prm);
      }
    }

    private void ReadSecondaryPartitionData(OleDbConnection conn, LocationData.PARAM prm)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT * FROM SecondaryPartitionAssociatedParamsTable WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\" AND SecondaryPartitionID=\"" + this.SecPartID + "\"";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          switch (prm)
          {
            case LocationData.PARAM.LFON:
              this.LeafOnDOY = (int) (short) oleDbDataReader["LeafOnDOY"];
              break;
            case LocationData.PARAM.LFOFF:
              this.LeafOffDOY = (int) (short) oleDbDataReader["LeafOffDOY"];
              break;
            case LocationData.PARAM.ELEV:
              this.Elevation = (double) oleDbDataReader["Elevation"];
              break;
            case LocationData.PARAM.GMT:
              this.GMTOffset = (double) oleDbDataReader["GMTOffset"];
              break;
            default:
              this.StLatID = (string) oleDbDataReader["StateLatID"];
              break;
          }
        }
      }
    }

    [Obsolete]
    private void ReadSecondaryPartitionData(string sDB, LocationData.PARAM prm)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sDB))
      {
        conn.Open();
        this.ReadSecondaryPartitionData(conn, prm);
      }
    }

    public void ReadOzoneData(OleDbConnection conn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT * FROM Ozone WHERE Ozone.StateLatID=\"" + this.StLatID + "\" ORDER BY Ozone.[MonthID]";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
            this.ozone.Add((double) oleDbDataReader["OzoneVal"]);
        }
      }
    }

    public void ReadAlbedoData(OleDbConnection conn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT * FROM AlbedoStationsAssigned WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\" AND SecondaryPartitionID=\"" + this.SecPartID + "\" AND TertiaryPartitionID=\"" + this.TerPartID + "\" ";
        using (OleDbDataReader oleDbDataReader1 = oleDbCommand.ExecuteReader())
        {
          if (oleDbDataReader1.Read())
          {
            this.AlbedoStID = (int) oleDbDataReader1["StationID"];
            oleDbDataReader1.Close();
          }
          else
          {
            oleDbDataReader1.Close();
            oleDbCommand.CommandText = "SELECT * FROM AlbedoStationsAssigned WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\" AND SecondaryPartitionID=\"" + this.SecPartID + "\" AND TertiaryPartitionID=\"00000\" ";
            using (OleDbDataReader oleDbDataReader2 = oleDbCommand.ExecuteReader())
            {
              if (oleDbDataReader2.Read())
              {
                this.AlbedoStID = (int) oleDbDataReader2["StationID"];
                if (oleDbDataReader2.Read())
                {
                  oleDbDataReader2.Close();
                  throw new Exception("Albedo station is missing for Nation:" + this.NationID + ", PrimaryPartiton:" + this.PriPartID + ", SecondaryPartition:" + this.SecPartID + ", ThirdParitition:" + this.TerPartID + ". And more than one albedo station is assigned to secondary partition.");
                }
                oleDbDataReader2.Close();
              }
              else
              {
                oleDbDataReader2.Close();
                throw new Exception("Albedo station is missing for Nation:" + this.NationID + ", PrimaryPartiton:" + this.PriPartID + ", SecondaryPartition:" + this.SecPartID + ", ThirdParitition:" + this.TerPartID + " and 00000");
              }
            }
          }
        }
        oleDbCommand.CommandText = "SELECT * FROM AlbedoStations WHERE StationID=" + this.AlbedoStID.ToString();
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.albedo = (int) (short) oleDbDataReader["albedo"];
          this.CoeffA = (double) oleDbDataReader["CoeffA"];
          this.CoeffC = (double) oleDbDataReader["CoeffC"];
          this.CoeffPHI = (double) oleDbDataReader["CoeffPHI"];
        }
        oleDbCommand.CommandText = "SELECT * FROM AlbedoTable WHERE Albedo=" + this.albedo.ToString() + " ORDER BY MonthID";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
            this.AlbedoVal.Add((double) oleDbDataReader["Albedo_Val"]);
        }
      }
    }

    public void ReadPollMonData(OleDbConnection conn, int year, string poll)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        List<PollutantMonitor> pollutantMonitorList = new List<PollutantMonitor>();
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT ASN.NationID, ASN.PrimaryPartitionID, ASN.SecondaryPartitionID,ASN.TertiaryPartitionID, ASN.Pollutant, ASN.MonYear,MON.PrimaryPartitionID AS MonState, MON.SecondaryPartitionID AS MonCounty,MON.SiteID, MON.Latitude, MON.Longitude FROM PollutantMonStationsAssigned AS ASN INNER JOIN PollutantMonStations AS MON ON ASN.[StationID] = MON.[StationID] WHERE ASN.NationID=\"" + this.NationID + "\" AND ASN.PrimaryPartitionID=\"" + this.PriPartID + "\" AND ASN.SecondaryPartitionID=\"" + this.SecPartID + "\" AND ASN.TertiaryPartitionID=\"" + this.TerPartID + "\" AND ASN.Pollutant=\"" + poll + "\" AND ASN.MonYear=" + year.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
            pollutantMonitorList.Add(new PollutantMonitor()
            {
              MonState = (string) oleDbDataReader["MonState"],
              MonCounty = (string) oleDbDataReader["MonCounty"],
              MonSiteID = (string) oleDbDataReader["SiteID"],
              MonLat = (double) oleDbDataReader["Latitude"],
              MonLong = (double) oleDbDataReader["Longitude"]
            });
        }
        this.PollMonDict.Add(poll, pollutantMonitorList);
      }
    }

    public void ReadPollCostData(OleDbConnection conn, int year, string poll)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        PollutantCost pollutantCost = new PollutantCost();
        oleDbCommand.Connection = conn;
        string str = this.NationID;
        if (!this.IsUSorUSlikeNation())
          str = this.USA.Code;
        bool flag1 = false;
        oleDbCommand.CommandText = "SELECT * FROM PollutantBaseCost  WHERE NationID=\"" + str + "\" AND Pollutant=\"" + poll + "\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            pollutantCost.BaseCost = (double) oleDbDataReader["BaseCost"];
            pollutantCost.BaseYear = (int) oleDbDataReader["BaseYear"];
            flag1 = true;
          }
        }
        int num1 = flag1 ? 1 : 0;
        oleDbCommand.CommandText = "SELECT * FROM PollutantPPIAdjustment  WHERE NationID=\"" + str + "\" AND PPIYear=" + pollutantCost.BaseYear.ToString() + ";";
        bool flag2 = false;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            pollutantCost.BaseYearPPI = (double) oleDbDataReader["PPIValue"];
            flag2 = true;
          }
        }
        int num2 = flag2 ? 1 : 0;
        this.PollCostDict.Add(poll, pollutantCost);
        oleDbCommand.CommandText = "SELECT * FROM PollutantPPIAdjustment  WHERE NationID=\"" + str + "\" AND PPIYear=" + year.ToString() + ";";
        bool flag3 = false;
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            this.ModelYearPPI = (double) oleDbDataReader["PPIValue"];
            flag3 = true;
          }
        }
        int num3 = flag3 ? 1 : 0;
      }
    }

    private void ReadPopulation(OleDbConnection conn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        if (this.NationID == this.USA.Code)
        {
          oleDbCommand.CommandText = "SELECT [Total population (all ages)] AS Pop FROM [Population2010] WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\" AND SecondaryPartitionID=\"" + this.SecPartID + "\" AND TertiaryPartitionID=\"" + this.TerPartID + "\";";
          using (OleDbDataReader oleDbDataReader1 = oleDbCommand.ExecuteReader())
          {
            if (oleDbDataReader1.Read())
            {
              this.Population = (int) (double) oleDbDataReader1["Pop"];
            }
            else
            {
              oleDbDataReader1.Close();
              oleDbCommand.CommandText = "SELECT [_Locations].TotalPopulation AS Pop FROM TertiaryPartitionTable INNER JOIN [_Locations] ON TertiaryPartitionTable.LocationId = [_Locations].LocationId WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\" AND SecondaryPartitionID=\"" + this.SecPartID + "\" AND TertiaryPartitionID=\"" + this.TerPartID + "\";";
              using (OleDbDataReader oleDbDataReader2 = oleDbCommand.ExecuteReader())
              {
                oleDbDataReader2.Read();
                if (oleDbDataReader2["Pop"] == DBNull.Value)
                  return;
                this.Population = (int) oleDbDataReader2["Pop"];
              }
            }
          }
        }
        else
        {
          if (!this.IsUSlikeNation())
            return;
          oleDbCommand.CommandText = "SELECT [_Locations].TotalPopulation AS Pop FROM TertiaryPartitionTable INNER JOIN [_Locations] ON TertiaryPartitionTable.LocationId = [_Locations].LocationId WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\" AND SecondaryPartitionID=\"" + this.SecPartID + "\" AND TertiaryPartitionID=\"" + this.TerPartID + "\";";
          using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
          {
            oleDbDataReader.Read();
            if (oleDbDataReader["Pop"] == DBNull.Value)
              return;
            this.Population = (int) oleDbDataReader["Pop"];
          }
        }
      }
    }

    private void ReadCO2Conc(OleDbConnection conn, int yr)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT [MeanCO2] FROM [_CO2Concentration] WHERE [Year]=" + yr.ToString() + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          if (!oleDbDataReader.Read())
            return;
          this.CO2Conc = (double) oleDbDataReader["MeanCO2"];
        }
      }
    }

    public class PlaceInfo
    {
      public string Name = "";
      public string Code = "";
      public int LocId;

      public PlaceInfo(string aName, string aCode, int anId)
      {
        this.Name = aName;
        this.Code = aCode;
        this.LocId = anId;
      }
    }

    public enum PARAM
    {
      LFON,
      LFOFF,
      ELEV,
      GMT,
      STLAT,
    }

    public enum PART
    {
      TERTIARY,
      SECONDARY,
      PRIMARY,
    }
  }
}
