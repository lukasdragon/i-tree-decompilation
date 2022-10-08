// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.IPED
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO;
using Eco.Domain.DTO.v6;

namespace Eco.Domain.v6
{
  public class IPED
  {
    public virtual int TSDieback { get; set; }

    public virtual int TSEpiSprout { get; set; }

    public virtual int TSWiltFoli { get; set; }

    public virtual int TSEnvStress { get; set; }

    public virtual int TSHumStress { get; set; }

    public virtual string TSNotes { get; set; }

    public virtual int FTChewFoli { get; set; }

    public virtual int FTDiscFoli { get; set; }

    public virtual int FTAbnFoli { get; set; }

    public virtual int FTInsectSigns { get; set; }

    public virtual int FTFoliAffect { get; set; }

    public virtual string FTNotes { get; set; }

    public virtual int BBInsectSigns { get; set; }

    public virtual int BBInsectPres { get; set; }

    public virtual int BBDiseaseSigns { get; set; }

    public virtual int BBProbLoc { get; set; }

    public virtual int BBAbnGrowth { get; set; }

    public virtual string BBNotes { get; set; }

    public virtual int Pest { get; set; }

    public virtual IPEDDTO GetDTO() => new IPEDDTO()
    {
      TSDieback = Util.NullIfDefault<int>(this.TSDieback, 0),
      TSEpiSprout = Util.NullIfDefault<int>(this.TSEpiSprout, 0),
      TSWiltFoli = Util.NullIfDefault<int>(this.TSWiltFoli, 0),
      TSEnvStress = Util.NullIfDefault<int>(this.TSEnvStress, 0),
      TSHumStress = Util.NullIfDefault<int>(this.TSHumStress, 0),
      TSNotes = this.TSNotes,
      FTChewFoli = Util.NullIfDefault<int>(this.FTChewFoli, 0),
      FTDiscFoli = Util.NullIfDefault<int>(this.FTDiscFoli, 0),
      FTAbnFoli = Util.NullIfDefault<int>(this.FTAbnFoli, 0),
      FTInsectSigns = Util.NullIfDefault<int>(this.FTInsectSigns, 0),
      FTFoliAffect = Util.NullIfDefault<int>(this.FTFoliAffect, 0),
      FTNotes = this.FTNotes,
      BBInsectSigns = Util.NullIfDefault<int>(this.BBInsectSigns, 0),
      BBInsectPres = Util.NullIfDefault<int>(this.BBInsectPres, 0),
      BBDiseaseSigns = Util.NullIfDefault<int>(this.BBDiseaseSigns, 0),
      BBProbLoc = Util.NullIfDefault<int>(this.BBProbLoc, 0),
      BBAbnGrowth = Util.NullIfDefault<int>(this.BBAbnGrowth, 0),
      BBNotes = this.BBNotes,
      Pest = Util.NullIfDefault<int>(this.Pest, 0)
    };

    public virtual IPED Clone() => new IPED()
    {
      TSDieback = this.TSDieback,
      TSEpiSprout = this.TSEpiSprout,
      TSWiltFoli = this.TSWiltFoli,
      TSEnvStress = this.TSEnvStress,
      TSHumStress = this.TSHumStress,
      TSNotes = this.TSNotes,
      FTChewFoli = this.FTChewFoli,
      FTDiscFoli = this.FTDiscFoli,
      FTAbnFoli = this.FTAbnFoli,
      FTInsectSigns = this.FTInsectSigns,
      FTFoliAffect = this.FTFoliAffect,
      FTNotes = this.FTNotes,
      BBInsectSigns = this.BBInsectSigns,
      BBInsectPres = this.BBInsectPres,
      BBDiseaseSigns = this.BBDiseaseSigns,
      BBProbLoc = this.BBProbLoc,
      BBAbnGrowth = this.BBAbnGrowth,
      BBNotes = this.BBNotes,
      Pest = this.Pest
    };
  }
}
