// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.AboutForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using CefSharp;
using CefSharp.WinForms;
using DaveyTree.Drawing.Extensions;
using i_Tree_Eco_v6.Resources;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class AboutForm : ContentForm
  {
    private string eco_version_number = new Version(Application.ProductVersion).ToString(3);
    private string itree_version_number = Strings.iTreeNotInstalled;
    private Control _browser;
    private IContainer components;

    public AboutForm()
    {
      this.InitializeComponent();
      this._browser = (Control) new ChromiumWebBrowser("about:blank");
      this._browser.Dock = DockStyle.Fill;
      this.Controls.Add(this._browser);
      this.getITreeVersionNumber();
      HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
      htmlDocument.LoadHtml(i_Tree_Eco_v6.Properties.Resources.AboutTemplate);
      htmlDocument.GetElementbyId("eco_version").InnerHtml = this.eco_version_number;
      htmlDocument.GetElementbyId("itree_version").InnerHtml = this.itree_version_number;
      htmlDocument.GetElementbyId("twitter-logo").Attributes.Add("src", i_Tree_Eco_v6.Properties.Resources.twitter_logo.ToUriData());
      htmlDocument.GetElementbyId("fs-logo").Attributes.Add("src", i_Tree_Eco_v6.Properties.Resources.forest_service.ToUriData());
      htmlDocument.GetElementbyId("davey-logo").Attributes.Add("src", i_Tree_Eco_v6.Properties.Resources.davey_logo.ToUriData());
      htmlDocument.GetElementbyId("ad-logo").Attributes.Add("src", i_Tree_Eco_v6.Properties.Resources.arbor_day_foundation.ToUriData());
      htmlDocument.GetElementbyId("sma-logo").Attributes.Add("src", i_Tree_Eco_v6.Properties.Resources.sma_logo.ToUriData());
      htmlDocument.GetElementbyId("isa-logo").Attributes.Add("src", i_Tree_Eco_v6.Properties.Resources.isa_logo.ToUriData());
      htmlDocument.GetElementbyId("casey-logo").Attributes.Add("src", i_Tree_Eco_v6.Properties.Resources.casey_trees.ToUriData());
      htmlDocument.GetElementbyId("esf-logo").Attributes.Add("src", i_Tree_Eco_v6.Properties.Resources.esf.ToUriData());
      this.Browser.LifeSpanHandler = (ILifeSpanHandler) new LifeSpanHandler();
      this.Browser.LoadHtml(htmlDocument.DocumentNode.OuterHtml, "http://www.itreetools.org");
    }

    private IWebBrowser Browser => this._browser as IWebBrowser;

    private string Url => "http://wwww.itreetools.org";

    private void getITreeVersionNumber()
    {
      RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("Software\\i-Tree\\v3\\", false);
      if (registryKey == null)
        return;
      this.itree_version_number = registryKey.GetValue("Version").ToString();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AboutForm));
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.BackColor = System.Drawing.Color.White;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Name = nameof (AboutForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.ResumeLayout(false);
    }
  }
}
