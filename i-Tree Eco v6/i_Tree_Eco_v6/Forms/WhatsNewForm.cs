// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.WhatsNewForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using CefSharp;
using CefSharp.Handler;
using CefSharp.WinForms;
using i_Tree_Eco_v6.Properties;
using i_Tree_Eco_v6.Resources;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class WhatsNewForm : Form
  {
    private Control _browser;
    private IContainer components;
    private CheckBox chkDontShow;
    private Button cmdOK;
    private Label lblInventoryType;
    private TableLayoutPanel tableLayoutPanel1;
    private Panel pnlBrowser;

    public WhatsNewForm() => this.InitializeComponent();

    private void WhatsNewForm_Load(object sender, EventArgs e)
    {
      this._browser = (Control) new ChromiumWebBrowser(Strings.UrlWhatsNew)
      {
        BrowserSettings = {
          BackgroundColor = (uint) Color.Ivory.ToArgb()
        }
      };
      this.Browser.RequestHandler = (IRequestHandler) new WhatsNewForm.InternalRequestHandler((Control) this);
      this.Browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(this.Browser_FrameLoadEnd);
      this.pnlBrowser.Controls.Add(this._browser);
      this.lblInventoryType.Text = string.Format("{0} v{1}", (object) this.lblInventoryType.Text, (object) new Version(Application.ProductVersion).ToString(3));
      this.chkDontShow.Checked = Settings.Default.ShowWhatsNewOnLaunch;
    }

    protected IWebBrowser Browser => this._browser as IWebBrowser;

    private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
    {
      if (!e.Frame.IsMain)
        return;
      this.Browser.ExecuteScriptAsync("\r\n                (function(){\r\n                    document.body.style.backgroundColor = 'transparent';\r\n                })();\r\n            ");
    }

    private void cmdOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void WhatsNewForm_FormClosing(object sender, FormClosingEventArgs e) => Settings.Default.ShowWhatsNewOnLaunch = this.chkDontShow.Checked;

    private void lblInventoryType_Paint(object sender, PaintEventArgs e)
    {
      SizeF sizeF = e.Graphics.MeasureString(this.lblInventoryType.Text, this.lblInventoryType.Font, this.lblInventoryType.Width, new StringFormat(StringFormat.GenericTypographic)
      {
        FormatFlags = StringFormatFlags.MeasureTrailingSpaces
      });
      float num = (float) ((double) e.Graphics.ClipBounds.Height / 2.0 - (double) sizeF.Height / 2.0);
      e.Graphics.DrawLine(Pens.LightGray, new PointF(0.0f, (float) ((double) num + (double) sizeF.Height + 3.0)), new PointF(sizeF.Width * 1.3f, (float) ((double) num + (double) sizeF.Height + 3.0)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (WhatsNewForm));
      this.chkDontShow = new CheckBox();
      this.cmdOK = new Button();
      this.lblInventoryType = new Label();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.pnlBrowser = new Panel();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.chkDontShow, "chkDontShow");
      this.chkDontShow.Name = "chkDontShow";
      this.chkDontShow.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.cmdOK, "cmdOK");
      this.cmdOK.Name = "cmdOK";
      this.cmdOK.UseVisualStyleBackColor = true;
      this.cmdOK.Click += new EventHandler(this.cmdOK_Click);
      this.lblInventoryType.BackColor = Color.Ivory;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblInventoryType, 2);
      componentResourceManager.ApplyResources((object) this.lblInventoryType, "lblInventoryType");
      this.lblInventoryType.ForeColor = Color.FromArgb(0, 112, 192);
      this.lblInventoryType.Name = "lblInventoryType";
      this.lblInventoryType.Paint += new PaintEventHandler(this.lblInventoryType_Paint);
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.lblInventoryType, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.cmdOK, 1, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.chkDontShow, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.pnlBrowser, 0, 1);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.pnlBrowser, "pnlBrowser");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.pnlBrowser, 2);
      this.pnlBrowser.Name = "pnlBrowser";
      this.AcceptButton = (IButtonControl) this.cmdOK;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Dpi;
      this.BackColor = Color.Ivory;
      this.ControlBox = false;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (WhatsNewForm);
      this.ShowInTaskbar = false;
      this.FormClosing += new FormClosingEventHandler(this.WhatsNewForm_FormClosing);
      this.Load += new EventHandler(this.WhatsNewForm_Load);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private class InternalRequestHandler : RequestHandler
    {
      private Control _owner;

      public InternalRequestHandler(Control owner) => this._owner = owner;

      protected override bool OnBeforeBrowse(
        IWebBrowser chromiumWebBrowser,
        IBrowser browser,
        IFrame frame,
        IRequest request,
        bool userGesture,
        bool isRedirect)
      {
        if (!(request.Url != Strings.UrlWhatsNew))
          return base.OnBeforeBrowse(chromiumWebBrowser, browser, frame, request, userGesture, isRedirect);
        try
        {
          Process.Start(request.Url);
        }
        catch (Win32Exception ex)
        {
          int num = (int) MessageBox.Show((IWin32Window) this._owner, string.Format(Strings.ErrOpenRTFLink, (object) ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return true;
      }
    }
  }
}
