// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.MobileForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Net.Extensions;
using Eco.Domain.DTO.v6;
using Eco.Domain.v6;
using i_Tree_Eco_v6.Properties;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Xml;

namespace i_Tree_Eco_v6.Forms
{
  public class MobileForm : ContentForm
  {
    protected ErrorProvider ep;
    private IContainer components;

    public MobileForm() => this.InitializeComponent();

    protected string RequestPwdReset(ISession s, Year y)
    {
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Refresh((object) y);
        transaction.Commit();
      }
      string requestUriString = Settings.Default.Https + Settings.Default.Host + Settings.Default.MobileResetPasswordUrl + "?key=" + HttpUtility.UrlEncode(y.MobileKey);
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.Method = "GET";
        httpWebRequest.UserAgent = Settings.Default.UserAgent;
        httpWebRequest.Referer = Settings.Default.Referer;
        httpWebRequest.KeepAlive = true;
        httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
        httpWebRequest.GetResponse().Close();
      }
      catch (WebException ex)
      {
        string str = !(ex.Response is HttpWebResponse response) ? ex.Message : response.StatusDescription;
        response?.Close();
        return str;
      }
      return (string) null;
    }

    protected int UploadData(ISession s, Year y, string pwd, IEnumerable<Plot> plots)
    {
      int num = 0;
      YearDTO dto;
      using (s.BeginTransaction())
      {
        dto = y.GetDTO();
        dto.Plots = new List<PlotDTO>();
        foreach (Plot plot in plots)
          dto.Plots.Add(plot.GetDTO());
      }
      string str1 = JsonConvert.SerializeObject((object) dto, new JsonSerializerSettings()
      {
        NullValueHandling = NullValueHandling.Ignore
      });
      string requestUriString = Settings.Default.Https + Settings.Default.Host + Settings.Default.MobileUploadUrl;
      if (!string.IsNullOrEmpty(y.MobileKey))
        requestUriString = string.Format("{0}?key={1}", new object[2]
        {
          (object) requestUriString,
          (object) HttpUtility.UrlEncode(y.MobileKey)
        });
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
        string str2 = "--" + boundary;
        Stream requestStream = req.GetRequestStream();
        StreamWriter streamWriter = new StreamWriter(requestStream);
        streamWriter.WriteLine(str2);
        streamWriter.WriteLine("Content-Disposition: file; name=\"config\"; filename=\"config.json\"");
        streamWriter.WriteLine("Content-Type: application/json");
        streamWriter.WriteLine();
        streamWriter.Flush();
        streamWriter.WriteLine(str1);
        streamWriter.WriteLine(str2);
        streamWriter.WriteLine("Content-Disposition: form-data; name=\"projectname\"");
        streamWriter.WriteLine();
        streamWriter.WriteLine(string.Format("{0}_{1}_{2}", (object) dto.Series.Project.Name, (object) dto.Series.Id, (object) dto.Id));
        streamWriter.WriteLine(str2);
        streamWriter.WriteLine("Content-Disposition: form-data; name=\"email\"");
        streamWriter.WriteLine();
        streamWriter.WriteLine(y.MobileEmail);
        streamWriter.WriteLine(str2);
        streamWriter.WriteLine("Content-Disposition: form-data; name=\"password\"");
        streamWriter.WriteLine();
        streamWriter.WriteLine(pwd);
        streamWriter.WriteLine(str2);
        streamWriter.WriteLine("Content-Disposition: form-data; name=\"appurl\"");
        streamWriter.WriteLine();
        streamWriter.WriteLine(Settings.Default.Https + Settings.Default.Host + Settings.Default.MobileAppUrl);
        streamWriter.WriteLine(str2 + "--");
        streamWriter.Close();
        requestStream.Close();
        HttpWebResponse response = (HttpWebResponse) req.GetResponse();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(response.GetResponseStream());
        response.Close();
        XmlNode xmlNode1 = xmlDocument.GetElementsByTagName("ResultMessage")[0];
        if (xmlNode1.InnerText == "SUCCESS")
        {
          XmlNode xmlNode2 = xmlDocument.GetElementsByTagName("Key").Item(0);
          if (string.IsNullOrEmpty(y.MobileKey))
            y.MobileKey = xmlNode2.InnerText;
        }
        else
          num = !xmlNode1.InnerText.Contains("password not match") ? -1 : 1;
      }
      catch (WebException ex)
      {
        string message = !(ex.Response is HttpWebResponse response) ? ex.Message : response.StatusDescription;
        response?.Close();
        WebException innerException = ex;
        throw new UploadException(message, (Exception) innerException);
      }
      return num;
    }

    protected bool EmailValid(string email)
    {
      bool flag = true;
      if (email == null)
      {
        flag = false;
      }
      else
      {
        int length = email.LastIndexOf("@");
        if (length == -1)
        {
          flag = false;
        }
        else
        {
          string input1 = email.Substring(length + 1);
          string input2 = email.Substring(0, length);
          if (input2.Length < 1 || input2.Length > 64)
            flag = false;
          else if (input1.Length < 1 || input1.Length > (int) byte.MaxValue)
            flag = false;
          else if (input2.StartsWith(".") || input2.EndsWith("."))
            flag = false;
          else if (Regex.IsMatch(input2, "\\.\\."))
            flag = false;
          else if (!Regex.IsMatch(input1, "^[A-Za-z0-9\\-\\.]+$"))
            flag = false;
          else if (!Regex.IsMatch(input2.Replace("\\\\", string.Empty), "^(\\\\\\\\.|[A-Za-z0-9!#%&`_=/$\\\\'*+?^{}|~.-])+$") && !Regex.IsMatch(input2.Replace("\\\\", string.Empty), "^\"(\\\\\\\\\"|[^\"])+\"$"))
            flag = false;
        }
      }
      return flag;
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.ep = new ErrorProvider(this.components);
      ((ISupportInitialize) this.ep).BeginInit();
      this.SuspendLayout();
      this.ep.ContainerControl = (ContainerControl) this;
      this.ClientSize = new Size(486, 261);
      this.Name = nameof (MobileForm);
      ((ISupportInitialize) this.ep).EndInit();
      this.ResumeLayout(false);
    }
  }
}
