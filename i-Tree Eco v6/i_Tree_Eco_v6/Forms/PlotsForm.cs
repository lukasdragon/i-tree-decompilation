// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.PlotsForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Util;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Interfaces;
using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class PlotsForm : DataContentForm, IActionable, IExportable
  {
    private PlotsContentForm m_frmPlots;
    private PlotTreesForm m_frmTrees;
    private PlotShrubsForm m_frmShrubs;
    private PlotGroundCoversForm m_frmGroundCovers;
    private PlotLandUsesForm m_frmLandUses;
    private PlotReferenceObjectsForm m_frmRefObjects;
    private PlotPlantingSitesForm m_frmPlantingSites;
    private ContentForm m_activeContent;
    private ContentForm m_inactiveContent;
    private IContainer components;
    private DockPanel ContentPanel;

    public PlotsForm()
    {
      this.InitializeComponent();
      this.m_frmPlots = new PlotsContentForm();
      this.m_frmPlots.RequestRefresh += new EventHandler<EventArgs>(this.ContentForm_RequestRefresh);
      this.m_frmPlots.Enter += new EventHandler(this.ContentForm_Enter);
      this.m_frmPlots.Leave += new EventHandler(this.ContentForm_Leave);
      Program.Session.InputSessionChanged += new EventHandler(this.ProgramSession_InputSessionChanged);
      Program.Session.InputSession.YearChanged += new EventHandler(this.InputSession_YearChanged);
    }

    protected override void OnDataLoaded()
    {
      base.OnDataLoaded();
      this.ShowPlots();
      if (this.Year.RecordReferenceObjects)
        this.ShowReferenceObjects();
      else if (this.m_frmRefObjects != null)
        this.m_frmRefObjects.Close();
      if (this.Year.RecordGroundCover)
        this.ShowGroundCovers();
      else if (this.m_frmGroundCovers != null)
        this.m_frmGroundCovers.Close();
      if (this.Year.RecordLanduse)
        this.ShowLandUses();
      else if (this.m_frmLandUses != null)
        this.m_frmLandUses.Close();
      this.ShowTrees();
      if (this.Year.RecordShrub)
        this.ShowShrubs();
      else if (this.m_frmShrubs != null)
        this.m_frmShrubs.Close();
      if (this.Year.RecordReferenceObjects)
        this.m_frmRefObjects.Activate();
      else if (this.Year.RecordGroundCover)
        this.m_frmGroundCovers.Activate();
      else if (this.Year.RecordLanduse)
        this.m_frmLandUses.Activate();
      else
        this.m_frmTrees.Activate();
      this.m_frmPlots.Activate();
    }

    public bool CanPerformAction(UserActions action) => this.m_activeContent is IActionable activeContent && activeContent.CanPerformAction(action);

    public void PerformAction(UserActions action)
    {
      if (!(this.m_activeContent is IActionable activeContent))
        return;
      activeContent.PerformAction(action);
    }

    private void InputSession_YearChanged(object sender, EventArgs e) => this.Close();

    private void ProgramSession_InputSessionChanged(object sender, EventArgs e) => this.Close();

    private void ShowPlots() => this.m_frmPlots.Show(this.ContentPanel, DockState.Document);

    public void ShowTrees()
    {
      if (this.m_frmTrees == null)
      {
        this.m_frmTrees = new PlotTreesForm();
        this.m_frmTrees.Enter += new EventHandler(this.ContentForm_Enter);
        this.m_frmTrees.Leave += new EventHandler(this.ContentForm_Leave);
        this.m_frmTrees.RequestRefresh += new EventHandler<EventArgs>(this.ContentForm_RequestRefresh);
        this.m_frmTrees.Disposed += new EventHandler(this.m_frmTrees_Disposed);
        this.m_frmPlots.PlotSelectionChanged += new EventHandler<PlotEventArgs>(this.m_frmTrees.OnPlotSelectionChanged);
      }
      this.m_frmTrees.Show(this.ContentPanel, DockState.DockBottom);
    }

    private void m_frmTrees_Disposed(object sender, EventArgs e)
    {
      this.m_frmPlots.PlotSelectionChanged -= new EventHandler<PlotEventArgs>(this.m_frmTrees.OnPlotSelectionChanged);
      this.m_frmTrees = (PlotTreesForm) null;
    }

    public void ShowReferenceObjects()
    {
      if (this.m_frmRefObjects == null)
      {
        this.m_frmRefObjects = new PlotReferenceObjectsForm();
        this.m_frmRefObjects.Enter += new EventHandler(this.ContentForm_Enter);
        this.m_frmRefObjects.Leave += new EventHandler(this.ContentForm_Leave);
        this.m_frmRefObjects.RequestRefresh += new EventHandler<EventArgs>(this.ContentForm_RequestRefresh);
        this.m_frmRefObjects.Disposed += new EventHandler(this.m_frmRefObjects_Disposed);
        this.m_frmPlots.PlotSelectionChanged += new EventHandler<PlotEventArgs>(this.m_frmRefObjects.OnPlotSelectionChanged);
      }
      this.m_frmRefObjects.Show(this.ContentPanel, DockState.DockBottom);
    }

    private void m_frmRefObjects_Disposed(object sender, EventArgs e)
    {
      this.m_frmPlots.PlotSelectionChanged -= new EventHandler<PlotEventArgs>(this.m_frmRefObjects.OnPlotSelectionChanged);
      this.m_frmRefObjects = (PlotReferenceObjectsForm) null;
    }

    public void ShowShrubs()
    {
      if (this.m_frmShrubs == null)
      {
        this.m_frmShrubs = new PlotShrubsForm();
        this.m_frmShrubs.Enter += new EventHandler(this.ContentForm_Enter);
        this.m_frmShrubs.Leave += new EventHandler(this.ContentForm_Leave);
        this.m_frmShrubs.RequestRefresh += new EventHandler<EventArgs>(this.ContentForm_RequestRefresh);
        this.m_frmShrubs.Disposed += new EventHandler(this.m_frmShrubs_Disposed);
        this.m_frmPlots.PlotSelectionChanged += new EventHandler<PlotEventArgs>(this.m_frmShrubs.OnPlotSelectionChanged);
      }
      this.m_frmShrubs.Show(this.ContentPanel, DockState.DockBottom);
    }

    private void m_frmShrubs_Disposed(object sender, EventArgs e)
    {
      this.m_frmPlots.PlotSelectionChanged -= new EventHandler<PlotEventArgs>(this.m_frmShrubs.OnPlotSelectionChanged);
      this.m_frmShrubs = (PlotShrubsForm) null;
    }

    public void ShowGroundCovers()
    {
      if (this.m_frmGroundCovers == null)
      {
        this.m_frmGroundCovers = new PlotGroundCoversForm();
        this.m_frmGroundCovers.Enter += new EventHandler(this.ContentForm_Enter);
        this.m_frmGroundCovers.Leave += new EventHandler(this.ContentForm_Leave);
        this.m_frmGroundCovers.RequestRefresh += new EventHandler<EventArgs>(this.ContentForm_RequestRefresh);
        this.m_frmGroundCovers.Disposed += new EventHandler(this.m_frmGroundCovers_Disposed);
        this.m_frmPlots.PlotSelectionChanged += new EventHandler<PlotEventArgs>(this.m_frmGroundCovers.OnPlotSelectionChanged);
      }
      this.m_frmGroundCovers.Show(this.ContentPanel, DockState.DockBottom);
    }

    private void m_frmGroundCovers_Disposed(object sender, EventArgs e)
    {
      this.m_frmPlots.PlotSelectionChanged -= new EventHandler<PlotEventArgs>(this.m_frmGroundCovers.OnPlotSelectionChanged);
      this.m_frmGroundCovers = (PlotGroundCoversForm) null;
    }

    public void ShowLandUses()
    {
      if (this.m_frmLandUses == null)
      {
        this.m_frmLandUses = new PlotLandUsesForm();
        this.m_frmLandUses.Enter += new EventHandler(this.ContentForm_Enter);
        this.m_frmLandUses.Leave += new EventHandler(this.ContentForm_Leave);
        this.m_frmLandUses.RequestRefresh += new EventHandler<EventArgs>(this.ContentForm_RequestRefresh);
        this.m_frmLandUses.Disposed += new EventHandler(this.m_frmLandUses_Disposed);
        this.m_frmPlots.PlotSelectionChanged += new EventHandler<PlotEventArgs>(this.m_frmLandUses.OnPlotSelectionChanged);
      }
      this.m_frmLandUses.Show(this.ContentPanel, DockState.DockBottom);
    }

    private void m_frmLandUses_Disposed(object sender, EventArgs e)
    {
      this.m_frmPlots.PlotSelectionChanged -= new EventHandler<PlotEventArgs>(this.m_frmLandUses.OnPlotSelectionChanged);
      this.m_frmLandUses = (PlotLandUsesForm) null;
    }

    public void ShowPlantingSites()
    {
      if (this.m_frmPlantingSites == null)
      {
        this.m_frmPlantingSites = new PlotPlantingSitesForm();
        this.m_frmPlantingSites.Enter += new EventHandler(this.ContentForm_Enter);
        this.m_frmPlantingSites.Leave += new EventHandler(this.ContentForm_Leave);
        this.m_frmLandUses.RequestRefresh += new EventHandler<EventArgs>(this.ContentForm_RequestRefresh);
        this.m_frmPlantingSites.Disposed += new EventHandler(this.m_frmPlantingSites_Disposed);
        this.m_frmPlots.PlotSelectionChanged += new EventHandler<PlotEventArgs>(this.m_frmPlantingSites.OnPlotSelectionChanged);
      }
      this.m_frmPlantingSites.Show(this.ContentPanel, DockState.DockBottom);
    }

    private void m_frmPlantingSites_Disposed(object sender, EventArgs e)
    {
      this.m_frmPlots.PlotSelectionChanged -= new EventHandler<PlotEventArgs>(this.m_frmPlantingSites.OnPlotSelectionChanged);
      this.m_frmPlantingSites = (PlotPlantingSitesForm) null;
    }

    public void Export(ExportFormat format, string file)
    {
      if (!(this.m_activeContent is IExportable activeContent))
        return;
      activeContent.Export(format, file);
    }

    public bool CanExport(ExportFormat format) => this.m_activeContent is IExportable activeContent && activeContent.CanExport(format);

    private void ContentForm_Leave(object sender, EventArgs e) => this.m_inactiveContent = sender as ContentForm;

    private void ContentForm_Enter(object sender, EventArgs e)
    {
      this.m_activeContent = sender as ContentForm;
      IPlotContent plotContent = sender as IPlotContent;
      IPlotContent inactiveContent = this.m_inactiveContent as IPlotContent;
      if (plotContent == null)
        return;
      inactiveContent?.ContentDeactivated();
      plotContent.ContentActivated();
      this.OnRequestRefresh();
    }

    private void ContentForm_RequestRefresh(object sender, EventArgs e)
    {
      if (sender == null || !sender.Equals((object) this.m_activeContent))
        return;
      this.OnRequestRefresh();
    }

    public Eco.Util.ImportSpec ImportSpec() => this.m_activeContent is IImport activeContent ? activeContent.ImportSpec() : throw new NotSupportedException();

    public Task ImportData(
      IList data,
      IProgress<ProgressEventArgs> progress,
      CancellationToken ct)
    {
      if (this.m_activeContent is IImport activeContent)
        return activeContent.ImportData(data, progress, ct);
      throw new NotSupportedException();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PlotsForm));
      this.ContentPanel = new DockPanel();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.ContentPanel, "ContentPanel");
      this.ContentPanel.DockBottomPortion = 0.5;
      this.ContentPanel.DocumentStyle = DocumentStyle.DockingSdi;
      this.ContentPanel.Name = "ContentPanel";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.ContentPanel);
      this.Name = nameof (PlotsForm);
      this.ShowHint = DockState.Document;
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.ContentPanel, 0);
      this.ResumeLayout(false);
    }
  }
}
