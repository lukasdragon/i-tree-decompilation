// Decompiled with JetBrains decompiler
// Type: UFORE.Deposition.UFORE_D
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading;
using UFORE.LAI;
using UFORE.Location;
using UFORE.MixingHeight;
using UFORE.Pollutant;
using UFORE.Weather;

namespace UFORE.Deposition
{
  public class UFORE_D
  {
    public int StartYear;
    public int EndYear;
    public int ModelYear;
    public DryDeposition.VEG_TYPE VegType;
    public string ModelDomain;
    public ForestData.RURAL_URBAN RuralUrban;
    public string LocDB;
    public string NationIDSelected;
    public string PriPartIDSelected;
    public string SecPartIDSelected;
    public string TerPartIDSelected;
    public int CityPopulationUserEntered;
    public double PopulationDensityUserEntered;
    public double Area;
    public double Lai;
    public double EvergreenPct;
    public double TreeCovPct;
    public double GrassCovPct;
    public double WildGrassCovPct;
    public double HerbCovPct;
    public string GrLaiDB;
    public string LaiDB;
    public string RainFile;
    public string SurfaceWeatherFile;
    public string WeatherDB;
    public string UpperAirFile;
    public string COMasterDB;
    public string NO2MasterDB;
    public string O3MasterDB;
    public string PM10MasterDB;
    public string PM25MasterDB;
    public string SO2MasterDB;
    public string PollSiteDB;
    public string DryDepDB;
    public string FinDryDepDB;
    public string BenMAPInputDB;
    public string BenMAPResultDB;
    public string FinBenMAPDB;
    private CancellationToken uforeCancellationToken;
    private IProgress<EngineProgressArg> uforeProgress;
    private EngineProgressArg uforeProgressArg;

    public UFORE_D(
      CancellationToken passinCancellationToken,
      IProgress<EngineProgressArg> passinProgress,
      EngineProgressArg passProgressArg)
    {
      this.uforeCancellationToken = passinCancellationToken;
      this.uforeProgress = passinProgress;
      this.uforeProgressArg = passProgressArg;
    }

    public UFORE_D(
      int iStYear,
      int iEndYear,
      int iModYear,
      ForestData.RURAL_URBAN ru,
      DryDeposition.VEG_TYPE sVeg,
      string sDomain,
      string sLocDB,
      string sNationID,
      string sPriPartID,
      string sSecPartID,
      string sTerPartID,
      int iCityPopulationUserEntered,
      double dPopulationDensityUserEntered,
      double dArea,
      double dLai,
      double dEvgPct,
      double dTrCovPct,
      double dGrCovPct,
      double dWdGrCovPct,
      double dHrCovPct,
      string sGrLaiDB,
      string sLaiDB,
      string sSurFile,
      string sUpFile,
      string sRainFile,
      string sWeaDB,
      string sCODB,
      string sNO2DB,
      string sO3DB,
      string sPM10DB,
      string sPM25DB,
      string sSO2DB,
      string sPollDB,
      string sDepDB,
      string sBmInDB,
      string sBmResDB,
      string sFinalDDB,
      string sFinalBMDB,
      CancellationToken passinCancellationToken,
      IProgress<EngineProgressArg> passinProgress,
      EngineProgressArg passProgressArg)
      : this(iStYear, iEndYear, iModYear, ru, sVeg, sDomain, sLocDB, sNationID, sPriPartID, sSecPartID, sTerPartID, iCityPopulationUserEntered, dPopulationDensityUserEntered, dArea, dLai, dEvgPct, dTrCovPct, dGrCovPct, dWdGrCovPct, dHrCovPct, sGrLaiDB, sLaiDB, sSurFile, sUpFile, sRainFile, sWeaDB, sCODB, sNO2DB, sO3DB, sPM10DB, sPM25DB, sSO2DB, sPollDB, sDepDB, sBmInDB, sBmResDB, sFinalDDB, sFinalBMDB)
    {
      this.uforeCancellationToken = passinCancellationToken;
      this.uforeProgress = passinProgress;
      this.uforeProgressArg = passProgressArg;
    }

    public void reportProgress(int percent)
    {
      if (this.uforeProgress == null)
        return;
      if (this.uforeCancellationToken.IsCancellationRequested)
        throw new Exception("User cancelled");
      if (percent < this.uforeProgressArg.Percent + 1)
        return;
      this.uforeProgressArg.Percent = percent <= 100 ? (percent >= 0 ? percent : 0) : 100;
      this.uforeProgress.Report(this.uforeProgressArg);
    }

    public UFORE_D()
    {
    }

    public UFORE_D(
      int iStYear,
      int iEndYear,
      int iModYear,
      ForestData.RURAL_URBAN ru,
      DryDeposition.VEG_TYPE sVeg,
      string sDomain,
      string sLocDB,
      string sNationID,
      string sPriPartID,
      string sSecPartID,
      string sTerPartID,
      int iCityPopulationUserEntered,
      double dPopulationDensityUserEntered,
      double dArea,
      double dLai,
      double dEvgPct,
      double dTrCovPct,
      double dGrCovPct,
      double dWdGrCovPct,
      double dHrCovPct,
      string sGrLaiDB,
      string sLaiDB,
      string sSurFile,
      string sUpFile,
      string sRainFile,
      string sWeaDB,
      string sCODB,
      string sNO2DB,
      string sO3DB,
      string sPM10DB,
      string sPM25DB,
      string sSO2DB,
      string sPollDB,
      string sDepDB,
      string sBmInDB,
      string sBmResDB,
      string sFinalDDB,
      string sFinalBMDB)
    {
      this.StartYear = iStYear;
      this.EndYear = iEndYear;
      this.ModelYear = iModYear;
      this.VegType = sVeg;
      this.ModelDomain = sDomain;
      this.RuralUrban = ru;
      this.LocDB = sLocDB;
      this.NationIDSelected = sNationID;
      this.PriPartIDSelected = sPriPartID;
      this.SecPartIDSelected = sSecPartID;
      this.TerPartIDSelected = sTerPartID;
      this.CityPopulationUserEntered = iCityPopulationUserEntered;
      this.PopulationDensityUserEntered = dPopulationDensityUserEntered;
      this.Area = dArea;
      this.Lai = dLai;
      this.EvergreenPct = dEvgPct;
      this.TreeCovPct = dTrCovPct;
      this.GrassCovPct = dGrCovPct;
      this.WildGrassCovPct = dWdGrCovPct;
      this.HerbCovPct = dHrCovPct;
      this.GrLaiDB = sGrLaiDB;
      this.LaiDB = sLaiDB;
      this.RainFile = sRainFile;
      this.SurfaceWeatherFile = sSurFile;
      this.UpperAirFile = sUpFile;
      this.WeatherDB = sWeaDB;
      this.COMasterDB = sCODB;
      this.NO2MasterDB = sNO2DB;
      this.O3MasterDB = sO3DB;
      this.PM10MasterDB = sPM10DB;
      this.PM25MasterDB = sPM25DB;
      this.SO2MasterDB = sSO2DB;
      this.PollSiteDB = sPollDB;
      this.DryDepDB = sDepDB;
      this.BenMAPInputDB = sBmInDB;
      this.BenMAPResultDB = sBmResDB;
      this.FinDryDepDB = sFinalDDB;
      this.FinBenMAPDB = sFinalBMDB;
    }

    public void RunUFORE_D()
    {
      LocationData locData = new LocationData();
      DryDeposition.SetVegType(this.VegType);
      this.reportProgress(0);
      locData.NationID = this.NationIDSelected;
      locData.PriPartID = this.PriPartIDSelected;
      locData.SecPartID = this.SecPartIDSelected;
      locData.TerPartID = this.TerPartIDSelected;
      string modelDomain = this.ModelDomain;
      if (!(modelDomain == "PriPart"))
      {
        if (!(modelDomain == "SecPart"))
        {
          if (modelDomain == "TerPart")
            locData.ReadTertiaryPartitionData(this.LocDB, this.ModelYear);
        }
        else
          locData.ReadSecondaryPartitionData(this.LocDB, this.ModelYear);
      }
      else
        locData.ReadPrimaryPartitionData(this.LocDB, this.ModelYear);
      ForestData forData;
      if (this.VegType == DryDeposition.VEG_TYPE.GRASS)
      {
        if (locData.isUSA_ContinentalState())
        {
          this.TreeCovPct = this.GrassCovPct + this.WildGrassCovPct + this.HerbCovPct;
          this.EvergreenPct = 100.0;
          forData = new ForestData(this.Area, this.Lai, this.EvergreenPct, this.TreeCovPct);
          Dictionary<string, string> ddd = new Dictionary<string, string>();
          UFORE_D.CreateDict(ddd);
          string st;
          ddd.TryGetValue(locData.PriPartID, out st);
          LeafAreaIndex.ProcessGrassLAI(this.ModelYear, st, this.GrLaiDB, this.GrassCovPct, this.WildGrassCovPct, this.HerbCovPct, this.LaiDB);
        }
        else
        {
          forData = new ForestData(this.Area, 0.0, 0.0, 0.0);
          LeafAreaIndex.ProcessLAI(forData.MaxLAI, forData.PctEvGrnCov, locData, this.StartYear, this.EndYear, this.LaiDB, this, 0, 4);
        }
      }
      else
      {
        forData = new ForestData(this.Area, this.Lai, this.EvergreenPct, this.TreeCovPct);
        LeafAreaIndex.ProcessLAI(forData.MaxLAI, forData.PctEvGrnCov, locData, this.StartYear, this.EndYear, this.LaiDB, this, 0, 4);
      }
      this.reportProgress(4);
      AccessFunc.CreateDB(this.WeatherDB);
      AccessFunc.CreateDB(this.PollSiteDB);
      AccessFunc.CreateDB(this.DryDepDB);
      using (OleDbConnection cnLaiDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.LaiDB))
      {
        using (OleDbConnection oleDbConnection1 = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.WeatherDB))
        {
          using (OleDbConnection oleDbConnection2 = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.PollSiteDB))
          {
            using (OleDbConnection oleDbConnection3 = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.DryDepDB))
            {
              cnLaiDB.Open();
              oleDbConnection1.Open();
              oleDbConnection2.Open();
              oleDbConnection3.Open();
              SurfaceWeather.ProcessSurfaceWeatherData(this.SurfaceWeatherFile, locData, cnLaiDB, this.VegType, this.RainFile, this.StartYear, this.EndYear, oleDbConnection1);
              if (!string.IsNullOrEmpty(this.UpperAirFile))
              {
                this.reportProgress(8);
                UpperAir.ProcessUpperAirData(this.UpperAirFile, oleDbConnection1, this, 8, 27);
                this.reportProgress(27);
                MixHt.CalcMixHts(oleDbConnection1, locData, this, 27, 43);
              }
              this.reportProgress(43);
              AirPollutant.ProcessAirPollutant(this.COMasterDB, this.NO2MasterDB, this.O3MasterDB, this.PM10MasterDB, this.PM25MasterDB, this.SO2MasterDB, oleDbConnection2, locData, oleDbConnection1, this, 43, 65);
              this.reportProgress(65);
              DryDeposition.ProcessDryDeposition(locData, forData, oleDbConnection1, cnLaiDB, oleDbConnection2, this.RuralUrban, oleDbConnection3, this.ModelDomain, this, 65, 95);
              this.reportProgress(95);
              DryDeposition.SummarizeDryDeposition(oleDbConnection3, this.ModelDomain);
              this.reportProgress(96);
              if (locData.isUsingActualBenMAP())
              {
                this.reportProgress(97);
                AirPollutantMetrics.CalcAirPollutantMetrics(locData, oleDbConnection3);
                AccessFunc.CreateDB(this.BenMAPResultDB);
                using (OleDbConnection cnLocDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.LocDB))
                {
                  using (OleDbConnection cnBmDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.BenMAPInputDB))
                  {
                    using (OleDbConnection cnResDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.BenMAPResultDB))
                    {
                      cnLocDB.Open();
                      cnBmDB.Open();
                      cnResDB.Open();
                      BenMAP.CalcBenMAPResult(this.ModelDomain, cnBmDB, cnLocDB, locData, oleDbConnection3, cnResDB);
                    }
                  }
                }
                this.reportProgress(98);
                this.CreateBenMapFinalDB();
                if (this.CityPopulationUserEntered != 0 && this.CityPopulationUserEntered != locData.Population)
                {
                  using (OleDbConnection oleDbConnection4 = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.FinBenMAPDB))
                  {
                    using (OleDbCommand oleDbCommand = new OleDbCommand())
                    {
                      oleDbConnection4.Open();
                      double num = (double) this.CityPopulationUserEntered / (double) locData.Population;
                      oleDbCommand.Connection = oleDbConnection4;
                      oleDbCommand.CommandText = "UPDATE BenMapTable SET NO2Incidence = NO2Incidence*@ratio, NO2Value = NO2Value*@ratio WHERE NO2Incidence>0";
                      oleDbCommand.Parameters.Clear();
                      oleDbCommand.Parameters.Add("@ratio", OleDbType.Double).Value = (object) num;
                      oleDbCommand.ExecuteNonQuery();
                      oleDbCommand.CommandText = "UPDATE BenMapTable SET SO2Incidence = SO2Incidence * @ratio, SO2Value = SO2Value * @ratio WHERE SO2Incidence>0";
                      oleDbCommand.Parameters.Clear();
                      oleDbCommand.Parameters.Add("@ratio", OleDbType.Double).Value = (object) num;
                      oleDbCommand.ExecuteNonQuery();
                      oleDbCommand.CommandText = "UPDATE BenMapTable SET O3Incidence = O3Incidence * @ratio, O3Value = O3Value * @ratio WHERE O3Incidence>0";
                      oleDbCommand.Parameters.Clear();
                      oleDbCommand.Parameters.Add("@ratio", OleDbType.Double).Value = (object) num;
                      oleDbCommand.ExecuteNonQuery();
                      oleDbCommand.CommandText = "UPDATE BenMapTable SET PM25Incidence = PM25Incidence * @ratio, PM25Value = PM25Value *@ratio WHERE PM25Incidence>0";
                      oleDbCommand.Parameters.Clear();
                      oleDbCommand.Parameters.Add("@ratio", OleDbType.Double).Value = (object) num;
                      oleDbCommand.ExecuteNonQuery();
                      oleDbConnection4.Close();
                    }
                  }
                }
              }
              else if (locData.isUsingBenMAPRegression())
              {
                this.reportProgress(98);
                if (this.PopulationDensityUserEntered != 0.0)
                  BenMAP.RegressValues(this.PopulationDensityUserEntered, forData.TrCovArea, oleDbConnection3);
                else if (this.CityPopulationUserEntered == 0)
                  BenMAP.RegressValues((double) locData.Population, locData.AreaKm2, forData.TrCovArea, oleDbConnection3);
                else if (locData.NationID == "199" && locData.AreaKm2 == 0.0)
                  BenMAP.RegressValues((double) this.CityPopulationUserEntered, 1.0, forData.TrCovArea, oleDbConnection3);
                else
                  BenMAP.RegressValues((double) this.CityPopulationUserEntered, locData.AreaKm2, forData.TrCovArea, oleDbConnection3);
              }
              this.reportProgress(99);
              this.CreateFinalDB();
              this.reportProgress(100);
            }
          }
        }
      }
    }

    private static void CreateDict(Dictionary<string, string> ddd)
    {
      ddd.Add("01", "Alabama");
      ddd.Add("04", "Arizona");
      ddd.Add("05", "Arkansas");
      ddd.Add("06", "California");
      ddd.Add("08", "Colorado");
      ddd.Add("09", "Connecticut");
      ddd.Add("10", "Delaware");
      ddd.Add("11", "DC");
      ddd.Add("12", "Florida");
      ddd.Add("13", "Georgia");
      ddd.Add("16", "Idaho");
      ddd.Add("17", "Illinois");
      ddd.Add("18", "Indiana");
      ddd.Add("19", "Iowa");
      ddd.Add("20", "Kansas");
      ddd.Add("21", "Kentucky");
      ddd.Add("22", "Louisiana");
      ddd.Add("23", "Maine");
      ddd.Add("24", "Maryland");
      ddd.Add("25", "Massachussets");
      ddd.Add("26", "Michigan");
      ddd.Add("27", "Minnessota");
      ddd.Add("28", "Mississippi");
      ddd.Add("29", "Missouri");
      ddd.Add("30", "Montana");
      ddd.Add("31", "Nebraska");
      ddd.Add("32", "Nevada");
      ddd.Add("33", "NewHampshire");
      ddd.Add("34", "NewJersey");
      ddd.Add("35", "NewMexico");
      ddd.Add("36", "NewYork");
      ddd.Add("37", "NorthCarolina");
      ddd.Add("38", "NorthDakota");
      ddd.Add("39", "Ohio");
      ddd.Add("40", "Oklahoma");
      ddd.Add("41", "Oregon");
      ddd.Add("42", "Pennsylvania");
      ddd.Add("44", "RhodeIsland");
      ddd.Add("45", "SouthCarolina");
      ddd.Add("46", "SouthDakota");
      ddd.Add("47", "Tennessee");
      ddd.Add("48", "Texas");
      ddd.Add("49", "Utah");
      ddd.Add("50", "Vermont");
      ddd.Add("51", "Virginia");
      ddd.Add("53", "Washington");
      ddd.Add("54", "WestVirginia");
      ddd.Add("55", "Wisconsin");
      ddd.Add("56", "Wyoming");
    }

    public void CreateFinalDB()
    {
      OleDbCommand oleDbCommand = new OleDbCommand();
      AccessFunc.CreateDB(this.FinDryDepDB);
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.FinDryDepDB))
      {
        try
        {
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "SELECT Year([TimeStamp]) AS [Year], DatePart(\"y\",[TimeStamp]) AS JDay, Month([TimeStamp]) AS [Month], Day([TimeStamp]) AS [Day], Hour([TimeStamp]) AS [Hour],  First(DryDeposition.MonitorSiteID) AS [SiteID], DryDeposition.Pollutant, Avg(DryDeposition.PPM) AS [PPM], Avg(DryDeposition.uGm3) AS [uGm3],  Avg(DryDeposition.DomainArea) AS [CityArea], Avg(DryDeposition.TrCovArea) AS [TrCovArea], Avg(DryDeposition.PARWm2) AS [PARWm2],  Avg(DryDeposition.RainMh) AS [RainMh], Avg(DryDeposition.TempF) AS [TempF], Avg(DryDeposition.TempK) AS [TempK],  Avg(DryDeposition.PerAqImp) AS [PerAqImp], Avg(DryDeposition.ActAqImp) AS [ActAqImp],  Avg(DryDeposition.Flux) AS [Flux], Avg(DryDeposition.FluxMax) AS [FluxMax], Avg(DryDeposition.FluxMin) AS [FluxMin],  Avg(DryDeposition.FluxWet) AS [FluxWet], Avg(DryDeposition.Trans) AS [Trans],  Avg(DryDeposition.Value) AS [Value], Avg(DryDeposition.ValueMax) AS [ValueMax], Avg(DryDeposition.ValueMin) AS [ValueMin] INTO Pollutants FROM [" + this.DryDepDB + "].[DryDeposition] GROUP BY Year([TimeStamp]), DatePart(\"y\",[TimeStamp]), Month([TimeStamp]), Day([TimeStamp]), Hour([TimeStamp]), DryDeposition.Pollutant;";
          oleDbCommand.ExecuteNonQuery();
          AccessFunc.CopyTable(this.DryDepDB, "08_DomainYearlySums", this.FinDryDepDB, "08_DomainYearlySums");
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

    private void CreateBenMapFinalDB()
    {
      OleDbCommand oleDbCommand = new OleDbCommand();
      string str;
      switch (this.VegType)
      {
        case DryDeposition.VEG_TYPE.TREE:
          str = "T";
          break;
        case DryDeposition.VEG_TYPE.SHRUB:
          str = "S";
          break;
        case DryDeposition.VEG_TYPE.GRASS:
          str = "G";
          break;
        case DryDeposition.VEG_TYPE.CORN:
          str = "C";
          break;
        default:
          str = "T";
          break;
      }
      AccessFunc.CreateDB(this.FinBenMAPDB);
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.BenMAPResultDB))
      {
        try
        {
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "SELECT DISTINCTROW 1 AS TimePeriod, \"" + str + "\" AS TreeShrub,AdverseHealthEffect AS HealthFactor, CDbl(-1) AS NO2Incidence, CDbl(-1) AS NO2Value,CDbl(-1) AS SO2Incidence, CDbl(-1) AS SO2Value,CDbl(-1) AS O3Incidence, CDbl(-1) AS O3Value,CDbl(-1) AS PM25Incidence, CDbl(-1) AS PM25Value INTO [" + this.FinBenMAPDB + "].[BenMapTable] FROM Pooled GROUP BY 1, \"" + str + "\", AdverseHealthEffect, CDbl(-1);";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "UPDATE [" + this.FinBenMAPDB + "].[BenMapTable] AS DST INNER JOIN Pooled AS SRC ON DST.HealthFactor = SRC.AdverseHealthEffect SET DST.NO2Incidence = SRC.[Incidence], DST.NO2Value = SRC.[Value] WHERE SRC.Pollutant=\"NO2\";";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "UPDATE [" + this.FinBenMAPDB + "].[BenMapTable] AS DST INNER JOIN Pooled AS SRC ON DST.HealthFactor = SRC.AdverseHealthEffect SET DST.SO2Incidence = SRC.[Incidence], DST.SO2Value = SRC.[Value] WHERE SRC.Pollutant=\"SO2\";";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "UPDATE [" + this.FinBenMAPDB + "].[BenMapTable] AS DST INNER JOIN Pooled AS SRC ON DST.HealthFactor = SRC.AdverseHealthEffect SET DST.O3Incidence = SRC.[Incidence], DST.O3Value = SRC.[Value] WHERE SRC.Pollutant=\"O3\";";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "UPDATE [" + this.FinBenMAPDB + "].[BenMapTable] AS DST INNER JOIN Pooled AS SRC ON DST.HealthFactor = SRC.AdverseHealthEffect SET DST.PM25Incidence = SRC.[Incidence], DST.PM25Value = SRC.[Value] WHERE SRC.Pollutant=\"PM2.5\";";
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
  }
}
