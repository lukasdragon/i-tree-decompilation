// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Tasks.ListEntitiesTask`1
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Tasks;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace i_Tree_Eco_v6.Tasks
{
  public class ListEntitiesTask<T> : ATask<ListResult<T>> where T : Entity
  {
    private InputSession m_inputSession;
    private List<KeyValuePair<string, string>> m_aliases;
    private List<Order> m_orders;
    private List<ICriterion> m_criteria;
    private List<KeyValuePair<string, SelectMode>> m_selectList;

    public ListEntitiesTask<T> AddAlias(string path, string alias)
    {
      if (this.m_aliases == null)
        this.m_aliases = new List<KeyValuePair<string, string>>();
      this.m_aliases.Add(new KeyValuePair<string, string>(path, alias));
      return this;
    }

    public ListEntitiesTask<T> AddOrder(Order order)
    {
      if (this.m_orders == null)
        this.m_orders = new List<Order>();
      this.m_orders.Add(order);
      return this;
    }

    public ListEntitiesTask<T> AddCriterion(ICriterion criterion)
    {
      if (this.m_criteria == null)
        this.m_criteria = new List<ICriterion>();
      this.m_criteria.Add(criterion);
      return this;
    }

    public ListEntitiesTask<T> Fetch(SelectMode mode, string path)
    {
      if (this.m_selectList == null)
        this.m_selectList = new List<KeyValuePair<string, SelectMode>>();
      this.m_selectList.Add(new KeyValuePair<string, SelectMode>(path, mode));
      return this;
    }

    public ListEntitiesTask(InputSession input_session) => this.m_inputSession = input_session;

    protected override ListResult<T> Work()
    {
      ISession session = this.m_inputSession.CreateSession();
      using (session.BeginTransaction())
      {
        ICriteria criteria = session.CreateCriteria<T>();
        if (this.m_aliases != null)
        {
          foreach (KeyValuePair<string, string> alias in this.m_aliases)
            criteria.CreateAlias(alias.Key, alias.Value);
        }
        if (this.m_criteria != null)
        {
          foreach (ICriterion criterion in this.m_criteria)
            criteria.Add(criterion);
        }
        if (this.m_selectList != null)
        {
          foreach (KeyValuePair<string, SelectMode> select in this.m_selectList)
            criteria.Fetch(select.Value, select.Key);
        }
        if (this.m_orders != null)
        {
          foreach (Order order in this.m_orders)
            criteria.AddOrder(order);
        }
        return new ListResult<T>()
        {
          Session = session,
          List = criteria.List<T>(),
          Data = this.Data
        };
      }
    }
  }
}
