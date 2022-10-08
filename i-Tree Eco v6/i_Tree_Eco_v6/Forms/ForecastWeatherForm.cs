// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ForecastWeatherForm
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
  public class ForecastWeatherForm : ForecastContentForm
  {
    private const Decimal PERCENT = 10.00M;
    private const int START = 1;
    private Dictionary<WeatherEvent, double> dWeatherMortalityRates;
    private DataGridViewManager _gridMgr;
    private TaskManager _taskMgr;
    private readonly object _syncobj;
    private ProgramSession _ps;
    private ISession _session;
    private Forecast _forecast;
    private IList<ForecastWeatherEvent> _events;
    private IContainer components;
    private DataGridView WeatherDGV;
    private ToolStrip OverrideMortalityToolStrip;
    private ToolStripButton UndoToolStripButton;
    private ToolStripButton DeleteToolStripButton;
    private ToolStripButton RedoToolStripButton;
    private Panel ControlsPanel;
    private Panel panel2;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn2;
    private ComboBox WeatherTypeComboBox;
    private NumericUpDown StartNumericUpDown;
    private NumericUpDown MortalityNumericUpDown;
    private Button ClearButton;
    private Button AddButton;
    private Label MortalityLabel;
    private Label StartLabel;
    private Label WeatherTypeLabel;
    private ErrorProvider ep;
    private DataGridViewTextBoxColumn WeatherTypeColumn;
    private DataGridViewNumericTextBoxColumn StartYearColumn;
    private DataGridViewNumericTextBoxColumn MortalityColumn;

    public ForecastWeatherForm()
    {
      this.InitializeComponent();
      this._ps = ProgramSession.GetInstance();
      this._session = this._ps.InputSession.CreateSession();
      this._taskMgr = new TaskManager(new WaitCursor((Form) this));
      this._gridMgr = new DataGridViewManager(this.WeatherDGV);
      this._syncobj = new object();
      EventPublisher.Register<EntityUpdated<Forecast>>(new EventHandler<EntityUpdated<Forecast>>(this.Forecast_Updated));
      Program.Session.InputSession.ForecastChanged += new EventHandler(this.InputSession_ForecastChanged);
      this.dWeatherMortalityRates = new Dictionary<WeatherEvent, double>()
      {
        {
          WeatherEvent.Storm,
          60.0
        },
        {
          WeatherEvent.Class1Huricane,
          10.0
        },
        {
          WeatherEvent.Class2Huricane,
          15.0
        },
        {
          WeatherEvent.Class3Huricane,
          20.0
        },
        {
          WeatherEvent.Class4Huricane,
          25.0
        },
        {
          WeatherEvent.Class5Huricane,
          35.0
        },
        {
          WeatherEvent.TropicalDepression,
          3.0
        },
        {
          WeatherEvent.TropicalStorm,
          5.0
        }
      };
      this.WeatherDGV.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.WeatherDGV.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.WeatherDGV.AutoGenerateColumns = false;
      using (TypeHelper<ForecastWeatherEvent> typeHelper = new TypeHelper<ForecastWeatherEvent>())
      {
        this.WeatherTypeColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<ForecastWeatherEvent, object>>) (we => (object) we.WeatherEvent));
        this.StartYearColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<ForecastWeatherEvent, object>>) (we => (object) we.StartYear));
        this.MortalityColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<ForecastWeatherEvent, object>>) (we => (object) we.MortalityPercent));
      }
      this.Init();
    }

    private void WeatherTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      int selectedIndex = this.WeatherTypeComboBox.SelectedIndex;
      this.AddButton.Enabled = selectedIndex != -1;
      if (selectedIndex == -1)
        return;
      this.MortalityNumericUpDown.Value = (Decimal) this.dWeatherMortalityRates[(WeatherEvent) this.WeatherTypeComboBox.SelectedValue];
    }

    private void ClearButton_Click(object sender, EventArgs e)
    {
      this.MortalityNumericUpDown.Value = 10.00M;
      this.StartNumericUpDown.Value = 1M;
      this.WeatherDGV.CurrentCell = (DataGridViewCell) null;
      this.Clear();
    }

    private void AddButton_Click(object sender, EventArgs e)
    {
      if (!this._forecast.Changed)
      {
        int num1 = (int) MessageBox.Show(string.Format(ForecastRes.EditModeCustomMessage, (object) ForecastRes.WeatherStr), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
      }
      else
      {
        this.WeatherTypeComboBox.SelectedValue.ToString();
        short num2 = (short) this.StartNumericUpDown.Value;
        if ((int) num2 > (int) this._forecast.NumYears)
        {
          int num3 = (int) MessageBox.Show(ForecastRes.TooLongEventError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        }
        else
        {
          if (!(this.WeatherDGV.DataSource is DataBindingList<ForecastWeatherEvent> dataSource))
            return;
          ForecastWeatherEvent weather = dataSource.AddNew();
          if (this._session.QueryOver<ForecastWeatherEvent>().Where((System.Linq.Expressions.Expression<Func<ForecastWeatherEvent, bool>>) (we => we.Forecast.Guid == weather.Forecast.Guid && (int) we.WeatherEvent == (int) weather.WeatherEvent && (int) we.StartYear == (int) weather.StartYear)).RowCount() != 0)
          {
            int num4 = (int) MessageBox.Show(string.Format(ForecastRes.DuplicateWeatherError, (object) this.WeatherTypeComboBox.Text, (object) num2), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            dataSource.CancelNew(dataSource.IndexOf(weather));
          }
          else
          {
            using (ITransaction transaction = this._session.BeginTransaction())
            {
              this._session.Save((object) weather);
              transaction.Commit();
            }
            this.WeatherDGV.CurrentCell = this.WeatherDGV.Rows[this.WeatherDGV.RowCount - 1].Cells[0];
            this.Clear();
          }
        }
      }
    }

    private void WeatherTypeComboBox_Click(object sender, EventArgs e) => (sender as ComboBox).DroppedDown = true;

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

    private void WeatherDGV_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void WeatherDGV_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void WeatherDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
    }

    private void WeatherDGV_CellValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.WeatherDGV.CurrentRow != null && !this.WeatherDGV.IsCurrentRowDirty || !(this.WeatherDGV.DataSource is DataBindingList<ForecastWeatherEvent> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      ForecastWeatherEvent forecastWeatherEvent = dataSource[e.RowIndex];
      try
      {
        using (ITransaction transaction = this._session.BeginTransaction())
        {
          this._session.SaveOrUpdate((object) forecastWeatherEvent);
          transaction.Commit();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private void WeatherDGV_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.WeatherDGV.CurrentRow != null && !this.WeatherDGV.IsCurrentRowDirty || !(this.WeatherDGV.DataSource is DataBindingList<ForecastWeatherEvent> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      ForecastEvent forecastEvent = (ForecastEvent) dataSource[e.RowIndex];
      string text = (string) null;
      if (forecastEvent.MortalityPercent < 0.1 || 99.9 < forecastEvent.MortalityPercent)
        text = ForecastRes.PercentError;
      else if ((int) forecastEvent.StartYear - 1 > (int) this._forecast.NumYears)
        text = ForecastRes.TooLongEventError;
      if (text == null)
        return;
      int num = (int) MessageBox.Show(text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      e.Cancel = true;
    }

    private void WeatherDGV_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (!(this.WeatherDGV.DataSource is DataBindingList<ForecastWeatherEvent> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      using (ITransaction transaction = this._session.BeginTransaction())
      {
        this._session.SaveOrUpdate((object) dataSource[e.RowIndex]);
        transaction.Commit();
      }
      this.OnRequestRefresh();
    }

    private void Weather_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void Weather_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<ForecastWeatherEvent> dataBindingList = sender as DataBindingList<ForecastWeatherEvent>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      ForecastEvent forecastEvent = (ForecastEvent) dataBindingList[e.NewIndex];
      if (forecastEvent.IsTransient)
        return;
      lock (this._syncobj)
      {
        using (ITransaction transaction = this._session.BeginTransaction())
        {
          this._session.Delete((object) forecastEvent);
          transaction.Commit();
        }
      }
    }

    private void Weather_AddingNew(object sender, AddingNewEventArgs e)
    {
      AddingNewEventArgs addingNewEventArgs = e;
      ForecastWeatherEvent forecastWeatherEvent = new ForecastWeatherEvent();
      forecastWeatherEvent.Forecast = this._forecast;
      forecastWeatherEvent.WeatherEvent = (WeatherEvent) this.WeatherTypeComboBox.SelectedValue;
      forecastWeatherEvent.StartYear = (short) this.StartNumericUpDown.Value;
      forecastWeatherEvent.MortalityPercent = (double) this.MortalityNumericUpDown.Value;
      addingNewEventArgs.NewObject = (object) forecastWeatherEvent;
    }

    private void Init()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this._taskMgr.Add(Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this._ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        if (t.IsFaulted)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrUnexpected, (object) t.Exception.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          this.InitUI();
      }), scheduler));
    }

    private void LoadData()
    {
      using (this._session.BeginTransaction())
      {
        Forecast f = this._session.Get<Forecast>((object) this._ps.InputSession.ForecastKey);
        IList<ForecastWeatherEvent> forecastWeatherEventList = this._session.CreateCriteria<ForecastWeatherEvent>().Add(Restrictions.Where<ForecastWeatherEvent>((System.Linq.Expressions.Expression<Func<ForecastWeatherEvent, bool>>) (we => we.Forecast == f))).AddOrder(Order.Asc((IProjection) Projections.Property<ForecastWeatherEvent>((System.Linq.Expressions.Expression<Func<ForecastWeatherEvent, object>>) (we => (object) we.StartYear)))).AddOrder(Order.Asc((IProjection) Projections.Property<ForecastWeatherEvent>((System.Linq.Expressions.Expression<Func<ForecastWeatherEvent, object>>) (we => (object) we.WeatherEvent)))).List<ForecastWeatherEvent>();
        lock (this._syncobj)
        {
          this._forecast = f;
          this._events = forecastWeatherEventList;
        }
      }
    }

    private void InitUI()
    {
      this.WeatherTypeComboBox.BindTo((object) new BindingSource((object) EnumHelper.ConvertToDictionary<WeatherEvent>(), (string) null), "Value", "Key");
      this.StartNumericUpDown.Maximum = (Decimal) this._forecast.NumYears;
      this.MortalityNumericUpDown.Value = 10.00M;
      this.StartNumericUpDown.Value = 1M;
      DataBindingList<ForecastWeatherEvent> dataBindingList = new DataBindingList<ForecastWeatherEvent>(this._events);
      this.WeatherDGV.DataSource = (object) dataBindingList;
      this.WeatherDGV.CurrentCell = (DataGridViewCell) null;
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.Weather_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.Weather_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.Weather_ListChanged);
      this.Clear();
      this.OnRequestRefresh();
    }

    protected override void OnRequestRefresh()
    {
      bool flag1 = this.WeatherDGV.SelectedRows.Count > 0;
      bool flag2 = this._forecast != null && this._forecast.Changed && this.WeatherDGV.DataSource != null;
      bool flag3 = flag2 & flag1 && this._events != null && this._events.Count > 0;
      this.WeatherDGV.AllowUserToDeleteRows = flag3;
      this.WeatherDGV.ReadOnly = !flag2;
      this.WeatherTypeColumn.ReadOnly = true;
      this.StartYearColumn.ReadOnly = true;
      this.DeleteToolStripButton.Enabled = flag3;
      this.UndoToolStripButton.Enabled = flag2 && this._gridMgr.CanUndo;
      this.RedoToolStripButton.Enabled = flag2 && this._gridMgr.CanRedo;
      base.OnRequestRefresh();
    }

    private void Clear()
    {
      this.WeatherTypeLabel.Enabled = true;
      this.WeatherTypeComboBox.SelectedIndex = -1;
      this.WeatherTypeComboBox.Enabled = true;
      this.StartLabel.Enabled = true;
      this.StartNumericUpDown.Enabled = true;
      this.MortalityLabel.Enabled = true;
      this.MortalityNumericUpDown.Enabled = true;
      this.WeatherDGV.Enabled = true;
      this.ClearButton.Enabled = true;
    }

    private void deleteSelectedRows()
    {
      if (!(this.WeatherDGV.DataSource is DataBindingList<ForecastWeatherEvent> dataSource))
        return;
      CurrencyManager currencyManager = this.WeatherDGV.BindingContext[(object) dataSource] as CurrencyManager;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.WeatherDGV.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
          dataSource.RemoveAt(selectedRow.Index);
      }
      if (currencyManager.Position == -1)
        return;
      this.WeatherDGV.Rows[currencyManager.Position].Selected = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ForecastWeatherForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      this.MortalityLabel = new Label();
      this.StartLabel = new Label();
      this.WeatherTypeLabel = new Label();
      this.WeatherDGV = new DataGridView();
      this.WeatherTypeColumn = new DataGridViewTextBoxColumn();
      this.StartYearColumn = new DataGridViewNumericTextBoxColumn();
      this.MortalityColumn = new DataGridViewNumericTextBoxColumn();
      this.OverrideMortalityToolStrip = new ToolStrip();
      this.UndoToolStripButton = new ToolStripButton();
      this.RedoToolStripButton = new ToolStripButton();
      this.DeleteToolStripButton = new ToolStripButton();
      this.ControlsPanel = new Panel();
      this.ClearButton = new Button();
      this.AddButton = new Button();
      this.MortalityNumericUpDown = new NumericUpDown();
      this.StartNumericUpDown = new NumericUpDown();
      this.WeatherTypeComboBox = new ComboBox();
      this.panel2 = new Panel();
      this.dataGridViewNumericTextBoxColumn1 = new DataGridViewNumericTextBoxColumn();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewNumericTextBoxColumn2 = new DataGridViewNumericTextBoxColumn();
      this.ep = new ErrorProvider(this.components);
      ((ISupportInitialize) this.WeatherDGV).BeginInit();
      this.OverrideMortalityToolStrip.SuspendLayout();
      this.ControlsPanel.SuspendLayout();
      this.MortalityNumericUpDown.BeginInit();
      this.StartNumericUpDown.BeginInit();
      this.panel2.SuspendLayout();
      ((ISupportInitialize) this.ep).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.MortalityLabel, "MortalityLabel");
      this.MortalityLabel.Name = "MortalityLabel";
      componentResourceManager.ApplyResources((object) this.StartLabel, "StartLabel");
      this.StartLabel.Name = "StartLabel";
      componentResourceManager.ApplyResources((object) this.WeatherTypeLabel, "WeatherTypeLabel");
      this.WeatherTypeLabel.Name = "WeatherTypeLabel";
      this.WeatherDGV.AllowUserToAddRows = false;
      this.WeatherDGV.AllowUserToResizeRows = false;
      this.WeatherDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.WeatherDGV.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.WeatherDGV.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.WeatherDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.WeatherDGV.Columns.AddRange((DataGridViewColumn) this.WeatherTypeColumn, (DataGridViewColumn) this.StartYearColumn, (DataGridViewColumn) this.MortalityColumn);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.WeatherDGV.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.WeatherDGV, "WeatherDGV");
      this.WeatherDGV.EditMode = DataGridViewEditMode.EditOnEnter;
      this.WeatherDGV.EnableHeadersVisualStyles = false;
      this.WeatherDGV.MultiSelect = false;
      this.WeatherDGV.Name = "WeatherDGV";
      this.WeatherDGV.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.WeatherDGV.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      this.WeatherDGV.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.WeatherDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.WeatherDGV.CellFormatting += new DataGridViewCellFormattingEventHandler(this.WeatherDGV_CellFormatting);
      this.WeatherDGV.CellValidated += new DataGridViewCellEventHandler(this.WeatherDGV_CellValidated);
      this.WeatherDGV.CurrentCellDirtyStateChanged += new EventHandler(this.WeatherDGV_CurrentCellDirtyStateChanged);
      this.WeatherDGV.RowValidated += new DataGridViewCellEventHandler(this.WeatherDGV_RowValidated);
      this.WeatherDGV.RowValidating += new DataGridViewCellCancelEventHandler(this.WeatherDGV_RowValidating);
      this.WeatherDGV.SelectionChanged += new EventHandler(this.WeatherDGV_SelectionChanged);
      componentResourceManager.ApplyResources((object) this.WeatherTypeColumn, "WeatherTypeColumn");
      this.WeatherTypeColumn.Name = "WeatherTypeColumn";
      this.WeatherTypeColumn.ReadOnly = true;
      this.StartYearColumn.DecimalPlaces = 0;
      gridViewCellStyle3.Format = "N0";
      gridViewCellStyle3.NullValue = (object) null;
      this.StartYearColumn.DefaultCellStyle = gridViewCellStyle3;
      this.StartYearColumn.Format = (string) null;
      this.StartYearColumn.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.StartYearColumn, "StartYearColumn");
      this.StartYearColumn.Name = "StartYearColumn";
      this.StartYearColumn.ReadOnly = true;
      this.StartYearColumn.Signed = false;
      this.MortalityColumn.DecimalPlaces = 1;
      gridViewCellStyle4.Format = "N1";
      gridViewCellStyle4.NullValue = (object) null;
      this.MortalityColumn.DefaultCellStyle = gridViewCellStyle4;
      this.MortalityColumn.Format = (string) null;
      this.MortalityColumn.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.MortalityColumn, "MortalityColumn");
      this.MortalityColumn.Name = "MortalityColumn";
      this.MortalityColumn.Signed = false;
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
      this.ControlsPanel.Controls.Add((Control) this.ClearButton);
      this.ControlsPanel.Controls.Add((Control) this.AddButton);
      this.ControlsPanel.Controls.Add((Control) this.MortalityNumericUpDown);
      this.ControlsPanel.Controls.Add((Control) this.StartNumericUpDown);
      this.ControlsPanel.Controls.Add((Control) this.MortalityLabel);
      this.ControlsPanel.Controls.Add((Control) this.StartLabel);
      this.ControlsPanel.Controls.Add((Control) this.WeatherTypeComboBox);
      this.ControlsPanel.Controls.Add((Control) this.WeatherTypeLabel);
      componentResourceManager.ApplyResources((object) this.ControlsPanel, "ControlsPanel");
      this.ControlsPanel.Name = "ControlsPanel";
      componentResourceManager.ApplyResources((object) this.ClearButton, "ClearButton");
      this.ClearButton.Name = "ClearButton";
      this.ClearButton.UseVisualStyleBackColor = true;
      this.ClearButton.Click += new EventHandler(this.ClearButton_Click);
      componentResourceManager.ApplyResources((object) this.AddButton, "AddButton");
      this.AddButton.Name = "AddButton";
      this.AddButton.UseVisualStyleBackColor = true;
      this.AddButton.Click += new EventHandler(this.AddButton_Click);
      componentResourceManager.ApplyResources((object) this.MortalityNumericUpDown, "MortalityNumericUpDown");
      this.MortalityNumericUpDown.DecimalPlaces = 1;
      this.MortalityNumericUpDown.Increment = new Decimal(new int[4]
      {
        1,
        0,
        0,
        65536
      });
      this.MortalityNumericUpDown.Maximum = new Decimal(new int[4]
      {
        999,
        0,
        0,
        65536
      });
      this.MortalityNumericUpDown.Name = "MortalityNumericUpDown";
      this.MortalityNumericUpDown.Value = new Decimal(new int[4]
      {
        10,
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
      componentResourceManager.ApplyResources((object) this.WeatherTypeComboBox, "WeatherTypeComboBox");
      this.WeatherTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.WeatherTypeComboBox.FormattingEnabled = true;
      this.WeatherTypeComboBox.Name = "WeatherTypeComboBox";
      this.WeatherTypeComboBox.SelectedIndexChanged += new EventHandler(this.WeatherTypeComboBox_SelectedIndexChanged);
      this.WeatherTypeComboBox.Click += new EventHandler(this.WeatherTypeComboBox_Click);
      this.panel2.Controls.Add((Control) this.WeatherDGV);
      this.panel2.Controls.Add((Control) this.OverrideMortalityToolStrip);
      componentResourceManager.ApplyResources((object) this.panel2, "panel2");
      this.panel2.Name = "panel2";
      this.dataGridViewNumericTextBoxColumn1.DecimalPlaces = 2;
      this.dataGridViewNumericTextBoxColumn1.Format = (string) null;
      this.dataGridViewNumericTextBoxColumn1.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dataGridViewNumericTextBoxColumn1, "dataGridViewNumericTextBoxColumn1");
      this.dataGridViewNumericTextBoxColumn1.Name = "dataGridViewNumericTextBoxColumn1";
      this.dataGridViewNumericTextBoxColumn1.Resizable = DataGridViewTriState.True;
      this.dataGridViewNumericTextBoxColumn1.Signed = false;
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.Resizable = DataGridViewTriState.True;
      this.dataGridViewNumericTextBoxColumn2.DecimalPlaces = 0;
      this.dataGridViewNumericTextBoxColumn2.Format = (string) null;
      this.dataGridViewNumericTextBoxColumn2.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dataGridViewNumericTextBoxColumn2, "dataGridViewNumericTextBoxColumn2");
      this.dataGridViewNumericTextBoxColumn2.Name = "dataGridViewNumericTextBoxColumn2";
      this.dataGridViewNumericTextBoxColumn2.Resizable = DataGridViewTriState.True;
      this.dataGridViewNumericTextBoxColumn2.Signed = false;
      this.ep.ContainerControl = (ContainerControl) this;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.ControlsPanel);
      this.Name = nameof (ForecastWeatherForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.ControlsPanel, 0);
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      ((ISupportInitialize) this.WeatherDGV).EndInit();
      this.OverrideMortalityToolStrip.ResumeLayout(false);
      this.OverrideMortalityToolStrip.PerformLayout();
      this.ControlsPanel.ResumeLayout(false);
      this.ControlsPanel.PerformLayout();
      this.MortalityNumericUpDown.EndInit();
      this.StartNumericUpDown.EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      ((ISupportInitialize) this.ep).EndInit();
      this.ResumeLayout(false);
    }
  }
}
