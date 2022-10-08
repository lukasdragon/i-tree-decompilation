// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.BaseEmission
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace UFORE.BioEmission
{
  public class BaseEmission
  {
    public double BaseIsoprene;
    public double BaseMonoterpene;
    public double BaseOtherVOC;

    [Obsolete]
    public static void ReadBaseEmission(
      string sSpeciesDB,
      string sTbl,
      ref Dictionary<string, BaseEmission> dict)
    {
      using (OleDbConnection cnSpeciesDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sSpeciesDB))
      {
        cnSpeciesDB.Open();
        BaseEmission.ReadBaseEmission(cnSpeciesDB, sTbl, ref dict);
      }
    }

    public static void ReadBaseEmission(
      OleDbConnection cnSpeciesDB,
      string sTbl,
      ref Dictionary<string, BaseEmission> dict)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnSpeciesDB;
        oleDbCommand.CommandText = "SELECT * FROM " + sTbl + " ORDER BY GENUS;";
        OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
        while (oleDbDataReader.Read())
          dict.Add((string) oleDbDataReader["GENUS"], new BaseEmission()
          {
            BaseIsoprene = (double) oleDbDataReader["BASEISO"],
            BaseMonoterpene = (double) oleDbDataReader["BASEMONO"],
            BaseOtherVOC = (double) oleDbDataReader["BASEOVOC"]
          });
        oleDbDataReader.Close();
      }
    }

    [Obsolete]
    public static void ReadBaseEmission(
      string sSpeciesDB,
      ref Dictionary<string, BaseEmission> dict)
    {
      using (OleDbConnection cnSpeciesDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + sSpeciesDB))
      {
        cnSpeciesDB.Open();
        BaseEmission.ReadBaseEmission(cnSpeciesDB, ref dict);
      }
    }

    public static void ReadBaseEmission(
      OleDbConnection cnSpeciesDB,
      ref Dictionary<string, BaseEmission> dict)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnSpeciesDB;
        oleDbCommand.CommandText = "SELECT [_SpeciesBioemissions].SpeciesId, [_SpeciesBioemissions].BioemissionId, [_SpeciesBioemissions].BioemissionValue, [_Species].Code  FROM _Species INNER JOIN _SpeciesBioemissions ON [_Species].SpeciesId = [_SpeciesBioemissions].SpeciesId  WHERE [_Species].SpeciesTypeId=1 Or [_Species].SpeciesTypeId=5  ORDER BY [_SpeciesBioemissions].SpeciesId, [_SpeciesBioemissions].BioemissionId;";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          string key = "";
          BaseEmission baseEmission = new BaseEmission();
          int ordinal1 = oleDbDataReader.GetOrdinal("Code");
          int ordinal2 = oleDbDataReader.GetOrdinal("BioemissionId");
          int ordinal3 = oleDbDataReader.GetOrdinal("BioemissionValue");
          while (oleDbDataReader.Read())
          {
            if (key != oleDbDataReader.GetString(ordinal1))
            {
              if (key != "")
                dict.Add(key, baseEmission);
              baseEmission = new BaseEmission();
              key = oleDbDataReader.GetString(ordinal1);
            }
            switch (oleDbDataReader.GetByte(ordinal2))
            {
              case 1:
                baseEmission.BaseIsoprene = oleDbDataReader.GetDouble(ordinal3);
                continue;
              case 2:
                baseEmission.BaseMonoterpene = oleDbDataReader.GetDouble(ordinal3);
                continue;
              case 3:
                baseEmission.BaseOtherVOC = oleDbDataReader.GetDouble(ordinal3);
                continue;
              default:
                throw new Exception("No such BioEmission ID");
            }
          }
          if (key != "")
            dict.Add(key, baseEmission);
          oleDbDataReader.Close();
        }
      }
    }

    private static void SetHardwoodSoftwoodBaseEmissions(ref Dictionary<string, BaseEmission> dict)
    {
      dict.Add("HARDWOOD", new BaseEmission()
      {
        BaseIsoprene = 6.36,
        BaseMonoterpene = 0.51
      });
      dict.Add("HARDWOOD SPECIES", new BaseEmission()
      {
        BaseIsoprene = 6.36,
        BaseMonoterpene = 0.51
      });
      dict.Add("SOFTWOOD", new BaseEmission()
      {
        BaseIsoprene = 3.47,
        BaseMonoterpene = 1.57
      });
      dict.Add("SOFTWOOD SPECIES", new BaseEmission()
      {
        BaseIsoprene = 3.47,
        BaseMonoterpene = 1.57
      });
    }
  }
}
