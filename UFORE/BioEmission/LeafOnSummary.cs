// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.LeafOnSummary
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System.Data.OleDb;

namespace UFORE.BioEmission
{
  internal class LeafOnSummary
  {
    public const string LFON_LANDUSE_TABLE = "03_LeafOnSummaryByLanduse";
    public const string LFON_GENERA_TABLE = "04_LeafOnSummaryByGenera";

    public static void CalcSummaryByLanduse(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = "LeafOnEmission";
        string str2 = "LeafOnCOFormation";
        string str3 = "LeafOnO3Formation";
        string str4 = "03_LeafOnSummaryByLanduse";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW EM.Landuse, EM.LanduseDesc, Sum(EM.[Isoprene emitted]) AS [Isoprene emitted], Sum(EM.[Monoterpene emitted]) AS [Monoterpene emitted],Sum(EM.[Other VOCs emitted]) AS [Other VOCs emitted],Sum(CO.[CO formed]) AS [CO formed],Sum(O3.[O3 formed minimum]) AS [O3 formed minimum],Sum(O3.[O3 formed]) AS [O3 formed],Sum(O3.[O3 formed maximum]) AS [O3 formed maximum] INTO [" + str4 + "] FROM (" + str2 + " AS CO INNER JOIN " + str1 + " AS EM ON CO.[ID] = EM.[ID]) INNER JOIN " + str3 + " AS O3 ON EM.[ID] = O3.[ID] GROUP BY EM.Landuse, EM.LanduseDesc;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    public static void CalcSummaryByGenera(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = "LeafOnEmission";
        string str2 = "LeafOnCOFormation";
        string str3 = "LeafOnO3Formation";
        string str4 = "04_LeafOnSummaryByGenera";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW EM.Genera, Sum(EM.[Isoprene emitted]) AS [Isoprene emitted], Sum(EM.[Monoterpene emitted]) AS [Monoterpene emitted],Sum(EM.[Other VOCs emitted]) AS [Other VOCs emitted],Sum(CO.[CO formed]) AS [CO formed],Sum(O3.[O3 formed minimum]) AS [O3 formed minimum],Sum(O3.[O3 formed]) AS [O3 formed],Sum(O3.[O3 formed maximum]) AS [O3 formed maximum] INTO [" + str4 + "] FROM (" + str2 + " AS CO INNER JOIN " + str1 + " AS EM ON CO.[ID] = EM.[ID]) INNER JOIN " + str3 + " AS O3 ON EM.[ID] = O3.[ID] GROUP BY EM.Genera;";
        oleDbCommand.ExecuteNonQuery();
      }
    }
  }
}
