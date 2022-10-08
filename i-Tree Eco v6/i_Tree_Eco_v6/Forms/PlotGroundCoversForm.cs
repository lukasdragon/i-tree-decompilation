// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.PlotGroundCoversForm
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
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class PlotGroundCoversForm : ContentForm, IPlotContent, IActionable, IExportable
  {
    private ProgramSession m_ps;
    private Plot m_boundPlot;
    private Plot m_selectedPlot;
    private Year m_year;
    private IList<GroundCover> m_groundCovers;
    private DataBindingList<PlotGroundCover> m_plotGroundCovers;
    private ISession m_session;
    private DataGridViewManager m_dgManager;
    private WaitCursor m_wc;
    private TaskManager m_taskManager;
    private object m_syncobj;
    private bool m_refresh;
    private IContainer components;
    private DataGridView dgGroundCovers;
    private FlowLayoutPanel flowLayoutPanel1;
    private Label lblPctCovered;
    private DataGridViewComboBoxColumn dcGroundCover;
    private DataGridViewNumericTextBoxColumn dcPctCovered;

    public PlotGroundCoversForm()
    {
      this.m_ps = Program.Session;
      this.InitializeComponent();
      this.m_dgManager = new DataGridViewManager(this.dgGroundCovers);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.m_syncobj = new object();
      this.dgGroundCovers.DoubleBuffered(true);
      this.dgGroundCovers.AutoGenerateColumns = false;
      this.dgGroundCovers.MultiSelect = false;
      this.dgGroundCovers.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgGroundCovers.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dgGroundCovers.CellEndEdit += new DataGridViewCellEventHandler(this.dgGroundCovers_CellEndEdit);
      this.dcPctCovered.DefaultCellStyle.DataSourceNullValue = (object) 0;
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
        IList<GroundCover> groundCoverList = session.QueryOver<GroundCover>().Where((Expression<Func<GroundCover, bool>>) (gc => gc.Year == y)).OrderBy((Expression<Func<GroundCover, object>>) (gc => gc.Description)).Asc.List();
        lock (this.m_syncobj)
        {
          this.m_session = session;
          this.m_year = y;
          this.m_groundCovers = groundCoverList;
        }
      }
    }

    private void InitGrid() => this.dcGroundCover.BindTo<GroundCover>((Expression<Func<GroundCover, object>>) (gc => gc.Description), (Expression<Func<GroundCover, object>>) (gc => gc.Self), (object) this.m_groundCovers);

    private void GetYear(Guid g)
    {
      lock (this.m_syncobj)
      {
        if (this.m_session == null || this.m_year == null || !(this.m_year.Guid == g))
          return;
        this.m_session.Refresh((object) this.m_year);
      }
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
          {
            this.m_plotGroundCovers = new DataBindingList<PlotGroundCover>(this.m_session.QueryOver<PlotGroundCover>().Where((Expression<Func<PlotGroundCover, bool>>) (pgc => pgc.Plot == p)).Fetch<PlotGroundCover, PlotGroundCover>(SelectMode.Fetch, (Expression<Func<PlotGroundCover, object>>) (pgc => pgc.GroundCover)).List());
            this.m_boundPlot = p;
          }
        }
        else
        {
          this.m_plotGroundCovers = (DataBindingList<PlotGroundCover>) null;
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
        if (this.m_plotGroundCovers != null)
        {
          if (this.dgGroundCovers.DataSource != this.m_plotGroundCovers)
          {
            this.dgGroundCovers.DataSource = (object) this.m_plotGroundCovers;
            this.m_plotGroundCovers.Sortable = true;
            this.m_plotGroundCovers.AddComparer<PlotGroundCover>((Expression<Func<PlotGroundCover, object>>) (pgc => pgc.GroundCover), (IComparer) new PropertyComparer<GroundCover>((Func<GroundCover, object>) (gc => (object) gc.Description)));
            this.m_plotGroundCovers.AddingNew += new AddingNewEventHandler(this.GroundCovers_AddingNew);
            this.m_plotGroundCovers.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.GroundCovers_BeforeRemove);
            this.m_plotGroundCovers.ListChanged += new ListChangedEventHandler(this.GroundCovers_ListChanged);
          }
        }
        else
          this.dgGroundCovers.DataSource = (object) null;
        this.m_refresh = false;
        this.OnRequestRefresh();
      }
    }

    private int PercentCovered(
      DataBindingList<PlotGroundCover> dsPlotGroundCovers)
    {
      int num = 0;
      if (dsPlotGroundCovers != null)
      {
        int newRowIndex = this.dgGroundCovers.NewRowIndex;
        for (int index = 0; index < dsPlotGroundCovers.Count; ++index)
        {
          if (index != newRowIndex)
          {
            PlotGroundCover dsPlotGroundCover = dsPlotGroundCovers[index];
            num += dsPlotGroundCover.PercentCovered;
          }
        }
      }
      return num;
    }

    private void GroundCovers_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void GroundCovers_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<PlotGroundCover> dataBindingList = sender as DataBindingList<PlotGroundCover>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      PlotGroundCover plotGroundCover = dataBindingList[e.NewIndex];
      if (plotGroundCover.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) plotGroundCover);
          transaction.Commit();
        }
      }
    }

    private void GroundCovers_AddingNew(object sender, AddingNewEventArgs e)
    {
      int num = 100 - this.PercentCovered(sender as DataBindingList<PlotGroundCover>);
      e.NewObject = (object) new PlotGroundCover()
      {
        Plot = this.m_boundPlot,
        PercentCovered = num
      };
    }

    private void dgGroundCovers_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgGroundCovers.NewRowIndex || !(this.dgGroundCovers.DataSource is DataBindingList<PlotGroundCover> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgGroundCovers.Columns[e.ColumnIndex];
      PlotGroundCover plotGroundCover = dataSource[e.RowIndex];
      if (column == this.dcGroundCover)
      {
        if (plotGroundCover.GroundCover != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else
      {
        if (column != this.dcPctCovered || plotGroundCover.PercentCovered != 0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
    }

    private void dgGroundCovers_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<PlotGroundCover> dataSource = this.dgGroundCovers.DataSource as DataBindingList<PlotGroundCover>;
      if (this.dgGroundCovers.CurrentRow != null && !this.dgGroundCovers.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      PlotGroundCover plotGroundCover1 = dataSource[e.RowIndex];
      DataGridViewRow row = this.dgGroundCovers.Rows[e.RowIndex];
      DataGridViewCell dataGridViewCell1 = (DataGridViewCell) null;
      string text = (string) null;
      if (plotGroundCover1.GroundCover != null)
      {
        foreach (PlotGroundCover plotGroundCover2 in (Collection<PlotGroundCover>) dataSource)
        {
          if (plotGroundCover2 != plotGroundCover1 && plotGroundCover1.GroundCover.Equals((object) plotGroundCover2.GroundCover))
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcGroundCover.HeaderText);
            dataGridViewCell1 = row.Cells[this.dcGroundCover.DisplayIndex];
            break;
          }
        }
      }
      if (text == null)
      {
        DataGridViewCell dataGridViewCell2 = row.ErrorCell();
        if (dataGridViewCell2 != null)
          text = dataGridViewCell2.ErrorText;
      }
      if (text == null)
      {
        int num = this.PercentCovered(dataSource);
        if (num > 100)
          text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRange, (object) this.dcPctCovered.HeaderText, (object) 1.ToString("D"), (object) (100 + plotGroundCover1.PercentCovered - num).ToString("D"));
      }
      if (text == null)
        return;
      e.Cancel = true;
      int num1 = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgGroundCovers_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgGroundCovers.ReadOnly || this.m_boundPlot == null)
        return;
      DataBindingList<PlotGroundCover> dataSource = this.dgGroundCovers.DataSource as DataBindingList<PlotGroundCover>;
      if (this.dgGroundCovers.CurrentRow != null && this.dgGroundCovers.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          PlotGroundCover plotGroundCover = dataSource[e.RowIndex];
          if (plotGroundCover.IsTransient || this.m_session.IsDirty())
          {
            lock (this.m_syncobj)
            {
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                this.m_session.SaveOrUpdate((object) plotGroundCover);
                transaction.Commit();
              }
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgGroundCovers_CellEndEdit(object sender, DataGridViewCellEventArgs e) => this.OnRequestRefresh();

    private void dgGroundCovers_SelectionChanged(object sender, EventArgs e)
    {
      if (this.m_boundPlot == null)
        return;
      this.OnRequestRefresh();
    }

    private void DeleteSelectedRows()
    {
      if (!(this.dgGroundCovers.DataSource is DataBindingList<PlotGroundCover> dataSource))
        return;
      CurrencyManager currencyManager = this.dgGroundCovers.BindingContext[(object) dataSource] as CurrencyManager;
      bool flag = false;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgGroundCovers.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          dataSource.RemoveAt(selectedRow.Index);
          flag = true;
        }
      }
      if (!(currencyManager.Position != -1 & flag))
        return;
      this.dgGroundCovers.Rows[currencyManager.Position].Selected = true;
    }

    private void PlotGroundCoversForm_VisibleChanged(object sender, EventArgs e)
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
      this.dgGroundCovers.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgGroundCovers.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgGroundCovers.DefaultCellStyle = Program.ActiveGridDefaultCellStyle;
    }

    public void ContentDeactivated()
    {
      this.dgGroundCovers.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgGroundCovers.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dgGroundCovers.DefaultCellStyle = Program.InActiveGridDefaultCellStyle;
    }

    protected override void OnRequestRefresh()
    {
      DataBindingList<PlotGroundCover> dataSource = this.dgGroundCovers.DataSource as DataBindingList<PlotGroundCover>;
      int num = this.PercentCovered(dataSource);
      bool flag1 = this.m_year != null && this.m_year.Changed && dataSource != null;
      bool flag2 = flag1 && num < 100;
      if (dataSource != null)
        this.lblPctCovered.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtTotal, (object) this.dcPctCovered.HeaderText), (object) ((double) num / 100.0).ToString("P0"));
      else
        this.lblPctCovered.Text = string.Empty;
      this.dgGroundCovers.AllowUserToAddRows = flag2;
      this.dgGroundCovers.AllowUserToDeleteRows = flag1;
      this.dgGroundCovers.ReadOnly = !flag1;
      if (num == 100)
        this.lblPctCovered.ForeColor = Color.Green;
      else
        this.lblPctCovered.ForeColor = Color.Red;
      this.Enabled = dataSource != null;
      base.OnRequestRefresh();
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgGroundCovers.DataSource != null;
      bool flag2 = flag1 && this.dgGroundCovers.AllowUserToAddRows;
      bool flag3 = this.dgGroundCovers.SelectedRows.Count > 0;
      bool flag4 = this.dgGroundCovers.CurrentRow != null && this.dgGroundCovers.IsCurrentRowDirty;
      bool flag5 = this.dgGroundCovers.CurrentRow != null && this.dgGroundCovers.CurrentRow.IsNewRow;
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
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgGroundCovers.SelectedRows)
            selectedRow.Selected = false;
          this.dgGroundCovers.Rows[this.dgGroundCovers.NewRowIndex].Selected = true;
          this.dgGroundCovers.FirstDisplayedScrollingRowIndex = this.dgGroundCovers.NewRowIndex - this.dgGroundCovers.DisplayedRowCount(false) + 1;
          this.dgGroundCovers.CurrentCell = this.dgGroundCovers.Rows[this.dgGroundCovers.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          DataBindingList<PlotGroundCover> dataSource = this.dgGroundCovers.DataSource as DataBindingList<PlotGroundCover>;
          DataGridViewRow selectedRow1 = this.dgGroundCovers.SelectedRows[0];
          if (selectedRow1.Index >= dataSource.Count)
            break;
          int val2 = 100 - this.PercentCovered(dataSource);
          PlotGroundCover plotGroundCover = dataSource[selectedRow1.Index].Clone() as PlotGroundCover;
          plotGroundCover.PercentCovered = Math.Min(plotGroundCover.PercentCovered, val2);
          dataSource.Add(plotGroundCover);
          this.dgGroundCovers.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          this.DeleteSelectedRows();
          break;
      }
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgGroundCovers.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgGroundCovers.DataSource != null;

    private void dgGroundCovers_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgGroundCovers_EditingControlShowing(
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PlotGroundCoversForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      this.dgGroundCovers = new DataGridView();
      this.dcGroundCover = new DataGridViewComboBoxColumn();
      this.dcPctCovered = new DataGridViewNumericTextBoxColumn();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.lblPctCovered = new Label();
      ((ISupportInitialize) this.dgGroundCovers).BeginInit();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgGroundCovers.AllowUserToAddRows = false;
      this.dgGroundCovers.AllowUserToDeleteRows = false;
      this.dgGroundCovers.AllowUserToResizeRows = false;
      this.dgGroundCovers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgGroundCovers.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgGroundCovers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgGroundCovers.Columns.AddRange((DataGridViewColumn) this.dcGroundCover, (DataGridViewColumn) this.dcPctCovered);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgGroundCovers.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgGroundCovers, "dgGroundCovers");
      this.dgGroundCovers.EnableHeadersVisualStyles = false;
      this.dgGroundCovers.Name = "dgGroundCovers";
      this.dgGroundCovers.ReadOnly = true;
      this.dgGroundCovers.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgGroundCovers.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgGroundCovers.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgGroundCovers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgGroundCovers.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgGroundCovers_CellErrorTextNeeded);
      this.dgGroundCovers.DataError += new DataGridViewDataErrorEventHandler(this.dgGroundCovers_DataError);
      this.dgGroundCovers.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgGroundCovers_EditingControlShowing);
      this.dgGroundCovers.RowValidated += new DataGridViewCellEventHandler(this.dgGroundCovers_RowValidated);
      this.dgGroundCovers.RowValidating += new DataGridViewCellCancelEventHandler(this.dgGroundCovers_RowValidating);
      this.dgGroundCovers.SelectionChanged += new EventHandler(this.dgGroundCovers_SelectionChanged);
      this.dcGroundCover.DataPropertyName = "GroundCover";
      this.dcGroundCover.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcGroundCover, "dcGroundCover");
      this.dcGroundCover.Name = "dcGroundCover";
      this.dcGroundCover.ReadOnly = true;
      this.dcGroundCover.Resizable = DataGridViewTriState.True;
      this.dcGroundCover.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctCovered.DataPropertyName = "PercentCovered";
      this.dcPctCovered.DecimalPlaces = 0;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle3.Format = "N0";
      this.dcPctCovered.DefaultCellStyle = gridViewCellStyle3;
      this.dcPctCovered.Format = "#;#";
      this.dcPctCovered.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcPctCovered, "dcPctCovered");
      this.dcPctCovered.Name = "dcPctCovered";
      this.dcPctCovered.ReadOnly = true;
      this.dcPctCovered.Resizable = DataGridViewTriState.True;
      this.dcPctCovered.Signed = false;
      componentResourceManager.ApplyResources((object) this.flowLayoutPanel1, "flowLayoutPanel1");
      this.flowLayoutPanel1.Controls.Add((Control) this.lblPctCovered);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.lblPctCovered, "lblPctCovered");
      this.lblPctCovered.Name = "lblPctCovered";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CloseButtonVisible = false;
      this.Controls.Add((Control) this.dgGroundCovers);
      this.Controls.Add((Control) this.flowLayoutPanel1);
      this.DockAreas = DockAreas.DockBottom;
      this.Name = nameof (PlotGroundCoversForm);
      this.ShowHint = DockState.DockBottom;
      this.VisibleChanged += new EventHandler(this.PlotGroundCoversForm_VisibleChanged);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.flowLayoutPanel1, 0);
      this.Controls.SetChildIndex((Control) this.dgGroundCovers, 0);
      ((ISupportInitialize) this.dgGroundCovers).EndInit();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
