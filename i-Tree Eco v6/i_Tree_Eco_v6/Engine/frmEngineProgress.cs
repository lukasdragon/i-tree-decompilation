// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Engine.frmEngineProgress
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using UFORE;

namespace i_Tree_Eco_v6.Engine
{
  public class frmEngineProgress : Form
  {
    public CancellationTokenSource engineCancelTokenSource;
    private bool _isCancelled;
    private bool _isDone;
    private IContainer components;
    private ProgressBar progressBar1;
    private Button btnCancel;
    private Label lblDescription;
    private ProgressBar progressBar2;
    private Label lblPercent;

    public frmEngineProgress() => this.InitializeComponent();

    private void frmProgress_Load(object sender, EventArgs e)
    {
      this.Text = EngineModelRes.EngineProgressForm_Title;
      this.btnCancel.Text = EngineModelRes.EngineProgressForm_btnCancel_CancelText;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (!this._isCancelled)
      {
        this._isCancelled = true;
        this.btnCancel.Text = EngineModelRes.EngineProgressForm_CancellingWord;
        this.engineCancelTokenSource.Cancel();
      }
      if (!this._isDone)
        return;
      this.Close();
    }

    public void DisplayProgress(EngineProgressArg anArg)
    {
      this.DisplayProgressWithAnnimation(anArg);
      Application.DoEvents();
      if (this.progressBar1.Value > 1)
      {
        --this.progressBar1.Value;
        ++this.progressBar1.Value;
        Application.DoEvents();
      }
      if (this.progressBar2.Value <= 1)
        return;
      --this.progressBar2.Value;
      ++this.progressBar2.Value;
      Application.DoEvents();
    }

    public void DisplayProgressWithAnnimation(EngineProgressArg anArg)
    {
      this.lblDescription.Text = "Steps: ( " + anArg.CurrentStep.ToString() + " / " + anArg.TotalSteps.ToString() + " )";
      this.progressBar1.Value = 100 * anArg.CurrentStep / anArg.TotalSteps;
      this.lblPercent.Text = anArg.Percent.ToString() + "% " + anArg.Description;
      this.progressBar2.Value = anArg.Percent;
      if (anArg.CurrentStep != anArg.TotalSteps || anArg.Percent != 100)
        return;
      this.SetFinished();
    }

    public bool IsCancelled() => this._isCancelled;

    public void SetFinished()
    {
      this._isDone = true;
      this.lblDescription.Text = "Finished";
      this.progressBar1.Value = 100;
      this.lblPercent.Text = "100%";
      this.progressBar1.Value = 100;
      this.btnCancel.Visible = true;
      this.btnCancel.Text = EngineModelRes.EngineProgressForm_btnCancel_DoneText;
      this.Cursor = Cursors.Default;
    }

    public void ShowCancelButton() => this.btnCancel.Visible = true;

    public void HideCancelButton() => this.btnCancel.Visible = false;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.progressBar1 = new ProgressBar();
      this.btnCancel = new Button();
      this.progressBar2 = new ProgressBar();
      this.lblPercent = new Label();
      this.lblDescription = new Label();
      this.SuspendLayout();
      this.progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.progressBar1.Location = new Point(12, 25);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new Size(381, 25);
      this.progressBar1.TabIndex = 0;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(306, 108);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(87, 27);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Visible = false;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.progressBar2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.progressBar2.Location = new Point(12, 69);
      this.progressBar2.Name = "progressBar2";
      this.progressBar2.Size = new Size(381, 25);
      this.progressBar2.TabIndex = 3;
      this.lblPercent.AutoSize = true;
      this.lblPercent.Location = new Point(12, 53);
      this.lblPercent.Name = "lblPercent";
      this.lblPercent.Size = new Size(54, 13);
      this.lblPercent.TabIndex = 2;
      this.lblPercent.Text = "lblPercent";
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(12, 9);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(70, 13);
      this.lblDescription.TabIndex = 1;
      this.lblDescription.Text = "lblDescription";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(405, 147);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblPercent);
      this.Controls.Add((Control) this.progressBar2);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblDescription);
      this.Controls.Add((Control) this.progressBar1);
      this.Cursor = Cursors.WaitCursor;
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (frmEngineProgress);
      this.Text = "frmProgress";
      this.Load += new EventHandler(this.frmProgress_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
