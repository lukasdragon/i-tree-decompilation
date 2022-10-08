// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.SimpleChartViewerForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.Win.C1Chart;
using C1.Win.C1FlexGrid;
using i_Tree_Eco_v6.Properties;
using i_Tree_Eco_v6.Reports.Chart;
using i_Tree_Eco_v6.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace i_Tree_Eco_v6.Forms
{
  public class SimpleChartViewerForm : ContentForm
  {
    protected SimpleDateValueChart _c;
    protected TextAlignEnum dataColFormat = TextAlignEnum.RightCenter;
    private IContainer components;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbPan;
    private ToolStripButton tsbZoomIn;
    private ToolStripButton tsbZoomOut;
    private ToolStripButton tsbShowAll;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton tsbExport;
    private TabPage tabPage2;
    private C1.Win.C1FlexGrid.C1FlexGrid fgTable;
    private TabControl tabControl1;
    private TabPage tabPage1;
    protected C1.Win.C1Chart.C1Chart chart;

    public SimpleChartViewerForm() => this.InitializeComponent();

    public SimpleDateValueChart Chart
    {
      get => this._c;
      set
      {
        this._c = value;
        this.MakeChart();
      }
    }

    public virtual void AddColoring(ChartDataSeriesCollection dsc0, ChartDataSeries cds)
    {
    }

    public virtual void ModifyChartData(DataTable copyDt)
    {
    }

    private DataRow FindDate(DataTable data, DateTime currDay) => data.AsEnumerable().Where<DataRow>((Func<DataRow, bool>) (row => row.Field<DateTime>("DT") == currDay)).FirstOrDefault<DataRow>();

    public List<Tuple<DateTime, DateTime>> HandleMissingDates(
      int year,
      int month,
      int day,
      int hour,
      int min,
      int sec,
      DataTable data)
    {
      DateTime currDay = new DateTime(year, month, day, hour, min, sec);
      DateTime dateTime1 = new DateTime(year, month, day, hour, min, sec);
      DateTime dateTime2 = new DateTime(year + 1, 1, 1, hour, min, sec);
      List<Tuple<DateTime, DateTime>> tupleList = new List<Tuple<DateTime, DateTime>>();
      while (currDay < dateTime2)
      {
        if (this.FindDate(data, currDay) == null)
        {
          DateTime dateTime3 = currDay;
          DateTime dateTime4 = currDay;
          if (currDay < dateTime2)
          {
            do
            {
              dateTime4 = currDay;
              currDay = currDay.AddDays(1.0);
            }
            while (this.FindDate(data, currDay) == null && currDay < dateTime2);
          }
          if (dateTime1.Equals(dateTime3))
            data.Rows.Add((object) dateTime3, (object) 0);
          else
            data.Rows.Add((object) dateTime3.AddDays(-1.0).AddSeconds(1.0), (object) 0);
          if (dateTime4.Equals(dateTime2.AddDays(-1.0)))
            data.Rows.Add((object) dateTime4, (object) 0);
          else
            data.Rows.Add((object) dateTime4.AddDays(1.0).AddSeconds(-1.0), (object) 0);
          tupleList.Add(new Tuple<DateTime, DateTime>(dateTime3, dateTime4));
        }
        else
          currDay = currDay.AddDays(1.0);
      }
      return tupleList;
    }

    public void CreateFooter(string note = "")
    {
      this.chart.Footer.Compass = CompassEnum.South;
      this.chart.Footer.Style.HorizontalAlignment = AlignHorzEnum.General;
      this.chart.Footer.Style.Font = new Font("Tahoma", 10f);
      this.chart.Footer.Visible = true;
      this.chart.Footer.Text = note;
    }

    public string Note(List<Tuple<DateTime, DateTime>> missingDates)
    {
      string str1 = string.Format("{0} ", (object) Strings.MsgAttentionDataIsMissingOnDates);
      foreach (Tuple<DateTime, DateTime> missingDate in missingDates)
      {
        DateTime dateTime = missingDate.Item1;
        if (dateTime.Equals(missingDate.Item2))
        {
          string str2 = str1;
          dateTime = missingDate.Item1;
          string str3 = dateTime.ToString("d", (IFormatProvider) CultureInfo.InvariantCulture);
          str1 = str2 + str3;
        }
        else
        {
          string str4 = str1;
          dateTime = missingDate.Item1;
          string str5 = dateTime.ToString("d", (IFormatProvider) CultureInfo.InvariantCulture);
          dateTime = missingDate.Item2;
          string str6 = dateTime.ToString("d", (IFormatProvider) CultureInfo.InvariantCulture);
          str1 = str4 + str5 + " - " + str6;
        }
        str1 += missingDate.Equals((object) missingDates.Last<Tuple<DateTime, DateTime>>()) ? "." : ", ";
      }
      return str1;
    }

    public void MakeChart()
    {
      DataTable dataTable = this._c.dt.Copy();
      List<Tuple<DateTime, DateTime>> missingDates = this.HandleMissingDates(((DateTime) dataTable.Rows[0][0]).Year, 1, 1, 12, 0, 0, dataTable);
      if (missingDates.Count > 0)
      {
        this.CreateFooter(this.Note(missingDates));
        DataView defaultView = dataTable.DefaultView;
        defaultView.Sort = "DT ASC";
        dataTable = defaultView.ToTable();
      }
      this.ModifyChartData(dataTable);
      this.chart.DataSource = (object) dataTable;
      ChartDataSeriesCollection seriesList = this.chart.ChartGroups[0].ChartData.SeriesList;
      seriesList.Clear();
      ChartDataSeries cds = seriesList.AddNewSeries();
      cds.X.DataField = this._c.dt.Columns[0].ColumnName;
      cds.Y.DataField = this._c.dt.Columns[1].ColumnName;
      cds.SeriesData.LegendEntry = false;
      cds.SymbolStyle.Shape = SymbolShapeEnum.None;
      cds.LineStyle.Color = Color.FromArgb(103, 159, 213);
      this.chart.Font = new Font("Calibri", 14f);
      this.chart.ChartArea.AxisX.AnnoMethod = AnnotationMethodEnum.Values;
      this.chart.ChartArea.AxisX.AnnoFormat = FormatEnum.DateShort;
      this.chart.ChartArea.AxisX.NoAnnoOverlap = true;
      this.chart.Interaction.Enabled = true;
      this.chart.ChartArea.AxisX.GridMajor.Visible = false;
      this.chart.ChartArea.AxisY.GridMajor.Visible = false;
      this.chart.ChartArea.AxisY.AutoOrigin = true;
      this.chart.ChartArea.AxisY.AutoMajor = true;
      this.chart.ChartArea.AxisX.Text = this._c.XAxisTitle;
      this.chart.ChartArea.AxisY.Text = this._c.YAxisTitle;
      this.chart.Header.Text = this._c.MainTitle;
      this.AddColoring(seriesList, cds);
      this.fgTable.Clear();
      this.fgTable.DataSource = (object) new DataView(this._c.dt);
      this.fgTable.Cols[1].Caption = this._c.TableXTitle;
      this.fgTable.Cols[2].Caption = this._c.TableYTitle;
      this.fgTable.Cols[1].Format = "d";
      this.fgTable.Cols[2].Format = "G3";
      this.fgTable.Cols[2].TextAlign = this.dataColFormat;
      this.fgTable.AutoSizeCols();
    }

    protected void chart_Transform(object sender, TransformEventArgs e)
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

    protected void tsbExport_Click(object sender, EventArgs e)
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

    protected void tsbPan_CheckedChanged(object sender, EventArgs e)
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

    protected void tsbShowAll_Click(object sender, EventArgs e)
    {
      this.chart.ChartArea.AxisX.AutoMax = true;
      this.chart.ChartArea.AxisX.AutoMin = true;
    }

    protected void tsbZoomIn_CheckedChanged(object sender, EventArgs e)
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

    protected void tsbZoomOut_Click(object sender, EventArgs e)
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

    protected void chart_MouseClick(object sender, MouseEventArgs e)
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

    protected void ChartViewerForm_Load(object sender, EventArgs e) => this.fgTable.AutoSizeCols();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SimpleChartViewerForm));
      this.toolStrip1 = new ToolStrip();
      this.tsbPan = new ToolStripButton();
      this.tsbZoomIn = new ToolStripButton();
      this.tsbZoomOut = new ToolStripButton();
      this.tsbShowAll = new ToolStripButton();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.tsbExport = new ToolStripButton();
      this.tabPage2 = new TabPage();
      this.fgTable = new C1.Win.C1FlexGrid.C1FlexGrid();
      this.tabControl1 = new TabControl();
      this.tabPage1 = new TabPage();
      this.chart = new C1.Win.C1Chart.C1Chart();
      this.toolStrip1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.fgTable.BeginInit();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.chart.BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
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
      this.tabPage2.Controls.Add((Control) this.fgTable);
      componentResourceManager.ApplyResources((object) this.tabPage2, "tabPage2");
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.fgTable, "fgTable");
      this.fgTable.Name = "fgTable";
      this.fgTable.Rows.DefaultSize = 19;
      this.tabControl1.Controls.Add((Control) this.tabPage1);
      this.tabControl1.Controls.Add((Control) this.tabPage2);
      componentResourceManager.ApplyResources((object) this.tabControl1, "tabControl1");
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabPage1.Controls.Add((Control) this.chart);
      componentResourceManager.ApplyResources((object) this.tabPage1, "tabPage1");
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.chart.BackColor = Color.Transparent;
      componentResourceManager.ApplyResources((object) this.chart, "chart");
      this.chart.Name = "chart";
      this.chart.PropBag = componentResourceManager.GetString("chart.PropBag");
      this.chart.Transform += new TransformEventHandler(this.chart_Transform);
      this.chart.MouseClick += new MouseEventHandler(this.chart_MouseClick);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.toolStrip1);
      this.Name = nameof (SimpleChartViewerForm);
      this.Load += new EventHandler(this.ChartViewerForm_Load);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.toolStrip1, 0);
      this.Controls.SetChildIndex((Control) this.tabControl1, 0);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.fgTable.EndInit();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.chart.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public class UVIndexSettings
    {
      public string Name { get; set; }

      public Color BackColor { get; set; }

      public int UpperExtent { get; set; }

      public int LowerExtent { get; set; }
    }
  }
}
