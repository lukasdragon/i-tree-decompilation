// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.WeatherChart
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.Win.C1Chart;
using C1.Win.C1FlexGrid;
using DaveyTree.NHibernate;
using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Forms;
using i_Tree_Eco_v6.Properties;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace i_Tree_Eco_v6.Reports
{
  public class WeatherChart : ContentForm
  {
    private WeatherReport _curReport;
    private EstimateUtil m_estUtil;
    private ProgramSession m_ps = ProgramSession.GetInstance();
    private ISession m_session;
    private Year curYear;
    private IContainer components;
    private C1.Win.C1Chart.C1Chart chart;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbPan;
    private ToolStripButton tsbZoomIn;
    private ToolStripButton tsbZoomOut;
    private ToolStripButton tsbShowAll;
    private ToolStripComboBox tscmbPollutant;
    private TabControl tabControl1;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private C1.Win.C1FlexGrid.C1FlexGrid fgTable;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton tsbExport;
    private Panel pnlWarning;
    private System.Windows.Forms.Label label1;

    public WeatherChart(WeatherReport CurReport)
    {
      this.InitializeComponent();
      this._curReport = CurReport;
      this.m_estUtil = new EstimateUtil(this.m_ps.InputSession, this.m_ps.LocSp);
      using (ISession session = this.m_ps.InputSession.CreateSession())
      {
        foreach (object obj in (IEnumerable<string>) session.GetNamedQuery("GetPollutants").SetParameter<Guid?>("y", this.m_ps.InputSession.YearKey).List<string>())
          this.tscmbPollutant.Items.Add(obj);
        if (this.tscmbPollutant.SelectedIndex == -1)
          this.tscmbPollutant.SelectedIndex = 0;
      }
      this.m_ps = Program.Session;
      this.ToggleBanner();
      this.m_ps.InputSessionChanged += new EventHandler(this.m_ps_InputSessionChanged);
    }

    private void m_ps_InputSessionChanged(object sender, EventArgs e)
    {
      if (this.m_ps.InputSession == null)
        return;
      this.m_ps.InputSession.YearChanged += new EventHandler(this.InputSession_YearChanged);
    }

    private void InputSession_YearChanged(object sender, EventArgs e)
    {
      if (!this.m_ps.InputSession.YearKey.HasValue)
        return;
      this.ToggleBanner();
    }

    private void ToggleBanner()
    {
      this.m_session = this.m_ps.InputSession.CreateSession();
      this.curYear = this.m_session.Get<Year>((object) this.m_ps.InputSession.YearKey.Value);
      int num = this.m_session.CreateCriteria<YearResult>().SetProjection((IProjection) Projections.Max("RevProcessed")).Add((ICriterion) Restrictions.Eq("Year.Guid", (object) this.m_ps.InputSession.YearKey)).UniqueResult<int>();
      this.pnlWarning.Visible = this.curYear.Changed || this.curYear.RevProcessed != num;
    }

    private void chart_Transform(object sender, TransformEventArgs e)
    {
      if (e.MinX < this.chart.ChartGroups.Group0.ChartData.MinX)
        e.Cancel = true;
      if (e.MaxX > this.chart.ChartGroups.Group0.ChartData.MaxX)
        e.Cancel = true;
      foreach (ToolStripItem toolStripItem in (ArrangedElementCollection) this.toolStrip1.Items)
      {
        if (toolStripItem is ToolStripButton toolStripButton)
          toolStripButton.Checked = false;
      }
      this.tsbPan.Checked = true;
    }

    public void RefreshChart()
    {
      string empty1 = string.Empty;
      string treeShrubCondition = " and TreeShrub='T' ";
      double valueMultiplier = 1.0;
      string empty2 = string.Empty;
      Tuple<Units, Units, Units> SourceEstUnit = Tuple.Create<Units, Units, Units>(Units.None, Units.None, Units.None);
      this.tscmbPollutant.Visible = true;
      this.chart.Font = new Font("Calibri", 14f);
      string valuePerHrStr = ReportUtil.GetValuePerHrStr(ReportUtil.GetValuePerValueStr(this.m_ps.UseEnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitOuncesAbbr : i_Tree_Eco_v6.Resources.Strings.UnitGramsAbbr, this.m_ps.UseEnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitSquareFeetAbbr : i_Tree_Eco_v6.Resources.Strings.UnitSquareMetersAbbr));
      string para;
      string str;
      switch (this._curReport)
      {
        case WeatherReport.AirPollutantConcentrationUGM3:
          string text = this.tscmbPollutant.Text;
          if (!(text == "PM2.5"))
          {
            if (text == "PM10*")
            {
              para = "uGm3";
              str = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.AirPollutantConcentration, ReportUtil.GetValuePerValueStr(i_Tree_Eco_v6.Resources.Strings.UnitMicroGramAbbr, i_Tree_Eco_v6.Resources.Strings.UnitCubicMetersAbbr));
              break;
            }
            para = "PPM";
            str = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.AirPollutantConcentration, i_Tree_Eco_v6.Resources.Strings.PPM);
            break;
          }
          para = "uGm3";
          str = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.AirPollutantConcentration, ReportUtil.GetValuePerValueStr(i_Tree_Eco_v6.Resources.Strings.UnitMicroGramAbbr, i_Tree_Eco_v6.Resources.Strings.UnitCubicMetersAbbr));
          break;
        case WeatherReport.PhotosyntheticallyActiveRadiation:
          para = "PARWm2";
          str = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.PhotosyntheticallyActiveRadiation, ReportUtil.GetValuePerValueStr("W", this.m_ps.UseEnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitSquareFeetAbbr : i_Tree_Eco_v6.Resources.Strings.UnitSquareMetersAbbr));
          SourceEstUnit = Tuple.Create<Units, Units, Units>(Units.Pounds, Units.Squaremeter, Units.None);
          this.tscmbPollutant.Visible = false;
          break;
        case WeatherReport.Rain:
          para = "RainMh";
          str = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Rain, ReportUtil.GetValuePerHrStr(this.m_ps.UseEnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitInchesAbbr : i_Tree_Eco_v6.Resources.Strings.UnitCentimetersAbbr));
          SourceEstUnit = Tuple.Create<Units, Units, Units>(Units.Centimeters, Units.None, Units.None);
          this.tscmbPollutant.Visible = false;
          break;
        case WeatherReport.TempF:
          para = "TempF";
          str = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Temperature, this.m_ps.UseEnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitFahrenheit : i_Tree_Eco_v6.Resources.Strings.UnitCelsius);
          SourceEstUnit = Tuple.Create<Units, Units, Units>(Units.TempFahrenheit, Units.None, Units.None);
          this.tscmbPollutant.Visible = false;
          break;
        case WeatherReport.TempK:
          para = "TempK";
          str = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Temperature, i_Tree_Eco_v6.Resources.Strings.UnitKelvin);
          this.tscmbPollutant.Visible = false;
          break;
        case WeatherReport.AirQualityImprovementForTreeCover:
          para = "ActAqImp";
          str = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.AirQualityImprovementByTrees, "%");
          break;
        case WeatherReport.AirQualityImprovementForShrubCover:
          para = "ActAqImp";
          str = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.AirQualityImprovementByShrubs, "%");
          treeShrubCondition = " and TreeShrub='S' ";
          break;
        case WeatherReport.AirQualityImprovementForGrassCover:
          para = "ActAqImp";
          str = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.AirQualityImprovementByGrasslands, "%");
          treeShrubCondition = " and TreeShrub='G' ";
          break;
        case WeatherReport.DryDepTree:
          para = "Flux";
          str = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.AirPollutantFluxPerUnitTreeCover, valuePerHrStr);
          SourceEstUnit = Tuple.Create<Units, Units, Units>(Units.Grams, Units.Squaremeter, Units.Hour);
          break;
        case WeatherReport.DryDepShrub:
          para = "Flux";
          treeShrubCondition = " and TreeShrub='S' ";
          str = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.AirPollutantFluxPerUnitShrubCover, valuePerHrStr);
          SourceEstUnit = Tuple.Create<Units, Units, Units>(Units.Grams, Units.Squaremeter, Units.Hour);
          break;
        case WeatherReport.DryDepGrass:
          para = "Flux";
          treeShrubCondition = " and TreeShrub='G' ";
          str = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.AirPollutantFluxPerUnitGrasslandCover, valuePerHrStr);
          SourceEstUnit = Tuple.Create<Units, Units, Units>(Units.Grams, Units.Squaremeter, Units.Hour);
          break;
        default:
          int num = (int) MessageBox.Show(i_Tree_Eco_v6.Resources.Strings.MsgNotImplemented);
          return;
      }
      IQuery hourlyPollution = this.m_estUtil.queryProvider.GetEstimateUtilProvider().GetHourlyPollution(para, treeShrubCondition, valueMultiplier);
      string val = para == "Trans" ? this.GetTopPollutantName(treeShrubCondition) : this.tscmbPollutant.SelectedItem.ToString();
      Guid? yearKey = this.m_ps.InputSession.YearKey;
      DataTable dataTable = hourlyPollution.SetParameter<Guid?>("y", yearKey).SetParameter<string>("pollName", val).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
      this.chart.ChartGroups.Group0.ChartData.SeriesList.Clear();
      ChartDataSeries chartDataSeries = this.chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      this.chart.ChartArea.AxisX.AnnoMethod = AnnotationMethodEnum.Values;
      this.chart.ChartArea.AxisX.AnnoFormat = FormatEnum.DateShort;
      this.chart.ChartArea.AxisX.NoAnnoOverlap = true;
      chartDataSeries.SymbolStyle.Shape = SymbolShapeEnum.None;
      this.chart.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Date;
      this.chart.ChartArea.AxisY.Text = str;
      this.chart.Interaction.Enabled = true;
      this.fgTable.Clear();
      List<KeyValuePair<DateTime, double>> keyValuePairList = new List<KeyValuePair<DateTime, double>>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        DateTime key = ReportUtil.ConvertFromDBVal<DateTime>(row["DT"]);
        double d = ReportUtil.ConvertFromDBVal<double>(row["avgValue"]);
        if (this._curReport == WeatherReport.Rain)
          d *= 100.0;
        if (!Tuple.Create<Units, Units, Units>(Units.None, Units.None, Units.None).Equals((object) SourceEstUnit))
          d = EstimateUtil.ConvertToEnglish(d, SourceEstUnit, this.m_ps.UseEnglishUnits);
        keyValuePairList.Add(new KeyValuePair<DateTime, double>(key, d));
      }
      if ((this._curReport.Equals((object) WeatherReport.DryDepGrass) || this._curReport.Equals((object) WeatherReport.AirQualityImprovementForGrassCover)) && keyValuePairList.Count <= 0)
      {
        this.fgTable.Visible = false;
        this.chart.ChartArea.Visible = false;
        this.chart.Header.Visible = true;
        this.chart.Style.HorizontalAlignment = AlignHorzEnum.Near;
        this.chart.Style.ForeColor = Color.Red;
        this.chart.Header.Text = i_Tree_Eco_v6.Resources.Strings.MsgGrassReport;
      }
      else
      {
        this.fgTable.Rows.Count = keyValuePairList.Count + 1;
        this.fgTable.Cols.Count = 3;
        this.fgTable.Cols[1].Caption = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtValuePerValue, (object) i_Tree_Eco_v6.Resources.Strings.Date, (object) i_Tree_Eco_v6.Resources.Strings.Time);
        this.fgTable.Cols[2].Caption = str;
        this.fgTable.Cols[2].TextAlign = TextAlignEnum.RightCenter;
        this.fgTable.Cols[2].Format = "G3";
        chartDataSeries.LineStyle.Color = Color.FromArgb(103, 159, 213);
        for (int index = 0; index < keyValuePairList.Count; ++index)
        {
          chartDataSeries.X.Add((object) keyValuePairList[index].Key);
          chartDataSeries.Y.Add((object) keyValuePairList[index].Value);
          this.fgTable[index + 1, 1] = (object) keyValuePairList[index].Key;
          this.fgTable[index + 1, 2] = (object) keyValuePairList[index].Value;
        }
        this.fgTable.AutoSizeCols();
        if (keyValuePairList.Count > 0)
          this.chart.Header.Text = ((DateTime) this.fgTable.Rows[1][1]).Year.ToString();
        this.chart.ChartArea.AxisY.AutoMax = true;
        this.chart.ChartArea.AxisY.AutoMin = true;
        this.chart.ChartArea.AxisY.AutoOrigin = true;
      }
    }

    private string GetTopPollutantName(string treeShrubCondition) => this.m_estUtil.queryProvider.GetEstimateUtilProvider().GetTopPollutant(treeShrubCondition).SetParameter<Guid?>("y", this.m_ps.InputSession.YearKey).UniqueResult<string>();

    private void tsbExport_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.ShowHelp = false;
      saveFileDialog.CreatePrompt = false;
      saveFileDialog.OverwritePrompt = true;
      if (this.tabControl1.SelectedIndex == 0)
      {
        saveFileDialog.Filter = string.Join("|", new string[2]
        {
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterPNG, (object) Settings.Default.ExtPNG),
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterJPG, (object) Settings.Default.ExtJPG)
        });
        if (saveFileDialog.ShowDialog() != DialogResult.OK)
          return;
        StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase;
        string extension = Path.GetExtension(saveFileDialog.FileName);
        if (Settings.Default.ExtPNG.EndsWith(extension, comparisonType))
        {
          this.chart.SaveImage(saveFileDialog.FileName, ImageFormat.Png);
        }
        else
        {
          if (!Settings.Default.ExtJPG.EndsWith(extension, comparisonType))
            return;
          this.chart.SaveImage(saveFileDialog.FileName, ImageFormat.Jpeg);
        }
      }
      else
      {
        saveFileDialog.Filter = string.Join("|", new string[2]
        {
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterCSV, (object) Settings.Default.ExtCSV),
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterExcel, (object) Settings.Default.ExtExcel)
        });
        saveFileDialog.Title = i_Tree_Eco_v6.Resources.Strings.ChartExportTableAs;
        if (saveFileDialog.ShowDialog() != DialogResult.OK)
          return;
        StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase;
        string extension = Path.GetExtension(saveFileDialog.FileName);
        if (Settings.Default.ExtCSV.EndsWith(extension, comparisonType))
        {
          this.fgTable.SaveGrid(saveFileDialog.FileName, FileFormatEnum.TextComma);
        }
        else
        {
          if (!Settings.Default.ExtExcel.EndsWith(extension, comparisonType))
            return;
          this.fgTable.SaveExcel(saveFileDialog.FileName);
        }
      }
    }

    private void tsbPan_CheckedChanged(object sender, EventArgs e)
    {
      this.chart.Interaction.IsDefault = true;
      if (!this.tsbPan.Checked)
        return;
      this.chart.Interaction.Actions["Rotate"].Modifier = Keys.Shift | Keys.Alt;
      this.chart.Interaction.Actions["Scale"].Modifier = Keys.Shift | Keys.Alt;
      this.chart.Interaction.Actions["Zoom"].Modifier = Keys.Shift | Keys.Alt;
      this.chart.Interaction.Actions["Translate"].Modifier = Keys.None;
      this.chart.Interaction.Actions["Translate"].Axis = AxisFlagEnum.AxisX;
    }

    private void tsbPan_Click(object sender, EventArgs e)
    {
    }

    private void tsbShowAll_Click(object sender, EventArgs e)
    {
      this.chart.ChartArea.AxisX.AutoMax = true;
      this.chart.ChartArea.AxisX.AutoMin = true;
    }

    private void tsbZoomIn_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.tsbZoomIn.Checked)
        return;
      this.tsbPan.Checked = false;
      this.chart.Interaction.Actions["Rotate"].Modifier = Keys.Shift | Keys.Alt;
      this.chart.Interaction.Actions["Scale"].Modifier = Keys.Shift | Keys.Alt;
      this.chart.Interaction.Actions["Translate"].Modifier = Keys.Shift | Keys.Alt;
      this.chart.Interaction.Actions["Zoom"].Modifier = Keys.None;
      this.chart.Interaction.Actions["Zoom"].Axis = AxisFlagEnum.AxisX;
    }

    private void tsbZoomIn_Click(object sender, EventArgs e)
    {
    }

    private void tsbZoomOut_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void tsbZoomOut_Click(object sender, EventArgs e)
    {
      double num = (this.chart.ChartArea.AxisX.Max - this.chart.ChartArea.AxisX.Min) * 0.25;
      if (this.chart.ChartArea.AxisX.Min - num >= this.chart.ChartGroups.Group0.ChartData.MinX)
        this.chart.ChartArea.AxisX.Min -= num;
      else
        this.chart.ChartArea.AxisX.Min = this.chart.ChartGroups.Group0.ChartData.MinX;
      if (this.chart.ChartArea.AxisX.Max + num <= this.chart.ChartGroups.Group0.ChartData.MaxX)
        this.chart.ChartArea.AxisX.Max += num;
      else
        this.chart.ChartArea.AxisX.Max = this.chart.ChartGroups.Group0.ChartData.MaxX;
    }

    private void tscmbPollutant_Click(object sender, EventArgs e) => this.RefreshChart();

    private void chart_MouseClick(object sender, MouseEventArgs e)
    {
      if (!this.tsbZoomIn.Checked)
        return;
      double XDataCoord = 0.0;
      double YDataCoord = 0.0;
      if (!this.chart.ChartGroups.Group0.CoordToDataCoord(e.X, e.Y, ref XDataCoord, ref YDataCoord))
        return;
      double num = (this.chart.ChartArea.AxisX.Max - this.chart.ChartArea.AxisX.Min) * 0.5;
      this.chart.ChartArea.AxisX.Min = XDataCoord - num / 2.0 <= this.chart.ChartGroups.Group0.ChartData.MinX ? this.chart.ChartGroups.Group0.ChartData.MinX : XDataCoord - num / 2.0;
      if (XDataCoord + num / 2.0 < this.chart.ChartGroups.Group0.ChartData.MaxX)
        this.chart.ChartArea.AxisX.Max = XDataCoord + num / 2.0;
      else
        this.chart.ChartArea.AxisX.Max = this.chart.ChartGroups.Group0.ChartData.MaxX;
    }

    private void WeatherChart_Load(object sender, EventArgs e) => this.fgTable.AutoSizeCols();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (WeatherChart));
      this.chart = new C1.Win.C1Chart.C1Chart();
      this.toolStrip1 = new ToolStrip();
      this.tsbPan = new ToolStripButton();
      this.tsbZoomIn = new ToolStripButton();
      this.tsbZoomOut = new ToolStripButton();
      this.tsbShowAll = new ToolStripButton();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.tscmbPollutant = new ToolStripComboBox();
      this.tsbExport = new ToolStripButton();
      this.tabControl1 = new TabControl();
      this.tabPage1 = new TabPage();
      this.tabPage2 = new TabPage();
      this.fgTable = new C1.Win.C1FlexGrid.C1FlexGrid();
      this.pnlWarning = new Panel();
      this.label1 = new System.Windows.Forms.Label();
      this.chart.BeginInit();
      this.toolStrip1.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.fgTable.BeginInit();
      this.pnlWarning.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.chart.BackColor = Color.Transparent;
      componentResourceManager.ApplyResources((object) this.chart, "chart");
      this.chart.Name = "chart";
      this.chart.PropBag = componentResourceManager.GetString("chart.PropBag");
      this.chart.Transform += new TransformEventHandler(this.chart_Transform);
      this.chart.MouseClick += new MouseEventHandler(this.chart_MouseClick);
      this.toolStrip1.Items.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.tsbPan,
        (ToolStripItem) this.tsbZoomIn,
        (ToolStripItem) this.tsbZoomOut,
        (ToolStripItem) this.tsbShowAll,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.tscmbPollutant,
        (ToolStripItem) this.tsbExport
      });
      componentResourceManager.ApplyResources((object) this.toolStrip1, "toolStrip1");
      this.toolStrip1.Name = "toolStrip1";
      this.tsbPan.CheckOnClick = true;
      this.tsbPan.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.tsbPan, "tsbPan");
      this.tsbPan.Name = "tsbPan";
      this.tsbPan.CheckedChanged += new EventHandler(this.tsbPan_CheckedChanged);
      this.tsbPan.Click += new EventHandler(this.tsbPan_Click);
      this.tsbZoomIn.CheckOnClick = true;
      this.tsbZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.tsbZoomIn, "tsbZoomIn");
      this.tsbZoomIn.Name = "tsbZoomIn";
      this.tsbZoomIn.CheckedChanged += new EventHandler(this.tsbZoomIn_CheckedChanged);
      this.tsbZoomIn.Click += new EventHandler(this.tsbZoomIn_Click);
      this.tsbZoomOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.tsbZoomOut, "tsbZoomOut");
      this.tsbZoomOut.Name = "tsbZoomOut";
      this.tsbZoomOut.CheckedChanged += new EventHandler(this.tsbZoomOut_CheckedChanged);
      this.tsbZoomOut.Click += new EventHandler(this.tsbZoomOut_Click);
      this.tsbShowAll.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.tsbShowAll.Image = (Image) i_Tree_Eco_v6.Properties.Resources.ShowAll;
      componentResourceManager.ApplyResources((object) this.tsbShowAll, "tsbShowAll");
      this.tsbShowAll.Name = "tsbShowAll";
      this.tsbShowAll.Click += new EventHandler(this.tsbShowAll_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator1, "toolStripSeparator1");
      this.tscmbPollutant.DropDownStyle = ComboBoxStyle.DropDownList;
      this.tscmbPollutant.Name = "tscmbPollutant";
      componentResourceManager.ApplyResources((object) this.tscmbPollutant, "tscmbPollutant");
      this.tscmbPollutant.SelectedIndexChanged += new EventHandler(this.tscmbPollutant_Click);
      this.tsbExport.DisplayStyle = ToolStripItemDisplayStyle.Text;
      componentResourceManager.ApplyResources((object) this.tsbExport, "tsbExport");
      this.tsbExport.Name = "tsbExport";
      this.tsbExport.Click += new EventHandler(this.tsbExport_Click);
      this.tabControl1.Controls.Add((Control) this.tabPage1);
      this.tabControl1.Controls.Add((Control) this.tabPage2);
      componentResourceManager.ApplyResources((object) this.tabControl1, "tabControl1");
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabPage1.Controls.Add((Control) this.chart);
      componentResourceManager.ApplyResources((object) this.tabPage1, "tabPage1");
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.tabPage2.Controls.Add((Control) this.fgTable);
      componentResourceManager.ApplyResources((object) this.tabPage2, "tabPage2");
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.fgTable, "fgTable");
      this.fgTable.Name = "fgTable";
      this.fgTable.Rows.DefaultSize = 19;
      this.pnlWarning.BackColor = Color.Red;
      this.pnlWarning.Controls.Add((Control) this.label1);
      this.pnlWarning.Dock = DockStyle.Top;
      this.pnlWarning.Location = new Point(0, 29);
      this.pnlWarning.Name = "pnlWarning";
      this.pnlWarning.Size = new Size(709, 27);
      this.pnlWarning.TabIndex = 3;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.White;
      this.label1.Location = new Point(4, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(700, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Warning! This chart is out of date due to recent edits. Please Submit Data for Processing to update it.";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.toolStrip1);
      this.Controls.Add((Control) this.pnlWarning);
      this.Name = nameof (WeatherChart);
      this.Load += new EventHandler(this.WeatherChart_Load);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.pnlWarning, 0);
      this.Controls.SetChildIndex((Control) this.toolStrip1, 0);
      this.Controls.SetChildIndex((Control) this.tabControl1, 0);
      this.chart.EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.fgTable.EndInit();
      this.pnlWarning.ResumeLayout(false);
      this.pnlWarning.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
