// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.MapForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using CefSharp;
using CefSharp.WinForms;
using i_Tree_Eco_v6.Reports;
using i_Tree_Eco_v6.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class MapForm : Form
  {
    private ReportViewerForm m_rvf;
    private string featuresScript;
    private Control _browser;
    private List<ColumnFormat> columnsFormat;
    private DataTable data;
    private IContainer components;
    private TableLayoutPanel tableLayoutPanel1;
    private Label lblMapData;

    private void Init()
    {
      this._browser = (Control) new ChromiumWebBrowser("https://dots.daveyinstitute.com/?embedded=true");
      this._browser.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Controls.Add(this._browser, 0, 1);
      this.Browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(this.Browser_FrameLoadEnd);
      this.lblMapData.Text = Strings.MapDataNote;
    }

    public MapForm(ReportViewerForm rvf)
    {
      this.InitializeComponent();
      this.Init();
      this.m_rvf = rvf;
      this.SetFormTitle(rvf.Report.ReportTitle);
      Dictionary<string, object> mapData = rvf.Report.GenerateMapData();
      this.data = mapData[nameof (data)] as DataTable;
      this.columnsFormat = mapData["data_format"] as List<ColumnFormat>;
      this.SetFeaturesScript();
    }

    public MapForm(short pollutionYear, int location)
    {
      this.InitializeComponent();
      this.Init();
      PollutionStations pollutionStations = new PollutionStations();
      this.data = pollutionStations.GetData(pollutionYear, location);
      this.columnsFormat = pollutionStations.ColumnsFormat(this.data);
      this.SetFormTitle("Selected Pollution Stations");
      this.SetFeaturesScript();
    }

    private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
    {
      if (!e.Frame.IsMain)
        return;
      this.Browser.EvaluateScriptAsync(this.featuresScript, new TimeSpan?(), false).ContinueWith((Action<Task<JavascriptResponse>>) (t =>
      {
        if (this.m_rvf == null)
          return;
        this.Invoke((Delegate) (() => this.m_rvf.AppReady()));
      }));
    }

    private IWebBrowser Browser => this._browser as IWebBrowser;

    private void SetFormTitle(string callingReportTitle) => this.Text = string.Format(Strings.FormTitle_Map, (object) callingReportTitle);

    private void SetFeaturesScript()
    {
      if (this.data == null || this.data.Rows.Count <= 0)
        throw new Exception(Strings.MsgNoData);
      string empty = string.Empty;
      StringBuilder pointsJson = this.GetPointsJson();
      if (pointsJson.Length <= 0)
        throw new Exception(Strings.MapCoordinatesErrorMessage);
      this.featuresScript = string.Format("updateLayer({{\"type\":\"FeatureCollection\",\"crs\":{{\"type\":\"name\",\"properties\":{{\"name\":\"EPSG:4326\"}}}},\"features\":[{0}]}});", (object) pointsJson.ToString().TrimEnd(',', ' ').Replace(Environment.NewLine, string.Empty));
    }

    private StringBuilder GetPointsJson()
    {
      StringBuilder pointsJson = new StringBuilder();
      string format1 = "{{\"type\":\"Feature\",\"geometry\":{{\"type\":\"Point\",\"coordinates\":[{0}, {1}]}},\"properties\":{{\"name\":\"{2}\", {3} }} }}, ";
      string format2 = "\"{0}\":\"{1}\", ";
      foreach (DataRow row in (InternalDataCollectionBase) this.data.Rows)
      {
        object x = row["xCoordinate"];
        object y = row["yCoordinate"];
        if (ValidationHelper.ValidateCoordinates(x, y))
        {
          StringBuilder stringBuilder = new StringBuilder();
          foreach (ColumnFormat columnFormat in this.columnsFormat)
            stringBuilder.AppendFormat(format2, (object) columnFormat.HeaderText, (object) columnFormat.Format(row[columnFormat.ColName]));
          object obj = this.data.Columns.Contains("TreeID") ? row["TreeID"] : row["PlotID"];
          Tuple<string, string> strings = this.ConvertCoordinatesToStrings(x, y);
          pointsJson.AppendFormat(format1, (object) strings.Item1, (object) strings.Item2, obj, (object) stringBuilder.ToString().TrimEnd(',', ' '));
        }
      }
      return pointsJson;
    }

    public bool ValidateCoordinates(double lng, double lat) => lat >= -90.0 && lat <= 90.0 && lng >= -180.0 && lng <= 180.0;

    private Tuple<string, string> ConvertCoordinatesToStrings(object x, object y)
    {
      double num = (double) x;
      string str1 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      num = (double) y;
      string str2 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      return new Tuple<string, string>(str1, str2);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.lblMapData = new Label();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Controls.Add((Control) this.lblMapData, 0, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Size = new Size(904, 566);
      this.tableLayoutPanel1.TabIndex = 2;
      this.lblMapData.Font = new Font("Calibri Light", 9f, FontStyle.Italic);
      this.lblMapData.Location = new Point(3, 0);
      this.lblMapData.Name = "lblMapData";
      this.lblMapData.Size = new Size(875, 20);
      this.lblMapData.TabIndex = 2;
      this.lblMapData.Text = "lblMapData";
      this.lblMapData.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(904, 566);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Name = nameof (MapForm);
      this.ShowIcon = false;
      this.Text = "Map";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
