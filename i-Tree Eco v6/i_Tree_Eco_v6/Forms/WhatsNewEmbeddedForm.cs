// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.WhatsNewEmbeddedForm
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
  public class WhatsNewEmbeddedForm : ContentForm
  {
    private Control _browser;
    private IContainer components;
    private Label lblInventoryType;
    private TableLayoutPanel tableLayoutPanel1;
    private CheckBox chkDontShow;
    private Panel pnlBrowser;

    public WhatsNewEmbeddedForm() => this.InitializeComponent();

    private void WhatsNewForm_Load(object sender, EventArgs e)
    {
      this._browser = (Control) new ChromiumWebBrowser(Strings.UrlWhatsNew)
      {
        BrowserSettings = {
          BackgroundColor = (uint) Color.Ivory.ToArgb()
        }
      };
      this.Browser.RequestHandler = (IRequestHandler) new WhatsNewEmbeddedForm.InternalRequestHandler((Control) this);
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

    private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
    {
      SizeF sizeF = e.Graphics.MeasureString(this.lblInventoryType.Text, this.lblInventoryType.Font, this.lblInventoryType.Width, new StringFormat(StringFormat.GenericTypographic)
      {
        FormatFlags = StringFormatFlags.MeasureTrailingSpaces
      });
      float num = (float) ((double) e.Graphics.ClipBounds.Height / 2.0 - (double) sizeF.Height / 2.0);
      e.Graphics.DrawLine(Pens.LightGray, new PointF(0.0f, (float) ((double) num + (double) sizeF.Height + 3.0)), new PointF(sizeF.Width * 1.3f, (float) ((double) num + (double) sizeF.Height + 3.0)));
    }

    private void chkDontShow_CheckedChanged(object sender, EventArgs e) => Settings.Default.ShowWhatsNewOnLaunch = this.chkDontShow.Checked;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblInventoryType = new Label();
      this.chkDontShow = new CheckBox();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.pnlBrowser = new Panel();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.lblInventoryType.BackColor = Color.Ivory;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblInventoryType, 2);
      this.lblInventoryType.Font = new Font("Calibri", 14f, FontStyle.Bold);
      this.lblInventoryType.ForeColor = Color.FromArgb(0, 112, 192);
      this.lblInventoryType.ImeMode = ImeMode.NoControl;
      this.lblInventoryType.Location = new Point(3, 0);
      this.lblInventoryType.Name = "lblInventoryType";
      this.lblInventoryType.Padding = new Padding(12, 5, 0, 0);
      this.lblInventoryType.Size = new Size(480, 40);
      this.lblInventoryType.TabIndex = 12;
      this.lblInventoryType.Text = "What's new in i-Tree Eco";
      this.lblInventoryType.TextAlign = ContentAlignment.MiddleLeft;
      this.chkDontShow.AutoSize = true;
      this.chkDontShow.Font = new Font("Calibri", 10f);
      this.chkDontShow.ImeMode = ImeMode.NoControl;
      this.chkDontShow.Location = new Point(3, 208);
      this.chkDontShow.Name = "chkDontShow";
      this.chkDontShow.Size = new Size(290, 21);
      this.chkDontShow.TabIndex = 0;
      this.chkDontShow.Text = "Show this dialog when the application launches";
      this.chkDontShow.UseVisualStyleBackColor = true;
      this.chkDontShow.CheckedChanged += new EventHandler(this.chkDontShow_CheckedChanged);
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
      this.tableLayoutPanel1.Controls.Add((Control) this.lblInventoryType, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.chkDontShow, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.pnlBrowser, 0, 1);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 29);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel1.Size = new Size(486, 232);
      this.tableLayoutPanel1.TabIndex = 14;
      this.tableLayoutPanel1.Paint += new PaintEventHandler(this.tableLayoutPanel1_Paint);
      this.pnlBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlBrowser.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.pnlBrowser, 2);
      this.pnlBrowser.Location = new Point(20, 43);
      this.pnlBrowser.Margin = new Padding(20, 3, 6, 3);
      this.pnlBrowser.Name = "pnlBrowser";
      this.pnlBrowser.Size = new Size(460, 159);
      this.pnlBrowser.TabIndex = 13;
      this.AutoScaleDimensions = new SizeF(96f, 96f);
      this.AutoScaleMode = AutoScaleMode.Dpi;
      this.BackColor = Color.Ivory;
      this.ClientSize = new Size(486, 261);
      this.ControlBox = false;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (WhatsNewEmbeddedForm);
      this.ShowInTaskbar = false;
      this.Load += new EventHandler(this.WhatsNewForm_Load);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel1, 0);
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
