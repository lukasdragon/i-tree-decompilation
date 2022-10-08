// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.BenefitPricesForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using CsvHelper;
using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Interfaces;
using i_Tree_Eco_v6.Properties;
using LocationSpecies.Domain;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class BenefitPricesForm : DataContentForm, IExportable
  {
    private const int UsLocId = 219;
    private const double nullPrice = -1.0;
    private Location m_location;
    private Location m_nation;
    private LocationSpecies.Domain.Currency m_currency;
    private double? defaultElectricPrice;
    private double? defaultGasPrice;
    private double? defaultCarbonPrice;
    private double? defaultH2OPrice;
    private double? defaultCoPrice;
    private double? defaultNO2Price;
    private double? defaultSO2Price;
    private double? defaultPM25Price;
    private double? defaultO3Price;
    private IContainer components;
    private Label label1;
    private ErrorProvider ep;
    private TableLayoutPanel tableLayoutPanel1;
    private TableLayoutPanel tblBasic;
    private NumericTextBox txtExchangeRate;
    private NumericTextBox txtAvoidedRunoff;
    private NumericTextBox txtCarbon;
    private NumericTextBox txtHeating;
    private NumericTextBox txtElectricity;
    private Label lblExchangeRate;
    private Label lblMeasurementUnits;
    private Button cmdDefaultExchangeRate;
    private TableLayoutPanel tblInternational;
    private NumericTextBox txtPM25;
    private NumericTextBox txtSO2;
    private NumericTextBox txtNO2;
    private NumericTextBox txtOzone;
    private NumericTextBox txtCO;
    private Button cmdDefaultPM25;
    private Label lblPM25;
    private Button cmdDefaultSO2;
    private Label lblSO2;
    private Label lblOzone;
    private Button cmdDefaultOzone;
    private Button cmdDefaultNO2;
    private Label lblNO2;
    private Label lblCO;
    private Button cmdDefaultCO;
    private Button cmdDefaultAvoidedRunoff;
    private Button cmdDefaultCarbon;
    private Button cmdDefaultHeating;
    private Button cmdDefaultElectricity;
    private Label lblAvoidedRunoff;
    private Label lblCarbon;
    private Label lblHeating;
    private Label lblElectricity;
    private Label label4;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button cmdOK;
    private Button cmdCancel;
    private Label lblDefaults;
    private Label lblNotes;
    private Label lblModify;
    private Label label2;
    private Label label3;
    private TableLayoutPanel tblExchangeRt;

    public BenefitPricesForm() => this.InitializeComponent();

    private void NumericTextBox_ValidatingGreaterThan0(object sender, CancelEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      double result;
      double.TryParse(textBox.Text, out result);
      this.ep.SetError((Control) textBox, result <= 0.0, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) i_Tree_Eco_v6.Resources.Strings.Value, (object) 0));
      e.Cancel = textBox.HasErrors(this.ep);
    }

    private void NumericTextBox_ValidatingGreaterThanOrEqualTo0(object sender, CancelEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      double result;
      double.TryParse(textBox.Text, out result);
      this.ep.SetError((Control) textBox, result < 0.0, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) i_Tree_Eco_v6.Resources.Strings.Value, (object) 0));
      e.Cancel = textBox.HasErrors(this.ep);
    }

    private void Rainfall_Format(object sender, ConvertEventArgs e)
    {
      if (e.DesiredType != typeof (string))
        return;
      if (e.Value == null || e.Value.Equals((object) -1.0))
      {
        e.Value = (object) string.Empty;
      }
      else
      {
        Binding binding = sender as Binding;
        e.Value = (object) this.ConvertM3ToGallon((double) e.Value).ToString(binding.FormatString);
      }
    }

    private void Rainfall_Parse(object sender, ConvertEventArgs e)
    {
      if (e.DesiredType != typeof (double))
        return;
      if (e.Value.ToString().Trim() == "")
        e.Value = (object) -1.0;
      else
        e.Value = (object) this.ConvertGallonToM3(double.Parse(e.Value.ToString()));
    }

    private void Ton_Format(object sender, ConvertEventArgs e)
    {
      if (e.DesiredType != typeof (string))
        return;
      if (e.Value == null || e.Value.Equals((object) -1.0))
      {
        e.Value = (object) string.Empty;
      }
      else
      {
        Binding binding = sender as Binding;
        e.Value = (object) this.ConvertTonneToTon((double) e.Value).ToString(binding.FormatString);
      }
    }

    private void Ton_Parse(object sender, ConvertEventArgs e)
    {
      if (e.DesiredType != typeof (double))
        return;
      if (e.Value.ToString().Trim() == "")
        e.Value = (object) -1.0;
      else
        e.Value = (object) this.ConvertTonToTonne(double.Parse(e.Value.ToString()));
    }

    private void Parse_Price(object sender, ConvertEventArgs e)
    {
      if (e.DesiredType != typeof (double))
        return;
      if (e.Value.ToString().Trim().Length == 0)
        e.Value = (object) -1.0;
      else
        e.Value = (object) double.Parse(e.Value.ToString());
    }

    private T PersistPrice<T>(T ep, double null_value) where T : ElementPrice
    {
      T obj = ep;
      if ((object) ep != null)
      {
        if (ep.Price != null_value)
        {
          this.Session.SaveOrUpdate((object) ep);
        }
        else
        {
          if (!ep.IsTransient)
            this.Session.Delete((object) ep);
          obj = default (T);
        }
      }
      return obj;
    }

    private void cmdOK_Click(object sender, EventArgs e)
    {
      if (this.m_isDirty && this.ValidateChildren(ValidationConstraints.Visible))
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          this.Year.ExchangeRate = this.PersistPrice<ExchangeRate>(this.Year.ExchangeRate, 0.0);
          this.Year.Carbon = this.PersistPrice<Carbon>(this.Year.Carbon, -1.0);
          this.Year.Electricity = this.PersistPrice<Electricity>(this.Year.Electricity, -1.0);
          this.Year.Gas = this.PersistPrice<Gas>(this.Year.Gas, -1.0);
          this.Year.H2O = this.PersistPrice<H2O>(this.Year.H2O, -1.0);
          this.Year.CO = this.PersistPrice<CO>(this.Year.CO, -1.0);
          this.Year.O3 = this.PersistPrice<O3>(this.Year.O3, -1.0);
          this.Year.NO2 = this.PersistPrice<NO2>(this.Year.NO2, -1.0);
          this.Year.SO2 = this.PersistPrice<SO2>(this.Year.SO2, -1.0);
          this.Year.PM25 = this.PersistPrice<PM25>(this.Year.PM25, -1.0);
          this.Session.SaveOrUpdate((object) this.Year);
          transaction.Commit();
        }
        EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(this.Year), (Control) this);
        this.m_isDirty = false;
      }
      this.Close();
    }

    private void cmdCancel_Click(object sender, EventArgs e) => this.Close();

    private void cmdDefaultExchangeRate_Click(object sender, EventArgs e) => this.Year.ExchangeRate.Price = this.getCurrencyExchangeRate(this.m_currency.Abbreviation);

    private void cmdDefaultElectricity_Click(object sender, EventArgs e)
    {
      this.SetElementPrice((ElementPrice) this.Year.Electricity, this.defaultElectricPrice);
      ((Control) sender).Hide();
    }

    private void cmdDefaultHeating_Click(object sender, EventArgs e)
    {
      this.SetElementPrice((ElementPrice) this.Year.Gas, this.defaultGasPrice);
      ((Control) sender).Hide();
    }

    private void cmdDefaultCarbon_Click(object sender, EventArgs e)
    {
      this.SetElementPrice((ElementPrice) this.Year.Carbon, this.defaultCarbonPrice);
      ((Control) sender).Hide();
    }

    private void cmdDefaultAvoidedRunoff_Click(object sender, EventArgs e)
    {
      this.SetElementPrice((ElementPrice) this.Year.H2O, this.defaultH2OPrice);
      ((Control) sender).Hide();
    }

    private void cmdDefaultCO_Click(object sender, EventArgs e)
    {
      this.SetElementPrice((ElementPrice) this.Year.CO, this.defaultCoPrice);
      ((Control) sender).Hide();
    }

    private void cmdDefaultOzone_Click(object sender, EventArgs e)
    {
      this.SetElementPrice((ElementPrice) this.Year.O3, this.defaultO3Price);
      ((Control) sender).Hide();
    }

    private void cmdDefaultNO2_Click(object sender, EventArgs e)
    {
      this.SetElementPrice((ElementPrice) this.Year.NO2, this.defaultNO2Price);
      ((Control) sender).Hide();
    }

    private void cmdDefaultSO2_Click(object sender, EventArgs e)
    {
      this.SetElementPrice((ElementPrice) this.Year.SO2, this.defaultSO2Price);
      ((Control) sender).Hide();
    }

    private void cmdDefaultPM25_Click(object sender, EventArgs e)
    {
      this.SetElementPrice((ElementPrice) this.Year.PM25, this.defaultPM25Price);
      ((Control) sender).Hide();
    }

    private void txtElectricity_Validated(object sender, EventArgs e) => this.UpdateButton(this.defaultElectricPrice, this.cmdDefaultElectricity, (ElementPrice) this.Year.Electricity);

    private void txtHeating_Validated(object sender, EventArgs e) => this.UpdateButton(this.defaultGasPrice, this.cmdDefaultHeating, (ElementPrice) this.Year.Gas);

    private void txtCarbon_Validated(object sender, EventArgs e) => this.UpdateButton(this.defaultCarbonPrice, this.cmdDefaultCarbon, (ElementPrice) this.Year.Carbon);

    private void txtAvoidedRunoff_Validated(object sender, EventArgs e) => this.UpdateButton(this.defaultH2OPrice, this.cmdDefaultAvoidedRunoff, (ElementPrice) this.Year.H2O);

    private void txtCO_Validated(object sender, EventArgs e) => this.UpdateButton(this.defaultCoPrice, this.cmdDefaultCO, (ElementPrice) this.Year.CO);

    private void txtOzone_Validated(object sender, EventArgs e) => this.UpdateButton(this.defaultO3Price, this.cmdDefaultOzone, (ElementPrice) this.Year.O3);

    private void txtNO2_Validated(object sender, EventArgs e) => this.UpdateButton(this.defaultNO2Price, this.cmdDefaultNO2, (ElementPrice) this.Year.NO2);

    private void txtSO2_Validated(object sender, EventArgs e) => this.UpdateButton(this.defaultSO2Price, this.cmdDefaultSO2, (ElementPrice) this.Year.SO2);

    private void txtPM25_Validated(object sender, EventArgs e) => this.UpdateButton(this.defaultPM25Price, this.cmdDefaultPM25, (ElementPrice) this.Year.PM25);

    private void SetElementPrice(ElementPrice ep, double? defaultPrice)
    {
      if (!defaultPrice.HasValue)
        return;
      ep.Price = defaultPrice.Value;
    }

    private void UpdateButton(double? defaultPrice, Button b, ElementPrice elementPice) => b.Visible = defaultPrice.HasValue && !defaultPrice.Equals((object) elementPice.Price);

    protected override void LoadData()
    {
      base.LoadData();
      this.InitLocationData(this.Project.LocationId);
    }

    protected override void OnDataLoaded()
    {
      this.InitYearOptions();
      this.registerChangeEvents((Control) this);
      base.OnDataLoaded();
    }

    protected override bool ShowReportWarning => false;

    protected override bool ShowHelpMsg => false;

    private void InitLocationData(int locId) => RetryExecutionHandler.Execute((System.Action) (() =>
    {
      using (ISession session = Program.Session.LocSp.OpenSession())
      {
        LocationRelation locationRelation = this.GetLocationRelation(session, locId);
        this.m_location = locationRelation.Location;
        while (locationRelation != null && locationRelation.Level > (short) 3)
          locationRelation = this.GetLocationRelation(session, locationRelation.Parent.Id);
        this.m_nation = locationRelation.Parent;
        this.defaultElectricPrice = this.GetElectricityPrice(session, this.m_location);
        this.defaultGasPrice = this.GetGasPrice(session, this.m_location);
        float? carbon = this.m_nation.EnvironmentalValue?.Carbon;
        this.defaultCarbonPrice = carbon.HasValue ? new double?((double) carbon.GetValueOrDefault()) : new double?();
        LocationEnvironmentalValue environmentalValue = this.m_nation.EnvironmentalValue;
        this.defaultH2OPrice = environmentalValue != null ? new double?(environmentalValue.RainfallInterception * 264.172) : new double?();
        this.m_currency = this.GetCurrency(session, this.m_location);
        this.GetDefaultPrices(session);
      }
    }));

    private void InitYearOptions()
    {
      Year year1 = this.Year;
      Year year2 = year1;
      Electricity electricity1 = year1.Electricity;
      if (electricity1 == null)
      {
        Electricity electricity2 = new Electricity();
        electricity2.Year = year1;
        electricity2.Price = -1.0;
        electricity1 = electricity2;
      }
      year2.Electricity = electricity1;
      Year year3 = year1;
      Gas gas1 = year1.Gas;
      if (gas1 == null)
      {
        Gas gas2 = new Gas();
        gas2.Year = year1;
        gas2.Price = -1.0;
        gas1 = gas2;
      }
      year3.Gas = gas1;
      Year year4 = year1;
      Carbon carbon1 = year1.Carbon;
      if (carbon1 == null)
      {
        Carbon carbon2 = new Carbon();
        carbon2.Year = year1;
        carbon2.Price = -1.0;
        carbon1 = carbon2;
      }
      year4.Carbon = carbon1;
      Year year5 = year1;
      H2O h2O1 = year1.H2O;
      if (h2O1 == null)
      {
        H2O h2O2 = new H2O();
        h2O2.Year = year1;
        h2O2.Price = -1.0;
        h2O1 = h2O2;
      }
      year5.H2O = h2O1;
      if (this.m_nation.Id != 219)
      {
        Year year6 = year1;
        ExchangeRate exchangeRate1 = year1.ExchangeRate;
        if (exchangeRate1 == null)
        {
          ExchangeRate exchangeRate2 = new ExchangeRate();
          exchangeRate2.Year = year1;
          exchangeRate2.Price = 0.0;
          exchangeRate1 = exchangeRate2;
        }
        year6.ExchangeRate = exchangeRate1;
      }
      if (year1.Unit == YearUnit.English)
      {
        this.txtAvoidedRunoff.DecimalPlaces = 6;
        this.txtAvoidedRunoff.Format = "0.######;-0.######";
      }
      else
      {
        this.txtAvoidedRunoff.DecimalPlaces = 3;
        this.txtAvoidedRunoff.Format = "0.###;-0.###";
      }
      if (this.m_nation.Id != 219)
        this.BindElementToTextBox((Control) this.txtExchangeRate, (object) year1.ExchangeRate, (object) 0.0);
      this.BindElementToTextBox((Control) this.txtElectricity, (object) year1.Electricity, (ConvertEventHandler) null, new ConvertEventHandler(this.Parse_Price), (object) -1.0);
      this.BindElementToTextBox((Control) this.txtHeating, (object) year1.Gas, (ConvertEventHandler) null, new ConvertEventHandler(this.Parse_Price), (object) -1.0);
      this.BindElementToTextBox((Control) this.txtCarbon, (object) year1.Carbon, new ConvertEventHandler(this.Ton_Format), new ConvertEventHandler(this.Ton_Parse), (object) -1.0);
      this.BindElementToTextBox((Control) this.txtAvoidedRunoff, (object) year1.H2O, new ConvertEventHandler(this.Rainfall_Format), new ConvertEventHandler(this.Rainfall_Parse), (object) -1.0);
      this.UpdateButton(this.defaultElectricPrice, this.cmdDefaultElectricity, (ElementPrice) year1.Electricity);
      this.UpdateButton(this.defaultGasPrice, this.cmdDefaultHeating, (ElementPrice) year1.Gas);
      this.UpdateButton(this.defaultCarbonPrice, this.cmdDefaultCarbon, (ElementPrice) year1.Carbon);
      this.UpdateButton(this.defaultH2OPrice, this.cmdDefaultAvoidedRunoff, (ElementPrice) year1.H2O);
      this.txtElectricity.Enabled = true;
      this.txtHeating.Enabled = true;
      this.txtCarbon.Enabled = true;
      this.txtAvoidedRunoff.Enabled = true;
      this.lblMeasurementUnits.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.MeasurementUnits, (object) EnumHelper.GetDescription<YearUnit>(year1.Unit));
      string currencyString = this.GetCurrencyString();
      string currencyPerElectricity = this.GetCurrencyPerElectricity();
      string currencyPerHeating = this.GetCurrencyPerHeating();
      string massPriceLabel = this.GetMassPriceLabel();
      string volumePriceLabel = this.GetVolumePriceLabel();
      this.lblElectricity.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.Electricity, (object) currencyPerElectricity);
      this.lblHeating.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.Heating, (object) currencyPerHeating);
      this.lblCarbon.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.Carbon, (object) massPriceLabel);
      this.lblAvoidedRunoff.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.AvoidedRunoff, (object) volumePriceLabel);
      this.lblCO.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.CarbonMonoxideCO, (object) massPriceLabel);
      this.lblOzone.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.Ozone, (object) currencyString, (object) massPriceLabel);
      this.lblNO2.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideNO2, (object) massPriceLabel);
      this.lblSO2.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.SulfurDioxideSO2, (object) massPriceLabel);
      this.lblPM25.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25, (object) massPriceLabel);
      bool flag1 = !NationFeatures.IsUSorUSlikeNation(this.m_nation.Id);
      bool flag2 = this.m_nation.Id != 219;
      this.tblExchangeRt.Visible = flag2;
      this.lblDefaults.Visible = !flag1;
      if (flag2)
      {
        this.lblExchangeRate.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.CurrencyExchangeRate, (object) currencyString);
        this.txtExchangeRate.Enabled = true;
        this.cmdDefaultExchangeRate.Enabled = true;
      }
      if (flag1)
      {
        Year year7 = year1;
        CO co1 = year1.CO;
        if (co1 == null)
        {
          CO co2 = new CO();
          co2.Year = year1;
          co2.Price = -1.0;
          co1 = co2;
        }
        year7.CO = co1;
        Year year8 = year1;
        O3 o3_1 = year1.O3;
        if (o3_1 == null)
        {
          O3 o3_2 = new O3();
          o3_2.Year = year1;
          o3_2.Price = -1.0;
          o3_1 = o3_2;
        }
        year8.O3 = o3_1;
        Year year9 = year1;
        NO2 no2_1 = year1.NO2;
        if (no2_1 == null)
        {
          NO2 no2_2 = new NO2();
          no2_2.Year = year1;
          no2_2.Price = -1.0;
          no2_1 = no2_2;
        }
        year9.NO2 = no2_1;
        Year year10 = year1;
        SO2 so2_1 = year1.SO2;
        if (so2_1 == null)
        {
          SO2 so2_2 = new SO2();
          so2_2.Year = year1;
          so2_2.Price = -1.0;
          so2_1 = so2_2;
        }
        year10.SO2 = so2_1;
        Year year11 = year1;
        PM25 pm25_1 = year1.PM25;
        if (pm25_1 == null)
        {
          PM25 pm25_2 = new PM25();
          pm25_2.Year = year1;
          pm25_2.Price = -1.0;
          pm25_1 = pm25_2;
        }
        year11.PM25 = pm25_1;
        this.BindElementToTextBox((Control) this.txtCO, (object) year1.CO, new ConvertEventHandler(this.Ton_Format), new ConvertEventHandler(this.Ton_Parse), (object) -1.0);
        this.BindElementToTextBox((Control) this.txtOzone, (object) year1.O3, new ConvertEventHandler(this.Ton_Format), new ConvertEventHandler(this.Ton_Parse), (object) -1.0);
        this.BindElementToTextBox((Control) this.txtNO2, (object) year1.NO2, new ConvertEventHandler(this.Ton_Format), new ConvertEventHandler(this.Ton_Parse), (object) -1.0);
        this.BindElementToTextBox((Control) this.txtSO2, (object) year1.SO2, new ConvertEventHandler(this.Ton_Format), new ConvertEventHandler(this.Ton_Parse), (object) -1.0);
        this.BindElementToTextBox((Control) this.txtPM25, (object) year1.PM25, new ConvertEventHandler(this.Ton_Format), new ConvertEventHandler(this.Ton_Parse), (object) -1.0);
        this.UpdateButton(this.defaultCoPrice, this.cmdDefaultCO, (ElementPrice) year1.CO);
        this.UpdateButton(this.defaultO3Price, this.cmdDefaultOzone, (ElementPrice) year1.O3);
        this.UpdateButton(this.defaultNO2Price, this.cmdDefaultNO2, (ElementPrice) year1.NO2);
        this.UpdateButton(this.defaultSO2Price, this.cmdDefaultSO2, (ElementPrice) year1.SO2);
        this.UpdateButton(this.defaultPM25Price, this.cmdDefaultPM25, (ElementPrice) year1.PM25);
        this.txtCO.Enabled = true;
        this.txtOzone.Enabled = true;
        this.txtNO2.Enabled = true;
        this.txtSO2.Enabled = true;
        this.txtPM25.Enabled = true;
        double? nullable = this.defaultCoPrice;
        double num1 = 0.0;
        int num2;
        if (nullable.GetValueOrDefault() == num1 & nullable.HasValue)
        {
          nullable = this.defaultO3Price;
          double num3 = 0.0;
          if (nullable.GetValueOrDefault() == num3 & nullable.HasValue)
          {
            nullable = this.defaultNO2Price;
            double num4 = 0.0;
            if (nullable.GetValueOrDefault() == num4 & nullable.HasValue)
            {
              nullable = this.defaultSO2Price;
              double num5 = 0.0;
              if (nullable.GetValueOrDefault() == num5 & nullable.HasValue)
              {
                nullable = this.defaultPM25Price;
                double num6 = 0.0;
                num2 = !(nullable.GetValueOrDefault() == num6 & nullable.HasValue) ? 1 : 0;
                goto label_36;
              }
            }
          }
        }
        num2 = 1;
label_36:
        this.tblBasic.SetColumnSpan((Control) this.tblInternational, num2 != 0 ? 3 : 2);
      }
      this.tblInternational.Visible = flag1;
    }

    private void GetDefaultPrices(ISession s)
    {
      double? nullable;
      // ISSUE: explicit reference operation
      ^ref nullable = new double?(s.Query<LocationPpiAdjustment>().WithOptions<LocationPpiAdjustment>((System.Action<NhQueryableOptions>) (o => o.SetCacheable(true))).Where<LocationPpiAdjustment>((System.Linq.Expressions.Expression<Func<LocationPpiAdjustment, bool>>) (ppi => ppi.Location == this.m_nation)).OrderByDescending<LocationPpiAdjustment, int>((System.Linq.Expressions.Expression<Func<LocationPpiAdjustment, int>>) (ppi => ppi.PpiYear)).Select<LocationPpiAdjustment, double>((System.Linq.Expressions.Expression<Func<LocationPpiAdjustment, double>>) (ppi => ppi.PpiValue)).FirstOrDefault<double>());
      if (!nullable.HasValue)
        return;
      List<PollutantBaseCost> list = s.Query<PollutantBaseCost>().WithOptions<PollutantBaseCost>((System.Action<NhQueryableOptions>) (o => o.SetCacheable(true))).Where<PollutantBaseCost>((System.Linq.Expressions.Expression<Func<PollutantBaseCost, bool>>) (pbc => pbc.Location == this.m_nation)).ToList<PollutantBaseCost>();
      double num1 = this.IsMetricUnitProject() ? 1.0 : 0.907185;
      foreach (PollutantBaseCost pollutantBaseCost in (IEnumerable<PollutantBaseCost>) list)
      {
        double num2 = Math.Round(num1 * pollutantBaseCost.BaseCost * nullable.Value / pollutantBaseCost.PpiAdjustment.PpiValue, 2);
        string upper = pollutantBaseCost.Pollutant.Name.ToUpper();
        if (!(upper == "CO"))
        {
          if (!(upper == "NO2"))
          {
            if (!(upper == "SO2"))
            {
              if (!(upper == "O3"))
              {
                if (upper == "PM2.5")
                  this.defaultPM25Price = new double?(num2);
              }
              else
                this.defaultO3Price = new double?(num2);
            }
            else
              this.defaultSO2Price = new double?(num2);
          }
          else
            this.defaultNO2Price = new double?(num2);
        }
        else
          this.defaultCoPrice = new double?(num2);
      }
    }

    private void BindElementToTextBox(Control c, object dataSource, object nullValue) => this.BindElementToTextBox(c, dataSource, (ConvertEventHandler) null, (ConvertEventHandler) null, nullValue);

    private void BindElementToTextBox(
      Control c,
      object dataSource,
      ConvertEventHandler format,
      ConvertEventHandler parse,
      object nullValue)
    {
      Interlocked.Increment(ref this.m_changes);
      string propertyName = "Text";
      string dataMember = "Price";
      Binding dataBinding = c.DataBindings[propertyName];
      if (dataBinding != null)
        c.DataBindings.Remove(dataBinding);
      Binding binding = new Binding(propertyName, dataSource, dataMember);
      if (c is NumericTextBox numericTextBox)
      {
        binding.FormatString = numericTextBox.Format;
        binding.FormattingEnabled = true;
      }
      if (format != null)
        binding.Format += format;
      else if (nullValue != null)
        binding.DataSourceNullValue = nullValue;
      if (parse != null)
        binding.Parse += parse;
      c.DataBindings.Add(binding);
      Interlocked.Decrement(ref this.m_changes);
    }

    private double? GetElectricityPrice(ISession ls, Location loc) => this.GetLocationCost(ls, loc)?.Electricity;

    private double? GetGasPrice(ISession ls, Location loc)
    {
      LocationCost locationCost = this.GetLocationCost(ls, loc);
      return locationCost == null ? new double?() : new double?(locationCost.Fuels / 10.002387672);
    }

    private LocationSpecies.Domain.Currency GetCurrency(ISession ls, Location loc)
    {
      Location location;
      LocationRelation locationRelation;
      for (location = loc; location != null && location.Currency == null; location = locationRelation.Parent)
      {
        locationRelation = this.GetLocationRelation(ls, location.Id);
        if (locationRelation == null)
        {
          location = (Location) null;
          break;
        }
      }
      if (location == null)
        return (LocationSpecies.Domain.Currency) null;
      NHibernateUtil.Initialize((object) location.Currency);
      return location.Currency;
    }

    private LocationRelation GetLocationRelation(ISession ls, int locId) => ls.CreateCriteria<LocationRelation>().Add((ICriterion) Restrictions.Eq("Location.Id", (object) locId)).Add((ICriterion) Restrictions.IsNotNull("Code")).SetCacheable(true).UniqueResult<LocationRelation>();

    private LocationCost GetLocationCost(ISession ls, Location loc)
    {
      Location location;
      LocationRelation locationRelation;
      for (location = loc; location != null && location.LocationCost == null; location = locationRelation.Parent)
      {
        locationRelation = this.GetLocationRelation(ls, location.Id);
        if (locationRelation == null)
        {
          location = (Location) null;
          break;
        }
      }
      if (location == null)
        return (LocationCost) null;
      NHibernateUtil.Initialize((object) location.LocationCost);
      return location.LocationCost;
    }

    private double ConvertM3ToGallon(double value) => !this.IsMetricUnitProject() ? value / 264.172 : value;

    private double ConvertGallonToM3(double value) => !this.IsMetricUnitProject() ? value * 264.172 : value;

    private double ConvertTonneToTon(double value) => !this.IsMetricUnitProject() ? value * 0.907185 : value;

    private double ConvertTonToTonne(double value) => !this.IsMetricUnitProject() ? value / 0.907185 : value;

    private double getCurrencyExchangeRate(string currencyAbbriviation)
    {
      try
      {
        string str = new WebClient()
        {
          UseDefaultCredentials = true
        }.DownloadString(Settings.Default.Https + Settings.Default.Host + Settings.Default.ExchangeRateUrl);
        if (!string.IsNullOrEmpty(str))
        {
          CurrencyRates currencyRates = JsonConvert.DeserializeObject<CurrencyRates>(str);
          if (currencyRates.Rates.ContainsKey(currencyAbbriviation))
            return currencyRates.Rates[currencyAbbriviation];
        }
      }
      catch (WebException ex)
      {
      }
      return 1.0;
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.ExportCSV(file);
    }

    private void ExportCSV(string file)
    {
      bool flag1 = !NationFeatures.IsUSorUSlikeNation(this.m_nation.Id);
      bool flag2 = this.m_nation.Id != 219;
      string currencyString = this.GetCurrencyString();
      string currencyPerElectricity = this.GetCurrencyPerElectricity();
      string currencyPerHeating = this.GetCurrencyPerHeating();
      string massPriceLabel = this.GetMassPriceLabel();
      string volumePriceLabel = this.GetVolumePriceLabel();
      using (FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite))
      {
        using (TextWriter writer = (TextWriter) new StreamWriter((Stream) fileStream))
        {
          using (CsvWriter csvWriter1 = new CsvWriter(writer, CultureInfo.CurrentCulture))
          {
            csvWriter1.WriteField(i_Tree_Eco_v6.Resources.Strings.BenefitName);
            csvWriter1.WriteField(i_Tree_Eco_v6.Resources.Strings.Units);
            csvWriter1.WriteField(i_Tree_Eco_v6.Resources.Strings.BenefitPrice);
            csvWriter1.NextRecord();
            double num;
            int decimalPlaces;
            if (flag2 && this.Year.ExchangeRate != null && this.Year.ExchangeRate.Price != 0.0)
            {
              csvWriter1.WriteField(i_Tree_Eco_v6.Resources.Strings.ExchangeRate);
              csvWriter1.WriteField(string.Format(i_Tree_Eco_v6.Resources.Strings.FmtValuePerUSD, (object) currencyString));
              CsvWriter csvWriter2 = csvWriter1;
              num = this.Year.ExchangeRate.Price;
              ref double local = ref num;
              decimalPlaces = this.txtExchangeRate.DecimalPlaces;
              string format = "F" + decimalPlaces.ToString();
              string field = local.ToString(format);
              csvWriter2.WriteField(field);
              csvWriter1.NextRecord();
            }
            if (this.Year.Electricity != null && this.Year.Electricity.Price != -1.0)
            {
              csvWriter1.WriteField(i_Tree_Eco_v6.Resources.Strings.Electricity);
              csvWriter1.WriteField(currencyPerElectricity);
              CsvWriter csvWriter3 = csvWriter1;
              num = this.Year.Electricity.Price;
              ref double local = ref num;
              decimalPlaces = this.txtElectricity.DecimalPlaces;
              string format = "F" + decimalPlaces.ToString();
              string field = local.ToString(format);
              csvWriter3.WriteField(field);
              csvWriter1.NextRecord();
            }
            if (this.Year.Gas != null && this.Year.Gas.Price != -1.0)
            {
              csvWriter1.WriteField(i_Tree_Eco_v6.Resources.Strings.Heating);
              csvWriter1.WriteField(currencyPerHeating);
              CsvWriter csvWriter4 = csvWriter1;
              num = this.Year.Gas.Price;
              ref double local = ref num;
              decimalPlaces = this.txtHeating.DecimalPlaces;
              string format = "F" + decimalPlaces.ToString();
              string field = local.ToString(format);
              csvWriter4.WriteField(field);
              csvWriter1.NextRecord();
            }
            if (this.Year.Carbon != null && this.Year.Carbon.Price != -1.0)
            {
              csvWriter1.WriteField(i_Tree_Eco_v6.Resources.Strings.Carbon);
              csvWriter1.WriteField(massPriceLabel);
              CsvWriter csvWriter5 = csvWriter1;
              num = this.ConvertTonneToTon(this.Year.Carbon.Price);
              ref double local = ref num;
              decimalPlaces = this.txtCarbon.DecimalPlaces;
              string format = "F" + decimalPlaces.ToString();
              string field = local.ToString(format);
              csvWriter5.WriteField(field);
              csvWriter1.NextRecord();
            }
            if (this.Year.H2O != null && this.Year.H2O.Price != -1.0)
            {
              csvWriter1.WriteField(i_Tree_Eco_v6.Resources.Strings.AvoidedRunoff);
              csvWriter1.WriteField(volumePriceLabel);
              CsvWriter csvWriter6 = csvWriter1;
              num = this.ConvertM3ToGallon(this.Year.H2O.Price);
              ref double local = ref num;
              decimalPlaces = this.txtAvoidedRunoff.DecimalPlaces;
              string format = "F" + decimalPlaces.ToString();
              string field = local.ToString(format);
              csvWriter6.WriteField(field);
              csvWriter1.NextRecord();
            }
            if (!flag1)
              return;
            if (this.Year.CO != null && this.Year.CO.Price != -1.0)
            {
              csvWriter1.WriteField(i_Tree_Eco_v6.Resources.Strings.CarbonMonoxideCO);
              csvWriter1.WriteField(massPriceLabel);
              CsvWriter csvWriter7 = csvWriter1;
              num = this.ConvertTonneToTon(this.Year.CO.Price);
              ref double local = ref num;
              decimalPlaces = this.txtCO.DecimalPlaces;
              string format = "F" + decimalPlaces.ToString();
              string field = local.ToString(format);
              csvWriter7.WriteField(field);
              csvWriter1.NextRecord();
            }
            if (this.Year.O3 != null && this.Year.O3.Price != -1.0)
            {
              csvWriter1.WriteField(i_Tree_Eco_v6.Resources.Strings.Ozone);
              csvWriter1.WriteField(massPriceLabel);
              CsvWriter csvWriter8 = csvWriter1;
              num = this.ConvertTonneToTon(this.Year.O3.Price);
              ref double local = ref num;
              decimalPlaces = this.txtOzone.DecimalPlaces;
              string format = "F" + decimalPlaces.ToString();
              string field = local.ToString(format);
              csvWriter8.WriteField(field);
              csvWriter1.NextRecord();
            }
            if (this.Year.NO2 != null && this.Year.NO2.Price != -1.0)
            {
              csvWriter1.WriteField(i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideNO2);
              csvWriter1.WriteField(massPriceLabel);
              CsvWriter csvWriter9 = csvWriter1;
              num = this.ConvertTonneToTon(this.Year.NO2.Price);
              ref double local = ref num;
              decimalPlaces = this.txtNO2.DecimalPlaces;
              string format = "F" + decimalPlaces.ToString();
              string field = local.ToString(format);
              csvWriter9.WriteField(field);
              csvWriter1.NextRecord();
            }
            if (this.Year.SO2 != null && this.Year.SO2.Price != -1.0)
            {
              csvWriter1.WriteField(i_Tree_Eco_v6.Resources.Strings.SulfurDioxideSO2);
              csvWriter1.WriteField(massPriceLabel);
              CsvWriter csvWriter10 = csvWriter1;
              num = this.ConvertTonneToTon(this.Year.SO2.Price);
              ref double local = ref num;
              decimalPlaces = this.txtSO2.DecimalPlaces;
              string format = "F" + decimalPlaces.ToString();
              string field = local.ToString(format);
              csvWriter10.WriteField(field);
              csvWriter1.NextRecord();
            }
            if (this.Year.PM25 == null || this.Year.PM25.Price == -1.0)
              return;
            csvWriter1.WriteField(i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25);
            csvWriter1.WriteField(massPriceLabel);
            CsvWriter csvWriter11 = csvWriter1;
            num = this.ConvertTonneToTon(this.Year.PM25.Price);
            ref double local1 = ref num;
            decimalPlaces = this.txtPM25.DecimalPlaces;
            string format1 = "F" + decimalPlaces.ToString();
            string field1 = local1.ToString(format1);
            csvWriter11.WriteField(field1);
            csvWriter1.NextRecord();
          }
        }
      }
    }

    private string GetCurrencyPerElectricity() => string.Format(i_Tree_Eco_v6.Resources.Strings.FmtValuePerValue, (object) this.GetCurrencyString(), (object) i_Tree_Eco_v6.Resources.Strings.UnitKilowattHourAbbr);

    private string GetCurrencyPerHeating() => string.Format(i_Tree_Eco_v6.Resources.Strings.FmtValuePerValue, (object) this.GetCurrencyString(), (object) i_Tree_Eco_v6.Resources.Strings.UnitTherm);

    private string GetMassPriceLabel()
    {
      string str = this.IsMetricUnitProject() ? i_Tree_Eco_v6.Resources.Strings.UnitTonne : i_Tree_Eco_v6.Resources.Strings.UnitTon;
      return string.Format(i_Tree_Eco_v6.Resources.Strings.FmtValuePerValue, (object) this.GetCurrencyString(), (object) str);
    }

    private string GetVolumePriceLabel()
    {
      string str = this.IsMetricUnitProject() ? i_Tree_Eco_v6.Resources.Strings.UnitCubicMetersAbbr : i_Tree_Eco_v6.Resources.Strings.UnitGallon;
      return string.Format(i_Tree_Eco_v6.Resources.Strings.FmtValuePerValue, (object) this.GetCurrencyString(), (object) str);
    }

    private bool IsMetricUnitProject() => this.Year.Unit == YearUnit.Metric;

    private string GetCurrencyString() => string.Format(i_Tree_Eco_v6.Resources.Strings.FmtCurrency, (object) this.m_currency.Symbol, (object) this.m_currency.Abbreviation);

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BenefitPricesForm));
      this.label1 = new Label();
      this.ep = new ErrorProvider(this.components);
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.tblBasic = new TableLayoutPanel();
      this.txtAvoidedRunoff = new NumericTextBox();
      this.txtCarbon = new NumericTextBox();
      this.txtHeating = new NumericTextBox();
      this.txtElectricity = new NumericTextBox();
      this.lblMeasurementUnits = new Label();
      this.lblAvoidedRunoff = new Label();
      this.lblCarbon = new Label();
      this.lblHeating = new Label();
      this.lblElectricity = new Label();
      this.label4 = new Label();
      this.tblInternational = new TableLayoutPanel();
      this.txtPM25 = new NumericTextBox();
      this.txtSO2 = new NumericTextBox();
      this.txtNO2 = new NumericTextBox();
      this.txtOzone = new NumericTextBox();
      this.txtCO = new NumericTextBox();
      this.cmdDefaultPM25 = new Button();
      this.lblPM25 = new Label();
      this.cmdDefaultSO2 = new Button();
      this.lblSO2 = new Label();
      this.lblOzone = new Label();
      this.cmdDefaultOzone = new Button();
      this.cmdDefaultNO2 = new Button();
      this.lblNO2 = new Label();
      this.cmdDefaultCO = new Button();
      this.lblCO = new Label();
      this.cmdDefaultElectricity = new Button();
      this.cmdDefaultHeating = new Button();
      this.cmdDefaultCarbon = new Button();
      this.cmdDefaultAvoidedRunoff = new Button();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.cmdOK = new Button();
      this.cmdCancel = new Button();
      this.lblDefaults = new Label();
      this.lblNotes = new Label();
      this.lblModify = new Label();
      this.label2 = new Label();
      this.tblExchangeRt = new TableLayoutPanel();
      this.txtExchangeRate = new NumericTextBox();
      this.label3 = new Label();
      this.lblExchangeRate = new Label();
      this.cmdDefaultExchangeRate = new Button();
      ((ISupportInitialize) this.ep).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.tblBasic.SuspendLayout();
      this.tblInternational.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.tblExchangeRt.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      this.ep.ContainerControl = (ContainerControl) this;
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.label1, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.tblBasic, 0, 4);
      this.tableLayoutPanel1.Controls.Add((Control) this.flowLayoutPanel1, 1, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblDefaults, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblNotes, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblModify, 0, 3);
      this.tableLayoutPanel1.Controls.Add((Control) this.label2, 0, 5);
      this.tableLayoutPanel1.Controls.Add((Control) this.tblExchangeRt, 0, 6);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.tblBasic, "tblBasic");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.tblBasic, 2);
      this.tblBasic.Controls.Add((Control) this.txtAvoidedRunoff, 1, 6);
      this.tblBasic.Controls.Add((Control) this.txtCarbon, 1, 5);
      this.tblBasic.Controls.Add((Control) this.txtHeating, 1, 4);
      this.tblBasic.Controls.Add((Control) this.txtElectricity, 1, 3);
      this.tblBasic.Controls.Add((Control) this.lblMeasurementUnits, 0, 0);
      this.tblBasic.Controls.Add((Control) this.lblAvoidedRunoff, 0, 6);
      this.tblBasic.Controls.Add((Control) this.lblCarbon, 0, 5);
      this.tblBasic.Controls.Add((Control) this.lblHeating, 0, 4);
      this.tblBasic.Controls.Add((Control) this.lblElectricity, 0, 3);
      this.tblBasic.Controls.Add((Control) this.label4, 0, 2);
      this.tblBasic.Controls.Add((Control) this.tblInternational, 0, 7);
      this.tblBasic.Controls.Add((Control) this.cmdDefaultElectricity, 2, 3);
      this.tblBasic.Controls.Add((Control) this.cmdDefaultHeating, 2, 4);
      this.tblBasic.Controls.Add((Control) this.cmdDefaultCarbon, 2, 5);
      this.tblBasic.Controls.Add((Control) this.cmdDefaultAvoidedRunoff, 2, 6);
      this.tblBasic.Name = "tblBasic";
      componentResourceManager.ApplyResources((object) this.txtAvoidedRunoff, "txtAvoidedRunoff");
      this.txtAvoidedRunoff.DecimalPlaces = 2;
      this.txtAvoidedRunoff.Format = "0.##;-0.##";
      this.txtAvoidedRunoff.HasDecimal = true;
      this.txtAvoidedRunoff.Name = "txtAvoidedRunoff";
      this.txtAvoidedRunoff.Signed = false;
      this.txtAvoidedRunoff.Validating += new CancelEventHandler(this.NumericTextBox_ValidatingGreaterThanOrEqualTo0);
      this.txtAvoidedRunoff.Validated += new EventHandler(this.txtAvoidedRunoff_Validated);
      componentResourceManager.ApplyResources((object) this.txtCarbon, "txtCarbon");
      this.txtCarbon.DecimalPlaces = 2;
      this.txtCarbon.Format = "0.##;-0.##";
      this.txtCarbon.HasDecimal = true;
      this.txtCarbon.Name = "txtCarbon";
      this.txtCarbon.Signed = false;
      this.txtCarbon.Validating += new CancelEventHandler(this.NumericTextBox_ValidatingGreaterThanOrEqualTo0);
      this.txtCarbon.Validated += new EventHandler(this.txtCarbon_Validated);
      componentResourceManager.ApplyResources((object) this.txtHeating, "txtHeating");
      this.txtHeating.DecimalPlaces = 2;
      this.txtHeating.Format = "0.##;-0.##";
      this.txtHeating.HasDecimal = true;
      this.txtHeating.Name = "txtHeating";
      this.txtHeating.Signed = false;
      this.txtHeating.Validating += new CancelEventHandler(this.NumericTextBox_ValidatingGreaterThanOrEqualTo0);
      this.txtHeating.Validated += new EventHandler(this.txtHeating_Validated);
      componentResourceManager.ApplyResources((object) this.txtElectricity, "txtElectricity");
      this.txtElectricity.DecimalPlaces = 2;
      this.txtElectricity.Format = "0.##;-0.##";
      this.txtElectricity.HasDecimal = true;
      this.txtElectricity.Name = "txtElectricity";
      this.txtElectricity.Signed = false;
      this.txtElectricity.Validating += new CancelEventHandler(this.NumericTextBox_ValidatingGreaterThanOrEqualTo0);
      this.txtElectricity.Validated += new EventHandler(this.txtElectricity_Validated);
      componentResourceManager.ApplyResources((object) this.lblMeasurementUnits, "lblMeasurementUnits");
      this.tblBasic.SetColumnSpan((Control) this.lblMeasurementUnits, 2);
      this.lblMeasurementUnits.Name = "lblMeasurementUnits";
      componentResourceManager.ApplyResources((object) this.lblAvoidedRunoff, "lblAvoidedRunoff");
      this.lblAvoidedRunoff.Name = "lblAvoidedRunoff";
      componentResourceManager.ApplyResources((object) this.lblCarbon, "lblCarbon");
      this.lblCarbon.Name = "lblCarbon";
      componentResourceManager.ApplyResources((object) this.lblHeating, "lblHeating");
      this.lblHeating.Name = "lblHeating";
      componentResourceManager.ApplyResources((object) this.lblElectricity, "lblElectricity");
      this.lblElectricity.Name = "lblElectricity";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.tblInternational, "tblInternational");
      this.tblBasic.SetColumnSpan((Control) this.tblInternational, 3);
      this.tblInternational.Controls.Add((Control) this.txtPM25, 1, 4);
      this.tblInternational.Controls.Add((Control) this.txtSO2, 1, 3);
      this.tblInternational.Controls.Add((Control) this.txtNO2, 1, 2);
      this.tblInternational.Controls.Add((Control) this.txtOzone, 1, 1);
      this.tblInternational.Controls.Add((Control) this.txtCO, 1, 0);
      this.tblInternational.Controls.Add((Control) this.cmdDefaultPM25, 2, 4);
      this.tblInternational.Controls.Add((Control) this.lblPM25, 0, 4);
      this.tblInternational.Controls.Add((Control) this.cmdDefaultSO2, 2, 3);
      this.tblInternational.Controls.Add((Control) this.lblSO2, 0, 3);
      this.tblInternational.Controls.Add((Control) this.lblOzone, 0, 1);
      this.tblInternational.Controls.Add((Control) this.cmdDefaultOzone, 2, 1);
      this.tblInternational.Controls.Add((Control) this.cmdDefaultNO2, 2, 2);
      this.tblInternational.Controls.Add((Control) this.lblNO2, 0, 2);
      this.tblInternational.Controls.Add((Control) this.cmdDefaultCO, 2, 0);
      this.tblInternational.Controls.Add((Control) this.lblCO, 0, 0);
      this.tblInternational.Name = "tblInternational";
      componentResourceManager.ApplyResources((object) this.txtPM25, "txtPM25");
      this.txtPM25.DecimalPlaces = 2;
      this.txtPM25.Format = "0.##;-0.##";
      this.txtPM25.HasDecimal = true;
      this.txtPM25.Name = "txtPM25";
      this.txtPM25.Signed = false;
      this.txtPM25.Validating += new CancelEventHandler(this.NumericTextBox_ValidatingGreaterThanOrEqualTo0);
      this.txtPM25.Validated += new EventHandler(this.txtPM25_Validated);
      componentResourceManager.ApplyResources((object) this.txtSO2, "txtSO2");
      this.txtSO2.DecimalPlaces = 2;
      this.txtSO2.Format = "0.##;-0.##";
      this.txtSO2.HasDecimal = true;
      this.txtSO2.Name = "txtSO2";
      this.txtSO2.Signed = false;
      this.txtSO2.Validating += new CancelEventHandler(this.NumericTextBox_ValidatingGreaterThanOrEqualTo0);
      this.txtSO2.Validated += new EventHandler(this.txtSO2_Validated);
      componentResourceManager.ApplyResources((object) this.txtNO2, "txtNO2");
      this.txtNO2.DecimalPlaces = 2;
      this.txtNO2.Format = "0.##;-0.##";
      this.txtNO2.HasDecimal = true;
      this.txtNO2.Name = "txtNO2";
      this.txtNO2.Signed = false;
      this.txtNO2.Validating += new CancelEventHandler(this.NumericTextBox_ValidatingGreaterThanOrEqualTo0);
      this.txtNO2.Validated += new EventHandler(this.txtNO2_Validated);
      componentResourceManager.ApplyResources((object) this.txtOzone, "txtOzone");
      this.txtOzone.DecimalPlaces = 2;
      this.txtOzone.Format = "0.##;-0.##";
      this.txtOzone.HasDecimal = true;
      this.txtOzone.Name = "txtOzone";
      this.txtOzone.Signed = false;
      this.txtOzone.Validating += new CancelEventHandler(this.NumericTextBox_ValidatingGreaterThanOrEqualTo0);
      this.txtOzone.Validated += new EventHandler(this.txtOzone_Validated);
      componentResourceManager.ApplyResources((object) this.txtCO, "txtCO");
      this.txtCO.DecimalPlaces = 2;
      this.txtCO.Format = "0.##;-0.##";
      this.txtCO.HasDecimal = true;
      this.txtCO.Name = "txtCO";
      this.txtCO.Signed = false;
      this.txtCO.Validating += new CancelEventHandler(this.NumericTextBox_ValidatingGreaterThanOrEqualTo0);
      this.txtCO.Validated += new EventHandler(this.txtCO_Validated);
      componentResourceManager.ApplyResources((object) this.cmdDefaultPM25, "cmdDefaultPM25");
      this.cmdDefaultPM25.Name = "cmdDefaultPM25";
      this.cmdDefaultPM25.UseVisualStyleBackColor = true;
      this.cmdDefaultPM25.Click += new EventHandler(this.cmdDefaultPM25_Click);
      componentResourceManager.ApplyResources((object) this.lblPM25, "lblPM25");
      this.lblPM25.Name = "lblPM25";
      componentResourceManager.ApplyResources((object) this.cmdDefaultSO2, "cmdDefaultSO2");
      this.cmdDefaultSO2.Name = "cmdDefaultSO2";
      this.cmdDefaultSO2.UseVisualStyleBackColor = true;
      this.cmdDefaultSO2.Click += new EventHandler(this.cmdDefaultSO2_Click);
      componentResourceManager.ApplyResources((object) this.lblSO2, "lblSO2");
      this.lblSO2.Name = "lblSO2";
      componentResourceManager.ApplyResources((object) this.lblOzone, "lblOzone");
      this.lblOzone.Name = "lblOzone";
      componentResourceManager.ApplyResources((object) this.cmdDefaultOzone, "cmdDefaultOzone");
      this.cmdDefaultOzone.Name = "cmdDefaultOzone";
      this.cmdDefaultOzone.UseVisualStyleBackColor = true;
      this.cmdDefaultOzone.Click += new EventHandler(this.cmdDefaultOzone_Click);
      componentResourceManager.ApplyResources((object) this.cmdDefaultNO2, "cmdDefaultNO2");
      this.cmdDefaultNO2.Name = "cmdDefaultNO2";
      this.cmdDefaultNO2.UseVisualStyleBackColor = true;
      this.cmdDefaultNO2.Click += new EventHandler(this.cmdDefaultNO2_Click);
      componentResourceManager.ApplyResources((object) this.lblNO2, "lblNO2");
      this.lblNO2.Name = "lblNO2";
      componentResourceManager.ApplyResources((object) this.cmdDefaultCO, "cmdDefaultCO");
      this.cmdDefaultCO.Name = "cmdDefaultCO";
      this.cmdDefaultCO.UseVisualStyleBackColor = true;
      this.cmdDefaultCO.Click += new EventHandler(this.cmdDefaultCO_Click);
      componentResourceManager.ApplyResources((object) this.lblCO, "lblCO");
      this.lblCO.Name = "lblCO";
      componentResourceManager.ApplyResources((object) this.cmdDefaultElectricity, "cmdDefaultElectricity");
      this.cmdDefaultElectricity.Name = "cmdDefaultElectricity";
      this.cmdDefaultElectricity.UseVisualStyleBackColor = true;
      this.cmdDefaultElectricity.Click += new EventHandler(this.cmdDefaultElectricity_Click);
      componentResourceManager.ApplyResources((object) this.cmdDefaultHeating, "cmdDefaultHeating");
      this.cmdDefaultHeating.Name = "cmdDefaultHeating";
      this.cmdDefaultHeating.UseVisualStyleBackColor = true;
      this.cmdDefaultHeating.Click += new EventHandler(this.cmdDefaultHeating_Click);
      componentResourceManager.ApplyResources((object) this.cmdDefaultCarbon, "cmdDefaultCarbon");
      this.cmdDefaultCarbon.Name = "cmdDefaultCarbon";
      this.cmdDefaultCarbon.UseVisualStyleBackColor = true;
      this.cmdDefaultCarbon.Click += new EventHandler(this.cmdDefaultCarbon_Click);
      componentResourceManager.ApplyResources((object) this.cmdDefaultAvoidedRunoff, "cmdDefaultAvoidedRunoff");
      this.cmdDefaultAvoidedRunoff.Name = "cmdDefaultAvoidedRunoff";
      this.cmdDefaultAvoidedRunoff.UseVisualStyleBackColor = true;
      this.cmdDefaultAvoidedRunoff.Click += new EventHandler(this.cmdDefaultAvoidedRunoff_Click);
      componentResourceManager.ApplyResources((object) this.flowLayoutPanel1, "flowLayoutPanel1");
      this.flowLayoutPanel1.Controls.Add((Control) this.cmdOK);
      this.flowLayoutPanel1.Controls.Add((Control) this.cmdCancel);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.cmdOK, "cmdOK");
      this.cmdOK.Name = "cmdOK";
      this.cmdOK.UseVisualStyleBackColor = true;
      this.cmdOK.Click += new EventHandler(this.cmdOK_Click);
      componentResourceManager.ApplyResources((object) this.cmdCancel, "cmdCancel");
      this.cmdCancel.Name = "cmdCancel";
      this.cmdCancel.UseVisualStyleBackColor = true;
      this.cmdCancel.Click += new EventHandler(this.cmdCancel_Click);
      componentResourceManager.ApplyResources((object) this.lblDefaults, "lblDefaults");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblDefaults, 2);
      this.lblDefaults.Name = "lblDefaults";
      componentResourceManager.ApplyResources((object) this.lblNotes, "lblNotes");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblNotes, 2);
      this.lblNotes.Name = "lblNotes";
      componentResourceManager.ApplyResources((object) this.lblModify, "lblModify");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblModify, 2);
      this.lblModify.Name = "lblModify";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label2, 2);
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.tblExchangeRt, "tblExchangeRt");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.tblExchangeRt, 2);
      this.tblExchangeRt.Controls.Add((Control) this.txtExchangeRate, 1, 1);
      this.tblExchangeRt.Controls.Add((Control) this.label3, 0, 0);
      this.tblExchangeRt.Controls.Add((Control) this.lblExchangeRate, 0, 1);
      this.tblExchangeRt.Controls.Add((Control) this.cmdDefaultExchangeRate, 2, 1);
      this.tblExchangeRt.Name = "tblExchangeRt";
      componentResourceManager.ApplyResources((object) this.txtExchangeRate, "txtExchangeRate");
      this.txtExchangeRate.DecimalPlaces = 5;
      this.txtExchangeRate.Format = "0.#####;-0.#####";
      this.txtExchangeRate.HasDecimal = true;
      this.txtExchangeRate.Name = "txtExchangeRate";
      this.txtExchangeRate.Signed = true;
      this.txtExchangeRate.Validating += new CancelEventHandler(this.NumericTextBox_ValidatingGreaterThan0);
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.tblExchangeRt.SetColumnSpan((Control) this.label3, 3);
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.lblExchangeRate, "lblExchangeRate");
      this.lblExchangeRate.Name = "lblExchangeRate";
      componentResourceManager.ApplyResources((object) this.cmdDefaultExchangeRate, "cmdDefaultExchangeRate");
      this.cmdDefaultExchangeRate.Name = "cmdDefaultExchangeRate";
      this.cmdDefaultExchangeRate.UseVisualStyleBackColor = true;
      this.cmdDefaultExchangeRate.Click += new EventHandler(this.cmdDefaultExchangeRate_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoValidate = AutoValidate.EnableAllowFocusChange;
      this.BackColor = SystemColors.Window;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.DoubleBuffered = true;
      this.Name = nameof (BenefitPricesForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel1, 0);
      ((ISupportInitialize) this.ep).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.tblBasic.ResumeLayout(false);
      this.tblBasic.PerformLayout();
      this.tblInternational.ResumeLayout(false);
      this.tblInternational.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.tblExchangeRt.ResumeLayout(false);
      this.tblExchangeRt.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
