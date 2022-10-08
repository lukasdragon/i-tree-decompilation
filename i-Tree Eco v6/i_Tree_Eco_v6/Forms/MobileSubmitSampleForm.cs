// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.MobileSubmitSampleForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Events;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class MobileSubmitSampleForm : MobileForm
  {
    private ProgramSession m_ps;
    private TaskManager m_taskManager;
    private ISession m_session;
    private DataBindingList<Plot> m_dsPlots;
    private DataBindingList<Plot> m_dsCompletedPlots;
    private DataBindingList<Plot> m_dsIncompletePlots;
    private DataBindingList<MobileLogEntry> m_dsLogEntries;
    private Year m_year;
    private ISet<Plot> m_selPlots;
    private bool m_filtering;
    private IContainer components;
    internal CheckBox chkAllPlots;
    private TableLayoutPanel tableLayoutPanel1;
    private Button btnSubmit;
    internal Label lblEmail2;
    internal TextBox txtEmail2;
    private Label Label3;
    private Label lblPassword2;
    private Label lblPassword1;
    private TextBox txtPassword2;
    private TextBox txtPassword1;
    internal TextBox txtEmail1;
    private Label lblEmail1;
    private Label lblSelPlots;
    private TableLayoutPanel panel1;
    private Label lblDisplay;
    private ComboBox cboDisplay;
    private Button btnResetPassword;
    private Label label1;
    private DataGridView dgPlots;
    private DataGridViewCheckBoxColumn dcSelect;
    private DataGridViewTextBoxColumn dcId;
    private DataGridViewComboBoxColumn dcStrata;
    private DataGridViewTextBoxColumn dcAddress;
    private DataGridViewTextBoxColumn dcLatitude;
    private DataGridViewTextBoxColumn dcLongitude;
    private DataGridViewNullableDateTimeColumn dcDate;
    private DataGridViewTextBoxColumn dcCrew;
    private DataGridViewTextBoxColumn dcContactInfo;
    private DataGridViewNumericTextBoxColumn dcSize;
    private DataGridViewNumericTextBoxColumn dcOffsetPoint;
    private DataGridViewTextBoxColumn dcPhoto;
    private DataGridViewCheckBoxColumn dcStake;
    private DataGridViewComboBoxColumn dcPctTree;
    private DataGridViewComboBoxColumn dcPctShrub;
    private DataGridViewComboBoxColumn dcPctPlantable;
    private DataGridViewNumericTextBoxColumn dcPctMeasured;
    private DataGridViewTextBoxColumn dcComments;
    private DataGridViewCheckBoxColumn dcIsComplete;
    private TableLayoutPanel tableLayoutPanel2;
    private Label lblResetPassword;
    private Label label2;
    private DataGridView dgSubmitLog;
    private DataGridViewTextBoxColumn dcMobileKey;
    private DataGridViewNullableDateTimeColumn dcDateTime;
    private DataGridViewTextBoxColumn dcDescription;

    public MobileSubmitSampleForm()
    {
      this.m_taskManager = new TaskManager(new WaitCursor((Form) this));
      this.m_ps = Program.Session;
      this.m_selPlots = (ISet<Plot>) new HashSet<Plot>();
      this.m_filtering = false;
      this.InitializeComponent();
      this.dgPlots.DoubleBuffered(true);
      this.dgPlots.AutoGenerateColumns = false;
      this.dgPlots.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgPlots.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgSubmitLog.DoubleBuffered(true);
      this.dgSubmitLog.AutoGenerateColumns = false;
      this.dgSubmitLog.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgSubmitLog.RowHeadersDefaultCellStyle = Program.InActiveGridDefaultCellStyle;
      this.Init();
    }

    private void Init()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed || t.IsFaulted)
          return;
        this.InitUI();
      }), scheduler));
    }

    private void LoadData()
    {
      if (this.m_ps.InputSession == null || !this.m_ps.InputSession.YearKey.HasValue)
        return;
      this.m_session = this.m_ps.InputSession.CreateSession();
      using (this.m_session.BeginTransaction())
      {
        this.m_year = this.m_session.Get<Year>((object) this.m_ps.InputSession.YearKey);
        NHibernateUtil.Initialize((object) this.m_year.Strata);
        NHibernateUtil.Initialize((object) this.m_year.Plots);
        List<Plot> list1 = new List<Plot>();
        List<Plot> list2 = new List<Plot>();
        foreach (Plot plot in (IEnumerable<Plot>) this.m_year.Plots)
        {
          if (plot.IsComplete)
            list1.Add(plot);
          else
            list2.Add(plot);
        }
        this.m_dsPlots = new DataBindingList<Plot>((IList<Plot>) this.m_year.Plots.ToList<Plot>());
        this.m_dsPlots.Sortable = true;
        this.m_dsPlots.AddComparer<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Strata), (IComparer) new PropertyComparer<Strata>((Func<Strata, object>) (s => (object) s.Description)));
        this.m_dsCompletedPlots = new DataBindingList<Plot>((IList<Plot>) list1);
        this.m_dsCompletedPlots.Sortable = true;
        this.m_dsCompletedPlots.AddComparer<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Strata), (IComparer) new PropertyComparer<Strata>((Func<Strata, object>) (s => (object) s.Description)));
        this.m_dsIncompletePlots = new DataBindingList<Plot>((IList<Plot>) list2);
        this.m_dsIncompletePlots.Sortable = true;
        this.m_dsIncompletePlots.AddComparer<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Strata), (IComparer) new PropertyComparer<Strata>((Func<Strata, object>) (s => (object) s.Description)));
        this.m_dsLogEntries = new DataBindingList<MobileLogEntry>(this.m_session.CreateCriteria<MobileLogEntry>().Add((ICriterion) Restrictions.Eq("Year", (object) this.m_year)).Add((ICriterion) Restrictions.Eq("Submitted", (object) true)).AddOrder(Order.Desc("DateTime")).List<MobileLogEntry>());
        this.m_dsLogEntries.Sortable = true;
      }
    }

    private void InitUI()
    {
      Year year = this.m_year;
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.dcPctTree.BindTo("Value", "Key", (object) new BindingSource((object) EnumHelper.ConvertToDictionary<PctMidRange>(), (string) null));
      this.dcPctShrub.BindTo("Value", "Key", (object) new BindingSource((object) EnumHelper.ConvertToDictionary<PctMidRange>(), (string) null));
      this.dcPctPlantable.BindTo("Value", "Key", (object) new BindingSource((object) EnumHelper.ConvertToDictionary<PctMidRange>(), (string) null));
      this.dcStrata.BindTo<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Description), (System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Self), (object) new BindingList<Strata>((IList<Strata>) year.Strata.ToList<Strata>()));
      if (!year.RecordGPS)
      {
        if (this.dgPlots.Columns.Contains((DataGridViewColumn) this.dcLatitude))
          this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcLatitude);
        if (this.dgPlots.Columns.Contains((DataGridViewColumn) this.dcLongitude))
          this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcLongitude);
      }
      else
      {
        if (!this.dgPlots.Columns.Contains((DataGridViewColumn) this.dcLongitude))
          this.dgPlots.Columns.Insert(3, (DataGridViewColumn) this.dcLongitude);
        if (!this.dgPlots.Columns.Contains((DataGridViewColumn) this.dcLatitude))
          this.dgPlots.Columns.Insert(3, (DataGridViewColumn) this.dcLatitude);
      }
      if (!year.RecordPlotAddress && this.dgPlots.Columns.Contains((DataGridViewColumn) this.dcAddress))
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcAddress);
      this.txtEmail1.Text = year.MobileEmail;
      this.txtEmail2.Text = year.MobileEmail;
      this.btnResetPassword.Visible = !string.IsNullOrEmpty(year.MobileKey);
      this.lblResetPassword.Visible = this.btnResetPassword.Visible;
      this.txtPassword2.Visible = string.IsNullOrEmpty(year.MobileKey);
      this.lblPassword2.Visible = this.txtPassword2.Visible;
      string str;
      if (this.m_year.Unit == YearUnit.English)
      {
        str = i_Tree_Eco_v6.Resources.Strings.UnitAcresAbbr;
        this.dcSize.DecimalPlaces = 2;
        this.dcSize.DefaultCellStyle.Format = "0.00";
      }
      else
      {
        str = i_Tree_Eco_v6.Resources.Strings.UnitHectaresAbbr;
        this.dcSize.DecimalPlaces = 4;
        this.dcSize.DefaultCellStyle.Format = "0.0000";
      }
      if (str != null)
        this.dcSize.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.Size, (object) str);
      this.dgPlots.DataSource = (object) this.m_dsPlots;
      this.dgPlots.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
      this.dgSubmitLog.DataSource = (object) this.m_dsLogEntries;
      this.dgSubmitLog.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
      this.cboDisplay.SelectedIndex = 0;
      this.DisplaySelectedPlots();
    }

    private void DisplaySelectedPlots() => this.lblSelPlots.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSelected, (object) this.m_selPlots.Count, (object) (this.m_dsPlots.Count > 100 ? 100 : this.m_dsPlots.Count), (object) v6Strings.Plot_PluralName);

    private void dgPlots_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (!(this.dgPlots.DataSource is DataBindingList<Plot> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      Plot plot = dataSource[e.RowIndex];
      if (this.dgPlots.Columns[e.ColumnIndex] != this.dcSelect)
        return;
      e.Value = (object) this.m_selPlots.Contains(plot);
      if (dataSource == this.m_dsCompletedPlots || this.m_dsCompletedPlots.Contains(plot))
      {
        this.dgPlots.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
        this.dgPlots.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.MediumSeaGreen;
      }
      else
      {
        this.dgPlots.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
        this.dgPlots.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
      }
    }

    private void dgPlots_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (!(this.dgPlots.DataSource is DataBindingList<Plot> dataSource) || e.ColumnIndex < 0 || e.RowIndex < 0 || this.dgPlots.Columns[e.ColumnIndex] != this.dcSelect)
        return;
      using (this.m_session.BeginTransaction())
      {
        Plot plot = dataSource[e.RowIndex];
        if (!this.m_selPlots.Contains(plot))
        {
          if (this.m_selPlots.Count < 100)
          {
            this.m_selPlots.Add(plot);
          }
          else
          {
            int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrMaxSelected, (object) v6Strings.Plot_SingularName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
        }
        else
          this.m_selPlots.Remove(plot);
        this.DisplaySelectedPlots();
      }
      if (this.m_selPlots.IsSupersetOf((IEnumerable<Plot>) dataSource))
        this.chkAllPlots.CheckState = CheckState.Checked;
      else if (this.chkAllPlots.CheckState == CheckState.Checked)
      {
        this.chkAllPlots.CheckState = CheckState.Indeterminate;
      }
      else
      {
        if (this.m_selPlots.Count != 0)
          return;
        this.chkAllPlots.CheckState = CheckState.Unchecked;
      }
    }

    private void chkAllPlots_CheckedChanged(object sender, EventArgs e)
    {
      DataBindingList<Plot> dataSource = this.dgPlots.DataSource as DataBindingList<Plot>;
      if (this.m_filtering || dataSource == null)
        return;
      if (this.chkAllPlots.Checked)
        this.m_selPlots.UnionWith((IEnumerable<Plot>) dataSource);
      else
        this.m_selPlots.ExceptWith((IEnumerable<Plot>) dataSource);
      this.DisplaySelectedPlots();
      this.dgPlots.Refresh();
    }

    private void cboDisplay_SelectedIndexChanged(object sender, EventArgs e)
    {
      int selectedIndex = this.cboDisplay.SelectedIndex;
      DataBindingList<Plot> other = (DataBindingList<Plot>) null;
      switch (selectedIndex)
      {
        case 0:
          other = this.m_dsPlots;
          break;
        case 1:
          other = this.m_dsCompletedPlots;
          break;
        case 2:
          other = this.m_dsIncompletePlots;
          break;
      }
      this.dgPlots.DataSource = (object) other;
      this.dgPlots.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      this.m_filtering = true;
      this.chkAllPlots.CheckState = other.Count <= 0 || !this.m_selPlots.IsSupersetOf((IEnumerable<Plot>) other) ? CheckState.Unchecked : CheckState.Checked;
      this.m_filtering = false;
      this.dgPlots.Refresh();
    }

    private bool IsValid()
    {
      int num = this.txtEmail1.Text.Length > 0 ? 1 : 0;
      bool flag1 = this.txtEmail2.Text.Length > 0;
      if (num != 0)
      {
        bool flag2 = this.EmailValid(this.txtEmail1.Text);
        this.ep.SetError((Control) this.txtEmail1, !flag2, i_Tree_Eco_v6.Resources.Strings.ErrInvalidEmail);
        this.ep.SetError((Control) this.txtEmail2, !flag1, i_Tree_Eco_v6.Resources.Strings.ErrConfirmEmail);
        if (flag2 & flag1)
        {
          bool flag3 = this.txtEmail1.Text.Equals(this.txtEmail2.Text, StringComparison.CurrentCultureIgnoreCase);
          this.ep.SetError((Control) this.txtEmail1, !flag3, i_Tree_Eco_v6.Resources.Strings.ErrEmailNotMatch);
          this.ep.SetError((Control) this.txtEmail2, !flag3, i_Tree_Eco_v6.Resources.Strings.ErrEmailNotMatch);
        }
      }
      else
        this.ep.SetError((Control) this.txtEmail1, true, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) i_Tree_Eco_v6.Resources.Strings.EmailAddress));
      bool flag4 = this.txtPassword1.Text.Length > 0;
      if (string.IsNullOrEmpty(this.m_year.MobileKey))
      {
        bool flag5 = this.txtPassword2.Text.Length > 0;
        if (flag4 & flag5)
        {
          bool flag6 = this.txtPassword1.Text.Equals(this.txtPassword2.Text, StringComparison.CurrentCulture);
          this.ep.SetError((Control) this.txtPassword1, !flag6, i_Tree_Eco_v6.Resources.Strings.ErrPasswordNotMatch);
          this.ep.SetError((Control) this.txtPassword2, !flag6, i_Tree_Eco_v6.Resources.Strings.ErrPasswordNotMatch);
        }
        else
        {
          this.ep.SetError((Control) this.txtPassword1, !flag4, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) i_Tree_Eco_v6.Resources.Strings.Password));
          this.ep.SetError((Control) this.txtPassword2, flag4 && !flag5, i_Tree_Eco_v6.Resources.Strings.ErrConfirmPassword);
        }
      }
      else
        this.ep.SetError((Control) this.txtPassword1, !flag4, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) i_Tree_Eco_v6.Resources.Strings.Password));
      return !this.HasErrors(this.ep);
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
      if (this.m_selPlots.Count == 0)
      {
        int num1 = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrSelection, (object) v6Strings.Plot_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (!this.IsValid())
          return;
        this.tableLayoutPanel1.Enabled = false;
        this.m_year.MobileEmail = this.txtEmail1.Text;
        TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        this.m_taskManager.Add(Task.Factory.StartNew<int>((Func<int>) (() => this.UploadData(this.m_session, this.m_year, this.txtPassword1.Text, (IEnumerable<Plot>) this.m_selPlots)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<int>>) (_ =>
        {
          if (_.IsFaulted)
          {
            StringBuilder stringBuilder = new StringBuilder(_.Exception.InnerException.Message);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.InternetErrorHelpMessage);
            int num2 = (int) MessageBox.Show((IWin32Window) this, stringBuilder.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.tableLayoutPanel1.Enabled = true;
          }
          else
          {
            switch (_.Result)
            {
              case 0:
                using (ITransaction transaction = this.m_session.BeginTransaction())
                {
                  this.m_session.SaveOrUpdate((object) this.m_year);
                  StringBuilder stringBuilder = new StringBuilder();
                  int num3 = 0;
                  foreach (Plot selPlot in (IEnumerable<Plot>) this.m_selPlots)
                  {
                    stringBuilder.Append(selPlot.Id);
                    if (++num3 < this.m_selPlots.Count)
                      stringBuilder.Append(", ");
                  }
                  this.m_session.Save((object) new MobileLogEntry()
                  {
                    Year = this.m_year,
                    Description = stringBuilder.ToString(),
                    MobileKey = this.m_year.MobileKey,
                    Submitted = true,
                    DateTime = DateTime.Now
                  });
                  transaction.Commit();
                }
                EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(this.m_year), (Control) this);
                this.Close();
                break;
              case 1:
                int num4 = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrInvalidPassword, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              default:
                int num5 = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrUnexpectedRetry, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                break;
            }
            this.tableLayoutPanel1.Enabled = true;
          }
        }), scheduler));
      }
    }

    private void btnResetPassword_Click(object sender, EventArgs e)
    {
      this.tableLayoutPanel1.Enabled = false;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew<string>((Func<string>) (() => this.RequestPwdReset(this.m_session, this.m_year)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<string>>) (t =>
      {
        string result = t.Result;
        if (string.IsNullOrEmpty(result))
        {
          using (Form form = (Form) new MobileResetPasswordForm())
          {
            int num = (int) form.ShowDialog((IWin32Window) this);
          }
        }
        else
        {
          StringBuilder stringBuilder = new StringBuilder(result);
          stringBuilder.Append(Environment.NewLine);
          stringBuilder.Append(Environment.NewLine);
          stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.InternetErrorHelpMessage);
          int num = (int) MessageBox.Show((IWin32Window) this, stringBuilder.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        this.tableLayoutPanel1.Enabled = true;
      }), scheduler));
    }

    private void dgPlots_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MobileSubmitSampleForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle7 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle8 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle9 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle10 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle11 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle12 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle13 = new DataGridViewCellStyle();
      this.chkAllPlots = new CheckBox();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.dgSubmitLog = new DataGridView();
      this.dcMobileKey = new DataGridViewTextBoxColumn();
      this.dcDateTime = new DataGridViewNullableDateTimeColumn();
      this.dcDescription = new DataGridViewTextBoxColumn();
      this.panel1 = new TableLayoutPanel();
      this.lblSelPlots = new Label();
      this.lblDisplay = new Label();
      this.cboDisplay = new ComboBox();
      this.dgPlots = new DataGridView();
      this.dcSelect = new DataGridViewCheckBoxColumn();
      this.dcId = new DataGridViewTextBoxColumn();
      this.dcStrata = new DataGridViewComboBoxColumn();
      this.dcAddress = new DataGridViewTextBoxColumn();
      this.dcLatitude = new DataGridViewTextBoxColumn();
      this.dcLongitude = new DataGridViewTextBoxColumn();
      this.dcDate = new DataGridViewNullableDateTimeColumn();
      this.dcCrew = new DataGridViewTextBoxColumn();
      this.dcContactInfo = new DataGridViewTextBoxColumn();
      this.dcSize = new DataGridViewNumericTextBoxColumn();
      this.dcOffsetPoint = new DataGridViewNumericTextBoxColumn();
      this.dcPhoto = new DataGridViewTextBoxColumn();
      this.dcStake = new DataGridViewCheckBoxColumn();
      this.dcPctTree = new DataGridViewComboBoxColumn();
      this.dcPctShrub = new DataGridViewComboBoxColumn();
      this.dcPctPlantable = new DataGridViewComboBoxColumn();
      this.dcPctMeasured = new DataGridViewNumericTextBoxColumn();
      this.dcComments = new DataGridViewTextBoxColumn();
      this.dcIsComplete = new DataGridViewCheckBoxColumn();
      this.tableLayoutPanel2 = new TableLayoutPanel();
      this.lblEmail1 = new Label();
      this.txtEmail1 = new TextBox();
      this.txtEmail2 = new TextBox();
      this.txtPassword2 = new TextBox();
      this.lblPassword2 = new Label();
      this.lblEmail2 = new Label();
      this.lblPassword1 = new Label();
      this.txtPassword1 = new TextBox();
      this.btnSubmit = new Button();
      this.Label3 = new Label();
      this.btnResetPassword = new Button();
      this.lblResetPassword = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      ((ISupportInitialize) this.ep).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.dgSubmitLog).BeginInit();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.dgPlots).BeginInit();
      this.tableLayoutPanel2.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.chkAllPlots, "chkAllPlots");
      this.chkAllPlots.Name = "chkAllPlots";
      this.chkAllPlots.UseVisualStyleBackColor = true;
      this.chkAllPlots.CheckedChanged += new EventHandler(this.chkAllPlots_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.dgSubmitLog, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.panel1, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel2, 1, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.label2, 1, 1);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.dgSubmitLog.AllowUserToAddRows = false;
      this.dgSubmitLog.AllowUserToDeleteRows = false;
      this.dgSubmitLog.AllowUserToResizeRows = false;
      this.dgSubmitLog.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgSubmitLog.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgSubmitLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgSubmitLog.Columns.AddRange((DataGridViewColumn) this.dcMobileKey, (DataGridViewColumn) this.dcDateTime, (DataGridViewColumn) this.dcDescription);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgSubmitLog.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgSubmitLog, "dgSubmitLog");
      this.dgSubmitLog.EditMode = DataGridViewEditMode.EditProgrammatically;
      this.dgSubmitLog.EnableHeadersVisualStyles = false;
      this.dgSubmitLog.MultiSelect = false;
      this.dgSubmitLog.Name = "dgSubmitLog";
      this.dgSubmitLog.ReadOnly = true;
      this.dgSubmitLog.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = SystemColors.Control;
      gridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.ForeColor = SystemColors.WindowText;
      gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.dgSubmitLog.RowHeadersDefaultCellStyle = gridViewCellStyle3;
      this.dgSubmitLog.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgSubmitLog.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgSubmitLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgSubmitLog.VirtualMode = true;
      this.dcMobileKey.DataPropertyName = "MobileKey";
      componentResourceManager.ApplyResources((object) this.dcMobileKey, "dcMobileKey");
      this.dcMobileKey.Name = "dcMobileKey";
      this.dcMobileKey.ReadOnly = true;
      this.dcDateTime.CustomFormat = (string) null;
      this.dcDateTime.DataPropertyName = "DateTime";
      componentResourceManager.ApplyResources((object) this.dcDateTime, "dcDateTime");
      this.dcDateTime.Name = "dcDateTime";
      this.dcDateTime.ReadOnly = true;
      this.dcDateTime.Resizable = DataGridViewTriState.True;
      this.dcDescription.DataPropertyName = "Description";
      componentResourceManager.ApplyResources((object) this.dcDescription, "dcDescription");
      this.dcDescription.Name = "dcDescription";
      this.dcDescription.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.Controls.Add((Control) this.lblSelPlots, 3, 0);
      this.panel1.Controls.Add((Control) this.lblDisplay, 0, 0);
      this.panel1.Controls.Add((Control) this.cboDisplay, 1, 0);
      this.panel1.Controls.Add((Control) this.chkAllPlots, 2, 0);
      this.panel1.Controls.Add((Control) this.dgPlots, 0, 1);
      this.panel1.Name = "panel1";
      this.tableLayoutPanel1.SetRowSpan((Control) this.panel1, 3);
      componentResourceManager.ApplyResources((object) this.lblSelPlots, "lblSelPlots");
      this.lblSelPlots.Name = "lblSelPlots";
      componentResourceManager.ApplyResources((object) this.lblDisplay, "lblDisplay");
      this.lblDisplay.Name = "lblDisplay";
      componentResourceManager.ApplyResources((object) this.cboDisplay, "cboDisplay");
      this.cboDisplay.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDisplay.FormattingEnabled = true;
      this.cboDisplay.Items.AddRange(new object[3]
      {
        (object) componentResourceManager.GetString("cboDisplay.Items"),
        (object) componentResourceManager.GetString("cboDisplay.Items1"),
        (object) componentResourceManager.GetString("cboDisplay.Items2")
      });
      this.cboDisplay.Name = "cboDisplay";
      this.cboDisplay.SelectedIndexChanged += new EventHandler(this.cboDisplay_SelectedIndexChanged);
      this.dgPlots.AllowUserToAddRows = false;
      this.dgPlots.AllowUserToDeleteRows = false;
      this.dgPlots.AllowUserToResizeRows = false;
      this.dgPlots.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle4.BackColor = SystemColors.Control;
      gridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle4.ForeColor = SystemColors.WindowText;
      gridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle4.WrapMode = DataGridViewTriState.True;
      this.dgPlots.ColumnHeadersDefaultCellStyle = gridViewCellStyle4;
      this.dgPlots.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgPlots.Columns.AddRange((DataGridViewColumn) this.dcSelect, (DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcStrata, (DataGridViewColumn) this.dcAddress, (DataGridViewColumn) this.dcLatitude, (DataGridViewColumn) this.dcLongitude, (DataGridViewColumn) this.dcDate, (DataGridViewColumn) this.dcCrew, (DataGridViewColumn) this.dcContactInfo, (DataGridViewColumn) this.dcSize, (DataGridViewColumn) this.dcOffsetPoint, (DataGridViewColumn) this.dcPhoto, (DataGridViewColumn) this.dcStake, (DataGridViewColumn) this.dcPctTree, (DataGridViewColumn) this.dcPctShrub, (DataGridViewColumn) this.dcPctPlantable, (DataGridViewColumn) this.dcPctMeasured, (DataGridViewColumn) this.dcComments, (DataGridViewColumn) this.dcIsComplete);
      this.panel1.SetColumnSpan((Control) this.dgPlots, 4);
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle5.BackColor = SystemColors.Window;
      gridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle5.ForeColor = SystemColors.ControlText;
      gridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle5.WrapMode = DataGridViewTriState.False;
      this.dgPlots.DefaultCellStyle = gridViewCellStyle5;
      componentResourceManager.ApplyResources((object) this.dgPlots, "dgPlots");
      this.dgPlots.EditMode = DataGridViewEditMode.EditProgrammatically;
      this.dgPlots.EnableHeadersVisualStyles = false;
      this.dgPlots.MultiSelect = false;
      this.dgPlots.Name = "dgPlots";
      this.dgPlots.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle6.BackColor = SystemColors.Control;
      gridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle6.ForeColor = SystemColors.WindowText;
      gridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle6.WrapMode = DataGridViewTriState.True;
      this.dgPlots.RowHeadersDefaultCellStyle = gridViewCellStyle6;
      this.dgPlots.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.dgPlots.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgPlots.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgPlots.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgPlots.VirtualMode = true;
      this.dgPlots.CellClick += new DataGridViewCellEventHandler(this.dgPlots_CellClick);
      this.dgPlots.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dgPlots_CellValueNeeded);
      this.dgPlots.DataError += new DataGridViewDataErrorEventHandler(this.dgPlots_DataError);
      this.dcSelect.Frozen = true;
      componentResourceManager.ApplyResources((object) this.dcSelect, "dcSelect");
      this.dcSelect.Name = "dcSelect";
      this.dcId.DataPropertyName = "Id";
      this.dcId.Frozen = true;
      componentResourceManager.ApplyResources((object) this.dcId, "dcId");
      this.dcId.Name = "dcId";
      this.dcStrata.DataPropertyName = "Strata";
      this.dcStrata.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcStrata, "dcStrata");
      this.dcStrata.Name = "dcStrata";
      this.dcStrata.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcAddress.DataPropertyName = "Address";
      componentResourceManager.ApplyResources((object) this.dcAddress, "dcAddress");
      this.dcAddress.Name = "dcAddress";
      this.dcLatitude.DataPropertyName = "Latitude";
      componentResourceManager.ApplyResources((object) this.dcLatitude, "dcLatitude");
      this.dcLatitude.Name = "dcLatitude";
      this.dcLongitude.DataPropertyName = "Longitude";
      componentResourceManager.ApplyResources((object) this.dcLongitude, "dcLongitude");
      this.dcLongitude.Name = "dcLongitude";
      this.dcDate.CustomFormat = (string) null;
      this.dcDate.DataPropertyName = "Date";
      this.dcDate.DateFormat = DateTimePickerFormat.Short;
      gridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcDate.DefaultCellStyle = gridViewCellStyle7;
      componentResourceManager.ApplyResources((object) this.dcDate, "dcDate");
      this.dcDate.Name = "dcDate";
      this.dcDate.Resizable = DataGridViewTriState.True;
      this.dcCrew.DataPropertyName = "Crew";
      componentResourceManager.ApplyResources((object) this.dcCrew, "dcCrew");
      this.dcCrew.Name = "dcCrew";
      this.dcContactInfo.DataPropertyName = "ContactInfo";
      componentResourceManager.ApplyResources((object) this.dcContactInfo, "dcContactInfo");
      this.dcContactInfo.Name = "dcContactInfo";
      this.dcSize.DataPropertyName = "Size";
      this.dcSize.DecimalPlaces = 1;
      gridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle8.Format = "0.0";
      this.dcSize.DefaultCellStyle = gridViewCellStyle8;
      this.dcSize.Format = "#.#";
      this.dcSize.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcSize, "dcSize");
      this.dcSize.Name = "dcSize";
      this.dcSize.Resizable = DataGridViewTriState.True;
      this.dcSize.Signed = false;
      this.dcOffsetPoint.DataPropertyName = "OffsetPoint";
      this.dcOffsetPoint.DecimalPlaces = 0;
      gridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcOffsetPoint.DefaultCellStyle = gridViewCellStyle9;
      this.dcOffsetPoint.Format = (string) null;
      this.dcOffsetPoint.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcOffsetPoint, "dcOffsetPoint");
      this.dcOffsetPoint.Name = "dcOffsetPoint";
      this.dcOffsetPoint.Resizable = DataGridViewTriState.True;
      this.dcOffsetPoint.Signed = false;
      this.dcPhoto.DataPropertyName = "Photo";
      componentResourceManager.ApplyResources((object) this.dcPhoto, "dcPhoto");
      this.dcPhoto.Name = "dcPhoto";
      this.dcStake.DataPropertyName = "Stake";
      componentResourceManager.ApplyResources((object) this.dcStake, "dcStake");
      this.dcStake.Name = "dcStake";
      this.dcStake.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctTree.DataPropertyName = "PercentTreeCover";
      gridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcPctTree.DefaultCellStyle = gridViewCellStyle10;
      this.dcPctTree.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcPctTree, "dcPctTree");
      this.dcPctTree.Name = "dcPctTree";
      this.dcPctTree.Resizable = DataGridViewTriState.True;
      this.dcPctTree.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctShrub.DataPropertyName = "PercentShrubCover";
      gridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcPctShrub.DefaultCellStyle = gridViewCellStyle11;
      this.dcPctShrub.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcPctShrub, "dcPctShrub");
      this.dcPctShrub.Name = "dcPctShrub";
      this.dcPctShrub.Resizable = DataGridViewTriState.True;
      this.dcPctShrub.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctPlantable.DataPropertyName = "PercentPlantable";
      gridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcPctPlantable.DefaultCellStyle = gridViewCellStyle12;
      this.dcPctPlantable.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcPctPlantable, "dcPctPlantable");
      this.dcPctPlantable.Name = "dcPctPlantable";
      this.dcPctPlantable.Resizable = DataGridViewTriState.True;
      this.dcPctPlantable.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctMeasured.DataPropertyName = "PercentMeasured";
      this.dcPctMeasured.DecimalPlaces = 0;
      gridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcPctMeasured.DefaultCellStyle = gridViewCellStyle13;
      this.dcPctMeasured.Format = "#;-#";
      this.dcPctMeasured.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcPctMeasured, "dcPctMeasured");
      this.dcPctMeasured.Name = "dcPctMeasured";
      this.dcPctMeasured.Resizable = DataGridViewTriState.True;
      this.dcPctMeasured.Signed = true;
      this.dcComments.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcComments.DataPropertyName = "Comments";
      componentResourceManager.ApplyResources((object) this.dcComments, "dcComments");
      this.dcComments.Name = "dcComments";
      this.dcIsComplete.DataPropertyName = "IsComplete";
      componentResourceManager.ApplyResources((object) this.dcIsComplete, "dcIsComplete");
      this.dcIsComplete.Name = "dcIsComplete";
      this.dcIsComplete.SortMode = DataGridViewColumnSortMode.Automatic;
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel2, "tableLayoutPanel2");
      this.tableLayoutPanel2.Controls.Add((Control) this.lblEmail1, 0, 0);
      this.tableLayoutPanel2.Controls.Add((Control) this.txtEmail1, 1, 0);
      this.tableLayoutPanel2.Controls.Add((Control) this.txtEmail2, 1, 1);
      this.tableLayoutPanel2.Controls.Add((Control) this.txtPassword2, 1, 3);
      this.tableLayoutPanel2.Controls.Add((Control) this.lblPassword2, 0, 3);
      this.tableLayoutPanel2.Controls.Add((Control) this.lblEmail2, 0, 1);
      this.tableLayoutPanel2.Controls.Add((Control) this.lblPassword1, 0, 2);
      this.tableLayoutPanel2.Controls.Add((Control) this.txtPassword1, 1, 2);
      this.tableLayoutPanel2.Controls.Add((Control) this.btnSubmit, 2, 6);
      this.tableLayoutPanel2.Controls.Add((Control) this.Label3, 0, 5);
      this.tableLayoutPanel2.Controls.Add((Control) this.btnResetPassword, 2, 4);
      this.tableLayoutPanel2.Controls.Add((Control) this.lblResetPassword, 0, 4);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      componentResourceManager.ApplyResources((object) this.lblEmail1, "lblEmail1");
      this.lblEmail1.Name = "lblEmail1";
      componentResourceManager.ApplyResources((object) this.txtEmail1, "txtEmail1");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.txtEmail1, 2);
      this.txtEmail1.Name = "txtEmail1";
      componentResourceManager.ApplyResources((object) this.txtEmail2, "txtEmail2");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.txtEmail2, 2);
      this.txtEmail2.Name = "txtEmail2";
      componentResourceManager.ApplyResources((object) this.txtPassword2, "txtPassword2");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.txtPassword2, 2);
      this.txtPassword2.Name = "txtPassword2";
      componentResourceManager.ApplyResources((object) this.lblPassword2, "lblPassword2");
      this.lblPassword2.Name = "lblPassword2";
      componentResourceManager.ApplyResources((object) this.lblEmail2, "lblEmail2");
      this.lblEmail2.Name = "lblEmail2";
      componentResourceManager.ApplyResources((object) this.lblPassword1, "lblPassword1");
      this.lblPassword1.Name = "lblPassword1";
      componentResourceManager.ApplyResources((object) this.txtPassword1, "txtPassword1");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.txtPassword1, 2);
      this.txtPassword1.Name = "txtPassword1";
      componentResourceManager.ApplyResources((object) this.btnSubmit, "btnSubmit");
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.UseVisualStyleBackColor = true;
      this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
      componentResourceManager.ApplyResources((object) this.Label3, "Label3");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.Label3, 3);
      this.Label3.ForeColor = Color.Blue;
      this.Label3.Name = "Label3";
      componentResourceManager.ApplyResources((object) this.btnResetPassword, "btnResetPassword");
      this.btnResetPassword.Name = "btnResetPassword";
      this.btnResetPassword.UseVisualStyleBackColor = true;
      this.btnResetPassword.Click += new EventHandler(this.btnResetPassword_Click);
      componentResourceManager.ApplyResources((object) this.lblResetPassword, "lblResetPassword");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.lblResetPassword, 2);
      this.lblResetPassword.Name = "lblResetPassword";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (MobileSubmitSampleForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.label1, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel1, 0);
      ((ISupportInitialize) this.ep).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((ISupportInitialize) this.dgSubmitLog).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.dgPlots).EndInit();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
