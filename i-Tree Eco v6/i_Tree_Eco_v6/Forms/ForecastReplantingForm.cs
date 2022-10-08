// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ForecastReplantingForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Forms.Resources;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ForecastReplantingForm : ForecastContentForm
  {
    private const Decimal DBH = 2.0M;
    private const int TREES = 100;
    private const int MAX_TREES = 999999;
    private const int START = 1;
    private ProgramSession _ps;
    private ISession _session;
    private TaskManager _taskMgr;
    private DataGridViewManager _gridMgr;
    private readonly object _syncObj;
    private Year _year;
    private Forecast _forecast;
    private IList<Replanting> _replantings;
    private IList<Strata> _strata;
    private IContainer components;
    private DataGridView ReplantingDGV;
    private ToolStrip OverrideMortalityToolStrip;
    private ToolStripButton UndoToolStripButton;
    private ToolStripButton DeleteToolStripButton;
    private ToolStripButton RedoToolStripButton;
    private Panel ControlsPanel;
    private Panel panel2;
    private Panel panel3;
    private NumericUpDown StartNumericUpDown;
    private Button AddButton;
    private NumericUpDown DbhNumericUpDown;
    private NumericUpDown TreesNumericUpDown;
    private ComboBox StratumComboBox;
    private Button ClearButton;
    private NumericUpDown DurationNumericUpDown;
    private Label UnitsLabel;
    private Label TreesLabel;
    private Label DbhLabel;
    private Label StrataLabel;
    private Label StartLabel;
    private Label DurationLabel;
    private ErrorProvider ep;
    private DataGridViewTextBoxColumn StrataColumn;
    private DataGridViewNumericTextBoxColumn DbhColumn;
    private DataGridViewNumericTextBoxColumn NumberColumn;
    private DataGridViewNumericTextBoxColumn StartYearColumn;
    private DataGridViewNumericTextBoxColumn DurationColumn;

    public ForecastReplantingForm()
    {
      this.InitializeComponent();
      this._ps = ProgramSession.GetInstance();
      this._session = this._ps.InputSession.CreateSession();
      this._taskMgr = new TaskManager(new WaitCursor((Form) this));
      this._gridMgr = new DataGridViewManager(this.ReplantingDGV);
      this._syncObj = new object();
      EventPublisher.Register<EntityUpdated<Forecast>>(new EventHandler<EntityUpdated<Forecast>>(this.Forecast_Updated));
      Program.Session.InputSession.ForecastChanged += new EventHandler(this.InputSession_ForecastChanged);
      this.ReplantingDGV.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.ReplantingDGV.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.ReplantingDGV.AutoGenerateColumns = false;
      using (TypeHelper<Replanting> typeHelper = new TypeHelper<Replanting>())
      {
        this.StrataColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Replanting, object>>) (r => r.StratumDesc));
        this.DbhColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Replanting, object>>) (r => (object) r.DBH));
        this.NumberColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Replanting, object>>) (r => (object) r.Number));
        this.StartYearColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Replanting, object>>) (r => (object) r.StartYear));
        this.DurationColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Replanting, object>>) (r => (object) r.Duration));
      }
      this.Init();
    }

    private void StratumComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      string text = this.StratumComboBox.Text;
      this.AddButton.Enabled = text != "" && text != ForecastRes.StratumStr;
    }

    private void AddButton_Click(object sender, EventArgs e)
    {
      if (!this._forecast.Changed)
      {
        int num1 = (int) MessageBox.Show(string.Format(ForecastRes.EditModeCustomMessage, (object) ForecastRes.ReplantTreesStr), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
      }
      else
      {
        string text = this.StratumComboBox.Text;
        double num2 = (double) this.DbhNumericUpDown.Value;
        if ((int) (short) this.StartNumericUpDown.Value + (int) (short) this.DurationNumericUpDown.Value - 1 > (int) this._forecast.NumYears)
        {
          int num3 = (int) MessageBox.Show(ForecastRes.TooLongReplantingError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        }
        else
        {
          if (!(this.ReplantingDGV.DataSource is DataBindingList<Replanting> dataSource))
            return;
          using (ITransaction transaction = this._session.BeginTransaction())
          {
            this._session.Save((object) dataSource.AddNew());
            transaction.Commit();
          }
          this.ReplantingDGV.CurrentCell = this.ReplantingDGV.Rows[this.ReplantingDGV.RowCount - 1].Cells[0];
          this.Clear();
        }
      }
    }

    private void ClearButton_Click(object sender, EventArgs e)
    {
      this.DbhNumericUpDown.Value = 2.0M;
      this.TreesNumericUpDown.Value = 100M;
      this.StartNumericUpDown.Value = 1M;
      this.DurationNumericUpDown.Value = this.DurationNumericUpDown.Maximum;
      this.ReplantingDGV.CurrentCell = (DataGridViewCell) null;
      this.Clear();
    }

    private void StratumComboBox_Click(object sender, EventArgs e) => (sender as ComboBox).DroppedDown = true;

    private void StartNumericUpDown_ValueChanged(object sender, EventArgs e) => this.DurationNumericUpDown.Value = (Decimal) this.adjustedDuration((short) this.StartNumericUpDown.Value, (short) this.DurationNumericUpDown.Value);

    private void InputSession_ForecastChanged(object sender, EventArgs e) => this.Init();

    private void Forecast_Updated(object sender, EntityUpdated<Forecast> e)
    {
      if (this._forecast == null || !(this._forecast.Guid == e.Guid))
        return;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      Task.Factory.StartNew((System.Action) (() =>
      {
        using (this._session.BeginTransaction())
          this._session.Refresh((object) this._forecast);
      }), CancellationToken.None, TaskCreationOptions.None, this._ps.Scheduler).ContinueWith((System.Action<Task>) (t => this.OnRequestRefresh()), scheduler);
    }

    private void UndoToolStripButton_Click(object sender, EventArgs e)
    {
      this._gridMgr.Undo();
      this.OnRequestRefresh();
    }

    private void RedoToolStripButton_Click(object sender, EventArgs e)
    {
      this._gridMgr.Redo();
      this.OnRequestRefresh();
    }

    private void DeleteToolStripButton_Click(object sender, EventArgs e) => this.deleteSelectedRows();

    private void ReplantingDGV_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void ReplantingDGV_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void ReplantingDGV_CellValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.ReplantingDGV.CurrentRow != null && !this.ReplantingDGV.IsCurrentRowDirty || !(this.ReplantingDGV.DataSource is DataBindingList<Replanting> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      Replanting replanting = dataSource[e.RowIndex];
      replanting.Duration = this.adjustedDuration(replanting.StartYear, replanting.Duration);
      try
      {
        using (ITransaction transaction = this._session.BeginTransaction())
        {
          this._session.SaveOrUpdate((object) replanting);
          transaction.Commit();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private void ReplantingDGV_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.ReplantingDGV.CurrentRow != null && !this.ReplantingDGV.IsCurrentRowDirty || !(this.ReplantingDGV.DataSource is DataBindingList<Replanting> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      Replanting replanting = dataSource[e.RowIndex];
      string text = (string) null;
      if (replanting.Number < 1 || 999999 < replanting.Number)
        text = string.Format(ForecastRes.TreeNumberError, (object) 999999);
      else if ((int) replanting.StartYear - 1 + (int) replanting.Duration > (int) this._forecast.NumYears)
        text = ForecastRes.TooLongReplantingError;
      if (text == null)
        return;
      int num = (int) MessageBox.Show(text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      e.Cancel = true;
    }

    private void ReplantingDGV_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (!(this.ReplantingDGV.DataSource is DataBindingList<Replanting> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      Replanting replanting = dataSource[e.RowIndex];
      try
      {
        using (ITransaction transaction = this._session.BeginTransaction())
        {
          this._session.SaveOrUpdate((object) replanting);
          transaction.Commit();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      this.OnRequestRefresh();
    }

    private void Replanting_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void Replanting_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<Replanting> dataBindingList = sender as DataBindingList<Replanting>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      Replanting replanting = dataBindingList[e.NewIndex];
      if (replanting.IsTransient)
        return;
      lock (this._syncObj)
      {
        using (ITransaction transaction = this._session.BeginTransaction())
        {
          this._session.Delete((object) replanting);
          transaction.Commit();
        }
      }
    }

    private void Replanting_AddingNew(object sender, AddingNewEventArgs e) => e.NewObject = (object) new Replanting()
    {
      Forecast = this._forecast,
      StratumDesc = this.StratumComboBox.SelectedValue.ToString(),
      DBH = (double) this.DbhNumericUpDown.Value,
      Number = (int) this.TreesNumericUpDown.Value,
      StartYear = (short) this.StartNumericUpDown.Value,
      Duration = (short) this.DurationNumericUpDown.Value
    };

    private void Init()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this._ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        if (t.IsFaulted)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrUnexpected, (object) t.Exception.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          this.InitUI();
      }), scheduler);
    }

    private void LoadData()
    {
      using (this._session.BeginTransaction())
      {
        Year year = this._session.Get<Year>((object) this._ps.InputSession.YearKey);
        Forecast forecast = this._session.Get<Forecast>((object) this._ps.InputSession.ForecastKey);
        IList<Replanting> replantingList = this._session.CreateCriteria<Replanting>().Add((ICriterion) Restrictions.Eq("Forecast", (object) forecast)).AddOrder(Order.Asc("StratumDesc")).AddOrder(Order.Asc("DBH")).List<Replanting>();
        IList<Strata> strataList = this._session.CreateCriteria<Strata>().Add((ICriterion) Restrictions.Eq("Year", (object) year)).AddOrder(Order.Asc("Description")).List<Strata>();
        lock (this._syncObj)
        {
          this._year = year;
          this._forecast = forecast;
          this._replantings = replantingList;
          this._strata = strataList;
        }
      }
    }

    private void InitUI()
    {
      string str = this.unitString();
      short numYears = this._forecast.NumYears;
      this.UnitsLabel.Text = str;
      this.DbhColumn.HeaderText = string.Format("DBH ({0})", (object) str);
      this.StartNumericUpDown.Maximum = (Decimal) numYears;
      this.DurationNumericUpDown.Maximum = (Decimal) numYears;
      this.DbhNumericUpDown.Value = 2.0M;
      this.TreesNumericUpDown.Value = 100M;
      this.StartNumericUpDown.Value = 1M;
      this.DurationNumericUpDown.Value = this.DurationNumericUpDown.Maximum;
      DataBindingList<Replanting> dataBindingList = new DataBindingList<Replanting>(this._replantings);
      this.ReplantingDGV.DataSource = (object) dataBindingList;
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.Replanting_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.Replanting_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.Replanting_ListChanged);
      this.ReplantingDGV.CurrentCell = (DataGridViewCell) null;
      this.StratumComboBox.BindTo<Strata>((object) this._strata, (System.Linq.Expressions.Expression<Func<Strata, object>>) (st => st.Description), (System.Linq.Expressions.Expression<Func<Strata, object>>) (st => st.Description));
      this.Clear();
      this.OnRequestRefresh();
    }

    private string unitString() => this._year.Unit != YearUnit.English ? i_Tree_Eco_v6.Resources.Strings.UnitCentimeters : i_Tree_Eco_v6.Resources.Strings.UnitInches;

    private short adjustedDuration(short start, short duration)
    {
      int num = (int) duration;
      if ((int) start <= (int) this._forecast.NumYears && (int) start - 1 + (int) duration > (int) this._forecast.NumYears)
        num = (int) this._forecast.NumYears - (int) start + 1;
      return (short) num;
    }

    protected override void OnRequestRefresh()
    {
      bool flag1 = this.ReplantingDGV.SelectedRows.Count > 0;
      bool flag2 = this._forecast != null && this._forecast.Changed && this.ReplantingDGV.DataSource != null;
      bool flag3 = flag2 & flag1 && this._replantings != null && this._replantings.Count > 0;
      this.ReplantingDGV.AllowUserToDeleteRows = flag3;
      this.ReplantingDGV.ReadOnly = !flag2;
      this.StrataColumn.ReadOnly = true;
      this.DbhColumn.ReadOnly = true;
      this.DeleteToolStripButton.Enabled = flag3;
      this.UndoToolStripButton.Enabled = flag2 && this._gridMgr.CanUndo;
      this.RedoToolStripButton.Enabled = flag2 && this._gridMgr.CanRedo;
      base.OnRequestRefresh();
    }

    private void deleteSelectedRows()
    {
      if (!(this.ReplantingDGV.DataSource is DataBindingList<Replanting> dataSource))
        return;
      CurrencyManager currencyManager = this.ReplantingDGV.BindingContext[(object) dataSource] as CurrencyManager;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.ReplantingDGV.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
          dataSource.RemoveAt(selectedRow.Index);
      }
      if (currencyManager.Position == -1)
        return;
      this.ReplantingDGV.Rows[currencyManager.Position].Selected = true;
    }

    private void Clear()
    {
      this.StrataLabel.Enabled = true;
      this.StratumComboBox.SelectedIndex = -1;
      this.StratumComboBox.Enabled = true;
      this.DbhLabel.Enabled = true;
      this.DbhNumericUpDown.Enabled = true;
      this.TreesLabel.Enabled = true;
      this.TreesNumericUpDown.Enabled = true;
      this.TreesNumericUpDown.Maximum = 999999M;
      this.StartLabel.Enabled = true;
      this.StartNumericUpDown.Enabled = true;
      this.DurationLabel.Enabled = true;
      this.DurationNumericUpDown.Enabled = true;
      this.ReplantingDGV.Enabled = true;
      this.ClearButton.Enabled = true;
      this.UnitsLabel.Enabled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ForecastReplantingForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
      this.TreesLabel = new Label();
      this.DbhLabel = new Label();
      this.StrataLabel = new Label();
      this.StartLabel = new Label();
      this.DurationLabel = new Label();
      this.ReplantingDGV = new DataGridView();
      this.StrataColumn = new DataGridViewTextBoxColumn();
      this.DbhColumn = new DataGridViewNumericTextBoxColumn();
      this.NumberColumn = new DataGridViewNumericTextBoxColumn();
      this.StartYearColumn = new DataGridViewNumericTextBoxColumn();
      this.DurationColumn = new DataGridViewNumericTextBoxColumn();
      this.OverrideMortalityToolStrip = new ToolStrip();
      this.UndoToolStripButton = new ToolStripButton();
      this.RedoToolStripButton = new ToolStripButton();
      this.DeleteToolStripButton = new ToolStripButton();
      this.ControlsPanel = new Panel();
      this.UnitsLabel = new Label();
      this.DurationNumericUpDown = new NumericUpDown();
      this.StartNumericUpDown = new NumericUpDown();
      this.AddButton = new Button();
      this.DbhNumericUpDown = new NumericUpDown();
      this.TreesNumericUpDown = new NumericUpDown();
      this.StratumComboBox = new ComboBox();
      this.ClearButton = new Button();
      this.panel2 = new Panel();
      this.panel3 = new Panel();
      this.ep = new ErrorProvider(this.components);
      ((ISupportInitialize) this.ReplantingDGV).BeginInit();
      this.OverrideMortalityToolStrip.SuspendLayout();
      this.ControlsPanel.SuspendLayout();
      this.DurationNumericUpDown.BeginInit();
      this.StartNumericUpDown.BeginInit();
      this.DbhNumericUpDown.BeginInit();
      this.TreesNumericUpDown.BeginInit();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      ((ISupportInitialize) this.ep).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.TreesLabel, "TreesLabel");
      this.TreesLabel.Name = "TreesLabel";
      componentResourceManager.ApplyResources((object) this.DbhLabel, "DbhLabel");
      this.DbhLabel.Name = "DbhLabel";
      componentResourceManager.ApplyResources((object) this.StrataLabel, "StrataLabel");
      this.StrataLabel.Name = "StrataLabel";
      componentResourceManager.ApplyResources((object) this.StartLabel, "StartLabel");
      this.StartLabel.Name = "StartLabel";
      componentResourceManager.ApplyResources((object) this.DurationLabel, "DurationLabel");
      this.DurationLabel.Name = "DurationLabel";
      this.ReplantingDGV.AllowUserToAddRows = false;
      this.ReplantingDGV.AllowUserToResizeRows = false;
      this.ReplantingDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.ReplantingDGV.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.ReplantingDGV.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.ReplantingDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ReplantingDGV.Columns.AddRange((DataGridViewColumn) this.StrataColumn, (DataGridViewColumn) this.DbhColumn, (DataGridViewColumn) this.NumberColumn, (DataGridViewColumn) this.StartYearColumn, (DataGridViewColumn) this.DurationColumn);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.ReplantingDGV.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.ReplantingDGV, "ReplantingDGV");
      this.ReplantingDGV.EditMode = DataGridViewEditMode.EditOnEnter;
      this.ReplantingDGV.EnableHeadersVisualStyles = false;
      this.ReplantingDGV.MultiSelect = false;
      this.ReplantingDGV.Name = "ReplantingDGV";
      this.ReplantingDGV.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.ReplantingDGV.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      this.ReplantingDGV.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ReplantingDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.ReplantingDGV.CellValidated += new DataGridViewCellEventHandler(this.ReplantingDGV_CellValidated);
      this.ReplantingDGV.CurrentCellDirtyStateChanged += new EventHandler(this.ReplantingDGV_CurrentCellDirtyStateChanged);
      this.ReplantingDGV.RowValidated += new DataGridViewCellEventHandler(this.ReplantingDGV_RowValidated);
      this.ReplantingDGV.RowValidating += new DataGridViewCellCancelEventHandler(this.ReplantingDGV_RowValidating);
      this.ReplantingDGV.SelectionChanged += new EventHandler(this.ReplantingDGV_SelectionChanged);
      componentResourceManager.ApplyResources((object) this.StrataColumn, "StrataColumn");
      this.StrataColumn.Name = "StrataColumn";
      this.StrataColumn.ReadOnly = true;
      this.StrataColumn.Resizable = DataGridViewTriState.True;
      this.DbhColumn.DecimalPlaces = 2;
      gridViewCellStyle3.Format = "N2";
      gridViewCellStyle3.NullValue = (object) null;
      this.DbhColumn.DefaultCellStyle = gridViewCellStyle3;
      this.DbhColumn.Format = (string) null;
      this.DbhColumn.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.DbhColumn, "DbhColumn");
      this.DbhColumn.Name = "DbhColumn";
      this.DbhColumn.ReadOnly = true;
      this.DbhColumn.Resizable = DataGridViewTriState.True;
      this.DbhColumn.Signed = false;
      this.NumberColumn.DecimalPlaces = 0;
      gridViewCellStyle4.Format = "N0";
      gridViewCellStyle4.NullValue = (object) null;
      this.NumberColumn.DefaultCellStyle = gridViewCellStyle4;
      this.NumberColumn.Format = (string) null;
      this.NumberColumn.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.NumberColumn, "NumberColumn");
      this.NumberColumn.Name = "NumberColumn";
      this.NumberColumn.Resizable = DataGridViewTriState.True;
      this.NumberColumn.Signed = false;
      this.StartYearColumn.DecimalPlaces = 0;
      gridViewCellStyle5.Format = "N0";
      gridViewCellStyle5.NullValue = (object) null;
      this.StartYearColumn.DefaultCellStyle = gridViewCellStyle5;
      this.StartYearColumn.Format = (string) null;
      this.StartYearColumn.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.StartYearColumn, "StartYearColumn");
      this.StartYearColumn.Name = "StartYearColumn";
      this.StartYearColumn.Signed = false;
      this.DurationColumn.DecimalPlaces = 0;
      gridViewCellStyle6.Format = "N0";
      gridViewCellStyle6.NullValue = (object) null;
      this.DurationColumn.DefaultCellStyle = gridViewCellStyle6;
      this.DurationColumn.Format = (string) null;
      this.DurationColumn.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.DurationColumn, "DurationColumn");
      this.DurationColumn.Name = "DurationColumn";
      this.DurationColumn.Signed = false;
      this.OverrideMortalityToolStrip.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.UndoToolStripButton,
        (ToolStripItem) this.RedoToolStripButton,
        (ToolStripItem) this.DeleteToolStripButton
      });
      componentResourceManager.ApplyResources((object) this.OverrideMortalityToolStrip, "OverrideMortalityToolStrip");
      this.OverrideMortalityToolStrip.Name = "OverrideMortalityToolStrip";
      this.UndoToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.UndoToolStripButton, "UndoToolStripButton");
      this.UndoToolStripButton.Name = "UndoToolStripButton";
      this.UndoToolStripButton.Click += new EventHandler(this.UndoToolStripButton_Click);
      this.RedoToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.RedoToolStripButton, "RedoToolStripButton");
      this.RedoToolStripButton.Name = "RedoToolStripButton";
      this.RedoToolStripButton.Click += new EventHandler(this.RedoToolStripButton_Click);
      this.DeleteToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.DeleteToolStripButton, "DeleteToolStripButton");
      this.DeleteToolStripButton.Name = "DeleteToolStripButton";
      this.DeleteToolStripButton.Click += new EventHandler(this.DeleteToolStripButton_Click);
      this.ControlsPanel.Controls.Add((Control) this.UnitsLabel);
      this.ControlsPanel.Controls.Add((Control) this.DurationLabel);
      this.ControlsPanel.Controls.Add((Control) this.DurationNumericUpDown);
      this.ControlsPanel.Controls.Add((Control) this.StartLabel);
      this.ControlsPanel.Controls.Add((Control) this.StartNumericUpDown);
      this.ControlsPanel.Controls.Add((Control) this.AddButton);
      this.ControlsPanel.Controls.Add((Control) this.TreesLabel);
      this.ControlsPanel.Controls.Add((Control) this.DbhNumericUpDown);
      this.ControlsPanel.Controls.Add((Control) this.TreesNumericUpDown);
      this.ControlsPanel.Controls.Add((Control) this.DbhLabel);
      this.ControlsPanel.Controls.Add((Control) this.StratumComboBox);
      this.ControlsPanel.Controls.Add((Control) this.StrataLabel);
      this.ControlsPanel.Controls.Add((Control) this.ClearButton);
      componentResourceManager.ApplyResources((object) this.ControlsPanel, "ControlsPanel");
      this.ControlsPanel.Name = "ControlsPanel";
      componentResourceManager.ApplyResources((object) this.UnitsLabel, "UnitsLabel");
      this.UnitsLabel.Name = "UnitsLabel";
      componentResourceManager.ApplyResources((object) this.DurationNumericUpDown, "DurationNumericUpDown");
      this.DurationNumericUpDown.Maximum = new Decimal(new int[4]
      {
        9999,
        0,
        0,
        131072
      });
      this.DurationNumericUpDown.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.DurationNumericUpDown.Name = "DurationNumericUpDown";
      this.DurationNumericUpDown.Value = new Decimal(new int[4]
      {
        5,
        0,
        0,
        0
      });
      componentResourceManager.ApplyResources((object) this.StartNumericUpDown, "StartNumericUpDown");
      this.StartNumericUpDown.Maximum = new Decimal(new int[4]
      {
        9999,
        0,
        0,
        131072
      });
      this.StartNumericUpDown.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.StartNumericUpDown.Name = "StartNumericUpDown";
      this.StartNumericUpDown.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      componentResourceManager.ApplyResources((object) this.AddButton, "AddButton");
      this.AddButton.Name = "AddButton";
      this.AddButton.UseVisualStyleBackColor = true;
      this.AddButton.Click += new EventHandler(this.AddButton_Click);
      componentResourceManager.ApplyResources((object) this.DbhNumericUpDown, "DbhNumericUpDown");
      this.DbhNumericUpDown.DecimalPlaces = 2;
      this.DbhNumericUpDown.Increment = new Decimal(new int[4]
      {
        1,
        0,
        0,
        65536
      });
      this.DbhNumericUpDown.Maximum = new Decimal(new int[4]
      {
        9999,
        0,
        0,
        131072
      });
      this.DbhNumericUpDown.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        131072
      });
      this.DbhNumericUpDown.Name = "DbhNumericUpDown";
      this.DbhNumericUpDown.Value = new Decimal(new int[4]
      {
        2,
        0,
        0,
        0
      });
      componentResourceManager.ApplyResources((object) this.TreesNumericUpDown, "TreesNumericUpDown");
      this.TreesNumericUpDown.Increment = new Decimal(new int[4]
      {
        10,
        0,
        0,
        0
      });
      this.TreesNumericUpDown.Maximum = new Decimal(new int[4]
      {
        999999,
        0,
        0,
        0
      });
      this.TreesNumericUpDown.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.TreesNumericUpDown.Name = "TreesNumericUpDown";
      this.TreesNumericUpDown.Value = new Decimal(new int[4]
      {
        100,
        0,
        0,
        0
      });
      componentResourceManager.ApplyResources((object) this.StratumComboBox, "StratumComboBox");
      this.StratumComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.StratumComboBox.FormattingEnabled = true;
      this.StratumComboBox.Name = "StratumComboBox";
      this.StratumComboBox.SelectedIndexChanged += new EventHandler(this.StratumComboBox_SelectedIndexChanged);
      this.StratumComboBox.Click += new EventHandler(this.StratumComboBox_Click);
      componentResourceManager.ApplyResources((object) this.ClearButton, "ClearButton");
      this.ClearButton.Name = "ClearButton";
      this.ClearButton.UseVisualStyleBackColor = true;
      this.ClearButton.Click += new EventHandler(this.ClearButton_Click);
      this.panel2.Controls.Add((Control) this.panel3);
      this.panel2.Controls.Add((Control) this.OverrideMortalityToolStrip);
      componentResourceManager.ApplyResources((object) this.panel2, "panel2");
      this.panel2.Name = "panel2";
      this.panel3.Controls.Add((Control) this.ReplantingDGV);
      componentResourceManager.ApplyResources((object) this.panel3, "panel3");
      this.panel3.Name = "panel3";
      this.ep.ContainerControl = (ContainerControl) this;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.ControlsPanel);
      this.Name = nameof (ForecastReplantingForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.ControlsPanel, 0);
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      ((ISupportInitialize) this.ReplantingDGV).EndInit();
      this.OverrideMortalityToolStrip.ResumeLayout(false);
      this.OverrideMortalityToolStrip.PerformLayout();
      this.ControlsPanel.ResumeLayout(false);
      this.ControlsPanel.PerformLayout();
      this.DurationNumericUpDown.EndInit();
      this.StartNumericUpDown.EndInit();
      this.DbhNumericUpDown.EndInit();
      this.TreesNumericUpDown.EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      ((ISupportInitialize) this.ep).EndInit();
      this.ResumeLayout(false);
    }
  }
}
