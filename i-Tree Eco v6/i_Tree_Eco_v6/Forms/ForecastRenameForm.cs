// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ForecastRenameForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Forms.Resources;
using NHibernate;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ForecastRenameForm : Form
  {
    private string _oldName;
    private InputSession _iSess;
    private IContainer components;
    public TextBox NameTextBox;
    private Button OKButton;
    private Button CancelBtn;

    public ForecastRenameForm()
    {
      this.InitializeComponent();
      this._iSess = Program.Session.InputSession;
      using (ISession session = this._iSess.CreateSession())
      {
        this._oldName = session.QueryOver<Forecast>().Where((Expression<Func<Forecast, bool>>) (fc => (Guid?) fc.Guid == this._iSess.ForecastKey)).Select((Expression<Func<Forecast, object>>) (fc => fc.Title)).SingleOrDefault<string>();
        this.NameTextBox.Text = this._oldName;
        this.NameTextBox.Select();
      }
    }

    private void NameTextBox_TextChanged(object sender, EventArgs e) => this.OKButton.Enabled = this.NameTextBox.Text != this._oldName;

    private void ForecastRenameForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.OKButton.Enabled)
        return;
      using (ISession session = Program.Session.InputSession.CreateSession())
      {
        if (session.QueryOver<Forecast>().Where((Expression<Func<Forecast, bool>>) (fc => fc.Title == this.NameTextBox.Text)).Inner.JoinQueryOver<Year>((Expression<Func<Forecast, Year>>) (fc => fc.Year)).Where((Expression<Func<Year, bool>>) (y => (Guid?) y.Guid == this._iSess.YearKey)).SingleOrDefault() == null)
          return;
        int num = (int) MessageBox.Show(string.Format(ForecastRes.RenameForecastError, (object) this.NameTextBox.Text), "", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        this.NameTextBox.Text = "";
        e.Cancel = true;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ForecastRenameForm));
      this.NameTextBox = new TextBox();
      this.OKButton = new Button();
      this.CancelBtn = new Button();
      Label label = new Label();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) label, "MessageLabel");
      label.Name = "MessageLabel";
      componentResourceManager.ApplyResources((object) this.NameTextBox, "NameTextBox");
      this.NameTextBox.Name = "NameTextBox";
      this.NameTextBox.TextChanged += new EventHandler(this.NameTextBox_TextChanged);
      this.OKButton.DialogResult = DialogResult.OK;
      componentResourceManager.ApplyResources((object) this.OKButton, "OKButton");
      this.OKButton.Name = "OKButton";
      this.OKButton.UseVisualStyleBackColor = true;
      this.CancelBtn.DialogResult = DialogResult.Cancel;
      componentResourceManager.ApplyResources((object) this.CancelBtn, "CancelBtn");
      this.CancelBtn.Name = "CancelBtn";
      this.CancelBtn.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.OKButton;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.CancelBtn;
      this.Controls.Add((Control) this.CancelBtn);
      this.Controls.Add((Control) this.OKButton);
      this.Controls.Add((Control) label);
      this.Controls.Add((Control) this.NameTextBox);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ForecastRenameForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.FormClosing += new FormClosingEventHandler(this.ForecastRenameForm_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
