// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PollutionStations
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Util;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.Drawing;
using System.Text;

namespace i_Tree_Eco_v6.Reports
{
  internal class PollutionStations : ReportBase
  {
    private ProgramSession m_ProgramSession;

    public PollutionStations(ProgramSession m_ProgramSession) => this.m_ProgramSession = m_ProgramSession;

    public PollutionStations() => this.m_ProgramSession = ProgramSession.GetInstance();

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
    }

    public override void RenderHeader(C1PrintDocument C1doc)
    {
    }

    public override void RenderFooter(C1PrintDocument C1doc)
    {
    }

    public override void InitDocument(C1PrintDocument C1doc)
    {
    }

    public DataTable GetData(short pollutionYear, int locationId)
    {
      DataTable data = new DataTable();
      data.Columns.AddRange(new DataColumn[10]
      {
        new DataColumn("TreeID", typeof (string)),
        new DataColumn("CO", typeof (bool)),
        new DataColumn("O3", typeof (bool)),
        new DataColumn("NO2", typeof (bool)),
        new DataColumn("SO2", typeof (bool)),
        new DataColumn("PM2.5", typeof (bool)),
        new DataColumn("Location", typeof (string)),
        new DataColumn("xCoordinate", typeof (double)),
        new DataColumn("yCoordinate", typeof (double)),
        new DataColumn("distance", typeof (double))
      });
      IList<PollutantStationPollutant> stationPollutants = this.GetPollutantStationPollutants(locationId, pollutionYear);
      ISession session = this.m_ProgramSession.LocSp.OpenSession();
      session.FlushMode = FlushMode.Manual;
      LocationSpecies.Domain.Location location = session.Get<LocationSpecies.Domain.Location>((object) locationId);
      PollutantStation pollutantStation = (PollutantStation) null;
      DataRow row = (DataRow) null;
      GeoCoordinate geoCoordinate = new GeoCoordinate(location.Latitude, location.Longitude);
      foreach (PollutantStationPollutant stationPollutant in (IEnumerable<PollutantStationPollutant>) stationPollutants)
      {
        string name = stationPollutant.Pollutant.Name;
        if (data.Columns.Contains(name))
        {
          if (!stationPollutant.PollutantStation.Equals((object) pollutantStation))
          {
            pollutantStation = stationPollutant.PollutantStation;
            row = data.NewRow();
            row["Location"] = (object) this.GetLocationName(pollutantStation.Location);
            row["TreeID"] = (object) pollutantStation.Site;
            row["xCoordinate"] = (object) pollutantStation.Longitude;
            row["yCoordinate"] = (object) pollutantStation.Latitude;
            row["distance"] = (object) (geoCoordinate.GetDistanceTo(new GeoCoordinate(pollutantStation.Latitude, pollutantStation.Longitude)) / 1000.0);
            data.Rows.Add(row);
          }
          row[name] = (object) true;
        }
      }
      return data;
    }

    private string GetLocationName(LocationSpecies.Domain.Location loc) => RetryExecutionHandler.Execute<string>((Func<string>) (() =>
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (loc == null)
        return i_Tree_Eco_v6.Resources.Strings.Unavailable;
      using (ISession session = ReportBase.m_ps.LocSp.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          do
          {
            stringBuilder.AppendFormat("{0}, ", (object) loc.Name);
            // ISSUE: reference to a compiler-generated field
            LocationRelation locationRelation = session.QueryOver<LocationRelation>().Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Location == this.loc)).Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Code != default (string))).Fetch<LocationRelation, LocationRelation>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<LocationRelation, object>>) (r => r.Parent)).Cacheable().SingleOrDefault();
            if (locationRelation.Level != (short) 2)
              loc = locationRelation.Parent;
            else
              break;
          }
          while (loc != null);
          transaction.Commit();
        }
      }
      stringBuilder.Length -= 2;
      return stringBuilder.ToString();
    }));

    private IList<PollutantStationPollutant> GetPollutantStationPollutants(
      int locationId,
      short pollutionYear)
    {
      return RetryExecutionHandler.Execute<IList<PollutantStationPollutant>>((Func<IList<PollutantStationPollutant>>) (() =>
      {
        using (ISession session = this.m_ProgramSession.LocSp.OpenSession())
        {
          IList<PollutantStationPollutant> stationPollutants;
          do
          {
            LocationSpecies.Domain.Location location = (LocationSpecies.Domain.Location) null;
            PollutantStation ps = (PollutantStation) null;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            stationPollutants = session.QueryOver<PollutantStationPollutant>().JoinAlias((System.Linq.Expressions.Expression<Func<PollutantStationPollutant, object>>) (psp => psp.PollutantStation), (System.Linq.Expressions.Expression<Func<object>>) (() => ps)).Fetch<PollutantStationPollutant, PollutantStationPollutant>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<PollutantStationPollutant, object>>) (psp => psp.PollutantStation)).Fetch<PollutantStationPollutant, PollutantStationPollutant>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<PollutantStationPollutant, object>>) (psp => psp.PollutantStation.Location)).Fetch<PollutantStationPollutant, PollutantStationPollutant>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<PollutantStationPollutant, object>>) (psp => psp.Pollutant)).Where((System.Linq.Expressions.Expression<Func<PollutantStationPollutant, bool>>) (psp => psp.MonYear == (int) this.pollutionYear)).Inner.JoinQueryOver<LocationSpecies.Domain.Location>((System.Linq.Expressions.Expression<Func<PollutantStationPollutant, IEnumerable<LocationSpecies.Domain.Location>>>) (psp => psp.Locations), (System.Linq.Expressions.Expression<Func<LocationSpecies.Domain.Location>>) (() => location)).Where((System.Linq.Expressions.Expression<Func<LocationSpecies.Domain.Location, bool>>) (l => l.Id == this.locationId)).OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) ps.Location.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => ps.Site)).Asc.Cacheable().List();
            if (stationPollutants.Count == 0)
            {
              IList<LocationSpecies.Domain.Location> locationList = session.CreateCriteria<LocationSpecies.Domain.Location>().CreateAlias("Children", "c").CreateAlias("c.Location", "l").Add((ICriterion) Restrictions.IsNotNull("c.Code")).Add((ICriterion) Restrictions.Eq("l.Id", (object) locationId)).SetCacheable(true).List<LocationSpecies.Domain.Location>();
              if (locationList.Count > 0)
                locationId = locationList[0].Id;
            }
          }
          while (stationPollutants.Count == 0 && locationId != 1);
          return stationPollutants;
        }
      }));
    }

    public override List<ColumnFormat> ColumnsFormat(DataTable data)
    {
      List<ColumnFormat> columnFormatList = new List<ColumnFormat>();
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.CarbonMonoxideFormula,
        ColName = data.Columns[1].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatBool),
        ColNum = 0
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.OzoneFurmula,
        ColName = data.Columns[2].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatBool),
        ColNum = 1
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideFurmula,
        ColName = data.Columns[3].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatBool),
        ColNum = 2
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.SulfurDioxideFurmula,
        ColName = data.Columns[4].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatBool),
        ColNum = 3
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25Furmula,
        ColName = data.Columns[5].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatBool),
        ColNum = 4
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.Location,
        ColName = data.Columns[6].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 5
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.StationID,
        ColName = data.Columns[0].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 6
      });
      if (this.hasCoordinates && ReportBase.m_ps.ShowGPS)
      {
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.xCoordinate,
          ColName = data.Columns[7].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 7
        });
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.yCoordinate,
          ColName = data.Columns[8].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 8
        });
      }
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Distance, ReportBase.KMUnits()),
        ColName = data.Columns[9].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleKiloMeters),
        ColNum = 9
      });
      return columnFormatList;
    }
  }
}
