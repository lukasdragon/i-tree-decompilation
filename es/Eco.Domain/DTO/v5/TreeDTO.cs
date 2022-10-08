// Decompiled with JetBrains decompiler
// Type: Eco.Domain.DTO.v5.TreeDTO
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System.Collections.Generic;

namespace Eco.Domain.DTO.v5
{
  public class TreeDTO
  {
    public int Id;
    public char FieldLandUse;
    public int DirectionFromCenter = -1;
    public float DistanceFromCenter = -1f;
    public char Status = 'O';
    public string Species;
    public float CrownBaseHeight = -1f;
    public float CrownTopHeight = -1f;
    public float TreeHeight = -1f;
    public float CrownWidthNS = -1f;
    public float CrownWidthEW = -1f;
    public int CrownLightExposure = -1;
    public int CrownPosition = -1;
    public int PercentCrownMissing = -1;
    public int CrownTransparency = -1;
    public int CrownDensity = -1;
    public int CrownDieback = -1;
    public int PercentImpervious = -1;
    public int PercentShrub = -1;
    public char Site = 'N';
    public string Comments;
    public int PestTSDieback;
    public int PestTSEpiSprout;
    public int PestTSWiltFoli;
    public int PestTSEnvStress;
    public int PestTSHumStress;
    public string PestTSNotes;
    public int PestFTChewFoli;
    public int PestFTDiscFoli;
    public int PestFTAbnFoli;
    public int PestFTInsectSigns;
    public int PestFTFoliAffect;
    public string PestFTNotes;
    public int PestBBInsectSigns;
    public int PestBBInsectPres;
    public int PestBBDiseaseSigns;
    public int PestBBProbLoc;
    public int PestBBAbnGrowth;
    public string PestBBNotes;
    public int PestPest;
    public bool PestTS;
    public bool PestFT;
    public bool PestBB;
    public List<StemDTO> Stems;
    public List<BuildingInteractionDTO> Buildings;
    public State State;
  }
}
