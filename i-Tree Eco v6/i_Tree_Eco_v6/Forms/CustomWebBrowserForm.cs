// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.CustomWebBrowserForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using CefSharp;
using CefSharp.WinForms;
using DaveyTree.Drawing.Extensions;
using HtmlAgilityPack;
using i_Tree_Eco_v6.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class CustomWebBrowserForm : ContentForm
  {
    private Control _browser;
    private bool _offline;
    private IContainer components;
    private ToolStrip toolStrip1;
    private ToolStripButton btnBack;
    private ToolStripButton btnForward;
    private ToolStripButton btnRefresh;
    private ToolStripButton btnStop;
    private ToolStripContainer toolStripContainer1;

    public CustomWebBrowserForm()
    {
      this.InitializeComponent();
      if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
        return;
      this.Load += new EventHandler(this.CustomWebBrowserForm_Load);
    }

    protected bool Offline => this._offline;

    protected IWebBrowser Browser => this._browser as IWebBrowser;

    private void Browser_LoadError(object sender, LoadErrorEventArgs e)
    {
      if (!e.Frame.IsMain || e.ErrorCode == CefErrorCode.Aborted)
        return;
      if (this._offline)
        Process.Start(new Uri(e.FailedUrl).AbsoluteUri);
      else
        this.OfflinePage();
      this._offline = true;
    }

    private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e) => this.Invoke((Delegate) (() =>
    {
      this.btnBack.Enabled = e.CanGoBack;
      this.btnForward.Enabled = e.CanGoForward;
      this.btnRefresh.Enabled = e.CanReload;
      this.btnStop.Enabled = e.IsLoading;
      this.btnRefresh.Visible = !e.IsLoading;
      this.btnStop.Visible = e.IsLoading;
    }));

    protected virtual string Url => string.Empty;

    protected virtual Image ScreenShot => (Image) null;

    protected virtual void OfflinePage()
    {
      HtmlAgilityPack.HtmlDocument htmlDocument1 = this.CreateBase();
      HtmlAgilityPack.HtmlDocument htmlDocument2 = new HtmlAgilityPack.HtmlDocument();
      htmlDocument2.LoadHtml(Resources.SupportTemplate);
      HtmlNode htmlNode = htmlDocument2.DocumentNode.SelectSingleNode("//h2[@id='not-launched']/a");
      htmlNode.Attributes.Add("href", this.Url);
      htmlNode.InnerHtml = this.Url;
      htmlDocument1.GetElementbyId("content").InnerHtml = htmlDocument2.GetElementbyId("content").InnerHtml;
      htmlDocument1.GetElementbyId("footer-header").InnerHtml = htmlDocument2.GetElementbyId("footer-header").InnerHtml;
      htmlDocument1.GetElementbyId("additional-help").InnerHtml = htmlDocument2.GetElementbyId("additional-help").InnerHtml;
      this.Browser.LoadHtml(htmlDocument1.DocumentNode.OuterHtml, this.Url);
    }

    protected HtmlAgilityPack.HtmlDocument CreateBase()
    {
      HtmlAgilityPack.HtmlDocument htmlDocument1 = new HtmlAgilityPack.HtmlDocument();
      htmlDocument1.LoadHtml(Resources.TemplatesMain);
      HtmlAgilityPack.HtmlDocument htmlDocument2 = new HtmlAgilityPack.HtmlDocument();
      htmlDocument2.LoadHtml(Resources.Base);
      htmlDocument2.DocumentNode.SelectSingleNode("//style").InnerHtml = htmlDocument1.DocumentNode.OuterHtml;
      htmlDocument2.GetElementbyId("warning-icon").Attributes.Add("src", Resources.Warning_Large.ToUriData());
      htmlDocument2.GetElementbyId("fs-logo").Attributes.Add("src", Resources.forest_service.ToUriData());
      htmlDocument2.GetElementbyId("davey-logo").Attributes.Add("src", Resources.davey_logo.ToUriData());
      htmlDocument2.GetElementbyId("ad-logo").Attributes.Add("src", Resources.arbor_day_foundation.ToUriData());
      htmlDocument2.GetElementbyId("sma-logo").Attributes.Add("src", Resources.sma_logo.ToUriData());
      htmlDocument2.GetElementbyId("isa-logo").Attributes.Add("src", Resources.isa_logo.ToUriData());
      htmlDocument2.GetElementbyId("casey-logo").Attributes.Add("src", Resources.casey_trees.ToUriData());
      htmlDocument2.GetElementbyId("esf-logo").Attributes.Add("src", Resources.esf.ToUriData());
      return htmlDocument2;
    }

    private void CustomWebBrowserForm_Load(object sender, EventArgs e)
    {
      this._browser = (Control) new ChromiumWebBrowser(this.Url);
      this._browser.Dock = DockStyle.Fill;
      IWebBrowser browser = this.Browser;
      browser.LoadingStateChanged += new EventHandler<LoadingStateChangedEventArgs>(this.Browser_LoadingStateChanged);
      browser.LoadError += new EventHandler<LoadErrorEventArgs>(this.Browser_LoadError);
      browser.DownloadHandler = (IDownloadHandler) new DownloadHandler();
      browser.LifeSpanHandler = (ILifeSpanHandler) new LifeSpanHandler();
      this.toolStripContainer1.ContentPanel.Controls.Add(this._browser);
    }

    private void btnBack_Click(object sender, EventArgs e)
    {
      if (!this.Browser.CanGoBack)
        return;
      this.Browser.Back();
    }

    private void btnForward_Click(object sender, EventArgs e)
    {
      if (!this.Browser.CanGoForward)
        return;
      this.Browser.Forward();
    }

    private void btnStop_Click(object sender, EventArgs e) => this.Browser.Stop();

    private void btnRefresh_Click(object sender, EventArgs e)
    {
      if (this.Offline)
        this._offline = false;
      this.Browser.Reload();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.toolStrip1 = new ToolStrip();
      this.btnBack = new ToolStripButton();
      this.btnForward = new ToolStripButton();
      this.btnRefresh = new ToolStripButton();
      this.btnStop = new ToolStripButton();
      this.toolStripContainer1 = new ToolStripContainer();
      this.toolStrip1.SuspendLayout();
      this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
      this.toolStripContainer1.SuspendLayout();
      this.SuspendLayout();
      this.lblBreadcrumb.Size = new Size(284, 19);
      this.toolStrip1.Dock = DockStyle.None;
      this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
      this.toolStrip1.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.btnBack,
        (ToolStripItem) this.btnForward,
        (ToolStripItem) this.btnRefresh,
        (ToolStripItem) this.btnStop
      });
      this.toolStrip1.Location = new Point(3, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new Size(126, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      this.btnBack.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.btnBack.Enabled = false;
      this.btnBack.Image = (Image) Resources.arrow_left;
      this.btnBack.ImageTransparentColor = Color.Magenta;
      this.btnBack.Name = "btnBack";
      this.btnBack.Size = new Size(23, 22);
      this.btnBack.Text = "&Back";
      this.btnBack.Click += new EventHandler(this.btnBack_Click);
      this.btnForward.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.btnForward.Enabled = false;
      this.btnForward.Image = (Image) Resources.arrow_right;
      this.btnForward.ImageTransparentColor = Color.Magenta;
      this.btnForward.Name = "btnForward";
      this.btnForward.Size = new Size(23, 22);
      this.btnForward.Text = "&Forward";
      this.btnForward.Click += new EventHandler(this.btnForward_Click);
      this.btnRefresh.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.btnRefresh.Image = (Image) Resources.redo_alt;
      this.btnRefresh.ImageTransparentColor = Color.Magenta;
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new Size(23, 22);
      this.btnRefresh.Text = "&Refresh";
      this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
      this.btnStop.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.btnStop.Image = (Image) Resources.stop;
      this.btnStop.ImageTransparentColor = Color.Magenta;
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new Size(23, 22);
      this.btnStop.Text = "Stop";
      this.btnStop.Visible = false;
      this.btnStop.Click += new EventHandler(this.btnStop_Click);
      this.toolStripContainer1.ContentPanel.Size = new Size(284, 218);
      this.toolStripContainer1.Dock = DockStyle.Fill;
      this.toolStripContainer1.Location = new Point(0, 19);
      this.toolStripContainer1.Name = "toolStripContainer1";
      this.toolStripContainer1.Size = new Size(284, 243);
      this.toolStripContainer1.TabIndex = 2;
      this.toolStripContainer1.Text = "toolStripContainer1";
      this.toolStripContainer1.TopToolStripPanel.Controls.Add((Control) this.toolStrip1);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(284, 262);
      this.Controls.Add((Control) this.toolStripContainer1);
      this.Name = nameof (CustomWebBrowserForm);
      this.Text = nameof (CustomWebBrowserForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.toolStripContainer1, 0);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
      this.toolStripContainer1.TopToolStripPanel.PerformLayout();
      this.toolStripContainer1.ResumeLayout(false);
      this.toolStripContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
