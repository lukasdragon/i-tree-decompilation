// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.DBHRptClassesForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class DBHRptClassesForm : DataContentForm, IActionable, IExportable
  {
    private DataGridViewManager m_dgManager;
    private IList<DBHRptClass> m_dbhClasses;
    private IContainer components;
    private DataGridView dgClasses;
    private FlowLayoutPanel flowLayoutPanel1;
    private Label lblClassifiedDBH;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewNumericTextBoxColumn dcRangeStart;
    private DataGridViewNumericTextBoxColumn dcRangeEnd;

    public DBHRptClassesForm()
    {
      this.InitializeComponent();
      this.dgClasses.DoubleBuffered(true);
      this.dgClasses.AutoGenerateColumns = false;
      this.dgClasses.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgClasses.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.dcRangeStart.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcRangeEnd.DefaultCellStyle.DataSourceNullValue = (object) 1000.0;
      this.m_dgManager = new DataGridViewManager(this.dgClasses);
    }

    private double MaxDBH => 1000.0;

    protected override void LoadData()
    {
      base.LoadData();
      lock (this.Session)
      {
        using (this.Session.BeginTransaction())
          this.m_dbhClasses = this.Session.QueryOver<DBHRptClass>().Where((Expression<Func<DBHRptClass, bool>>) (c => c.Year == this.Year)).OrderBy((Expression<Func<DBHRptClass, object>>) (c => (object) c.RangeStart)).Asc.List();
      }
    }

    protected override void OnDataLoaded()
    {
      this.InitGrid();
      base.OnDataLoaded();
    }

    private void InitGrid()
    {
      Year year = this.Year;
      if (year == null)
        return;
      string str = year.Unit != YearUnit.English ? i_Tree_Eco_v6.Resources.Strings.UnitCentimetersAbbr : i_Tree_Eco_v6.Resources.Strings.UnitInchesAbbr;
      this.dcRangeStart.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.RangeFrom, (object) str);
      this.dcRangeEnd.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.RangeTo, (object) str);
      this.lblClassifiedDBH.Text = i_Tree_Eco_v6.Resources.Strings.MsgDBHReporting;
      DataBindingList<DBHRptClass> dataBindingList = new DataBindingList<DBHRptClass>(this.m_dbhClasses);
      this.dgClasses.DataSource = (object) dataBindingList;
      this.dgClasses.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.DBHClasses_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.DBHClasses_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.DBHClasses_ListChanged);
    }

    protected override void OnRequestRefresh()
    {
      Year year = this.Year;
      DataGridViewRow currentRow = this.dgClasses.CurrentRow;
      bool flag1 = year != null && year.Changed && this.dgClasses.DataSource != null;
      bool flag2 = flag1 && (this.m_dbhClasses.Count < 10 || currentRow != null && currentRow.IsNewRow && !this.dgClasses.IsCurrentRowDirty);
      bool flag3 = flag1 && this.m_dbhClasses.Count > 1;
      this.dgClasses.ReadOnly = !flag1;
      this.dgClasses.AllowUserToAddRows = flag2;
      this.dgClasses.AllowUserToDeleteRows = flag3;
      base.OnRequestRefresh();
    }

    private void DBHClasses_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType != ListChangedType.ItemDeleted)
        return;
      this.FixDBHClasses(sender as DataBindingList<DBHRptClass>);
    }

    private void DBHClasses_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      if (!(sender is DataBindingList<DBHRptClass> dataBindingList) || e.NewIndex >= dataBindingList.Count)
        return;
      DBHRptClass dbhRptClass = dataBindingList[e.NewIndex];
      if (dbhRptClass.IsTransient)
        return;
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          this.Session.Delete((object) dbhRptClass);
          transaction.Commit();
        }
      }
    }

    private void DBHClasses_AddingNew(object sender, AddingNewEventArgs e)
    {
      DataBindingList<DBHRptClass> dataBindingList = sender as DataBindingList<DBHRptClass>;
      DBHRptClass dbhRptClass1 = new DBHRptClass();
      int num = 1;
      foreach (DBHRptClass dbhRptClass2 in (Collection<DBHRptClass>) dataBindingList)
      {
        if (dbhRptClass2.Id >= num)
          num = dbhRptClass2.Id + 1;
      }
      dbhRptClass1.Id = num;
      dbhRptClass1.RangeStart = -1.0;
      dbhRptClass1.Year = this.Year;
      e.NewObject = (object) dbhRptClass1;
    }

    private void dgClasses_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<DBHRptClass> dataSource = this.dgClasses.DataSource as DataBindingList<DBHRptClass>;
      if (this.dgClasses.CurrentRow != null && !this.dgClasses.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      DBHRptClass dbhRptClass1 = dataSource[e.RowIndex];
      string text = (string) null;
      if (dbhRptClass1.Id == 0)
        text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) this.dcId.HeaderText);
      else if (dbhRptClass1.RangeStart < 0.0)
        text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) this.dcRangeStart.HeaderText, (object) 0);
      if (text == null)
      {
        foreach (DBHRptClass dbhRptClass2 in (Collection<DBHRptClass>) dataSource)
        {
          if (dbhRptClass2 != dbhRptClass1 && dbhRptClass2.Id == dbhRptClass1.Id)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcId.HeaderText);
            break;
          }
          if (dbhRptClass2 != dbhRptClass1 && Math.Abs(dbhRptClass2.RangeStart - dbhRptClass1.RangeStart) < double.Epsilon)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcRangeStart.HeaderText);
            break;
          }
        }
      }
      if (text == null)
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgClasses_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgClasses.ReadOnly)
        return;
      if (this.dgClasses.CurrentRow != null && this.dgClasses.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (!(this.dgClasses.DataSource is DataBindingList<DBHRptClass> dataSource) || e.RowIndex >= dataSource.Count)
          return;
        this.FixDBHClasses(dataSource);
      }
    }

    private void FixDBHClasses(DataBindingList<DBHRptClass> dsDBHClasses)
    {
      List<DBHRptClass> list = dsDBHClasses.ToList<DBHRptClass>();
      if (list.Count > 0)
      {
        list.Sort((Comparison<DBHRptClass>) ((d1, d2) => d1.RangeStart.CompareTo(d2.RangeStart)));
        bool flag = false;
        for (int index = 1; index < list.Count; ++index)
        {
          list[index - 1].RangeEnd = list[index].RangeStart;
          flag |= list[index - 1].IsTransient;
        }
        list[list.Count - 1].RangeEnd = this.MaxDBH;
        if (flag | list[list.Count - 1].IsTransient || this.Session.IsDirty())
        {
          lock (this.Session)
          {
            using (ITransaction transaction = this.Session.BeginTransaction())
            {
              for (int index = 0; index < list.Count; ++index)
                this.Session.SaveOrUpdate((object) list[index]);
              transaction.Commit();
            }
          }
        }
      }
      this.OnRequestRefresh();
    }

    private void DeleteSelectedRows()
    {
      if (!(this.dgClasses.DataSource is DataBindingList<DBHRptClass> dataSource))
        return;
      CurrencyManager currencyManager = this.dgClasses.BindingContext[(object) dataSource] as CurrencyManager;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgClasses.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
          dataSource.RemoveAt(selectedRow.Index);
      }
      if (currencyManager.Position == -1)
        return;
      this.dgClasses.Rows[currencyManager.Position].Selected = true;
    }

    public bool CanPerformAction(UserActions action)
    {
      Year year = this.Year;
      DataGridViewRow currentRow = this.dgClasses.CurrentRow;
      bool flag1 = year != null && year.Changed && this.dgClasses.DataSource != null;
      bool flag2 = flag1 && this.dgClasses.AllowUserToAddRows;
      bool flag3 = this.dgClasses.SelectedRows.Count > 0;
      bool flag4 = currentRow != null && currentRow.IsNewRow;
      bool flag5 = currentRow != null && this.dgClasses.IsCurrentRowDirty;
      DBHRptClass dbhRptClass = currentRow == null ? (DBHRptClass) null : currentRow.DataBoundItem as DBHRptClass;
      bool flag6 = flag1 & flag3 && !flag4 | flag5 && this.m_dbhClasses.Count > 1 && dbhRptClass != null && dbhRptClass.RangeStart != 0.0;
      bool flag7 = false;
      switch (action)
      {
        case UserActions.New:
          flag7 = flag2 && !flag5 && !flag4;
          break;
        case UserActions.Copy:
          flag7 = false;
          break;
        case UserActions.Undo:
          flag7 = flag1 && this.m_dgManager.CanUndo;
          break;
        case UserActions.Redo:
          flag7 = flag1 && this.m_dgManager.CanRedo;
          break;
        case UserActions.Delete:
          flag7 = flag6;
          break;
        case UserActions.RestoreDefaults:
          flag7 = flag1;
          break;
      }
      return flag7;
    }

    public void PerformAction(UserActions action)
    {
      switch (action)
      {
        case UserActions.New:
          if (!this.dgClasses.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgClasses.SelectedRows)
            selectedRow.Selected = false;
          this.dgClasses.Rows[this.dgClasses.NewRowIndex].Selected = true;
          this.dgClasses.FirstDisplayedScrollingRowIndex = this.dgClasses.NewRowIndex - this.dgClasses.DisplayedRowCount(false) + 1;
          this.dgClasses.CurrentCell = this.dgClasses.Rows[this.dgClasses.NewRowIndex].Cells[0];
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          if (this.dgClasses.SelectedRows.Count <= 0)
            break;
          this.DeleteSelectedRows();
          break;
        case UserActions.RestoreDefaults:
          this.RestoreDefaults();
          break;
      }
    }

    private void dgClasses_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void dgClasses_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void ntbMaxDBH_Validated(object sender, EventArgs e)
    {
      if (!(this.dgClasses.DataSource is DataBindingList<DBHRptClass> dataSource) || dataSource.Count <= 0)
        return;
      List<DBHRptClass> list = dataSource.ToList<DBHRptClass>();
      list.Sort((Comparison<DBHRptClass>) ((d1, d2) => d2.RangeStart.CompareTo(d1.RangeStart)));
      list[0].RangeEnd = this.MaxDBH;
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          this.Session.SaveOrUpdate((object) list[0]);
          transaction.Commit();
        }
      }
    }

    private void RestoreDefaults()
    {
      DataBindingList<DBHRptClass> dataSource = this.dgClasses.DataSource as DataBindingList<DBHRptClass>;
      double num1 = 7.62;
      if (this.Year.Unit == YearUnit.English)
        num1 = 3.0;
      this.dgClasses.DataSource = (object) null;
      List<DBHRptClass> list = new List<DBHRptClass>();
      int num2 = 0;
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          foreach (object obj in (Collection<DBHRptClass>) dataSource)
            this.Session.Delete(obj);
          transaction.Commit();
        }
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          for (int index = 0; index < 10; ++index)
          {
            DBHRptClass dbhRptClass = new DBHRptClass();
            dbhRptClass.Year = this.Year;
            dbhRptClass.Id = index + 1;
            dbhRptClass.RangeStart = Math.Round((double) num2 * num1, 1);
            if (index == 2)
            {
              num1 *= 2.0;
              num2 /= 2;
            }
            dbhRptClass.RangeEnd = index >= 9 ? 1000.0 : Math.Round((double) (num2 + 1) * num1, 1);
            list.Add(dbhRptClass);
            this.Session.SaveOrUpdate((object) dbhRptClass);
            ++num2;
          }
          transaction.Commit();
        }
      }
      DataBindingList<DBHRptClass> dataBindingList = new DataBindingList<DBHRptClass>((IList<DBHRptClass>) list);
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.DBHClasses_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.DBHClasses_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.DBHClasses_ListChanged);
      this.dgClasses.DataSource = (object) dataBindingList;
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgClasses.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgClasses.DataSource != null;

    private void dgClasses_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (e.RowIndex == this.dgClasses.NewRowIndex)
        return;
      DataGridViewRow row = this.dgClasses.Rows[e.RowIndex];
      if (!(row.DataBoundItem is DBHRptClass dataBoundItem) || dataBoundItem.RangeStart != 0.0 || e.ColumnIndex != this.dcRangeStart.DisplayIndex)
        return;
      DataGridViewCell cell = row.Cells[e.ColumnIndex];
      cell.ReadOnly = true;
      cell.Style.BackColor = SystemColors.ControlLight;
    }

    private void dgClasses_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (e.Row.IsNewRow)
        return;
      DBHRptClass dataBoundItem = e.Row.DataBoundItem as DBHRptClass;
      e.Cancel = dataBoundItem != null && dataBoundItem.RangeStart == 0.0;
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
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      this.dgClasses = new DataGridView();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.lblClassifiedDBH = new Label();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcRangeStart = new DataGridViewNumericTextBoxColumn();
      this.dcRangeEnd = new DataGridViewNumericTextBoxColumn();
      ((ISupportInitialize) this.dgClasses).BeginInit();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.lblBreadcrumb.Size = new Size(591, 31);
      this.dgClasses.AllowUserToResizeRows = false;
      this.dgClasses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgClasses.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgClasses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgClasses.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcRangeStart, (DataGridViewColumn) this.dcRangeEnd);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgClasses.DefaultCellStyle = gridViewCellStyle2;
      this.dgClasses.Dock = DockStyle.Fill;
      this.dgClasses.EnableHeadersVisualStyles = false;
      this.dgClasses.Location = new Point(0, 62);
      this.dgClasses.MultiSelect = false;
      this.dgClasses.Name = "dgClasses";
      this.dgClasses.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgClasses.RowHeadersWidth = 20;
      this.dgClasses.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgClasses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgClasses.Size = new Size(591, 254);
      this.dgClasses.TabIndex = 2;
      this.dgClasses.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgClasses_CellFormatting);
      this.dgClasses.CurrentCellDirtyStateChanged += new EventHandler(this.dgClasses_CurrentCellDirtyStateChanged);
      this.dgClasses.RowValidated += new DataGridViewCellEventHandler(this.dgClasses_RowValidated);
      this.dgClasses.RowValidating += new DataGridViewCellCancelEventHandler(this.dgClasses_RowValidating);
      this.dgClasses.SelectionChanged += new EventHandler(this.dgClasses_SelectionChanged);
      this.dgClasses.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.dgClasses_UserDeletingRow);
      this.flowLayoutPanel1.AutoSize = true;
      this.flowLayoutPanel1.Controls.Add((Control) this.lblClassifiedDBH);
      this.flowLayoutPanel1.Dock = DockStyle.Top;
      this.flowLayoutPanel1.Location = new Point(0, 31);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(591, 31);
      this.flowLayoutPanel1.TabIndex = 4;
      this.lblClassifiedDBH.AutoSize = true;
      this.flowLayoutPanel1.SetFlowBreak((Control) this.lblClassifiedDBH, true);
      this.lblClassifiedDBH.Font = new Font("Calibri", 13f);
      this.lblClassifiedDBH.Location = new Point(3, 6);
      this.lblClassifiedDBH.Margin = new Padding(3, 6, 3, 3);
      this.lblClassifiedDBH.Name = "lblClassifiedDBH";
      this.lblClassifiedDBH.Size = new Size(282, 22);
      this.lblClassifiedDBH.TabIndex = 2;
      this.lblClassifiedDBH.Text = "DBH Classes are used for REPORTING";
      this.dcId.DataPropertyName = "Id";
      this.dcId.DecimalPlaces = 0;
      gridViewCellStyle3.BackColor = Color.White;
      this.dcId.DefaultCellStyle = gridViewCellStyle3;
      this.dcId.Format = "#;#";
      this.dcId.Frozen = true;
      this.dcId.HasDecimal = false;
      this.dcId.HeaderText = "ID";
      this.dcId.Name = "dcId";
      this.dcId.Signed = false;
      this.dcRangeStart.DataPropertyName = "RangeStart";
      this.dcRangeStart.DecimalPlaces = 1;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle4.BackColor = Color.White;
      gridViewCellStyle4.Format = "0.#";
      gridViewCellStyle4.NullValue = (object) null;
      this.dcRangeStart.DefaultCellStyle = gridViewCellStyle4;
      this.dcRangeStart.Format = "0.#;-0.#";
      this.dcRangeStart.HasDecimal = true;
      this.dcRangeStart.HeaderText = "From >";
      this.dcRangeStart.Name = "dcRangeStart";
      this.dcRangeStart.Resizable = DataGridViewTriState.True;
      this.dcRangeStart.Signed = true;
      this.dcRangeEnd.DataPropertyName = "RangeEnd";
      this.dcRangeEnd.DecimalPlaces = 1;
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle5.BackColor = SystemColors.ControlLight;
      gridViewCellStyle5.Format = "#.#";
      gridViewCellStyle5.NullValue = (object) null;
      this.dcRangeEnd.DefaultCellStyle = gridViewCellStyle5;
      this.dcRangeEnd.Format = "#.#;#.#";
      this.dcRangeEnd.HasDecimal = true;
      this.dcRangeEnd.HeaderText = "To <=";
      this.dcRangeEnd.Name = "dcRangeEnd";
      this.dcRangeEnd.ReadOnly = true;
      this.dcRangeEnd.Resizable = DataGridViewTriState.True;
      this.dcRangeEnd.Signed = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(591, 316);
      this.Controls.Add((Control) this.dgClasses);
      this.Controls.Add((Control) this.flowLayoutPanel1);
      this.Name = "DBHClassesForm";
      this.Text = "DBH Class";
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.flowLayoutPanel1, 0);
      this.Controls.SetChildIndex((Control) this.dgClasses, 0);
      ((ISupportInitialize) this.dgClasses).EndInit();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
