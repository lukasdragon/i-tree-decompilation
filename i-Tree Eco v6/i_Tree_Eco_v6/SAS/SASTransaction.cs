// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.SAS.SASTransaction
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using NHibernate;

namespace i_Tree_Eco_v6.SAS
{
  internal class SASTransaction
  {
    private ISession _s;
    private ITransaction _t;
    private int _maxNumber = 300;
    private int _count;

    public int MaxNumber
    {
      get => this._maxNumber;
      set => this._maxNumber = value;
    }

    public void Begin(ISession s)
    {
      this._s = s;
      this._t = this._s.BeginTransaction();
      this._count = 0;
    }

    public void Pause()
    {
      this._t.Commit();
      this._t.Dispose();
      this._count = 0;
    }

    public void Resume()
    {
      this._t = this._s.BeginTransaction();
      this._count = 0;
    }

    public void End()
    {
      this._t.Commit();
      this._t.Dispose();
      this._s = (ISession) null;
    }

    public void Abort()
    {
      this._t.Rollback();
      this._t.Dispose();
      this._s = (ISession) null;
    }

    public void CommitPrevious()
    {
      this._t.Commit();
      this._t.Dispose();
      this._t = this._s.BeginTransaction();
      this._count = 0;
    }

    public void IncreaseOperationNumber() => this.IncreaseOperationNumber(1);

    public void IncreaseOperationNumber(int num)
    {
      this._count += num;
      if (this._count < this._maxNumber)
        return;
      this.CommitPrevious();
    }
  }
}
