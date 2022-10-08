// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.HealthRptClassesForm
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
using i_Tree_Eco_v6.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class HealthRptClassesForm : DataContentForm, IActionable, IExportable
  {
    private DataGridViewManager m_dgManager;
    private IList<HealthRptClass> m_healthClasses;
    private IContainer components;
    private DataGridView dgClasses;
    private FlowLayoutPanel flowLayoutPanel1;
    private Label lblHealthClasses;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewTextBoxColumn dcDescription;
    private DataGridViewNumericTextBoxColumn dcRangeEnd;

    public HealthRptClassesForm()
    {
      this.InitializeComponent();
      this.dgClasses.DoubleBuffered(true);
      this.dgClasses.AutoGenerateColumns = false;
      this.dgClasses.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgClasses.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.m_dgManager = new DataGridViewManager(this.dgClasses);
    }

    protected override void LoadData()
    {
      base.LoadData();
      lock (this.Session)
      {
        using (this.Session.BeginTransaction())
          this.m_healthClasses = this.Session.QueryOver<HealthRptClass>().Where((Expression<Func<HealthRptClass, bool>>) (c => c.Year == this.Year)).OrderBy((Expression<Func<HealthRptClass, object>>) (c => (object) c.Extent)).Asc.List();
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
      if (year.DisplayCondition)
        this.dcRangeEnd.HeaderText = string.Format("{0} >=", (object) v6Strings.Condition_Percent);
      else
        this.dcRangeEnd.HeaderText = string.Format("{0} <=", (object) v6Strings.Condition_PctDieback);
      DataBindingList<HealthRptClass> dataBindingList = new DataBindingList<HealthRptClass>(this.m_healthClasses);
      this.dgClasses.DataSource = (object) dataBindingList;
      this.dgClasses.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.HealthClasses_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.HealthClasses_BeforeRemove);
    }

    protected override void OnRequestRefresh()
    {
      Year year = this.Year;
      DataGridViewRow currentRow = this.dgClasses.CurrentRow;
      bool flag1 = year != null && year.Changed && this.dgClasses.DataSource != null;
      bool flag2 = flag1 && (this.m_healthClasses.Count < 10 || currentRow != null && currentRow.IsNewRow && !this.dgClasses.IsCurrentRowDirty);
      bool flag3 = flag1 && this.m_healthClasses.Count > 1;
      this.dgClasses.ReadOnly = !flag1;
      this.dgClasses.AllowUserToAddRows = flag2;
      this.dgClasses.AllowUserToDeleteRows = flag3;
      base.OnRequestRefresh();
    }

    private void HealthClasses_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      if (!(sender is DataBindingList<HealthRptClass> dataBindingList) || e.NewIndex >= dataBindingList.Count)
        return;
      HealthRptClass healthRptClass = dataBindingList[e.NewIndex];
      if (healthRptClass.IsTransient)
        return;
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          this.Session.Delete((object) healthRptClass);
          transaction.Commit();
        }
      }
    }

    private void HealthClasses_AddingNew(object sender, AddingNewEventArgs e)
    {
      DataBindingList<HealthRptClass> dataBindingList = sender as DataBindingList<HealthRptClass>;
      HealthRptClass healthRptClass1 = new HealthRptClass();
      int num = 1;
      foreach (HealthRptClass healthRptClass2 in (Collection<HealthRptClass>) dataBindingList)
      {
        if (healthRptClass2.Id >= num)
          num = healthRptClass2.Id + 1;
      }
      healthRptClass1.Id = num;
      healthRptClass1.Year = this.Year;
      e.NewObject = (object) healthRptClass1;
    }

    private void dgClasses_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<HealthRptClass> dataSource = this.dgClasses.DataSource as DataBindingList<HealthRptClass>;
      if (this.dgClasses.CurrentRow != null && !this.dgClasses.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      HealthRptClass healthRptClass1 = dataSource[e.RowIndex];
      string text = (string) null;
      if (healthRptClass1.Id == 0)
        text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) this.dcId.HeaderText);
      else if (healthRptClass1.Extent < 0.0)
        text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) this.dcRangeEnd.HeaderText, (object) 0);
      if (text == null)
      {
        foreach (HealthRptClass healthRptClass2 in (Collection<HealthRptClass>) dataSource)
        {
          if (healthRptClass2 != healthRptClass1 && healthRptClass2.Id == healthRptClass1.Id)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcId.HeaderText);
            break;
          }
          if (healthRptClass2 != healthRptClass1 && Math.Abs(healthRptClass2.Extent - healthRptClass1.Extent) < double.Epsilon)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcRangeEnd.HeaderText);
            break;
          }
          if (healthRptClass2 != healthRptClass1 && healthRptClass2.Description == healthRptClass1.Description)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcDescription.HeaderText);
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
      DataBindingList<HealthRptClass> dataSource = this.dgClasses.DataSource as DataBindingList<HealthRptClass>;
      if (this.dgClasses.CurrentRow != null && this.dgClasses.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          HealthRptClass healthRptClass = dataSource[e.RowIndex];
          if (healthRptClass.IsTransient || this.Session.IsDirty())
          {
            lock (this.Session)
            {
              using (ITransaction transaction = this.Session.BeginTransaction())
              {
                this.Session.SaveOrUpdate((object) healthRptClass);
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
      if (!(this.dgClasses.DataSource is DataBindingList<HealthRptClass> dataSource))
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
      HealthRptClass healthRptClass = currentRow == null ? (HealthRptClass) null : currentRow.DataBoundItem as HealthRptClass;
      bool flag6 = flag1 & flag3 && !flag4 | flag5 && this.m_healthClasses.Count > 1 && healthRptClass != null && healthRptClass.Extent != 100.0;
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

    private void RestoreDefaults()
    {
      DataBindingList<HealthRptClass> dataSource = this.dgClasses.DataSource as DataBindingList<HealthRptClass>;
      List<HealthRptClass> list = new List<HealthRptClass>();
      this.dgClasses.DataSource = (object) null;
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          foreach (object obj in (Collection<HealthRptClass>) dataSource)
            this.Session.Delete(obj);
          transaction.Commit();
        }
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          int num = 1;
          foreach (ConditionCategory conditionCategory in Enum.GetValues(typeof (ConditionCategory)))
          {
            HealthRptClass healthRptClass = new HealthRptClass();
            healthRptClass.Year = this.Year;
            healthRptClass.Id = num++;
            healthRptClass.Extent = (double) conditionCategory;
            healthRptClass.Description = EnumHelper.GetDescription<ConditionCategory>(conditionCategory);
            list.Add(healthRptClass);
            this.Session.SaveOrUpdate((object) healthRptClass);
          }
          transaction.Commit();
        }
      }
      DataBindingList<HealthRptClass> dataBindingList = new DataBindingList<HealthRptClass>((IList<HealthRptClass>) list);
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.HealthClasses_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.HealthClasses_BeforeRemove);
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
      if (!(row.DataBoundItem is HealthRptClass dataBoundItem) || e.ColumnIndex != this.dcRangeEnd.DisplayIndex)
        return;
      if (this.Year.DisplayCondition)
        e.Value = (object) (100.0 - dataBoundItem.Extent);
      if (dataBoundItem.Extent != 100.0)
        return;
      DataGridViewCell cell = row.Cells[e.ColumnIndex];
      cell.ReadOnly = true;
      cell.Style.BackColor = SystemColors.ControlLight;
    }

    private void dgClasses_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (e.Row.IsNewRow)
        return;
      HealthRptClass dataBoundItem = e.Row.DataBoundItem as HealthRptClass;
      e.Cancel = dataBoundItem != null && dataBoundItem.Extent == 100.0;
    }

    private void dgClasses_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
    {
      if (this.dgClasses.Columns[e.ColumnIndex] != this.dcRangeEnd || !this.Year.DisplayCondition)
        return;
      e.Value = (object) (100.0 - Convert.ToDouble(e.Value));
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
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      this.dgClasses = new DataGridView();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.lblHealthClasses = new Label();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcDescription = new DataGridViewTextBoxColumn();
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
      this.dgClasses.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcDescription, (DataGridViewColumn) this.dcRangeEnd);
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
      this.dgClasses.CellParsing += new DataGridViewCellParsingEventHandler(this.dgClasses_CellParsing);
      this.dgClasses.CurrentCellDirtyStateChanged += new EventHandler(this.dgClasses_CurrentCellDirtyStateChanged);
      this.dgClasses.RowValidated += new DataGridViewCellEventHandler(this.dgClasses_RowValidated);
      this.dgClasses.RowValidating += new DataGridViewCellCancelEventHandler(this.dgClasses_RowValidating);
      this.dgClasses.SelectionChanged += new EventHandler(this.dgClasses_SelectionChanged);
      this.dgClasses.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.dgClasses_UserDeletingRow);
      this.flowLayoutPanel1.AutoSize = true;
      this.flowLayoutPanel1.Controls.Add((Control) this.lblHealthClasses);
      this.flowLayoutPanel1.Dock = DockStyle.Top;
      this.flowLayoutPanel1.Location = new Point(0, 31);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(591, 31);
      this.flowLayoutPanel1.TabIndex = 4;
      this.lblHealthClasses.AutoSize = true;
      this.flowLayoutPanel1.SetFlowBreak((Control) this.lblHealthClasses, true);
      this.lblHealthClasses.Font = new Font("Calibri", 13f);
      this.lblHealthClasses.Location = new Point(3, 6);
      this.lblHealthClasses.Margin = new Padding(3, 6, 3, 3);
      this.lblHealthClasses.Name = "lblHealthClasses";
      this.lblHealthClasses.Size = new Size(298, 22);
      this.lblHealthClasses.TabIndex = 2;
      this.lblHealthClasses.Text = "Health Classes are used for REPORTING";
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
      this.dcDescription.DataPropertyName = "Description";
      this.dcDescription.HeaderText = "Description";
      this.dcDescription.MaxInputLength = 30;
      this.dcDescription.Name = "dcDescription";
      this.dcRangeEnd.DataPropertyName = "Extent";
      this.dcRangeEnd.DecimalPlaces = 1;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle4.Format = "#0.#";
      gridViewCellStyle4.NullValue = (object) null;
      this.dcRangeEnd.DefaultCellStyle = gridViewCellStyle4;
      this.dcRangeEnd.Format = "#0.#;#0.#";
      this.dcRangeEnd.HasDecimal = true;
      this.dcRangeEnd.HeaderText = "Dieback <=";
      this.dcRangeEnd.Name = "dcRangeEnd";
      this.dcRangeEnd.Resizable = DataGridViewTriState.True;
      this.dcRangeEnd.Signed = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(591, 316);
      this.Controls.Add((Control) this.dgClasses);
      this.Controls.Add((Control) this.flowLayoutPanel1);
      this.Name = "HealthClassesForm";
      this.Text = "Health Classes";
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
