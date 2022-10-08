// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ForecastSummaryForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Core;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Forecast;
using Eco.Util.Views;
using i_Tree_Eco_v6.Forms.Resources;
using i_Tree_Eco_v6.Properties;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ForecastSummaryForm : Form
  {
    private const int SEPARATION = 6;
    private const int PROGRESS_MAX = 2147483645;
    private TaskManager _taskMgr;
    private CancellationTokenSource _cts;
    private Stage _stage;
    private string _result;
    private ISession _session;
    private ProgramSession _ps;
    private object _syncobj;
    private Year _year;
    private Eco.Domain.v6.Forecast _forecast;
    private IList<ForecastEvent> _events;
    private IList<Replanting> _replantings;
    private IContainer components;
    private Panel panel1;
    private Button ContinueButton;
    private Button ExitForecastButton;
    private ListBox BasicListBox;
    private ListBox MortalityListBox;
    private ListBox ReplantingListBox;
    private ListBox EventListBox;
    private Label ContinueLabel;
    private ProgressBar ForecastProgressBar;
    private TableLayoutPanel MainTableLayout;
    private Label MentionReportLabel;
    private Label CohortFileLabel;
    private Button CohortFileBrowseButton;
    private TextBox textCohortFile;

    public event EventHandler<EventArgs> ForecastSuccessful;

    public event EventHandler<EventArgs> ForecastFailed;

    public event EventHandler<EventArgs> ForecastCancelled;

    public ForecastSummaryForm()
    {
      this.InitializeComponent();
      this._taskMgr = new TaskManager(new WaitCursor((Form) this));
      this._ps = ProgramSession.GetInstance();
      this._session = this._ps.InputSession.CreateSession();
      this._syncobj = new object();
      ForecastSimulation.ProgressAdvanced += new ForecastEventHandler(this.Forecast_ProgressAdvanced);
      PollutantCalculation.ProgressAdvanced += new ForecastEventHandler(this.Forecast_ProgressAdvanced);
      PopulateResults.ProgressAdvanced += new ForecastEventHandler(this.Forecast_ProgressAdvanced);
      this.Init();
    }

    private void ContinueButton_Click(object sender, EventArgs e)
    {
      string tempDb = "";
      this.textCohortFile.Text = this.textCohortFile.Text.Trim();
      if (!string.IsNullOrEmpty(this.textCohortFile.Text))
      {
        string directoryName = Path.GetDirectoryName(this.textCohortFile.Text);
        if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
        {
          int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.DirectoryNotExist, (object) directoryName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        tempDb = this.textCohortFile.Text;
      }
      else
        tempDb = Path.GetTempFileName() + ".ieco";
      this.textCohortFile.Enabled = false;
      this.CohortFileBrowseButton.Enabled = false;
      this.ForecastProgressBar.Maximum = 2147483645;
      this.ContinueButton.Enabled = false;
      this._cts = new CancellationTokenSource();
      this._taskMgr.Add(Task.Factory.StartNew((System.Action) (() =>
      {
        File.Copy(this._ps.InputSession.InputDb, tempDb, true);
        InputSession inputSession = new InputSession(tempDb);
        inputSession.YearKey = this._ps.InputSession.YearKey;
        inputSession.ForecastKey = this._ps.InputSession.ForecastKey;
        using (ISession session = inputSession.CreateSession())
        {
          using (ITransaction transaction = session.BeginTransaction())
          {
            try
            {
              session.GetNamedQuery("DeleteCohorts").SetGuid("guid", inputSession.ForecastKey.Value).ExecuteUpdate();
              IQuery namedQuery1 = session.GetNamedQuery("DeletePollutantResults");
              Guid? forecastKey = inputSession.ForecastKey;
              Guid val1 = forecastKey.Value;
              namedQuery1.SetGuid("guid", val1).ExecuteUpdate();
              IQuery namedQuery2 = session.GetNamedQuery("DeleteCohortResults");
              forecastKey = inputSession.ForecastKey;
              Guid val2 = forecastKey.Value;
              namedQuery2.SetGuid("guid", val2).ExecuteUpdate();
              transaction.Commit();
            }
            catch
            {
              transaction.Rollback();
              throw;
            }
          }
          ProgramSession instance = ProgramSession.GetInstance();
          using (ISession ls = Program.Session.LocSp.OpenSession())
          {
            ForecastSimulation.begin(new EstimateUtil(inputSession, instance.LocSp), inputSession, session, ls, this._cts.Token);
            PollutantCalculation.begin(inputSession, session, ls, this._cts.Token);
          }
          PopulateResults.begin(this._ps.InputSession, inputSession, session, this._cts.Token);
          this._result = string.Format(ForecastRes.SuccessfulForecastMessage, (object) this.getForecastTitle());
        }
        if (!string.IsNullOrEmpty(this.textCohortFile.Text))
          return;
        File.Delete(tempDb);
      }), this._cts.Token, TaskCreationOptions.None, this._ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (t.IsCanceled)
        {
          this.onForecastCompletion(EventArgs.Empty, this.ForecastCancelled);
        }
        else
        {
          this._result = !t.IsFaulted ? string.Format(ForecastRes.SuccessfulForecastMessage, (object) this.getForecastTitle()) : ForecastRes.FailedForecastMessage + "\n\n" + t.Exception.InnerExceptions[0].Message;
          int num = (int) MessageBox.Show((IWin32Window) this, this._result, Application.ProductName, MessageBoxButtons.OK, t.IsFaulted ? MessageBoxIcon.Hand : MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
          this.onForecastCompletion(EventArgs.Empty, t.IsFaulted ? this.ForecastFailed : this.ForecastSuccessful);
          this._cts = (CancellationTokenSource) null;
          this.DialogResult = DialogResult.OK;
        }
      }), TaskScheduler.FromCurrentSynchronizationContext()));
    }

    private void Forecast_ProgressAdvanced(object sender, ForecastEventArgs e) => this.advance(e);

    private void ForecastSummaryForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this._cts == null)
        return;
      if (MessageBox.Show((IWin32Window) this, ForecastRes.CancelForecastWarning, "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        this._cts.Cancel();
      else
        e.Cancel = true;
    }

    private void Init()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this._taskMgr.Add(Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this._ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        if (t.IsFaulted)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrUnexpected, (object) t.Exception.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          this.InitUI();
      }), scheduler));
    }

    private void LoadData()
    {
      using (this._session.BeginTransaction())
      {
        Year year = this._session.Get<Year>((object) this._ps.InputSession.YearKey);
        Eco.Domain.v6.Forecast forecast = this._session.Get<Eco.Domain.v6.Forecast>((object) this._ps.InputSession.ForecastKey);
        NHibernateUtil.Initialize((object) forecast.Mortalities);
        IList<Replanting> replantingList = this._session.CreateCriteria<Replanting>().Add((ICriterion) Restrictions.Eq("Forecast", (object) forecast)).List<Replanting>();
        IList<ForecastEvent> forecastEventList = this._session.CreateCriteria<ForecastEvent>().Add((ICriterion) Restrictions.Eq("Forecast", (object) forecast)).List<ForecastEvent>();
        lock (this._syncobj)
        {
          this._year = year;
          this._forecast = forecast;
          this._replantings = replantingList;
          this._events = forecastEventList;
        }
      }
    }

    private void InitUI()
    {
      Mortality mortality1 = this._forecast.Mortalities.Single<Mortality>((Func<Mortality, bool>) (m => m.Value == ForecastRes.Dieback00_49));
      Mortality mortality2 = this._forecast.Mortalities.Single<Mortality>((Func<Mortality, bool>) (m => m.Value == ForecastRes.Dieback50_74));
      Mortality mortality3 = this._forecast.Mortalities.Single<Mortality>((Func<Mortality, bool>) (m => m.Value == ForecastRes.Dieback75_99));
      this.ContinueLabel.Text = string.Format("{0}  {1}", (object) string.Format(ForecastRes.ContinueForecastMessage, (object) this._forecast.Title), (object) ForecastRes.ForecastDurationWarning);
      int x = this.ContinueButton.Location.X - this.ContinueLabel.Size.Width - 6;
      if (x < 0)
        x = 0;
      int y = this.ContinueLabel.Location.Y;
      this.ContinueLabel.Location = new Point(x, y);
      this.BasicListBox.Items.Add((object) ForecastRes.BasicParametersStr.ToUpper());
      this.BasicListBox.Items.Add((object) "");
      int totalWidth = Enumerable.Max((IEnumerable<int>) new int[5]
      {
        ForecastRes.ForecastYearsMessage.Length,
        ForecastRes.FrostFreeDaysMessage.Length,
        ForecastRes.HealthyMessage.Length,
        ForecastRes.SickMessage.Length,
        ForecastRes.DyingMessage.Length
      }) + 5;
      this.BasicListBox.Items.Add((object) ((ForecastRes.ForecastYearsMessage + ":").PadRight(totalWidth) + this._forecast.NumYears.ToString()));
      this.BasicListBox.Items.Add((object) ((ForecastRes.FrostFreeDaysMessage + ":").PadRight(totalWidth) + this._forecast.FrostFreeDays.ToString()));
      this.BasicListBox.Items.Add((object) ((ForecastRes.HealthyMessage + ":").PadRight(totalWidth) + mortality1.Percent.ToString("#0.0") + "%"));
      this.BasicListBox.Items.Add((object) ((ForecastRes.SickMessage + ":").PadRight(totalWidth) + mortality2.Percent.ToString("#0.0") + "%"));
      this.BasicListBox.Items.Add((object) ((ForecastRes.DyingMessage + ":").PadRight(totalWidth) + mortality3.Percent.ToString("#0.0") + "%"));
      this.EventListBox.Items.Add((object) ForecastRes.ExtremeEventsStr.ToUpper());
      this.EventListBox.Items.Add((object) "");
      this.MortalityListBox.Items.Add((object) ForecastRes.CustomMortalitiesStr.ToUpper());
      this.MortalityListBox.Items.Add((object) "");
      this.EventListBox.Items.AddRange((object[]) RetryExecutionHandler.Execute<IList<string>>((Func<IList<string>>) (() =>
      {
        IList<string> stringList = (IList<string>) new List<string>();
        using (ISession session = Program.Session.LocSp.OpenSession())
        {
          foreach (ForecastEvent e in (IEnumerable<ForecastEvent>) this._events)
          {
            string pestName = "";
            ForecastPestEvent pe = e as ForecastPestEvent;
            if (pe != null)
              pestName = session.QueryOver<Pest>().Where((System.Linq.Expressions.Expression<Func<Pest, bool>>) (p => p.Id == pe.PestId)).Select((System.Linq.Expressions.Expression<Func<Pest, object>>) (p => p.CommonName)).Cacheable().SingleOrDefault<string>();
            stringList.Add(this.toString(e, pestName));
          }
        }
        return stringList;
      })).ToArray<string>());
      foreach (Mortality mortality4 in (IEnumerable<Mortality>) this._forecast.Mortalities)
      {
        if (mortality4.Type == ForecastRes.GenusStr)
        {
          SpeciesView speciesView;
          if (this._ps.Species.TryGetValue(mortality4.Value, out speciesView))
            this.MortalityListBox.Items.Add((object) this.toString(mortality4, speciesView.CommonScientificName));
        }
        else
          this.MortalityListBox.Items.Add((object) this.toString(mortality4));
      }
      this.ReplantingListBox.Items.Add((object) ForecastRes.ReplantTreesStr.ToUpper());
      this.ReplantingListBox.Items.Add((object) "");
      this.ReplantingListBox.Items.AddRange((object[]) this._replantings.Select<Replanting, string>((Func<Replanting, string>) (i => this.toString(i))).ToArray<string>());
    }

    private string toString(Mortality m, string commonName = "")
    {
      string str1 = m.Type == ForecastRes.GenusStr ? commonName : m.Value;
      string str2 = m.IsPercentStarting ? "starting population" : "annual";
      return string.Format("{0}% of trees annually from {1} '{2}' (% {3})", (object) m.Percent.ToString("#0.0"), (object) m.Type, (object) str1, (object) str2);
    }

    private string toString(Replanting i)
    {
      string str1 = this._year.Unit == YearUnit.English ? i_Tree_Eco_v6.Resources.Strings.UnitInch : i_Tree_Eco_v6.Resources.Strings.UnitCentimeter;
      string str2 = i.Duration > (short) 1 ? "s" : "";
      return string.Format("Starting year {0}, plant {1} trees with DBH {2} {3} in {4} areas for {5} year{6}", (object) i.StartYear, (object) i.Number.ToString("###,###"), (object) i.DBH, (object) str1, (object) i.StratumDesc, (object) i.Duration, (object) str2);
    }

    private string toString(ForecastEvent e, string pestName = "")
    {
      switch (e)
      {
        case null:
          throw new ArgumentNullException(nameof (e));
        case ForecastWeatherEvent _:
          return string.Format("{0} in year {1}, killing {2}% of all trees annually", (object) EnumHelper.GetDescription<WeatherEvent>(((ForecastWeatherEvent) e).WeatherEvent), (object) e.StartYear, (object) e.MortalityPercent);
        case ForecastPestEvent _:
          ForecastPestEvent forecastPestEvent = (ForecastPestEvent) e;
          string str = forecastPestEvent.PlantPestHosts ? "" : " (no host planting during)";
          return string.Format("{0} strike in year {1}, killing {2}% of all host trees annually for {3} years", (object) pestName, (object) forecastPestEvent.StartYear, (object) forecastPestEvent.MortalityPercent.ToString("#0.0"), (object) forecastPestEvent.Duration, (object) str);
        default:
          throw new ArgumentException("Is not a valid type of ForecastEvent", nameof (e));
      }
    }

    private void advance(ForecastEventArgs e)
    {
      string msg;
      switch (e.Stage)
      {
        case Stage.Initializing:
          msg = "Initializing parameters...";
          break;
        case Stage.CalculatingCohorts:
          msg = "Creating Cohorts based on current data...";
          break;
        case Stage.CalculatingReplantCohorts:
          msg = "Creating Cohorts for trees to be planted";
          break;
        case Stage.Forecasting:
          msg = string.Format("Forecasting tree growth for year {0}...", (object) e.CurrentYear);
          break;
        case Stage.CalculatingPollutants:
          msg = string.Format("Calculating Pollutant removal for year {0}...", (object) e.CurrentYear);
          break;
        case Stage.PolulatingResults:
          msg = string.Format("Populating results...");
          break;
        case Stage.Succeeded:
          msg = string.Format(ForecastRes.SuccessfulForecastMessage, (object) e.Forecast.Title);
          e.Progress = this.ForecastProgressBar.Maximum - this.ForecastProgressBar.Value;
          break;
        default:
          msg = "Performing calculations...";
          break;
      }
      if (!this.ContinueLabel.IsDisposed && this.ContinueLabel.IsHandleCreated)
      {
        if (this.ContinueLabel.InvokeRequired)
          this.ContinueLabel.Invoke((Delegate) (() => this.ContinueLabel.Text = msg));
        else
          this.ContinueLabel.Text = msg;
      }
      if (!this.ForecastProgressBar.IsDisposed && this.ForecastProgressBar.IsHandleCreated)
        this.ForecastProgressBar.Invoke((Delegate) new ForecastSummaryForm.step(this.stepProgressBar), (object) e);
      else
        this.stepProgressBar(e);
    }

    private void stepProgressBar(ForecastEventArgs e)
    {
      if (this._stage != e.Stage)
      {
        this._stage = e.Stage;
        this.ForecastProgressBar.Value = 0;
      }
      this.ForecastProgressBar.Step = (int) ((double) e.Progress / (double) e.EndProgress * 2147483645.0);
      this.ForecastProgressBar.PerformStep();
    }

    private string getForecastTitle() => this._forecast != null ? this._forecast.Title : string.Empty;

    private void onForecastCompletion(EventArgs e, EventHandler<EventArgs> completionHandler)
    {
      EventHandler<EventArgs> eventHandler = completionHandler;
      if (eventHandler == null)
        return;
      foreach (Delegate invocation in eventHandler.GetInvocationList())
      {
        Control target = invocation.Target as Control;
        if (target.InvokeRequired)
          target.Invoke(invocation, (object) this, (object) e);
        else
          invocation.DynamicInvoke((object) this, (object) e);
      }
    }

    private void CohortFileBrowseButton_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();
      saveFileDialog1.Title = i_Tree_Eco_v6.Resources.Strings.SelectCohortFile;
      saveFileDialog1.Filter = string.Join("|", new string[2]
      {
        string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterEcoFile, (object) string.Join(";", Settings.Default.ExtEcoProj)),
        string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
      });
      saveFileDialog1.CheckPathExists = true;
      saveFileDialog1.OverwritePrompt = true;
      saveFileDialog1.AddExtension = true;
      saveFileDialog1.ShowHelp = false;
      saveFileDialog1.CreatePrompt = false;
      SaveFileDialog saveFileDialog2 = saveFileDialog1;
      if (saveFileDialog2.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.textCohortFile.Text = saveFileDialog2.FileName;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ForecastSummaryForm));
      this.BasicListBox = new ListBox();
      this.MortalityListBox = new ListBox();
      this.ReplantingListBox = new ListBox();
      this.EventListBox = new ListBox();
      this.panel1 = new Panel();
      this.CohortFileBrowseButton = new Button();
      this.textCohortFile = new TextBox();
      this.CohortFileLabel = new Label();
      this.ForecastProgressBar = new ProgressBar();
      this.ContinueButton = new Button();
      this.ExitForecastButton = new Button();
      this.ContinueLabel = new Label();
      this.MainTableLayout = new TableLayoutPanel();
      this.MentionReportLabel = new Label();
      this.panel1.SuspendLayout();
      this.MainTableLayout.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.BasicListBox, "BasicListBox");
      this.BasicListBox.FormattingEnabled = true;
      this.BasicListBox.Name = "BasicListBox";
      this.BasicListBox.SelectionMode = SelectionMode.None;
      this.BasicListBox.TabStop = false;
      componentResourceManager.ApplyResources((object) this.MortalityListBox, "MortalityListBox");
      this.MortalityListBox.FormattingEnabled = true;
      this.MortalityListBox.Name = "MortalityListBox";
      this.MortalityListBox.SelectionMode = SelectionMode.None;
      this.MortalityListBox.TabStop = false;
      componentResourceManager.ApplyResources((object) this.ReplantingListBox, "ReplantingListBox");
      this.ReplantingListBox.FormattingEnabled = true;
      this.ReplantingListBox.Name = "ReplantingListBox";
      this.ReplantingListBox.SelectionMode = SelectionMode.None;
      this.ReplantingListBox.TabStop = false;
      componentResourceManager.ApplyResources((object) this.EventListBox, "EventListBox");
      this.EventListBox.FormattingEnabled = true;
      this.EventListBox.Name = "EventListBox";
      this.EventListBox.SelectionMode = SelectionMode.None;
      this.EventListBox.TabStop = false;
      this.panel1.Controls.Add((Control) this.CohortFileBrowseButton);
      this.panel1.Controls.Add((Control) this.textCohortFile);
      this.panel1.Controls.Add((Control) this.CohortFileLabel);
      this.panel1.Controls.Add((Control) this.ForecastProgressBar);
      this.panel1.Controls.Add((Control) this.ContinueButton);
      this.panel1.Controls.Add((Control) this.ExitForecastButton);
      this.panel1.Controls.Add((Control) this.ContinueLabel);
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.Name = "panel1";
      componentResourceManager.ApplyResources((object) this.CohortFileBrowseButton, "CohortFileBrowseButton");
      this.CohortFileBrowseButton.Name = "CohortFileBrowseButton";
      this.CohortFileBrowseButton.UseVisualStyleBackColor = true;
      this.CohortFileBrowseButton.Click += new EventHandler(this.CohortFileBrowseButton_Click);
      componentResourceManager.ApplyResources((object) this.textCohortFile, "textCohortFile");
      this.textCohortFile.Name = "textCohortFile";
      componentResourceManager.ApplyResources((object) this.CohortFileLabel, "CohortFileLabel");
      this.CohortFileLabel.Name = "CohortFileLabel";
      componentResourceManager.ApplyResources((object) this.ForecastProgressBar, "ForecastProgressBar");
      this.ForecastProgressBar.Name = "ForecastProgressBar";
      componentResourceManager.ApplyResources((object) this.ContinueButton, "ContinueButton");
      this.ContinueButton.Name = "ContinueButton";
      this.ContinueButton.UseVisualStyleBackColor = true;
      this.ContinueButton.Click += new EventHandler(this.ContinueButton_Click);
      componentResourceManager.ApplyResources((object) this.ExitForecastButton, "ExitForecastButton");
      this.ExitForecastButton.DialogResult = DialogResult.Cancel;
      this.ExitForecastButton.Name = "ExitForecastButton";
      this.ExitForecastButton.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.ContinueLabel, "ContinueLabel");
      this.ContinueLabel.Name = "ContinueLabel";
      componentResourceManager.ApplyResources((object) this.MainTableLayout, "MainTableLayout");
      this.MainTableLayout.Controls.Add((Control) this.panel1, 0, 5);
      this.MainTableLayout.Controls.Add((Control) this.MentionReportLabel, 0, 0);
      this.MainTableLayout.Controls.Add((Control) this.BasicListBox, 0, 1);
      this.MainTableLayout.Controls.Add((Control) this.ReplantingListBox, 0, 3);
      this.MainTableLayout.Controls.Add((Control) this.EventListBox, 0, 4);
      this.MainTableLayout.Controls.Add((Control) this.MortalityListBox, 0, 2);
      this.MainTableLayout.Name = "MainTableLayout";
      componentResourceManager.ApplyResources((object) this.MentionReportLabel, "MentionReportLabel");
      this.MentionReportLabel.Name = "MentionReportLabel";
      this.AcceptButton = (IButtonControl) this.ContinueButton;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.CancelButton = (IButtonControl) this.ExitForecastButton;
      this.Controls.Add((Control) this.MainTableLayout);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ForecastSummaryForm);
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.FormClosing += new FormClosingEventHandler(this.ForecastSummaryForm_FormClosing);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.MainTableLayout.ResumeLayout(false);
      this.MainTableLayout.PerformLayout();
      this.ResumeLayout(false);
    }

    private delegate void step(ForecastEventArgs args);
  }
}
