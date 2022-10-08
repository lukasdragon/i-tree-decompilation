// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.GroundCoversDataForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Constraints;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Interfaces;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class GroundCoversDataForm : DataContentForm, IActionable, IExportable
  {
    private DataGridViewManager _dgManager;
    private IList<PlotGroundCover> _plotGroundCovers;
    private IList<GroundCover> _groundCovers;
    private IList<Plot> _plots;
    private IContainer components;
    private DataGridView dgGroundCovers;
    private DataGridViewComboBoxColumn dcPlot;
    private DataGridViewComboBoxColumn dcGroundCover;
    private DataGridViewNumericTextBoxColumn dcPctCovered;
    private TableLayoutPanel pnlEditHelp;

    public GroundCoversDataForm()
    {
      this.InitializeComponent();
      this._dgManager = new DataGridViewManager(this.dgGroundCovers);
      this.dgGroundCovers.DoubleBuffered(true);
      this.dgGroundCovers.AutoGenerateColumns = false;
      this.dgGroundCovers.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgGroundCovers.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgGroundCovers.CellEndEdit += new DataGridViewCellEventHandler(this.dgGroundCovers_CellEndEdit);
      this.dcPctCovered.DefaultCellStyle.DataSourceNullValue = (object) 0;
    }

    protected override void LoadData()
    {
      base.LoadData();
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          try
          {
            this._plotGroundCovers = this.Session.QueryOver<PlotGroundCover>().JoinQueryOver<Plot>((Expression<Func<PlotGroundCover, Plot>>) (gc => gc.Plot)).Where((Expression<Func<Plot, bool>>) (p => p.Year == this.Year)).OrderBy((Expression<Func<Plot, object>>) (p => (object) p.Id)).Asc.List();
            this._plots = this.Session.QueryOver<Plot>().Where((Expression<Func<Plot, bool>>) (p => p.Year == this.Year)).Fetch<Plot, Plot>(SelectMode.Fetch, (Expression<Func<Plot, object>>) (p => p.PlotGroundCovers)).OrderBy((Expression<Func<Plot, object>>) (p => (object) p.Id)).Asc.TransformUsing(Transformers.DistinctRootEntity).List();
            this._groundCovers = (IList<GroundCover>) this.Session.QueryOver<GroundCover>().Where((Expression<Func<GroundCover, bool>>) (gc => gc.Year == this.Year)).OrderBy((Expression<Func<GroundCover, object>>) (gc => gc.Description)).Asc.List().Select<GroundCover, GroundCover>((Func<GroundCover, GroundCover>) (gc => gc.Self)).ToList<GroundCover>();
            transaction.Commit();
          }
          catch (HibernateException ex)
          {
            transaction.Rollback();
          }
        }
      }
    }

    protected override void OnDataLoaded()
    {
      this.InitGrid();
      base.OnDataLoaded();
    }

    private void InitGrid()
    {
      this.dcPlot.BindTo<Plot>((Expression<Func<Plot, object>>) (p => (object) p.Id), (Expression<Func<Plot, object>>) (p => p.Self), (object) this._plots);
      this.dcGroundCover.BindTo<GroundCover>((Expression<Func<GroundCover, object>>) (gc => gc.Description), (Expression<Func<GroundCover, object>>) (gc => gc.Self), (object) this._groundCovers);
      DataBindingList<PlotGroundCover> dataBindingList = new DataBindingList<PlotGroundCover>(this._plotGroundCovers);
      this.dgGroundCovers.DataSource = (object) dataBindingList;
      this.dgGroundCovers.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddComparer<Plot>((IComparer) new PropertyComparer<Plot>((Func<Plot, object>) (p => (object) p.Id)));
      dataBindingList.AddComparer<GroundCover>((IComparer) new PropertyComparer<GroundCover>((Func<GroundCover, object>) (gc => (object) gc.Description)));
      dataBindingList.AddingNew += new AddingNewEventHandler(this.PlotGroundCovers_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.PlotGroundCovers_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.PlotGroundCovers_ListChanged);
    }

    private void PlotGroundCovers_AddingNew(object sender, AddingNewEventArgs e) => e.NewObject = (object) new PlotGroundCover();

    private void PlotGroundCovers_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<PlotGroundCover> dataBindingList = sender as DataBindingList<PlotGroundCover>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      PlotGroundCover plotGroundCover = dataBindingList[e.NewIndex];
      if (plotGroundCover.IsTransient)
        return;
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          plotGroundCover.Plot?.PlotGroundCovers.Remove(plotGroundCover);
          this.Session.Delete((object) plotGroundCover);
          transaction.Commit();
        }
      }
    }

    private void PlotGroundCovers_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void dgGroundCovers_CellEndEdit(object sender, DataGridViewCellEventArgs e) => this.OnRequestRefresh();

    protected override void OnRequestRefresh()
    {
      Year year = this.Year;
      bool flag = year != null && year.Changed && this.dgGroundCovers.DataSource != null && this._plots != null && this._plots.Count > 0;
      this.dgGroundCovers.ReadOnly = !flag;
      this.dgGroundCovers.AllowUserToAddRows = flag;
      this.dgGroundCovers.AllowUserToDeleteRows = flag;
      base.OnRequestRefresh();
    }

    private void dgGroundCovers_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgGroundCovers.NewRowIndex || !(this.dgGroundCovers.DataSource is DataBindingList<PlotGroundCover> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgGroundCovers.Columns[e.ColumnIndex];
      PlotGroundCover plotGroundCover = dataSource[e.RowIndex];
      if (column == this.dcPlot)
      {
        if (plotGroundCover.Plot != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcGroundCover)
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
      if (this.dgGroundCovers.CurrentRow != null && !this.dgGroundCovers.IsCurrentRowDirty || !(this.dgGroundCovers.DataSource is DataBindingList<PlotGroundCover> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      PlotGroundCover pgc = dataSource[e.RowIndex];
      DataGridViewRow row = this.dgGroundCovers.Rows[e.RowIndex];
      string text = row.ErrorCell()?.ErrorText;
      if (text == null && pgc.GroundCover != null)
      {
        foreach (PlotGroundCover plotGroundCover in (IEnumerable<PlotGroundCover>) pgc.Plot.PlotGroundCovers)
        {
          if (plotGroundCover != pgc && pgc.GroundCover.Equals((object) plotGroundCover.GroundCover))
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcGroundCover.HeaderText);
            DataGridViewCell cell = row.Cells[this.dcGroundCover.DisplayIndex];
            break;
          }
        }
      }
      if (text == null && GroundCoversDataForm.PercentCovered(pgc) > 100)
        text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldTotal, (object) this.dcPctCovered.HeaderText, (object) 100);
      if (text == null)
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private static int PercentCovered(PlotGroundCover pgc)
    {
      int num = 0;
      foreach (PlotGroundCover plotGroundCover in (IEnumerable<PlotGroundCover>) pgc.Plot.PlotGroundCovers)
        num += plotGroundCover.PercentCovered;
      return num;
    }

    private void dgGroundCovers_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgGroundCovers.ReadOnly)
        return;
      if (this.dgGroundCovers.CurrentRow != null && this.dgGroundCovers.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (this.dgGroundCovers.DataSource is DataBindingList<PlotGroundCover> dataSource && e.RowIndex < dataSource.Count)
        {
          PlotGroundCover plotGroundCover = dataSource[e.RowIndex];
          if (plotGroundCover.IsTransient || this.Session.IsDirty())
          {
            lock (this.Session)
            {
              using (ITransaction transaction = this.Session.BeginTransaction())
              {
                plotGroundCover.Plot.PlotGroundCovers.Add(plotGroundCover);
                this.Session.SaveOrUpdate((object) plotGroundCover);
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

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.Year != null && this.Year.Changed && this.dgGroundCovers.DataSource != null;
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
          flag6 = flag1 && this._dgManager.CanUndo;
          break;
        case UserActions.Redo:
          flag6 = flag1 && this._dgManager.CanRedo;
          break;
        case UserActions.Delete:
          flag6 = flag1 & flag3 && (flag4 || !flag5);
          break;
        case UserActions.ImportData:
          flag6 = flag2 && !flag5 && !flag4;
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
          PlotGroundCover pgc = dataSource[selectedRow1.Index].Clone() as PlotGroundCover;
          int val2 = 100 - GroundCoversDataForm.PercentCovered(pgc);
          pgc.PercentCovered = Math.Min(pgc.PercentCovered, val2);
          dataSource.Add(pgc);
          this.dgGroundCovers.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this._dgManager.Undo();
          break;
        case UserActions.Redo:
          this._dgManager.Redo();
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

    private void dgGroundCovers_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      DataGridViewCell currentCell = this.dgGroundCovers.CurrentCell;
      if (!this.dgGroundCovers.IsCurrentCellDirty || currentCell == null || !(this.dgGroundCovers.DataSource is DataBindingList<PlotGroundCover> dataSource) || currentCell.RowIndex >= dataSource.Count || currentCell.ColumnIndex != this.dcPlot.Index)
        return;
      PlotGroundCover plotGroundCover = dataSource[currentCell.RowIndex];
      plotGroundCover.Plot?.PlotGroundCovers.Remove(plotGroundCover);
    }

    private void dgGroundCovers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (!(this.dgGroundCovers.DataSource is DataBindingList<PlotGroundCover> dataSource) || e.RowIndex >= dataSource.Count || e.ColumnIndex != this.dcPlot.Index)
        return;
      PlotGroundCover plotGroundCover = dataSource[e.RowIndex];
      plotGroundCover.Plot?.PlotGroundCovers.Add(plotGroundCover);
    }

    public Eco.Util.ImportSpec ImportSpec()
    {
      Eco.Util.ImportSpec<PlotGroundCover> importSpec1 = new Eco.Util.ImportSpec<PlotGroundCover>();
      importSpec1.RecordCount = this._plotGroundCovers.Count;
      Eco.Util.ImportSpec importSpec2 = (Eco.Util.ImportSpec) importSpec1;
      List<FieldSpec> fieldsSpecs = importSpec2.FieldsSpecs;
      fieldsSpecs.Add(new FieldSpec<PlotGroundCover>((Expression<Func<PlotGroundCover, object>>) (pgc => pgc.Plot), this.dcPlot.HeaderText, true).AddFormat((FieldFormat) new FieldFormat<Plot>((Expression<Func<Plot, object>>) (p => (object) p.Id))).SetData(this.dcPlot.DataSource, TypeHelper.NameOf<Plot>((Expression<Func<Plot, object>>) (p => p.Self))));
      fieldsSpecs.Add(new FieldSpec<PlotGroundCover>((Expression<Func<PlotGroundCover, object>>) (pgc => pgc.GroundCover), this.dcGroundCover.HeaderText, true).AddFormat((FieldFormat) new FieldFormat<GroundCover>((Expression<Func<GroundCover, object>>) (gc => (object) gc.Id))).AddFormat((FieldFormat) new FieldFormat<GroundCover>((Expression<Func<GroundCover, object>>) (gc => gc.Description))).SetData((object) this._groundCovers, TypeHelper.NameOf<GroundCover>((Expression<Func<GroundCover, object>>) (gc => gc.Self))));
      List<FieldSpec> fieldSpecList = fieldsSpecs;
      FieldSpec<PlotGroundCover> fieldSpec1 = new FieldSpec<PlotGroundCover>((Expression<Func<PlotGroundCover, object>>) (pgc => (object) pgc.PercentCovered), this.dcPctCovered.HeaderText, true);
      AndConstraint constraint1 = new AndConstraint((AConstraint) new GtConstraint((object) 0), (AConstraint) new LtEqConstraint((object) 100));
      string errFieldRange = i_Tree_Eco_v6.Resources.Strings.ErrFieldRange;
      string headerText = this.dcPctCovered.HeaderText;
      int num = 0;
      string str1 = num.ToString("P0");
      num = 100;
      string str2 = num.ToString("P0");
      string errorFormat = string.Format(errFieldRange, (object) headerText, (object) str1, (object) str2);
      FieldConstraint constraint2 = new FieldConstraint((AConstraint) constraint1, errorFormat);
      FieldSpec fieldSpec2 = fieldSpec1.AddConstraint(constraint2);
      fieldSpecList.Add(fieldSpec2);
      return importSpec2;
    }

    public Task ImportData(
      IList data,
      IProgress<ProgressEventArgs> progress,
      CancellationToken ct)
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      return Task.Factory.StartNew<IList<PlotGroundCover>>((Func<IList<PlotGroundCover>>) (() => this.DoImport(data, progress, ct)), ct, TaskCreationOptions.None, Program.Session.Scheduler).ContinueWith((System.Action<Task<IList<PlotGroundCover>>>) (t =>
      {
        if (this.Year == null || t.IsCanceled || t.IsFaulted)
          return;
        IList<PlotGroundCover> result = t.Result;
        lock (this.Session)
        {
          DataBindingList<PlotGroundCover> dataSource = this.dgGroundCovers.DataSource as DataBindingList<PlotGroundCover>;
          foreach (PlotGroundCover plotGroundCover in (IEnumerable<PlotGroundCover>) result)
            dataSource.Add(plotGroundCover);
        }
      }), scheduler);
    }

    private IList<PlotGroundCover> DoImport(
      IList data,
      IProgress<ProgressEventArgs> progress,
      CancellationToken ct)
    {
      IList<PlotGroundCover> plotGroundCoverList = (IList<PlotGroundCover>) new List<PlotGroundCover>();
      lock (this.Session)
      {
        int count = data.Count;
        int num1 = Math.Max(Math.Min(count / 100, 1000), 1);
        int num2 = 0;
        ITransaction transaction1 = this.Session.BeginTransaction();
        IList<Guid> guidList = (IList<Guid>) new List<Guid>();
        try
        {
          foreach (object obj in (IEnumerable) data)
          {
            ct.ThrowIfCancellationRequested();
            if (obj is PlotGroundCover plotGroundCover)
            {
              plotGroundCover.Plot?.PlotGroundCovers.Add(plotGroundCover);
              this.Session.Persist((object) plotGroundCover);
              guidList.Add(plotGroundCover.Guid);
              plotGroundCoverList.Add(plotGroundCover);
              ++num2;
              if (num2 % num1 == 0)
              {
                transaction1.Commit();
                transaction1.Dispose();
                this.Session.Flush();
                this.Session.Clear();
                transaction1 = this.Session.BeginTransaction();
                progress.Report(new ProgressEventArgs(i_Tree_Eco_v6.Resources.Strings.MsgImporting, count / num1, num2 / num1));
              }
            }
          }
          transaction1.Commit();
          transaction1.Dispose();
        }
        catch (Exception ex) when (ex is OperationCanceledException || ex is HibernateException)
        {
          transaction1.Rollback();
          transaction1.Dispose();
          ITransaction transaction2 = this.Session.BeginTransaction();
          for (int index = 0; index < num2 / num1 * num1; ++index)
          {
            this.Session.Delete((object) this.Session.Load<PlotGroundCover>((object) guidList[index]));
            if (index % num1 == 0)
            {
              transaction2.Commit();
              transaction2.Dispose();
              this.Session.Flush();
              transaction2 = this.Session.BeginTransaction();
              progress.Report(new ProgressEventArgs(i_Tree_Eco_v6.Resources.Strings.MsgCanceling, count / num1, (num2 - 1) / num1));
            }
          }
          transaction2.Commit();
          transaction2.Dispose();
        }
        ct.ThrowIfCancellationRequested();
      }
      return plotGroundCoverList;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
      this.dgGroundCovers = new DataGridView();
      this.dcPlot = new DataGridViewComboBoxColumn();
      this.dcGroundCover = new DataGridViewComboBoxColumn();
      this.dcPctCovered = new DataGridViewNumericTextBoxColumn();
      this.pnlEditHelp = new TableLayoutPanel();
      ((ISupportInitialize) this.dgGroundCovers).BeginInit();
      this.SuspendLayout();
      this.lblBreadcrumb.Margin = new Padding(9, 10, 9, 10);
      this.dgGroundCovers.AllowUserToAddRows = false;
      this.dgGroundCovers.AllowUserToDeleteRows = false;
      this.dgGroundCovers.AllowUserToResizeRows = false;
      this.dgGroundCovers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgGroundCovers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgGroundCovers.Columns.AddRange((DataGridViewColumn) this.dcPlot, (DataGridViewColumn) this.dcGroundCover, (DataGridViewColumn) this.dcPctCovered);
      this.dgGroundCovers.Dock = DockStyle.Fill;
      this.dgGroundCovers.EnableHeadersVisualStyles = false;
      this.dgGroundCovers.Location = new Point(0, 80);
      this.dgGroundCovers.Margin = new Padding(4);
      this.dgGroundCovers.Name = "dgGroundCovers";
      this.dgGroundCovers.ReadOnly = true;
      this.dgGroundCovers.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgGroundCovers.RowHeadersWidth = 20;
      this.dgGroundCovers.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgGroundCovers.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgGroundCovers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgGroundCovers.Size = new Size(1067, 543);
      this.dgGroundCovers.TabIndex = 2;
      this.dgGroundCovers.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgGroundCovers_CellErrorTextNeeded);
      this.dgGroundCovers.CellValueChanged += new DataGridViewCellEventHandler(this.dgGroundCovers_CellValueChanged);
      this.dgGroundCovers.CurrentCellDirtyStateChanged += new EventHandler(this.dgGroundCovers_CurrentCellDirtyStateChanged);
      this.dgGroundCovers.DataError += new DataGridViewDataErrorEventHandler(this.dgGroundCovers_DataError);
      this.dgGroundCovers.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgGroundCovers_EditingControlShowing);
      this.dgGroundCovers.RowValidated += new DataGridViewCellEventHandler(this.dgGroundCovers_RowValidated);
      this.dgGroundCovers.RowValidating += new DataGridViewCellCancelEventHandler(this.dgGroundCovers_RowValidating);
      this.dcPlot.DataPropertyName = "Plot";
      this.dcPlot.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcPlot.Frozen = true;
      this.dcPlot.HeaderText = "Plot";
      this.dcPlot.Name = "dcPlot";
      this.dcPlot.ReadOnly = true;
      this.dcPlot.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcGroundCover.DataPropertyName = "GroundCover";
      this.dcGroundCover.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcGroundCover.HeaderText = "Ground Cover";
      this.dcGroundCover.Name = "dcGroundCover";
      this.dcGroundCover.ReadOnly = true;
      this.dcGroundCover.Resizable = DataGridViewTriState.True;
      this.dcGroundCover.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctCovered.DataPropertyName = "PercentCovered";
      this.dcPctCovered.DecimalPlaces = 0;
      gridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle.Format = "N0";
      this.dcPctCovered.DefaultCellStyle = gridViewCellStyle;
      this.dcPctCovered.Format = "#;#";
      this.dcPctCovered.HasDecimal = false;
      this.dcPctCovered.HeaderText = "% of Plot";
      this.dcPctCovered.Name = "dcPctCovered";
      this.dcPctCovered.ReadOnly = true;
      this.dcPctCovered.Resizable = DataGridViewTriState.True;
      this.dcPctCovered.Signed = false;
      this.pnlEditHelp.AutoSize = true;
      this.pnlEditHelp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlEditHelp.ColumnCount = 1;
      this.pnlEditHelp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.pnlEditHelp.Dock = DockStyle.Top;
      this.pnlEditHelp.Location = new Point(0, 80);
      this.pnlEditHelp.Margin = new Padding(4);
      this.pnlEditHelp.Name = "pnlEditHelp";
      this.pnlEditHelp.RowCount = 1;
      this.pnlEditHelp.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
      this.pnlEditHelp.Size = new Size(1067, 0);
      this.pnlEditHelp.TabIndex = 14;
      this.AutoScaleDimensions = new SizeF(8f, 18f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1067, 623);
      this.Controls.Add((Control) this.dgGroundCovers);
      this.Controls.Add((Control) this.pnlEditHelp);
      this.Margin = new Padding(5, 6, 5, 6);
      this.Name = nameof (GroundCoversDataForm);
      this.Text = "Ground Covers";
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.pnlEditHelp, 0);
      this.Controls.SetChildIndex((Control) this.dgGroundCovers, 0);
      ((ISupportInitialize) this.dgGroundCovers).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
