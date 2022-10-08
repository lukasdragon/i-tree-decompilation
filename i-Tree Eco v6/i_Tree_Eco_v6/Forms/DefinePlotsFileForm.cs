// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.DefinePlotsFileForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls.Extensions;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Properties;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class DefinePlotsFileForm : ContentForm
  {
    private ProgramSession _ps;
    private Year _year;
    private string _fnPlots;
    private string _fnStrata;
    private string _fnProjection;
    private IContainer components;
    private TableLayoutPanel definePlotsFromFilePanel;
    private Label plotListLbl;
    private Label strataFileLbl;
    private Label gisFileLbl;
    private TextBox txtPlots;
    private TextBox txtStrata;
    private TextBox txtProjection;
    private Button plotListBtn;
    private Button strataFileBtn;
    private Button gisFileBtn;
    private Button btnImport;
    private Label label1;

    public DefinePlotsFileForm()
    {
      this.InitializeComponent();
      this._ps = Program.Session;
      using (ISession session = this._ps.InputSession.CreateSession())
      {
        using (session.BeginTransaction())
          this._year = session.Get<Year>((object) this._ps.InputSession.YearKey);
      }
    }

    private void plotListBtn_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Multiselect = false;
        openFileDialog.Filter = string.Join("|", new string[2]
        {
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterText, (object) string.Join(";", Settings.Default.ExtText)),
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
        });
        openFileDialog.CheckFileExists = true;
        openFileDialog.Title = i_Tree_Eco_v6.Resources.Strings.SelectPlotListFile;
        openFileDialog.ShowHelp = false;
        if (openFileDialog.ShowDialog() != DialogResult.OK)
          return;
        this.txtPlots.Text = openFileDialog.FileName;
        this._fnPlots = openFileDialog.FileName;
      }
    }

    private void strataFileBtn_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Multiselect = false;
        openFileDialog.Filter = string.Join("|", new string[2]
        {
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterText, (object) string.Join(";", Settings.Default.ExtText)),
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
        });
        openFileDialog.CheckFileExists = true;
        openFileDialog.Title = i_Tree_Eco_v6.Resources.Strings.SelectStrataFile;
        openFileDialog.ShowHelp = false;
        if (openFileDialog.ShowDialog() != DialogResult.OK)
          return;
        this.txtStrata.Text = openFileDialog.FileName;
        this._fnStrata = openFileDialog.FileName;
      }
    }

    private void gisFileBtn_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Multiselect = false;
        openFileDialog.Filter = string.Join("|", new string[2]
        {
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterGISProj, (object) string.Join(";", Settings.Default.ExtGISProj)),
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFiles, (object) string.Join(";", Settings.Default.ExtAllFiles))
        });
        openFileDialog.CheckFileExists = true;
        openFileDialog.Title = i_Tree_Eco_v6.Resources.Strings.SelectGISProjectionFile;
        openFileDialog.ShowHelp = false;
        if (openFileDialog.ShowDialog() != DialogResult.OK)
          return;
        this.txtProjection.Text = openFileDialog.FileName;
        this._fnProjection = openFileDialog.FileName;
      }
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      bool delete = false;
      using (ISession session = this._ps.InputSession.CreateSession())
      {
        if (session.Get<Year>((object) this._ps.InputSession.YearKey).Plots.Count > 0)
        {
          ExistingPlotsForm existingPlotsForm = new ExistingPlotsForm();
          if (existingPlotsForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          delete = existingPlotsForm.ImportAction == PlotImportActionEnum.Delete;
        }
        else
          delete = true;
      }
      Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
      CancellationTokenSource cts = new CancellationTokenSource();
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      new ProcessingForm(Task.Factory.StartNew<int>((Func<int>) (() => this.ImportPlots(delete, (IProgress<ProgressEventArgs>) progress, cts)), cts.Token, TaskCreationOptions.None, this._ps.Scheduler).ContinueWith((System.Action<Task<int>>) (task =>
      {
        if (task.IsFaulted)
        {
          int num1 = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrUnexpected, (object) task.Exception.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int num2 = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.MsgImportSuccess, (object) task.Result, (object) v6Strings.Plot_PluralName), Application.ProductName, MessageBoxButtons.OK);
          this.txtProjection.Text = string.Empty;
          this.txtPlots.Text = string.Empty;
          this.txtStrata.Text = string.Empty;
        }
      }), scheduler), progress, cts).ShowWithParentFormLock((Form) this);
    }

    private int ImportPlots(
      bool delete,
      IProgress<ProgressEventArgs> progress,
      CancellationTokenSource cts)
    {
      List<Strata> lstStrata = this.processStrataFile(progress, cts.Token);
      List<Plot> plotList1 = this.processPlotFile(lstStrata, progress, cts.Token);
      string str = this.processGISFile();
      using (ISession session = this._ps.InputSession.CreateSession())
      {
        if (delete)
        {
          using (ITransaction transaction = session.BeginTransaction())
          {
            Year entity = session.Load<Year>((object) this._ps.InputSession.YearKey);
            List<Strata> strataList = new List<Strata>((IEnumerable<Strata>) entity.Strata);
            for (int index = 0; index < strataList.Count; ++index)
            {
              Strata strata = strataList[index];
              session.Delete((object) strata);
              entity.Strata.Remove(strata);
              progress.Report(new ProgressEventArgs(string.Format(i_Tree_Eco_v6.Resources.Strings.MsgDeleting, (object) v6Strings.Plot_PluralName), strataList.Count, index));
            }
            try
            {
              transaction.Commit();
              EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(entity), (Control) this);
            }
            catch (HibernateException ex)
            {
              transaction.Rollback();
              throw;
            }
          }
        }
        using (ITransaction transaction = session.BeginTransaction())
        {
          Year entity = session.Get<Year>((object) this._ps.InputSession.YearKey);
          List<Strata> strataList1 = new List<Strata>((IEnumerable<Strata>) entity.Strata);
          List<Strata> strataList2 = new List<Strata>((IEnumerable<Strata>) entity.Strata);
          List<Strata> strataList3 = new List<Strata>((IEnumerable<Strata>) entity.Strata);
          List<Plot> plotList2 = new List<Plot>((IEnumerable<Plot>) entity.Plots);
          strataList1.Sort((IComparer<Strata>) new PropertyComparer<Strata>((Func<Strata, object>) (st => (object) st.Id)));
          strataList2.Sort((IComparer<Strata>) new PropertyComparer<Strata>((Func<Strata, object>) (st => (object) st.Description)));
          strataList3.Sort((IComparer<Strata>) new PropertyComparer<Strata>((Func<Strata, object>) (st => (object) st.Abbreviation)));
          plotList2.Sort((IComparer<Plot>) new PropertyComparer<Plot>((Func<Plot, object>) (p => (object) p.Id)));
          foreach (Plot plot in plotList1)
          {
            int num1 = 0;
            int num2;
            progress.Report(new ProgressEventArgs(string.Format(i_Tree_Eco_v6.Resources.Strings.MsgValidating, (object) v6Strings.Plot_PluralName), plotList1.Count, num2 = num1 + 1));
            int index;
            for (index = 0; index < plotList2.Count; ++index)
            {
              int num3 = plotList2[index].Id;
              int num4 = num3.CompareTo(plot.Id);
              if (num4 == 0)
                num3 = ++plot.Id;
              else if (num4 > 0)
                break;
            }
            cts.Token.ThrowIfCancellationRequested();
            plotList2.Insert(index, plot);
          }
          foreach (Strata strata in lstStrata)
          {
            int num5 = 0;
            int num6;
            progress.Report(new ProgressEventArgs(string.Format(i_Tree_Eco_v6.Resources.Strings.MsgValidating, (object) v6Strings.Strata_PluralName), plotList1.Count, num6 = num5 + 1));
            int index1;
            for (index1 = 0; index1 < strataList1.Count; ++index1)
            {
              int num7 = strataList1[index1].Id;
              int num8 = num7.CompareTo(strata.Id);
              if (num8 == 0)
                num7 = ++strata.Id;
              else if (num8 > 0)
                break;
            }
            cts.Token.ThrowIfCancellationRequested();
            strataList1.Insert(index1, strata);
            int num9 = 1;
            string strB1 = strata.Description;
            int index2 = 0;
            while (index2 < strataList2.Count)
            {
              int num10 = strataList2[index2].Description.CompareTo(strB1);
              if (num10 == 0)
              {
                strB1 = strata.Description.Substring(0, Math.Min(strata.Description.Length, 30 - num9.ToString().Length)) + num9.ToString();
                ++num9;
                index2 = 0;
              }
              else
              {
                if (num10 > 0)
                {
                  strata.Description = strB1;
                  break;
                }
                ++index2;
              }
            }
            cts.Token.ThrowIfCancellationRequested();
            strataList2.Insert(index2, strata);
            int num11 = 1;
            string strB2 = strata.Abbreviation;
            int index3 = 0;
            while (index3 < strataList3.Count)
            {
              int num12 = strataList3[index3].Abbreviation.CompareTo(strB2);
              if (num12 == 0)
              {
                strB2 = strata.Abbreviation.Substring(0, Math.Min(strata.Abbreviation.Length, 8 - num11.ToString().Length)) + num11.ToString();
                ++num11;
                index3 = 0;
              }
              else
              {
                if (num12 > 0)
                {
                  strata.Abbreviation = strB2;
                  break;
                }
                ++index3;
              }
            }
            cts.Token.ThrowIfCancellationRequested();
            strataList3.Insert(index3, strata);
            session.Flush();
            session.SaveOrUpdate((object) strata);
          }
          entity.Series.GISProjection = str;
          session.SaveOrUpdate((object) entity);
          try
          {
            transaction.Commit();
            EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(entity), (Control) this);
          }
          catch (HibernateException ex)
          {
            transaction.Rollback();
            throw;
          }
        }
        return plotList1.Count;
      }
    }

    private List<Plot> processPlotFile(
      List<Strata> lstStrata,
      IProgress<ProgressEventArgs> progress,
      CancellationToken token)
    {
      List<Plot> plotList = new List<Plot>();
      using (FileStream fileStream = new FileStream(this._fnPlots, FileMode.Open, FileAccess.Read))
      {
        using (StreamReader streamReader = new StreamReader((Stream) fileStream))
        {
          string str = streamReader.ReadLine();
          float num = 0.1f;
          progress.Report(new ProgressEventArgs(string.Format(i_Tree_Eco_v6.Resources.Strings.MsgReading, (object) v6Strings.Plot_PluralName), (int) fileStream.Length, (int) fileStream.Position));
          if (str == null || !str.Contains("$U4PLLS!") && !str.Contains("$ U4PLLS!"))
            throw new FileFormatException(new Uri(this._fnPlots), i_Tree_Eco_v6.Resources.Strings.MsgInvalidPlotFile);
          string s1 = streamReader.ReadLine();
          if (s1 == null || !int.TryParse(s1, out int _))
            throw new FileFormatException(new Uri(this._fnPlots), i_Tree_Eco_v6.Resources.Strings.MsgInvalidPlotFile);
          if (this._year.Unit == YearUnit.Metric)
            num = 0.04047f;
          Regex regex = new Regex("^(\\d+)\\s+(\\d+)\\s+([+-]?\\d+(?:\\.\\d+)?)\\s+([+-]?\\d+(?:\\.\\d+)?)\\s*$");
          string input;
          while ((input = streamReader.ReadLine()) != null)
          {
            Match match = regex.Match(input);
            token.ThrowIfCancellationRequested();
            if (match.Success)
            {
              Plot plot1 = new Plot();
              int result1;
              double result2;
              double result3;
              int strataId;
              if (int.TryParse(match.Groups[1].Value, out result1) && int.TryParse(match.Groups[2].Value, out strataId) && double.TryParse(match.Groups[3].Value, out result2) && double.TryParse(match.Groups[4].Value, out result3))
              {
                Strata strata = lstStrata.Find((Predicate<Strata>) (s => s.Id == strataId));
                if (strata != null)
                {
                  Plot plot2 = new Plot()
                  {
                    Id = result1,
                    Size = num,
                    Strata = strata,
                    Latitude = new double?(result3),
                    Longitude = new double?(result2),
                    Year = this._year
                  };
                  strata.Plots.Add(plot2);
                  plotList.Add(plot2);
                }
                else
                  continue;
              }
              else
                continue;
            }
            progress.Report(new ProgressEventArgs(string.Format(i_Tree_Eco_v6.Resources.Strings.MsgReading, (object) v6Strings.Plot_PluralName), (int) fileStream.Length, (int) fileStream.Position));
          }
        }
      }
      return plotList;
    }

    private List<Strata> processStrataFile(
      IProgress<ProgressEventArgs> progress,
      CancellationToken token)
    {
      List<Strata> strataList = new List<Strata>();
      using (FileStream fileStream = new FileStream(this._fnStrata, FileMode.Open, FileAccess.Read))
      {
        using (StreamReader streamReader = new StreamReader((Stream) fileStream))
        {
          string str1 = streamReader.ReadLine();
          float num = 1f;
          progress.Report(new ProgressEventArgs(string.Format(i_Tree_Eco_v6.Resources.Strings.MsgReading, (object) v6Strings.Strata_PluralName), (int) fileStream.Length, (int) fileStream.Position));
          if (str1 == null || !str1.Contains("$U4STAR!") && !str1.Contains("$ U4STAR!"))
            throw new FileFormatException(new Uri(this._fnStrata), i_Tree_Eco_v6.Resources.Strings.MsgInvalidStrataFile);
          string s = streamReader.ReadLine();
          int result1;
          if (s == null || !int.TryParse(s, out result1))
            throw new FileFormatException(new Uri(this._fnStrata), i_Tree_Eco_v6.Resources.Strings.MsgInvalidStrataFile);
          if (this._year.Unit == YearUnit.Metric && result1 == 0)
            num = 0.404686f;
          else if (this._year.Unit == YearUnit.English && result1 == 1)
            num = 2.47105f;
          Regex regex1 = new Regex("^(\\d+)\\s+(\\d+(?:\\.\\d+)?)\\s+([^\\s\"].*?|\"(?:[^\"]|\"\")+\")\\s*$");
          Regex regex2 = new Regex("^\"((?:[^\"]|\"\")+)\"$");
          string input1;
          while ((input1 = streamReader.ReadLine()) != null)
          {
            Match match1 = regex1.Match(input1);
            token.ThrowIfCancellationRequested();
            if (match1.Success)
            {
              int result2;
              float result3;
              if (int.TryParse(match1.Groups[1].Value, out result2) && float.TryParse(match1.Groups[2].Value, out result3))
              {
                string input2 = match1.Groups[3].Value;
                Match match2 = regex2.Match(input2);
                if (match2.Success)
                  input2 = match2.Groups[1].Value.Replace("\"\"", "\"");
                string str2 = input2.Substring(0, Math.Min(input2.Length, 30));
                string str3 = str2.Substring(0, Math.Min(str2.Length, 8));
                Strata strata = new Strata()
                {
                  Id = result2,
                  Size = result3 * num,
                  Description = str2,
                  Abbreviation = str3,
                  Year = this._year
                };
                strataList.Add(strata);
              }
              else
                continue;
            }
            progress.Report(new ProgressEventArgs(string.Format(i_Tree_Eco_v6.Resources.Strings.MsgReading, (object) v6Strings.Strata_PluralName), (int) fileStream.Length, (int) fileStream.Position));
          }
        }
      }
      return strataList;
    }

    private string processGISFile()
    {
      using (FileStream fileStream = new FileStream(this.txtProjection.Text, FileMode.Open, FileAccess.Read))
      {
        using (StreamReader streamReader = new StreamReader((Stream) fileStream))
          return streamReader.ReadToEnd();
      }
    }

    private void txtFile_TextChanged(object sender, EventArgs e) => this.btnImport.Enabled = this.txtPlots.Text.Length != 0 && this.txtStrata.Text.Length != 0 && this.txtProjection.Text.Length != 0;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DefinePlotsFileForm));
      this.definePlotsFromFilePanel = new TableLayoutPanel();
      this.plotListLbl = new Label();
      this.strataFileLbl = new Label();
      this.gisFileLbl = new Label();
      this.txtPlots = new TextBox();
      this.txtStrata = new TextBox();
      this.txtProjection = new TextBox();
      this.plotListBtn = new Button();
      this.strataFileBtn = new Button();
      this.gisFileBtn = new Button();
      this.btnImport = new Button();
      this.label1 = new Label();
      this.definePlotsFromFilePanel.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.definePlotsFromFilePanel, "definePlotsFromFilePanel");
      this.definePlotsFromFilePanel.Controls.Add((Control) this.plotListLbl, 0, 1);
      this.definePlotsFromFilePanel.Controls.Add((Control) this.strataFileLbl, 0, 2);
      this.definePlotsFromFilePanel.Controls.Add((Control) this.gisFileLbl, 0, 3);
      this.definePlotsFromFilePanel.Controls.Add((Control) this.txtPlots, 1, 1);
      this.definePlotsFromFilePanel.Controls.Add((Control) this.txtStrata, 1, 2);
      this.definePlotsFromFilePanel.Controls.Add((Control) this.txtProjection, 1, 3);
      this.definePlotsFromFilePanel.Controls.Add((Control) this.plotListBtn, 2, 1);
      this.definePlotsFromFilePanel.Controls.Add((Control) this.strataFileBtn, 2, 2);
      this.definePlotsFromFilePanel.Controls.Add((Control) this.gisFileBtn, 2, 3);
      this.definePlotsFromFilePanel.Controls.Add((Control) this.btnImport, 2, 4);
      this.definePlotsFromFilePanel.Controls.Add((Control) this.label1, 0, 0);
      this.definePlotsFromFilePanel.Name = "definePlotsFromFilePanel";
      componentResourceManager.ApplyResources((object) this.plotListLbl, "plotListLbl");
      this.plotListLbl.BackColor = Color.Transparent;
      this.plotListLbl.Name = "plotListLbl";
      componentResourceManager.ApplyResources((object) this.strataFileLbl, "strataFileLbl");
      this.strataFileLbl.BackColor = Color.Transparent;
      this.strataFileLbl.Name = "strataFileLbl";
      componentResourceManager.ApplyResources((object) this.gisFileLbl, "gisFileLbl");
      this.gisFileLbl.BackColor = Color.Transparent;
      this.gisFileLbl.Name = "gisFileLbl";
      componentResourceManager.ApplyResources((object) this.txtPlots, "txtPlots");
      this.txtPlots.Name = "txtPlots";
      this.txtPlots.ReadOnly = true;
      this.txtPlots.TextChanged += new EventHandler(this.txtFile_TextChanged);
      componentResourceManager.ApplyResources((object) this.txtStrata, "txtStrata");
      this.txtStrata.Name = "txtStrata";
      this.txtStrata.ReadOnly = true;
      this.txtStrata.TextChanged += new EventHandler(this.txtFile_TextChanged);
      componentResourceManager.ApplyResources((object) this.txtProjection, "txtProjection");
      this.txtProjection.Name = "txtProjection";
      this.txtProjection.ReadOnly = true;
      this.txtProjection.TextChanged += new EventHandler(this.txtFile_TextChanged);
      componentResourceManager.ApplyResources((object) this.plotListBtn, "plotListBtn");
      this.plotListBtn.Name = "plotListBtn";
      this.plotListBtn.UseVisualStyleBackColor = true;
      this.plotListBtn.Click += new EventHandler(this.plotListBtn_Click);
      componentResourceManager.ApplyResources((object) this.strataFileBtn, "strataFileBtn");
      this.strataFileBtn.Name = "strataFileBtn";
      this.strataFileBtn.UseVisualStyleBackColor = true;
      this.strataFileBtn.Click += new EventHandler(this.strataFileBtn_Click);
      componentResourceManager.ApplyResources((object) this.gisFileBtn, "gisFileBtn");
      this.gisFileBtn.Name = "gisFileBtn";
      this.gisFileBtn.UseVisualStyleBackColor = true;
      this.gisFileBtn.Click += new EventHandler(this.gisFileBtn_Click);
      componentResourceManager.ApplyResources((object) this.btnImport, "btnImport");
      this.btnImport.Name = "btnImport";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.definePlotsFromFilePanel.SetColumnSpan((Control) this.label1, 3);
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.Controls.Add((Control) this.definePlotsFromFilePanel);
      this.Name = nameof (DefinePlotsFileForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.definePlotsFromFilePanel, 0);
      this.definePlotsFromFilePanel.ResumeLayout(false);
      this.definePlotsFromFilePanel.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
