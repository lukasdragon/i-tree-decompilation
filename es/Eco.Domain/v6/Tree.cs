// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Tree
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.Attributes;
using Eco.Domain.DTO;
using Eco.Domain.DTO.v6;
using Eco.Domain.Properties;
using System;
using System.Collections.Generic;

namespace Eco.Domain.v6
{
  public class Tree : Entity<Tree>
  {
    public Tree()
    {
      this.DirectionFromCenter = -1;
      this.DistanceFromCenter = -1f;
      this.Status = 'U';
      this.TreeHeight = -1f;
      this.PercentImpervious = PctMidRange.PRINV;
      this.PercentShrub = PctMidRange.PRINV;
      this.NoteThisTree = false;
      this.Init();
    }

    public virtual Plot Plot
    {
      get => this.\u003CPlot\u003Ek__BackingField;
      set
      {
        if (this.\u003CPlot\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Plot));
        this.\u003CPlot\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Plot);
      }
    }

    [LocalizedDescription(typeof (v6Strings), "LandUse_SingularName")]
    public virtual PlotLandUse PlotLandUse
    {
      get => this.\u003CPlotLandUse\u003Ek__BackingField;
      set
      {
        if (this.\u003CPlotLandUse\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PlotLandUse));
        this.\u003CPlotLandUse\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PlotLandUse);
      }
    }

    public virtual MaintRec MaintRec
    {
      get => this.\u003CMaintRec\u003Ek__BackingField;
      set
      {
        if (this.\u003CMaintRec\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (MaintRec));
        this.\u003CMaintRec\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MaintRec);
      }
    }

    public virtual MaintTask MaintTask
    {
      get => this.\u003CMaintTask\u003Ek__BackingField;
      set
      {
        if (this.\u003CMaintTask\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (MaintTask));
        this.\u003CMaintTask\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MaintTask);
      }
    }

    public virtual Sidewalk SidewalkDamage
    {
      get => this.\u003CSidewalkDamage\u003Ek__BackingField;
      set
      {
        if (this.\u003CSidewalkDamage\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (SidewalkDamage));
        this.\u003CSidewalkDamage\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.SidewalkDamage);
      }
    }

    public virtual WireConflict WireConflict
    {
      get => this.\u003CWireConflict\u003Ek__BackingField;
      set
      {
        if (this.\u003CWireConflict\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (WireConflict));
        this.\u003CWireConflict\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.WireConflict);
      }
    }

    public virtual OtherOne OtherOne
    {
      get => this.\u003COtherOne\u003Ek__BackingField;
      set
      {
        if (this.\u003COtherOne\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (OtherOne));
        this.\u003COtherOne\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.OtherOne);
      }
    }

    public virtual OtherTwo OtherTwo
    {
      get => this.\u003COtherTwo\u003Ek__BackingField;
      set
      {
        if (this.\u003COtherTwo\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (OtherTwo));
        this.\u003COtherTwo\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.OtherTwo);
      }
    }

    public virtual OtherThree OtherThree
    {
      get => this.\u003COtherThree\u003Ek__BackingField;
      set
      {
        if (this.\u003COtherThree\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (OtherThree));
        this.\u003COtherThree\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.OtherThree);
      }
    }

    public virtual Crown Crown
    {
      get => this.\u003CCrown\u003Ek__BackingField;
      set
      {
        if (this.\u003CCrown\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Crown));
        this.\u003CCrown\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Crown);
      }
    }

    public virtual IPED IPED
    {
      get => this.\u003CIPED\u003Ek__BackingField;
      set
      {
        if (this.\u003CIPED\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (IPED));
        this.\u003CIPED\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.IPED);
      }
    }

    public virtual int Id
    {
      get => this.\u003CId\u003Ek__BackingField;
      set
      {
        if (this.\u003CId\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Id));
        this.\u003CId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Id);
      }
    }

    public virtual ISet<Stem> Stems
    {
      get => this.\u003CStems\u003Ek__BackingField;
      protected internal set
      {
        if (this.Stems == value)
          return;
        this.OnPropertyChanging(nameof (Stems));
        this.\u003CStems\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Stems);
      }
    }

    public virtual ISet<Building> Buildings
    {
      get => this.\u003CBuildings\u003Ek__BackingField;
      protected internal set
      {
        if (this.Buildings == value)
          return;
        this.OnPropertyChanging(nameof (Buildings));
        this.\u003CBuildings\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Buildings);
      }
    }

    public virtual short? CityManaged
    {
      get => this.\u003CCityManaged\u003Ek__BackingField;
      set
      {
        if (Nullable.Equals<short>(this.CityManaged, value))
          return;
        this.OnPropertyChanging(nameof (CityManaged));
        this.\u003CCityManaged\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.CityManaged);
      }
    }

    public virtual int DirectionFromCenter
    {
      get => this.\u003CDirectionFromCenter\u003Ek__BackingField;
      set
      {
        if (this.\u003CDirectionFromCenter\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (DirectionFromCenter));
        this.\u003CDirectionFromCenter\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DirectionFromCenter);
      }
    }

    public virtual float DistanceFromCenter
    {
      get => this.\u003CDistanceFromCenter\u003Ek__BackingField;
      set
      {
        if (this.\u003CDistanceFromCenter\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (DistanceFromCenter));
        this.\u003CDistanceFromCenter\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DistanceFromCenter);
      }
    }

    public virtual string UserId
    {
      get => this.\u003CUserId\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CUserId\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (UserId));
        this.\u003CUserId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.UserId);
      }
    }

    [LocalizedDescription(typeof (v6Strings), "Tree_Status")]
    public virtual char Status
    {
      get => this.\u003CStatus\u003Ek__BackingField;
      set
      {
        if ((int) this.\u003CStatus\u003Ek__BackingField == (int) value)
          return;
        this.OnPropertyChanging(nameof (Status));
        this.\u003CStatus\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Status);
      }
    }

    [LocalizedDescription(typeof (v6Strings), "Tree_Species")]
    public virtual string Species
    {
      get => this.\u003CSpecies\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CSpecies\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Species));
        this.\u003CSpecies\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Species);
      }
    }

    public virtual float TreeHeight
    {
      get => this.\u003CTreeHeight\u003Ek__BackingField;
      set
      {
        if (this.\u003CTreeHeight\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (TreeHeight));
        this.\u003CTreeHeight\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.TreeHeight);
      }
    }

    public virtual PctMidRange PercentImpervious
    {
      get => this.\u003CPercentImpervious\u003Ek__BackingField;
      set
      {
        if (this.\u003CPercentImpervious\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PercentImpervious));
        this.\u003CPercentImpervious\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PercentImpervious);
      }
    }

    public virtual PctMidRange PercentShrub
    {
      get => this.\u003CPercentShrub\u003Ek__BackingField;
      set
      {
        if (this.\u003CPercentShrub\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PercentShrub));
        this.\u003CPercentShrub\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PercentShrub);
      }
    }

    public virtual bool StreetTree
    {
      get => this.\u003CStreetTree\u003Ek__BackingField;
      set
      {
        if (this.\u003CStreetTree\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (StreetTree));
        this.\u003CStreetTree\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.StreetTree);
      }
    }

    public virtual SiteType SiteType
    {
      get => this.\u003CSiteType\u003Ek__BackingField;
      set
      {
        if (this.\u003CSiteType\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (SiteType));
        this.\u003CSiteType\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.SiteType);
      }
    }

    public virtual Street Street
    {
      get => this.\u003CStreet\u003Ek__BackingField;
      set
      {
        if (this.\u003CStreet\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Street));
        this.\u003CStreet\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Street);
      }
    }

    public virtual string Address
    {
      get => this.\u003CAddress\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CAddress\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Address));
        this.\u003CAddress\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Address);
      }
    }

    public virtual LocSite LocSite
    {
      get => this.\u003CLocSite\u003Ek__BackingField;
      set
      {
        if (this.\u003CLocSite\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (LocSite));
        this.\u003CLocSite\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.LocSite);
      }
    }

    public virtual int? LocNo
    {
      get => this.\u003CLocNo\u003Ek__BackingField;
      set
      {
        if (Nullable.Equals<int>(this.LocNo, value))
          return;
        this.OnPropertyChanging(nameof (LocNo));
        this.\u003CLocNo\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.LocNo);
      }
    }

    public virtual double? Latitude
    {
      get => this.\u003CLatitude\u003Ek__BackingField;
      set
      {
        if (Nullable.Equals<double>(this.Latitude, value))
          return;
        this.OnPropertyChanging(nameof (Latitude));
        this.\u003CLatitude\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Latitude);
      }
    }

    public virtual double? Longitude
    {
      get => this.\u003CLongitude\u003Ek__BackingField;
      set
      {
        if (Nullable.Equals<double>(this.Longitude, value))
          return;
        this.OnPropertyChanging(nameof (Longitude));
        this.\u003CLongitude\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Longitude);
      }
    }

    public virtual DateTime? SurveyDate
    {
      get => this.\u003CSurveyDate\u003Ek__BackingField;
      set
      {
        if (Nullable.Equals<DateTime>(this.SurveyDate, value))
          return;
        this.OnPropertyChanging(nameof (SurveyDate));
        this.\u003CSurveyDate\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.SurveyDate);
      }
    }

    public virtual bool NoteThisTree
    {
      get => this.\u003CNoteThisTree\u003Ek__BackingField;
      set
      {
        if (this.\u003CNoteThisTree\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (NoteThisTree));
        this.\u003CNoteThisTree\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.NoteThisTree);
      }
    }

    public virtual string Comments
    {
      get => this.\u003CComments\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CComments\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Comments));
        this.\u003CComments\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Comments);
      }
    }

    public virtual Guid? PriorYear
    {
      get => this.\u003CPriorYear\u003Ek__BackingField;
      set
      {
        if (Nullable.Equals<Guid>(this.PriorYear, value))
          return;
        this.OnPropertyChanging(nameof (PriorYear));
        this.\u003CPriorYear\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PriorYear);
      }
    }

    private void Init()
    {
      this.Stems = (ISet<Stem>) new HashSet<Stem>();
      this.Buildings = (ISet<Building>) new HashSet<Building>();
    }

    public virtual TreeDTO GetDTO()
    {
      TreeDTO treeDto = new TreeDTO();
      treeDto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      treeDto.Id = this.Id;
      treeDto.PlotLandUse = this.PlotLandUse != null ? Util.NullIfDefault<Guid>(this.PlotLandUse.Guid, new Guid()) : new Guid?();
      treeDto.CityManaged = this.CityManaged;
      treeDto.Direction = Util.NullIfDefault<int>(this.DirectionFromCenter, -1);
      treeDto.Distance = Util.NullIfDefault(this.DistanceFromCenter, -1f);
      treeDto.Status = Util.NullIfDefault<char>(this.Status, 'O');
      treeDto.Species = this.Species;
      treeDto.TreeHeight = Util.NullIfDefault(this.TreeHeight, -1f);
      treeDto.PercentImpervious = Util.NullIfDefault<int>((int) this.PercentImpervious, -1);
      treeDto.PercentShrub = Util.NullIfDefault<int>((int) this.PercentShrub, -1);
      treeDto.StreetTree = Util.NullIfDefault<bool>(this.StreetTree, false);
      treeDto.SiteType = this.SiteType != null ? Util.NullIfDefault<Guid>(this.SiteType.Guid, new Guid()) : new Guid?();
      treeDto.Street = this.Street != null ? Util.NullIfDefault<Guid>(this.Street.Guid, new Guid()) : new Guid?();
      treeDto.Address = this.Address;
      treeDto.LocSite = this.LocSite != null ? Util.NullIfDefault<Guid>(this.LocSite.Guid, new Guid()) : new Guid?();
      treeDto.LocNo = this.LocNo;
      treeDto.MaintRec = this.MaintRec != null ? Util.NullIfDefault<Guid>(this.MaintRec.Guid, new Guid()) : new Guid?();
      treeDto.MaintTask = this.MaintTask != null ? Util.NullIfDefault<Guid>(this.MaintTask.Guid, new Guid()) : new Guid?();
      treeDto.SidewalkDamage = this.SidewalkDamage != null ? Util.NullIfDefault<Guid>(this.SidewalkDamage.Guid, new Guid()) : new Guid?();
      treeDto.WireConflict = this.WireConflict != null ? Util.NullIfDefault<Guid>(this.WireConflict.Guid, new Guid()) : new Guid?();
      treeDto.OtherOne = this.OtherOne != null ? Util.NullIfDefault<Guid>(this.OtherOne.Guid, new Guid()) : new Guid?();
      treeDto.OtherTwo = this.OtherTwo != null ? Util.NullIfDefault<Guid>(this.OtherTwo.Guid, new Guid()) : new Guid?();
      treeDto.OtherThree = this.OtherThree != null ? Util.NullIfDefault<Guid>(this.OtherThree.Guid, new Guid()) : new Guid?();
      treeDto.Latitude = this.Latitude;
      treeDto.Longitude = this.Longitude;
      treeDto.SurveyDate = this.SurveyDate;
      treeDto.NoteThisTree = Util.NullIfDefault<bool>(this.NoteThisTree, true);
      treeDto.Comments = this.Comments;
      treeDto.Crown = this.Crown != null ? this.Crown.GetDTO() : (CrownDTO) null;
      treeDto.IPED = this.IPED != null ? this.IPED.GetDTO() : (IPEDDTO) null;
      treeDto.Stems = this.Stems.Count > 0 ? new List<StemDTO>() : (List<StemDTO>) null;
      treeDto.Buildings = this.Buildings.Count > 0 ? new List<BuildingDTO>() : (List<BuildingDTO>) null;
      treeDto.Revision = this.Revision;
      treeDto.PriorYear = this.PriorYear;
      TreeDTO dto = treeDto;
      foreach (Stem stem in (IEnumerable<Stem>) this.Stems)
        dto.Stems.Add(stem.GetDTO());
      foreach (Building building in (IEnumerable<Building>) this.Buildings)
        dto.Buildings.Add(building.GetDTO());
      return dto;
    }

    public override Tree Clone(bool deep) => Tree.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) Tree.Clone(this, new EntityMap());

    internal static Tree Clone(Tree t, EntityMap map, bool deep = true)
    {
      Tree eNew;
      if (map.Contains((Entity) t))
      {
        eNew = map.GetEntity<Tree>(t);
      }
      else
      {
        eNew = new Tree();
        eNew.Id = t.Id;
        eNew.UserId = t.UserId;
        eNew.CityManaged = t.CityManaged;
        eNew.DirectionFromCenter = t.DirectionFromCenter;
        eNew.DistanceFromCenter = t.DistanceFromCenter;
        eNew.Status = t.Status;
        eNew.Species = t.Species;
        eNew.TreeHeight = t.TreeHeight;
        eNew.PercentImpervious = t.PercentImpervious;
        eNew.PercentShrub = t.PercentShrub;
        eNew.StreetTree = t.StreetTree;
        eNew.Address = t.Address;
        eNew.LocNo = t.LocNo;
        eNew.Latitude = t.Latitude;
        eNew.Longitude = t.Longitude;
        eNew.SurveyDate = t.SurveyDate;
        eNew.NoteThisTree = t.NoteThisTree;
        eNew.Comments = t.Comments;
        eNew.Crown = t.Crown != null ? Crown.Clone(t.Crown, map) : (Crown) null;
        eNew.IPED = t.IPED != null ? t.IPED.Clone() : (IPED) null;
        eNew.PriorYear = t.PriorYear;
        map.Add((Entity) t, (Entity) eNew);
        if (deep)
        {
          foreach (Stem stem in (IEnumerable<Stem>) t.Stems)
            eNew.Stems.Add(Stem.Clone(stem, map));
          foreach (Building building in (IEnumerable<Building>) t.Buildings)
            eNew.Buildings.Add(Building.Clone(building, map));
        }
      }
      if (deep)
      {
        eNew.Plot = map.GetEntity<Plot>(t.Plot);
        eNew.PlotLandUse = map.GetEntity<PlotLandUse>(t.PlotLandUse);
        eNew.Street = map.GetEntity<Street>(t.Street);
        eNew.SiteType = map.GetEntity<SiteType>(t.SiteType);
        eNew.LocSite = map.GetEntity<LocSite>(t.LocSite);
        eNew.MaintRec = map.GetEntity<MaintRec>(t.MaintRec);
        eNew.MaintTask = map.GetEntity<MaintTask>(t.MaintTask);
        eNew.SidewalkDamage = map.GetEntity<Sidewalk>(t.SidewalkDamage);
        eNew.WireConflict = map.GetEntity<WireConflict>(t.WireConflict);
        eNew.OtherOne = map.GetEntity<OtherOne>(t.OtherOne);
        eNew.OtherTwo = map.GetEntity<OtherTwo>(t.OtherTwo);
        eNew.OtherThree = map.GetEntity<OtherThree>(t.OtherThree);
      }
      return eNew;
    }
  }
}
