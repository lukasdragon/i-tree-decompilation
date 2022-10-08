// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.Landuse
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace UFORE.BioEmission
{
  public class Landuse
  {
    public const int HECTOR2SQM = 10000;
    public double CityTreePctCov;
    public string LanduseAbbr;
    public string LanduseDesc;
    public double LanduseArea;
    public double ShrubPctCov;
    public double ShrubLAI;
    public double TreePctCov;
    public double TreeLAI;

    [Obsolete]
    public static void ReadCityTreeCover(string sAceDB, string sTbl, out double tc)
    {
      using (OleDbConnection cnAceDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sAceDB))
      {
        cnAceDB.Open();
        Landuse.ReadCityTreeCover(cnAceDB, sTbl, out tc);
      }
    }

    public static void ReadCityTreeCover(OleDbConnection cnAceDB, string sTbl, out double tc)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnAceDB;
        oleDbCommand.CommandText = "SELECT CM_PTREE FROM " + sTbl + ";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          oleDbDataReader.Read();
          tc = (double) oleDbDataReader["CM_PTREE"];
          oleDbDataReader.Close();
        }
      }
    }

    [Obsolete]
    public static int ReadLanduseData(
      string sAceDB,
      string sTbl,
      ref List<string> landuse,
      ref Dictionary<string, Landuse> luDict)
    {
      using (OleDbConnection cnAceDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sAceDB))
      {
        cnAceDB.Open();
        return Landuse.ReadLanduseData(cnAceDB, sTbl, ref landuse, ref luDict);
      }
    }

    public static int ReadLanduseData(
      OleDbConnection cnAceDB,
      string sTbl,
      ref List<string> landuse,
      ref Dictionary<string, Landuse> luDict)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnAceDB;
        oleDbCommand.CommandText = "SELECT * FROM " + sTbl + " ORDER BY LANDUSE;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            landuse.Add((string) oleDbDataReader["LANDUSE"]);
            Landuse landuse1 = new Landuse();
            landuse1.LanduseAbbr = (string) oleDbDataReader["LANDUSE"];
            landuse1.LanduseDesc = (string) oleDbDataReader["LU_DESC"];
            landuse1.LanduseArea = (double) oleDbDataReader["LND_AREA"] * 10000.0;
            landuse1.ShrubPctCov = oleDbDataReader["MN_PSHRB"] == DBNull.Value ? 0.0 : (double) oleDbDataReader["MN_PSHRB"];
            landuse1.ShrubLAI = oleDbDataReader["LS_LAI"] == DBNull.Value ? 0.0 : (double) oleDbDataReader["LS_LAI"];
            landuse1.TreePctCov = oleDbDataReader["MN_PTREE"] == DBNull.Value ? 0.0 : (double) oleDbDataReader["MN_PTREE"];
            landuse1.TreeLAI = oleDbDataReader["LT_LAI"] == DBNull.Value ? 0.0 : (double) oleDbDataReader["LT_LAI"];
            luDict.Add(landuse1.LanduseAbbr, landuse1);
          }
          oleDbDataReader.Close();
        }
        return luDict.Count;
      }
    }
  }
}
