// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.PlotReferenceObjectsForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Interfaces;
using NHibernate;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class PlotReferenceObjectsForm : ContentForm, IPlotContent, IActionable, IExportable
  {
    private ProgramSession m_ps;
    private Plot m_boundPlot;
    private Plot m_selectedPlot;
    private Year m_year;
    private DataBindingList<ReferenceObject> m_referenceObjects;
    private ISession m_session;
    private DataGridViewManager m_dgManager;
    private TaskManager m_taskManager;
    private WaitCursor m_wc;
    private readonly object m_syncobj;
    private bool m_refresh;
    private IContainer components;
    private DataGridView dgRefObjects;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn1;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn2;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewComboBoxColumn dcObject;
    private DataGridViewNumericTextBoxColumn dcDirection;
    private DataGridViewNumericTextBoxColumn dcDistance;
    private DataGridViewNumericTextBoxColumn dcDBH;
    private DataGridViewTextBoxColumn dcNotes;

    public PlotReferenceObjectsForm()
    {
      this.m_ps = Program.Session;
      this.InitializeComponent();
      this.dcDBH.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.m_dgManager = new DataGridViewManager(this.dgRefObjects);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.m_syncobj = new object();
      this.dgRefObjects.DoubleBuffered(true);
      this.dgRefObjects.AutoGenerateColumns = false;
      this.dgRefObjects.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgRefObjects.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dgRefObjects.CellEndEdit += new DataGridViewCellEventHandler(this.dgRefObjects_CellEndEdit);
      this.dcObject.DisplayMember = "Value";
      this.dcObject.ValueMember = "Key";
      this.dcObject.DataSource = (object) new BindingSource((object) EnumHelper.ConvertToDictionary<ReferenceObjectType>(), (string) null);
      this.dcDirection.DefaultCellStyle.DataSourceNullValue = (object) -1;
      this.dcDistance.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcDBH.DefaultCellStyle.DataSourceNullValue = (object) -1f;
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
        Year year = session.Get<Year>((object) this.m_ps.InputSession.YearKey);
        lock (this.m_syncobj)
        {
          this.m_year = year;
          this.m_session = session;
        }
      }
    }

    private void InitGrid()
    {
      string str1;
      string str2;
      if (this.m_year.Unit == YearUnit.English)
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
    }

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
            this.m_referenceObjects = new DataBindingList<ReferenceObject>(this.m_session.QueryOver<ReferenceObject>().Where((Expression<Func<ReferenceObject, bool>>) (ro => ro.Plot == p)).List());
            this.m_boundPlot = p;
          }
        }
        else
        {
          this.m_boundPlot = p;
          this.m_referenceObjects = (DataBindingList<ReferenceObject>) null;
        }
      }
    }

    private void InitBoundPlot()
    {
      lock (this.m_syncobj)
      {
        if (this.m_boundPlot != this.m_selectedPlot)
          return;
        if (this.m_referenceObjects != null)
        {
          if (this.dgRefObjects.DataSource != this.m_referenceObjects)
          {
            this.dgRefObjects.DataSource = (object) this.m_referenceObjects;
            this.dgRefObjects.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
            this.m_referenceObjects.AddingNew += new AddingNewEventHandler(this.RefObjects_AddingNew);
            this.m_referenceObjects.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.RefObjects_BeforeRemove);
            this.m_referenceObjects.ListChanged += new ListChangedEventHandler(this.RefOjects_ListChanged);
            this.m_referenceObjects.Sortable = true;
          }
        }
        else
          this.dgRefObjects.DataSource = (object) null;
        this.m_refresh = false;
        this.OnRequestRefresh();
      }
    }

    private void RefOjects_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void RefObjects_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      if (!(sender is DataBindingList<ReferenceObject> dataBindingList) || e.NewIndex >= dataBindingList.Count)
        return;
      ReferenceObject referenceObject = dataBindingList[e.NewIndex];
      if (referenceObject.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) referenceObject);
          transaction.Commit();
        }
      }
    }

    private void RefObjects_AddingNew(object sender, AddingNewEventArgs e) => e.NewObject = (object) new ReferenceObject()
    {
      Plot = this.m_boundPlot,
      Object = ReferenceObjectType.Unknown,
      DBH = -1f
    };

    private void dgRefObjects_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgRefObjects.NewRowIndex || !(this.dgRefObjects.DataSource is DataBindingList<ReferenceObject> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgRefObjects.Columns[e.ColumnIndex];
      ReferenceObject referenceObject = dataSource[e.RowIndex];
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
      DataBindingList<ReferenceObject> dataSource = this.dgRefObjects.DataSource as DataBindingList<ReferenceObject>;
      if (this.dgRefObjects.CurrentRow != null && !this.dgRefObjects.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
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
      if (this.dgRefObjects.ReadOnly || this.m_boundPlot == null)
        return;
      DataBindingList<ReferenceObject> dataSource = this.dgRefObjects.DataSource as DataBindingList<ReferenceObject>;
      if (this.dgRefObjects.CurrentRow != null && this.dgRefObjects.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          ReferenceObject referenceObject = dataSource[e.RowIndex];
          if (referenceObject.IsTransient || this.m_session.IsDirty())
          {
            lock (this.m_syncobj)
            {
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                this.m_session.SaveOrUpdate((object) referenceObject);
                transaction.Commit();
              }
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgRefObjects_CellEndEdit(object sender, DataGridViewCellEventArgs e) => this.OnRequestRefresh();

    private void dgRefObjects_SelectionChanged(object sender, EventArgs e)
    {
      if (this.m_boundPlot == null)
        return;
      this.OnRequestRefresh();
    }

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

    private void dgRefObjects_CellValueChanged(object sender, DataGridViewCellEventArgs e) => this.EnableDBH(e.ColumnIndex, e.RowIndex);

    private void dgRefObjects_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) => this.EnableDBH(e.ColumnIndex, e.RowIndex);

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

    private void PlotReferenceObjectsForm_VisibleChanged(object sender, EventArgs e)
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
      this.dgRefObjects.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgRefObjects.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgRefObjects.DefaultCellStyle = Program.ActiveGridDefaultCellStyle;
    }

    public void ContentDeactivated()
    {
      this.dgRefObjects.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgRefObjects.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dgRefObjects.DefaultCellStyle = Program.InActiveGridDefaultCellStyle;
    }

    protected override void OnRequestRefresh()
    {
      DataBindingList<ReferenceObject> dataSource = this.dgRefObjects.DataSource as DataBindingList<ReferenceObject>;
      bool flag = this.m_year != null && this.m_year.Changed && dataSource != null;
      this.dgRefObjects.AllowUserToAddRows = flag;
      this.dgRefObjects.AllowUserToDeleteRows = flag;
      this.dgRefObjects.ReadOnly = !flag;
      this.Enabled = dataSource != null;
      base.OnRequestRefresh();
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgRefObjects.DataSource != null;
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

    private void dgRefObjects_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PlotReferenceObjectsForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      this.dgRefObjects = new DataGridView();
      this.dataGridViewNumericTextBoxColumn1 = new DataGridViewNumericTextBoxColumn();
      this.dataGridViewNumericTextBoxColumn2 = new DataGridViewNumericTextBoxColumn();
      this.dataGridViewNumericTextBoxColumn3 = new DataGridViewNumericTextBoxColumn();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dcObject = new DataGridViewComboBoxColumn();
      this.dcDirection = new DataGridViewNumericTextBoxColumn();
      this.dcDistance = new DataGridViewNumericTextBoxColumn();
      this.dcDBH = new DataGridViewNumericTextBoxColumn();
      this.dcNotes = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dgRefObjects).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgRefObjects.AllowUserToAddRows = false;
      this.dgRefObjects.AllowUserToDeleteRows = false;
      this.dgRefObjects.AllowUserToResizeRows = false;
      this.dgRefObjects.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgRefObjects.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgRefObjects.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgRefObjects.Columns.AddRange((DataGridViewColumn) this.dcObject, (DataGridViewColumn) this.dcDirection, (DataGridViewColumn) this.dcDistance, (DataGridViewColumn) this.dcDBH, (DataGridViewColumn) this.dcNotes);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgRefObjects.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgRefObjects, "dgRefObjects");
      this.dgRefObjects.EnableHeadersVisualStyles = false;
      this.dgRefObjects.MultiSelect = false;
      this.dgRefObjects.Name = "dgRefObjects";
      this.dgRefObjects.ReadOnly = true;
      this.dgRefObjects.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgRefObjects.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgRefObjects.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgRefObjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgRefObjects.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgRefObjects_CellErrorTextNeeded);
      this.dgRefObjects.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgRefObjects_CellFormatting);
      this.dgRefObjects.CellValueChanged += new DataGridViewCellEventHandler(this.dgRefObjects_CellValueChanged);
      this.dgRefObjects.DataError += new DataGridViewDataErrorEventHandler(this.dgRefObjects_DataError);
      this.dgRefObjects.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgRefObjects_EditingControlShowing);
      this.dgRefObjects.RowValidated += new DataGridViewCellEventHandler(this.dgRefObjects_RowValidated);
      this.dgRefObjects.RowValidating += new DataGridViewCellCancelEventHandler(this.dgRefObjects_RowValidating);
      this.dgRefObjects.SelectionChanged += new EventHandler(this.dgRefObjects_SelectionChanged);
      this.dataGridViewNumericTextBoxColumn1.DataPropertyName = "Direction";
      this.dataGridViewNumericTextBoxColumn1.DecimalPlaces = 0;
      this.dataGridViewNumericTextBoxColumn1.Format = "#;#";
      this.dataGridViewNumericTextBoxColumn1.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dataGridViewNumericTextBoxColumn1, "dataGridViewNumericTextBoxColumn1");
      this.dataGridViewNumericTextBoxColumn1.Name = "dataGridViewNumericTextBoxColumn1";
      this.dataGridViewNumericTextBoxColumn1.Resizable = DataGridViewTriState.True;
      this.dataGridViewNumericTextBoxColumn1.Signed = false;
      this.dataGridViewNumericTextBoxColumn2.DataPropertyName = "Distance";
      this.dataGridViewNumericTextBoxColumn2.DecimalPlaces = 0;
      this.dataGridViewNumericTextBoxColumn2.Format = "#;#";
      this.dataGridViewNumericTextBoxColumn2.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dataGridViewNumericTextBoxColumn2, "dataGridViewNumericTextBoxColumn2");
      this.dataGridViewNumericTextBoxColumn2.Name = "dataGridViewNumericTextBoxColumn2";
      this.dataGridViewNumericTextBoxColumn2.Signed = false;
      this.dataGridViewNumericTextBoxColumn3.DataPropertyName = "DBH";
      this.dataGridViewNumericTextBoxColumn3.DecimalPlaces = 2;
      this.dataGridViewNumericTextBoxColumn3.Format = (string) null;
      this.dataGridViewNumericTextBoxColumn3.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dataGridViewNumericTextBoxColumn3, "dataGridViewNumericTextBoxColumn3");
      this.dataGridViewNumericTextBoxColumn3.Name = "dataGridViewNumericTextBoxColumn3";
      this.dataGridViewNumericTextBoxColumn3.Signed = true;
      this.dataGridViewTextBoxColumn1.DataPropertyName = "Notes";
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dcObject.DataPropertyName = "Object";
      this.dcObject.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcObject, "dcObject");
      this.dcObject.Name = "dcObject";
      this.dcObject.ReadOnly = true;
      this.dcObject.Resizable = DataGridViewTriState.True;
      this.dcObject.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDirection.DataPropertyName = "Direction";
      this.dcDirection.DecimalPlaces = 0;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle3.Format = "N0";
      this.dcDirection.DefaultCellStyle = gridViewCellStyle3;
      this.dcDirection.Format = "#;#";
      this.dcDirection.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcDirection, "dcDirection");
      this.dcDirection.Name = "dcDirection";
      this.dcDirection.ReadOnly = true;
      this.dcDirection.Resizable = DataGridViewTriState.True;
      this.dcDirection.Signed = false;
      this.dcDistance.DataPropertyName = "Distance";
      this.dcDistance.DecimalPlaces = 1;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle4.Format = "N1";
      this.dcDistance.DefaultCellStyle = gridViewCellStyle4;
      this.dcDistance.Format = "#;#";
      this.dcDistance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDistance, "dcDistance");
      this.dcDistance.Name = "dcDistance";
      this.dcDistance.ReadOnly = true;
      this.dcDistance.Signed = false;
      this.dcDBH.DataPropertyName = "DBH";
      this.dcDBH.DecimalPlaces = 1;
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle5.Format = "N1";
      this.dcDBH.DefaultCellStyle = gridViewCellStyle5;
      this.dcDBH.Format = (string) null;
      this.dcDBH.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH, "dcDBH");
      this.dcDBH.Name = "dcDBH";
      this.dcDBH.ReadOnly = true;
      this.dcDBH.Signed = true;
      this.dcNotes.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcNotes.DataPropertyName = "Notes";
      componentResourceManager.ApplyResources((object) this.dcNotes, "dcNotes");
      this.dcNotes.MaxInputLength = 100;
      this.dcNotes.Name = "dcNotes";
      this.dcNotes.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CloseButtonVisible = false;
      this.Controls.Add((Control) this.dgRefObjects);
      this.DockAreas = DockAreas.DockBottom;
      this.Name = nameof (PlotReferenceObjectsForm);
      this.ShowHint = DockState.DockBottom;
      this.VisibleChanged += new EventHandler(this.PlotReferenceObjectsForm_VisibleChanged);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgRefObjects, 0);
      ((ISupportInitialize) this.dgRefObjects).EndInit();
      this.ResumeLayout(false);
    }
  }
}
