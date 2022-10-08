// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ReportExportMenuCategory
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ReportExportMenuCategory : UserControl
  {
    private string _panelName;
    private IContainer components;
    public CheckBox chkMenuLabel;
    public CheckedListBox checkedListBoxMenuItems;

    public ReportExportMenuCategory(string panelName)
    {
      this.InitializeComponent();
      this._panelName = panelName;
    }

    public override string ToString() => this.chkMenuLabel.Text;

    private void checkedListBoxMenuItems_ClientSizeChanged(object sender, EventArgs e)
    {
      if (this.checkedListBoxMenuItems.Height >= this.checkedListBoxMenuItems.PreferredHeight)
        return;
      this.checkedListBoxMenuItems.Height = this.checkedListBoxMenuItems.PreferredHeight;
    }

    private void chkMenuLabel_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkMenuLabel.CheckState == CheckState.Indeterminate)
        return;
      for (int index = 0; index < this.checkedListBoxMenuItems.Items.Count; ++index)
        this.checkedListBoxMenuItems.SetItemChecked(index, this.chkMenuLabel.CheckState == CheckState.Checked);
    }

    private void chkMenuLabel_CheckStateChanged(object sender, EventArgs e) => ((ReportExportForm) this.Parent.FindForm()).ManageCheckBox(this._panelName, (string) this.chkMenuLabel.Tag, this.chkMenuLabel.CheckState);

    private void checkedListBoxMenuItems_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.chkMenuLabel.CheckState = this.checkedListBoxMenuItems.CheckedItems.Count >= this.checkedListBoxMenuItems.Items.Count ? CheckState.Checked : (this.checkedListBoxMenuItems.CheckedItems.Count != 0 ? CheckState.Indeterminate : CheckState.Unchecked);
      this.checkedListBoxMenuItems.ClearSelected();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.chkMenuLabel = new CheckBox();
      this.checkedListBoxMenuItems = new CheckedListBox();
      this.SuspendLayout();
      this.chkMenuLabel.AutoSize = true;
      this.chkMenuLabel.Dock = DockStyle.Top;
      this.chkMenuLabel.Location = new Point(0, 0);
      this.chkMenuLabel.Name = "chkMenuLabel";
      this.chkMenuLabel.Size = new Size(242, 17);
      this.chkMenuLabel.TabIndex = 0;
      this.chkMenuLabel.Text = "Report Category";
      this.chkMenuLabel.UseVisualStyleBackColor = true;
      this.chkMenuLabel.CheckedChanged += new EventHandler(this.chkMenuLabel_CheckedChanged);
      this.chkMenuLabel.CheckStateChanged += new EventHandler(this.chkMenuLabel_CheckStateChanged);
      this.checkedListBoxMenuItems.CheckOnClick = true;
      this.checkedListBoxMenuItems.Dock = DockStyle.Right;
      this.checkedListBoxMenuItems.FormattingEnabled = true;
      this.checkedListBoxMenuItems.HorizontalScrollbar = true;
      this.checkedListBoxMenuItems.Location = new Point(12, 17);
      this.checkedListBoxMenuItems.Name = "checkedListBoxMenuItems";
      this.checkedListBoxMenuItems.Size = new Size(230, 21);
      this.checkedListBoxMenuItems.TabIndex = 1;
      this.checkedListBoxMenuItems.SelectedIndexChanged += new EventHandler(this.checkedListBoxMenuItems_SelectedIndexChanged);
      this.checkedListBoxMenuItems.ClientSizeChanged += new EventHandler(this.checkedListBoxMenuItems_ClientSizeChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.Controls.Add((Control) this.checkedListBoxMenuItems);
      this.Controls.Add((Control) this.chkMenuLabel);
      this.Name = nameof (ReportExportMenuCategory);
      this.Size = new Size(242, 38);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
