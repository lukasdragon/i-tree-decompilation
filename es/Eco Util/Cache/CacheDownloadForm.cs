// Decompiled with JetBrains decompiler
// Type: Eco.Util.Cache.CacheDownloadForm
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Eco.Util.Cache
{
  public class CacheDownloadForm : Form
  {
    private const string total_files_label_format = "Total Progress (File {0}/{1})";
    private const string current_file_label_format = "Downloading File {0}...";
    private IContainer components;
    private ProgressBar totalFileProgressBar;
    private Label totalFileProgressLabel;
    private Label currentFileProgressLabel;
    private ProgressBar currentFileProgressBar;

    private int total_files { get; set; }

    private int downloaded_files { get; set; }

    public CacheDownloadForm() => this.InitializeComponent();

    public void SetFileCount(int total_files)
    {
      this.total_files = total_files;
      this.downloaded_files = 0;
      this.totalFileProgressBar.Maximum = total_files;
      this.totalFileProgressLabel.Text = string.Format("Total Progress (File {0}/{1})", (object) this.downloaded_files, (object) this.total_files);
      this.currentFileProgressLabel.Text = string.Format("Downloading File {0}...", (object) (this.downloaded_files + 1));
    }

    public void UpdateTotalFileProgress(int downloaded_files)
    {
      if (downloaded_files > this.totalFileProgressBar.Maximum)
        return;
      this.downloaded_files = downloaded_files;
      this.totalFileProgressBar.Value = downloaded_files;
      this.totalFileProgressLabel.Text = string.Format("Total Progress (File {0}/{1})", (object) this.downloaded_files, (object) this.total_files);
    }

    public void UpdateCurrentFileProgress(int percent_complete)
    {
      this.currentFileProgressBar.Value = percent_complete;
      this.currentFileProgressLabel.Text = string.Format("Downloading File {0}...", (object) (this.downloaded_files + 1));
    }

    private void CacheDownloadForm_Load(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.totalFileProgressBar = new ProgressBar();
      this.totalFileProgressLabel = new Label();
      this.currentFileProgressLabel = new Label();
      this.currentFileProgressBar = new ProgressBar();
      this.SuspendLayout();
      this.totalFileProgressBar.Location = new Point(12, 48);
      this.totalFileProgressBar.Name = "totalFileProgressBar";
      this.totalFileProgressBar.Size = new Size(483, 23);
      this.totalFileProgressBar.TabIndex = 0;
      this.totalFileProgressLabel.AutoSize = true;
      this.totalFileProgressLabel.Location = new Point(13, 29);
      this.totalFileProgressLabel.Name = "totalFileProgressLabel";
      this.totalFileProgressLabel.Size = new Size(75, 13);
      this.totalFileProgressLabel.TabIndex = 1;
      this.totalFileProgressLabel.Text = "Total Progress";
      this.currentFileProgressLabel.AutoSize = true;
      this.currentFileProgressLabel.Location = new Point(13, 92);
      this.currentFileProgressLabel.Name = "currentFileProgressLabel";
      this.currentFileProgressLabel.Size = new Size(23, 13);
      this.currentFileProgressLabel.TabIndex = 2;
      this.currentFileProgressLabel.Text = "File";
      this.currentFileProgressBar.Location = new Point(12, 108);
      this.currentFileProgressBar.Name = "currentFileProgressBar";
      this.currentFileProgressBar.Size = new Size(483, 23);
      this.currentFileProgressBar.TabIndex = 3;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(507, 185);
      this.ControlBox = false;
      this.Controls.Add((Control) this.currentFileProgressBar);
      this.Controls.Add((Control) this.currentFileProgressLabel);
      this.Controls.Add((Control) this.totalFileProgressLabel);
      this.Controls.Add((Control) this.totalFileProgressBar);
      this.Name = nameof (CacheDownloadForm);
      this.Text = "Downloading Cache";
      this.Load += new EventHandler(this.CacheDownloadForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
