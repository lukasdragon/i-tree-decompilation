// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ProcessingForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Util;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ProcessingForm : Form
  {
    private Task _task;
    private CancellationTokenSource _cts;
    private Progress<ProgressEventArgs> _progress;
    private IContainer components;
    private Button btnCancel;
    private ProgressBar pbStatus;
    private Label lblStatus;
    private TableLayoutPanel tableLayoutPanel1;

    public ProcessingForm() => this.InitializeComponent();

    public ProcessingForm(Task t, Progress<ProgressEventArgs> progress)
      : this(t, progress, (CancellationTokenSource) null)
    {
    }

    public ProcessingForm(
      Task t,
      Progress<ProgressEventArgs> progress,
      CancellationTokenSource cts)
      : this()
    {
      this._task = t;
      this._progress = progress;
      this._cts = cts;
    }

    public DialogResult ShowDialog(
      IWin32Window owner,
      Task t,
      Progress<ProgressEventArgs> progress)
    {
      this._task = t;
      this._progress = progress;
      return this._task != null && this._progress != null ? this.ShowDialog(owner) : DialogResult.Cancel;
    }

    private void Progress_ProgressChanged(object sender, ProgressEventArgs e)
    {
      this.lblStatus.Text = e.Status;
      if (e.Total < e.Progress)
      {
        this.pbStatus.Style = ProgressBarStyle.Marquee;
      }
      else
      {
        this.pbStatus.Style = ProgressBarStyle.Blocks;
        this.pbStatus.Maximum = e.Total;
        if (e.Progress < e.Total)
          this.pbStatus.Value = e.Progress + 1;
        this.pbStatus.Value = e.Progress;
      }
    }

    public DialogResult ShowDialog(
      IWin32Window owner,
      Task t,
      Progress<ProgressEventArgs> progress,
      CancellationTokenSource token)
    {
      this._cts = token;
      return this.ShowDialog(owner, t, progress);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (this._cts != null)
        this._cts.Cancel();
      this.btnCancel.Enabled = false;
    }

    private void DataProcessingForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (e.CloseReason != CloseReason.UserClosing || this._cts == null)
        return;
      this._cts.Cancel();
    }

    private void ProcessingForm_Shown(object sender, EventArgs e)
    {
      this.btnCancel.Visible = this._cts != null;
      if (this._task != null)
        this._task.ContinueWith((System.Action<Task>) (task =>
        {
          if (task.IsFaulted || task.IsCanceled)
            this.DialogResult = DialogResult.Cancel;
          else
            this.DialogResult = DialogResult.OK;
          this.Close();
        }), TaskScheduler.FromCurrentSynchronizationContext());
      if (this._progress == null)
        return;
      this._progress.ProgressChanged += new EventHandler<ProgressEventArgs>(this.Progress_ProgressChanged);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ProcessingForm));
      this.btnCancel = new Button();
      this.pbStatus = new ProgressBar();
      this.lblStatus = new Label();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      componentResourceManager.ApplyResources((object) this.pbStatus, "pbStatus");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.pbStatus, 2);
      this.pbStatus.Name = "pbStatus";
      componentResourceManager.ApplyResources((object) this.lblStatus, "lblStatus");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblStatus, 2);
      this.lblStatus.Name = "lblStatus";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.pbStatus, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblStatus, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.btnCancel, 1, 2);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ProcessingForm);
      this.ShowInTaskbar = false;
      this.FormClosed += new FormClosedEventHandler(this.DataProcessingForm_FormClosed);
      this.Shown += new EventHandler(this.ProcessingForm_Shown);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
