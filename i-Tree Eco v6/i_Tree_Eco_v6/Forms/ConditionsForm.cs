// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ConditionsForm
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
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ConditionsForm : ProjectContentForm, IActionable, IExportable
  {
    private ProgramSession m_ps;
    private DataGridViewManager m_dgManager;
    private ISession m_session;
    private Year m_year;
    private TaskManager m_taskManager;
    private WaitCursor m_wc;
    private readonly object m_syncobj;
    private IList<Condition> m_conditions;
    private IList<HealthRptClass> m_rptClasses;
    private IContainer components;
    private DataGridView dgConditions;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewTextBoxColumn dcDescription;
    private DataGridViewNumericTextBoxColumn dcPercent;
    private DataGridViewTextBoxColumn dcHealthClass;
    private DataGridViewTextBoxColumn dcCategory;

    public ConditionsForm()
    {
      this.m_ps = Program.Session;
      this.InitializeComponent();
      this.m_dgManager = new DataGridViewManager(this.dgConditions);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.m_syncobj = new object();
      this.dgConditions.DoubleBuffered(true);
      this.dgConditions.AutoGenerateColumns = false;
      this.dgConditions.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgConditions.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.dcCategory.Visible = false;
      this.dcHealthClass.Visible = false;
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
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t => this.InitGrid()), scheduler));
    }

    private void LoadData()
    {
      ISession session = this.m_ps.InputSession.CreateSession();
      using (session.BeginTransaction())
      {
        Year y = session.Get<Year>((object) this.m_ps.InputSession.YearKey);
        IList<Condition> conditionList = session.QueryOver<Condition>().Where((System.Linq.Expressions.Expression<Func<Condition, bool>>) (c => c.Year == y && c.Id > 0)).OrderBy((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => (object) c.Id)).Asc.List();
        IList<HealthRptClass> healthRptClassList = session.QueryOver<HealthRptClass>().Where((System.Linq.Expressions.Expression<Func<HealthRptClass, bool>>) (c => c.Year == y)).OrderBy((System.Linq.Expressions.Expression<Func<HealthRptClass, object>>) (c => (object) c.Extent)).Asc.List();
        lock (this.m_syncobj)
        {
          this.m_session = session;
          this.m_conditions = conditionList;
          this.m_rptClasses = healthRptClassList;
          this.m_year = y;
        }
      }
    }

    private void InitGrid()
    {
      DataBindingList<Condition> dataBindingList = new DataBindingList<Condition>(this.m_conditions);
      this.dgConditions.DataSource = (object) dataBindingList;
      this.dgConditions.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.Conditions_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.Conditions_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.Conditions_ListChanged);
    }

    protected override void OnRequestRefresh()
    {
      Year year = this.m_year;
      bool flag1 = year != null && year.ConfigEnabled && this.dgConditions.DataSource != null;
      bool flag2 = flag1 && this.m_conditions.Count > 0;
      this.dgConditions.ReadOnly = !flag1;
      this.dgConditions.AllowUserToAddRows = flag1;
      this.dgConditions.AllowUserToDeleteRows = flag2;
      this.dcHealthClass.ReadOnly = true;
      this.dcCategory.ReadOnly = true;
      base.OnRequestRefresh();
    }

    private void GetYear(Guid g)
    {
      if (this.m_year == null || this.m_session == null || !(this.m_year.Guid == g))
        return;
      using (this.m_session.BeginTransaction())
        this.m_session.Refresh((object) this.m_year);
    }

    private void Conditions_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void Conditions_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<Condition> dataBindingList = sender as DataBindingList<Condition>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      Condition condition = dataBindingList[e.NewIndex];
      if (condition.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) condition);
          transaction.Commit();
        }
      }
    }

    private void Conditions_AddingNew(object sender, AddingNewEventArgs e)
    {
      DataBindingList<Condition> dataBindingList = sender as DataBindingList<Condition>;
      Condition condition1 = new Condition();
      int num = 1;
      foreach (Condition condition2 in (Collection<Condition>) dataBindingList)
      {
        if (condition2.Id >= num)
          num = condition2.Id + 1;
      }
      condition1.Year = this.m_year;
      condition1.Id = num;
      e.NewObject = (object) condition1;
    }

    private void dgConditions_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void dgConditions_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgConditions.NewRowIndex || !(this.dgConditions.DataSource is DataBindingList<Condition> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgConditions.Columns[e.ColumnIndex];
      Condition condition = dataSource[e.RowIndex];
      if (column == this.dcId)
      {
        if (condition.Id != 0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else
      {
        if (column != this.dcPercent || condition.Percent >= 0.0 && condition.Percent <= 100.0)
          return;
        DataGridViewCellErrorTextNeededEventArgs textNeededEventArgs = e;
        string errFieldRange = i_Tree_Eco_v6.Resources.Strings.ErrFieldRange;
        string headerText = column.HeaderText;
        int num = 0;
        string str1 = num.ToString("P0");
        num = 1;
        string str2 = num.ToString("P0");
        string str3 = string.Format(errFieldRange, (object) headerText, (object) str1, (object) str2);
        textNeededEventArgs.ErrorText = str3;
      }
    }

    private void dgConditions_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<Condition> dataSource = this.dgConditions.DataSource as DataBindingList<Condition>;
      if (this.dgConditions.CurrentRow != null && !this.dgConditions.IsCurrentRowDirty)
        return;
      string text = (string) null;
      if (dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      Condition condition1 = dataSource[e.RowIndex];
      DataGridViewRow row = this.dgConditions.Rows[e.RowIndex];
      DataGridViewCell dataGridViewCell1 = (DataGridViewCell) null;
      foreach (Condition condition2 in (Collection<Condition>) dataSource)
      {
        if (condition2 != condition1 && condition2.Id == condition1.Id)
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

    private void dgConditions_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgConditions.ReadOnly)
        return;
      DataBindingList<Condition> dataSource = this.dgConditions.DataSource as DataBindingList<Condition>;
      if (this.dgConditions.CurrentRow != null && this.dgConditions.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          Condition condition = dataSource[e.RowIndex];
          if (condition.IsTransient || this.m_session.IsDirty())
          {
            lock (this.m_syncobj)
            {
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                this.m_session.SaveOrUpdate((object) condition);
                transaction.Commit();
              }
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgConditions_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void DeleteSelectedRows()
    {
      if (!(this.dgConditions.DataSource is DataBindingList<Condition> dataSource))
        return;
      CurrencyManager currencyManager = this.dgConditions.BindingContext[(object) dataSource] as CurrencyManager;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgConditions.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          if (this.CanDeleteCondition(dataSource[selectedRow.Index]))
          {
            dataSource.RemoveAt(selectedRow.Index);
          }
          else
          {
            int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDelete, (object) v6Strings.Condition_SingularName, (object) v6Strings.Tree_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
      if (currencyManager.Position == -1)
        return;
      this.dgConditions.Rows[currencyManager.Position].Selected = true;
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgConditions.DataSource != null;
      bool flag2 = flag1 && this.dgConditions.AllowUserToAddRows;
      bool flag3 = this.dgConditions.SelectedRows.Count > 0;
      bool flag4 = this.dgConditions.CurrentRow != null && this.dgConditions.CurrentRow.IsNewRow;
      bool flag5 = this.dgConditions.CurrentRow != null && this.dgConditions.IsCurrentRowDirty;
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
          if (!this.dgConditions.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgConditions.SelectedRows)
            selectedRow.Selected = false;
          this.dgConditions.Rows[this.dgConditions.NewRowIndex].Selected = true;
          this.dgConditions.FirstDisplayedScrollingRowIndex = this.dgConditions.NewRowIndex - this.dgConditions.DisplayedRowCount(false) + 1;
          this.dgConditions.CurrentCell = this.dgConditions.Rows[this.dgConditions.NewRowIndex].Cells[0];
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          if (this.dgConditions.SelectedRows.Count <= 0)
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
        this.dgConditions.DataSource = (object) null;
        YearHelper.RestoreDefaultConditions(this.m_year, this.m_session);
      }
      catch (HibernateException ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrRestore, (object) v6Strings.Condition_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      this.Init();
    }

    public void Export(string file) => this.dgConditions.ExportToCSV(file);

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgConditions.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgConditions.DataSource != null;

    private void dgConditions_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (this.CanDeleteCondition(e.Row.DataBoundItem as Condition))
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDelete, (object) v6Strings.Condition_SingularName, (object) v6Strings.Tree_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private bool CanDeleteCondition(Condition c)
    {
      if (c.IsTransient)
        return true;
      using (this.m_session.BeginTransaction())
        return this.m_session.CreateCriteria<Tree>().SetProjection((IProjection) Projections.ProjectionList().Add(Projections.RowCount())).Add((ICriterion) Restrictions.Eq("Crown.Condition", (object) c)).UniqueResult<int>() == 0;
    }

    private void dgConditions_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgConditions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dgConditions.Columns[e.ColumnIndex] != this.dcHealthClass || this.m_rptClasses == null || e.Value == null)
        return;
      double num1 = (double) e.Value;
      foreach (HealthRptClass rptClass in (IEnumerable<HealthRptClass>) this.m_rptClasses)
      {
        double num2 = 100.0 - rptClass.Extent;
        if (num1 >= num2)
        {
          e.Value = (object) rptClass.Description;
          break;
        }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConditionsForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      this.dgConditions = new DataGridView();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcDescription = new DataGridViewTextBoxColumn();
      this.dcPercent = new DataGridViewNumericTextBoxColumn();
      this.dcHealthClass = new DataGridViewTextBoxColumn();
      this.dcCategory = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dgConditions).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgConditions.AllowUserToResizeRows = false;
      this.dgConditions.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgConditions.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgConditions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgConditions.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcDescription, (DataGridViewColumn) this.dcPercent, (DataGridViewColumn) this.dcHealthClass, (DataGridViewColumn) this.dcCategory);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgConditions.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgConditions, "dgConditions");
      this.dgConditions.EnableHeadersVisualStyles = false;
      this.dgConditions.MultiSelect = false;
      this.dgConditions.Name = "dgConditions";
      this.dgConditions.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgConditions.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgConditions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgConditions.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgConditions_CellErrorTextNeeded);
      this.dgConditions.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgConditions_CellFormatting);
      this.dgConditions.CurrentCellDirtyStateChanged += new EventHandler(this.dgConditions_CurrentCellDirtyStateChanged);
      this.dgConditions.DataError += new DataGridViewDataErrorEventHandler(this.dgConditions_DataError);
      this.dgConditions.RowValidated += new DataGridViewCellEventHandler(this.dgConditions_RowValidated);
      this.dgConditions.RowValidating += new DataGridViewCellCancelEventHandler(this.dgConditions_RowValidating);
      this.dgConditions.SelectionChanged += new EventHandler(this.dgConditions_SelectionChanged);
      this.dgConditions.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.dgConditions_UserDeletingRow);
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
      this.dcPercent.DataPropertyName = "Percent";
      this.dcPercent.DecimalPlaces = 0;
      this.dcPercent.Format = "0.#;0.#";
      this.dcPercent.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcPercent, "dcPercent");
      this.dcPercent.Name = "dcPercent";
      this.dcPercent.Signed = false;
      this.dcHealthClass.DataPropertyName = "Percent";
      gridViewCellStyle3.BackColor = Color.LightGray;
      gridViewCellStyle3.ForeColor = Color.Black;
      this.dcHealthClass.DefaultCellStyle = gridViewCellStyle3;
      componentResourceManager.ApplyResources((object) this.dcHealthClass, "dcHealthClass");
      this.dcHealthClass.Name = "dcHealthClass";
      this.dcHealthClass.ReadOnly = true;
      this.dcCategory.DataPropertyName = "Category";
      gridViewCellStyle4.BackColor = Color.LightGray;
      gridViewCellStyle4.ForeColor = Color.Black;
      this.dcCategory.DefaultCellStyle = gridViewCellStyle4;
      componentResourceManager.ApplyResources((object) this.dcCategory, "dcCategory");
      this.dcCategory.Name = "dcCategory";
      this.dcCategory.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.dgConditions);
      this.Name = nameof (ConditionsForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgConditions, 0);
      ((ISupportInitialize) this.dgConditions).EndInit();
      this.ResumeLayout(false);
    }
  }
}
