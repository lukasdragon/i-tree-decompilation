// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ConvertProjectForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Util;
using Eco.Util.Tasks;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ConvertProjectForm : Form
  {
    private string m_v5db;
    private string m_v6db;
    private CancellationTokenSource m_cts;
    private IContainer components;
    private Label label1;
    private Button btnCancel;
    private Label lblStatus;
    private ProgressBar pbCompleted;
    private ProgressBar pbProgress;

    public ConvertProjectForm(string v5db, string v6db)
    {
      this.InitializeComponent();
      this.m_cts = new CancellationTokenSource();
      this.m_v5db = v5db;
      this.m_v6db = v6db;
    }

    private void ConvertProjectForm_Shown(object sender, EventArgs e)
    {
      TaskScheduler context = TaskScheduler.FromCurrentSynchronizationContext();
      Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
      progress.ProgressChanged += new EventHandler<ProgressEventArgs>(this.Conversion_ProgressChanged);
      new CreateProjectTask(this.m_v6db).DoWork().ContinueWith((System.Action<Task<bool>>) (t => new ConvertV5Task(this.m_v5db, this.m_v6db, Program.Session.LocSp, this.m_cts.Token, (IProgress<ProgressEventArgs>) progress).DoWork().ContinueWith((System.Action<Task>) (task =>
      {
        if (task.IsFaulted)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, task.Exception.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (task.IsCanceled)
        {
          this.DialogResult = DialogResult.Cancel;
          File.Delete(this.m_v6db);
        }
        else
          this.DialogResult = DialogResult.OK;
        this.Close();
      }), context)));
    }

    private void Conversion_ProgressChanged(object sender, ProgressEventArgs e)
    {
      this.lblStatus.Text = e.Status;
      if (e.Total < e.Progress)
      {
        this.pbCompleted.Style = ProgressBarStyle.Marquee;
        this.pbProgress.PerformStep();
      }
      else
      {
        this.pbCompleted.Style = ProgressBarStyle.Blocks;
        this.pbCompleted.Maximum = e.Total;
        this.pbCompleted.Value = e.Progress;
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.m_cts.Cancel();
      this.btnCancel.Enabled = false;
    }

    private void ConvertProjectForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (e.CloseReason != CloseReason.UserClosing)
        return;
      this.m_cts.Cancel();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConvertProjectForm));
      this.label1 = new Label();
      this.btnCancel = new Button();
      this.lblStatus = new Label();
      this.pbCompleted = new ProgressBar();
      this.pbProgress = new ProgressBar();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      componentResourceManager.ApplyResources((object) this.lblStatus, "lblStatus");
      this.lblStatus.Name = "lblStatus";
      componentResourceManager.ApplyResources((object) this.pbCompleted, "pbCompleted");
      this.pbCompleted.Name = "pbCompleted";
      componentResourceManager.ApplyResources((object) this.pbProgress, "pbProgress");
      this.pbProgress.Maximum = 13;
      this.pbProgress.Name = "pbProgress";
      this.pbProgress.Step = 1;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.Controls.Add((Control) this.pbProgress);
      this.Controls.Add((Control) this.pbCompleted);
      this.Controls.Add((Control) this.lblStatus);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConvertProjectForm);
      this.ShowInTaskbar = false;
      this.FormClosed += new FormClosedEventHandler(this.ConvertProjectForm_FormClosed);
      this.Shown += new EventHandler(this.ConvertProjectForm_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
