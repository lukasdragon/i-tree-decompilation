// Decompiled with JetBrains decompiler
// Type: Eco.Util.EquationTypes
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

namespace Eco.Util
{
  public enum EquationTypes
  {
    None = 1,
    Percent = 2,
    SampleMean = 5,
    SampleCovarinace = 7,
    StratumSampleMeanEstimator = 8,
    PerAreaStratumSampleMeanEstimator = 10, // 0x0000000A
    PercentStratumSampleMeanEstimator = 12, // 0x0000000C
    TotalStratumEstimator = 14, // 0x0000000E
    RatioStratumEstimator = 16, // 0x00000010
    StrataSampleMeanEstimator = 18, // 0x00000012
    PerAreaStrataSampleMeanEstimator = 20, // 0x00000014
    PercentStrataSampleMeanEstimator = 22, // 0x00000016
    TotalStrataEstimator = 24, // 0x00000018
    RatioStrataEstimator = 26, // 0x0000001A
    ShannonWienerDiversityIndex = 50, // 0x00000032
    MenhinickDiversityIndex = 51, // 0x00000033
    SimpsonDiversityIndex = 52, // 0x00000034
    ShannonWienerEvennessIndex = 53, // 0x00000035
    SandersRarefractionTechnique = 54, // 0x00000036
    SpeciesRichness = 55, // 0x00000037
  }
}
