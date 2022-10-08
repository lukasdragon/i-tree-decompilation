// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.LandUsesForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

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
  public class LandUsesForm : ProjectContentForm, IExportable, IActionable
  {
    private ProgramSession m_ps;
    private DataGridViewManager m_dgManager;
    private WaitCursor m_wc;
    private TaskManager m_taskManager;
    private readonly object m_syncobj;
    private ISession m_session;
    private Year m_year;
    private IList<LandUse> m_landuses;
    private IList<FieldLandUse> m_luTypes;
    private IContainer components;
    private DataGridView dgLandUse;
    private DataGridViewTextBoxColumn dcCode;
    private DataGridViewTextBoxColumn dcDescription;
    private DataGridViewComboBoxColumn dcCategory;

    public LandUsesForm()
    {
      this.m_ps = Program.Session;
      this.InitializeComponent();
      this.m_dgManager = new DataGridViewManager(this.dgLandUse);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.m_syncobj = new object();
      this.dgLandUse.DoubleBuffered(true);
      this.dgLandUse.AutoGenerateColumns = false;
      this.dgLandUse.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgLandUse.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.m_ps.InputSession.YearChanged += new EventHandler(this.InputSession_YearChanged);
      EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));
      this.dcCategory.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.Init();
    }

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
        IList<LandUse> landUseList = session1.QueryOver<LandUse>().Where((System.Linq.Expressions.Expression<Func<LandUse, bool>>) (lu => lu.Year == y)).OrderBy((System.Linq.Expressions.Expression<Func<LandUse, object>>) (lu => (object) lu.Id)).Asc.List();
        lock (this.m_syncobj)
        {
          this.m_session = session1;
          this.m_landuses = landUseList;
          this.m_year = y;
        }
      }
      IList<FieldLandUse> fieldLandUseList1 = RetryExecutionHandler.Execute<IList<FieldLandUse>>((Func<IList<FieldLandUse>>) (() =>
      {
        using (ISession session2 = this.m_ps.LocSp.OpenSession())
        {
          using (ITransaction transaction = session2.BeginTransaction())
          {
            IList<FieldLandUse> fieldLandUseList2 = session2.QueryOver<FieldLandUse>().OrderBy((System.Linq.Expressions.Expression<Func<FieldLandUse, object>>) (lu => lu.Description)).Asc.Cacheable().List();
            transaction.Commit();
            return fieldLandUseList2;
          }
        }
      }));
      lock (this.m_syncobj)
        this.m_luTypes = fieldLandUseList1;
    }

    private void InitGrid()
    {
      this.dcCategory.BindTo<FieldLandUse>((System.Linq.Expressions.Expression<Func<FieldLandUse, object>>) (t => t.Description), (System.Linq.Expressions.Expression<Func<FieldLandUse, object>>) (t => (object) t.Id), (object) this.m_luTypes);
      DataBindingList<LandUse> dataBindingList = new DataBindingList<LandUse>(this.m_landuses);
      this.dgLandUse.DataSource = (object) dataBindingList;
      this.dgLandUse.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.LandUse_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.LandUse_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.LandUse_ListChanged);
    }

    private void GetYear(Guid g)
    {
      if (this.m_session == null)
        return;
      this.m_session.Refresh((object) this.m_year);
    }

    protected override void OnRequestRefresh()
    {
      bool flag = this.m_year != null && this.m_year.ConfigEnabled && this.dgLandUse.DataSource != null;
      this.dgLandUse.ReadOnly = !flag;
      this.dgLandUse.AllowUserToAddRows = flag;
      this.dgLandUse.AllowUserToDeleteRows = flag;
      base.OnRequestRefresh();
    }

    private void LandUse_AddingNew(object sender, AddingNewEventArgs e)
    {
      LandUse landUse = new LandUse()
      {
        Year = this.m_year
      };
      e.NewObject = (object) landUse;
    }

    private void LandUse_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void LandUse_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<LandUse> dataBindingList = sender as DataBindingList<LandUse>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      LandUse landUse = dataBindingList[e.NewIndex];
      if (landUse.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) landUse);
          transaction.Commit();
        }
      }
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

    private void InputSession_YearChanged(object sender, EventArgs e) => this.Init();

    private void dgLandUse_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void dgLandUse_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<LandUse> dataSource = this.dgLandUse.DataSource as DataBindingList<LandUse>;
      if (this.dgLandUse.CurrentRow != null && !this.dgLandUse.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      LandUse landUse1 = dataSource[e.RowIndex];
      DataGridViewRow row = this.dgLandUse.Rows[e.RowIndex];
      DataGridViewCell dataGridViewCell1 = (DataGridViewCell) null;
      string text = (string) null;
      foreach (LandUse landUse2 in (Collection<LandUse>) dataSource)
      {
        if (landUse2 != landUse1)
        {
          if ((int) char.ToUpperInvariant(landUse2.Id) == (int) char.ToUpperInvariant(landUse1.Id))
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcCode.HeaderText);
            dataGridViewCell1 = row.Cells[this.dcCode.DisplayIndex];
            break;
          }
          if (string.Compare(landUse1.Description, landUse2.Description, true) == 0)
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

    private void dgLandUse_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgLandUse.ReadOnly)
        return;
      DataBindingList<LandUse> dataSource = this.dgLandUse.DataSource as DataBindingList<LandUse>;
      if (this.dgLandUse.CurrentRow != null && this.dgLandUse.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          LandUse landUse = dataSource[e.RowIndex];
          if (landUse.IsTransient || this.m_session.IsDirty())
          {
            lock (this.m_syncobj)
            {
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                this.m_session.SaveOrUpdate((object) landUse);
                transaction.Commit();
              }
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgLandUse_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void DeleteSelectedRows()
    {
      if (!(this.dgLandUse.DataSource is DataBindingList<LandUse> dataSource))
        return;
      CurrencyManager currencyManager = this.dgLandUse.BindingContext[(object) dataSource] as CurrencyManager;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgLandUse.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          if (this.CanDeleteLandUse(dataSource[selectedRow.Index]))
          {
            dataSource.RemoveAt(selectedRow.Index);
          }
          else
          {
            int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDelete, (object) v6Strings.LandUse_SingularName, this.m_year.Series.IsSample ? (object) v6Strings.Plot_PluralName : (object) v6Strings.Tree_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
      if (currencyManager.Position == -1)
        return;
      this.dgLandUse.Rows[currencyManager.Position].Selected = true;
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgLandUse.DataSource != null;
      bool flag2 = flag1 && this.dgLandUse.AllowUserToAddRows;
      bool flag3 = this.dgLandUse.SelectedRows.Count > 0;
      bool flag4 = this.dgLandUse.CurrentRow != null && this.dgLandUse.CurrentRow.IsNewRow;
      bool flag5 = this.dgLandUse.CurrentRow != null && this.dgLandUse.IsCurrentRowDirty;
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
          if (!this.dgLandUse.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgLandUse.SelectedRows)
            selectedRow.Selected = false;
          this.dgLandUse.Rows[this.dgLandUse.NewRowIndex].Selected = true;
          this.dgLandUse.FirstDisplayedScrollingRowIndex = this.dgLandUse.NewRowIndex - this.dgLandUse.DisplayedRowCount(false) + 1;
          this.dgLandUse.CurrentCell = this.dgLandUse.Rows[this.dgLandUse.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          if (this.dgLandUse.SelectedRows.Count <= 0)
            break;
          DataGridViewRow selectedRow1 = this.dgLandUse.SelectedRows[0];
          DataBindingList<LandUse> dataSource = this.dgLandUse.DataSource as DataBindingList<LandUse>;
          if (selectedRow1.Index >= dataSource.Count)
            break;
          LandUse landUse = dataSource[selectedRow1.Index].Clone() as LandUse;
          dataSource.Add(landUse);
          this.dgLandUse.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          if (this.dgLandUse.SelectedRows.Count <= 0)
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
        this.dgLandUse.DataSource = (object) null;
        YearHelper.RestoreDefaultLandUses(this.m_year, this.m_session, this.m_ps.LocSp);
      }
      catch (HibernateException ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrRestore, (object) v6Strings.LandUse_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      this.Init();
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgLandUse.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgLandUse.DataSource != null;

    private bool CanDeleteLandUse(LandUse lu)
    {
      if (lu.IsTransient)
        return true;
      using (this.m_session.BeginTransaction())
        return this.m_session.CreateCriteria<PlotLandUse>().SetProjection((IProjection) Projections.ProjectionList().Add(Projections.RowCount())).Add((ICriterion) Restrictions.Eq("LandUse", (object) lu)).UniqueResult<int>() == 0;
    }

    private void dgLandUse_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (this.CanDeleteLandUse(e.Row.DataBoundItem as LandUse))
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDelete, (object) v6Strings.LandUse_SingularName, this.m_year.Series.IsSample ? (object) v6Strings.Plot_PluralName : (object) v6Strings.Tree_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgLandUse_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgLandUse_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgLandUse.NewRowIndex || !(this.dgLandUse.DataSource is DataBindingList<LandUse> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgLandUse.Columns[e.ColumnIndex];
      LandUse landUse = dataSource[e.RowIndex];
      if (column == this.dcCode)
      {
        if (landUse.Id != char.MinValue)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcDescription)
      {
        if (!string.IsNullOrEmpty(landUse.Description))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else
      {
        if (column != this.dcCategory || landUse.LandUseId != 0)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LandUsesForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      this.dgLandUse = new DataGridView();
      this.dcCode = new DataGridViewTextBoxColumn();
      this.dcDescription = new DataGridViewTextBoxColumn();
      this.dcCategory = new DataGridViewComboBoxColumn();
      ((ISupportInitialize) this.dgLandUse).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgLandUse.AllowUserToAddRows = false;
      this.dgLandUse.AllowUserToResizeRows = false;
      this.dgLandUse.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgLandUse.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgLandUse.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgLandUse.Columns.AddRange((DataGridViewColumn) this.dcCode, (DataGridViewColumn) this.dcDescription, (DataGridViewColumn) this.dcCategory);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgLandUse.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgLandUse, "dgLandUse");
      this.dgLandUse.EnableHeadersVisualStyles = false;
      this.dgLandUse.MultiSelect = false;
      this.dgLandUse.Name = "dgLandUse";
      this.dgLandUse.ReadOnly = true;
      this.dgLandUse.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgLandUse.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      this.dgLandUse.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgLandUse.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgLandUse.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgLandUse.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgLandUse_CellErrorTextNeeded);
      this.dgLandUse.CurrentCellDirtyStateChanged += new EventHandler(this.dgLandUse_CurrentCellDirtyStateChanged);
      this.dgLandUse.DataError += new DataGridViewDataErrorEventHandler(this.dgLandUse_DataError);
      this.dgLandUse.RowValidated += new DataGridViewCellEventHandler(this.dgLandUse_RowValidated);
      this.dgLandUse.RowValidating += new DataGridViewCellCancelEventHandler(this.dgLandUse_RowValidating);
      this.dgLandUse.SelectionChanged += new EventHandler(this.dgLandUse_SelectionChanged);
      this.dgLandUse.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.dgLandUse_UserDeletingRow);
      this.dcCode.DataPropertyName = "Id";
      componentResourceManager.ApplyResources((object) this.dcCode, "dcCode");
      this.dcCode.MaxInputLength = 1;
      this.dcCode.Name = "dcCode";
      this.dcCode.ReadOnly = true;
      this.dcCode.Resizable = DataGridViewTriState.True;
      this.dcDescription.DataPropertyName = "Description";
      componentResourceManager.ApplyResources((object) this.dcDescription, "dcDescription");
      this.dcDescription.MaxInputLength = 50;
      this.dcDescription.Name = "dcDescription";
      this.dcDescription.ReadOnly = true;
      this.dcCategory.DataPropertyName = "LandUseId";
      this.dcCategory.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcCategory, "dcCategory");
      this.dcCategory.Name = "dcCategory";
      this.dcCategory.ReadOnly = true;
      this.dcCategory.SortMode = DataGridViewColumnSortMode.Automatic;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.dgLandUse);
      this.DockAreas = DockAreas.Document;
      this.Name = nameof (LandUsesForm);
      this.ShowHint = DockState.Document;
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgLandUse, 0);
      ((ISupportInitialize) this.dgLandUse).EndInit();
      this.ResumeLayout(false);
    }
  }
}
