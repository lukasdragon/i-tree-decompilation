// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.PlotPlantingSitesForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Interfaces;
using i_Tree_Eco_v6.Tasks;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class PlotPlantingSitesForm : ContentForm, IPlotContent, IActionable, IExportable
  {
    private ProgramSession m_ps;
    private Plot m_boundPlot;
    private Plot m_selectedPlot;
    private Year m_year;
    private ISession m_session;
    private DataGridViewManager m_dgManaager;
    private WaitCursor m_wc;
    private TaskManager m_taskManager;
    private object m_syncobj;
    private bool m_refresh;
    private IContainer components;
    private DataGridView dgPlantingSites;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewComboBoxColumn dcSiteType;
    private DataGridViewComboBoxColumn dcLandUse;
    private DataGridViewComboBoxColumn dcStreet;
    private DataGridViewTextBoxColumn dcAddres;
    private DataGridViewNumericTextBoxColumn dcLatitude;
    private DataGridViewNumericTextBoxColumn dcLongitude;

    public PlotPlantingSitesForm()
    {
      this.m_ps = Program.Session;
      this.m_session = this.m_ps.InputSession.CreateSession();
      this.InitializeComponent();
      this.m_dgManaager = new DataGridViewManager(this.dgPlantingSites);
      this.dgPlantingSites.DoubleBuffered(true);
      this.dgPlantingSites.AutoGenerateColumns = false;
      this.dgPlantingSites.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgPlantingSites.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.m_syncobj = new object();
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));
      this.Init();
    }

    private void Year_Updated(object sender, EntityUpdated<Year> e)
    {
      if (this.m_year == null || !(this.m_year.Guid == e.Guid))
        return;
      TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(this.GetYear(e.Guid, taskScheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.OnRequestRefresh();
      }), taskScheduler));
    }

    private void Init()
    {
      TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(this.GetYear(this.m_ps.InputSession.YearKey.Value, taskScheduler));
      this.m_taskManager.Add(new ListEntitiesTask<PlantingSiteType>(this.m_ps.InputSession).AddCriterion((ICriterion) Restrictions.NaturalId().Set("Year.Guid", (object) this.m_ps.InputSession.YearKey)).AddOrder(Order.Asc("Description")).DoWork().ContinueWith((System.Action<Task<ListResult<PlantingSiteType>>>) (task =>
      {
        BindingList<PlantingSiteType> bindingList = new BindingList<PlantingSiteType>(task.Result.List);
        this.dcSiteType.DisplayMember = "Description";
        this.dcSiteType.ValueMember = "Self";
        this.dcSiteType.DataSource = (object) bindingList;
      }), taskScheduler));
    }

    private Task GetYear(Guid g, TaskScheduler context) => Task.Factory.StartNew<Year>((Func<Year>) (() => this.m_session.Get<Year>((object) this.m_ps.InputSession.YearKey)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<Year>>) (t =>
    {
      if (this.IsDisposed)
        return;
      Year result = t.Result;
      lock (this.m_syncobj)
        this.m_year = result;
      this.OnRequestRefresh();
    }), context);

    public void OnPlotSelectionChanged(object sender, PlotEventArgs e)
    {
      lock (this.m_syncobj)
      {
        this.m_selectedPlot = e.Plot;
        this.m_refresh = true;
      }
      if (!this.Visible)
        return;
      this.LoadSelectedPlot();
    }

    private void LoadSelectedPlot()
    {
      Plot p = this.m_selectedPlot;
      this.m_refresh = false;
      if (p == null)
      {
        lock (this.m_syncobj)
          this.m_boundPlot = p;
        this.dgPlantingSites.AllowUserToAddRows = false;
        this.dgPlantingSites.DataSource = (object) null;
        this.Enabled = false;
      }
      else
      {
        this.Enabled = true;
        Task<ListResult<Street>> StreetsTask = new ListEntitiesTask<Street>(this.m_ps.InputSession).AddCriterion((ICriterion) Restrictions.NaturalId().Set("ProjectLocation.Guid", (object) p.ProjectLocation.Guid)).AddOrder(Order.Asc("Name")).DoWork();
        Task<ListResult<LandUse>> LandUsesTask = new ListEntitiesTask<LandUse>(this.m_ps.InputSession).AddAlias("PlotLandUses", "plu").AddCriterion((ICriterion) Restrictions.Eq("plu.Plot.Guid", (object) p.Guid)).AddOrder(Order.Asc("Description")).DoWork();
        Task<DataBindingListResult<PlantingSite>> PlantingSitesTask = new ListEntitiesTask<PlantingSite>(this.m_ps.InputSession).AddCriterion((ICriterion) Restrictions.NaturalId().Set("Plot", (object) p)).AddOrder(Order.Asc("Id")).Fetch(SelectMode.Fetch, "PlantingSiteType").Fetch(SelectMode.Fetch, "FieldLandUse").Fetch(SelectMode.Fetch, "Street").SetData((object) p).DoWork().ContinueWith<DataBindingListResult<PlantingSite>>((Func<Task<ListResult<PlantingSite>>, DataBindingListResult<PlantingSite>>) (t =>
        {
          ListResult<PlantingSite> result = t.Result;
          DataBindingList<PlantingSite> dataBindingList = new DataBindingList<PlantingSite>(result.List);
          dataBindingList.Sortable = true;
          dataBindingList.AddComparer<PlantingSite>((System.Linq.Expressions.Expression<Func<PlantingSite, object>>) (ps => ps.PlantingSiteType), (IComparer) new PropertyComparer<PlantingSiteType>((Func<PlantingSiteType, object>) (pst => (object) pst.Description)));
          dataBindingList.AddComparer<PlantingSite>((System.Linq.Expressions.Expression<Func<PlantingSite, object>>) (ps => ps.PlotLandUse), (IComparer) new ChildComparer<PlotLandUse, LandUse>((Func<PlotLandUse, LandUse>) (plu => plu.LandUse), (IComparer<LandUse>) new PropertyComparer<LandUse>((Func<LandUse, object>) (lu => (object) lu.Description))));
          dataBindingList.AddComparer<PlantingSite>((System.Linq.Expressions.Expression<Func<PlantingSite, object>>) (ps => ps.Street), (IComparer) new PropertyComparer<Street>((Func<Street, object>) (s => (object) s.Name)));
          return new DataBindingListResult<PlantingSite>()
          {
            List = dataBindingList,
            Data = result.Data,
            Session = result.Session
          };
        }));
        this.m_taskManager.Add((Task) StreetsTask, (Task) LandUsesTask, (Task) PlantingSitesTask);
        this.m_taskManager.Add(this.m_taskManager.WhenAll().ContinueWith((System.Action<Task>) (task =>
        {
          BindingList<Street> bindingList1 = new BindingList<Street>(StreetsTask.Result.List);
          BindingList<LandUse> bindingList2 = new BindingList<LandUse>(LandUsesTask.Result.List);
          DataBindingList<PlantingSite> list = PlantingSitesTask.Result.List;
          if (!this.m_selectedPlot.Equals((object) p))
            return;
          this.dgPlantingSites.DataSource = (object) null;
          this.dcStreet.DisplayMember = "Name";
          this.dcStreet.ValueMember = "Self";
          this.dcStreet.DataSource = (object) bindingList1;
          this.dcLandUse.DisplayMember = "Description";
          this.dcLandUse.ValueMember = "Self";
          this.dcLandUse.DataSource = (object) bindingList2;
          this.dgPlantingSites.DataSource = (object) list;
          this.dgPlantingSites.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
          list.AddingNew += new AddingNewEventHandler(this.PlantingSites_AddingNew);
          list.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.PlantingSites_BeforeRemove);
          list.ListChanged += new ListChangedEventHandler(this.PlantingSites_ListChanged);
          lock (this.m_syncobj)
            this.m_boundPlot = PlantingSitesTask.Result.Data as Plot;
          this.OnRequestRefresh();
        }), TaskScheduler.FromCurrentSynchronizationContext()));
      }
    }

    private void PlantingSites_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void PlantingSites_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<PlantingSite> dataBindingList = sender as DataBindingList<PlantingSite>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      PlantingSite plantingSite = dataBindingList[e.NewIndex];
      if (plantingSite.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) plantingSite);
          transaction.Commit();
        }
      }
    }

    private void PlantingSites_AddingNew(object sender, AddingNewEventArgs e)
    {
      DataBindingList<PlantingSite> dataBindingList = sender as DataBindingList<PlantingSite>;
      int num = 1;
      foreach (PlantingSite plantingSite in (Collection<PlantingSite>) dataBindingList)
      {
        if (plantingSite.Id > num)
          num = plantingSite.Id + 1;
      }
      e.NewObject = (object) new PlantingSite()
      {
        Id = num,
        Plot = this.m_boundPlot
      };
    }

    private void dgPlantingSites_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<PlantingSite> dataSource = this.dgPlantingSites.DataSource as DataBindingList<PlantingSite>;
      if (this.dgPlantingSites.CurrentRow != null && !this.dgPlantingSites.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      PlantingSite plantingSite1 = dataSource[e.RowIndex];
      string text = (string) null;
      if (plantingSite1.Id == 0)
        text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) this.dcId.HeaderText);
      else if (plantingSite1.PlantingSiteType == null)
      {
        text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) this.dcSiteType.HeaderText);
      }
      else
      {
        foreach (PlantingSite plantingSite2 in (Collection<PlantingSite>) dataSource)
        {
          if (plantingSite2 != plantingSite1 && plantingSite2.Id == plantingSite1.Id)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcId.HeaderText);
            break;
          }
        }
      }
      if (text == null)
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgPlantingSites_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.m_boundPlot == null)
        return;
      DataBindingList<PlantingSite> dataSource = this.dgPlantingSites.DataSource as DataBindingList<PlantingSite>;
      if (this.dgPlantingSites.CurrentRow != null && this.dgPlantingSites.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (e.RowIndex >= dataSource.Count)
          return;
        PlantingSite plantingSite = dataSource[e.RowIndex];
        lock (this.m_syncobj)
        {
          using (ITransaction transaction = this.m_session.BeginTransaction())
          {
            this.m_session.SaveOrUpdate((object) plantingSite);
            transaction.Commit();
          }
        }
      }
    }

    private void dgPlantingSites_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void dgPlantingSites_SelectionChanged(object sender, EventArgs e)
    {
      if (this.m_boundPlot == null)
        return;
      this.OnRequestRefresh();
    }

    private void DeleteSelectedRows()
    {
      if (!(this.dgPlantingSites.DataSource is DataBindingList<PlantingSite> dataSource))
        return;
      CurrencyManager currencyManager = this.dgPlantingSites.BindingContext[(object) dataSource] as CurrencyManager;
      bool flag = false;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgPlantingSites.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          dataSource.RemoveAt(selectedRow.Index);
          flag = true;
        }
      }
      if (!(currencyManager.Position != -1 & flag))
        return;
      this.dgPlantingSites.Rows[currencyManager.Position].Selected = true;
    }

    private void PlotPlantingSitesForm_VisibleChanged(object sender, EventArgs e)
    {
      bool flag = false;
      lock (this.m_syncobj)
        flag = this.m_selectedPlot == null ? this.m_boundPlot != null && this.Visible : !this.m_selectedPlot.Equals((object) this.m_boundPlot) && this.Visible;
      if (!(this.m_refresh | flag))
        return;
      this.LoadSelectedPlot();
    }

    public void ContentActivated()
    {
      this.dgPlantingSites.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgPlantingSites.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgPlantingSites.DefaultCellStyle = Program.ActiveGridDefaultCellStyle;
    }

    public void ContentDeactivated()
    {
      this.dgPlantingSites.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgPlantingSites.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dgPlantingSites.DefaultCellStyle = Program.InActiveGridDefaultCellStyle;
    }

    protected override void OnRequestRefresh()
    {
      this.UpdateGUI();
      base.OnRequestRefresh();
    }

    private void UpdateGUI()
    {
      bool flag = this.m_year != null && this.m_year.Changed && this.dgPlantingSites.DataSource != null;
      this.dgPlantingSites.AllowUserToAddRows = flag;
      this.dgPlantingSites.AllowUserToDeleteRows = flag;
      this.dgPlantingSites.ReadOnly = !flag;
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgPlantingSites.DataSource != null;
      bool flag2 = flag1 && this.dgPlantingSites.AllowUserToAddRows;
      bool flag3 = this.dgPlantingSites.SelectedRows.Count > 0;
      bool flag4 = this.dgPlantingSites.CurrentRow != null && this.dgPlantingSites.IsCurrentRowDirty;
      bool flag5 = this.dgPlantingSites.CurrentRow != null && this.dgPlantingSites.CurrentRow.IsNewRow;
      bool flag6 = false;
      switch (action)
      {
        case UserActions.New:
          flag6 = flag2 && !flag5 && !flag5;
          break;
        case UserActions.Copy:
          flag6 = flag2 & flag3 && !flag4 && !flag5;
          break;
        case UserActions.Undo:
          flag6 = flag1 && this.m_dgManaager.CanUndo;
          break;
        case UserActions.Redo:
          flag6 = flag1 && this.m_dgManaager.CanRedo;
          break;
        case UserActions.Delete:
          flag6 = flag2 & flag3 && (flag4 || !flag5);
          break;
      }
      return flag6;
    }

    public void PerformAction(UserActions action)
    {
      switch (action)
      {
        case UserActions.New:
          if (!this.dgPlantingSites.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgPlantingSites.SelectedRows)
            selectedRow.Selected = false;
          this.dgPlantingSites.Rows[this.dgPlantingSites.NewRowIndex].Selected = true;
          this.dgPlantingSites.FirstDisplayedScrollingRowIndex = this.dgPlantingSites.NewRowIndex - this.dgPlantingSites.DisplayedRowCount(false) + 1;
          this.dgPlantingSites.CurrentCell = this.dgPlantingSites.Rows[this.dgPlantingSites.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          if (this.dgPlantingSites.SelectedRows.Count <= 0)
            break;
          DataGridViewRow selectedRow1 = this.dgPlantingSites.SelectedRows[0];
          if (!(this.dgPlantingSites.DataSource is DataBindingList<PlantingSite> dataSource) || selectedRow1.Index >= dataSource.Count)
            break;
          PlantingSite plantingSite1 = dataSource[selectedRow1.Index].Clone() as PlantingSite;
          int num = 1;
          foreach (PlantingSite plantingSite2 in (Collection<PlantingSite>) dataSource)
          {
            if (plantingSite2.Id >= num)
              num = plantingSite2.Id + 1;
          }
          plantingSite1.Id = num;
          dataSource.Add(plantingSite1);
          this.dgPlantingSites.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this.m_dgManaager.Undo();
          this.UpdateGUI();
          break;
        case UserActions.Redo:
          this.m_dgManaager.Redo();
          this.UpdateGUI();
          break;
        case UserActions.Delete:
          if (this.dgPlantingSites.SelectedRows.Count <= 0)
            break;
          this.DeleteSelectedRows();
          break;
      }
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgPlantingSites.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgPlantingSites.DataSource != null;

    private void dgPlantingSites_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgPlantingSites_EditingControlShowing(
      object sender,
      DataGridViewEditingControlShowingEventArgs e)
    {
      if (!(e.Control is ComboBox control))
        return;
      control.KeyDown -= new KeyEventHandler(this.DataGridViewComboBoxCell_KeyDown);
      control.KeyDown += new KeyEventHandler(this.DataGridViewComboBoxCell_KeyDown);
    }

    private void DataGridViewComboBoxCell_KeyDown(object sender, KeyEventArgs e)
    {
      if (!(sender is ComboBox comboBox) || e.KeyCode != Keys.Delete)
        return;
      comboBox.SelectedIndex = -1;
      e.Handled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PlotPlantingSitesForm));
      this.dgPlantingSites = new DataGridView();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcSiteType = new DataGridViewComboBoxColumn();
      this.dcLandUse = new DataGridViewComboBoxColumn();
      this.dcStreet = new DataGridViewComboBoxColumn();
      this.dcAddres = new DataGridViewTextBoxColumn();
      this.dcLatitude = new DataGridViewNumericTextBoxColumn();
      this.dcLongitude = new DataGridViewNumericTextBoxColumn();
      ((ISupportInitialize) this.dgPlantingSites).BeginInit();
      this.SuspendLayout();
      this.lblBreadcrumb.Size = new Size(753, 29);
      this.lblBreadcrumb.Visible = false;
      this.dgPlantingSites.AllowUserToAddRows = false;
      this.dgPlantingSites.AllowUserToDeleteRows = false;
      this.dgPlantingSites.AllowUserToResizeRows = false;
      this.dgPlantingSites.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgPlantingSites.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgPlantingSites.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgPlantingSites.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcSiteType, (DataGridViewColumn) this.dcLandUse, (DataGridViewColumn) this.dcStreet, (DataGridViewColumn) this.dcAddres, (DataGridViewColumn) this.dcLatitude, (DataGridViewColumn) this.dcLongitude);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgPlantingSites.DefaultCellStyle = gridViewCellStyle2;
      this.dgPlantingSites.Dock = DockStyle.Fill;
      this.dgPlantingSites.EnableHeadersVisualStyles = false;
      this.dgPlantingSites.Location = new Point(0, 29);
      this.dgPlantingSites.MultiSelect = false;
      this.dgPlantingSites.Name = "dgPlantingSites";
      this.dgPlantingSites.ReadOnly = true;
      this.dgPlantingSites.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgPlantingSites.RowHeadersWidth = 20;
      this.dgPlantingSites.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      this.dgPlantingSites.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgPlantingSites.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgPlantingSites.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgPlantingSites.Size = new Size(753, 406);
      this.dgPlantingSites.TabIndex = 1;
      this.dgPlantingSites.CurrentCellDirtyStateChanged += new EventHandler(this.dgPlantingSites_CurrentCellDirtyStateChanged);
      this.dgPlantingSites.DataError += new DataGridViewDataErrorEventHandler(this.dgPlantingSites_DataError);
      this.dgPlantingSites.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgPlantingSites_EditingControlShowing);
      this.dgPlantingSites.RowValidated += new DataGridViewCellEventHandler(this.dgPlantingSites_RowValidated);
      this.dgPlantingSites.RowValidating += new DataGridViewCellCancelEventHandler(this.dgPlantingSites_RowValidating);
      this.dgPlantingSites.SelectionChanged += new EventHandler(this.dgPlantingSites_SelectionChanged);
      this.dcId.DataPropertyName = "Id";
      this.dcId.DecimalPlaces = 0;
      this.dcId.Format = "#;#";
      this.dcId.Frozen = true;
      this.dcId.HasDecimal = false;
      this.dcId.HeaderText = "ID";
      this.dcId.Name = "dcId";
      this.dcId.ReadOnly = true;
      this.dcId.Resizable = DataGridViewTriState.True;
      this.dcId.Signed = false;
      this.dcSiteType.DataPropertyName = "PlantingSiteType";
      this.dcSiteType.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcSiteType.HeaderText = "Site Type";
      this.dcSiteType.Name = "dcSiteType";
      this.dcSiteType.ReadOnly = true;
      this.dcSiteType.Resizable = DataGridViewTriState.True;
      this.dcSiteType.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcLandUse.DataPropertyName = "LandUse";
      this.dcLandUse.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcLandUse.HeaderText = "Land Use";
      this.dcLandUse.Name = "dcLandUse";
      this.dcLandUse.ReadOnly = true;
      this.dcStreet.DataPropertyName = "Street";
      this.dcStreet.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcStreet.HeaderText = "Street";
      this.dcStreet.Name = "dcStreet";
      this.dcStreet.ReadOnly = true;
      this.dcAddres.DataPropertyName = "StreetAddress";
      this.dcAddres.HeaderText = "Address";
      this.dcAddres.Name = "dcAddres";
      this.dcAddres.ReadOnly = true;
      this.dcLatitude.DataPropertyName = "yCoordinate";
      this.dcLatitude.DecimalPlaces = 0;
      this.dcLatitude.Format = "#.#;-#.#";
      this.dcLatitude.HasDecimal = true;
      this.dcLatitude.HeaderText = "Latitude (Y)";
      this.dcLatitude.Name = "dcLatitude";
      this.dcLatitude.ReadOnly = true;
      this.dcLatitude.Resizable = DataGridViewTriState.True;
      this.dcLatitude.Signed = true;
      this.dcLongitude.DataPropertyName = "xCoordinate";
      this.dcLongitude.DecimalPlaces = 0;
      this.dcLongitude.Format = "#.#;-#.#";
      this.dcLongitude.HasDecimal = true;
      this.dcLongitude.HeaderText = "Longitude (X)";
      this.dcLongitude.Name = "dcLongitude";
      this.dcLongitude.ReadOnly = true;
      this.dcLongitude.Resizable = DataGridViewTriState.True;
      this.dcLongitude.Signed = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(753, 435);
      this.CloseButtonVisible = false;
      this.Controls.Add((Control) this.dgPlantingSites);
      this.DockAreas = DockAreas.DockBottom;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (PlotPlantingSitesForm);
      this.ShowHint = DockState.DockBottom;
      this.Text = "Planting Sites";
      this.VisibleChanged += new EventHandler(this.PlotPlantingSitesForm_VisibleChanged);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgPlantingSites, 0);
      ((ISupportInitialize) this.dgPlantingSites).EndInit();
      this.ResumeLayout(false);
    }
  }
}
