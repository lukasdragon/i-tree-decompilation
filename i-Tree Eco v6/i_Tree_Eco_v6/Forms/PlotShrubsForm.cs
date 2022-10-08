// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.PlotShrubsForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Views;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class PlotShrubsForm : ContentForm, IPlotContent, IActionable, IExportable
  {
    private ProgramSession m_ps;
    private Plot m_boundPlot;
    private Plot m_selectedPlot;
    private Year m_year;
    private ISession m_session;
    private DataBindingList<Shrub> m_shrubs;
    private DataGridViewManager m_dgManager;
    private TaskManager m_taskManager;
    private WaitCursor m_wc;
    private object m_syncobj;
    private int m_dgHorizPos;
    private bool m_refresh;
    private BindingList<SpeciesView> m_species;
    private IContainer components;
    private DataGridView dgShrubs;
    private Label lblTotalShrubArea;
    private FlowLayoutPanel flowLayoutPanel1;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewFilteredComboBoxColumn dcSpecies;
    private DataGridViewNumericTextBoxColumn dcHeight;
    private DataGridViewNumericTextBoxColumn dcPctShrubArea;
    private DataGridViewComboBoxColumn dcPctMissing;
    private DataGridViewTextBoxColumn dcComments;

    public PlotShrubsForm()
    {
      this.InitializeComponent();
      this.dcHeight.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.m_ps = Program.Session;
      this.m_dgManager = new DataGridViewManager(this.dgShrubs);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.m_syncobj = new object();
      this.dgShrubs.DoubleBuffered(true);
      this.dgShrubs.AutoGenerateColumns = false;
      this.dgShrubs.MultiSelect = false;
      this.dgShrubs.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgShrubs.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dgShrubs.CellEndEdit += new DataGridViewCellEventHandler(this.dgShrubs_CellEndEdit);
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));
      EventPublisher.Register<EntityUpdated<Shrub>>(new EventHandler<EntityUpdated<Shrub>>(this.Shrub_Updated));
      this.dcPctMissing.DefaultCellStyle.DataSourceNullValue = (object) PctMidRange.PRINV;
      this.dcPctShrubArea.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.Init();
    }

    private void Year_Updated(object sender, EntityUpdated<Year> e)
    {
      if (this.m_year == null || !(this.m_year.Guid == e.Guid))
        return;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.GetYear()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.OnRequestRefresh();
      }), scheduler));
    }

    private void Shrub_Updated(object sender, EntityUpdated<Shrub> e)
    {
      if (this.m_session == null)
        return;
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() =>
      {
        lock (this.m_syncobj)
        {
          using (ITransaction transaction = this.m_session.BeginTransaction())
          {
            Shrub proxy = this.m_session.Load<Shrub>((object) e.Guid);
            if (NHibernateUtil.IsInitialized((object) proxy))
              this.m_session.Refresh((object) proxy, LockMode.None);
            transaction.Commit();
          }
        }
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler));
    }

    private void GetYear()
    {
      if (this.m_session == null)
        return;
      using (this.m_session.BeginTransaction())
        this.m_session.Refresh((object) this.m_year);
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
        Year year = session.Get<Year>((object) this.m_ps.InputSession.YearKey);
        List<SpeciesView> list = this.m_ps.Species.Values.ToList<SpeciesView>();
        list.Sort((IComparer<SpeciesView>) new PropertyComparer<SpeciesView>((Func<SpeciesView, object>) (sp => (object) sp.CommonName)));
        lock (this.m_syncobj)
        {
          this.m_session = session;
          this.m_species = (BindingList<SpeciesView>) new ExtendedBindingList<SpeciesView>((IList<SpeciesView>) list);
          this.m_year = year;
        }
      }
    }

    private void InitGrid()
    {
      Year year = this.m_year;
      this.dcSpecies.BindTo<SpeciesView>((Expression<Func<SpeciesView, object>>) (sv => sv.CommonScientificName), (Expression<Func<SpeciesView, object>>) (sv => sv.Code), (object) this.m_species);
      this.dcPctMissing.BindTo("Value", "Key", (object) new BindingSource((object) EnumHelper.ConvertToDictionary<PctMidRange>(), (string) null));
      this.dcHeight.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.Height, year.Unit != YearUnit.English ? (object) i_Tree_Eco_v6.Resources.Strings.UnitMetersAbbr : (object) i_Tree_Eco_v6.Resources.Strings.UnitFeetAbbr);
      this.OnRequestRefresh();
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
        if (p == null || p.PercentShrubCover <= PctMidRange.PR0)
        {
          this.m_boundPlot = p;
          this.m_shrubs = (DataBindingList<Shrub>) null;
        }
        else
        {
          if (this.m_session == null)
            return;
          using (this.m_session.BeginTransaction())
          {
            this.m_shrubs = new DataBindingList<Shrub>(this.m_session.QueryOver<Shrub>().Where((Expression<Func<Shrub, bool>>) (s => s.Plot == p)).OrderBy((Expression<Func<Shrub, object>>) (s => (object) s.Id)).Asc.List());
            this.m_boundPlot = p;
          }
        }
      }
    }

    private void InitBoundPlot()
    {
      lock (this.m_syncobj)
      {
        if (this.m_boundPlot != this.m_selectedPlot)
          return;
        if (this.m_shrubs != null)
        {
          if (this.dgShrubs.DataSource != this.m_shrubs)
          {
            this.dgShrubs.DataSource = (object) this.m_shrubs;
            this.dgShrubs.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
            this.m_shrubs.AddingNew += new AddingNewEventHandler(this.Shrubs_AddingNew);
            this.m_shrubs.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.Shrubs_Removing);
            this.m_shrubs.ListChanged += new ListChangedEventHandler(this.Shrubs_ListChanged);
            this.m_shrubs.Sortable = true;
          }
        }
        else
          this.dgShrubs.DataSource = (object) null;
        this.m_refresh = false;
        this.OnRequestRefresh();
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

    private void Shrubs_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private int TotalShrubArea(DataBindingList<Shrub> dsShrubs)
    {
      int num = 0;
      if (dsShrubs != null)
      {
        int newRowIndex = this.dgShrubs.NewRowIndex;
        for (int index = 0; index < dsShrubs.Count; ++index)
        {
          if (index != newRowIndex)
          {
            Shrub dsShrub = dsShrubs[index];
            num += dsShrub.PercentOfShrubArea;
          }
        }
      }
      return num;
    }

    private void Shrubs_AddingNew(object sender, AddingNewEventArgs e)
    {
      DataBindingList<Shrub> dataBindingList = sender as DataBindingList<Shrub>;
      int num1 = 100;
      int num2 = 1;
      foreach (Shrub shrub in (Collection<Shrub>) dataBindingList)
      {
        num1 -= shrub.PercentOfShrubArea;
        if (shrub.Id >= num2)
          num2 = shrub.Id + 1;
      }
      e.NewObject = (object) new Shrub()
      {
        Id = num2,
        Plot = this.m_boundPlot,
        PercentOfShrubArea = num1,
        PercentMissing = PctMidRange.PR0
      };
    }

    private void Shrubs_Removing(object sender, ListChangedEventArgs e)
    {
      DataBindingList<Shrub> dataBindingList = sender as DataBindingList<Shrub>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      Shrub shrub = dataBindingList[e.NewIndex];
      if (shrub.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) shrub);
          transaction.Commit();
        }
      }
    }

    private void dgShrubs_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgShrubs.NewRowIndex || !(this.dgShrubs.DataSource is DataBindingList<Shrub> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgShrubs.Columns[e.ColumnIndex];
      Shrub shrub = dataSource[e.RowIndex];
      if (column == this.dcId)
      {
        if (shrub.Id != 0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcSpecies)
      {
        if (string.IsNullOrEmpty(shrub.Species))
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
        }
        else
        {
          DataGridViewCell cell = this.dgShrubs.Rows[e.RowIndex].Cells[e.ColumnIndex];
          if (cell.FormattedValue != null && !cell.FormattedValue.Equals((object) string.Empty))
            return;
          SpeciesView speciesView;
          if (this.m_ps.InvalidSpecies.TryGetValue(shrub.Species, out speciesView))
            e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrSpeciesInvalid, (object) speciesView.CommonScientificName);
          else
            e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrSpeciesCode, (object) shrub.Species);
        }
      }
      else if (column == this.dcPctShrubArea)
      {
        if (shrub.PercentOfShrubArea >= 1)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else
      {
        if (column != this.dcHeight)
          return;
        if ((double) shrub.Height <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) this.dcHeight.HeaderText, (object) 0);
        }
        else
        {
          if ((double) shrub.Height <= 99.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) this.dcHeight.HeaderText, (object) 99);
        }
      }
    }

    private void dgShrubs_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<Shrub> dataSource = this.dgShrubs.DataSource as DataBindingList<Shrub>;
      if (this.dgShrubs.CurrentRow != null && !this.dgShrubs.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      Shrub shrub1 = dataSource[e.RowIndex];
      DataGridViewRow row = this.dgShrubs.Rows[e.RowIndex];
      DataGridViewCell dataGridViewCell1 = (DataGridViewCell) null;
      string text = (string) null;
      foreach (Shrub shrub2 in (Collection<Shrub>) dataSource)
      {
        if (shrub2 != shrub1 && shrub2.Id == shrub1.Id)
        {
          text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcId.HeaderText);
          dataGridViewCell1 = row.Cells[this.dcId.DisplayIndex];
          break;
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
        int num = this.TotalShrubArea(dataSource);
        if (num > 100)
        {
          text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRange, (object) this.dcPctShrubArea.HeaderText, (object) 1, (object) (100 + shrub1.PercentOfShrubArea - num));
          dataGridViewCell1 = row.Cells[this.dcPctShrubArea.DisplayIndex];
        }
      }
      if (text == null)
        return;
      e.Cancel = true;
      int num1 = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgShrubs_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgShrubs.ReadOnly || this.m_boundPlot == null)
        return;
      DataBindingList<Shrub> dataSource = this.dgShrubs.DataSource as DataBindingList<Shrub>;
      if (this.dgShrubs.CurrentRow != null && this.dgShrubs.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          Shrub shrub = dataSource[e.RowIndex];
          if (shrub.IsTransient || this.m_session.IsDirty())
          {
            lock (this.m_syncobj)
            {
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                this.m_session.SaveOrUpdate((object) shrub);
                transaction.Commit();
              }
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void DeleteSelectedRows()
    {
      if (!(this.dgShrubs.DataSource is DataBindingList<Shrub> dataSource))
        return;
      CurrencyManager currencyManager = this.dgShrubs.BindingContext[(object) dataSource] as CurrencyManager;
      bool flag = false;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgShrubs.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          dataSource.RemoveAt(selectedRow.Index);
          flag = true;
        }
      }
      if (!(currencyManager.Position != -1 & flag))
        return;
      this.dgShrubs.Rows[currencyManager.Position].Selected = true;
    }

    private void dgShrubs_CellEndEdit(object sender, DataGridViewCellEventArgs e) => this.OnRequestRefresh();

    private void dgShrubs_SelectionChanged(object sender, EventArgs e)
    {
      if (this.m_boundPlot == null)
        return;
      this.OnRequestRefresh();
    }

    private void PlotShrubsForm_VisibleChanged(object sender, EventArgs e)
    {
      bool flag = false;
      lock (this.m_syncobj)
      {
        flag = this.m_selectedPlot == null ? this.m_boundPlot != null && this.Visible : !this.m_selectedPlot.Equals((object) this.m_boundPlot) && this.Visible;
        flag |= this.m_refresh;
      }
      if (!flag)
        return;
      this.LoadAndBindPlot();
    }

    private void dgShrubs_Scroll(object sender, ScrollEventArgs e)
    {
      if (e.ScrollOrientation != ScrollOrientation.HorizontalScroll)
        return;
      this.m_dgHorizPos = e.NewValue;
    }

    private void dgShrubs_Sorted(object sender, EventArgs e) => this.dgShrubs.HorizontalScrollingOffset = this.m_dgHorizPos;

    public void ContentActivated()
    {
      this.dgShrubs.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgShrubs.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgShrubs.DefaultCellStyle = Program.ActiveGridDefaultCellStyle;
    }

    public void ContentDeactivated()
    {
      this.dgShrubs.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgShrubs.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dgShrubs.DefaultCellStyle = Program.InActiveGridDefaultCellStyle;
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgShrubs.DataSource != null;
      bool flag2 = flag1 && this.dgShrubs.AllowUserToAddRows;
      bool flag3 = this.dgShrubs.SelectedRows.Count > 0;
      bool flag4 = this.dgShrubs.CurrentRow != null && this.dgShrubs.IsCurrentRowDirty;
      bool flag5 = this.dgShrubs.CurrentRow != null && this.dgShrubs.CurrentRow.IsNewRow;
      bool flag6 = false;
      switch (action)
      {
        case UserActions.New:
          flag6 = flag2 && !flag5 && !flag4;
          break;
        case UserActions.Copy:
          flag6 = flag2 & flag3 && !flag5 && !flag4;
          break;
        case UserActions.Undo:
          flag6 = flag1 && this.m_dgManager.CanUndo;
          break;
        case UserActions.Redo:
          flag6 = flag1 && this.m_dgManager.CanRedo;
          break;
        case UserActions.Delete:
          flag6 = flag1 & flag3 && !flag5 | flag4;
          break;
      }
      return flag6;
    }

    protected override void OnRequestRefresh()
    {
      DataBindingList<Shrub> dataSource = this.dgShrubs.DataSource as DataBindingList<Shrub>;
      int num = this.TotalShrubArea(dataSource);
      bool flag1 = this.m_year != null && this.m_year.Changed && dataSource != null;
      bool flag2 = flag1 && num < 100;
      if (this.m_boundPlot != null)
        this.lblTotalShrubArea.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtTotal, (object) this.dcPctShrubArea.HeaderText), (object) ((double) num / 100.0).ToString("P0"));
      else
        this.lblTotalShrubArea.Text = string.Empty;
      if (num == 100)
        this.lblTotalShrubArea.ForeColor = Color.Green;
      else
        this.lblTotalShrubArea.ForeColor = Color.Red;
      this.dgShrubs.AllowUserToAddRows = flag2;
      this.dgShrubs.AllowUserToDeleteRows = flag1;
      this.dgShrubs.ReadOnly = !flag1;
      this.Enabled = dataSource != null;
      base.OnRequestRefresh();
    }

    public void PerformAction(UserActions action)
    {
      if (!this.CanPerformAction(action))
        return;
      switch (action)
      {
        case UserActions.New:
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgShrubs.SelectedRows)
            selectedRow.Selected = false;
          this.dgShrubs.Rows[this.dgShrubs.NewRowIndex].Selected = true;
          this.dgShrubs.FirstDisplayedScrollingRowIndex = this.dgShrubs.NewRowIndex - this.dgShrubs.DisplayedRowCount(false) + 1;
          this.dgShrubs.CurrentCell = this.dgShrubs.Rows[this.dgShrubs.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          DataBindingList<Shrub> dataSource = this.dgShrubs.DataSource as DataBindingList<Shrub>;
          DataGridViewRow selectedRow1 = this.dgShrubs.SelectedRows[0];
          if (dataSource == null || selectedRow1.Index >= dataSource.Count)
            break;
          Shrub shrub1 = dataSource[selectedRow1.Index].Clone() as Shrub;
          int val2 = 100;
          int num = 1;
          foreach (Shrub shrub2 in (Collection<Shrub>) dataSource)
          {
            val2 -= shrub2.PercentOfShrubArea;
            if (shrub2.Id >= num)
              num = shrub2.Id + 1;
          }
          shrub1.Id = num;
          shrub1.PercentOfShrubArea = Math.Min(shrub1.PercentOfShrubArea, val2);
          dataSource.Add(shrub1);
          this.dgShrubs.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
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
      this.dgShrubs.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgShrubs.DataSource != null;

    private void dgShrubs_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgShrubs_EditingControlShowing(
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

    private void dgShrubs_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
    {
      DataGridViewCell currentCell = this.dgShrubs.CurrentCell;
      if (currentCell == null)
        return;
      DataGridViewColumn owningColumn = currentCell.OwningColumn;
      if (!(owningColumn is DataGridViewComboBoxColumn) || e.Value != null && !string.Empty.Equals(e.Value))
        return;
      e.Value = owningColumn.DefaultCellStyle.DataSourceNullValue;
      e.ParsingApplied = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PlotShrubsForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
      this.dgShrubs = new DataGridView();
      this.lblTotalShrubArea = new Label();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcSpecies = new DataGridViewFilteredComboBoxColumn();
      this.dcHeight = new DataGridViewNumericTextBoxColumn();
      this.dcPctShrubArea = new DataGridViewNumericTextBoxColumn();
      this.dcPctMissing = new DataGridViewComboBoxColumn();
      this.dcComments = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dgShrubs).BeginInit();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgShrubs.AllowUserToAddRows = false;
      this.dgShrubs.AllowUserToDeleteRows = false;
      this.dgShrubs.AllowUserToResizeRows = false;
      this.dgShrubs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgShrubs.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgShrubs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgShrubs.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcSpecies, (DataGridViewColumn) this.dcHeight, (DataGridViewColumn) this.dcPctShrubArea, (DataGridViewColumn) this.dcPctMissing, (DataGridViewColumn) this.dcComments);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgShrubs.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgShrubs, "dgShrubs");
      this.dgShrubs.EnableHeadersVisualStyles = false;
      this.dgShrubs.Name = "dgShrubs";
      this.dgShrubs.ReadOnly = true;
      this.dgShrubs.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = SystemColors.Control;
      gridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.ForeColor = SystemColors.WindowText;
      gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.dgShrubs.RowHeadersDefaultCellStyle = gridViewCellStyle3;
      this.dgShrubs.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgShrubs.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgShrubs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgShrubs.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgShrubs_CellErrorTextNeeded);
      this.dgShrubs.CellParsing += new DataGridViewCellParsingEventHandler(this.dgShrubs_CellParsing);
      this.dgShrubs.DataError += new DataGridViewDataErrorEventHandler(this.dgShrubs_DataError);
      this.dgShrubs.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgShrubs_EditingControlShowing);
      this.dgShrubs.RowValidated += new DataGridViewCellEventHandler(this.dgShrubs_RowValidated);
      this.dgShrubs.RowValidating += new DataGridViewCellCancelEventHandler(this.dgShrubs_RowValidating);
      this.dgShrubs.Scroll += new ScrollEventHandler(this.dgShrubs_Scroll);
      this.dgShrubs.SelectionChanged += new EventHandler(this.dgShrubs_SelectionChanged);
      this.dgShrubs.Sorted += new EventHandler(this.dgShrubs_Sorted);
      componentResourceManager.ApplyResources((object) this.lblTotalShrubArea, "lblTotalShrubArea");
      this.lblTotalShrubArea.Name = "lblTotalShrubArea";
      componentResourceManager.ApplyResources((object) this.flowLayoutPanel1, "flowLayoutPanel1");
      this.flowLayoutPanel1.Controls.Add((Control) this.lblTotalShrubArea);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.dcId.DataPropertyName = "Id";
      this.dcId.DecimalPlaces = 0;
      this.dcId.Format = "#;#";
      this.dcId.Frozen = true;
      this.dcId.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcId, "dcId");
      this.dcId.Name = "dcId";
      this.dcId.ReadOnly = true;
      this.dcId.Signed = false;
      this.dcSpecies.DataPropertyName = "Species";
      this.dcSpecies.DataSource = (object) null;
      this.dcSpecies.DisplayMember = (string) null;
      componentResourceManager.ApplyResources((object) this.dcSpecies, "dcSpecies");
      this.dcSpecies.Name = "dcSpecies";
      this.dcSpecies.ReadOnly = true;
      this.dcSpecies.Resizable = DataGridViewTriState.True;
      this.dcSpecies.ValueMember = (string) null;
      this.dcHeight.DataPropertyName = "Height";
      this.dcHeight.DecimalPlaces = 2;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle4.Format = "N2";
      this.dcHeight.DefaultCellStyle = gridViewCellStyle4;
      this.dcHeight.Format = "#.#;#.#";
      this.dcHeight.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcHeight, "dcHeight");
      this.dcHeight.Name = "dcHeight";
      this.dcHeight.ReadOnly = true;
      this.dcHeight.Signed = false;
      this.dcPctShrubArea.DataPropertyName = "PercentOfShrubArea";
      this.dcPctShrubArea.DecimalPlaces = 0;
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle5.Format = "N0";
      this.dcPctShrubArea.DefaultCellStyle = gridViewCellStyle5;
      this.dcPctShrubArea.Format = "#;#";
      this.dcPctShrubArea.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcPctShrubArea, "dcPctShrubArea");
      this.dcPctShrubArea.Name = "dcPctShrubArea";
      this.dcPctShrubArea.ReadOnly = true;
      this.dcPctShrubArea.Signed = false;
      this.dcPctMissing.DataPropertyName = "PercentMissing";
      gridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcPctMissing.DefaultCellStyle = gridViewCellStyle6;
      this.dcPctMissing.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcPctMissing, "dcPctMissing");
      this.dcPctMissing.Name = "dcPctMissing";
      this.dcPctMissing.ReadOnly = true;
      this.dcPctMissing.Resizable = DataGridViewTriState.True;
      this.dcPctMissing.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcComments.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcComments.DataPropertyName = "Comments";
      componentResourceManager.ApplyResources((object) this.dcComments, "dcComments");
      this.dcComments.MaxInputLength = (int) byte.MaxValue;
      this.dcComments.Name = "dcComments";
      this.dcComments.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CloseButtonVisible = false;
      this.Controls.Add((Control) this.dgShrubs);
      this.Controls.Add((Control) this.flowLayoutPanel1);
      this.DockAreas = DockAreas.DockBottom;
      this.Name = nameof (PlotShrubsForm);
      this.VisibleChanged += new EventHandler(this.PlotShrubsForm_VisibleChanged);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.flowLayoutPanel1, 0);
      this.Controls.SetChildIndex((Control) this.dgShrubs, 0);
      ((ISupportInitialize) this.dgShrubs).EndInit();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
