// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.MonthlySummary
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System.Data.OleDb;

namespace UFORE.BioEmission
{
  internal class MonthlySummary
  {
    public const string MONTH_TABLE = "05_MonthlySummary";
    public const string MONTH_DAY_TABLE = "06_MonthlyDaytimeSummary";

    public static void CalcSummary(OleDbConnection cn, bool daytime)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        string str1 = daytime ? "MonthlyDaytimeEmission" : "MonthlyEmission";
        string str2 = daytime ? "MonthlyDaytimeCOFormation" : "MonthlyCOFormation";
        string str3 = daytime ? "MonthlyDaytimeO3Formation" : "MonthlyO3Formation";
        string str4 = daytime ? "06_MonthlyDaytimeSummary" : "05_MonthlySummary";
        oleDbCommand.Connection = cn;
        oleDbCommand.CommandText = "SELECT DISTINCTROW EM.Month, Sum(EM.[Isoprene emitted]) AS [Isoprene emitted], Sum(EM.[Monoterpene emitted]) AS [Monoterpene emitted],Sum(EM.[Other VOCs emitted]) AS [Other VOCs emitted],Sum(CO.[CO formed]) AS [CO formed],Sum(O3.[O3 formed minimum]) AS [O3 formed minimum],Sum(O3.[O3 formed]) AS [O3 formed],Sum(O3.[O3 formed maximum]) AS [O3 formed maximum] INTO [" + str4 + "] FROM (" + str2 + " AS CO INNER JOIN " + str1 + " AS EM ON CO.[ID] = EM.[ID]) INNER JOIN " + str3 + " AS O3 ON EM.[ID] = O3.[ID] GROUP BY EM.Month;";
        oleDbCommand.ExecuteNonQuery();
      }
    }
  }
}
