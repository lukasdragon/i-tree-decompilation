// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ForecastMortalityForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Views;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Forms.Resources;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
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
  public class ForecastMortalityForm : ForecastContentForm
  {
    private const Decimal PERCENT = 10.00M;
    private ISession _session;
    private TaskManager _taskMgr;
    private DataGridViewManager _gridMgr;
    private BindingSource _genera;
    private BindingList<Condition> _conds;
    private BindingList<Strata> _strata;
    private readonly object _syncobj;
    private ProgramSession _ps;
    private Forecast _forecast;
    private IList<Mortality> _mortalities;
    private IContainer components;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn1;
    private DataGridViewNumericTextBoxColumn dataGridViewNumericTextBoxColumn2;
    private ToolStrip miniToolStrip;
    internal DataGridView MortalitiesDGV;
    private Panel ControlsPanel;
    private Button ClearButton;
    private ComboBox TypeComboBox;
    private Button AddButton;
    private ComboBox ValueComboBox;
    private CheckBox PercentStartingCheckBox;
    private NumericUpDown PercentNumericUpDown;
    private Label TypeLabel;
    private Label PercentStartingLabel;
    private Label PercentLabel;
    private ErrorProvider ep;
    private TableLayoutPanel tableLayoutPanel1;
    private ToolStrip OverrideToolStrip;
    private ToolStripButton UndoToolStripButton;
    private ToolStripButton RedoToolStripButton;
    private ToolStripButton DeleteToolStripButton;
    private DataGridViewTextBoxColumn TypeColumn;
    private DataGridViewTextBoxColumn ValueColumn;
    private DataGridViewNumericTextBoxColumn PercentColumn;
    private DataGridViewCheckBoxColumn PercentStartingColumn;

    public ForecastMortalityForm()
    {
      this.InitializeComponent();
      this._ps = ProgramSession.GetInstance();
      this._session = this._ps.InputSession.CreateSession();
      this._taskMgr = new TaskManager(new WaitCursor((Form) this));
      this._gridMgr = new DataGridViewManager(this.MortalitiesDGV);
      this._syncobj = new object();
      EventPublisher.Register<EntityUpdated<Forecast>>(new EventHandler<EntityUpdated<Forecast>>(this.Forecast_Updated));
      Program.Session.InputSession.ForecastChanged += new EventHandler(this.InputSession_ForecastChanged);
      this.MortalitiesDGV.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.MortalitiesDGV.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.MortalitiesDGV.AutoGenerateColumns = false;
      using (TypeHelper<Mortality> typeHelper = new TypeHelper<Mortality>())
      {
        this.TypeColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Mortality, object>>) (m => m.Type));
        this.ValueColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Mortality, object>>) (m => m.Value));
        this.PercentColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Mortality, object>>) (m => (object) m.Percent));
        this.PercentStartingColumn.DataPropertyName = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Mortality, object>>) (m => (object) m.IsPercentStarting));
      }
      this.Init();
    }

    private void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.ValueComboBox.Enabled = true;
      string selectedItem = this.TypeComboBox.SelectedItem as string;
      this.ValueComboBox.DataSource = (object) null;
      if (selectedItem == ForecastRes.GenusStr)
      {
        this.ValueComboBox.BindTo<Species>((object) this._genera, (System.Linq.Expressions.Expression<Func<Species, object>>) (s => s.Code), (System.Linq.Expressions.Expression<Func<Species, object>>) (s => s.Code));
        this.ValueComboBox.SelectedIndex = -1;
        this.ValueComboBox.Text = ForecastRes.SelectStr + " " + ForecastRes.GenusStr;
      }
      else if (selectedItem == ForecastRes.ConditionStr)
      {
        this.ValueComboBox.BindTo<Condition>((object) this._conds, (System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Description), (System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Description));
        this.ValueComboBox.SelectedIndex = -1;
        this.ValueComboBox.Text = ForecastRes.SelectStr + " " + ForecastRes.ConditionStr;
      }
      else
      {
        if (!(selectedItem == ForecastRes.StratumStr))
          return;
        this.ValueComboBox.BindTo<Strata>((object) this._strata, (System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Description), (System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Description));
        this.ValueComboBox.SelectedIndex = -1;
        this.ValueComboBox.Text = ForecastRes.SelectStr + " " + ForecastRes.StratumStr;
      }
    }

    private void ValueComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      string text = this.ValueComboBox.Text;
      this.AddButton.Enabled = text != "" && text != ForecastRes.GenusStr && text != ForecastRes.ConditionStr && text != ForecastRes.StratumStr;
    }

    private void ValueComboBox_Format(object sender, ListControlConvertEventArgs e)
    {
      if (!(sender is ComboBox comboBox) || comboBox.Name != this.ValueComboBox.Name || this.TypeComboBox.SelectedItem.ToString() != ForecastRes.GenusStr)
        return;
      Species listItem = e.ListItem as Species;
      e.Value = (object) string.Format("{0} ({1})", (object) listItem.CommonName, (object) listItem.ScientificName);
    }

    private void AddButton_Click(object sender, EventArgs e)
    {
      if (!this._forecast.Changed)
      {
        int num1 = (int) MessageBox.Show(string.Format(ForecastRes.EditModeCustomMessage, (object) ForecastRes.CustomMortalitiesStr), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
      }
      else
      {
        string text = this.TypeComboBox.Text;
        string str1 = this.ValueComboBox.SelectedValue.ToString();
        int num2 = this.PercentStartingCheckBox.Checked ? 1 : 0;
        if (!(this.MortalitiesDGV.DataSource is DataBindingList<Mortality> dataSource))
          return;
        Mortality mort = dataSource.AddNew();
        if (this._session.QueryOver<Mortality>().Where((System.Linq.Expressions.Expression<Func<Mortality, bool>>) (m => m.Forecast.Guid == mort.Forecast.Guid && m.Type == mort.Type && m.Value == mort.Value)).RowCount() != 0)
        {
          string str2 = str1;
          if (text == ForecastRes.GenusStr)
          {
            Species selectedItem = this.ValueComboBox.SelectedItem as Species;
            str2 = string.Format("{0} ({1})", (object) selectedItem.CommonName, (object) selectedItem.ScientificName);
          }
          int num3 = (int) MessageBox.Show(string.Format(ForecastRes.DuplicateMortalityError, (object) text, (object) str2), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
          dataSource.CancelNew(dataSource.IndexOf(mort));
        }
        else
        {
          using (ITransaction transaction = this._session.BeginTransaction())
          {
            this._session.Save((object) mort);
            transaction.Commit();
          }
          this.MortalitiesDGV.CurrentCell = this.MortalitiesDGV.Rows[this.MortalitiesDGV.RowCount - 1].Cells[0];
          this.Clear();
        }
      }
    }

    private void ClearButton_Click(object sender, EventArgs e)
    {
      this.PercentNumericUpDown.Value = 10.00M;
      this.MortalitiesDGV.CurrentCell = (DataGridViewCell) null;
      this.Clear();
    }

    private void TypeComboBox_Click(object sender, EventArgs e) => (sender as ComboBox).DroppedDown = true;

    private void ValueComboBox_Click(object sender, EventArgs e) => (sender as ComboBox).DroppedDown = true;

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

    private void MortalitiesDGV_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void MortalitiesDGV_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void MortalitiesDGV_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.ColumnIndex != this.ValueColumn.Index || !(this.MortalitiesDGV.Rows[e.RowIndex].Cells[this.TypeColumn.Index].Value as string == ForecastRes.GenusStr) || this._genera == null)
        return;
      string spCode = this.MortalitiesDGV.Rows[e.RowIndex].Cells[this.ValueColumn.Index].Value as string;
      if (string.IsNullOrEmpty(spCode) || Enumerable.Cast<Species>(this._genera.List).SingleOrDefault<Species>((Func<Species, bool>) (g => g.Code == spCode)) != null)
        return;
      SpeciesView speciesView;
      if (this._ps.InvalidSpecies.TryGetValue(spCode, out speciesView))
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrSpeciesInvalid, (object) speciesView.CommonScientificName);
      else
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrSpeciesCode, (object) spCode);
    }

    private void MortalitiesDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (e.ColumnIndex != this.ValueColumn.Index || !(this.MortalitiesDGV.Rows[e.RowIndex].Cells[this.TypeColumn.Index].Value as string == ForecastRes.GenusStr))
        return;
      if (this._genera == null)
        return;
      string spCode = this.MortalitiesDGV.Rows[e.RowIndex].Cells[this.ValueColumn.Index].Value as string;
      Species species = Enumerable.Cast<Species>(this._genera.List).SingleOrDefault<Species>((Func<Species, bool>) (g => g.Code == spCode));
      if (species != null)
        e.Value = (object) string.Format("{0} ({1})", (object) species.CommonName, (object) species.ScientificName);
      else
        e.Value = (object) string.Empty;
      e.FormattingApplied = true;
    }

    private void MortalitiesDGV_CellValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.MortalitiesDGV.CurrentRow != null && !this.MortalitiesDGV.IsCurrentRowDirty || !(this.MortalitiesDGV.DataSource is DataBindingList<Mortality> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      Mortality mortality = dataSource[e.RowIndex];
      try
      {
        using (ITransaction transaction = this._session.BeginTransaction())
        {
          this._session.SaveOrUpdate((object) mortality);
          transaction.Commit();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private void MortalitiesDGV_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.MortalitiesDGV.CurrentRow != null && !this.MortalitiesDGV.IsCurrentRowDirty || !(this.MortalitiesDGV.DataSource is DataBindingList<Mortality> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      Mortality mortality = dataSource[e.RowIndex];
      string text = (string) null;
      if (mortality.Percent < 0.1 || 99.9 < mortality.Percent)
        text = ForecastRes.PercentError;
      if (text == null)
        return;
      int num = (int) MessageBox.Show(text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      e.Cancel = true;
    }

    private void MortalitiesDGV_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (!(this.MortalitiesDGV.DataSource is DataBindingList<Mortality> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      using (ITransaction transaction = this._session.BeginTransaction())
      {
        this._session.SaveOrUpdate((object) dataSource[e.RowIndex]);
        transaction.Commit();
      }
      this.OnRequestRefresh();
    }

    private void Mortalities_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void Mortalities_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<Mortality> dataBindingList = sender as DataBindingList<Mortality>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      Mortality mortality = dataBindingList[e.NewIndex];
      if (mortality.IsTransient)
        return;
      lock (this._syncobj)
      {
        using (ITransaction transaction = this._session.BeginTransaction())
        {
          this._session.Delete((object) mortality);
          transaction.Commit();
        }
      }
    }

    private void Mortalities_AddingNew(object sender, AddingNewEventArgs e) => e.NewObject = (object) new Mortality()
    {
      Forecast = this._forecast,
      Type = this.TypeComboBox.Text,
      Value = this.ValueComboBox.SelectedValue.ToString(),
      Percent = (double) this.PercentNumericUpDown.Value,
      IsPercentStarting = this.PercentStartingCheckBox.Checked
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
        Forecast forecast = this._session.Get<Forecast>((object) this._ps.InputSession.ForecastKey);
        IList<Mortality> mortalityList = this._session.CreateCriteria<Mortality>().Add((ICriterion) Restrictions.Eq("Forecast", (object) forecast)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("Type", (object) ForecastRes.BaseStr))).List<Mortality>();
        string[] spCodes = this._session.QueryOver<Tree>().Select(Projections.Distinct((IProjection) Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => tr.Species)))).Inner.JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (tr => tr.Plot)).Inner.JoinQueryOver<Year>((System.Linq.Expressions.Expression<Func<Plot, Year>>) (p => p.Year)).Where((System.Linq.Expressions.Expression<Func<Year, bool>>) (y => (Guid?) y.Guid == Program.Session.InputSession.YearKey)).List<string>().ToArray<string>();
        IList<Species> list1 = RetryExecutionHandler.Execute<IList<Species>>((Func<IList<Species>>) (() =>
        {
          using (ISession session = this._ps.LocSp.OpenSession())
          {
            IEnumerable<Species> source1 = this.listTaxa(session, SpeciesRank.Species, spCodes);
            IEnumerable<Species> first1 = this.listTaxa(session, SpeciesRank.Subgenus, spCodes);
            IEnumerable<Species> first2 = this.listTaxa(session, SpeciesRank.Genus, spCodes);
            IEnumerable<Species> second1 = source1.Select<Species, Species>((Func<Species, Species>) (s => s.Parent)).Where<Species>((Func<Species, bool>) (p => p.Rank == SpeciesRank.Subgenus));
            IEnumerable<Species> source2 = first1.Union<Species>(second1);
            IEnumerable<Species> second2 = source1.Select<Species, Species>((Func<Species, Species>) (s => s.Parent)).Where<Species>((Func<Species, bool>) (p => p.Rank == SpeciesRank.Genus));
            IEnumerable<Species> second3 = source2.Select<Species, Species>((Func<Species, Species>) (s => s.Parent)).Where<Species>((Func<Species, bool>) (p => p.Rank == SpeciesRank.Genus));
            return (IList<Species>) first2.Union<Species>(second2).Union<Species>(second3).ToList<Species>();
          }
        }));
        IList<Condition> list2 = this._session.CreateCriteria<Condition>().Add((ICriterion) Restrictions.Eq("Year.Guid", (object) this._ps.InputSession.YearKey)).AddOrder(Order.Asc("PctDieback")).List<Condition>();
        IList<Strata> list3 = this._session.CreateCriteria<Strata>().Add((ICriterion) Restrictions.Eq("Year.Guid", (object) this._ps.InputSession.YearKey)).AddOrder(Order.Asc("Description")).List<Strata>();
        lock (this._syncobj)
        {
          this._forecast = forecast;
          this._mortalities = mortalityList;
          DataBindingList<Species> dataSource = new DataBindingList<Species>(list1);
          dataSource.AllowNew = false;
          dataSource.AllowEdit = false;
          dataSource.AllowRemove = false;
          dataSource.Sortable = true;
          this._genera = new BindingSource((object) dataSource, (string) null);
          this._genera.Sort = TypeHelper.NameOf<Species>((System.Linq.Expressions.Expression<Func<Species, object>>) (s => s.CommonName));
          DataBindingList<Condition> dataBindingList1 = new DataBindingList<Condition>(list2);
          dataBindingList1.AllowNew = false;
          dataBindingList1.AllowEdit = false;
          dataBindingList1.AllowRemove = false;
          this._conds = (BindingList<Condition>) dataBindingList1;
          DataBindingList<Strata> dataBindingList2 = new DataBindingList<Strata>(list3);
          dataBindingList2.AllowNew = false;
          dataBindingList2.AllowEdit = false;
          dataBindingList2.AllowRemove = false;
          this._strata = (BindingList<Strata>) dataBindingList2;
        }
      }
    }

    private IEnumerable<Species> listTaxa(
      ISession session,
      SpeciesRank rank,
      string[] spCodes)
    {
      return (IEnumerable<Species>) session.QueryOver<Species>().Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => (int) s.Rank == (int) rank)).WhereRestrictionOn((System.Linq.Expressions.Expression<Func<Species, object>>) (s => s.Code)).IsIn((object[]) spCodes).Cacheable().List();
    }

    private void InitUI()
    {
      DataBindingList<Mortality> dataBindingList = new DataBindingList<Mortality>(this._mortalities);
      this.MortalitiesDGV.DataSource = (object) dataBindingList;
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.Mortalities_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.Mortalities_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.Mortalities_ListChanged);
      this.MortalitiesDGV.CurrentCell = (DataGridViewCell) null;
      this.TypeComboBox.Items.Add((object) ForecastRes.GenusStr);
      this.TypeComboBox.Items.Add((object) ForecastRes.ConditionStr);
      this.TypeComboBox.Items.Add((object) ForecastRes.StratumStr);
      this.PercentNumericUpDown.Value = 10.00M;
      this.Clear();
      this.OnRequestRefresh();
    }

    protected override void OnRequestRefresh()
    {
      bool flag1 = this.MortalitiesDGV.SelectedRows.Count > 0;
      bool flag2 = this._forecast != null && this._forecast.Changed && this.MortalitiesDGV.DataSource != null;
      bool flag3 = flag2 & flag1 && this._mortalities != null && this._mortalities.Count > 0;
      this.MortalitiesDGV.AllowUserToDeleteRows = flag3;
      this.MortalitiesDGV.ReadOnly = !flag2;
      this.TypeColumn.ReadOnly = true;
      this.ValueColumn.ReadOnly = true;
      this.DeleteToolStripButton.Enabled = flag3;
      this.UndoToolStripButton.Enabled = flag2 && this._gridMgr.CanUndo;
      this.RedoToolStripButton.Enabled = flag2 && this._gridMgr.CanRedo;
      base.OnRequestRefresh();
    }

    private void deleteSelectedRows()
    {
      if (!(this.MortalitiesDGV.DataSource is DataBindingList<Mortality> dataSource))
        return;
      CurrencyManager currencyManager = this.MortalitiesDGV.BindingContext[(object) dataSource] as CurrencyManager;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.MortalitiesDGV.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
          dataSource.RemoveAt(selectedRow.Index);
      }
      if (currencyManager.Position == -1)
        return;
      this.MortalitiesDGV.Rows[currencyManager.Position].Selected = true;
    }

    private void Clear()
    {
      this.TypeLabel.Enabled = true;
      this.TypeComboBox.Enabled = true;
      this.TypeComboBox.SelectedIndex = -1;
      this.ValueComboBox.DataSource = (object) null;
      this.ValueComboBox.Enabled = false;
      this.ValueComboBox.Text = "";
      this.PercentLabel.Enabled = true;
      this.PercentNumericUpDown.Enabled = true;
      this.PercentStartingLabel.Enabled = true;
      this.PercentStartingCheckBox.Checked = false;
      this.PercentStartingCheckBox.Enabled = true;
      this.MortalitiesDGV.Enabled = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ForecastMortalityForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      this.TypeLabel = new Label();
      this.PercentStartingLabel = new Label();
      this.PercentLabel = new Label();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewNumericTextBoxColumn1 = new DataGridViewNumericTextBoxColumn();
      this.dataGridViewNumericTextBoxColumn2 = new DataGridViewNumericTextBoxColumn();
      this.miniToolStrip = new ToolStrip();
      this.MortalitiesDGV = new DataGridView();
      this.TypeColumn = new DataGridViewTextBoxColumn();
      this.ValueColumn = new DataGridViewTextBoxColumn();
      this.PercentColumn = new DataGridViewNumericTextBoxColumn();
      this.PercentStartingColumn = new DataGridViewCheckBoxColumn();
      this.ControlsPanel = new Panel();
      this.ClearButton = new Button();
      this.TypeComboBox = new ComboBox();
      this.AddButton = new Button();
      this.ValueComboBox = new ComboBox();
      this.PercentStartingCheckBox = new CheckBox();
      this.PercentNumericUpDown = new NumericUpDown();
      this.ep = new ErrorProvider(this.components);
      this.OverrideToolStrip = new ToolStrip();
      this.UndoToolStripButton = new ToolStripButton();
      this.RedoToolStripButton = new ToolStripButton();
      this.DeleteToolStripButton = new ToolStripButton();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      ((ISupportInitialize) this.MortalitiesDGV).BeginInit();
      this.ControlsPanel.SuspendLayout();
      this.PercentNumericUpDown.BeginInit();
      ((ISupportInitialize) this.ep).BeginInit();
      this.OverrideToolStrip.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.TypeLabel, "TypeLabel");
      this.TypeLabel.Name = "TypeLabel";
      componentResourceManager.ApplyResources((object) this.PercentStartingLabel, "PercentStartingLabel");
      this.PercentStartingLabel.Name = "PercentStartingLabel";
      componentResourceManager.ApplyResources((object) this.PercentLabel, "PercentLabel");
      this.PercentLabel.Name = "PercentLabel";
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Resizable = DataGridViewTriState.True;
      this.dataGridViewTextBoxColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      this.dataGridViewTextBoxColumn2.Resizable = DataGridViewTriState.True;
      this.dataGridViewNumericTextBoxColumn1.DecimalPlaces = 2;
      this.dataGridViewNumericTextBoxColumn1.Format = (string) null;
      this.dataGridViewNumericTextBoxColumn1.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dataGridViewNumericTextBoxColumn1, "dataGridViewNumericTextBoxColumn1");
      this.dataGridViewNumericTextBoxColumn1.Name = "dataGridViewNumericTextBoxColumn1";
      this.dataGridViewNumericTextBoxColumn1.Resizable = DataGridViewTriState.True;
      this.dataGridViewNumericTextBoxColumn1.Signed = false;
      this.dataGridViewNumericTextBoxColumn2.DecimalPlaces = 2;
      this.dataGridViewNumericTextBoxColumn2.Format = (string) null;
      this.dataGridViewNumericTextBoxColumn2.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dataGridViewNumericTextBoxColumn2, "dataGridViewNumericTextBoxColumn2");
      this.dataGridViewNumericTextBoxColumn2.Name = "dataGridViewNumericTextBoxColumn2";
      this.dataGridViewNumericTextBoxColumn2.Resizable = DataGridViewTriState.True;
      this.dataGridViewNumericTextBoxColumn2.Signed = false;
      componentResourceManager.ApplyResources((object) this.miniToolStrip, "miniToolStrip");
      this.miniToolStrip.CanOverflow = false;
      this.miniToolStrip.GripStyle = ToolStripGripStyle.Hidden;
      this.miniToolStrip.Name = "miniToolStrip";
      this.MortalitiesDGV.AllowUserToAddRows = false;
      this.MortalitiesDGV.AllowUserToResizeRows = false;
      this.MortalitiesDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.MortalitiesDGV.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.MortalitiesDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.MortalitiesDGV.Columns.AddRange((DataGridViewColumn) this.TypeColumn, (DataGridViewColumn) this.ValueColumn, (DataGridViewColumn) this.PercentColumn, (DataGridViewColumn) this.PercentStartingColumn);
      componentResourceManager.ApplyResources((object) this.MortalitiesDGV, "MortalitiesDGV");
      this.MortalitiesDGV.EditMode = DataGridViewEditMode.EditOnEnter;
      this.MortalitiesDGV.EnableHeadersVisualStyles = false;
      this.MortalitiesDGV.MultiSelect = false;
      this.MortalitiesDGV.Name = "MortalitiesDGV";
      this.MortalitiesDGV.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.MortalitiesDGV.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      this.MortalitiesDGV.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MortalitiesDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.MortalitiesDGV.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.MortalitiesDGV_CellErrorTextNeeded);
      this.MortalitiesDGV.CellFormatting += new DataGridViewCellFormattingEventHandler(this.MortalitiesDGV_CellFormatting);
      this.MortalitiesDGV.CellValidated += new DataGridViewCellEventHandler(this.MortalitiesDGV_CellValidated);
      this.MortalitiesDGV.CurrentCellDirtyStateChanged += new EventHandler(this.MortalitiesDGV_CurrentCellDirtyStateChanged);
      this.MortalitiesDGV.RowValidated += new DataGridViewCellEventHandler(this.MortalitiesDGV_RowValidated);
      this.MortalitiesDGV.RowValidating += new DataGridViewCellCancelEventHandler(this.MortalitiesDGV_RowValidating);
      this.MortalitiesDGV.SelectionChanged += new EventHandler(this.MortalitiesDGV_SelectionChanged);
      this.TypeColumn.FillWeight = 80f;
      componentResourceManager.ApplyResources((object) this.TypeColumn, "TypeColumn");
      this.TypeColumn.Name = "TypeColumn";
      this.TypeColumn.ReadOnly = true;
      this.TypeColumn.Resizable = DataGridViewTriState.True;
      this.ValueColumn.FillWeight = 140f;
      componentResourceManager.ApplyResources((object) this.ValueColumn, "ValueColumn");
      this.ValueColumn.Name = "ValueColumn";
      this.ValueColumn.ReadOnly = true;
      this.ValueColumn.Resizable = DataGridViewTriState.True;
      this.PercentColumn.DecimalPlaces = 1;
      gridViewCellStyle1.Format = "N1";
      gridViewCellStyle1.NullValue = (object) null;
      this.PercentColumn.DefaultCellStyle = gridViewCellStyle1;
      this.PercentColumn.FillWeight = 80f;
      this.PercentColumn.Format = (string) null;
      this.PercentColumn.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.PercentColumn, "PercentColumn");
      this.PercentColumn.Name = "PercentColumn";
      this.PercentColumn.Resizable = DataGridViewTriState.True;
      this.PercentColumn.Signed = false;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
      gridViewCellStyle2.NullValue = (object) "False";
      this.PercentStartingColumn.DefaultCellStyle = gridViewCellStyle2;
      this.PercentStartingColumn.FillWeight = 90f;
      componentResourceManager.ApplyResources((object) this.PercentStartingColumn, "PercentStartingColumn");
      this.PercentStartingColumn.Name = "PercentStartingColumn";
      this.PercentStartingColumn.Resizable = DataGridViewTriState.True;
      this.PercentStartingColumn.SortMode = DataGridViewColumnSortMode.Automatic;
      this.ControlsPanel.Controls.Add((Control) this.TypeLabel);
      this.ControlsPanel.Controls.Add((Control) this.ClearButton);
      this.ControlsPanel.Controls.Add((Control) this.TypeComboBox);
      this.ControlsPanel.Controls.Add((Control) this.AddButton);
      this.ControlsPanel.Controls.Add((Control) this.ValueComboBox);
      this.ControlsPanel.Controls.Add((Control) this.PercentStartingLabel);
      this.ControlsPanel.Controls.Add((Control) this.PercentStartingCheckBox);
      this.ControlsPanel.Controls.Add((Control) this.PercentNumericUpDown);
      this.ControlsPanel.Controls.Add((Control) this.PercentLabel);
      componentResourceManager.ApplyResources((object) this.ControlsPanel, "ControlsPanel");
      this.ControlsPanel.Name = "ControlsPanel";
      componentResourceManager.ApplyResources((object) this.ClearButton, "ClearButton");
      this.ClearButton.Name = "ClearButton";
      this.ClearButton.UseVisualStyleBackColor = true;
      this.ClearButton.Click += new EventHandler(this.ClearButton_Click);
      this.TypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.TypeComboBox, "TypeComboBox");
      this.TypeComboBox.FormattingEnabled = true;
      this.TypeComboBox.Name = "TypeComboBox";
      this.TypeComboBox.SelectedIndexChanged += new EventHandler(this.TypeComboBox_SelectedIndexChanged);
      this.TypeComboBox.Click += new EventHandler(this.TypeComboBox_Click);
      componentResourceManager.ApplyResources((object) this.AddButton, "AddButton");
      this.AddButton.Name = "AddButton";
      this.AddButton.UseVisualStyleBackColor = true;
      this.AddButton.Click += new EventHandler(this.AddButton_Click);
      this.ValueComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.ValueComboBox, "ValueComboBox");
      this.ValueComboBox.FormattingEnabled = true;
      this.ValueComboBox.Name = "ValueComboBox";
      this.ValueComboBox.SelectedIndexChanged += new EventHandler(this.ValueComboBox_SelectedIndexChanged);
      this.ValueComboBox.Format += new ListControlConvertEventHandler(this.ValueComboBox_Format);
      this.ValueComboBox.Click += new EventHandler(this.ValueComboBox_Click);
      componentResourceManager.ApplyResources((object) this.PercentStartingCheckBox, "PercentStartingCheckBox");
      this.PercentStartingCheckBox.Name = "PercentStartingCheckBox";
      this.PercentStartingCheckBox.UseVisualStyleBackColor = true;
      this.PercentNumericUpDown.DecimalPlaces = 1;
      componentResourceManager.ApplyResources((object) this.PercentNumericUpDown, "PercentNumericUpDown");
      this.PercentNumericUpDown.Increment = new Decimal(new int[4]
      {
        1,
        0,
        0,
        65536
      });
      this.PercentNumericUpDown.Maximum = new Decimal(new int[4]
      {
        999,
        0,
        0,
        65536
      });
      this.PercentNumericUpDown.Name = "PercentNumericUpDown";
      this.PercentNumericUpDown.Value = new Decimal(new int[4]
      {
        10,
        0,
        0,
        0
      });
      this.ep.ContainerControl = (ContainerControl) this;
      componentResourceManager.ApplyResources((object) this.OverrideToolStrip, "OverrideToolStrip");
      this.OverrideToolStrip.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.UndoToolStripButton,
        (ToolStripItem) this.RedoToolStripButton,
        (ToolStripItem) this.DeleteToolStripButton
      });
      this.OverrideToolStrip.Name = "OverrideToolStrip";
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
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.MortalitiesDGV, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.ControlsPanel, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.OverrideToolStrip, 0, 1);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Name = nameof (ForecastMortalityForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel1, 0);
      ((ISupportInitialize) this.MortalitiesDGV).EndInit();
      this.ControlsPanel.ResumeLayout(false);
      this.ControlsPanel.PerformLayout();
      this.PercentNumericUpDown.EndInit();
      ((ISupportInitialize) this.ep).EndInit();
      this.OverrideToolStrip.ResumeLayout(false);
      this.OverrideToolStrip.PerformLayout();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
