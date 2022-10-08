// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.DeleteStrataForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls.Extensions;
using Eco.Domain.v6;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Resources;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class DeleteStrataForm : Form
  {
    private ProgramSession _ps;
    private Strata _s;
    private bool _isSample;
    private object _syncobj;
    private ISession _session;
    private IList<Strata> _strata;
    private IContainer components;
    private TableLayoutPanel tableLayoutPanel1;
    private Label lblMessage;
    private Button btnOK;
    private Button btnCancel;
    private Panel panel1;
    private ComboBox cboStrata;
    private RadioButton rdoReassign;
    private RadioButton rdoDelete;
    private Label lblWarning;
    private CheckBox chkSize;

    public DeleteStrataForm(Strata s, bool isSample)
    {
      this._s = s;
      this._isSample = isSample;
      this._syncobj = new object();
      this._ps = Program.Session;
      this._session = this._ps.InputSession.CreateSession();
      this.InitializeComponent();
      this.Text = string.Format(this.Text, (object) s.Description);
      this.Init();
    }

    private void Init()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      if (!this._isSample)
      {
        this.rdoDelete.Text = Strings.LblDeleteTrees;
        this.rdoReassign.Text = Strings.LblReassignTrees;
      }
      Task.Factory.StartNew((Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this._ps.Scheduler).ContinueWith((Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.InitUI();
      }), scheduler);
    }

    private void LoadData()
    {
      using (this._session.BeginTransaction())
      {
        IList<Strata> strataList = this._session.CreateCriteria<Strata>().Add((ICriterion) Restrictions.Eq("Year.Guid", (object) this._ps.InputSession.YearKey)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.In("Guid", new object[1]
        {
          (object) this._s.Guid
        }))).AddOrder(Order.Asc("Id")).List<Strata>();
        lock (this._syncobj)
          this._strata = strataList;
      }
    }

    private void InitUI() => this.cboStrata.BindTo<Strata>((object) this._strata, (System.Linq.Expressions.Expression<Func<Strata, object>>) (st => st.Description), (System.Linq.Expressions.Expression<Func<Strata, object>>) (st => st.Self));

    private void rdoOption_CheckedChanged(object sender, EventArgs e)
    {
      this.lblWarning.Visible = this.rdoReassign.Checked;
      this.cboStrata.Enabled = this.rdoReassign.Checked;
      this.chkSize.Enabled = this.rdoReassign.Checked;
      this.btnOK.Enabled = this.rdoDelete.Enabled || this.rdoReassign.Enabled;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.rdoReassign.Checked)
      {
        this.btnOK.Enabled = false;
        this.ReassignPlots();
      }
      this.DialogResult = DialogResult.OK;
    }

    private void ReassignPlots()
    {
      if (!(this.cboStrata.SelectedValue is Strata selectedValue))
        return;
      using (ITransaction transaction = this._session.BeginTransaction())
      {
        if (this.chkSize.Checked)
        {
          selectedValue.Size += this._s.Size;
          this._session.SaveOrUpdate((object) selectedValue);
        }
        foreach (Plot plot in (IEnumerable<Plot>) this._session.CreateCriteria<Plot>().Add((ICriterion) Restrictions.Eq("Strata", (object) this._s)).List<Plot>())
        {
          plot.Strata = selectedValue;
          this._session.SaveOrUpdate((object) plot);
        }
        transaction.Commit();
      }
      if (!this.chkSize.Checked)
        return;
      EventPublisher.Publish<EntityUpdated<Strata>>(new EntityUpdated<Strata>(selectedValue), (Control) this);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DeleteStrataForm));
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.lblWarning = new Label();
      this.lblMessage = new Label();
      this.panel1 = new Panel();
      this.chkSize = new CheckBox();
      this.cboStrata = new ComboBox();
      this.rdoReassign = new RadioButton();
      this.rdoDelete = new RadioButton();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.lblWarning, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblMessage, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.panel1, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.btnCancel, 2, 3);
      this.tableLayoutPanel1.Controls.Add((Control) this.btnOK, 1, 3);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.lblWarning, "lblWarning");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblWarning, 3);
      this.lblWarning.ForeColor = Color.Red;
      this.lblWarning.Name = "lblWarning";
      componentResourceManager.ApplyResources((object) this.lblMessage, "lblMessage");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblMessage, 3);
      this.lblMessage.Name = "lblMessage";
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.panel1, 3);
      this.panel1.Controls.Add((Control) this.chkSize);
      this.panel1.Controls.Add((Control) this.cboStrata);
      this.panel1.Controls.Add((Control) this.rdoReassign);
      this.panel1.Controls.Add((Control) this.rdoDelete);
      this.panel1.Name = "panel1";
      componentResourceManager.ApplyResources((object) this.chkSize, "chkSize");
      this.chkSize.Name = "chkSize";
      this.chkSize.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.cboStrata, "cboStrata");
      this.cboStrata.FormattingEnabled = true;
      this.cboStrata.Name = "cboStrata";
      componentResourceManager.ApplyResources((object) this.rdoReassign, "rdoReassign");
      this.rdoReassign.Name = "rdoReassign";
      this.rdoReassign.TabStop = true;
      this.rdoReassign.UseVisualStyleBackColor = true;
      this.rdoReassign.CheckedChanged += new EventHandler(this.rdoOption_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.rdoDelete, "rdoDelete");
      this.rdoDelete.Name = "rdoDelete";
      this.rdoDelete.TabStop = true;
      this.rdoDelete.UseVisualStyleBackColor = true;
      this.rdoDelete.CheckedChanged += new EventHandler(this.rdoOption_CheckedChanged);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
      this.btnOK.Name = "btnOK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DeleteStrataForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
