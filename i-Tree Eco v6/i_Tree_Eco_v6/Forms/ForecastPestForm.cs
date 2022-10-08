// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ForecastPestForm
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
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ForecastPestForm : ForecastContentForm
  {
    private const Decimal PERCENT = 10.00M;
    private const int START = 1;
    private ProgramSession _ps;
    private ISession _session;
    private DataBindingList<Pest> _pests;
    private DataGridViewManager _gridMgr;
    private TaskManager _taskMgr;
    private readonly object _syncobj;
    private Forecast _forecast;
    private IList<ForecastPestEvent> _events;
    private IContainer components;
    private ToolStrip OverrideMortalityToolStrip;
    private ToolStripButton UndoToolStripButton;
    private ToolStripButton DeleteToolStripButton;
    private ToolStripButton RedoToolStripButton;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn2;
    private ComboBox PestSpeciesComboBox;
    private NumericUpDown StartNumericUpDown;
    private NumericUpDown MortalityNumericUpDown;
    private Button ClearButton;
    private Button AddButton;
    private NumericUpDown DurationNumericUpDown;
    private CheckBox PlantHostsCheckBox;
    private Label PlantHostsLabel;
    private Label MortalityLabel;
    private Label StartLabel;
    private Label PestSpeciesLabel;
    private Label DurationLabel;
    private ErrorProvider ep;
    private TableLayoutPanel tableLayoutPanel1;
    private DataGridView PestsDGV;
    private DataGridViewTextBoxColumn PestSpeciesColumn;
    private DataGridViewNumericTextBoxColumn StartYearColumn;
    private DataGridViewNumericTextBoxColumn DurationColumn;
    private DataGridViewNumericTextBoxColumn MortalityColumn;
    private DataGridViewCheckBoxColumn PlantHostsColumn;

    public ForecastPestForm()
    {
      this.InitializeComponent();
      this._ps = ProgramSession.GetInstance();
      this._session = this._ps.InputSession.CreateSession();
      this._taskMgr = new TaskManager(new WaitCursor((Form) this));
      this._gridMgr = new DataGridViewManager(this.PestsDGV);
      this._syncobj = new object();
      EventPublisher.Register<EntityUpdated<Forecast>>(new EventHandler<EntityUpdated<Forecast>>(this.Forecast_Updated));
      Program.Session.InputSession.ForecastChanged += new EventHandler(this.InputSession_ForecastChanged);
      this.PestsDGV.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.PestsDGV.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.PestsDGV.AutoGenerateColumns = false;
      using (TypeHelper<ForecastPestEvent> typeHelper = new TypeHelper<ForecastPestEvent>())
      {
        this.PestSpeciesColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<ForecastPestEvent, object>>) (pe => (object) pe.PestId));
        this.StartYearColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<ForecastPestEvent, object>>) (pe => (object) pe.StartYear));
        this.DurationColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<ForecastPestEvent, object>>) (pe => (object) pe.Duration));
        this.MortalityColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<ForecastPestEvent, object>>) (pe => (object) pe.MortalityPercent));
        this.PlantHostsColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<ForecastPestEvent, object>>) (pe => (object) pe.PlantPestHosts));
      }
      this.Init();
    }

    private void PestSpeciesComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      string text = this.PestSpeciesComboBox.Text;
      bool flag = text != "" && text != ForecastRes.PestStr;
      this.AddButton.Enabled = flag;
      if (!flag)
        return;
      Pest selectedItem = this.PestSpeciesComboBox.SelectedItem as Pest;
      double? percentMortality = selectedItem.PercentMortality;
      Decimal num;
      if (!percentMortality.HasValue)
      {
        num = 10.00M;
      }
      else
      {
        percentMortality = selectedItem.PercentMortality;
        num = (Decimal) percentMortality.Value / 15M;
      }
      this.MortalityNumericUpDown.Value = num;
    }

    private void ClearButton_Click(object sender, EventArgs e)
    {
      this.MortalityNumericUpDown.Value = 10.00M;
      this.StartNumericUpDown.Value = 1M;
      this.DurationNumericUpDown.Value = this.DurationNumericUpDown.Maximum;
      this.PestsDGV.CurrentCell = (DataGridViewCell) null;
      this.Clear();
    }

    private void AddButton_Click(object sender, EventArgs e)
    {
      if (!this._forecast.Changed)
      {
        int num1 = (int) MessageBox.Show(string.Format(ForecastRes.EditModeCustomMessage, (object) ForecastRes.PestStr), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
      }
      else
      {
        short num2 = (short) this.StartNumericUpDown.Value;
        short num3 = (short) this.DurationNumericUpDown.Value;
        if ((int) num2 + (int) num3 - 1 > (int) this._forecast.NumYears)
        {
          int num4 = (int) MessageBox.Show(ForecastRes.TooLongEventError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        }
        else
        {
          if (!(this.PestsDGV.DataSource is DataBindingList<ForecastPestEvent> dataSource))
            return;
          ForecastPestEvent pest = dataSource.AddNew();
          if (this._session.QueryOver<ForecastPestEvent>().Where((System.Linq.Expressions.Expression<Func<ForecastPestEvent, bool>>) (pe => pe.Forecast.Guid == pest.Forecast.Guid && pe.PestId == pest.PestId && (int) pe.StartYear == (int) pest.StartYear)).RowCount() != 0)
          {
            Pest selectedItem = this.PestSpeciesComboBox.SelectedItem as Pest;
            int num5 = (int) MessageBox.Show(string.Format(ForecastRes.DuplicatePestError, (object) string.Format("{0} ({1})", (object) selectedItem.CommonName, (object) selectedItem.ScientificName), (object) num2), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            dataSource.CancelNew(dataSource.IndexOf(pest));
          }
          else
          {
            using (ITransaction transaction = this._session.BeginTransaction())
            {
              this._session.Save((object) pest);
              transaction.Commit();
            }
            this.PestsDGV.CurrentCell = this.PestsDGV.Rows[this.PestsDGV.RowCount - 1].Cells[0];
            this.Clear();
          }
        }
      }
    }

    private void PestSpeciesComboBox_Click(object sender, EventArgs e) => (sender as ComboBox).DroppedDown = true;

    private void PestSpeciesComboBox_Format(object sender, ListControlConvertEventArgs e)
    {
      if (sender as ComboBox != this.PestSpeciesComboBox)
        return;
      Pest listItem = e.ListItem as Pest;
      e.Value = (object) string.Format("{0} ({1})", (object) listItem.CommonName, (object) listItem.ScientificName);
    }

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

    private void PestsDGV_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void PestsDGV_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void PestsDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (e.ColumnIndex != this.PestSpeciesColumn.Index)
        return;
      if (this._pests == null)
        return;
      int id = Convert.ToInt32(this.PestsDGV.Rows[e.RowIndex].Cells[this.PestSpeciesColumn.Index].Value);
      Pest pest = this._pests.SingleOrDefault<Pest>((Func<Pest, bool>) (p => p.Id == id));
      e.Value = (object) string.Format("{0} ({1})", (object) pest.CommonName, (object) pest.ScientificName);
      e.FormattingApplied = true;
    }

    private void PestsDGV_CellValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.PestsDGV.CurrentRow != null && !this.PestsDGV.IsCurrentRowDirty || !(this.PestsDGV.DataSource is DataBindingList<ForecastPestEvent> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      ForecastPestEvent forecastPestEvent = dataSource[e.RowIndex];
      forecastPestEvent.Duration = this.adjustedDuration(forecastPestEvent.StartYear, forecastPestEvent.Duration);
      try
      {
        using (ITransaction transaction = this._session.BeginTransaction())
        {
          this._session.SaveOrUpdate((object) forecastPestEvent);
          transaction.Commit();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private void PestsDGV_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.PestsDGV.CurrentRow != null && !this.PestsDGV.IsCurrentRowDirty || !(this.PestsDGV.DataSource is DataBindingList<ForecastPestEvent> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      ForecastPestEvent forecastPestEvent = dataSource[e.RowIndex];
      string text = (string) null;
      if (forecastPestEvent.MortalityPercent < 0.1 || 99.9 < forecastPestEvent.MortalityPercent)
        text = ForecastRes.PercentError;
      else if ((int) forecastPestEvent.StartYear - 1 + (int) forecastPestEvent.Duration > (int) this._forecast.NumYears)
        text = ForecastRes.TooLongEventError;
      if (text == null)
        return;
      int num = (int) MessageBox.Show(text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      e.Cancel = true;
    }

    private void PestsDGV_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (!(this.PestsDGV.DataSource is DataBindingList<ForecastPestEvent> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      using (ITransaction transaction = this._session.BeginTransaction())
      {
        this._session.SaveOrUpdate((object) dataSource[e.RowIndex]);
        transaction.Commit();
      }
      this.OnRequestRefresh();
    }

    private void Pests_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void Pests_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<ForecastPestEvent> dataBindingList = sender as DataBindingList<ForecastPestEvent>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      ForecastPestEvent forecastPestEvent = dataBindingList[e.NewIndex];
      if (forecastPestEvent.IsTransient)
        return;
      lock (this._syncobj)
      {
        using (ITransaction transaction = this._session.BeginTransaction())
        {
          this._session.Delete((object) forecastPestEvent);
          transaction.Commit();
        }
      }
    }

    private void Pests_AddingNew(object sender, AddingNewEventArgs e)
    {
      AddingNewEventArgs addingNewEventArgs = e;
      ForecastPestEvent forecastPestEvent = new ForecastPestEvent();
      forecastPestEvent.Forecast = this._forecast;
      forecastPestEvent.PestId = Convert.ToInt32(this.PestSpeciesComboBox.SelectedValue);
      forecastPestEvent.StartYear = (short) this.StartNumericUpDown.Value;
      forecastPestEvent.Duration = (short) this.DurationNumericUpDown.Value;
      forecastPestEvent.MortalityPercent = (double) this.MortalityNumericUpDown.Value;
      forecastPestEvent.PlantPestHosts = this.PlantHostsCheckBox.Checked;
      addingNewEventArgs.NewObject = (object) forecastPestEvent;
    }

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
        Forecast forecast = this._session.Get<Forecast>((object) this._ps.InputSession.ForecastKey);
        IList<ForecastPestEvent> forecastPestEventList = this._session.CreateCriteria<ForecastPestEvent>().Add((ICriterion) Restrictions.Eq("Forecast", (object) forecast)).AddOrder(Order.Asc("StartYear")).AddOrder(Order.Asc("PestId")).List<ForecastPestEvent>();
        IList<Pest> list = RetryExecutionHandler.Execute<IList<Pest>>((Func<IList<Pest>>) (() =>
        {
          using (ISession session = Program.Session.LocSp.OpenSession())
            return session.QueryOver<Pest>().Inner.JoinQueryOver<ISet<Species>>((System.Linq.Expressions.Expression<Func<Pest, ISet<Species>>>) (p => p.SusceptableSpecies)).TransformUsing(Transformers.DistinctRootEntity).Cacheable().List();
        }));
        lock (this._syncobj)
        {
          this._forecast = forecast;
          this._events = forecastPestEventList;
          this._pests = new DataBindingList<Pest>(list);
        }
      }
    }

    private void InitUI()
    {
      DataBindingList<ForecastPestEvent> dataBindingList = new DataBindingList<ForecastPestEvent>(this._events);
      this.PestsDGV.DataSource = (object) dataBindingList;
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.Pests_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.Pests_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.Pests_ListChanged);
      this.PestsDGV.CurrentCell = (DataGridViewCell) null;
      short numYears = this._forecast.NumYears;
      this.StartNumericUpDown.Maximum = (Decimal) numYears;
      this.DurationNumericUpDown.Maximum = (Decimal) numYears;
      this.MortalityNumericUpDown.Value = 10.00M;
      this.StartNumericUpDown.Value = 1M;
      this.PestSpeciesComboBox.BindTo<Pest>((object) this._pests, (System.Linq.Expressions.Expression<Func<Pest, object>>) null, (System.Linq.Expressions.Expression<Func<Pest, object>>) (p => (object) p.Id));
      this.DurationNumericUpDown.Value = this.DurationNumericUpDown.Maximum;
      this.Clear();
      this.OnRequestRefresh();
    }

    private short adjustedDuration(short start, short duration)
    {
      int num = (int) duration;
      if ((int) start <= (int) this._forecast.NumYears && (int) start - 1 + (int) duration > (int) this._forecast.NumYears)
        num = (int) this._forecast.NumYears - (int) start + 1;
      return (short) num;
    }

    protected override void OnRequestRefresh()
    {
      bool flag1 = this.PestsDGV.SelectedRows.Count > 0;
      bool flag2 = this._forecast != null && this._forecast.Changed && this.PestsDGV.DataSource != null;
      bool flag3 = flag2 & flag1 && this._events != null && this._events.Count > 0;
      this.PestsDGV.AllowUserToDeleteRows = flag3;
      this.PestsDGV.ReadOnly = !flag2;
      this.PestSpeciesColumn.ReadOnly = true;
      this.StartYearColumn.ReadOnly = true;
      this.DeleteToolStripButton.Enabled = flag3;
      this.UndoToolStripButton.Enabled = flag2 && this._gridMgr.CanUndo;
      this.RedoToolStripButton.Enabled = flag2 && this._gridMgr.CanRedo;
      base.OnRequestRefresh();
    }

    private void deleteSelectedRows()
    {
      if (!(this.PestsDGV.DataSource is DataBindingList<ForecastPestEvent> dataSource))
        return;
      CurrencyManager currencyManager = this.PestsDGV.BindingContext[(object) dataSource] as CurrencyManager;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.PestsDGV.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
          dataSource.RemoveAt(selectedRow.Index);
      }
      if (currencyManager.Position == -1)
        return;
      this.PestsDGV.Rows[currencyManager.Position].Selected = true;
    }

    private void Clear()
    {
      this.PestSpeciesLabel.Enabled = true;
      this.PestSpeciesComboBox.SelectedIndex = -1;
      this.PestSpeciesComboBox.Enabled = true;
      this.StartLabel.Enabled = true;
      this.StartNumericUpDown.Enabled = true;
      this.DurationLabel.Enabled = true;
      this.DurationNumericUpDown.Enabled = true;
      this.MortalityLabel.Enabled = true;
      this.MortalityNumericUpDown.Enabled = true;
      this.PlantHostsLabel.Enabled = true;
      this.PlantHostsCheckBox.Enabled = true;
      this.PestsDGV.Enabled = true;
      this.ClearButton.Enabled = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ForecastPestForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
      this.MortalityLabel = new Label();
      this.StartLabel = new Label();
      this.PestSpeciesLabel = new Label();
      this.DurationLabel = new Label();
      this.OverrideMortalityToolStrip = new ToolStrip();
      this.UndoToolStripButton = new ToolStripButton();
      this.RedoToolStripButton = new ToolStripButton();
      this.DeleteToolStripButton = new ToolStripButton();
      this.PlantHostsCheckBox = new CheckBox();
      this.PlantHostsLabel = new Label();
      this.ClearButton = new Button();
      this.AddButton = new Button();
      this.MortalityNumericUpDown = new NumericUpDown();
      this.DurationNumericUpDown = new NumericUpDown();
      this.StartNumericUpDown = new NumericUpDown();
      this.PestSpeciesComboBox = new ComboBox();
      this.dataGridViewNumericTextBoxColumn1 = new DataGridViewNumericTextBoxColumn();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewNumericTextBoxColumn2 = new DataGridViewNumericTextBoxColumn();
      this.ep = new ErrorProvider(this.components);
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.PestsDGV = new DataGridView();
      this.PestSpeciesColumn = new DataGridViewTextBoxColumn();
      this.StartYearColumn = new DataGridViewNumericTextBoxColumn();
      this.DurationColumn = new DataGridViewNumericTextBoxColumn();
      this.MortalityColumn = new DataGridViewNumericTextBoxColumn();
      this.PlantHostsColumn = new DataGridViewCheckBoxColumn();
      this.OverrideMortalityToolStrip.SuspendLayout();
      this.MortalityNumericUpDown.BeginInit();
      this.DurationNumericUpDown.BeginInit();
      this.StartNumericUpDown.BeginInit();
      ((ISupportInitialize) this.ep).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.PestsDGV).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.MortalityLabel, "MortalityLabel");
      this.MortalityLabel.Name = "MortalityLabel";
      componentResourceManager.ApplyResources((object) this.StartLabel, "StartLabel");
      this.StartLabel.Name = "StartLabel";
      componentResourceManager.ApplyResources((object) this.PestSpeciesLabel, "PestSpeciesLabel");
      this.PestSpeciesLabel.Name = "PestSpeciesLabel";
      componentResourceManager.ApplyResources((object) this.DurationLabel, "DurationLabel");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.DurationLabel, 2);
      this.DurationLabel.Name = "DurationLabel";
      this.tableLayoutPanel1.SetColumnSpan((Control) this.OverrideMortalityToolStrip, 6);
      componentResourceManager.ApplyResources((object) this.OverrideMortalityToolStrip, "OverrideMortalityToolStrip");
      this.OverrideMortalityToolStrip.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.UndoToolStripButton,
        (ToolStripItem) this.RedoToolStripButton,
        (ToolStripItem) this.DeleteToolStripButton
      });
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
      componentResourceManager.ApplyResources((object) this.PlantHostsCheckBox, "PlantHostsCheckBox");
      this.PlantHostsCheckBox.Name = "PlantHostsCheckBox";
      this.PlantHostsCheckBox.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.PlantHostsLabel, "PlantHostsLabel");
      this.PlantHostsLabel.Name = "PlantHostsLabel";
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
      componentResourceManager.ApplyResources((object) this.PestSpeciesComboBox, "PestSpeciesComboBox");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.PestSpeciesComboBox, 4);
      this.PestSpeciesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.PestSpeciesComboBox.DropDownWidth = 500;
      this.PestSpeciesComboBox.FormattingEnabled = true;
      this.PestSpeciesComboBox.Name = "PestSpeciesComboBox";
      this.PestSpeciesComboBox.SelectedIndexChanged += new EventHandler(this.PestSpeciesComboBox_SelectedIndexChanged);
      this.PestSpeciesComboBox.Format += new ListControlConvertEventHandler(this.PestSpeciesComboBox_Format);
      this.PestSpeciesComboBox.Click += new EventHandler(this.PestSpeciesComboBox_Click);
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
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.PestSpeciesLabel, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.OverrideMortalityToolStrip, 0, 6);
      this.tableLayoutPanel1.Controls.Add((Control) this.DurationLabel, 2, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.PlantHostsCheckBox, 1, 3);
      this.tableLayoutPanel1.Controls.Add((Control) this.PestSpeciesComboBox, 1, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.MortalityNumericUpDown, 1, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.StartNumericUpDown, 1, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.StartLabel, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.MortalityLabel, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.PlantHostsLabel, 0, 3);
      this.tableLayoutPanel1.Controls.Add((Control) this.DurationNumericUpDown, 4, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.AddButton, 1, 5);
      this.tableLayoutPanel1.Controls.Add((Control) this.ClearButton, 2, 5);
      this.tableLayoutPanel1.Controls.Add((Control) this.PestsDGV, 0, 7);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.PestsDGV.AllowUserToAddRows = false;
      this.PestsDGV.AllowUserToResizeRows = false;
      this.PestsDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.PestsDGV.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.PestsDGV.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.PestsDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.PestsDGV.Columns.AddRange((DataGridViewColumn) this.PestSpeciesColumn, (DataGridViewColumn) this.StartYearColumn, (DataGridViewColumn) this.DurationColumn, (DataGridViewColumn) this.MortalityColumn, (DataGridViewColumn) this.PlantHostsColumn);
      this.tableLayoutPanel1.SetColumnSpan((Control) this.PestsDGV, 6);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.PestsDGV.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.PestsDGV, "PestsDGV");
      this.PestsDGV.EditMode = DataGridViewEditMode.EditOnEnter;
      this.PestsDGV.EnableHeadersVisualStyles = false;
      this.PestsDGV.MultiSelect = false;
      this.PestsDGV.Name = "PestsDGV";
      this.PestsDGV.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = SystemColors.Control;
      gridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.ForeColor = SystemColors.WindowText;
      gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.PestsDGV.RowHeadersDefaultCellStyle = gridViewCellStyle3;
      this.PestsDGV.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      this.PestsDGV.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.PestsDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.PestsDGV.CellFormatting += new DataGridViewCellFormattingEventHandler(this.PestsDGV_CellFormatting);
      this.PestsDGV.CellValidated += new DataGridViewCellEventHandler(this.PestsDGV_CellValidated);
      this.PestsDGV.CurrentCellDirtyStateChanged += new EventHandler(this.PestsDGV_CurrentCellDirtyStateChanged);
      this.PestsDGV.RowValidated += new DataGridViewCellEventHandler(this.PestsDGV_RowValidated);
      this.PestsDGV.RowValidating += new DataGridViewCellCancelEventHandler(this.PestsDGV_RowValidating);
      this.PestsDGV.SelectionChanged += new EventHandler(this.PestsDGV_SelectionChanged);
      this.PestSpeciesColumn.FillWeight = 72f;
      componentResourceManager.ApplyResources((object) this.PestSpeciesColumn, "PestSpeciesColumn");
      this.PestSpeciesColumn.Name = "PestSpeciesColumn";
      this.PestSpeciesColumn.ReadOnly = true;
      this.StartYearColumn.DecimalPlaces = 0;
      gridViewCellStyle4.Format = "N0";
      gridViewCellStyle4.NullValue = (object) null;
      this.StartYearColumn.DefaultCellStyle = gridViewCellStyle4;
      this.StartYearColumn.FillWeight = 35f;
      this.StartYearColumn.Format = (string) null;
      this.StartYearColumn.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.StartYearColumn, "StartYearColumn");
      this.StartYearColumn.Name = "StartYearColumn";
      this.StartYearColumn.ReadOnly = true;
      this.StartYearColumn.Signed = false;
      this.DurationColumn.DecimalPlaces = 0;
      gridViewCellStyle5.Format = "N0";
      gridViewCellStyle5.NullValue = (object) null;
      this.DurationColumn.DefaultCellStyle = gridViewCellStyle5;
      this.DurationColumn.FillWeight = 35f;
      this.DurationColumn.Format = (string) null;
      this.DurationColumn.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.DurationColumn, "DurationColumn");
      this.DurationColumn.Name = "DurationColumn";
      this.DurationColumn.Signed = false;
      this.MortalityColumn.DecimalPlaces = 1;
      gridViewCellStyle6.Format = "N1";
      gridViewCellStyle6.NullValue = (object) null;
      this.MortalityColumn.DefaultCellStyle = gridViewCellStyle6;
      this.MortalityColumn.FillWeight = 50f;
      this.MortalityColumn.Format = (string) null;
      this.MortalityColumn.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.MortalityColumn, "MortalityColumn");
      this.MortalityColumn.Name = "MortalityColumn";
      this.MortalityColumn.Signed = false;
      this.PlantHostsColumn.FillWeight = 42f;
      componentResourceManager.ApplyResources((object) this.PlantHostsColumn, "PlantHostsColumn");
      this.PlantHostsColumn.Name = "PlantHostsColumn";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Name = nameof (ForecastPestForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel1, 0);
      this.OverrideMortalityToolStrip.ResumeLayout(false);
      this.OverrideMortalityToolStrip.PerformLayout();
      this.MortalityNumericUpDown.EndInit();
      this.DurationNumericUpDown.EndInit();
      this.StartNumericUpDown.EndInit();
      ((ISupportInitialize) this.ep).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((ISupportInitialize) this.PestsDGV).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
