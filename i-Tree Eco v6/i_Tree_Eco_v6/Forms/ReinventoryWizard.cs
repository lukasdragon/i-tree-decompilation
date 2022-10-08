// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ReinventoryWizard
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using Eco.Util;
using Eco.Util.Tasks;
using i_Tree_Eco_v6.Properties;
using NHibernate;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ReinventoryWizard : Form
  {
    private ProgramSession m_ps;
    private Eco.Domain.v6.Year m_year;
    private TaskManager m_tm;
    private IContainer components;
    private Wizard wizard1;
    private WizardPage wpIntro;
    private RichTextLabel richTextLabel1;
    private WizardPage wpReinvYear;
    private TableLayoutPanel pnlReinvOptions;
    private Label lblProjectLocation;
    private Label lblReinvYear;
    private RichTextLabel richTextLabel2;
    private TextBox txtProjectLocation;
    private DateTimePicker dtReinvYear;
    private CheckBox chkOpenAfter;
    private ErrorProvider ep;

    public ReinventoryWizard()
    {
      this.InitializeComponent();
      this.m_ps = Program.Session;
      this.m_tm = new TaskManager(new WaitCursor((Form) this));
      this.m_tm.Add(Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (t.IsFaulted || this.IsDisposed)
          return;
        this.dtReinvYear.MinDate = DateTime.ParseExact(((int) this.m_year.Id + 1).ToString(), "yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
      }), TaskScheduler.FromCurrentSynchronizationContext()));
    }

    private void LoadData()
    {
      using (ISession session = this.m_ps.InputSession.CreateSession())
      {
        using (session.BeginTransaction())
          this.m_year = session.Get<Eco.Domain.v6.Year>((object) this.m_ps.InputSession.YearKey);
      }
    }

    private void txtProjectLocation_Click(object sender, EventArgs e)
    {
      if (this.m_ps == null || this.m_ps.InputSession == null || this.m_ps.InputSession.InputDb == null)
        return;
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();
      saveFileDialog1.Title = i_Tree_Eco_v6.Resources.Strings.SaveProjectAs;
      saveFileDialog1.Filter = string.Join("|", new string[2]
      {
        string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterEcoFile, (object) string.Join(";", Settings.Default.ExtEcoProj)),
        string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
      });
      saveFileDialog1.CheckPathExists = true;
      saveFileDialog1.OverwritePrompt = true;
      saveFileDialog1.AddExtension = true;
      saveFileDialog1.DefaultExt = "ieco";
      saveFileDialog1.ShowHelp = false;
      saveFileDialog1.CreatePrompt = false;
      SaveFileDialog saveFileDialog2 = saveFileDialog1;
      if (saveFileDialog2.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.txtProjectLocation.Text = saveFileDialog2.FileName;
    }

    private void wizard1_Cancel(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

    private void wizard1_Finished(object sender, EventArgs e)
    {
      CancellationTokenSource token = new CancellationTokenSource();
      Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
      ReinventoryTask reinventoryTask = new ReinventoryTask(this.m_ps.InputSession, this.txtProjectLocation.Text, (short) this.dtReinvYear.Value.Year);
      ProcessingForm processingForm = new ProcessingForm();
      reinventoryTask.CancellationToken = token.Token;
      reinventoryTask.Progress = (IProgress<ProgressEventArgs>) progress;
      Task t1 = reinventoryTask.Execute().ContinueWith((System.Action<Task<bool>>) (t =>
      {
        if (t.IsFaulted)
        {
          int num = (int) MessageBox.Show(string.Format(i_Tree_Eco_v6.Resources.Strings.ErrUnexpected, (object) t.Exception.InnerException.Message));
        }
        else
        {
          if (t.IsCanceled)
            return;
          this.OpenOnceCompleted = this.chkOpenAfter.Checked;
          this.ProjectLocation = this.txtProjectLocation.Text;
          this.Year = (short) this.dtReinvYear.Value.Year;
          this.DialogResult = DialogResult.OK;
        }
      }), TaskScheduler.FromCurrentSynchronizationContext());
      int num1 = (int) processingForm.ShowDialog((IWin32Window) this, t1, progress, token);
    }

    public bool OpenOnceCompleted { get; private set; }

    public string ProjectLocation { get; private set; }

    public short Year { get; private set; }

    private void wpReinvYear_Validating(object sender, CancelEventArgs e)
    {
      string text = this.txtProjectLocation.Text;
      if (string.IsNullOrEmpty(text))
        this.ep.SetError((Control) this.txtProjectLocation, "Project location is required.");
      else
        this.ep.SetError((Control) this.txtProjectLocation, text.Equals(this.m_ps.InputSession.InputDb), "Project location cannot be the same as the current project's location.");
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ReinventoryWizard));
      this.wizard1 = new Wizard();
      this.wpIntro = new WizardPage();
      this.richTextLabel1 = new RichTextLabel();
      this.wpReinvYear = new WizardPage();
      this.pnlReinvOptions = new TableLayoutPanel();
      this.lblProjectLocation = new Label();
      this.lblReinvYear = new Label();
      this.txtProjectLocation = new TextBox();
      this.dtReinvYear = new DateTimePicker();
      this.chkOpenAfter = new CheckBox();
      this.richTextLabel2 = new RichTextLabel();
      this.ep = new ErrorProvider(this.components);
      this.wpIntro.SuspendLayout();
      this.wpReinvYear.SuspendLayout();
      this.pnlReinvOptions.SuspendLayout();
      ((ISupportInitialize) this.ep).BeginInit();
      this.SuspendLayout();
      this.wizard1.BackColor = Color.White;
      this.wizard1.Dock = DockStyle.Fill;
      this.wizard1.Location = new Point(0, 0);
      this.wizard1.Name = "wizard1";
      this.wizard1.Pages.Add(this.wpIntro);
      this.wizard1.Pages.Add(this.wpReinvYear);
      this.wizard1.Sidebar.BackColor = Color.White;
      this.wizard1.Sidebar.BackgroundImage = (Image) i_Tree_Eco_v6.Properties.Resources.iTreeSidebar;
      this.wizard1.Sidebar.BackgroundImageLayout = ImageLayout.Stretch;
      this.wizard1.Sidebar.Name = "Sidebar";
      this.wizard1.Sidebar.Size = new Size(108, 265);
      this.wizard1.Sidebar.TabIndex = 1;
      this.wizard1.SidebarWidth = 109;
      this.wizard1.Size = new Size(429, 312);
      this.wizard1.TabIndex = 0;
      this.wizard1.Finished += new EventHandler(this.wizard1_Finished);
      this.wizard1.Cancel += new EventHandler(this.wizard1_Cancel);
      this.wpIntro.Controls.Add((Control) this.richTextLabel1);
      this.wpIntro.Name = "wpIntro";
      this.wpIntro.Padding = new Padding(10);
      this.wpIntro.Size = new Size(320, 265);
      this.wpIntro.TabIndex = 0;
      this.wpIntro.Text = "wizardPage1";
      this.richTextLabel1.Dock = DockStyle.Fill;
      this.richTextLabel1.Location = new Point(10, 10);
      this.richTextLabel1.Name = "richTextLabel1";
      this.richTextLabel1.RichText = componentResourceManager.GetString("richTextLabel1.RichText");
      this.richTextLabel1.Size = new Size(300, 245);
      this.richTextLabel1.TabIndex = 0;
      this.richTextLabel1.TabStop = false;
      this.wpReinvYear.Controls.Add((Control) this.pnlReinvOptions);
      this.wpReinvYear.Controls.Add((Control) this.richTextLabel2);
      this.wpReinvYear.Name = "wpReinvYear";
      this.wpReinvYear.Padding = new Padding(10);
      this.wpReinvYear.Size = new Size(320, 265);
      this.wpReinvYear.TabIndex = 2;
      this.wpReinvYear.Text = "wizardPage1";
      this.wpReinvYear.Validating += new CancelEventHandler(this.wpReinvYear_Validating);
      this.pnlReinvOptions.ColumnCount = 3;
      this.pnlReinvOptions.ColumnStyles.Add(new ColumnStyle());
      this.pnlReinvOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.pnlReinvOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.pnlReinvOptions.Controls.Add((Control) this.lblProjectLocation, 0, 0);
      this.pnlReinvOptions.Controls.Add((Control) this.lblReinvYear, 0, 1);
      this.pnlReinvOptions.Controls.Add((Control) this.txtProjectLocation, 1, 0);
      this.pnlReinvOptions.Controls.Add((Control) this.dtReinvYear, 1, 1);
      this.pnlReinvOptions.Controls.Add((Control) this.chkOpenAfter, 0, 3);
      this.pnlReinvOptions.Dock = DockStyle.Fill;
      this.pnlReinvOptions.Location = new Point(10, 92);
      this.pnlReinvOptions.Name = "pnlReinvOptions";
      this.pnlReinvOptions.RowCount = 5;
      this.pnlReinvOptions.RowStyles.Add(new RowStyle());
      this.pnlReinvOptions.RowStyles.Add(new RowStyle());
      this.pnlReinvOptions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.pnlReinvOptions.RowStyles.Add(new RowStyle());
      this.pnlReinvOptions.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.pnlReinvOptions.Size = new Size(300, 163);
      this.pnlReinvOptions.TabIndex = 1;
      this.lblProjectLocation.Anchor = AnchorStyles.Left;
      this.lblProjectLocation.AutoSize = true;
      this.lblProjectLocation.Location = new Point(3, 6);
      this.lblProjectLocation.Name = "lblProjectLocation";
      this.lblProjectLocation.Size = new Size(87, 13);
      this.lblProjectLocation.TabIndex = 0;
      this.lblProjectLocation.Text = "Project Location:";
      this.lblReinvYear.Anchor = AnchorStyles.Left;
      this.lblReinvYear.AutoSize = true;
      this.lblReinvYear.Location = new Point(3, 32);
      this.lblReinvYear.Name = "lblReinvYear";
      this.lblReinvYear.Size = new Size(92, 13);
      this.lblReinvYear.TabIndex = 1;
      this.lblReinvYear.Text = "Reinventory Year:";
      this.txtProjectLocation.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.pnlReinvOptions.SetColumnSpan((Control) this.txtProjectLocation, 2);
      this.txtProjectLocation.Location = new Point(101, 3);
      this.txtProjectLocation.Name = "txtProjectLocation";
      this.txtProjectLocation.ReadOnly = true;
      this.txtProjectLocation.Size = new Size(196, 20);
      this.txtProjectLocation.TabIndex = 2;
      this.txtProjectLocation.Click += new EventHandler(this.txtProjectLocation_Click);
      this.dtReinvYear.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.dtReinvYear.CustomFormat = "yyyy";
      this.dtReinvYear.Format = DateTimePickerFormat.Custom;
      this.dtReinvYear.Location = new Point(101, 29);
      this.dtReinvYear.Name = "dtReinvYear";
      this.dtReinvYear.ShowUpDown = true;
      this.dtReinvYear.Size = new Size(95, 20);
      this.dtReinvYear.TabIndex = 4;
      this.chkOpenAfter.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.chkOpenAfter.AutoSize = true;
      this.chkOpenAfter.Checked = true;
      this.chkOpenAfter.CheckState = CheckState.Checked;
      this.pnlReinvOptions.SetColumnSpan((Control) this.chkOpenAfter, 3);
      this.chkOpenAfter.Location = new Point(3, 75);
      this.chkOpenAfter.Name = "chkOpenAfter";
      this.chkOpenAfter.Size = new Size(294, 17);
      this.chkOpenAfter.TabIndex = 6;
      this.chkOpenAfter.Text = "Open reinventory once finished?";
      this.chkOpenAfter.UseVisualStyleBackColor = true;
      this.richTextLabel2.AutoSize = true;
      this.richTextLabel2.Dock = DockStyle.Top;
      this.richTextLabel2.Location = new Point(10, 10);
      this.richTextLabel2.Name = "richTextLabel2";
      this.richTextLabel2.RichText = componentResourceManager.GetString("richTextLabel2.RichText");
      this.richTextLabel2.Size = new Size(300, 82);
      this.richTextLabel2.TabIndex = 0;
      this.richTextLabel2.TabStop = false;
      this.ep.ContainerControl = (ContainerControl) this;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(429, 312);
      this.Controls.Add((Control) this.wizard1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ReinventoryWizard);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "i-Tree Eco - Re-inventory Wizard";
      this.wpIntro.ResumeLayout(false);
      this.wpReinvYear.ResumeLayout(false);
      this.wpReinvYear.PerformLayout();
      this.pnlReinvOptions.ResumeLayout(false);
      this.pnlReinvOptions.PerformLayout();
      ((ISupportInitialize) this.ep).EndInit();
      this.ResumeLayout(false);
    }
  }
}
