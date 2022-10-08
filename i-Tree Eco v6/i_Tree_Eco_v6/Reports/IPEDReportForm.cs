// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IPEDReportForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using EcoIpedReportGenerator;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Forms;
using i_Tree_Eco_v6.Properties;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Reports
{
  public class IPEDReportForm : ContentForm
  {
    private ProgramSession m_ps;
    private IPEDReports m_ipedReport;
    private IContainer components;
    private RgControl rgControl1;
    private Label lblOutOfDateWarning;
    private Label label1;
    private PictureBox pictureBox1;
    private TableLayoutPanel pnlWarning;

    public IPEDReportForm()
    {
      this.InitializeComponent();
      this.m_ps = Program.Session;
      this.ToggleBanner();
      this.m_ps.InputSessionChanged += new EventHandler(this.m_ps_InputSessionChanged);
      EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));
    }

    public IPEDReports IpedReport => this.m_ipedReport;

    private void ToggleBanner()
    {
      using (ISession session = this.m_ps.InputSession.CreateSession())
      {
        using (session.BeginTransaction())
        {
          Year year = session.Get<Year>((object) this.m_ps.InputSession.YearKey.Value);
          int num = session.CreateCriteria<YearResult>().SetProjection((IProjection) Projections.Max("RevProcessed")).Add((ICriterion) Restrictions.Eq("Year", (object) year)).UniqueResult<int>();
          this.pnlWarning.Visible = year.RevProcessed > 0 && (year.Changed || year.RevProcessed != num);
        }
      }
    }

    private void Year_Updated(object sender, EntityUpdated<Year> e) => this.ToggleBanner();

    private void m_ps_InputSessionChanged(object sender, EventArgs e) => this.m_ps.InputSession.YearChanged += new EventHandler(this.InputSession_YearChanged);

    private void InputSession_YearChanged(object sender, EventArgs e)
    {
      if (!this.m_ps.InputSession.YearKey.HasValue)
        return;
      this.ToggleBanner();
    }

    public void SetReport(IPEDReports curReport)
    {
      this.m_ipedReport = curReport;
      this.RunReport();
    }

    public void RunReport()
    {
      ProgramSession instance = ProgramSession.GetInstance();
      using (ISession session = instance.InputSession.CreateSession())
      {
        Year year = session.Load<Year>((object) instance.InputSession.YearKey);
        Path.GetDirectoryName(Application.ExecutablePath);
        this.rgControl1.GenerateReport(year.Series.IsSample, instance.SpeciesDisplayName == SpeciesDisplayEnum.ScientificName, (int) this.m_ipedReport, instance.InputSession.YearKey.Value, instance);
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
      this.rgControl1 = new RgControl();
      this.lblOutOfDateWarning = new Label();
      this.label1 = new Label();
      this.pictureBox1 = new PictureBox();
      this.pnlWarning = new TableLayoutPanel();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.pnlWarning.SuspendLayout();
      this.SuspendLayout();
      this.lblBreadcrumb.Size = new Size(1007, 29);
      this.rgControl1.AutoSize = true;
      this.rgControl1.Dock = DockStyle.Fill;
      this.rgControl1.Location = new Point(0, 73);
      this.rgControl1.Name = "rgControl1";
      this.rgControl1.Size = new Size(1007, 428);
      this.rgControl1.TabIndex = 0;
      this.rgControl1.UseScientificName = false;
      this.lblOutOfDateWarning.AutoSize = true;
      this.lblOutOfDateWarning.BackColor = Color.Transparent;
      this.lblOutOfDateWarning.Dock = DockStyle.Fill;
      this.lblOutOfDateWarning.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblOutOfDateWarning.ForeColor = Color.White;
      this.lblOutOfDateWarning.Location = new Point(44, 0);
      this.lblOutOfDateWarning.Name = "lblOutOfDateWarning";
      this.lblOutOfDateWarning.Size = new Size(722, 45);
      this.lblOutOfDateWarning.TabIndex = 4;
      this.lblOutOfDateWarning.Text = "Warning! Reports may display inconsistent results due to recent edits.\r\nPlease Submit Data for Processing and Retrieve Results to update them.";
      this.lblOutOfDateWarning.TextAlign = ContentAlignment.MiddleLeft;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Dock = DockStyle.Top;
      this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.White;
      this.label1.Location = new Point(44, 3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(957, 32);
      this.label1.TabIndex = 1;
      this.label1.Text = "Warning! Reports may display inconsistent results due to recent edits.\r\nPlease Submit Data for Processing and Retrieve Results to update them.";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.pictureBox1.Dock = DockStyle.Fill;
      this.pictureBox1.Image = (Image) Resources.Warning_Large;
      this.pictureBox1.Location = new Point(6, 6);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 2;
      this.pictureBox1.TabStop = false;
      this.pnlWarning.AutoSize = true;
      this.pnlWarning.BackColor = Color.Red;
      this.pnlWarning.ColumnCount = 2;
      this.pnlWarning.ColumnStyles.Add(new ColumnStyle());
      this.pnlWarning.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.pnlWarning.Controls.Add((Control) this.pictureBox1, 0, 0);
      this.pnlWarning.Controls.Add((Control) this.label1, 1, 0);
      this.pnlWarning.Dock = DockStyle.Top;
      this.pnlWarning.Location = new Point(0, 29);
      this.pnlWarning.Name = "pnlWarning";
      this.pnlWarning.Padding = new Padding(3);
      this.pnlWarning.RowStyles.Add(new RowStyle());
      this.pnlWarning.Size = new Size(1007, 44);
      this.pnlWarning.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1007, 501);
      this.Controls.Add((Control) this.rgControl1);
      this.Controls.Add((Control) this.pnlWarning);
      this.Name = nameof (IPEDReportForm);
      this.Text = nameof (IPEDReportForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.pnlWarning, 0);
      this.Controls.SetChildIndex((Control) this.rgControl1, 0);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.pnlWarning.ResumeLayout(false);
      this.pnlWarning.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
