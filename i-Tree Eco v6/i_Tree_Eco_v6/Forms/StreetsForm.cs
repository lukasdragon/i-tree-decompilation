// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.StreetsForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls.Extensions;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class StreetsForm : ProjectContentForm, IActionable, IExportable
  {
    private ProgramSession m_ps;
    private DataGridViewManager m_dgManager;
    private WaitCursor m_wc;
    private TaskManager m_taskManager;
    private readonly object m_syncobj;
    private ProjectLocation m_location;
    private ISession m_session;
    private Year m_year;
    private IContainer components;
    private DataGridView dgStreets;
    private DataGridViewTextBoxColumn dcName;

    public StreetsForm()
    {
      this.m_ps = Program.Session;
      this.m_session = this.m_ps.InputSession.CreateSession();
      this.InitializeComponent();
      this.m_dgManager = new DataGridViewManager(this.dgStreets);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.m_syncobj = new object();
      this.dgStreets.DoubleBuffered(true);
      this.dgStreets.AutoGenerateColumns = false;
      this.dgStreets.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgStreets.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.m_ps.InputSession.YearChanged += new EventHandler(this.InputSession_YearChanged);
      EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));
      this.Init();
    }

    private void Year_Updated(object sender, EntityUpdated<Year> e)
    {
      if (!(this.m_year.Guid == e.Guid))
        return;
      TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(this.GetYear(e.Guid, taskScheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.OnRequestRefresh();
      }), taskScheduler));
    }

    private void InputSession_YearChanged(object sender, EventArgs e) => this.Init();

    private void Init()
    {
      if (this.m_ps.InputSession == null)
        return;
      Guid? yearKey = this.m_ps.InputSession.YearKey;
      if (!yearKey.HasValue)
        return;
      TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
      TaskManager taskManager = this.m_taskManager;
      Task[] taskArray = new Task[1];
      yearKey = this.m_ps.InputSession.YearKey;
      taskArray[0] = this.GetYear(yearKey.Value, taskScheduler);
      taskManager.Add(taskArray);
      this.m_taskManager.Add(Task.Factory.StartNew<ProjectLocation>((Func<ProjectLocation>) (() =>
      {
        using (this.m_session.BeginTransaction())
          return this.m_session.CreateCriteria<ProjectLocation>().CreateAlias("Project", "p").CreateAlias("p.Series", "s").CreateAlias("s.Years", "y").Add((ICriterion) Restrictions.Eq("y.Guid", (object) this.m_ps.InputSession.YearKey)).Fetch(SelectMode.Fetch, "Streets").UniqueResult<ProjectLocation>();
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<ProjectLocation>>) (task =>
      {
        ProjectLocation result = task.Result;
        if (result != null)
        {
          DataBindingList<Street> dataBindingList = new DataBindingList<Street>((IList<Street>) result.Streets.ToList<Street>());
          dataBindingList.Sortable = true;
          dataBindingList.AddingNew += new AddingNewEventHandler(this.Streets_AddingNew);
          dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.Streets_BeforeRemove);
          dataBindingList.ListChanged += new ListChangedEventHandler(this.Streets_ListChanged);
          this.dgStreets.DataSource = (object) null;
          lock (this.m_syncobj)
            this.m_location = result;
          this.dgStreets.DataSource = (object) dataBindingList;
          this.dgStreets.Enabled = true;
        }
        else
          this.dgStreets.Enabled = false;
        this.OnRequestRefresh();
      }), taskScheduler));
    }

    private Task GetYear(Guid g, TaskScheduler context) => Task.Factory.StartNew<Year>((Func<Year>) (() => this.m_session.Get<Year>((object) this.m_ps.InputSession.YearKey)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<Year>>) (t =>
    {
      if (this.IsDisposed)
        return;
      lock (this.m_syncobj)
        this.m_year = t.Result;
      bool configEnabled = this.m_year.ConfigEnabled;
      this.dgStreets.ReadOnly = !configEnabled;
      this.dgStreets.AllowUserToAddRows = configEnabled;
      this.dgStreets.AllowUserToDeleteRows = configEnabled;
      this.OnRequestRefresh();
    }), context);

    private void Streets_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void Streets_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<Street> dataBindingList = sender as DataBindingList<Street>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      Street street = dataBindingList[e.NewIndex];
      if (street.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (this.m_session.BeginTransaction())
          this.m_session.Delete((object) street);
      }
    }

    private void Streets_AddingNew(object sender, AddingNewEventArgs e) => e.NewObject = (object) new Street()
    {
      ProjectLocation = this.m_location
    };

    private void dgStreets_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void dgStreets_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<Street> dataSource = this.dgStreets.DataSource as DataBindingList<Street>;
      if (this.dgStreets.CurrentRow != null && !this.dgStreets.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      Street street1 = dataSource[e.RowIndex];
      string text = (string) null;
      foreach (Street street2 in (Collection<Street>) dataSource)
      {
        if (street2 != street1 && street2.Name.Equals(street1.Name, StringComparison.OrdinalIgnoreCase))
          text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcName.HeaderText);
      }
      if (text == null)
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgStreets_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      DataBindingList<Street> dataSource = this.dgStreets.DataSource as DataBindingList<Street>;
      if (this.dgStreets.CurrentRow != null && this.dgStreets.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          Street street = dataSource[e.RowIndex];
          lock (this.m_syncobj)
          {
            using (ITransaction transaction = this.m_session.BeginTransaction())
            {
              this.m_session.SaveOrUpdate((object) street);
              transaction.Commit();
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgStreets_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void DeleteSelectedRows()
    {
      if (!(this.dgStreets.DataSource is DataBindingList<Street> dataSource))
        return;
      CurrencyManager currencyManager = this.dgStreets.BindingContext[(object) dataSource] as CurrencyManager;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgStreets.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
          dataSource.RemoveAt(selectedRow.Index);
      }
      if (currencyManager.Position == -1)
        return;
      this.dgStreets.Rows[currencyManager.Position].Selected = true;
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgStreets.DataSource != null;
      bool flag2 = flag1 && this.dgStreets.AllowUserToAddRows;
      bool flag3 = this.dgStreets.SelectedRows.Count > 0;
      bool flag4 = this.dgStreets.CurrentRow != null && this.dgStreets.IsCurrentRowDirty;
      bool flag5 = this.dgStreets.CurrentRow != null && this.dgStreets.CurrentRow.IsNewRow;
      bool flag6 = false;
      if (!this.dgStreets.Enabled)
        return flag6;
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
      switch (action)
      {
        case UserActions.New:
          if (!this.dgStreets.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgStreets.SelectedRows)
            selectedRow.Selected = false;
          this.dgStreets.Rows[this.dgStreets.NewRowIndex].Selected = true;
          this.dgStreets.FirstDisplayedScrollingRowIndex = this.dgStreets.NewRowIndex - this.dgStreets.DisplayedRowCount(false) + 1;
          this.dgStreets.CurrentCell = this.dgStreets.Rows[this.dgStreets.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          if (this.dgStreets.SelectedRows.Count <= 0)
            break;
          DataGridViewRow selectedRow1 = this.dgStreets.SelectedRows[0];
          DataBindingList<Street> dataSource = this.dgStreets.DataSource as DataBindingList<Street>;
          if (selectedRow1.Index >= dataSource.Count)
            break;
          Street street = dataSource[selectedRow1.Index].Clone() as Street;
          dataSource.Add(street);
          this.dgStreets.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          if (this.dgStreets.SelectedRows.Count <= 0)
            break;
          this.DeleteSelectedRows();
          break;
      }
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgStreets.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgStreets.DataSource != null;

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
      this.dgStreets = new DataGridView();
      this.dcName = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dgStreets).BeginInit();
      this.SuspendLayout();
      this.lblBreadcrumb.Size = new Size(541, 29);
      this.dgStreets.AllowUserToAddRows = false;
      this.dgStreets.AllowUserToDeleteRows = false;
      this.dgStreets.AllowUserToResizeRows = false;
      this.dgStreets.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgStreets.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgStreets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgStreets.Columns.AddRange((DataGridViewColumn) this.dcName);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgStreets.DefaultCellStyle = gridViewCellStyle2;
      this.dgStreets.Dock = DockStyle.Fill;
      this.dgStreets.Enabled = false;
      this.dgStreets.EnableHeadersVisualStyles = false;
      this.dgStreets.Location = new Point(0, 29);
      this.dgStreets.MultiSelect = false;
      this.dgStreets.Name = "dgStreets";
      this.dgStreets.ReadOnly = true;
      this.dgStreets.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgStreets.RowHeadersWidth = 20;
      this.dgStreets.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      this.dgStreets.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgStreets.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgStreets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgStreets.Size = new Size(541, 315);
      this.dgStreets.TabIndex = 3;
      this.dgStreets.CurrentCellDirtyStateChanged += new EventHandler(this.dgStreets_CurrentCellDirtyStateChanged);
      this.dgStreets.RowValidated += new DataGridViewCellEventHandler(this.dgStreets_RowValidated);
      this.dgStreets.RowValidating += new DataGridViewCellCancelEventHandler(this.dgStreets_RowValidating);
      this.dgStreets.SelectionChanged += new EventHandler(this.dgStreets_SelectionChanged);
      this.dcName.DataPropertyName = "Name";
      this.dcName.HeaderText = "Name";
      this.dcName.MaxInputLength = 50;
      this.dcName.Name = "dcName";
      this.dcName.ReadOnly = true;
      this.dcName.Width = 300;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(541, 344);
      this.Controls.Add((Control) this.dgStreets);
      this.Name = nameof (StreetsForm);
      this.Text = "Street Names";
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgStreets, 0);
      ((ISupportInitialize) this.dgStreets).EndInit();
      this.ResumeLayout(false);
    }
  }
}
