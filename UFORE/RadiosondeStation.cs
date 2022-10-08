// Decompiled with JetBrains decompiler
// Type: UFORE.RadiosondeStation
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Data.OleDb;
using UFORE.Location;

namespace UFORE
{
  public class RadiosondeStation
  {
    private const string RDO_TABLE = "RadiosondeStationAssigned";
    public string wmo;
    public string id;
    public string dt;

    public void ReadAssignedMonitor(string sDB, LocationData loc)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sDB))
      {
        OleDbCommand oleDbCommand = new OleDbCommand();
        try
        {
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "SELECT * FROM RadiosondeStationAssigned WHERE NationID=\"" + loc.NationID + "\" AND PrimaryPartitionID=\"" + loc.PriPartID + "\" And SecondaryPartitionID=\"" + loc.SecPartID + "\";";
          OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
          oleDbDataReader.Read();
          this.wmo = (string) oleDbDataReader["WMO"];
          this.id = (string) oleDbDataReader["ID"];
          this.dt = (string) oleDbDataReader["DateTime"];
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
  }
}
