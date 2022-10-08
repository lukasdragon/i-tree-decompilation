// Decompiled with JetBrains decompiler
// Type: UFORE.Location.LocationDataForB
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Data.OleDb;

namespace UFORE.Location
{
  public class LocationDataForB
  {
    public string NationID;
    public string PriPartID;
    public string SecPartID;
    public string TerPartID;
    public double MoirIsoprene;
    public double MoirMonoterpene;
    public double MoirVoc;
    public double MoirCO;

    [Obsolete]
    public void ReadMoirData(string sLocDB)
    {
      using (OleDbConnection cnLocDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data source= " + sLocDB))
      {
        cnLocDB.Open();
        this.ReadMoirData(cnLocDB);
      }
    }

    public void ReadMoirData(OleDbConnection cnLocDB)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnLocDB;
        oleDbCommand.CommandText = "SELECT * FROM MOIR WHERE NationID=\"" + this.NationID + "\" AND PrimaryPartitionID=\"" + this.PriPartID + "\" AND SecondaryPartitionID=\"" + this.SecPartID + "\" AND TertiaryPartitionID=\"" + this.TerPartID + "\" ";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.MoirIsoprene = (double) oleDbDataReader["MOIRISO"];
          this.MoirMonoterpene = (double) oleDbDataReader["MOIRMON"];
          this.MoirVoc = (double) oleDbDataReader["MOIRVOC"];
          this.MoirCO = (double) oleDbDataReader["MOIRCO"];
        }
      }
    }
  }
}
