// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.LandUsesDataForm
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
  public class LandUsesDataForm : DataContentForm, IActionable, IExportable
  {
    private DataGridViewManager _dgManager;
    private IList<PlotLandUse> _plotLandUses;
    private IList<LandUse> _landUses;
    private IList<Plot> _plots;
    private IContainer components;
    private DataGridView dgLandUses;
    private DataGridViewComboBoxColumn dcPlot;
    private DataGridViewComboBoxColumn dcLandUse;
    private DataGridViewNumericTextBoxColumn dcPctCovered;

    public LandUsesDataForm()
    {
      this.InitializeComponent();
      this._dgManager = new DataGridViewManager(this.dgLandUses);
      this.dgLandUses.DoubleBuffered(true);
      this.dgLandUses.AutoGenerateColumns = false;
      this.dgLandUses.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgLandUses.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgLandUses.CellEndEdit += new DataGridViewCellEventHandler(this.dgLandUses_CellEndEdit);
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
            this._plotLandUses = this.Session.QueryOver<PlotLandUse>().JoinQueryOver<Plot>((Expression<Func<PlotLandUse, Plot>>) (lu => lu.Plot)).Where((Expression<Func<Plot, bool>>) (p => p.Year == this.Year)).OrderBy((Expression<Func<Plot, object>>) (p => (object) p.Id)).Asc.List();
            this._plots = this.Session.QueryOver<Plot>().Where((Expression<Func<Plot, bool>>) (p => p.Year == this.Year)).Fetch<Plot, Plot>(SelectMode.Fetch, (Expression<Func<Plot, object>>) (p => p.PlotLandUses)).OrderBy((Expression<Func<Plot, object>>) (p => (object) p.Id)).Asc.TransformUsing(Transformers.DistinctRootEntity).List();
            this._landUses = (IList<LandUse>) this.Session.QueryOver<LandUse>().Where((Expression<Func<LandUse, bool>>) (lu => lu.Year == this.Year)).OrderBy((Expression<Func<LandUse, object>>) (lu => lu.Description)).Asc.List().Select<LandUse, LandUse>((Func<LandUse, LandUse>) (lu => lu.Self)).ToList<LandUse>();
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
      this.dcLandUse.BindTo<LandUse>((Expression<Func<LandUse, object>>) (lu => lu.Description), (Expression<Func<LandUse, object>>) (lu => lu.Self), (object) this._landUses);
      DataBindingList<PlotLandUse> dataBindingList = new DataBindingList<PlotLandUse>(this._plotLandUses);
      this.dgLandUses.DataSource = (object) dataBindingList;
      this.dgLandUses.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddComparer<Plot>((IComparer) new PropertyComparer<Plot>((Func<Plot, object>) (p => (object) p.Id)));
      dataBindingList.AddComparer<LandUse>((IComparer) new PropertyComparer<LandUse>((Func<LandUse, object>) (lu => (object) lu.Description)));
      dataBindingList.AddingNew += new AddingNewEventHandler(this.PlotLandUses_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.PlotLandUses_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.PlotLandUses_ListChanged);
    }

    private void PlotLandUses_AddingNew(object sender, AddingNewEventArgs e) => e.NewObject = (object) new PlotLandUse();

    private void PlotLandUses_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<PlotLandUse> dataBindingList = sender as DataBindingList<PlotLandUse>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      PlotLandUse plotLandUse = dataBindingList[e.NewIndex];
      if (plotLandUse.IsTransient)
        return;
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          plotLandUse.Plot?.PlotLandUses.Remove(plotLandUse);
          this.Session.Delete((object) plotLandUse);
          transaction.Commit();
        }
      }
    }

    private void PlotLandUses_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void dgLandUses_CellEndEdit(object sender, DataGridViewCellEventArgs e) => this.OnRequestRefresh();

    protected override void OnRequestRefresh()
    {
      Year year = this.Year;
      bool flag = year != null && year.Changed && this.dgLandUses.DataSource != null && this._plots != null && this._plots.Count > 0;
      this.dgLandUses.ReadOnly = !flag;
      this.dgLandUses.AllowUserToAddRows = flag;
      base.OnRequestRefresh();
    }

    private void dgLandUses_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgLandUses.NewRowIndex || !(this.dgLandUses.DataSource is DataBindingList<PlotLandUse> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgLandUses.Columns[e.ColumnIndex];
      PlotLandUse plotLandUse = dataSource[e.RowIndex];
      if (column == this.dcPlot)
      {
        if (plotLandUse.Plot != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcLandUse)
      {
        if (plotLandUse.LandUse != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else
      {
        if (column != this.dcPctCovered || plotLandUse.PercentOfPlot != (short) 0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
    }

    private void dgLandUses_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.dgLandUses.CurrentRow != null && !this.dgLandUses.IsCurrentRowDirty || !(this.dgLandUses.DataSource is DataBindingList<PlotLandUse> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      PlotLandUse plotLandUse1 = dataSource[e.RowIndex];
      DataGridViewRow row = this.dgLandUses.Rows[e.RowIndex];
      string text = row.ErrorCell()?.ErrorText;
      if (text == null && plotLandUse1.LandUse != null)
      {
        foreach (PlotLandUse plotLandUse2 in (IEnumerable<PlotLandUse>) plotLandUse1.Plot.PlotLandUses)
        {
          if (plotLandUse2 != plotLandUse1 && plotLandUse1.LandUse.Equals((object) plotLandUse2.LandUse))
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcLandUse.HeaderText);
            DataGridViewCell cell = row.Cells[this.dcLandUse.DisplayIndex];
            break;
          }
        }
      }
      if (text == null && (int) LandUsesDataForm.PercentCovered(plotLandUse1.Plot) + (int) plotLandUse1.PercentOfPlot > 100)
        text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldTotal, (object) this.dcPctCovered.HeaderText, (object) 100);
      if (text == null)
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private static short PercentCovered(Plot p)
    {
      short num = 0;
      foreach (PlotLandUse plotLandUse in (IEnumerable<PlotLandUse>) p.PlotLandUses)
        num += plotLandUse.PercentOfPlot;
      return num;
    }

    private void dgLandUses_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgLandUses.ReadOnly)
        return;
      if (this.dgLandUses.CurrentRow != null && this.dgLandUses.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (this.dgLandUses.DataSource is DataBindingList<PlotLandUse> dataSource && e.RowIndex < dataSource.Count)
        {
          PlotLandUse plotLandUse = dataSource[e.RowIndex];
          if (plotLandUse.IsTransient || this.Session.IsDirty())
          {
            lock (this.Session)
            {
              using (ITransaction transaction = this.Session.BeginTransaction())
              {
                plotLandUse.Plot.PlotLandUses.Add(plotLandUse);
                this.Session.SaveOrUpdate((object) plotLandUse);
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
      if (!(this.dgLandUses.DataSource is DataBindingList<PlotLandUse> dataSource))
        return;
      CurrencyManager currencyManager = this.dgLandUses.BindingContext[(object) dataSource] as CurrencyManager;
      bool flag = false;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgLandUses.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          dataSource.RemoveAt(selectedRow.Index);
          flag = true;
        }
      }
      if (!(currencyManager.Position != -1 & flag))
        return;
      this.dgLandUses.Rows[currencyManager.Position].Selected = true;
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

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.Year != null && this.Year.Changed && this.dgLandUses.DataSource != null;
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
          flag6 = flag1 && this._dgManager.CanUndo;
          break;
        case UserActions.Redo:
          flag6 = flag1 && this._dgManager.CanRedo;
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
          foreach (DataGridViewBand row in (IEnumerable) this.dgLandUses.Rows)
            row.Selected = false;
          this.dgLandUses.Rows[this.dgLandUses.NewRowIndex].Selected = true;
          this.dgLandUses.FirstDisplayedScrollingRowIndex = this.dgLandUses.NewRowIndex - this.dgLandUses.DisplayedRowCount(false) + 1;
          this.dgLandUses.CurrentCell = this.dgLandUses.Rows[this.dgLandUses.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          DataBindingList<PlotLandUse> dataSource = this.dgLandUses.DataSource as DataBindingList<PlotLandUse>;
          DataGridViewRow selectedRow = this.dgLandUses.SelectedRows[0];
          if (selectedRow.Index >= dataSource.Count)
            break;
          PlotLandUse plotLandUse = dataSource[selectedRow.Index].Clone() as PlotLandUse;
          short val2 = (short) (100 - (int) LandUsesDataForm.PercentCovered(plotLandUse.Plot));
          plotLandUse.PercentOfPlot = Math.Min(plotLandUse.PercentOfPlot, val2);
          dataSource.Add(plotLandUse);
          this.dgLandUses.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this._dgManager.Undo();
          break;
        case UserActions.Redo:
          this._dgManager.Redo();
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

    private void dgLandUses_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (!(this.dgLandUses.DataSource is DataBindingList<PlotLandUse> dataSource) || this.dgLandUses.Columns[e.ColumnIndex] != this.dcPlot || e.RowIndex >= dataSource.Count || dataSource[e.RowIndex].IsTransient)
        return;
      DataGridViewCell dgLandUse = this.dgLandUses[e.ColumnIndex, e.RowIndex];
      dgLandUse.ReadOnly = true;
      dgLandUse.Style.BackColor = Color.LightGray;
      dgLandUse.Style.SelectionForeColor = Color.DarkGray;
    }

    public Eco.Util.ImportSpec ImportSpec()
    {
      Eco.Util.ImportSpec<PlotLandUse> importSpec1 = new Eco.Util.ImportSpec<PlotLandUse>();
      importSpec1.RecordCount = this._plotLandUses.Count;
      Eco.Util.ImportSpec importSpec2 = (Eco.Util.ImportSpec) importSpec1;
      List<FieldSpec> fieldsSpecs = importSpec2.FieldsSpecs;
      fieldsSpecs.Add(new FieldSpec<PlotLandUse>((Expression<Func<PlotLandUse, object>>) (plu => plu.Plot), this.dcPlot.HeaderText, true).AddFormat((FieldFormat) new FieldFormat<Plot>((Expression<Func<Plot, object>>) (p => (object) p.Id))).SetData(this.dcPlot.DataSource, TypeHelper.NameOf<Plot>((Expression<Func<Plot, object>>) (p => p.Self))));
      fieldsSpecs.Add(new FieldSpec<PlotLandUse>((Expression<Func<PlotLandUse, object>>) (plu => plu.LandUse), this.dcLandUse.HeaderText, true).AddFormat((FieldFormat) new FieldFormat<LandUse>((Expression<Func<LandUse, object>>) (lu => (object) lu.Id))).AddFormat((FieldFormat) new FieldFormat<LandUse>((Expression<Func<LandUse, object>>) (lu => lu.Description))).SetData(this.dcLandUse.DataSource, TypeHelper.NameOf<LandUse>((Expression<Func<LandUse, object>>) (lu => lu.Self))));
      List<FieldSpec> fieldSpecList = fieldsSpecs;
      FieldSpec fieldSpec1 = new FieldSpec<PlotLandUse>((Expression<Func<PlotLandUse, object>>) (plu => (object) plu.PercentOfPlot), this.dcPctCovered.HeaderText, true).SetNullValue((object) 0);
      AndConstraint constraint1 = new AndConstraint((AConstraint) new GtEqConstraint((object) 1), (AConstraint) new LtEqConstraint((object) 100));
      string errFieldRequired = i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired;
      string headerText = this.dcPctCovered.HeaderText;
      int num = 1;
      string str1 = num.ToString("P0");
      num = 100;
      string str2 = num.ToString("P0");
      string errorFormat = string.Format(errFieldRequired, (object) headerText, (object) str1, (object) str2);
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
      return Task.Factory.StartNew<IList<PlotLandUse>>((Func<IList<PlotLandUse>>) (() => this.DoImport(data, progress, ct)), ct, TaskCreationOptions.None, Program.Session.Scheduler).ContinueWith((System.Action<Task<IList<PlotLandUse>>>) (t =>
      {
        if (this.Year == null || t.IsCanceled || t.IsFaulted)
          return;
        IList<PlotLandUse> result = t.Result;
        DataBindingList<PlotLandUse> dataSource = this.dgLandUses.DataSource as DataBindingList<PlotLandUse>;
        foreach (PlotLandUse plotLandUse in (IEnumerable<PlotLandUse>) result)
          dataSource.Add(plotLandUse);
      }), scheduler);
    }

    private IList<PlotLandUse> DoImport(
      IList data,
      IProgress<ProgressEventArgs> progress,
      CancellationToken ct)
    {
      IList<PlotLandUse> plotLandUseList = (IList<PlotLandUse>) new List<PlotLandUse>();
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
            if (obj is PlotLandUse plotLandUse)
            {
              plotLandUse.Plot?.PlotLandUses.Add(plotLandUse);
              this.Session.Persist((object) plotLandUse);
              guidList.Add(plotLandUse.Guid);
              plotLandUseList.Add(plotLandUse);
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
            this.Session.Delete((object) this.Session.Load<PlotLandUse>((object) guidList[index]));
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
      return plotLandUseList;
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
      this.dgLandUses = new DataGridView();
      this.dcPlot = new DataGridViewComboBoxColumn();
      this.dcLandUse = new DataGridViewComboBoxColumn();
      this.dcPctCovered = new DataGridViewNumericTextBoxColumn();
      ((ISupportInitialize) this.dgLandUses).BeginInit();
      this.SuspendLayout();
      this.lblBreadcrumb.Size = new Size(800, 40);
      this.dgLandUses.AllowUserToAddRows = false;
      this.dgLandUses.AllowUserToDeleteRows = false;
      this.dgLandUses.AllowUserToResizeRows = false;
      this.dgLandUses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgLandUses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgLandUses.Columns.AddRange((DataGridViewColumn) this.dcPlot, (DataGridViewColumn) this.dcLandUse, (DataGridViewColumn) this.dcPctCovered);
      this.dgLandUses.Dock = DockStyle.Fill;
      this.dgLandUses.EnableHeadersVisualStyles = false;
      this.dgLandUses.Location = new Point(0, 80);
      this.dgLandUses.Margin = new Padding(4);
      this.dgLandUses.Name = "dgLandUses";
      this.dgLandUses.ReadOnly = true;
      this.dgLandUses.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgLandUses.RowHeadersWidth = 20;
      this.dgLandUses.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgLandUses.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgLandUses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgLandUses.Size = new Size(800, 370);
      this.dgLandUses.TabIndex = 15;
      this.dgLandUses.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgLandUses_CellErrorTextNeeded);
      this.dgLandUses.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgLandUses_CellFormatting);
      this.dgLandUses.DataError += new DataGridViewDataErrorEventHandler(this.dgLandUses_DataError);
      this.dgLandUses.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgLandUses_EditingControlShowing);
      this.dgLandUses.RowValidated += new DataGridViewCellEventHandler(this.dgLandUses_RowValidated);
      this.dgLandUses.RowValidating += new DataGridViewCellCancelEventHandler(this.dgLandUses_RowValidating);
      this.dcPlot.DataPropertyName = "Plot";
      this.dcPlot.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcPlot.Frozen = true;
      this.dcPlot.HeaderText = "Plot";
      this.dcPlot.Name = "dcPlot";
      this.dcPlot.ReadOnly = true;
      this.dcPlot.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcLandUse.DataPropertyName = "LandUse";
      this.dcLandUse.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcLandUse.HeaderText = "Land Use";
      this.dcLandUse.Name = "dcLandUse";
      this.dcLandUse.ReadOnly = true;
      this.dcLandUse.Resizable = DataGridViewTriState.True;
      this.dcLandUse.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctCovered.DataPropertyName = "PercentOfPlot";
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
      this.AutoScaleDimensions = new SizeF(8f, 18f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(800, 450);
      this.Controls.Add((Control) this.dgLandUses);
      this.Name = nameof (LandUsesDataForm);
      this.Text = "Land Uses";
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgLandUses, 0);
      ((ISupportInitialize) this.dgLandUses).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
