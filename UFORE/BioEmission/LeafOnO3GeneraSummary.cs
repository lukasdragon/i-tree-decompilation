// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.LeafOnO3GeneraSummary
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Deposition;

namespace UFORE.BioEmission
{
  internal class LeafOnO3GeneraSummary
  {
    private string Genera;
    private double Formed;
    private double Removed;
    private double Net;

    public static void CalcSummaryByGenera(
      ref List<string> genera,
      ref Dictionary<string, LeafBiomass> genDict,
      ref List<DryDeposition> O3List,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      LeafOnO3GeneraSummary onO3GeneraSummary = new LeafOnO3GeneraSummary();
      onO3GeneraSummary.AddFields(cnBioEDB);
      for (int index = 0; index < genera.Count; ++index)
      {
        LeafBiomass lb;
        genDict.TryGetValue(genera[index], out lb);
        onO3GeneraSummary.ReadO3Formed(cnBioEDB, genera[index]);
        onO3GeneraSummary.CalcSummary(lb, ref O3List, inv);
        onO3GeneraSummary.InsertSummary(cnBioEDB);
      }
    }

    private void AddFields(OleDbConnection cn)
    {
      string tbl = "04_LeafOnSummaryByGenera";
      AccessFunc.AppendField(cn, tbl, "O3 removed", "DOUBLE");
      AccessFunc.AppendField(cn, tbl, "O3 net (formed-removed)", "DOUBLE");
    }

    private void CalcSummary(LeafBiomass lb, ref List<DryDeposition> ddList, bool bInv)
    {
      double num1 = 0.0;
      double num2 = !bInv ? Math.Pow(10.0, -3.0) : 1.0;
      this.Genera = lb.Genera;
      for (int index = 0; index < ddList.Count; ++index)
      {
        if (ddList[index].GrowSeason)
          num1 += ddList[index].Flux;
      }
      this.Removed = num1 * ddList[0].TrCovArea * num2 * lb.TotalLeafBiomass / lb.CityTotalLeafBiomass;
      this.Net = this.Formed - this.Removed;
    }

    private void ReadO3Formed(OleDbConnection cn, string genera)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = "04_LeafOnSummaryByGenera";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT * FROM " + str + " WHERE Genera=\"" + genera + "\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          if (oleDbDataReader.Read())
            this.Formed = (double) oleDbDataReader["O3 formed"];
          oleDbDataReader.Close();
        }
      }
    }

    private void InsertSummary(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = "04_LeafOnSummaryByGenera";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "UPDATE " + str + " SET [O3 removed] = " + this.Removed.ToString() + ",[O3 net (formed-removed)] = " + this.Net.ToString() + " WHERE [Genera]=\"" + this.Genera + "\";";
        oleDbCommand.ExecuteNonQuery();
      }
    }
  }
}
