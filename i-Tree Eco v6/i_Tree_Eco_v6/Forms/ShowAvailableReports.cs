// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ShowAvailableReports
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.Win.C1Ribbon;
using Eco.Domain.v6;
using Eco.Util;
using LocationSpecies.Domain;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ShowAvailableReports : Form
  {
    private int _disabledCount;
    private ReportOptions _reportOpt;
    private MainRibbonForm ribbonform;
    private IContainer components;
    private TreeView treeView1;
    private Button btnOK;
    private Button btnCancel;
    private Label lblOKCancel;
    private Label lblAvailability2;
    private Label lblAvail1;
    private Label lblAvail2;
    private Label lblAvail3;
    private Label lblInternational;
    private Panel panel1;
    private Panel panel2;
    private Panel panel3;

    public TreeView tvReportTree => this.treeView1;

    public ShowAvailableReports(Location nation, Year y, bool expand, bool export)
    {
      this._reportOpt = new ReportOptions(y);
      this._disabledCount = 0;
      this.InitializeComponent();
      this.BuildTree();
      if (expand)
        this.treeView1.ExpandAll();
      if (!export && this._disabledCount == 0)
      {
        this.lblAvail1.Text = i_Tree_Eco_v6.Resources.Strings.AllReportsWillBeAvailable;
        this.lblAvail2.Visible = false;
        this.lblAvail3.Visible = false;
      }
      else if (export)
      {
        this.treeView1.CheckBoxes = true;
        this.lblAvail1.Text = i_Tree_Eco_v6.Resources.Strings.SelectTheReportsYouWouldLikeToExport;
        this.lblAvail2.Visible = false;
        this.lblAvail3.Visible = false;
      }
      if (nation != null && !NationFeatures.IsUSorUSlikeNation(nation.Id))
        return;
      this.lblInternational.Visible = false;
    }

    private void BuildTree()
    {
      this.ribbonform = (MainRibbonForm) Application.OpenForms["MainRibbonForm"];
      this.SetRibbonGroup(this.ribbonform.rgReports, i_Tree_Eco_v6.Resources.Strings.FormattedReports);
      this.SetRibbonGroup(this.ribbonform.rgCharts, i_Tree_Eco_v6.Resources.Strings.Charts);
      this.SetRibbonGroup(this.ribbonform.rgForecastReports, i_Tree_Eco_v6.Resources.Strings.ForecastReports);
    }

    private void SetRibbonGroup(RibbonGroup rg, string groupTitle)
    {
      TreeNode groupNode = this.treeView1.Nodes.Add(groupTitle);
      foreach (object obj in (CollectionBase) rg.Items)
      {
        if (obj.GetType() == typeof (RibbonMenu))
        {
          RibbonMenu rm = (RibbonMenu) obj;
          bool flag = false;
          ReportCondition condition = this._reportOpt.GetCondition(rm.Name);
          string text;
          if (condition != null && (!condition.IsAvailable || !condition.Enabled))
          {
            flag = true;
            ++this._disabledCount;
            text = this.GetUnavailableString(condition, rm.Text);
          }
          else
            text = rm.Text;
          TreeNode addedNode = groupNode.Nodes.Add(rm.Name, text);
          if (flag)
            this.StyleDisabledNode(addedNode);
          this.SetMenuItems(rm, groupNode);
        }
        else if (obj.GetType() == typeof (RibbonButton))
        {
          RibbonButton ribbonButton = (RibbonButton) obj;
          if (ribbonButton.Enabled)
          {
            bool flag = false;
            ReportCondition condition = this._reportOpt.GetCondition(ribbonButton.Name);
            string text;
            if (condition != null && (!condition.IsAvailable || !condition.Enabled))
            {
              flag = true;
              ++this._disabledCount;
              text = this.GetUnavailableString(condition, ribbonButton.Text);
            }
            else
              text = ribbonButton.Text;
            TreeNode addedNode = groupNode.Nodes.Add(ribbonButton.Name, text);
            if (flag)
              this.StyleDisabledNode(addedNode);
          }
        }
      }
    }

    private void SetMenuItems(RibbonMenu rm, TreeNode groupNode)
    {
      string str = string.Empty;
      foreach (object obj in (CollectionBase) rm.Items)
      {
        if (obj.GetType() == typeof (RibbonLabel))
        {
          RibbonLabel ribbonLabel = (RibbonLabel) obj;
          if (ribbonLabel.Enabled)
          {
            str = ribbonLabel.Name;
            bool flag = false;
            ReportCondition condition = this._reportOpt.GetCondition(str);
            string text;
            if (condition != null && (!condition.IsAvailable || !condition.Enabled))
            {
              flag = true;
              ++this._disabledCount;
              text = this.GetUnavailableString(condition, ribbonLabel.Text);
            }
            else
              text = ribbonLabel.Text;
            TreeNode addedNode = groupNode.Nodes[rm.Name].Nodes.Add(str, text);
            if (flag)
              this.StyleDisabledNode(addedNode);
          }
          else
            continue;
        }
        if (obj.GetType() == typeof (RibbonButton))
        {
          RibbonButton ribbonButton = (RibbonButton) obj;
          if (ribbonButton.Enabled)
          {
            if (groupNode.Nodes[rm.Name].Nodes[str] != null)
            {
              bool flag = false;
              ReportCondition condition = this._reportOpt.GetCondition(ribbonButton.Name);
              string text;
              if (condition != null && (!condition.IsAvailable || !condition.Enabled))
              {
                flag = true;
                ++this._disabledCount;
                text = this.GetUnavailableString(condition, ribbonButton.Text);
              }
              else
                text = ribbonButton.Text;
              TreeNode addedNode = groupNode.Nodes[rm.Name].Nodes[str].Nodes.Add(ribbonButton.Name, text);
              if (flag)
                this.StyleDisabledNode(addedNode);
            }
            else
            {
              bool flag = false;
              ReportCondition condition = this._reportOpt.GetCondition(ribbonButton.Name);
              string text;
              if (condition != null && (!condition.IsAvailable || !condition.Enabled))
              {
                flag = true;
                ++this._disabledCount;
                text = this.GetUnavailableString(condition, ribbonButton.Text);
              }
              else
                text = ribbonButton.Text;
              TreeNode addedNode = groupNode.Nodes[rm.Name].Nodes.Add(ribbonButton.Name, text);
              if (flag)
                this.StyleDisabledNode(addedNode);
            }
          }
        }
      }
    }

    private void StyleDisabledNode(TreeNode addedNode) => addedNode.ForeColor = Color.Red;

    private string GetUnavailableString(ReportCondition repCond, string strRibbonItemName)
    {
      StringBuilder stringBuilder = new StringBuilder(strRibbonItemName);
      int num = repCond.Conditions.Count<ConditionBase>();
      if (repCond.IsAvailable)
      {
        stringBuilder.Append(string.Format(" ({0}: ", (object) i_Tree_Eco_v6.Resources.Strings.Unavailable));
        foreach (ConditionBase condition in repCond.Conditions)
        {
          if (!condition.Result)
          {
            --num;
            stringBuilder.AppendFormat(i_Tree_Eco_v6.Resources.Strings.FmtQuotation, (object) condition.Option);
            if (num >= 1)
              stringBuilder.Append(", ");
          }
        }
        stringBuilder.Append(string.Format(" {0})", (object) i_Tree_Eco_v6.Resources.Strings.NotChecked));
      }
      else
        stringBuilder.Append(string.Format(" ({0})", (object) i_Tree_Eco_v6.Resources.Strings.UnavailableForThisProjectTypeOrLocation));
      return stringBuilder.ToString();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ShowAvailableReports));
      this.treeView1 = new TreeView();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lblOKCancel = new Label();
      this.lblAvailability2 = new Label();
      this.lblAvail1 = new Label();
      this.lblAvail2 = new Label();
      this.lblAvail3 = new Label();
      this.lblInternational = new Label();
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.panel3 = new Panel();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.treeView1, "treeView1");
      this.treeView1.Name = "treeView1";
      this.btnOK.DialogResult = DialogResult.OK;
      componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
      this.btnOK.Name = "btnOK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.lblOKCancel, "lblOKCancel");
      this.lblOKCancel.Name = "lblOKCancel";
      componentResourceManager.ApplyResources((object) this.lblAvailability2, "lblAvailability2");
      this.lblAvailability2.Name = "lblAvailability2";
      componentResourceManager.ApplyResources((object) this.lblAvail1, "lblAvail1");
      this.lblAvail1.Name = "lblAvail1";
      componentResourceManager.ApplyResources((object) this.lblAvail2, "lblAvail2");
      this.lblAvail2.ForeColor = Color.Red;
      this.lblAvail2.Name = "lblAvail2";
      componentResourceManager.ApplyResources((object) this.lblAvail3, "lblAvail3");
      this.lblAvail3.Name = "lblAvail3";
      componentResourceManager.ApplyResources((object) this.lblInternational, "lblInternational");
      this.lblInternational.Name = "lblInternational";
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.Controls.Add((Control) this.lblInternational);
      this.panel1.Controls.Add((Control) this.lblAvail1);
      this.panel1.Controls.Add((Control) this.lblAvail3);
      this.panel1.Controls.Add((Control) this.lblAvail2);
      this.panel1.Name = "panel1";
      this.panel2.Controls.Add((Control) this.btnCancel);
      this.panel2.Controls.Add((Control) this.btnOK);
      this.panel2.Controls.Add((Control) this.lblOKCancel);
      componentResourceManager.ApplyResources((object) this.panel2, "panel2");
      this.panel2.Name = "panel2";
      this.panel3.Controls.Add((Control) this.treeView1);
      componentResourceManager.ApplyResources((object) this.panel3, "panel3");
      this.panel3.Name = "panel3";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.lblAvailability2);
      this.Name = nameof (ShowAvailableReports);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
