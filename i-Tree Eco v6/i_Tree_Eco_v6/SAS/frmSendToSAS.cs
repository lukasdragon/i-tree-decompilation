// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.SAS.frmSendToSAS
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Convert;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Properties;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.SAS
{
  public class frmSendToSAS : Form
  {
    private ProgramSession m_ps;
    private bool inProcessing;
    private bool inFtp;
    private bool errorOccur;
    private CancellationTokenSource uploadCancellationSource = new CancellationTokenSource();
    private Progress<SASProgressArg> uploadProgress = new Progress<SASProgressArg>();
    private SASProgressArg uploadProgressArg = new SASProgressArg();
    private IContainer components;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private TextBox textName;
    private TextBox textAddress;
    private MaskedTextBox maskedTextPhone;
    private TextBox textEmail1;
    private TextBox textEmail2;
    private TextBox textNotes;
    private Button bttnOK;
    private Button bttnCancel;
    private Label label7;
    private MaskedTextBox maskedTextPhoneExtension;
    private ErrorProvider errorProvider1;
    private Label label9;
    private Label labelWaiting;
    private ProgressBar progressBar1;
    private Label labelPercent;
    private Panel panelSend;
    private Label label8;
    public Label labelProjectInfo;
    private Label labelError;

    public frmSendToSAS() => this.InitializeComponent();

    public async void InitializeForm(ProgramSession passin_m_ps, Form ParentForm)
    {
      frmSendToSAS frmSendToSas = this;
      frmSendToSas.m_ps = passin_m_ps;
      int num = (int) frmSendToSas.ShowDialog((IWin32Window) ParentForm);
    }

    private async void bttnOK_Click(object sender, EventArgs e)
    {
      frmSendToSAS sender1 = this;
      sender1.textName.Text = sender1.textName.Text.Trim();
      if (sender1.textName.Text == "")
      {
        sender1.errorProvider1.SetError((Control) sender1.textName, SASResources.ErrorNameRequired);
      }
      else
      {
        sender1.errorProvider1.SetError((Control) sender1.textName, "");
        sender1.textEmail1.Text = sender1.textEmail1.Text.Trim();
        sender1.textEmail2.Text = sender1.textEmail2.Text.Trim();
        if (sender1.textEmail1.Text == "" || sender1.textEmail1.Text.ToLower() != sender1.textEmail2.Text.ToLower())
        {
          sender1.errorProvider1.SetError((Control) sender1.textEmail1, SASResources.ErrorEmailMatch);
          sender1.errorProvider1.SetError((Control) sender1.textEmail2, SASResources.ErrorEmailMatch);
        }
        else
        {
          sender1.errorProvider1.SetError((Control) sender1.textEmail1, "");
          sender1.errorProvider1.SetError((Control) sender1.textEmail2, "");
          try
          {
            if (new MailAddress(sender1.textEmail1.Text).Address.ToLower() != sender1.textEmail1.Text.ToLower())
            {
              sender1.errorProvider1.SetError((Control) sender1.textEmail1, SASResources.ErrorInvalidEmail);
              sender1.errorProvider1.SetError((Control) sender1.textEmail2, SASResources.ErrorInvalidEmail);
              return;
            }
          }
          catch
          {
            sender1.errorProvider1.SetError((Control) sender1.textEmail1, SASResources.ErrorInvalidEmail);
            sender1.errorProvider1.SetError((Control) sender1.textEmail2, SASResources.ErrorInvalidEmail);
            return;
          }
          sender1.errorProvider1.SetError((Control) sender1.textEmail1, "");
          sender1.errorProvider1.SetError((Control) sender1.textEmail2, "");
          if (sender1.inProcessing)
            return;
          sender1.inProcessing = true;
          sender1.labelWaiting.Visible = true;
          sender1.labelError.Visible = false;
          sender1.UseWaitCursor = true;
          sender1.progressBar1.Value = 0;
          sender1.labelPercent.Text = "0%";
          sender1.uploadCancellationSource = new CancellationTokenSource();
          CancellationToken uploadCancellationToken = sender1.uploadCancellationSource.Token;
          sender1.uploadProgressArg.Description = SASResources.MsgProcessing;
          sender1.uploadProgressArg.TotalSteps = 1;
          sender1.uploadProgressArg.CurrentStep = 1;
          sender1.uploadProgressArg.Percent = 0;
          sender1.uploadProgress.ProgressChanged += (EventHandler<SASProgressArg>) ((o, myArg) =>
          {
            if (myArg.Percent < 0 || myArg.Percent > 100)
              return;
            this.progressBar1.Visible = true;
            this.progressBar1.Value = myArg.Percent;
            this.labelPercent.Visible = true;
            this.labelPercent.Text = myArg.Percent.ToString() + "%";
            this.labelWaiting.Text = myArg.Description;
          });
          Settings.Default.Submitter = new Submitter()
          {
            Name = sender1.textName.Text,
            Address = sender1.textAddress.Text,
            Email = sender1.textEmail1.Text,
            Phone = new Phone()
            {
              Number = sender1.maskedTextPhone.Text,
              Extension = sender1.maskedTextPhoneExtension.Text
            }
          };
          bool errorOccur = false;
          using (SASProcessor aSASProcessor = new SASProcessor())
          {
            await Task.Factory.StartNew((System.Action) (() =>
            {
              try
              {
                aSASProcessor.prepareToStart(this.m_ps, (Form) this);
                int num1 = 0;
                int progressToRange = 2;
                IProgress<SASProgressArg> uploadProgress = (IProgress<SASProgressArg>) this.uploadProgress;
                this.uploadProgressArg.Percent = progressToRange;
                uploadProgress.Report(this.uploadProgressArg);
                if (uploadCancellationToken.IsCancellationRequested)
                  errorOccur = true;
                if (!errorOccur)
                {
                  int progressFromRange = progressToRange;
                  progressToRange = 35;
                  if (!aSASProcessor.Export(false, (IProgress<SASProgressArg>) this.uploadProgress, uploadCancellationToken, this.uploadProgressArg, progressFromRange, progressToRange))
                    errorOccur = true;
                }
                if (!errorOccur)
                  aSASProcessor.AddContactInfoToReadMeFile(this.textName.Text, this.textAddress.Text, this.maskedTextPhone.Text, this.maskedTextPhoneExtension.Text, this.textEmail1.Text, this.textNotes.Text);
                if (!errorOccur && !aSASProcessor.ZipExportedFiles())
                  errorOccur = true;
                num1 = progressToRange;
                int num2 = 40;
                this.uploadProgressArg.Percent = num2;
                uploadProgress.Report(this.uploadProgressArg);
                if (!errorOccur)
                {
                  this.inFtp = true;
                  int progressFromRange = num2;
                  num2 += 2;
                  if (!aSASProcessor.UploadEmailAddressFile((IProgress<SASProgressArg>) this.uploadProgress, uploadCancellationToken, this.uploadProgressArg, progressFromRange, num2, (Form) this))
                    errorOccur = true;
                  this.inFtp = false;
                }
                if (errorOccur)
                  return;
                this.inFtp = true;
                if (!aSASProcessor.UploadZipFile((IProgress<SASProgressArg>) this.uploadProgress, uploadCancellationToken, this.uploadProgressArg, num2, 99, (Form) this))
                  errorOccur = true;
                this.inFtp = false;
                this.uploadProgressArg.Percent = 100;
                uploadProgress.Report(this.uploadProgressArg);
              }
              catch (Exception ex)
              {
                StringBuilder stringBuilder = new StringBuilder(ex.Message);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.InternetErrorHelpMessage);
                int num = (int) MessageBox.Show(ex.Message, SASResources.ExportTitle, MessageBoxButtons.OK);
                errorOccur = true;
              }
            }), CancellationToken.None, TaskCreationOptions.None, sender1.m_ps.Scheduler);
            sender1.inProcessing = false;
            sender1.UseWaitCursor = false;
            sender1.labelWaiting.Visible = false;
            sender1.progressBar1.Visible = false;
            sender1.labelPercent.Visible = false;
            if (uploadCancellationToken.IsCancellationRequested)
              return;
            if (errorOccur)
            {
              sender1.labelError.Visible = true;
              sender1.Close();
            }
            else
            {
              using (ISession session = sender1.m_ps.InputSession.CreateSession())
              {
                using (ITransaction transaction = session.BeginTransaction())
                {
                  Year entity1 = session.Get<Year>((object) sender1.m_ps.InputSession.YearKey);
                  entity1.Changed = false;
                  session.Evict((object) entity1);
                  session.SaveOrUpdate((object) entity1);
                  YearResult yearResult = new YearResult()
                  {
                    Year = entity1,
                    RevProcessed = entity1.Revision,
                    DateTime = new DateTime?(DateTime.Now),
                    Data = Path.GetFileName(aSASProcessor.zipFileToSend),
                    Email = sender1.textEmail1.Text
                  };
                  session.SaveOrUpdate((object) yearResult);
                  IList<Forecast> forecastList = session.CreateCriteria<Forecast>().Add((ICriterion) Restrictions.Eq("Year.Guid", (object) sender1.m_ps.InputSession.YearKey)).List<Forecast>();
                  foreach (Forecast forecast in (IEnumerable<Forecast>) forecastList)
                  {
                    forecast.Changed = true;
                    session.SaveOrUpdate((object) forecast);
                  }
                  transaction.Commit();
                  EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(entity1), (Control) sender1);
                  foreach (Forecast entity2 in (IEnumerable<Forecast>) forecastList)
                    EventPublisher.Publish<EntityUpdated<Forecast>>(new EntityUpdated<Forecast>(entity2), (Control) sender1);
                }
              }
              sender1.labelWaiting.Visible = false;
              int num = (int) MessageBox.Show(SASResources.SendFinishMessage, SASResources.ExportTitle, MessageBoxButtons.OK);
              sender1.Close();
            }
          }
        }
      }
    }

    private void frmSendToSAS_Load(object sender, EventArgs e)
    {
      Submitter submitter = Settings.Default.Submitter;
      if (submitter == null)
        return;
      this.textName.Text = submitter.Name;
      this.textAddress.Text = submitter.Address;
      this.maskedTextPhone.Text = submitter.Phone.Number;
      this.maskedTextPhoneExtension.Text = submitter.Phone.Extension;
      this.textEmail1.Text = submitter.Email;
      this.textEmail2.Text = submitter.Email;
    }

    private void bttnCancel_Click(object sender, EventArgs e)
    {
      if (this.inProcessing && MessageBox.Show((IWin32Window) this, SASResources.ConfirmCancel, SASResources.ExportTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        this.progressBar1.Visible = false;
        this.labelPercent.Visible = false;
        this.uploadCancellationSource.Cancel();
        this.inProcessing = false;
      }
      if (this.inProcessing)
        return;
      this.Close();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (frmSendToSAS));
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.textName = new TextBox();
      this.textAddress = new TextBox();
      this.maskedTextPhone = new MaskedTextBox();
      this.textEmail1 = new TextBox();
      this.textEmail2 = new TextBox();
      this.textNotes = new TextBox();
      this.bttnOK = new Button();
      this.bttnCancel = new Button();
      this.label7 = new Label();
      this.maskedTextPhoneExtension = new MaskedTextBox();
      this.errorProvider1 = new ErrorProvider(this.components);
      this.label9 = new Label();
      this.labelWaiting = new Label();
      this.progressBar1 = new ProgressBar();
      this.labelPercent = new Label();
      this.panelSend = new Panel();
      this.labelProjectInfo = new Label();
      this.label8 = new Label();
      this.labelError = new Label();
      ((ISupportInitialize) this.errorProvider1).BeginInit();
      this.panelSend.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.Name = "label6";
      componentResourceManager.ApplyResources((object) this.textName, "textName");
      this.textName.Name = "textName";
      componentResourceManager.ApplyResources((object) this.textAddress, "textAddress");
      this.textAddress.Name = "textAddress";
      componentResourceManager.ApplyResources((object) this.maskedTextPhone, "maskedTextPhone");
      this.maskedTextPhone.Name = "maskedTextPhone";
      componentResourceManager.ApplyResources((object) this.textEmail1, "textEmail1");
      this.textEmail1.Name = "textEmail1";
      componentResourceManager.ApplyResources((object) this.textEmail2, "textEmail2");
      this.textEmail2.Name = "textEmail2";
      componentResourceManager.ApplyResources((object) this.textNotes, "textNotes");
      this.textNotes.Name = "textNotes";
      componentResourceManager.ApplyResources((object) this.bttnOK, "bttnOK");
      this.bttnOK.Name = "bttnOK";
      this.bttnOK.UseVisualStyleBackColor = true;
      this.bttnOK.Click += new EventHandler(this.bttnOK_Click);
      componentResourceManager.ApplyResources((object) this.bttnCancel, "bttnCancel");
      this.bttnCancel.Name = "bttnCancel";
      this.bttnCancel.UseVisualStyleBackColor = true;
      this.bttnCancel.Click += new EventHandler(this.bttnCancel_Click);
      componentResourceManager.ApplyResources((object) this.label7, "label7");
      this.label7.Name = "label7";
      componentResourceManager.ApplyResources((object) this.maskedTextPhoneExtension, "maskedTextPhoneExtension");
      this.maskedTextPhoneExtension.Name = "maskedTextPhoneExtension";
      this.errorProvider1.ContainerControl = (ContainerControl) this;
      componentResourceManager.ApplyResources((object) this.label9, "label9");
      this.label9.ForeColor = Color.Red;
      this.label9.Name = "label9";
      componentResourceManager.ApplyResources((object) this.labelWaiting, "labelWaiting");
      this.labelWaiting.Name = "labelWaiting";
      componentResourceManager.ApplyResources((object) this.progressBar1, "progressBar1");
      this.progressBar1.Name = "progressBar1";
      componentResourceManager.ApplyResources((object) this.labelPercent, "labelPercent");
      this.labelPercent.Name = "labelPercent";
      this.panelSend.Controls.Add((Control) this.labelProjectInfo);
      this.panelSend.Controls.Add((Control) this.label8);
      this.panelSend.Controls.Add((Control) this.bttnCancel);
      this.panelSend.Controls.Add((Control) this.labelPercent);
      this.panelSend.Controls.Add((Control) this.label1);
      this.panelSend.Controls.Add((Control) this.label2);
      this.panelSend.Controls.Add((Control) this.label3);
      this.panelSend.Controls.Add((Control) this.labelWaiting);
      this.panelSend.Controls.Add((Control) this.label4);
      this.panelSend.Controls.Add((Control) this.label9);
      this.panelSend.Controls.Add((Control) this.label5);
      this.panelSend.Controls.Add((Control) this.maskedTextPhoneExtension);
      this.panelSend.Controls.Add((Control) this.label6);
      this.panelSend.Controls.Add((Control) this.label7);
      this.panelSend.Controls.Add((Control) this.textName);
      this.panelSend.Controls.Add((Control) this.textAddress);
      this.panelSend.Controls.Add((Control) this.bttnOK);
      this.panelSend.Controls.Add((Control) this.maskedTextPhone);
      this.panelSend.Controls.Add((Control) this.textNotes);
      this.panelSend.Controls.Add((Control) this.textEmail1);
      this.panelSend.Controls.Add((Control) this.textEmail2);
      this.panelSend.Controls.Add((Control) this.labelError);
      this.panelSend.Controls.Add((Control) this.progressBar1);
      componentResourceManager.ApplyResources((object) this.panelSend, "panelSend");
      this.panelSend.Name = "panelSend";
      componentResourceManager.ApplyResources((object) this.labelProjectInfo, "labelProjectInfo");
      this.labelProjectInfo.Name = "labelProjectInfo";
      componentResourceManager.ApplyResources((object) this.label8, "label8");
      this.label8.Name = "label8";
      componentResourceManager.ApplyResources((object) this.labelError, "labelError");
      this.labelError.ForeColor = Color.Red;
      this.labelError.Name = "labelError";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelSend);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (frmSendToSAS);
      this.ShowInTaskbar = false;
      this.Load += new EventHandler(this.frmSendToSAS_Load);
      ((ISupportInitialize) this.errorProvider1).EndInit();
      this.panelSend.ResumeLayout(false);
      this.panelSend.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
