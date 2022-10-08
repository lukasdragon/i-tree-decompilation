// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.DBHsForm
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

namespace i_Tree_Eco_v6.Forms
{
  public class DBHsForm : ProjectContentForm, IActionable, IExportable
  {
    private ProgramSession m_ps;
    private DataGridViewManager m_dgManager;
    private ISession m_session;
    private Year m_year;
    private TaskManager m_taskManager;
    private WaitCursor m_wc;
    private readonly object m_syncobj;
    private IList<DBHRptClass> m_rptClasses;
    private IList<DBH> m_dbhs;
    private IContainer components;
    private DataGridView dgDBHs;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewTextBoxColumn dcDescription;
    private DataGridViewNumericTextBoxColumn dcValue;
    private DataGridViewTextBoxColumn dcRptClass;
    private DataGridViewTextBoxColumn dcUSFSCategory;

    public DBHsForm()
    {
      this.m_ps = Program.Session;
      this.InitializeComponent();
      this.m_dgManager = new DataGridViewManager(this.dgDBHs);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.m_syncobj = new object();
      this.dgDBHs.DoubleBuffered(true);
      this.dgDBHs.AutoGenerateColumns = false;
      this.dgDBHs.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgDBHs.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.dcValue.DefaultCellStyle.DataSourceNullValue = (object) 0.0;
      this.dcRptClass.Visible = false;
      this.dcUSFSCategory.Visible = false;
      this.m_ps.InputSession.YearChanged += new EventHandler(this.InputSession_YearChanged);
      EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));
      this.Init();
    }

    private void Year_Updated(object sender, EntityUpdated<Year> e)
    {
      if (!(this.m_year.Guid == e.Guid))
        return;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.GetYear(e.Guid)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.OnRequestRefresh();
      }), scheduler));
    }

    private void InputSession_YearChanged(object sender, EventArgs e) => this.Init();

    private void Init()
    {
      if (this.m_ps.InputSession == null || !this.m_ps.InputSession.YearKey.HasValue)
        return;
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
        NHibernateUtil.Initialize((object) y.DBHs);
        IList<DBHRptClass> dbhRptClassList = session.QueryOver<DBHRptClass>().Where((Expression<Func<DBHRptClass, bool>>) (c => c.Year == y)).OrderBy((Expression<Func<DBHRptClass, object>>) (c => (object) c.RangeStart)).Asc.List();
        lock (this.m_syncobj)
        {
          this.m_dbhs = (IList<DBH>) y.DBHs.ToList<DBH>();
          this.m_rptClasses = dbhRptClassList;
          this.m_session = session;
          this.m_year = y;
        }
      }
    }

    private void InitGrid()
    {
      Year year = this.m_year;
      if (year == null)
        return;
      this.dcValue.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtUnit, year.Unit == YearUnit.English ? (object) i_Tree_Eco_v6.Resources.Strings.UnitInchesAbbr : (object) i_Tree_Eco_v6.Resources.Strings.UnitCentimetersAbbr));
      DataBindingList<DBH> dataBindingList = new DataBindingList<DBH>(this.m_dbhs);
      this.dgDBHs.DataSource = (object) dataBindingList;
      this.dgDBHs.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.DBHs_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.DBHs_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.DBHs_ListChanged);
    }

    protected override void OnRequestRefresh()
    {
      Year year = this.m_year;
      DataGridViewRow currentRow = this.dgDBHs.CurrentRow;
      bool flag1 = year != null && year.ConfigEnabled && this.dgDBHs.DataSource != null;
      bool flag2 = flag1;
      bool flag3 = flag1 && this.m_dbhs.Count > 1;
      this.dgDBHs.ReadOnly = !flag1;
      this.dgDBHs.AllowUserToAddRows = flag2;
      this.dgDBHs.AllowUserToDeleteRows = flag3;
      this.dcRptClass.ReadOnly = true;
      this.dcUSFSCategory.ReadOnly = true;
      base.OnRequestRefresh();
    }

    private void GetYear(Guid g)
    {
      if (!(this.m_year != null & this.m_session != null) || !(this.m_year.Guid == g))
        return;
      using (this.m_session.BeginTransaction())
        this.m_session.Refresh((object) this.m_year);
    }

    private void DBHs_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void DBHs_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<DBH> dataBindingList = sender as DataBindingList<DBH>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      DBH dbh = dataBindingList[e.NewIndex];
      if (dbh.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) dbh);
          transaction.Commit();
        }
      }
    }

    private void DBHs_AddingNew(object sender, AddingNewEventArgs e)
    {
      DataBindingList<DBH> dataBindingList = sender as DataBindingList<DBH>;
      DBH dbh1 = new DBH();
      int num = 1;
      foreach (DBH dbh2 in (Collection<DBH>) dataBindingList)
      {
        if (dbh2.Id >= num)
          num = dbh2.Id + 1;
      }
      dbh1.Year = this.m_year;
      dbh1.Id = num;
      e.NewObject = (object) dbh1;
    }

    private void dgDBHs_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void dgDBHs_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgDBHs.NewRowIndex || !(this.dgDBHs.DataSource is DataBindingList<DBH> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgDBHs.Columns[e.ColumnIndex];
      DBH dbh = dataSource[e.RowIndex];
      if (column == this.dcId)
      {
        if (dbh.Id != 0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else
      {
        if (column != this.dcValue || dbh.Value > 0.0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
      }
    }

    private void dgDBHs_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<DBH> dataSource = this.dgDBHs.DataSource as DataBindingList<DBH>;
      if (this.dgDBHs.CurrentRow != null && !this.dgDBHs.IsCurrentRowDirty)
        return;
      string text = (string) null;
      if (dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      DBH dbh1 = dataSource[e.RowIndex];
      DataGridViewRow row = this.dgDBHs.Rows[e.RowIndex];
      DataGridViewCell dataGridViewCell1 = (DataGridViewCell) null;
      foreach (DBH dbh2 in (Collection<DBH>) dataSource)
      {
        if (dbh2 != dbh1 && dbh2.Id == dbh1.Id)
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
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgDBHs_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgDBHs.ReadOnly)
        return;
      DataBindingList<DBH> dataSource = this.dgDBHs.DataSource as DataBindingList<DBH>;
      if (this.dgDBHs.CurrentRow != null && this.dgDBHs.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          DBH dbh = dataSource[e.RowIndex];
          if (dbh.IsTransient || this.m_session.IsDirty())
          {
            lock (this.m_syncobj)
            {
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                this.m_session.SaveOrUpdate((object) dbh);
                transaction.Commit();
              }
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgDBHs_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void DeleteSelectedRows()
    {
      if (!(this.dgDBHs.DataSource is DataBindingList<DBH> dataSource))
        return;
      CurrencyManager currencyManager = this.dgDBHs.BindingContext[(object) dataSource] as CurrencyManager;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgDBHs.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
          dataSource.RemoveAt(selectedRow.Index);
      }
      if (currencyManager.Position == -1)
        return;
      this.dgDBHs.Rows[currencyManager.Position].Selected = true;
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgDBHs.DataSource != null;
      bool flag2 = flag1 && this.dgDBHs.AllowUserToAddRows;
      bool flag3 = this.dgDBHs.SelectedRows.Count > 0;
      bool flag4 = this.dgDBHs.CurrentRow != null && this.dgDBHs.CurrentRow.IsNewRow;
      bool flag5 = this.dgDBHs.CurrentRow != null && this.dgDBHs.IsCurrentRowDirty;
      bool flag6 = false;
      switch (action)
      {
        case UserActions.New:
          flag6 = flag2 && !flag4 && !flag5;
          break;
        case UserActions.Undo:
          flag6 = flag1 && this.m_dgManager.CanUndo;
          break;
        case UserActions.Redo:
          flag6 = flag1 && this.m_dgManager.CanRedo;
          break;
        case UserActions.Delete:
          flag6 = flag1 & flag3 && !flag4 | flag5;
          break;
        case UserActions.RestoreDefaults:
          flag6 = flag1;
          break;
      }
      return flag6;
    }

    public void PerformAction(UserActions action)
    {
      switch (action)
      {
        case UserActions.New:
          if (!this.dgDBHs.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgDBHs.SelectedRows)
            selectedRow.Selected = false;
          this.dgDBHs.Rows[this.dgDBHs.NewRowIndex].Selected = true;
          this.dgDBHs.FirstDisplayedScrollingRowIndex = this.dgDBHs.NewRowIndex - this.dgDBHs.DisplayedRowCount(false) + 1;
          this.dgDBHs.CurrentCell = this.dgDBHs.Rows[this.dgDBHs.NewRowIndex].Cells[0];
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          if (this.dgDBHs.SelectedRows.Count <= 0)
            break;
          this.DeleteSelectedRows();
          break;
        case UserActions.RestoreDefaults:
          this.RestoreDefaults();
          break;
      }
    }

    private void RestoreDefaults()
    {
      try
      {
        this.m_session.Lock((object) this.m_year, LockMode.None);
        this.dgDBHs.DataSource = (object) null;
        if (!YearHelper.RestoreDefaultDBHs(this.m_year, this.m_session))
        {
          int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrRestore, (object) v6Strings.DBH_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      catch (HibernateException ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrRestore, (object) v6Strings.DBH_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      this.Init();
    }

    public void Export(string file) => this.dgDBHs.ExportToCSV(file);

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgDBHs.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgDBHs.DataSource != null;

    private void dgDBHs_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgDBHs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      DataGridViewColumn column = this.dgDBHs.Columns[e.ColumnIndex];
      if (e.Value == null)
        return;
      if (column == this.dcRptClass)
      {
        double num = (double) e.Value;
        if (num == 0.0)
        {
          e.Value = (object) null;
        }
        else
        {
          string str = this.m_year.Unit == YearUnit.English ? i_Tree_Eco_v6.Resources.Strings.UnitInchesAbbr : i_Tree_Eco_v6.Resources.Strings.UnitCentimetersAbbr;
          foreach (DBHRptClass rptClass in (IEnumerable<DBHRptClass>) this.m_rptClasses)
          {
            if (rptClass.RangeStart < num && rptClass.RangeEnd > num)
            {
              if (Math.Abs(rptClass.RangeEnd - 1000.0) > double.Epsilon)
              {
                e.Value = (object) string.Format("> {0} and <= {1}{2}", (object) rptClass.RangeStart, (object) rptClass.RangeEnd, (object) str);
                break;
              }
              e.Value = (object) string.Format("> {0}{1}", (object) rptClass.RangeStart, (object) str);
              break;
            }
          }
        }
      }
      else
      {
        if (column != this.dcUSFSCategory)
          return;
        double num1 = (double) e.Value;
        double num2 = this.m_year.Unit == YearUnit.English ? 3.0 : 7.6;
        string str = this.m_year.Unit == YearUnit.English ? i_Tree_Eco_v6.Resources.Strings.UnitInchesAbbr : i_Tree_Eco_v6.Resources.Strings.UnitCentimetersAbbr;
        double num3 = num2;
        double num4 = Math.Ceiling(num1 / num3);
        double num5 = (num4 - 1.0) * num2;
        double num6 = num4 * num2;
        e.Value = (object) string.Format("> {0:0.#} and <= {1:0.#}{2}", (object) num5, (object) num6, (object) str);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DBHsForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      this.dgDBHs = new DataGridView();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcDescription = new DataGridViewTextBoxColumn();
      this.dcValue = new DataGridViewNumericTextBoxColumn();
      this.dcRptClass = new DataGridViewTextBoxColumn();
      this.dcUSFSCategory = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dgDBHs).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgDBHs.AllowUserToResizeRows = false;
      this.dgDBHs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgDBHs.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgDBHs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgDBHs.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcDescription, (DataGridViewColumn) this.dcValue, (DataGridViewColumn) this.dcRptClass, (DataGridViewColumn) this.dcUSFSCategory);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgDBHs.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgDBHs, "dgDBHs");
      this.dgDBHs.EnableHeadersVisualStyles = false;
      this.dgDBHs.MultiSelect = false;
      this.dgDBHs.Name = "dgDBHs";
      this.dgDBHs.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgDBHs.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgDBHs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgDBHs.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgDBHs_CellErrorTextNeeded);
      this.dgDBHs.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgDBHs_CellFormatting);
      this.dgDBHs.CurrentCellDirtyStateChanged += new EventHandler(this.dgDBHs_CurrentCellDirtyStateChanged);
      this.dgDBHs.DataError += new DataGridViewDataErrorEventHandler(this.dgDBHs_DataError);
      this.dgDBHs.RowValidated += new DataGridViewCellEventHandler(this.dgDBHs_RowValidated);
      this.dgDBHs.RowValidating += new DataGridViewCellCancelEventHandler(this.dgDBHs_RowValidating);
      this.dgDBHs.SelectionChanged += new EventHandler(this.dgDBHs_SelectionChanged);
      this.dcId.DataPropertyName = "Id";
      this.dcId.DecimalPlaces = 0;
      this.dcId.Format = "#;#";
      this.dcId.Frozen = true;
      this.dcId.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcId, "dcId");
      this.dcId.Name = "dcId";
      this.dcId.Signed = false;
      this.dcDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDescription.DataPropertyName = "Description";
      componentResourceManager.ApplyResources((object) this.dcDescription, "dcDescription");
      this.dcDescription.MaxInputLength = 50;
      this.dcDescription.Name = "dcDescription";
      this.dcValue.DataPropertyName = "Value";
      this.dcValue.DecimalPlaces = 2;
      gridViewCellStyle3.Format = "0.##";
      this.dcValue.DefaultCellStyle = gridViewCellStyle3;
      this.dcValue.Format = "0.#;0.#";
      this.dcValue.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcValue, "dcValue");
      this.dcValue.Name = "dcValue";
      this.dcValue.Signed = false;
      this.dcRptClass.DataPropertyName = "Value";
      gridViewCellStyle4.BackColor = Color.LightGray;
      gridViewCellStyle4.ForeColor = Color.Black;
      this.dcRptClass.DefaultCellStyle = gridViewCellStyle4;
      componentResourceManager.ApplyResources((object) this.dcRptClass, "dcRptClass");
      this.dcRptClass.Name = "dcRptClass";
      this.dcRptClass.ReadOnly = true;
      this.dcUSFSCategory.DataPropertyName = "Value";
      gridViewCellStyle5.BackColor = Color.LightGray;
      gridViewCellStyle5.ForeColor = Color.Black;
      gridViewCellStyle5.Format = "0.#";
      this.dcUSFSCategory.DefaultCellStyle = gridViewCellStyle5;
      componentResourceManager.ApplyResources((object) this.dcUSFSCategory, "dcUSFSCategory");
      this.dcUSFSCategory.Name = "dcUSFSCategory";
      this.dcUSFSCategory.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.dgDBHs);
      this.Name = nameof (DBHsForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgDBHs, 0);
      ((ISupportInitialize) this.dgDBHs).EndInit();
      this.ResumeLayout(false);
    }
  }
}
