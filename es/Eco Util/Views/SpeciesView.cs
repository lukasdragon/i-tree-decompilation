// Decompiled with JetBrains decompiler
// Type: Eco.Util.Views.SpeciesView
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.Core;
using Eco.Util.Attributes;
using LocationSpecies.Domain;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Eco.Util.Views
{
  public class SpeciesView
  {
    private Species m_species;
    private string _cn;
    private string _sn;

    public SpeciesView(Species s) => this.m_species = s;

    public int Id => this.m_species.Id;

    [LocalizedDescription(typeof (LocationSpecies.Domain.Properties.Strings), "ScientificName")]
    public virtual string ScientificName
    {
      get
      {
        if (this._sn == null)
        {
          Species species = this.m_species;
          this._sn = species.ScientificName;
          while (species.Rank > SpeciesRank.Subgenus)
          {
            species = species.Parent;
            this._sn = string.Format("{0} {1}", (object) species.ScientificName, (object) this._sn);
          }
        }
        return this._sn;
      }
    }

    public static string ScientificCommonNameDescription => string.Format("{0} ({1})", (object) TypeHelper.DescriptionOf<Species>((Expression<Func<Species, object>>) (s => s.ScientificName)), (object) TypeHelper.DescriptionOf<Species>((Expression<Func<Species, object>>) (s => s.CommonName)));

    public static string CommonScientificNameDescription => string.Format("{0} ({1})", (object) TypeHelper.DescriptionOf<Species>((Expression<Func<Species, object>>) (s => s.CommonName)), (object) TypeHelper.DescriptionOf<Species>((Expression<Func<Species, object>>) (s => s.ScientificName)));

    [LocalizedDescription(typeof (SpeciesView), "ScientificCommonNameDescription")]
    public virtual string ScientificCommonName => string.Format("{0} ({1})", (object) this.ScientificName, (object) this.CommonName);

    [LocalizedDescription(typeof (SpeciesView), "CommonScientificNameDescription")]
    public virtual string CommonScientificName => string.Format("{0} ({1})", (object) this.CommonName, (object) this.ScientificName);

    [LocalizedDescription(typeof (LocationSpecies.Domain.Properties.Strings), "CommonName")]
    public virtual string CommonName
    {
      get
      {
        if (this._cn == null)
        {
          for (CultureInfo ci = CultureInfo.CurrentCulture; ci != CultureInfo.InvariantCulture; ci = ci.Parent)
          {
            Language key = this.m_species.Names.Keys.Where<Language>((Func<Language, bool>) (l => l.Culture.Equals((object) ci))).FirstOrDefault<Language>();
            if (key != null)
            {
              this._cn = this.m_species.Names[key];
              break;
            }
          }
          if (this._cn == null)
            this._cn = this.m_species.CommonName;
        }
        return this._cn;
      }
    }

    [LocalizedDescription(typeof (LocationSpecies.Domain.Properties.Strings), "Code")]
    public virtual string Code => this.m_species.Code;

    public virtual SpeciesRank Rank => this.m_species.Rank;

    public virtual SpeciesView Self => this;

    public virtual Species Species => this.m_species;
  }
}
