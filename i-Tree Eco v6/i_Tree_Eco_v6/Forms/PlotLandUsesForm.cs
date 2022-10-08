// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.PlotLandUsesForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Interfaces;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class PlotLandUsesForm : ContentForm, IPlotContent, IActionable, IExportable
  {
    private ProgramSession m_ps;
    private Plot m_boundPlot;
    private Plot m_selectedPlot;
    private Year m_year;
    private ISession m_session;
    private IList<LandUse> m_landuses;
    private DataBindingList<PlotLandUse> m_plotLandUses;
    private DataGridViewManager m_dgManager;
    private WaitCursor m_wc;
    private TaskManager m_taskManager;
    private readonly object m_syncobj;
    private bool m_refresh;
    private IContainer components;
    private DataGridView dgLandUses;
    private DataGridViewComboBoxColumn dcLandUse;
    private DataGridViewNumericTextBoxColumn dcPctPlot;
    private FlowLayoutPanel flowLayoutPanel1;
    private Label lblPctPlot;

    public PlotLandUsesForm()
    {
      this.m_ps = Program.Session;
      this.InitializeComponent();
      this.m_dgManager = new DataGridViewManager(this.dgLandUses);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.m_syncobj = new object();
      this.dgLandUses.DoubleBuffered(true);
      this.dgLandUses.AutoGenerateColumns = false;
      this.dgLandUses.MultiSelect = false;
      this.dgLandUses.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgLandUses.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dgLandUses.CellEndEdit += new DataGridViewCellEventHandler(this.dgLandUses_CellEndEdit);
      this.dcPctPlot.DefaultCellStyle.DataSourceNullValue = (object) 0;
      EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));
      this.Init();
    }

    private void Year_Updated(object sender, EntityUpdated<Year> e)
    {
      if (this.m_year == null || !(this.m_year.Guid == e.Guid))
        return;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.GetYear(e.Guid)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.OnRequestRefresh();
      }), scheduler));
    }

    private void Init()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.InitGrid();
      }), scheduler));
    }

    private void LoadData()
    {
      ISession session = this.m_ps.InputSession.CreateSession();
      using (session.BeginTransaction())
      {
        Year y = session.Get<Year>((object) this.m_ps.InputSession.YearKey);
        IList<LandUse> landUseList = session.QueryOver<LandUse>().Where((System.Linq.Expressions.Expression<Func<LandUse, bool>>) (lu => lu.Year == y)).OrderBy((System.Linq.Expressions.Expression<Func<LandUse, object>>) (lu => lu.Description)).Asc.List();
        lock (this.m_syncobj)
        {
          this.m_session = session;
          this.m_year = y;
          this.m_landuses = landUseList;
        }
      }
    }

    private void InitGrid() => this.dcLandUse.BindTo<LandUse>((System.Linq.Expressions.Expression<Func<LandUse, object>>) (lu => lu.Description), (System.Linq.Expressions.Expression<Func<LandUse, object>>) (lu => lu.Self), (object) this.m_landuses);

    private void GetYear(Guid g)
    {
      if (this.m_session == null || this.m_year == null || !(this.m_year.Guid == g))
        return;
      lock (this.m_syncobj)
        this.m_session.Refresh((object) this.m_year);
    }

    public void OnPlotSelectionChanged(object sender, PlotEventArgs e)
    {
      lock (this.m_syncobj)
      {
        this.m_selectedPlot = e.Plot;
        if (this.Visible)
          this.LoadAndBindPlot();
        else
          this.m_refresh = true;
      }
    }

    private void LoadAndBindPlot()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.LoadSelectedPlot()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.InitBoundPlot();
      }), scheduler));
    }

    private void LoadSelectedPlot()
    {
      lock (this.m_syncobj)
      {
        Plot p = this.m_selectedPlot;
        if (p != null)
        {
          if (this.m_session == null || this.m_boundPlot == p)
            return;
          using (this.m_session.BeginTransaction())
            this.m_plotLandUses = new DataBindingList<PlotLandUse>(this.m_session.QueryOver<PlotLandUse>().Where((System.Linq.Expressions.Expression<Func<PlotLandUse, bool>>) (plu => plu.Plot == p)).Fetch<PlotLandUse, PlotLandUse>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<PlotLandUse, object>>) (plu => plu.LandUse)).List());
          this.m_boundPlot = p;
        }
        else
        {
          this.m_plotLandUses = (DataBindingList<PlotLandUse>) null;
          this.m_boundPlot = p;
        }
      }
    }

    private void InitBoundPlot()
    {
      lock (this.m_syncobj)
      {
        if (this.m_boundPlot != this.m_selectedPlot)
          return;
        if (this.m_plotLandUses != null)
        {
          if (this.dgLandUses.DataSource != this.m_plotLandUses)
          {
            this.dgLandUses.DataSource = (object) this.m_plotLandUses;
            this.dgLandUses.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
            this.m_plotLandUses.Sortable = true;
            this.m_plotLandUses.AddComparer<PlotLandUse>((System.Linq.Expressions.Expression<Func<PlotLandUse, object>>) (plu => plu.LandUse), (IComparer) new PropertyComparer<LandUse>((Func<LandUse, object>) (lu => (object) lu.Description)));
            this.m_plotLandUses.AddingNew += new AddingNewEventHandler(this.LandUses_AddingNew);
            this.m_plotLandUses.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.LandUses_BeforeRemove);
            this.m_plotLandUses.ListChanged += new ListChangedEventHandler(this.LandUses_ListChanged);
          }
        }
        else
          this.dgLandUses.DataSource = (object) null;
        this.m_refresh = false;
        this.OnRequestRefresh();
      }
    }

    private int PercentOfPlot(DataBindingList<PlotLandUse> dsPlotLandUses)
    {
      int num = 100;
      if (dsPlotLandUses != null)
      {
        if (this.dgLandUses.CurrentRow != null && !this.dgLandUses.CurrentRow.IsNewRow)
        {
          num = dsPlotLandUses.Sum<PlotLandUse>((Func<PlotLandUse, int>) (plu => (int) plu.PercentOfPlot));
        }
        else
        {
          num = 0;
          for (int index = 0; index < this.dgLandUses.NewRowIndex; ++index)
          {
            PlotLandUse dsPlotLandUse = dsPlotLandUses[index];
            num += (int) dsPlotLandUse.PercentOfPlot;
          }
        }
      }
      return num;
    }

    private void LandUses_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void LandUses_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<PlotLandUse> dataBindingList = sender as DataBindingList<PlotLandUse>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      PlotLandUse entity = dataBindingList[e.NewIndex];
      if (entity.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) entity);
          transaction.Commit();
        }
        EventPublisher.Publish<EntityDeleted<PlotLandUse>>(new EntityDeleted<PlotLandUse>(entity), (Control) this);
      }
    }

    private void LandUses_AddingNew(object sender, AddingNewEventArgs e)
    {
      int num = 100 - this.PercentOfPlot(sender as DataBindingList<PlotLandUse>);
      e.NewObject = (object) new PlotLandUse()
      {
        Plot = this.m_boundPlot,
        PercentOfPlot = (short) num
      };
    }

    private void dgLandUses_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgLandUses.NewRowIndex || !(this.dgLandUses.DataSource is DataBindingList<PlotLandUse> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgLandUses.Columns[e.ColumnIndex];
      PlotLandUse plotLandUse = dataSource[e.RowIndex];
      if (column == this.dcLandUse)
      {
        if (plotLandUse.LandUse != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else
      {
        if (column != this.dcPctPlot || plotLandUse.PercentOfPlot != (short) 0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
    }

    private void dgLandUses_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<PlotLandUse> dataSource = this.dgLandUses.DataSource as DataBindingList<PlotLandUse>;
      if (this.dgLandUses.CurrentRow != null && !this.dgLandUses.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      PlotLandUse plotLandUse1 = dataSource[e.RowIndex];
      DataGridViewRow row = this.dgLandUses.Rows[e.RowIndex];
      string text = (string) null;
      if (plotLandUse1.LandUse != null)
      {
        foreach (PlotLandUse plotLandUse2 in (Collection<PlotLandUse>) dataSource)
        {
          if (plotLandUse2 != plotLandUse1 && plotLandUse1.LandUse.Equals((object) plotLandUse2.LandUse))
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcLandUse.HeaderText);
            break;
          }
        }
      }
      if (text == null)
      {
        DataGridViewCell dataGridViewCell = row.ErrorCell();
        if (dataGridViewCell != null)
          text = dataGridViewCell.ErrorText;
      }
      if (text == null)
      {
        int num = this.PercentOfPlot(dataSource);
        if (num > 100)
          text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRange, (object) this.dcPctPlot.HeaderText, (object) 1.ToString("D"), (object) (100 + (int) plotLandUse1.PercentOfPlot - num).ToString("D"));
      }
      if (text == null)
        return;
      e.Cancel = true;
      int num1 = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgLandUses_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgLandUses.ReadOnly || this.m_boundPlot == null)
        return;
      DataBindingList<PlotLandUse> dataSource = this.dgLandUses.DataSource as DataBindingList<PlotLandUse>;
      if (this.dgLandUses.CurrentRow != null && this.dgLandUses.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          PlotLandUse entity = dataSource[e.RowIndex];
          bool isTransient = entity.IsTransient;
          if (isTransient || this.m_session.IsDirty())
          {
            lock (this.m_syncobj)
            {
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                this.m_session.SaveOrUpdate((object) entity);
                transaction.Commit();
              }
            }
            if (isTransient)
              EventPublisher.Publish<EntityCreated<PlotLandUse>>(new EntityCreated<PlotLandUse>(entity), (Control) this);
            else
              EventPublisher.Publish<EntityUpdated<PlotLandUse>>(new EntityUpdated<PlotLandUse>(entity), (Control) this);
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgLandUses_CellEndEdit(object sender, DataGridViewCellEventArgs e) => this.OnRequestRefresh();

    private void dgLandUses_SelectionChanged(object sender, EventArgs e)
    {
      if (this.m_boundPlot == null)
        return;
      this.OnRequestRefresh();
    }

    private void DeleteSelectedRows()
    {
      if (!(this.dgLandUses.DataSource is DataBindingList<PlotLandUse> dataSource))
        return;
      CurrencyManager currencyManager = this.dgLandUses.BindingContext[(object) dataSource] as CurrencyManager;
      bool flag = false;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgLandUses.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          if (this.CanDeletePlotLandUse(dataSource[selectedRow.Index]))
          {
            dataSource.RemoveAt(selectedRow.Index);
            flag = true;
          }
          else
          {
            int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDelete, (object) v6Strings.LandUse_SingularName, (object) v6Strings.Tree_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
      if (!(currencyManager.Position != -1 & flag))
        return;
      this.dgLandUses.Rows[currencyManager.Position].Selected = true;
    }

    private bool CanDeletePlotLandUse(PlotLandUse plu)
    {
      if (plu.IsTransient)
        return true;
      using (this.m_session.BeginTransaction())
        return this.m_session.CreateCriteria<Tree>().SetProjection((IProjection) Projections.ProjectionList().Add(Projections.RowCount())).Add((ICriterion) Restrictions.Eq("PlotLandUse", (object) plu)).UniqueResult<int>() == 0;
    }

    private void PlotLandUsesForm_VisibleChanged(object sender, EventArgs e)
    {
      bool flag = false;
      lock (this.m_syncobj)
        flag = this.m_selectedPlot == null ? this.m_boundPlot != null && this.Visible : !this.m_selectedPlot.Equals((object) this.m_boundPlot) && this.Visible;
      if (!(this.m_refresh | flag))
        return;
      this.LoadAndBindPlot();
    }

    public void ContentActivated()
    {
      this.dgLandUses.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgLandUses.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgLandUses.DefaultCellStyle = Program.ActiveGridDefaultCellStyle;
    }

    public void ContentDeactivated()
    {
      this.dgLandUses.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgLandUses.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dgLandUses.DefaultCellStyle = Program.InActiveGridDefaultCellStyle;
    }

    protected override void OnRequestRefresh()
    {
      DataBindingList<PlotLandUse> dataSource = this.dgLandUses.DataSource as DataBindingList<PlotLandUse>;
      int num = this.PercentOfPlot(dataSource);
      bool flag1 = this.m_year != null && this.m_year.Changed && dataSource != null;
      bool flag2 = flag1 && num < 100;
      if (dataSource != null)
        this.lblPctPlot.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtTotal, (object) this.dcPctPlot.HeaderText), (object) ((double) num / 100.0).ToString("P0"));
      else
        this.lblPctPlot.Text = string.Empty;
      this.dgLandUses.AllowUserToAddRows = flag2;
      this.dgLandUses.AllowUserToDeleteRows = flag1;
      this.dgLandUses.ReadOnly = !flag1;
      if (num == 100)
        this.lblPctPlot.ForeColor = Color.Green;
      else
        this.lblPctPlot.ForeColor = Color.Red;
      this.Enabled = dataSource != null;
      base.OnRequestRefresh();
    }

    public bool CanPerformAction(UserActions action)
    {
      object dataSource = this.dgLandUses.DataSource;
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgLandUses.DataSource != null;
      bool flag2 = flag1 && this.dgLandUses.AllowUserToAddRows;
      bool flag3 = this.dgLandUses.SelectedRows.Count > 0;
      bool flag4 = this.dgLandUses.CurrentRow != null && this.dgLandUses.IsCurrentRowDirty;
      bool flag5 = this.dgLandUses.CurrentRow != null && this.dgLandUses.CurrentRow.IsNewRow;
      bool flag6 = false;
      switch (action)
      {
        case UserActions.New:
          flag6 = flag2 && !flag5 && !flag4;
          break;
        case UserActions.Copy:
          flag6 = flag2 & flag3 && !flag4 && !flag5;
          break;
        case UserActions.Undo:
          flag6 = flag1 && this.m_dgManager.CanUndo;
          break;
        case UserActions.Redo:
          flag6 = flag1 && this.m_dgManager.CanRedo;
          break;
        case UserActions.Delete:
          flag6 = flag1 & flag3 && (flag4 || !flag5);
          break;
      }
      return flag6;
    }

    public void PerformAction(UserActions action)
    {
      if (!this.CanPerformAction(action))
        return;
      switch (action)
      {
        case UserActions.New:
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgLandUses.SelectedRows)
            selectedRow.Selected = false;
          this.dgLandUses.Rows[this.dgLandUses.NewRowIndex].Selected = true;
          this.dgLandUses.FirstDisplayedScrollingRowIndex = this.dgLandUses.NewRowIndex - this.dgLandUses.DisplayedRowCount(false) + 1;
          this.dgLandUses.CurrentCell = this.dgLandUses.Rows[this.dgLandUses.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          DataGridViewRow selectedRow1 = this.dgLandUses.SelectedRows[0];
          if (!(this.dgLandUses.DataSource is DataBindingList<PlotLandUse> dataSource) || selectedRow1.Index >= dataSource.Count)
            break;
          int val2 = 100 - this.PercentOfPlot(dataSource);
          PlotLandUse plotLandUse = dataSource[selectedRow1.Index].Clone() as PlotLandUse;
          plotLandUse.PercentOfPlot = Math.Min(plotLandUse.PercentOfPlot, (short) val2);
          dataSource.Add(plotLandUse);
          this.dgLandUses.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          if (this.dgLandUses.SelectedRows.Count <= 0)
            break;
          this.DeleteSelectedRows();
          break;
      }
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgLandUses.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgLandUses.DataSource != null;

    private void dgLandUses_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (this.CanDeletePlotLandUse(e.Row.DataBoundItem as PlotLandUse))
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDelete, (object) v6Strings.LandUse_SingularName, (object) v6Strings.Tree_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgLandUses_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgLandUses_EditingControlShowing(
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PlotLandUsesForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      this.dgLandUses = new DataGridView();
      this.dcLandUse = new DataGridViewComboBoxColumn();
      this.dcPctPlot = new DataGridViewNumericTextBoxColumn();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.lblPctPlot = new Label();
      ((ISupportInitialize) this.dgLandUses).BeginInit();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgLandUses.AllowUserToAddRows = false;
      this.dgLandUses.AllowUserToDeleteRows = false;
      this.dgLandUses.AllowUserToResizeRows = false;
      this.dgLandUses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgLandUses.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgLandUses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgLandUses.Columns.AddRange((DataGridViewColumn) this.dcLandUse, (DataGridViewColumn) this.dcPctPlot);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgLandUses.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgLandUses, "dgLandUses");
      this.dgLandUses.EnableHeadersVisualStyles = false;
      this.dgLandUses.Name = "dgLandUses";
      this.dgLandUses.ReadOnly = true;
      this.dgLandUses.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgLandUses.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgLandUses.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgLandUses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgLandUses.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgLandUses_CellErrorTextNeeded);
      this.dgLandUses.DataError += new DataGridViewDataErrorEventHandler(this.dgLandUses_DataError);
      this.dgLandUses.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgLandUses_EditingControlShowing);
      this.dgLandUses.RowValidated += new DataGridViewCellEventHandler(this.dgLandUses_RowValidated);
      this.dgLandUses.RowValidating += new DataGridViewCellCancelEventHandler(this.dgLandUses_RowValidating);
      this.dgLandUses.SelectionChanged += new EventHandler(this.dgLandUses_SelectionChanged);
      this.dgLandUses.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.dgLandUses_UserDeletingRow);
      this.dcLandUse.DataPropertyName = "LandUse";
      this.dcLandUse.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcLandUse, "dcLandUse");
      this.dcLandUse.Name = "dcLandUse";
      this.dcLandUse.ReadOnly = true;
      this.dcLandUse.Resizable = DataGridViewTriState.True;
      this.dcLandUse.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctPlot.DataPropertyName = "PercentOfPlot";
      this.dcPctPlot.DecimalPlaces = 0;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle3.Format = "N0";
      this.dcPctPlot.DefaultCellStyle = gridViewCellStyle3;
      this.dcPctPlot.Format = "#;#";
      this.dcPctPlot.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcPctPlot, "dcPctPlot");
      this.dcPctPlot.Name = "dcPctPlot";
      this.dcPctPlot.ReadOnly = true;
      this.dcPctPlot.Resizable = DataGridViewTriState.True;
      this.dcPctPlot.Signed = false;
      componentResourceManager.ApplyResources((object) this.flowLayoutPanel1, "flowLayoutPanel1");
      this.flowLayoutPanel1.Controls.Add((Control) this.lblPctPlot);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.lblPctPlot, "lblPctPlot");
      this.lblPctPlot.Name = "lblPctPlot";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CloseButtonVisible = false;
      this.Controls.Add((Control) this.dgLandUses);
      this.Controls.Add((Control) this.flowLayoutPanel1);
      this.DockAreas = DockAreas.DockBottom;
      this.Name = nameof (PlotLandUsesForm);
      this.ShowHint = DockState.DockBottom;
      this.VisibleChanged += new EventHandler(this.PlotLandUsesForm_VisibleChanged);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.flowLayoutPanel1, 0);
      this.Controls.SetChildIndex((Control) this.dgLandUses, 0);
      ((ISupportInitialize) this.dgLandUses).EndInit();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
