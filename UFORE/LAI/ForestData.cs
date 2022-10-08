// Decompiled with JetBrains decompiler
// Type: UFORE.LAI.ForestData
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Data.OleDb;
using UFORE.Location;

namespace UFORE.LAI
{
  public class ForestData
  {
    public double DomainArea;
    public double EvGrnLAI;
    public double MaxLAI;
    public double PctEvGrnCov;
    public double PctTrCov;
    public double TrCovArea;
    private const double RURAL_LAI = 3.2;
    private const double URBAN_LAI = 4.9;

    public ForestData(double area, double lai, double evergreen, double treeCover)
    {
      this.DomainArea = area;
      this.MaxLAI = lai;
      this.PctEvGrnCov = evergreen;
      this.PctTrCov = treeCover;
      this.EvGrnLAI = lai * evergreen / 100.0;
      this.TrCovArea = area * treeCover / 100.0;
    }

    public ForestData()
    {
    }

    public void ReadForestInfo(string DB, LocationData locData, ForestData.RURAL_URBAN ru)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + DB))
      {
        OleDbCommand oleDbCommand = new OleDbCommand();
        try
        {
          string str = ru == ForestData.RURAL_URBAN.RURAL ? "RuralForestInfo" : "UrbanForestInfo";
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "SELECT DISTINCTROW *  FROM " + str + " WHERE PrimaryPartitionID =\"" + locData.PriPartID + "\" AND SecondaryPartitionID=\"" + locData.SecPartID + "\" AND TertiaryPartitionID=\"" + locData.TerPartID + "\";";
          OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
          oleDbDataReader.Read();
          this.MaxLAI = (double) oleDbDataReader["LAI"];
          this.MaxLAI = ru != ForestData.RURAL_URBAN.RURAL ? (this.MaxLAI < 4.9 ? 4.9 : this.MaxLAI) : (this.MaxLAI < 3.2 ? 3.2 : this.MaxLAI);
          this.DomainArea = (double) oleDbDataReader["Area"];
          this.PctTrCov = (double) oleDbDataReader["PercentTreeCov"];
          this.PctEvGrnCov = (double) oleDbDataReader["PercentEvergreen"];
          this.EvGrnLAI = this.MaxLAI * this.PctEvGrnCov / 100.0;
          this.TrCovArea = this.DomainArea * this.PctTrCov / 100.0;
          oleDbDataReader.Close();
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

    public enum RURAL_URBAN
    {
      RURAL,
      URBAN,
    }
  }
}
