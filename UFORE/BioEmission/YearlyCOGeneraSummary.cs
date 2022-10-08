// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.YearlyCOGeneraSummary
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Deposition;

namespace UFORE.BioEmission
{
  internal class YearlyCOGeneraSummary
  {
    private string Genera;
    private double Formed;
    private double Removed;
    private double Net;

    public static void CalcSummaryByGenera(
      ref List<string> genera,
      ref Dictionary<string, LeafBiomass> genDict,
      ref List<DryDeposition> COList,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      YearlyCOGeneraSummary yearlyCoGeneraSummary = new YearlyCOGeneraSummary();
      yearlyCoGeneraSummary.AddFields(cnBioEDB);
      for (int index = 0; index < genera.Count; ++index)
      {
        LeafBiomass lb;
        genDict.TryGetValue(genera[index], out lb);
        yearlyCoGeneraSummary.ReadCOFormed(cnBioEDB, genera[index]);
        yearlyCoGeneraSummary.CalcSummary(lb, ref COList, inv);
        yearlyCoGeneraSummary.InsertSummary(cnBioEDB);
      }
    }

    private void AddFields(OleDbConnection cn)
    {
      string tbl = "02_YearlySummaryByGenera";
      AccessFunc.AppendField(cn, tbl, "CO removed", "DOUBLE");
      AccessFunc.AppendField(cn, tbl, "CO net (formed-removed)", "DOUBLE");
    }

    private void CalcSummary(LeafBiomass lb, ref List<DryDeposition> ddList, bool bInv)
    {
      double num1 = 0.0;
      double num2 = !bInv ? Math.Pow(10.0, -3.0) : 1.0;
      this.Genera = lb.Genera;
      for (int index = 0; index < ddList.Count; ++index)
        num1 += ddList[index].Flux;
      this.Removed = num1 * ddList[0].TrCovArea * num2 * lb.TotalLeafBiomass / lb.CityTotalLeafBiomass;
      this.Net = this.Formed - this.Removed;
    }

    private void ReadCOFormed(OleDbConnection cn, string genera)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = "02_YearlySummaryByGenera";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT * FROM " + str + " WHERE Genera=\"" + genera + "\";";
        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
        {
          if (oleDbDataReader.Read())
            this.Formed = (double) oleDbDataReader["CO formed"];
          oleDbDataReader.Close();
        }
      }
    }

    public void InsertSummary(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = "02_YearlySummaryByGenera";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "UPDATE " + str + " SET [CO removed] = " + this.Removed.ToString() + ",[CO net (formed-removed)] = " + this.Net.ToString() + " WHERE [Genera]=\"" + this.Genera + "\";";
        oleDbCommand.ExecuteNonQuery();
      }
    }
  }
}
