// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ReportExportHelper
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Reports
{
  public static class ReportExportHelper
  {
    public static void RunExport(string buttonName, int fileType, string directory)
    {
      Graphics graphics = new Form().CreateGraphics();
      Report report1;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(buttonName))
      {
        case 412696751:
          if (!(buttonName == "AnnualNetCarbonSequestrationOfTreesByStrataChartMenuItem"))
            return;
          report1 = (Report) new Report<NetCarbonSequestrationByStrata>();
          break;
        case 799678258:
          if (!(buttonName == "AnnualNetCarbonSequestrationOfTreesBySpeciesMenuItem"))
            return;
          report1 = (Report) new Report<NetCarbonSequestrationBySpecies>();
          break;
        case 835894039:
          if (!(buttonName == "rbStructureSummaryBySpecies"))
            return;
          report1 = (Report) new Report<StructureSummaryBySpecies>();
          break;
        case 908065479:
          if (!(buttonName == "rbStreetTreesByStrata"))
            return;
          report1 = (Report) new Report<StreetTrees>();
          break;
        case 1371567746:
          if (!(buttonName == "numberOfTreesPerUnitAreaByStrataChartMenuItem"))
            return;
          report1 = (Report) new Report<PopulationSummaryByStrataPerUnitArea>();
          break;
        case 1615050677:
          if (!(buttonName == "AnnualNetCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem"))
            return;
          report1 = (Report) new Report<NetCarbonSequestrationByStrataPerUnitArea>();
          break;
        case 2804167542:
          if (!(buttonName == "numberOfTreesByStrataChartMenuItem"))
            return;
          report1 = (Report) new Report<PopulationSummaryByStrata>();
          break;
        case 3492859963:
          if (!(buttonName == "numberOfTreesBySpecies"))
            return;
          report1 = (Report) new Report<PopulationSummaryBySpecies>();
          break;
        case 3534502800:
          if (!(buttonName == "rbStructureSummaryByStrataSpecies"))
            return;
          report1 = (Report) new Report<StructureSummaryByStrataSpecies>();
          break;
        case 3800444543:
          if (!(buttonName == "rbPublicPrivateByStrata"))
            return;
          report1 = (Report) new Report<PublicPrivate>();
          break;
        default:
          return;
      }
      C1PrintDocument report2 = report1.GenerateReport(graphics);
      string reportTitle = report1.ReportTitle;
      string empty = string.Empty;
      switch (fileType)
      {
        case 0:
          ReportExportHelper.ExportFile(report2, reportTitle, directory, ".pdf");
          break;
        case 1:
          ReportExportHelper.ExportFile(report2, reportTitle, directory, ".xlsx");
          break;
        case 2:
          ReportExportHelper.ExportFile(report2, reportTitle, directory, ".xlsx");
          ReportExportHelper.ExportFile(report2, reportTitle, directory, ".pdf");
          break;
      }
    }

    private static void ExportFile(
      C1PrintDocument doc,
      string reportName,
      string directory,
      string fileExt)
    {
      string path2 = string.Format("{0}_{1}{2}", (object) reportName.Replace(" ", "_"), (object) DateTime.Now.Ticks, (object) fileExt);
      doc.Export(Path.Combine(directory, path2));
    }
  }
}
