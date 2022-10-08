// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.City
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Data.OleDb;

namespace UFORE.BioEmission
{
  public class City
  {
    public double AreaM2;
    public double ShrubPctCov;
    public double TreePctCov;

    [Obsolete]
    public void ReadCityData(string sAceDB, string sTbl)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sAceDB))
        oleDbConnection.Open();
    }

    public void ReadCityData(OleDbConnection cnAceDB, string sTbl)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnAceDB;
        oleDbCommand.CommandText = "SELECT TOP 1 * FROM " + sTbl + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          this.AreaM2 = (double) oleDbDataReader["TOT_AREA"] * 10000.0;
          this.ShrubPctCov = oleDbDataReader["CM_PSHRB"] == DBNull.Value ? 0.0 : (double) oleDbDataReader["CM_PSHRB"];
          this.TreePctCov = oleDbDataReader["CM_PTREE"] == DBNull.Value ? 0.0 : (double) oleDbDataReader["CM_PTREE"];
          oleDbDataReader.Close();
        }
      }
    }
  }
}
