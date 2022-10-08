// Decompiled with JetBrains decompiler
// Type: Eco.Util.YearHelper
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.Core;
using Eco.Domain.v6;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Eco.Util
{
  public static class YearHelper
  {
    public static void RestoreDefaultGroundCovers(Year y, ISession s, ISessionFactory sfLocSp)
    {
      using (ITransaction transaction = s.BeginTransaction())
      {
        foreach (GroundCover groundCover in (IEnumerable<GroundCover>) y.GroundCovers.ToList<GroundCover>())
          s.Delete((object) groundCover);
        try
        {
          y.GroundCovers.Clear();
          transaction.Commit();
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
        }
      }
      using (ITransaction transaction = s.BeginTransaction())
      {
        YearHelper.CreateGroundCovers(y, sfLocSp);
        foreach (GroundCover groundCover in (IEnumerable<GroundCover>) y.GroundCovers)
          s.SaveOrUpdate((object) groundCover);
        try
        {
          transaction.Commit();
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
          throw;
        }
      }
    }

    public static void CreateGroundCovers(Year y, ISessionFactory sfLocSp)
    {
      foreach (CoverType coverType in (IEnumerable<CoverType>) RetryExecutionHandler.Execute<IList<CoverType>>((Func<IList<CoverType>>) (() =>
      {
        using (ISession session = sfLocSp.OpenSession())
          return session.CreateCriteria<CoverType>().SetCacheable(true).List<CoverType>();
      })))
      {
        GroundCover groundCover = new GroundCover()
        {
          Id = coverType.Id,
          Description = coverType.Description,
          Year = y,
          CoverTypeId = coverType.Id
        };
        y.GroundCovers.Add(groundCover);
      }
    }

    public static void RestoreDefaultLandUses(Year y, ISession s, ISessionFactory sfLocSp)
    {
      using (ITransaction transaction = s.BeginTransaction())
      {
        foreach (LandUse landUse in (IEnumerable<LandUse>) y.LandUses.ToList<LandUse>())
          s.Delete((object) landUse);
        try
        {
          y.LandUses.Clear();
          transaction.Commit();
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
          throw;
        }
      }
      using (ITransaction transaction = s.BeginTransaction())
      {
        YearHelper.CreateLandUses(y, sfLocSp);
        foreach (LandUse landUse in (IEnumerable<LandUse>) y.LandUses)
          s.SaveOrUpdate((object) landUse);
        try
        {
          transaction.Commit();
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
        }
      }
    }

    public static void CreateLandUses(Year y, ISessionFactory sfLocSp)
    {
      foreach (FieldLandUse fieldLandUse in (IEnumerable<FieldLandUse>) RetryExecutionHandler.Execute<IList<FieldLandUse>>((Func<IList<FieldLandUse>>) (() =>
      {
        using (ISession session = sfLocSp.OpenSession())
          return session.CreateCriteria<FieldLandUse>().SetCacheable(true).List<FieldLandUse>();
      })))
      {
        LandUse landUse = new LandUse()
        {
          Id = fieldLandUse.Code,
          Description = fieldLandUse.Description,
          Year = y,
          LandUseId = fieldLandUse.Id
        };
        y.LandUses.Add(landUse);
      }
    }

    public static void CreateLookup<LSLU, T>(Year y, ISet<T> lookups, ISessionFactory sfLocSp)
      where LSLU : LocationSpecies.Domain.Lookup
      where T : Eco.Domain.v6.Lookup
    {
      foreach (LSLU lslu in (IEnumerable<LSLU>) RetryExecutionHandler.Execute<IList<LSLU>>((Func<IList<LSLU>>) (() =>
      {
        using (IStatelessSession statelessSession = sfLocSp.OpenStatelessSession())
          return statelessSession.CreateCriteria<LSLU>().SetCacheable(true).List<LSLU>();
      })))
      {
        T instance = Activator.CreateInstance<T>();
        instance.Id = lslu.Id;
        instance.Description = lslu.Description;
        instance.Year = y;
        lookups.Add(instance);
      }
    }

    public static void RestoreDefaultConditions(Year y, ISession s)
    {
      using (ITransaction transaction = s.BeginTransaction())
      {
        foreach (Condition condition in (IEnumerable<Condition>) y.Conditions.ToList<Condition>())
          s.Delete((object) condition);
        try
        {
          y.Conditions.Clear();
          transaction.Commit();
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
          throw;
        }
      }
      using (ITransaction transaction = s.BeginTransaction())
      {
        YearHelper.CreateConditions(y);
        foreach (Condition condition in (IEnumerable<Condition>) y.Conditions)
          s.SaveOrUpdate((object) condition);
        try
        {
          transaction.Commit();
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
          throw;
        }
      }
    }

    public static void CreateConditions(Year y)
    {
      for (int index = 0; index < 23; ++index)
      {
        Condition condition = new Condition()
        {
          Year = y,
          Id = index
        };
        switch (index)
        {
          case 0:
            condition.Description = "-- Not Entered --";
            condition.DiebackDesc = "-- Not Entered --";
            condition.PctDieback = -1.0;
            break;
          case 1:
            condition.Description = "100%";
            condition.DiebackDesc = "0%";
            condition.PctDieback = 0.0;
            break;
          case 2:
            condition.Description = "95% - 99%";
            condition.DiebackDesc = "1% - 5%";
            condition.PctDieback = 3.0;
            break;
          default:
            if (index != 21)
            {
              if (index == 22)
              {
                condition.Description = "0%";
                condition.DiebackDesc = "100%";
                condition.PctDieback = 100.0;
                break;
              }
              condition.Description = string.Format("{0}% - {1}%", (object) (100 - (index - 1) * 5), (object) (100 - (index - 2) * 5));
              condition.DiebackDesc = string.Format("{0}% - {1}%", (object) ((index - 2) * 5), (object) ((index - 1) * 5));
              condition.PctDieback = (double) ((index - 2) * 5 + 3);
              break;
            }
            condition.Description = "1% - 5%";
            condition.DiebackDesc = "95% - 99%";
            condition.PctDieback = 98.0;
            break;
        }
        y.Conditions.Add(condition);
      }
    }

    public static void RestoreDefaultHealthRptClasses(Year y, ISession s)
    {
      IList<HealthRptClass> list = (IList<HealthRptClass>) y.HealthRptClasses.ToList<HealthRptClass>();
      using (ITransaction transaction = s.BeginTransaction())
      {
        foreach (HealthRptClass healthRptClass in (IEnumerable<HealthRptClass>) list)
          s.Delete((object) healthRptClass);
        try
        {
          y.HealthRptClasses.Clear();
          transaction.Commit();
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
          throw;
        }
      }
      using (ITransaction transaction = s.BeginTransaction())
      {
        YearHelper.CreateHealthRptClasses(y);
        foreach (HealthRptClass healthRptClass in (IEnumerable<HealthRptClass>) y.HealthRptClasses)
          s.SaveOrUpdate((object) healthRptClass);
        try
        {
          transaction.Commit();
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
          throw;
        }
      }
    }

    public static void CreateHealthRptClasses(Year y)
    {
      int num = 1;
      foreach (ConditionCategory conditionCategory in Enum.GetValues(typeof (ConditionCategory)))
        y.HealthRptClasses.Add(new HealthRptClass()
        {
          Year = y,
          Id = num++,
          Extent = (double) conditionCategory,
          Description = EnumHelper.GetDescription<ConditionCategory>(conditionCategory)
        });
    }

    public static bool RestoreDefaultDBHs(Year y, ISession s)
    {
      using (ITransaction transaction = s.BeginTransaction())
      {
        if (!y.DBHActual)
        {
          IList<int> source = s.CreateCriteria<Stem>().CreateAlias("Tree", "t").CreateAlias("t.Plot", "p").CreateAlias("p.Year", nameof (y)).SetProjection((IProjection) Projections.Property<Stem>((System.Linq.Expressions.Expression<Func<Stem, object>>) (st => (object) st.Id))).Add((ICriterion) Restrictions.Eq(nameof (y), (object) y)).List<int>();
          if (s.CreateCriteria<Stem>().CreateAlias("Tree", "t").CreateAlias("t.Plot", "p").CreateAlias("p.Year", nameof (y)).Add((ICriterion) Restrictions.In("Diameter", (ICollection) source.ToArray<int>())).Add((ICriterion) Restrictions.Eq(nameof (y), (object) y)).SetProjection(Projections.RowCount()).UniqueResult<int>() > 0)
            return false;
        }
        foreach (DBH dbh in (IEnumerable<DBH>) y.DBHs.ToList<DBH>())
          s.Delete((object) dbh);
        try
        {
          y.DBHs.Clear();
          transaction.Commit();
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
          throw;
        }
      }
      using (ITransaction transaction = s.BeginTransaction())
      {
        YearHelper.CreateDBHs(y);
        foreach (DBH dbH in (IEnumerable<DBH>) y.DBHs)
          s.SaveOrUpdate((object) dbH);
        try
        {
          transaction.Commit();
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
          throw;
        }
      }
      return true;
    }

    public static void CreateDBHs(Year v6Year)
    {
      double num1 = 7.62;
      int num2 = 0;
      string str = "cm";
      if (v6Year.Unit == YearUnit.English)
      {
        num1 = 3.0;
        str = "in";
      }
      for (int index = 0; index < 10; ++index)
      {
        double num3 = Math.Round((double) num2 * num1, 1);
        if (index == 2)
        {
          num1 *= 2.0;
          num2 /= 2;
        }
        double num4 = Math.Round((double) (num2 + 1) * num1, 1);
        v6Year.DBHs.Add(new DBH()
        {
          Year = v6Year,
          Id = index + 1,
          Value = index < 9 ? (num3 + num4) / 2.0 : num3 * 11.0 / 10.0,
          Description = index < 9 ? string.Format("{0} - {1} {2}", (object) num3, (object) num4, (object) str) : string.Format("> {0} {1}", (object) num3, (object) str)
        });
        ++num2;
      }
    }

    public static void RestoreDefaultDBHRptClasses(Year y, ISession s)
    {
      IList<DBHRptClass> list = (IList<DBHRptClass>) y.DBHRptClasses.ToList<DBHRptClass>();
      using (ITransaction transaction = s.BeginTransaction())
      {
        foreach (DBHRptClass dbhRptClass in (IEnumerable<DBHRptClass>) list)
          s.Delete((object) dbhRptClass);
        try
        {
          y.DBHRptClasses.Clear();
          transaction.Commit();
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
          throw;
        }
      }
      using (ITransaction transaction = s.BeginTransaction())
      {
        YearHelper.CreateDBHRptClasses(y);
        foreach (DBHRptClass dbhRptClass in (IEnumerable<DBHRptClass>) y.DBHRptClasses)
          s.SaveOrUpdate((object) dbhRptClass);
        try
        {
          transaction.Commit();
        }
        catch (HibernateException ex)
        {
          transaction.Rollback();
          throw;
        }
      }
    }

    public static void CreateDBHRptClasses(Year v6Year)
    {
      double num1 = 7.62;
      int num2 = 0;
      if (v6Year.Unit == YearUnit.English)
        num1 = 3.0;
      for (int index = 0; index < 10; ++index)
      {
        DBHRptClass dbhRptClass = new DBHRptClass();
        dbhRptClass.Year = v6Year;
        dbhRptClass.Id = index + 1;
        dbhRptClass.RangeStart = Math.Round((double) num2 * num1, 1);
        if (index == 2)
        {
          num1 *= 2.0;
          num2 /= 2;
        }
        dbhRptClass.RangeEnd = index >= 9 ? 1000.0 : Math.Round((double) (num2 + 1) * num1, 1);
        v6Year.DBHRptClasses.Add(dbhRptClass);
        ++num2;
      }
    }

    public static PctMidRange MapPctMidRange(object val)
    {
      double a = Convert.ToDouble(val);
      int num = a > 1.0 || a <= 0.0 ? Convert.ToInt32(a) : (int) Math.Ceiling(a);
      return num == 0 || num == 100 ? (PctMidRange) num : (PctMidRange) (num / 5 * 5 + 3);
    }

    public static HealthRptClass ReturnHealthClass(
      ISet<HealthRptClass> healthClasses,
      double dieback)
    {
      int num1 = -1;
      double num2 = 0.0;
      HealthRptClass healthRptClass = (HealthRptClass) null;
      foreach (HealthRptClass healthClass in (IEnumerable<HealthRptClass>) healthClasses)
      {
        if (dieback <= healthClass.Extent)
        {
          if (num1 == -1)
          {
            num1 = healthClass.Id;
            num2 = healthClass.Extent;
            healthRptClass = healthClass;
          }
          else if (healthClass.Extent < num2)
          {
            num1 = healthClass.Id;
            num2 = healthClass.Extent;
            healthRptClass = healthClass;
          }
        }
      }
      if (num1 <= 0)
        throw new Exception(Strings.HealthClassLookupErrorMessage);
      return healthRptClass;
    }
  }
}
