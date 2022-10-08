// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.GroundCoversForm
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
using LocationSpecies.Domain;
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
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class GroundCoversForm : ProjectContentForm, IExportable, IActionable
  {
    private ProgramSession m_ps;
    private DataGridViewManager m_dgManager;
    private WaitCursor m_wc;
    private TaskManager m_taskManager;
    private readonly object m_syncobj;
    private Year m_year;
    private ISession m_session;
    private IList<GroundCover> m_groundCovers;
    private IList<CoverType> m_coverTypes;
    private IContainer components;
    private DataGridView dgGroundCovers;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewTextBoxColumn dcDescription;
    private DataGridViewComboBoxColumn dcCategory;

    public GroundCoversForm()
    {
      this.m_ps = Program.Session;
      this.InitializeComponent();
      this.m_dgManager = new DataGridViewManager(this.dgGroundCovers);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.m_syncobj = new object();
      this.dgGroundCovers.DoubleBuffered(true);
      this.dgGroundCovers.AutoGenerateColumns = false;
      this.dgGroundCovers.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgGroundCovers.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.dcCategory.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.m_ps.InputSession.YearChanged += new EventHandler(this.InputSession_YearChanged);
      this.Init();
      EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));
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

    private void GetYear(Guid g)
    {
      if (this.m_session == null || this.m_year == null || !(this.m_year.Guid == g))
        return;
      this.m_session.Refresh((object) this.m_year);
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
      ISession session1 = this.m_ps.InputSession.CreateSession();
      using (session1.BeginTransaction())
      {
        Year y = session1.Get<Year>((object) this.m_ps.InputSession.YearKey);
        IList<GroundCover> groundCoverList = session1.QueryOver<GroundCover>().Where((System.Linq.Expressions.Expression<Func<GroundCover, bool>>) (gc => gc.Year == y)).OrderBy((System.Linq.Expressions.Expression<Func<GroundCover, object>>) (gc => (object) gc.Id)).Asc.List();
        lock (this.m_syncobj)
        {
          this.m_session = session1;
          this.m_groundCovers = groundCoverList;
          this.m_year = y;
        }
      }
      IList<CoverType> coverTypeList1 = RetryExecutionHandler.Execute<IList<CoverType>>((Func<IList<CoverType>>) (() =>
      {
        using (ISession session2 = this.m_ps.LocSp.OpenSession())
        {
          using (ITransaction transaction = session2.BeginTransaction())
          {
            IList<CoverType> coverTypeList2 = session2.QueryOver<CoverType>().OrderBy((System.Linq.Expressions.Expression<Func<CoverType, object>>) (ct => ct.Description)).Asc.Cacheable().List();
            transaction.Commit();
            return coverTypeList2;
          }
        }
      }));
      lock (this.m_syncobj)
        this.m_coverTypes = coverTypeList1;
    }

    private void InitGrid()
    {
      this.dcCategory.BindTo<CoverType>((System.Linq.Expressions.Expression<Func<CoverType, object>>) (ct => ct.Description), (System.Linq.Expressions.Expression<Func<CoverType, object>>) (ct => (object) ct.Id), (object) this.m_coverTypes);
      DataBindingList<GroundCover> dataBindingList = new DataBindingList<GroundCover>(this.m_groundCovers);
      this.dgGroundCovers.DataSource = (object) dataBindingList;
      this.dgGroundCovers.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.GroundCovers_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.GroundCovers_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.GroundCovers_ListChanged);
    }

    protected override void OnRequestRefresh()
    {
      bool flag = this.m_year != null && this.m_year.Changed && this.dgGroundCovers.DataSource != null;
      this.dgGroundCovers.ReadOnly = !flag;
      this.dgGroundCovers.AllowUserToAddRows = flag;
      this.dgGroundCovers.AllowUserToDeleteRows = flag;
      base.OnRequestRefresh();
    }

    private void GroundCovers_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void GroundCovers_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<GroundCover> dataBindingList = sender as DataBindingList<GroundCover>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      GroundCover groundCover = dataBindingList[e.NewIndex];
      if (groundCover.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) groundCover);
          transaction.Commit();
        }
      }
    }

    private void GroundCovers_AddingNew(object sender, AddingNewEventArgs e)
    {
      DataBindingList<GroundCover> dataBindingList = sender as DataBindingList<GroundCover>;
      int num = 1;
      foreach (GroundCover groundCover in (Collection<GroundCover>) dataBindingList)
      {
        if (groundCover.Id >= num)
          num = groundCover.Id + 1;
      }
      e.NewObject = (object) new GroundCover()
      {
        Id = num,
        Year = this.m_year
      };
    }

    private void dgGroundCovers_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void dgGroundCovers_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.dgGroundCovers.CurrentRow != null && !this.dgGroundCovers.IsCurrentRowDirty || !(this.dgGroundCovers.DataSource is DataBindingList<GroundCover> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      GroundCover groundCover1 = dataSource[e.RowIndex];
      DataGridViewRow row = this.dgGroundCovers.Rows[e.RowIndex];
      DataGridViewCell dataGridViewCell1 = (DataGridViewCell) null;
      string text = (string) null;
      foreach (GroundCover groundCover2 in (Collection<GroundCover>) dataSource)
      {
        if (groundCover2 != groundCover1)
        {
          if (groundCover2.Id == groundCover1.Id)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcId.HeaderText);
            dataGridViewCell1 = row.Cells[this.dcId.DisplayIndex];
            break;
          }
          if (string.Compare(groundCover1.Description, groundCover2.Description, true) == 0)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcDescription.HeaderText);
            dataGridViewCell1 = row.Cells[this.dcDescription.DisplayIndex];
            break;
          }
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

    private void dgGroundCovers_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgGroundCovers.ReadOnly)
        return;
      DataBindingList<GroundCover> dataSource = this.dgGroundCovers.DataSource as DataBindingList<GroundCover>;
      if (this.dgGroundCovers.CurrentRow != null && this.dgGroundCovers.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          GroundCover groundCover = dataSource[e.RowIndex];
          if (groundCover.IsTransient || this.m_session.IsDirty())
          {
            lock (this.m_syncobj)
            {
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                this.m_session.SaveOrUpdate((object) groundCover);
                transaction.Commit();
              }
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgGroundCovers_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void DeleteSelectedRows()
    {
      if (!(this.dgGroundCovers.DataSource is DataBindingList<GroundCover> dataSource))
        return;
      CurrencyManager currencyManager = this.dgGroundCovers.BindingContext[(object) dataSource] as CurrencyManager;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgGroundCovers.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          if (this.CanDeleteGroundCover(dataSource[selectedRow.Index]))
          {
            dataSource.RemoveAt(selectedRow.Index);
          }
          else
          {
            int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDelete, (object) v6Strings.GroundCover_SingularName, (object) v6Strings.Plot_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
      if (currencyManager.Position == -1)
        return;
      this.dgGroundCovers.Rows[currencyManager.Position].Selected = true;
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgGroundCovers.DataSource != null;
      bool flag2 = flag1 && this.dgGroundCovers.AllowUserToAddRows;
      bool flag3 = this.dgGroundCovers.SelectedRows.Count > 0;
      bool flag4 = this.dgGroundCovers.CurrentRow != null && this.dgGroundCovers.CurrentRow.IsNewRow;
      bool flag5 = this.dgGroundCovers.CurrentRow != null && this.dgGroundCovers.IsCurrentRowDirty;
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
          if (!this.dgGroundCovers.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgGroundCovers.SelectedRows)
            selectedRow.Selected = false;
          this.dgGroundCovers.Rows[this.dgGroundCovers.NewRowIndex].Selected = true;
          this.dgGroundCovers.FirstDisplayedScrollingRowIndex = this.dgGroundCovers.NewRowIndex - this.dgGroundCovers.DisplayedRowCount(false) + 1;
          this.dgGroundCovers.CurrentCell = this.dgGroundCovers.Rows[this.dgGroundCovers.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          if (this.dgGroundCovers.SelectedRows.Count <= 0)
            break;
          DataGridViewRow selectedRow1 = this.dgGroundCovers.SelectedRows[0];
          if (!(this.dgGroundCovers.DataSource is DataBindingList<GroundCover> dataSource) || selectedRow1.Index >= dataSource.Count)
            break;
          GroundCover groundCover1 = dataSource[selectedRow1.Index].Clone() as GroundCover;
          int num = 1;
          foreach (GroundCover groundCover2 in (Collection<GroundCover>) dataSource)
          {
            if (groundCover2.Id > num)
              num = groundCover2.Id + 1;
          }
          groundCover1.Id = num;
          dataSource.Add(groundCover1);
          this.dgGroundCovers.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          if (this.dgGroundCovers.SelectedRows.Count <= 0)
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
        this.dgGroundCovers.DataSource = (object) null;
        YearHelper.RestoreDefaultGroundCovers(this.m_year, this.m_session, this.m_ps.LocSp);
      }
      catch (HibernateException ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrRestore, (object) v6Strings.GroundCover_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      this.Init();
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgGroundCovers.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgGroundCovers.DataSource != null;

    private bool CanDeleteGroundCover(GroundCover gc)
    {
      if (gc.IsTransient)
        return true;
      using (this.m_session.BeginTransaction())
        return this.m_session.CreateCriteria<PlotGroundCover>().SetProjection((IProjection) Projections.ProjectionList().Add(Projections.RowCount())).Add((ICriterion) Restrictions.Eq("GroundCover", (object) gc)).UniqueResult<int>() == 0;
    }

    private void dsGroundCovers_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (this.CanDeleteGroundCover(e.Row.DataBoundItem as GroundCover))
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDelete, (object) v6Strings.GroundCover_SingularName, (object) v6Strings.Plot_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgGroundCovers_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgGroundCovers_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgGroundCovers.NewRowIndex || !(this.dgGroundCovers.DataSource is DataBindingList<GroundCover> dataSource))
        return;
      DataGridViewColumn column = this.dgGroundCovers.Columns[e.ColumnIndex];
      GroundCover groundCover = dataSource[e.RowIndex];
      if (column == this.dcId)
      {
        if (groundCover.Id != 0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcDescription)
      {
        if (!string.IsNullOrEmpty(groundCover.Description))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else
      {
        if (column != this.dcCategory || groundCover.CoverTypeId != 0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (GroundCoversForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      this.dgGroundCovers = new DataGridView();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcDescription = new DataGridViewTextBoxColumn();
      this.dcCategory = new DataGridViewComboBoxColumn();
      ((ISupportInitialize) this.dgGroundCovers).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgGroundCovers.AllowUserToAddRows = false;
      this.dgGroundCovers.AllowUserToDeleteRows = false;
      this.dgGroundCovers.AllowUserToResizeRows = false;
      this.dgGroundCovers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgGroundCovers.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgGroundCovers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgGroundCovers.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcDescription, (DataGridViewColumn) this.dcCategory);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgGroundCovers.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgGroundCovers, "dgGroundCovers");
      this.dgGroundCovers.EnableHeadersVisualStyles = false;
      this.dgGroundCovers.MultiSelect = false;
      this.dgGroundCovers.Name = "dgGroundCovers";
      this.dgGroundCovers.ReadOnly = true;
      this.dgGroundCovers.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgGroundCovers.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      this.dgGroundCovers.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgGroundCovers.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgGroundCovers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgGroundCovers.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgGroundCovers_CellErrorTextNeeded);
      this.dgGroundCovers.CurrentCellDirtyStateChanged += new EventHandler(this.dgGroundCovers_CurrentCellDirtyStateChanged);
      this.dgGroundCovers.DataError += new DataGridViewDataErrorEventHandler(this.dgGroundCovers_DataError);
      this.dgGroundCovers.RowValidated += new DataGridViewCellEventHandler(this.dgGroundCovers_RowValidated);
      this.dgGroundCovers.RowValidating += new DataGridViewCellCancelEventHandler(this.dgGroundCovers_RowValidating);
      this.dgGroundCovers.SelectionChanged += new EventHandler(this.dgGroundCovers_SelectionChanged);
      this.dgGroundCovers.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.dsGroundCovers_UserDeletingRow);
      this.dcId.DataPropertyName = "Id";
      this.dcId.DecimalPlaces = 0;
      this.dcId.Format = "#;#";
      this.dcId.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcId, "dcId");
      this.dcId.Name = "dcId";
      this.dcId.ReadOnly = true;
      this.dcId.Resizable = DataGridViewTriState.True;
      this.dcId.Signed = false;
      this.dcDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDescription.DataPropertyName = "Description";
      componentResourceManager.ApplyResources((object) this.dcDescription, "dcDescription");
      this.dcDescription.MaxInputLength = 30;
      this.dcDescription.Name = "dcDescription";
      this.dcDescription.ReadOnly = true;
      this.dcCategory.DataPropertyName = "CoverTypeId";
      this.dcCategory.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcCategory, "dcCategory");
      this.dcCategory.Name = "dcCategory";
      this.dcCategory.ReadOnly = true;
      this.dcCategory.SortMode = DataGridViewColumnSortMode.Automatic;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.dgGroundCovers);
      this.DockAreas = DockAreas.Document;
      this.Name = nameof (GroundCoversForm);
      this.ShowHint = DockState.Document;
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgGroundCovers, 0);
      ((ISupportInitialize) this.dgGroundCovers).EndInit();
      this.ResumeLayout(false);
    }
  }
}
