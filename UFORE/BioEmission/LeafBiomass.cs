// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.LeafBiomass
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Deposition;

namespace UFORE.BioEmission
{
  public class LeafBiomass
  {
    public double CityTotalLeafBiomass;
    public double CityTotalLiveTrees;
    public string Landuse;
    public string Genera;
    public double TotalLeafBiomass;
    public double TotalLiveTrees;
    public bool Deciduous;
    public bool Tree;

    [Obsolete]
    public static int ReadLeafBiomassData(
      string sAceDB,
      string sTbl,
      DryDeposition.VEG_TYPE vegType,
      ref List<LeafBiomass> lbList)
    {
      using (OleDbConnection cnAceDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sAceDB))
      {
        cnAceDB.Open();
        return LeafBiomass.ReadLeafBiomassData(cnAceDB, sTbl, vegType, ref lbList);
      }
    }

    public static int ReadLeafBiomassData(
      OleDbConnection cnAceDB,
      string sTbl,
      DryDeposition.VEG_TYPE vegType,
      ref List<LeafBiomass> lbList)
    {
      string str;
      switch (vegType)
      {
        case DryDeposition.VEG_TYPE.TREE:
          str = "TREE";
          break;
        case DryDeposition.VEG_TYPE.SHRUB:
          str = "SHRUB";
          break;
        default:
          str = "TREE";
          break;
      }
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnAceDB;
        oleDbCommand.CommandText = "SELECT * FROM " + sTbl + " WHERE FORM_IND=\"" + str + "\" ORDER BY LANDUSE, GENUS;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            LeafBiomass leafBiomass = new LeafBiomass()
            {
              CityTotalLeafBiomass = oleDbDataReader["CTOT_LB"] == DBNull.Value ? 0.0 : (double) oleDbDataReader["CTOT_LB"],
              CityTotalLiveTrees = oleDbDataReader["CTOT_LIV"] == DBNull.Value ? 0.0 : (double) oleDbDataReader["CTOT_LIV"],
              Landuse = (string) oleDbDataReader["Landuse"],
              Genera = ((string) oleDbDataReader["GENUS"]).ToUpper(),
              TotalLeafBiomass = oleDbDataReader["STOT_LB"] == DBNull.Value ? 0.0 : (double) oleDbDataReader["STOT_LB"],
              TotalLiveTrees = oleDbDataReader["STOT_LIV"] == DBNull.Value ? 0.0 : (double) oleDbDataReader["STOT_LIV"],
              Deciduous = (string) oleDbDataReader["LVE_TYPE"] == "DECIDUOUS",
              Tree = (string) oleDbDataReader["FORM_IND"] == "TREE"
            };
            lbList.Add(leafBiomass);
          }
          oleDbDataReader.Close();
        }
        return lbList.Count;
      }
    }

    [Obsolete]
    public static int ReadGeneraLeafBiomass(
      string sAceDB,
      string sTbl,
      DryDeposition.VEG_TYPE vegType,
      ref List<string> genera,
      ref Dictionary<string, LeafBiomass> genDict)
    {
      using (OleDbConnection cnAceDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sAceDB))
      {
        cnAceDB.Open();
        return LeafBiomass.ReadGeneraLeafBiomass(cnAceDB, sTbl, vegType, ref genera, ref genDict);
      }
    }

    public static int ReadGeneraLeafBiomass(
      OleDbConnection cnAceDB,
      string sTbl,
      DryDeposition.VEG_TYPE vegType,
      ref List<string> genera,
      ref Dictionary<string, LeafBiomass> genDict)
    {
      string str;
      switch (vegType)
      {
        case DryDeposition.VEG_TYPE.TREE:
          str = "TREE";
          break;
        case DryDeposition.VEG_TYPE.SHRUB:
          str = "SHRUB";
          break;
        default:
          str = "TREE";
          break;
      }
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnAceDB;
        oleDbCommand.CommandText = "SELECT DISTINCTROW GENUS, CTOT_LB, Sum([STOT_LB]) AS TotLb FROM " + sTbl + " GROUP BY GENUS, FORM_IND, CTOT_LB HAVING FORM_IND=\"" + str + "\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          while (oleDbDataReader.Read())
          {
            LeafBiomass leafBiomass = new LeafBiomass()
            {
              Genera = ((string) oleDbDataReader["GENUS"]).ToUpper(),
              CityTotalLeafBiomass = oleDbDataReader["CTOT_LB"] == DBNull.Value ? 0.0 : (double) oleDbDataReader["CTOT_LB"],
              TotalLeafBiomass = oleDbDataReader["TotLb"] == DBNull.Value ? 0.0 : (double) oleDbDataReader["TotLb"]
            };
            genera.Add(leafBiomass.Genera);
            genDict.Add(leafBiomass.Genera, leafBiomass);
          }
          oleDbDataReader.Close();
        }
        return genera.Count;
      }
    }
  }
}
