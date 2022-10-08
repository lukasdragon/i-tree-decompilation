// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ShrubsForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Constraints;
using Eco.Util.Views;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Interfaces;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class ShrubsForm : DataContentForm, IActionable, IExportable
  {
    private DataGridViewManager m_dgManager;
    private int m_dgHorizPos;
    private BindingList<SpeciesView> m_species;
    private IList<Shrub> m_shrubs;
    private IContainer components;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewFilteredComboBoxColumn dataGridViewFilteredComboBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private DataGridView dgShrubs;
    private TableLayoutPanel pnlEditHelp;
    private DataGridViewComboBoxColumn dcPlot;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewFilteredComboBoxColumn dcSpecies;
    private DataGridViewNumericTextBoxColumn dcHeight;
    private DataGridViewNumericTextBoxColumn dcPctShrubArea;
    private DataGridViewComboBoxColumn dcPctMissing;
    private DataGridViewTextBoxColumn dcComments;

    public ShrubsForm()
    {
      this.InitializeComponent();
      this.dcHeight.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.m_dgManager = new DataGridViewManager(this.dgShrubs);
      this.dgShrubs.DoubleBuffered(true);
      this.dgShrubs.AutoGenerateColumns = false;
      this.dgShrubs.MultiSelect = false;
      this.dgShrubs.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgShrubs.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgShrubs.CellEndEdit += new DataGridViewCellEventHandler(this.dgShrubs_CellEndEdit);
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      EventPublisher.Register<EntityUpdated<Shrub>>(new EventHandler<EntityUpdated<Shrub>>(this.Shrub_Updated));
    }

    private void Shrub_Updated(object sender, EntityUpdated<Shrub> e) => this.TaskManager.Add(Task.Factory.StartNew((System.Action) (() =>
    {
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          Shrub proxy = this.Session.Load<Shrub>((object) e.Guid);
          if (NHibernateUtil.IsInitialized((object) proxy))
            this.Session.Refresh((object) proxy, LockMode.None);
          transaction.Commit();
        }
      }
    }), CancellationToken.None, TaskCreationOptions.None, Program.Session.Scheduler));

    protected override void InitializeYear(Year year)
    {
      base.InitializeYear(year);
      NHibernateUtil.Initialize((object) year.Plots);
    }

    protected override void LoadData()
    {
      base.LoadData();
      List<SpeciesView> list = Program.Session.Species.Values.ToList<SpeciesView>();
      list.Sort((IComparer<SpeciesView>) new PropertyComparer<SpeciesView>((Func<SpeciesView, object>) (sp => (object) sp.CommonName)));
      this.m_species = (BindingList<SpeciesView>) new ExtendedBindingList<SpeciesView>((IList<SpeciesView>) list);
      using (ITransaction transaction = this.Session.BeginTransaction())
      {
        Shrub shrub = (Shrub) null;
        this.m_shrubs = this.Session.QueryOver<Shrub>((System.Linq.Expressions.Expression<Func<Shrub>>) (() => shrub)).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Shrub, Plot>>) (s => s.Plot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.Year)).OrderBy((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) shrub.Id)).Asc.List();
        transaction.Commit();
      }
    }

    protected override void OnDataLoaded()
    {
      base.OnDataLoaded();
      this.InitGrid();
    }

    private void InitGrid()
    {
      Year year = this.Year;
      this.dcHeight.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.Height, year.Unit != YearUnit.English ? (object) i_Tree_Eco_v6.Resources.Strings.UnitMetersAbbr : (object) i_Tree_Eco_v6.Resources.Strings.UnitFeetAbbr);
      this.dcSpecies.BindTo<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.ScientificCommonName), (System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.Code), (object) this.m_species);
      this.dcPlot.BindTo<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id), (System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Self), (object) year.Plots.ToList<Plot>());
      this.dcPctMissing.BindTo("Value", "Key", (object) new BindingSource((object) EnumHelper.ConvertToDictionary<PctMidRange>(), (string) null));
      this.dcPctMissing.DefaultCellStyle.DataSourceNullValue = (object) PctMidRange.PRINV;
      this.dcPctShrubArea.DefaultCellStyle.DataSourceNullValue = (object) 0;
      DataBindingList<Shrub> dataBindingList = new DataBindingList<Shrub>(this.m_shrubs);
      this.dgShrubs.DataSource = (object) dataBindingList;
      this.dgShrubs.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddComparer<Plot>((IComparer) new PropertyComparer<Plot>((Func<Plot, object>) (p => (object) p.Id)));
      dataBindingList.AddingNew += new AddingNewEventHandler(this.Shrubs_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.Shrubs_Removing);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.Shrubs_ListChanged);
    }

    protected override void OnRequestRefresh()
    {
      Year year = this.Year;
      bool flag = year != null && year.Changed && this.dgShrubs.DataSource != null && year.Plots.Count > 0;
      this.dgShrubs.ReadOnly = !flag;
      this.dgShrubs.AllowUserToAddRows = flag;
      this.dgShrubs.AllowUserToDeleteRows = flag;
      base.OnRequestRefresh();
    }

    private void Shrubs_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private int TotalShrubArea(Plot p)
    {
      int num = 100;
      if (p != null)
        num = p.Shrubs.Sum<Shrub>((Func<Shrub, int>) (s => s.PercentOfShrubArea));
      return num;
    }

    private void Shrubs_AddingNew(object sender, AddingNewEventArgs e)
    {
      Shrub shrub = new Shrub()
      {
        PercentMissing = PctMidRange.PR0,
        PercentOfShrubArea = 0
      };
      e.NewObject = (object) shrub;
    }

    private void Shrubs_Removing(object sender, ListChangedEventArgs e)
    {
      DataBindingList<Shrub> dataBindingList = sender as DataBindingList<Shrub>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      Shrub shrub = dataBindingList[e.NewIndex];
      if (shrub.IsTransient)
        return;
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          shrub.Plot.Shrubs.Remove(shrub);
          this.Session.Delete((object) shrub);
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
      if (column == this.dcPlot)
      {
        if (shrub.Plot != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcId)
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
          if (Program.Session.InvalidSpecies.TryGetValue(shrub.Species, out speciesView))
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
      DataGridViewCell dataGridViewCell = this.dgShrubs.Rows[e.RowIndex].ErrorCell();
      string text = (string) null;
      if (dataGridViewCell != null)
      {
        text = dataGridViewCell.ErrorText;
      }
      else
      {
        int num = 0;
        bool flag = false;
        if (shrub1.Plot != null)
        {
          foreach (Shrub shrub2 in (IEnumerable<Shrub>) shrub1.Plot.Shrubs)
          {
            num += shrub2.PercentOfShrubArea;
            if (!shrub2.Equals((object) shrub1) && shrub2.Id == shrub1.Id)
            {
              text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcId.HeaderText);
              break;
            }
            if (shrub2.Equals((object) shrub1))
              flag = true;
          }
        }
        if (text == null)
        {
          if (!flag && num >= 100)
            text = i_Tree_Eco_v6.Resources.Strings.ErrNoShrubArea;
          else if (flag && num > 100)
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRange, (object) this.dcPctShrubArea.HeaderText, (object) 1, (object) (100 + shrub1.PercentOfShrubArea - num));
          else if (!flag && shrub1.PercentOfShrubArea > 100 - num)
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRange, (object) this.dcPctShrubArea.HeaderText, (object) 1, (object) (100 - num));
        }
      }
      if (text == null)
        return;
      e.Cancel = true;
      int num1 = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgShrubs_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
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
          if (shrub.IsTransient || this.Session.IsDirty())
          {
            lock (this.Session)
            {
              using (ITransaction transaction = this.Session.BeginTransaction())
              {
                shrub.Plot.Shrubs.Add(shrub);
                this.Session.SaveOrUpdate((object) shrub);
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
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgShrubs.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
          dataSource.RemoveAt(selectedRow.Index);
      }
      if (currencyManager.Position == -1)
        return;
      this.dgShrubs.Rows[currencyManager.Position].Selected = true;
    }

    private void dgShrubs_CellEndEdit(object sender, DataGridViewCellEventArgs e) => this.OnRequestRefresh();

    private void dgShrubs_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    public bool CanPerformAction(UserActions action)
    {
      IList<Plot> dataSource = this.dcPlot.DataSource as IList<Plot>;
      bool flag1 = this.Year != null && this.Year.Changed && this.dgShrubs.DataSource != null && dataSource != null && dataSource.Count > 0;
      bool flag2 = flag1 && this.dgShrubs.AllowUserToAddRows;
      bool flag3 = this.dgShrubs.SelectedRows.Count > 0;
      bool flag4 = this.dgShrubs.CurrentRow != null && this.dgShrubs.IsCurrentRowDirty;
      bool flag5 = this.dgShrubs.CurrentRow != null && this.dgShrubs.CurrentRow.IsNewRow;
      switch (action)
      {
        case UserActions.New:
          return flag2 && !flag5 && !flag4;
        case UserActions.Copy:
          return false;
        case UserActions.Undo:
          return flag1 && this.m_dgManager.CanUndo;
        case UserActions.Redo:
          return flag1 && this.m_dgManager.CanRedo;
        case UserActions.Delete:
          return flag1 & flag3 && !flag5 | flag4;
        case UserActions.ImportData:
          return flag2 && !flag5 && !flag4;
        default:
          return false;
      }
    }

    public void PerformAction(UserActions action)
    {
      switch (action)
      {
        case UserActions.New:
          if (!this.dgShrubs.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgShrubs.SelectedRows)
            selectedRow.Selected = false;
          this.dgShrubs.Rows[this.dgShrubs.NewRowIndex].Selected = true;
          this.dgShrubs.FirstDisplayedScrollingRowIndex = this.dgShrubs.NewRowIndex - this.dgShrubs.DisplayedRowCount(false) + 1;
          this.dgShrubs.CurrentCell = this.dgShrubs.Rows[this.dgShrubs.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          if (this.dgShrubs.SelectedRows.Count <= 0)
            break;
          DataGridViewRow selectedRow1 = this.dgShrubs.SelectedRows[0];
          DataBindingList<Shrub> dataSource = this.dgShrubs.DataSource as DataBindingList<Shrub>;
          if (selectedRow1.Index >= dataSource.Count)
            break;
          Shrub shrub1 = dataSource[selectedRow1.Index].Clone() as Shrub;
          int num = 1;
          foreach (Shrub shrub2 in (IEnumerable<Shrub>) shrub1.Plot.Shrubs)
          {
            if (shrub2.Id >= num)
              num = shrub2.Id + 1;
          }
          shrub1.Id = num;
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

    private void dgShrubs_Scroll(object sender, ScrollEventArgs e)
    {
      if (e.ScrollOrientation != ScrollOrientation.HorizontalScroll)
        return;
      this.m_dgHorizPos = e.NewValue;
    }

    private void dgShrubs_Sorted(object sender, EventArgs e) => this.dgShrubs.HorizontalScrollingOffset = this.m_dgHorizPos;

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgShrubs.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgShrubs.DataSource != null;

    private void dgShrubs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dgShrubs.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn != this.dcPlot || e.Value == null || !(this.dgShrubs.DataSource is DataBindingList<Shrub> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      Plot plot = dataSource[e.RowIndex].Plot;
      if (plot == null)
        return;
      string str = string.Format("Plot {0}: {1} Tree(s); {2} Shrub(s)", (object) plot.Id, (object) plot.Trees.Count.ToString(), (object) plot.Shrubs.Count.ToString());
      this.dgShrubs.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = str;
    }

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

    private void dgShrubs_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgShrubs_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
    {
      DataGridViewCell currentCell = this.dgShrubs.CurrentCell;
      if (currentCell == null)
        return;
      DataGridViewColumn owningColumn = currentCell.OwningColumn;
      if (!(owningColumn is DataGridViewComboBoxColumn))
        return;
      object dataSourceNullValue = owningColumn.DefaultCellStyle.DataSourceNullValue;
      if (!string.Empty.Equals(e.Value) || dataSourceNullValue == null)
        return;
      e.Value = dataSourceNullValue;
      e.ParsingApplied = true;
    }

    public Eco.Util.ImportSpec ImportSpec()
    {
      Eco.Util.ImportSpec<Shrub> importSpec1 = new Eco.Util.ImportSpec<Shrub>();
      importSpec1.RecordCount = this.m_shrubs.Count;
      Eco.Util.ImportSpec<Shrub> importSpec2 = importSpec1;
      List<FieldSpec> fieldsSpecs = importSpec2.FieldsSpecs;
      fieldsSpecs.Add(new FieldSpec<Shrub>((System.Linq.Expressions.Expression<Func<Shrub, object>>) (s => s.Plot), this.dcPlot.HeaderText, true).AddFormat((FieldFormat) new FieldFormat<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id))).SetData(this.dcPlot.DataSource, TypeHelper.NameOf<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Self))));
      fieldsSpecs.Add(new FieldSpec<Shrub>((System.Linq.Expressions.Expression<Func<Shrub, object>>) (s => s.Species), this.dcSpecies.HeaderText, true).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.CommonName))).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.ScientificName))).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.CommonScientificName))).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.ScientificCommonName))).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.Code))).SetData(this.dcSpecies.DataSource, TypeHelper.NameOf<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.Code))));
      fieldsSpecs.Add(new FieldSpec<Shrub>((System.Linq.Expressions.Expression<Func<Shrub, object>>) (s => (object) s.Height), this.dcHeight.HeaderText, true).AddConstraint(new FieldConstraint((AConstraint) new GtConstraint((object) 0), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) this.dcHeight.HeaderText, (object) 0))).AddConstraint(new FieldConstraint((AConstraint) new LtEqConstraint((object) 99), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) this.dcHeight.HeaderText, (object) 99))));
      fieldsSpecs.Add(new FieldSpec<Shrub>((System.Linq.Expressions.Expression<Func<Shrub, object>>) (s => (object) s.PercentOfShrubArea), this.dcPctShrubArea.HeaderText, true).AddConstraint(new FieldConstraint((AConstraint) new AndConstraint((AConstraint) new GtEqConstraint((object) 1), (AConstraint) new LtEqConstraint((object) 100)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRange, (object) this.dcPctShrubArea.HeaderText, (object) 1, (object) 100))));
      fieldsSpecs.Add(new FieldSpec<Shrub>((System.Linq.Expressions.Expression<Func<Shrub, object>>) (s => (object) s.PercentMissing), this.dcPctMissing.HeaderText, false).SetNullValue((object) PctMidRange.PRINV));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<Shrub>((System.Linq.Expressions.Expression<Func<Shrub, object>>) (s => s.Comments), this.dcComments.HeaderText, false));
      return (Eco.Util.ImportSpec) importSpec2;
    }

    private int NextId(Plot p) => this.Session.QueryOver<Shrub>().Where((System.Linq.Expressions.Expression<Func<Shrub, bool>>) (s => s.Plot == p)).Select((IProjection) Projections.ProjectionList().Add((IProjection) Projections.Max<Shrub>((System.Linq.Expressions.Expression<Func<Shrub, object>>) (s => (object) s.Id)))).SingleOrDefault<int>() + 1;

    public Task ImportData(
      IList data,
      IProgress<ProgressEventArgs> progress,
      CancellationToken ct)
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      return Task.Factory.StartNew<IList<Shrub>>((Func<IList<Shrub>>) (() => this.DoImport(data, progress, ct)), ct, TaskCreationOptions.None, Program.Session.Scheduler).ContinueWith((System.Action<Task<IList<Shrub>>>) (t =>
      {
        if (this.Year == null || t.IsCanceled || t.IsFaulted)
          return;
        DataBindingList<Shrub> dataSource = this.dgShrubs.DataSource as DataBindingList<Shrub>;
        foreach (Shrub shrub in (IEnumerable<Shrub>) t.Result)
          dataSource.Add(shrub);
        this.OnRequestRefresh();
      }), scheduler);
    }

    private IList<Shrub> DoImport(
      IList data,
      IProgress<ProgressEventArgs> progress,
      CancellationToken ct)
    {
      IList<Shrub> shrubList = (IList<Shrub>) new List<Shrub>();
      Dictionary<Plot, int> dictionary = new Dictionary<Plot, int>();
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
            if (obj is Shrub shrub)
            {
              shrub.Plot.Shrubs.Add(shrub);
              int num3;
              if (dictionary.TryGetValue(shrub.Plot, out num3))
                ++num3;
              else
                num3 = this.NextId(shrub.Plot);
              dictionary[shrub.Plot] = num3;
              shrub.Id = num3;
              this.Session.Persist((object) shrub);
              guidList.Add(shrub.Guid);
              shrubList.Add(shrub);
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
            this.Session.Delete((object) this.Session.Load<Shrub>((object) guidList[index]));
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
      return shrubList;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ShrubsForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewFilteredComboBoxColumn1 = new DataGridViewFilteredComboBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
      this.dgShrubs = new DataGridView();
      this.dcPlot = new DataGridViewComboBoxColumn();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcSpecies = new DataGridViewFilteredComboBoxColumn();
      this.dcHeight = new DataGridViewNumericTextBoxColumn();
      this.dcPctShrubArea = new DataGridViewNumericTextBoxColumn();
      this.dcPctMissing = new DataGridViewComboBoxColumn();
      this.dcComments = new DataGridViewTextBoxColumn();
      this.pnlEditHelp = new TableLayoutPanel();
      ((ISupportInitialize) this.dgShrubs).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewFilteredComboBoxColumn1.DataPropertyName = "Species";
      this.dataGridViewFilteredComboBoxColumn1.DataSource = (object) null;
      this.dataGridViewFilteredComboBoxColumn1.DisplayMember = (string) null;
      componentResourceManager.ApplyResources((object) this.dataGridViewFilteredComboBoxColumn1, "dataGridViewFilteredComboBoxColumn1");
      this.dataGridViewFilteredComboBoxColumn1.Name = "dataGridViewFilteredComboBoxColumn1";
      this.dataGridViewFilteredComboBoxColumn1.Resizable = DataGridViewTriState.True;
      this.dataGridViewFilteredComboBoxColumn1.ValueMember = (string) null;
      this.dataGridViewTextBoxColumn2.DataPropertyName = "Height";
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn3.DataPropertyName = "PercentOfShrubArea";
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn4.DataPropertyName = "PercentMissing";
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn5.DataPropertyName = "Comments";
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
      this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
      this.dgShrubs.AllowUserToAddRows = false;
      this.dgShrubs.AllowUserToDeleteRows = false;
      this.dgShrubs.AllowUserToResizeRows = false;
      this.dgShrubs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgShrubs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgShrubs.Columns.AddRange((DataGridViewColumn) this.dcPlot, (DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcSpecies, (DataGridViewColumn) this.dcHeight, (DataGridViewColumn) this.dcPctShrubArea, (DataGridViewColumn) this.dcPctMissing, (DataGridViewColumn) this.dcComments);
      componentResourceManager.ApplyResources((object) this.dgShrubs, "dgShrubs");
      this.dgShrubs.EnableHeadersVisualStyles = false;
      this.dgShrubs.Name = "dgShrubs";
      this.dgShrubs.ReadOnly = true;
      this.dgShrubs.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgShrubs.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgShrubs.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgShrubs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgShrubs.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgShrubs_CellErrorTextNeeded);
      this.dgShrubs.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgShrubs_CellFormatting);
      this.dgShrubs.CellParsing += new DataGridViewCellParsingEventHandler(this.dgShrubs_CellParsing);
      this.dgShrubs.DataError += new DataGridViewDataErrorEventHandler(this.dgShrubs_DataError);
      this.dgShrubs.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgShrubs_EditingControlShowing);
      this.dgShrubs.RowValidated += new DataGridViewCellEventHandler(this.dgShrubs_RowValidated);
      this.dgShrubs.RowValidating += new DataGridViewCellCancelEventHandler(this.dgShrubs_RowValidating);
      this.dgShrubs.Scroll += new ScrollEventHandler(this.dgShrubs_Scroll);
      this.dgShrubs.SelectionChanged += new EventHandler(this.dgShrubs_SelectionChanged);
      this.dgShrubs.Sorted += new EventHandler(this.dgShrubs_Sorted);
      this.dcPlot.DataPropertyName = "Plot";
      this.dcPlot.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcPlot.Frozen = true;
      componentResourceManager.ApplyResources((object) this.dcPlot, "dcPlot");
      this.dcPlot.Name = "dcPlot";
      this.dcPlot.ReadOnly = true;
      this.dcPlot.SortMode = DataGridViewColumnSortMode.Automatic;
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
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle1.Format = "N2";
      this.dcHeight.DefaultCellStyle = gridViewCellStyle1;
      this.dcHeight.Format = "#.#;#.#";
      this.dcHeight.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcHeight, "dcHeight");
      this.dcHeight.Name = "dcHeight";
      this.dcHeight.ReadOnly = true;
      this.dcHeight.Signed = false;
      this.dcPctShrubArea.DataPropertyName = "PercentOfShrubArea";
      this.dcPctShrubArea.DecimalPlaces = 0;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcPctShrubArea.DefaultCellStyle = gridViewCellStyle2;
      this.dcPctShrubArea.Format = "#;#";
      this.dcPctShrubArea.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcPctShrubArea, "dcPctShrubArea");
      this.dcPctShrubArea.Name = "dcPctShrubArea";
      this.dcPctShrubArea.ReadOnly = true;
      this.dcPctShrubArea.Signed = false;
      this.dcPctMissing.DataPropertyName = "PercentMissing";
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcPctMissing.DefaultCellStyle = gridViewCellStyle3;
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
      componentResourceManager.ApplyResources((object) this.pnlEditHelp, "pnlEditHelp");
      this.pnlEditHelp.Name = "pnlEditHelp";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.dgShrubs);
      this.Controls.Add((Control) this.pnlEditHelp);
      this.DockAreas = DockAreas.Document;
      this.Name = nameof (ShrubsForm);
      this.ShowHint = DockState.Document;
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.pnlEditHelp, 0);
      this.Controls.SetChildIndex((Control) this.dgShrubs, 0);
      ((ISupportInitialize) this.dgShrubs).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
