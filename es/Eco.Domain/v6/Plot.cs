// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Plot
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO;
using Eco.Domain.DTO.v6;
using System;
using System.Collections.Generic;

namespace Eco.Domain.v6
{
  public class Plot : Entity<Plot>
  {
    public Plot()
    {
      this.PercentMeasured = 100;
      this.PercentPlantable = PctMidRange.PRINV;
      this.PercentShrubCover = PctMidRange.PRINV;
      this.PercentTreeCover = PctMidRange.PRINV;
      this.Size = -1f;
      this.Stake = false;
      this.IsComplete = false;
      this.Init();
    }

    public virtual Year Year
    {
      get => this.\u003CYear\u003Ek__BackingField;
      set
      {
        if (this.\u003CYear\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Year));
        this.\u003CYear\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Year);
      }
    }

    public virtual Strata Strata
    {
      get => this.\u003CStrata\u003Ek__BackingField;
      set
      {
        if (this.\u003CStrata\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Strata));
        this.\u003CStrata\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Strata);
      }
    }

    public virtual ProjectLocation ProjectLocation
    {
      get => this.\u003CProjectLocation\u003Ek__BackingField;
      set
      {
        if (this.\u003CProjectLocation\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (ProjectLocation));
        this.\u003CProjectLocation\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.ProjectLocation);
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

    public virtual ISet<Tree> Trees
    {
      get => this.\u003CTrees\u003Ek__BackingField;
      protected internal set
      {
        if (this.Trees == value)
          return;
        this.OnPropertyChanging(nameof (Trees));
        this.\u003CTrees\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Trees);
      }
    }

    public virtual ISet<Shrub> Shrubs
    {
      get => this.\u003CShrubs\u003Ek__BackingField;
      protected internal set
      {
        if (this.Shrubs == value)
          return;
        this.OnPropertyChanging(nameof (Shrubs));
        this.\u003CShrubs\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Shrubs);
      }
    }

    public virtual ISet<PlotGroundCover> PlotGroundCovers
    {
      get => this.\u003CPlotGroundCovers\u003Ek__BackingField;
      protected internal set
      {
        if (this.PlotGroundCovers == value)
          return;
        this.OnPropertyChanging(nameof (PlotGroundCovers));
        this.\u003CPlotGroundCovers\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PlotGroundCovers);
      }
    }

    public virtual ISet<ReferenceObject> ReferenceObjects
    {
      get => this.\u003CReferenceObjects\u003Ek__BackingField;
      protected internal set
      {
        if (this.ReferenceObjects == value)
          return;
        this.OnPropertyChanging(nameof (ReferenceObjects));
        this.\u003CReferenceObjects\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.ReferenceObjects);
      }
    }

    public virtual ISet<PlotLandUse> PlotLandUses
    {
      get => this.\u003CPlotLandUses\u003Ek__BackingField;
      protected internal set
      {
        if (this.PlotLandUses == value)
          return;
        this.OnPropertyChanging(nameof (PlotLandUses));
        this.\u003CPlotLandUses\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PlotLandUses);
      }
    }

    public virtual ISet<PlantingSite> PlantingSites
    {
      get => this.\u003CPlantingSites\u003Ek__BackingField;
      protected internal set
      {
        if (this.PlantingSites == value)
          return;
        this.OnPropertyChanging(nameof (PlantingSites));
        this.\u003CPlantingSites\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PlantingSites);
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

    public virtual DateTime? Date
    {
      get => this.\u003CDate\u003Ek__BackingField;
      set
      {
        if (Nullable.Equals<DateTime>(this.Date, value))
          return;
        this.OnPropertyChanging(nameof (Date));
        this.\u003CDate\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Date);
      }
    }

    public virtual string Crew
    {
      get => this.\u003CCrew\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CCrew\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Crew));
        this.\u003CCrew\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Crew);
      }
    }

    public virtual string ContactInfo
    {
      get => this.\u003CContactInfo\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CContactInfo\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (ContactInfo));
        this.\u003CContactInfo\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.ContactInfo);
      }
    }

    public virtual float Size
    {
      get => this.\u003CSize\u003Ek__BackingField;
      set
      {
        if (this.\u003CSize\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Size));
        this.\u003CSize\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Size);
      }
    }

    public virtual string Photo
    {
      get => this.\u003CPhoto\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CPhoto\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Photo));
        this.\u003CPhoto\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Photo);
      }
    }

    public virtual bool Stake
    {
      get => this.\u003CStake\u003Ek__BackingField;
      set
      {
        if (this.\u003CStake\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Stake));
        this.\u003CStake\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Stake);
      }
    }

    public virtual PctMidRange PercentTreeCover
    {
      get => this.\u003CPercentTreeCover\u003Ek__BackingField;
      set
      {
        if (this.\u003CPercentTreeCover\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PercentTreeCover));
        this.\u003CPercentTreeCover\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PercentTreeCover);
      }
    }

    public virtual PctMidRange PercentShrubCover
    {
      get => this.\u003CPercentShrubCover\u003Ek__BackingField;
      set
      {
        if (this.\u003CPercentShrubCover\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PercentShrubCover));
        this.\u003CPercentShrubCover\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PercentShrubCover);
      }
    }

    public virtual PctMidRange PercentPlantable
    {
      get => this.\u003CPercentPlantable\u003Ek__BackingField;
      set
      {
        if (this.\u003CPercentPlantable\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PercentPlantable));
        this.\u003CPercentPlantable\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PercentPlantable);
      }
    }

    public virtual int PercentMeasured
    {
      get => this.\u003CPercentMeasured\u003Ek__BackingField;
      set
      {
        if (this.\u003CPercentMeasured\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PercentMeasured));
        this.\u003CPercentMeasured\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PercentMeasured);
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

    public virtual bool IsComplete
    {
      get => this.\u003CIsComplete\u003Ek__BackingField;
      set
      {
        if (this.\u003CIsComplete\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (IsComplete));
        this.\u003CIsComplete\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.IsComplete);
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
      this.Trees = (ISet<Tree>) new HashSet<Tree>();
      this.Shrubs = (ISet<Shrub>) new HashSet<Shrub>();
      this.PlotGroundCovers = (ISet<PlotGroundCover>) new HashSet<PlotGroundCover>();
      this.ReferenceObjects = (ISet<ReferenceObject>) new HashSet<ReferenceObject>();
      this.PlotLandUses = (ISet<PlotLandUse>) new HashSet<PlotLandUse>();
      this.PlantingSites = (ISet<PlantingSite>) new HashSet<PlantingSite>();
    }

    public virtual PlotDTO GetDTO()
    {
      PlotDTO plotDto = new PlotDTO();
      plotDto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      plotDto.Id = this.Id;
      plotDto.Street = this.Street != null ? Util.NullIfDefault<Guid>(this.Street.Guid, new Guid()) : new Guid?();
      plotDto.Address = this.Address;
      plotDto.Comments = this.Comments;
      plotDto.ContactInfo = this.ContactInfo;
      plotDto.Crew = this.Crew;
      plotDto.Date = this.Date;
      plotDto.Latitude = this.Latitude;
      plotDto.Longitude = this.Longitude;
      plotDto.PercentMeasured = Util.NullIfDefault<int>(this.PercentMeasured, 100);
      plotDto.PercentPlantable = Util.NullIfDefault<short>((short) this.PercentPlantable, (short) -1);
      plotDto.PercentShrubCover = Util.NullIfDefault<short>((short) this.PercentShrubCover, (short) -1);
      plotDto.PercentTreeCover = Util.NullIfDefault<short>((short) this.PercentTreeCover, (short) -1);
      plotDto.IsComplete = Util.NullIfDefault<bool>(this.IsComplete, false);
      plotDto.Photo = this.Photo;
      plotDto.Size = Util.NullIfDefault(this.Size, -1f);
      plotDto.Stake = Util.NullIfDefault<bool>(this.Stake, false);
      plotDto.Strata = this.Strata != null ? new Guid?(this.Strata.Guid) : new Guid?();
      plotDto.Revision = this.Revision;
      plotDto.PlotGroundCovers = this.PlotGroundCovers.Count > 0 ? new List<PlotGroundCoverDTO>() : (List<PlotGroundCoverDTO>) null;
      plotDto.LandUses = this.PlotLandUses.Count > 0 ? new List<PlotLandUseDTO>() : (List<PlotLandUseDTO>) null;
      plotDto.ReferenceObjects = this.ReferenceObjects.Count > 0 ? new List<ReferenceObjectDTO>() : (List<ReferenceObjectDTO>) null;
      plotDto.Shrubs = this.Shrubs.Count > 0 ? new List<ShrubDTO>() : (List<ShrubDTO>) null;
      plotDto.Trees = this.Trees.Count > 0 ? new List<TreeDTO>() : (List<TreeDTO>) null;
      plotDto.PlantingSites = this.PlantingSites.Count > 0 ? new List<PlantingSiteDTO>() : (List<PlantingSiteDTO>) null;
      plotDto.PriorYear = this.PriorYear;
      PlotDTO dto = plotDto;
      foreach (PlotGroundCover plotGroundCover in (IEnumerable<PlotGroundCover>) this.PlotGroundCovers)
        dto.PlotGroundCovers.Add(plotGroundCover.GetDTO());
      foreach (PlotLandUse plotLandUse in (IEnumerable<PlotLandUse>) this.PlotLandUses)
        dto.LandUses.Add(plotLandUse.GetDTO());
      foreach (ReferenceObject referenceObject in (IEnumerable<ReferenceObject>) this.ReferenceObjects)
        dto.ReferenceObjects.Add(referenceObject.GetDTO());
      foreach (Shrub shrub in (IEnumerable<Shrub>) this.Shrubs)
        dto.Shrubs.Add(shrub.GetDTO());
      foreach (Tree tree in (IEnumerable<Tree>) this.Trees)
        dto.Trees.Add(tree.GetDTO());
      foreach (PlantingSite plantingSite in (IEnumerable<PlantingSite>) this.PlantingSites)
        dto.PlantingSites.Add(plantingSite.GetDTO());
      return dto;
    }

    public override Plot Clone(bool deep) => Plot.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) Plot.Clone(this, new EntityMap());

    internal static Plot Clone(Plot p, EntityMap map, bool deep = true)
    {
      Plot eNew;
      if (map.Contains((Entity) p))
      {
        eNew = map.GetEntity<Plot>(p);
      }
      else
      {
        eNew = new Plot();
        eNew.Id = p.Id;
        eNew.Address = p.Address;
        eNew.Comments = p.Comments;
        eNew.ContactInfo = p.ContactInfo;
        eNew.Crew = p.Crew;
        eNew.Date = p.Date;
        eNew.IsComplete = p.IsComplete;
        eNew.Latitude = p.Latitude;
        eNew.Longitude = p.Longitude;
        eNew.PercentMeasured = p.PercentMeasured;
        eNew.PercentPlantable = p.PercentPlantable;
        eNew.PercentShrubCover = p.PercentShrubCover;
        eNew.PercentTreeCover = p.PercentTreeCover;
        eNew.Photo = p.Photo;
        eNew.Size = p.Size;
        eNew.Stake = p.Stake;
        eNew.Revision = 0;
        eNew.PriorYear = p.PriorYear;
        map.Add((Entity) p, (Entity) eNew);
        if (deep)
        {
          eNew.ProjectLocation = map.GetEntity<ProjectLocation>(p.ProjectLocation);
          foreach (PlotLandUse plotLandUse in (IEnumerable<PlotLandUse>) p.PlotLandUses)
            eNew.PlotLandUses.Add(PlotLandUse.Clone(plotLandUse, map));
          foreach (PlotGroundCover plotGroundCover in (IEnumerable<PlotGroundCover>) p.PlotGroundCovers)
            eNew.PlotGroundCovers.Add(PlotGroundCover.Clone(plotGroundCover, map));
          foreach (Tree tree in (IEnumerable<Tree>) p.Trees)
            eNew.Trees.Add(Tree.Clone(tree, map));
          foreach (PlantingSite plantingSite in (IEnumerable<PlantingSite>) p.PlantingSites)
            eNew.PlantingSites.Add(PlantingSite.Clone(plantingSite, map));
          foreach (Shrub shrub in (IEnumerable<Shrub>) p.Shrubs)
            eNew.Shrubs.Add(Shrub.Clone(shrub, map));
          foreach (ReferenceObject referenceObject in (IEnumerable<ReferenceObject>) p.ReferenceObjects)
            eNew.ReferenceObjects.Add(ReferenceObject.Clone(referenceObject, map));
        }
      }
      if (deep)
      {
        eNew.Year = map.GetEntity<Year>(p.Year);
        eNew.Strata = map.GetEntity<Strata>(p.Strata);
      }
      return eNew;
    }
  }
}
