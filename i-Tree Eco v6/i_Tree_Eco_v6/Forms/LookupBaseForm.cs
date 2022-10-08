// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.LookupBaseForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class LookupBaseForm : ProjectContentForm
  {
    private IContainer components;
    protected DataGridViewNumericTextBoxColumn dcId;
    protected DataGridViewTextBoxColumn dcDescription;
    protected DataGridView dgLookup;

    public LookupBaseForm()
      : this((string) null, (string) null, (string) null)
    {
    }

    public LookupBaseForm(string singularName)
      : this(singularName, (string) null, (string) null)
    {
    }

    public LookupBaseForm(string singularName, string pluralName)
      : this(singularName, pluralName, (string) null)
    {
    }

    public LookupBaseForm(string singularName, string pluralName, string helpTopic)
    {
      this.InitializeComponent();
      this.SingularName = singularName == null ? string.Empty : singularName;
      this.PluralName = pluralName == null ? string.Empty : pluralName;
      this.Text = this.SingularName;
      this.Tag = (object) helpTopic;
    }

    protected string SingularName { get; private set; }

    protected string PluralName { get; private set; }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LookupBaseForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      this.dgLookup = new DataGridView();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcDescription = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dgLookup).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgLookup.AllowUserToAddRows = false;
      this.dgLookup.AllowUserToDeleteRows = false;
      this.dgLookup.AllowUserToResizeRows = false;
      this.dgLookup.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgLookup.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgLookup.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgLookup.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcDescription);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgLookup.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgLookup, "dgLookup");
      this.dgLookup.EnableHeadersVisualStyles = false;
      this.dgLookup.MultiSelect = false;
      this.dgLookup.Name = "dgLookup";
      this.dgLookup.ReadOnly = true;
      this.dgLookup.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgLookup.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      this.dgLookup.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgLookup.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgLookup.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dcId.DataPropertyName = "Id";
      this.dcId.DecimalPlaces = 0;
      this.dcId.Format = "#;#";
      this.dcId.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcId, "dcId");
      this.dcId.Name = "dcId";
      this.dcId.ReadOnly = true;
      this.dcId.Resizable = DataGridViewTriState.True;
      this.dcId.Signed = false;
      this.dcDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDescription.DataPropertyName = "Description";
      componentResourceManager.ApplyResources((object) this.dcDescription, "dcDescription");
      this.dcDescription.MaxInputLength = 50;
      this.dcDescription.Name = "dcDescription";
      this.dcDescription.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.dgLookup);
      this.DockAreas = DockAreas.Document;
      this.Name = nameof (LookupBaseForm);
      this.ShowHint = DockState.Document;
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgLookup, 0);
      ((ISupportInitialize) this.dgLookup).EndInit();
      this.ResumeLayout(false);
    }
  }
}
