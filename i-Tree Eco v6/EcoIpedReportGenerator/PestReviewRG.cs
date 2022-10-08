// Decompiled with JetBrains decompiler
// Type: EcoIpedReportGenerator.PestReviewRG
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace EcoIpedReportGenerator
{
  public class PestReviewRG : ReportGenerator, IBindable
  {
    private List<DataBinding> m_bindings;
    private PestData m_output;
    private BindingSource withEventsField_m_bsPestCmn;
    private BindingSource withEventsField_m_bsPestSci;

    private BindingSource m_bsPestCmn
    {
      get => this.withEventsField_m_bsPestCmn;
      set
      {
        if (this.withEventsField_m_bsPestCmn != null)
          this.withEventsField_m_bsPestCmn.PositionChanged -= new EventHandler(this.m_bsPestCmn_PositionChanged);
        this.withEventsField_m_bsPestCmn = value;
        if (this.withEventsField_m_bsPestCmn == null)
          return;
        this.withEventsField_m_bsPestCmn.PositionChanged += new EventHandler(this.m_bsPestCmn_PositionChanged);
      }
    }

    private BindingSource m_bsPestSci
    {
      get => this.withEventsField_m_bsPestSci;
      set
      {
        if (this.withEventsField_m_bsPestSci != null)
          this.withEventsField_m_bsPestSci.PositionChanged -= new EventHandler(this.m_bsPestSci_PositionChanged);
        this.withEventsField_m_bsPestSci = value;
        if (this.withEventsField_m_bsPestSci == null)
          return;
        this.withEventsField_m_bsPestSci.PositionChanged += new EventHandler(this.m_bsPestSci_PositionChanged);
      }
    }

    public PestReviewRG()
    {
      DataView dataSource1 = new DataView(staticData.DsData.Tables["Pests"], "Id > 0", "CommonName ASC", DataViewRowState.CurrentRows);
      DataView dataSource2 = new DataView(staticData.DsData.Tables["Pests"], "Id > 0", "ScientificName ASC", DataViewRowState.CurrentRows);
      this.m_bsPestCmn = new BindingSource((object) dataSource1, (string) null);
      this.m_bsPestSci = new BindingSource((object) dataSource2, (string) null);
      this.m_bsPestSci.Position = this.m_bsPestSci.Find("Id", ((DataRowView) this.m_bsPestCmn[this.m_bsPestCmn.Position])["Id"]);
      this.m_bindings = new List<DataBinding>();
      this.m_bindings.Add(new DataBinding(7)
      {
        Description = Strings.PestCommonName,
        DataSource = (object) this.m_bsPestCmn,
        DisplayMember = "CommonName",
        ValueMember = "Id"
      });
      this.m_bindings.Add(new DataBinding(7)
      {
        Description = Strings.PestScientificName,
        DataSource = (object) this.m_bsPestSci,
        DisplayMember = "ScientificName",
        ValueMember = "Id"
      });
      this.m_output = new PestData();
    }

    public override object Generate()
    {
      int int32 = Convert.ToInt32(this.m_bindings[0].Value);
      Dictionary<string, DataSet> recordsForPest = this.m_output.GetRecordsForPest(int32);
      return (object) new FlexGridReport()
      {
        Title = string.Format(Strings.PestReviewForPestNmaeOfAssessedTrees, (object) this.m_output.GetPestName(int32)),
        Data = {
          new GridInfo(Strings.PestSignsSlashSymptoms, (object) recordsForPest["Symptoms"].DefaultViewManager, "Data", false),
          new GridInfo(Strings.MatchingRecordsDashPrimaryHosts, (object) recordsForPest["HostRecords"].DefaultViewManager, "Data", false),
          new GridInfo(Strings.MatchingRecordsDashOtherHosts, (object) recordsForPest["OtherRecords"].DefaultViewManager, "Data", false)
        }
      };
    }

    public List<DataBinding> Bindings() => this.m_bindings;

    private void m_bsPestCmn_PositionChanged(object sender, EventArgs e) => this.m_bsPestSci.Position = this.m_bsPestSci.Find("Id", ((DataRowView) this.m_bsPestCmn[this.m_bsPestCmn.Position])["Id"]);

    private void m_bsPestSci_PositionChanged(object sender, EventArgs e) => this.m_bsPestCmn.Position = this.m_bsPestCmn.Find("Id", ((DataRowView) this.m_bsPestSci[this.m_bsPestSci.Position])["Id"]);
  }
}
