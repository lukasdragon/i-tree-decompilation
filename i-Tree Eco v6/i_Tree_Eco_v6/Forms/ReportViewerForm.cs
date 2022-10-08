// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ReportViewerForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using C1.Win.C1Preview;
using DaveyTree.Controls.Extensions;
using Eco.Domain.v6;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Properties;
using i_Tree_Eco_v6.Reports;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ReportViewerForm : ReportContentForm
  {
    private readonly object m_syncobj;
    private ProgramSession m_ps;
    private Year m_year;
    private C1PrintDocument m_curDocument;
    private Report m_Report;
    private short largeCount = 10000;
    private ISession m_sesion;
    internal Form frmProgress;
    private IContainer components;
    private C1PrintPreviewControl ReportView;
    private Label lblOutOfDateWarning;
    private TableLayoutPanel pnlWarning;
    private PictureBox pictureBox1;

    public ReportViewerForm()
    {
      this.InitializeComponent();
      this.ReportView.PreviewPane.DoEvents = false;
      this.ReportView.ToolBars.File.Open.Visible = false;
      this.ReportView.ToolBars.File.Reflow.Visible = false;
      this.m_ps = Program.Session;
      this.m_sesion = this.m_ps.InputSession.CreateSession();
      this.m_syncobj = new object();
      this.Init();
    }

    private void Init()
    {
      TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.GetYear(this.m_ps.InputSession.YearKey.Value, taskScheduler).ContinueWith((Action<Task>) (t => this.ToggleBanner()), taskScheduler);
    }

    public Report Report
    {
      get => this.m_Report;
      set
      {
        this.m_Report = value;
        if (value != null)
        {
          this.Text = this.m_Report.ReportTitle;
          if (!string.IsNullOrEmpty(this.m_Report.HelpTopic))
            this.OnShowHelp(this.m_Report.HelpTopic);
        }
        this.RefreshReport();
      }
    }

    public string Title
    {
      get => this.Text;
      set => this.Text = value;
    }

    private void Year_Updated(object sender, EntityUpdated<Year> e)
    {
      if (this.m_year == null || !(e.Guid == this.m_year.Guid))
        return;
      TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.GetYear(e.Guid, taskScheduler).ContinueWith((Action<Task>) (t =>
      {
        if (this.m_year.Revision == 0)
        {
          this.Close();
        }
        else
        {
          this.ToggleBanner();
          this.RefreshReport();
        }
      }), taskScheduler);
    }

    private Task GetYear(Guid g, TaskScheduler context) => Task.Factory.StartNew<Year>((Func<Year>) (() => this.m_sesion.Get<Year>((object) this.m_ps.InputSession.YearKey)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((Action<Task<Year>>) (t =>
    {
      if (this.IsDisposed)
        return;
      Year result = t.Result;
      lock (this.m_syncobj)
        this.m_year = result;
    }), context);

    private void ToggleBanner()
    {
      using (ISession session = this.m_ps.InputSession.CreateSession())
      {
        using (session.BeginTransaction())
        {
          Year year = session.Get<Year>((object) this.m_ps.InputSession.YearKey);
          int num = session.CreateCriteria<YearResult>().SetProjection((IProjection) Projections.Max("RevProcessed")).Add((ICriterion) Restrictions.Eq("Year", (object) year)).UniqueResult<int>();
          this.pnlWarning.Visible = year.RevProcessed > 0 && (year.Changed || year.RevProcessed != num);
        }
      }
    }

    public async void RefreshReport()
    {
      ReportViewerForm reportViewerForm = this;
      if (reportViewerForm.IsDisposed)
        ;
      else if (reportViewerForm.m_Report == null)
        ;
      else
      {
        reportViewerForm.Visible = false;
        using (Graphics g = reportViewerForm.CreateGraphics())
        {
          try
          {
            reportViewerForm.frmProgress = (Form) new ReportProgress();
            reportViewerForm.frmProgress.ShowWithParentFormLock(reportViewerForm.ParentForm);
            if (reportViewerForm.IsLargeExporableReport())
            {
              switch (MessageBox.Show((IWin32Window) reportViewerForm, i_Tree_Eco_v6.Resources.Strings.WarnLargeReport, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
              {
                case DialogResult.Yes:
                  C1PrintDocument c1PrintDocument1 = await reportViewerForm.RenderReport(g);
                  reportViewerForm.m_curDocument = c1PrintDocument1;
                  break;
                case DialogResult.No:
                  string csvFileName = reportViewerForm.CreateExportFile();
                  if (csvFileName != string.Empty)
                  {
                    C1PrintDocument c1PrintDocument2 = await Task.Factory.StartNew<C1PrintDocument>((Func<C1PrintDocument>) (() => this.m_Report.ExportCSV(g, csvFileName)), CancellationToken.None, TaskCreationOptions.None, reportViewerForm.m_ps.Scheduler);
                    reportViewerForm.m_curDocument = c1PrintDocument2;
                    break;
                  }
                  break;
                default:
                  ((MainRibbonForm) Application.OpenForms["MainRibbonForm"]).showTabSplash();
                  break;
              }
            }
            else
            {
              C1PrintDocument c1PrintDocument3 = await reportViewerForm.RenderReport(g);
              reportViewerForm.m_curDocument = c1PrintDocument3;
            }
          }
          catch (Exception ex)
          {
            reportViewerForm.m_curDocument = reportViewerForm.CreateErrorReport(g, ex, true);
          }
          if (!reportViewerForm.IsDisposed)
          {
            reportViewerForm.Visible = true;
            reportViewerForm.ReportView.PreviewPane.Busy = false;
            reportViewerForm.ReportView.PreviewPane.ZoomMode = ZoomModeEnum.PageWidth;
            reportViewerForm.ReportView.Document = (object) reportViewerForm.m_curDocument;
            reportViewerForm.ShowWaitCursor(false);
          }
          reportViewerForm.frmProgress.Close();
          reportViewerForm.frmProgress.Dispose();
        }
        reportViewerForm.OnRequestRefresh();
      }
    }

    protected bool IsLargeExporableReport()
    {
      this.ShowWaitCursor(true);
      int num = !this.m_Report.CanExport(ExportFormat.CSV) ? 0 : (this.m_Report.GetData().Rows.Count > (int) this.largeCount ? 1 : 0);
      this.ShowWaitCursor(false);
      return num != 0;
    }

    protected Task<C1PrintDocument> RenderReport(Graphics g)
    {
      this.ShowWaitCursor(true);
      return Task.Factory.StartNew<C1PrintDocument>((Func<C1PrintDocument>) (() => this.m_Report.GenerateReport(g)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler);
    }

    protected string CreateExportFile()
    {
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        saveFileDialog.Title = i_Tree_Eco_v6.Resources.Strings.SaveReportAs;
        saveFileDialog.Filter = string.Join("|", new string[2]
        {
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterCSV, (object) Settings.Default.ExtCSV),
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
        });
        saveFileDialog.CheckPathExists = true;
        saveFileDialog.OverwritePrompt = true;
        saveFileDialog.AddExtension = true;
        saveFileDialog.DefaultExt = Settings.Default.ExtCSV.Replace("*.", "");
        saveFileDialog.ShowHelp = false;
        saveFileDialog.CreatePrompt = false;
        if (saveFileDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          return saveFileDialog.FileName;
        this.OnRequestRefresh();
        return string.Empty;
      }
    }

    private C1PrintDocument CreateErrorReport(
      Graphics g,
      Exception ex,
      bool isExport)
    {
      ErrorReport errorReport = new ErrorReport();
      errorReport.ReportTitle = this.Title;
      errorReport.ExceptionMsg = ex.Message;
      if (ex.InnerException != null)
        errorReport.InnerExceptionMsg = ex.InnerException.Message;
      errorReport.StackTrace = ex.StackTrace;
      errorReport.IsExport = isExport;
      this.m_Report = (Report) errorReport;
      return this.m_Report.GenerateReport(g);
    }

    public void Print() => this.ReportView.PreviewPane.Print();

    private void ReportViewerForm_Load(object sender, EventArgs e) => EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));

    public void AppBusy(Form f, string m)
    {
      this.ShowWaitCursor(true);
      this.frmProgress = (Form) new ReportProgress(m);
      this.frmProgress.ShowWithParentFormLock(f);
    }

    public void AppReady()
    {
      this.ShowWaitCursor(false);
      if (this.frmProgress == null)
        return;
      this.frmProgress.Close();
      this.frmProgress.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ReportViewerForm));
      this.ReportView = new C1PrintPreviewControl();
      this.lblOutOfDateWarning = new Label();
      this.pnlWarning = new TableLayoutPanel();
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.ReportView).BeginInit();
      ((ISupportInitialize) this.ReportView.PreviewPane).BeginInit();
      this.ReportView.SuspendLayout();
      this.pnlWarning.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.ReportView.AvailablePreviewActions = C1PreviewActionFlags.Zoom | C1PreviewActionFlags.PageView | C1PreviewActionFlags.Text | C1PreviewActionFlags.FileSave | C1PreviewActionFlags.PageSetup | C1PreviewActionFlags.Print | C1PreviewActionFlags.GoFirst | C1PreviewActionFlags.GoPrev | C1PreviewActionFlags.GoNext | C1PreviewActionFlags.GoLast | C1PreviewActionFlags.GoPage | C1PreviewActionFlags.HistoryNext | C1PreviewActionFlags.HistoryPrev;
      componentResourceManager.ApplyResources((object) this.ReportView, "ReportView");
      this.ReportView.Name = "ReportView";
      this.ReportView.NavigationPanelVisible = false;
      this.ReportView.PreviewPane.ExportOptions.Content = new ExporterOptions[15]
      {
        new ExporterOptions("C1dExportProvider", "C1.C1Preview.Export.C1dOptionsForm", false, true, false),
        new ExporterOptions("EmfExportProvider", "C1.C1Preview.Export.EmfOptionsForm", false, true, true),
        new ExporterOptions("TiffExportProvider", "C1.C1Preview.Export.ImagesOptionsForm", false, true, true),
        new ExporterOptions("PngExportProvider", "C1.C1Preview.Export.ImagesOptionsForm", false, true, true),
        new ExporterOptions("JpegExportProvider", "C1.C1Preview.Export.ImagesOptionsForm", false, true, true),
        new ExporterOptions("GifExportProvider", "C1.C1Preview.Export.ImagesOptionsForm", false, true, true),
        new ExporterOptions("BmpExportProvider", "C1.C1Preview.Export.ImagesOptionsForm", false, true, true),
        new ExporterOptions("HtmlExportProvider", "C1.C1Preview.Export.HtmlOptionsForm", false, true, true),
        new ExporterOptions("XpsExportProvider", "C1.C1Preview.Export.DefaultExportOptionsForm", false, true, true),
        new ExporterOptions("C1dxExportProvider", "C1.C1Preview.Export.C1dOptionsForm", false, true, false),
        new ExporterOptions("C1dbExportProvider", "C1.C1Preview.Export.C1dOptionsForm", false, true, false),
        new ExporterOptions("C1mdxExportProvider", "C1.C1Preview.Export.C1mdxOptionsForm", false, true, false),
        new ExporterOptions("ReportHTMLExportProvider", "C1.C1Preview.Export.ReportHtmlOptionsForm", false, true, true),
        new ExporterOptions("ReportTextExportProvider", "C1.C1Preview.Export.ReportTextOptionsForm", false, true, true),
        new ExporterOptions("ReportMetafileExportProvider", "C1.C1Preview.Export.DefaultExportOptionsForm", false, true, true)
      };
      this.ReportView.PreviewPane.HideMargins = HideMarginsFlags.None;
      this.ReportView.PreviewPane.IntegrateExternalTools = true;
      componentResourceManager.ApplyResources((object) this.ReportView.PreviewPane, "ReportView.PreviewPane");
      this.ReportView.PreviewPane.ZoomMode = ZoomModeEnum.PageWidth;
      this.ReportView.ToolBars.File.Open.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.File.Open.Image");
      this.ReportView.ToolBars.File.Open.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.File.Open.ImageTransparentColor");
      this.ReportView.ToolBars.File.Open.Name = "btnFileOpen";
      this.ReportView.ToolBars.File.Open.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.File.Open.Size");
      this.ReportView.ToolBars.File.Open.Tag = (object) "C1PreviewActionEnum.FileOpen";
      this.ReportView.ToolBars.File.Open.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.File.Open.ToolTipText");
      this.ReportView.ToolBars.File.Open.Visible = (bool) componentResourceManager.GetObject("ReportView.ToolBars.File.Open.Visible");
      this.ReportView.ToolBars.File.PageSetup.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.File.PageSetup.Image");
      this.ReportView.ToolBars.File.PageSetup.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.File.PageSetup.ImageTransparentColor");
      this.ReportView.ToolBars.File.PageSetup.Name = "btnPageSetup";
      this.ReportView.ToolBars.File.PageSetup.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.File.PageSetup.Size");
      this.ReportView.ToolBars.File.PageSetup.Tag = (object) "C1PreviewActionEnum.PageSetup";
      this.ReportView.ToolBars.File.PageSetup.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.File.PageSetup.ToolTipText");
      this.ReportView.ToolBars.File.Parameters.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.File.Parameters.Image");
      this.ReportView.ToolBars.File.Parameters.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.File.Parameters.ImageTransparentColor");
      this.ReportView.ToolBars.File.Parameters.Name = "btnParameters";
      this.ReportView.ToolBars.File.Parameters.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.File.Parameters.Size");
      this.ReportView.ToolBars.File.Parameters.Tag = (object) "C1PreviewActionEnum.Parameters";
      this.ReportView.ToolBars.File.Parameters.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.File.Parameters.ToolTipText");
      this.ReportView.ToolBars.File.Parameters.Visible = (bool) componentResourceManager.GetObject("ReportView.ToolBars.File.Parameters.Visible");
      this.ReportView.ToolBars.File.Print.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.File.Print.Image");
      this.ReportView.ToolBars.File.Print.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.File.Print.ImageTransparentColor");
      this.ReportView.ToolBars.File.Print.Name = "btnPrint";
      this.ReportView.ToolBars.File.Print.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.File.Print.Size");
      this.ReportView.ToolBars.File.Print.Tag = (object) "C1PreviewActionEnum.Print";
      this.ReportView.ToolBars.File.Print.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.File.Print.ToolTipText");
      this.ReportView.ToolBars.File.PrintLayout.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.File.PrintLayout.Image");
      this.ReportView.ToolBars.File.PrintLayout.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.File.PrintLayout.ImageTransparentColor");
      this.ReportView.ToolBars.File.PrintLayout.Name = "btnPrintLayout";
      this.ReportView.ToolBars.File.PrintLayout.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.File.PrintLayout.Size");
      this.ReportView.ToolBars.File.PrintLayout.Tag = (object) "C1PreviewActionEnum.PrintLayout";
      this.ReportView.ToolBars.File.PrintLayout.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.File.PrintLayout.ToolTipText");
      this.ReportView.ToolBars.File.PrintLayout.Visible = (bool) componentResourceManager.GetObject("ReportView.ToolBars.File.PrintLayout.Visible");
      this.ReportView.ToolBars.File.Reflow.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.File.Reflow.Image");
      this.ReportView.ToolBars.File.Reflow.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.File.Reflow.ImageTransparentColor");
      this.ReportView.ToolBars.File.Reflow.Name = "btnReflow";
      this.ReportView.ToolBars.File.Reflow.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.File.Reflow.Size");
      this.ReportView.ToolBars.File.Reflow.Tag = (object) "C1PreviewActionEnum.Reflow";
      this.ReportView.ToolBars.File.Reflow.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.File.Reflow.ToolTipText");
      this.ReportView.ToolBars.File.Reflow.Visible = (bool) componentResourceManager.GetObject("ReportView.ToolBars.File.Reflow.Visible");
      this.ReportView.ToolBars.File.Save.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.File.Save.Image");
      this.ReportView.ToolBars.File.Save.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.File.Save.ImageTransparentColor");
      this.ReportView.ToolBars.File.Save.Name = "btnFileSave";
      this.ReportView.ToolBars.File.Save.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.File.Save.Size");
      this.ReportView.ToolBars.File.Save.Tag = (object) "C1PreviewActionEnum.FileSave";
      this.ReportView.ToolBars.File.Save.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.File.Save.ToolTipText");
      this.ReportView.ToolBars.File.Stop.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.File.Stop.Image");
      this.ReportView.ToolBars.File.Stop.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.File.Stop.ImageTransparentColor");
      this.ReportView.ToolBars.File.Stop.Name = "btnStop";
      this.ReportView.ToolBars.File.Stop.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.File.Stop.Size");
      this.ReportView.ToolBars.File.Stop.Tag = (object) "C1PreviewActionEnum.Stop";
      this.ReportView.ToolBars.File.Stop.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.File.Stop.ToolTipText");
      this.ReportView.ToolBars.File.Stop.Visible = (bool) componentResourceManager.GetObject("ReportView.ToolBars.File.Stop.Visible");
      this.ReportView.ToolBars.Navigation.GoFirst.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.GoFirst.Image");
      this.ReportView.ToolBars.Navigation.GoFirst.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.GoFirst.ImageTransparentColor");
      this.ReportView.ToolBars.Navigation.GoFirst.Name = "btnGoFirst";
      this.ReportView.ToolBars.Navigation.GoFirst.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.GoFirst.Size");
      this.ReportView.ToolBars.Navigation.GoFirst.Tag = (object) "C1PreviewActionEnum.GoFirst";
      this.ReportView.ToolBars.Navigation.GoFirst.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Navigation.GoFirst.ToolTipText");
      this.ReportView.ToolBars.Navigation.GoLast.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.GoLast.Image");
      this.ReportView.ToolBars.Navigation.GoLast.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.GoLast.ImageTransparentColor");
      this.ReportView.ToolBars.Navigation.GoLast.Name = "btnGoLast";
      this.ReportView.ToolBars.Navigation.GoLast.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.GoLast.Size");
      this.ReportView.ToolBars.Navigation.GoLast.Tag = (object) "C1PreviewActionEnum.GoLast";
      this.ReportView.ToolBars.Navigation.GoLast.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Navigation.GoLast.ToolTipText");
      this.ReportView.ToolBars.Navigation.GoNext.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.GoNext.Image");
      this.ReportView.ToolBars.Navigation.GoNext.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.GoNext.ImageTransparentColor");
      this.ReportView.ToolBars.Navigation.GoNext.Name = "btnGoNext";
      this.ReportView.ToolBars.Navigation.GoNext.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.GoNext.Size");
      this.ReportView.ToolBars.Navigation.GoNext.Tag = (object) "C1PreviewActionEnum.GoNext";
      this.ReportView.ToolBars.Navigation.GoNext.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Navigation.GoNext.ToolTipText");
      this.ReportView.ToolBars.Navigation.GoPrev.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.GoPrev.Image");
      this.ReportView.ToolBars.Navigation.GoPrev.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.GoPrev.ImageTransparentColor");
      this.ReportView.ToolBars.Navigation.GoPrev.Name = "btnGoPrev";
      this.ReportView.ToolBars.Navigation.GoPrev.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.GoPrev.Size");
      this.ReportView.ToolBars.Navigation.GoPrev.Tag = (object) "C1PreviewActionEnum.GoPrev";
      this.ReportView.ToolBars.Navigation.GoPrev.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Navigation.GoPrev.ToolTipText");
      this.ReportView.ToolBars.Navigation.HistoryNext.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.HistoryNext.Image");
      this.ReportView.ToolBars.Navigation.HistoryNext.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.HistoryNext.ImageTransparentColor");
      this.ReportView.ToolBars.Navigation.HistoryNext.Name = "btnHistoryNext";
      this.ReportView.ToolBars.Navigation.HistoryNext.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.HistoryNext.Size");
      this.ReportView.ToolBars.Navigation.HistoryNext.Tag = (object) "C1PreviewActionEnum.HistoryNext";
      this.ReportView.ToolBars.Navigation.HistoryNext.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Navigation.HistoryNext.ToolTipText");
      this.ReportView.ToolBars.Navigation.HistoryPrev.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.HistoryPrev.Image");
      this.ReportView.ToolBars.Navigation.HistoryPrev.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.HistoryPrev.ImageTransparentColor");
      this.ReportView.ToolBars.Navigation.HistoryPrev.Name = "btnHistoryPrev";
      this.ReportView.ToolBars.Navigation.HistoryPrev.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.HistoryPrev.Size");
      this.ReportView.ToolBars.Navigation.HistoryPrev.Tag = (object) "C1PreviewActionEnum.HistoryPrev";
      this.ReportView.ToolBars.Navigation.HistoryPrev.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Navigation.HistoryPrev.ToolTipText");
      this.ReportView.ToolBars.Navigation.LblOfPages.Name = "lblOfPages";
      this.ReportView.ToolBars.Navigation.LblOfPages.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.LblOfPages.Size");
      this.ReportView.ToolBars.Navigation.LblOfPages.Tag = (object) "C1PreviewActionEnum.GoPageCount";
      this.ReportView.ToolBars.Navigation.LblOfPages.Text = componentResourceManager.GetString("ReportView.ToolBars.Navigation.LblOfPages.Text");
      this.ReportView.ToolBars.Navigation.LblPage.Name = "lblPage";
      this.ReportView.ToolBars.Navigation.LblPage.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.LblPage.Size");
      this.ReportView.ToolBars.Navigation.LblPage.Tag = (object) "C1PreviewActionEnum.GoPageLabel";
      this.ReportView.ToolBars.Navigation.LblPage.Text = componentResourceManager.GetString("ReportView.ToolBars.Navigation.LblPage.Text");
      this.ReportView.ToolBars.Navigation.NavigationPane.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.NavigationPane.Image");
      this.ReportView.ToolBars.Navigation.NavigationPane.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.NavigationPane.ImageTransparentColor");
      this.ReportView.ToolBars.Navigation.NavigationPane.Name = "btnNavigationPane";
      this.ReportView.ToolBars.Navigation.NavigationPane.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.NavigationPane.Size");
      this.ReportView.ToolBars.Navigation.NavigationPane.Tag = (object) "C1PreviewActionEnum.NavigationPane";
      this.ReportView.ToolBars.Navigation.NavigationPane.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Navigation.NavigationPane.ToolTipText");
      this.ReportView.ToolBars.Navigation.NavigationPane.Visible = (bool) componentResourceManager.GetObject("ReportView.ToolBars.Navigation.NavigationPane.Visible");
      this.ReportView.ToolBars.Navigation.ToolTipPageNo = (string) null;
      this.ReportView.ToolBars.Page.Continuous.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Page.Continuous.Image");
      this.ReportView.ToolBars.Page.Continuous.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Page.Continuous.ImageTransparentColor");
      this.ReportView.ToolBars.Page.Continuous.Name = "btnPageContinuous";
      this.ReportView.ToolBars.Page.Continuous.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Page.Continuous.Size");
      this.ReportView.ToolBars.Page.Continuous.Tag = (object) "C1PreviewActionEnum.PageContinuous";
      this.ReportView.ToolBars.Page.Continuous.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Page.Continuous.ToolTipText");
      this.ReportView.ToolBars.Page.Facing.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Page.Facing.Image");
      this.ReportView.ToolBars.Page.Facing.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Page.Facing.ImageTransparentColor");
      this.ReportView.ToolBars.Page.Facing.Name = "btnPageFacing";
      this.ReportView.ToolBars.Page.Facing.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Page.Facing.Size");
      this.ReportView.ToolBars.Page.Facing.Tag = (object) "C1PreviewActionEnum.PageFacing";
      this.ReportView.ToolBars.Page.Facing.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Page.Facing.ToolTipText");
      this.ReportView.ToolBars.Page.FacingContinuous.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Page.FacingContinuous.Image");
      this.ReportView.ToolBars.Page.FacingContinuous.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Page.FacingContinuous.ImageTransparentColor");
      this.ReportView.ToolBars.Page.FacingContinuous.Name = "btnPageFacingContinuous";
      this.ReportView.ToolBars.Page.FacingContinuous.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Page.FacingContinuous.Size");
      this.ReportView.ToolBars.Page.FacingContinuous.Tag = (object) "C1PreviewActionEnum.PageFacingContinuous";
      this.ReportView.ToolBars.Page.FacingContinuous.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Page.FacingContinuous.ToolTipText");
      this.ReportView.ToolBars.Page.Single.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Page.Single.Image");
      this.ReportView.ToolBars.Page.Single.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Page.Single.ImageTransparentColor");
      this.ReportView.ToolBars.Page.Single.Name = "btnPageSingle";
      this.ReportView.ToolBars.Page.Single.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Page.Single.Size");
      this.ReportView.ToolBars.Page.Single.Tag = (object) "C1PreviewActionEnum.PageSingle";
      this.ReportView.ToolBars.Page.Single.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Page.Single.ToolTipText");
      this.ReportView.ToolBars.Search.CloseSearch.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Search.CloseSearch.Image");
      this.ReportView.ToolBars.Search.CloseSearch.Name = "btnCloseSearch";
      this.ReportView.ToolBars.Search.CloseSearch.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Search.CloseSearch.Size");
      this.ReportView.ToolBars.Search.CloseSearch.Tag = (object) "C1PreviewActionEnum.CloseSearch";
      this.ReportView.ToolBars.Search.CloseSearch.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Search.CloseSearch.ToolTipText");
      this.ReportView.ToolBars.Search.CloseSearch.Visible = (bool) componentResourceManager.GetObject("ReportView.ToolBars.Search.CloseSearch.Visible");
      this.ReportView.ToolBars.Search.MatchCase.Name = "btnMatchCase";
      this.ReportView.ToolBars.Search.MatchCase.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Search.MatchCase.Size");
      this.ReportView.ToolBars.Search.MatchCase.Tag = (object) "C1PreviewActionEnum.MatchCase";
      this.ReportView.ToolBars.Search.MatchCase.Text = componentResourceManager.GetString("ReportView.ToolBars.Search.MatchCase.Text");
      this.ReportView.ToolBars.Search.MatchCase.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Search.MatchCase.ToolTipText");
      this.ReportView.ToolBars.Search.MatchCase.Visible = (bool) componentResourceManager.GetObject("ReportView.ToolBars.Search.MatchCase.Visible");
      this.ReportView.ToolBars.Search.MatchWholeWord.Name = "btnMatchWholeWord";
      this.ReportView.ToolBars.Search.MatchWholeWord.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Search.MatchWholeWord.Size");
      this.ReportView.ToolBars.Search.MatchWholeWord.Tag = (object) "C1PreviewActionEnum.MatchWholeWord";
      this.ReportView.ToolBars.Search.MatchWholeWord.Text = componentResourceManager.GetString("ReportView.ToolBars.Search.MatchWholeWord.Text");
      this.ReportView.ToolBars.Search.MatchWholeWord.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Search.MatchWholeWord.ToolTipText");
      this.ReportView.ToolBars.Search.MatchWholeWord.Visible = (bool) componentResourceManager.GetObject("ReportView.ToolBars.Search.MatchWholeWord.Visible");
      this.ReportView.ToolBars.Search.SearchLabel.Name = "lblSearch";
      this.ReportView.ToolBars.Search.SearchLabel.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Search.SearchLabel.Size");
      this.ReportView.ToolBars.Search.SearchLabel.Tag = (object) "C1PreviewActionEnum.SearchLabel";
      this.ReportView.ToolBars.Search.SearchLabel.Text = componentResourceManager.GetString("ReportView.ToolBars.Search.SearchLabel.Text");
      this.ReportView.ToolBars.Search.SearchLabel.Visible = (bool) componentResourceManager.GetObject("ReportView.ToolBars.Search.SearchLabel.Visible");
      this.ReportView.ToolBars.Search.SearchNext.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Search.SearchNext.Image");
      this.ReportView.ToolBars.Search.SearchNext.Name = "btnSearchNext";
      this.ReportView.ToolBars.Search.SearchNext.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Search.SearchNext.Size");
      this.ReportView.ToolBars.Search.SearchNext.Tag = (object) "C1PreviewActionEnum.SearchNext";
      this.ReportView.ToolBars.Search.SearchNext.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Search.SearchNext.ToolTipText");
      this.ReportView.ToolBars.Search.SearchNext.Visible = (bool) componentResourceManager.GetObject("ReportView.ToolBars.Search.SearchNext.Visible");
      this.ReportView.ToolBars.Search.SearchPrevious.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Search.SearchPrevious.Image");
      this.ReportView.ToolBars.Search.SearchPrevious.Name = "btnSearchPrevious";
      this.ReportView.ToolBars.Search.SearchPrevious.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Search.SearchPrevious.Size");
      this.ReportView.ToolBars.Search.SearchPrevious.Tag = (object) "C1PreviewActionEnum.SearchPrevious";
      this.ReportView.ToolBars.Search.SearchPrevious.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Search.SearchPrevious.ToolTipText");
      this.ReportView.ToolBars.Search.SearchPrevious.Visible = (bool) componentResourceManager.GetObject("ReportView.ToolBars.Search.SearchPrevious.Visible");
      this.ReportView.ToolBars.Search.SearchText.Name = "txtSearchText";
      this.ReportView.ToolBars.Search.SearchText.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Search.SearchText.Size");
      this.ReportView.ToolBars.Search.SearchText.Tag = (object) "C1PreviewActionEnum.SearchText";
      this.ReportView.ToolBars.Search.SearchText.Visible = (bool) componentResourceManager.GetObject("ReportView.ToolBars.Search.SearchText.Visible");
      this.ReportView.ToolBars.Text.Find.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Text.Find.Image");
      this.ReportView.ToolBars.Text.Find.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Text.Find.ImageTransparentColor");
      this.ReportView.ToolBars.Text.Find.Name = "btnFind";
      this.ReportView.ToolBars.Text.Find.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Text.Find.Size");
      this.ReportView.ToolBars.Text.Find.Tag = (object) "C1PreviewActionEnum.Find";
      this.ReportView.ToolBars.Text.Find.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Text.Find.ToolTipText");
      this.ReportView.ToolBars.Text.Hand.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Text.Hand.Image");
      this.ReportView.ToolBars.Text.Hand.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Text.Hand.ImageTransparentColor");
      this.ReportView.ToolBars.Text.Hand.Name = "btnHandTool";
      this.ReportView.ToolBars.Text.Hand.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Text.Hand.Size");
      this.ReportView.ToolBars.Text.Hand.Tag = (object) "C1PreviewActionEnum.HandTool";
      this.ReportView.ToolBars.Text.Hand.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Text.Hand.ToolTipText");
      this.ReportView.ToolBars.Text.SelectText.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Text.SelectText.Image");
      this.ReportView.ToolBars.Text.SelectText.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Text.SelectText.ImageTransparentColor");
      this.ReportView.ToolBars.Text.SelectText.Name = "btnSelectTextTool";
      this.ReportView.ToolBars.Text.SelectText.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Text.SelectText.Size");
      this.ReportView.ToolBars.Text.SelectText.Tag = (object) "C1PreviewActionEnum.SelectTextTool";
      this.ReportView.ToolBars.Text.SelectText.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Text.SelectText.ToolTipText");
      this.ReportView.ToolBars.Zoom.DropZoomFactor.Name = "dropZoomFactor";
      this.ReportView.ToolBars.Zoom.DropZoomFactor.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.DropZoomFactor.Size");
      this.ReportView.ToolBars.Zoom.DropZoomFactor.Tag = (object) "C1PreviewActionEnum.ZoomFactor";
      this.ReportView.ToolBars.Zoom.ToolTipToolZoomIn = (string) null;
      this.ReportView.ToolBars.Zoom.ToolTipToolZoomOut = (string) null;
      this.ReportView.ToolBars.Zoom.ToolTipZoomFactor = (string) null;
      this.ReportView.ToolBars.Zoom.ZoomIn.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomIn.Image");
      this.ReportView.ToolBars.Zoom.ZoomIn.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomIn.ImageTransparentColor");
      this.ReportView.ToolBars.Zoom.ZoomIn.Name = "btnZoomIn";
      this.ReportView.ToolBars.Zoom.ZoomIn.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomIn.Size");
      this.ReportView.ToolBars.Zoom.ZoomIn.Tag = (object) "C1PreviewActionEnum.ZoomIn";
      this.ReportView.ToolBars.Zoom.ZoomIn.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Zoom.ZoomIn.ToolTipText");
      this.ReportView.ToolBars.Zoom.ZoomInTool.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomInTool.Image");
      this.ReportView.ToolBars.Zoom.ZoomInTool.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomInTool.ImageTransparentColor");
      this.ReportView.ToolBars.Zoom.ZoomInTool.Name = "itemZoomInTool";
      this.ReportView.ToolBars.Zoom.ZoomInTool.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomInTool.Size");
      this.ReportView.ToolBars.Zoom.ZoomInTool.Tag = (object) "C1PreviewActionEnum.ZoomInTool";
      this.ReportView.ToolBars.Zoom.ZoomInTool.Text = componentResourceManager.GetString("ReportView.ToolBars.Zoom.ZoomInTool.Text");
      this.ReportView.ToolBars.Zoom.ZoomOut.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomOut.Image");
      this.ReportView.ToolBars.Zoom.ZoomOut.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomOut.ImageTransparentColor");
      this.ReportView.ToolBars.Zoom.ZoomOut.Name = "btnZoomOut";
      this.ReportView.ToolBars.Zoom.ZoomOut.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomOut.Size");
      this.ReportView.ToolBars.Zoom.ZoomOut.Tag = (object) "C1PreviewActionEnum.ZoomOut";
      this.ReportView.ToolBars.Zoom.ZoomOut.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Zoom.ZoomOut.ToolTipText");
      this.ReportView.ToolBars.Zoom.ZoomOutTool.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomOutTool.Image");
      this.ReportView.ToolBars.Zoom.ZoomOutTool.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomOutTool.ImageTransparentColor");
      this.ReportView.ToolBars.Zoom.ZoomOutTool.Name = "itemZoomOutTool";
      this.ReportView.ToolBars.Zoom.ZoomOutTool.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomOutTool.Size");
      this.ReportView.ToolBars.Zoom.ZoomOutTool.Tag = (object) "C1PreviewActionEnum.ZoomOutTool";
      this.ReportView.ToolBars.Zoom.ZoomOutTool.Text = componentResourceManager.GetString("ReportView.ToolBars.Zoom.ZoomOutTool.Text");
      this.ReportView.ToolBars.Zoom.ZoomTool.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.ReportView.ToolBars.Zoom.ZoomInTool,
        (ToolStripItem) this.ReportView.ToolBars.Zoom.ZoomOutTool
      });
      this.ReportView.ToolBars.Zoom.ZoomTool.Image = (Image) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomTool.Image");
      this.ReportView.ToolBars.Zoom.ZoomTool.ImageTransparentColor = (Color) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomTool.ImageTransparentColor");
      this.ReportView.ToolBars.Zoom.ZoomTool.Name = "btnZoomTool";
      this.ReportView.ToolBars.Zoom.ZoomTool.Size = (Size) componentResourceManager.GetObject("ReportView.ToolBars.Zoom.ZoomTool.Size");
      this.ReportView.ToolBars.Zoom.ZoomTool.Tag = (object) "C1PreviewActionEnum.ZoomInTool";
      this.ReportView.ToolBars.Zoom.ZoomTool.ToolTipText = componentResourceManager.GetString("ReportView.ToolBars.Zoom.ZoomTool.ToolTipText");
      componentResourceManager.ApplyResources((object) this.lblOutOfDateWarning, "lblOutOfDateWarning");
      this.lblOutOfDateWarning.BackColor = Color.Transparent;
      this.lblOutOfDateWarning.ForeColor = Color.White;
      this.lblOutOfDateWarning.Name = "lblOutOfDateWarning";
      componentResourceManager.ApplyResources((object) this.pnlWarning, "pnlWarning");
      this.pnlWarning.BackColor = Color.Red;
      this.pnlWarning.Controls.Add((Control) this.lblOutOfDateWarning, 1, 0);
      this.pnlWarning.Controls.Add((Control) this.pictureBox1, 0, 0);
      this.pnlWarning.Name = "pnlWarning";
      componentResourceManager.ApplyResources((object) this.pictureBox1, "pictureBox1");
      this.pictureBox1.Image = (Image) i_Tree_Eco_v6.Properties.Resources.Warning_Large;
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.TabStop = false;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.ReportView);
      this.Controls.Add((Control) this.pnlWarning);
      this.Name = nameof (ReportViewerForm);
      this.Load += new EventHandler(this.ReportViewerForm_Load);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.pnlWarning, 0);
      this.Controls.SetChildIndex((Control) this.ReportView, 0);
      ((ISupportInitialize) this.ReportView.PreviewPane).EndInit();
      ((ISupportInitialize) this.ReportView).EndInit();
      this.ReportView.ResumeLayout(false);
      this.ReportView.PerformLayout();
      this.pnlWarning.ResumeLayout(false);
      this.pnlWarning.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
