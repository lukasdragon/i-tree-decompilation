// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.YearlyCOLanduseSummary
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Deposition;

namespace UFORE.BioEmission
{
  internal class YearlyCOLanduseSummary
  {
    private const int PPM2PPB = 1000;
    private string LanduseDesc;
    private double TotalRemoved;
    private double Net;
    private double ChangeInForestPct;
    private double ChangeInForestPPB;
    private double ChangeInLandusePct;
    private double ChangeInLandusePPB;

    public static void CalcSummaryByLanduse(
      ref List<string> landuse,
      ref Dictionary<string, Landuse> luDict,
      ref List<DryDeposition> COList,
      DryDeposition.VEG_TYPE veg,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      List<Formation> formationList = new List<Formation>();
      Formation.ReadYearlyCOFormationRecords(cnBioEDB, ref formationList);
      YearlyCOLanduseSummary coLanduseSummary = new YearlyCOLanduseSummary();
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
      string tbl = "01_YearlySummaryByLanduse";
      AccessFunc.AppendField(cn, tbl, "CO removed", "DOUBLE");
      AccessFunc.AppendField(cn, tbl, "CO Net (formed-removed)", "DOUBLE");
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
      this.TotalRemoved = num2 * (lu.LanduseArea * num7 / 100.0) * num6;
      this.Net = num1 - this.TotalRemoved;
      double num11 = num9 - this.Net;
      this.ChangeInForestPct = num11 == 0.0 ? 0.0 : this.Net / num11 * 100.0;
      this.ChangeInForestPPB = num8 * this.ChangeInForestPct / 100.0;
      double net = this.Net;
      double num12 = num10 - net;
      this.ChangeInLandusePct = num12 == 0.0 ? 0.0 : this.Net / num12 * 100.0;
      this.ChangeInLandusePPB = num8 * this.ChangeInLandusePct / 100.0;
    }

    private void InsertSummary(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = "01_YearlySummaryByLanduse";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "UPDATE " + str + " SET [CO removed] = " + this.TotalRemoved.ToString() + ",[CO net (formed-removed)] = " + this.Net.ToString() + ",[CO change in forest air conc %] = " + this.ChangeInForestPct.ToString() + ",[CO change in forest air conc ppb] = " + this.ChangeInForestPPB.ToString() + ",[CO change in city air conc %] = " + this.ChangeInLandusePct.ToString() + ",[CO change in city air conc ppb] = " + this.ChangeInLandusePPB.ToString() + " WHERE [LanduseDesc]=\"" + this.LanduseDesc + "\";";
        oleDbCommand.ExecuteNonQuery();
      }
    }
  }
}
