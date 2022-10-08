// Decompiled with JetBrains decompiler
// Type: Eco.Util.Convert.DefaultTreeData
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.NHibernate;
using DaveyTree.NHibernate.Extensions;
using Eco.Domain.v6;
using Eco.Util.Forecast;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Eco.Util.Convert
{
  public class DefaultTreeData
  {
    private ISession _input;
    private ISession _ls;
    private Year _year;
    private Condition thirteenPctDieback;
    private Dictionary<string, TaxonModel> _taxa;
    private double _dbhToInches;
    private double _feetToProjUnits;
    private bool initialized;

    public DBHCrownSize calculateDbhCrownSize(Tree aTree)
    {
      DBHCrownSize dbhCrownSize = new DBHCrownSize()
      {
        dieback = !this._year.RecordCrownCondition ? (int) this.thirteenPctDieback.PctDieback : (int) aTree.Crown.Condition.PctDieback,
        dbh = this.avgDbhOf(aTree)
      };
      if (this._year.RecordCrownSize)
      {
        dbhCrownSize.crownTopHeight = (double) aTree.Crown.TopHeight < 0.0 ? 0.0 : (double) aTree.Crown.TopHeight;
        dbhCrownSize.crownHeight = (double) aTree.Crown.TopHeight - (double) aTree.Crown.BaseHeight;
        dbhCrownSize.crownWidth = ((double) aTree.Crown.WidthEW + (double) aTree.Crown.WidthNS) / 2.0;
        dbhCrownSize.crownTopHeight = dbhCrownSize.crownTopHeight < 0.0 ? 0.0 : dbhCrownSize.crownTopHeight;
        dbhCrownSize.crownHeight = dbhCrownSize.crownHeight < 0.0 ? 0.0 : dbhCrownSize.crownHeight;
        dbhCrownSize.crownWidth = dbhCrownSize.crownWidth < 0.0 ? 0.0 : dbhCrownSize.crownWidth;
      }
      else
      {
        double num1 = this._dbhToInches * dbhCrownSize.dbh;
        dbhCrownSize.crownTopHeight = this._year.RecordHeight ? (double) aTree.TreeHeight : this._feetToProjUnits * this._taxa[aTree.Species].Height(num1);
        dbhCrownSize.crownWidth = this._feetToProjUnits * this._taxa[aTree.Species].CrownWidthBySpeciesGroup(num1, dbhCrownSize.crownTopHeight / this._feetToProjUnits) * Math.Sqrt(1.0 - 0.01 * (double) dbhCrownSize.dieback);
        double num2 = this._feetToProjUnits * this._taxa[aTree.Species].CrownHeight(num1) * Math.Sqrt(1.0 - 0.01 * (double) dbhCrownSize.dieback);
        dbhCrownSize.crownHeight = dbhCrownSize.crownTopHeight - num2 > 0.0 ? num2 : dbhCrownSize.crownTopHeight;
      }
      dbhCrownSize.treeHeight = this._year.RecordHeight ? (double) aTree.TreeHeight : dbhCrownSize.crownHeight;
      if (dbhCrownSize.treeHeight < 0.0)
        dbhCrownSize.treeHeight = 0.0;
      return dbhCrownSize;
    }

    public bool calculate(
      ISession workingProject_s,
      Guid yearId,
      ISession ls,
      IProgress<SASProgressArg> uploadProgress,
      CancellationToken uploadCancellationToken,
      SASProgressArg uploadProgressArg,
      int progressFromRange,
      int progressToRange)
    {
      this.initialize(workingProject_s, yearId, ls);
      if (this._year.RecordHeight && this._year.RecordCrownSize)
        return true;
      double num1 = 0.0;
      PagedList<Tree> pagedList = this._input.CreateCriteria<Tree>().CreateAlias("Plot", "p").CreateAlias("p.Year", "y").Add((ICriterion) Restrictions.Eq("y.Guid", (object) yearId)).Add((ICriterion) Restrictions.On<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[5]
      {
        (object) TreeStatus.InitialSample,
        (object) TreeStatus.Ingrowth,
        (object) TreeStatus.NoChange,
        (object) TreeStatus.Planted,
        (object) TreeStatus.Unknown
      })).PagedList<Tree>(2000);
      int count = pagedList.Count;
      ITransaction transaction = this._input.BeginTransaction();
      int num2 = 0;
      uploadProgressArg.Description = "Calculating ...";
      foreach (Tree t in (IEnumerable<Tree>) pagedList)
      {
        int num3 = (int) ((double) num2 / (double) count * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
        if (uploadProgressArg.Percent != num3)
        {
          uploadProgressArg.Percent = num3;
          uploadProgress.Report(uploadProgressArg);
        }
        if (uploadCancellationToken.IsCancellationRequested)
          return false;
        ++num2;
        if (!this._year.RecordCrownCondition)
          t.Crown.Condition = this.thirteenPctDieback;
        if (t.Crown.Condition.PctDieback == 100.0)
        {
          if (!this._year.RecordHeight)
          {
            num1 = this._dbhToInches * this.avgDbhOf(t);
            double num4 = this._feetToProjUnits * this._taxa[t.Species].Height(num1);
            t.TreeHeight = (float) num4;
          }
        }
        else
        {
          if (!this._year.RecordHeight || !this._year.RecordCrownSize)
            num1 = this._dbhToInches * this.avgDbhOf(t);
          if (!this._year.RecordHeight)
          {
            double num5 = this._feetToProjUnits * this._taxa[t.Species].Height(num1);
            t.TreeHeight = (float) num5;
          }
          if (!this._year.RecordCrownSize)
          {
            double num6 = this._feetToProjUnits * this._taxa[t.Species].CrownHeight(num1) * Math.Sqrt(1.0 - 0.01 * t.Crown.Condition.PctDieback);
            double num7 = this._feetToProjUnits * this._taxa[t.Species].CrownWidthBySpeciesGroup(num1, (double) t.TreeHeight / this._feetToProjUnits) * Math.Sqrt(1.0 - 0.01 * t.Crown.Condition.PctDieback);
            t.Crown.TopHeight = t.TreeHeight;
            t.Crown.BaseHeight = (double) t.TreeHeight - num6 <= 0.0 ? 0.0f : t.TreeHeight - (float) num6;
            t.Crown.WidthEW = (float) num7;
            t.Crown.WidthNS = (float) num7;
            t.Crown.PercentMissing = PctMidRange.PR13;
          }
        }
        if ((Decimal) num2 % 1000M == 0M)
        {
          transaction.Commit();
          transaction.Dispose();
          transaction = this._input.BeginTransaction();
          this._input.Flush();
        }
      }
      transaction.Commit();
      transaction.Dispose();
      return true;
    }

    public void initialize(ISession project_s, Guid yearId, ISession ls)
    {
      if (this.initialized)
        return;
      this._input = project_s;
      this._ls = ls;
      this._year = this._input.Get(typeof (Year), (object) yearId) as Year;
      this._dbhToInches = this._year.Unit == YearUnit.English ? 1.0 : 0.393700787;
      this._feetToProjUnits = this._year.Unit == YearUnit.English ? 1.0 : 0.30480000000121921;
      if (!this._year.RecordCrownCondition)
        this.thirteenPctDieback = this.getThirteenPctDieback();
      if (this._year.RecordCrownSize && this._year.RecordHeight)
        return;
      this._taxa = new Dictionary<string, TaxonModel>();
      IQueryOver<Tree, Year> queryOver = this._input.QueryOver<Tree>().WhereRestrictionOn((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[3]
      {
        (object) TreeStatus.Ingrowth,
        (object) TreeStatus.Planted,
        (object) TreeStatus.Unknown
      }).Inner.JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot)).Inner.JoinQueryOver<Year>((System.Linq.Expressions.Expression<Func<Plot, Year>>) (p => p.Year)).Where((System.Linq.Expressions.Expression<Func<Year, bool>>) (y => y.Guid == this._year.Guid));
      IProjection[] projectionArray = new IProjection[1]
      {
        Projections.Distinct((IProjection) Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => t.Species)))
      };
      foreach (string str in (IEnumerable<string>) queryOver.Select(projectionArray).List<string>())
      {
        if (!this._taxa.ContainsKey(str))
          this._taxa.Add(str, new TaxonModel(str, this._ls));
      }
    }

    private double avgDbhOf(Tree t)
    {
      if (t.Plot.Year.DBHActual)
        return Math.Sqrt(t.Stems.Sum<Stem>((Func<Stem, double>) (s => s.Diameter * s.Diameter)));
      double d1 = 0.0;
      foreach (Stem stem1 in (IEnumerable<Stem>) t.Stems)
      {
        Stem stem = stem1;
        DBH dbh = this._year.DBHs.SingleOrDefault<DBH>((Func<DBH, bool>) (d => (double) d.Id == stem.Diameter));
        if (dbh != null)
          d1 += dbh.Value * dbh.Value;
      }
      return Math.Sqrt(d1);
    }

    private Condition getThirteenPctDieback()
    {
      using (ITransaction transaction = this._input.BeginTransaction())
      {
        Condition thirteenPctDieback = this._input.QueryOver<Condition>().Where((System.Linq.Expressions.Expression<Func<Condition, bool>>) (c => c.PctDieback == Condition.Default.PctDieback)).Inner.JoinQueryOver<Year>((System.Linq.Expressions.Expression<Func<Condition, Year>>) (c => c.Year)).Where((System.Linq.Expressions.Expression<Func<Year, bool>>) (y => y.Guid == this._year.Guid)).SingleOrDefault();
        if (thirteenPctDieback == null)
        {
          thirteenPctDieback = new Condition()
          {
            PctDieback = Condition.Default.PctDieback,
            Id = this._input.CreateCriteria<Condition>().Add((ICriterion) Restrictions.Eq("Year", (object) this._year)).SetProjection((IProjection) Projections.Max("Id")).UniqueResult<int>() + 1,
            Description = "13% Dieback",
            Year = this._year
          };
          this._input.Save((object) thirteenPctDieback);
        }
        transaction.Commit();
        return thirteenPctDieback;
      }
    }

    public static string getTempFileName(string targetFolder, string extension)
    {
      string path = Path.Combine(targetFolder, Guid.NewGuid().ToString() + "." + extension);
      for (int index = 0; index < 1000; ++index)
      {
        if (!File.Exists(path))
          return path;
        path = Path.Combine(targetFolder, Guid.NewGuid().ToString() + "." + extension);
      }
      throw new ApplicationException("Can not get a temporary file name");
    }
  }
}
