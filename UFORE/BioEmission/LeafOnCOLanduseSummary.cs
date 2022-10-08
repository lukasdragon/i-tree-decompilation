// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.LeafOnCOLanduseSummary
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Deposition;

namespace UFORE.BioEmission
{
  internal class LeafOnCOLanduseSummary
  {
    private const int PPM2PPB = 1000;
    private string LanduseDesc;
    private double totalRemoved;
    private double net;
    private double changeInForestPct;
    private double changeInForestPPB;
    private double changeInLandusePct;
    private double changeInLandusePPB;

    public static void CalcSummaryByLanduse(
      ref List<string> landuse,
      ref Dictionary<string, Landuse> luDict,
      ref List<DryDeposition> COList,
      DryDeposition.VEG_TYPE veg,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Formation> formationList = new List<Formation>();
      Formation.ReadLeafOnCOFormationRecords(cnBioEDB, ref formationList);
      LeafOnCOLanduseSummary coLanduseSummary = new LeafOnCOLanduseSummary();
      coLanduseSummary.AddFields(cnBioEDB);
      for (int index = 0; index < landuse.Count; ++index)
      {
        Landuse lu;
        luDict.TryGetValue(landuse[index], out lu);
        coLanduseSummary.CalcSummary(lu, ref COList, ref formationList, veg, inv);
        coLanduseSummary.InsertSummary(cnBioEDB);
      }
    }

    private void AddFields(OleDbConnection cn)
    {
      string tbl = "03_LeafOnSummaryByLanduse";
      AccessFunc.AppendField(cn, tbl, "CO removed", "DOUBLE");
      AccessFunc.AppendField(cn, tbl, "CO net (formed-removed)", "DOUBLE");
      AccessFunc.AppendField(cn, tbl, "CO change in forest air conc %", "DOUBLE");
      AccessFunc.AppendField(cn, tbl, "CO change in forest air conc ppb", "DOUBLE");
      AccessFunc.AppendField(cn, tbl, "CO change in city air conc %", "DOUBLE");
      AccessFunc.AppendField(cn, tbl, "CO change in city air conc ppb", "DOUBLE");
    }

    private void CalcSummary(
      Landuse lu,
      ref List<DryDeposition> ddList,
      ref List<Formation> frmList,
      DryDeposition.VEG_TYPE veg,
      bool bInv)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = 0.0;
      double num5 = !bInv ? Math.Pow(10.0, -9.0) : Math.Pow(10.0, -6.0);
      double num6 = !bInv ? Math.Pow(10.0, -3.0) : 1.0;
      double num7 = veg == DryDeposition.VEG_TYPE.TREE ? lu.TreePctCov : lu.ShrubPctCov;
      this.LanduseDesc = lu.LanduseDesc;
      for (int index = 0; index < frmList.Count; ++index)
      {
        if (frmList[index].LanduseDesc == lu.LanduseDesc)
          num1 += frmList[index].COFormed;
      }
      for (int index = 0; index < ddList.Count; ++index)
      {
        num2 += ddList[index].Flux;
        num3 += ddList[index].uGm3 * ddList[index].MixHt;
        num4 += ddList[index].PPM * 1000.0;
      }
      double num8 = num4 / (double) ddList.Count;
      double num9 = num3 * (lu.LanduseArea * num7 / 100.0) * num5;
      double num10 = num3 * lu.LanduseArea * num5;
      this.totalRemoved = num2 * (lu.LanduseArea * num7 / 100.0) * num6;
      this.net = num1 - this.totalRemoved;
      double num11 = num9 - this.net;
      this.changeInForestPct = num11 == 0.0 ? 0.0 : this.net / num11 * 100.0;
      this.changeInForestPPB = num8 * this.changeInForestPct / 100.0;
      double net = this.net;
      double num12 = num10 - net;
      this.changeInLandusePct = num12 == 0.0 ? 0.0 : this.net / num12 * 100.0;
      this.changeInLandusePPB = num8 * this.changeInLandusePct / 100.0;
    }

    private void InsertSummary(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = "03_LeafOnSummaryByLanduse";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "UPDATE " + str + " SET [CO removed] = " + this.totalRemoved.ToString() + ",[CO net (formed-removed)] = " + this.net.ToString() + ",[CO change in forest air conc %] = " + this.changeInForestPct.ToString() + ",[CO change in forest air conc ppb] = " + this.changeInForestPPB.ToString() + ",[CO change in city air conc %] = " + this.changeInLandusePct.ToString() + ",[CO change in city air conc ppb] = " + this.changeInLandusePPB.ToString() + " WHERE [LanduseDesc]=\"" + this.LanduseDesc + "\";";
        oleDbCommand.ExecuteNonQuery();
      }
    }
  }
}
