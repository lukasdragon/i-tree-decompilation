// Decompiled with JetBrains decompiler
// Type: Streets.Domain.Lookup
// Assembly: Streets.Domain, Version=1.1.5560.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C8C80D5-C46C-4F96-B160-AECDF3D99BE1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Streets.Domain.dll

namespace Streets.Domain
{
  public abstract class Lookup
  {
    private int? m_hash;
    private LookupId m_id;

    public Lookup()
    {
    }

    public Lookup(LookupId id) => this.m_id = id;

    public virtual LookupId Id => this.m_id;

    public virtual string Description { get; set; }

    public virtual bool IsTransient => this.m_id == null;

    public override bool Equals(object obj) => obj is Lookup lookup && !(this.GetType() != lookup.GetType()) && !(this.IsTransient ^ lookup.IsTransient) && this == lookup && this.Id.Equals((object) lookup.Id);

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.m_id.GetHashCode());
      return this.m_hash.Value;
    }
  }
}
