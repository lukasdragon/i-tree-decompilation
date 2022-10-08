// Decompiled with JetBrains decompiler
// Type: UFORE.WeatherStation
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Data.OleDb;
using UFORE.Location;

namespace UFORE
{
  public class WeatherStation
  {
    private const string WTR_TABLE = "WeatherStationAssigned";
    public string usaf;
    public string wban;
    public string year;

    public void ReadAssignedMonitor(string sDB, LocationData loc)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        OleDbCommand oleDbCommand = new OleDbCommand();
        try
        {
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "SELECT * FROM WeatherStationAssigned WHERE NationID=\"" + loc.NationID + "\" AND PrimaryPartitionID=\"" + loc.PriPartID + "\" And SecondaryPartitionID=\"" + loc.SecPartID + "\";";
          OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
          oleDbDataReader.Read();
          this.usaf = (string) oleDbDataReader["USAF"];
          this.wban = (string) oleDbDataReader["WBAN"];
          this.year = (string) oleDbDataReader["FileYear"];
          oleDbDataReader.Close();
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }
  }
}
