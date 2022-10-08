// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.DataContentForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Events;
using NHibernate;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class DataContentForm : ContentForm
  {
    private Year _year;
    private readonly WaitCursor _wc;
    private readonly TaskManager _taskManager;
    private IContainer components;
    private TableLayoutPanel pnlEditHelp;
    private Label lblEditHelp;
    private Label lblReportWarning;

    public DataContentForm()
    {
      this.InitializeComponent();
      if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
        return;
      this._wc = new WaitCursor((Form) this);
      this._taskManager = new TaskManager(this._wc);
      this.Session = Program.Session.InputSession.CreateSession();
      EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));
      this.Init();
    }

    private void Year_Updated(object sender, EntityEventArgs e)
    {
      if (this._year == null || !(this._year.Guid == e.Guid))
        return;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.TaskManager.Add(Task.Factory.StartNew((System.Action) (() => this.GetYear(e.Guid)), CancellationToken.None, TaskCreationOptions.None, Program.Session.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.OnRequestRefresh();
      }), scheduler));
    }

    private void GetYear(Guid g)
    {
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          try
          {
            if (this._year == null)
            {
              Year year = this.Session.QueryOver<Year>().Where((Expression<Func<Year, bool>>) (y => y.Guid == g)).JoinQueryOver<Series>((Expression<Func<Year, Series>>) (y => y.Series)).JoinQueryOver<Project>((Expression<Func<Series, Project>>) (s => s.Project)).SingleOrDefault();
              this.InitializeYear(year);
              this._year = year;
            }
            else
              this.Session.Refresh((object) this._year, LockMode.None);
            transaction.Commit();
          }
          catch (HibernateException ex)
          {
            transaction.Rollback();
          }
        }
      }
    }

    protected Year Year => this._year;

    protected Series Series => this._year.Series;

    protected Project Project => this._year.Series.Project;

    protected TaskManager TaskManager => this._taskManager;

    private void Init()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.TaskManager.Add(Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, Program.Session.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (t.IsFaulted || this.IsDisposed)
          return;
        this.OnDataLoaded();
      }), scheduler));
    }

    protected virtual void LoadData() => this.GetYear(Program.Session.InputSession.YearKey.Value);

    protected virtual void OnDataLoaded() => this.OnRequestRefresh();

    protected virtual void InitializeYear(Year year)
    {
    }

    protected override void OnRequestRefresh()
    {
      base.OnRequestRefresh();
      bool showHelpMsg = this.ShowHelpMsg;
      bool showReportWarning = this.ShowReportWarning;
      bool flag = showHelpMsg | showReportWarning;
      this.lblEditHelp.Visible = showHelpMsg;
      this.lblReportWarning.Visible = showReportWarning;
      this.pnlEditHelp.Visible = flag;
    }

    protected ISession Session { get; private set; }

    protected virtual bool ShowHelpMsg
    {
      get
      {
        Year year = this.Year;
        return year != null && year.Changed;
      }
    }

    protected virtual bool ShowReportWarning
    {
      get
      {
        Year year = this.Year;
        return year != null && year.Changed && year.RevProcessed > 0;
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
      this.pnlEditHelp = new TableLayoutPanel();
      this.lblEditHelp = new Label();
      this.lblReportWarning = new Label();
      this.pnlEditHelp.SuspendLayout();
      this.SuspendLayout();
      this.lblBreadcrumb.Margin = new Padding(7);
      this.lblBreadcrumb.Size = new Size(1067, 40);
      this.pnlEditHelp.AutoSize = true;
      this.pnlEditHelp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlEditHelp.ColumnCount = 1;
      this.pnlEditHelp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.pnlEditHelp.Controls.Add((Control) this.lblEditHelp, 0, 0);
      this.pnlEditHelp.Controls.Add((Control) this.lblReportWarning, 0, 1);
      this.pnlEditHelp.Dock = DockStyle.Top;
      this.pnlEditHelp.Location = new Point(0, 40);
      this.pnlEditHelp.Margin = new Padding(3, 2, 3, 2);
      this.pnlEditHelp.Name = "pnlEditHelp";
      this.pnlEditHelp.RowCount = 2;
      this.pnlEditHelp.RowStyles.Add(new RowStyle());
      this.pnlEditHelp.RowStyles.Add(new RowStyle());
      this.pnlEditHelp.Size = new Size(1067, 40);
      this.pnlEditHelp.TabIndex = 14;
      this.pnlEditHelp.Visible = false;
      this.lblEditHelp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblEditHelp.AutoSize = true;
      this.lblEditHelp.Font = new Font("Calibri", 9.75f, FontStyle.Bold);
      this.lblEditHelp.ImeMode = ImeMode.NoControl;
      this.lblEditHelp.Location = new Point(3, 2);
      this.lblEditHelp.Margin = new Padding(3, 2, 3, 2);
      this.lblEditHelp.Name = "lblEditHelp";
      this.lblEditHelp.Size = new Size(1061, 15);
      this.lblEditHelp.TabIndex = 1;
      this.lblEditHelp.Text = "Required inputs MUST be completely and properly filled out. If you get stuck, you can delete the row and start over.";
      this.lblEditHelp.Visible = false;
      this.lblReportWarning.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblReportWarning.AutoSize = true;
      this.lblReportWarning.Font = new Font("Calibri", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblReportWarning.ForeColor = Color.DarkOrange;
      this.lblReportWarning.Location = new Point(3, 22);
      this.lblReportWarning.Margin = new Padding(3);
      this.lblReportWarning.Name = "lblReportWarning";
      this.lblReportWarning.Size = new Size(1061, 15);
      this.lblReportWarning.TabIndex = 1;
      this.lblReportWarning.Text = "Warning: Reports may not reflect changes to your data until you reprocess your project.";
      this.lblReportWarning.Visible = false;
      this.AutoScaleDimensions = new SizeF(8f, 18f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1067, 623);
      this.Controls.Add((Control) this.pnlEditHelp);
      this.Margin = new Padding(4);
      this.Name = nameof (DataContentForm);
      this.Text = nameof (DataContentForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.pnlEditHelp, 0);
      this.pnlEditHelp.ResumeLayout(false);
      this.pnlEditHelp.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
