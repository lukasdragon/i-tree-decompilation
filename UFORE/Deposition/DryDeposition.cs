// Decompiled with JetBrains decompiler
// Type: UFORE.Deposition.DryDeposition
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.LAI;
using UFORE.Location;
using UFORE.MixingHeight;
using UFORE.Pollutant;
using UFORE.Weather;

namespace UFORE.Deposition
{
  public class DryDeposition
  {
    private const double Pi = 3.141592654;
    public const double NODATA = -999.0;
    private const double LAI_DIF1 = 0.1;
    private const double LAI_DIF2 = 0.5;
    private const double LAI_DIF3 = 1.0;
    private const double RAD2DEG = 57.29577794;
    private const double DEG2RAD = 0.017453292;
    private const double ZCK = 273.16;
    private const double NU = 15.5;
    private const double DC = 15.1;
    private const double DH = 22.2;
    private const double DO3 = 15.3;
    private const double DSO2 = 13.2;
    private const double DNO2 = 15.6;
    private const double DCO = 20.0;
    private const double DV = 24.9;
    private const double PR_2 = 0.69819819819819817;
    private const double SCC = 1.0264900662251655;
    private const double SCO3_2 = 1.0130718954248366;
    private const double SCSO2_2 = 1.1742424242424243;
    private const double SCNO2_2 = 0.99358974358974361;
    private const double SCCO_2 = 0.775;
    private const double PPFD_CON = 0.001;
    private const double SBET_CON = 2E-05;
    private const double LAI_CON = 0.001;
    private const double LEAFANG = 1.0471975513333334;
    private const double CO_ALPHA = 0.0027;
    private const double CO_CL = 1.066;
    private const double WA = 1320.0;
    private const double GAS_CONST = 8.314;
    private const double T25 = 298.16;
    private const int CONS_S = 710;
    private const int CONS_H = 220000;
    private const int KC25 = 333;
    private const int EKC = 65120;
    private const int KO25 = 295;
    private const int EKO = 13990;
    private const int O2 = 210;
    private const int ERD = 51176;
    private const int EJM = 37000;
    private const int EVC = 64637;
    private const double ATMOSPH = 1.01325;
    private const double M3_MOLE = 0.0224;
    private const double KBALL = 10.0;
    private const double BPRIME = 0.02;
    private const double BPRIME16 = 0.012499999999999999;
    private const double QALPHA = 0.055;
    private const int KG_TO_G = 1000;
    private const int SD_TO_HR = 3600;
    private const int HA_TO_M2 = 10000;
    private const int MT_TO_G = 1000000;
    private const int TH_TO_D = 1000;
    private const int CO_LEAFOFF_RC = 1000000;
    private const int CO_LEAFON_RC = 50000;
    private const double PM10AVGVD = 0.0064;
    private const double PM10MINVD = 0.0025;
    private const double PM10MAXVD = 0.01;
    private const int PM10_LAI = 6;
    private const double BAI = 1.7;
    public const string DRY_DEP_TABLE = "DryDeposition";
    private const string SITE_MONTHLY_MEAN_TABLE = "01_SiteMonthlyMeans";
    private const string SITE_YEARLY_MEAN_TABLE = "02_SiteYearlyMeans";
    private const string DOMAIN_MONTHLY_MEAN_TABLE = "03_DomainMonthlyMeans";
    public const string DOMAIN_YEARLY_MEAN_TABLE = "04_DomainYearlyMeans";
    public const string DOMAIN_LEAFON_MEAN_TABLE = "04_DomainLeafOnMeans";
    private const string SITE_MONTHLY_SUM_TABLE = "05_SiteMonthlySums";
    private const string SITE_YEARLY_SUM_TABLE = "06_SiteYearlySums";
    private const string DOMAIN_MONTHLY_SUM_TABLE = "07_DomainMonthlySums";
    public const string DOMAIN_YEARLY_SUM_TABLE = "08_DomainYearlySums";
    public const string DOMAIN_LEAFON_SUM_TABLE = "09_DomainLeafOnSums";
    private static double VC25;
    private static int JMAX25;
    private static int KS_VAL;
    private static int NLAY;
    public DateTime TimeStamp;
    private string PriPart;
    private string SecPart;
    private string TerPart;
    private double Latitude;
    private double Longitude;
    private double GMTOffset;
    private int LeafOnDOY;
    private int LeafOffDOY;
    private double PollBaseCost;
    private double PollBaseYearPPI;
    private double ModelYearPPI;
    private string MonitorState;
    private string MonitorCounty;
    private string MonitorSiteID;
    public string Pollutant;
    public double PPM;
    public double uGm3;
    private double DomainArea;
    private double EvGrnLAI;
    private double preLai;
    private double Lai;
    private double PctEvGrnCov;
    private double PctTrCov;
    public double TrCovArea;
    private double Ceiling;
    public bool GrowSeason;
    private double OpCldCov;
    private double PARWm2;
    private double PARuEm2s;
    private double PrsMbar;
    private double RainMh;
    private double VegStMh;
    private double RelHum;
    private double SatVPrsKpa;
    private double SolZenAgl;
    private int Stability;
    private double TempF;
    public double TempK;
    private double ToCldCov;
    public double MixHt;
    private double VapPrsKpa;
    private double WdSpdMs;
    public double IsprLtCor;
    public string Period;
    private double Ra;
    private double Rb;
    private double Rc;
    private double RCuticle;
    private double RMesophyll;
    private double Rsoil;
    private double RStomatal;
    public double Trans;
    private double Ustar;
    private double VdAct;
    private double VdDry;
    private double VdMax;
    private double VdMin;
    private double VdWet;
    public double Flux;
    private double FluxMax;
    private double FluxMin;
    private double FluxWet;
    private double AccumFluxPM25;
    private double AccumFluxPM25Min;
    private double AccumFluxPM25Max;
    private double Value;
    private double ValueMin;
    private double ValueMax;
    private double PerAqImp;
    private double PerAqImpMin;
    private double PerAqImpMax;
    private double ActAqImp;
    private double ActAqImpMin;
    private double ActAqImpMax;
    public double ConcChg;
    public double ConcChgMin;
    public double ConcChgMax;

    public static void SetVegType(DryDeposition.VEG_TYPE sVeg)
    {
      switch (sVeg)
      {
        case DryDeposition.VEG_TYPE.TREE:
        case DryDeposition.VEG_TYPE.SHRUB:
          DryDeposition.VC25 = 90.0;
          DryDeposition.JMAX25 = 171;
          DryDeposition.KS_VAL = 1;
          DryDeposition.NLAY = 30;
          break;
        case DryDeposition.VEG_TYPE.GRASS:
          DryDeposition.VC25 = 50.0;
          DryDeposition.JMAX25 = 87;
          DryDeposition.KS_VAL = 2;
          DryDeposition.NLAY = 1;
          break;
        case DryDeposition.VEG_TYPE.CORN:
          DryDeposition.VC25 = 66.0;
          DryDeposition.JMAX25 = 160;
          DryDeposition.KS_VAL = 2;
          DryDeposition.NLAY = 1;
          break;
      }
    }

    [Obsolete]
    public static void ProcessDryDeposition(
      LocationData locData,
      ForestData forData,
      string sWeatherDB,
      string sLaiDB,
      string sPolDB,
      ForestData.RURAL_URBAN ru,
      string sDryDepDB,
      string domain)
    {
      DryDeposition.ProcessDryDeposition(locData, forData, sWeatherDB, sLaiDB, sPolDB, ru, sDryDepDB, domain, (UFORE_D) null, 0, 0);
    }

    [Obsolete]
    public static void ProcessDryDeposition(
      LocationData locData,
      ForestData forData,
      string sWeatherDB,
      string sLaiDB,
      string sPolDB,
      ForestData.RURAL_URBAN ru,
      string sDryDepDB,
      string domain,
      UFORE_D uforeDObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      AccessFunc.CreateDB(sDryDepDB);
      using (OleDbConnection cnWeatherDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sWeatherDB))
      {
        using (OleDbConnection cnLaiDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sLaiDB))
        {
          using (OleDbConnection cnPolDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sPolDB))
          {
            using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDryDepDB))
            {
              cnWeatherDB.Open();
              cnLaiDB.Open();
              cnPolDB.Open();
              cnDryDepDB.Open();
              DryDeposition.ProcessDryDeposition(locData, forData, cnWeatherDB, cnLaiDB, cnPolDB, ru, cnDryDepDB, domain, uforeDObj, PercentRangeFrom, PercentRangeTo);
            }
          }
        }
      }
    }

    public static void ProcessDryDeposition(
      LocationData locData,
      ForestData forData,
      OleDbConnection cnWeatherDB,
      OleDbConnection cnLaiDB,
      OleDbConnection cnPolDB,
      ForestData.RURAL_URBAN ru,
      OleDbConnection cnDryDepDB,
      string domain,
      UFORE_D uforeDObj,
      int PercentRangeFrom,
      int PercentRangeTo)
    {
      List<SurfaceWeather> surfaceWeatherList = new List<SurfaceWeather>();
      List<HourlyMixHt> mhList = new List<HourlyMixHt>();
      List<LeafAreaIndex> laiList = new List<LeafAreaIndex>();
      List<Resistance> resList = new List<Resistance>();
      double num = (double) (PercentRangeTo - PercentRangeFrom) / 7.0;
      int recCnt = SurfaceWeather.ReadSurfaceAllRecords(cnWeatherDB, ref surfaceWeatherList);
      DateTime timeStamp1 = surfaceWeatherList[0].TimeStamp;
      DateTime timeStamp2 = surfaceWeatherList[recCnt - 1].TimeStamp;
      LeafAreaIndex.ReadLAIPartialRecords(cnLaiDB, ref laiList, timeStamp1, timeStamp2);
      if (HourlyMixHt.ReadMixHtAllRecords(cnWeatherDB, ref mhList) == 0)
        HourlyMixHt.SetNullMixHtData(recCnt, ref mhList);
      Resistance.CalcResistances(locData, ref surfaceWeatherList, recCnt, cnDryDepDB);
      Resistance.ReadResistanceRecords(cnDryDepDB, ref resList);
      DryDeposition.CreateDryDepTable(cnDryDepDB);
      uforeDObj?.reportProgress((int) ((double) PercentRangeFrom + num));
      List<AirPollutant> airPollutantList = new List<AirPollutant>();
      int polRecCnt1 = AirPollutant.ReadPollAllRecords(cnPolDB, "CO", ref airPollutantList);
      if (polRecCnt1 == 0)
        polRecCnt1 = AirPollutant.SetNullPollData(recCnt, ref airPollutantList);
      DryDeposition.ProcessHourlyDryDeposition("CO", locData, forData, ref surfaceWeatherList, ref mhList, ru, ref laiList, ref resList, recCnt, ref airPollutantList, polRecCnt1, cnDryDepDB, domain);
      uforeDObj?.reportProgress((int) ((double) PercentRangeFrom + 2.0 * num));
      airPollutantList = new List<AirPollutant>();
      int polRecCnt2 = AirPollutant.ReadPollAllRecords(cnPolDB, "NO2", ref airPollutantList);
      if (polRecCnt2 == 0)
        polRecCnt2 = AirPollutant.SetNullPollData(recCnt, ref airPollutantList);
      DryDeposition.ProcessHourlyDryDeposition("NO2", locData, forData, ref surfaceWeatherList, ref mhList, ru, ref laiList, ref resList, recCnt, ref airPollutantList, polRecCnt2, cnDryDepDB, domain);
      uforeDObj?.reportProgress((int) ((double) PercentRangeFrom + 3.0 * num));
      airPollutantList = new List<AirPollutant>();
      int polRecCnt3 = AirPollutant.ReadPollAllRecords(cnPolDB, "O3", ref airPollutantList);
      if (polRecCnt3 == 0)
        polRecCnt3 = AirPollutant.SetNullPollData(recCnt, ref airPollutantList);
      DryDeposition.ProcessHourlyDryDeposition("O3", locData, forData, ref surfaceWeatherList, ref mhList, ru, ref laiList, ref resList, recCnt, ref airPollutantList, polRecCnt3, cnDryDepDB, domain);
      uforeDObj?.reportProgress((int) ((double) PercentRangeFrom + 4.0 * num));
      airPollutantList = new List<AirPollutant>();
      int polRecCnt4 = AirPollutant.ReadPollAllRecords(cnPolDB, "PM10*", ref airPollutantList);
      if (polRecCnt4 == 0)
        polRecCnt4 = AirPollutant.SetNullPollData(recCnt, ref airPollutantList);
      DryDeposition.ProcessHourlyDryDepositionPM10("PM10*", locData, forData, ref surfaceWeatherList, ref mhList, ru, ref laiList, ref resList, recCnt, ref airPollutantList, polRecCnt4, cnDryDepDB, domain);
      uforeDObj?.reportProgress((int) ((double) PercentRangeFrom + 5.0 * num));
      airPollutantList = new List<AirPollutant>();
      int polRecCnt5 = AirPollutant.ReadPollAllRecords(cnPolDB, "PM2.5", ref airPollutantList);
      if (polRecCnt5 == 0)
        polRecCnt5 = AirPollutant.SetNullPollData(recCnt, ref airPollutantList);
      DryDeposition.ProcessHourlyDryDepositionPM25("PM2.5", locData, forData, ref surfaceWeatherList, ref mhList, ru, ref laiList, ref resList, recCnt, ref airPollutantList, polRecCnt5, cnDryDepDB, domain);
      uforeDObj?.reportProgress((int) ((double) PercentRangeFrom + 6.0 * num));
      airPollutantList = new List<AirPollutant>();
      int polRecCnt6 = AirPollutant.ReadPollAllRecords(cnPolDB, "SO2", ref airPollutantList);
      if (polRecCnt6 == 0)
        polRecCnt6 = AirPollutant.SetNullPollData(recCnt, ref airPollutantList);
      DryDeposition.ProcessHourlyDryDeposition("SO2", locData, forData, ref surfaceWeatherList, ref mhList, ru, ref laiList, ref resList, recCnt, ref airPollutantList, polRecCnt6, cnDryDepDB, domain);
      uforeDObj?.reportProgress(PercentRangeTo);
    }

    private static void ProcessHourlyDryDeposition(
      string sPoll,
      LocationData locData,
      ForestData forData,
      ref List<SurfaceWeather> sfcList,
      ref List<HourlyMixHt> mhList,
      ForestData.RURAL_URBAN ru,
      ref List<LeafAreaIndex> laiList,
      ref List<Resistance> resList,
      int recCnt,
      ref List<AirPollutant> polList,
      int polRecCnt,
      OleDbConnection conn,
      string domain)
    {
      List<DryDeposition> ddList = new List<DryDeposition>();
      int num = polRecCnt / recCnt;
      int recCnt1 = 0;
      int polIdx = 0;
      for (int index1 = 0; index1 < num; ++index1)
      {
        for (int index2 = 0; index2 < recCnt; ++index2)
        {
          DryDeposition dryDeposition = new DryDeposition();
          dryDeposition.SetDryDepData(locData, forData, ref sfcList, ref mhList, ru, ref laiList, ref resList, index2, sPoll, ref polList, polIdx);
          dryDeposition.CalcDryDeposition(locData.CO2Conc);
          if (laiList[index2].Lai == 0.0)
            dryDeposition.SetDryDepResultForLAI0();
          ddList.Add(dryDeposition);
          ++polIdx;
          ++recCnt1;
        }
        if (sPoll != "CO")
          DryDeposition.AdjustTranspiration(sfcList, ddList, recCnt);
      }
      DryDeposition.WriteDryDepRecords(conn, ref ddList, recCnt1, domain);
    }

    private void SetDryDepData(
      LocationData locData,
      ForestData forData,
      ref List<SurfaceWeather> sfcList,
      ref List<HourlyMixHt> mhList,
      ForestData.RURAL_URBAN ru,
      ref List<LeafAreaIndex> laiList,
      ref List<Resistance> resList,
      int idx,
      string Poll,
      ref List<AirPollutant> polList,
      int polIdx)
    {
      try
      {
        this.TimeStamp = sfcList[idx].TimeStamp;
        this.PriPart = locData.PriPart;
        this.SecPart = locData.SecPart;
        this.TerPart = locData.TerPart;
        this.Latitude = locData.Latitude;
        this.Longitude = locData.Longitude;
        this.GMTOffset = locData.GMTOffset;
        this.LeafOffDOY = locData.LeafOffDOY;
        this.LeafOnDOY = locData.LeafOnDOY;
        this.ModelYearPPI = locData.ModelYearPPI;
        PollutantCost pollutantCost = new PollutantCost();
        if (Poll == "PM10*")
          locData.PollCostDict.TryGetValue("PM10", out pollutantCost);
        else
          locData.PollCostDict.TryGetValue(Poll, out pollutantCost);
        this.PollBaseCost = pollutantCost.BaseCost;
        this.PollBaseYearPPI = pollutantCost.BaseYearPPI;
        if (Poll != "PM10*" && Poll != "PM2.5")
        {
          double num;
          resList[idx].Rb.TryGetValue(Poll, out num);
          this.Rb = num;
        }
        this.MonitorState = polList[polIdx].State;
        this.MonitorCounty = polList[polIdx].County;
        this.MonitorSiteID = polList[polIdx].SiteID;
        this.Pollutant = Poll;
        this.PPM = polList[polIdx].PPM;
        this.uGm3 = polList[polIdx].uGm3;
        this.DomainArea = forData.DomainArea;
        this.EvGrnLAI = forData.EvGrnLAI;
        this.preLai = idx == 0 ? laiList[idx].Lai : laiList[idx - 1].Lai;
        this.Lai = laiList[idx].Lai;
        this.PctEvGrnCov = forData.PctEvGrnCov;
        this.PctTrCov = forData.PctTrCov;
        this.TrCovArea = forData.TrCovArea;
        this.Ceiling = sfcList[idx].Ceiling;
        this.OpCldCov = sfcList[idx].OpCldCov;
        this.PARuEm2s = sfcList[idx].PARuEm2s;
        this.PARWm2 = sfcList[idx].PARWm2;
        this.PrsMbar = sfcList[idx].PrsMbar;
        this.RainMh = sfcList[idx].RainMh;
        this.RelHum = sfcList[idx].RelHum;
        this.SatVPrsKpa = sfcList[idx].SatVPrsKpa;
        this.SolZenAgl = sfcList[idx].SolZenAgl;
        this.TempF = sfcList[idx].TempF;
        this.TempK = sfcList[idx].TempK;
        this.ToCldCov = sfcList[idx].ToCldCov;
        this.MixHt = ru == ForestData.RURAL_URBAN.URBAN ? mhList[idx].UrbanMixHt : mhList[idx].RuralMixHt;
        this.VapPrsKpa = sfcList[idx].VapPrsKpa;
        this.VegStMh = sfcList[idx].VegStMh;
        this.WdSpdMs = sfcList[idx].WdSpdMs;
        this.Stability = resList[idx].Stability;
        this.Period = resList[idx].Period;
        this.Ustar = resList[idx].Ustar;
        this.Ra = resList[idx].Ra;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void SetDryDepData(
      LocationData locData,
      ForestData forData,
      ref List<SurfaceWeather> sfcList,
      ref List<HourlyMixHt> mhList,
      ForestData.RURAL_URBAN ru,
      ref List<LeafAreaIndex> laiList,
      ref List<Resistance> resList,
      int idx,
      string Poll,
      ref List<AirPollutant> polList,
      int polIdx,
      double prAccumFlux,
      double prAccumFluxMin,
      double prAccumFluxMax)
    {
      try
      {
        this.TimeStamp = sfcList[idx].TimeStamp;
        this.PriPart = locData.PriPart;
        this.SecPart = locData.SecPart;
        this.TerPart = locData.TerPart;
        this.Latitude = locData.Latitude;
        this.Longitude = locData.Longitude;
        this.GMTOffset = locData.GMTOffset;
        this.LeafOffDOY = locData.LeafOffDOY;
        this.LeafOnDOY = locData.LeafOnDOY;
        this.ModelYearPPI = locData.ModelYearPPI;
        PollutantCost pollutantCost = new PollutantCost();
        if (Poll == "PM10*")
          locData.PollCostDict.TryGetValue("PM10", out pollutantCost);
        else
          locData.PollCostDict.TryGetValue(Poll, out pollutantCost);
        this.PollBaseCost = pollutantCost.BaseCost;
        this.PollBaseYearPPI = pollutantCost.BaseYearPPI;
        if (Poll != "PM10*" && Poll != "PM2.5")
        {
          double num;
          resList[idx].Rb.TryGetValue(Poll, out num);
          this.Rb = num;
        }
        this.MonitorState = polList[polIdx].State;
        this.MonitorCounty = polList[polIdx].County;
        this.MonitorSiteID = polList[polIdx].SiteID;
        this.Pollutant = Poll;
        this.PPM = polList[polIdx].PPM;
        this.uGm3 = polList[polIdx].uGm3;
        this.DomainArea = forData.DomainArea;
        this.EvGrnLAI = forData.EvGrnLAI;
        this.preLai = idx == 0 ? laiList[idx].Lai : laiList[idx - 1].Lai;
        this.Lai = laiList[idx].Lai;
        this.PctEvGrnCov = forData.PctEvGrnCov;
        this.PctTrCov = forData.PctTrCov;
        this.TrCovArea = forData.TrCovArea;
        this.Ceiling = sfcList[idx].Ceiling;
        this.OpCldCov = sfcList[idx].OpCldCov;
        this.PARuEm2s = sfcList[idx].PARuEm2s;
        this.PARWm2 = sfcList[idx].PARWm2;
        this.PrsMbar = sfcList[idx].PrsMbar;
        this.RainMh = sfcList[idx].RainMh;
        this.RelHum = sfcList[idx].RelHum;
        this.SatVPrsKpa = sfcList[idx].SatVPrsKpa;
        this.SolZenAgl = sfcList[idx].SolZenAgl;
        this.TempF = sfcList[idx].TempF;
        this.TempK = sfcList[idx].TempK;
        this.ToCldCov = sfcList[idx].ToCldCov;
        this.MixHt = ru == ForestData.RURAL_URBAN.URBAN ? mhList[idx].UrbanMixHt : mhList[idx].RuralMixHt;
        this.VapPrsKpa = sfcList[idx].VapPrsKpa;
        this.VegStMh = sfcList[idx].VegStMh;
        this.WdSpdMs = sfcList[idx].WdSpdMs;
        this.Stability = resList[idx].Stability;
        this.Period = resList[idx].Period;
        this.Ustar = resList[idx].Ustar;
        this.Ra = resList[idx].Ra;
        this.AccumFluxPM25 = prAccumFlux;
        this.AccumFluxPM25Min = prAccumFluxMin;
        this.AccumFluxPM25Max = prAccumFluxMax;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void SetDryDepResultForLAI0()
    {
      try
      {
        this.Flux = 0.0;
        this.FluxMax = 0.0;
        this.FluxMin = 0.0;
        this.FluxWet = 0.0;
        this.IsprLtCor = 0.0;
        this.Rc = 0.0;
        this.RCuticle = 0.0;
        this.RMesophyll = 0.0;
        this.Rsoil = 0.0;
        this.RStomatal = 0.0;
        this.Trans = 0.0;
        this.Ustar = 0.0;
        this.Value = 0.0;
        this.ValueMax = 0.0;
        this.ValueMin = 0.0;
        this.VdAct = 0.0;
        this.VdDry = 0.0;
        this.VdMax = 0.0;
        this.VdMin = 0.0;
        this.VdWet = 0.0;
        this.ActAqImp = 0.0;
        this.ConcChg = 0.0;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void CalcDryDeposition(double co2conc)
    {
      double[] sclFactor = new double[DryDeposition.NLAY];
      double avgsclfactor = 0.0;
      double parDirect = 0.0;
      double parDiffuse = 0.0;
      double[] laiSunlit = new double[DryDeposition.NLAY];
      double[] laiShaded = new double[DryDeposition.NLAY];
      double[] parSunlit = new double[DryDeposition.NLAY];
      double[] parShaded = new double[DryDeposition.NLAY];
      double gsTotal = 0.0;
      double ciguess = co2conc * 0.7;
      try
      {
        this.CalcScalingFactor(ref sclFactor, ref avgsclfactor);
        this.CalcIsopreneLightCorrectionFactor(sclFactor, avgsclfactor, ref parDirect, ref parDiffuse, ref laiSunlit, ref laiShaded, ref parSunlit, ref parShaded);
        this.CheckGrowSeason();
        this.CalcCanopyRes(co2conc, ciguess, parDirect, parDiffuse, laiSunlit, laiShaded, parSunlit, parShaded, ref gsTotal);
        this.CalcDepositionVelocity();
        this.AdjustMixHt();
        this.CalcPollFlux();
        this.CalcValue();
        this.CalcAirQualityImprovement();
        this.CalcConcChange();
        this.CalcTranspiration(gsTotal);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private static double Deg2Rad(double angle) => Math.PI * angle / 180.0;

    private static double Rad2Deg(double angle) => 180.0 * angle / Math.PI;

    private void CalcIsopreneLightCorrectionFactor(
      double[] sclFactor,
      double avgsclfactor,
      ref double parDirect,
      ref double parDiffuse,
      ref double[] laiSunlit,
      ref double[] laiShaded,
      ref double[] parSunlit,
      ref double[] parShaded)
    {
      double num = DryDeposition.Deg2Rad(this.SolZenAgl);
      this.CalcSolarRadiationComponent(num, ref parDirect, ref parDiffuse);
      double deltaLai = this.Lai / (double) DryDeposition.NLAY;
      if (parDirect <= 0.001 && parDiffuse <= 0.001 || Math.Cos(num) <= 2E-05 || this.Lai <= 0.001)
        return;
      this.IsprLtCor = 0.0;
      double liteCor = 0.0;
      for (int layerIdx = 1; layerIdx <= DryDeposition.NLAY; ++layerIdx)
      {
        int index = layerIdx - 1;
        this.CalcSunlitShadedLeafArea(deltaLai, layerIdx, num, ref laiSunlit[index], ref laiShaded[index]);
        this.CalcSunlitShadedPar(num, parDirect, parDiffuse, deltaLai, layerIdx, laiSunlit[index], laiShaded[index], sclFactor[index], avgsclfactor, ref parSunlit[index], ref parShaded[index], out liteCor);
        this.IsprLtCor += liteCor;
      }
      this.IsprLtCor /= (double) DryDeposition.NLAY;
    }

    private void CalcScalingFactor(ref double[] sclFactor, ref double avgsclfactor)
    {
      int index1 = 0;
      for (int index2 = 1; index2 <= DryDeposition.NLAY; ++index2)
      {
        sclFactor[index1] = Math.Exp(-0.5 * this.Lai * (((double) index2 - 0.5) / (double) DryDeposition.NLAY));
        avgsclfactor += sclFactor[index1];
        ++index1;
      }
      avgsclfactor /= (double) DryDeposition.NLAY;
    }

    private void CalcSolarRadiationComponent(
      double solTheta,
      ref double parDirect,
      ref double parDiffuse)
    {
      double num1 = this.PrsMbar / 10.0 / (101.3 * Math.Cos(solTheta));
      double num2 = 600.0 * Math.Exp(-0.185 * num1) * Math.Cos(solTheta);
      double num3 = 0.4 * (600.0 - num2) * Math.Cos(solTheta);
      double num4 = (720.0 * Math.Exp(-0.06 * num1) - 1320.0) * Math.Cos(solTheta);
      double num5 = 0.6 * (-600.0 - num4) * Math.Cos(solTheta);
      double num6 = num2 + num3;
      double num7 = num4 + num5;
      if (num6 < 0.0)
        num6 = 0.1;
      if (num7 < 0.0)
        num7 = 0.1;
      double num8 = this.PARWm2 * 2.0 / (num6 + num7);
      if (num8 >= 0.9 || num8 == 0.0)
        num8 = 0.9;
      double num9 = num2 / num6 * (1.0 - Math.Pow((0.9 - num8) / 0.7, 0.67));
      parDirect = num9 * this.PARuEm2s;
      parDiffuse = (1.0 - num9) * this.PARuEm2s;
      if (parDirect > 0.0)
        return;
      parDirect = 0.0;
      parDiffuse = this.PARuEm2s;
    }

    private void CalcSunlitShadedLeafArea(
      double deltaLai,
      int layerIdx,
      double theta,
      ref double laiSunlit,
      ref double laiShaded)
    {
      laiSunlit = (Math.Exp(-deltaLai * (double) (layerIdx - 1) / (2.0 * Math.Cos(theta))) - Math.Exp(-deltaLai * (double) layerIdx / (2.0 * Math.Cos(theta)))) * 2.0 * Math.Cos(theta);
      laiShaded = deltaLai - laiSunlit;
    }

    private void CalcSunlitShadedPar(
      double theta,
      double parDirect,
      double parDiffuse,
      double deltaLai,
      int layerIdx,
      double laiSunlit,
      double laiShaded,
      double sclFactor,
      double avgsclfactor,
      ref double parSunlit,
      ref double parShaded,
      out double liteCor)
    {
      double a = 1.57079628 - theta;
      double num1 = 0.07 * parDirect * (1.1 - 0.1 * ((double) layerIdx * deltaLai - deltaLai / 2.0)) * Math.Exp(-Math.Sin(a));
      if (num1 < 0.0)
        num1 = 0.0;
      parShaded = this.Lai <= 4.5 ? parDiffuse * Math.Exp(-0.5 * Math.Pow(this.Lai, 0.7)) * (sclFactor / avgsclfactor) + num1 : parDiffuse * 1.07 / this.Lai * (sclFactor / avgsclfactor) + num1;
      parSunlit = parDirect * Math.Cos(Math.PI / 3.0) / Math.Sin(a) + parShaded;
      double num2 = laiSunlit / deltaLai;
      double num3 = this.CalcParCorrFactor(parSunlit);
      double num4 = this.CalcParCorrFactor(parShaded);
      liteCor = num2 * num3 + (1.0 - num2) * num4;
    }

    private void CheckGrowSeason()
    {
      if (this.LeafOnDOY > this.LeafOffDOY)
      {
        if (this.TimeStamp.DayOfYear < this.LeafOffDOY || this.LeafOnDOY <= this.TimeStamp.DayOfYear)
          this.GrowSeason = true;
        else
          this.GrowSeason = false;
      }
      else if (this.LeafOnDOY < this.LeafOffDOY)
      {
        if (this.TimeStamp.DayOfYear < this.LeafOnDOY || this.LeafOffDOY <= this.TimeStamp.DayOfYear)
          this.GrowSeason = false;
        else
          this.GrowSeason = true;
      }
      else
        this.GrowSeason = true;
    }

    private void CalcDepositionVelocity()
    {
      this.VdAct = this.VdMin = this.VdMax = this.VdDry = this.VdWet = 0.0;
      this.VdDry = 1.0 / (this.Ra + this.Rb + this.Rc);
      if (this.RainMh == 0.0)
      {
        this.VdAct = this.VdDry;
        this.VdWet = 0.0;
        if (this.GrowSeason)
        {
          if (this.PARuEm2s > 0.0)
          {
            string pollutant = this.Pollutant;
            if (!(pollutant == "CO"))
            {
              if (!(pollutant == "O3"))
              {
                if (!(pollutant == "NO2"))
                {
                  if (!(pollutant == "SO2"))
                    return;
                  this.VdMin = this.VdAct < 0.002 ? this.VdAct : 0.002;
                  this.VdMax = this.VdAct > 0.01 ? this.VdAct : 0.01;
                }
                else
                {
                  this.VdMin = this.VdAct < 0.001 ? this.VdAct : 0.001;
                  this.VdMax = this.VdAct > 0.005 ? this.VdAct : 0.005;
                }
              }
              else
              {
                this.VdMin = this.VdAct < 0.001 ? this.VdAct : 0.001;
                this.VdMax = this.VdAct > 0.008 ? this.VdAct : 0.008;
              }
            }
            else
            {
              this.VdMin = this.VdAct;
              this.VdMax = this.VdAct;
            }
          }
          else
          {
            this.VdMin = this.VdAct;
            this.VdMax = this.VdAct;
          }
        }
        else
        {
          this.VdMin = this.VdAct;
          this.VdMax = this.VdAct;
        }
      }
      else
      {
        this.VdWet = 100.0 / (this.Ra + this.Rb + this.Rc);
        if (this.VdWet < 0.0)
          this.VdWet = 0.0;
        this.VdAct = 0.0;
        this.VdMin = 0.0;
        this.VdMax = 0.0;
      }
    }

    private void CalcPollFlux()
    {
      this.Flux = this.VdAct * this.uGm3 * Math.Pow(10.0, -6.0) * 3600.0;
      this.FluxWet = this.VdWet * this.uGm3 * Math.Pow(10.0, -6.0) * 3600.0;
      this.FluxMin = this.VdMin * this.uGm3 * Math.Pow(10.0, -6.0) * 3600.0;
      this.FluxMax = this.VdMax * this.uGm3 * Math.Pow(10.0, -6.0) * 3600.0;
    }

    private void CalcValue()
    {
      if (this.PollBaseYearPPI == 0.0)
      {
        this.Value = 0.0;
        this.ValueMin = 0.0;
        this.ValueMax = 0.0;
      }
      else
      {
        double num = this.PollBaseCost * this.ModelYearPPI / this.PollBaseYearPPI;
        this.Value = this.Flux / 1000000.0 * num;
        this.ValueMin = this.FluxMin / 1000000.0 * num;
        this.ValueMax = this.FluxMax / 1000000.0 * num;
      }
    }

    private void CalcTranspiration(double gsTotal)
    {
      double num1 = 18000.0 * this.SatVPrsKpa / (8.314 * this.TempK);
      double num2 = 18000.0 * this.VapPrsKpa / (8.314 * this.TempK);
      if (gsTotal == 0.0)
      {
        this.Trans = 0.0;
      }
      else
      {
        this.Trans = (num1 - num2) / (1.0 / gsTotal + this.Ra) * (3600.0 / this.Lai);
        if (this.Trans < 0.0)
          this.Trans = 0.0;
      }
      this.Trans *= Math.Pow(10.0, -6.0);
    }

    private void AdjustMixHt()
    {
      if (this.MixHt == -999.0)
        return;
      if (this.PARuEm2s == 0.0 && this.MixHt < 150.0)
      {
        this.MixHt = 150.0;
      }
      else
      {
        if (this.PARuEm2s <= 0.0 || this.MixHt >= 250.0)
          return;
        this.MixHt = 250.0;
      }
    }

    private void CalcAirQualityImprovement()
    {
      if (this.MixHt != -999.0)
      {
        double num1 = this.uGm3 * this.MixHt * Math.Pow(10.0, -6.0);
        if (this.RainMh > 0.0)
          this.PerAqImp = this.PerAqImpMin = this.PerAqImpMax = this.ActAqImp = this.ActAqImpMin = this.ActAqImpMax = 0.0;
        else if (num1 == 0.0)
        {
          this.PerAqImp = this.PerAqImpMin = this.PerAqImpMax = this.ActAqImp = this.ActAqImpMin = this.ActAqImpMax = 0.0;
        }
        else
        {
          this.PerAqImp = this.Flux < 0.0 ? this.Flux / num1 * 100.0 : this.Flux / (this.Flux + num1) * 100.0;
          this.PerAqImpMin = this.FluxMin < 0.0 ? this.FluxMin / num1 * 100.0 : this.FluxMin / (this.FluxMin + num1) * 100.0;
          this.PerAqImpMax = this.FluxMax < 0.0 ? this.FluxMax / num1 * 100.0 : this.FluxMax / (this.FluxMax + num1) * 100.0;
          double num2 = num1 * this.DomainArea;
          double num3 = this.Flux * this.TrCovArea;
          this.ActAqImp = num3 + num2 == 0.0 ? this.PerAqImp * this.PctTrCov / 100.0 : num3 / (num3 + num2) * 100.0;
          double num4 = this.FluxMin * this.TrCovArea;
          this.ActAqImpMin = num4 + num2 == 0.0 ? this.PerAqImpMin * this.PctTrCov / 100.0 : num4 / (num4 + num2) * 100.0;
          double num5 = this.FluxMax * this.TrCovArea;
          this.ActAqImpMax = num5 + num2 == 0.0 ? this.PerAqImpMax * this.PctTrCov / 100.0 : num5 / (num5 + num2) * 100.0;
        }
      }
      else
        this.PerAqImp = this.PerAqImpMin = this.PerAqImpMax = this.ActAqImp = this.ActAqImpMin = this.ActAqImpMax = -999.0;
    }

    private void CalcConcChange()
    {
      this.ConcChg = this.MixHt != -999.0 ? this.PPM / (1.0 - this.ActAqImp / 100.0) - this.PPM : -999.0;
      this.ConcChgMin = this.MixHt != -999.0 ? this.PPM / (1.0 - this.ActAqImpMin / 100.0) - this.PPM : -999.0;
      this.ConcChgMax = this.MixHt != -999.0 ? this.PPM / (1.0 - this.ActAqImpMax / 100.0) - this.PPM : -999.0;
    }

    private static void AdjustTranspiration(
      List<SurfaceWeather> sfcList,
      List<DryDeposition> ddList,
      int recCnt)
    {
      int num1 = 0;
      double num2 = 0.0;
      for (int index = 0; index < recCnt; ++index)
      {
        if (ddList[index].GrowSeason && sfcList[index].PtTrMh >= ddList[index].Trans && sfcList[index].PtTrMh != 0.0)
        {
          num2 += ddList[index].Trans / sfcList[index].PtTrMh;
          ++num1;
        }
      }
      double num3 = num2 / (double) num1;
      for (int index = 0; index < recCnt; ++index)
      {
        if (!ddList[index].GrowSeason)
        {
          if (ddList[index].Trans != 0.0)
            ddList[index].Trans = num3 * sfcList[index].PtTrMh;
        }
        else if (sfcList[index].PtTrMh < ddList[index].Trans)
          ddList[index].Trans = num3 * sfcList[index].PtTrMh;
      }
    }

    private void CalcCanopyRes(
      double co2conc,
      double ciguess,
      double parDirect,
      double parDiffuse,
      double[] laiSunlit,
      double[] laiShaded,
      double[] parSunlit,
      double[] parShaded,
      ref double gsTotal)
    {
      if (this.Pollutant == "CO")
      {
        this.Rc = this.GrowSeason ? 50000.0 : 1000000.0;
      }
      else
      {
        this.CalcSoilRes();
        this.CalcCuticleRes();
        if (this.RainMh > 0.0)
        {
          this.RMesophyll = this.RStomatal = gsTotal = 0.0;
          this.Rc = this.RCuticle / 2.0 * this.Rsoil / (this.RCuticle / 2.0 + this.Rsoil);
        }
        else
        {
          this.CalcMesophyllRes();
          this.RStomatal = 100000000.0;
          gsTotal = 0.0;
          if (this.PARWm2 > 0.0)
          {
            double solTheta;
            double kc;
            double ko;
            double jMax;
            double vcMax;
            double lgGamma;
            double wc;
            double pStat273;
            double gb;
            double alpha;
            double beta;
            double gamma;
            double theta;
            double rd;
            double jsucrose;
            this.CalcRsVariables(co2conc, ciguess, out solTheta, out kc, out ko, out jMax, out vcMax, out lgGamma, out wc, out pStat273, out gb, out alpha, out beta, out gamma, out theta, out rd, out jsucrose);
            this.RStomatal = this.CalcStomatalRes(co2conc, ciguess, parDirect, parDiffuse, laiSunlit, laiShaded, parSunlit, parShaded, solTheta, kc, ko, jMax, vcMax, lgGamma, wc, pStat273, gb, alpha, beta, gamma, theta, rd, jsucrose, ref gsTotal);
          }
          if (this.RMesophyll == 0.0 && this.RStomatal == 0.0)
            this.Rc = this.RCuticle == 0.0 ? 0.0 : 1.0 / (1.0 / this.Rsoil + 1.0 / this.RCuticle);
          else
            this.Rc = this.RCuticle == 0.0 ? 0.0 : 1.0 / (1.0 / (this.RStomatal + this.RMesophyll) + 1.0 / this.Rsoil + 1.0 / this.RCuticle);
        }
      }
    }

    private void CalcMesophyllRes()
    {
      double num = 0.0;
      string pollutant = this.Pollutant;
      if (!(pollutant == "O3"))
      {
        if (!(pollutant == "CO"))
        {
          if (!(pollutant == "SO2"))
          {
            if (pollutant == "NO2")
              num = 600.0;
          }
          else
            num = 0.0;
        }
        else
          num = 10.0;
      }
      else
        num = 60.0;
      this.RMesophyll = this.Lai == 0.0 ? 0.0 : num / this.Lai;
    }

    private void CalcSoilRes()
    {
      this.Rsoil = 2000.0;
      if (!this.GrowSeason || this.RainMh != 0.0)
        return;
      this.Rsoil = 2941.0;
    }

    private void CalcCuticleRes()
    {
      string pollutant = this.Pollutant;
      if (!(pollutant == "O3"))
      {
        if (!(pollutant == "SO2"))
        {
          if (!(pollutant == "NO2"))
            return;
          double num = 20000.0;
          this.RCuticle = this.Lai == 0.0 ? 0.0 : num * 1.6 / (2.0 * this.Lai);
        }
        else
        {
          double num = 8000.0;
          this.RCuticle = this.Lai == 0.0 ? 0.0 : num * 1.89 / (2.0 * this.Lai);
        }
      }
      else
      {
        double num = 10000.0;
        this.RCuticle = this.Lai == 0.0 ? 0.0 : num * 1.66 / (2.0 * this.Lai);
      }
    }

    private void CalcRsVariables(
      double co2cons,
      double ciguess,
      out double solTheta,
      out double kc,
      out double ko,
      out double jMax,
      out double vcMax,
      out double lgGamma,
      out double wc,
      out double pStat273,
      out double gb,
      out double alpha,
      out double beta,
      out double gamma,
      out double theta,
      out double rd,
      out double jsucrose)
    {
      solTheta = DryDeposition.Deg2Rad(this.SolZenAgl);
      kc = 333.0 * Math.Exp((this.TempK - 298.16) * 65120.0 / (2478.9022400000003 * this.TempK));
      ko = 295.0 * Math.Exp((this.TempK - 298.16) * 13990.0 / (2478.9022400000003 * this.TempK));
      jMax = (double) DryDeposition.JMAX25 * Math.Exp((this.TempK - 298.16) * 37000.0 / (2478.9022400000003 * this.TempK)) / (1.0 + Math.Exp((710.0 * this.TempK - 220000.0) / (8.314 * this.TempK)));
      vcMax = DryDeposition.VC25 * Math.Exp((this.TempK - 298.16) * 64637.0 / (2478.9022400000003 * this.TempK)) / (1.0 + Math.Exp((710.0 * this.TempK - 220000.0) / (8.314 * this.TempK)));
      lgGamma = 0.105 * kc * 210.0 / ko;
      wc = vcMax * (ciguess - lgGamma) / (ciguess + kc * (1.0 + 210.0 / ko));
      double num = 2.0 / (0.41 * this.Ustar * (155.0 / 222.0)) * Math.Pow(222.0 / 151.0, 0.6666);
      pStat273 = 273.16 * this.PrsMbar / 1000.0;
      gb = 1.0 / ((num + this.Ra) * (14.0 / 625.0) * 1.01325 * this.TempK / pStat273);
      alpha = 1.0 + 1.0 / 80.0 / gb - 10.0 * this.RelHum / 1.6;
      beta = co2cons * 2.0 * (gb * 10.0 * this.RelHum / 1.6 - 1.0 / 80.0 - gb);
      gamma = 4.0 * Math.Pow(co2cons, 2.0) * (1.0 / 80.0) * gb;
      theta = gb * 10.0 * this.RelHum / 1.6 * 2.0 - 1.0 / 80.0;
      rd = DryDeposition.VC25 * 0.015 * Math.Exp((this.TempK - 298.16) * 51176.0 / (2478.9022400000003 * this.TempK)) / (1.0 + Math.Exp(1.3 * (this.TempK - 328.0)));
      jsucrose = vcMax / 2.0 - rd;
    }

    private double CalcStomatalRes(
      double co2conc,
      double ciguess,
      double parDirect,
      double parDiffuse,
      double[] laiSunlit,
      double[] laiShaded,
      double[] parSunlit,
      double[] parShaded,
      double solTheta,
      double kc,
      double ko,
      double jMax,
      double vcMax,
      double lgGamma,
      double wc,
      double pStat273,
      double gb,
      double alpha,
      double beta,
      double gamma,
      double theta,
      double rd,
      double jsucrose,
      ref double gsTotal)
    {
      double num1 = 0.0;
      if ((parDirect > 0.001 || parDiffuse > 0.001) && Math.Cos(solTheta) > 2E-05 && this.Lai > 0.001)
      {
        gsTotal = 0.0;
        for (int index1 = 1; index1 <= DryDeposition.NLAY; ++index1)
        {
          int index2 = index1 - 1;
          double num2 = this.CalcStomatalConductance(co2conc, ciguess, parSunlit[index2], kc, ko, jMax, vcMax, lgGamma, wc, gb, alpha, beta, gamma, theta, rd, jsucrose, pStat273);
          double num3 = this.CalcStomatalConductance(co2conc, ciguess, parShaded[index2], kc, ko, jMax, vcMax, lgGamma, wc, gb, alpha, beta, gamma, theta, rd, jsucrose, pStat273);
          double num4 = laiSunlit[index2];
          double num5 = num2 * num4 + num3 * laiShaded[index2];
          gsTotal += num5;
        }
        gsTotal *= (double) DryDeposition.KS_VAL;
        string pollutant = this.Pollutant;
        double num6 = pollutant == "SO2" ? gsTotal * (44.0 / 83.0) : (pollutant == "NO2" ? gsTotal * (52.0 / 83.0) : gsTotal * (51.0 / 83.0));
        num1 = num6 >= 0.0 ? 1.0 / num6 : 0.0;
      }
      return num1;
    }

    private double CalcParCorrFactor(double orgPar) => 0.0028782000000000005 * orgPar / Math.Pow(1.0 + Math.Pow(0.0027, 2.0) * Math.Pow(orgPar, 2.0), 0.5);

    private double CalcStomatalConductance(
      double co2conc,
      double ciguess,
      double fracPar,
      double kc,
      double ko,
      double jMax,
      double vcMax,
      double lgGamma,
      double wc,
      double gb,
      double alpha,
      double beta,
      double gamma,
      double theta,
      double rd,
      double jsucrose,
      double pStat273)
    {
      double num1 = 0.22 * fracPar / Math.Sqrt(1.0 + Math.Pow(0.22 * fracPar, 2.0) / Math.Pow(jMax, 2.0));
      double num2;
      double num3;
      double num4;
      double num5;
      if (num1 * (ciguess - lgGamma) / (4.0 * ciguess + 8.0 * lgGamma) < wc)
      {
        num2 = num1;
        num3 = 8.0 * lgGamma;
        num4 = lgGamma;
        num5 = 4.0;
      }
      else
      {
        num2 = vcMax;
        num3 = kc * (1.0 + 210.0 / ko);
        num4 = lgGamma;
        num5 = 1.0;
      }
      double x1 = (num5 * beta + num3 * theta - num2 * alpha + num5 * alpha * rd) / (num5 * alpha);
      double num6 = (num5 * gamma + num3 * gamma / co2conc - num2 * beta + num2 * num4 * theta + num5 * rd * beta + rd * num3 * theta) / (num5 * alpha);
      double num7 = (-num2 * gamma + num2 * num4 * gamma / co2conc + num5 * rd * gamma + rd * num3 * gamma / co2conc) / (num5 * alpha);
      double x2 = (Math.Pow(x1, 2.0) - 3.0 * num6) / 9.0;
      double x3 = (2.0 * Math.Pow(x1, 3.0) - 9.0 * x1 * num6 + 27.0 * num7) / 54.0;
      double num8;
      if (Math.Pow(x2, 3.0) - Math.Pow(x3, 2.0) >= 0.0)
      {
        double x4 = x3 / Math.Sqrt(Math.Pow(x2, 3.0));
        double num9 = Math.Abs(Math.Pow(x4, 2.0)) < 1.0 ? 1.570796 - Math.Atan(x4 / Math.Sqrt(1.0 - Math.Pow(x4, 2.0))) : 0.0;
        num8 = -2.0 * Math.Pow(x2, 0.5) * Math.Cos((num9 + 12.566370616) / 3.0) - x1 / 3.0;
      }
      else
        num8 = 0.0;
      if (jsucrose < num8)
        num8 = jsucrose;
      double num10;
      if (num8 > 0.0)
      {
        double num11 = co2conc - num8 / gb;
        num10 = (10.0 * this.RelHum * num8 / num11 + 0.02) * (14.0 / 625.0) * 1.01325 * this.TempK / pStat273;
      }
      else
        num10 = 0.02 * (14.0 / 625.0) * 1.01325 * this.TempK / pStat273;
      return num10;
    }

    private static void ProcessHourlyDryDepositionPM10(
      string sPoll,
      LocationData locData,
      ForestData forData,
      ref List<SurfaceWeather> sfcList,
      ref List<HourlyMixHt> mhList,
      ForestData.RURAL_URBAN ru,
      ref List<LeafAreaIndex> laiList,
      ref List<Resistance> resList,
      int recCnt,
      ref List<AirPollutant> polList,
      int polRecCnt,
      OleDbConnection cnDryDepDB,
      string domain)
    {
      List<DryDeposition> ddList = new List<DryDeposition>();
      int num = polRecCnt / recCnt;
      int recCnt1 = 0;
      int polIdx = 0;
      for (int index1 = 0; index1 < num; ++index1)
      {
        for (int index2 = 0; index2 < recCnt; ++index2)
        {
          DryDeposition dryDeposition = new DryDeposition();
          dryDeposition.SetDryDepData(locData, forData, ref sfcList, ref mhList, ru, ref laiList, ref resList, index2, sPoll, ref polList, polIdx);
          dryDeposition.CalcDryDepositionPM10();
          if (laiList[index2].Lai == 0.0)
            dryDeposition.SetDryDepResultForLAI0();
          ddList.Add(dryDeposition);
          ++polIdx;
          ++recCnt1;
        }
      }
      DryDeposition.WriteDryDepRecords(cnDryDepDB, ref ddList, recCnt1, domain);
    }

    private void CalcDryDepositionPM10()
    {
      try
      {
        this.CheckGrowSeason();
        this.CalcDepositionVelocityPM10();
        this.AdjustMixHt();
        this.CalcPollFluxPM10();
        this.CalcAirQualityImprovement();
        this.CalcValue();
        this.CalcConcChangePM();
        this.Trans = -999.0;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void CalcDepositionVelocityPM10()
    {
      this.VdAct = this.VdMin = this.VdMax = this.VdDry = this.VdWet = 0.0;
      this.VdAct = 4.0 / 625.0 * ((1.7 + this.Lai) / 7.7);
      this.VdMin = 1.0 / 400.0 * ((1.7 + this.Lai) / 7.7);
      this.VdMax = 0.01 * ((1.7 + this.Lai) / 7.7);
      if (this.RainMh == 0.0)
      {
        this.VdDry = this.VdAct;
        this.VdWet = 0.0;
      }
      else
      {
        this.VdAct = 0.0;
        this.VdMin = 0.0;
        this.VdMax = 0.0;
      }
    }

    private void CalcPollFluxPM10()
    {
      this.Flux = this.VdAct * this.uGm3 * Math.Pow(10.0, -6.0) * 3600.0;
      this.FluxMin = this.VdMin * this.uGm3 * Math.Pow(10.0, -6.0) * 3600.0;
      this.FluxMax = this.VdMax * this.uGm3 * Math.Pow(10.0, -6.0) * 3600.0;
      this.FluxWet = this.VdWet * this.uGm3 * Math.Pow(10.0, -6.0) * 3600.0;
    }

    private void CalcConcChangePM()
    {
      this.ConcChg = this.MixHt != -999.0 ? this.uGm3 / (1.0 - this.ActAqImp / 100.0) - this.uGm3 : -999.0;
      this.ConcChgMin = this.MixHt != -999.0 ? this.uGm3 / (1.0 - this.ActAqImpMin / 100.0) - this.uGm3 : -999.0;
      this.ConcChgMax = this.MixHt != -999.0 ? this.uGm3 / (1.0 - this.ActAqImpMax / 100.0) - this.uGm3 : -999.0;
    }

    private static void ProcessHourlyDryDepositionPM25(
      string sPoll,
      LocationData locData,
      ForestData forData,
      ref List<SurfaceWeather> sfcList,
      ref List<HourlyMixHt> mhList,
      ForestData.RURAL_URBAN ru,
      ref List<LeafAreaIndex> laiList,
      ref List<Resistance> resList,
      int recCnt,
      ref List<AirPollutant> polList,
      int polRecCnt,
      OleDbConnection conn,
      string domain)
    {
      List<DryDeposition> ddList = new List<DryDeposition>();
      int num1 = polRecCnt / recCnt;
      int recCnt1 = 0;
      int polIdx = 0;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        double num2;
        double prAccumFluxMax = num2 = 0.0;
        double prAccumFluxMin = num2;
        double prAccumFlux = num2;
        for (int index2 = 0; index2 < recCnt; ++index2)
        {
          DryDeposition dryDeposition = new DryDeposition();
          dryDeposition.SetDryDepData(locData, forData, ref sfcList, ref mhList, ru, ref laiList, ref resList, index2, sPoll, ref polList, polIdx, prAccumFlux, prAccumFluxMin, prAccumFluxMax);
          dryDeposition.CalcDryDepositionPM25();
          prAccumFlux = dryDeposition.AccumFluxPM25;
          prAccumFluxMin = dryDeposition.AccumFluxPM25Min;
          prAccumFluxMax = dryDeposition.AccumFluxPM25Max;
          if (laiList[index2].Lai == 0.0)
            dryDeposition.SetDryDepResultForLAI0();
          ddList.Add(dryDeposition);
          ++polIdx;
          ++recCnt1;
        }
      }
      DryDeposition.WriteDryDepRecords(conn, ref ddList, recCnt1, domain);
    }

    private void CalcDryDepositionPM25()
    {
      try
      {
        this.CheckGrowSeason();
        this.CalcDepositionVelocityPM25();
        this.AdjustMixHt();
        this.CalcPollFluxPM25();
        this.CalcValue();
        this.CalcAirQualityImprovement();
        this.CalcConcChangePM();
        this.Trans = -999.0;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void CalcDepositionVelocityPM25()
    {
      this.VdAct = this.VdMin = this.VdMax = this.VdDry = this.VdWet = 0.0;
      this.GetDepositionVelocityForPM25();
      this.VdAct *= this.Lai;
      this.VdMin *= this.Lai;
      this.VdMax *= this.Lai;
      if (this.RainMh == 0.0)
      {
        this.VdDry = this.VdAct;
        this.VdWet = 0.0;
      }
      else
      {
        this.VdAct = 0.0;
        this.VdMin = 0.0;
        this.VdMax = 0.0;
      }
    }

    private void GetDepositionVelocityForPM25()
    {
      this.VdMin = 0.0;
      if (this.WdSpdMs < 0.5)
      {
        this.VdMin = 0.0;
        this.VdAct = 0.0;
        this.VdMax = 0.0;
      }
      else if (0.5 <= this.WdSpdMs && this.WdSpdMs < 1.5)
      {
        this.VdMin = 0.006;
        this.VdAct = 0.03;
        this.VdMax = 0.042;
      }
      else if (1.5 <= this.WdSpdMs && this.WdSpdMs < 2.5)
      {
        this.VdMin = 0.012;
        this.VdAct = 0.09;
        this.VdMax = 0.163;
      }
      else if (2.5 <= this.WdSpdMs && this.WdSpdMs < 3.5)
      {
        this.VdMin = 0.018;
        this.VdAct = 0.15;
        this.VdMax = 0.285;
      }
      else if (3.5 <= this.WdSpdMs && this.WdSpdMs < 4.5)
      {
        this.VdMin = 0.022;
        this.VdAct = 0.17;
        this.VdMax = 0.349;
      }
      else if (4.5 <= this.WdSpdMs && this.WdSpdMs < 5.5)
      {
        this.VdMin = 0.025;
        this.VdAct = 0.19;
        this.VdMax = 0.414;
      }
      else if (5.5 <= this.WdSpdMs && this.WdSpdMs < 6.5)
      {
        this.VdMin = 0.029;
        this.VdAct = 0.2;
        this.VdMax = 0.478;
      }
      else if (6.5 <= this.WdSpdMs && this.WdSpdMs < 7.5)
      {
        this.VdMin = 0.056;
        this.VdAct = 0.56;
        this.VdMax = 1.506;
      }
      else if (7.5 <= this.WdSpdMs && this.WdSpdMs < 8.5)
      {
        this.VdMin = 0.082;
        this.VdAct = 0.92;
        this.VdMax = 2.534;
      }
      else if (8.5 <= this.WdSpdMs && this.WdSpdMs < 9.5)
      {
        this.VdMin = 0.082;
        this.VdAct = 0.92;
        this.VdMax = 2.534;
      }
      else if (9.5 <= this.WdSpdMs)
      {
        this.VdMin = 0.57;
        this.VdAct = 2.11;
        this.VdMax = 7.367;
      }
      this.VdMin *= 0.01;
      this.VdAct *= 0.01;
      this.VdMax *= 0.01;
    }

    private double GetResuspensionRateForPM25()
    {
      double resuspensionRateForPm25 = 0.0;
      if (this.WdSpdMs < 0.5)
        resuspensionRateForPm25 = 0.0;
      else if (0.5 <= this.WdSpdMs && this.WdSpdMs < 1.5)
        resuspensionRateForPm25 = 1.5;
      else if (1.5 <= this.WdSpdMs && this.WdSpdMs < 2.5)
        resuspensionRateForPm25 = 3.0;
      else if (2.5 <= this.WdSpdMs && this.WdSpdMs < 3.5)
        resuspensionRateForPm25 = 4.5;
      else if (3.5 <= this.WdSpdMs && this.WdSpdMs < 4.5)
        resuspensionRateForPm25 = 6.0;
      else if (4.5 <= this.WdSpdMs && this.WdSpdMs < 5.5)
        resuspensionRateForPm25 = 7.5;
      else if (5.5 <= this.WdSpdMs && this.WdSpdMs < 6.5)
        resuspensionRateForPm25 = 9.0;
      else if (6.5 <= this.WdSpdMs && this.WdSpdMs < 7.5)
        resuspensionRateForPm25 = 10.0;
      else if (7.5 <= this.WdSpdMs && this.WdSpdMs < 8.5)
        resuspensionRateForPm25 = 11.0;
      else if (8.5 <= this.WdSpdMs && this.WdSpdMs < 9.5)
        resuspensionRateForPm25 = 12.0;
      else if (9.5 <= this.WdSpdMs && this.WdSpdMs < 10.5)
        resuspensionRateForPm25 = 13.0;
      else if (10.5 <= this.WdSpdMs && this.WdSpdMs < 11.5)
        resuspensionRateForPm25 = 16.0;
      else if (11.5 <= this.WdSpdMs && this.WdSpdMs < 12.5)
        resuspensionRateForPm25 = 20.0;
      else if (12.5 <= this.WdSpdMs)
        resuspensionRateForPm25 = 23.0;
      return resuspensionRateForPm25;
    }

    private void CalcPollFluxPM25()
    {
      double num1 = Math.Round(0.0002 * this.Lai, 10);
      double accumFluxPm25 = this.AccumFluxPM25;
      double accumFluxPm25Min = this.AccumFluxPM25Min;
      double accumFluxPm25Max = this.AccumFluxPM25Max;
      if (this.RainMh > 0.0 && Math.Round(this.VegStMh, 10) >= num1)
      {
        this.AccumFluxPM25 = this.AccumFluxPM25Max = this.AccumFluxPM25Min = 0.0;
        this.Flux = this.FluxMin = this.FluxMax = 0.0;
      }
      else
      {
        double resuspensionRateForPm25 = this.GetResuspensionRateForPM25();
        double num2 = this.VdAct * this.uGm3 * Math.Pow(10.0, -6.0) * 3600.0;
        double num3 = accumFluxPm25 + num2;
        double num4 = num3 * resuspensionRateForPm25 / 100.0;
        this.AccumFluxPM25 = num3 - num4;
        this.Flux = num2 - num4;
        if (this.MixHt != -999.0)
        {
          double num5 = this.uGm3 * this.MixHt * Math.Pow(10.0, -6.0);
          if (this.Flux < 0.0 && Math.Abs(this.Flux) > num5)
          {
            double num6 = -1.0 * num5 - this.Flux;
            double num7 = num4 - num6;
            this.AccumFluxPM25 = num3 - num7;
            this.Flux = -1.0 * num5;
          }
        }
        double num8 = this.VdMax * this.uGm3 * Math.Pow(10.0, -6.0) * 3600.0;
        double num9 = accumFluxPm25Max + num8;
        double num10 = num9 * resuspensionRateForPm25 / 100.0;
        this.AccumFluxPM25Max = num9 - num10;
        this.FluxMax = num8 - num10;
        if (this.MixHt != -999.0)
        {
          double num11 = this.uGm3 * this.MixHt * Math.Pow(10.0, -6.0);
          if (this.FluxMax < 0.0 && Math.Abs(this.FluxMax) > num11)
          {
            double num12 = -1.0 * num11 - this.FluxMax;
            double num13 = num10 - num12;
            this.AccumFluxPM25Max = num9 - num13;
            this.FluxMax = -1.0 * num11;
          }
        }
        double num14 = this.VdMin * this.uGm3 * Math.Pow(10.0, -6.0) * 3600.0;
        double num15 = accumFluxPm25Min + num14;
        double num16 = num15 * resuspensionRateForPm25 / 100.0;
        this.AccumFluxPM25Min = num15 - num16;
        this.FluxMin = num14 - num16;
        if (this.MixHt == -999.0)
          return;
        double num17 = this.uGm3 * this.MixHt * Math.Pow(10.0, -6.0);
        if (this.FluxMin >= 0.0 || Math.Abs(this.FluxMin) <= num17)
          return;
        double num18 = -1.0 * num17 - this.FluxMin;
        double num19 = num16 - num18;
        this.AccumFluxPM25Min = num15 - num19;
        this.FluxMin = -1.0 * num17;
      }
    }

    private static void CreateDryDepTable(OleDbConnection cnDryDepDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "CREATE TABLE DryDeposition([TimeStamp] DateTime,[PriPart] TEXT (100),[SecPart] TEXT (100),[TerPart] TEXT (100),[Latitude] DOUBLE,[Longitude] DOUBLE,[GMTOffset] DOUBLE,[LeafOnDOY] INT,[LeafOffDOY] INT,[MonitorState] TEXT (10),[MonitorCounty] TEXT (10),[MonitorSiteID] TEXT (10),[Pollutant] TEXT (5),[PPM] DOUBLE,[uGm3] DOUBLE,[DomainArea] DOUBLE,[EvGrnLAI] DOUBLE,[LAI] DOUBLE,[PctEvGrnCov] DOUBLE,[PctTrCov] DOUBLE,[TrCovArea] DOUBLE,[Ceiling] DOUBLE,[GrowSeason] BIT,[OpCldCov] DOUBLE,[PARuEm2s] DOUBLE,[PARWm2] DOUBLE,[PrsMbar] DOUBLE,[RainMh] DOUBLE,[RelHum] DOUBLE,[SatVPrsKpa] DOUBLE,[SolZenAgl] DOUBLE,[Stability] INT,[TempF] DOUBLE,[TempK] DOUBLE,[ToCldCov] DOUBLE,[MixHt] DOUBLE,[VapPrsKpa] DOUBLE,[WdSpdMs] DOUBLE,[IsprLtCor] DOUBLE,[Period] TEXT (1),[Ra] DOUBLE,[Rb] DOUBLE,[Rc] DOUBLE,[RCuticle] DOUBLE,[RMesophyll] DOUBLE,[Rsoil] DOUBLE,[RStomatal] DOUBLE,[Trans] DOUBLE,[Ustar] DOUBLE,[VdAct] DOUBLE,[VdDry] DOUBLE,[VdMax] DOUBLE,[VdMin] DOUBLE,[VdWet] DOUBLE,[Flux] DOUBLE,[FluxMax] DOUBLE,[FluxMin] DOUBLE,[FluxWet] DOUBLE,[AccumFluxPM25] DOUBLE,[AccumFluxPM25Max] DOUBLE,[AccumFluxPM25Min] DOUBLE,[Value] DOUBLE,[ValueMax] DOUBLE,[ValueMin] DOUBLE,[PerAqImp] DOUBLE,[PerAqImpMin] DOUBLE,[PerAqImpMax] DOUBLE,[ActAqImp] DOUBLE,[ActAqImpMin] DOUBLE,[ActAqImpMax] DOUBLE,[ConcChg] DOUBLE,[ConcChgMin] DOUBLE,[ConcChgMax] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void WriteDryDepRecords(
      OleDbConnection cnDryDepDB,
      ref List<DryDeposition> ddList,
      int recCnt,
      string domain)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        for (int index = 0; index < ddList.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO DryDeposition ([TimeStamp],[PriPart],[SecPart],[TerPart],[Latitude],[Longitude],[GMTOffset],[LeafOnDOY],[LeafOffDOY],[MonitorState],[MonitorCounty],[MonitorSiteID],[Pollutant],[PPM],[uGm3],[DomainArea],[EvGrnLAI],[LAI],[PctEvGrnCov],[PctTrCov],[TrCovArea],[Ceiling],[GrowSeason],[OpCldCov],[PARuEm2s],[PARWm2],[PrsMbar],[RainMh],[RelHum],[SatVPrsKpa],[SolZenAgl],[Stability],[TempF],[TempK],[ToCldCov],[MixHt],[VapPrsKpa],[WdSpdMs],[IsprLtCor],[Period],[Ra],[Rb],[Rc],[RCuticle],[RMesophyll],[Rsoil],[RStomatal],[Trans],[Ustar],[VdAct],[VdDry],[VdMax],[VdMin],[VdWet],[Flux],[FluxMax],[FluxMin],[FluxWet],[AccumFluxPM25],[AccumFluxPM25Max],[AccumFluxPM25Min],[Value],[ValueMax],[ValueMin],[PerAqImp],[PerAqImpMin],[PerAqImpMax],[ActAqImp],[ActAqImpMin],[ActAqImpMax],[ConcChg],[ConcChgMin],[ConcChgMax]) Values (#" + ddList[index].TimeStamp.ToString() + "#,\"" + ddList[index].PriPart + "\",\"" + ddList[index].SecPart + "\",\"" + ddList[index].TerPart + "\"," + ddList[index].Latitude.ToString() + "," + ddList[index].Longitude.ToString() + "," + ddList[index].GMTOffset.ToString() + "," + ddList[index].LeafOnDOY.ToString() + "," + ddList[index].LeafOffDOY.ToString() + ",\"" + ddList[index].MonitorState + "\",\"" + ddList[index].MonitorCounty + "\",\"" + ddList[index].MonitorSiteID + "\",\"" + ddList[index].Pollutant + "\"," + ddList[index].PPM.ToString() + "," + ddList[index].uGm3.ToString() + "," + ddList[index].DomainArea.ToString() + "," + ddList[index].EvGrnLAI.ToString() + "," + ddList[index].Lai.ToString() + "," + ddList[index].PctEvGrnCov.ToString() + "," + ddList[index].PctTrCov.ToString() + "," + ddList[index].TrCovArea.ToString() + "," + ddList[index].Ceiling.ToString() + "," + ddList[index].GrowSeason.ToString() + "," + ddList[index].OpCldCov.ToString() + "," + ddList[index].PARWm2.ToString() + "," + ddList[index].PARuEm2s.ToString() + "," + ddList[index].PrsMbar.ToString() + "," + ddList[index].RainMh.ToString() + "," + ddList[index].RelHum.ToString() + "," + ddList[index].SatVPrsKpa.ToString() + "," + ddList[index].SolZenAgl.ToString() + "," + ddList[index].Stability.ToString() + "," + ddList[index].TempF.ToString() + "," + ddList[index].TempK.ToString() + "," + ddList[index].ToCldCov.ToString() + "," + ddList[index].MixHt.ToString() + "," + ddList[index].VapPrsKpa.ToString() + "," + ddList[index].WdSpdMs.ToString() + "," + ddList[index].IsprLtCor.ToString() + ",\"" + ddList[index].Period + "\"," + ddList[index].Ra.ToString() + "," + ddList[index].Rb.ToString() + "," + ddList[index].Rc.ToString() + "," + ddList[index].RCuticle.ToString() + "," + ddList[index].RMesophyll.ToString() + "," + ddList[index].Rsoil.ToString() + "," + ddList[index].RStomatal.ToString() + "," + ddList[index].Trans.ToString() + "," + ddList[index].Ustar.ToString() + "," + ddList[index].VdAct.ToString() + "," + ddList[index].VdDry.ToString() + "," + ddList[index].VdMax.ToString() + "," + ddList[index].VdMin.ToString() + "," + ddList[index].VdWet.ToString() + "," + ddList[index].Flux.ToString() + "," + ddList[index].FluxMax.ToString() + "," + ddList[index].FluxMin.ToString() + "," + ddList[index].FluxWet.ToString() + "," + ddList[index].AccumFluxPM25.ToString() + "," + ddList[index].AccumFluxPM25Max.ToString() + "," + ddList[index].AccumFluxPM25Min.ToString() + "," + ddList[index].Value.ToString() + "," + ddList[index].ValueMax.ToString() + "," + ddList[index].ValueMin.ToString() + "," + ddList[index].PerAqImp.ToString() + "," + ddList[index].PerAqImpMin.ToString() + "," + ddList[index].PerAqImpMax.ToString() + "," + ddList[index].ActAqImp.ToString() + "," + ddList[index].ActAqImpMin.ToString() + "," + ddList[index].ActAqImpMax.ToString() + "," + ddList[index].ConcChg.ToString() + "," + ddList[index].ConcChgMin.ToString() + "," + ddList[index].ConcChgMax.ToString() + ");";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    public static int GetRecordCount(string sDryDepDB, string sPoll)
    {
      using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDryDepDB))
      {
        cnDryDepDB.Open();
        return DryDeposition.GetRecordCount(cnDryDepDB, sPoll);
      }
    }

    public static int GetRecordCount(OleDbConnection cnDryDepDB, string sPoll)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        int recordCount = 0;
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Count(*) AS NumRec FROM DryDeposition GROUP BY Pollutant,MonitorState,MonitorCounty,MonitorSiteID Having Pollutant=\"" + sPoll + "\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          if (oleDbDataReader.Read())
            recordCount = oleDbDataReader.GetInt32(0);
          oleDbDataReader.Close();
        }
        return recordCount;
      }
    }

    [Obsolete]
    public static int GetRecordCount(
      string sDryDepDB,
      string sPoll,
      string st,
      string cty,
      string id)
    {
      using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDryDepDB))
      {
        cnDryDepDB.Open();
        return DryDeposition.GetRecordCount(cnDryDepDB, sPoll, st, cty, id);
      }
    }

    public static int GetRecordCount(
      OleDbConnection cnDryDepDB,
      string sPoll,
      string st,
      string cty,
      string id)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        int recordCount = 0;
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Count(*) AS NumRec FROM DryDeposition GROUP BY Pollutant,MonitorState,MonitorCounty,MonitorSiteID HAVING  Pollutant=\"" + sPoll + "\" AND MonitorState=\"" + st + "\" AND MonitorCounty=\"" + cty + "\" AND MonitorSiteID=\"" + id + "\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          if (oleDbDataReader.Read())
            recordCount = oleDbDataReader.GetInt32(0);
          oleDbDataReader.Close();
        }
        return recordCount;
      }
    }

    [Obsolete]
    public static void ReadLightCorrFct(
      string sDryDepDB,
      string sPoll,
      ref List<DryDeposition> ddList)
    {
      using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDryDepDB))
      {
        cnDryDepDB.Open();
        DryDeposition.ReadLightCorrFct(cnDryDepDB, sPoll, ref ddList);
      }
    }

    public static void ReadLightCorrFct(
      OleDbConnection cnDryDepDB,
      string sPoll,
      ref List<DryDeposition> ddList)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW TimeStamp, Pollutant,GrowSeason,Period, TempK, Avg(Trans) as Trans, Avg(IsprLtCor) AS IsprLtCor FROM DryDeposition GROUP BY TimeStamp, Pollutant, GrowSeason, Period, TempK Having Pollutant=\"" + sPoll + "\" ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            DryDeposition dryDeposition = new DryDeposition()
            {
              TimeStamp = (DateTime) oleDbDataReader["TimeStamp"],
              GrowSeason = (bool) oleDbDataReader["GrowSeason"],
              Period = (string) oleDbDataReader["Period"],
              Trans = (double) oleDbDataReader["Trans"],
              TempK = (double) oleDbDataReader["TempK"],
              IsprLtCor = (double) oleDbDataReader["IsprLtCor"]
            };
            ddList.Add(dryDeposition);
          }
          oleDbDataReader.Close();
        }
      }
    }

    [Obsolete]
    public static void ReadDryDeposition(
      string sDryDepDB,
      string sPoll,
      ref List<DryDeposition> ddList)
    {
      using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDryDepDB))
      {
        cnDryDepDB.Open();
        DryDeposition.ReadDryDeposition(cnDryDepDB, sPoll, ref ddList);
      }
    }

    public static void ReadDryDeposition(
      OleDbConnection cnDryDepDB,
      string sPoll,
      ref List<DryDeposition> ddList)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW TimeStamp, Pollutant, GrowSeason, TrCovArea, Avg(PPM) AS PPM, Avg(uGm3) AS uGm3, Avg(MixHt) AS MixHt, Avg(Flux) AS Flux, Avg(IsprLtCor) AS IsprLtCor FROM DryDeposition GROUP BY TimeStamp, Pollutant, GrowSeason, TrCovArea Having Pollutant=\"" + sPoll + "\" ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            DryDeposition dryDeposition = new DryDeposition()
            {
              TimeStamp = (DateTime) oleDbDataReader["TimeStamp"],
              GrowSeason = (bool) oleDbDataReader["GrowSeason"],
              TrCovArea = (double) oleDbDataReader["TrCovArea"],
              PPM = (double) oleDbDataReader["PPM"],
              uGm3 = (double) oleDbDataReader["uGm3"],
              MixHt = (double) oleDbDataReader["MixHt"],
              Flux = (double) oleDbDataReader["Flux"],
              IsprLtCor = (double) oleDbDataReader["IsprLtCor"]
            };
            ddList.Add(dryDeposition);
          }
          oleDbDataReader.Close();
        }
      }
    }

    [Obsolete]
    public static void ReadPollutant(
      string sDB,
      string sPoll,
      PollutantMonitor mon,
      List<DryDeposition> list)
    {
      using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        cnDryDepDB.Open();
        DryDeposition.ReadPollutant(cnDryDepDB, sPoll, mon, list);
      }
    }

    public static void ReadPollutant(
      OleDbConnection cnDryDepDB,
      string sPoll,
      PollutantMonitor mon,
      List<DryDeposition> list)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW TimeStamp, Pollutant, PPM, uGm3, ConcChg, ConcChgMin, ConcChgMax FROM DryDeposition WHERE Pollutant=\"" + sPoll + "\" AND MonitorState=\"" + mon.MonState + "\" AND MonitorCounty=\"" + mon.MonCounty + "\" AND MonitorSiteID=\"" + mon.MonSiteID + "\" ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            DryDeposition dryDeposition = new DryDeposition()
            {
              TimeStamp = (DateTime) oleDbDataReader["TimeStamp"],
              Pollutant = (string) oleDbDataReader["Pollutant"],
              PPM = (double) oleDbDataReader["PPM"],
              uGm3 = (double) oleDbDataReader["uGm3"],
              ConcChg = (double) oleDbDataReader["ConcChg"],
              ConcChgMin = (double) oleDbDataReader["ConcChgMin"],
              ConcChgMax = (double) oleDbDataReader["ConcChgMax"]
            };
            list.Add(dryDeposition);
          }
          oleDbDataReader.Close();
        }
      }
    }

    [Obsolete]
    public static void SummarizeDryDeposition(string sDryDepDB, string domain)
    {
      using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDryDepDB))
      {
        cnDryDepDB.Open();
        DryDeposition.SummarizeDryDeposition(cnDryDepDB, domain);
      }
    }

    public static void SummarizeDryDeposition(OleDbConnection cnDryDepDB, string domain)
    {
      DryDeposition.SiteMonthlyMeans(cnDryDepDB, domain);
      DryDeposition.SiteYearlyMeans(cnDryDepDB, domain);
      DryDeposition.DomainMonthlyMeans(cnDryDepDB, domain);
      DryDeposition.DomainYearlyMeans(cnDryDepDB, domain);
      DryDeposition.DomainLeafOnMeans(cnDryDepDB, domain);
      DryDeposition.SiteMonthlySums(cnDryDepDB, domain);
      DryDeposition.SiteYearlySums(cnDryDepDB, domain);
      DryDeposition.DomainMonthlySums(cnDryDepDB, domain);
      DryDeposition.DomainYearlySums(cnDryDepDB, domain);
      DryDeposition.DomainLeafOnSums(cnDryDepDB, domain);
    }

    private static void SiteMonthlyMeans(OleDbConnection cnDryDepDB, string domain)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Month(TimeStamp) As [Month],MonitorState,MonitorCounty,MonitorSiteID,Pollutant," + domain + ", Avg(VdDry) AS [MeanVdDry (m/s)], Avg(Flux) AS [MeanFlux (g/m2)], Avg(Value) AS [MeanValue ($/m2)], Avg(PPM) AS [MeanPPM (ppm)], Avg(ugm3) AS [MeanUg (ug)] INTO 01_SiteMonthlyMeans From DryDeposition GROUP BY Month(TimeStamp),MonitorState,MonitorCounty,MonitorSiteID,Pollutant," + domain + " ORDER BY Pollutant,MonitorState, MonitorCounty, MonitorSiteID;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void SiteYearlyMeans(OleDbConnection cnDryDepDB, string domain)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Year(TimeStamp) As [Year],MonitorState,MonitorCounty,MonitorSiteID,Pollutant," + domain + ", Avg(VdDry) AS [MeanVdDry (m/s)], Avg(Flux) AS [MeanFlux (g/m2)], Avg(Value) AS [MeanValue ($/m2)], Avg(PPM) AS [MeanPPM (ppm)], Avg(ugm3) AS [MeanUg (ug)] INTO 02_SiteYearlyMeans From DryDeposition GROUP BY Year(TimeStamp),MonitorState,MonitorCounty,MonitorSiteID,Pollutant," + domain + " ORDER BY Pollutant,MonitorState, MonitorCounty, MonitorSiteID;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void DomainMonthlyMeans(OleDbConnection cnDryDepDB, string domain)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Month(TimeStamp) As [Month],Pollutant," + domain + ", Avg(VdDry) AS [MeanVdDry (m/s)], Avg(Flux) AS [MeanFlux (g/m2)], Avg(Value) AS [MeanValue ($/m2)], Avg(PPM) AS [MeanPPM (ppm)], Avg(ugm3) AS [MeanUg (ug)] INTO 03_DomainMonthlyMeans From DryDeposition GROUP BY Month(TimeStamp),Pollutant," + domain + " ORDER BY Pollutant;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void DomainYearlyMeans(OleDbConnection cnDryDepDB, string domain)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Year(TimeStamp) As [Year],Pollutant," + domain + ", Avg(VdDry) AS [MeanVdDry (m/s)], Avg(Flux) AS [MeanFlux (g/m2)], Avg(Value) AS [MeanValue ($/m2)], Avg(PPM) AS [MeanPPM (ppm)], Avg(ugm3) AS [MeanUg (ug)], Avg(PerAqImp) AS [MeanPerAqImp (%)], Avg(PerAqImpMin) AS [MeanPerAqImpMin (%)], Avg(PerAqImpMax) AS [MeanPerAqImpMax (%)], Avg(ActAqImp) AS [MeanActAqImp (%)], Avg(ActAqImpMin) AS [MeanActAqImpMin (%)], Avg(ActAqImpMax) AS [MeanActAqImpMax (%)] INTO 04_DomainYearlyMeans From DryDeposition GROUP BY Year(TimeStamp),Pollutant," + domain + " ORDER BY Pollutant;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void DomainLeafOnMeans(OleDbConnection cnDryDepDB, string domain)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Year(TimeStamp) As [Year],Pollutant, GrowSeason," + domain + ", Avg(VdDry) AS [MeanVdDry (m/s)], Avg(Flux) AS [MeanFlux (g/m2)], Avg(Value) AS [MeanValue ($/m2)], Avg(PPM) AS [MeanPPM (ppm)], Avg(ugm3) AS [MeanUg (ug)], Avg(PerAqImp) AS [MeanPerAqImp (%)], Avg(PerAqImpMin) AS [MeanPerAqImpMin (%)], Avg(PerAqImpMax) AS [MeanPerAqImpMax (%)], Avg(ActAqImp) AS [MeanActAqImp (%)], Avg(ActAqImpMin) AS [MeanActAqImpMin (%)], Avg(ActAqImpMax) AS [MeanActAqImpMax (%)] INTO 04_DomainLeafOnMeans From DryDeposition GROUP BY Year(TimeStamp),Pollutant,GrowSeason," + domain + " HAVING GrowSeason=True ORDER BY Pollutant;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void SiteMonthlySums(OleDbConnection cnDryDepDB, string domain)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Month(TimeStamp) As [Month],MonitorState,MonitorCounty,MonitorSiteID,Pollutant," + domain + ", Sum(FluxMin) AS [CumFluxMin (g/m2)],  Sum(FluxMax) AS [CumFluxMax (g/m2)],  Sum(Flux) AS [CumFlux (g/m2)],  Sum(ValueMin) AS [CumValMin ($/m2)],  Sum(ValueMax) AS [CumValMax ($/m2)],  Sum(Value) AS [CumVal ($/m2)] Into 05_SiteMonthlySums From DryDeposition GROUP BY Month(TimeStamp),MonitorState,MonitorCounty,MonitorSiteID,Pollutant," + domain + " ORDER BY Pollutant,MonitorState, MonitorCounty, MonitorSiteID;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void SiteYearlySums(OleDbConnection cnDryDepDB, string domain)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Year(TimeStamp) As [Year],MonitorState,MonitorCounty,MonitorSiteID,Pollutant," + domain + ", Sum(FluxMin) AS [CumFluxMin (g/m2)],  Sum(FluxMax) AS [CumFluxMax (g/m2)],  Sum(Flux) AS [CumFlux (g/m2)],  Sum(ValueMin) AS [CumValMin ($/m2)],  Sum(ValueMax) AS [CumValMax ($/m2)],  Sum(Value) AS [CumVal ($/m2)] Into 06_SiteYearlySums From DryDeposition GROUP BY Year(TimeStamp),MonitorState,MonitorCounty,MonitorSiteID,Pollutant," + domain + " ORDER BY Pollutant,MonitorState, MonitorCounty, MonitorSiteID;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void DomainMonthlySums(OleDbConnection cnDryDepDB, string domain)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT MonitorState,MonitorCounty,MonitorSiteID,Pollutant INTO aaa FROM DryDeposition GROUP BY MonitorState,MonitorCounty,MonitorSiteID,Pollutant ORDER BY Pollutant";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT Pollutant, Count(*) AS Num INTO SiteNum FROM aaa GROUP BY Pollutant;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Month(TimeStamp) AS [Month], DryDeposition.Pollutant, " + domain + ",Sum(Flux)/Num AS [CumFlux (g/m2)],Sum(FluxMax)/Num AS [CumFluxMax (g/m2)],Sum(FluxMin)/Num AS [CumFluxMin (g/m2)], Sum(Value)/Num AS [CumVal ($/m2)], Sum(ValueMax)/Num AS [CumValMax ($/m2)], Sum(ValueMin)/Num AS [CumValMin ($/m2)], Sum(Flux)/Num*TrCovArea/1000000 AS [FluxDomain (m-tons)], Sum(FluxMax)/Num*TrCovArea/1000000 AS [FluxDomainMax (m-tons)], Sum(FluxMin)/Num*TrCovArea/1000000 AS [FluxDomainMin (m-tons)], Sum(Value)/Num*TrCovArea/1000 AS [ValDomain ($1000)], Sum(ValueMax)/Num*TrCovArea/1000 AS [ValDomainMax ($1000)], Sum(ValueMin)/Num*TrCovArea/1000 AS [ValDomainMin ($1000)] INTO 07_DomainMonthlySums FROM DryDeposition INNER JOIN SiteNum ON DryDeposition.Pollutant = SiteNum.Pollutant GROUP BY DryDeposition.Pollutant, " + domain + ", Num, TrCovArea, Month([TimeStamp]) ORDER BY DryDeposition.Pollutant;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE aaa;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE SiteNum;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    public static void DomainYearlySums(OleDbConnection cnDryDepDB, string domain)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT MonitorState,MonitorCounty,MonitorSiteID,Pollutant INTO aaa FROM DryDeposition GROUP BY MonitorState,MonitorCounty,MonitorSiteID,Pollutant ORDER BY Pollutant";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT Pollutant, Count(*) AS Num INTO SiteNum FROM aaa GROUP BY Pollutant;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Year(TimeStamp) AS [Year], DryDeposition.Pollutant, " + domain + ", Sum(Flux)/Num AS [CumFlux (g/m2)], Sum(FluxMax)/Num AS [CumFluxMax (g/m2)], Sum(FluxMin)/Num AS [CumFluxMin (g/m2)],  Sum(Value)/Num AS [CumVal ($/m2)],  Sum(ValueMax)/Num AS [CumValMax ($/m2)],  Sum(ValueMin)/Num AS [CumValMin ($/m2)],  Sum(Flux)/Num*TrCovArea/1000000 AS [FluxDomain (m-tons)],  Sum(FluxMax)/Num*TrCovArea/1000000 AS [FluxDomainMax (m-tons)],  Sum(FluxMin)/Num*TrCovArea/1000000 AS [FluxDomainMin (m-tons)],  Sum(Value)/Num*TrCovArea/1000 AS [ValDomain ($1000)],  Sum(ValueMax)/Num*TrCovArea/1000 AS [ValDomainMax ($1000)],  Sum(ValueMin)/Num*TrCovArea/1000 AS [ValDomainMin ($1000)] INTO 08_DomainYearlySums FROM DryDeposition INNER JOIN SiteNum ON DryDeposition.Pollutant = SiteNum.Pollutant GROUP BY DryDeposition.Pollutant, " + domain + ", Num, TrCovArea, Year([TimeStamp]) ORDER BY DryDeposition.Pollutant;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE aaa;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE SiteNum;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void DomainYearlySums(string sDryDepDB, string domain)
    {
      using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDryDepDB))
      {
        cnDryDepDB.Open();
        DryDeposition.DomainYearlySums(cnDryDepDB, domain);
      }
    }

    private static void DomainLeafOnSums(OleDbConnection cnDryDepDB, string domain)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "SELECT MonitorState,MonitorCounty,MonitorSiteID,Pollutant INTO aaa FROM DryDeposition GROUP BY MonitorState,MonitorCounty,MonitorSiteID,Pollutant ORDER BY Pollutant";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT Pollutant, Count(*) AS Num INTO SiteNum FROM aaa GROUP BY Pollutant;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Year(TimeStamp) AS [Year], DryDeposition.Pollutant, " + domain + ", GrowSeason, Sum(Flux)/Num AS [CumFlux (g/m2)], Sum(FluxMax)/Num AS [CumFluxMax (g/m2)], Sum(FluxMin)/Num AS [CumFluxMin (g/m2)], Sum(Value)/Num AS [CumVal ($/m2)], Sum(ValueMax)/Num AS [CumValMax ($/m2)], Sum(ValueMin)/Num AS [CumValMin ($/m2)], Sum(Flux)/Num*TrCovArea/1000000 AS [FluxDomain (m-tons)], Sum(FluxMax)/Num*TrCovArea/1000000 AS [FluxDomainMax (m-tons)], Sum(FluxMin)/Num*TrCovArea/1000000 AS [FluxDomainMin (m-tons)], Sum(Value)/Num*TrCovArea/1000 AS [ValDomain ($1000)], Sum(ValueMax)/Num*TrCovArea/1000 AS [ValDomainMax ($1000)], Sum(ValueMin)/Num*TrCovArea/1000 AS [ValDomainMin ($1000)] INTO 09_DomainLeafOnSums FROM DryDeposition INNER JOIN SiteNum ON DryDeposition.Pollutant = SiteNum.Pollutant GROUP BY DryDeposition.Pollutant, " + domain + ",GrowSeason, Num, TrCovArea, Year([TimeStamp]) HAVING (((DryDeposition.GrowSeason)=True)) ORDER BY DryDeposition.Pollutant;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE aaa;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "DROP TABLE SiteNum;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    public enum VEG_TYPE
    {
      TREE,
      SHRUB,
      GRASS,
      CORN,
    }
  }
}
