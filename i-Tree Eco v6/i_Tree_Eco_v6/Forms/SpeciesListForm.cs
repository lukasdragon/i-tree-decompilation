// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.SpeciesListForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls.Extensions;
using Eco.Util;
using Eco.Util.Views;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class SpeciesListForm : ViewContentForm, IExportable
  {
    private ProgramSession m_ps;
    private DataGridViewManager m_dgManager;
    private TaskManager m_taskManager;
    private WaitCursor m_wc;
    private IContainer components;
    private DataGridView dgSpeciesList;
    private DataGridViewTextBoxColumn dcId;
    private DataGridViewTextBoxColumn dcCode;
    private DataGridViewTextBoxColumn dcScientificName;
    private DataGridViewTextBoxColumn dcCommonName;

    public SpeciesListForm()
    {
      this.InitializeComponent();
      this.m_ps = Program.Session;
      this.m_dgManager = new DataGridViewManager(this.dgSpeciesList);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.dgSpeciesList.AutoGenerateColumns = false;
      this.dgSpeciesList.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgSpeciesList.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.Init();
    }

    private void Init()
    {
      this.dgSpeciesList.DataSource = (object) new DataBindingList<SpeciesView>((IList<SpeciesView>) this.m_ps.Species.Values.ToList<SpeciesView>())
      {
        Sortable = true
      };
      this.dgSpeciesList.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      this.OnRequestRefresh();
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgSpeciesList.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgSpeciesList.DataSource != null;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SpeciesListForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      this.dgSpeciesList = new DataGridView();
      this.dcId = new DataGridViewTextBoxColumn();
      this.dcCode = new DataGridViewTextBoxColumn();
      this.dcScientificName = new DataGridViewTextBoxColumn();
      this.dcCommonName = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dgSpeciesList).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgSpeciesList.AllowUserToAddRows = false;
      this.dgSpeciesList.AllowUserToDeleteRows = false;
      this.dgSpeciesList.AllowUserToResizeRows = false;
      this.dgSpeciesList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgSpeciesList.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgSpeciesList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgSpeciesList.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcCode, (DataGridViewColumn) this.dcScientificName, (DataGridViewColumn) this.dcCommonName);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgSpeciesList.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgSpeciesList, "dgSpeciesList");
      this.dgSpeciesList.EnableHeadersVisualStyles = false;
      this.dgSpeciesList.Name = "dgSpeciesList";
      this.dgSpeciesList.ReadOnly = true;
      this.dgSpeciesList.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = SystemColors.Control;
      gridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.ForeColor = SystemColors.WindowText;
      gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.dgSpeciesList.RowHeadersDefaultCellStyle = gridViewCellStyle3;
      this.dgSpeciesList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dcId.DataPropertyName = "Id";
      componentResourceManager.ApplyResources((object) this.dcId, "dcId");
      this.dcId.Name = "dcId";
      this.dcId.ReadOnly = true;
      this.dcCode.DataPropertyName = "Code";
      componentResourceManager.ApplyResources((object) this.dcCode, "dcCode");
      this.dcCode.Name = "dcCode";
      this.dcCode.ReadOnly = true;
      this.dcScientificName.DataPropertyName = "ScientificName";
      componentResourceManager.ApplyResources((object) this.dcScientificName, "dcScientificName");
      this.dcScientificName.Name = "dcScientificName";
      this.dcScientificName.ReadOnly = true;
      this.dcCommonName.DataPropertyName = "CommonName";
      componentResourceManager.ApplyResources((object) this.dcCommonName, "dcCommonName");
      this.dcCommonName.Name = "dcCommonName";
      this.dcCommonName.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.dgSpeciesList);
      this.Name = nameof (SpeciesListForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgSpeciesList, 0);
      ((ISupportInitialize) this.dgSpeciesList).EndInit();
      this.ResumeLayout(false);
    }
  }
}
