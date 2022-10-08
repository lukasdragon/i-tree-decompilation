// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ReferenceObjectsForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Constraints;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Interfaces;
using NHibernate;
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
  public class ReferenceObjectsForm : DataContentForm, IActionable, IExportable
  {
    private DataGridViewManager m_dgManager;
    private IList<ReferenceObject> m_referenceObjects;
    private IContainer components;
    private TableLayoutPanel pnlEditHelp;
    private DataGridView dgRefObjects;
    private DataGridViewComboBoxColumn dcPlot;
    private DataGridViewComboBoxColumn dcObject;
    private DataGridViewNumericTextBoxColumn dcDirection;
    private DataGridViewNumericTextBoxColumn dcDistance;
    private DataGridViewNumericTextBoxColumn dcDBH;
    private DataGridViewTextBoxColumn dcNotes;

    public ReferenceObjectsForm()
    {
      this.InitializeComponent();
      this.m_dgManager = new DataGridViewManager(this.dgRefObjects);
      this.dgRefObjects.DoubleBuffered(true);
      this.dgRefObjects.AutoGenerateColumns = false;
      this.dgRefObjects.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgRefObjects.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgRefObjects.CellEndEdit += new DataGridViewCellEventHandler(this.dgRefObjects_CellEndEdit);
      this.dcObject.DisplayMember = "Value";
      this.dcObject.ValueMember = "Key";
      this.dcObject.DataSource = (object) new BindingSource((object) EnumHelper.ConvertToDictionary<ReferenceObjectType>(), (string) null);
      this.dcDirection.DefaultCellStyle.DataSourceNullValue = (object) -1;
      this.dcDistance.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcDBH.DefaultCellStyle.DataSourceNullValue = (object) -1f;
    }

    protected override void InitializeYear(Year year)
    {
      base.InitializeYear(year);
      NHibernateUtil.Initialize((object) year.Plots);
    }

    protected override void LoadData()
    {
      base.LoadData();
      lock (this.Session)
      {
        using (this.Session.BeginTransaction())
        {
          Plot p = (Plot) null;
          this.m_referenceObjects = this.Session.QueryOver<ReferenceObject>().JoinAlias((Expression<Func<ReferenceObject, object>>) (ro => ro.Plot), (Expression<Func<object>>) (() => p)).Where((Expression<Func<ReferenceObject, bool>>) (ro => p.Year == this.Year)).OrderBy((Expression<Func<ReferenceObject, object>>) (ro => (object) p.Id)).Asc.List();
        }
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
      string str1;
      string str2;
      if (year.Unit == YearUnit.English)
      {
        str1 = i_Tree_Eco_v6.Resources.Strings.UnitInchesAbbr;
        str2 = i_Tree_Eco_v6.Resources.Strings.UnitFeetAbbr;
      }
      else
      {
        str1 = i_Tree_Eco_v6.Resources.Strings.UnitCentimetersAbbr;
        str2 = i_Tree_Eco_v6.Resources.Strings.UnitMetersAbbr;
      }
      this.dcDBH.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) str1);
      this.dcDistance.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) v6Strings.ReferenceObject_Distance, (object) str2);
      this.dcPlot.BindTo<Plot>((Expression<Func<Plot, object>>) (p => (object) p.Id), (Expression<Func<Plot, object>>) (p => p.Self), (object) year.Plots.ToList<Plot>());
      DataBindingList<ReferenceObject> dataBindingList = new DataBindingList<ReferenceObject>(this.m_referenceObjects);
      this.dgRefObjects.DataSource = (object) dataBindingList;
      this.dgRefObjects.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddComparer<Plot>((IComparer) new PropertyComparer<Plot>((Func<Plot, object>) (p => (object) p.Id)));
      dataBindingList.AddingNew += new AddingNewEventHandler(this.ReferenceObjects_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.ReferenceObjects_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.ReferenceObjects_ListChanged);
      this.OnRequestRefresh();
    }

    private void ReferenceObjects_AddingNew(object sender, AddingNewEventArgs e)
    {
      ReferenceObject referenceObject = new ReferenceObject()
      {
        Object = ReferenceObjectType.Unknown,
        DBH = -1f
      };
      e.NewObject = (object) referenceObject;
    }

    private void ReferenceObjects_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      if (!(sender is DataBindingList<ReferenceObject> dataBindingList) || e.NewIndex >= dataBindingList.Count)
        return;
      ReferenceObject referenceObject = dataBindingList[e.NewIndex];
      if (referenceObject.IsTransient)
        return;
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          referenceObject.Plot?.ReferenceObjects.Remove(referenceObject);
          referenceObject.Plot = (Plot) null;
          this.Session.Delete((object) referenceObject);
          transaction.Commit();
        }
      }
    }

    private void ReferenceObjects_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    protected override void OnRequestRefresh()
    {
      Year year = this.Year;
      bool flag = year != null && year.Changed && this.dgRefObjects.DataSource != null && year.Plots.Count > 0;
      this.dgRefObjects.ReadOnly = !flag;
      this.dgRefObjects.AllowUserToAddRows = flag;
      this.dgRefObjects.AllowUserToDeleteRows = flag;
      base.OnRequestRefresh();
    }

    private void dgRefObjects_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgRefObjects.NewRowIndex || !(this.dgRefObjects.DataSource is DataBindingList<ReferenceObject> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgRefObjects.Columns[e.ColumnIndex];
      ReferenceObject referenceObject = dataSource[e.RowIndex];
      if (column == this.dcPlot && referenceObject.Plot == null)
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      if (column == this.dcObject)
      {
        if (referenceObject.Object != ~ReferenceObjectType.Unknown)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcDirection)
      {
        if (referenceObject.Direction >= 1 && referenceObject.Direction <= 360)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRange, (object) column.HeaderText, (object) 1, (object) 360);
      }
      else if (column == this.dcDistance)
      {
        if ((double) referenceObject.Distance <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) referenceObject.Distance <= 1000.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 1000);
        }
      }
      else
      {
        if (column != this.dcDBH || referenceObject.Object != ReferenceObjectType.Tree || (double) referenceObject.DBH >= 0.0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
      }
    }

    private void dgRefObjects_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.dgRefObjects.CurrentRow != null && !this.dgRefObjects.IsCurrentCellDirty || !(this.dgRefObjects.DataSource is DataBindingList<ReferenceObject> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewCell dataGridViewCell = this.dgRefObjects.Rows[e.RowIndex].ErrorCell();
      string text = (string) null;
      if (dataGridViewCell != null)
        text = dataGridViewCell.ErrorText;
      if (text == null)
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgRefObjects_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgRefObjects.ReadOnly)
        return;
      if (this.dgRefObjects.CurrentRow != null && this.dgRefObjects.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (this.dgRefObjects.DataSource is DataBindingList<ReferenceObject> dataSource && e.RowIndex < dataSource.Count)
        {
          ReferenceObject referenceObject = dataSource[e.RowIndex];
          if (referenceObject.IsTransient || this.Session.IsDirty())
          {
            lock (this.Session)
            {
              using (ITransaction transaction = this.Session.BeginTransaction())
              {
                referenceObject.Plot.ReferenceObjects.Add(referenceObject);
                this.Session.SaveOrUpdate((object) referenceObject);
                transaction.Commit();
              }
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgRefObjects_CellEndEdit(object sender, DataGridViewCellEventArgs e) => this.OnRequestRefresh();

    private void dgRefObjects_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) => this.EnableDBH(e.ColumnIndex, e.RowIndex);

    private void dgRefObjects_CellValueChanged(object sender, DataGridViewCellEventArgs e) => this.EnableDBH(e.ColumnIndex, e.RowIndex);

    private void dgRefObjects_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void DeleteSelectedRows()
    {
      if (!(this.dgRefObjects.DataSource is DataBindingList<ReferenceObject> dataSource))
        return;
      CurrencyManager currencyManager = this.dgRefObjects.BindingContext[(object) dataSource] as CurrencyManager;
      bool flag = false;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgRefObjects.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          dataSource.RemoveAt(selectedRow.Index);
          flag = true;
        }
      }
      if (!(currencyManager.Position != -1 & flag))
        return;
      this.dgRefObjects.Rows[currencyManager.Position].Selected = true;
    }

    private void EnableDBH(int columnIndex, int rowIndex)
    {
      if (!(this.dgRefObjects.DataSource is DataBindingList<ReferenceObject> dataSource))
        return;
      int num = this.dgRefObjects.Columns.IndexOf((DataGridViewColumn) this.dcObject);
      if (columnIndex != num || rowIndex >= dataSource.Count)
        return;
      bool enabled = dataSource[rowIndex].Object == ReferenceObjectType.Tree;
      int columnIndex1 = this.dgRefObjects.Columns.IndexOf((DataGridViewColumn) this.dcDBH);
      this.EnableCell(columnIndex1, rowIndex, enabled);
      if (enabled || this.dgRefObjects[columnIndex1, rowIndex].Value.Equals((object) -1f))
        return;
      this.dgRefObjects[columnIndex1, rowIndex].Value = (object) -1;
    }

    private void EnableCell(int columnIndex, int rowIndex, bool enabled)
    {
      DataGridViewCell dgRefObject = this.dgRefObjects[columnIndex, rowIndex];
      dgRefObject.ReadOnly = !enabled;
      if (enabled)
      {
        dgRefObject.Style.BackColor = dgRefObject.OwningColumn.DefaultCellStyle.BackColor;
        dgRefObject.Style.ForeColor = dgRefObject.OwningColumn.DefaultCellStyle.ForeColor;
        dgRefObject.Style.SelectionForeColor = dgRefObject.OwningColumn.DefaultCellStyle.SelectionForeColor;
      }
      else
      {
        dgRefObject.Style.BackColor = Color.LightGray;
        dgRefObject.Style.ForeColor = Color.DarkGray;
        dgRefObject.Style.SelectionForeColor = Color.DarkGray;
      }
    }

    public bool CanPerformAction(UserActions action)
    {
      Year year = this.Year;
      bool flag1 = year != null && year.Changed && this.dgRefObjects.DataSource != null;
      bool flag2 = flag1 && this.dgRefObjects.AllowUserToAddRows;
      bool flag3 = this.dgRefObjects.SelectedRows.Count > 0;
      bool flag4 = this.dgRefObjects.CurrentRow != null && this.dgRefObjects.IsCurrentRowDirty;
      bool flag5 = this.dgRefObjects.CurrentRow != null && this.dgRefObjects.CurrentRow.IsNewRow;
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
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgRefObjects.SelectedRows)
            selectedRow.Selected = false;
          this.dgRefObjects.Rows[this.dgRefObjects.NewRowIndex].Selected = true;
          this.dgRefObjects.FirstDisplayedScrollingRowIndex = this.dgRefObjects.NewRowIndex - this.dgRefObjects.DisplayedRowCount(false) + 1;
          this.dgRefObjects.CurrentCell = this.dgRefObjects.Rows[this.dgRefObjects.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          DataGridViewRow selectedRow1 = this.dgRefObjects.SelectedRows[0];
          if (!(this.dgRefObjects.DataSource is DataBindingList<ReferenceObject> dataSource) || selectedRow1.Index >= dataSource.Count)
            break;
          ReferenceObject referenceObject = dataSource[selectedRow1.Index].Clone() as ReferenceObject;
          dataSource.Add(referenceObject);
          this.dgRefObjects.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          if (this.dgRefObjects.SelectedRows.Count <= 0)
            break;
          this.DeleteSelectedRows();
          break;
      }
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgRefObjects.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgRefObjects.DataSource != null;

    private void dgRefObjects_EditingControlShowing(
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

    public Eco.Util.ImportSpec ImportSpec()
    {
      Eco.Util.ImportSpec<ReferenceObject> importSpec1 = new Eco.Util.ImportSpec<ReferenceObject>();
      importSpec1.RecordCount = this.m_referenceObjects.Count;
      Eco.Util.ImportSpec importSpec2 = (Eco.Util.ImportSpec) importSpec1;
      List<FieldSpec> fieldsSpecs = importSpec2.FieldsSpecs;
      fieldsSpecs.Add(new FieldSpec<ReferenceObject>((Expression<Func<ReferenceObject, object>>) (ro => ro.Plot), this.dcPlot.HeaderText, true).AddFormat((FieldFormat) new FieldFormat<Plot>((Expression<Func<Plot, object>>) (p => (object) p.Id))).SetData(this.dcPlot.DataSource, TypeHelper.NameOf<Plot>((Expression<Func<Plot, object>>) (p => p.Self))));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<ReferenceObject>((Expression<Func<ReferenceObject, object>>) (ro => (object) ro.Object), this.dcObject.HeaderText, true));
      List<FieldSpec> fieldSpecList1 = fieldsSpecs;
      FieldSpec fieldSpec1 = new FieldSpec<ReferenceObject>((Expression<Func<ReferenceObject, object>>) (ro => (object) ro.Direction), this.dcDirection.HeaderText, false).SetNullValue((object) -1);
      AndConstraint constraint1 = new AndConstraint((AConstraint) new GtEqConstraint((object) 1), (AConstraint) new LtEqConstraint((object) 360));
      string errFieldRange1 = i_Tree_Eco_v6.Resources.Strings.ErrFieldRange;
      string headerText1 = this.dcDirection.HeaderText;
      int num1 = 1;
      string str1 = num1.ToString("P0");
      num1 = 360;
      string str2 = num1.ToString("P0");
      string errorFormat1 = string.Format(errFieldRange1, (object) headerText1, (object) str1, (object) str2);
      FieldConstraint constraint2 = new FieldConstraint((AConstraint) constraint1, errorFormat1);
      FieldSpec fieldSpec2 = fieldSpec1.AddConstraint(constraint2);
      fieldSpecList1.Add(fieldSpec2);
      List<FieldSpec> fieldSpecList2 = fieldsSpecs;
      FieldSpec fieldSpec3 = new FieldSpec<ReferenceObject>((Expression<Func<ReferenceObject, object>>) (ro => (object) ro.Distance), this.dcDistance.HeaderText, false).SetNullValue((object) -1f);
      AndConstraint constraint3 = new AndConstraint((AConstraint) new GtConstraint((object) 0), (AConstraint) new LtEqConstraint((object) 1000));
      string errFieldRange2 = i_Tree_Eco_v6.Resources.Strings.ErrFieldRange;
      string headerText2 = this.dcDistance.HeaderText;
      int num2 = 0;
      string str3 = num2.ToString("P0");
      num2 = 1000;
      string str4 = num2.ToString("P0");
      string errorFormat2 = string.Format(errFieldRange2, (object) headerText2, (object) str3, (object) str4);
      FieldConstraint constraint4 = new FieldConstraint((AConstraint) constraint3, errorFormat2);
      FieldSpec fieldSpec4 = fieldSpec3.AddConstraint(constraint4);
      fieldSpecList2.Add(fieldSpec4);
      fieldsSpecs.Add(new FieldSpec<ReferenceObject>((Expression<Func<ReferenceObject, object>>) (ro => (object) ro.DBH), this.dcDBH.HeaderText, false).SetNullValue((object) -1f));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<ReferenceObject>((Expression<Func<ReferenceObject, object>>) (ro => ro.Notes), this.dcNotes.HeaderText, false));
      return importSpec2;
    }

    public Task ImportData(
      IList data,
      IProgress<ProgressEventArgs> progress,
      CancellationToken ct)
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      return Task.Factory.StartNew<IList<ReferenceObject>>((Func<IList<ReferenceObject>>) (() => this.DoImport(data, progress, ct)), ct, TaskCreationOptions.None, Program.Session.Scheduler).ContinueWith((System.Action<Task<IList<ReferenceObject>>>) (t =>
      {
        if (this.Year == null || t.IsCanceled || t.IsFaulted)
          return;
        IList<ReferenceObject> result = t.Result;
        DataBindingList<ReferenceObject> dataSource = this.dgRefObjects.DataSource as DataBindingList<ReferenceObject>;
        foreach (ReferenceObject referenceObject in (IEnumerable<ReferenceObject>) result)
          dataSource.Add(referenceObject);
      }), scheduler);
    }

    private IList<ReferenceObject> DoImport(
      IList data,
      IProgress<ProgressEventArgs> progress,
      CancellationToken ct)
    {
      IList<ReferenceObject> referenceObjectList = (IList<ReferenceObject>) new List<ReferenceObject>();
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
            if (obj is ReferenceObject referenceObject)
            {
              referenceObject.Plot?.ReferenceObjects.Add(referenceObject);
              this.Session.Persist((object) referenceObject);
              guidList.Add(referenceObject.Guid);
              referenceObjectList.Add(referenceObject);
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
            this.Session.Delete((object) this.Session.Load<ReferenceObject>((object) guidList[index]));
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
      return referenceObjectList;
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
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      this.pnlEditHelp = new TableLayoutPanel();
      this.dgRefObjects = new DataGridView();
      this.dcPlot = new DataGridViewComboBoxColumn();
      this.dcObject = new DataGridViewComboBoxColumn();
      this.dcDirection = new DataGridViewNumericTextBoxColumn();
      this.dcDistance = new DataGridViewNumericTextBoxColumn();
      this.dcDBH = new DataGridViewNumericTextBoxColumn();
      this.dcNotes = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dgRefObjects).BeginInit();
      this.SuspendLayout();
      this.lblBreadcrumb.Margin = new Padding(9, 10, 9, 10);
      this.pnlEditHelp.AutoSize = true;
      this.pnlEditHelp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlEditHelp.ColumnCount = 1;
      this.pnlEditHelp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.pnlEditHelp.Dock = DockStyle.Top;
      this.pnlEditHelp.Location = new Point(0, 58);
      this.pnlEditHelp.Margin = new Padding(4, 4, 4, 4);
      this.pnlEditHelp.Name = "pnlEditHelp";
      this.pnlEditHelp.RowCount = 1;
      this.pnlEditHelp.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
      this.pnlEditHelp.Size = new Size(1067, 0);
      this.pnlEditHelp.TabIndex = 12;
      this.dgRefObjects.AllowUserToAddRows = false;
      this.dgRefObjects.AllowUserToDeleteRows = false;
      this.dgRefObjects.AllowUserToResizeRows = false;
      this.dgRefObjects.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgRefObjects.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgRefObjects.Columns.AddRange((DataGridViewColumn) this.dcPlot, (DataGridViewColumn) this.dcObject, (DataGridViewColumn) this.dcDirection, (DataGridViewColumn) this.dcDistance, (DataGridViewColumn) this.dcDBH, (DataGridViewColumn) this.dcNotes);
      this.dgRefObjects.Dock = DockStyle.Fill;
      this.dgRefObjects.EnableHeadersVisualStyles = false;
      this.dgRefObjects.Location = new Point(0, 58);
      this.dgRefObjects.Margin = new Padding(4, 4, 4, 4);
      this.dgRefObjects.MultiSelect = false;
      this.dgRefObjects.Name = "dgRefObjects";
      this.dgRefObjects.ReadOnly = true;
      this.dgRefObjects.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgRefObjects.RowHeadersWidth = 20;
      this.dgRefObjects.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgRefObjects.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgRefObjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgRefObjects.Size = new Size(1067, 565);
      this.dgRefObjects.TabIndex = 13;
      this.dgRefObjects.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgRefObjects_CellErrorTextNeeded);
      this.dgRefObjects.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgRefObjects_CellFormatting);
      this.dgRefObjects.CellValueChanged += new DataGridViewCellEventHandler(this.dgRefObjects_CellValueChanged);
      this.dgRefObjects.DataError += new DataGridViewDataErrorEventHandler(this.dgRefObjects_DataError);
      this.dgRefObjects.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgRefObjects_EditingControlShowing);
      this.dgRefObjects.RowValidated += new DataGridViewCellEventHandler(this.dgRefObjects_RowValidated);
      this.dgRefObjects.RowValidating += new DataGridViewCellCancelEventHandler(this.dgRefObjects_RowValidating);
      this.dcPlot.DataPropertyName = "Plot";
      this.dcPlot.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcPlot.Frozen = true;
      this.dcPlot.HeaderText = "Plot";
      this.dcPlot.Name = "dcPlot";
      this.dcPlot.ReadOnly = true;
      this.dcPlot.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcObject.DataPropertyName = "Object";
      this.dcObject.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcObject.HeaderText = "Object Type";
      this.dcObject.Name = "dcObject";
      this.dcObject.ReadOnly = true;
      this.dcObject.Resizable = DataGridViewTriState.True;
      this.dcObject.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDirection.DataPropertyName = "Direction";
      this.dcDirection.DecimalPlaces = 0;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle1.Format = "N0";
      this.dcDirection.DefaultCellStyle = gridViewCellStyle1;
      this.dcDirection.Format = "#;#";
      this.dcDirection.HasDecimal = false;
      this.dcDirection.HeaderText = "Direction";
      this.dcDirection.Name = "dcDirection";
      this.dcDirection.ReadOnly = true;
      this.dcDirection.Resizable = DataGridViewTriState.True;
      this.dcDirection.Signed = false;
      this.dcDistance.DataPropertyName = "Distance";
      this.dcDistance.DecimalPlaces = 1;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle2.Format = "N1";
      this.dcDistance.DefaultCellStyle = gridViewCellStyle2;
      this.dcDistance.Format = "#;#";
      this.dcDistance.HasDecimal = true;
      this.dcDistance.HeaderText = "Distance";
      this.dcDistance.Name = "dcDistance";
      this.dcDistance.ReadOnly = true;
      this.dcDistance.Signed = false;
      this.dcDBH.DataPropertyName = "DBH";
      this.dcDBH.DecimalPlaces = 1;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle3.Format = "N1";
      this.dcDBH.DefaultCellStyle = gridViewCellStyle3;
      this.dcDBH.Format = (string) null;
      this.dcDBH.HasDecimal = true;
      this.dcDBH.HeaderText = "DBH";
      this.dcDBH.Name = "dcDBH";
      this.dcDBH.ReadOnly = true;
      this.dcDBH.Signed = true;
      this.dcDBH.Width = 42;
      this.dcNotes.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcNotes.DataPropertyName = "Notes";
      this.dcNotes.HeaderText = "Notes";
      this.dcNotes.MaxInputLength = 100;
      this.dcNotes.Name = "dcNotes";
      this.dcNotes.ReadOnly = true;
      this.dcNotes.Width = 200;
      this.AutoScaleDimensions = new SizeF(8f, 18f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1067, 623);
      this.Controls.Add((Control) this.dgRefObjects);
      this.Controls.Add((Control) this.pnlEditHelp);
      this.Margin = new Padding(5, 6, 5, 6);
      this.Name = nameof (ReferenceObjectsForm);
      this.Text = "Reference Objects";
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.pnlEditHelp, 0);
      this.Controls.SetChildIndex((Control) this.dgRefObjects, 0);
      ((ISupportInitialize) this.dgRefObjects).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
