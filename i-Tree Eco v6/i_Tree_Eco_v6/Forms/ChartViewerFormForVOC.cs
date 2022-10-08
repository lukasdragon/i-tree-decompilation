// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ChartViewerFormForVOC
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.Win.C1Chart;
using C1.Win.C1FlexGrid;
using i_Tree_Eco_v6.Properties;
using i_Tree_Eco_v6.Reports.Chart;
using i_Tree_Eco_v6.Resources;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace i_Tree_Eco_v6.Forms
{
  public class ChartViewerFormForVOC : ContentForm
  {
    private VOCDateValueChart _c;
    private readonly TextAlignEnum dataColFormat = TextAlignEnum.RightCenter;
    private IContainer components;
    private C1.Win.C1Chart.C1Chart chart;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbPan;
    private ToolStripButton tsbZoomIn;
    private ToolStripButton tsbZoomOut;
    private ToolStripButton tsbShowAll;
    private TabControl tabControl1;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private C1.Win.C1FlexGrid.C1FlexGrid fgTable;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton tsbExport;

    public ChartViewerFormForVOC() => this.InitializeComponent();

    public VOCDateValueChart Chart
    {
      get => this._c;
      set
      {
        this._c = value;
        this.OnShowHelp(this._c.HelpTopic);
        this.MakeChart();
      }
    }

    public void MakeChart()
    {
      this.chart.DataSource = (object) new DataView(this._c.dt);
      ChartDataSeriesCollection seriesList = this.chart.ChartGroups[0].ChartData.SeriesList;
      seriesList.Clear();
      ChartDataSeries chartDataSeries = seriesList.AddNewSeries();
      chartDataSeries.X.DataField = this._c.dt.Columns[0].ColumnName;
      chartDataSeries.Y.DataField = this._c.dt.Columns[1].ColumnName;
      chartDataSeries.SymbolStyle.Shape = SymbolShapeEnum.None;
      chartDataSeries.LineStyle.Color = Color.FromArgb(103, 159, 213);
      this.chart.Font = new Font("Calibri", 14f);
      this.chart.ChartArea.AxisX.AnnoMethod = AnnotationMethodEnum.Values;
      this.chart.ChartArea.AxisX.AnnoFormat = FormatEnum.DateShort;
      this.chart.ChartArea.AxisX.NoAnnoOverlap = true;
      this.chart.Interaction.Enabled = true;
      this.chart.Legend.Visible = false;
      this.chart.ChartArea.AxisY.AutoOrigin = true;
      this.chart.ChartArea.AxisY2.AutoOrigin = true;
      this.chart.ChartArea.AxisY.AutoMajor = true;
      this.chart.ChartArea.AxisY2.AutoMajor = true;
      this.chart.ChartArea.AxisY.GridMajor.Visible = false;
      this.chart.ChartArea.AxisY2.GridMajor.Visible = false;
      this.chart.ChartArea.AxisX.Text = this._c.XAxisTitle;
      this.chart.ChartArea.AxisY.Text = this._c.YAxisTitle;
      this.chart.ChartArea.AxisY2.AutoMax = false;
      this.chart.ChartArea.AxisY2.AutoMin = false;
      this.chart.ChartArea.AxisY.AutoMax = false;
      this.chart.ChartArea.AxisY.AutoMin = false;
      if (this._c.dt.Rows.Count > 0)
      {
        double num = Convert.ToDouble(this._c.dt.Compute("min(ITEM)", string.Empty));
        this.chart.ChartArea.AxisY.Max = Convert.ToDouble(this._c.dt.Compute("max(ITEM)", string.Empty));
        this.chart.ChartArea.AxisY.Min = num;
      }
      this.chart.Header.Text = this._c.MainTitle;
      this.fgTable.Clear();
      this.fgTable.DataSource = (object) new DataView(this._c.dt);
      this.fgTable.Cols[1].Caption = this._c.TableXTitle;
      this.fgTable.Cols[2].Caption = this._c.TableYTitle;
      this.fgTable.Cols[1].Format = "g";
      this.fgTable.Cols[2].Format = "G3";
      this.fgTable.Cols[2].TextAlign = this.dataColFormat;
      this.fgTable.AutoSizeCols();
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
          string.Format(Strings.FmtFilter, (object) Strings.FilterPNG, (object) Settings.Default.ExtPNG),
          string.Format(Strings.FmtFilter, (object) Strings.FilterJPG, (object) Settings.Default.ExtJPG)
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
          string.Format(Strings.FmtFilter, (object) Strings.FilterCSV, (object) Settings.Default.ExtCSV),
          string.Format(Strings.FmtFilter, (object) Strings.FilterExcel, (object) Settings.Default.ExtExcel)
        });
        saveFileDialog.Title = Strings.ChartExportTableAs;
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

    private void tsbShowAll_Click(object sender, EventArgs e)
    {
      this.chart.ChartArea.AxisX.AutoMax = true;
      this.chart.ChartArea.AxisX.AutoMin = true;
    }

    private void tsbZoomIn_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.tsbZoomIn.Checked)
        return;
      Interaction interaction = this.chart.Interaction;
      this.tsbPan.Checked = false;
      interaction.Actions["Zoom"].Axis = AxisFlagEnum.AxisX;
      interaction.Actions["Rotate"].Modifier = Keys.Shift | Keys.Alt;
      interaction.Actions["Scale"].Modifier = Keys.Shift | Keys.Alt;
      interaction.Actions["Translate"].Modifier = Keys.Shift | Keys.Alt;
      interaction.Actions["Zoom"].Modifier = Keys.None;
    }

    private void tsbZoomOut_Click(object sender, EventArgs e)
    {
      double num = (this.chart.ChartArea.AxisX.Max - this.chart.ChartArea.AxisX.Min) * 0.25;
      Area chartArea = this.chart.ChartArea;
      ChartData chartData = this.chart.ChartGroups.Group0.ChartData;
      if (chartArea.AxisX.Min - num >= chartData.MinX)
        chartArea.AxisX.Min -= num;
      else
        chartArea.AxisX.Min = chartData.MinX;
      if (chartArea.AxisX.Max + num <= chartData.MaxX)
        chartArea.AxisX.Max += num;
      else
        chartArea.AxisX.Max = chartData.MaxX;
    }

    private void chart_MouseClick(object sender, MouseEventArgs e)
    {
      if (!this.tsbZoomIn.Checked)
        return;
      double XDataCoord = 0.0;
      double YDataCoord = 0.0;
      ChartGroup group0 = this.chart.ChartGroups.Group0;
      if (!group0.CoordToDataCoord(e.X, e.Y, ref XDataCoord, ref YDataCoord))
        return;
      Area chartArea = this.chart.ChartArea;
      double num = (chartArea.AxisX.Max - chartArea.AxisX.Min) * 0.5;
      chartArea.AxisX.Min = XDataCoord - num / 2.0 <= group0.ChartData.MinX ? group0.ChartData.MinX : XDataCoord - num / 2.0;
      if (XDataCoord + num / 2.0 < group0.ChartData.MaxX)
        chartArea.AxisX.Max = XDataCoord + num / 2.0;
      else
        chartArea.AxisX.Max = group0.ChartData.MaxX;
    }

    private void ChartViewerFormForVOC_Load(object sender, EventArgs e) => this.fgTable.AutoSizeCols();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ChartViewerFormForVOC));
      this.chart = new C1.Win.C1Chart.C1Chart();
      this.toolStrip1 = new ToolStrip();
      this.tsbPan = new ToolStripButton();
      this.tsbZoomIn = new ToolStripButton();
      this.tsbZoomOut = new ToolStripButton();
      this.tsbShowAll = new ToolStripButton();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.tsbExport = new ToolStripButton();
      this.tabControl1 = new TabControl();
      this.tabPage1 = new TabPage();
      this.tabPage2 = new TabPage();
      this.fgTable = new C1.Win.C1FlexGrid.C1FlexGrid();
      this.chart.BeginInit();
      this.toolStrip1.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.fgTable.BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.chart.BackColor = Color.Transparent;
      componentResourceManager.ApplyResources((object) this.chart, "chart");
      this.chart.Name = "chart";
      this.chart.PropBag = componentResourceManager.GetString("chart.PropBag");
      this.chart.Transform += new TransformEventHandler(this.chart_Transform);
      this.chart.MouseClick += new MouseEventHandler(this.chart_MouseClick);
      this.toolStrip1.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.tsbPan,
        (ToolStripItem) this.tsbZoomIn,
        (ToolStripItem) this.tsbZoomOut,
        (ToolStripItem) this.tsbShowAll,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.tsbExport
      });
      componentResourceManager.ApplyResources((object) this.toolStrip1, "toolStrip1");
      this.toolStrip1.Name = "toolStrip1";
      this.tsbPan.CheckOnClick = true;
      this.tsbPan.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.tsbPan, "tsbPan");
      this.tsbPan.Name = "tsbPan";
      this.tsbPan.CheckedChanged += new EventHandler(this.tsbPan_CheckedChanged);
      this.tsbZoomIn.CheckOnClick = true;
      this.tsbZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.tsbZoomIn, "tsbZoomIn");
      this.tsbZoomIn.Name = "tsbZoomIn";
      this.tsbZoomIn.CheckedChanged += new EventHandler(this.tsbZoomIn_CheckedChanged);
      this.tsbZoomOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.tsbZoomOut, "tsbZoomOut");
      this.tsbZoomOut.Name = "tsbZoomOut";
      this.tsbZoomOut.Click += new EventHandler(this.tsbZoomOut_Click);
      this.tsbShowAll.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.tsbShowAll.Image = (Image) i_Tree_Eco_v6.Properties.Resources.ShowAll;
      componentResourceManager.ApplyResources((object) this.tsbShowAll, "tsbShowAll");
      this.tsbShowAll.Name = "tsbShowAll";
      this.tsbShowAll.Click += new EventHandler(this.tsbShowAll_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator1, "toolStripSeparator1");
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
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.toolStrip1);
      this.Name = nameof (ChartViewerFormForVOC);
      this.Load += new EventHandler(this.ChartViewerFormForVOC_Load);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.toolStrip1, 0);
      this.Controls.SetChildIndex((Control) this.tabControl1, 0);
      this.chart.EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.fgTable.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
