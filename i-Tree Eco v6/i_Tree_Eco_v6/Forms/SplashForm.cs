// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.SplashForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class SplashForm : Form
  {
    private IContainer components;
    private PictureBox pictureBox1;
    private Timer timer1;

    public SplashForm()
    {
      this.InitializeComponent();
      this.ClientSize = Resources.EcoSplash.Size;
    }

    public void CloseSplashForm() => this.Close();

    private void timer1_Tick(object sender, EventArgs e)
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
      this.components = (IContainer) new Container();
      this.pictureBox1 = new PictureBox();
      this.timer1 = new Timer(this.components);
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.pictureBox1.Dock = DockStyle.Fill;
      this.pictureBox1.Image = (Image) Resources.EcoSplash;
      this.pictureBox1.Location = new Point(0, 0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(500, 459);
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.timer1.Interval = 5000;
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(500, 459);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pictureBox1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (SplashForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (SplashForm);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
    }

    public delegate void closeSplashForm();
  }
}
