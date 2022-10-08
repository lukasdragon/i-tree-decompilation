// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ForecastBasicForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Forms.Resources;
using NHibernate;
using NHibernate.Criterion;
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
  public class ForecastBasicForm : ForecastContentForm
  {
    private ISession _input;
    private TaskManager _taskMgr;
    private Forecast _forecast;
    private Mortality _healthy;
    private Mortality _sick;
    private Mortality _dying;
    private ProgramSession _ps;
    private IContainer components;
    private ToolStrip miniToolStrip;
    private TableLayoutPanel tableLayoutPanel1;
    private NumericUpDown DyingNumericUpDown;
    private NumericUpDown SickNumericUpDown;
    private NumericUpDown HealthyNumericUpDown;
    private Label DyingLabel;
    private Label SickLabel;
    private Label DurationLabel;
    public NumericUpDown ForecastYearsNumericUpDown;
    private Label HealthyLabel;
    private Label FrostFreeLabel;
    private Label BaseMortalitiesLabel;
    private Label ForecastYearsLabel;
    private NumericUpDown FrostFreeNumericUpDown;
    private Label label1;
    private Button btnOK;
    private Button btnCancel;
    private Panel panel1;
    private ErrorProvider ep;

    public ForecastBasicForm()
    {
      this.InitializeComponent();
      this._taskMgr = new TaskManager(new WaitCursor((Form) this));
      this._ps = ProgramSession.GetInstance();
      this._input = this._ps.InputSession.CreateSession();
      EventPublisher.Register<EntityUpdated<Forecast>>(new EventHandler<EntityUpdated<Forecast>>(this.Forecast_Updated));
      this._ps.InputSession.ForecastChanged += new EventHandler(this.InputSession_ForecastChanged);
      this.loadData();
      this._taskMgr.WhenAll().ContinueWith((System.Action<Task>) (t => this.registerChangeEvents((Control) this)));
    }

    private void InputSession_ForecastChanged(object sender, EventArgs e)
    {
      this.loadData();
      this.m_isDirty = false;
    }

    private void Forecast_Updated(object sender, EntityUpdated<Forecast> e)
    {
      if (this._forecast != null && this._forecast.Guid == e.Guid)
        this.refresh();
      this.m_isDirty = false;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.m_isDirty && !this._forecast.Changed)
      {
        int num = (int) MessageBox.Show(ForecastRes.EditModeBasicMessage, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
      }
      else
      {
        using (ITransaction transaction = this._input.BeginTransaction())
        {
          this._input.SaveOrUpdate((object) this._forecast);
          this._input.SaveOrUpdate((object) this._sick);
          this._input.SaveOrUpdate((object) this._dying);
          this._input.SaveOrUpdate((object) this._healthy);
          this.adjustDurations();
          transaction.Commit();
        }
        this._forecast.Changed = true;
        EventPublisher.Publish<EntityUpdated<Forecast>>(new EntityUpdated<Forecast>(this._forecast), (Control) this);
        this.m_isDirty = false;
        this.Close();
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (!this._forecast.Changed)
        this.m_isDirty = false;
      this.Close();
    }

    private Task forecastTask(TaskScheduler context) => Task.Factory.StartNew<Forecast>((Func<Forecast>) (() => this._input.CreateCriteria<Forecast>().Add((ICriterion) Restrictions.Eq("Guid", (object) this._ps.InputSession.ForecastKey)).Fetch(SelectMode.Fetch, "Mortalities").UniqueResult<Forecast>()), CancellationToken.None, TaskCreationOptions.None, this._ps.Scheduler).ContinueWith((System.Action<Task<Forecast>>) (t =>
    {
      if (this.IsDisposed)
        return;
      this._forecast = t.Result;
      this._healthy = this._forecast.Mortalities.Single<Mortality>((Func<Mortality, bool>) (m => m.Value == ForecastRes.Dieback00_49));
      this._sick = this._forecast.Mortalities.Single<Mortality>((Func<Mortality, bool>) (m => m.Value == ForecastRes.Dieback50_74));
      this._dying = this._forecast.Mortalities.Single<Mortality>((Func<Mortality, bool>) (m => m.Value == ForecastRes.Dieback75_99));
      Interlocked.Increment(ref this.m_changes);
      this.ForecastYearsNumericUpDown.DataBindings.Clear();
      this.FrostFreeNumericUpDown.DataBindings.Clear();
      this.HealthyNumericUpDown.DataBindings.Clear();
      this.SickNumericUpDown.DataBindings.Clear();
      this.DyingNumericUpDown.DataBindings.Clear();
      this.ForecastYearsNumericUpDown.DataBindings.Add("Value", (object) this._forecast, "NumYears");
      this.FrostFreeNumericUpDown.DataBindings.Add("Value", (object) this._forecast, "FrostFreeDays");
      this.HealthyNumericUpDown.DataBindings.Add("Value", (object) this._healthy, "Percent");
      this.SickNumericUpDown.DataBindings.Add("Value", (object) this._sick, "Percent");
      this.DyingNumericUpDown.DataBindings.Add("Value", (object) this._dying, "Percent");
      Interlocked.Decrement(ref this.m_changes);
    }), context);

    private void loadData()
    {
      this._taskMgr.Add(this.forecastTask(TaskScheduler.FromCurrentSynchronizationContext()));
      this._taskMgr.Add(this._taskMgr.WhenAll().ContinueWith((System.Action<Task>) (t => this.refresh())));
    }

    private void refresh()
    {
      if (this.IsDisposed)
        return;
      this._input.Refresh((object) this._forecast);
      this._input.Refresh((object) this._healthy);
      this._input.Refresh((object) this._sick);
      this._input.Refresh((object) this._dying);
    }

    private void adjustDurations()
    {
      IList<Replanting> replantingList = this._input.QueryOver<Replanting>().Where((System.Linq.Expressions.Expression<Func<Replanting, bool>>) (i => (int) i.Duration > (int) this._forecast.NumYears)).List();
      IList<ForecastPestEvent> forecastPestEventList = this._input.QueryOver<ForecastPestEvent>().Where((System.Linq.Expressions.Expression<Func<ForecastPestEvent, bool>>) (ee => (int) ee.Duration > (int) this._forecast.NumYears)).List();
      foreach (Replanting replanting in (IEnumerable<Replanting>) replantingList)
        replanting.Duration = this._forecast.NumYears;
      foreach (ForecastPestEvent forecastPestEvent in (IEnumerable<ForecastPestEvent>) forecastPestEventList)
        forecastPestEvent.Duration = this._forecast.NumYears;
      bool flag1 = replantingList.Count > 0;
      bool flag2 = forecastPestEventList.Count > 0;
      if (!flag1 && !flag2)
        return;
      string str1 = flag1 ? ForecastRes.ReplantTreesStr.ToLower() : "";
      string str2 = flag2 ? ForecastRes.ExtremeEventsStr.ToLower() : "";
      string str3 = " " + (flag1 & flag2 ? ForecastRes.AndStr.ToLower() : "") + " ";
      int num = (int) MessageBox.Show(string.Format(ForecastRes.DurationsShortenedMessage, (object) str1, (object) str3, (object) str2), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ForecastBasicForm));
      this.miniToolStrip = new ToolStrip();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.DyingNumericUpDown = new NumericUpDown();
      this.SickNumericUpDown = new NumericUpDown();
      this.HealthyNumericUpDown = new NumericUpDown();
      this.DyingLabel = new Label();
      this.SickLabel = new Label();
      this.DurationLabel = new Label();
      this.ForecastYearsNumericUpDown = new NumericUpDown();
      this.HealthyLabel = new Label();
      this.FrostFreeLabel = new Label();
      this.BaseMortalitiesLabel = new Label();
      this.ForecastYearsLabel = new Label();
      this.FrostFreeNumericUpDown = new NumericUpDown();
      this.label1 = new Label();
      this.panel1 = new Panel();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.ep = new ErrorProvider(this.components);
      this.tableLayoutPanel1.SuspendLayout();
      this.DyingNumericUpDown.BeginInit();
      this.SickNumericUpDown.BeginInit();
      this.HealthyNumericUpDown.BeginInit();
      this.ForecastYearsNumericUpDown.BeginInit();
      this.FrostFreeNumericUpDown.BeginInit();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.ep).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.miniToolStrip, "miniToolStrip");
      this.miniToolStrip.CanOverflow = false;
      this.miniToolStrip.GripStyle = ToolStripGripStyle.Hidden;
      this.miniToolStrip.Name = "miniToolStrip";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.DyingNumericUpDown, 1, 7);
      this.tableLayoutPanel1.Controls.Add((Control) this.SickNumericUpDown, 1, 6);
      this.tableLayoutPanel1.Controls.Add((Control) this.HealthyNumericUpDown, 1, 5);
      this.tableLayoutPanel1.Controls.Add((Control) this.DyingLabel, 0, 7);
      this.tableLayoutPanel1.Controls.Add((Control) this.SickLabel, 0, 6);
      this.tableLayoutPanel1.Controls.Add((Control) this.DurationLabel, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.ForecastYearsNumericUpDown, 1, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.HealthyLabel, 0, 5);
      this.tableLayoutPanel1.Controls.Add((Control) this.FrostFreeLabel, 0, 3);
      this.tableLayoutPanel1.Controls.Add((Control) this.BaseMortalitiesLabel, 0, 4);
      this.tableLayoutPanel1.Controls.Add((Control) this.ForecastYearsLabel, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.FrostFreeNumericUpDown, 1, 3);
      this.tableLayoutPanel1.Controls.Add((Control) this.label1, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.panel1, 1, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.DyingNumericUpDown.DecimalPlaces = 1;
      componentResourceManager.ApplyResources((object) this.DyingNumericUpDown, "DyingNumericUpDown");
      this.DyingNumericUpDown.Increment = new Decimal(new int[4]
      {
        1,
        0,
        0,
        65536
      });
      this.DyingNumericUpDown.Maximum = new Decimal(new int[4]
      {
        999,
        0,
        0,
        65536
      });
      this.DyingNumericUpDown.Name = "DyingNumericUpDown";
      this.DyingNumericUpDown.Value = new Decimal(new int[4]
      {
        999,
        0,
        0,
        65536
      });
      this.SickNumericUpDown.DecimalPlaces = 1;
      componentResourceManager.ApplyResources((object) this.SickNumericUpDown, "SickNumericUpDown");
      this.SickNumericUpDown.Increment = new Decimal(new int[4]
      {
        1,
        0,
        0,
        65536
      });
      this.SickNumericUpDown.Maximum = new Decimal(new int[4]
      {
        999,
        0,
        0,
        65536
      });
      this.SickNumericUpDown.Name = "SickNumericUpDown";
      this.SickNumericUpDown.Value = new Decimal(new int[4]
      {
        999,
        0,
        0,
        65536
      });
      this.HealthyNumericUpDown.DecimalPlaces = 1;
      componentResourceManager.ApplyResources((object) this.HealthyNumericUpDown, "HealthyNumericUpDown");
      this.HealthyNumericUpDown.Increment = new Decimal(new int[4]
      {
        1,
        0,
        0,
        65536
      });
      this.HealthyNumericUpDown.Maximum = new Decimal(new int[4]
      {
        999,
        0,
        0,
        65536
      });
      this.HealthyNumericUpDown.Name = "HealthyNumericUpDown";
      this.HealthyNumericUpDown.Value = new Decimal(new int[4]
      {
        999,
        0,
        0,
        65536
      });
      componentResourceManager.ApplyResources((object) this.DyingLabel, "DyingLabel");
      this.DyingLabel.Name = "DyingLabel";
      componentResourceManager.ApplyResources((object) this.SickLabel, "SickLabel");
      this.SickLabel.Name = "SickLabel";
      componentResourceManager.ApplyResources((object) this.DurationLabel, "DurationLabel");
      this.DurationLabel.Name = "DurationLabel";
      componentResourceManager.ApplyResources((object) this.ForecastYearsNumericUpDown, "ForecastYearsNumericUpDown");
      this.ForecastYearsNumericUpDown.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.ForecastYearsNumericUpDown.Name = "ForecastYearsNumericUpDown";
      this.ForecastYearsNumericUpDown.Value = new Decimal(new int[4]
      {
        30,
        0,
        0,
        0
      });
      componentResourceManager.ApplyResources((object) this.HealthyLabel, "HealthyLabel");
      this.HealthyLabel.Name = "HealthyLabel";
      componentResourceManager.ApplyResources((object) this.FrostFreeLabel, "FrostFreeLabel");
      this.FrostFreeLabel.Name = "FrostFreeLabel";
      componentResourceManager.ApplyResources((object) this.BaseMortalitiesLabel, "BaseMortalitiesLabel");
      this.BaseMortalitiesLabel.Name = "BaseMortalitiesLabel";
      componentResourceManager.ApplyResources((object) this.ForecastYearsLabel, "ForecastYearsLabel");
      this.ForecastYearsLabel.Name = "ForecastYearsLabel";
      componentResourceManager.ApplyResources((object) this.FrostFreeNumericUpDown, "FrostFreeNumericUpDown");
      this.FrostFreeNumericUpDown.Maximum = new Decimal(new int[4]
      {
        365,
        0,
        0,
        0
      });
      this.FrostFreeNumericUpDown.Name = "FrostFreeNumericUpDown";
      this.FrostFreeNumericUpDown.Value = new Decimal(new int[4]
      {
        149,
        0,
        0,
        0
      });
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      this.panel1.Controls.Add((Control) this.btnOK);
      this.panel1.Controls.Add((Control) this.btnCancel);
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.Name = "panel1";
      componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
      this.btnOK.Name = "btnOK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.ep.ContainerControl = (ContainerControl) this;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Name = nameof (ForecastBasicForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel1, 0);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.DyingNumericUpDown.EndInit();
      this.SickNumericUpDown.EndInit();
      this.HealthyNumericUpDown.EndInit();
      this.ForecastYearsNumericUpDown.EndInit();
      this.FrostFreeNumericUpDown.EndInit();
      this.panel1.ResumeLayout(false);
      ((ISupportInitialize) this.ep).EndInit();
      this.ResumeLayout(false);
    }
  }
}
