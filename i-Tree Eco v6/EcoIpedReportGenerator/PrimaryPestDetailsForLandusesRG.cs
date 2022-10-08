// Decompiled with JetBrains decompiler
// Type: EcoIpedReportGenerator.PrimaryPestDetailsForLandusesRG
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.Properties;
using i_Tree_Eco_v6.Resources;
using System.Collections.Generic;
using System.Data;

namespace EcoIpedReportGenerator
{
  internal class PrimaryPestDetailsForLandusesRG : ReportGenerator, IBindable
  {
    private List<DataBinding> m_bindings = new List<DataBinding>();
    private PestData m_output;

    public List<DataBinding> Bindings() => this.m_bindings;

    public PrimaryPestDetailsForLandusesRG()
    {
      this.m_output = new PestData();
      DataBinding dataBinding = new DataBinding(0);
      DataTable dataTable = staticData.DsData.Tables["Strata"].Copy();
      DataRow row = dataTable.NewRow();
      row["Id"] = (object) -1;
      row["Description"] = (object) Strings.AllStrata;
      dataTable.Rows.Add(row);
      dataTable.DefaultView.Sort = "Id";
      dataBinding.DisplayMember = "Description";
      dataBinding.Description = v6Strings.Strata_SingularName;
      dataBinding.ValueMember = "Description";
      dataBinding.DataSource = (object) dataTable.DefaultView;
      this.m_bindings.Add(dataBinding);
    }

    public override object Generate()
    {
      FlexGridReport flexGridReport = new FlexGridReport();
      flexGridReport.Title = (string) this.m_bindings[0].Value == Strings.AllStrata ? Strings.ReportTitlePrimaryPestImpactedTreeDetailsByStratum : string.Format("{0} {1}", (object) Strings.ReportTitlePrimaryPestImpactedTreeDetailsForStratum, (object) (string) this.m_bindings[0].Value);
      List<GridInfo> gridInfoList = new List<GridInfo>();
      flexGridReport.Data.Add(new GridInfo(string.Empty, (object) this.m_output.BuildPrimaryPestDetailsForLandusesSample((string) this.m_bindings[0].Value).Tables["Trees"].DefaultView, string.Empty, false));
      return (object) flexGridReport;
    }
  }
}
