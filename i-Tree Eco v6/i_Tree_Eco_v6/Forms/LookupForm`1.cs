// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.LookupForm`1
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class LookupForm<T> : LookupBaseForm, IActionable, IExportable
    where T : Eco.Domain.v6.Lookup<T>
  {
    private ProgramSession m_ps;
    private DataGridViewManager m_dgManager;
    private WaitCursor m_wc;
    private TaskManager m_taskManager;
    private readonly object m_syncobj;
    private ISession m_session;
    private Year m_year;
    private IList<T> m_lookups;
    private IContainer components;

    public LookupForm(string singularName)
      : this(singularName, (string) null, (string) null)
    {
    }

    public LookupForm(string singularName, string pluralName)
      : this(singularName, pluralName, (string) null)
    {
    }

    public LookupForm(string singularName, string pluralName, string helpTopic)
      : base(singularName, pluralName, helpTopic)
    {
      this.m_ps = Program.Session;
      this.InitializeComponent();
      this.m_dgManager = new DataGridViewManager(this.dgLookup);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.m_syncobj = new object();
      this.dgLookup.AutoGenerateColumns = false;
      this.dgLookup.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgLookup.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgLookup.CurrentCellDirtyStateChanged += new EventHandler(this.dgLookup_CurrentCellDirtyStateChanged);
      this.dgLookup.RowValidating += new DataGridViewCellCancelEventHandler(this.dgLookup_RowValidating);
      this.dgLookup.RowValidated += new DataGridViewCellEventHandler(this.dgLookup_RowValidated);
      this.dgLookup.SelectionChanged += new EventHandler(this.dgLookup_SelectionChanged);
      this.dgLookup.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.dgLookup_UserDeletingRow);
      this.dgLookup.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgLookup_CellErrorTextNeeded);
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
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
        IList<T> objList = session.QueryOver<T>().Where((Expression<Func<T, bool>>) (lu => lu.Year == y)).OrderBy((Expression<Func<T, object>>) (lu => (object) lu.Id)).Asc.List();
        lock (this.m_syncobj)
        {
          this.m_session = session;
          this.m_year = y;
          this.m_lookups = objList;
        }
      }
    }

    private void InitGrid()
    {
      DataBindingList<T> dataBindingList = new DataBindingList<T>(this.m_lookups);
      this.dgLookup.DataSource = (object) dataBindingList;
      this.dgLookup.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.Lookup_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.Lookup_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.Lookup_ListChanged);
    }

    protected override void OnRequestRefresh()
    {
      Year year = this.m_year;
      bool flag = year != null && year.ConfigEnabled && this.dgLookup.DataSource != null;
      this.dgLookup.ReadOnly = !flag;
      this.dgLookup.AllowUserToAddRows = flag;
      this.dgLookup.AllowUserToDeleteRows = flag;
      base.OnRequestRefresh();
    }

    private void GetYear(Guid g)
    {
      if (this.m_session == null || this.m_year == null || !(this.m_year.Guid == g))
        return;
      this.m_session.Refresh((object) this.m_year);
    }

    private void Lookup_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void Lookup_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<T> dataBindingList = sender as DataBindingList<T>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      T obj = dataBindingList[e.NewIndex];
      if (obj.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) obj);
          transaction.Commit();
        }
      }
    }

    private void Lookup_AddingNew(object sender, AddingNewEventArgs e)
    {
      DataBindingList<T> dataBindingList = sender as DataBindingList<T>;
      int num = 1;
      foreach (T obj in (Collection<T>) dataBindingList)
      {
        if (obj.Id >= num)
          num = obj.Id + 1;
      }
      T instance = Activator.CreateInstance<T>();
      instance.Id = num;
      instance.Year = this.m_year;
      e.NewObject = (object) instance;
    }

    private void dgLookup_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void dgLookup_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgLookup.NewRowIndex || !(this.dgLookup.DataSource is DataBindingList<T> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgLookup.Columns[e.ColumnIndex];
      T obj = dataSource[e.RowIndex];
      if (column == this.dcId)
      {
        if (obj.Id != 0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else
      {
        if (column != this.dcDescription || !string.IsNullOrWhiteSpace(obj.Description))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
    }

    private void dgLookup_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<T> dataSource = this.dgLookup.DataSource as DataBindingList<T>;
      if (this.dgLookup.CurrentRow != null && this.dgLookup.CurrentRow.IsNewRow && !this.dgLookup.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      T obj1 = dataSource[e.RowIndex];
      DataGridViewCell dataGridViewCell = this.dgLookup.Rows[e.RowIndex].ErrorCell();
      string text = (string) null;
      if (dataGridViewCell != null)
      {
        text = dataGridViewCell.ErrorText;
      }
      else
      {
        foreach (T obj2 in (Collection<T>) dataSource)
        {
          if ((object) obj2 != (object) obj1 && obj2.Id == obj1.Id)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcId.HeaderText);
            break;
          }
        }
      }
      if (text == null)
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgLookup_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgLookup.ReadOnly)
        return;
      DataBindingList<T> dataSource = this.dgLookup.DataSource as DataBindingList<T>;
      if (this.dgLookup.CurrentRow != null && this.dgLookup.CurrentRow.IsNewRow && !this.dgLookup.IsCurrentRowDirty)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          T obj = dataSource[e.RowIndex];
          if (obj.IsTransient || this.m_session.IsDirty())
          {
            lock (this.m_syncobj)
            {
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                this.m_session.SaveOrUpdate((object) obj);
                transaction.Commit();
              }
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgLookup_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void DeleteSelectedRows()
    {
      if (!(this.dgLookup.DataSource is DataBindingList<T> dataSource))
        return;
      CurrencyManager currencyManager = this.dgLookup.BindingContext[(object) dataSource] as CurrencyManager;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgLookup.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          if (this.CanDeleteLookup((Eco.Domain.v6.Lookup) dataSource[selectedRow.Index]))
          {
            dataSource.RemoveAt(selectedRow.Index);
          }
          else
          {
            int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDelete, (object) this.SingularName, (object) v6Strings.Tree_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
      if (currencyManager.Position == -1)
        return;
      this.dgLookup.Rows[currencyManager.Position].Selected = true;
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgLookup.DataSource != null;
      bool flag2 = flag1 && this.dgLookup.AllowUserToAddRows;
      bool flag3 = this.dgLookup.SelectedRows.Count > 0;
      bool flag4 = this.dgLookup.CurrentRow != null && this.dgLookup.CurrentRow.IsNewRow;
      bool flag5 = this.dgLookup.CurrentRow != null && this.dgLookup.IsCurrentRowDirty;
      bool flag6 = false;
      switch (action)
      {
        case UserActions.New:
          flag6 = flag2 && !flag4 && !flag5;
          break;
        case UserActions.Copy:
          flag6 = flag2 & flag3 && !flag4 && !flag5;
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
          Type type = typeof (T);
          if (type.Equals(typeof (MaintRec)) || type.Equals(typeof (Eco.Domain.v6.MaintTask)) || type.Equals(typeof (Sidewalk)) || type.Equals(typeof (Eco.Domain.v6.WireConflict)))
          {
            flag6 = flag1;
            break;
          }
          break;
      }
      return flag6;
    }

    public void PerformAction(UserActions action)
    {
      switch (action)
      {
        case UserActions.New:
          if (!this.dgLookup.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgLookup.SelectedRows)
            selectedRow.Selected = false;
          this.dgLookup.Rows[this.dgLookup.NewRowIndex].Selected = true;
          this.dgLookup.FirstDisplayedScrollingRowIndex = this.dgLookup.NewRowIndex - this.dgLookup.DisplayedRowCount(false) + 1;
          this.dgLookup.CurrentCell = this.dgLookup.Rows[this.dgLookup.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          if (this.dgLookup.SelectedRows.Count <= 0)
            break;
          DataGridViewRow selectedRow1 = this.dgLookup.SelectedRows[0];
          if (!(this.dgLookup.DataSource is DataBindingList<T> dataSource) || selectedRow1.Index >= dataSource.Count)
            break;
          T obj1 = dataSource[selectedRow1.Index].Clone() as T;
          int num1 = 1;
          foreach (T obj2 in (Collection<T>) dataSource)
          {
            if (obj2.Id >= num1)
              num1 = obj2.Id + 1;
          }
          obj1.Id = num1;
          dataSource.Add(obj1);
          this.dgLookup.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          if (this.dgLookup.SelectedRows.Count <= 0)
            break;
          this.DeleteSelectedRows();
          break;
        case UserActions.RestoreDefaults:
          Type type = typeof (T);
          Task task1 = (Task) null;
          if (type.Equals(typeof (MaintRec)))
            task1 = this.RestoreDefaults<MaintType>();
          else if (type.Equals(typeof (Eco.Domain.v6.MaintTask)))
            task1 = this.RestoreDefaults<LocationSpecies.Domain.MaintTask>();
          else if (type.Equals(typeof (Eco.Domain.v6.WireConflict)))
            task1 = this.RestoreDefaults<LocationSpecies.Domain.WireConflict>();
          else if (type.Equals(typeof (Sidewalk)))
            task1 = this.RestoreDefaults<SidewalkDamage>();
          if (task1 == null)
            break;
          TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
          task1.ContinueWith((System.Action<Task>) (task =>
          {
            if (task.IsFaulted)
            {
              int num2 = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrRestore, (object) this.PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            this.Init();
          }), scheduler);
          break;
      }
    }

    private Task RestoreDefaults<LSLU>() where LSLU : LocationSpecies.Domain.Lookup => Task.Factory.StartNew((System.Action) (() =>
    {
      DataBindingList<T> dataSource = this.dgLookup.DataSource as DataBindingList<T>;
      using (ISession session1 = this.m_ps.InputSession.CreateSession())
      {
        using (ITransaction transaction = session1.BeginTransaction())
        {
          foreach (T obj1 in (Collection<T>) dataSource)
          {
            if (!obj1.IsTransient)
            {
              T obj2 = session1.Load<T>((object) obj1.Guid);
              session1.Delete((object) obj2);
            }
          }
          try
          {
            transaction.Commit();
          }
          catch (HibernateException ex)
          {
            transaction.Rollback();
            throw;
          }
        }
        IList<LSLU> lsluList = RetryExecutionHandler.Execute<IList<LSLU>>((Func<IList<LSLU>>) (() =>
        {
          using (ISession session2 = this.m_ps.LocSp.OpenSession())
            return session2.CreateCriteria<LSLU>().SetCacheable(true).List<LSLU>();
        }));
        using (ITransaction transaction = session1.BeginTransaction())
        {
          foreach (LSLU lslu in (IEnumerable<LSLU>) lsluList)
          {
            T instance = Activator.CreateInstance<T>();
            instance.Id = lslu.Id;
            instance.Description = lslu.Description;
            instance.Year = this.m_year;
            session1.SaveOrUpdate((object) instance);
          }
          try
          {
            transaction.Commit();
          }
          catch (HibernateException ex)
          {
            transaction.Rollback();
            throw;
          }
        }
      }
    }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler);

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgLookup.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgLookup.DataSource != null;

    private bool CanDeleteLookup(Eco.Domain.v6.Lookup lu) => lu.IsTransient || lu.Trees.Count == 0;

    private void dgLookup_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (this.CanDeleteLookup(e.Row.DataBoundItem as Eco.Domain.v6.Lookup))
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDelete, (object) this.SingularName, (object) v6Strings.Tree_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.lblBreadcrumb.Size = new Size(488, 29);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(488, 319);
      this.DockAreas = DockAreas.Document;
      this.Name = nameof (LookupForm<T>);
      this.ShowHint = DockState.Document;
      this.Text = "Lookups";
      this.ResumeLayout(false);
    }
  }
}
