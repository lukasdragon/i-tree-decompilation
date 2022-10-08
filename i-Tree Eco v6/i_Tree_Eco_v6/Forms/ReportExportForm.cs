// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ReportExportForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.Win.C1Ribbon;
using Eco.Domain.v6;
using i_Tree_Eco_v6.Reports;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace i_Tree_Eco_v6.Forms
{
  public class ReportExportForm : Form
  {
    private ReportOptions _reportOpt;
    private Dictionary<string, CheckState> dictCompStructure = new Dictionary<string, CheckState>();
    private Dictionary<string, CheckState> dictBenefitsCosts = new Dictionary<string, CheckState>();
    private Dictionary<string, CheckState> dictMeasuredTrees = new Dictionary<string, CheckState>();
    private IContainer components;
    private Label lblExport;
    private Button btnExport;
    private Panel panel2;
    private Panel panel3;
    private CheckBox chkCompStructure;
    private TableLayoutPanel tableLayoutPanel1;
    private Panel panel4;
    private Panel panel1;
    private CheckBox chkBenefitsCosts;
    private CheckBox chkMeasuredTree;
    private RadioButton radioButton3;
    private RadioButton radioButton2;
    private RadioButton radioButton1;
    private GroupBox groupBox1;

    public ReportExportForm()
    {
      this.InitializeComponent();
      ProgramSession session = Program.Session;
      this._reportOpt = new ReportOptions(session.InputSession.SessionFactory.OpenSession().CreateCriteria<Year>().Add((ICriterion) Restrictions.Eq("Guid", (object) session.InputSession.YearKey)).Fetch(SelectMode.Fetch, "Series").UniqueResult<Year>());
      this.SetRibbonGroup(((MainRibbonForm) Application.OpenForms["MainRibbonForm"]).rgReports, "Formatted Reports");
    }

    private void SetRibbonGroup(RibbonGroup rg, string groupTitle)
    {
      foreach (object obj in (CollectionBase) rg.Items)
      {
        if (obj.GetType() == typeof (RibbonMenu))
        {
          RibbonMenu rm = (RibbonMenu) obj;
          if (rm.Enabled)
          {
            ReportCondition condition = this._reportOpt.GetCondition(rm.Name);
            if (condition == null || condition.Enabled)
            {
              if (rm.Name == "resourceStructuralAnalysisSampleMenuButton")
                this.SetMenuItems(rm, this.panel1, this.dictCompStructure);
              else if (rm.Name == "resourceEcosystemServicesSampleMenuButton")
                this.SetMenuItems(rm, this.panel2, this.dictBenefitsCosts);
              else if (rm.Name == "FullInventoryIndividualTreeDetailsMenuButton")
                this.SetMenuItems(rm, this.panel3, this.dictMeasuredTrees);
            }
          }
        }
        else if (obj.GetType() == typeof (RibbonButton))
        {
          RibbonButton ribbonButton = (RibbonButton) obj;
          if (ribbonButton.Enabled)
          {
            ReportCondition condition = this._reportOpt.GetCondition(ribbonButton.Name);
            if (condition == null || condition.Enabled)
            {
              string text = ribbonButton.Text;
            }
          }
        }
      }
    }

    private void SetMenuItems(
      RibbonMenu rm,
      Panel panel,
      Dictionary<string, CheckState> dictPanelItems)
    {
      int menuHeight = 0;
      ReportExportMenuCategory category = (ReportExportMenuCategory) null;
      bool flag = false;
      foreach (object obj in (CollectionBase) rm.Items)
      {
        if (obj.GetType() == typeof (RibbonLabel))
        {
          if (category != null && category.checkedListBoxMenuItems.Items.Count > 0)
            this.AddCategory(category, ref menuHeight, panel, dictPanelItems);
          category = new ReportExportMenuCategory(panel.Name);
          flag = true;
          RibbonLabel ribbonLabel = (RibbonLabel) obj;
          if (ribbonLabel.Enabled)
          {
            ReportCondition condition = this._reportOpt.GetCondition(ribbonLabel.Name);
            if (condition == null || condition.Enabled)
            {
              category.chkMenuLabel.Text = ribbonLabel.Text;
              category.chkMenuLabel.Tag = (object) ribbonLabel.Name;
            }
          }
          else
            continue;
        }
        if (obj.GetType() == typeof (RibbonButton))
        {
          RibbonButton ribbonButton = (RibbonButton) obj;
          if (ribbonButton.Enabled && flag)
          {
            ReportCondition condition = this._reportOpt.GetCondition(ribbonButton.Name);
            if (condition == null || condition.Enabled)
            {
              ReportExportItem reportExportItem = new ReportExportItem()
              {
                menuItemName = ribbonButton.Name,
                menuItemTitle = ribbonButton.Text
              };
              category.checkedListBoxMenuItems.Items.Add((object) reportExportItem);
            }
          }
        }
      }
      if (category.checkedListBoxMenuItems.Items.Count <= 0)
        return;
      this.AddCategory(category, ref menuHeight, panel, dictPanelItems);
    }

    private void AddCategory(
      ReportExportMenuCategory category,
      ref int menuHeight,
      Panel panel,
      Dictionary<string, CheckState> dictPanelItems)
    {
      category.Top = menuHeight;
      category.Height = category.chkMenuLabel.Height + category.checkedListBoxMenuItems.PreferredHeight + 9;
      panel.Controls.Add((Control) category);
      dictPanelItems.Add((string) category.chkMenuLabel.Tag, CheckState.Unchecked);
      menuHeight += category.Height;
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      int FileType = 0;
      if (this.radioButton2.Checked)
        FileType = 1;
      else if (this.radioButton3.Checked)
        FileType = 2;
      string ExportPath = "C:\\Exports";
      this.ExportReports(this.panel1, ExportPath, FileType);
      this.ExportReports(this.panel2, ExportPath, FileType);
      this.ExportReports(this.panel3, ExportPath, FileType);
    }

    private void ExportReports(Panel panel, string ExportPath, int FileType)
    {
      foreach (Control control in (ArrangedElementCollection) panel.Controls)
      {
        if (control.GetType() == typeof (ReportExportMenuCategory))
        {
          foreach (ReportExportItem checkedItem in ((ReportExportMenuCategory) control).checkedListBoxMenuItems.CheckedItems)
            ReportExportHelper.RunExport(checkedItem.menuItemName, FileType, ExportPath);
        }
      }
    }

    public void ManageCheckBox(string panelName, string checkBoxName, CheckState checkState)
    {
      Dictionary<string, CheckState> dictionary = new Dictionary<string, CheckState>();
      Dictionary<string, CheckState> source;
      CheckBox checkBox;
      if (panelName == this.panel1.Name)
      {
        source = this.dictCompStructure;
        checkBox = this.chkCompStructure;
      }
      else if (panelName == this.panel2.Name)
      {
        source = this.dictBenefitsCosts;
        checkBox = this.chkBenefitsCosts;
      }
      else
      {
        source = this.dictMeasuredTrees;
        checkBox = this.chkMeasuredTree;
      }
      source[checkBoxName] = checkState;
      int num1 = source.Count<KeyValuePair<string, CheckState>>((Func<KeyValuePair<string, CheckState>, bool>) (x => x.Value == CheckState.Checked));
      int num2 = source.Count<KeyValuePair<string, CheckState>>((Func<KeyValuePair<string, CheckState>, bool>) (x => x.Value == CheckState.Indeterminate));
      if (num1 == source.Count<KeyValuePair<string, CheckState>>())
        checkBox.CheckState = CheckState.Checked;
      else if (num1 == 0 && num2 == 0)
        checkBox.CheckState = CheckState.Unchecked;
      else
        checkBox.CheckState = CheckState.Indeterminate;
    }

    private void chkCategory_CheckStateChanged(object sender, EventArgs e)
    {
      CheckBox checkBox = (CheckBox) sender;
      CheckState checkState = checkBox.CheckState;
      Panel panel = !(checkBox.Name == this.chkCompStructure.Name) ? (!(checkBox.Name == this.chkBenefitsCosts.Name) ? this.panel3 : this.panel2) : this.panel1;
      if (checkState != CheckState.Checked && checkState != CheckState.Unchecked)
        return;
      foreach (Control control in (ArrangedElementCollection) panel.Controls)
      {
        if (control.GetType() == typeof (ReportExportMenuCategory))
          ((ReportExportMenuCategory) control).chkMenuLabel.CheckState = checkState;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblExport = new Label();
      this.chkCompStructure = new CheckBox();
      this.btnExport = new Button();
      this.panel2 = new Panel();
      this.panel3 = new Panel();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.panel1 = new Panel();
      this.chkBenefitsCosts = new CheckBox();
      this.chkMeasuredTree = new CheckBox();
      this.panel4 = new Panel();
      this.groupBox1 = new GroupBox();
      this.radioButton3 = new RadioButton();
      this.radioButton1 = new RadioButton();
      this.radioButton2 = new RadioButton();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel4.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.lblExport.AutoSize = true;
      this.lblExport.Location = new Point(3, 3);
      this.lblExport.Name = "lblExport";
      this.lblExport.Size = new Size(434, 13);
      this.lblExport.TabIndex = 2;
      this.lblExport.Text = "Select the reports you would like to export and the file format. Then click the Export button.";
      this.chkCompStructure.AutoSize = true;
      this.chkCompStructure.Location = new Point(1, 1);
      this.chkCompStructure.Margin = new Padding(1);
      this.chkCompStructure.Name = "chkCompStructure";
      this.chkCompStructure.Size = new Size(150, 17);
      this.chkCompStructure.TabIndex = 1;
      this.chkCompStructure.Text = "Composition and Structure";
      this.chkCompStructure.UseVisualStyleBackColor = true;
      this.chkCompStructure.CheckStateChanged += new EventHandler(this.chkCategory_CheckStateChanged);
      this.btnExport.Location = new Point(739, 19);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(75, 23);
      this.btnExport.TabIndex = 4;
      this.btnExport.Text = "Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.panel2.AutoScroll = true;
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(287, 22);
      this.panel2.Margin = new Padding(18, 3, 3, 3);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(249, 449);
      this.panel2.TabIndex = 5;
      this.panel3.AutoScroll = true;
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(557, 22);
      this.panel3.Margin = new Padding(18, 3, 3, 3);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(257, 449);
      this.panel3.TabIndex = 6;
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
      this.tableLayoutPanel1.Controls.Add((Control) this.panel2, 1, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.panel3, 2, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.chkCompStructure, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.panel1, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.chkBenefitsCosts, 1, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.chkMeasuredTree, 2, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 47);
      this.tableLayoutPanel1.Margin = new Padding(3, 3, 3, 1);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Size = new Size(817, 474);
      this.tableLayoutPanel1.TabIndex = 8;
      this.panel1.AutoScroll = true;
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(18, 22);
      this.panel1.Margin = new Padding(18, 3, 3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(248, 449);
      this.panel1.TabIndex = 7;
      this.chkBenefitsCosts.AutoSize = true;
      this.chkBenefitsCosts.Location = new Point(270, 1);
      this.chkBenefitsCosts.Margin = new Padding(1);
      this.chkBenefitsCosts.Name = "chkBenefitsCosts";
      this.chkBenefitsCosts.Size = new Size(114, 17);
      this.chkBenefitsCosts.TabIndex = 8;
      this.chkBenefitsCosts.Text = "Benefits and Costs";
      this.chkBenefitsCosts.UseVisualStyleBackColor = true;
      this.chkBenefitsCosts.CheckStateChanged += new EventHandler(this.chkCategory_CheckStateChanged);
      this.chkMeasuredTree.AutoSize = true;
      this.chkMeasuredTree.Location = new Point(540, 1);
      this.chkMeasuredTree.Margin = new Padding(1);
      this.chkMeasuredTree.Name = "chkMeasuredTree";
      this.chkMeasuredTree.Size = new Size(133, 17);
      this.chkMeasuredTree.TabIndex = 9;
      this.chkMeasuredTree.Text = "Measured Tree Details";
      this.chkMeasuredTree.UseVisualStyleBackColor = true;
      this.chkMeasuredTree.CheckStateChanged += new EventHandler(this.chkCategory_CheckStateChanged);
      this.panel4.Controls.Add((Control) this.groupBox1);
      this.panel4.Controls.Add((Control) this.lblExport);
      this.panel4.Controls.Add((Control) this.btnExport);
      this.panel4.Dock = DockStyle.Top;
      this.panel4.Location = new Point(0, 0);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(817, 47);
      this.panel4.TabIndex = 9;
      this.groupBox1.Controls.Add((Control) this.radioButton3);
      this.groupBox1.Controls.Add((Control) this.radioButton1);
      this.groupBox1.Controls.Add((Control) this.radioButton2);
      this.groupBox1.Location = new Point(460, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(169, 41);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "File Format";
      this.radioButton3.AutoSize = true;
      this.radioButton3.Location = new Point(115, 19);
      this.radioButton3.Name = "radioButton3";
      this.radioButton3.Size = new Size(47, 17);
      this.radioButton3.TabIndex = 7;
      this.radioButton3.TabStop = true;
      this.radioButton3.Text = "Both";
      this.radioButton3.UseVisualStyleBackColor = true;
      this.radioButton1.AutoSize = true;
      this.radioButton1.Checked = true;
      this.radioButton1.Location = new Point(6, 19);
      this.radioButton1.Name = "radioButton1";
      this.radioButton1.Size = new Size(46, 17);
      this.radioButton1.TabIndex = 5;
      this.radioButton1.TabStop = true;
      this.radioButton1.Text = "PDF";
      this.radioButton1.UseVisualStyleBackColor = true;
      this.radioButton2.AutoSize = true;
      this.radioButton2.Location = new Point(58, 19);
      this.radioButton2.Name = "radioButton2";
      this.radioButton2.Size = new Size(51, 17);
      this.radioButton2.TabIndex = 6;
      this.radioButton2.TabStop = true;
      this.radioButton2.Text = "Excel";
      this.radioButton2.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.AutoSize = true;
      this.ClientSize = new Size(817, 521);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Controls.Add((Control) this.panel4);
      this.Name = nameof (ReportExportForm);
      this.Text = nameof (ReportExportForm);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
