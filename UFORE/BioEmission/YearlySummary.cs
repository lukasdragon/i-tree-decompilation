// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.YearlySummary
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System.Data.OleDb;

namespace UFORE.BioEmission
{
  internal class YearlySummary
  {
    public const string YEAR_LANDUSE_TABLE = "01_YearlySummaryByLanduse";
    public const string YEAR_GENERA_TABLE = "02_YearlySummaryByGenera";

    public static void CalcSummaryByLanduse(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = "YearlyEmission";
        string str2 = "01_YearlySummaryByLanduse";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW EM.Landuse, EM.LanduseDesc, Sum(EM.[Isoprene emitted]) AS [Isoprene emitted], Sum(EM.[Monoterpene emitted]) AS [Monoterpene emitted],Sum(EM.[Other VOCs emitted]) AS [Other VOCs emitted] INTO [" + str2 + "] FROM " + str1 + " AS EM GROUP BY EM.Landuse, EM.LanduseDesc;";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    public static void CalcSummaryByGenera(OleDbConnection cn)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = "YearlyEmission";
        string str2 = "02_YearlySummaryByGenera";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW EM.Genera, Sum(EM.[Isoprene emitted]) AS [Isoprene emitted], Sum(EM.[Monoterpene emitted]) AS [Monoterpene emitted],Sum(EM.[Other VOCs emitted]) AS [Other VOCs emitted] INTO [" + str2 + "] FROM " + str1 + " AS EM GROUP BY EM.Genera;";
        oleDbCommand.ExecuteNonQuery();
      }
    }
  }
}
