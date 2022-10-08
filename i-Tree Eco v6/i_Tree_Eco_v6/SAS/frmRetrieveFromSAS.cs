// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.SAS.frmRetrieveFromSAS
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.NHibernate;
using Eco.Domain.v6;
using Eco.Util.Convert;
using Eco.Util.Queries.Interfaces;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Properties;
using i_Tree_Eco_v6.Resources;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.SAS
{
  public class frmRetrieveFromSAS : Form
  {
    private ProgramSession m_ps;
    private CancellationTokenSource _sasCS;
    private IContainer components;
    private Label lblFileName;
    private TextBox textFileName;
    private Button btnOK;
    private Button btnCancel;
    private ProgressBar progressBar1;
    private Label lblStatus;
    private Label lblPercent;
    public Label labelProjectInfo;
    private Label label2;
    private TableLayoutPanel tableLayoutPanel1;
    private FlowLayoutPanel flowLayoutPanel1;
    private TableLayoutPanel tableLayoutPanel2;

    public frmRetrieveFromSAS()
    {
      this.m_ps = Program.Session;
      this.InitializeComponent();
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

    private void btnOK_Click(object sender, EventArgs e)
    {
      try
      {
        using (SASProcessor sasProcessor = new SASProcessor())
        {
          sasProcessor.prepareToStart(this.m_ps, (Form) this);
          string str = "";
          string fileName = "";
          using (ISession session = this.m_ps.InputSession.CreateSession())
          {
            using (session.BeginTransaction())
            {
              YearResult yearResult = session.CreateCriteria<YearResult>().Add((ICriterion) Restrictions.Eq("Year", (object) sasProcessor.currYear)).AddOrder(Order.Desc("RevProcessed")).SetMaxResults(1).List<YearResult>().FirstOrDefault<YearResult>();
              if (yearResult == null)
              {
                int num = (int) MessageBox.Show((IWin32Window) this, SASResources.ErrorDataNotSubmitted, Application.ProductName, MessageBoxButtons.OK);
                this.DialogResult = DialogResult.Abort;
                return;
              }
              str = yearResult.Data;
              fileName = Path.GetFileNameWithoutExtension(yearResult.Data) + "_processed.zip";
            }
          }
          if (sasProcessor.getExactCaseFileNameFromDownloadFolderInFTPServer(fileName).Length == 0)
          {
            if (sasProcessor.getExactCaseFileNameFromUploadFolderInFTPServer(str).Length != 0)
            {
              int summittedProject = sasProcessor.getQueueNumberOfSummittedProject(Settings.Default.FromUsersFtpUrl, str);
              if (summittedProject == -1)
              {
                int num1 = (int) MessageBox.Show((IWin32Window) this, SASResources.MsgProjectProcessing, Application.ProductName, MessageBoxButtons.OK);
              }
              else
              {
                int num2 = (int) MessageBox.Show((IWin32Window) this, string.Format(SASResources.MsgProjectQueue, (object) summittedProject, (object) str), Application.ProductName, MessageBoxButtons.OK);
              }
              this.DialogResult = DialogResult.Abort;
              return;
            }
            if (sasProcessor.getExactCaseFileNameFromFailedFolderInFTPServer(str).Length != 0)
            {
              int num = (int) MessageBox.Show((IWin32Window) this, string.Format(SASResources.ErrorProjectFailed, (object) str));
              this.DialogResult = DialogResult.Abort;
              return;
            }
            int num3 = (int) MessageBox.Show((IWin32Window) this, string.Format(SASResources.ErrorProjectNotFound, (object) str), Application.ProductName, MessageBoxButtons.OK);
            this.DialogResult = DialogResult.Abort;
            return;
          }
        }
        string projectName = "";
        string series = "";
        int year = 0;
        Guid projectYearGUID = new Guid();
        int yearRevision = 0;
        string SampleTypeSASProcessed = "P";
        this.lblStatus.Visible = true;
        this.UseWaitCursor = true;
        this.progressBar1.Value = 0;
        this.lblPercent.Text = "0%";
        this._sasCS = new CancellationTokenSource();
        CancellationToken ct = this._sasCS.Token;
        Progress<SASProgressArg> sasProgress = new Progress<SASProgressArg>();
        sasProgress.ProgressChanged += (EventHandler<SASProgressArg>) ((o, myArg) =>
        {
          if (myArg.Percent < 0 || myArg.Percent > 100)
            return;
          this.lblPercent.Visible = true;
          this.progressBar1.Value = myArg.Percent;
          this.lblStatus.Text = myArg.Description;
          this.lblPercent.Text = myArg.Percent.ToString() + "%";
        });
        this.tableLayoutPanel2.Visible = true;
        this.lblFileName.Visible = false;
        this.textFileName.Visible = false;
        this.btnOK.Visible = false;
        bool finishedDownloading = false;
        Task.Factory.StartNew((Action) (() =>
        {
          using (SASProcessor aSASProcessor = new SASProcessor())
          {
            aSASProcessor.prepareToStart(this.m_ps, (Form) this);
            string RemoteFilename = "";
            using (ISession session = this.m_ps.InputSession.CreateSession())
            {
              using (session.BeginTransaction())
              {
                YearResult yearResult = session.CreateCriteria<YearResult>().Add((ICriterion) Restrictions.Eq("Year", (object) aSASProcessor.currYear)).AddOrder(Order.Desc("RevProcessed")).SetMaxResults(1).List<YearResult>().FirstOrDefault<YearResult>();
                if (yearResult == null)
                  throw new Exception(SASResources.ErrorDataNotSubmitted);
                RemoteFilename = Path.GetFileNameWithoutExtension(yearResult.Data) + "_processed.zip";
              }
            }
            try
            {
              aSASProcessor.DownloadZipFile(RemoteFilename, (IProgress<SASProgressArg>) sasProgress, ct);
              finishedDownloading = true;
              aSASProcessor.UnzipFile(aSASProcessor.outputFolder, aSASProcessor.zipFileReceived);
              aSASProcessor.LoadReadme(ref projectName, ref series, ref year, ref projectYearGUID, ref yearRevision, ref SampleTypeSASProcessed);
            }
            catch (Exception ex)
            {
              if (new DriveInfo(this.m_ps.InputSession.InputDb).AvailableFreeSpace < 5000000L)
              {
                int num = (int) MessageBox.Show("Error occurs in retrieving. It might be the drive space is full.");
              }
              throw ex;
            }
            if (aSASProcessor.currYear.Guid != projectYearGUID)
              throw new SASException(string.Format(SASResources.ErrorProjectMismatch, (object) projectName, (object) series, (object) year));
            Action[] actionArray = new Action[6]
            {
              (Action) (() => this.deleteResultData(aSASProcessor.currYear.Guid)),
              (Action) (() => aSASProcessor.setupClassifiers(this.m_ps.InputSession)),
              (Action) (() => aSASProcessor.setupEstimateType(this.m_ps.InputSession)),
              (Action) (() => aSASProcessor.setupEquationType(this.m_ps.InputSession)),
              (Action) (() => aSASProcessor.setupUnits(this.m_ps.InputSession)),
              (Action) (() => aSASProcessor.getClassValues())
            };
            int length = actionArray.Length;
            int num4 = 0;
            IProgress<SASProgressArg> iprogress = (IProgress<SASProgressArg>) sasProgress;
            foreach (Action action in actionArray)
            {
              action();
              ++num4;
              iprogress.Report(new SASProgressArg()
              {
                Description = SASResources.PreparingToLoad,
                Percent = num4 * 100 / length
              });
            }
            aSASProcessor.loadSASResults((IProgress<SASProgressArg>) sasProgress, ct);
            iprogress.Report(new SASProgressArg()
            {
              Description = SASResources.Finished,
              Percent = 100
            });
          }
        }), ct, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((Action<Task>) (task =>
        {
          this.lblPercent.Visible = false;
          this.btnOK.Visible = true;
          this.UseWaitCursor = false;
          this.lblStatus.Visible = false;
          this.tableLayoutPanel2.Visible = false;
          this._sasCS = (CancellationTokenSource) null;
          if (task.IsCanceled)
            this.clearResults(this.m_ps.InputSession.YearKey.Value);
          else if (task.IsFaulted)
          {
            foreach (Exception innerException in task.Exception.InnerExceptions)
            {
              int num = (int) MessageBox.Show((IWin32Window) this, innerException.Message, SASResources.ErrorLoadingResults, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            if (!finishedDownloading)
            {
              this.btnOK.Text = SASResources.Retry;
            }
            else
            {
              this.clearResults(this.m_ps.InputSession.YearKey.Value);
              this.DialogResult = DialogResult.Cancel;
            }
          }
          else
          {
            using (ISession session = this.m_ps.InputSession.CreateSession())
            {
              using (ITransaction transaction = session.BeginTransaction())
              {
                YearResult yearResult = session.CreateCriteria<YearResult>().Add((ICriterion) Restrictions.Eq("Year.Guid", (object) this.m_ps.InputSession.YearKey)).Add((ICriterion) Restrictions.Eq("RevProcessed", (object) yearRevision)).UniqueResult<YearResult>();
                if (yearResult != null)
                {
                  yearResult.Completed = true;
                  session.SaveOrUpdate((object) yearResult);
                }
                IList<Forecast> forecastList = session.CreateCriteria<Forecast>().Add((ICriterion) Restrictions.Eq("Year.Guid", (object) this.m_ps.InputSession.YearKey)).List<Forecast>();
                foreach (Forecast forecast in (IEnumerable<Forecast>) forecastList)
                {
                  forecast.Changed = true;
                  session.SaveOrUpdate((object) forecast);
                }
                yearResult.Year.RevProcessed = yearRevision;
                yearResult.Year.Culture = this.m_ps.SpeciesCulture;
                yearResult.Year.SpeciesVersion = this.m_ps.SpeciesVersion;
                yearResult.Year.LocationVersion = this.m_ps.LocationVersion;
                session.SaveOrUpdate((object) yearResult.Year);
                transaction.Commit();
                EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(yearResult.Year), (Control) this);
                foreach (Forecast entity in (IEnumerable<Forecast>) forecastList)
                  EventPublisher.Publish<EntityUpdated<Forecast>>(new EntityUpdated<Forecast>(entity), (Control) this);
              }
            }
            int num = (int) MessageBox.Show((IWin32Window) this, SASResources.ResultsRetrieved, SASResources.ResultLoadTitle, MessageBoxButtons.OK);
            this.Close();
          }
        }), TaskScheduler.FromCurrentSynchronizationContext());
      }
      catch (Exception ex)
      {
        StringBuilder stringBuilder = new StringBuilder(ex.Message);
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(Strings.InternetErrorHelpMessage);
        int num = (int) MessageBox.Show((IWin32Window) this, stringBuilder.ToString(), SASResources.ResultLoadTitle, MessageBoxButtons.OK);
        this.Close();
      }
    }

    private void clearResults(Guid yearGuid)
    {
      this.deleteResultData(yearGuid);
      using (ISession session = this.m_ps.InputSession.CreateSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          Year year = session.Get<Year>((object) yearGuid);
          year.RevProcessed = 0;
          session.SaveOrUpdate((object) year);
          transaction.Commit();
        }
      }
      EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(yearGuid), (Control) this);
    }

    private void deleteResultData(Guid yearGuid)
    {
      using (ISession session = this.m_ps.InputSession.CreateSession())
      {
        SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(session).GetSASUtilProvider();
        try
        {
          sasUtilProvider.GetSQLQueryClearContentOfProject("BenMapTable").SetGuid(nameof (yearGuid), yearGuid).ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfProject("ClassValueTable").SetGuid(nameof (yearGuid), yearGuid).ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfProject("HourlyHydroResults").SetGuid(nameof (yearGuid), yearGuid).ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfProject("HourlyUFOREBResults").SetGuid(nameof (yearGuid), yearGuid).ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfProject("IndividualTreeEffects").SetGuid(nameof (yearGuid), yearGuid).ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfProject("IndividualTreeEnergyEffects").SetGuid(nameof (yearGuid), yearGuid).ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfProject("IndividualTreePollutionEffects").SetGuid(nameof (yearGuid), yearGuid).ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfProject("ModelNotes").SetGuid(nameof (yearGuid), yearGuid).ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfProject("Pollutants").SetGuid(nameof (yearGuid), yearGuid).ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfProject("UVIndexReduction").SetGuid(nameof (yearGuid), yearGuid).ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfProject("UVIndexReductionByStrataYearly").SetGuid(nameof (yearGuid), yearGuid).ExecuteUpdate();
          SASTransaction sasTransaction = new SASTransaction();
          sasTransaction.MaxNumber = 500;
          sasTransaction.Begin(session);
          foreach ((int, string) tuple in (IEnumerable<(int, string)>) session.GetNamedQuery("selectRecordsFromTableOfStatisticalEstimates").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, string)>()).List<(int, string)>())
          {
            IQuery contentOfProject = sasUtilProvider.GetSQLQueryClearContentOfProject(tuple.Item2);
            sasTransaction.IncreaseOperationNumber(contentOfProject.SetGuid(nameof (yearGuid), yearGuid).ExecuteUpdate());
          }
          sasTransaction.End();
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    private void OpenConnection(OleDbConnection conToOpen, string databaseName)
    {
      conToOpen.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + databaseName;
      conToOpen.Open();
    }

    private void CloseConnection(OleDbConnection conToClose)
    {
      if (conToClose == null || conToClose.State == ConnectionState.Closed)
        return;
      conToClose.Close();
    }

    private void textFileName_TextChanged(object sender, EventArgs e)
    {
    }

    private void frmRetrieveFromSAS_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this._sasCS == null)
        return;
      if (MessageBox.Show((IWin32Window) this, SASResources.ConfirmCancel, SASResources.ResultLoadTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        e.Cancel = true;
      else
        this._sasCS.Cancel();
    }

    private void label2_Click(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (frmRetrieveFromSAS));
      this.lblFileName = new Label();
      this.textFileName = new TextBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lblStatus = new Label();
      this.progressBar1 = new ProgressBar();
      this.lblPercent = new Label();
      this.label2 = new Label();
      this.labelProjectInfo = new Label();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.tableLayoutPanel2 = new TableLayoutPanel();
      this.tableLayoutPanel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblFileName, "lblFileName");
      this.lblFileName.Name = "lblFileName";
      componentResourceManager.ApplyResources((object) this.textFileName, "textFileName");
      this.textFileName.Name = "textFileName";
      this.textFileName.TextChanged += new EventHandler(this.textFileName_TextChanged);
      componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
      this.btnOK.Name = "btnOK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      componentResourceManager.ApplyResources((object) this.lblStatus, "lblStatus");
      this.lblStatus.Name = "lblStatus";
      componentResourceManager.ApplyResources((object) this.progressBar1, "progressBar1");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.progressBar1, 2);
      this.progressBar1.Name = "progressBar1";
      componentResourceManager.ApplyResources((object) this.lblPercent, "lblPercent");
      this.lblPercent.Name = "lblPercent";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      this.label2.Click += new EventHandler(this.label2_Click);
      componentResourceManager.ApplyResources((object) this.labelProjectInfo, "labelProjectInfo");
      this.labelProjectInfo.Name = "labelProjectInfo";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.flowLayoutPanel1, 0, 5);
      this.tableLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel2, 0, 4);
      this.tableLayoutPanel1.Controls.Add((Control) this.label2, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblFileName, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.textFileName, 0, 3);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.SetColumnSpan((Control) this.flowLayoutPanel1, 2);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCancel);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnOK);
      componentResourceManager.ApplyResources((object) this.flowLayoutPanel1, "flowLayoutPanel1");
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel2, "tableLayoutPanel2");
      this.tableLayoutPanel2.Controls.Add((Control) this.lblPercent, 1, 0);
      this.tableLayoutPanel2.Controls.Add((Control) this.lblStatus, 0, 0);
      this.tableLayoutPanel2.Controls.Add((Control) this.progressBar1, 0, 1);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.AcceptButton = (IButtonControl) this.btnOK;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Controls.Add((Control) this.labelProjectInfo);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (frmRetrieveFromSAS);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.FormClosing += new FormClosingEventHandler(this.frmRetrieveFromSAS_FormClosing);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
