// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.MobileResetPasswordForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls.Extensions;
using DaveyTree.Net.Extensions;
using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Properties;
using NHibernate;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class MobileResetPasswordForm : Form
  {
    private TaskManager m_taskmanager;
    private ProgramSession m_ps;
    private ISession m_session;
    private Year m_year;
    private IContainer components;
    private Label lblResetKey;
    private TextBox txtResetKey;
    private Label label5;
    private Label lblPassword;
    private TextBox txtResetPasswd2;
    private TextBox txtResetPasswd1;
    private Label lblPasswordEmail;
    private Button btnCancel;
    private Button btnOK;
    private ErrorProvider ep;

    public MobileResetPasswordForm()
    {
      this.InitializeComponent();
      this.m_ps = Program.Session;
      this.m_taskmanager = new TaskManager(new WaitCursor((Form) this));
      this.Init();
    }

    private void Init()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskmanager.Add(Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t => this.InitUI()), scheduler));
    }

    private void LoadData()
    {
      if (this.m_ps.InputSession == null || !this.m_ps.InputSession.YearKey.HasValue)
        return;
      this.m_session = this.m_ps.InputSession.CreateSession();
      using (this.m_session.BeginTransaction())
        this.m_year = this.m_session.Get<Year>((object) this.m_ps.InputSession.YearKey);
    }

    private void InitUI() => this.lblPasswordEmail.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.MsgPasswordResetTokenSent, (object) this.m_year.MobileEmail);

    private string ResetPassword(string key, string password)
    {
      string requestUriString = Settings.Default.Https + Settings.Default.Host + Settings.Default.MobileResetPasswordUrl + "?key=" + HttpUtility.UrlEncode(this.m_year.MobileKey);
      try
      {
        HttpWebRequest req = (HttpWebRequest) WebRequest.Create(requestUriString);
        string boundary = req.CreateBoundary();
        req.Method = "POST";
        req.UserAgent = Settings.Default.UserAgent;
        req.Referer = Settings.Default.Referer;
        req.ContentType = "multipart/form-data; boundary=" + boundary;
        req.Accept = "*/*";
        req.KeepAlive = true;
        req.Credentials = CredentialCache.DefaultCredentials;
        string str = "--" + boundary;
        Stream requestStream = req.GetRequestStream();
        StreamWriter streamWriter = new StreamWriter(requestStream);
        streamWriter.WriteLine(str);
        streamWriter.WriteLine("Content-Disposition: form-data; name=\"secret\"");
        streamWriter.WriteLine();
        streamWriter.WriteLine(key);
        streamWriter.WriteLine(str);
        streamWriter.WriteLine("Content-Disposition: form-data; name=\"password\"");
        streamWriter.WriteLine();
        streamWriter.WriteLine(password);
        streamWriter.WriteLine(str + "--");
        streamWriter.Close();
        requestStream.Close();
        req.GetResponse().Close();
      }
      catch (WebException ex)
      {
        string str = !(ex.Response is HttpWebResponse response) ? ex.Message : response.StatusDescription;
        response?.Close();
        return str;
      }
      return (string) null;
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.IsValid())
        return;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.Enabled = false;
      this.m_taskmanager.Add(Task.Factory.StartNew<string>((Func<string>) (() => this.ResetPassword(this.txtResetKey.Text, this.txtResetPasswd1.Text)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<string>>) (t =>
      {
        string result = t.Result;
        if (!string.IsNullOrEmpty(result))
        {
          StringBuilder stringBuilder = new StringBuilder(result);
          stringBuilder.Append(Environment.NewLine);
          stringBuilder.Append(Environment.NewLine);
          stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.InternetErrorHelpMessage);
          int num = (int) MessageBox.Show((IWin32Window) this, stringBuilder.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
          this.DialogResult = DialogResult.OK;
        this.Enabled = true;
      }), scheduler));
    }

    private bool IsValid()
    {
      this.ep.SetError((Control) this.txtResetKey, string.IsNullOrEmpty(this.txtResetKey.Text), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) this.lblResetKey));
      bool flag1 = this.txtResetPasswd1.Text.Length > 0;
      bool flag2 = this.txtResetPasswd2.Text.Length > 0;
      if (flag1 & flag2)
      {
        bool flag3 = this.txtResetPasswd1.Text.Equals(this.txtResetPasswd2.Text, StringComparison.CurrentCulture);
        this.ep.SetError((Control) this.txtResetPasswd1, !flag3, i_Tree_Eco_v6.Resources.Strings.ErrPasswordNotMatch);
        this.ep.SetError((Control) this.txtResetPasswd2, !flag3, i_Tree_Eco_v6.Resources.Strings.ErrPasswordNotMatch);
      }
      else
      {
        this.ep.SetError((Control) this.txtResetPasswd1, !flag1, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) i_Tree_Eco_v6.Resources.Strings.Password));
        this.ep.SetError((Control) this.txtResetPasswd2, flag1 && !flag2, i_Tree_Eco_v6.Resources.Strings.ErrConfirmPassword);
      }
      return !this.HasErrors(this.ep);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MobileResetPasswordForm));
      this.lblResetKey = new Label();
      this.txtResetKey = new TextBox();
      this.label5 = new Label();
      this.lblPassword = new Label();
      this.txtResetPasswd2 = new TextBox();
      this.txtResetPasswd1 = new TextBox();
      this.lblPasswordEmail = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.ep = new ErrorProvider(this.components);
      ((ISupportInitialize) this.ep).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblResetKey, "lblResetKey");
      this.lblResetKey.Name = "lblResetKey";
      componentResourceManager.ApplyResources((object) this.txtResetKey, "txtResetKey");
      this.txtResetKey.Name = "txtResetKey";
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.lblPassword, "lblPassword");
      this.lblPassword.Name = "lblPassword";
      componentResourceManager.ApplyResources((object) this.txtResetPasswd2, "txtResetPasswd2");
      this.txtResetPasswd2.Name = "txtResetPasswd2";
      componentResourceManager.ApplyResources((object) this.txtResetPasswd1, "txtResetPasswd1");
      this.txtResetPasswd1.Name = "txtResetPasswd1";
      componentResourceManager.ApplyResources((object) this.lblPasswordEmail, "lblPasswordEmail");
      this.lblPasswordEmail.ForeColor = Color.Blue;
      this.lblPasswordEmail.Name = "lblPasswordEmail";
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
      this.btnOK.Name = "btnOK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.ep.ContainerControl = (ContainerControl) this;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblResetKey);
      this.Controls.Add((Control) this.txtResetKey);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.lblPassword);
      this.Controls.Add((Control) this.txtResetPasswd2);
      this.Controls.Add((Control) this.txtResetPasswd1);
      this.Controls.Add((Control) this.lblPasswordEmail);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MobileResetPasswordForm);
      this.ShowInTaskbar = false;
      ((ISupportInitialize) this.ep).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
