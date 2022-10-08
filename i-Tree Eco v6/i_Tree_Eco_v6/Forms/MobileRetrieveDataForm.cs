// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.MobileRetrieveDataForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using Eco.Domain.DTO.v6;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Processors;
using i_Tree_Eco_v6.Properties;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml;

namespace i_Tree_Eco_v6.Forms
{
  public class MobileRetrieveDataForm : MobileForm
  {
    private ProgramSession m_ps;
    private TaskManager m_taskManager;
    private ISession m_session;
    private Year m_year;
    private DataBindingList<MobileLogEntry> m_dsLogEntries;
    private IContainer components;
    private Label label1;
    private TableLayoutPanel tableLayoutPanel1;
    private Panel pnlRetrieveList;
    private Button btnShowList;
    private TextBox txtPassword;
    private Label label2;
    private Button btnResetPassword;
    private Button btnRetrieveData;
    private DataGridView dgRetrieve;
    private SplitContainer splitContainer1;
    private TableLayoutPanel tableLayoutPanel2;
    private Label lblRetrievedData;
    private DataGridView dgRetrieveLog;
    private DataGridViewTextBoxColumn dcMobileKey;
    private DataGridViewNullableDateTimeColumn dcDateTime;
    private DataGridViewTextBoxColumn dcDescription;

    public MobileRetrieveDataForm()
    {
      this.m_ps = Program.Session;
      this.m_taskManager = new TaskManager(new WaitCursor((Form) this));
      this.InitializeComponent();
      this.dgRetrieve.DoubleBuffered(true);
      this.dgRetrieve.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgRetrieve.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgRetrieveLog.DoubleBuffered(true);
      this.dgRetrieveLog.AutoGenerateColumns = false;
      this.dgRetrieveLog.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgRetrieveLog.RowHeadersDefaultCellStyle = Program.InActiveGridDefaultCellStyle;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler));
      this.m_taskManager.Add(this.m_taskManager.WhenAll().ContinueWith((System.Action<Task>) (t => this.Init()), scheduler));
    }

    private void Init()
    {
      this.dgRetrieveLog.DataSource = (object) this.m_dsLogEntries;
      this.dgRetrieveLog.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
    }

    private void LoadData()
    {
      if (this.m_ps.InputSession == null || !this.m_ps.InputSession.YearKey.HasValue)
        return;
      if (this.m_session == null)
        this.m_session = this.m_ps.InputSession.CreateSession();
      using (this.m_session.BeginTransaction())
      {
        this.m_year = this.m_session.Get<Year>((object) this.m_ps.InputSession.YearKey);
        NHibernateUtil.Initialize((object) this.m_year.Series);
        this.m_dsLogEntries = new DataBindingList<MobileLogEntry>(this.m_session.CreateCriteria<MobileLogEntry>().Add((ICriterion) Restrictions.Eq("Year", (object) this.m_year)).Add((ICriterion) Restrictions.Eq("Submitted", (object) false)).AddOrder(Order.Desc("DateTime")).List<MobileLogEntry>());
        this.m_dsLogEntries.Sortable = true;
      }
    }

    private int RetrieveData()
    {
      int num1 = 0;
      DataTable changes = (this.dgRetrieve.DataSource as DataTable).GetChanges(DataRowState.Modified);
      string text = this.txtPassword.Text;
      bool isSample = this.m_year.Series.IsSample;
      foreach (DataRow row in (InternalDataCollectionBase) changes.Rows)
      {
        int id = (int) row["Id"];
        YearDTO dto = (YearDTO) null;
        while (dto == null)
        {
          try
          {
            dto = this.FetchData(id, text);
          }
          catch (FetchException ex)
          {
            switch (MessageBox.Show(this.AppendInternetErrorHelpMessage(string.Format(i_Tree_Eco_v6.Resources.Strings.ErrMobileDataRetrieve, row["Description"], row["Date"], (object) ex.Message)), Application.ProductName, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2))
            {
              case DialogResult.Abort:
                return -num1 - 1;
              case DialogResult.Ignore:
                goto label_7;
              default:
                continue;
            }
          }
        }
label_7:
        if (dto != null)
        {
          Updater u = (Updater) new SimpleUpdater(this.m_ps.InputSession.SessionFactory);
          u.Conflicted += new EventHandler<ConflictEventArgs>(this.On_UpdateConflict);
          try
          {
            if (isSample)
              this.ProcessSampleUpdate(u, dto);
            else
              this.ProcessInventoryUpdate(u, dto);
            if (u.Status != Eco.Util.Processors.UpdateStatus.Aborted)
            {
              bool flag = false;
              while (!flag)
              {
                try
                {
                  flag = this.MarkAsRetrieved(id, text);
                  using (ITransaction transaction = this.m_session.BeginTransaction())
                  {
                    this.m_session.Save((object) new MobileLogEntry()
                    {
                      Year = this.m_year,
                      MobileKey = this.m_year.MobileKey,
                      Description = row["Description"].ToString(),
                      DateTime = DateTime.Parse(row["Date"].ToString()),
                      Submitted = false
                    });
                    transaction.Commit();
                  }
                }
                catch (ProcessingException ex)
                {
                  StringBuilder stringBuilder = new StringBuilder(string.Format(i_Tree_Eco_v6.Resources.Strings.ErrMobileDataProcessed, row["Description"], row["Date"], (object) ex.Message));
                  stringBuilder.Append(Environment.NewLine);
                  stringBuilder.Append(Environment.NewLine);
                  stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.InternetErrorHelpMessage);
                  if (MessageBox.Show(stringBuilder.ToString(), Application.ProductName, MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    break;
                }
              }
              ++num1;
            }
          }
          catch (Exception ex)
          {
            StringBuilder stringBuilder = new StringBuilder(string.Format(i_Tree_Eco_v6.Resources.Strings.ErrMobileDataProcessing, row["Description"], row["Date"], (object) ex.Message));
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.InternetErrorHelpMessage);
            int num2 = (int) MessageBox.Show(stringBuilder.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
        }
      }
      return num1;
    }

    private void On_UpdateConflict(object sender, ConflictEventArgs e)
    {
      string str = string.Empty;
      ResolveConflictForm resolveConflictForm = (ResolveConflictForm) null;
      if (e.Theirs is Plot)
        str = v6Strings.Plot_SingularName;
      else if (e.Theirs is PlotLandUse)
        str = v6Strings.LandUse_SingularName;
      else if (e.Theirs is PlotGroundCover)
        str = v6Strings.PlotGroundCover_SingularName;
      else if (e.Theirs is Building)
        str = v6Strings.Building_SingularName;
      else if (e.Theirs is ReferenceObject)
        str = v6Strings.ReferenceObject_SingularName;
      else if (e.Theirs is Shrub)
        str = v6Strings.Shrub_SingularName;
      else if (e.Theirs is Stem)
        str = v6Strings.Stem_SingularName;
      else if (e.Theirs is Tree)
        str = v6Strings.Tree_SingularName;
      switch (e.Conflict)
      {
        case Conflict.RevisionMismatch:
          resolveConflictForm = new ResolveConflictForm(string.Format(i_Tree_Eco_v6.Resources.Strings.ErrConflictUpdate, (object) str, (object) e.Mine.Revision, (object) e.TheirRevision));
          break;
        case Conflict.EntityDeleted:
          resolveConflictForm = new ResolveConflictForm(string.Format(i_Tree_Eco_v6.Resources.Strings.ErrConflictDelete, (object) str));
          break;
      }
      if (resolveConflictForm == null)
        return;
      int num = (int) resolveConflictForm.ShowDialog();
      e.Resolution = resolveConflictForm.Resolution;
    }

    private void ProcessSampleUpdate(Updater u, YearDTO dto)
    {
      Year y = u.Session.Get<Year>((object) this.m_ps.InputSession.YearKey);
      using (ITransaction transaction = u.Session.BeginTransaction())
      {
        foreach (PlotDTO plot in dto.Plots)
        {
          if ((plot.State & Eco.Domain.DTO.v6.State.Deleted) != Eco.Domain.DTO.v6.State.Existing)
            u.PlotProcessor.Delete(plot);
          else if ((plot.State & Eco.Domain.DTO.v6.State.New) != Eco.Domain.DTO.v6.State.Existing)
            u.PlotProcessor.Create(plot, y);
          else
            u.PlotProcessor.Update(plot, y);
          if (u.Status != Eco.Util.Processors.UpdateStatus.Normal)
            break;
        }
        if (u.Status == Eco.Util.Processors.UpdateStatus.Normal)
        {
          try
          {
            transaction.Commit();
          }
          catch (Exception ex)
          {
            transaction.Rollback();
            throw;
          }
        }
        else
          transaction.Rollback();
      }
    }

    private void ProcessInventoryUpdate(Updater u, YearDTO dto)
    {
      Year y = u.Session.Get<Year>((object) this.m_ps.InputSession.YearKey);
      short num = (short) u.Session.CreateCriteria<Plot>().Add((ICriterion) Restrictions.Eq("Year", (object) y)).SetProjection((IProjection) Projections.Max("Id")).UniqueResult<int>();
      using (ITransaction transaction = u.Session.BeginTransaction())
      {
        foreach (PlotDTO plot in dto.Plots)
        {
          plot.IsComplete = new bool?(true);
          if ((plot.State & Eco.Domain.DTO.v6.State.Deleted) != Eco.Domain.DTO.v6.State.Existing)
            u.PlotProcessor.Delete(plot);
          else if ((plot.State & Eco.Domain.DTO.v6.State.New) != Eco.Domain.DTO.v6.State.Existing)
          {
            plot.Id = (int) ++num;
            for (int index = 0; index < plot.Trees.Count; ++index)
              plot.Trees[index].Id = index != 0 ? (int) ++num : plot.Id;
            u.PlotProcessor.Create(plot, y);
          }
          else
            u.PlotProcessor.Update(plot, y);
          if (u.Status != Eco.Util.Processors.UpdateStatus.Normal)
            break;
        }
        if (u.Status == Eco.Util.Processors.UpdateStatus.Normal)
        {
          try
          {
            transaction.Commit();
          }
          catch (Exception ex)
          {
            transaction.Rollback();
            throw;
          }
        }
        else
          transaction.Rollback();
      }
    }

    private DataTable FetchList(string password)
    {
      DataTable dataTable = new DataTable("DataList");
      dataTable.Columns.AddRange(new DataColumn[4]
      {
        new DataColumn("Retrieve", typeof (bool)),
        new DataColumn("Id", typeof (int)),
        new DataColumn("Description", typeof (string)),
        new DataColumn("Date", typeof (DateTime))
      });
      string requestUriString = Settings.Default.Https + Settings.Default.Host + Settings.Default.MobileFetchUrl + "?key=" + HttpUtility.UrlEncode(this.m_year.MobileKey);
      try
      {
        HttpWebRequest httpWebRequest = WebRequest.Create(requestUriString) as HttpWebRequest;
        string str = string.Format("password={0}", (object) HttpUtility.UrlEncode(password));
        httpWebRequest.Method = "POST";
        httpWebRequest.UserAgent = Settings.Default.UserAgent;
        httpWebRequest.Referer = Settings.Default.Referer;
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.ContentLength = (long) str.Length;
        httpWebRequest.Accept = "*/*";
        httpWebRequest.KeepAlive = true;
        httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
        StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
        streamWriter.Write(str);
        streamWriter.Flush();
        streamWriter.Close();
        HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
        Stream responseStream = response.GetResponseStream();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(responseStream);
        response.Close();
        foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("Item"))
        {
          DataRow row = dataTable.NewRow();
          foreach (XmlNode childNode in xmlNode.ChildNodes)
          {
            string name = childNode.Name;
            if (!(name == "DataID"))
            {
              if (!(name == "Description"))
              {
                if (name == "UploadTime")
                  row["Date"] = (object) DateTime.Parse(childNode.InnerText);
              }
              else
                row["Description"] = (object) childNode.InnerText;
            }
            else
              row["Id"] = (object) int.Parse(childNode.InnerText);
          }
          dataTable.Rows.Add(row);
        }
        dataTable.AcceptChanges();
      }
      catch (WebException ex)
      {
        string message = !(ex.Response is HttpWebResponse response) ? ex.Message : response.StatusDescription;
        response?.Close();
        WebException innerException = ex;
        throw new FetchException(message, (Exception) innerException);
      }
      return dataTable;
    }

    private YearDTO FetchData(int id, string password)
    {
      string requestUriString = Settings.Default.Https + Settings.Default.Host + Settings.Default.MobileFetchUrl + "?key=" + HttpUtility.UrlEncode(this.m_year.MobileKey) + "&dataid=" + (object) id;
      YearDTO yearDto;
      try
      {
        HttpWebRequest httpWebRequest = WebRequest.Create(requestUriString) as HttpWebRequest;
        string str = string.Format("password={0}", (object) HttpUtility.UrlEncode(password));
        httpWebRequest.Method = "POST";
        httpWebRequest.UserAgent = Settings.Default.UserAgent;
        httpWebRequest.Referer = Settings.Default.Referer;
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.ContentLength = (long) str.Length;
        httpWebRequest.Accept = "*/*";
        httpWebRequest.KeepAlive = true;
        httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
        StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
        streamWriter.Write(str);
        streamWriter.Flush();
        streamWriter.Close();
        HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
        yearDto = JsonConvert.DeserializeObject<YearDTO>(new StreamReader(response.GetResponseStream()).ReadToEnd());
        response.Close();
      }
      catch (WebException ex)
      {
        string message = !(ex.Response is HttpWebResponse response) ? ex.Message : response.StatusDescription;
        response?.Close();
        WebException innerException = ex;
        throw new FetchException(message, (Exception) innerException);
      }
      catch (JsonSerializationException ex)
      {
        throw new FetchException(ex.Message, (Exception) ex);
      }
      return yearDto;
    }

    private bool MarkAsRetrieved(int id, string password)
    {
      string requestUriString = Settings.Default.Https + Settings.Default.Host + Settings.Default.MobileRetrievedUrl + "?key=" + HttpUtility.UrlEncode(this.m_year.MobileKey) + "&dataid=" + (object) id;
      try
      {
        HttpWebRequest httpWebRequest = WebRequest.Create(requestUriString) as HttpWebRequest;
        string str = string.Format("password={0}", (object) HttpUtility.UrlEncode(password));
        httpWebRequest.Method = "POST";
        httpWebRequest.UserAgent = Settings.Default.UserAgent;
        httpWebRequest.Referer = Settings.Default.Referer;
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.ContentLength = (long) str.Length;
        httpWebRequest.Accept = "*/*";
        httpWebRequest.KeepAlive = true;
        httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
        StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
        streamWriter.Write(str);
        streamWriter.Flush();
        streamWriter.Close();
        (httpWebRequest.GetResponse() as HttpWebResponse).Close();
      }
      catch (WebException ex)
      {
        string message = !(ex.Response is HttpWebResponse response) ? ex.Message : response.StatusDescription;
        response?.Close();
        WebException innerException = ex;
        throw new ProcessingException(message, (Exception) innerException);
      }
      return true;
    }

    private void btnShowList_Click(object sender, EventArgs e)
    {
      this.ep.SetError((Control) this.txtPassword, string.IsNullOrEmpty(this.txtPassword.Text), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) i_Tree_Eco_v6.Resources.Strings.Password));
      if (this.pnlRetrieveList.HasErrors(this.ep))
        return;
      this.tableLayoutPanel1.Enabled = false;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.UpdateDataList().ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        if (t.IsFaulted)
        {
          foreach (Exception innerException in t.Exception.InnerExceptions)
          {
            int num = (int) MessageBox.Show((IWin32Window) this, this.AppendInternetErrorHelpMessage(!(innerException is AggregateException) ? innerException.Message : innerException.InnerException.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
        }
        else if (this.dgRetrieve.DataSource == null)
        {
          int num1 = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.MsgMobileNoDataFound, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          this.txtPassword.Enabled = false;
          this.btnResetPassword.Visible = false;
        }
        this.tableLayoutPanel1.Enabled = true;
      }), scheduler);
    }

    private void btnRetrieveData_Click(object sender, EventArgs e)
    {
      int num1 = this.RetrieveData();
      if (num1 > 0)
      {
        TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler));
        this.m_taskManager.Add(this.m_taskManager.WhenAll().ContinueWith((System.Action<Task>) (t => this.Init()), scheduler));
        int num2 = (int) MessageBox.Show(string.Format(i_Tree_Eco_v6.Resources.Strings.MsgMobileImportSuccess, (object) num1), Application.ProductName, MessageBoxButtons.OK);
      }
      else
      {
        int num3 = (int) MessageBox.Show(string.Format(i_Tree_Eco_v6.Resources.Strings.MsgMobileImportAborted, (object) -num1), Application.ProductName, MessageBoxButtons.OK);
      }
      this.UpdateDataList();
    }

    private Task UpdateDataList()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      Task task = Task.Factory.StartNew<DataTable>((Func<DataTable>) (() => this.FetchList(this.txtPassword.Text)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<DataTable>>) (t =>
      {
        if (t.IsFaulted)
        {
          Exception innerException = t.Exception.InnerException;
          StringBuilder stringBuilder = new StringBuilder(innerException.Message);
          if (innerException.InnerException != null && !innerException.InnerException.Message.Contains("500"))
          {
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.InternetErrorHelpMessage);
          }
          int num = (int) MessageBox.Show((IWin32Window) this, stringBuilder.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.tableLayoutPanel1.Enabled = true;
        }
        else
        {
          if (this.IsDisposed)
            return;
          if (t.Result.Rows.Count == 0)
          {
            this.dgRetrieve.DataSource = (object) null;
          }
          else
          {
            this.dgRetrieve.DataSource = (object) t.Result;
            this.dgRetrieve.Columns["Id"].Visible = false;
            this.dgRetrieve.Columns["Description"].ReadOnly = true;
            this.dgRetrieve.Columns["Date"].ReadOnly = true;
          }
          this.dgRetrieve.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
          this.tableLayoutPanel1.Enabled = true;
          this.dgRetrieve.Enabled = this.dgRetrieve.DataSource != null;
          this.btnRetrieveData.Enabled = this.dgRetrieve.DataSource != null;
        }
      }), scheduler);
      this.m_taskManager.Add(task);
      return task;
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

    private string AppendInternetErrorHelpMessage(string msg)
    {
      StringBuilder stringBuilder = new StringBuilder(msg);
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.InternetErrorHelpMessage);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MobileRetrieveDataForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
      this.label1 = new Label();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.pnlRetrieveList = new Panel();
      this.btnShowList = new Button();
      this.btnRetrieveData = new Button();
      this.txtPassword = new TextBox();
      this.btnResetPassword = new Button();
      this.label2 = new Label();
      this.dgRetrieve = new DataGridView();
      this.splitContainer1 = new SplitContainer();
      this.tableLayoutPanel2 = new TableLayoutPanel();
      this.dgRetrieveLog = new DataGridView();
      this.dcMobileKey = new DataGridViewTextBoxColumn();
      this.dcDateTime = new DataGridViewNullableDateTimeColumn();
      this.dcDescription = new DataGridViewTextBoxColumn();
      this.lblRetrievedData = new Label();
      ((ISupportInitialize) this.ep).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.pnlRetrieveList.SuspendLayout();
      ((ISupportInitialize) this.dgRetrieve).BeginInit();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      ((ISupportInitialize) this.dgRetrieveLog).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.pnlRetrieveList, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.label1, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.dgRetrieve, 0, 2);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.pnlRetrieveList, "pnlRetrieveList");
      this.pnlRetrieveList.Controls.Add((Control) this.btnShowList);
      this.pnlRetrieveList.Controls.Add((Control) this.btnRetrieveData);
      this.pnlRetrieveList.Controls.Add((Control) this.txtPassword);
      this.pnlRetrieveList.Controls.Add((Control) this.btnResetPassword);
      this.pnlRetrieveList.Controls.Add((Control) this.label2);
      this.pnlRetrieveList.Name = "pnlRetrieveList";
      componentResourceManager.ApplyResources((object) this.btnShowList, "btnShowList");
      this.btnShowList.Name = "btnShowList";
      this.btnShowList.UseVisualStyleBackColor = true;
      this.btnShowList.Click += new EventHandler(this.btnShowList_Click);
      componentResourceManager.ApplyResources((object) this.btnRetrieveData, "btnRetrieveData");
      this.btnRetrieveData.Name = "btnRetrieveData";
      this.btnRetrieveData.UseVisualStyleBackColor = true;
      this.btnRetrieveData.Click += new EventHandler(this.btnRetrieveData_Click);
      componentResourceManager.ApplyResources((object) this.txtPassword, "txtPassword");
      this.txtPassword.Name = "txtPassword";
      componentResourceManager.ApplyResources((object) this.btnResetPassword, "btnResetPassword");
      this.btnResetPassword.Name = "btnResetPassword";
      this.btnResetPassword.UseVisualStyleBackColor = true;
      this.btnResetPassword.Click += new EventHandler(this.btnResetPassword_Click);
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      this.dgRetrieve.AllowUserToAddRows = false;
      this.dgRetrieve.AllowUserToDeleteRows = false;
      this.dgRetrieve.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgRetrieve.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgRetrieve.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgRetrieve.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgRetrieve, "dgRetrieve");
      this.dgRetrieve.EnableHeadersVisualStyles = false;
      this.dgRetrieve.Name = "dgRetrieve";
      this.dgRetrieve.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = SystemColors.Control;
      gridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.ForeColor = SystemColors.WindowText;
      gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.dgRetrieve.RowHeadersDefaultCellStyle = gridViewCellStyle3;
      this.dgRetrieve.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.tableLayoutPanel1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.tableLayoutPanel2);
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel2, "tableLayoutPanel2");
      this.tableLayoutPanel2.Controls.Add((Control) this.dgRetrieveLog, 0, 1);
      this.tableLayoutPanel2.Controls.Add((Control) this.lblRetrievedData, 0, 0);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.dgRetrieveLog.AllowUserToAddRows = false;
      this.dgRetrieveLog.AllowUserToDeleteRows = false;
      this.dgRetrieveLog.AllowUserToResizeRows = false;
      this.dgRetrieveLog.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle4.BackColor = SystemColors.Control;
      gridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle4.ForeColor = SystemColors.WindowText;
      gridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle4.WrapMode = DataGridViewTriState.True;
      this.dgRetrieveLog.ColumnHeadersDefaultCellStyle = gridViewCellStyle4;
      this.dgRetrieveLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgRetrieveLog.Columns.AddRange((DataGridViewColumn) this.dcMobileKey, (DataGridViewColumn) this.dcDateTime, (DataGridViewColumn) this.dcDescription);
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle5.BackColor = SystemColors.Window;
      gridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle5.ForeColor = SystemColors.ControlText;
      gridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle5.WrapMode = DataGridViewTriState.False;
      this.dgRetrieveLog.DefaultCellStyle = gridViewCellStyle5;
      componentResourceManager.ApplyResources((object) this.dgRetrieveLog, "dgRetrieveLog");
      this.dgRetrieveLog.EditMode = DataGridViewEditMode.EditProgrammatically;
      this.dgRetrieveLog.EnableHeadersVisualStyles = false;
      this.dgRetrieveLog.MultiSelect = false;
      this.dgRetrieveLog.Name = "dgRetrieveLog";
      this.dgRetrieveLog.ReadOnly = true;
      this.dgRetrieveLog.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle6.BackColor = SystemColors.Control;
      gridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle6.ForeColor = SystemColors.WindowText;
      gridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle6.WrapMode = DataGridViewTriState.True;
      this.dgRetrieveLog.RowHeadersDefaultCellStyle = gridViewCellStyle6;
      this.dgRetrieveLog.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgRetrieveLog.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgRetrieveLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgRetrieveLog.VirtualMode = true;
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
      componentResourceManager.ApplyResources((object) this.lblRetrievedData, "lblRetrievedData");
      this.lblRetrievedData.Name = "lblRetrievedData";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.splitContainer1);
      this.Name = nameof (MobileRetrieveDataForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.splitContainer1, 0);
      ((ISupportInitialize) this.ep).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.pnlRetrieveList.ResumeLayout(false);
      this.pnlRetrieveList.PerformLayout();
      ((ISupportInitialize) this.dgRetrieve).EndInit();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      ((ISupportInitialize) this.dgRetrieveLog).EndInit();
      this.ResumeLayout(false);
    }
  }
}
