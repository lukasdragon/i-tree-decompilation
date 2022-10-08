// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.EulaForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using CefSharp;
using i_Tree_Eco_v6.Resources;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class EulaForm : CustomWebBrowserForm
  {
    private IContainer components;
    private TableLayoutPanel tableLayoutPanel1;
    private Button btnOK;
    private Button btnCancel;

    public EulaForm() => this.InitializeComponent();

    protected override string Url => Strings.UrlEula;

    private void EulaForm_Load(object sender, EventArgs e)
    {
      if (this.Modal)
      {
        this.lblBreadcrumb.Visible = false;
        this.Browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(this.Browser_FrameLoadEnd);
      }
      else
      {
        if (!NetworkInterface.GetIsNetworkAvailable())
          return;
        this.tableLayoutPanel1.Visible = false;
      }
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.lblBreadcrumb.Margin = new Padding(9, 10, 9, 10);
      this.lblBreadcrumb.Size = new Size(815, 26);
      this.tableLayoutPanel1.AutoSize = true;
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
      this.tableLayoutPanel1.Controls.Add((Control) this.btnOK, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.btnCancel, 1, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Bottom;
      this.tableLayoutPanel1.Location = new Point(0, 518);
      this.tableLayoutPanel1.Margin = new Padding(4);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 31f));
      this.tableLayoutPanel1.Size = new Size(815, 33);
      this.tableLayoutPanel1.TabIndex = 9;
      this.btnOK.Anchor = AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Enabled = false;
      this.btnOK.Font = new Font("Calibri", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnOK.Location = new Point(653, 4);
      this.btnOK.Margin = new Padding(4);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 25);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnCancel.Anchor = AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Font = new Font("Calibri", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnCancel.Location = new Point(736, 4);
      this.btnCancel.Margin = new Padding(4);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 25);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(8f, 18f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.BackColor = Color.White;
      this.ClientSize = new Size(815, 551);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Margin = new Padding(5, 6, 5, 6);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EulaForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "i-Tree End User's License Agreement";
      this.Load += new EventHandler(this.EulaForm_Load);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel1, 0);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
