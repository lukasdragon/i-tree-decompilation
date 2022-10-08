// Decompiled with JetBrains decompiler
// Type: Streets.Domain.IPED
// Assembly: Streets.Domain, Version=1.1.5560.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C8C80D5-C46C-4F96-B160-AECDF3D99BE1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Streets.Domain.dll

namespace Streets.Domain
{
  public class IPED
  {
    private IPED()
    {
    }

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

    public virtual bool PestTS { get; protected set; }

    public virtual bool PestFT { get; protected set; }

    public virtual bool PestBB { get; protected set; }
  }
}
