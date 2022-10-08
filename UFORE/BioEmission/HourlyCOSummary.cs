// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.HourlyCOSummary
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using UFORE.Deposition;

namespace UFORE.BioEmission
{
  internal class HourlyCOSummary
  {
    private int Hour;
    private double avgRemoved;
    private double net;

    public static void CalcSummary(
      City ctData,
      ref List<DryDeposition> COList,
      DryDeposition.VEG_TYPE veg,
      bool inv,
      OleDbConnection cnBioEDB)
    {
      HourlyCOSummary hourlyCoSummary = new HourlyCOSummary();
      List<Formation> formationList = new List<Formation>();
      Formation.ReadHourlyCOFormationRecords(cnBioEDB, ref formationList);
      hourlyCoSummary.AddFields(cnBioEDB);
      for (int hr = 0; hr < 24; ++hr)
      {
        hourlyCoSummary.CalcSummary(ctData, ref COList, ref formationList, hr, veg, inv);
        hourlyCoSummary.InsertSummary(cnBioEDB);
      }
    }

    private void AddFields(OleDbConnection cn)
    {
      string tbl = "07_HourlySummary";
      AccessFunc.AppendField(cn, tbl, "CO removed", "DOUBLE");
      AccessFunc.AppendField(cn, tbl, "CO net (formed-removed)", "DOUBLE");
    }

    private void CalcSummary(
      City ctData,
      ref List<DryDeposition> ddList,
      ref List<Formation> frmList,
      int hr,
      DryDeposition.VEG_TYPE veg,
      bool bInv)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      int num3 = 0;
      if (bInv)
        Math.Pow(10.0, -6.0);
      else
        Math.Pow(10.0, -9.0);
      double num4 = !bInv ? Math.Pow(10.0, -3.0) : 1.0;
      double num5 = veg == DryDeposition.VEG_TYPE.TREE ? ctData.TreePctCov : ctData.ShrubPctCov;
      this.Hour = hr;
      for (int index = 0; index < frmList.Count; ++index)
      {
        if (frmList[index].Hour == this.Hour)
          num1 += frmList[index].COFormed;
      }
      for (int index = 0; index < ddList.Count; ++index)
      {
        if (ddList[index].GrowSeason && ddList[index].TimeStamp.Hour == this.Hour)
        {
          num2 += ddList[index].Flux;
          ++num3;
        }
      }
      this.avgRemoved = num2 / (double) num3 * (ctData.AreaM2 * num5 / 100.0) * num4;
      this.net = num1 - this.avgRemoved;
    }

    private void InsertSummary(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str = "07_HourlySummary";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "UPDATE " + str + " SET [CO removed] = " + this.avgRemoved.ToString() + ",[CO net (formed-removed)] = " + this.net.ToString() + " WHERE [Hour]=" + this.Hour.ToString() + ";";
        oleDbCommand.ExecuteNonQuery();
      }
    }
  }
}
