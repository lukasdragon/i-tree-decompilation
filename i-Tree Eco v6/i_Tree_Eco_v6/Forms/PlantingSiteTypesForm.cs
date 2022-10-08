// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.PlantingSiteTypesForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class PlantingSiteTypesForm : ProjectContentForm, IActionable, IExportable
  {
    private ProgramSession m_ps;
    private DataGridViewManager m_dgManager;
    private WaitCursor m_wc;
    private TaskManager m_taskManager;
    private readonly object m_syncobj;
    private ISession m_session;
    private Year m_year;
    private IList<PlantingSiteType> m_siteTypes;
    private IContainer components;
    private DataGridView dgPlantingSiteTypes;
    private DataGridViewTextBoxColumn dcDescription;
    private DataGridViewNumericTextBoxColumn dcSize;

    public PlantingSiteTypesForm()
    {
      this.m_ps = Program.Session;
      this.m_session = this.m_ps.InputSession.CreateSession();
      this.InitializeComponent();
      this.dcSize.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.m_dgManager = new DataGridViewManager(this.dgPlantingSiteTypes);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.m_syncobj = new object();
      this.dgPlantingSiteTypes.DoubleBuffered(true);
      this.dgPlantingSiteTypes.AutoGenerateColumns = false;
      this.dgPlantingSiteTypes.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgPlantingSiteTypes.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.m_ps.InputSession.YearChanged += new EventHandler(this.InputSession_YearChanged);
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
      using (this.m_session.BeginTransaction())
      {
        Year year = this.m_session.Get<Year>((object) this.m_ps.InputSession.YearKey);
        IList<PlantingSiteType> plantingSiteTypeList = this.m_session.CreateCriteria<PlantingSite>().Add((ICriterion) Restrictions.Eq("Year", (object) year)).AddOrder(Order.Asc("Description")).List<PlantingSiteType>();
        lock (this.m_syncobj)
        {
          this.m_year = year;
          this.m_siteTypes = plantingSiteTypeList;
        }
      }
    }

    private void InitGrid()
    {
      DataBindingList<PlantingSiteType> dataBindingList = new DataBindingList<PlantingSiteType>(this.m_siteTypes);
      this.dgPlantingSiteTypes.DataSource = (object) dataBindingList;
      this.dgPlantingSiteTypes.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.PlantingSiteTypes_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.PlantingSiteTypes_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.PlantingSiteTypes_ListChanged);
      this.OnRequestRefresh();
    }

    protected override void OnRequestRefresh()
    {
      Year year = this.m_year;
      bool flag1 = year != null && year.ConfigEnabled && this.dgPlantingSiteTypes.DataSource != null;
      bool flag2 = flag1 && this.m_siteTypes != null && this.m_siteTypes.Count > 0;
      this.dgPlantingSiteTypes.ReadOnly = !flag1;
      this.dgPlantingSiteTypes.AllowUserToAddRows = flag1;
      this.dgPlantingSiteTypes.AllowUserToDeleteRows = flag2;
      base.OnRequestRefresh();
    }

    private void GetYear(Guid g)
    {
      if (this.m_year == null || this.m_session == null || !(this.m_year.Guid == g))
        return;
      using (this.m_session.BeginTransaction())
        this.m_session.Refresh((object) this.m_year);
    }

    private void PlantingSiteTypes_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void PlantingSiteTypes_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<PlantingSiteType> dataBindingList = sender as DataBindingList<PlantingSiteType>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      PlantingSiteType plantingSiteType = dataBindingList[e.NewIndex];
      if (plantingSiteType.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) plantingSiteType);
          transaction.Commit();
        }
      }
    }

    private void PlantingSiteTypes_AddingNew(object sender, AddingNewEventArgs e) => e.NewObject = (object) new PlantingSiteType()
    {
      Year = this.m_year
    };

    private void dgPlantingSiteTypes_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void dgPlantingSiteTypes_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<PlantingSiteType> dataSource = this.dgPlantingSiteTypes.DataSource as DataBindingList<PlantingSiteType>;
      if (this.dgPlantingSiteTypes.CurrentRow != null && !this.dgPlantingSiteTypes.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      PlantingSiteType plantingSiteType1 = dataSource[e.RowIndex];
      string text = (string) null;
      if (string.IsNullOrEmpty(plantingSiteType1.Description))
      {
        text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) this.dcDescription.HeaderText);
      }
      else
      {
        foreach (PlantingSiteType plantingSiteType2 in (Collection<PlantingSiteType>) dataSource)
        {
          if (plantingSiteType2 != plantingSiteType1 && plantingSiteType2.Description == plantingSiteType1.Description)
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

    private void dgPlantingSiteTypes_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      DataBindingList<PlantingSiteType> dataSource = this.dgPlantingSiteTypes.DataSource as DataBindingList<PlantingSiteType>;
      if (this.dgPlantingSiteTypes.CurrentRow != null && this.dgPlantingSiteTypes.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          PlantingSiteType plantingSiteType = dataSource[e.RowIndex];
          lock (this.m_syncobj)
          {
            using (ITransaction transaction = this.m_session.BeginTransaction())
            {
              this.m_session.SaveOrUpdate((object) plantingSiteType);
              transaction.Commit();
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgPlantingSiteTypes_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void DeleteSelectedRows()
    {
      if (!(this.dgPlantingSiteTypes.DataSource is DataBindingList<PlantingSiteType> dataSource))
        return;
      CurrencyManager currencyManager = this.dgPlantingSiteTypes.BindingContext[(object) dataSource] as CurrencyManager;
      bool flag = false;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgPlantingSiteTypes.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          dataSource.RemoveAt(selectedRow.Index);
          flag = true;
        }
      }
      if (!(currencyManager.Position != -1 & flag))
        return;
      this.dgPlantingSiteTypes.Rows[currencyManager.Position].Selected = true;
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgPlantingSiteTypes.DataSource != null;
      bool flag2 = flag1 && this.dgPlantingSiteTypes.AllowUserToAddRows;
      bool flag3 = this.dgPlantingSiteTypes.SelectedRows.Count > 0;
      bool flag4 = this.dgPlantingSiteTypes.CurrentRow != null && this.dgPlantingSiteTypes.CurrentRow.IsNewRow;
      bool flag5 = this.dgPlantingSiteTypes.CurrentRow != null && this.dgPlantingSiteTypes.IsCurrentRowDirty;
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
      }
      return flag6;
    }

    public void PerformAction(UserActions action)
    {
      switch (action)
      {
        case UserActions.New:
          if (!this.dgPlantingSiteTypes.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgPlantingSiteTypes.SelectedRows)
            selectedRow.Selected = false;
          this.dgPlantingSiteTypes.Rows[this.dgPlantingSiteTypes.NewRowIndex].Selected = true;
          this.dgPlantingSiteTypes.FirstDisplayedScrollingRowIndex = this.dgPlantingSiteTypes.NewRowIndex - this.dgPlantingSiteTypes.DisplayedRowCount(false) + 1;
          this.dgPlantingSiteTypes.CurrentCell = this.dgPlantingSiteTypes.Rows[this.dgPlantingSiteTypes.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          if (this.dgPlantingSiteTypes.SelectedRows.Count <= 0)
            break;
          DataGridViewRow selectedRow1 = this.dgPlantingSiteTypes.SelectedRows[0];
          if (!(this.dgPlantingSiteTypes.DataSource is DataBindingList<PlantingSiteType> dataSource) || selectedRow1.Index >= dataSource.Count)
            break;
          PlantingSiteType plantingSiteType = dataSource[selectedRow1.Index].Clone() as PlantingSiteType;
          dataSource.Add(plantingSiteType);
          this.dgPlantingSiteTypes.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          if (this.dgPlantingSiteTypes.SelectedRows.Count <= 0)
            break;
          this.DeleteSelectedRows();
          break;
      }
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgPlantingSiteTypes.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgPlantingSiteTypes.DataSource != null;

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
      this.dgPlantingSiteTypes = new DataGridView();
      this.dcDescription = new DataGridViewTextBoxColumn();
      this.dcSize = new DataGridViewNumericTextBoxColumn();
      ((ISupportInitialize) this.dgPlantingSiteTypes).BeginInit();
      this.SuspendLayout();
      this.lblBreadcrumb.Size = new Size(526, 29);
      this.dgPlantingSiteTypes.AllowUserToAddRows = false;
      this.dgPlantingSiteTypes.AllowUserToDeleteRows = false;
      this.dgPlantingSiteTypes.AllowUserToResizeRows = false;
      this.dgPlantingSiteTypes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgPlantingSiteTypes.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgPlantingSiteTypes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgPlantingSiteTypes.Columns.AddRange((DataGridViewColumn) this.dcDescription, (DataGridViewColumn) this.dcSize);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgPlantingSiteTypes.DefaultCellStyle = gridViewCellStyle2;
      this.dgPlantingSiteTypes.Dock = DockStyle.Fill;
      this.dgPlantingSiteTypes.EnableHeadersVisualStyles = false;
      this.dgPlantingSiteTypes.Location = new Point(0, 29);
      this.dgPlantingSiteTypes.MultiSelect = false;
      this.dgPlantingSiteTypes.Name = "dgPlantingSiteTypes";
      this.dgPlantingSiteTypes.ReadOnly = true;
      this.dgPlantingSiteTypes.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgPlantingSiteTypes.RowHeadersWidth = 20;
      this.dgPlantingSiteTypes.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgPlantingSiteTypes.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgPlantingSiteTypes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgPlantingSiteTypes.Size = new Size(526, 285);
      this.dgPlantingSiteTypes.TabIndex = 3;
      this.dgPlantingSiteTypes.CurrentCellDirtyStateChanged += new EventHandler(this.dgPlantingSiteTypes_CurrentCellDirtyStateChanged);
      this.dgPlantingSiteTypes.RowValidated += new DataGridViewCellEventHandler(this.dgPlantingSiteTypes_RowValidated);
      this.dgPlantingSiteTypes.RowValidating += new DataGridViewCellCancelEventHandler(this.dgPlantingSiteTypes_RowValidating);
      this.dgPlantingSiteTypes.SelectionChanged += new EventHandler(this.dgPlantingSiteTypes_SelectionChanged);
      this.dcDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDescription.DataPropertyName = "Description";
      this.dcDescription.HeaderText = "Description";
      this.dcDescription.MaxInputLength = 50;
      this.dcDescription.Name = "dcDescription";
      this.dcDescription.ReadOnly = true;
      this.dcDescription.Width = 300;
      this.dcSize.DataPropertyName = "Size";
      this.dcSize.DecimalPlaces = 2;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle3.Format = "N2";
      this.dcSize.DefaultCellStyle = gridViewCellStyle3;
      this.dcSize.Format = "#.#;#.#";
      this.dcSize.HasDecimal = true;
      this.dcSize.HeaderText = "Size";
      this.dcSize.Name = "dcSize";
      this.dcSize.ReadOnly = true;
      this.dcSize.Signed = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(526, 314);
      this.Controls.Add((Control) this.dgPlantingSiteTypes);
      this.Name = nameof (PlantingSiteTypesForm);
      this.Text = "Planting Site Type";
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgPlantingSiteTypes, 0);
      ((ISupportInitialize) this.dgPlantingSiteTypes).EndInit();
      this.ResumeLayout(false);
    }
  }
}
