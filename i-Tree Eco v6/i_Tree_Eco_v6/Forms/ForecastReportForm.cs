// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ForecastReportForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using i_Tree_Eco_v6.Events;
using NHibernate;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ForecastReportForm : ReportViewerForm
  {
    private IContainer components;

    public ForecastReportForm()
    {
      EventPublisher.Register<EntityUpdated<Forecast>>(new EventHandler<EntityUpdated<Forecast>>(this.Forecast_Updated));
      Program.Session.InputSession.ForecastChanged += new EventHandler(this.InputSession_ForecastChanged);
    }

    private void InputSession_ForecastChanged(object sender, EventArgs e)
    {
      using (ISession session = Program.Session.InputSession.CreateSession())
      {
        if (!session.Get<Forecast>((object) Program.Session.InputSession.ForecastKey).Changed)
          return;
        this.Close();
      }
    }

    private void Forecast_Updated(object sender, EntityUpdated<Forecast> e)
    {
      using (ISession session = Program.Session.InputSession.CreateSession())
      {
        if (!session.Get<Forecast>((object) e.Guid).Changed)
          return;
        this.Close();
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
      this.components = (IContainer) new Container();
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Text = nameof (ForecastReportForm);
    }
  }
}
