// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.SpeciesCodesForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Core;
using Eco.Util;
using Eco.Util.Views;
using LocationSpecies.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class SpeciesCodesForm : ContentForm
  {
    private ProgramSession m_ps;
    private TaskManager m_taskManager;
    private WaitCursor m_wc;
    private bool m_cmbBusy;
    private IContainer components;
    private Label label1;
    private Label label3;
    private Label lblParent;
    private Label label4;
    private Label label5;
    private ComboBox acmbCode;
    private ComboBox acmbScientificName;
    private ComboBox acmbCommonName;
    private Label lblGenus;
    private Panel panel1;

    public SpeciesCodesForm()
    {
      this.InitializeComponent();
      this.m_ps = Program.Session;
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.Init();
    }

    private void Init()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew<List<SpeciesView>>((Func<List<SpeciesView>>) (() => this.m_ps.Species.Values.ToList<SpeciesView>()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<List<SpeciesView>>>) (t =>
      {
        List<SpeciesView> result = t.Result;
        DataBindingList<SpeciesView> dataBindingList1 = new DataBindingList<SpeciesView>((IList<SpeciesView>) result.OrderBy<SpeciesView, string>((Func<SpeciesView, string>) (s => s.Code)).ToList<SpeciesView>());
        DataBindingList<SpeciesView> dataBindingList2 = new DataBindingList<SpeciesView>((IList<SpeciesView>) result.OrderBy<SpeciesView, string>((Func<SpeciesView, string>) (s => s.ScientificName)).ToList<SpeciesView>());
        DataBindingList<SpeciesView> dataBindingList3 = new DataBindingList<SpeciesView>((IList<SpeciesView>) result.OrderBy<SpeciesView, string>((Func<SpeciesView, string>) (s => s.CommonName)).ToList<SpeciesView>());
        this.acmbCode.DisplayMember = "Code";
        this.acmbCode.ValueMember = "Self";
        this.acmbCode.DataSource = (object) dataBindingList1;
        this.acmbScientificName.DisplayMember = "ScientificName";
        this.acmbScientificName.ValueMember = "Self";
        this.acmbScientificName.DataSource = (object) dataBindingList2;
        this.acmbCommonName.DisplayMember = "CommonName";
        this.acmbCommonName.ValueMember = "Self";
        this.acmbCommonName.DataSource = (object) dataBindingList3;
        this.acmbCode.TextChanged += new EventHandler(this.Combo_TextChanged);
        this.acmbCommonName.TextChanged += new EventHandler(this.Combo_TextChanged);
        this.acmbScientificName.TextChanged += new EventHandler(this.Combo_TextChanged);
        this.acmbCode.KeyPress += new KeyPressEventHandler(this.Combo_KeyPress);
        this.acmbCommonName.KeyPress += new KeyPressEventHandler(this.Combo_KeyPress);
        this.acmbScientificName.KeyPress += new KeyPressEventHandler(this.Combo_KeyPress);
        this.acmbCode.Focus();
        this.Combo_TextChanged((object) this.acmbCode, EventArgs.Empty);
      }), scheduler));
    }

    private void Combo_KeyPress(object sender, KeyPressEventArgs e)
    {
      ComboBox comboBox = (ComboBox) sender;
      if (e.KeyChar != '\b' || comboBox.SelectionStart <= 0)
        return;
      --comboBox.SelectionStart;
      ++comboBox.SelectionLength;
    }

    private void Combo_TextChanged(object sender, EventArgs e)
    {
      if (this.m_cmbBusy)
        return;
      this.m_cmbBusy = true;
      ComboBox comboBox = (ComboBox) sender;
      int index = comboBox.FindString(comboBox.Text);
      if (index != -1)
      {
        SpeciesView speciesView = (comboBox.DataSource as DataBindingList<SpeciesView>)[index];
        if (sender != this.acmbCode)
          this.acmbCode.SelectedValue = (object) speciesView;
        if (sender != this.acmbCommonName)
          this.acmbCommonName.SelectedValue = (object) speciesView;
        if (sender != this.acmbScientificName)
          this.acmbScientificName.SelectedValue = (object) speciesView;
        if (speciesView.Rank >= SpeciesRank.Genus)
        {
          this.lblParent.Text = EnumHelper.GetDescription<SpeciesRank>(speciesView.Rank);
          this.lblGenus.Text = string.Format("{0} ({1})", (object) speciesView.ScientificName, (object) speciesView.CommonName);
          this.lblGenus.Visible = true;
          this.lblParent.Visible = true;
        }
        else
        {
          this.lblGenus.Visible = false;
          this.lblParent.Visible = false;
        }
        int selectionStart = comboBox.SelectionStart;
        comboBox.SelectedValue = (object) speciesView;
        comboBox.SelectionStart = selectionStart;
        comboBox.SelectionLength = comboBox.Text.Length;
      }
      else
      {
        if (sender != this.acmbCode)
          this.acmbCode.SelectedIndex = -1;
        if (sender != this.acmbCommonName)
          this.acmbCommonName.SelectedIndex = -1;
        if (sender != this.acmbScientificName)
          this.acmbScientificName.SelectedIndex = -1;
        this.lblGenus.Visible = false;
        this.lblParent.Visible = false;
      }
      this.m_cmbBusy = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SpeciesCodesForm));
      this.label1 = new Label();
      this.label3 = new Label();
      this.lblParent = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.acmbCode = new ComboBox();
      this.acmbScientificName = new ComboBox();
      this.acmbCommonName = new ComboBox();
      this.lblGenus = new Label();
      this.panel1 = new Panel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.lblParent, "lblParent");
      this.lblParent.Name = "lblParent";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      this.acmbCode.AllowDrop = true;
      this.acmbCode.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.acmbCode.FormattingEnabled = true;
      componentResourceManager.ApplyResources((object) this.acmbCode, "acmbCode");
      this.acmbCode.Name = "acmbCode";
      this.acmbScientificName.AllowDrop = true;
      this.acmbScientificName.AutoCompleteSource = AutoCompleteSource.ListItems;
      componentResourceManager.ApplyResources((object) this.acmbScientificName, "acmbScientificName");
      this.acmbScientificName.FormattingEnabled = true;
      this.acmbScientificName.Name = "acmbScientificName";
      this.acmbCommonName.AllowDrop = true;
      this.acmbCommonName.AutoCompleteSource = AutoCompleteSource.ListItems;
      componentResourceManager.ApplyResources((object) this.acmbCommonName, "acmbCommonName");
      this.acmbCommonName.FormattingEnabled = true;
      this.acmbCommonName.Name = "acmbCommonName";
      componentResourceManager.ApplyResources((object) this.lblGenus, "lblGenus");
      this.lblGenus.Name = "lblGenus";
      this.panel1.Controls.Add((Control) this.lblGenus);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.acmbCommonName);
      this.panel1.Controls.Add((Control) this.acmbScientificName);
      this.panel1.Controls.Add((Control) this.acmbCode);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.lblParent);
      this.panel1.Controls.Add((Control) this.label3);
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.Name = "panel1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (SpeciesCodesForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.panel1, 0);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
