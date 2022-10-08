// Decompiled with JetBrains decompiler
// Type: UFORE.Deposition.Resistance
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Location;
using UFORE.Weather;

namespace UFORE.Deposition
{
  public class Resistance
  {
    private const double ZCONS = 6.096;
    private const double ZO = 0.2;
    private const double DISPL = 0.6;
    private const double WINCONS = 5.496;
    private const double K = 0.41;
    private const double BETAM = 4.7;
    private const double ACCGRAV = 9.81;
    private const double PR = 0.72;
    private const double SCNO2 = 0.98;
    private const double SCSO2 = 1.15;
    private const double SCO3 = 1.0;
    private const double SCCO = 0.76;
    public const string RES_TABLE = "Resistances";
    private DateTime TimeStamp;
    public string Period;
    public int Stability;
    public double Ustar;
    public double Ra;
    public Dictionary<string, double> Rb;
    public double RbCO;
    public double RbNO2;
    public double RbO3;
    public double RbSO2;

    public static void CalcResistances(
      LocationData locData,
      ref List<SurfaceWeather> sfcList,
      int recCnt,
      OleDbConnection conn)
    {
      double solElevAgl = 0.0;
      List<Resistance> resList = new List<Resistance>();
      for (int index = 0; index < sfcList.Count; ++index)
      {
        Resistance resistance = new Resistance();
        resistance.Rb = new Dictionary<string, double>();
        resistance.SetTimeStamp(ref sfcList, index);
        resistance.CheckDayNight(sfcList[index].TimeStamp.DayOfYear, sfcList[index].TimeStamp.Hour, locData.GMTOffset, locData.Latitude, locData.Longitude, ref solElevAgl);
        resistance.CalcAerodynamicRes(sfcList[index].ToCldCov, sfcList[index].Ceiling, sfcList[index].WdSpdMs, solElevAgl, sfcList[index].OpCldCov, sfcList[index].TempK);
        resistance.CalcQuasilaminarBoundaryLayerRes();
        resList.Add(resistance);
      }
      Resistance.CreateResistanceTable(conn);
      Resistance.WriteResistanceRecords(conn, ref resList, resList.Count);
    }

    public static void CalcResistances(
      LocationData locData,
      ref List<SurfaceWeather> sfcList,
      int recCnt,
      string dryDepDB)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + dryDepDB))
      {
        conn.Open();
        Resistance.CalcResistances(locData, ref sfcList, recCnt, conn);
      }
    }

    private void SetTimeStamp(ref List<SurfaceWeather> sfcList, int i) => this.TimeStamp = sfcList[i].TimeStamp;

    private void CheckDayNight(
      int jDate,
      int hr,
      double zone,
      double lat,
      double lon,
      ref double solElevAgl)
    {
      double num1 = zone * 15.0;
      int num2 = hr * 100;
      double num3 = 2.0 * Math.PI * (double) (jDate - 1) / 365.0;
      double num4 = 0.006918 - 0.399912 * Math.Cos(num3) + 0.070257 * Math.Sin(num3) - 0.0067758 * Math.Cos(2.0 * num3) + 0.000907 * Math.Sin(2.0 * num3) - 0.002697 * Math.Cos(3.0 * num3) + 0.00148 * Math.Sin(3.0 * num3);
      double d = -Math.Tan(Resistance.Deg2Rad(Math.Abs(lat))) * Math.Tan(num4);
      double num5 = Resistance.Rad2Deg(d >= 0.0 ? Math.Acos(d) : Math.PI - Math.Acos(-d)) / 15.0;
      double num6 = (7.5E-05 + 0.001868 * Math.Cos(num3) - 0.032077 * Math.Sin(num3) - 0.014615 * Math.Cos(2.0 * num3) - 0.040849 * Math.Sin(2.0 * num3)) * 229.18;
      double num7 = lon;
      double num8 = num1 - num7;
      double num9 = 12.0 - num5;
      double num10 = 12.0 + num5;
      double num11 = num8 / 15.0;
      double num12 = num9 - num11 - num6 / 60.0;
      double num13 = num10 - num8 / 15.0 - num6 / 60.0;
      int num14 = num2 / 100;
      double num15 = (double) num14 + (double) (num2 - num14 * 100) / 60.0;
      double angle = (12.0 - (num15 + num8 / 15.0 + num6 / 60.0)) * 15.0;
      this.Period = num15 <= num12 || num15 > num13 ? "N" : "D";
      solElevAgl = !(this.Period == "D") ? 0.0 : Math.Asin(Math.Sin(num4) * Math.Sin(Resistance.Deg2Rad(lat)) + Math.Cos(num4) * Math.Cos(Resistance.Deg2Rad(lat)) * Math.Cos(Resistance.Deg2Rad(angle)));
      solElevAgl = Resistance.Rad2Deg(solElevAgl);
    }

    private void CalcAerodynamicRes(
      double totCc,
      double Ceiling,
      double windMs,
      double solElevAgl,
      double opqCc,
      double TempK)
    {
      this.AssignStabilityClass(totCc, Ceiling, windMs, solElevAgl);
      double invL;
      this.CalcMoninObukhovLen(out invL);
      double windSp;
      this.CalcFrictionVelocity(windMs, opqCc, TempK, invL, out windSp);
      this.CalcRa(windSp);
    }

    private void AssignStabilityClass(
      double totalCldCov,
      double Ceiling,
      double windMs,
      double solElevAgl)
    {
      if (totalCldCov == 10.0 && Ceiling < 70.0)
        this.Stability = 4;
      else if (this.Period == "N")
      {
        if (totalCldCov <= 4.0)
        {
          if (windMs < 6.0)
            this.Stability = 5;
          else
            this.Stability = 4;
        }
        else if (windMs < 3.0)
          this.Stability = 5;
        else
          this.Stability = 4;
      }
      else if (totalCldCov <= 5.0)
      {
        if (solElevAgl >= 60.0)
        {
          if (windMs < 3.0)
            this.Stability = 1;
          else if (windMs >= 5.0)
            this.Stability = 3;
          else
            this.Stability = 2;
        }
        else if (35.0 <= solElevAgl && solElevAgl < 60.0)
        {
          if (windMs < 1.0)
            this.Stability = 1;
          else if (1.0 <= windMs && windMs < 4.0)
            this.Stability = 2;
          else if (4.0 <= windMs && windMs < 6.0)
            this.Stability = 3;
          else
            this.Stability = 4;
        }
        else if (15.0 <= solElevAgl && solElevAgl < 35.0)
        {
          if (windMs < 2.0)
            this.Stability = 2;
          else if (2.0 <= windMs && windMs < 5.0)
            this.Stability = 3;
          else
            this.Stability = 4;
        }
        else if (windMs <= 2.0)
          this.Stability = 3;
        else
          this.Stability = 4;
      }
      else if (Ceiling < 70.0)
      {
        if (solElevAgl >= 60.0)
        {
          if (windMs < 2.0)
            this.Stability = 2;
          else if (2.0 <= windMs && windMs < 5.0)
            this.Stability = 3;
          else
            this.Stability = 4;
        }
        else if (windMs < 2.0)
          this.Stability = 3;
        else
          this.Stability = 4;
      }
      else if (70.0 <= Ceiling && Ceiling < 160.0 || totalCldCov == 10.0 && Ceiling >= 160.0)
      {
        if (solElevAgl >= 60.0)
        {
          if (windMs < 1.0)
            this.Stability = 1;
          else if (1.0 <= windMs && windMs < 4.0)
            this.Stability = 2;
          else if (4.0 <= windMs && windMs < 6.0)
            this.Stability = 3;
          else
            this.Stability = 4;
        }
        else if (35.0 <= solElevAgl && solElevAgl < 60.0)
        {
          if (windMs < 2.0)
            this.Stability = 2;
          else if (2.0 <= windMs && windMs < 5.0)
            this.Stability = 3;
          else
            this.Stability = 4;
        }
        else if (windMs < 2.0)
          this.Stability = 3;
        else
          this.Stability = 4;
      }
      else if (solElevAgl >= 60.0)
      {
        if (windMs < 3.0)
          this.Stability = 1;
        else if (windMs >= 5.0)
          this.Stability = 3;
        else
          this.Stability = 2;
      }
      else if (35.0 <= solElevAgl && solElevAgl < 60.0)
      {
        if (windMs < 1.0)
          this.Stability = 1;
        else if (1.0 <= windMs && windMs < 4.0)
          this.Stability = 2;
        else if (4.0 <= windMs && windMs < 6.0)
          this.Stability = 3;
        else
          this.Stability = 4;
      }
      else if (15.0 <= solElevAgl && solElevAgl < 35.0)
      {
        if (windMs < 2.0)
          this.Stability = 2;
        else if (2.0 <= windMs && windMs < 5.0)
          this.Stability = 3;
        else
          this.Stability = 4;
      }
      else if (windMs <= 2.0)
        this.Stability = 3;
      else
        this.Stability = 4;
    }

    private void CalcMoninObukhovLen(out double invL)
    {
      invL = 0.0;
      switch (this.Stability)
      {
        case 1:
          invL = -7.0 / 80.0 * Math.Pow(0.2, -0.1029);
          break;
        case 2:
          invL = -0.03849 * Math.Pow(0.2, -0.1714);
          break;
        case 3:
          invL = -0.00807 * Math.Pow(0.2, -0.3049);
          break;
        case 4:
          invL = 0.0 * Math.Pow(0.2, 0.0);
          break;
        case 5:
          invL = 0.00807 * Math.Pow(0.2, -0.3049);
          break;
        case 6:
          invL = 0.03849 * Math.Pow(0.2, -0.1714);
          break;
      }
    }

    private void CalcFrictionVelocity(
      double windMs,
      double opqCc,
      double TempK,
      double invL,
      out double windSp)
    {
      windSp = windMs != 0.0 ? windMs : 0.5;
      if (invL == 0.0)
      {
        this.Ustar = 0.41 * windSp / Math.Log(27.48);
      }
      else
      {
        double num1 = 1.0 / invL;
        if (num1 < 0.0)
        {
          double num2 = Math.Pow(1.0 - 170.688 / num1, -0.25);
          double num3 = 2.0 * Math.Log((1.0 + num2) / 2.0) + Math.Log((1.0 + Math.Pow(num2, 2.0)) / 2.0) - 2.0 * Math.Atan(num2) + Math.PI / 2.0;
          this.Ustar = 0.41 * windSp / (Math.Log(27.48) - num3 * (5.496 / num1) + num3 * (0.2 / num1));
        }
        else
        {
          if (num1 <= 0.0)
            return;
          double num4 = 0.09 * (1.0 - 0.5 * Math.Pow(opqCc / 10.0, 2.0));
          double d = 0.41 / Math.Log(30.48);
          double num5 = Math.Sqrt(281.06827200000004 * num4 / TempK);
          if (2.0 * num5 / (Math.Sqrt(d) * windSp) <= 1.0)
          {
            this.Ustar = d * windSp * (0.5 + 0.5 * Math.Sqrt(1.0 - Math.Pow(2.0 * num5 / (Math.Sqrt(d) * windSp), 2.0)));
          }
          else
          {
            double num6 = Math.Sqrt(4.0 / d) * num5;
            this.Ustar = d * num6 / 2.0 * (windSp / num6);
          }
        }
      }
    }

    private void CalcRa(double windSp) => this.Ra = windSp / Math.Pow(this.Ustar, 2.0);

    private void CalcQuasilaminarBoundaryLayerRes()
    {
      this.Rb.Add("CO", 2.0 * Math.Pow(0.76, 0.67) * Math.Pow(0.72, -0.67) * (1.0 / (0.41 * this.Ustar)));
      this.Rb.Add("NO2", 2.0 * Math.Pow(0.98, 0.67) * Math.Pow(0.72, -0.67) * (1.0 / (0.41 * this.Ustar)));
      this.Rb.Add("O3", 2.0 * Math.Pow(1.0, 0.67) * Math.Pow(0.72, -0.67) * (1.0 / (0.41 * this.Ustar)));
      this.Rb.Add("SO2", 2.0 * Math.Pow(1.15, 0.67) * Math.Pow(0.72, -0.67) * (1.0 / (0.41 * this.Ustar)));
    }

    private static double Deg2Rad(double angle) => Math.PI * angle / 180.0;

    private static double Rad2Deg(double angle) => 180.0 * angle / Math.PI;

    public static void CreateResistanceTable(OleDbConnection conn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "CREATE TABLE Resistances([TimeStamp] DateTime,[Stability] INT,[Period] TEXT (1),[Ustar] DOUBLE,[Ra] DOUBLE,[RbCO] DOUBLE,[RbNO2] DOUBLE,[RbO3] DOUBLE,[RbSO2] DOUBLE);";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    private static void WriteResistanceRecords(
      OleDbConnection conn,
      ref List<Resistance> resList,
      int recCnt)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        for (int index = 0; index < recCnt; ++index)
        {
          double num1;
          resList[index].Rb.TryGetValue("CO", out num1);
          double num2;
          resList[index].Rb.TryGetValue("NO2", out num2);
          double num3;
          resList[index].Rb.TryGetValue("O3", out num3);
          double num4;
          resList[index].Rb.TryGetValue("SO2", out num4);
          oleDbCommand.CommandText = "INSERT INTO Resistances ([TimeStamp],[Stability],[Period], [Ustar],[Ra],[RbCO],[RbNO2],[RbO3],[RbSO2]) Values (#" + resList[index].TimeStamp.ToString() + "#," + resList[index].Stability.ToString() + ",\"" + resList[index].Period + "\"," + resList[index].Ustar.ToString() + "," + resList[index].Ra.ToString() + "," + num1.ToString() + "," + num2.ToString() + "," + num3.ToString() + "," + num4.ToString() + ");";
          oleDbCommand.ExecuteNonQuery();
        }
      }
    }

    public static int ReadResistanceRecords(OleDbConnection conn, ref List<Resistance> resList)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "SELECT * FROM Resistances ORDER BY TimeStamp;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            Resistance resistance = new Resistance()
            {
              Rb = new Dictionary<string, double>(),
              TimeStamp = (DateTime) oleDbDataReader["TimeStamp"],
              Stability = (int) oleDbDataReader["Stability"],
              Period = (string) oleDbDataReader["Period"],
              Ustar = (double) oleDbDataReader["Ustar"],
              Ra = (double) oleDbDataReader["Ra"]
            };
            resistance.Rb.Add("CO", (double) oleDbDataReader["RbCO"]);
            resistance.Rb.Add("NO2", (double) oleDbDataReader["RbNO2"]);
            resistance.Rb.Add("O3", (double) oleDbDataReader["RbO3"]);
            resistance.Rb.Add("SO2", (double) oleDbDataReader["RbSO2"]);
            resList.Add(resistance);
          }
        }
      }
      return resList.Count;
    }
  }
}
