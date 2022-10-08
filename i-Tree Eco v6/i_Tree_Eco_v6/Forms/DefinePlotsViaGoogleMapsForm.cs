// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.DefinePlotsViaGoogleMapsForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using CefSharp;
using CefSharp.WinForms;
using Eco.Domain.v6;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Properties;
using i_Tree_Eco_v6.Resources;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class DefinePlotsViaGoogleMapsForm : ContentForm
  {
    private ISession m_session;
    private ProgramSession m_ps;
    private Year year;
    private List<Plot> plots;
    private List<Strata> stratas;
    private int plotsAdded;
    private Control _browser;
    private IContainer components;
    private Label label1;
    private TableLayoutPanel tableLayoutPanel1;

    public DefinePlotsViaGoogleMapsForm()
    {
      this.InitializeComponent();
      this.m_ps = Program.Session;
      this.m_session = this.m_ps.InputSession.CreateSession();
      this.year = this.m_session.Get<Year>((object) Program.Session.InputSession.YearKey);
      this._browser = (Control) new ChromiumWebBrowser(string.Format(Settings.Default.PlotSamplingUrl, (object) (char) this.year.Unit, (object) "true"));
      this._browser.Dock = DockStyle.Fill;
      this.Browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(this.Browser_FrameLoadEnd);
      this.Browser.JavascriptMessageReceived += new EventHandler<JavascriptMessageReceivedEventArgs>(this.Browser_JavascriptMessageReceived);
      this.tableLayoutPanel1.Controls.Add(this._browser, 0, 1);
    }

    private IWebBrowser Browser => this._browser as IWebBrowser;

    private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
    {
      if (!e.Frame.IsMain || !e.Frame.Url.Contains("sampling.cfm"))
        return;
      this.Browser.ExecuteScriptAsync("\r\n                    (function(){\r\n                        document.getElementById('generateplots')\r\n                            .addEventListener('click', function() {\r\n                                var message = {\r\n                                    action:'click',\r\n                                    source:this.id,\r\n                                };\r\n\r\n                                CefSharp.PostMessage(message);\r\n                            });\r\n\r\n                        var inputs = document.getElementsByTagName('input');\r\n\r\n                        for (var i in inputs) {\r\n                            var input = inputs[i];\r\n\r\n                            input.addEventListener('input', function() {\r\n                                var message = {\r\n                                    action:'input',\r\n                                    source:this.id,\r\n                                };\r\n\r\n                                CefSharp.PostMessage(message);\r\n                            });\r\n                        }\r\n                    })();\r\n                ");
    }

    private void Browser_JavascriptMessageReceived(
      object sender,
      JavascriptMessageReceivedEventArgs e)
    {
      if (!(e.Message is IDictionary<string, object> message))
        return;
      string str = message["action"].ToString();
      if (!(str == "click"))
      {
        if (!(str == "input"))
          return;
        this.m_isDirty = true;
      }
      else
        this.Invoke((Delegate) new MethodInvoker(this.OnSubmitPlots));
    }

    protected virtual async void OnSubmitPlots()
    {
      DefinePlotsViaGoogleMapsForm owner = this;
      IList<DefinePlotsViaGoogleMapsForm.SamplingResult> results;
      try
      {
        string script = "\r\n                    (function(){\r\n                        var valid = vb_Validation();\r\n                        var rv = 0;\r\n                    \r\n                        if (!valid) {\r\n                            var el = document.getElementById('hiddenexchange');\r\n                            var result = el.innerText;\r\n                        \r\n                            switch (result) {\r\n                                case 'No plots sampled':\r\n                                    rv = 1;\r\n                                    break;\r\n                                case 'Sampling is incomplete':\r\n                                    rv = 2;\r\n                                    break;\r\n                            }\r\n                        }\r\n\r\n                        return rv;\r\n                    })();\r\n                ";
        JavascriptResponse scriptAsync = await owner.Browser.EvaluateScriptAsync(script, new TimeSpan?(), false);
        if (!scriptAsync.Success)
        {
          results = (IList<DefinePlotsViaGoogleMapsForm.SamplingResult>) null;
          return;
        }
        switch ((int) scriptAsync.Result)
        {
          case 1:
            int num = (int) MessageBox.Show((IWin32Window) owner, Strings.MsgNoPlotsSampled, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            results = (IList<DefinePlotsViaGoogleMapsForm.SamplingResult>) null;
            return;
          case 2:
            if (MessageBox.Show((IWin32Window) owner, Strings.MsgSamplingIncomplete, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
              results = (IList<DefinePlotsViaGoogleMapsForm.SamplingResult>) null;
              return;
            }
            break;
        }
      }
      catch (ArgumentOutOfRangeException ex)
      {
      }
      float plot_size = 0.0404686f;
      try
      {
        JavascriptResponse scriptAsync = await owner.Browser.EvaluateScriptAsync("getPlotSize()", new TimeSpan?(), false);
        if (scriptAsync.Success)
        {
          string result = (string) scriptAsync.Result;
          if (result != null)
            float.TryParse(result, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out plot_size);
        }
      }
      catch (ArgumentOutOfRangeException ex)
      {
      }
      if (owner.year.Unit == YearUnit.English)
        plot_size *= 2.47105f;
      owner.stratas = owner.year.Strata.ToList<Strata>();
      owner.plots = new List<Plot>();
      results = (IList<DefinePlotsViaGoogleMapsForm.SamplingResult>) new List<DefinePlotsViaGoogleMapsForm.SamplingResult>();
      try
      {
        string script = "\r\n                    (function(){\r\n                        return JSON.stringify(SamplingResults);\r\n                    })();\r\n                ";
        JavascriptResponse scriptAsync = await owner.Browser.EvaluateScriptAsync(script, new TimeSpan?(), false);
        if (!scriptAsync.Success)
        {
          results = (IList<DefinePlotsViaGoogleMapsForm.SamplingResult>) null;
          return;
        }
        string result = (string) scriptAsync.Result;
        if (result != null)
          results = JsonConvert.DeserializeObject<IList<DefinePlotsViaGoogleMapsForm.SamplingResult>>(result);
      }
      catch (ArgumentOutOfRangeException ex)
      {
      }
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      foreach (Strata strata in owner.stratas)
      {
        if (!stringList1.Contains(strata.Description))
          stringList1.Add(strata.Description);
        if (!stringList2.Contains(strata.Abbreviation))
          stringList2.Add(strata.Abbreviation);
      }
      int strataId = owner.GenerateStrataId();
      for (int index1 = 0; index1 < results.Count; ++index1)
      {
        DefinePlotsViaGoogleMapsForm.SamplingResult samplingResult = results[index1];
        string stratum = samplingResult.Stratum;
        string str1 = stratum.Substring(0, Math.Min(stratum.Length, 8));
        Strata strata1 = (Strata) null;
        if (stringList1.Contains(stratum) || stringList2.Contains(str1))
        {
          string str2 = stratum;
          string str3 = str1;
          int num1 = 1;
          int num2 = 1;
          int index2 = 0;
          while (index2 < owner.stratas.Count)
          {
            Strata strata2 = owner.stratas[index2];
            if (strata2.Description == str2)
            {
              if (strata2.Plots.Count > 0)
              {
                str2 = stratum + (object) num1++;
                index2 = 0;
              }
              else if (stringList2.Contains(str3) && strata2.Abbreviation != str3)
              {
                str2 = stratum + (object) num1++;
                string str4 = num2.ToString("D");
                str3 = stratum.Substring(0, Math.Min(stratum.Length, 8 - str4.Length)) + str4;
                ++num2;
                index2 = 0;
              }
              else
              {
                strata1 = strata2;
                break;
              }
            }
            else if (strata2.Abbreviation == str3)
            {
              if (strata2.Plots.Count > 0)
              {
                string str5 = num2.ToString("D");
                str3 = stratum.Substring(0, Math.Min(stratum.Length, 8 - str5.Length)) + str5;
                ++num2;
                index2 = 0;
              }
              else if (stringList1.Contains(str2) && strata2.Description != str2)
              {
                str2 = stratum + (object) num1++;
                string str6 = num2.ToString("D");
                str3 = stratum.Substring(0, Math.Min(stratum.Length, 8 - str6.Length)) + str6;
                ++num2;
                index2 = 0;
              }
              else
              {
                strata1 = strata2;
                break;
              }
            }
            else
              ++index2;
          }
          if (strata1 == null)
          {
            strata1 = new Strata()
            {
              Id = strataId++,
              Year = owner.year,
              Description = str2,
              Abbreviation = str3
            };
            owner.year.Strata.Add(strata1);
          }
          else
          {
            stringList1.Remove(strata1.Description);
            stringList2.Remove(strata1.Abbreviation);
            stringList1.Add(str2);
            stringList2.Add(str3);
            strata1.Description = str2;
            strata1.Abbreviation = str3;
          }
        }
        else
        {
          strata1 = new Strata()
          {
            Id = strataId++,
            Year = owner.year,
            Description = stratum,
            Abbreviation = str1
          };
          stringList1.Add(stratum);
          stringList2.Add(str1);
          owner.year.Strata.Add(strata1);
        }
        strata1.Size = samplingResult.StratumArea;
        if (owner.year.Unit == YearUnit.English)
          strata1.Size *= 2.47105f;
        for (int index3 = 0; index3 < samplingResult.Plots.Count; ++index3)
        {
          DefinePlotsViaGoogleMapsForm.Coord plot1 = samplingResult.Plots[index3];
          Plot plot2 = new Plot()
          {
            Size = plot_size,
            Year = owner.year,
            Strata = strata1
          };
          plot2.Id = index1 != 0 || index3 != 0 ? owner.plots[owner.plots.Count - 1].Id + 1 : owner.GeneratePlotId();
          owner.year.Plots.Add(plot2);
          plot2.Latitude = new double?(plot1.Latitude);
          plot2.Longitude = new double?(plot1.Longitude);
          owner.plots.Add(plot2);
        }
        owner.plotsAdded = owner.plots.Count;
        if (MessageBox.Show((IWin32Window) owner, string.Format(Strings.MsgConfirmAddPlots, (object) owner.plotsAdded), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          owner.SaveAll();
        else
          owner.m_session.Refresh((object) owner.year);
      }
      results = (IList<DefinePlotsViaGoogleMapsForm.SamplingResult>) null;
    }

    private int GeneratePlotId() => this.m_session.QueryOver<Plot>().Select((IProjection) Projections.Max<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id))).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.year)).SingleOrDefault<int>() + 1;

    private int GenerateStrataId() => this.m_session.QueryOver<Strata>().Select((IProjection) Projections.Max<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => (object) s.Id))).Where((System.Linq.Expressions.Expression<Func<Strata, bool>>) (s => s.Year == this.year)).SingleOrDefault<int>() + 1;

    private void SaveAll()
    {
      if (this.m_session != null)
      {
        int count = this.year.Plots.Count;
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.SaveOrUpdate((object) this.year);
          transaction.Commit();
        }
        EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(this.year), (Control) this);
        this.m_isDirty = false;
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format(Strings.MsgPlotsCreated, (object) this.plotsAdded), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.Close();
      }
      else
      {
        this.m_session = this.m_ps.InputSession.CreateSession();
        this.SaveAll();
      }
    }

    private void DefinePlotsViaGoogleMapsForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.m_session.Close();
      this.m_session.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DefinePlotsViaGoogleMapsForm));
      this.label1 = new Label();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.label1, 0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Name = nameof (DefinePlotsViaGoogleMapsForm);
      this.FormClosed += new FormClosedEventHandler(this.DefinePlotsViaGoogleMapsForm_FormClosed);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel1, 0);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private class SamplingResult
    {
      [JsonProperty("Name")]
      public string Stratum { get; set; }

      [JsonProperty("Area")]
      public float StratumArea { get; set; }

      public IList<DefinePlotsViaGoogleMapsForm.Coord> Plots { get; set; }
    }

    private class Coord
    {
      [JsonProperty("Lat")]
      public double Latitude { get; set; }

      [JsonProperty("Lng")]
      public double Longitude { get; set; }
    }
  }
}
