// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.LoggedErrorForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using i_Tree_Eco_v6.Properties;
using i_Tree_Eco_v6.Resources;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class LoggedErrorForm : Form
  {
    public bool continueClicked;
    private bool errorFromAFile = true;
    private string sourceLoggedFile;
    private bool _saved;
    private IContainer components;
    private Label lblMessage;
    private Button btnSave;
    private Button btnClose;
    private RichTextLabel richTextLabel1;
    private TableLayoutPanel tableLayoutPanel1;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnContinue;

    public LoggedErrorForm()
    {
      this.InitializeComponent();
      this.btnContinue.Text = LoggedErrorForm.MBGetString(LoggedErrorForm.DialogBtn.CONTINUE);
    }

    private void LoggedErrorForm_Load(object sender, EventArgs e) => this.lblMessage.Text = Strings.ErrorCheckData;

    public void InitForContinue(string msg)
    {
      this.lblMessage.Visible = false;
      this.btnSave.Visible = false;
      this.btnClose.Visible = false;
      this.btnContinue.Visible = true;
      this.AcceptButton = (IButtonControl) this.btnContinue;
      this._saved = true;
      this.initializeForm(msg, false);
    }

    public void initializeForm(string fromLoggedFileOrErrorMessage, bool fromErrorFile)
    {
      if (fromErrorFile)
      {
        this.errorFromAFile = true;
        this.sourceLoggedFile = fromLoggedFileOrErrorMessage;
        this.richTextLabel1.Text = File.ReadAllText(this.sourceLoggedFile);
      }
      else
      {
        this.errorFromAFile = false;
        this.sourceLoggedFile = "";
        this.richTextLabel1.Text = fromLoggedFileOrErrorMessage;
      }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void LoggedErrorForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.Visible || this._saved)
        return;
      DialogResult dialogResult = MessageBox.Show((IWin32Window) this, Strings.MsgConfirmCloseWithoutSaving, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      e.Cancel = dialogResult == DialogResult.No;
    }

    private void LoggedErrorForm_Resize(object sender, EventArgs e) => this.lblMessage.MaximumSize = new Size(this.ClientSize.Width, 0);

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.btnSave.Text.ToLower() == "continue")
      {
        this.continueClicked = true;
        this.Hide();
      }
      else
      {
        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        {
          saveFileDialog.ShowHelp = false;
          saveFileDialog.CreatePrompt = false;
          saveFileDialog.OverwritePrompt = true;
          saveFileDialog.Filter = string.Join("|", new string[2]
          {
            string.Format(Strings.FmtFilter, (object) Strings.FilterText, (object) string.Join(";", Settings.Default.ExtText)),
            string.Format(Strings.FmtFilter, (object) Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
          });
          if (saveFileDialog.ShowDialog() != DialogResult.OK)
            return;
          if (this.errorFromAFile)
            File.Copy(this.sourceLoggedFile, saveFileDialog.FileName, true);
          else
            File.WriteAllText(saveFileDialog.FileName, this.richTextLabel1.Text);
          this._saved = true;
        }
      }
    }

    private void btnContinue_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams createParams = base.CreateParams;
        createParams.ClassStyle |= 512;
        return createParams;
      }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr MB_GetString(LoggedErrorForm.DialogBtn btn);

    private static string MBGetString(LoggedErrorForm.DialogBtn btn) => Marshal.PtrToStringAuto(LoggedErrorForm.MB_GetString(btn));

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoggedErrorForm));
      this.lblMessage = new Label();
      this.btnSave = new Button();
      this.btnClose = new Button();
      this.richTextLabel1 = new RichTextLabel();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnContinue = new Button();
      this.tableLayoutPanel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblMessage, "lblMessage");
      this.lblMessage.Name = "lblMessage";
      componentResourceManager.ApplyResources((object) this.btnSave, "btnSave");
      this.btnSave.Name = "btnSave";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      componentResourceManager.ApplyResources((object) this.btnClose, "btnClose");
      this.btnClose.Name = "btnClose";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      componentResourceManager.ApplyResources((object) this.richTextLabel1, "richTextLabel1");
      this.richTextLabel1.Name = "richTextLabel1";
      this.richTextLabel1.TabStop = false;
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.lblMessage, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.richTextLabel1, 0, 1);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.flowLayoutPanel1.Controls.Add((Control) this.btnClose);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSave);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnContinue);
      componentResourceManager.ApplyResources((object) this.flowLayoutPanel1, "flowLayoutPanel1");
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.btnContinue, "btnContinue");
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.UseVisualStyleBackColor = true;
      this.btnContinue.Click += new EventHandler(this.btnContinue_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Controls.Add((Control) this.flowLayoutPanel1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoggedErrorForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.FormClosing += new FormClosingEventHandler(this.LoggedErrorForm_FormClosing);
      this.Load += new EventHandler(this.LoggedErrorForm_Load);
      this.Resize += new EventHandler(this.LoggedErrorForm_Resize);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    internal enum DialogBtn : uint
    {
      OK,
      CANCEL,
      ABORT,
      RETRY,
      IGNORE,
      YES,
      NO,
      CLOSE,
      HELP,
      TRYAGAIN,
      CONTINUE,
    }
  }
}
