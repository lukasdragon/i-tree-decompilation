// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.StrataForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Views;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Interfaces;
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
  public class StrataForm : ProjectContentForm, IActionable, IExportable
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
    private FlowLayoutPanel pnlSampleInfo;
    private Label lblTotalPlots;
    private Label lblTotalSize;
    private Label lblPlots;
    private Label lblSize;
    private TableLayoutPanel tableLayoutPanel1;
    private Button btnOK;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn2;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn3;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn4;
    private Label lblStrata;
    private Label lblNote;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewTextBoxColumn dcDescription;
    private DataGridViewTextBoxColumn dcAbbreviation;
    private DataGridViewNumericTextBoxColumn dcSize;
    private DataGridViewNumericTextBoxColumn dcExistingPlots;

    public StrataForm()
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
      this.dcSize.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.m_ps.InputSession.YearChanged += new EventHandler(this.InputSession_YearChanged);
      EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));
      EventPublisher.Register<EntityUpdated<Strata>>(new EventHandler<EntityUpdated<Strata>>(this.Strata_Updated));
      this.Init();
    }

    private void Year_Updated(object sender, EntityUpdated<Year> e)
    {
      if (this.m_year == null || !(this.m_year.Guid == e.Guid))
        return;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() =>
      {
        lock (this.m_syncobj)
        {
          if (this.m_session == null)
            return;
          this.m_session.Refresh((object) this.m_year);
        }
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.OnRequestRefresh();
      }), scheduler));
    }

    private void Strata_Updated(object sender, EntityUpdated<Strata> e)
    {
      foreach (PlotStrata plotStrata in (Collection<PlotStrata>) (this.dgStrata.DataSource as DataBindingList<PlotStrata>))
      {
        if (plotStrata.Strata.Guid == e.Guid)
        {
          lock (this.m_syncobj)
          {
            using (this.m_session.BeginTransaction())
            {
              this.m_session.Refresh((object) plotStrata.Strata);
              break;
            }
          }
        }
      }
      this.OnRequestRefresh();
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
        IList<Strata> strataList = session.QueryOver<Strata>().Where((Expression<Func<Strata, bool>>) (st => st.Year == y)).OrderBy((Expression<Func<Strata, object>>) (st => (object) st.Id)).Asc.List();
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
      if (!this.m_series.IsSample)
        this.dcExistingPlots.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtNumberOf, (object) v6Strings.Tree_PluralName);
      this.lblTotalPlots.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtLabel, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtTotal, (object) this.dcExistingPlots.HeaderText));
      this.dcSize.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.Area, this.m_year.Unit != YearUnit.English ? (object) i_Tree_Eco_v6.Resources.Strings.UnitHectaresAbbr : (object) i_Tree_Eco_v6.Resources.Strings.UnitAcresAbbr);
      this.lblTotalSize.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtLabel, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtTotal, (object) this.dcSize.HeaderText));
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
      this.m_wc.Show();
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) plotStrata.Strata);
          transaction.Commit();
        }
      }
      this.m_wc.Hide();
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
      if (this.dgStrata.CurrentRow != null && this.dgStrata.CurrentRow.IsNewRow && !this.dgStrata.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
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
      if (this.dgStrata.CurrentRow != null && this.dgStrata.CurrentRow.IsNewRow && !this.dgStrata.IsCurrentRowDirty)
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
              this.m_wc.Show();
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                this.m_session.SaveOrUpdate((object) plotStrata.Strata);
                transaction.Commit();
              }
              this.m_wc.Hide();
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
      this.m_wc.Show();
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgStrata.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          PlotStrata plotStrata = dataSource[selectedRow.Index];
          if (dataSource.Count > 1 & plotStrata.TotalPlots > 0)
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
      if (currencyManager.Position != -1)
        this.dgStrata.Rows[currencyManager.Position].Selected = true;
      this.m_wc.Hide();
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.dgStrata.SelectedRows.Count > 0;
      bool flag2 = this.m_year != null && this.m_year.Changed;
      bool flag3 = this.m_year != null && this.m_year.RecordStrata || this.m_series != null && this.m_series.IsSample;
      bool flag4 = this.dgStrata.CurrentRow != null && this.dgStrata.IsCurrentRowDirty;
      bool flag5 = this.dgStrata.CurrentRow != null && this.dgStrata.CurrentRow.IsNewRow;
      bool flag6 = false;
      switch (action)
      {
        case UserActions.New:
          flag6 = flag2 & flag3 && !flag5 && !flag4;
          break;
        case UserActions.Copy:
          flag6 = ((!(flag2 & flag1) || flag5 ? 0 : (!flag4 ? 1 : 0)) & (flag3 ? 1 : 0)) != 0;
          break;
        case UserActions.Undo:
          flag6 = flag2 && this.m_dgManager.CanUndo;
          break;
        case UserActions.Redo:
          flag6 = flag2 && this.m_dgManager.CanRedo;
          break;
        case UserActions.Delete:
          flag6 = ((!(flag2 & flag1) ? 0 : (!flag5 | flag4 ? 1 : 0)) & (flag3 ? 1 : 0)) != 0;
          break;
      }
      return flag6;
    }

    protected override void OnRequestRefresh()
    {
      DataBindingList<PlotStrata> dataSource = this.dgStrata.DataSource as DataBindingList<PlotStrata>;
      bool flag = this.m_year != null && this.m_year.ConfigEnabled && dataSource != null;
      this.dgStrata.ReadOnly = !flag;
      this.dgStrata.AllowUserToAddRows = flag;
      this.dgStrata.AllowUserToDeleteRows = flag;
      this.dcExistingPlots.ReadOnly = true;
      if (dataSource != null)
      {
        int num1 = 0;
        float num2 = 0.0f;
        foreach (PlotStrata plotStrata in (Collection<PlotStrata>) dataSource)
        {
          num1 += plotStrata.TotalPlots + plotStrata.PlotsToAdd;
          if ((double) plotStrata.Size > 0.0)
            num2 += plotStrata.Size;
        }
        this.lblPlots.Text = num1.ToString("N0");
        this.lblSize.Text = num2.ToString("N2");
      }
      base.OnRequestRefresh();
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
        if (column != this.dcSize)
          return;
        if ((double) plotStrata.Size <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) this.dcSize.HeaderText, (object) 0);
        }
        else
        {
          if (this.m_series == null || !this.m_series.IsSample)
            return;
          float num1 = 0.0f;
          foreach (Plot plot in (IEnumerable<Plot>) plotStrata.Strata.Plots)
            num1 += plot.Size * (float) plot.PercentMeasured;
          float num2 = num1 / 100f;
          if ((double) num2 <= (double) plotStrata.Size)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrStrataArea, (object) num2);
        }
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.ValidateChildren())
        return;
      DataBindingList<PlotStrata> dataSource = this.dgStrata.DataSource as DataBindingList<PlotStrata>;
      using (ITransaction transaction = this.m_session.BeginTransaction())
      {
        foreach (PlotStrata plotStrata in (Collection<PlotStrata>) dataSource)
          this.m_session.SaveOrUpdate((object) plotStrata.Strata);
        transaction.Commit();
      }
      this.Close();
    }

    private void dgStrata_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (!(this.dgStrata.DataSource is DataBindingList<PlotStrata> dataSource) || e.Row.Index >= dataSource.Count)
        return;
      PlotStrata plotStrata = dataSource[e.Row.Index];
      if (!(dataSource.Count > 1 & plotStrata.TotalPlots > 0))
        return;
      using (DeleteStrataForm deleteStrataForm = new DeleteStrataForm(plotStrata.Strata, this.m_series.IsSample))
      {
        DialogResult dialogResult = deleteStrataForm.ShowDialog((IWin32Window) this);
        e.Cancel = dialogResult == DialogResult.Cancel;
      }
    }

    private void StrataForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!(this.dgStrata.DataSource is DataBindingList<PlotStrata> dataSource) || dataSource.Count != 0)
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrOneRequired, (object) v6Strings.Strata_SingularName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (StrataForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
      this.dgStrata = new DataGridView();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcDescription = new DataGridViewTextBoxColumn();
      this.dcAbbreviation = new DataGridViewTextBoxColumn();
      this.dcSize = new DataGridViewNumericTextBoxColumn();
      this.dcExistingPlots = new DataGridViewNumericTextBoxColumn();
      this.pnlSampleInfo = new FlowLayoutPanel();
      this.lblTotalSize = new Label();
      this.lblSize = new Label();
      this.lblTotalPlots = new Label();
      this.lblPlots = new Label();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.lblStrata = new Label();
      this.lblNote = new Label();
      this.btnOK = new Button();
      this.dataGridViewNumericTextBoxColumn1 = new DataGridViewNumericTextBoxColumn();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewNumericTextBoxColumn2 = new DataGridViewNumericTextBoxColumn();
      this.dataGridViewNumericTextBoxColumn3 = new DataGridViewNumericTextBoxColumn();
      this.dataGridViewNumericTextBoxColumn4 = new DataGridViewNumericTextBoxColumn();
      ((ISupportInitialize) this.dgStrata).BeginInit();
      this.pnlSampleInfo.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgStrata.AllowUserToDeleteRows = false;
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
      this.dgStrata.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcDescription, (DataGridViewColumn) this.dcAbbreviation, (DataGridViewColumn) this.dcSize, (DataGridViewColumn) this.dcExistingPlots);
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
      this.dcDescription.MaxInputLength = 30;
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
      this.dcSize.Format = "0.#;0.#";
      this.dcSize.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcSize, "dcSize");
      this.dcSize.Name = "dcSize";
      this.dcSize.Signed = false;
      this.dcExistingPlots.DataPropertyName = "TotalPlots";
      this.dcExistingPlots.DecimalPlaces = 0;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle4.BackColor = Color.LightGray;
      gridViewCellStyle4.ForeColor = Color.Black;
      this.dcExistingPlots.DefaultCellStyle = gridViewCellStyle4;
      this.dcExistingPlots.Format = "#;-#";
      this.dcExistingPlots.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcExistingPlots, "dcExistingPlots");
      this.dcExistingPlots.Name = "dcExistingPlots";
      this.dcExistingPlots.ReadOnly = true;
      this.dcExistingPlots.Signed = false;
      componentResourceManager.ApplyResources((object) this.pnlSampleInfo, "pnlSampleInfo");
      this.pnlSampleInfo.Controls.Add((Control) this.lblTotalSize);
      this.pnlSampleInfo.Controls.Add((Control) this.lblSize);
      this.pnlSampleInfo.Controls.Add((Control) this.lblTotalPlots);
      this.pnlSampleInfo.Controls.Add((Control) this.lblPlots);
      this.pnlSampleInfo.Name = "pnlSampleInfo";
      componentResourceManager.ApplyResources((object) this.lblTotalSize, "lblTotalSize");
      this.lblTotalSize.Name = "lblTotalSize";
      componentResourceManager.ApplyResources((object) this.lblSize, "lblSize");
      this.lblSize.Name = "lblSize";
      componentResourceManager.ApplyResources((object) this.lblTotalPlots, "lblTotalPlots");
      this.lblTotalPlots.Name = "lblTotalPlots";
      componentResourceManager.ApplyResources((object) this.lblPlots, "lblPlots");
      this.lblPlots.Name = "lblPlots";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.CausesValidation = false;
      this.tableLayoutPanel1.Controls.Add((Control) this.lblStrata, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblNote, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.pnlSampleInfo, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.btnOK, 1, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.lblStrata, "lblStrata");
      this.lblStrata.Name = "lblStrata";
      componentResourceManager.ApplyResources((object) this.lblNote, "lblNote");
      this.lblNote.Name = "lblNote";
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
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle5.BackColor = Color.LightGray;
      gridViewCellStyle5.ForeColor = Color.Black;
      gridViewCellStyle5.Format = "N2";
      this.dataGridViewNumericTextBoxColumn2.DefaultCellStyle = gridViewCellStyle5;
      this.dataGridViewNumericTextBoxColumn2.Format = "#.#;#.#";
      this.dataGridViewNumericTextBoxColumn2.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dataGridViewNumericTextBoxColumn2, "dataGridViewNumericTextBoxColumn2");
      this.dataGridViewNumericTextBoxColumn2.Name = "dataGridViewNumericTextBoxColumn2";
      this.dataGridViewNumericTextBoxColumn2.ReadOnly = true;
      this.dataGridViewNumericTextBoxColumn2.Signed = false;
      this.dataGridViewNumericTextBoxColumn3.DataPropertyName = "TotalPlots";
      this.dataGridViewNumericTextBoxColumn3.DecimalPlaces = 0;
      gridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle6.BackColor = Color.LightGray;
      gridViewCellStyle6.ForeColor = Color.Black;
      this.dataGridViewNumericTextBoxColumn3.DefaultCellStyle = gridViewCellStyle6;
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
      this.Name = nameof (StrataForm);
      this.ShowHint = DockState.Document;
      this.FormClosing += new FormClosingEventHandler(this.StrataForm_FormClosing);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel1, 0);
      this.Controls.SetChildIndex((Control) this.dgStrata, 0);
      ((ISupportInitialize) this.dgStrata).EndInit();
      this.pnlSampleInfo.ResumeLayout(false);
      this.pnlSampleInfo.PerformLayout();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
