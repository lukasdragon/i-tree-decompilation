// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.WeatherStationSelectorForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using CefSharp;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class WeatherStationSelectorForm : CustomWebBrowserForm
  {
    private IContainer components;
    private Panel panel1;
    private TableLayoutPanel tableLayoutPanel1;
    private Button btnOK;
    private Button btnCancel;

    public WeatherStationSelectorForm()
    {
      this.InitializeComponent();
      this.tableLayoutPanel1.SendToBack();
      this.StationId = string.Empty;
    }

    private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
    {
      if (!e.Frame.IsMain)
        return;
      this.Invoke((Delegate) (() =>
      {
        this.btnOK.Enabled = true;
        this.btnCancel.Visible = !this.Offline;
      }));
    }

    public string StationId { get; set; }

    public string Year { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    protected override string Url => string.Format("http://www.itreetools.org/eco/weather/?latitude={0}&longitude={1}&year={2}&station={3}&embed={4}", (object) HttpUtility.UrlEncode(this.Latitude.ToString((IFormatProvider) CultureInfo.InvariantCulture)), (object) HttpUtility.UrlEncode(this.Longitude.ToString((IFormatProvider) CultureInfo.InvariantCulture)), (object) HttpUtility.UrlEncode(this.Year.ToString((IFormatProvider) CultureInfo.InvariantCulture)), (object) HttpUtility.UrlEncode(this.StationId), !this.Offline ? (object) "true" : (object) "false");

    private void btnOK_Click(object sender, EventArgs e) => this.Browser.EvaluateScriptAsync("\r\n                (function() {\r\n                    var el = document.getElementById('station');\r\n\r\n                    if (el) {\r\n                        return el.innerText;\r\n                    }\r\n                \r\n                    return '';\r\n                })();\r\n            ", new TimeSpan?(), false).ContinueWith((Action<Task<JavascriptResponse>>) (t =>
    {
      if (t.IsFaulted)
        return;
      this.StationId = (string) t.Result.Result;
      this.DialogResult = DialogResult.OK;
    }), TaskScheduler.FromCurrentSynchronizationContext());

    private void WeatherStationSelectorForm_Load(object sender, EventArgs e)
    {
      if (!NetworkInterface.GetIsNetworkAvailable())
        return;
      this.Browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(this.Browser_FrameLoadEnd);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.lblBreadcrumb.Size = new Size(904, 19);
      this.lblBreadcrumb.Visible = false;
      this.panel1.AutoSize = true;
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 19);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(904, 0);
      this.panel1.TabIndex = 6;
      this.tableLayoutPanel1.AutoSize = true;
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
      this.tableLayoutPanel1.Controls.Add((Control) this.btnOK, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.btnCancel, 1, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Bottom;
      this.tableLayoutPanel1.Location = new Point(0, 590);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel1.Size = new Size(904, 31);
      this.tableLayoutPanel1.TabIndex = 8;
      this.btnOK.Anchor = AnchorStyles.Right;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(745, 3);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 25);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(826, 3);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 25);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(904, 621);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.MinimumSize = new Size(400, 300);
      this.Name = nameof (WeatherStationSelectorForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Weather Station Selector";
      this.Load += new EventHandler(this.WeatherStationSelectorForm_Load);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel1, 0);
      this.Controls.SetChildIndex((Control) this.panel1, 0);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
