// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.DefinePlotsManuallyForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Views;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Interfaces;
using i_Tree_Eco_v6.Properties;
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
  public class DefinePlotsManuallyForm : ProjectContentForm, IActionable, IExportable
  {
    private ProgramSession m_ps;
    private DataGridViewManager m_dgManager;
    private WaitCursor m_wc;
    private TaskManager m_taskManager;
    private readonly object m_syncobj;
    private ISession m_session;
    private Year m_year;
    private Series m_series;
    private IList<Strata> m_strata;
    private IContainer components;
    private DataGridView dgStrata;
    private FlowLayoutPanel flowLayoutPanel1;
    private Label lblTotalPlots;
    private Label lblTotalSize;
    private Label lblPlots;
    private Label lblSize;
    private TableLayoutPanel tableLayoutPanel1;
    private FlowLayoutPanel flowLayoutPanel2;
    private Button btnCancel;
    private Button btnOK;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn2;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn3;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn4;
    private Label lblPlotSize;
    private NumericTextBox ntbPlotSize;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewTextBoxColumn dcDescription;
    private DataGridViewTextBoxColumn dcAbbreviation;
    private DataGridViewNumericTextBoxColumn dcSize;
    private DataGridViewNumericTextBoxColumn dcExistingPlots;
    private DataGridViewNumericTextBoxColumn dcPlotsToAdd;
    private Label label1;

    public DefinePlotsManuallyForm()
    {
      this.m_ps = Program.Session;
      this.InitializeComponent();
      this.m_dgManager = new DataGridViewManager(this.dgStrata);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.m_syncobj = new object();
      this.dgStrata.DoubleBuffered(true);
      this.dgStrata.AutoGenerateColumns = false;
      this.dgStrata.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgStrata.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.dcSize.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcPlotsToAdd.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.m_ps.InputSession.YearChanged += new EventHandler(this.InputSession_YearChanged);
      this.Init();
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
        NHibernateUtil.Initialize((object) y.Series);
        IList<Strata> strataList = session.QueryOver<Strata>().Where((System.Linq.Expressions.Expression<Func<Strata, bool>>) (st => st.Year == y)).OrderBy((System.Linq.Expressions.Expression<Func<Strata, object>>) (st => (object) st.Id)).Asc.List();
        lock (this.m_syncobj)
        {
          this.m_session = session;
          this.m_year = y;
          this.m_series = y.Series;
          this.m_strata = strataList;
        }
      }
    }

    private void InitGrid()
    {
      Year year = this.m_year;
      double num = Settings.Default.PlotSize;
      string str;
      if (year.Unit == YearUnit.English)
      {
        str = i_Tree_Eco_v6.Resources.Strings.UnitAcresAbbr;
      }
      else
      {
        str = i_Tree_Eco_v6.Resources.Strings.UnitHectaresAbbr;
        this.ntbPlotSize.DecimalPlaces = 5;
        num = Math.Round(num * 158080329.0 / 390625000.0, 5);
      }
      this.ntbPlotSize.Text = num.ToString();
      this.dcSize.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.Area, (object) str);
      this.lblTotalSize.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtLabel, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtTotal, (object) i_Tree_Eco_v6.Resources.Strings.Area), (object) str));
      this.lblPlotSize.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtLabel, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.NewPlotArea, (object) str));
      DataBindingList<PlotStrata> dataBindingList = new DataBindingList<PlotStrata>();
      foreach (Strata stratum in (IEnumerable<Strata>) this.m_strata)
      {
        PlotStrata plotStrata = new PlotStrata(stratum);
        dataBindingList.Add(plotStrata);
      }
      this.dgStrata.DataSource = (object) dataBindingList;
      this.dgStrata.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.PlotStrata_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.PlotStrata_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.PlotStrata_ListChanged);
    }

    protected override void OnRequestRefresh()
    {
      bool flag = this.m_year != null && this.m_year.ConfigEnabled;
      this.dgStrata.ReadOnly = !flag;
      this.dgStrata.AllowUserToAddRows = flag;
      this.dgStrata.AllowUserToDeleteRows = flag;
      this.dcExistingPlots.ReadOnly = true;
      base.OnRequestRefresh();
    }

    private void PlotStrata_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void PlotStrata_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<PlotStrata> dataBindingList = sender as DataBindingList<PlotStrata>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      PlotStrata plotStrata = dataBindingList[e.NewIndex];
      if (plotStrata.Strata.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) plotStrata.Strata);
          transaction.Commit();
        }
      }
    }

    private void PlotStrata_AddingNew(object sender, AddingNewEventArgs e)
    {
      DataBindingList<PlotStrata> dataBindingList = sender as DataBindingList<PlotStrata>;
      int num = 1;
      foreach (PlotStrata plotStrata in (Collection<PlotStrata>) dataBindingList)
      {
        if (plotStrata.Id >= num)
          num = plotStrata.Id + 1;
      }
      e.NewObject = (object) new PlotStrata()
      {
        Year = this.m_year,
        Id = num
      };
    }

    private void dgStrata_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void dgStrata_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<PlotStrata> dataSource = this.dgStrata.DataSource as DataBindingList<PlotStrata>;
      if (this.dgStrata.CurrentRow != null && !this.dgStrata.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      PlotStrata plotStrata1 = dataSource[e.RowIndex];
      DataGridViewRow row = this.dgStrata.Rows[e.RowIndex];
      DataGridViewCell dataGridViewCell1 = (DataGridViewCell) null;
      string text = (string) null;
      foreach (PlotStrata plotStrata2 in (Collection<PlotStrata>) dataSource)
      {
        if (plotStrata2 != plotStrata1)
        {
          if (plotStrata2.Id == plotStrata1.Id)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcId.HeaderText);
            dataGridViewCell1 = row.Cells[this.dcId.DisplayIndex];
            break;
          }
          if (plotStrata2.Description == plotStrata1.Description)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcDescription.HeaderText);
            dataGridViewCell1 = row.Cells[this.dcDescription.DisplayIndex];
            break;
          }
          if (plotStrata2.Abbreviation == plotStrata1.Abbreviation)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcAbbreviation.HeaderText);
            dataGridViewCell1 = row.Cells[this.dcAbbreviation.DisplayIndex];
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

    private void dgStrata_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      DataBindingList<PlotStrata> dataSource = this.dgStrata.DataSource as DataBindingList<PlotStrata>;
      if (this.dgStrata.CurrentRow != null && this.dgStrata.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          PlotStrata plotStrata = dataSource[e.RowIndex];
          if (plotStrata.Strata.IsTransient || this.m_session.IsDirty())
          {
            lock (this.m_syncobj)
            {
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                this.m_session.SaveOrUpdate((object) plotStrata.Strata);
                transaction.Commit();
              }
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgStrata_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void DeleteSelectedRows()
    {
      if (!(this.dgStrata.DataSource is DataBindingList<PlotStrata> dataSource))
        return;
      CurrencyManager currencyManager = this.dgStrata.BindingContext[(object) dataSource] as CurrencyManager;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgStrata.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          PlotStrata plotStrata = dataSource[selectedRow.Index];
          if (dataSource.Count > 1 && plotStrata.TotalPlots > 0)
          {
            using (DeleteStrataForm deleteStrataForm = new DeleteStrataForm(plotStrata.Strata, this.m_series.IsSample))
            {
              if (deleteStrataForm.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
                continue;
            }
          }
          dataSource.RemoveAt(selectedRow.Index);
        }
      }
      if (currencyManager.Position == -1)
        return;
      this.dgStrata.Rows[currencyManager.Position].Selected = true;
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgStrata.DataSource != null;
      bool flag2 = flag1 && this.dgStrata.AllowUserToAddRows;
      bool flag3 = this.dgStrata.SelectedRows.Count > 0;
      bool flag4 = this.dgStrata.CurrentRow != null && this.dgStrata.IsCurrentRowDirty;
      bool flag5 = this.dgStrata.CurrentRow != null && this.dgStrata.CurrentRow.IsNewRow;
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
      switch (action)
      {
        case UserActions.New:
          if (!this.dgStrata.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgStrata.SelectedRows)
            selectedRow.Selected = false;
          this.dgStrata.Rows[this.dgStrata.NewRowIndex].Selected = true;
          this.dgStrata.FirstDisplayedScrollingRowIndex = this.dgStrata.NewRowIndex - this.dgStrata.DisplayedRowCount(false) + 1;
          this.dgStrata.CurrentCell = this.dgStrata.Rows[this.dgStrata.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          if (this.dgStrata.SelectedRows.Count <= 0)
            break;
          DataGridViewRow selectedRow1 = this.dgStrata.SelectedRows[0];
          if (!(this.dgStrata.DataSource is DataBindingList<PlotStrata> dataSource) || selectedRow1.Index >= dataSource.Count)
            break;
          PlotStrata plotStrata1 = dataSource[selectedRow1.Index].Clone() as PlotStrata;
          int num = 1;
          foreach (PlotStrata plotStrata2 in (Collection<PlotStrata>) dataSource)
          {
            if (plotStrata2.Id >= num)
              num = plotStrata2.Id + 1;
          }
          plotStrata1.Id = num;
          dataSource.Add(plotStrata1);
          this.dgStrata.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          if (this.dgStrata.SelectedRows.Count <= 0)
            break;
          this.DeleteSelectedRows();
          break;
      }
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgStrata.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgStrata.DataSource != null;

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void dgStrata_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (!(this.dgStrata.DataSource is DataBindingList<PlotStrata> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgStrata.Columns[e.ColumnIndex];
      PlotStrata plotStrata = dataSource[e.RowIndex];
      if (column == this.dcId)
      {
        if (plotStrata.Id > 0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcDescription)
      {
        if (plotStrata.Description != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcAbbreviation)
      {
        if (plotStrata.Abbreviation != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else
      {
        if (column != this.dcSize || (double) plotStrata.Size + 1.0 >= 1.4012984643248171E-45)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.ValidateChildren())
        return;
      this.m_taskManager.Add(Task.Factory.StartNew<int>((Func<int>) (() => this.CreatePlots()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<int>>) (task =>
      {
        if (this.IsDisposed)
          return;
        if (task.IsFaulted)
        {
          int num1 = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrUnexpected, (object) task.Exception.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          if (task.Result > 0)
          {
            EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(this.m_year), (Control) this);
            int num2 = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.MsgPlotsCreated, (object) task.Result), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          this.Close();
        }
      }), TaskScheduler.FromCurrentSynchronizationContext()));
    }

    private int CreatePlots()
    {
      DataBindingList<PlotStrata> dataSource = this.dgStrata.DataSource as DataBindingList<PlotStrata>;
      float num1 = float.Parse(this.ntbPlotSize.Text);
      int plots = 0;
      using (ITransaction transaction = this.m_session.BeginTransaction())
      {
        this.m_session.SaveOrUpdate((object) this.m_series);
        foreach (PlotStrata plotStrata in (Collection<PlotStrata>) dataSource)
          this.m_session.SaveOrUpdate((object) plotStrata.Strata);
        transaction.Commit();
      }
      using (IStatelessSession statelessSession = this.m_ps.InputSession.CreateStatelessSession())
      {
        using (ITransaction transaction = statelessSession.BeginTransaction())
        {
          int num2 = statelessSession.QueryOver<Plot>().Select((IProjection) Projections.Max<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id))).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.m_year)).SingleOrDefault<int>() + 1;
          foreach (PlotStrata plotStrata in (Collection<PlotStrata>) dataSource)
          {
            int num3 = 0;
            while (num3 < plotStrata.PlotsToAdd)
            {
              Plot entity = new Plot();
              entity.Id = num2;
              entity.Size = num1;
              entity.Year = this.m_year;
              entity.Strata = plotStrata.Strata;
              ++plots;
              statelessSession.Insert((object) entity);
              ++num3;
              ++num2;
            }
          }
          transaction.Commit();
        }
      }
      return plots;
    }

    private void ntbPlotSize_Validating(object sender, CancelEventArgs e)
    {
      float result = 0.0f;
      if (!float.TryParse(this.ntbPlotSize.Text, out result) || (double) result > 0.0)
        return;
      int num = (int) MessageBox.Show(i_Tree_Eco_v6.Resources.Strings.ErrInvalidPlotSize, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      e.Cancel = true;
    }

    private void DefinePlotsManuallyForm_RequestRefresh(object sender, EventArgs e)
    {
      if (!(this.dgStrata.DataSource is DataBindingList<PlotStrata> dataSource))
        return;
      int num1 = 0;
      float num2 = 0.0f;
      foreach (PlotStrata plotStrata in (Collection<PlotStrata>) dataSource)
      {
        num1 += plotStrata.TotalPlots + plotStrata.PlotsToAdd;
        num2 += plotStrata.Size;
      }
      this.lblPlots.Text = num1.ToString("N0");
      this.lblSize.Text = num2.ToString("N2");
    }

    private void dgStrata_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (!(this.dgStrata.DataSource is DataBindingList<PlotStrata> dataSource) || e.Row.Index >= dataSource.Count)
        return;
      PlotStrata plotStrata = dataSource[e.Row.Index];
      if (dataSource.Count <= 1 || plotStrata.TotalPlots <= 0)
        return;
      using (DeleteStrataForm deleteStrataForm = new DeleteStrataForm(plotStrata.Strata, this.m_series.IsSample))
      {
        DialogResult dialogResult = deleteStrataForm.ShowDialog((IWin32Window) this);
        e.Cancel = dialogResult == DialogResult.Cancel;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DefinePlotsManuallyForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle7 = new DataGridViewCellStyle();
      this.dgStrata = new DataGridView();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcDescription = new DataGridViewTextBoxColumn();
      this.dcAbbreviation = new DataGridViewTextBoxColumn();
      this.dcSize = new DataGridViewNumericTextBoxColumn();
      this.dcExistingPlots = new DataGridViewNumericTextBoxColumn();
      this.dcPlotsToAdd = new DataGridViewNumericTextBoxColumn();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.lblPlotSize = new Label();
      this.ntbPlotSize = new NumericTextBox();
      this.lblTotalPlots = new Label();
      this.lblPlots = new Label();
      this.lblTotalSize = new Label();
      this.lblSize = new Label();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.label1 = new Label();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.dataGridViewNumericTextBoxColumn1 = new DataGridViewNumericTextBoxColumn();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewNumericTextBoxColumn2 = new DataGridViewNumericTextBoxColumn();
      this.dataGridViewNumericTextBoxColumn3 = new DataGridViewNumericTextBoxColumn();
      this.dataGridViewNumericTextBoxColumn4 = new DataGridViewNumericTextBoxColumn();
      ((ISupportInitialize) this.dgStrata).BeginInit();
      this.flowLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgStrata.AllowUserToResizeRows = false;
      this.dgStrata.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgStrata.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgStrata.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgStrata.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcDescription, (DataGridViewColumn) this.dcAbbreviation, (DataGridViewColumn) this.dcSize, (DataGridViewColumn) this.dcExistingPlots, (DataGridViewColumn) this.dcPlotsToAdd);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgStrata.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgStrata, "dgStrata");
      this.dgStrata.EnableHeadersVisualStyles = false;
      this.dgStrata.MultiSelect = false;
      this.dgStrata.Name = "dgStrata";
      this.dgStrata.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgStrata.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgStrata.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgStrata.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgStrata_CellErrorTextNeeded);
      this.dgStrata.CurrentCellDirtyStateChanged += new EventHandler(this.dgStrata_CurrentCellDirtyStateChanged);
      this.dgStrata.RowValidated += new DataGridViewCellEventHandler(this.dgStrata_RowValidated);
      this.dgStrata.RowValidating += new DataGridViewCellCancelEventHandler(this.dgStrata_RowValidating);
      this.dgStrata.SelectionChanged += new EventHandler(this.dgStrata_SelectionChanged);
      this.dgStrata.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.dgStrata_UserDeletingRow);
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
      this.dcDescription.Name = "dcDescription";
      this.dcAbbreviation.DataPropertyName = "Abbreviation";
      componentResourceManager.ApplyResources((object) this.dcAbbreviation, "dcAbbreviation");
      this.dcAbbreviation.MaxInputLength = 8;
      this.dcAbbreviation.Name = "dcAbbreviation";
      this.dcSize.DataPropertyName = "Size";
      this.dcSize.DecimalPlaces = 2;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle3.Format = "N2";
      this.dcSize.DefaultCellStyle = gridViewCellStyle3;
      this.dcSize.Format = "#.#;#.#";
      this.dcSize.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcSize, "dcSize");
      this.dcSize.Name = "dcSize";
      this.dcSize.Signed = false;
      this.dcExistingPlots.DataPropertyName = "TotalPlots";
      this.dcExistingPlots.DecimalPlaces = 0;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle4.BackColor = Color.LightGray;
      gridViewCellStyle4.ForeColor = Color.Black;
      gridViewCellStyle4.Format = "N0";
      this.dcExistingPlots.DefaultCellStyle = gridViewCellStyle4;
      this.dcExistingPlots.Format = "#;-#";
      this.dcExistingPlots.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcExistingPlots, "dcExistingPlots");
      this.dcExistingPlots.Name = "dcExistingPlots";
      this.dcExistingPlots.ReadOnly = true;
      this.dcExistingPlots.Signed = false;
      this.dcPlotsToAdd.DataPropertyName = "PlotsToAdd";
      this.dcPlotsToAdd.DecimalPlaces = 0;
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle5.Format = "N0";
      this.dcPlotsToAdd.DefaultCellStyle = gridViewCellStyle5;
      this.dcPlotsToAdd.Format = "#;-#";
      this.dcPlotsToAdd.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcPlotsToAdd, "dcPlotsToAdd");
      this.dcPlotsToAdd.Name = "dcPlotsToAdd";
      this.dcPlotsToAdd.Signed = false;
      componentResourceManager.ApplyResources((object) this.flowLayoutPanel1, "flowLayoutPanel1");
      this.flowLayoutPanel1.Controls.Add((Control) this.lblPlotSize);
      this.flowLayoutPanel1.Controls.Add((Control) this.ntbPlotSize);
      this.flowLayoutPanel1.Controls.Add((Control) this.lblTotalPlots);
      this.flowLayoutPanel1.Controls.Add((Control) this.lblPlots);
      this.flowLayoutPanel1.Controls.Add((Control) this.lblTotalSize);
      this.flowLayoutPanel1.Controls.Add((Control) this.lblSize);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.lblPlotSize, "lblPlotSize");
      this.lblPlotSize.Name = "lblPlotSize";
      this.ntbPlotSize.BorderStyle = BorderStyle.FixedSingle;
      this.ntbPlotSize.DecimalPlaces = 2;
      this.ntbPlotSize.Format = "0.#;-0.#";
      this.ntbPlotSize.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.ntbPlotSize, "ntbPlotSize");
      this.ntbPlotSize.Name = "ntbPlotSize";
      this.ntbPlotSize.Signed = true;
      this.ntbPlotSize.Validating += new CancelEventHandler(this.ntbPlotSize_Validating);
      componentResourceManager.ApplyResources((object) this.lblTotalPlots, "lblTotalPlots");
      this.lblTotalPlots.Name = "lblTotalPlots";
      componentResourceManager.ApplyResources((object) this.lblPlots, "lblPlots");
      this.lblPlots.Name = "lblPlots";
      componentResourceManager.ApplyResources((object) this.lblTotalSize, "lblTotalSize");
      this.lblTotalSize.Name = "lblTotalSize";
      componentResourceManager.ApplyResources((object) this.lblSize, "lblSize");
      this.lblSize.Name = "lblSize";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.CausesValidation = false;
      this.tableLayoutPanel1.Controls.Add((Control) this.label1, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.flowLayoutPanel1, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.flowLayoutPanel2, 1, 1);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label1, 2);
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.flowLayoutPanel2, "flowLayoutPanel2");
      this.flowLayoutPanel2.CausesValidation = false;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnCancel);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnOK);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.btnCancel.CausesValidation = false;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
      this.btnOK.Name = "btnOK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.dataGridViewNumericTextBoxColumn1.DataPropertyName = "Id";
      this.dataGridViewNumericTextBoxColumn1.DecimalPlaces = 0;
      this.dataGridViewNumericTextBoxColumn1.Format = "#;#";
      this.dataGridViewNumericTextBoxColumn1.Frozen = true;
      this.dataGridViewNumericTextBoxColumn1.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dataGridViewNumericTextBoxColumn1, "dataGridViewNumericTextBoxColumn1");
      this.dataGridViewNumericTextBoxColumn1.Name = "dataGridViewNumericTextBoxColumn1";
      this.dataGridViewNumericTextBoxColumn1.Signed = false;
      this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dataGridViewTextBoxColumn1.DataPropertyName = "Description";
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn2.DataPropertyName = "Abbreviation";
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewNumericTextBoxColumn2.DataPropertyName = "Size";
      this.dataGridViewNumericTextBoxColumn2.DecimalPlaces = 2;
      gridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle6.BackColor = Color.LightGray;
      gridViewCellStyle6.ForeColor = Color.Black;
      gridViewCellStyle6.Format = "N2";
      this.dataGridViewNumericTextBoxColumn2.DefaultCellStyle = gridViewCellStyle6;
      this.dataGridViewNumericTextBoxColumn2.Format = "#.#;#.#";
      this.dataGridViewNumericTextBoxColumn2.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dataGridViewNumericTextBoxColumn2, "dataGridViewNumericTextBoxColumn2");
      this.dataGridViewNumericTextBoxColumn2.Name = "dataGridViewNumericTextBoxColumn2";
      this.dataGridViewNumericTextBoxColumn2.ReadOnly = true;
      this.dataGridViewNumericTextBoxColumn2.Signed = false;
      this.dataGridViewNumericTextBoxColumn3.DataPropertyName = "TotalPlots";
      this.dataGridViewNumericTextBoxColumn3.DecimalPlaces = 0;
      gridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle7.BackColor = Color.LightGray;
      gridViewCellStyle7.ForeColor = Color.Black;
      this.dataGridViewNumericTextBoxColumn3.DefaultCellStyle = gridViewCellStyle7;
      this.dataGridViewNumericTextBoxColumn3.Format = "#;-#";
      this.dataGridViewNumericTextBoxColumn3.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dataGridViewNumericTextBoxColumn3, "dataGridViewNumericTextBoxColumn3");
      this.dataGridViewNumericTextBoxColumn3.Name = "dataGridViewNumericTextBoxColumn3";
      this.dataGridViewNumericTextBoxColumn3.ReadOnly = true;
      this.dataGridViewNumericTextBoxColumn3.Signed = false;
      this.dataGridViewNumericTextBoxColumn4.DataPropertyName = "PlotsToAdd";
      this.dataGridViewNumericTextBoxColumn4.DecimalPlaces = 0;
      this.dataGridViewNumericTextBoxColumn4.Format = "#;-#";
      this.dataGridViewNumericTextBoxColumn4.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dataGridViewNumericTextBoxColumn4, "dataGridViewNumericTextBoxColumn4");
      this.dataGridViewNumericTextBoxColumn4.Name = "dataGridViewNumericTextBoxColumn4";
      this.dataGridViewNumericTextBoxColumn4.Signed = false;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoValidate = AutoValidate.EnableAllowFocusChange;
      this.Controls.Add((Control) this.dgStrata);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.DockAreas = DockAreas.Document;
      this.Name = nameof (DefinePlotsManuallyForm);
      this.ShowHint = DockState.Document;
      this.RequestRefresh += new EventHandler<EventArgs>(this.DefinePlotsManuallyForm_RequestRefresh);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel1, 0);
      this.Controls.SetChildIndex((Control) this.dgStrata, 0);
      ((ISupportInitialize) this.dgStrata).EndInit();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.flowLayoutPanel2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
