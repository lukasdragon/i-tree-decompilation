// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ProjectOverviewForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Services;
using i_Tree_Eco_v6.Events;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace i_Tree_Eco_v6.Forms
{
  public class ProjectOverviewForm : ProjectContentForm
  {
    private const int UsLocId = 219;
    private Project m_project;
    private Series m_series;
    private Year m_year;
    private IList<Location> m_nations;
    private ISession m_session;
    private readonly object m_syncobj;
    private ProgramSession m_ps;
    private WaitCursor m_wc;
    private TaskManager m_taskManager;
    private bool m_projectCreated;
    private int m_nationId = -1;
    private int m_stateId = -1;
    private int m_countyId = -1;
    private int m_placeId = -1;
    private YearLocationData m_yearLocData;
    private bool _bindingNation;
    private bool _bindingState;
    private bool _bindingCounty;
    private bool _bindingPlace;
    private bool _formatState;
    private bool _formatCounty;
    private bool _formatPlace;
    private WeatherService _weatherService;
    private SampleType _sampleType;
    private List<ComboBox> m_Comboboxes = new List<ComboBox>();
    private Form currMap;
    private short currPollutionYear;
    private IContainer components;
    private Label lblHeader;
    private TabControl tabControl1;
    private TabPage tpProjectInfo;
    private TabPage tpProjectLocation;
    private TableLayoutPanel tableLayoutPanel1;
    private Label label2;
    private TabPage tpProjectOptions;
    private Label label3;
    private Label label9;
    private Label label7;
    private Label label4;
    private TextBox txtProjectName;
    private TextBox txtSeriesName;
    private Label label5;
    private Label label6;
    private TextBox txtSeriesYear;
    private Label label8;
    private TableLayoutPanel pnlLocation;
    private Label label11;
    private Label lblSelDataYear;
    private Label lblSelWeatherStation;
    private ComboBox cmbPlace;
    private ComboBox cmbCounty;
    private ComboBox cmbState;
    private Label lblNation;
    private Label lblState;
    private Label lblCounty;
    private Label lblPlace;
    private ComboBox cmbNation;
    private Label lblWeatherStation;
    private TextBox txtWeatherStation;
    private Button cmdShowMap;
    private TableLayoutPanel pnlProjectOptions;
    private Label lblAccuracyCaption;
    private Label label21;
    private RadioButton rdoUnitsEnglish;
    private Label label23;
    private Label label24;
    private Label label25;
    private Label label26;
    private Label label27;
    private WrappingCheckBox chkOverheadUtilityConflict;
    private WrappingCheckBox chkMaintTask;
    private WrappingCheckBox chkMaintRec;
    private WrappingCheckBox chkPlotGPS;
    private WrappingCheckBox chkEnergyEffect;
    private WrappingCheckBox chkCrownWidth;
    private WrappingCheckBox chkSpecies;
    private WrappingCheckBox chkDBH;
    private WrappingCheckBox chkHeight;
    private WrappingCheckBox chkCLE;
    private WrappingCheckBox chkLandUseTree;
    private WrappingCheckBox chkTreeStatus;
    private WrappingCheckBox chkIPED;
    private WrappingCheckBox chkShrubs;
    private WrappingCheckBox chkPercentPlantable;
    private WrappingCheckBox chkHydro;
    private WrappingCheckBox chkTreeGPS;
    private WrappingCheckBox chkPlotAddress;
    private WrappingCheckBox chkTreeLocationType;
    private WrappingCheckBox chkSiteType;
    private WrappingCheckBox chkSiteNumber;
    private WrappingCheckBox chkPublic;
    private WrappingCheckBox chkSidewalkConflict;
    private Label label28;
    private TextBox txtOtherOne;
    private TextBox txtOtherTwo;
    private ErrorProvider ep;
    private CheckBox chkOtherOne;
    private CheckBox chkOtherTwo;
    private ComboBox cmbPublicPrivate;
    private TableLayoutPanel tableLayoutPanel7;
    private CheckBox chkOtherThree;
    private TextBox txtOtherThree;
    private Label label29;
    private CheckBox chkUrban;
    private WrappingCheckBox chkCrownHealth;
    private ToolTip toolTip1;
    private Button cmdCancel;
    private Button cmdOK;
    private Label lblInventoryType;
    private PictureBox pbHelpImage;
    private Label lblPlotInformation;
    private Label label12;
    private ComboBox cmbDBHMeasurement;
    private Label lblPlotMinimumRequired;
    private WrappingCheckBox chkPlotPercentMeasured;
    private WrappingCheckBox chkPlotPercentTreeCover;
    private Label lblPlotGeneralFields;
    private WrappingCheckBox chkPlotLandUse;
    private WrappingCheckBox chkReferenceObjects;
    private WrappingCheckBox chkGroundCover;
    private WrappingCheckBox chkPercentShrubCover;
    private WrappingCheckBox chkPlotCenter;
    private Label lblCoverPercentImpervious;
    private Label lblCoverPercentShrub;
    private Label label30;
    private Label label31;
    private Label label32;
    private Label label37;
    private Label label15;
    private Label label38;
    private Label lblPlotTreeLandUse;
    private Label label45;
    private Label label42;
    private Label label41;
    private Label label43;
    private Label label46;
    private Label label47;
    private Label label48;
    private Label label49;
    private Label label50;
    private Label label51;
    private Label label52;
    private Label label53;
    private Label label54;
    private Label label55;
    private ComboBox cmbStreetTree;
    private FlowLayoutPanel flpPlotGeneralFieldsExt;
    private FlowLayoutPanel flpPlotGeneralFields;
    private FlowLayoutPanel flpPlotMinimumRequiredFields;
    private FlowLayoutPanel flowLayoutPanel5;
    private FlowLayoutPanel flowLayoutPanel6;
    private FlowLayoutPanel flowLayoutPanel7;
    private Label label56;
    private Label label35;
    private Label label57;
    private Label label58;
    private RadioButton rdoUnitsMetric;
    private Label lblOptionalFields;
    private Label lblRecommendedFields;
    private Label lblRequiredFields;
    private TableLayoutPanel tableLayoutPanel4;
    private CheckBox chkStrata;
    private TableLayoutPanel tableLayoutPanel5;
    private TableLayoutPanel tableLayoutPanel6;
    private Label label10;
    private TableLayoutPanel tlpLegend;
    private Label label13;
    private Label label14;
    private Label label22;
    private Label label33;
    private NumericTextBox ntbPopulation;
    private Label label34;
    private Label label1;
    private Label label36;
    private Label lblStrataInfo1;
    private Label lblStrataInfo2;
    private System.Windows.Forms.Timer tmValidateWeatherStation;
    private TableLayoutPanel tableLayoutPanel3;
    private WrappingCheckBox chkTreeAddress;
    private RadioButton rdoDieback;
    private RadioButton rdoCondition;
    private FlowLayoutPanel flpCrownHealth;
    private Label lblDataYear;
    private ComboBox cmbWeatherYear;
    private RichTextLabel rtlDataRequired;
    private TableLayoutPanel pnlDataRequired;
    private PictureBox pbInfo;
    private Label label16;
    private Label label17;
    private RichTextLabel rtlInternationalLocationWarning;
    private RichTextLabel rtlLocationIssue;
    private WrappingCheckBox chkTreeUserId;
    private TableLayoutPanel tableLayoutPanel2;
    private PictureBox pbUnitWarning;
    private RichTextLabel rtlUnitWarning;
    private RichTextLabel rtlWeatherData;
    private Label label18;
    private RichTextLabel rtlUseTropicalEq;
    private CheckBox chkUseTropicalEq;
    private Label lblUseTropicalEq;
    private Button btnShowPollutionStations;
    private Label lblPopDensity;
    private NumericTextBox ntbPopDensity;

    public ProjectOverviewForm()
    {
      this.InitializeComponent();
      this.m_ps = Program.Session;
      this.m_syncobj = new object();
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this._weatherService = new WeatherService(this.m_ps.LocSp);
      this.SetCheckBoxEvents((Control) this.tpProjectOptions);
      EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));
      this.Init();
    }

    public ProjectOverviewForm(SampleType NewSampleType)
    {
      this.InitializeComponent();
      this.m_ps = Program.Session;
      this.m_syncobj = new object();
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this._weatherService = new WeatherService(this.m_ps.LocSp);
      this.SetCheckBoxEvents((Control) this.tpProjectOptions);
      this._sampleType = NewSampleType;
      this.Init();
    }

    private void Year_Updated(object sender, EntityUpdated<Year> e)
    {
      if (this.m_year == null || !(this.m_year.Guid == e.Guid))
        return;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() =>
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Refresh((object) this.m_year);
          transaction.Commit();
        }
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        bool flag1 = this.m_year.ConfigEnabled && this.cmbNation.DataSource != null;
        this.cmbNation.Enabled = flag1;
        bool flag2 = ((flag1 ? 1 : 0) & (this.cmbNation.SelectedIndex == -1 ? 0 : (this.cmbState.DataSource != null ? 1 : 0))) != 0;
        this.cmbState.Enabled = flag2;
        bool flag3 = ((flag2 ? 1 : 0) & (this.cmbState.SelectedIndex == -1 ? 0 : (this.cmbCounty.DataSource != null ? 1 : 0))) != 0;
        this.cmbCounty.Enabled = flag3;
        bool flag4 = ((flag3 ? 1 : 0) & (this.cmbCounty.SelectedIndex == -1 ? 0 : (this.cmbPlace.DataSource != null ? 1 : 0))) != 0;
        this.cmbPlace.Enabled = flag4;
        this.cmdShowMap.Enabled = flag4;
        this.cmbWeatherYear.Enabled = flag4;
        this.txtWeatherStation.Enabled = flag4;
        if (flag4)
          this.btnShowPollutionStations.Enabled = !((ComboBoxItem) this.cmbWeatherYear.SelectedItem).WeatherOnly;
        this.InitYearOptions();
        this.OnRequestRefresh();
      }), scheduler));
    }

    private void UpdateBenfitPrices(Year y, Location l)
    {
      if (y.Carbon == null)
      {
        double carbonPrice = this.GetCarbonPrice(l.Id);
        if (carbonPrice >= 0.0)
        {
          Year year = y;
          Carbon carbon = new Carbon();
          carbon.Price = carbonPrice;
          carbon.Year = y;
          year.Carbon = carbon;
        }
      }
      if (y.Electricity == null)
      {
        double electricityPrice = this.GetElectricityPrice(l.Id);
        if (electricityPrice >= 0.0)
        {
          Year year = y;
          Electricity electricity = new Electricity();
          electricity.Price = electricityPrice;
          electricity.Year = y;
          year.Electricity = electricity;
        }
      }
      if (y.Gas == null)
      {
        double gasPrice = this.GetGasPrice(l.Id);
        if (gasPrice >= 0.0)
        {
          Year year = y;
          Gas gas = new Gas();
          gas.Price = gasPrice;
          gas.Year = y;
          year.Gas = gas;
        }
      }
      if (y.H2O != null)
        return;
      double h2Oprice = this.GetH2OPrice(l.Id);
      if (h2Oprice < 0.0)
        return;
      Year year1 = y;
      H2O h2O = new H2O();
      h2O.Price = h2Oprice;
      h2O.Year = y;
      year1.H2O = h2O;
    }

    private void cmbNation_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this._bindingNation)
        return;
      TaskScheduler context = TaskScheduler.FromCurrentSynchronizationContext();
      Location curNation = this.cmbNation.SelectedItem as Location;
      Interlocked.Increment(ref this.m_changes);
      this.cmbCounty.Visible = false;
      this.lblCounty.Visible = false;
      this.cmbCounty.Enabled = false;
      this.cmbCounty.DataSource = (object) null;
      this.cmbPlace.Visible = false;
      this.lblPlace.Visible = false;
      this.cmbPlace.Enabled = false;
      this.cmbPlace.DataSource = (object) null;
      this.cmbState.Enabled = false;
      this.cmbState.DataSource = (object) null;
      if (curNation != null)
      {
        this.m_taskManager.Add(this.FetchChildren(curNation).ContinueWith((System.Action<Task<IList<Location>>>) (t =>
        {
          IList<Location> result = t.Result;
          Interlocked.Increment(ref this.m_changes);
          if (result.Count > 0)
          {
            IList<LocationType> locationTypes = this.GetLocationTypes(result);
            this._formatState = locationTypes.Count > 1;
            this._bindingState = true;
            this.cmbState.BindTo<Location>((object) result, (System.Linq.Expressions.Expression<Func<Location, object>>) (l => l.Self), (System.Linq.Expressions.Expression<Func<Location, object>>) (l => (object) l.Id));
            this.cmbState.SelectedIndex = -1;
            this._bindingState = false;
            if (this.m_stateId == -1 && curNation.Id == this.m_nationId)
              this.m_taskManager.Add(this.SetState(curNation, context));
            if (locationTypes.Count > 0)
              this.lblState.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtLabel, (object) EnumHelper.GetDescription<LocationType>(locationTypes.First<LocationType>()));
            else
              this.lblState.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtLabel, (object) EnumHelper.GetDescription<LocationType>(LocationType.State));
            this.cmbState.Enabled = this.m_year.ConfigEnabled;
            this.cmbState.Visible = true;
            this.lblState.Visible = true;
          }
          else
          {
            this.cmbState.Enabled = false;
            this.cmbState.Visible = false;
            this.lblState.Visible = false;
            this.cmbState.DataSource = (object) null;
            if (this.m_project.Locations.Count == 0 || curNation.Id != this.m_project.Locations.First<ProjectLocation>().LocationId)
            {
              this.chkUrban.Checked = curNation.PercentUrban > 0.5;
              this.chkUseTropicalEq.Checked = curNation.TropicalClimate.HasValue;
            }
          }
          this.DisplayLocation(curNation, context);
          Interlocked.Decrement(ref this.m_changes);
        }), context));
      }
      else
      {
        this.EnableYearData(false);
        this.pnlDataRequired.Visible = false;
        this.cmbState.Visible = false;
        this.lblState.Visible = false;
      }
      Interlocked.Decrement(ref this.m_changes);
      this.cmbNation_Validating(sender, new CancelEventArgs());
    }

    private void EnableYearData(bool available)
    {
      this.cmbWeatherYear.Visible = available;
      this.lblDataYear.Visible = available;
      this.lblSelDataYear.Visible = available;
      this.rtlWeatherData.Visible = available;
      this.btnShowPollutionStations.Visible = available;
      this.lblSelWeatherStation.Visible = available;
      this.lblWeatherStation.Visible = available;
      this.txtWeatherStation.Visible = available;
      this.cmdShowMap.Visible = available;
      Location selectedItem1 = (Location) this.cmbNation.SelectedItem;
      Location selectedItem2 = (Location) this.cmbState.SelectedItem;
      bool flag1 = available && selectedItem1 != null && selectedItem2 != null && NationFeatures.isUsingBenMAPRegression(selectedItem1.Id, selectedItem2.Id);
      this.ntbPopDensity.Visible = flag1;
      this.lblPopDensity.Visible = flag1;
      bool flag2 = available && this.m_year != null && this.m_year.ConfigEnabled;
      this.cmbWeatherYear.Enabled = flag2;
      this.txtWeatherStation.Enabled = flag2;
      this.cmdShowMap.Enabled = flag2;
      this.ntbPopDensity.Enabled = flag2;
      if (available)
        return;
      this.cmbWeatherYear.DataSource = (object) null;
    }

    private void cmbPlace_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this._bindingPlace)
        return;
      Location selectedItem = this.cmbPlace.SelectedItem as Location;
      TaskScheduler context = TaskScheduler.FromCurrentSynchronizationContext();
      Interlocked.Increment(ref this.m_changes);
      if (selectedItem != null)
      {
        this.DisplayLocation(selectedItem, context);
        if (this.m_project.Locations.Count == 0 || selectedItem.Id != this.m_project.Locations.First<ProjectLocation>().LocationId)
        {
          this.chkUrban.Checked = selectedItem.PercentUrban > 0.5;
          this.chkUseTropicalEq.Checked = selectedItem.TropicalClimate.HasValue;
        }
        this.lblPlace.Text = EnumHelper.GetDescription<LocationType>(selectedItem.Type);
      }
      else
        this.DisplayLocation(this.cmbCounty.SelectedItem as Location, context);
      Interlocked.Decrement(ref this.m_changes);
    }

    private void cmbState_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this._bindingState)
        return;
      TaskScheduler context = TaskScheduler.FromCurrentSynchronizationContext();
      Location curState = this.cmbState.SelectedItem as Location;
      Interlocked.Increment(ref this.m_changes);
      this.lblPlace.Visible = false;
      this.cmbPlace.Visible = false;
      this.cmbPlace.Enabled = false;
      this.cmbPlace.DataSource = (object) null;
      this.cmbCounty.Enabled = false;
      this.cmbCounty.DataSource = (object) null;
      if (curState != null)
      {
        this.m_taskManager.Add(this.FetchChildren(curState).ContinueWith((System.Action<Task<IList<Location>>>) (t =>
        {
          IList<Location> result = t.Result;
          this.cmbWeatherYear.Enabled = false;
          this.txtWeatherStation.Enabled = false;
          this.cmdShowMap.Enabled = false;
          this.btnShowPollutionStations.Enabled = false;
          Interlocked.Increment(ref this.m_changes);
          if (result.Count > 0)
          {
            IList<LocationType> locationTypes = this.GetLocationTypes(result);
            this._formatCounty = locationTypes.Count > 1;
            this._bindingCounty = true;
            this.cmbCounty.BindTo<Location>((object) result, (System.Linq.Expressions.Expression<Func<Location, object>>) (l => l.Self), (System.Linq.Expressions.Expression<Func<Location, object>>) (l => (object) l.Id));
            this.cmbCounty.SelectedIndex = -1;
            this._bindingCounty = false;
            if (this.m_countyId == -1 && curState.Id == this.m_stateId)
              this.m_taskManager.Add(this.SetCounty(curState, context));
            if (locationTypes.Count > 0)
              this.lblCounty.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtLabel, (object) EnumHelper.GetDescription<LocationType>(locationTypes.First<LocationType>()));
            else
              this.lblCounty.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtLabel, (object) EnumHelper.GetDescription<LocationType>(LocationType.County));
            this.cmbCounty.Enabled = this.m_year.ConfigEnabled;
            this.cmbCounty.Visible = true;
            this.lblCounty.Visible = true;
          }
          else
          {
            this.cmbCounty.DataSource = (object) null;
            this.cmbCounty.Enabled = false;
            this.cmbCounty.Visible = false;
            this.lblCounty.Visible = false;
            if (this.m_project.Locations.Count == 0 || curState.Id != this.m_project.Locations.First<ProjectLocation>().LocationId)
            {
              this.chkUrban.Checked = curState.PercentUrban > 0.5;
              this.chkUseTropicalEq.Checked = curState.TropicalClimate.HasValue;
            }
          }
          this.DisplayLocation(curState, context);
          Interlocked.Decrement(ref this.m_changes);
        }), context));
      }
      else
      {
        this.DisplayLocation(this.cmbNation.SelectedItem as Location, context);
        this.cmbCounty.Visible = false;
        this.lblCounty.Visible = false;
      }
      Interlocked.Decrement(ref this.m_changes);
    }

    private void DisplayLocation(Location l, TaskScheduler context)
    {
      bool flag1 = l != null && (l.Attributes & LocationAttributes.CanBeProcessed) != 0;
      if (flag1)
      {
        if (this.m_project != null && l.Id == this.m_project.LocationId)
          this.m_taskManager.Add(this.SetLocationInfo(l, context));
        else
          this.m_taskManager.Add(this.LoadDataYears(l.Id, context), this.FetchPopulation(l.Id, context));
      }
      else
      {
        this.EnableYearData(false);
        if (this.m_yearLocData != null && l != null && this.m_project != null)
        {
          this.btnShowPollutionStations.Enabled = this.GetPollutionYears(this.m_project.LocationId).Contains((int) this.m_yearLocData.PollutionYear);
          if (l.Id == this.m_project.LocationId)
            this.ntbPopulation.Text = this.m_yearLocData.Population.ToString();
          else
            this.ntbPopulation.Text = string.Empty;
        }
      }
      bool flag2 = l != null && l.TropicalClimate.HasValue;
      this.lblUseTropicalEq.Visible = flag2;
      this.chkUseTropicalEq.Visible = flag2;
      this.rtlUseTropicalEq.Visible = flag2;
      this.pnlDataRequired.Visible = !flag1;
    }

    private void cmbWeatherYear_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.m_year == null || !this.m_year.ConfigEnabled)
        return;
      this.txtWeatherStation.Text = string.Empty;
      this.cmbWeatherYear_Validating(sender, new CancelEventArgs());
    }

    private void btnShowPollutionStations_Click(object sender, EventArgs e)
    {
      try
      {
        short pollutionYear = ((ComboBoxItem) this.cmbWeatherYear.SelectedItem).Value;
        if (this.currMap != null && this.currMap.Visible && (int) pollutionYear == (int) this.currPollutionYear)
        {
          this.currMap.Activate();
          this.currMap.WindowState = FormWindowState.Normal;
        }
        else
        {
          this.currPollutionYear = pollutionYear;
          Form form = (Form) new MapForm(pollutionYear, ((this.m_year.Changed ? this.GetSelectedLocation()?.Id : new int?(this.m_project.LocationId)) ?? throw new ArgumentException("Invalid Location Selected")).Value);
          form.FormClosed += new FormClosedEventHandler(this.MapForm_FormClosed);
          if (this.currMap != null)
            this.currMap.Close();
          this.currMap = form;
          form.Show();
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        string oops = i_Tree_Eco_v6.Resources.Strings.Oops;
        MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK;
        string caption = oops;
        int buttons = (int) messageBoxButtons;
        int num = (int) MessageBox.Show(message, caption, (MessageBoxButtons) buttons);
      }
    }

    private void MapForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (!(sender is Form form))
        return;
      form.FormClosed -= new FormClosedEventHandler(this.MapForm_FormClosed);
      if (this.currMap != form)
        return;
      this.currMap = (Form) null;
    }

    private void cmdShowMap_Click(object sender, EventArgs e)
    {
      this.tpProjectLocation.ValidateChildren();
      if (this.ep.HasError((Control) this.cmbNation) || this.ep.HasError((Control) this.cmbState) || this.ep.HasError((Control) this.cmbCounty) || this.ep.HasError((Control) this.cmbPlace) || this.ep.HasError((Control) this.cmbWeatherYear))
        return;
      Location location = !this.cmbPlace.Enabled ? (!this.cmbCounty.Enabled ? (!this.cmbState.Enabled ? (Location) this.cmbNation.SelectedItem : (Location) this.cmbState.SelectedItem) : (Location) this.cmbCounty.SelectedItem) : (Location) this.cmbPlace.SelectedItem;
      if (NetworkInterface.GetIsNetworkAvailable())
      {
        WeatherStationSelectorForm stationSelectorForm = new WeatherStationSelectorForm()
        {
          Latitude = location.Latitude,
          Longitude = location.Longitude,
          Year = this.cmbWeatherYear.SelectedValue.ToString(),
          StationId = this.txtWeatherStation.Text
        };
        int num = (int) stationSelectorForm.ShowDialog((IWin32Window) this);
        if (stationSelectorForm.DialogResult != DialogResult.OK)
          return;
        this.txtWeatherStation.Focus();
        this.txtWeatherStation.Text = stationSelectorForm.StationId;
      }
      else
        Process.Start(string.Format("http://www.itreetools.org/eco/weather/?latitude={0}&longitude={1}&year={2}&station={3}&embed={4}", (object) location.Latitude.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) location.Longitude.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.cmbWeatherYear.SelectedValue.ToString(), (object) this.txtWeatherStation.Text, (object) "false"));
    }

    private void CheckBoxChanged(object sender, EventArgs e)
    {
      if (sender == this.chkShrubs)
      {
        if (this.chkShrubs.Checked)
        {
          this.chkPercentShrubCover.CheckState = CheckState.Indeterminate;
          this.chkPercentShrubCover.AutoCheck = false;
        }
        else
        {
          if (this.chkPercentShrubCover.CheckState == CheckState.Indeterminate)
            this.chkPercentShrubCover.CheckState = CheckState.Checked;
          this.chkPercentShrubCover.AutoCheck = true;
        }
      }
      else if (sender == this.chkPercentShrubCover)
      {
        if (this.chkPercentShrubCover.Checked || this.chkPercentShrubCover.CheckState == CheckState.Indeterminate)
          this.chkShrubs.Enabled = true;
        else
          this.chkShrubs.Enabled = false;
      }
      else if (sender == this.chkSiteType)
        this.cmbStreetTree.Enabled = this.chkSiteType.Checked;
      else if (sender == this.chkPublic)
        this.cmbPublicPrivate.Enabled = this.chkPublic.Checked;
      else if (sender == this.chkOtherOne)
        this.txtOtherOne.Enabled = this.chkOtherOne.Checked;
      else if (sender == this.chkOtherTwo)
      {
        this.txtOtherTwo.Enabled = this.chkOtherTwo.Checked;
      }
      else
      {
        if (sender != this.chkOtherThree)
          return;
        this.txtOtherThree.Enabled = this.chkOtherThree.Checked;
      }
    }

    private void cmbCounty_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this._bindingCounty)
        return;
      TaskScheduler context = TaskScheduler.FromCurrentSynchronizationContext();
      Location curCounty = this.cmbCounty.SelectedItem as Location;
      Interlocked.Increment(ref this.m_changes);
      this.cmbPlace.Enabled = false;
      this.cmbPlace.DataSource = (object) null;
      if (curCounty != null)
      {
        this.m_taskManager.Add(this.FetchChildren(curCounty).ContinueWith((System.Action<Task<IList<Location>>>) (t =>
        {
          IList<Location> result = t.Result;
          Interlocked.Increment(ref this.m_changes);
          if (result.Count > 0)
          {
            IList<LocationType> locationTypes = this.GetLocationTypes(result);
            this._formatPlace = locationTypes.Count > 1;
            this._bindingPlace = true;
            this.cmbPlace.BindTo<Location>((object) result, (System.Linq.Expressions.Expression<Func<Location, object>>) (l => l.Self), (System.Linq.Expressions.Expression<Func<Location, object>>) (l => (object) l.Id));
            this.cmbPlace.SelectedIndex = -1;
            this._bindingPlace = false;
            if (this.m_placeId == -1 && curCounty.Id == this.m_countyId)
              this.m_taskManager.Add(this.SetPlace(curCounty, context));
            if (locationTypes.Count > 0)
              this.lblPlace.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtLabel, (object) EnumHelper.GetDescription<LocationType>(locationTypes.First<LocationType>()));
            else
              this.lblPlace.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtLabel, (object) EnumHelper.GetDescription<LocationType>(LocationType.Place));
            this.cmbPlace.Enabled = this.m_year.ConfigEnabled;
            this.cmbPlace.Visible = true;
            this.lblPlace.Visible = true;
          }
          else
          {
            this.cmbPlace.Visible = false;
            this.lblPlace.Visible = false;
            this.cmbPlace.Enabled = false;
            this.cmbPlace.DataSource = (object) null;
            if (this.m_project.Locations.Count == 0 || curCounty.Id != this.m_project.Locations.First<ProjectLocation>().LocationId)
            {
              this.chkUrban.Checked = curCounty.PercentUrban > 0.5;
              this.chkUseTropicalEq.Checked = curCounty.TropicalClimate.HasValue;
            }
          }
          this.DisplayLocation(curCounty, context);
          Interlocked.Decrement(ref this.m_changes);
        }), context));
      }
      else
      {
        this.DisplayLocation(this.cmbState.SelectedItem as Location, context);
        this.cmbPlace.Visible = false;
        this.lblPlace.Visible = false;
      }
      Interlocked.Decrement(ref this.m_changes);
    }

    private void tabControl1_Deselecting(object sender, TabControlCancelEventArgs e)
    {
      if (this.m_year == null || !this.m_year.ConfigEnabled)
        return;
      if (e.TabPageIndex == 0)
      {
        this.ValidateProjectInfo(sender, (CancelEventArgs) e);
        if (!e.Cancel)
        {
          this.m_project.Name = this.txtProjectName.Text;
          this.m_series.Id = this.txtSeriesName.Text;
          this.m_year.Id = short.Parse(this.txtSeriesYear.Text);
        }
      }
      if (e.TabPageIndex == 1)
        this.ValidateLocation(sender, (CancelEventArgs) e);
      if (e.TabPageIndex != 2)
        return;
      this.ValidateDataCollectionOptions(sender, (CancelEventArgs) e);
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      TabPage selectedTab = this.tabControl1.SelectedTab;
      if (selectedTab == this.tpProjectOptions)
      {
        if (this.m_series.SampleType != SampleType.Inventory)
          this.OnShowHelp("tpProjectOptions_plot");
        else
          this.OnShowHelp("tpProjectOptions_full");
      }
      else
        this.OnShowHelp(selectedTab.Name);
      if (selectedTab == this.tpProjectInfo)
        this.tableLayoutPanel1.Focus();
      else if (selectedTab == this.tpProjectLocation)
      {
        this.pnlLocation.Focus();
      }
      else
      {
        if (selectedTab != this.tpProjectOptions)
          return;
        this.pnlProjectOptions.Focus();
      }
    }

    private void txtProjectName_Validating(object sender, CancelEventArgs e)
    {
      if (this.m_year == null || !this.m_year.ConfigEnabled)
        return;
      string text = this.txtProjectName.Text;
      this.ep.SetError((Control) this.txtProjectName, string.IsNullOrEmpty(text) || !Regex.IsMatch(text, "\\w"), i_Tree_Eco_v6.Resources.Strings.ErrProjectName);
    }

    private void txtSeriesName_Validating(object sender, CancelEventArgs e)
    {
      if (this.m_year == null || !this.m_year.ConfigEnabled)
        return;
      string text = this.txtSeriesName.Text;
      this.ep.SetError((Control) this.txtSeriesName, string.IsNullOrEmpty(text) || !Regex.IsMatch(text, "\\w"), i_Tree_Eco_v6.Resources.Strings.ErrSeriesName);
    }

    private void txtSeriesYear_Validating(object sender, CancelEventArgs e)
    {
      if (this.m_year == null || !this.m_year.ConfigEnabled)
        return;
      int result;
      this.ep.SetError((Control) this.txtSeriesYear, !int.TryParse(this.txtSeriesYear.Text, out result) || result < 1990 || result > 2100, i_Tree_Eco_v6.Resources.Strings.ErrSeriesYear);
    }

    private void customFields_Validating(object sender, CancelEventArgs e)
    {
      if (this.m_year == null || !this.m_year.ConfigEnabled)
        return;
      TextBox c = sender as TextBox;
      this.ep.SetError((Control) c, c.Enabled && string.IsNullOrEmpty(c.Text), i_Tree_Eco_v6.Resources.Strings.ErrCustomFieldName);
      if (!c.Enabled || this.ep.HasError((Control) c))
        return;
      this.ep.SetError((Control) c, !Regex.IsMatch(c.Text, "^[a-zA-Z0-9\\s]+$"), i_Tree_Eco_v6.Resources.Strings.ErrCustomFieldFormat);
    }

    private void ValidateDataCollectionOptions(object sender, CancelEventArgs e)
    {
      this.tpProjectOptions.ValidateChildren();
      e.Cancel = this.tpProjectOptions.HasErrors(this.ep);
    }

    private void ValidateLocation(object sender, CancelEventArgs e)
    {
      this.tpProjectLocation.ValidateChildren();
      e.Cancel = this.tpProjectLocation.HasErrors(this.ep);
    }

    private void ValidateProjectInfo(object sender, CancelEventArgs e)
    {
      this.tpProjectInfo.ValidateChildren();
      e.Cancel = this.tpProjectInfo.HasErrors(this.ep);
    }

    private void cmdOK_Click(object sender, EventArgs e)
    {
      if (this.m_year != null && this.m_year.ConfigEnabled)
      {
        CancelEventArgs e1 = new CancelEventArgs();
        this.ValidateProjectInfo(sender, e1);
        if (e1.Cancel)
        {
          this.tabControl1.SelectedTab = this.tpProjectInfo;
          return;
        }
        this.ValidateLocation(sender, e1);
        if (e1.Cancel)
        {
          this.tabControl1.SelectedTab = this.tpProjectLocation;
          return;
        }
        this.ValidateDataCollectionOptions(sender, e1);
        if (e1.Cancel)
        {
          this.tabControl1.SelectedTab = this.tpProjectOptions;
          return;
        }
        if (!this.chkCrownHealth.Checked && MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.WarnCrownHealth, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        {
          this.tabControl1.SelectedTab = this.tpProjectOptions;
          return;
        }
        if (this.rdoUnitsEnglish.Checked)
          this.m_year.Unit = YearUnit.English;
        else if (this.rdoUnitsMetric.Checked)
          this.m_year.Unit = YearUnit.Metric;
        Location selectedLocation = this.GetSelectedLocation();
        if (selectedLocation != null)
          this.UpdateBenfitPrices(this.m_year, selectedLocation);
        string str1 = "00000";
        string str2 = "000";
        string str3 = "00";
        string NationCode = "000";
        if (this.cmbPlace.Enabled && this.cmbPlace.SelectedIndex != -1)
        {
          LocationRelation locationRelation = this.GetLocationRelation((Location) this.cmbPlace.SelectedItem);
          if (locationRelation.Level == (short) 5)
            str1 = locationRelation.Code;
        }
        if (this.cmbCounty.Enabled && this.cmbCounty.SelectedIndex != -1)
        {
          LocationRelation locationRelation = this.GetLocationRelation((Location) this.cmbCounty.SelectedItem);
          if (locationRelation.Level == (short) 4)
            str2 = locationRelation.Code;
          else if (locationRelation.Level == (short) 5)
            str1 = locationRelation.Code;
        }
        if (this.cmbState.Enabled && this.cmbState.SelectedIndex != -1)
        {
          LocationRelation locationRelation = this.GetLocationRelation((Location) this.cmbState.SelectedItem);
          if (locationRelation.Level == (short) 3)
            str3 = locationRelation.Code;
          else if (locationRelation.Level == (short) 4)
            str2 = locationRelation.Code;
          else if (locationRelation.Level == (short) 5)
            str1 = locationRelation.Code;
        }
        if (this.cmbNation.SelectedIndex != -1)
          NationCode = this.GetLocationRelation((Location) this.cmbNation.SelectedItem).Code;
        if (this.m_project.Locations.Count == 0)
        {
          this.m_project.Locations.Add(new ProjectLocation()
          {
            NationCode = NationCode,
            PrimaryPartitionCode = str3,
            SecondaryPartitionCode = str2,
            TertiaryPartitionCode = str1,
            LocationId = selectedLocation.Id,
            Project = this.m_project,
            IsUrban = this.chkUrban.Checked,
            UseTropical = this.chkUseTropicalEq.Checked
          });
        }
        else
        {
          ProjectLocation projectLocation = this.m_project.Locations.First<ProjectLocation>();
          projectLocation.NationCode = NationCode;
          projectLocation.PrimaryPartitionCode = str3;
          projectLocation.SecondaryPartitionCode = str2;
          projectLocation.TertiaryPartitionCode = str1;
          projectLocation.LocationId = selectedLocation.Id;
          projectLocation.IsUrban = this.chkUrban.Checked;
          projectLocation.UseTropical = this.chkUseTropicalEq.Checked;
        }
        this.m_project.NationCode = NationCode;
        this.m_project.PrimaryPartitionCode = str3;
        this.m_project.SecondaryPartitionCode = str2;
        this.m_project.TertiaryPartitionCode = str1;
        this.m_project.LocationId = selectedLocation.Id;
        if (this.m_year.YearLocationData.Count == 0)
          this.m_year.YearLocationData.Add(new YearLocationData()
          {
            ProjectLocation = this.m_project.Locations.First<ProjectLocation>(),
            Year = this.m_year
          });
        YearLocationData yearLocationData = this.m_year.YearLocationData.First<YearLocationData>();
        yearLocationData.Year = this.m_year;
        if ((selectedLocation.Attributes & LocationAttributes.CanBeProcessed) != LocationAttributes.None)
        {
          yearLocationData.WeatherYear = (short) this.cmbWeatherYear.SelectedValue;
          yearLocationData.WeatherStationId = this.txtWeatherStation.Text;
          yearLocationData.PollutionYear = !this.GetPollutionYears(selectedLocation.Id).Contains((int) yearLocationData.WeatherYear) ? (short) -1 : yearLocationData.WeatherYear;
        }
        else
        {
          yearLocationData.WeatherYear = (short) -1;
          yearLocationData.PollutionYear = (short) -1;
          yearLocationData.WeatherStationId = string.Empty;
        }
        yearLocationData.Population = int.Parse(this.ntbPopulation.Text);
        if (NationFeatures.IsUSlikeNation(NationCode))
        {
          PopulationDensity populationDensity = this.m_year.PopulationDensity;
          if (populationDensity == null)
          {
            populationDensity = new PopulationDensity();
            populationDensity.Year = this.m_year;
            this.m_year.PopulationDensity = populationDensity;
          }
          populationDensity.Price = double.Parse(this.ntbPopDensity.Text);
        }
        bool isTransient1 = this.m_project.IsTransient;
        bool isTransient2 = this.m_series.IsTransient;
        bool isTransient3 = this.m_year.IsTransient;
        this.SaveAll();
        if ((selectedLocation.Attributes & LocationAttributes.CanBeProcessed) != LocationAttributes.None)
        {
          using (ShowAvailableReports availableReports = new ShowAvailableReports(this.cmbNation.SelectedItem as Location, this.m_year, true, false))
          {
            if (availableReports.ShowDialog() == DialogResult.Cancel)
              return;
          }
        }
        if (!this.m_ps.InputSession.YearKey.HasValue)
          this.InitProjectDefaults(this.m_year);
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.SaveOrUpdate((object) this.m_project);
          this.m_session.SaveOrUpdate((object) this.m_series);
          this.m_session.SaveOrUpdate((object) this.m_year);
          this.m_session.SaveOrUpdate((object) yearLocationData);
          transaction.Commit();
        }
        if (!isTransient1)
          EventPublisher.Publish<EntityUpdated<Project>>(new EntityUpdated<Project>(this.m_project), (Control) this);
        else
          this.m_projectCreated = true;
        if (!isTransient2)
          EventPublisher.Publish<EntityUpdated<Series>>(new EntityUpdated<Series>(this.m_series), (Control) this);
        if (!isTransient3)
          EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(this.m_year), (Control) this);
        ((MainRibbonForm) Application.OpenForms["MainRibbonForm"]).rbStrata.Enabled = this.m_year.RecordStrata;
      }
      this.m_isDirty = false;
      this.Close();
    }

    private Location GetSelectedLocation()
    {
      Location selectedLocation = (Location) null;
      if (this.cmbPlace.Enabled && this.cmbPlace.SelectedIndex != -1)
        selectedLocation = (Location) this.cmbPlace.SelectedItem;
      else if (this.cmbCounty.Enabled && this.cmbCounty.SelectedIndex != -1)
        selectedLocation = (Location) this.cmbCounty.SelectedItem;
      else if (this.cmbState.Enabled && this.cmbState.SelectedIndex != -1)
        selectedLocation = (Location) this.cmbState.SelectedItem;
      else if (this.cmbNation.SelectedIndex != -1)
        selectedLocation = (Location) this.cmbNation.SelectedItem;
      return selectedLocation;
    }

    private LocationRelation GetLocationRelation(Location l) => RetryExecutionHandler.Execute<LocationRelation>((Func<LocationRelation>) (() =>
    {
      using (ISession session = this.m_ps.LocSp.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          // ISSUE: reference to a compiler-generated field
          LocationRelation locationRelation = session.QueryOver<LocationRelation>().Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Location == this.l)).And((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Code != default (string))).Cacheable().SingleOrDefault();
          transaction.Commit();
          return locationRelation;
        }
      }
    }));

    private Dictionary<string, DisabledReport> SetDisabledReports()
    {
      Dictionary<string, DisabledReport> dictionary = new Dictionary<string, DisabledReport>();
      if (this.chkLandUseTree.Tag != null && !this.chkLandUseTree.Checked)
      {
        string tag = (string) this.chkLandUseTree.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkLandUseTree.Text,
            visible = this.chkLandUseTree.Visible
          });
      }
      if (this.chkMaintRec.Tag != null && !this.chkMaintRec.Checked)
      {
        string tag = (string) this.chkMaintRec.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkMaintRec.Text,
            visible = this.chkMaintRec.Visible
          });
      }
      if (this.chkMaintTask.Tag != null && !this.chkMaintTask.Checked)
      {
        string tag = (string) this.chkMaintTask.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkMaintTask.Text,
            visible = this.chkMaintTask.Visible
          });
      }
      if (this.chkSidewalkConflict.Tag != null && !this.chkSidewalkConflict.Checked)
      {
        string tag = (string) this.chkSidewalkConflict.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkSidewalkConflict.Text,
            visible = this.chkSidewalkConflict.Visible
          });
      }
      if (this.chkOverheadUtilityConflict.Tag != null && !this.chkOverheadUtilityConflict.Checked)
      {
        string tag = (string) this.chkOverheadUtilityConflict.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkOverheadUtilityConflict.Text,
            visible = this.chkOverheadUtilityConflict.Visible
          });
      }
      if (this.chkOtherOne.Tag != null && !this.chkOtherOne.Checked)
      {
        string tag = (string) this.chkOtherOne.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkOtherOne.Text,
            visible = this.chkOtherOne.Visible
          });
      }
      if (this.chkOtherTwo.Tag != null && !this.chkOtherTwo.Checked)
      {
        string tag = (string) this.chkOtherTwo.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkOtherTwo.Text,
            visible = this.chkOtherTwo.Visible
          });
      }
      if (this.chkOtherThree.Tag != null && !this.chkOtherThree.Checked)
      {
        string tag = (string) this.chkOtherThree.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkOtherThree.Text,
            visible = this.chkOtherThree.Visible
          });
      }
      if (this.chkShrubs.Tag != null && !this.chkShrubs.Checked)
      {
        string tag = (string) this.chkShrubs.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkShrubs.Text,
            visible = this.chkShrubs.Visible
          });
      }
      if (this.chkGroundCover.Tag != null && !this.chkGroundCover.Checked)
      {
        string tag = (string) this.chkGroundCover.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkGroundCover.Text,
            visible = this.chkGroundCover.Visible
          });
      }
      if (this.chkIPED.Tag != null && !this.chkIPED.Checked)
      {
        string tag = (string) this.chkIPED.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkIPED.Text,
            visible = this.chkIPED.Visible
          });
      }
      if (this.chkEnergyEffect.Tag != null && !this.chkEnergyEffect.Checked)
      {
        string tag = (string) this.chkEnergyEffect.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkEnergyEffect.Text,
            visible = this.chkEnergyEffect.Visible
          });
      }
      if (this.chkSiteType.Tag != null && !this.chkSiteType.Checked)
      {
        string tag = (string) this.chkSiteType.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkSiteType.Text,
            visible = this.chkSiteType.Visible
          });
      }
      if (this.chkPublic.Tag != null && !this.chkPublic.Checked)
      {
        string tag = (string) this.chkPublic.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkPublic.Text,
            visible = this.chkPublic.Visible
          });
      }
      if (this.chkStrata.Visible && this.chkStrata.Tag != null && !this.chkStrata.Checked)
      {
        string tag = (string) this.chkStrata.Tag;
        char[] chArray = new char[1]{ ',' };
        foreach (string key in tag.Split(chArray))
          dictionary.Add(key, new DisabledReport()
          {
            checkbox = this.chkStrata.Text,
            visible = true
          });
      }
      return dictionary;
    }

    private void cmdCancel_Click(object sender, EventArgs e)
    {
      this.m_projectCreated = false;
      this.Close();
    }

    private void chkPlotLandUse_Click(object sender, EventArgs e) => this.chkLandUseTree.Checked = this.chkPlotLandUse.Checked;

    private void chkLandUseTree_Click(object sender, EventArgs e) => this.chkPlotLandUse.Checked = this.chkLandUseTree.Checked;

    private void ProjectOverviewForm_Resize(object sender, EventArgs e)
    {
      foreach (ComboBox combobox in this.m_Comboboxes)
        combobox.Width = combobox.Parent.Width - (combobox.Margin.Left + 4);
    }

    private void ProjectOverviewForm_Closed(object sender, EventArgs e)
    {
      if (this.currMap == null)
        return;
      this.currMap.Close();
      this.currMap = (Form) null;
    }

    private void SetCheckBoxEvents(Control topControl)
    {
      foreach (Control control in (ArrangedElementCollection) topControl.Controls)
      {
        if (control.HasChildren)
          this.SetCheckBoxEvents(control);
        else if (control is CheckBox)
          ((CheckBox) control).CheckedChanged += new EventHandler(this.CheckBoxChanged);
        else if (control is ComboBox)
          this.m_Comboboxes.Add((ComboBox) control);
      }
    }

    private void Init()
    {
      this.m_changes = 0;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.InitUI();
      }), scheduler));
    }

    private void LoadData()
    {
      ISession session1 = this.m_ps.InputSession.CreateSession();
      IList<Location> locationList1 = RetryExecutionHandler.Execute<IList<Location>>((Func<IList<Location>>) (() =>
      {
        using (ISession session2 = this.m_ps.LocSp.OpenSession())
        {
          using (ITransaction transaction = session2.BeginTransaction())
          {
            IList<Location> locationList2 = session2.QueryOver<Location>().OrderBy((System.Linq.Expressions.Expression<Func<Location, object>>) (l => l.Name)).Asc.JoinQueryOver<LocationRelation>((System.Linq.Expressions.Expression<Func<Location, IEnumerable<LocationRelation>>>) (l => l.Relations)).Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Code != default (string))).Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => (int) r.Level == 2)).Cacheable().List();
            transaction.Commit();
            return locationList2;
          }
        }
      }));
      Project project;
      Series series;
      Year year;
      if (!this.m_ps.InputSession.YearKey.HasValue)
      {
        SampleType sampleType = this._sampleType;
        SampleMethod sampleMethod = sampleType == SampleType.Inventory ? SampleMethod.None : SampleMethod.SimpleRandom;
        project = new Project();
        series = new Series()
        {
          Project = project,
          DefaultPlotSize = -1f,
          SampleType = sampleType,
          SampleMethod = sampleMethod
        };
        year = new Year()
        {
          Changed = true,
          Series = series,
          IsInitialMeasurement = true,
          RecordStrata = this._sampleType != SampleType.Inventory,
          Id = (short) DateTime.Now.Year
        };
        project.Series.Add(this.m_series);
        series.Years.Add(this.m_year);
      }
      else
      {
        using (session1.BeginTransaction())
        {
          year = session1.Get<Year>((object) this.m_ps.InputSession.YearKey);
          series = year.Series;
          project = series.Project;
          NHibernateUtil.Initialize((object) year.Strata);
          NHibernateUtil.Initialize((object) year.YearLocationData);
          NHibernateUtil.Initialize((object) project.Locations);
        }
      }
      lock (this.m_syncobj)
      {
        this.m_session = session1;
        this.m_nations = locationList1;
        this.m_project = project;
        this.m_series = series;
        this.m_year = year;
        if (year.YearLocationData.Count <= 0)
          return;
        this.m_yearLocData = this.m_year.YearLocationData.First<YearLocationData>();
      }
    }

    private void InitUI()
    {
      this.toolTip1.SetToolTip((Control) this.chkCLE, i_Tree_Eco_v6.Resources.Strings.ToolTipCrownLightExposure);
      this.toolTip1.SetToolTip((Control) this.chkEnergyEffect, i_Tree_Eco_v6.Resources.Strings.ToolTipEnergyEffects);
      this.tableLayoutPanel1.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);
      this.pnlLocation.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);
      this.pnlProjectOptions.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);
      this._bindingNation = true;
      this.cmbNation.BindTo<Location>((object) this.m_nations, (System.Linq.Expressions.Expression<Func<Location, object>>) (l => l.Name), (System.Linq.Expressions.Expression<Func<Location, object>>) (l => (object) l.Id));
      this.cmbNation.SelectedIndex = -1;
      this._bindingNation = false;
      this.SetInventoryType();
      this.InitYearOptions();
      if (this.m_project.Locations.Count > 0)
      {
        ProjectLocation projectLocation = this.m_project.Locations.First<ProjectLocation>();
        this.chkUrban.Checked = projectLocation.IsUrban;
        this.chkUseTropicalEq.Checked = projectLocation.UseTropical;
      }
      if (this.m_project.IsTransient)
      {
        this.cmbNation.SelectedValue = (object) 219;
      }
      else
      {
        this.rdoUnitsEnglish.Enabled = false;
        this.rdoUnitsMetric.Enabled = false;
        this.cmbDBHMeasurement.Enabled = false;
        this.m_taskManager.Add(this.SetNation(TaskScheduler.FromCurrentSynchronizationContext()));
      }
      this.OnRequestRefresh();
      this.registerChangeEvents((Control) this);
    }

    private double GetElectricityPrice(int locId)
    {
      LocationCost locationCost = this.GetLocationCost(locId);
      return locationCost != null ? locationCost.Electricity : -1.0;
    }

    private double GetGasPrice(int locId)
    {
      LocationCost locationCost = this.GetLocationCost(locId);
      return locationCost != null ? locationCost.Fuels / 10.002387672 : -1.0;
    }

    private double GetCarbonPrice(int locId)
    {
      LocationEnvironmentalValue environmentalValue = this.GetEnvironmentalValue(locId);
      return environmentalValue != null ? (double) environmentalValue.Carbon : -1.0;
    }

    private double GetH2OPrice(int locId)
    {
      LocationEnvironmentalValue environmentalValue = this.GetEnvironmentalValue(locId);
      return environmentalValue != null ? environmentalValue.RainfallInterception * 264.172 : -1.0;
    }

    private LocationCost GetLocationCost(int locId)
    {
      using (ISession session = this.m_ps.LocSp.OpenSession())
      {
        Location location;
        LocationRelation locationRelation;
        for (location = session.Get<Location>((object) locId); location != null && location.LocationCost == null; location = locationRelation == null ? (Location) null : locationRelation.Parent)
          locationRelation = session.CreateCriteria<LocationRelation>().Add((ICriterion) Restrictions.Eq("Location", (object) location)).Add((ICriterion) Restrictions.IsNotNull("Code")).SetCacheable(true).UniqueResult<LocationRelation>();
        return location?.LocationCost;
      }
    }

    private LocationEnvironmentalValue GetEnvironmentalValue(int locId)
    {
      using (ISession session = this.m_ps.LocSp.OpenSession())
      {
        Location location;
        LocationRelation locationRelation;
        for (location = session.Get<Location>((object) locId); location != null && location.EnvironmentalValue == null; location = locationRelation == null ? (Location) null : locationRelation.Parent)
          locationRelation = session.CreateCriteria<LocationRelation>().Add((ICriterion) Restrictions.Eq("Location", (object) location)).Add((ICriterion) Restrictions.IsNotNull("Code")).SetCacheable(true).UniqueResult<LocationRelation>();
        return location?.EnvironmentalValue;
      }
    }

    private void SetInventoryType()
    {
      SampleType sampleType = this.m_series.SampleType;
      this.lblAccuracyCaption.Text = i_Tree_Eco_v6.Resources.Strings.MsgAccuracy;
      Bitmap bitmap;
      if (sampleType == SampleType.RegularPlot)
      {
        this.lblInventoryType.Text = i_Tree_Eco_v6.Resources.Strings.PlotSample;
        bitmap = i_Tree_Eco_v6.Properties.Resources.ProjectOverviewSampleTable;
        this.chkStrata.Visible = false;
        this.chkTreeAddress.Visible = false;
        this.lblStrataInfo1.Visible = false;
        this.lblStrataInfo2.Visible = false;
      }
      else
      {
        if (sampleType != SampleType.Inventory)
          throw new ArgumentOutOfRangeException("Sample Type not plot sample or complete inventory.");
        this.lblInventoryType.Text = i_Tree_Eco_v6.Resources.Strings.CompleteInventory;
        this.chkPlotGPS.Visible = false;
        bitmap = i_Tree_Eco_v6.Properties.Resources.ProjectOverviewInventoryTable;
        this.lblPlotTreeLandUse.Visible = false;
        this.lblPlotGeneralFields.Visible = false;
        this.lblPlotInformation.Visible = false;
        this.lblPlotMinimumRequired.Visible = false;
        this.flpPlotGeneralFields.Visible = false;
        this.flpPlotGeneralFieldsExt.Visible = false;
        this.flpPlotMinimumRequiredFields.Visible = false;
        this.chkHydro.Visible = false;
        this.lblCoverPercentImpervious.Visible = false;
        this.lblCoverPercentShrub.Visible = false;
        this.chkPlotCenter.Visible = false;
      }
      this.pbHelpImage.Image = (Image) bitmap;
      HighDpiHelper.AdjustControlImagesDpiScale((Control) this);
      TableLayoutPanel pnlProjectOptions = this.pnlProjectOptions;
      int width = this.pbHelpImage.Image.Width;
      Padding margin = this.pbHelpImage.Margin;
      int left = margin.Left;
      int num = width + left;
      margin = this.pbHelpImage.Margin;
      int right = margin.Right;
      Size size = new Size(num + right, 0);
      pnlProjectOptions.AutoScrollMinSize = size;
    }

    private void InitProjectDefaults(Year y)
    {
      using (ISession session = this.m_ps.LocSp.OpenSession())
      {
        foreach (CoverType coverType in (IEnumerable<CoverType>) session.CreateCriteria<CoverType>().SetCacheable(true).List<CoverType>())
        {
          GroundCover groundCover = new GroundCover()
          {
            Id = coverType.Id,
            Description = coverType.Description,
            Year = y,
            CoverTypeId = coverType.Id
          };
          y.GroundCovers.Add(groundCover);
        }
        foreach (FieldLandUse fieldLandUse in (IEnumerable<FieldLandUse>) session.CreateCriteria<FieldLandUse>().SetCacheable(true).List<FieldLandUse>())
        {
          LandUse landUse = new LandUse()
          {
            Id = fieldLandUse.Code,
            Description = fieldLandUse.Description,
            Year = y,
            LandUseId = fieldLandUse.Id
          };
          y.LandUses.Add(landUse);
        }
      }
      y.Strata.Add(new Strata()
      {
        Abbreviation = "Urban",
        Description = "Urban",
        Id = 1,
        Size = 1f,
        Year = y
      });
      YearHelper.CreateDBHRptClasses(y);
      YearHelper.CreateConditions(y);
      YearHelper.CreateHealthRptClasses(y);
      YearHelper.CreateDBHs(y);
      YearHelper.CreateLookup<LocationSpecies.Domain.MaintTask, Eco.Domain.v6.MaintTask>(y, y.MaintTasks, this.m_ps.LocSp);
      YearHelper.CreateLookup<MaintType, MaintRec>(y, y.MaintRecs, this.m_ps.LocSp);
      YearHelper.CreateLookup<SidewalkDamage, Sidewalk>(y, y.SidewalkDamages, this.m_ps.LocSp);
      YearHelper.CreateLookup<LocationSpecies.Domain.WireConflict, Eco.Domain.v6.WireConflict>(y, y.WireConflicts, this.m_ps.LocSp);
    }

    public bool ProjectCreated => this.m_projectCreated;

    private IList<LocationType> GetLocationTypes(IList<Location> locations) => (IList<LocationType>) locations.GroupBy<Location, LocationType>((Func<Location, LocationType>) (l => l.Type)).OrderByDescending<IGrouping<LocationType, Location>, int>((Func<IGrouping<LocationType, Location>, int>) (t => t.Count<Location>())).Select<IGrouping<LocationType, Location>, LocationType>((Func<IGrouping<LocationType, Location>, LocationType>) (g => g.Key)).ToList<LocationType>();

    private void SaveAll()
    {
      this.m_project.Name = this.txtProjectName.Text;
      this.m_series.Id = this.txtSeriesName.Text;
      this.m_year.Id = short.Parse(this.txtSeriesYear.Text);
      this.m_year.RecordCrownSize = this.chkCrownWidth.CheckState != 0;
      this.m_year.RecordCLE = this.chkCLE.CheckState != 0;
      this.m_year.RecordCrownCondition = this.chkCrownHealth.Checked;
      this.m_year.DisplayCondition = this.rdoCondition.Checked;
      this.m_year.RecordEnergy = this.chkEnergyEffect.CheckState != 0;
      this.m_year.RecordTreeUserId = this.chkTreeUserId.Checked;
      this.m_year.RecordTreeStatus = this.chkTreeStatus.Checked;
      this.m_year.RecordHeight = this.chkHeight.CheckState != 0;
      this.m_year.RecordIPED = this.chkIPED.Checked;
      this.m_year.RecordShrub = this.chkShrubs.Checked;
      this.m_year.RecordPlantableSpace = this.chkPercentPlantable.Checked;
      this.m_year.RecordHydro = this.chkHydro.Checked;
      this.m_year.DBHActual = this.cmbDBHMeasurement.SelectedIndex == 0;
      this.m_year.RecordPlotCenter = this.chkPlotCenter.Checked;
      this.m_year.RecordGroundCover = this.chkGroundCover.Checked;
      this.m_year.RecordPercentShrub = this.chkPercentShrubCover.Checked;
      this.m_year.DefaultStreetTree = this.cmbStreetTree.SelectedIndex == 1;
      if (this.m_series.SampleType == SampleType.Inventory)
      {
        this.m_year.RecordTreeGPS = this.chkTreeGPS.Checked;
        this.m_year.RecordGPS = false;
        this.m_year.RecordStrata = this.chkStrata.Checked;
        this.m_year.RecordTreeAddress = this.chkTreeAddress.Checked;
      }
      else
      {
        this.m_year.RecordGPS = this.chkPlotGPS.Checked;
        this.m_year.RecordTreeGPS = this.chkTreeGPS.Checked;
      }
      this.m_year.RecordLanduse = this.chkLandUseTree.Checked;
      this.m_year.RecordLocSite = this.chkTreeLocationType.Checked;
      this.m_year.RecordLocNo = this.chkSiteNumber.Checked;
      this.m_year.RecordMaintRec = this.chkMaintRec.Checked;
      this.m_year.RecordMaintTask = this.chkMaintTask.Checked;
      this.m_year.RecordWireConflict = this.chkOverheadUtilityConflict.Checked;
      this.m_year.RecordSidewalk = this.chkSidewalkConflict.Checked;
      this.m_year.RecordPlotAddress = this.chkPlotAddress.Checked;
      this.m_year.RecordStreetTree = this.chkSiteType.Checked;
      this.m_year.RecordReferenceObjects = this.chkReferenceObjects.Checked;
      if (this.cmbPublicPrivate.Enabled)
      {
        int selectedIndex = this.cmbPublicPrivate.SelectedIndex;
        if (selectedIndex > -1)
          this.m_year.MgmtStyle = new MgmtStyleEnum?((MgmtStyleEnum) (2 | selectedIndex));
      }
      else
        this.m_year.MgmtStyle = new MgmtStyleEnum?();
      this.m_year.RecordOtherOne = this.chkOtherOne.Checked;
      this.m_year.OtherOne = this.txtOtherOne.Text;
      this.m_year.RecordOtherTwo = this.chkOtherTwo.Checked;
      this.m_year.OtherTwo = this.txtOtherTwo.Text;
      this.m_year.RecordOtherThree = this.chkOtherThree.Checked;
      this.m_year.OtherThree = this.txtOtherThree.Text;
    }

    private Task<IList<Location>> FetchChildren(Location parent) => Task.Factory.StartNew<IList<Location>>((Func<IList<Location>>) (() => RetryExecutionHandler.Execute<IList<Location>>((Func<IList<Location>>) (() =>
    {
      using (ISession session = this.m_ps.LocSp.OpenSession())
        return session.CreateCriteria<Location>().CreateAlias("Relations", "r").Add((ICriterion) Restrictions.IsNotNull("r.Code")).Add((ICriterion) Restrictions.Eq("r.Parent", (object) parent)).AddOrder(Order.Asc("Name")).SetCacheable(true).List<Location>();
    }))), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler);

    protected override void OnRequestRefresh()
    {
      if (this.m_project != null && this.m_series != null && this.m_year != null)
      {
        this.txtProjectName.Enabled = this.m_year.ConfigEnabled;
        this.txtProjectName.Text = this.m_project.Name;
        this.txtSeriesName.Enabled = this.m_year.ConfigEnabled;
        this.txtSeriesName.Text = this.m_series.Id;
        this.txtSeriesYear.Enabled = this.m_year.ConfigEnabled;
        this.txtSeriesYear.Text = this.m_year.Id.ToString();
      }
      base.OnRequestRefresh();
    }

    private void InitYearOptions()
    {
      this.ntbPopulation.Enabled = this.m_year.ConfigEnabled;
      this.ntbPopDensity.Enabled = this.m_year.ConfigEnabled;
      this.txtWeatherStation.Enabled = this.m_year.ConfigEnabled;
      this.cmbWeatherYear.Enabled = this.m_year.ConfigEnabled;
      this.chkUrban.AutoCheck = this.m_year.ConfigEnabled;
      this.chkUseTropicalEq.AutoCheck = this.m_year.ConfigEnabled;
      this.chkCrownHealth.Checked = this.m_year.RecordCrownCondition;
      this.chkCrownHealth.AutoCheck = this.m_year.ConfigEnabled;
      this.flpCrownHealth.Enabled = this.m_year.RecordCrownCondition;
      this.rdoCondition.Checked = this.m_year.DisplayCondition;
      this.rdoCondition.AutoCheck = this.m_year.ConfigEnabled;
      this.rdoDieback.Checked = !this.m_year.DisplayCondition;
      this.rdoDieback.AutoCheck = this.m_year.ConfigEnabled;
      this.rdoUnitsEnglish.Checked = this.m_year.Unit == YearUnit.English;
      this.rdoUnitsEnglish.AutoCheck = this.m_year.ConfigEnabled;
      this.rdoUnitsMetric.Checked = this.m_year.Unit == YearUnit.Metric;
      this.rdoUnitsMetric.AutoCheck = this.m_year.ConfigEnabled;
      this.chkCrownWidth.Checked = this.m_year.RecordCrownSize;
      this.chkCrownWidth.AutoCheck = this.m_year.ConfigEnabled;
      this.chkCLE.Checked = this.m_year.RecordCLE;
      this.chkCLE.AutoCheck = this.m_year.ConfigEnabled;
      this.chkEnergyEffect.Checked = this.m_year.RecordEnergy;
      this.chkEnergyEffect.AutoCheck = this.m_year.ConfigEnabled;
      this.chkTreeUserId.Checked = this.m_year.RecordTreeUserId;
      this.chkTreeUserId.AutoCheck = this.m_year.ConfigEnabled;
      this.chkTreeStatus.Checked = this.m_year.RecordTreeStatus;
      this.chkTreeStatus.AutoCheck = this.m_year.ConfigEnabled;
      this.chkHeight.Checked = this.m_year.RecordHeight;
      this.chkHeight.AutoCheck = this.m_year.ConfigEnabled;
      this.chkIPED.Checked = this.m_year.RecordIPED;
      this.chkIPED.AutoCheck = this.m_year.ConfigEnabled;
      this.chkShrubs.Checked = this.m_year.RecordShrub;
      this.chkShrubs.AutoCheck = this.m_year.ConfigEnabled;
      this.chkPercentPlantable.Checked = this.m_year.RecordPlantableSpace;
      this.chkPercentPlantable.AutoCheck = this.m_year.ConfigEnabled;
      this.chkHydro.Checked = this.m_year.RecordHydro;
      this.chkHydro.AutoCheck = this.m_year.ConfigEnabled;
      this.cmbDBHMeasurement.SelectedIndex = this.m_year.DBHActual ? 0 : 1;
      this.chkPlotCenter.Checked = this.m_year.RecordPlotCenter;
      this.chkPlotCenter.AutoCheck = this.m_year.ConfigEnabled;
      this.chkGroundCover.Checked = this.m_year.RecordGroundCover;
      this.chkGroundCover.AutoCheck = this.m_year.ConfigEnabled;
      this.chkPercentShrubCover.Checked = this.m_year.RecordPercentShrub;
      this.chkPercentShrubCover.AutoCheck = this.m_year.ConfigEnabled;
      this.chkSiteType.Checked = this.m_year.RecordStreetTree;
      this.chkSiteType.AutoCheck = this.m_year.ConfigEnabled;
      if (this.m_year.RecordStreetTree)
      {
        this.cmbStreetTree.SelectedIndex = this.m_year.DefaultStreetTree ? 1 : 0;
        this.cmbStreetTree.Enabled = this.m_year.ConfigEnabled;
      }
      else
      {
        this.cmbStreetTree.SelectedIndex = 0;
        this.cmbStreetTree.Enabled = false;
      }
      this.chkReferenceObjects.Checked = this.m_year.RecordReferenceObjects;
      this.chkReferenceObjects.AutoCheck = this.m_year.ConfigEnabled;
      this.chkTreeGPS.Checked = this.m_year.RecordTreeGPS;
      this.chkTreeGPS.AutoCheck = this.m_year.ConfigEnabled;
      if (this.m_series.SampleType != SampleType.Inventory)
      {
        this.chkPlotGPS.Checked = this.m_year.RecordGPS;
        this.chkPlotGPS.AutoCheck = this.m_year.ConfigEnabled;
      }
      else
      {
        this.chkStrata.Checked = this.m_year.RecordStrata;
        this.chkStrata.AutoCheck = this.m_year.ConfigEnabled;
        this.chkTreeAddress.Checked = this.m_year.RecordTreeAddress;
        this.chkTreeAddress.AutoCheck = this.m_year.ConfigEnabled;
      }
      this.chkLandUseTree.Checked = this.m_year.RecordLanduse;
      this.chkLandUseTree.AutoCheck = this.m_year.ConfigEnabled;
      this.chkPlotLandUse.Checked = this.m_year.RecordLanduse;
      this.chkPlotLandUse.AutoCheck = this.m_year.ConfigEnabled;
      this.chkTreeLocationType.Checked = this.m_year.RecordLocSite;
      this.chkTreeLocationType.AutoCheck = this.m_year.ConfigEnabled;
      this.chkSiteNumber.Checked = this.m_year.RecordLocNo;
      this.chkSiteNumber.AutoCheck = this.m_year.ConfigEnabled;
      this.chkMaintRec.Checked = this.m_year.RecordMaintRec;
      this.chkMaintRec.AutoCheck = this.m_year.ConfigEnabled;
      this.chkMaintTask.Checked = this.m_year.RecordMaintTask;
      this.chkMaintTask.AutoCheck = this.m_year.ConfigEnabled;
      this.chkOverheadUtilityConflict.Checked = this.m_year.RecordWireConflict;
      this.chkOverheadUtilityConflict.AutoCheck = this.m_year.ConfigEnabled;
      this.chkSidewalkConflict.Checked = this.m_year.RecordSidewalk;
      this.chkSidewalkConflict.AutoCheck = this.m_year.ConfigEnabled;
      this.chkPlotAddress.Checked = this.m_year.RecordPlotAddress;
      this.chkPlotAddress.AutoCheck = this.m_year.ConfigEnabled;
      this.chkPublic.Checked = this.m_year.MgmtStyle.HasValue;
      this.chkPublic.AutoCheck = this.m_year.ConfigEnabled;
      MgmtStyleEnum? mgmtStyle = this.m_year.MgmtStyle;
      if (mgmtStyle.HasValue)
      {
        mgmtStyle = this.m_year.MgmtStyle;
        int num1 = (int) mgmtStyle.Value;
        MgmtStyleEnum mgmtStyleEnum = (MgmtStyleEnum) (num1 & 1);
        int num2 = (int) (short) (num1 & 2);
        this.cmbPublicPrivate.SelectedIndex = (int) mgmtStyleEnum;
        this.cmbPublicPrivate.Enabled = this.m_year.ConfigEnabled;
      }
      else
      {
        this.cmbPublicPrivate.SelectedIndex = 0;
        this.cmbPublicPrivate.Enabled = false;
      }
      this.chkOtherOne.Checked = this.m_year.RecordOtherOne;
      this.chkOtherOne.AutoCheck = this.m_year.ConfigEnabled;
      this.chkOtherTwo.Checked = this.m_year.RecordOtherTwo;
      this.chkOtherTwo.AutoCheck = this.m_year.ConfigEnabled;
      this.chkOtherThree.Checked = this.m_year.RecordOtherThree;
      this.chkOtherThree.AutoCheck = this.m_year.ConfigEnabled;
      this.txtOtherOne.Enabled = this.m_year.ConfigEnabled && this.m_year.RecordOtherOne;
      this.txtOtherOne.Text = this.m_year.OtherOne;
      this.txtOtherTwo.Enabled = this.m_year.ConfigEnabled && this.m_year.RecordOtherTwo;
      this.txtOtherTwo.Text = this.m_year.OtherTwo;
      this.txtOtherThree.Enabled = this.m_year.ConfigEnabled && this.m_year.RecordOtherThree;
      this.txtOtherThree.Text = this.m_year.OtherThree;
      this.cmdCancel.Visible = this.m_year.ConfigEnabled;
      this.cmdShowMap.Visible = this.m_year.ConfigEnabled;
    }

    private Task FetchPopulation(int LocationId, TaskScheduler context) => Task.Factory.StartNew<Location>((Func<Location>) (() => RetryExecutionHandler.Execute<Location>((Func<Location>) (() =>
    {
      using (ISession session = this.m_ps.LocSp.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          Location location = session.Get<Location>((object) LocationId);
          transaction.Commit();
          return location;
        }
      }
    }))), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<Location>>) (t =>
    {
      Location result = t.Result;
      double? nullable1;
      if (!result.Population.HasValue || !result.Area.HasValue)
      {
        nullable1 = new double?();
      }
      else
      {
        int? population = result.Population;
        double? nullable2 = population.HasValue ? new double?((double) population.GetValueOrDefault()) : new double?();
        double? area = result.Area;
        double num = 1000000.0;
        double? nullable3 = area.HasValue ? new double?(area.GetValueOrDefault() / num) : new double?();
        nullable1 = nullable2.HasValue & nullable3.HasValue ? new double?(nullable2.GetValueOrDefault() / nullable3.GetValueOrDefault()) : new double?();
      }
      double? nullable4 = nullable1;
      Interlocked.Increment(ref this.m_changes);
      NumericTextBox ntbPopulation = this.ntbPopulation;
      int? population1 = result.Population;
      ref int? local = ref population1;
      string str = (local.HasValue ? local.GetValueOrDefault().ToString() : (string) null) ?? string.Empty;
      ntbPopulation.Text = str;
      this.ntbPopDensity.Text = nullable4?.ToString() ?? string.Empty;
      Interlocked.Decrement(ref this.m_changes);
    }), context);

    private Task LoadDataYears(int LocationId, TaskScheduler context)
    {
      Task<IList<int>> tPollutantYears = Task.Factory.StartNew<IList<int>>((Func<IList<int>>) (() => this.GetPollutionYears(LocationId)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler);
      Task<IList<int>> tWeatherYears = Task.Factory.StartNew<IList<int>>((Func<IList<int>>) (() => this._weatherService.GetWeatherYears()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler);
      return Task.WhenAll<IList<int>>(new Task<IList<int>>[2]
      {
        tPollutantYears,
        tWeatherYears
      }).ContinueWith((System.Action<Task<IList<int>[]>>) (t =>
      {
        if (t.IsFaulted || this.IsDisposed)
          return;
        IList<int> result1 = tPollutantYears.Result;
        IList<int> result2 = tWeatherYears.Result;
        Interlocked.Increment(ref this.m_changes);
        List<ComboBoxItem> dataSource = new List<ComboBoxItem>();
        foreach (int v in (IEnumerable<int>) result2)
        {
          if (result1.Contains(v))
            dataSource.Add(new ComboBoxItem((short) v, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtYearWeatherPollution, (object) v), false));
          else
            dataSource.Add(new ComboBoxItem((short) v, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtYearWeatherOnly, (object) v), true));
        }
        this.cmbWeatherYear.BindTo((object) new BindingSource((object) dataSource, (string) null), "description", "value");
        this.cmbWeatherYear.SelectedIndex = 0;
        this.EnableYearData(true);
        Interlocked.Decrement(ref this.m_changes);
      }), context);
    }

    private IList<int> GetPollutionYears(int LocationId) => RetryExecutionHandler.Execute<IList<int>>((Func<IList<int>>) (() =>
    {
      using (ISession session = this.m_ps.LocSp.OpenSession())
      {
        IList<int> pollutionYears;
        do
        {
          pollutionYears = session.CreateCriteria<PollutantStationPollutant>().CreateAlias("Locations", "l").Add((ICriterion) Restrictions.Eq("l.Id", (object) LocationId)).SetProjection(Projections.Distinct((IProjection) Projections.Property("MonYear"))).AddOrder(Order.Asc("MonYear")).SetCacheable(true).List<int>();
          if (pollutionYears.Count == 0)
          {
            IList<Location> locationList = session.CreateCriteria<Location>().CreateAlias("Children", "c").CreateAlias("c.Location", "l").Add((ICriterion) Restrictions.IsNotNull("c.Code")).Add((ICriterion) Restrictions.Eq("l.Id", (object) LocationId)).SetCacheable(true).List<Location>();
            if (locationList.Count > 0)
              LocationId = locationList[0].Id;
          }
        }
        while (pollutionYears.Count == 0 && LocationId != 1);
        return pollutionYears;
      }
    }));

    private Task SetLocationInfo(Location location, TaskScheduler context) => this.LoadDataYears(location.Id, context).ContinueWith((System.Action<Task>) (t =>
    {
      if (t.IsFaulted || this.IsDisposed || this.m_yearLocData == null)
        return;
      int? nullable1 = new int?(this.m_yearLocData.Population != 0 ? this.m_yearLocData.Population : 0);
      double? area = location.Area;
      PopulationDensity populationDensity = this.m_year.PopulationDensity;
      double? nullable2;
      if (populationDensity == null)
      {
        if (!nullable1.HasValue || !area.HasValue)
        {
          nullable2 = new double?();
        }
        else
        {
          int? nullable3 = nullable1;
          double? nullable4 = nullable3.HasValue ? new double?((double) nullable3.GetValueOrDefault()) : new double?();
          double? nullable5 = area;
          double num = 1000000.0;
          double? nullable6 = nullable5.HasValue ? new double?(nullable5.GetValueOrDefault() / num) : new double?();
          if (!(nullable4.HasValue & nullable6.HasValue))
          {
            nullable5 = new double?();
            nullable2 = nullable5;
          }
          else
            nullable2 = new double?(nullable4.GetValueOrDefault() / nullable6.GetValueOrDefault());
        }
      }
      else
        nullable2 = new double?(populationDensity.Price);
      double? nullable7 = nullable2;
      Interlocked.Increment(ref this.m_changes);
      this.cmbWeatherYear.SelectedValue = (object) this.m_yearLocData.WeatherYear;
      this.ntbPopulation.Text = this.m_yearLocData.Population.ToString();
      this.ntbPopDensity.Text = nullable7.ToString();
      this.txtWeatherStation.Text = this.m_yearLocData.WeatherStationId;
      Interlocked.Decrement(ref this.m_changes);
    }), context);

    private Task SetNation(TaskScheduler context) => Task.Factory.StartNew<Location>((Func<Location>) (() => RetryExecutionHandler.Execute<Location>((Func<Location>) (() =>
    {
      using (ISession session = this.m_ps.LocSp.OpenSession())
        return session.QueryOver<LocationRelation>().Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Code == this.m_project.NationCode)).Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => (int) r.Level == 2)).Cacheable().SingleOrDefault()?.Location;
    }))), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<Location>>) (t =>
    {
      Location result = t.Result;
      if (result == null)
        return;
      this.m_nationId = result.Id;
      Interlocked.Increment(ref this.m_changes);
      this.cmbNation.SelectedValue = (object) result.Id;
      this.cmbNation.Enabled = this.m_year.ConfigEnabled;
      Interlocked.Decrement(ref this.m_changes);
    }), context);

    private Location GetLocationChildWithCode(Location loc, string code, ISession s) => s.QueryOver<Location>().JoinQueryOver<LocationRelation>((System.Linq.Expressions.Expression<Func<Location, IEnumerable<LocationRelation>>>) (l => l.Relations)).Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Code == code)).And((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Parent == loc)).Cacheable().SingleOrDefault();

    private Task SetState(Location nation, TaskScheduler context) => Task.Factory.StartNew<Location>((Func<Location>) (() => RetryExecutionHandler.Execute<Location>((Func<Location>) (() =>
    {
      using (ISession s = this.m_ps.LocSp.OpenSession())
      {
        Regex regex = new Regex("^0+$");
        string primaryPartitionCode = this.m_project.PrimaryPartitionCode;
        Location locationChildWithCode = this.GetLocationChildWithCode(nation, primaryPartitionCode, s);
        if (locationChildWithCode == null && regex.IsMatch(primaryPartitionCode))
        {
          string secondaryPartitionCode = this.m_project.SecondaryPartitionCode;
          locationChildWithCode = this.GetLocationChildWithCode(nation, secondaryPartitionCode, s);
          if (locationChildWithCode == null && regex.IsMatch(secondaryPartitionCode))
            locationChildWithCode = this.GetLocationChildWithCode(nation, this.m_project.TertiaryPartitionCode, s);
        }
        return locationChildWithCode;
      }
    }))), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<Location>>) (t =>
    {
      Location result = t.Result;
      Interlocked.Increment(ref this.m_changes);
      if (result != null)
      {
        this.m_stateId = result.Id;
        this.cmbState.SelectedValue = (object) result.Id;
      }
      else
        this.cmbState.SelectedIndex = -1;
      Interlocked.Decrement(ref this.m_changes);
    }), context);

    private Task SetCounty(Location state, TaskScheduler context) => Task.Factory.StartNew<Location>((Func<Location>) (() => RetryExecutionHandler.Execute<Location>((Func<Location>) (() =>
    {
      using (ISession s = this.m_ps.LocSp.OpenSession())
      {
        Regex regex = new Regex("^0+$");
        string secondaryPartitionCode = this.m_project.SecondaryPartitionCode;
        Location locationChildWithCode = this.GetLocationChildWithCode(state, secondaryPartitionCode, s);
        if (locationChildWithCode == null && regex.IsMatch(secondaryPartitionCode))
          locationChildWithCode = this.GetLocationChildWithCode(state, this.m_project.TertiaryPartitionCode, s);
        return locationChildWithCode;
      }
    }))), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<Location>>) (t =>
    {
      Location result = t.Result;
      Interlocked.Increment(ref this.m_changes);
      if (result != null)
      {
        this.m_countyId = result.Id;
        this.cmbCounty.SelectedValue = (object) result.Id;
      }
      else
        this.cmbCounty.SelectedIndex = -1;
      Interlocked.Decrement(ref this.m_changes);
    }), context);

    private Task SetPlace(Location county, TaskScheduler context) => Task.Factory.StartNew<Location>((Func<Location>) (() => RetryExecutionHandler.Execute<Location>((Func<Location>) (() =>
    {
      using (ISession session = this.m_ps.LocSp.OpenSession())
        return session.QueryOver<LocationRelation>().Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Code == this.m_project.TertiaryPartitionCode)).Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Parent == county)).Cacheable().SingleOrDefault()?.Location;
    }))), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<Location>>) (t =>
    {
      Location result = t.Result;
      Interlocked.Increment(ref this.m_changes);
      if (result != null)
      {
        this.m_placeId = result.Id;
        this.cmbPlace.SelectedValue = (object) result.Id;
      }
      else
        this.cmbPlace.SelectedIndex = -1;
      Interlocked.Decrement(ref this.m_changes);
    }), context);

    private void tlpLegend_Paint(object sender, PaintEventArgs e)
    {
      Graphics graphics = e.Graphics;
      Pen black = Pens.Black;
      Rectangle clientRectangle = this.tlpLegend.ClientRectangle;
      int left = clientRectangle.Left;
      clientRectangle = this.tlpLegend.ClientRectangle;
      int top = clientRectangle.Top;
      clientRectangle = this.tlpLegend.ClientRectangle;
      int width = clientRectangle.Width - 1;
      clientRectangle = this.tlpLegend.ClientRectangle;
      int height = clientRectangle.Height - 1;
      graphics.DrawRectangle(black, left, top, width, height);
    }

    private void rdoUnits_CheckedChanged(object sender, EventArgs e) => this.rdoUnits_Validating(sender, new CancelEventArgs());

    private void txtWeatherStation_Validating(object sender, CancelEventArgs e)
    {
      string station_id = this.txtWeatherStation.Text.Trim();
      if (!this.txtWeatherStation.Enabled)
        return;
      if (string.IsNullOrEmpty(station_id))
      {
        this.ep.SetError((Control) this.txtWeatherStation, string.Empty);
      }
      else
      {
        short? selectedValue = (short?) this.cmbWeatherYear.SelectedValue;
        if (selectedValue.HasValue)
        {
          WeatherStationValidator stationValidator = new WeatherStationValidator();
          int error = stationValidator.Validate((int) selectedValue.Value, station_id);
          switch (error)
          {
            case 1:
              this.ep.SetError((Control) this.txtWeatherStation, stationValidator.TranslateError(error));
              break;
            case 2:
              int num = (int) MessageBox.Show((IWin32Window) this, stationValidator.TranslateError(error), Application.ProductName, MessageBoxButtons.OK);
              this.ep.SetError((Control) this.txtWeatherStation, string.Empty);
              break;
            default:
              this.ep.SetError((Control) this.txtWeatherStation, string.Empty);
              break;
          }
        }
        else
          this.ep.SetError((Control) this.txtWeatherStation, i_Tree_Eco_v6.Resources.Strings.ErrWeatherStationInvalid);
      }
    }

    private void cmbNation_Validating(object sender, CancelEventArgs e)
    {
      if (this.m_year == null || !this.m_year.ConfigEnabled)
        return;
      this.ep.SetError((Control) this.cmbNation, !this._bindingNation && this.cmbNation.SelectedIndex == -1, i_Tree_Eco_v6.Resources.Strings.ErrNation);
    }

    private void cmbWeatherYear_Validating(object sender, CancelEventArgs e)
    {
      if (this.m_year == null || !this.m_year.ConfigEnabled)
        return;
      if (this.cmbWeatherYear.SelectedItem != null)
      {
        bool flag = !((ComboBoxItem) this.cmbWeatherYear.SelectedItem).WeatherOnly;
        this.btnShowPollutionStations.Enabled = flag;
        if (this.currMap != null)
        {
          if (flag)
            this.btnShowPollutionStations.PerformClick();
          else
            this.currMap.Close();
        }
      }
      this.ep.SetError((Control) this.cmbWeatherYear, this.cmbWeatherYear.Enabled && this.cmbWeatherYear.SelectedIndex == -1, i_Tree_Eco_v6.Resources.Strings.ErrYear);
    }

    private void ntbPopulation_Validating(object sender, CancelEventArgs e)
    {
      if (this.m_year == null || !this.m_year.ConfigEnabled)
        return;
      this.ep.SetError((Control) this.ntbPopulation, string.IsNullOrEmpty(this.ntbPopulation.Text) || int.Parse(this.ntbPopulation.Text) == 0, i_Tree_Eco_v6.Resources.Strings.ErrLocationPopulation);
    }

    private void ntbPopDensity_Validating(object sender, CancelEventArgs e)
    {
      if (this.m_year == null || !this.m_year.ConfigEnabled || !this.ntbPopDensity.Visible)
        return;
      this.ep.SetError((Control) this.ntbPopDensity, string.IsNullOrEmpty(this.ntbPopDensity.Text) || double.Parse(this.ntbPopDensity.Text) == 0.0, i_Tree_Eco_v6.Resources.Strings.ErrLocationPopulationDensity);
    }

    private void rdoUnits_Validating(object sender, CancelEventArgs e)
    {
      if (this.m_year == null || !this.m_year.ConfigEnabled)
        return;
      bool hasError = !this.rdoUnitsEnglish.Checked && !this.rdoUnitsMetric.Checked;
      this.ep.SetError((Control) this.rdoUnitsEnglish, hasError, i_Tree_Eco_v6.Resources.Strings.ErrProjectUnits);
      this.ep.SetError((Control) this.rdoUnitsMetric, hasError, i_Tree_Eco_v6.Resources.Strings.ErrProjectUnits);
    }

    private void txtWeatherStation_TextChanged(object sender, EventArgs e)
    {
      this.tmValidateWeatherStation.Stop();
      this.tmValidateWeatherStation.Start();
    }

    private void cmbPublicPrivate_Validating(object sender, CancelEventArgs e)
    {
      if (this.m_year == null || !this.m_year.ConfigEnabled)
        return;
      this.ep.SetError((Control) this.cmbPublicPrivate, this.cmbPublicPrivate.Enabled && this.cmbPublicPrivate.SelectedIndex == -1, i_Tree_Eco_v6.Resources.Strings.ErrPublicPrivate);
    }

    private void cmbStreetTree_Validating(object sender, CancelEventArgs e)
    {
      if (this.m_year == null || !this.m_year.ConfigEnabled)
        return;
      this.ep.SetError((Control) this.cmbStreetTree, this.cmbStreetTree.Enabled && this.cmbStreetTree.SelectedIndex == -1, i_Tree_Eco_v6.Resources.Strings.ErrStreetTree);
    }

    private void txtCustomField_TextChanged(object sender, EventArgs e)
    {
      if (!(sender as TextBox).Focused)
        return;
      this.customFields_Validating(sender, new CancelEventArgs());
    }

    private void txtProjectName_TextChanged(object sender, EventArgs e)
    {
      if (!this.txtProjectName.Focused)
        return;
      this.txtProjectName_Validating(sender, new CancelEventArgs());
    }

    private void txtSeriesName_TextChanged(object sender, EventArgs e)
    {
      if (!this.txtSeriesName.Focused)
        return;
      this.txtSeriesName_Validating(sender, new CancelEventArgs());
    }

    private void txtSeriesYear_TextChanged(object sender, EventArgs e)
    {
      if (!this.txtSeriesName.Focused)
        return;
      this.txtSeriesName_Validating(sender, new CancelEventArgs());
    }

    private void tmValidateWeatherStation_Tick(object sender, EventArgs e)
    {
      this.tmValidateWeatherStation.Stop();
      this.txtWeatherStation_Validating((object) this.txtWeatherStation, new CancelEventArgs());
    }

    private void chkStrata_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkStrata.Checked || this.m_year.Strata.Count <= 1)
        return;
      int num = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrStrata, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.chkStrata.Checked = true;
    }

    private void chkCrownHealth_CheckedChanged(object sender, EventArgs e) => this.flpCrownHealth.Enabled = this.chkCrownHealth.Checked;

    private void rdoCrownHealth_Validating(object sender, CancelEventArgs e)
    {
      if (this.m_year == null || !this.m_year.RecordCrownCondition)
        return;
      bool hasError = !this.rdoCondition.Checked && !this.rdoDieback.Checked;
      this.ep.SetError((Control) this.rdoCondition, hasError, i_Tree_Eco_v6.Resources.Strings.ErrCrownHealth);
      this.ep.SetError((Control) this.rdoDieback, hasError, i_Tree_Eco_v6.Resources.Strings.ErrCrownHealth);
    }

    private void richTextLabel1_LinkClicked(object sender, LinkClickedEventArgs e)
    {
      try
      {
        Process.Start(e.LinkText);
      }
      catch (Win32Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrOpenRTFLink, (object) ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void cmbLocation_KeyDown(object sender, KeyEventArgs e)
    {
      if (!(sender is ComboBox comboBox) || e.KeyCode != Keys.Delete)
        return;
      comboBox.SelectedIndex = -1;
      e.Handled = true;
    }

    private void cmbState_Format(object sender, ListControlConvertEventArgs e)
    {
      Location location = e.Value as Location;
      if (this._formatState)
        e.Value = (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) location.Name, (object) EnumHelper.GetDescription<LocationType>(location.Type));
      else
        e.Value = (object) location.Name;
    }

    private void cmbCounty_Format(object sender, ListControlConvertEventArgs e)
    {
      Location location = e.Value as Location;
      if (this._formatCounty)
        e.Value = (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) location.Name, (object) EnumHelper.GetDescription<LocationType>(location.Type));
      else
        e.Value = (object) location.Name;
    }

    private void cmbPlace_Format(object sender, ListControlConvertEventArgs e)
    {
      Location location = e.Value as Location;
      if (this._formatState)
        e.Value = (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) location.Name, (object) EnumHelper.GetDescription<LocationType>(location.Type));
      else
        e.Value = (object) location.Name;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ProjectOverviewForm));
      this.lblHeader = new Label();
      this.tabControl1 = new TabControl();
      this.tpProjectInfo = new TabPage();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.label9 = new Label();
      this.label7 = new Label();
      this.label4 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.txtSeriesName = new TextBox();
      this.label5 = new Label();
      this.label6 = new Label();
      this.txtSeriesYear = new TextBox();
      this.label8 = new Label();
      this.txtProjectName = new TextBox();
      this.lblInventoryType = new Label();
      this.tpProjectLocation = new TabPage();
      this.pnlLocation = new TableLayoutPanel();
      this.btnShowPollutionStations = new Button();
      this.rtlUseTropicalEq = new RichTextLabel();
      this.chkUseTropicalEq = new CheckBox();
      this.lblUseTropicalEq = new Label();
      this.lblSelDataYear = new Label();
      this.lblSelWeatherStation = new Label();
      this.cmbPlace = new ComboBox();
      this.cmbCounty = new ComboBox();
      this.cmbState = new ComboBox();
      this.label11 = new Label();
      this.lblState = new Label();
      this.lblCounty = new Label();
      this.lblPlace = new Label();
      this.cmbNation = new ComboBox();
      this.lblDataYear = new Label();
      this.cmbWeatherYear = new ComboBox();
      this.lblWeatherStation = new Label();
      this.txtWeatherStation = new TextBox();
      this.cmdShowMap = new Button();
      this.lblNation = new Label();
      this.label29 = new Label();
      this.chkUrban = new CheckBox();
      this.label33 = new Label();
      this.ntbPopulation = new NumericTextBox();
      this.pnlDataRequired = new TableLayoutPanel();
      this.rtlDataRequired = new RichTextLabel();
      this.pbInfo = new PictureBox();
      this.label16 = new Label();
      this.rtlInternationalLocationWarning = new RichTextLabel();
      this.rtlLocationIssue = new RichTextLabel();
      this.rtlWeatherData = new RichTextLabel();
      this.lblPopDensity = new Label();
      this.ntbPopDensity = new NumericTextBox();
      this.tpProjectOptions = new TabPage();
      this.pnlProjectOptions = new TableLayoutPanel();
      this.label10 = new Label();
      this.label27 = new Label();
      this.label25 = new Label();
      this.label24 = new Label();
      this.label23 = new Label();
      this.label12 = new Label();
      this.flpPlotGeneralFields = new FlowLayoutPanel();
      this.chkPlotLandUse = new WrappingCheckBox();
      this.label41 = new Label();
      this.label42 = new Label();
      this.label43 = new Label();
      this.chkPercentPlantable = new WrappingCheckBox();
      this.chkPlotAddress = new WrappingCheckBox();
      this.chkPlotGPS = new WrappingCheckBox();
      this.label34 = new Label();
      this.label1 = new Label();
      this.label36 = new Label();
      this.chkReferenceObjects = new WrappingCheckBox();
      this.label52 = new Label();
      this.label53 = new Label();
      this.label54 = new Label();
      this.label55 = new Label();
      this.flpPlotGeneralFieldsExt = new FlowLayoutPanel();
      this.chkGroundCover = new WrappingCheckBox();
      this.label45 = new Label();
      this.label46 = new Label();
      this.label17 = new Label();
      this.chkPercentShrubCover = new WrappingCheckBox();
      this.chkShrubs = new WrappingCheckBox();
      this.label47 = new Label();
      this.label48 = new Label();
      this.label49 = new Label();
      this.label50 = new Label();
      this.label51 = new Label();
      this.flpPlotMinimumRequiredFields = new FlowLayoutPanel();
      this.chkPlotPercentMeasured = new WrappingCheckBox();
      this.chkPlotPercentTreeCover = new WrappingCheckBox();
      this.flowLayoutPanel6 = new FlowLayoutPanel();
      this.chkHeight = new WrappingCheckBox();
      this.chkCrownWidth = new WrappingCheckBox();
      this.label30 = new Label();
      this.label31 = new Label();
      this.label32 = new Label();
      this.label37 = new Label();
      this.chkCrownHealth = new WrappingCheckBox();
      this.flpCrownHealth = new FlowLayoutPanel();
      this.rdoDieback = new RadioButton();
      this.rdoCondition = new RadioButton();
      this.chkCLE = new WrappingCheckBox();
      this.chkEnergyEffect = new WrappingCheckBox();
      this.label15 = new Label();
      this.label38 = new Label();
      this.flowLayoutPanel7 = new FlowLayoutPanel();
      this.chkMaintRec = new WrappingCheckBox();
      this.chkMaintTask = new WrappingCheckBox();
      this.chkSidewalkConflict = new WrappingCheckBox();
      this.chkOverheadUtilityConflict = new WrappingCheckBox();
      this.chkIPED = new WrappingCheckBox();
      this.label56 = new Label();
      this.label35 = new Label();
      this.label57 = new Label();
      this.label58 = new Label();
      this.chkTreeUserId = new WrappingCheckBox();
      this.label18 = new Label();
      this.label28 = new Label();
      this.tableLayoutPanel5 = new TableLayoutPanel();
      this.cmbDBHMeasurement = new ComboBox();
      this.chkDBH = new WrappingCheckBox();
      this.chkSpecies = new WrappingCheckBox();
      this.tableLayoutPanel6 = new TableLayoutPanel();
      this.chkTreeAddress = new WrappingCheckBox();
      this.lblCoverPercentShrub = new Label();
      this.lblCoverPercentImpervious = new Label();
      this.chkHydro = new WrappingCheckBox();
      this.cmbPublicPrivate = new ComboBox();
      this.chkPublic = new WrappingCheckBox();
      this.chkTreeGPS = new WrappingCheckBox();
      this.cmbStreetTree = new ComboBox();
      this.chkSiteType = new WrappingCheckBox();
      this.chkPlotCenter = new WrappingCheckBox();
      this.chkTreeStatus = new WrappingCheckBox();
      this.chkStrata = new CheckBox();
      this.lblPlotTreeLandUse = new Label();
      this.chkLandUseTree = new WrappingCheckBox();
      this.lblStrataInfo1 = new Label();
      this.lblStrataInfo2 = new Label();
      this.flowLayoutPanel5 = new FlowLayoutPanel();
      this.label21 = new Label();
      this.tableLayoutPanel7 = new TableLayoutPanel();
      this.label26 = new Label();
      this.chkSiteNumber = new WrappingCheckBox();
      this.chkTreeLocationType = new WrappingCheckBox();
      this.tlpLegend = new TableLayoutPanel();
      this.lblOptionalFields = new Label();
      this.lblRecommendedFields = new Label();
      this.lblRequiredFields = new Label();
      this.label13 = new Label();
      this.label14 = new Label();
      this.label22 = new Label();
      this.lblPlotMinimumRequired = new Label();
      this.lblPlotInformation = new Label();
      this.lblPlotGeneralFields = new Label();
      this.tableLayoutPanel3 = new TableLayoutPanel();
      this.txtOtherThree = new TextBox();
      this.txtOtherTwo = new TextBox();
      this.txtOtherOne = new TextBox();
      this.chkOtherThree = new CheckBox();
      this.chkOtherOne = new CheckBox();
      this.chkOtherTwo = new CheckBox();
      this.pbHelpImage = new PictureBox();
      this.tableLayoutPanel2 = new TableLayoutPanel();
      this.pbUnitWarning = new PictureBox();
      this.rtlUnitWarning = new RichTextLabel();
      this.rdoUnitsEnglish = new RadioButton();
      this.rdoUnitsMetric = new RadioButton();
      this.lblAccuracyCaption = new Label();
      this.ep = new ErrorProvider(this.components);
      this.toolTip1 = new ToolTip(this.components);
      this.cmdOK = new Button();
      this.cmdCancel = new Button();
      this.tableLayoutPanel4 = new TableLayoutPanel();
      this.tmValidateWeatherStation = new System.Windows.Forms.Timer(this.components);
      this.tabControl1.SuspendLayout();
      this.tpProjectInfo.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.tpProjectLocation.SuspendLayout();
      this.pnlLocation.SuspendLayout();
      this.pnlDataRequired.SuspendLayout();
      ((ISupportInitialize) this.pbInfo).BeginInit();
      this.tpProjectOptions.SuspendLayout();
      this.pnlProjectOptions.SuspendLayout();
      this.flpPlotGeneralFields.SuspendLayout();
      this.flpPlotGeneralFieldsExt.SuspendLayout();
      this.flpPlotMinimumRequiredFields.SuspendLayout();
      this.flowLayoutPanel6.SuspendLayout();
      this.flpCrownHealth.SuspendLayout();
      this.flowLayoutPanel7.SuspendLayout();
      this.tableLayoutPanel5.SuspendLayout();
      this.tableLayoutPanel6.SuspendLayout();
      this.tableLayoutPanel7.SuspendLayout();
      this.tlpLegend.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      ((ISupportInitialize) this.pbHelpImage).BeginInit();
      this.tableLayoutPanel2.SuspendLayout();
      ((ISupportInitialize) this.pbUnitWarning).BeginInit();
      ((ISupportInitialize) this.ep).BeginInit();
      this.tableLayoutPanel4.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.lblHeader, "lblHeader");
      this.lblHeader.Name = "lblHeader";
      this.tabControl1.CausesValidation = false;
      this.tabControl1.Controls.Add((Control) this.tpProjectInfo);
      this.tabControl1.Controls.Add((Control) this.tpProjectLocation);
      this.tabControl1.Controls.Add((Control) this.tpProjectOptions);
      componentResourceManager.ApplyResources((object) this.tabControl1, "tabControl1");
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.TabStop = false;
      this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
      this.tabControl1.Deselecting += new TabControlCancelEventHandler(this.tabControl1_Deselecting);
      this.tpProjectInfo.Controls.Add((Control) this.tableLayoutPanel1);
      componentResourceManager.ApplyResources((object) this.tpProjectInfo, "tpProjectInfo");
      this.tpProjectInfo.Name = "tpProjectInfo";
      this.tpProjectInfo.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.label9, 0, 7);
      this.tableLayoutPanel1.Controls.Add((Control) this.label7, 0, 5);
      this.tableLayoutPanel1.Controls.Add((Control) this.label4, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.label2, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.label3, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtSeriesName, 1, 3);
      this.tableLayoutPanel1.Controls.Add((Control) this.label5, 0, 3);
      this.tableLayoutPanel1.Controls.Add((Control) this.label6, 0, 4);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtSeriesYear, 1, 5);
      this.tableLayoutPanel1.Controls.Add((Control) this.label8, 0, 6);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtProjectName, 1, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblInventoryType, 1, 7);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.label9, "label9");
      this.label9.Name = "label9";
      componentResourceManager.ApplyResources((object) this.label7, "label7");
      this.label7.Name = "label7";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label4, 2);
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label2, 2);
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.txtSeriesName, "txtSeriesName");
      this.txtSeriesName.Name = "txtSeriesName";
      this.txtSeriesName.TextChanged += new EventHandler(this.txtSeriesName_TextChanged);
      this.txtSeriesName.Validating += new CancelEventHandler(this.txtSeriesName_Validating);
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label6, 2);
      this.label6.Name = "label6";
      componentResourceManager.ApplyResources((object) this.txtSeriesYear, "txtSeriesYear");
      this.txtSeriesYear.Name = "txtSeriesYear";
      this.txtSeriesYear.TextChanged += new EventHandler(this.txtSeriesYear_TextChanged);
      this.txtSeriesYear.Validating += new CancelEventHandler(this.txtSeriesYear_Validating);
      componentResourceManager.ApplyResources((object) this.label8, "label8");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label8, 2);
      this.label8.Name = "label8";
      componentResourceManager.ApplyResources((object) this.txtProjectName, "txtProjectName");
      this.txtProjectName.Name = "txtProjectName";
      this.txtProjectName.TextChanged += new EventHandler(this.txtProjectName_TextChanged);
      this.txtProjectName.Validating += new CancelEventHandler(this.txtProjectName_Validating);
      componentResourceManager.ApplyResources((object) this.lblInventoryType, "lblInventoryType");
      this.lblInventoryType.Name = "lblInventoryType";
      this.tpProjectLocation.Controls.Add((Control) this.pnlLocation);
      componentResourceManager.ApplyResources((object) this.tpProjectLocation, "tpProjectLocation");
      this.tpProjectLocation.Name = "tpProjectLocation";
      this.tpProjectLocation.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.pnlLocation, "pnlLocation");
      this.pnlLocation.Controls.Add((Control) this.btnShowPollutionStations, 2, 15);
      this.pnlLocation.Controls.Add((Control) this.rtlUseTropicalEq, 2, 11);
      this.pnlLocation.Controls.Add((Control) this.chkUseTropicalEq, 1, 11);
      this.pnlLocation.Controls.Add((Control) this.lblUseTropicalEq, 0, 11);
      this.pnlLocation.Controls.Add((Control) this.lblSelDataYear, 0, 14);
      this.pnlLocation.Controls.Add((Control) this.lblSelWeatherStation, 0, 16);
      this.pnlLocation.Controls.Add((Control) this.cmbPlace, 1, 6);
      this.pnlLocation.Controls.Add((Control) this.cmbCounty, 1, 5);
      this.pnlLocation.Controls.Add((Control) this.cmbState, 1, 4);
      this.pnlLocation.Controls.Add((Control) this.label11, 0, 0);
      this.pnlLocation.Controls.Add((Control) this.lblState, 0, 4);
      this.pnlLocation.Controls.Add((Control) this.lblCounty, 0, 5);
      this.pnlLocation.Controls.Add((Control) this.lblPlace, 0, 6);
      this.pnlLocation.Controls.Add((Control) this.cmbNation, 1, 3);
      this.pnlLocation.Controls.Add((Control) this.lblDataYear, 0, 15);
      this.pnlLocation.Controls.Add((Control) this.cmbWeatherYear, 1, 15);
      this.pnlLocation.Controls.Add((Control) this.lblWeatherStation, 0, 17);
      this.pnlLocation.Controls.Add((Control) this.txtWeatherStation, 1, 17);
      this.pnlLocation.Controls.Add((Control) this.cmdShowMap, 2, 17);
      this.pnlLocation.Controls.Add((Control) this.lblNation, 0, 3);
      this.pnlLocation.Controls.Add((Control) this.label29, 0, 7);
      this.pnlLocation.Controls.Add((Control) this.chkUrban, 1, 7);
      this.pnlLocation.Controls.Add((Control) this.label33, 0, 8);
      this.pnlLocation.Controls.Add((Control) this.ntbPopulation, 1, 8);
      this.pnlLocation.Controls.Add((Control) this.pnlDataRequired, 0, 18);
      this.pnlLocation.Controls.Add((Control) this.label16, 0, 1);
      this.pnlLocation.Controls.Add((Control) this.rtlInternationalLocationWarning, 0, 2);
      this.pnlLocation.Controls.Add((Control) this.rtlLocationIssue, 2, 3);
      this.pnlLocation.Controls.Add((Control) this.rtlWeatherData, 2, 14);
      this.pnlLocation.Controls.Add((Control) this.lblPopDensity, 0, 9);
      this.pnlLocation.Controls.Add((Control) this.ntbPopDensity, 1, 9);
      this.pnlLocation.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
      this.pnlLocation.Name = "pnlLocation";
      componentResourceManager.ApplyResources((object) this.btnShowPollutionStations, "btnShowPollutionStations");
      this.btnShowPollutionStations.Name = "btnShowPollutionStations";
      this.btnShowPollutionStations.UseVisualStyleBackColor = true;
      this.btnShowPollutionStations.Click += new EventHandler(this.btnShowPollutionStations_Click);
      componentResourceManager.ApplyResources((object) this.rtlUseTropicalEq, "rtlUseTropicalEq");
      this.pnlLocation.SetColumnSpan((Control) this.rtlUseTropicalEq, 2);
      this.rtlUseTropicalEq.Name = "rtlUseTropicalEq";
      this.pnlLocation.SetRowSpan((Control) this.rtlUseTropicalEq, 2);
      this.rtlUseTropicalEq.TabStop = false;
      componentResourceManager.ApplyResources((object) this.chkUseTropicalEq, "chkUseTropicalEq");
      this.chkUseTropicalEq.Name = "chkUseTropicalEq";
      this.chkUseTropicalEq.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.lblUseTropicalEq, "lblUseTropicalEq");
      this.lblUseTropicalEq.Name = "lblUseTropicalEq";
      componentResourceManager.ApplyResources((object) this.lblSelDataYear, "lblSelDataYear");
      this.pnlLocation.SetColumnSpan((Control) this.lblSelDataYear, 2);
      this.lblSelDataYear.Name = "lblSelDataYear";
      componentResourceManager.ApplyResources((object) this.lblSelWeatherStation, "lblSelWeatherStation");
      this.pnlLocation.SetColumnSpan((Control) this.lblSelWeatherStation, 2);
      this.lblSelWeatherStation.Name = "lblSelWeatherStation";
      this.cmbPlace.DropDownStyle = ComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.cmbPlace, "cmbPlace");
      this.cmbPlace.FormattingEnabled = true;
      this.cmbPlace.Name = "cmbPlace";
      this.cmbPlace.SelectedIndexChanged += new EventHandler(this.cmbPlace_SelectedIndexChanged);
      this.cmbPlace.Format += new ListControlConvertEventHandler(this.cmbPlace_Format);
      this.cmbPlace.KeyDown += new KeyEventHandler(this.cmbLocation_KeyDown);
      this.cmbCounty.DropDownStyle = ComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.cmbCounty, "cmbCounty");
      this.cmbCounty.FormattingEnabled = true;
      this.cmbCounty.Name = "cmbCounty";
      this.cmbCounty.SelectedIndexChanged += new EventHandler(this.cmbCounty_SelectedIndexChanged);
      this.cmbCounty.Format += new ListControlConvertEventHandler(this.cmbCounty_Format);
      this.cmbCounty.KeyDown += new KeyEventHandler(this.cmbLocation_KeyDown);
      this.cmbState.DropDownStyle = ComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.cmbState, "cmbState");
      this.cmbState.FormattingEnabled = true;
      this.cmbState.Name = "cmbState";
      this.cmbState.SelectedIndexChanged += new EventHandler(this.cmbState_SelectedIndexChanged);
      this.cmbState.Format += new ListControlConvertEventHandler(this.cmbState_Format);
      this.cmbState.KeyDown += new KeyEventHandler(this.cmbLocation_KeyDown);
      componentResourceManager.ApplyResources((object) this.label11, "label11");
      this.pnlLocation.SetColumnSpan((Control) this.label11, 2);
      this.label11.Name = "label11";
      componentResourceManager.ApplyResources((object) this.lblState, "lblState");
      this.lblState.Name = "lblState";
      componentResourceManager.ApplyResources((object) this.lblCounty, "lblCounty");
      this.lblCounty.Name = "lblCounty";
      componentResourceManager.ApplyResources((object) this.lblPlace, "lblPlace");
      this.lblPlace.Name = "lblPlace";
      this.cmbNation.DropDownStyle = ComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.cmbNation, "cmbNation");
      this.cmbNation.FormattingEnabled = true;
      this.cmbNation.Name = "cmbNation";
      this.cmbNation.SelectedIndexChanged += new EventHandler(this.cmbNation_SelectedIndexChanged);
      this.cmbNation.KeyDown += new KeyEventHandler(this.cmbLocation_KeyDown);
      this.cmbNation.Validating += new CancelEventHandler(this.cmbNation_Validating);
      componentResourceManager.ApplyResources((object) this.lblDataYear, "lblDataYear");
      this.lblDataYear.Name = "lblDataYear";
      this.cmbWeatherYear.DropDownStyle = ComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.cmbWeatherYear, "cmbWeatherYear");
      this.cmbWeatherYear.FormattingEnabled = true;
      this.cmbWeatherYear.Name = "cmbWeatherYear";
      this.cmbWeatherYear.SelectedIndexChanged += new EventHandler(this.cmbWeatherYear_SelectedIndexChanged);
      this.cmbWeatherYear.Validating += new CancelEventHandler(this.cmbWeatherYear_Validating);
      componentResourceManager.ApplyResources((object) this.lblWeatherStation, "lblWeatherStation");
      this.lblWeatherStation.Name = "lblWeatherStation";
      this.txtWeatherStation.BackColor = SystemColors.Window;
      componentResourceManager.ApplyResources((object) this.txtWeatherStation, "txtWeatherStation");
      this.txtWeatherStation.Name = "txtWeatherStation";
      this.txtWeatherStation.TextChanged += new EventHandler(this.txtWeatherStation_TextChanged);
      this.txtWeatherStation.Validating += new CancelEventHandler(this.txtWeatherStation_Validating);
      componentResourceManager.ApplyResources((object) this.cmdShowMap, "cmdShowMap");
      this.cmdShowMap.Name = "cmdShowMap";
      this.cmdShowMap.UseVisualStyleBackColor = true;
      this.cmdShowMap.Click += new EventHandler(this.cmdShowMap_Click);
      componentResourceManager.ApplyResources((object) this.lblNation, "lblNation");
      this.lblNation.Name = "lblNation";
      componentResourceManager.ApplyResources((object) this.label29, "label29");
      this.label29.Name = "label29";
      componentResourceManager.ApplyResources((object) this.chkUrban, "chkUrban");
      this.chkUrban.Name = "chkUrban";
      this.chkUrban.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.label33, "label33");
      this.label33.Name = "label33";
      this.ntbPopulation.DecimalPlaces = 2;
      componentResourceManager.ApplyResources((object) this.ntbPopulation, "ntbPopulation");
      this.ntbPopulation.Format = "0;-#";
      this.ntbPopulation.HasDecimal = false;
      this.ntbPopulation.Name = "ntbPopulation";
      this.ntbPopulation.Signed = false;
      this.ntbPopulation.Validating += new CancelEventHandler(this.ntbPopulation_Validating);
      componentResourceManager.ApplyResources((object) this.pnlDataRequired, "pnlDataRequired");
      this.pnlLocation.SetColumnSpan((Control) this.pnlDataRequired, 4);
      this.pnlDataRequired.Controls.Add((Control) this.rtlDataRequired, 1, 0);
      this.pnlDataRequired.Controls.Add((Control) this.pbInfo, 0, 0);
      this.pnlDataRequired.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
      this.pnlDataRequired.Name = "pnlDataRequired";
      componentResourceManager.ApplyResources((object) this.rtlDataRequired, "rtlDataRequired");
      this.rtlDataRequired.Name = "rtlDataRequired";
      this.rtlDataRequired.TabStop = false;
      this.rtlDataRequired.LinkClicked += new LinkClickedEventHandler(this.richTextLabel1_LinkClicked);
      this.pbInfo.Image = (Image) i_Tree_Eco_v6.Properties.Resources.info;
      componentResourceManager.ApplyResources((object) this.pbInfo, "pbInfo");
      this.pbInfo.Name = "pbInfo";
      this.pbInfo.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label16, "label16");
      this.pnlLocation.SetColumnSpan((Control) this.label16, 4);
      this.label16.ForeColor = SystemColors.GrayText;
      this.label16.Name = "label16";
      componentResourceManager.ApplyResources((object) this.rtlInternationalLocationWarning, "rtlInternationalLocationWarning");
      this.pnlLocation.SetColumnSpan((Control) this.rtlInternationalLocationWarning, 4);
      this.rtlInternationalLocationWarning.Name = "rtlInternationalLocationWarning";
      this.rtlInternationalLocationWarning.TabStop = false;
      this.rtlInternationalLocationWarning.LinkClicked += new LinkClickedEventHandler(this.richTextLabel1_LinkClicked);
      componentResourceManager.ApplyResources((object) this.rtlLocationIssue, "rtlLocationIssue");
      this.pnlLocation.SetColumnSpan((Control) this.rtlLocationIssue, 2);
      this.rtlLocationIssue.Name = "rtlLocationIssue";
      this.pnlLocation.SetRowSpan((Control) this.rtlLocationIssue, 8);
      this.rtlLocationIssue.TabStop = false;
      componentResourceManager.ApplyResources((object) this.rtlWeatherData, "rtlWeatherData");
      this.pnlLocation.SetColumnSpan((Control) this.rtlWeatherData, 2);
      this.rtlWeatherData.Name = "rtlWeatherData";
      this.rtlWeatherData.TabStop = false;
      componentResourceManager.ApplyResources((object) this.lblPopDensity, "lblPopDensity");
      this.lblPopDensity.Name = "lblPopDensity";
      this.ntbPopDensity.DecimalPlaces = 2;
      componentResourceManager.ApplyResources((object) this.ntbPopDensity, "ntbPopDensity");
      this.ntbPopDensity.HasDecimal = true;
      this.ntbPopDensity.Name = "ntbPopDensity";
      this.ntbPopDensity.Signed = false;
      this.ntbPopDensity.Validating += new CancelEventHandler(this.ntbPopDensity_Validating);
      componentResourceManager.ApplyResources((object) this.tpProjectOptions, "tpProjectOptions");
      this.tpProjectOptions.Controls.Add((Control) this.pnlProjectOptions);
      this.tpProjectOptions.Name = "tpProjectOptions";
      this.tpProjectOptions.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.pnlProjectOptions, "pnlProjectOptions");
      this.pnlProjectOptions.Controls.Add((Control) this.label10, 0, 24);
      this.pnlProjectOptions.Controls.Add((Control) this.label27, 3, 19);
      this.pnlProjectOptions.Controls.Add((Control) this.label25, 2, 19);
      this.pnlProjectOptions.Controls.Add((Control) this.label24, 1, 19);
      this.pnlProjectOptions.Controls.Add((Control) this.label23, 0, 19);
      this.pnlProjectOptions.Controls.Add((Control) this.label12, 0, 18);
      this.pnlProjectOptions.Controls.Add((Control) this.flpPlotGeneralFields, 1, 7);
      this.pnlProjectOptions.Controls.Add((Control) this.flpPlotGeneralFieldsExt, 2, 7);
      this.pnlProjectOptions.Controls.Add((Control) this.flpPlotMinimumRequiredFields, 0, 7);
      this.pnlProjectOptions.Controls.Add((Control) this.flowLayoutPanel6, 2, 20);
      this.pnlProjectOptions.Controls.Add((Control) this.flowLayoutPanel7, 3, 20);
      this.pnlProjectOptions.Controls.Add((Control) this.label28, 0, 21);
      this.pnlProjectOptions.Controls.Add((Control) this.tableLayoutPanel5, 0, 20);
      this.pnlProjectOptions.Controls.Add((Control) this.tableLayoutPanel6, 1, 20);
      this.pnlProjectOptions.Controls.Add((Control) this.flowLayoutPanel5, 2, 21);
      this.pnlProjectOptions.Controls.Add((Control) this.label21, 0, 0);
      this.pnlProjectOptions.Controls.Add((Control) this.tableLayoutPanel7, 1, 18);
      this.pnlProjectOptions.Controls.Add((Control) this.tlpLegend, 2, 0);
      this.pnlProjectOptions.Controls.Add((Control) this.lblPlotMinimumRequired, 0, 6);
      this.pnlProjectOptions.Controls.Add((Control) this.lblPlotInformation, 0, 5);
      this.pnlProjectOptions.Controls.Add((Control) this.lblPlotGeneralFields, 1, 6);
      this.pnlProjectOptions.Controls.Add((Control) this.tableLayoutPanel3, 0, 25);
      this.pnlProjectOptions.Controls.Add((Control) this.pbHelpImage, 0, 26);
      this.pnlProjectOptions.Controls.Add((Control) this.tableLayoutPanel2, 0, 1);
      this.pnlProjectOptions.Name = "pnlProjectOptions";
      componentResourceManager.ApplyResources((object) this.label10, "label10");
      this.pnlProjectOptions.SetColumnSpan((Control) this.label10, 2);
      this.label10.Name = "label10";
      componentResourceManager.ApplyResources((object) this.label27, "label27");
      this.label27.Name = "label27";
      componentResourceManager.ApplyResources((object) this.label25, "label25");
      this.label25.Name = "label25";
      componentResourceManager.ApplyResources((object) this.label24, "label24");
      this.label24.Name = "label24";
      componentResourceManager.ApplyResources((object) this.label23, "label23");
      this.label23.Name = "label23";
      componentResourceManager.ApplyResources((object) this.label12, "label12");
      this.label12.Name = "label12";
      componentResourceManager.ApplyResources((object) this.flpPlotGeneralFields, "flpPlotGeneralFields");
      this.flpPlotGeneralFields.Controls.Add((Control) this.chkPlotLandUse);
      this.flpPlotGeneralFields.Controls.Add((Control) this.label41);
      this.flpPlotGeneralFields.Controls.Add((Control) this.label42);
      this.flpPlotGeneralFields.Controls.Add((Control) this.label43);
      this.flpPlotGeneralFields.Controls.Add((Control) this.chkPercentPlantable);
      this.flpPlotGeneralFields.Controls.Add((Control) this.chkPlotAddress);
      this.flpPlotGeneralFields.Controls.Add((Control) this.chkPlotGPS);
      this.flpPlotGeneralFields.Controls.Add((Control) this.label34);
      this.flpPlotGeneralFields.Controls.Add((Control) this.label1);
      this.flpPlotGeneralFields.Controls.Add((Control) this.label36);
      this.flpPlotGeneralFields.Controls.Add((Control) this.chkReferenceObjects);
      this.flpPlotGeneralFields.Controls.Add((Control) this.label52);
      this.flpPlotGeneralFields.Controls.Add((Control) this.label53);
      this.flpPlotGeneralFields.Controls.Add((Control) this.label54);
      this.flpPlotGeneralFields.Controls.Add((Control) this.label55);
      this.flpPlotGeneralFields.Name = "flpPlotGeneralFields";
      componentResourceManager.ApplyResources((object) this.chkPlotLandUse, "chkPlotLandUse");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.chkPlotLandUse, true);
      this.chkPlotLandUse.ForeColor = Color.Blue;
      this.chkPlotLandUse.Name = "chkPlotLandUse";
      this.chkPlotLandUse.UseVisualStyleBackColor = true;
      this.chkPlotLandUse.Click += new EventHandler(this.chkPlotLandUse_Click);
      componentResourceManager.ApplyResources((object) this.label41, "label41");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.label41, true);
      this.label41.Name = "label41";
      componentResourceManager.ApplyResources((object) this.label42, "label42");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.label42, true);
      this.label42.Name = "label42";
      componentResourceManager.ApplyResources((object) this.label43, "label43");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.label43, true);
      this.label43.Name = "label43";
      componentResourceManager.ApplyResources((object) this.chkPercentPlantable, "chkPercentPlantable");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.chkPercentPlantable, true);
      this.chkPercentPlantable.Name = "chkPercentPlantable";
      this.chkPercentPlantable.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkPlotAddress, "chkPlotAddress");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.chkPlotAddress, true);
      this.chkPlotAddress.Name = "chkPlotAddress";
      this.chkPlotAddress.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkPlotGPS, "chkPlotGPS");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.chkPlotGPS, true);
      this.chkPlotGPS.Name = "chkPlotGPS";
      this.chkPlotGPS.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.label34, "label34");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.label34, true);
      this.label34.Name = "label34";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.label1, true);
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.label36, "label36");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.label36, true);
      this.label36.Name = "label36";
      componentResourceManager.ApplyResources((object) this.chkReferenceObjects, "chkReferenceObjects");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.chkReferenceObjects, true);
      this.chkReferenceObjects.Name = "chkReferenceObjects";
      this.chkReferenceObjects.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.label52, "label52");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.label52, true);
      this.label52.Name = "label52";
      componentResourceManager.ApplyResources((object) this.label53, "label53");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.label53, true);
      this.label53.Name = "label53";
      componentResourceManager.ApplyResources((object) this.label54, "label54");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.label54, true);
      this.label54.Name = "label54";
      componentResourceManager.ApplyResources((object) this.label55, "label55");
      this.flpPlotGeneralFields.SetFlowBreak((Control) this.label55, true);
      this.label55.Name = "label55";
      componentResourceManager.ApplyResources((object) this.flpPlotGeneralFieldsExt, "flpPlotGeneralFieldsExt");
      this.pnlProjectOptions.SetColumnSpan((Control) this.flpPlotGeneralFieldsExt, 2);
      this.flpPlotGeneralFieldsExt.Controls.Add((Control) this.chkGroundCover);
      this.flpPlotGeneralFieldsExt.Controls.Add((Control) this.label45);
      this.flpPlotGeneralFieldsExt.Controls.Add((Control) this.label46);
      this.flpPlotGeneralFieldsExt.Controls.Add((Control) this.label17);
      this.flpPlotGeneralFieldsExt.Controls.Add((Control) this.chkPercentShrubCover);
      this.flpPlotGeneralFieldsExt.Controls.Add((Control) this.chkShrubs);
      this.flpPlotGeneralFieldsExt.Controls.Add((Control) this.label47);
      this.flpPlotGeneralFieldsExt.Controls.Add((Control) this.label48);
      this.flpPlotGeneralFieldsExt.Controls.Add((Control) this.label49);
      this.flpPlotGeneralFieldsExt.Controls.Add((Control) this.label50);
      this.flpPlotGeneralFieldsExt.Controls.Add((Control) this.label51);
      this.flpPlotGeneralFieldsExt.Name = "flpPlotGeneralFieldsExt";
      componentResourceManager.ApplyResources((object) this.chkGroundCover, "chkGroundCover");
      this.flpPlotGeneralFieldsExt.SetFlowBreak((Control) this.chkGroundCover, true);
      this.chkGroundCover.Name = "chkGroundCover";
      this.chkGroundCover.Tag = (object) "groundCoverCompositionByStrataMenuItem";
      this.chkGroundCover.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.label45, "label45");
      this.flpPlotGeneralFieldsExt.SetFlowBreak((Control) this.label45, true);
      this.label45.Name = "label45";
      componentResourceManager.ApplyResources((object) this.label46, "label46");
      this.flpPlotGeneralFieldsExt.SetFlowBreak((Control) this.label46, true);
      this.label46.Name = "label46";
      componentResourceManager.ApplyResources((object) this.label17, "label17");
      this.flpPlotGeneralFieldsExt.SetFlowBreak((Control) this.label17, true);
      this.label17.Name = "label17";
      componentResourceManager.ApplyResources((object) this.chkPercentShrubCover, "chkPercentShrubCover");
      this.flpPlotGeneralFieldsExt.SetFlowBreak((Control) this.chkPercentShrubCover, true);
      this.chkPercentShrubCover.Name = "chkPercentShrubCover";
      this.chkPercentShrubCover.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkShrubs, "chkShrubs");
      this.flpPlotGeneralFieldsExt.SetFlowBreak((Control) this.chkShrubs, true);
      this.chkShrubs.Name = "chkShrubs";
      this.chkShrubs.Tag = (object) "rbWeatherAirQualImprovementShrub,rbWeatherDryDepShrub,leafAreaAndBiomassOfShrubsByStrataMenuItem";
      this.chkShrubs.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.label47, "label47");
      this.flpPlotGeneralFieldsExt.SetFlowBreak((Control) this.label47, true);
      this.label47.Name = "label47";
      componentResourceManager.ApplyResources((object) this.label48, "label48");
      this.flpPlotGeneralFieldsExt.SetFlowBreak((Control) this.label48, true);
      this.label48.Name = "label48";
      componentResourceManager.ApplyResources((object) this.label49, "label49");
      this.flpPlotGeneralFieldsExt.SetFlowBreak((Control) this.label49, true);
      this.label49.Name = "label49";
      componentResourceManager.ApplyResources((object) this.label50, "label50");
      this.flpPlotGeneralFieldsExt.SetFlowBreak((Control) this.label50, true);
      this.label50.Name = "label50";
      componentResourceManager.ApplyResources((object) this.label51, "label51");
      this.flpPlotGeneralFieldsExt.SetFlowBreak((Control) this.label51, true);
      this.label51.Name = "label51";
      this.flpPlotMinimumRequiredFields.Controls.Add((Control) this.chkPlotPercentMeasured);
      this.flpPlotMinimumRequiredFields.Controls.Add((Control) this.chkPlotPercentTreeCover);
      componentResourceManager.ApplyResources((object) this.flpPlotMinimumRequiredFields, "flpPlotMinimumRequiredFields");
      this.flpPlotMinimumRequiredFields.Name = "flpPlotMinimumRequiredFields";
      this.chkPlotPercentMeasured.AutoCheck = false;
      componentResourceManager.ApplyResources((object) this.chkPlotPercentMeasured, "chkPlotPercentMeasured");
      this.chkPlotPercentMeasured.Checked = true;
      this.chkPlotPercentMeasured.CheckState = CheckState.Checked;
      this.flpPlotMinimumRequiredFields.SetFlowBreak((Control) this.chkPlotPercentMeasured, true);
      this.chkPlotPercentMeasured.ForeColor = Color.Red;
      this.chkPlotPercentMeasured.Name = "chkPlotPercentMeasured";
      this.chkPlotPercentMeasured.UseVisualStyleBackColor = true;
      this.chkPlotPercentTreeCover.AutoCheck = false;
      componentResourceManager.ApplyResources((object) this.chkPlotPercentTreeCover, "chkPlotPercentTreeCover");
      this.chkPlotPercentTreeCover.Checked = true;
      this.chkPlotPercentTreeCover.CheckState = CheckState.Checked;
      this.chkPlotPercentTreeCover.ForeColor = Color.Red;
      this.chkPlotPercentTreeCover.Name = "chkPlotPercentTreeCover";
      this.chkPlotPercentTreeCover.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.flowLayoutPanel6, "flowLayoutPanel6");
      this.flowLayoutPanel6.Controls.Add((Control) this.chkHeight);
      this.flowLayoutPanel6.Controls.Add((Control) this.chkCrownWidth);
      this.flowLayoutPanel6.Controls.Add((Control) this.label30);
      this.flowLayoutPanel6.Controls.Add((Control) this.label31);
      this.flowLayoutPanel6.Controls.Add((Control) this.label32);
      this.flowLayoutPanel6.Controls.Add((Control) this.label37);
      this.flowLayoutPanel6.Controls.Add((Control) this.chkCrownHealth);
      this.flowLayoutPanel6.Controls.Add((Control) this.flpCrownHealth);
      this.flowLayoutPanel6.Controls.Add((Control) this.chkCLE);
      this.flowLayoutPanel6.Controls.Add((Control) this.chkEnergyEffect);
      this.flowLayoutPanel6.Controls.Add((Control) this.label15);
      this.flowLayoutPanel6.Controls.Add((Control) this.label38);
      this.flowLayoutPanel6.Name = "flowLayoutPanel6";
      componentResourceManager.ApplyResources((object) this.chkHeight, "chkHeight");
      this.flowLayoutPanel6.SetFlowBreak((Control) this.chkHeight, true);
      this.chkHeight.ForeColor = Color.Blue;
      this.chkHeight.Name = "chkHeight";
      this.chkHeight.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkCrownWidth, "chkCrownWidth");
      this.flowLayoutPanel6.SetFlowBreak((Control) this.chkCrownWidth, true);
      this.chkCrownWidth.ForeColor = Color.Blue;
      this.chkCrownWidth.Name = "chkCrownWidth";
      this.chkCrownWidth.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.label30, "label30");
      this.flowLayoutPanel6.SetFlowBreak((Control) this.label30, true);
      this.label30.Name = "label30";
      componentResourceManager.ApplyResources((object) this.label31, "label31");
      this.flowLayoutPanel6.SetFlowBreak((Control) this.label31, true);
      this.label31.Name = "label31";
      componentResourceManager.ApplyResources((object) this.label32, "label32");
      this.flowLayoutPanel6.SetFlowBreak((Control) this.label32, true);
      this.label32.Name = "label32";
      componentResourceManager.ApplyResources((object) this.label37, "label37");
      this.flowLayoutPanel6.SetFlowBreak((Control) this.label37, true);
      this.label37.Name = "label37";
      componentResourceManager.ApplyResources((object) this.chkCrownHealth, "chkCrownHealth");
      this.flowLayoutPanel6.SetFlowBreak((Control) this.chkCrownHealth, true);
      this.chkCrownHealth.ForeColor = Color.Blue;
      this.chkCrownHealth.Name = "chkCrownHealth";
      this.chkCrownHealth.UseVisualStyleBackColor = true;
      this.chkCrownHealth.CheckedChanged += new EventHandler(this.chkCrownHealth_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.flpCrownHealth, "flpCrownHealth");
      this.flpCrownHealth.Controls.Add((Control) this.rdoDieback);
      this.flpCrownHealth.Controls.Add((Control) this.rdoCondition);
      this.flowLayoutPanel6.SetFlowBreak((Control) this.flpCrownHealth, true);
      this.flpCrownHealth.Name = "flpCrownHealth";
      componentResourceManager.ApplyResources((object) this.rdoDieback, "rdoDieback");
      this.flpCrownHealth.SetFlowBreak((Control) this.rdoDieback, true);
      this.rdoDieback.Name = "rdoDieback";
      this.rdoDieback.TabStop = true;
      this.rdoDieback.UseVisualStyleBackColor = true;
      this.rdoDieback.Validating += new CancelEventHandler(this.rdoCrownHealth_Validating);
      componentResourceManager.ApplyResources((object) this.rdoCondition, "rdoCondition");
      this.flpCrownHealth.SetFlowBreak((Control) this.rdoCondition, true);
      this.rdoCondition.Name = "rdoCondition";
      this.rdoCondition.TabStop = true;
      this.rdoCondition.UseVisualStyleBackColor = true;
      this.rdoCondition.Validating += new CancelEventHandler(this.rdoCrownHealth_Validating);
      componentResourceManager.ApplyResources((object) this.chkCLE, "chkCLE");
      this.flowLayoutPanel6.SetFlowBreak((Control) this.chkCLE, true);
      this.chkCLE.ForeColor = Color.Blue;
      this.chkCLE.Name = "chkCLE";
      this.chkCLE.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkEnergyEffect, "chkEnergyEffect");
      this.flowLayoutPanel6.SetFlowBreak((Control) this.chkEnergyEffect, true);
      this.chkEnergyEffect.Name = "chkEnergyEffect";
      this.chkEnergyEffect.Tag = (object) "energyEffectsMenuItem";
      this.chkEnergyEffect.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.label15, "label15");
      this.flowLayoutPanel6.SetFlowBreak((Control) this.label15, true);
      this.label15.Name = "label15";
      componentResourceManager.ApplyResources((object) this.label38, "label38");
      this.flowLayoutPanel6.SetFlowBreak((Control) this.label38, true);
      this.label38.Name = "label38";
      componentResourceManager.ApplyResources((object) this.flowLayoutPanel7, "flowLayoutPanel7");
      this.flowLayoutPanel7.Controls.Add((Control) this.chkMaintRec);
      this.flowLayoutPanel7.Controls.Add((Control) this.chkMaintTask);
      this.flowLayoutPanel7.Controls.Add((Control) this.chkSidewalkConflict);
      this.flowLayoutPanel7.Controls.Add((Control) this.chkOverheadUtilityConflict);
      this.flowLayoutPanel7.Controls.Add((Control) this.chkIPED);
      this.flowLayoutPanel7.Controls.Add((Control) this.label56);
      this.flowLayoutPanel7.Controls.Add((Control) this.label35);
      this.flowLayoutPanel7.Controls.Add((Control) this.label57);
      this.flowLayoutPanel7.Controls.Add((Control) this.label58);
      this.flowLayoutPanel7.Controls.Add((Control) this.chkTreeUserId);
      this.flowLayoutPanel7.Controls.Add((Control) this.label18);
      this.flowLayoutPanel7.Name = "flowLayoutPanel7";
      componentResourceManager.ApplyResources((object) this.chkMaintRec, "chkMaintRec");
      this.flowLayoutPanel7.SetFlowBreak((Control) this.chkMaintRec, true);
      this.chkMaintRec.Name = "chkMaintRec";
      this.chkMaintRec.Tag = (object) "rbMaintRecReport";
      this.chkMaintRec.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkMaintTask, "chkMaintTask");
      this.flowLayoutPanel7.SetFlowBreak((Control) this.chkMaintTask, true);
      this.chkMaintTask.Name = "chkMaintTask";
      this.chkMaintTask.Tag = (object) "rbMaintTaskReport";
      this.chkMaintTask.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkSidewalkConflict, "chkSidewalkConflict");
      this.flowLayoutPanel7.SetFlowBreak((Control) this.chkSidewalkConflict, true);
      this.chkSidewalkConflict.Name = "chkSidewalkConflict";
      this.chkSidewalkConflict.Tag = (object) "rbSidewalkConflictsReport";
      this.chkSidewalkConflict.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkOverheadUtilityConflict, "chkOverheadUtilityConflict");
      this.flowLayoutPanel7.SetFlowBreak((Control) this.chkOverheadUtilityConflict, true);
      this.chkOverheadUtilityConflict.Name = "chkOverheadUtilityConflict";
      this.chkOverheadUtilityConflict.Tag = (object) "rbUtilityConflictsReport";
      this.chkOverheadUtilityConflict.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkIPED, "chkIPED");
      this.flowLayoutPanel7.SetFlowBreak((Control) this.chkIPED, true);
      this.chkIPED.Name = "chkIPED";
      this.chkIPED.Tag = (object) componentResourceManager.GetString("chkIPED.Tag");
      this.chkIPED.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.label56, "label56");
      this.flowLayoutPanel7.SetFlowBreak((Control) this.label56, true);
      this.label56.Name = "label56";
      componentResourceManager.ApplyResources((object) this.label35, "label35");
      this.flowLayoutPanel7.SetFlowBreak((Control) this.label35, true);
      this.label35.Name = "label35";
      componentResourceManager.ApplyResources((object) this.label57, "label57");
      this.flowLayoutPanel7.SetFlowBreak((Control) this.label57, true);
      this.label57.Name = "label57";
      componentResourceManager.ApplyResources((object) this.label58, "label58");
      this.flowLayoutPanel7.SetFlowBreak((Control) this.label58, true);
      this.label58.Name = "label58";
      componentResourceManager.ApplyResources((object) this.chkTreeUserId, "chkTreeUserId");
      this.flowLayoutPanel7.SetFlowBreak((Control) this.chkTreeUserId, true);
      this.chkTreeUserId.Name = "chkTreeUserId";
      this.chkTreeUserId.Tag = (object) "";
      this.chkTreeUserId.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.label18, "label18");
      this.flowLayoutPanel7.SetFlowBreak((Control) this.label18, true);
      this.label18.Name = "label18";
      componentResourceManager.ApplyResources((object) this.label28, "label28");
      this.pnlProjectOptions.SetColumnSpan((Control) this.label28, 2);
      this.label28.Name = "label28";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel5, "tableLayoutPanel5");
      this.tableLayoutPanel5.Controls.Add((Control) this.cmbDBHMeasurement, 0, 2);
      this.tableLayoutPanel5.Controls.Add((Control) this.chkDBH, 0, 1);
      this.tableLayoutPanel5.Controls.Add((Control) this.chkSpecies, 0, 0);
      this.tableLayoutPanel5.Name = "tableLayoutPanel5";
      componentResourceManager.ApplyResources((object) this.cmbDBHMeasurement, "cmbDBHMeasurement");
      this.cmbDBHMeasurement.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbDBHMeasurement.FormattingEnabled = true;
      this.cmbDBHMeasurement.Items.AddRange(new object[2]
      {
        (object) componentResourceManager.GetString("cmbDBHMeasurement.Items"),
        (object) componentResourceManager.GetString("cmbDBHMeasurement.Items1")
      });
      this.cmbDBHMeasurement.Name = "cmbDBHMeasurement";
      this.chkDBH.AutoCheck = false;
      componentResourceManager.ApplyResources((object) this.chkDBH, "chkDBH");
      this.chkDBH.Checked = true;
      this.chkDBH.CheckState = CheckState.Checked;
      this.chkDBH.ForeColor = Color.Red;
      this.chkDBH.Name = "chkDBH";
      this.chkDBH.UseVisualStyleBackColor = true;
      this.chkSpecies.AutoCheck = false;
      componentResourceManager.ApplyResources((object) this.chkSpecies, "chkSpecies");
      this.chkSpecies.Checked = true;
      this.chkSpecies.CheckState = CheckState.Checked;
      this.chkSpecies.ForeColor = Color.Red;
      this.chkSpecies.Name = "chkSpecies";
      this.chkSpecies.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel6, "tableLayoutPanel6");
      this.tableLayoutPanel6.Controls.Add((Control) this.chkTreeAddress, 0, 0);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblCoverPercentShrub, 0, 15);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblCoverPercentImpervious, 0, 14);
      this.tableLayoutPanel6.Controls.Add((Control) this.chkHydro, 0, 13);
      this.tableLayoutPanel6.Controls.Add((Control) this.cmbPublicPrivate, 0, 12);
      this.tableLayoutPanel6.Controls.Add((Control) this.chkPublic, 0, 11);
      this.tableLayoutPanel6.Controls.Add((Control) this.chkTreeGPS, 0, 10);
      this.tableLayoutPanel6.Controls.Add((Control) this.cmbStreetTree, 0, 9);
      this.tableLayoutPanel6.Controls.Add((Control) this.chkSiteType, 0, 8);
      this.tableLayoutPanel6.Controls.Add((Control) this.chkPlotCenter, 0, 7);
      this.tableLayoutPanel6.Controls.Add((Control) this.chkTreeStatus, 0, 6);
      this.tableLayoutPanel6.Controls.Add((Control) this.chkStrata, 0, 3);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblPlotTreeLandUse, 0, 2);
      this.tableLayoutPanel6.Controls.Add((Control) this.chkLandUseTree, 0, 1);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblStrataInfo1, 0, 5);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblStrataInfo2, 0, 4);
      this.tableLayoutPanel6.Name = "tableLayoutPanel6";
      componentResourceManager.ApplyResources((object) this.chkTreeAddress, "chkTreeAddress");
      this.chkTreeAddress.Name = "chkTreeAddress";
      this.chkTreeAddress.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.lblCoverPercentShrub, "lblCoverPercentShrub");
      this.lblCoverPercentShrub.Name = "lblCoverPercentShrub";
      componentResourceManager.ApplyResources((object) this.lblCoverPercentImpervious, "lblCoverPercentImpervious");
      this.lblCoverPercentImpervious.Name = "lblCoverPercentImpervious";
      componentResourceManager.ApplyResources((object) this.chkHydro, "chkHydro");
      this.chkHydro.Name = "chkHydro";
      this.chkHydro.UseVisualStyleBackColor = true;
      this.cmbPublicPrivate.DropDownStyle = ComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.cmbPublicPrivate, "cmbPublicPrivate");
      this.cmbPublicPrivate.FormattingEnabled = true;
      this.cmbPublicPrivate.Items.AddRange(new object[2]
      {
        (object) componentResourceManager.GetString("cmbPublicPrivate.Items"),
        (object) componentResourceManager.GetString("cmbPublicPrivate.Items1")
      });
      this.cmbPublicPrivate.Name = "cmbPublicPrivate";
      this.cmbPublicPrivate.Validating += new CancelEventHandler(this.cmbPublicPrivate_Validating);
      componentResourceManager.ApplyResources((object) this.chkPublic, "chkPublic");
      this.chkPublic.Name = "chkPublic";
      this.chkPublic.Tag = (object) "rbPublicPrivateByStrata";
      this.chkPublic.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkTreeGPS, "chkTreeGPS");
      this.chkTreeGPS.Name = "chkTreeGPS";
      this.chkTreeGPS.UseVisualStyleBackColor = true;
      this.cmbStreetTree.DropDownStyle = ComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.cmbStreetTree, "cmbStreetTree");
      this.cmbStreetTree.FormattingEnabled = true;
      this.cmbStreetTree.Items.AddRange(new object[2]
      {
        (object) componentResourceManager.GetString("cmbStreetTree.Items"),
        (object) componentResourceManager.GetString("cmbStreetTree.Items1")
      });
      this.cmbStreetTree.Name = "cmbStreetTree";
      this.cmbStreetTree.Validating += new CancelEventHandler(this.cmbStreetTree_Validating);
      componentResourceManager.ApplyResources((object) this.chkSiteType, "chkSiteType");
      this.chkSiteType.Name = "chkSiteType";
      this.chkSiteType.Tag = (object) "rbStreetTreesByStrata";
      this.chkSiteType.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkPlotCenter, "chkPlotCenter");
      this.chkPlotCenter.Name = "chkPlotCenter";
      this.chkPlotCenter.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkTreeStatus, "chkTreeStatus");
      this.chkTreeStatus.Name = "chkTreeStatus";
      this.chkTreeStatus.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkStrata, "chkStrata");
      this.chkStrata.Name = "chkStrata";
      this.chkStrata.Tag = (object) componentResourceManager.GetString("chkStrata.Tag");
      this.chkStrata.UseVisualStyleBackColor = true;
      this.chkStrata.CheckedChanged += new EventHandler(this.chkStrata_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.lblPlotTreeLandUse, "lblPlotTreeLandUse");
      this.lblPlotTreeLandUse.Name = "lblPlotTreeLandUse";
      componentResourceManager.ApplyResources((object) this.chkLandUseTree, "chkLandUseTree");
      this.chkLandUseTree.ForeColor = Color.Blue;
      this.chkLandUseTree.Name = "chkLandUseTree";
      this.chkLandUseTree.UseVisualStyleBackColor = true;
      this.chkLandUseTree.Click += new EventHandler(this.chkLandUseTree_Click);
      componentResourceManager.ApplyResources((object) this.lblStrataInfo1, "lblStrataInfo1");
      this.lblStrataInfo1.Name = "lblStrataInfo1";
      componentResourceManager.ApplyResources((object) this.lblStrataInfo2, "lblStrataInfo2");
      this.lblStrataInfo2.Name = "lblStrataInfo2";
      componentResourceManager.ApplyResources((object) this.flowLayoutPanel5, "flowLayoutPanel5");
      this.flowLayoutPanel5.Name = "flowLayoutPanel5";
      componentResourceManager.ApplyResources((object) this.label21, "label21");
      this.pnlProjectOptions.SetColumnSpan((Control) this.label21, 2);
      this.label21.Name = "label21";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel7, "tableLayoutPanel7");
      this.tableLayoutPanel7.Controls.Add((Control) this.label26, 0, 0);
      this.tableLayoutPanel7.Controls.Add((Control) this.chkSiteNumber, 0, 5);
      this.tableLayoutPanel7.Controls.Add((Control) this.chkTreeLocationType, 0, 3);
      this.tableLayoutPanel7.Name = "tableLayoutPanel7";
      componentResourceManager.ApplyResources((object) this.label26, "label26");
      this.label26.Name = "label26";
      componentResourceManager.ApplyResources((object) this.chkSiteNumber, "chkSiteNumber");
      this.chkSiteNumber.Name = "chkSiteNumber";
      this.chkSiteNumber.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkTreeLocationType, "chkTreeLocationType");
      this.chkTreeLocationType.Name = "chkTreeLocationType";
      this.chkTreeLocationType.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.tlpLegend, "tlpLegend");
      this.pnlProjectOptions.SetColumnSpan((Control) this.tlpLegend, 2);
      this.tlpLegend.Controls.Add((Control) this.lblOptionalFields, 1, 2);
      this.tlpLegend.Controls.Add((Control) this.lblRecommendedFields, 1, 1);
      this.tlpLegend.Controls.Add((Control) this.lblRequiredFields, 1, 0);
      this.tlpLegend.Controls.Add((Control) this.label13, 0, 0);
      this.tlpLegend.Controls.Add((Control) this.label14, 0, 1);
      this.tlpLegend.Controls.Add((Control) this.label22, 0, 2);
      this.tlpLegend.Name = "tlpLegend";
      this.pnlProjectOptions.SetRowSpan((Control) this.tlpLegend, 2);
      this.tlpLegend.Paint += new PaintEventHandler(this.tlpLegend_Paint);
      componentResourceManager.ApplyResources((object) this.lblOptionalFields, "lblOptionalFields");
      this.lblOptionalFields.ForeColor = SystemColors.ControlText;
      this.lblOptionalFields.Name = "lblOptionalFields";
      componentResourceManager.ApplyResources((object) this.lblRecommendedFields, "lblRecommendedFields");
      this.lblRecommendedFields.ForeColor = SystemColors.ControlText;
      this.lblRecommendedFields.Name = "lblRecommendedFields";
      componentResourceManager.ApplyResources((object) this.lblRequiredFields, "lblRequiredFields");
      this.lblRequiredFields.ForeColor = SystemColors.ControlText;
      this.lblRequiredFields.Name = "lblRequiredFields";
      componentResourceManager.ApplyResources((object) this.label13, "label13");
      this.label13.ForeColor = Color.Red;
      this.label13.Name = "label13";
      componentResourceManager.ApplyResources((object) this.label14, "label14");
      this.label14.ForeColor = Color.Blue;
      this.label14.Name = "label14";
      componentResourceManager.ApplyResources((object) this.label22, "label22");
      this.label22.Name = "label22";
      componentResourceManager.ApplyResources((object) this.lblPlotMinimumRequired, "lblPlotMinimumRequired");
      this.lblPlotMinimumRequired.Name = "lblPlotMinimumRequired";
      componentResourceManager.ApplyResources((object) this.lblPlotInformation, "lblPlotInformation");
      this.lblPlotInformation.Name = "lblPlotInformation";
      componentResourceManager.ApplyResources((object) this.lblPlotGeneralFields, "lblPlotGeneralFields");
      this.lblPlotGeneralFields.Name = "lblPlotGeneralFields";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel3, "tableLayoutPanel3");
      this.pnlProjectOptions.SetColumnSpan((Control) this.tableLayoutPanel3, 4);
      this.tableLayoutPanel3.Controls.Add((Control) this.txtOtherThree, 1, 2);
      this.tableLayoutPanel3.Controls.Add((Control) this.txtOtherTwo, 1, 1);
      this.tableLayoutPanel3.Controls.Add((Control) this.txtOtherOne, 1, 0);
      this.tableLayoutPanel3.Controls.Add((Control) this.chkOtherThree, 0, 2);
      this.tableLayoutPanel3.Controls.Add((Control) this.chkOtherOne, 0, 0);
      this.tableLayoutPanel3.Controls.Add((Control) this.chkOtherTwo, 0, 1);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      componentResourceManager.ApplyResources((object) this.txtOtherThree, "txtOtherThree");
      this.txtOtherThree.Name = "txtOtherThree";
      this.txtOtherThree.TextChanged += new EventHandler(this.txtCustomField_TextChanged);
      componentResourceManager.ApplyResources((object) this.txtOtherTwo, "txtOtherTwo");
      this.txtOtherTwo.Name = "txtOtherTwo";
      this.txtOtherTwo.TextChanged += new EventHandler(this.txtCustomField_TextChanged);
      componentResourceManager.ApplyResources((object) this.txtOtherOne, "txtOtherOne");
      this.txtOtherOne.Name = "txtOtherOne";
      this.txtOtherOne.TextChanged += new EventHandler(this.txtCustomField_TextChanged);
      this.txtOtherOne.Validating += new CancelEventHandler(this.customFields_Validating);
      componentResourceManager.ApplyResources((object) this.chkOtherThree, "chkOtherThree");
      this.chkOtherThree.Name = "chkOtherThree";
      this.chkOtherThree.Tag = (object) "rbFieldThreeReport";
      this.chkOtherThree.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkOtherOne, "chkOtherOne");
      this.chkOtherOne.Name = "chkOtherOne";
      this.chkOtherOne.Tag = (object) "rbFieldOneReport";
      this.chkOtherOne.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkOtherTwo, "chkOtherTwo");
      this.chkOtherTwo.Name = "chkOtherTwo";
      this.chkOtherTwo.Tag = (object) "rbFieldTwoReport";
      this.chkOtherTwo.UseVisualStyleBackColor = true;
      this.pnlProjectOptions.SetColumnSpan((Control) this.pbHelpImage, 4);
      componentResourceManager.ApplyResources((object) this.pbHelpImage, "pbHelpImage");
      this.pbHelpImage.Name = "pbHelpImage";
      this.pbHelpImage.TabStop = false;
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel2, "tableLayoutPanel2");
      this.pnlProjectOptions.SetColumnSpan((Control) this.tableLayoutPanel2, 2);
      this.tableLayoutPanel2.Controls.Add((Control) this.pbUnitWarning, 1, 0);
      this.tableLayoutPanel2.Controls.Add((Control) this.rtlUnitWarning, 2, 0);
      this.tableLayoutPanel2.Controls.Add((Control) this.rdoUnitsEnglish, 0, 0);
      this.tableLayoutPanel2.Controls.Add((Control) this.rdoUnitsMetric, 0, 1);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.pbUnitWarning.Image = (Image) i_Tree_Eco_v6.Properties.Resources.info;
      componentResourceManager.ApplyResources((object) this.pbUnitWarning, "pbUnitWarning");
      this.pbUnitWarning.Name = "pbUnitWarning";
      this.tableLayoutPanel2.SetRowSpan((Control) this.pbUnitWarning, 2);
      this.pbUnitWarning.TabStop = false;
      componentResourceManager.ApplyResources((object) this.rtlUnitWarning, "rtlUnitWarning");
      this.rtlUnitWarning.Name = "rtlUnitWarning";
      this.tableLayoutPanel2.SetRowSpan((Control) this.rtlUnitWarning, 3);
      this.rtlUnitWarning.TabStop = false;
      componentResourceManager.ApplyResources((object) this.rdoUnitsEnglish, "rdoUnitsEnglish");
      this.rdoUnitsEnglish.Name = "rdoUnitsEnglish";
      this.rdoUnitsEnglish.TabStop = true;
      this.rdoUnitsEnglish.UseVisualStyleBackColor = true;
      this.rdoUnitsEnglish.CheckedChanged += new EventHandler(this.rdoUnits_CheckedChanged);
      this.rdoUnitsEnglish.Validating += new CancelEventHandler(this.rdoUnits_Validating);
      componentResourceManager.ApplyResources((object) this.rdoUnitsMetric, "rdoUnitsMetric");
      this.rdoUnitsMetric.Name = "rdoUnitsMetric";
      this.rdoUnitsMetric.TabStop = true;
      this.rdoUnitsMetric.UseVisualStyleBackColor = true;
      this.rdoUnitsMetric.CheckedChanged += new EventHandler(this.rdoUnits_CheckedChanged);
      this.rdoUnitsMetric.Validating += new CancelEventHandler(this.rdoUnits_Validating);
      componentResourceManager.ApplyResources((object) this.lblAccuracyCaption, "lblAccuracyCaption");
      this.lblAccuracyCaption.ForeColor = Color.Red;
      this.lblAccuracyCaption.Name = "lblAccuracyCaption";
      this.ep.ContainerControl = (ContainerControl) this;
      this.toolTip1.ShowAlways = true;
      componentResourceManager.ApplyResources((object) this.cmdOK, "cmdOK");
      this.cmdOK.Name = "cmdOK";
      this.cmdOK.UseVisualStyleBackColor = true;
      this.cmdOK.Click += new EventHandler(this.cmdOK_Click);
      componentResourceManager.ApplyResources((object) this.cmdCancel, "cmdCancel");
      this.cmdCancel.Name = "cmdCancel";
      this.cmdCancel.UseVisualStyleBackColor = true;
      this.cmdCancel.Click += new EventHandler(this.cmdCancel_Click);
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel4, "tableLayoutPanel4");
      this.tableLayoutPanel4.Controls.Add((Control) this.lblHeader, 0, 0);
      this.tableLayoutPanel4.Controls.Add((Control) this.cmdOK, 1, 0);
      this.tableLayoutPanel4.Controls.Add((Control) this.cmdCancel, 2, 0);
      this.tableLayoutPanel4.Name = "tableLayoutPanel4";
      this.tmValidateWeatherStation.Interval = 500;
      this.tmValidateWeatherStation.Tick += new EventHandler(this.tmValidateWeatherStation_Tick);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoValidate = AutoValidate.EnableAllowFocusChange;
      this.BackColor = Color.White;
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.tableLayoutPanel4);
      this.DoubleBuffered = true;
      this.Name = nameof (ProjectOverviewForm);
      this.FormClosed += new FormClosedEventHandler(this.ProjectOverviewForm_Closed);
      this.Resize += new EventHandler(this.ProjectOverviewForm_Resize);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel4, 0);
      this.Controls.SetChildIndex((Control) this.tabControl1, 0);
      this.tabControl1.ResumeLayout(false);
      this.tpProjectInfo.ResumeLayout(false);
      this.tpProjectInfo.PerformLayout();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.tpProjectLocation.ResumeLayout(false);
      this.tpProjectLocation.PerformLayout();
      this.pnlLocation.ResumeLayout(false);
      this.pnlLocation.PerformLayout();
      this.pnlDataRequired.ResumeLayout(false);
      this.pnlDataRequired.PerformLayout();
      ((ISupportInitialize) this.pbInfo).EndInit();
      this.tpProjectOptions.ResumeLayout(false);
      this.tpProjectOptions.PerformLayout();
      this.pnlProjectOptions.ResumeLayout(false);
      this.pnlProjectOptions.PerformLayout();
      this.flpPlotGeneralFields.ResumeLayout(false);
      this.flpPlotGeneralFields.PerformLayout();
      this.flpPlotGeneralFieldsExt.ResumeLayout(false);
      this.flpPlotGeneralFieldsExt.PerformLayout();
      this.flpPlotMinimumRequiredFields.ResumeLayout(false);
      this.flpPlotMinimumRequiredFields.PerformLayout();
      this.flowLayoutPanel6.ResumeLayout(false);
      this.flowLayoutPanel6.PerformLayout();
      this.flpCrownHealth.ResumeLayout(false);
      this.flpCrownHealth.PerformLayout();
      this.flowLayoutPanel7.ResumeLayout(false);
      this.flowLayoutPanel7.PerformLayout();
      this.tableLayoutPanel5.ResumeLayout(false);
      this.tableLayoutPanel5.PerformLayout();
      this.tableLayoutPanel6.ResumeLayout(false);
      this.tableLayoutPanel6.PerformLayout();
      this.tableLayoutPanel7.ResumeLayout(false);
      this.tableLayoutPanel7.PerformLayout();
      this.tlpLegend.ResumeLayout(false);
      this.tlpLegend.PerformLayout();
      this.tableLayoutPanel3.ResumeLayout(false);
      this.tableLayoutPanel3.PerformLayout();
      ((ISupportInitialize) this.pbHelpImage).EndInit();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      ((ISupportInitialize) this.pbUnitWarning).EndInit();
      ((ISupportInitialize) this.ep).EndInit();
      this.tableLayoutPanel4.ResumeLayout(false);
      this.tableLayoutPanel4.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
