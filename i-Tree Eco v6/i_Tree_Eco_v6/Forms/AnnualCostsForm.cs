// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.AnnualCostsForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using CsvHelper;
using DaveyTree.Controls;
using Eco.Domain.v6;
using Eco.Util.Services;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Interfaces;
using i_Tree_Eco_v6.Resources;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class AnnualCostsForm : DataContentForm, IExportable
  {
    private NumericTextBox[] m_private;
    private NumericTextBox[] m_public;
    private NumericTextBox[] m_total;
    private YearlyCost m_privateCosts;
    private YearlyCost m_publicCosts;
    private LocationSpecies.Domain.Currency m_currency;
    private IContainer components;
    private TableLayoutPanel pnlCosts;
    private Label lblPublic;
    private Label lblPrivate;
    private Label lblPlanting;
    private Label lblPruning;
    private Label lblTreeRemoval;
    private Label lblPestControl;
    private Label lblIrrigation;
    private Label lblRepair;
    private Label lblCleanup;
    private Label lblLegal;
    private Label lblAdministrative;
    private Label lblInspection;
    private Label lblOther;
    private NumericTextBox ntbPublicPlanting;
    private NumericTextBox ntbPrivatePlanting;
    private NumericTextBox ntbPublicPruning;
    private NumericTextBox ntbPrivatePruning;
    private NumericTextBox ntbPublicTreeRemoval;
    private NumericTextBox ntbPublicPestControl;
    private NumericTextBox ntbPrivateTreeRemoval;
    private NumericTextBox ntbPrivatePestControl;
    private NumericTextBox ntbPublicIrrigation;
    private NumericTextBox ntbPrivateIrrigation;
    private NumericTextBox ntbPublicRepair;
    private NumericTextBox ntbPrivateRepair;
    private NumericTextBox ntbPublicCleanup;
    private NumericTextBox ntbPrivateCleanup;
    private NumericTextBox ntbPublicLegal;
    private NumericTextBox ntbPublicAdministrative;
    private NumericTextBox ntbPrivateAdministrative;
    private NumericTextBox ntbPrivateLegal;
    private NumericTextBox ntbPublicInspection;
    private NumericTextBox ntbPrivateInspection;
    private NumericTextBox ntbPublicOther;
    private NumericTextBox ntbPrivateOther;
    private Label lblTotalCosts;
    private NumericTextBox ntbPublicTotal;
    private NumericTextBox ntbPrivateTotal;
    private Label lblTotal;
    private NumericTextBox ntbTotalPlanting;
    private NumericTextBox ntbTotalPruning;
    private NumericTextBox ntbTotalTreeRemoval;
    private NumericTextBox ntbTotalPestControl;
    private NumericTextBox ntbTotalIrrigation;
    private NumericTextBox ntbTotalRepair;
    private NumericTextBox ntbTotalCleanup;
    private NumericTextBox ntbTotalLegal;
    private NumericTextBox ntbTotalAdministrative;
    private NumericTextBox ntbTotalInspection;
    private NumericTextBox ntbTotalOther;
    private NumericTextBox ntbTotalTotal;
    private TableLayoutPanel tableLayoutPanel4;
    private FlowLayoutPanel flowLayoutPanel2;
    private Button cmdCancel;
    private Button cmdOK;
    private Label label16;
    private Label label1;
    private Label lblNotes;

    public AnnualCostsForm()
    {
      this.InitializeComponent();
      this.m_private = new NumericTextBox[11]
      {
        this.ntbPrivateAdministrative,
        this.ntbPrivateCleanup,
        this.ntbPrivateInspection,
        this.ntbPrivateIrrigation,
        this.ntbPrivateLegal,
        this.ntbPrivateOther,
        this.ntbPrivatePestControl,
        this.ntbPrivatePlanting,
        this.ntbPrivatePruning,
        this.ntbPrivateRepair,
        this.ntbPrivateTreeRemoval
      };
      this.m_public = new NumericTextBox[11]
      {
        this.ntbPublicAdministrative,
        this.ntbPublicCleanup,
        this.ntbPublicInspection,
        this.ntbPublicIrrigation,
        this.ntbPublicLegal,
        this.ntbPublicOther,
        this.ntbPublicPestControl,
        this.ntbPublicPlanting,
        this.ntbPublicPruning,
        this.ntbPublicRepair,
        this.ntbPublicTreeRemoval
      };
      this.m_total = new NumericTextBox[11]
      {
        this.ntbTotalAdministrative,
        this.ntbTotalCleanup,
        this.ntbTotalInspection,
        this.ntbTotalIrrigation,
        this.ntbTotalLegal,
        this.ntbTotalOther,
        this.ntbTotalPestControl,
        this.ntbTotalPlanting,
        this.ntbTotalPruning,
        this.ntbTotalRepair,
        this.ntbTotalTreeRemoval
      };
      foreach (Control control in this.m_private)
        control.TextChanged += new EventHandler(this.ntbPrivate_TextChanged);
      foreach (Control control in this.m_public)
        control.TextChanged += new EventHandler(this.ntbPublic_TextChanged);
    }

    protected override void InitializeYear(Year year)
    {
      base.InitializeYear(year);
      NHibernateUtil.Initialize((object) year.YearlyCosts);
    }

    protected override void LoadData()
    {
      base.LoadData();
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          try
          {
            Year year = this.Year;
            YearlyCost yearlyCost1 = new YearlyCost()
            {
              Year = year,
              Public = true
            };
            YearlyCost yearlyCost2 = new YearlyCost()
            {
              Year = year,
              Public = false
            };
            foreach (YearlyCost yearlyCost3 in (IEnumerable<YearlyCost>) year.YearlyCosts)
            {
              if (yearlyCost3.Public)
                yearlyCost1 = yearlyCost3;
              else
                yearlyCost2 = yearlyCost3;
            }
            transaction.Commit();
            this.m_publicCosts = yearlyCost1;
            this.m_privateCosts = yearlyCost2;
            this.m_isDirty = false;
            this.InitLocationData(this.Project.LocationId);
          }
          catch (HibernateException ex)
          {
            transaction.Rollback();
          }
        }
      }
    }

    private void InitLocationData(int locationId) => this.m_currency = new LocationService(Program.Session.LocSp).GetLocationCurrency(locationId);

    protected override void OnDataLoaded()
    {
      this.InitForm();
      this.registerChangeEvents((Control) this);
      base.OnDataLoaded();
    }

    private void InitForm()
    {
      this.pnlCosts.Enabled = true;
      this.cmdCancel.Visible = true;
      string str = string.Empty;
      if (this.m_currency != null)
        str = string.Format(Strings.FmtCurrency, (object) this.m_currency.Symbol, (object) this.m_currency.Abbreviation);
      this.lblAdministrative.Text = string.Format(Strings.FmtLabel, (object) string.Format(Strings.FmtCost, (object) Strings.Administrative, (object) str));
      this.lblCleanup.Text = string.Format(Strings.FmtLabel, (object) string.Format(Strings.FmtCost, (object) Strings.Cleanup, (object) str));
      this.lblInspection.Text = string.Format(Strings.FmtLabel, (object) string.Format(Strings.FmtCost, (object) Strings.Inspection, (object) str));
      this.lblIrrigation.Text = string.Format(Strings.FmtLabel, (object) string.Format(Strings.FmtCost, (object) Strings.Irrigation, (object) str));
      this.lblLegal.Text = string.Format(Strings.FmtLabel, (object) string.Format(Strings.FmtCost, (object) Strings.Legal, (object) str));
      this.lblOther.Text = string.Format(Strings.FmtLabel, (object) string.Format(Strings.FmtCost, (object) Strings.Other, (object) str));
      this.lblPestControl.Text = string.Format(Strings.FmtLabel, (object) string.Format(Strings.FmtCost, (object) Strings.PestControl, (object) str));
      this.lblPlanting.Text = string.Format(Strings.FmtLabel, (object) string.Format(Strings.FmtCost, (object) Strings.Planting, (object) str));
      this.lblPruning.Text = string.Format(Strings.FmtLabel, (object) string.Format(Strings.FmtCost, (object) Strings.Pruning, (object) str));
      this.lblRepair.Text = string.Format(Strings.FmtLabel, (object) string.Format(Strings.FmtCost, (object) Strings.Repair, (object) str));
      this.lblTotalCosts.Text = string.Format(Strings.FmtLabel, (object) string.Format(Strings.FmtCost, (object) string.Format(Strings.FmtTotal, (object) Strings.Costs), (object) str));
      this.lblTreeRemoval.Text = string.Format(Strings.FmtLabel, (object) string.Format(Strings.FmtCost, (object) Strings.TreeRemoval, (object) str));
      this.lblPrivate.Text = string.Format(Strings.FmtCost, (object) Strings.Private, (object) str);
      this.lblPublic.Text = string.Format(Strings.FmtCost, (object) Strings.Public, (object) str);
      this.lblTotal.Text = string.Format(Strings.FmtCost, (object) Strings.Total, (object) str);
      Interlocked.Increment(ref this.m_changes);
      this.ntbPrivateAdministrative.Text = this.m_privateCosts.Administrative.ToString();
      this.ntbPrivateCleanup.Text = this.m_privateCosts.CleanUp.ToString();
      this.ntbPrivateInspection.Text = this.m_privateCosts.Inspection.ToString();
      this.ntbPrivateIrrigation.Text = this.m_privateCosts.Irrigation.ToString();
      this.ntbPrivateLegal.Text = this.m_privateCosts.Legal.ToString();
      this.ntbPrivateOther.Text = this.m_privateCosts.Other.ToString();
      this.ntbPrivatePestControl.Text = this.m_privateCosts.PestControl.ToString();
      this.ntbPrivatePlanting.Text = this.m_privateCosts.Planting.ToString();
      this.ntbPrivatePruning.Text = this.m_privateCosts.Pruning.ToString();
      this.ntbPrivateRepair.Text = this.m_privateCosts.Repair.ToString();
      this.ntbPrivateTreeRemoval.Text = this.m_privateCosts.TreeRemoval.ToString();
      this.ntbPublicAdministrative.Text = this.m_publicCosts.Administrative.ToString();
      this.ntbPublicCleanup.Text = this.m_publicCosts.CleanUp.ToString();
      this.ntbPublicInspection.Text = this.m_publicCosts.Inspection.ToString();
      this.ntbPublicIrrigation.Text = this.m_publicCosts.Irrigation.ToString();
      this.ntbPublicLegal.Text = this.m_publicCosts.Legal.ToString();
      this.ntbPublicOther.Text = this.m_publicCosts.Other.ToString();
      this.ntbPublicPestControl.Text = this.m_publicCosts.PestControl.ToString();
      this.ntbPublicPlanting.Text = this.m_publicCosts.Planting.ToString();
      this.ntbPublicPruning.Text = this.m_publicCosts.Pruning.ToString();
      this.ntbPublicRepair.Text = this.m_publicCosts.Repair.ToString();
      this.ntbPublicTreeRemoval.Text = this.m_publicCosts.TreeRemoval.ToString();
      Interlocked.Decrement(ref this.m_changes);
    }

    protected override bool ShowReportWarning => false;

    protected override bool ShowHelpMsg => false;

    private void ntbPublic_TextChanged(object sender, EventArgs e)
    {
      int index = Array.IndexOf<object>((object[]) this.m_public, sender);
      if (index <= -1)
        return;
      this.UpdateTotal(index);
    }

    private void ntbPrivate_TextChanged(object sender, EventArgs e)
    {
      int index = Array.IndexOf<object>((object[]) this.m_private, sender);
      if (index <= -1)
        return;
      this.UpdateTotal(index);
    }

    private void UpdateTotal(int index)
    {
      NumericTextBox numericTextBox1 = this.m_private[index];
      NumericTextBox numericTextBox2 = this.m_public[index];
      NumericTextBox numericTextBox3 = this.m_total[index];
      Decimal result1 = 0M;
      Decimal result2 = 0M;
      Decimal.TryParse(numericTextBox1.Text, out result1);
      Decimal.TryParse(numericTextBox2.Text, out result2);
      string str = (result2 + result1).ToString();
      numericTextBox3.Text = str;
      this.CalculateTotals();
    }

    private void CalculateTotals()
    {
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      foreach (NumericTextBox numericTextBox in this.m_private)
      {
        Decimal result = 0M;
        Decimal.TryParse(numericTextBox.Text, out result);
        num1 += result;
      }
      foreach (NumericTextBox numericTextBox in this.m_public)
      {
        Decimal result = 0M;
        Decimal.TryParse(numericTextBox.Text, out result);
        num2 += result;
      }
      this.ntbPublicTotal.Text = num2.ToString();
      this.ntbPrivateTotal.Text = num1.ToString();
      this.ntbTotalTotal.Text = (num2 + num1).ToString();
    }

    private void cmdOK_Click(object sender, EventArgs e)
    {
      if (this.m_isDirty)
      {
        this.UpdateCosts();
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          this.Session.SaveOrUpdate((object) this.m_privateCosts);
          this.Session.SaveOrUpdate((object) this.m_publicCosts);
          transaction.Commit();
        }
        this.m_isDirty = false;
      }
      this.Close();
    }

    private void UpdateCosts()
    {
      this.m_privateCosts.Administrative = this.CostOf(this.ntbPrivateAdministrative);
      this.m_privateCosts.CleanUp = this.CostOf(this.ntbPrivateCleanup);
      this.m_privateCosts.Inspection = this.CostOf(this.ntbPrivateInspection);
      this.m_privateCosts.Irrigation = this.CostOf(this.ntbPrivateIrrigation);
      this.m_privateCosts.Legal = this.CostOf(this.ntbPrivateLegal);
      this.m_privateCosts.Other = this.CostOf(this.ntbPrivateOther);
      this.m_privateCosts.PestControl = this.CostOf(this.ntbPrivatePestControl);
      this.m_privateCosts.Planting = this.CostOf(this.ntbPrivatePlanting);
      this.m_privateCosts.Pruning = this.CostOf(this.ntbPrivatePruning);
      this.m_privateCosts.Repair = this.CostOf(this.ntbPrivateRepair);
      this.m_privateCosts.TreeRemoval = this.CostOf(this.ntbPrivateTreeRemoval);
      this.m_publicCosts.Administrative = this.CostOf(this.ntbPublicAdministrative);
      this.m_publicCosts.CleanUp = this.CostOf(this.ntbPublicCleanup);
      this.m_publicCosts.Inspection = this.CostOf(this.ntbPublicInspection);
      this.m_publicCosts.Irrigation = this.CostOf(this.ntbPublicIrrigation);
      this.m_publicCosts.Legal = this.CostOf(this.ntbPublicLegal);
      this.m_publicCosts.Other = this.CostOf(this.ntbPublicOther);
      this.m_publicCosts.PestControl = this.CostOf(this.ntbPublicPestControl);
      this.m_publicCosts.Planting = this.CostOf(this.ntbPublicPlanting);
      this.m_publicCosts.Pruning = this.CostOf(this.ntbPublicPruning);
      this.m_publicCosts.Repair = this.CostOf(this.ntbPublicRepair);
      this.m_publicCosts.TreeRemoval = this.CostOf(this.ntbPublicTreeRemoval);
    }

    private Decimal CostOf(NumericTextBox ntb)
    {
      Decimal result = 0M;
      if (ntb != null)
        Decimal.TryParse(ntb.Text, out result);
      return result;
    }

    private void cmdCancel_Click(object sender, EventArgs e) => this.Close();

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.ExportCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV;

    private void ExportCSV(string file)
    {
      this.UpdateCosts();
      using (FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite))
      {
        using (TextWriter writer = (TextWriter) new StreamWriter((Stream) fileStream))
        {
          string str = string.Empty;
          if (this.m_currency != null)
            str = string.Format(Strings.FmtCurrency, (object) this.m_currency.Symbol, (object) this.m_currency.Abbreviation);
          using (CsvWriter csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture))
          {
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.AnnualCost, (object) str));
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.Public, (object) str));
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.Private, (object) str));
            csvWriter.NextRecord();
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.Administrative, (object) str));
            csvWriter.WriteField(this.m_publicCosts.Administrative.ToString("F2"));
            csvWriter.WriteField(this.m_privateCosts.Administrative.ToString("F2"));
            csvWriter.NextRecord();
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.Cleanup, (object) str));
            csvWriter.WriteField(this.m_publicCosts.CleanUp.ToString("F2"));
            csvWriter.WriteField(this.m_privateCosts.CleanUp.ToString("F2"));
            csvWriter.NextRecord();
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.Inspection, (object) str));
            csvWriter.WriteField(this.m_publicCosts.Inspection.ToString("F2"));
            csvWriter.WriteField(this.m_privateCosts.Inspection.ToString("F2"));
            csvWriter.NextRecord();
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.Irrigation, (object) str));
            csvWriter.WriteField(this.m_publicCosts.Irrigation.ToString("F2"));
            csvWriter.WriteField(this.m_privateCosts.Irrigation.ToString("F2"));
            csvWriter.NextRecord();
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.Legal, (object) str));
            csvWriter.WriteField(this.m_publicCosts.Legal.ToString("F2"));
            csvWriter.WriteField(this.m_privateCosts.Legal.ToString("F2"));
            csvWriter.NextRecord();
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.Other, (object) str));
            csvWriter.WriteField(this.m_publicCosts.Other.ToString("F2"));
            csvWriter.WriteField(this.m_privateCosts.Other.ToString("F2"));
            csvWriter.NextRecord();
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.PestControl, (object) str));
            csvWriter.WriteField(this.m_publicCosts.PestControl.ToString("F2"));
            csvWriter.WriteField(this.m_privateCosts.PestControl.ToString("F2"));
            csvWriter.NextRecord();
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.Planting, (object) str));
            csvWriter.WriteField(this.m_publicCosts.Planting.ToString("F2"));
            csvWriter.WriteField(this.m_privateCosts.Planting.ToString("F2"));
            csvWriter.NextRecord();
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.Pruning, (object) str));
            csvWriter.WriteField(this.m_publicCosts.Pruning.ToString("F2"));
            csvWriter.WriteField(this.m_privateCosts.Pruning.ToString("F2"));
            csvWriter.NextRecord();
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.Repair, (object) str));
            csvWriter.WriteField(this.m_publicCosts.Repair.ToString("F2"));
            csvWriter.WriteField(this.m_privateCosts.Repair.ToString("F2"));
            csvWriter.NextRecord();
            csvWriter.WriteField(string.Format(Strings.FmtCost, (object) Strings.TreeRemoval, (object) str));
            csvWriter.WriteField(this.m_publicCosts.TreeRemoval.ToString("F2"));
            csvWriter.WriteField(this.m_privateCosts.TreeRemoval.ToString("F2"));
            csvWriter.NextRecord();
          }
        }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AnnualCostsForm));
      this.pnlCosts = new TableLayoutPanel();
      this.lblPublic = new Label();
      this.lblPrivate = new Label();
      this.lblPlanting = new Label();
      this.lblPruning = new Label();
      this.lblTreeRemoval = new Label();
      this.lblPestControl = new Label();
      this.lblIrrigation = new Label();
      this.lblRepair = new Label();
      this.lblCleanup = new Label();
      this.lblLegal = new Label();
      this.lblAdministrative = new Label();
      this.lblInspection = new Label();
      this.lblOther = new Label();
      this.ntbPublicPlanting = new NumericTextBox();
      this.ntbPrivatePlanting = new NumericTextBox();
      this.ntbPublicPruning = new NumericTextBox();
      this.ntbPrivatePruning = new NumericTextBox();
      this.ntbPublicTreeRemoval = new NumericTextBox();
      this.ntbPublicPestControl = new NumericTextBox();
      this.ntbPrivateTreeRemoval = new NumericTextBox();
      this.ntbPrivatePestControl = new NumericTextBox();
      this.ntbPublicIrrigation = new NumericTextBox();
      this.ntbPrivateIrrigation = new NumericTextBox();
      this.ntbPublicRepair = new NumericTextBox();
      this.ntbPrivateRepair = new NumericTextBox();
      this.ntbPublicCleanup = new NumericTextBox();
      this.ntbPrivateCleanup = new NumericTextBox();
      this.ntbPublicLegal = new NumericTextBox();
      this.ntbPublicAdministrative = new NumericTextBox();
      this.ntbPrivateAdministrative = new NumericTextBox();
      this.ntbPrivateLegal = new NumericTextBox();
      this.ntbPublicInspection = new NumericTextBox();
      this.ntbPrivateInspection = new NumericTextBox();
      this.ntbPublicOther = new NumericTextBox();
      this.ntbPrivateOther = new NumericTextBox();
      this.ntbPublicTotal = new NumericTextBox();
      this.ntbPrivateTotal = new NumericTextBox();
      this.lblTotal = new Label();
      this.ntbTotalPlanting = new NumericTextBox();
      this.ntbTotalPruning = new NumericTextBox();
      this.ntbTotalTreeRemoval = new NumericTextBox();
      this.ntbTotalPestControl = new NumericTextBox();
      this.ntbTotalIrrigation = new NumericTextBox();
      this.ntbTotalRepair = new NumericTextBox();
      this.ntbTotalCleanup = new NumericTextBox();
      this.ntbTotalLegal = new NumericTextBox();
      this.ntbTotalAdministrative = new NumericTextBox();
      this.ntbTotalInspection = new NumericTextBox();
      this.ntbTotalOther = new NumericTextBox();
      this.ntbTotalTotal = new NumericTextBox();
      this.lblTotalCosts = new Label();
      this.tableLayoutPanel4 = new TableLayoutPanel();
      this.lblNotes = new Label();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.cmdCancel = new Button();
      this.cmdOK = new Button();
      this.label16 = new Label();
      this.label1 = new Label();
      this.pnlCosts.SuspendLayout();
      this.tableLayoutPanel4.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.pnlCosts, "pnlCosts");
      this.tableLayoutPanel4.SetColumnSpan((Control) this.pnlCosts, 2);
      this.pnlCosts.Controls.Add((Control) this.lblPublic, 1, 0);
      this.pnlCosts.Controls.Add((Control) this.lblPrivate, 2, 0);
      this.pnlCosts.Controls.Add((Control) this.lblPlanting, 0, 1);
      this.pnlCosts.Controls.Add((Control) this.lblPruning, 0, 2);
      this.pnlCosts.Controls.Add((Control) this.lblTreeRemoval, 0, 3);
      this.pnlCosts.Controls.Add((Control) this.lblPestControl, 0, 4);
      this.pnlCosts.Controls.Add((Control) this.lblIrrigation, 0, 5);
      this.pnlCosts.Controls.Add((Control) this.lblRepair, 0, 6);
      this.pnlCosts.Controls.Add((Control) this.lblCleanup, 0, 7);
      this.pnlCosts.Controls.Add((Control) this.lblLegal, 0, 8);
      this.pnlCosts.Controls.Add((Control) this.lblAdministrative, 0, 9);
      this.pnlCosts.Controls.Add((Control) this.lblInspection, 0, 10);
      this.pnlCosts.Controls.Add((Control) this.lblOther, 0, 11);
      this.pnlCosts.Controls.Add((Control) this.ntbPublicPlanting, 1, 1);
      this.pnlCosts.Controls.Add((Control) this.ntbPrivatePlanting, 2, 1);
      this.pnlCosts.Controls.Add((Control) this.ntbPublicPruning, 1, 2);
      this.pnlCosts.Controls.Add((Control) this.ntbPrivatePruning, 2, 2);
      this.pnlCosts.Controls.Add((Control) this.ntbPublicTreeRemoval, 1, 3);
      this.pnlCosts.Controls.Add((Control) this.ntbPublicPestControl, 1, 4);
      this.pnlCosts.Controls.Add((Control) this.ntbPrivateTreeRemoval, 2, 3);
      this.pnlCosts.Controls.Add((Control) this.ntbPrivatePestControl, 2, 4);
      this.pnlCosts.Controls.Add((Control) this.ntbPublicIrrigation, 1, 5);
      this.pnlCosts.Controls.Add((Control) this.ntbPrivateIrrigation, 2, 5);
      this.pnlCosts.Controls.Add((Control) this.ntbPublicRepair, 1, 6);
      this.pnlCosts.Controls.Add((Control) this.ntbPrivateRepair, 2, 6);
      this.pnlCosts.Controls.Add((Control) this.ntbPublicCleanup, 1, 7);
      this.pnlCosts.Controls.Add((Control) this.ntbPrivateCleanup, 2, 7);
      this.pnlCosts.Controls.Add((Control) this.ntbPublicLegal, 1, 8);
      this.pnlCosts.Controls.Add((Control) this.ntbPublicAdministrative, 1, 9);
      this.pnlCosts.Controls.Add((Control) this.ntbPrivateAdministrative, 2, 9);
      this.pnlCosts.Controls.Add((Control) this.ntbPrivateLegal, 2, 8);
      this.pnlCosts.Controls.Add((Control) this.ntbPublicInspection, 1, 10);
      this.pnlCosts.Controls.Add((Control) this.ntbPrivateInspection, 2, 10);
      this.pnlCosts.Controls.Add((Control) this.ntbPublicOther, 1, 11);
      this.pnlCosts.Controls.Add((Control) this.ntbPrivateOther, 2, 11);
      this.pnlCosts.Controls.Add((Control) this.ntbPublicTotal, 1, 12);
      this.pnlCosts.Controls.Add((Control) this.ntbPrivateTotal, 2, 12);
      this.pnlCosts.Controls.Add((Control) this.lblTotal, 3, 0);
      this.pnlCosts.Controls.Add((Control) this.ntbTotalPlanting, 3, 1);
      this.pnlCosts.Controls.Add((Control) this.ntbTotalPruning, 3, 2);
      this.pnlCosts.Controls.Add((Control) this.ntbTotalTreeRemoval, 3, 3);
      this.pnlCosts.Controls.Add((Control) this.ntbTotalPestControl, 3, 4);
      this.pnlCosts.Controls.Add((Control) this.ntbTotalIrrigation, 3, 5);
      this.pnlCosts.Controls.Add((Control) this.ntbTotalRepair, 3, 6);
      this.pnlCosts.Controls.Add((Control) this.ntbTotalCleanup, 3, 7);
      this.pnlCosts.Controls.Add((Control) this.ntbTotalLegal, 3, 8);
      this.pnlCosts.Controls.Add((Control) this.ntbTotalAdministrative, 3, 9);
      this.pnlCosts.Controls.Add((Control) this.ntbTotalInspection, 3, 10);
      this.pnlCosts.Controls.Add((Control) this.ntbTotalOther, 3, 11);
      this.pnlCosts.Controls.Add((Control) this.ntbTotalTotal, 3, 12);
      this.pnlCosts.Controls.Add((Control) this.lblTotalCosts, 0, 12);
      this.pnlCosts.Name = "pnlCosts";
      componentResourceManager.ApplyResources((object) this.lblPublic, "lblPublic");
      this.lblPublic.Name = "lblPublic";
      componentResourceManager.ApplyResources((object) this.lblPrivate, "lblPrivate");
      this.lblPrivate.Name = "lblPrivate";
      componentResourceManager.ApplyResources((object) this.lblPlanting, "lblPlanting");
      this.lblPlanting.Name = "lblPlanting";
      componentResourceManager.ApplyResources((object) this.lblPruning, "lblPruning");
      this.lblPruning.Name = "lblPruning";
      componentResourceManager.ApplyResources((object) this.lblTreeRemoval, "lblTreeRemoval");
      this.lblTreeRemoval.Name = "lblTreeRemoval";
      componentResourceManager.ApplyResources((object) this.lblPestControl, "lblPestControl");
      this.lblPestControl.Name = "lblPestControl";
      componentResourceManager.ApplyResources((object) this.lblIrrigation, "lblIrrigation");
      this.lblIrrigation.Name = "lblIrrigation";
      componentResourceManager.ApplyResources((object) this.lblRepair, "lblRepair");
      this.lblRepair.Name = "lblRepair";
      componentResourceManager.ApplyResources((object) this.lblCleanup, "lblCleanup");
      this.lblCleanup.Name = "lblCleanup";
      componentResourceManager.ApplyResources((object) this.lblLegal, "lblLegal");
      this.lblLegal.Name = "lblLegal";
      componentResourceManager.ApplyResources((object) this.lblAdministrative, "lblAdministrative");
      this.lblAdministrative.Name = "lblAdministrative";
      componentResourceManager.ApplyResources((object) this.lblInspection, "lblInspection");
      this.lblInspection.Name = "lblInspection";
      componentResourceManager.ApplyResources((object) this.lblOther, "lblOther");
      this.lblOther.Name = "lblOther";
      componentResourceManager.ApplyResources((object) this.ntbPublicPlanting, "ntbPublicPlanting");
      this.ntbPublicPlanting.DecimalPlaces = 2;
      this.ntbPublicPlanting.Format = "#,0;-#,0";
      this.ntbPublicPlanting.HasDecimal = false;
      this.ntbPublicPlanting.Name = "ntbPublicPlanting";
      this.ntbPublicPlanting.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPrivatePlanting, "ntbPrivatePlanting");
      this.ntbPrivatePlanting.DecimalPlaces = 2;
      this.ntbPrivatePlanting.Format = "#,0;-#,0";
      this.ntbPrivatePlanting.HasDecimal = false;
      this.ntbPrivatePlanting.Name = "ntbPrivatePlanting";
      this.ntbPrivatePlanting.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPublicPruning, "ntbPublicPruning");
      this.ntbPublicPruning.DecimalPlaces = 2;
      this.ntbPublicPruning.Format = "#,0;-#,0";
      this.ntbPublicPruning.HasDecimal = false;
      this.ntbPublicPruning.Name = "ntbPublicPruning";
      this.ntbPublicPruning.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPrivatePruning, "ntbPrivatePruning");
      this.ntbPrivatePruning.DecimalPlaces = 2;
      this.ntbPrivatePruning.Format = "#,0;-#,0";
      this.ntbPrivatePruning.HasDecimal = false;
      this.ntbPrivatePruning.Name = "ntbPrivatePruning";
      this.ntbPrivatePruning.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPublicTreeRemoval, "ntbPublicTreeRemoval");
      this.ntbPublicTreeRemoval.DecimalPlaces = 2;
      this.ntbPublicTreeRemoval.Format = "#,0;-#,0";
      this.ntbPublicTreeRemoval.HasDecimal = false;
      this.ntbPublicTreeRemoval.Name = "ntbPublicTreeRemoval";
      this.ntbPublicTreeRemoval.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPublicPestControl, "ntbPublicPestControl");
      this.ntbPublicPestControl.DecimalPlaces = 2;
      this.ntbPublicPestControl.Format = "#,0;-#,0";
      this.ntbPublicPestControl.HasDecimal = false;
      this.ntbPublicPestControl.Name = "ntbPublicPestControl";
      this.ntbPublicPestControl.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPrivateTreeRemoval, "ntbPrivateTreeRemoval");
      this.ntbPrivateTreeRemoval.DecimalPlaces = 2;
      this.ntbPrivateTreeRemoval.Format = "#,0;-#,0";
      this.ntbPrivateTreeRemoval.HasDecimal = false;
      this.ntbPrivateTreeRemoval.Name = "ntbPrivateTreeRemoval";
      this.ntbPrivateTreeRemoval.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPrivatePestControl, "ntbPrivatePestControl");
      this.ntbPrivatePestControl.DecimalPlaces = 2;
      this.ntbPrivatePestControl.Format = "#,0;-#,0";
      this.ntbPrivatePestControl.HasDecimal = false;
      this.ntbPrivatePestControl.Name = "ntbPrivatePestControl";
      this.ntbPrivatePestControl.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPublicIrrigation, "ntbPublicIrrigation");
      this.ntbPublicIrrigation.DecimalPlaces = 2;
      this.ntbPublicIrrigation.Format = "#,0;-#,0";
      this.ntbPublicIrrigation.HasDecimal = false;
      this.ntbPublicIrrigation.Name = "ntbPublicIrrigation";
      this.ntbPublicIrrigation.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPrivateIrrigation, "ntbPrivateIrrigation");
      this.ntbPrivateIrrigation.DecimalPlaces = 2;
      this.ntbPrivateIrrigation.Format = "#,0;-#,0";
      this.ntbPrivateIrrigation.HasDecimal = false;
      this.ntbPrivateIrrigation.Name = "ntbPrivateIrrigation";
      this.ntbPrivateIrrigation.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPublicRepair, "ntbPublicRepair");
      this.ntbPublicRepair.DecimalPlaces = 2;
      this.ntbPublicRepair.Format = "#,0;-#,0";
      this.ntbPublicRepair.HasDecimal = false;
      this.ntbPublicRepair.Name = "ntbPublicRepair";
      this.ntbPublicRepair.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPrivateRepair, "ntbPrivateRepair");
      this.ntbPrivateRepair.DecimalPlaces = 2;
      this.ntbPrivateRepair.Format = "#,0;-#,0";
      this.ntbPrivateRepair.HasDecimal = false;
      this.ntbPrivateRepair.Name = "ntbPrivateRepair";
      this.ntbPrivateRepair.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPublicCleanup, "ntbPublicCleanup");
      this.ntbPublicCleanup.DecimalPlaces = 2;
      this.ntbPublicCleanup.Format = "#,0;-#,0";
      this.ntbPublicCleanup.HasDecimal = false;
      this.ntbPublicCleanup.Name = "ntbPublicCleanup";
      this.ntbPublicCleanup.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPrivateCleanup, "ntbPrivateCleanup");
      this.ntbPrivateCleanup.DecimalPlaces = 2;
      this.ntbPrivateCleanup.Format = "#,0;-#,0";
      this.ntbPrivateCleanup.HasDecimal = false;
      this.ntbPrivateCleanup.Name = "ntbPrivateCleanup";
      this.ntbPrivateCleanup.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPublicLegal, "ntbPublicLegal");
      this.ntbPublicLegal.DecimalPlaces = 2;
      this.ntbPublicLegal.Format = "#,0;-#,0";
      this.ntbPublicLegal.HasDecimal = false;
      this.ntbPublicLegal.Name = "ntbPublicLegal";
      this.ntbPublicLegal.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPublicAdministrative, "ntbPublicAdministrative");
      this.ntbPublicAdministrative.DecimalPlaces = 2;
      this.ntbPublicAdministrative.Format = "#,0;-#,0";
      this.ntbPublicAdministrative.HasDecimal = false;
      this.ntbPublicAdministrative.Name = "ntbPublicAdministrative";
      this.ntbPublicAdministrative.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPrivateAdministrative, "ntbPrivateAdministrative");
      this.ntbPrivateAdministrative.DecimalPlaces = 2;
      this.ntbPrivateAdministrative.Format = "#,0;-#,0";
      this.ntbPrivateAdministrative.HasDecimal = false;
      this.ntbPrivateAdministrative.Name = "ntbPrivateAdministrative";
      this.ntbPrivateAdministrative.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPrivateLegal, "ntbPrivateLegal");
      this.ntbPrivateLegal.DecimalPlaces = 2;
      this.ntbPrivateLegal.Format = "#,0;-#,0";
      this.ntbPrivateLegal.HasDecimal = false;
      this.ntbPrivateLegal.Name = "ntbPrivateLegal";
      this.ntbPrivateLegal.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPublicInspection, "ntbPublicInspection");
      this.ntbPublicInspection.DecimalPlaces = 2;
      this.ntbPublicInspection.Format = "#,0;-#,0";
      this.ntbPublicInspection.HasDecimal = false;
      this.ntbPublicInspection.Name = "ntbPublicInspection";
      this.ntbPublicInspection.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPrivateInspection, "ntbPrivateInspection");
      this.ntbPrivateInspection.DecimalPlaces = 2;
      this.ntbPrivateInspection.Format = "#,0;-#,0";
      this.ntbPrivateInspection.HasDecimal = false;
      this.ntbPrivateInspection.Name = "ntbPrivateInspection";
      this.ntbPrivateInspection.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPublicOther, "ntbPublicOther");
      this.ntbPublicOther.DecimalPlaces = 2;
      this.ntbPublicOther.Format = "#,0;-#,0";
      this.ntbPublicOther.HasDecimal = false;
      this.ntbPublicOther.Name = "ntbPublicOther";
      this.ntbPublicOther.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPrivateOther, "ntbPrivateOther");
      this.ntbPrivateOther.DecimalPlaces = 2;
      this.ntbPrivateOther.Format = "#,0;-#,0";
      this.ntbPrivateOther.HasDecimal = false;
      this.ntbPrivateOther.Name = "ntbPrivateOther";
      this.ntbPrivateOther.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbPublicTotal, "ntbPublicTotal");
      this.ntbPublicTotal.DecimalPlaces = 2;
      this.ntbPublicTotal.Format = "#,0;-#,0";
      this.ntbPublicTotal.HasDecimal = false;
      this.ntbPublicTotal.Name = "ntbPublicTotal";
      this.ntbPublicTotal.Signed = true;
      componentResourceManager.ApplyResources((object) this.ntbPrivateTotal, "ntbPrivateTotal");
      this.ntbPrivateTotal.DecimalPlaces = 2;
      this.ntbPrivateTotal.Format = "#,0;-#,0";
      this.ntbPrivateTotal.HasDecimal = false;
      this.ntbPrivateTotal.Name = "ntbPrivateTotal";
      this.ntbPrivateTotal.Signed = true;
      componentResourceManager.ApplyResources((object) this.lblTotal, "lblTotal");
      this.lblTotal.Name = "lblTotal";
      componentResourceManager.ApplyResources((object) this.ntbTotalPlanting, "ntbTotalPlanting");
      this.ntbTotalPlanting.DecimalPlaces = 2;
      this.ntbTotalPlanting.Format = "#,0;-#,0";
      this.ntbTotalPlanting.HasDecimal = false;
      this.ntbTotalPlanting.Name = "ntbTotalPlanting";
      this.ntbTotalPlanting.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbTotalPruning, "ntbTotalPruning");
      this.ntbTotalPruning.DecimalPlaces = 2;
      this.ntbTotalPruning.Format = "#,0;-#,0";
      this.ntbTotalPruning.HasDecimal = false;
      this.ntbTotalPruning.Name = "ntbTotalPruning";
      this.ntbTotalPruning.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbTotalTreeRemoval, "ntbTotalTreeRemoval");
      this.ntbTotalTreeRemoval.DecimalPlaces = 2;
      this.ntbTotalTreeRemoval.Format = "#,0;-#,0";
      this.ntbTotalTreeRemoval.HasDecimal = false;
      this.ntbTotalTreeRemoval.Name = "ntbTotalTreeRemoval";
      this.ntbTotalTreeRemoval.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbTotalPestControl, "ntbTotalPestControl");
      this.ntbTotalPestControl.DecimalPlaces = 2;
      this.ntbTotalPestControl.Format = "#,0;-#,0";
      this.ntbTotalPestControl.HasDecimal = false;
      this.ntbTotalPestControl.Name = "ntbTotalPestControl";
      this.ntbTotalPestControl.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbTotalIrrigation, "ntbTotalIrrigation");
      this.ntbTotalIrrigation.DecimalPlaces = 2;
      this.ntbTotalIrrigation.Format = "#,0;-#,0";
      this.ntbTotalIrrigation.HasDecimal = false;
      this.ntbTotalIrrigation.Name = "ntbTotalIrrigation";
      this.ntbTotalIrrigation.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbTotalRepair, "ntbTotalRepair");
      this.ntbTotalRepair.DecimalPlaces = 2;
      this.ntbTotalRepair.Format = "#,0;-#,0";
      this.ntbTotalRepair.HasDecimal = false;
      this.ntbTotalRepair.Name = "ntbTotalRepair";
      this.ntbTotalRepair.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbTotalCleanup, "ntbTotalCleanup");
      this.ntbTotalCleanup.DecimalPlaces = 2;
      this.ntbTotalCleanup.Format = "#,0;-#,0";
      this.ntbTotalCleanup.HasDecimal = false;
      this.ntbTotalCleanup.Name = "ntbTotalCleanup";
      this.ntbTotalCleanup.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbTotalLegal, "ntbTotalLegal");
      this.ntbTotalLegal.DecimalPlaces = 2;
      this.ntbTotalLegal.Format = "#,0;-#,0";
      this.ntbTotalLegal.HasDecimal = false;
      this.ntbTotalLegal.Name = "ntbTotalLegal";
      this.ntbTotalLegal.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbTotalAdministrative, "ntbTotalAdministrative");
      this.ntbTotalAdministrative.DecimalPlaces = 2;
      this.ntbTotalAdministrative.Format = "#,0;-#,0";
      this.ntbTotalAdministrative.HasDecimal = false;
      this.ntbTotalAdministrative.Name = "ntbTotalAdministrative";
      this.ntbTotalAdministrative.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbTotalInspection, "ntbTotalInspection");
      this.ntbTotalInspection.DecimalPlaces = 2;
      this.ntbTotalInspection.Format = "#,0;-#,0";
      this.ntbTotalInspection.HasDecimal = false;
      this.ntbTotalInspection.Name = "ntbTotalInspection";
      this.ntbTotalInspection.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbTotalOther, "ntbTotalOther");
      this.ntbTotalOther.DecimalPlaces = 2;
      this.ntbTotalOther.Format = "#,0;-#,0";
      this.ntbTotalOther.HasDecimal = false;
      this.ntbTotalOther.Name = "ntbTotalOther";
      this.ntbTotalOther.Signed = false;
      componentResourceManager.ApplyResources((object) this.ntbTotalTotal, "ntbTotalTotal");
      this.ntbTotalTotal.DecimalPlaces = 2;
      this.ntbTotalTotal.Format = "#,0;-#,0";
      this.ntbTotalTotal.HasDecimal = false;
      this.ntbTotalTotal.Name = "ntbTotalTotal";
      this.ntbTotalTotal.Signed = false;
      componentResourceManager.ApplyResources((object) this.lblTotalCosts, "lblTotalCosts");
      this.lblTotalCosts.Name = "lblTotalCosts";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel4, "tableLayoutPanel4");
      this.tableLayoutPanel4.Controls.Add((Control) this.lblNotes, 0, 1);
      this.tableLayoutPanel4.Controls.Add((Control) this.flowLayoutPanel2, 1, 0);
      this.tableLayoutPanel4.Controls.Add((Control) this.pnlCosts, 0, 3);
      this.tableLayoutPanel4.Controls.Add((Control) this.label16, 0, 0);
      this.tableLayoutPanel4.Controls.Add((Control) this.label1, 0, 2);
      this.tableLayoutPanel4.Name = "tableLayoutPanel4";
      componentResourceManager.ApplyResources((object) this.lblNotes, "lblNotes");
      this.tableLayoutPanel4.SetColumnSpan((Control) this.lblNotes, 2);
      this.lblNotes.Name = "lblNotes";
      componentResourceManager.ApplyResources((object) this.flowLayoutPanel2, "flowLayoutPanel2");
      this.flowLayoutPanel2.Controls.Add((Control) this.cmdCancel);
      this.flowLayoutPanel2.Controls.Add((Control) this.cmdOK);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      componentResourceManager.ApplyResources((object) this.cmdCancel, "cmdCancel");
      this.cmdCancel.Name = "cmdCancel";
      this.cmdCancel.UseVisualStyleBackColor = true;
      this.cmdCancel.Click += new EventHandler(this.cmdCancel_Click);
      componentResourceManager.ApplyResources((object) this.cmdOK, "cmdOK");
      this.cmdOK.Name = "cmdOK";
      this.cmdOK.UseVisualStyleBackColor = true;
      this.cmdOK.Click += new EventHandler(this.cmdOK_Click);
      componentResourceManager.ApplyResources((object) this.label16, "label16");
      this.label16.Name = "label16";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.tableLayoutPanel4.SetColumnSpan((Control) this.label1, 2);
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tableLayoutPanel4);
      this.Name = nameof (AnnualCostsForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel4, 0);
      this.pnlCosts.ResumeLayout(false);
      this.pnlCosts.PerformLayout();
      this.tableLayoutPanel4.ResumeLayout(false);
      this.tableLayoutPanel4.PerformLayout();
      this.flowLayoutPanel2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
