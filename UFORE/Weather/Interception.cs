// Decompiled with JetBrains decompiler
// Type: UFORE.Weather.Interception
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Data.OleDb;
using UFORE.Deposition;

namespace UFORE.Weather
{
  public class Interception
  {
    public const string INTRCPT_TABLE = "RainIntercept";
    public const string YEARLY_INTCPT_TABLE = "01_InterceptYearlySums";
    private const string LEAFON_INTCPT_TABLE = "02_InterceptLeafOnSums";
    private const string MONTHLY_INTCPT_TABLE = "03_InterceptMonthlySums";
    private const string HOURLY_INTCPT_TABLE = "04_InterceptHourly";
    private const double IMPERVIOUS_PCT = 25.5;
    private const double PERVIOUS_PCT = 74.5;
    public DryDeposition.VEG_TYPE VegType;
    public double Area;
    public double TreeCoverPct;
    public string WeatherDB;
    private string DryDepDB;
    public string InterceptDB;
    private string FinIntcptDB;

    public Interception(
      DryDeposition.VEG_TYPE sVegType,
      double dArea,
      double dTrCov,
      string sWeatherDB,
      string sDrydepDB,
      string sIntcptDB,
      string sFinDB)
    {
      this.VegType = sVegType;
      this.Area = dArea;
      this.TreeCoverPct = dTrCov;
      this.WeatherDB = sWeatherDB;
      this.DryDepDB = sDrydepDB;
      this.InterceptDB = sIntcptDB;
      this.FinIntcptDB = sFinDB;
    }

    public Interception()
    {
    }

    public void SummarizeIntercept()
    {
      AccessFunc.CreateDB(this.InterceptDB);
      this.JoinSfcAndDepTables(this.WeatherDB, "SurfaceWeather", this.DryDepDB, "DryDeposition", this.InterceptDB);
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.InterceptDB))
      {
        conn.Open();
        this.HourlySums(conn, "RainIntercept");
        this.MonthlySums(conn, "RainIntercept");
        this.YearlySums(conn, "RainIntercept");
        this.LeafOnSums(conn, "RainIntercept");
        conn.Close();
      }
      this.CreateFinalDB();
    }

    private void JoinSfcAndDepTables(
      string sfcDB,
      string sfcTbl,
      string depDB,
      string depTbl,
      string intcptDB)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + depDB))
      {
        OleDbCommand oleDbCommand = new OleDbCommand();
        string[] strArray = new string[5]
        {
          "NO2",
          "O3",
          "PM10",
          "PM25",
          "SO2"
        };
        try
        {
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          int index;
          for (index = 0; index < strArray.Length; ++index)
          {
            oleDbCommand.CommandText = "SELECT DISTINCTROW Pollutant, Count(*) AS [RecCnt] FROM " + depTbl + " GROUP BY Pollutant HAVING Pollutant=\"" + strArray[index] + "\";";
            OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
            oleDbDataReader.Read();
            if (oleDbDataReader.GetInt32(1) > 0)
            {
              oleDbDataReader.Close();
              break;
            }
            oleDbDataReader.Close();
          }
          oleDbCommand.CommandText = "SELECT DISTINCTROW MonitorState, MonitorCounty, MonitorSiteID, Pollutant, Count(*) AS [MonCount] FROM " + depTbl + " GROUP BY MonitorState, MonitorCounty, MonitorSiteID, Pollutant HAVING Pollutant=\"" + strArray[index] + "\";";
          OleDbDataReader oleDbDataReader1 = oleDbCommand.ExecuteReader();
          oleDbDataReader1.Read();
          string str1 = (string) oleDbDataReader1["MonitorState"];
          string str2 = (string) oleDbDataReader1["MonitorCounty"];
          string str3 = (string) oleDbDataReader1["MonitorSiteID"];
          oleDbDataReader1.Close();
          oleDbCommand.CommandText = "SELECT SFC.TimeStamp, " + (this.Area * this.TreeCoverPct / 100.0).ToString() + " AS [TreeCover (m2)], SFC.RainMh, SFC.PeTrMh, SFC.PtTrMh, SFC.VegStMh, SFC.VegEvMh, SFC.VegIntcptMh, SFC.UnderCanThrufallMh, SFC.UnderCanPervStMh, SFC.UnderCanPervEvMh, SFC.UnderCanPervInfilMh,  SFC.UnderCanImpervStMh, SFC.UnderCanImpervEvMh, SFC.UnderCanImpervRunoffMh,  SFC.NoCanPervStMh, SFC.NoCanPervEvMh, SFC.NoCanPervInfilMh,  SFC.NoCanImpervStMh, SFC.NoCanImpervEvMh, SFC.NoCanImpervRunoffMh,  DEP.MonitorState, DEP.MonitorCounty, DEP.MonitorSiteID, DEP.Trans AS TransMh, DEP.GrowSeason INTO [" + intcptDB + "].[RainIntercept] FROM " + depTbl + " AS DEP INNER JOIN [" + sfcDB + "].[" + sfcTbl + "] AS SFC ON DEP.[TimeStamp] = SFC.[TimeStamp] WHERE DEP.Pollutant=\"" + strArray[index] + "\" AND DEP.MonitorState=\"" + str1 + "\" AND DEP.MonitorCounty=\"" + str2 + "\" AND DEP.MonitorSiteID=\"" + str3 + "\";";
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

    private void HourlySums(OleDbConnection conn, string sTbl)
    {
      using (OleDbCommand oleDbCommand1 = new OleDbCommand())
      {
        double num1 = this.Area * this.TreeCoverPct / 100.0;
        double num2 = this.Area * (100.0 - this.TreeCoverPct) / 100.0;
        double num3 = num1 * 74.5 / 100.0;
        double num4 = num1 * 25.5 / 100.0;
        double num5 = num2 * 74.5 / 100.0;
        double num6 = num2 * 25.5 / 100.0;
        double num7 = this.Area * 74.5 / 100.0;
        double num8 = this.Area * 25.5 / 100.0;
        try
        {
          oleDbCommand1.Connection = conn;
          OleDbCommand oleDbCommand2 = oleDbCommand1;
          string[] strArray = new string[59];
          strArray[0] = "SELECT DISTINCTROW TimeStamp,RainMh AS [Rain (m/h)],RainMh * ";
          strArray[1] = this.Area.ToString();
          strArray[2] = " AS [Rain (m3/h)],PeTrMh AS [Potential Evaporation (m/h)],PeTrMh * ";
          strArray[3] = num1.ToString();
          strArray[4] = " AS [Potential Evaporation (m3/h)],VegEvMh As [Evaporation (m/h)],VegEvMh * ";
          strArray[5] = num1.ToString();
          strArray[6] = " AS [Evaporation (m3/h)],PtTrMh AS [Potential Evapotranspiration (m/h)],PtTrMh * ";
          strArray[7] = num1.ToString();
          strArray[8] = " AS [Potential Evapotranspiration (m3/h)],TransMh AS [Transpiration (m/h)],TransMh * ";
          strArray[9] = num1.ToString();
          strArray[10] = " AS [Transpiration (m3/h)],VegIntcptMh AS [Veg Intercept no Adjust (m/h)],VegIntcptMh * ";
          strArray[11] = num1.ToString();
          strArray[12] = " AS [Veg Intercept no Adjust (m3/h)],";
          string str1;
          if (num1 != 0.0)
            str1 = "(NoCanImpervRunoffMh * " + num8.ToString() + " - (UnderCanImpervRunoffMh * " + num4.ToString() + " + NoCanImpervRunoffMh * " + num6.ToString() + "))/" + num1.ToString();
          else
            str1 = "0";
          strArray[13] = str1;
          strArray[14] = " AS [VegIntercept (m/h)],NoCanImpervRunoffMh * ";
          strArray[15] = num8.ToString();
          strArray[16] = " - (UnderCanImpervRunoffMh * ";
          strArray[17] = num4.ToString();
          strArray[18] = " + NoCanImpervRunoffMh * ";
          strArray[19] = num6.ToString();
          strArray[20] = ") AS [VegIntercept (m3/h)],RainMh * ";
          strArray[21] = num1.ToString();
          strArray[22] = " AS [Rain on Canopy (m3/h)],RainMh * ";
          strArray[23] = num2.ToString();
          strArray[24] = " AS [Rain on No Canopy (m3/h)],UnderCanThrufallMh AS [Under Canopy Thrufall (m/h)],UnderCanThrufallMh * ";
          strArray[25] = num1.ToString();
          strArray[26] = " AS [Under Canopy Thrufall (m3/h)],UnderCanPervInfilMh AS [Under Canopy Infiltration (m/h)],UnderCanPervInfilMh * ";
          strArray[27] = num3.ToString();
          strArray[28] = " AS [Under Canopy Infiltration (m3/h)],UnderCanImpervRunoffMh AS [Under Canopy Runoff (m/h)],UnderCanImpervRunoffMh * ";
          strArray[29] = num4.ToString();
          strArray[30] = " AS [Under Canopy Runoff (m3/h)],NoCanPervInfilMh AS [No Canopy Infiltration (m/h)],NoCanPervInfilMh * ";
          strArray[31] = num5.ToString();
          strArray[32] = " AS [No Canopy Infiltration (m3/h)],NoCanImpervRunoffMh AS [No Canopy Runoff (m/h)],NoCanImpervRunoffMh * ";
          strArray[33] = num6.ToString();
          strArray[34] = " AS [No Canopy Runoff (m3/h)],RainMh * ";
          strArray[35] = num2.ToString();
          strArray[36] = " + UnderCanThrufallMh * ";
          strArray[37] = num1.ToString();
          strArray[38] = " - (UnderCanPervInfilMh * ";
          strArray[39] = num3.ToString();
          strArray[40] = " + UnderCanImpervRunoffMh * ";
          strArray[41] = num4.ToString();
          strArray[42] = " + NoCanPervInfilMh * ";
          strArray[43] = num5.ToString();
          strArray[44] = " + NoCanImpervRunoffMh * ";
          strArray[45] = num6.ToString();
          strArray[46] = ") AS [Ground Intercept (m3/h)],NoCanPervInfilMh AS [No Tree Cover Infiltration (m/h)],NoCanPervInfilMh * ";
          strArray[47] = num7.ToString();
          strArray[48] = " AS [No Tree Cover Infiltration (m3/h)],NoCanImpervRunoffMh AS [No Tree Cover Runoff (m/h)],NoCanImpervRunoffMh * ";
          strArray[49] = num8.ToString();
          strArray[50] = " AS [No Tree Cover Runoff (m3/h)],RainMh * ";
          strArray[51] = this.Area.ToString();
          strArray[52] = " - (NoCanPervInfilMh * ";
          strArray[53] = num7.ToString();
          strArray[54] = " + NoCanImpervRunoffMh * ";
          strArray[55] = num8.ToString();
          strArray[56] = ") AS [No Tree Cover Ground Intercept (m3/h)] INTO 04_InterceptHourly FROM ";
          strArray[57] = sTbl;
          strArray[58] = ";";
          string str2 = string.Concat(strArray);
          oleDbCommand2.CommandText = str2;
          oleDbCommand1.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    private void MonthlySums(OleDbConnection conn, string sTbl)
    {
      using (OleDbCommand oleDbCommand1 = new OleDbCommand())
      {
        double num1 = this.Area * this.TreeCoverPct / 100.0;
        double num2 = this.Area * (100.0 - this.TreeCoverPct) / 100.0;
        double num3 = num1 * 74.5 / 100.0;
        double num4 = num1 * 25.5 / 100.0;
        double num5 = num2 * 74.5 / 100.0;
        double num6 = num2 * 25.5 / 100.0;
        double num7 = this.Area * 74.5 / 100.0;
        double num8 = this.Area * 25.5 / 100.0;
        try
        {
          oleDbCommand1.Connection = conn;
          OleDbCommand oleDbCommand2 = oleDbCommand1;
          string[] strArray = new string[59];
          strArray[0] = "SELECT DISTINCTROW Month(TimeStamp) AS [Month],Sum(RainMh) AS [Rain (m/m)],Sum(RainMh) * ";
          strArray[1] = this.Area.ToString();
          strArray[2] = " AS [Rain (m3/m)],Sum(PeTrMh) AS [Potential Evaporation (m/mo)],Sum(PeTrMh) * ";
          strArray[3] = num1.ToString();
          strArray[4] = " AS [Potential Evaporation (m3/mo)],Sum(VegEvMh) As [Evaporation (m/mo)],Sum(VegEvMh) * ";
          strArray[5] = num1.ToString();
          strArray[6] = " AS [Evaporation (m3/mo)],Sum(PtTrMh) AS [Potential Evapotranspiration (m/mo)],Sum(PtTrMh) * ";
          strArray[7] = num1.ToString();
          strArray[8] = " AS [Potential Evapotranspiration (m3/mo)],Sum(TransMh) AS [Transpiration (m/mo)],Sum(TransMh) * ";
          strArray[9] = num1.ToString();
          strArray[10] = " AS [Transpiration (m3/mo)],Sum(VegIntcptMh) AS [Veg Intercept no Adjust (m/mo)],Sum(VegIntcptMh) * ";
          strArray[11] = num1.ToString();
          strArray[12] = " AS [Veg Intercept no Adjust (m3/mo)],";
          string str1;
          if (num1 != 0.0)
            str1 = "(Sum(NoCanImpervRunoffMh) * " + num8.ToString() + " - (Sum(UnderCanImpervRunoffMh) * " + num4.ToString() + " + Sum(NoCanImpervRunoffMh) * " + num6.ToString() + "))/" + num1.ToString();
          else
            str1 = "0";
          strArray[13] = str1;
          strArray[14] = " AS [VegIntercept (m/mo)],Sum(NoCanImpervRunoffMh) * ";
          strArray[15] = num8.ToString();
          strArray[16] = " - (Sum(UnderCanImpervRunoffMh) * ";
          strArray[17] = num4.ToString();
          strArray[18] = " + Sum(NoCanImpervRunoffMh) * ";
          strArray[19] = num6.ToString();
          strArray[20] = ") AS [VegIntercept (m3/mo)],Sum(RainMh) * ";
          strArray[21] = num1.ToString();
          strArray[22] = " AS [Rain on Canopy (m3/mo)],Sum(RainMh) * ";
          strArray[23] = num2.ToString();
          strArray[24] = " AS [Rain on No Canopy (m3/mo)],Sum(UnderCanThrufallMh) AS [Under Canopy Thrufall (m/mo)],Sum(UnderCanThrufallMh) * ";
          strArray[25] = num1.ToString();
          strArray[26] = " AS [Under Canopy Thrufall (m3/mo)],Sum(UnderCanPervInfilMh) AS [Under Canopy Infiltration (m/mo)],Sum(UnderCanPervInfilMh) * ";
          strArray[27] = num3.ToString();
          strArray[28] = " AS [Under Canopy Infiltration (m3/mo)],Sum(UnderCanImpervRunoffMh) AS [Under Canopy Runoff (m/mo)],Sum(UnderCanImpervRunoffMh) * ";
          strArray[29] = num4.ToString();
          strArray[30] = " AS [Under Canopy Runoff (m3/mo)],Sum(NoCanPervInfilMh) AS [No Canopy Infiltration (m/mo)],Sum(NoCanPervInfilMh) * ";
          strArray[31] = num5.ToString();
          strArray[32] = " AS [No Canopy Infiltration (m3/mo)],Sum(NoCanImpervRunoffMh) AS [No Canopy Runoff (m/mo)],Sum(NoCanImpervRunoffMh) * ";
          strArray[33] = num6.ToString();
          strArray[34] = " AS [No Canopy Runoff (m3/mo)],Sum(RainMh) * ";
          strArray[35] = num2.ToString();
          strArray[36] = " + Sum(UnderCanThrufallMh) * ";
          strArray[37] = num1.ToString();
          strArray[38] = " - (Sum(UnderCanPervInfilMh) * ";
          strArray[39] = num3.ToString();
          strArray[40] = " + Sum(UnderCanImpervRunoffMh) * ";
          strArray[41] = num4.ToString();
          strArray[42] = " + Sum(NoCanPervInfilMh) * ";
          strArray[43] = num5.ToString();
          strArray[44] = " + Sum(NoCanImpervRunoffMh) * ";
          strArray[45] = num6.ToString();
          strArray[46] = ") AS [Ground Intercept (m3/mo)],Sum(NoCanPervInfilMh) AS [No Tree Cover Infiltration (m/mo)],Sum(NoCanPervInfilMh) * ";
          strArray[47] = num7.ToString();
          strArray[48] = " AS [No Tree Cover Infiltration (m3/mo)],Sum(NoCanImpervRunoffMh) AS [No Tree Cover Runoff (m/mo)],Sum(NoCanImpervRunoffMh) * ";
          strArray[49] = num8.ToString();
          strArray[50] = " AS [No Tree Cover Runoff (m3/mo)],Sum(RainMh) * ";
          strArray[51] = this.Area.ToString();
          strArray[52] = " - (Sum(NoCanPervInfilMh) * ";
          strArray[53] = num7.ToString();
          strArray[54] = " + Sum(NoCanImpervRunoffMh) * ";
          strArray[55] = num8.ToString();
          strArray[56] = ") AS [No Tree Cover Ground Intercept (m3/mo)] INTO 03_InterceptMonthlySums FROM ";
          strArray[57] = sTbl;
          strArray[58] = " GROUP BY Month(TimeStamp),MonitorState,MonitorCounty,MonitorSiteID;";
          string str2 = string.Concat(strArray);
          oleDbCommand2.CommandText = str2;
          oleDbCommand1.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    private void YearlySums(OleDbConnection conn, string sTbl)
    {
      using (OleDbCommand oleDbCommand1 = new OleDbCommand())
      {
        double num1 = this.Area * this.TreeCoverPct / 100.0;
        double num2 = this.Area * (100.0 - this.TreeCoverPct) / 100.0;
        double num3 = num1 * 74.5 / 100.0;
        double num4 = num1 * 25.5 / 100.0;
        double num5 = num2 * 74.5 / 100.0;
        double num6 = num2 * 25.5 / 100.0;
        double num7 = this.Area * 74.5 / 100.0;
        double num8 = this.Area * 25.5 / 100.0;
        try
        {
          oleDbCommand1.Connection = conn;
          OleDbCommand oleDbCommand2 = oleDbCommand1;
          string[] strArray = new string[59];
          strArray[0] = "SELECT DISTINCTROW Sum(RainMh) AS [Rain (m/yr)],Sum(RainMh) * ";
          strArray[1] = this.Area.ToString();
          strArray[2] = " AS [Rain (m3/yr)],Sum(PeTrMh) AS [Potential Evaporation (m/yr)],Sum(PeTrMh) * ";
          strArray[3] = num1.ToString();
          strArray[4] = " AS [Potential Evaporation (m3/yr)],Sum(VegEvMh) As [Evaporation (m/yr)],Sum(VegEvMh) * ";
          strArray[5] = num1.ToString();
          strArray[6] = " AS [Evaporation (m3/yr)],Sum(PtTrMh) AS [Potential Evapotranspiration (m/yr)],Sum(PtTrMh) * ";
          strArray[7] = num1.ToString();
          strArray[8] = " AS [Potential Evapotranspiration (m3/yr)],Sum(TransMh) AS [Transpiration (m/yr)],Sum(TransMh) * ";
          strArray[9] = num1.ToString();
          strArray[10] = " AS [Transpiration (m3/yr)],Sum(VegIntcptMh) AS [Veg Intercept no Adjust (m/yr)],Sum(VegIntcptMh) * ";
          strArray[11] = num1.ToString();
          strArray[12] = " AS [Veg Intercept no Adjust (m3/yr)],";
          string str1;
          if (num1 != 0.0)
            str1 = "(Sum(NoCanImpervRunoffMh) * " + num8.ToString() + " - (Sum(UnderCanImpervRunoffMh) * " + num4.ToString() + " + Sum(NoCanImpervRunoffMh) * " + num6.ToString() + "))/" + num1.ToString();
          else
            str1 = "0";
          strArray[13] = str1;
          strArray[14] = " AS [VegIntercept (m/yr)],Sum(NoCanImpervRunoffMh) * ";
          strArray[15] = num8.ToString();
          strArray[16] = " - (Sum(UnderCanImpervRunoffMh) * ";
          strArray[17] = num4.ToString();
          strArray[18] = " + Sum(NoCanImpervRunoffMh) * ";
          strArray[19] = num6.ToString();
          strArray[20] = ") AS [VegIntercept (m3/yr)],Sum(RainMh) * ";
          strArray[21] = num1.ToString();
          strArray[22] = " AS [Rain on Canopy (m3/yr)],Sum(RainMh) * ";
          strArray[23] = num2.ToString();
          strArray[24] = " AS [Rain on No Canopy (m3/yr)],Sum(UnderCanThrufallMh) AS [Under Canopy Thrufall (m/yr)],Sum(UnderCanThrufallMh) * ";
          strArray[25] = num1.ToString();
          strArray[26] = " AS [Under Canopy Thrufall (m3/yr)],Sum(UnderCanPervInfilMh) AS [Under Canopy Infiltration (m/yr)],Sum(UnderCanPervInfilMh) * ";
          strArray[27] = num3.ToString();
          strArray[28] = " AS [Under Canopy Infiltration (m3/yr)],Sum(UnderCanImpervRunoffMh) AS [Under Canopy Runoff (m/yr)],Sum(UnderCanImpervRunoffMh) * ";
          strArray[29] = num4.ToString();
          strArray[30] = " AS [Under Canopy Runoff (m3/yr)],Sum(NoCanPervInfilMh) AS [No Canopy Infiltration (m/yr)],Sum(NoCanPervInfilMh) * ";
          strArray[31] = num5.ToString();
          strArray[32] = " AS [No Canopy Infiltration (m3/yr)],Sum(NoCanImpervRunoffMh) AS [No Canopy Runoff (m/yr)],Sum(NoCanImpervRunoffMh) * ";
          strArray[33] = num6.ToString();
          strArray[34] = " AS [No Canopy Runoff (m3/yr)],Sum(RainMh) * ";
          strArray[35] = num2.ToString();
          strArray[36] = " + Sum(UnderCanThrufallMh) * ";
          strArray[37] = num1.ToString();
          strArray[38] = " - (Sum(UnderCanPervInfilMh) * ";
          strArray[39] = num3.ToString();
          strArray[40] = " + Sum(UnderCanImpervRunoffMh) * ";
          strArray[41] = num4.ToString();
          strArray[42] = " + Sum(NoCanPervInfilMh) * ";
          strArray[43] = num5.ToString();
          strArray[44] = " + Sum(NoCanImpervRunoffMh) * ";
          strArray[45] = num6.ToString();
          strArray[46] = ") AS [Ground Intercept (m3/yr)],Sum(NoCanPervInfilMh) AS [No Tree Cover Infiltration (m/yr)],Sum(NoCanPervInfilMh) * ";
          strArray[47] = num7.ToString();
          strArray[48] = " AS [No Tree Cover Infiltration (m3/yr)],Sum(NoCanImpervRunoffMh) AS [No Tree Cover Runoff (m/yr)],Sum(NoCanImpervRunoffMh) * ";
          strArray[49] = num8.ToString();
          strArray[50] = " AS [No Tree Cover Runoff (m3/yr)],Sum(RainMh) * ";
          strArray[51] = this.Area.ToString();
          strArray[52] = " - (Sum(NoCanPervInfilMh) * ";
          strArray[53] = num7.ToString();
          strArray[54] = " + Sum(NoCanImpervRunoffMh) * ";
          strArray[55] = num8.ToString();
          strArray[56] = ") AS [No Tree Cover Ground Intercept (m3/yr)] INTO 01_InterceptYearlySums FROM ";
          strArray[57] = sTbl;
          strArray[58] = " GROUP BY Year([TimeStamp]);";
          string str2 = string.Concat(strArray);
          oleDbCommand2.CommandText = str2;
          oleDbCommand1.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    private void LeafOnSums(OleDbConnection conn, string sTbl)
    {
      using (OleDbCommand oleDbCommand1 = new OleDbCommand())
      {
        double num1 = this.Area * this.TreeCoverPct / 100.0;
        double num2 = this.Area * (100.0 - this.TreeCoverPct) / 100.0;
        double num3 = num1 * 74.5 / 100.0;
        double num4 = num1 * 25.5 / 100.0;
        double num5 = num2 * 74.5 / 100.0;
        double num6 = num2 * 25.5 / 100.0;
        double num7 = this.Area * 74.5 / 100.0;
        double num8 = this.Area * 25.5 / 100.0;
        try
        {
          oleDbCommand1.Connection = conn;
          OleDbCommand oleDbCommand2 = oleDbCommand1;
          string[] strArray = new string[59];
          strArray[0] = "SELECT DISTINCTROW Sum(RainMh) AS [Rain (m/lfon)],Sum(RainMh) * ";
          strArray[1] = this.Area.ToString();
          strArray[2] = " AS [Rain (m3/lfon)],Sum(PeTrMh) AS [Potential Evaporation (m/lfon)],Sum(PeTrMh) * ";
          strArray[3] = num1.ToString();
          strArray[4] = " AS [Potential Evaporation (m3/lfon)],Sum(VegEvMh) As [Evaporation (m/lfon)],Sum(VegEvMh) * ";
          strArray[5] = num1.ToString();
          strArray[6] = " AS [Evaporation (m3/lfon)],Sum(PtTrMh) AS [Potential Evapotranspiration (m/lfon)],Sum(PtTrMh) * ";
          strArray[7] = num1.ToString();
          strArray[8] = " AS [Potential Evapotranspiration (m3/lfon)],Sum(TransMh) AS [Transpiration (m/lfon)],Sum(TransMh) * ";
          strArray[9] = num1.ToString();
          strArray[10] = " AS [Transpiration (m3/lfon)],Sum(VegIntcptMh) AS [Veg Intercept no Adjust (m/lfon)],Sum(VegIntcptMh) * ";
          strArray[11] = num1.ToString();
          strArray[12] = " AS [Veg Intercept no Adjust (m3/lfon)],";
          string str1;
          if (num1 != 0.0)
            str1 = "(Sum(NoCanImpervRunoffMh) * " + num8.ToString() + " - (Sum(UnderCanImpervRunoffMh) * " + num4.ToString() + " + Sum(NoCanImpervRunoffMh) * " + num6.ToString() + "))/" + num1.ToString();
          else
            str1 = "0";
          strArray[13] = str1;
          strArray[14] = " AS [VegIntercept (m/yr)],Sum(NoCanImpervRunoffMh) * ";
          strArray[15] = num8.ToString();
          strArray[16] = " - (Sum(UnderCanImpervRunoffMh) * ";
          strArray[17] = num4.ToString();
          strArray[18] = " + Sum(NoCanImpervRunoffMh) * ";
          strArray[19] = num6.ToString();
          strArray[20] = ") AS [VegIntercept (m3/yr)],Sum(RainMh) * ";
          strArray[21] = num1.ToString();
          strArray[22] = " AS [Rain on Canopy (m3/lfon)],Sum(RainMh) * ";
          strArray[23] = num2.ToString();
          strArray[24] = " AS [Rain on No Canopy (m3/lfon)],Sum(UnderCanThrufallMh) AS [Under Canopy Thrufall (m/lfon)],Sum(UnderCanThrufallMh) * ";
          strArray[25] = num1.ToString();
          strArray[26] = " AS [Under Canopy Thrufall (m3/lfon)],Sum(UnderCanPervInfilMh) AS [Under Canopy Infiltration (m/lfon)],Sum(UnderCanPervInfilMh) * ";
          strArray[27] = num3.ToString();
          strArray[28] = " AS [Under Canopy Infiltration (m3/lfon)],Sum(UnderCanImpervRunoffMh) AS [Under Canopy Runoff (m/lfon)],Sum(UnderCanImpervRunoffMh) * ";
          strArray[29] = num4.ToString();
          strArray[30] = " AS [Under Canopy Runoff (m3/lfon)],Sum(NoCanPervInfilMh) AS [No Canopy Infiltration (m/lfon)],Sum(NoCanPervInfilMh) * ";
          strArray[31] = num5.ToString();
          strArray[32] = " AS [No Canopy Infiltration (m3/lfon)],Sum(NoCanImpervRunoffMh) AS [No Canopy Runoff (m/lfon)],Sum(NoCanImpervRunoffMh) * ";
          strArray[33] = num6.ToString();
          strArray[34] = " AS [No Canopy Runoff (m3/lfon)],Sum(RainMh) * ";
          strArray[35] = num2.ToString();
          strArray[36] = " + Sum(UnderCanThrufallMh) * ";
          strArray[37] = num1.ToString();
          strArray[38] = " - (Sum(UnderCanPervInfilMh) * ";
          strArray[39] = num3.ToString();
          strArray[40] = " + Sum(UnderCanImpervRunoffMh) * ";
          strArray[41] = num4.ToString();
          strArray[42] = " + Sum(NoCanPervInfilMh) * ";
          strArray[43] = num5.ToString();
          strArray[44] = " + Sum(NoCanImpervRunoffMh) * ";
          strArray[45] = num6.ToString();
          strArray[46] = ") AS [Ground Intercept (m3/lfon)],Sum(NoCanPervInfilMh) AS [No Tree Cover Infiltration (m/lfon)],Sum(NoCanPervInfilMh) * ";
          strArray[47] = num7.ToString();
          strArray[48] = " AS [No Tree Cover Infiltration (m3/lfon)],Sum(NoCanImpervRunoffMh) AS [No Tree Cover Runoff (m/lfon)],Sum(NoCanImpervRunoffMh) * ";
          strArray[49] = num8.ToString();
          strArray[50] = " AS [No Tree Cover Runoff (m3/lfon)],Sum(RainMh) * ";
          strArray[51] = this.Area.ToString();
          strArray[52] = " - (Sum(NoCanPervInfilMh) * ";
          strArray[53] = num7.ToString();
          strArray[54] = " + Sum(NoCanImpervRunoffMh) * ";
          strArray[55] = num8.ToString();
          strArray[56] = ") AS [No Tree Cover Ground Intercept (m3/lfon)] INTO 02_InterceptLeafOnSums FROM ";
          strArray[57] = sTbl;
          strArray[58] = " GROUP BY Year([TimeStamp]),GrowSeason HAVING GrowSeason=True;";
          string str2 = string.Concat(strArray);
          oleDbCommand2.CommandText = str2;
          oleDbCommand1.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    private void CreateFinalDB()
    {
      AccessFunc.CreateDB(this.FinIntcptDB);
      using (OleDbConnection cnDstDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.FinIntcptDB))
      {
        try
        {
          cnDstDB.Open();
          AccessFunc.CopyTable(this.InterceptDB, "01_InterceptYearlySums", cnDstDB, "01_InterceptYearlySums");
          AccessFunc.CopyTable(this.InterceptDB, "02_InterceptLeafOnSums", cnDstDB, "02_InterceptLeafOnSums");
          AccessFunc.CopyTable(this.InterceptDB, "03_InterceptMonthlySums", cnDstDB, "03_InterceptMonthlySums");
          AccessFunc.CopyTable(this.InterceptDB, "04_InterceptHourly", cnDstDB, "04_InterceptHourly");
          cnDstDB.Close();
        }
        catch (Exception ex)
        {
          throw;
        }
        finally
        {
          cnDstDB.Close();
        }
      }
    }
  }
}
