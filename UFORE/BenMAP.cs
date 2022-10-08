// Decompiled with JetBrains decompiler
// Type: UFORE.BenMAP
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Location;

namespace UFORE
{
  public class BenMAP
  {
    public const string BM_FUNC_TABLE = "HealthImpactFunctionsTable";
    private const string POP_TABLE = "Population2010";
    private const string INT_TABLE = "IntermediateCalc";
    public const string POOL_TABLE = "Pooled";
    public const string RES_TABLE = "BenMAPResult";
    private const string NO2_TABLE = "NO2ResultsTable";
    private const string O3_TABLE = "O3ResultsTable";
    private const string PM25_TABLE = "PM25ResultsTable";
    private const string SO2_TABLE = "SO2ResultsTable";
    public const string EFFECT1 = "Hospital Admissions";
    public const string EFFECT2 = "Emergency Room Visits";
    public const string EFFECT3 = "Asthma Exacerbation";
    public const string EFFECT4 = "Acute Respiratory Symptoms";
    public const string EFFECT5 = "Mortality";
    public const string EFFECT6 = "School Loss Days";
    public const string EFFECT7 = "Acute Bronchitis";
    public const string EFFECT8 = "Acute Myocardial Infarction";
    public const string EFFECT9 = "Chronic Bronchitis";
    public const string EFFECT10 = "Hospital Admissions, Cardiovascular";
    public const string EFFECT11 = "Hospital Admissions, Respiratory";
    public const string EFFECT12 = "Lower Respiratory Symptoms";
    public const string EFFECT13 = "Upper Respiratory Symptoms";
    public const string EFFECT14 = "Work Loss Days";
    private const double NODATA = -999.0;
    public int func;
    public string priId;
    public string secId;
    public string terId;
    public string pollutant;
    public string metric;
    public string effect;
    public int startAge;
    public int endAge;
    public double population;
    public double concChg;
    public double concChgMin;
    public double concChgMax;
    public double incMult;
    public double valMult;
    public double incidence;
    public double incidenceMin;
    public double incidenceMax;
    public double value;
    public double valueMin;
    public double valueMax;
    public string metricTable;

    public BenMAP()
    {
    }

    public BenMAP(string priPartID, string secPartID, string terPartID)
    {
      this.priId = priPartID;
      this.secId = secPartID;
      this.terId = terPartID;
    }

    [Obsolete]
    public static void RegressValues(double pop, double areaKm2, double tcAreaM2, string dryDepDB)
    {
      using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + dryDepDB))
      {
        cnDryDepDB.Open();
        BenMAP.RegressValues(pop, areaKm2, tcAreaM2, cnDryDepDB);
      }
    }

    public static void RegressValues(
      double pop,
      double areaKm2,
      double tcAreaM2,
      OleDbConnection cnDryDepDB)
    {
      BenMAP.RegressValues(pop / areaKm2, tcAreaM2, cnDryDepDB);
    }

    public static void RegressValues(double popDens, double tcAreaM2, OleDbConnection cnDryDepDB)
    {
      double num1 = 0.7298 + 0.5242 * popDens;
      double num2 = 9.4667 + 3.5089 * popDens;
      double num3 = 428.0011 + 121.7864 * popDens;
      double num4 = 0.1442 + 0.191 * popDens;
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        oleDbCommand.CommandText = "UPDATE 08_DomainYearlySums SET [ValDomain ($1000)] = " + num1.ToString() + "*[FluxDomain (m-tons)]/1000,[ValDomainMax ($1000)] = " + num1.ToString() + "*[FluxDomainMax (m-tons)]/1000,[ValDomainMin ($1000)] = " + num1.ToString() + "*[FluxDomainMin (m-tons)]/1000 WHERE Pollutant='NO2';";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "UPDATE 08_DomainYearlySums SET [ValDomain ($1000)] = " + num2.ToString() + "*[FluxDomain (m-tons)]/1000,[ValDomainMax ($1000)] = " + num2.ToString() + "*[FluxDomainMax (m-tons)]/1000,[ValDomainMin ($1000)] = " + num2.ToString() + "*[FluxDomainMin (m-tons)]/1000 WHERE Pollutant='O3';";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "UPDATE 08_DomainYearlySums SET [ValDomain ($1000)] = " + num3.ToString() + "*[FluxDomain (m-tons)]/1000,[ValDomainMax ($1000)] = " + num3.ToString() + "*[FluxDomainMax (m-tons)]/1000,[ValDomainMin ($1000)] = " + num3.ToString() + "*[FluxDomainMin (m-tons)]/1000 WHERE Pollutant='PM2.5';";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "UPDATE 08_DomainYearlySums SET [ValDomain ($1000)] = " + num4.ToString() + "*[FluxDomain (m-tons)]/1000,[ValDomainMax ($1000)] = " + num4.ToString() + "*[FluxDomainMax (m-tons)]/1000,[ValDomainMin ($1000)] = " + num4.ToString() + "*[FluxDomainMin (m-tons)]/1000 WHERE Pollutant='SO2';";
        oleDbCommand.ExecuteNonQuery();
        if (tcAreaM2 == 0.0)
          return;
        oleDbCommand.CommandText = "UPDATE 08_DomainYearlySums SET [CumVal ($/m2)] = [ValDomain ($1000)]*1000/" + tcAreaM2.ToString() + ",[CumValMax ($/m2)] = [ValDomainMax ($1000)]*1000/" + tcAreaM2.ToString() + ",[CumValMin ($/m2)] = [ValDomainMin ($1000)]*1000/" + tcAreaM2.ToString() + ";";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void CalcBenMAPResult(
      string domain,
      string bmDB,
      string locDB,
      LocationData locData,
      string dryDepDB,
      string resDB)
    {
      AccessFunc.CreateDB(resDB);
      using (OleDbConnection cnBmDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + bmDB))
      {
        using (OleDbConnection cnLocDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + locDB))
        {
          using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + dryDepDB))
          {
            using (OleDbConnection cnResDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + resDB))
            {
              cnBmDB.Open();
              cnLocDB.Open();
              cnDryDepDB.Open();
              cnResDB.Open();
              BenMAP.CalcBenMAPResult(domain, cnBmDB, cnLocDB, locData, cnDryDepDB, cnResDB);
            }
          }
        }
      }
    }

    public static void CalcBenMAPResult(
      string domain,
      OleDbConnection cnBmDB,
      OleDbConnection cnLocDB,
      LocationData locData,
      OleDbConnection cnDryDepDB,
      OleDbConnection cnResDB)
    {
      List<BenMAP> benMapList1 = new List<BenMAP>();
      List<BenMAP> benMapList2 = new List<BenMAP>();
      int[] agePop = new int[100];
      BenMAP.ReadPopulation(cnLocDB, locData, domain, agePop);
      BenMAP.ReadBenMAPData(cnBmDB, locData, ref benMapList1);
      for (int index = 0; index < benMapList1.Count; ++index)
      {
        int func = index + 1;
        benMapList1[index].SetMetricTableName(func);
        benMapList1[index].CalcPopulation(agePop);
        benMapList1[index].RetrievePollConcChg(cnDryDepDB, func);
        benMapList1[index].RetrieveMultiplier(cnBmDB, locData, func);
        benMapList1[index].CalcIncidenceValue();
      }
      BenMAP.CreateIntTable(cnResDB);
      BenMAP.WriteIntTable(cnResDB, ref benMapList1);
      BenMAP.SetPooledData(locData, ref benMapList2);
      BenMAP.PoolResults(ref benMapList1, ref benMapList2);
      BenMAP.PoolResultsMin(ref benMapList1, ref benMapList2);
      BenMAP.PoolResultsMax(ref benMapList1, ref benMapList2);
      BenMAP.CreatePoolTable(cnResDB);
      BenMAP.WritePoolTable(cnResDB, ref benMapList2);
      BenMAP.CreateResults(cnDryDepDB, locData, cnResDB);
    }

    [Obsolete]
    private static void ReadPopulation(
      string locDB,
      LocationData locData,
      string domain,
      int[] agePop)
    {
      using (OleDbConnection cnLocDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + locDB))
      {
        cnLocDB.Open();
        BenMAP.ReadPopulation(cnLocDB, locData, domain, agePop);
      }
    }

    private static void ReadPopulation(
      OleDbConnection cnLocDB,
      LocationData locData,
      string domain,
      int[] agePop)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnLocDB;
        if (!(domain == "PriPart"))
        {
          if (!(domain == "SecPart"))
          {
            if (domain == "TerPart")
              oleDbCommand.CommandText = "SELECT * FROM Population2010 WHERE NationID=\"" + locData.NationID + "\" AND PrimaryPartitionID=\"" + locData.PriPartID + "\" AND SecondaryPartitionID=\"" + locData.SecPartID + "\" AND TertiaryPartitionID=\"" + locData.TerPartID + "\";";
          }
          else
            oleDbCommand.CommandText = "SELECT * FROM Population2010 WHERE NationID=\"" + locData.NationID + "\" AND PrimaryPartitionID=\"" + locData.PriPartID + "\" AND SecondaryPartitionID=\"" + locData.SecPartID + "\" AND TertiaryPartitionID=\"00000\";";
        }
        else
          oleDbCommand.CommandText = "SELECT * FROM Population2010 WHERE NationID=\"" + locData.NationID + "\" AND PrimaryPartitionID=\"" + locData.PriPartID + "\" AND SecondaryPartitionID=\"000\" AND TertiaryPartitionID=\"00000\";";
        OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
        int ordinal1 = oleDbDataReader.GetOrdinal("Under 5 years");
        oleDbDataReader.Read();
        int ordinal2 = ordinal1 + 1;
        for (int index = 0; index <= 4; ++index)
        {
          oleDbDataReader.GetDouble(ordinal2);
          agePop[index] = (int) oleDbDataReader.GetDouble(ordinal2++);
        }
        int num1 = oleDbDataReader.GetOrdinal("5 to 9 years") + 1;
        for (int index = 5; index <= 9; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num1++);
        int num2 = oleDbDataReader.GetOrdinal("10 to 14 years") + 1;
        for (int index = 10; index <= 14; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num2++);
        int num3 = oleDbDataReader.GetOrdinal("15 to 19 years") + 1;
        for (int index = 15; index <= 19; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num3++);
        int num4 = oleDbDataReader.GetOrdinal("20 to 24 years") + 1;
        for (int index = 20; index <= 24; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num4++);
        int num5 = oleDbDataReader.GetOrdinal("25 to 29 years") + 1;
        for (int index = 25; index <= 29; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num5++);
        int num6 = oleDbDataReader.GetOrdinal("30 to 34 years") + 1;
        for (int index = 30; index <= 34; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num6++);
        int num7 = oleDbDataReader.GetOrdinal("35 to 39 years") + 1;
        for (int index = 35; index <= 39; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num7++);
        int num8 = oleDbDataReader.GetOrdinal("40 to 44 years") + 1;
        for (int index = 40; index <= 44; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num8++);
        int num9 = oleDbDataReader.GetOrdinal("45 to 49 years") + 1;
        for (int index = 45; index <= 49; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num9++);
        int num10 = oleDbDataReader.GetOrdinal("50 to 54 years") + 1;
        for (int index = 50; index <= 54; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num10++);
        int num11 = oleDbDataReader.GetOrdinal("55 to 59 years") + 1;
        for (int index = 55; index <= 59; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num11++);
        int num12 = oleDbDataReader.GetOrdinal("60 to 64 years") + 1;
        for (int index = 60; index <= 64; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num12++);
        int num13 = oleDbDataReader.GetOrdinal("65 to 69 years") + 1;
        for (int index = 65; index <= 69; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num13++);
        int num14 = oleDbDataReader.GetOrdinal("70 to 74 years") + 1;
        for (int index = 70; index <= 74; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num14++);
        int num15 = oleDbDataReader.GetOrdinal("75 to 79 years") + 1;
        for (int index = 75; index <= 79; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num15++);
        int num16 = oleDbDataReader.GetOrdinal("80 to 84 years") + 1;
        for (int index = 80; index <= 84; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num16++);
        int num17 = oleDbDataReader.GetOrdinal("85 to 89 years") + 1;
        for (int index = 85; index <= 89; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num17++);
        int num18 = oleDbDataReader.GetOrdinal("90 to 94 years") + 1;
        for (int index = 90; index <= 94; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num18++);
        int num19 = oleDbDataReader.GetOrdinal("95 to 99 years") + 1;
        for (int index = 95; index <= 99; ++index)
          agePop[index] = (int) oleDbDataReader.GetDouble(num19++);
        oleDbDataReader.Close();
      }
    }

    [Obsolete]
    public static void ReadBenMAPData(string DB, LocationData locData, ref List<BenMAP> bmData)
    {
      using (OleDbConnection cnDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + DB))
      {
        cnDB.Open();
        BenMAP.ReadBenMAPData(cnDB, locData, ref bmData);
      }
    }

    public static void ReadBenMAPData(
      OleDbConnection cnDB,
      LocationData locData,
      ref List<BenMAP> bmData)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDB;
        oleDbCommand.CommandText = "SELECT * FROM HealthImpactFunctionsTable;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            BenMAP benMap = new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
            {
              func = (int) oleDbDataReader["FunctionID"],
              pollutant = (string) oleDbDataReader["Pollutant"],
              effect = (string) oleDbDataReader["EndpointGroup"],
              metric = (string) oleDbDataReader["Metric"],
              startAge = (int) (double) oleDbDataReader["StartAge"],
              endAge = (int) (double) oleDbDataReader["EndAge"]
            };
            bmData.Add(benMap);
          }
          oleDbDataReader.Close();
        }
      }
    }

    public void SetMetricTableName(int func)
    {
      if (51 <= func && func <= 57)
        this.metricTable = "11_AirPollutantMetricsQuarterlyMean";
      else
        this.metricTable = "10_AirPollutantMetricsAnnualMean";
    }

    public void CalcPopulation(int[] agePop)
    {
      for (int startAge = this.startAge; startAge <= this.endAge; ++startAge)
        this.population += (double) agePop[startAge];
    }

    public void RetrievePollConcChg(OleDbConnection cnDryDepDB, int func)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDryDepDB;
        if (51 <= func && func <= 57)
        {
          oleDbCommand.CommandText = "SELECT DISTINCTROW Pollutant, MonitorState, MonitorCounty, MonitorSiteID,  Avg(D24HourMeanChg) AS AvgData, Avg(D24HourMeanChgMin) AS AvgDataMin, Avg(D24HourMeanChgMax) AS AvgDataMax FROM " + this.metricTable + " GROUP BY Pollutant, MonitorState, MonitorCounty, MonitorSiteID HAVING (((Pollutant)=\"" + this.pollutant + "\"));";
          double num1;
          double num2;
          double num3;
          double num4;
          using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
          {
            int ordinal1 = oleDbDataReader.GetOrdinal("AvgData");
            int ordinal2 = oleDbDataReader.GetOrdinal("AvgDataMin");
            int ordinal3 = oleDbDataReader.GetOrdinal("AvgDataMax");
            double num5;
            num1 = num5 = 0.0;
            num2 = num5;
            num3 = num5;
            num4 = num5;
            while (oleDbDataReader.Read())
            {
              num4 += oleDbDataReader.GetDouble(ordinal1);
              num3 += oleDbDataReader.GetDouble(ordinal2);
              num2 += oleDbDataReader.GetDouble(ordinal3);
              ++num1;
            }
          }
          this.concChg = num4 / num1;
          this.concChgMin = num3 / num1;
          this.concChgMax = num2 / num1;
        }
        else
        {
          oleDbCommand.CommandText = "SELECT * FROM " + this.metricTable + " WHERE Pollutant=\"" + this.pollutant + "\";";
          double num6;
          double num7;
          double num8;
          double num9;
          using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
          {
            int ordinal4 = oleDbDataReader.GetOrdinal(this.metric + "Chg");
            int ordinal5 = oleDbDataReader.GetOrdinal(this.metric + "ChgMin");
            int ordinal6 = oleDbDataReader.GetOrdinal(this.metric + "ChgMax");
            double num10;
            num6 = num10 = 0.0;
            num7 = num10;
            num8 = num10;
            num9 = num10;
            while (oleDbDataReader.Read())
            {
              num9 += oleDbDataReader.GetDouble(ordinal4);
              num8 += oleDbDataReader.GetDouble(ordinal5);
              num7 += oleDbDataReader.GetDouble(ordinal6);
              ++num6;
            }
          }
          this.concChg = num9 / num6;
          this.concChgMin = num8 / num6;
          this.concChgMax = num7 / num6;
        }
      }
    }

    [Obsolete]
    public void RetrievePollConcChg(string DryDepDB, int func)
    {
      using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + DryDepDB))
      {
        cnDryDepDB.Open();
        this.RetrievePollConcChg(cnDryDepDB, func);
      }
    }

    [Obsolete]
    public void RetrieveMultiplier(string bmDB, LocationData loc, int func)
    {
      using (OleDbConnection cnBmDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + bmDB))
      {
        cnBmDB.Open();
        this.RetrieveMultiplier(cnBmDB, loc, func);
      }
    }

    public void RetrieveMultiplier(OleDbConnection cnBmDB, LocationData loc, int func)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnBmDB;
        if (1 <= func && func <= 26)
        {
          oleDbCommand.CommandText = "SELECT * FROM SO2ResultsTable WHERE [FunctionID]=" + func.ToString() + " AND [Nation ID]=\"" + loc.NationID + "\" AND [PrimaryPartitionID]=\"" + loc.PriPartID + "\" AND [SecondaryPartitionID]=\"" + loc.SecPartID + "\";";
          using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
          {
            oleDbDataReader.Read();
            this.incMult = (double) oleDbDataReader["IncidenceMultiplier"] == -999.0 ? 0.0 : (double) oleDbDataReader["IncidenceMultiplier"];
            this.valMult = (double) oleDbDataReader["ValueMultiplier"] == -999.0 ? 0.0 : (double) oleDbDataReader["ValueMultiplier"];
          }
        }
        else if (27 <= func && func <= 57)
        {
          oleDbCommand.CommandText = "SELECT * FROM PM25ResultsTable WHERE [FunctionID]=" + func.ToString() + " AND [Nation ID]=\"" + loc.NationID + "\" AND [PrimaryPartitionID]=\"" + loc.PriPartID + "\" AND [SecondaryPartitionID]=\"" + loc.SecPartID + "\";";
          using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
          {
            oleDbDataReader.Read();
            this.incMult = (double) oleDbDataReader["IncidenceMultiplier"] == -999.0 ? 0.0 : (double) oleDbDataReader["IncidenceMultiplier"];
            this.valMult = (double) oleDbDataReader["ValueMultiplier"] == -999.0 ? 0.0 : (double) oleDbDataReader["ValueMultiplier"];
          }
        }
        else if (58 <= func && func <= 72)
        {
          oleDbCommand.CommandText = "SELECT * FROM NO2ResultsTable WHERE [FunctionID]=" + func.ToString() + " AND [Nation ID]=\"" + loc.NationID + "\" AND [PrimaryPartitionID]=\"" + loc.PriPartID + "\" AND [SecondaryPartitionID]=\"" + loc.SecPartID + "\";";
          using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
          {
            oleDbDataReader.Read();
            this.incMult = (double) oleDbDataReader["IncidenceMultiplier"] == -999.0 ? 0.0 : (double) oleDbDataReader["IncidenceMultiplier"];
            this.valMult = (double) oleDbDataReader["ValueMultiplier"] == -999.0 ? 0.0 : (double) oleDbDataReader["ValueMultiplier"];
          }
        }
        else
        {
          oleDbCommand.CommandText = "SELECT * FROM O3ResultsTable WHERE [FunctionID]=" + func.ToString() + " AND [Nation ID]=\"" + loc.NationID + "\" AND [PrimaryPartitionID]=\"" + loc.PriPartID + "\" AND [SecondaryPartitionID]=\"" + loc.SecPartID + "\";";
          using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
          {
            oleDbDataReader.Read();
            this.incMult = (double) oleDbDataReader["IncidenceMultiplier"] == -999.0 ? 0.0 : (double) oleDbDataReader["IncidenceMultiplier"];
            this.valMult = (double) oleDbDataReader["ValueMultiplier"] == -999.0 ? 0.0 : (double) oleDbDataReader["ValueMultiplier"];
          }
        }
      }
    }

    public void CalcIncidenceValue()
    {
      this.incidence = this.population * this.concChg * this.incMult;
      this.incidenceMin = this.population * this.concChgMin * this.incMult;
      this.incidenceMax = this.population * this.concChgMax * this.incMult;
      this.value = this.population * this.concChg * this.valMult;
      this.valueMin = this.population * this.concChgMin * this.valMult;
      this.valueMax = this.population * this.concChgMax * this.valMult;
    }

    public static void CreateIntTable(OleDbConnection cnResDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnResDB;
        oleDbCommand.CommandText = "CREATE TABLE IntermediateCalc([FunctionID] INT,[Pollutant] TEXT (5),[AdverseHealthEffect] TEXT (255),[Metric] TEXT (255),[StartAge] INT,[EndAge] INT,[Population] INT,[ConcentrationChange] DOUBLE,[ConcentrationChangeMin] DOUBLE,[ConcentrationChangeMax] DOUBLE,[IncidenceMultiplier] DOUBLE,[ValueMultiplier] DOUBLE,[Incidence] DOUBLE,[IncidenceMin] DOUBLE,[IncidenceMax] DOUBLE,[Value] DOUBLE,[ValueMin] DOUBLE,[ValueMax] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void CreateIntTable(string resDB)
    {
      using (OleDbConnection cnResDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + resDB))
      {
        cnResDB.Open();
        BenMAP.CreateIntTable(cnResDB);
      }
    }

    public static void WriteIntTable(OleDbConnection cnResDB, ref List<BenMAP> bm)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnResDB;
        for (int index = 0; index < bm.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO IntermediateCalc ([FunctionID],[Pollutant],[AdverseHealthEffect],[Metric],[StartAge],[EndAge],[Population], [ConcentrationChange],[ConcentrationChangeMin],[ConcentrationChangeMax],[IncidenceMultiplier],[ValueMultiplier], [Incidence],[IncidenceMin],[IncidenceMax],[Value],[ValueMin],[ValueMax]) Values (" + bm[index].func.ToString() + ",\"" + bm[index].pollutant + "\",\"" + bm[index].effect + "\",\"" + bm[index].metric + "\"," + bm[index].startAge.ToString() + "," + bm[index].endAge.ToString() + "," + bm[index].population.ToString() + "," + bm[index].concChg.ToString() + "," + bm[index].concChgMin.ToString() + "," + bm[index].concChgMax.ToString() + "," + bm[index].incMult.ToString() + "," + bm[index].valMult.ToString() + "," + bm[index].incidence.ToString() + "," + bm[index].incidenceMin.ToString() + "," + bm[index].incidenceMax.ToString() + "," + bm[index].value.ToString() + "," + bm[index].valueMin.ToString() + "," + bm[index].valueMax.ToString() + ");";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    public static void WriteIntTable(string resDB, ref List<BenMAP> bm)
    {
      using (OleDbConnection cnResDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + resDB))
      {
        cnResDB.Open();
        BenMAP.WriteIntTable(cnResDB, ref bm);
      }
    }

    public static void SetPooledData(LocationData locData, ref List<BenMAP> bm)
    {
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "NO2",
        effect = "Hospital Admissions"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "NO2",
        effect = "Emergency Room Visits"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "NO2",
        effect = "Asthma Exacerbation"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "NO2",
        effect = "Acute Respiratory Symptoms"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "O3",
        effect = "Acute Respiratory Symptoms"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "O3",
        effect = "Hospital Admissions"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "O3",
        effect = "Mortality"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "O3",
        effect = "School Loss Days"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "O3",
        effect = "Emergency Room Visits"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "PM2.5",
        effect = "Acute Bronchitis"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "PM2.5",
        effect = "Acute Myocardial Infarction"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "PM2.5",
        effect = "Acute Respiratory Symptoms"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "PM2.5",
        effect = "Asthma Exacerbation"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "PM2.5",
        effect = "Chronic Bronchitis"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "PM2.5",
        effect = "Emergency Room Visits"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "PM2.5",
        effect = "Hospital Admissions, Cardiovascular"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "PM2.5",
        effect = "Hospital Admissions, Respiratory"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "PM2.5",
        effect = "Lower Respiratory Symptoms"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "PM2.5",
        effect = "Mortality"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "PM2.5",
        effect = "Upper Respiratory Symptoms"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "PM2.5",
        effect = "Work Loss Days"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "SO2",
        effect = "Acute Respiratory Symptoms"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "SO2",
        effect = "Asthma Exacerbation"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "SO2",
        effect = "Emergency Room Visits"
      });
      bm.Add(new BenMAP(locData.PriPartID, locData.SecPartID, locData.TerPartID)
      {
        pollutant = "SO2",
        effect = "Hospital Admissions"
      });
    }

    public static void PoolResults(ref List<BenMAP> intBM, ref List<BenMAP> poolBM)
    {
      int index1 = 0;
      poolBM[index1].incidence = intBM[57].incidence + intBM[58].incidence + intBM[63].incidence;
      poolBM[index1].value = intBM[57].value + intBM[58].value + intBM[63].value;
      int index2 = index1 + 1;
      poolBM[index2].incidence = (intBM[60].incidence + intBM[62].incidence) / 2.0;
      poolBM[index2].value = (intBM[60].value + intBM[62].value) / 2.0;
      int index3 = index2 + 1;
      poolBM[index3].incidence = intBM[64].incidence + intBM[65].incidence + (intBM[61].incidence + intBM[67].incidence) / 2.0 + (intBM[68].incidence * 2.0 / 3.0 + intBM[69].incidence * 2.0 / 3.0 + intBM[70].incidence * 2.0 / 3.0) / 3.0;
      poolBM[index3].value = intBM[64].value + intBM[65].value + (intBM[61].value + intBM[67].value) / 2.0 + (intBM[68].value * 2.0 / 3.0 + intBM[69].value * 2.0 / 3.0 + intBM[70].value * 2.0 / 3.0) / 3.0;
      int index4 = index3 + 1;
      poolBM[index4].incidence = intBM[66].incidence;
      poolBM[index4].value = intBM[66].value;
      int index5 = index4 + 1;
      poolBM[index5].incidence = (intBM[72].incidence + intBM[82].incidence) / 2.0;
      poolBM[index5].value = (intBM[72].value + intBM[82].value) / 2.0;
      int index6 = index5 + 1;
      poolBM[index6].incidence = (intBM[83].incidence + intBM[73].incidence) / 2.0 + (intBM[76].incidence + intBM[79].incidence) / 2.0;
      poolBM[index6].value = (intBM[83].value + intBM[73].value) / 2.0 + (intBM[76].value + intBM[79].value) / 2.0;
      int index7 = index6 + 1;
      poolBM[index7].incidence = (intBM[74].incidence + intBM[77].incidence + intBM[80].incidence) / 3.0;
      poolBM[index7].value = (intBM[74].value + intBM[77].value + intBM[80].value) / 3.0;
      int index8 = index7 + 1;
      poolBM[index8].incidence = (intBM[75].incidence + intBM[81].incidence + intBM[84].incidence) / 3.0;
      poolBM[index8].value = (intBM[75].value + intBM[81].value + intBM[84].value) / 3.0;
      int index9 = index8 + 1;
      poolBM[index9].incidence = intBM[78].incidence;
      poolBM[index9].value = intBM[78].value;
      int index10 = index9 + 1;
      poolBM[index10].incidence = intBM[52].incidence;
      poolBM[index10].value = intBM[52].value;
      int index11 = index10 + 1;
      poolBM[index11].incidence = ((intBM[26].incidence + intBM[27].incidence + intBM[28].incidence + intBM[29].incidence + intBM[30].incidence + intBM[31].incidence) / 6.0 + (intBM[38].incidence + intBM[39].incidence + intBM[40].incidence + intBM[41].incidence + intBM[42].incidence + intBM[43].incidence) / 6.0) / 2.0;
      poolBM[index11].value = ((intBM[26].value + intBM[27].value + intBM[28].value + intBM[29].value + intBM[30].value + intBM[31].value) / 6.0 + (intBM[38].value + intBM[39].value + intBM[40].value + intBM[41].value + intBM[42].value + intBM[43].value) / 6.0) / 2.0;
      int index12 = index11 + 1;
      poolBM[index12].incidence = intBM[44].incidence;
      poolBM[index12].value = intBM[44].value;
      int index13 = index12 + 1;
      poolBM[index13].incidence = intBM[32].incidence;
      poolBM[index13].value = intBM[32].value;
      int index14 = index13 + 1;
      poolBM[index14].incidence = (intBM[54].incidence + intBM[55].incidence + intBM[56].incidence) / 3.0;
      poolBM[index14].value = (intBM[54].value + intBM[55].value + intBM[56].value) / 3.0;
      int index15 = index14 + 1;
      poolBM[index15].incidence = intBM[33].incidence * 0.83 + intBM[45].incidence;
      poolBM[index15].value = intBM[33].value * 0.83 + intBM[45].value;
      int index16 = index15 + 1;
      poolBM[index16].incidence = intBM[34].incidence + intBM[46].incidence;
      poolBM[index16].value = intBM[34].value + intBM[46].value;
      int index17 = index16 + 1;
      poolBM[index17].incidence = intBM[47].incidence;
      poolBM[index17].value = intBM[47].value;
      int index18 = index17 + 1;
      poolBM[index18].incidence = intBM[49].incidence;
      poolBM[index18].value = intBM[49].value;
      int index19 = index18 + 1;
      poolBM[index19].incidence = intBM[50].incidence + intBM[53].incidence;
      poolBM[index19].value = intBM[50].value + intBM[53].value;
      int index20 = index19 + 1;
      poolBM[index20].incidence = intBM[36].incidence;
      poolBM[index20].value = intBM[36].value;
      int index21 = index20 + 1;
      poolBM[index21].incidence = intBM[37].incidence;
      poolBM[index21].value = intBM[37].value;
      int index22 = index21 + 1;
      poolBM[index22].incidence = intBM[20].incidence;
      poolBM[index22].value = intBM[20].value;
      int index23 = index22 + 1;
      poolBM[index23].incidence = (intBM[5].incidence + intBM[24].incidence) / 2.0 + intBM[19].incidence + intBM[18].incidence;
      poolBM[index23].value = (intBM[5].value + intBM[24].value) / 2.0 + intBM[19].value + intBM[18].value;
      int index24 = index23 + 1;
      poolBM[index24].incidence = (intBM[4].incidence + intBM[6].incidence + intBM[7].incidence + intBM[8].incidence + intBM[11].incidence + intBM[12].incidence + intBM[13].incidence + intBM[14].incidence + intBM[15].incidence + intBM[16].incidence + intBM[17].incidence + intBM[23].incidence) / 12.0;
      poolBM[index24].value = (intBM[4].value + intBM[6].value + intBM[7].value + intBM[8].value + intBM[11].value + intBM[12].value + intBM[13].value + intBM[14].value + intBM[15].value + intBM[16].value + intBM[17].value + intBM[23].value) / 12.0;
      int index25 = index24 + 1;
      poolBM[index25].incidence = ((intBM[10].incidence + intBM[3].incidence) / 2.0 + intBM[0].incidence + intBM[2].incidence + intBM[1].incidence) / 2.0;
      poolBM[index25].value = ((intBM[10].value + intBM[3].value) / 2.0 + intBM[0].value + intBM[2].value + intBM[1].value) / 2.0;
    }

    public static void PoolResultsMin(ref List<BenMAP> intBM, ref List<BenMAP> poolBM)
    {
      int index1 = 0;
      poolBM[index1].incidenceMin = intBM[57].incidenceMin + intBM[58].incidenceMin + intBM[63].incidenceMin;
      poolBM[index1].valueMin = intBM[57].valueMin + intBM[58].valueMin + intBM[63].valueMin;
      int index2 = index1 + 1;
      poolBM[index2].incidenceMin = (intBM[60].incidenceMin + intBM[62].incidenceMin) / 2.0;
      poolBM[index2].valueMin = (intBM[60].valueMin + intBM[62].valueMin) / 2.0;
      int index3 = index2 + 1;
      poolBM[index3].incidenceMin = intBM[64].incidenceMin + intBM[65].incidenceMin + (intBM[61].incidenceMin + intBM[67].incidenceMin) / 2.0 + (intBM[68].incidenceMin * 2.0 / 3.0 + intBM[69].incidenceMin * 2.0 / 3.0 + intBM[70].incidenceMin * 2.0 / 3.0) / 3.0;
      poolBM[index3].valueMin = intBM[64].valueMin + intBM[65].valueMin + (intBM[61].valueMin + intBM[67].valueMin) / 2.0 + (intBM[68].valueMin * 2.0 / 3.0 + intBM[69].valueMin * 2.0 / 3.0 + intBM[70].valueMin * 2.0 / 3.0) / 3.0;
      int index4 = index3 + 1;
      poolBM[index4].incidenceMin = intBM[66].incidenceMin;
      poolBM[index4].valueMin = intBM[66].valueMin;
      int index5 = index4 + 1;
      poolBM[index5].incidenceMin = (intBM[72].incidenceMin + intBM[82].incidenceMin) / 2.0;
      poolBM[index5].valueMin = (intBM[72].valueMin + intBM[82].valueMin) / 2.0;
      int index6 = index5 + 1;
      poolBM[index6].incidenceMin = (intBM[83].incidenceMin + intBM[73].incidenceMin) / 2.0 + (intBM[76].incidenceMin + intBM[79].incidenceMin) / 2.0;
      poolBM[index6].valueMin = (intBM[83].valueMin + intBM[73].valueMin) / 2.0 + (intBM[76].valueMin + intBM[79].valueMin) / 2.0;
      int index7 = index6 + 1;
      poolBM[index7].incidenceMin = (intBM[74].incidenceMin + intBM[77].incidenceMin + intBM[80].incidenceMin) / 3.0;
      poolBM[index7].valueMin = (intBM[74].valueMin + intBM[77].valueMin + intBM[80].valueMin) / 3.0;
      int index8 = index7 + 1;
      poolBM[index8].incidenceMin = (intBM[75].incidenceMin + intBM[81].incidenceMin + intBM[84].incidenceMin) / 3.0;
      poolBM[index8].valueMin = (intBM[75].valueMin + intBM[81].valueMin + intBM[84].valueMin) / 3.0;
      int index9 = index8 + 1;
      poolBM[index9].incidenceMin = intBM[78].incidenceMin;
      poolBM[index9].valueMin = intBM[78].valueMin;
      int index10 = index9 + 1;
      poolBM[index10].incidenceMin = intBM[52].incidenceMin;
      poolBM[index10].valueMin = intBM[52].valueMin;
      int index11 = index10 + 1;
      poolBM[index11].incidenceMin = ((intBM[26].incidenceMin + intBM[27].incidenceMin + intBM[28].incidenceMin + intBM[29].incidenceMin + intBM[30].incidenceMin + intBM[31].incidenceMin) / 6.0 + (intBM[38].incidenceMin + intBM[39].incidenceMin + intBM[40].incidenceMin + intBM[41].incidenceMin + intBM[42].incidenceMin + intBM[43].incidenceMin) / 6.0) / 2.0;
      poolBM[index11].valueMin = ((intBM[26].valueMin + intBM[27].valueMin + intBM[28].valueMin + intBM[29].valueMin + intBM[30].valueMin + intBM[31].valueMin) / 6.0 + (intBM[38].valueMin + intBM[39].valueMin + intBM[40].valueMin + intBM[41].valueMin + intBM[42].valueMin + intBM[43].valueMin) / 6.0) / 2.0;
      int index12 = index11 + 1;
      poolBM[index12].incidenceMin = intBM[44].incidenceMin;
      poolBM[index12].valueMin = intBM[44].valueMin;
      int index13 = index12 + 1;
      poolBM[index13].incidenceMin = intBM[32].incidenceMin;
      poolBM[index13].valueMin = intBM[32].valueMin;
      int index14 = index13 + 1;
      poolBM[index14].incidenceMin = (intBM[54].incidenceMin + intBM[55].incidenceMin + intBM[56].incidenceMin) / 3.0;
      poolBM[index14].valueMin = (intBM[54].valueMin + intBM[55].valueMin + intBM[56].valueMin) / 3.0;
      int index15 = index14 + 1;
      poolBM[index15].incidenceMin = intBM[33].incidenceMin * 0.83 + intBM[45].incidenceMin;
      poolBM[index15].valueMin = intBM[33].valueMin * 0.83 + intBM[45].valueMin;
      int index16 = index15 + 1;
      poolBM[index16].incidenceMin = intBM[34].incidenceMin + intBM[46].incidenceMin;
      poolBM[index16].valueMin = intBM[34].valueMin + intBM[46].valueMin;
      int index17 = index16 + 1;
      poolBM[index17].incidenceMin = intBM[47].incidenceMin;
      poolBM[index17].valueMin = intBM[47].valueMin;
      int index18 = index17 + 1;
      poolBM[index18].incidenceMin = intBM[49].incidenceMin;
      poolBM[index18].valueMin = intBM[49].valueMin;
      int index19 = index18 + 1;
      poolBM[index19].incidenceMin = intBM[50].incidenceMin + intBM[53].incidenceMin;
      poolBM[index19].valueMin = intBM[50].valueMin + intBM[53].valueMin;
      int index20 = index19 + 1;
      poolBM[index20].incidenceMin = intBM[36].incidenceMin;
      poolBM[index20].valueMin = intBM[36].valueMin;
      int index21 = index20 + 1;
      poolBM[index21].incidenceMin = intBM[37].incidenceMin;
      poolBM[index21].valueMin = intBM[37].valueMin;
      int index22 = index21 + 1;
      poolBM[index22].incidenceMin = intBM[20].incidenceMin;
      poolBM[index22].valueMin = intBM[20].valueMin;
      int index23 = index22 + 1;
      poolBM[index23].incidenceMin = (intBM[5].incidenceMin + intBM[24].incidenceMin) / 2.0 + intBM[19].incidenceMin + intBM[18].incidenceMin;
      poolBM[index23].valueMin = (intBM[5].valueMin + intBM[24].valueMin) / 2.0 + intBM[19].valueMin + intBM[18].valueMin;
      int index24 = index23 + 1;
      poolBM[index24].incidenceMin = (intBM[4].incidenceMin + intBM[6].incidenceMin + intBM[7].incidenceMin + intBM[8].incidenceMin + intBM[11].incidenceMin + intBM[12].incidenceMin + intBM[13].incidenceMin + intBM[14].incidenceMin + intBM[15].incidenceMin + intBM[16].incidenceMin + intBM[17].incidenceMin + intBM[23].incidenceMin) / 12.0;
      poolBM[index24].valueMin = (intBM[4].valueMin + intBM[6].valueMin + intBM[7].valueMin + intBM[8].valueMin + intBM[11].valueMin + intBM[12].valueMin + intBM[13].valueMin + intBM[14].valueMin + intBM[15].valueMin + intBM[16].valueMin + intBM[17].valueMin + intBM[23].valueMin) / 12.0;
      int index25 = index24 + 1;
      poolBM[index25].incidenceMin = ((intBM[10].incidenceMin + intBM[3].incidenceMin) / 2.0 + intBM[0].incidenceMin + intBM[2].incidenceMin + intBM[1].incidenceMin) / 2.0;
      poolBM[index25].valueMin = ((intBM[10].valueMin + intBM[3].valueMin) / 2.0 + intBM[0].valueMin + intBM[2].valueMin + intBM[1].valueMin) / 2.0;
    }

    public static void PoolResultsMax(ref List<BenMAP> intBM, ref List<BenMAP> poolBM)
    {
      int index1 = 0;
      poolBM[index1].incidenceMax = intBM[57].incidenceMax + intBM[58].incidenceMax + intBM[63].incidenceMax;
      poolBM[index1].valueMax = intBM[57].valueMax + intBM[58].valueMax + intBM[63].valueMax;
      int index2 = index1 + 1;
      poolBM[index2].incidenceMax = (intBM[60].incidenceMax + intBM[62].incidenceMax) / 2.0;
      poolBM[index2].valueMax = (intBM[60].valueMax + intBM[62].valueMax) / 2.0;
      int index3 = index2 + 1;
      poolBM[index3].incidenceMax = intBM[64].incidenceMax + intBM[65].incidenceMax + (intBM[61].incidenceMax + intBM[67].incidenceMax) / 2.0 + (intBM[68].incidenceMax * 2.0 / 3.0 + intBM[69].incidenceMax * 2.0 / 3.0 + intBM[70].incidenceMax * 2.0 / 3.0) / 3.0;
      poolBM[index3].valueMax = intBM[64].valueMax + intBM[65].valueMax + (intBM[61].valueMax + intBM[67].valueMax) / 2.0 + (intBM[68].valueMax * 2.0 / 3.0 + intBM[69].valueMax * 2.0 / 3.0 + intBM[70].valueMax * 2.0 / 3.0) / 3.0;
      int index4 = index3 + 1;
      poolBM[index4].incidenceMax = intBM[66].incidenceMax;
      poolBM[index4].valueMax = intBM[66].valueMax;
      int index5 = index4 + 1;
      poolBM[index5].incidenceMax = (intBM[72].incidenceMax + intBM[82].incidenceMax) / 2.0;
      poolBM[index5].valueMax = (intBM[72].valueMax + intBM[82].valueMax) / 2.0;
      int index6 = index5 + 1;
      poolBM[index6].incidenceMax = (intBM[83].incidenceMax + intBM[73].incidenceMax) / 2.0 + (intBM[76].incidenceMax + intBM[79].incidenceMax) / 2.0;
      poolBM[index6].valueMax = (intBM[83].valueMax + intBM[73].valueMax) / 2.0 + (intBM[76].valueMax + intBM[79].valueMax) / 2.0;
      int index7 = index6 + 1;
      poolBM[index7].incidenceMax = (intBM[74].incidenceMax + intBM[77].incidenceMax + intBM[80].incidenceMax) / 3.0;
      poolBM[index7].valueMax = (intBM[74].valueMax + intBM[77].valueMax + intBM[80].valueMax) / 3.0;
      int index8 = index7 + 1;
      poolBM[index8].incidenceMax = (intBM[75].incidenceMax + intBM[81].incidenceMax + intBM[84].incidenceMax) / 3.0;
      poolBM[index8].valueMax = (intBM[75].valueMax + intBM[81].valueMax + intBM[84].valueMax) / 3.0;
      int index9 = index8 + 1;
      poolBM[index9].incidenceMax = intBM[78].incidenceMax;
      poolBM[index9].valueMax = intBM[78].valueMax;
      int index10 = index9 + 1;
      poolBM[index10].incidenceMax = intBM[52].incidenceMax;
      poolBM[index10].valueMax = intBM[52].valueMax;
      int index11 = index10 + 1;
      poolBM[index11].incidenceMax = ((intBM[26].incidenceMax + intBM[27].incidenceMax + intBM[28].incidenceMax + intBM[29].incidenceMax + intBM[30].incidenceMax + intBM[31].incidenceMax) / 6.0 + (intBM[38].incidenceMax + intBM[39].incidenceMax + intBM[40].incidenceMax + intBM[41].incidenceMax + intBM[42].incidenceMax + intBM[43].incidenceMax) / 6.0) / 2.0;
      poolBM[index11].valueMax = ((intBM[26].valueMax + intBM[27].valueMax + intBM[28].valueMax + intBM[29].valueMax + intBM[30].valueMax + intBM[31].valueMax) / 6.0 + (intBM[38].valueMax + intBM[39].valueMax + intBM[40].valueMax + intBM[41].valueMax + intBM[42].valueMax + intBM[43].valueMax) / 6.0) / 2.0;
      int index12 = index11 + 1;
      poolBM[index12].incidenceMax = intBM[44].incidenceMax;
      poolBM[index12].valueMax = intBM[44].valueMax;
      int index13 = index12 + 1;
      poolBM[index13].incidenceMax = intBM[32].incidenceMax;
      poolBM[index13].valueMax = intBM[32].valueMax;
      int index14 = index13 + 1;
      poolBM[index14].incidenceMax = (intBM[54].incidenceMax + intBM[55].incidenceMax + intBM[56].incidenceMax) / 3.0;
      poolBM[index14].valueMax = (intBM[54].valueMax + intBM[55].valueMax + intBM[56].valueMax) / 3.0;
      int index15 = index14 + 1;
      poolBM[index15].incidenceMax = intBM[33].incidenceMax * 0.83 + intBM[45].incidenceMax;
      poolBM[index15].valueMax = intBM[33].valueMax * 0.83 + intBM[45].valueMax;
      int index16 = index15 + 1;
      poolBM[index16].incidenceMax = intBM[34].incidenceMax + intBM[46].incidenceMax;
      poolBM[index16].valueMax = intBM[34].valueMax + intBM[46].valueMax;
      int index17 = index16 + 1;
      poolBM[index17].incidenceMax = intBM[47].incidenceMax;
      poolBM[index17].valueMax = intBM[47].valueMax;
      int index18 = index17 + 1;
      poolBM[index18].incidenceMax = intBM[49].incidenceMax;
      poolBM[index18].valueMax = intBM[49].valueMax;
      int index19 = index18 + 1;
      poolBM[index19].incidenceMax = intBM[50].incidenceMax + intBM[53].incidenceMax;
      poolBM[index19].valueMax = intBM[50].valueMax + intBM[53].valueMax;
      int index20 = index19 + 1;
      poolBM[index20].incidenceMax = intBM[36].incidenceMax;
      poolBM[index20].valueMax = intBM[36].valueMax;
      int index21 = index20 + 1;
      poolBM[index21].incidenceMax = intBM[37].incidenceMax;
      poolBM[index21].valueMax = intBM[37].valueMax;
      int index22 = index21 + 1;
      poolBM[index22].incidenceMax = intBM[20].incidenceMax;
      poolBM[index22].valueMax = intBM[20].valueMax;
      int index23 = index22 + 1;
      poolBM[index23].incidenceMax = (intBM[5].incidenceMax + intBM[24].incidenceMax) / 2.0 + intBM[19].incidenceMax + intBM[18].incidenceMax;
      poolBM[index23].valueMax = (intBM[5].valueMax + intBM[24].valueMax) / 2.0 + intBM[19].valueMax + intBM[18].valueMax;
      int index24 = index23 + 1;
      poolBM[index24].incidenceMax = (intBM[4].incidenceMax + intBM[6].incidenceMax + intBM[7].incidenceMax + intBM[8].incidenceMax + intBM[11].incidenceMax + intBM[12].incidenceMax + intBM[13].incidenceMax + intBM[14].incidenceMax + intBM[15].incidenceMax + intBM[16].incidenceMax + intBM[17].incidenceMax + intBM[23].incidenceMax) / 12.0;
      poolBM[index24].valueMax = (intBM[4].valueMax + intBM[6].valueMax + intBM[7].valueMax + intBM[8].valueMax + intBM[11].valueMax + intBM[12].valueMax + intBM[13].valueMax + intBM[14].valueMax + intBM[15].valueMax + intBM[16].valueMax + intBM[17].valueMax + intBM[23].valueMax) / 12.0;
      int index25 = index24 + 1;
      poolBM[index25].incidenceMax = ((intBM[10].incidenceMax + intBM[3].incidenceMax) / 2.0 + intBM[0].incidenceMax + intBM[2].incidenceMax + intBM[1].incidenceMax) / 2.0;
      poolBM[index25].valueMax = ((intBM[10].valueMax + intBM[3].valueMax) / 2.0 + intBM[0].valueMax + intBM[2].valueMax + intBM[1].valueMax) / 2.0;
    }

    private static void CreatePoolTable(OleDbConnection cnResDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnResDB;
        oleDbCommand.CommandText = "CREATE TABLE Pooled([Pollutant] TEXT (5),[PrimaryPartitionID] TEXT (2),[SecondaryPartitionID] TEXT (3),[TertiaryPartitionID] TEXT (5),[AdverseHealthEffect] TEXT (255),[Incidence] DOUBLE,[IncidenceMin] DOUBLE,[IncidenceMax] DOUBLE,[Value] DOUBLE,[ValueMin] DOUBLE,[ValueMax] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    private static void CreatePoolTable(string resDB)
    {
      using (OleDbConnection cnResDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + resDB))
      {
        cnResDB.Open();
        BenMAP.CreatePoolTable(cnResDB);
      }
    }

    private static void WritePoolTable(OleDbConnection cnResDB, ref List<BenMAP> bm)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnResDB;
        for (int index = 0; index < bm.Count; ++index)
        {
          oleDbCommand.CommandText = "INSERT INTO Pooled([Pollutant],[PrimaryPartitionID],[SecondaryPartitionID],[TertiaryPartitionID],[AdverseHealthEffect],[Incidence],[IncidenceMin],[IncidenceMax],[Value],[ValueMin],[ValueMax]) Values (\"" + bm[index].pollutant + "\",\"" + bm[index].priId + "\",\"" + bm[index].secId + "\",\"" + bm[index].terId + "\",\"" + bm[index].effect + "\"," + bm[index].incidence.ToString() + "," + bm[index].incidenceMin.ToString() + "," + bm[index].incidenceMax.ToString() + "," + bm[index].value.ToString() + "," + bm[index].valueMin.ToString() + "," + bm[index].valueMax.ToString() + ");";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    [Obsolete]
    private static void WritePoolTable(string resDB, ref List<BenMAP> bm)
    {
      using (OleDbConnection cnResDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + resDB))
      {
        cnResDB.Open();
        BenMAP.WritePoolTable(cnResDB, ref bm);
      }
    }

    public static void CreateResults(
      OleDbConnection cnDryDepDB,
      LocationData locData,
      OleDbConnection cnResDB)
    {
      string dataSource = cnDryDepDB.DataSource;
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnResDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW Pollutant,PrimaryPartitionID,SecondaryPartitionID,TertiaryPartitionId,Sum(Value) AS [Removal Value ($/yr)],Sum(ValueMin) AS [Minimum Removal Value ($/yr)], Sum(ValueMax) AS [Maximum Removal Value ($/yr)] INTO BenMAPResult FROM Pooled GROUP BY Pollutant,PrimaryPartitionID,SecondaryPartitionID,TertiaryPartitionId;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO BenMAPResult([Pollutant],PrimaryPartitionID,SecondaryPartitionID,TertiaryPartitionId,[Removal Value ($/yr)],[Minimum Removal Value ($/yr)],[Maximum Removal Value ($/yr)]) Values (\"CO\",\"" + locData.PriPartID + "\",\"" + locData.SecPartID + "\",\"" + locData.TerPartID + "\",0,0,0)";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "INSERT INTO BenMAPResult([Pollutant],PrimaryPartitionID,SecondaryPartitionID,TertiaryPartitionId,[Removal Value ($/yr)],[Minimum Removal Value ($/yr)],[Maximum Removal Value ($/yr)]) Values (\"PM10*\",\"" + locData.PriPartID + "\",\"" + locData.SecPartID + "\",\"" + locData.TerPartID + "\",0,0,0)";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "UPDATE BenMAPResult AS RES INNER JOIN [" + dataSource + "].[08_DomainYearlySums] AS DD ON RES.Pollutant = DD.Pollutant  SET RES.[Removal Value ($/yr)] = DD.[ValDomain ($1000)]*1000, RES.[Minimum Removal Value ($/yr)] = DD.[ValDomainMin ($1000)]*1000, RES.[Maximum Removal Value ($/yr)] = DD.[ValDomainMax ($1000)]*1000 WHERE RES.Pollutant=\"CO\" OR RES.Pollutant=\"PM10*\";";
        oleDbCommand.ExecuteNonQuery();
        AccessFunc.AppendField(cnResDB, "BenMAPResult", "Pollution Removal (tons/yr)", "DOUBLE");
        AccessFunc.AppendField(cnResDB, "BenMAPResult", "Minimum Pollution Removal (tons/yr)", "DOUBLE");
        AccessFunc.AppendField(cnResDB, "BenMAPResult", "Maximum Pollution Removal (tons/yr)", "DOUBLE");
        AccessFunc.AppendField(cnResDB, "BenMAPResult", "Annual Mean of Concentration Change (ppb or µg/m3)", "DOUBLE");
        AccessFunc.AppendField(cnResDB, "BenMAPResult", "Annual Mean of Minimum Concentration Change (ppb or µg/m3)", "DOUBLE");
        AccessFunc.AppendField(cnResDB, "BenMAPResult", "Annual Mean of Maximum Concentration Change (ppb or µg/m3)", "DOUBLE");
        AccessFunc.AppendField(cnResDB, "BenMAPResult", "Leaf On Mean of Concentration Change (ppb or µg/m3)", "DOUBLE");
        AccessFunc.AppendField(cnResDB, "BenMAPResult", "Leaf On Mean of Minimum Concentration Change (ppb or µg/m3)", "DOUBLE");
        AccessFunc.AppendField(cnResDB, "BenMAPResult", "Leaf On Mean of Maximum Concentration Change (ppb or µg/m3)", "DOUBLE");
        oleDbCommand.CommandText = "UPDATE BenMAPResult AS RES INNER JOIN [" + dataSource + "].[08_DomainYearlySums] AS DD ON RES.Pollutant = DD.Pollutant SET RES.[Pollution Removal (tons/yr)] = DD.[FluxDomain (m-tons)],  RES.[Minimum Pollution Removal (tons/yr)] = DD.[FluxDomainMin (m-tons)],  RES.[Maximum Pollution Removal (tons/yr)] = DD.[FluxDomainMax (m-tons)];";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Pollutant, Avg(AvgConcChg) AS [Avg Of AvgConcChg], Avg(AvgConcChgMin) AS [Avg Of AvgConcChgMin], Avg(AvgConcChgMax) AS [Avg Of AvgConcChgMax] INTO annualMean FROM [" + dataSource + "].[12_ConcentrationChangeAnnualMean] AS DD GROUP BY DD.Pollutant;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "SELECT DISTINCTROW Pollutant, Avg(AvgConcChg) AS [Avg Of AvgConcChg], Avg(AvgConcChgMin) AS [Avg Of AvgConcChgMin], Avg(AvgConcChgMax) AS [Avg Of AvgConcChgMax] INTO leafOnMean FROM [" + dataSource + "].[13_ConcentrationChangeLeafOnMean] AS DD GROUP BY DD.Pollutant;";
        oleDbCommand.ExecuteNonQuery();
        oleDbCommand.CommandText = "UPDATE leafOnMean  INNER JOIN (annualMean  INNER JOIN BenMAPResult ON annualMean.Pollutant = BenMAPResult.Pollutant) ON leafOnMean.Pollutant = BenMAPResult.Pollutant  SET BenMAPResult.[Annual Mean of Concentration Change (ppb or µg/m3)] = [annualMean].[Avg Of AvgConcChg],BenMAPResult.[Annual Mean of Minimum Concentration Change (ppb or µg/m3)] = [annualMean].[Avg Of AvgConcChgMin],BenMAPResult.[Annual Mean of Maximum Concentration Change (ppb or µg/m3)] = [annualMean].[Avg Of AvgConcChg],BenMAPResult.[Leaf On Mean of Concentration Change (ppb or µg/m3)] = [leafOnMean].[Avg Of AvgConcChg],BenMAPResult.[Leaf On Mean of Minimum Concentration Change (ppb or µg/m3)] = [leafOnMean].[Avg Of AvgConcChg],BenMAPResult.[Leaf On Mean of Maximum Concentration Change (ppb or µg/m3)] = [leafOnMean].[Avg Of AvgConcChg];";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void CreateResults(string ddDB, LocationData locData, string resDB)
    {
      using (OleDbConnection cnResDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + resDB))
      {
        using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + ddDB))
        {
          cnResDB.Open();
          cnDryDepDB.Open();
          BenMAP.CreateResults(cnDryDepDB, locData, cnResDB);
        }
      }
    }
  }
}
