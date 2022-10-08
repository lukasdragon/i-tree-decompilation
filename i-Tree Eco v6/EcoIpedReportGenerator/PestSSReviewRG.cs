// Decompiled with JetBrains decompiler
// Type: EcoIpedReportGenerator.PestSSReviewRG
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Resources;
using System.Data;
using System.Windows.Forms;

namespace EcoIpedReportGenerator
{
  public class PestSSReviewRG : ReportGenerator
  {
    private PestData m_output;
    private BindingSource m_bsRecords;
    private BindingSource m_bsStats;
    private DataSet m_dsSymptoms;

    public PestSSReviewRG()
    {
      this.m_output = new PestData();
      this.m_dsSymptoms = this.m_output.GetAllSymptoms();
      this.m_dsSymptoms.Tables["Data"].RowChanged += new DataRowChangeEventHandler(this.OnSymptoms_RowChanged);
    }

    public override object Generate()
    {
      FlexGridReport flexGridReport = new FlexGridReport();
      DataSet recordsForSymptoms = this.m_output.GetRecordsForSymptoms(this.m_output.GetSeletedSymptoms(this.m_dsSymptoms.Tables["Data"]));
      this.m_bsRecords = new BindingSource();
      this.m_bsRecords.DataMember = "Data";
      this.m_bsRecords.DataSource = (object) recordsForSymptoms;
      this.m_bsStats = new BindingSource();
      this.m_bsStats.DataMember = "SymptomStats";
      this.m_bsStats.DataSource = (object) recordsForSymptoms;
      flexGridReport.Title = Strings.ReportTitleFindTreesShowingSelectedSignsAndSymptoms;
      flexGridReport.Data.Add(new GridInfo(Strings.PestSignsSlashSymptoms, (object) this.m_dsSymptoms, "Data", true));
      flexGridReport.Data.Add(new GridInfo(Strings.MatchingTrees, (object) this.m_bsRecords, (string) null, false));
      flexGridReport.Data.Add(new GridInfo(Strings.AdditionalSignsSymptomsOfMatchingTrees, (object) this.m_bsStats, (string) null, false));
      return (object) flexGridReport;
    }

    private void OnSymptoms_RowChanged(object sender, DataRowChangeEventArgs e)
    {
      DataSet recordsForSymptoms = this.m_output.GetRecordsForSymptoms(this.m_output.GetSeletedSymptoms((DataTable) sender));
      this.m_bsRecords.DataSource = (object) recordsForSymptoms;
      this.m_bsStats.DataSource = (object) recordsForSymptoms;
    }
  }
}
