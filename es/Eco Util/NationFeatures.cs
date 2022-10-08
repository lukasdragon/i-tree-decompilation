// Decompiled with JetBrains decompiler
// Type: Eco.Util.NationFeatures
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

namespace Eco.Util
{
  public class NationFeatures
  {
    private static NationFeatures.PlaceInfo USA = new NationFeatures.PlaceInfo("United States of America", "001", 219);
    public static NationFeatures.PlaceInfo defaultLocation = NationFeatures.USA;
    private static NationFeatures.PlaceInfo[] US_NonContinentalStates = new NationFeatures.PlaceInfo[5]
    {
      new NationFeatures.PlaceInfo("Alaska", "02", 236),
      new NationFeatures.PlaceInfo("Hawaii", "15", 256),
      new NationFeatures.PlaceInfo("Puerto Rico", "72", 303),
      new NationFeatures.PlaceInfo("Guam", "66", 90),
      new NationFeatures.PlaceInfo("Virgin Islands", "78", 226)
    };
    private static NationFeatures.PlaceInfo[] FourNations = new NationFeatures.PlaceInfo[9]
    {
      new NationFeatures.PlaceInfo("Canada", "002", 45),
      new NationFeatures.PlaceInfo("Australia", "230", 21),
      new NationFeatures.PlaceInfo("United Kingdom", "021", 218),
      new NationFeatures.PlaceInfo("Mexico", "107", 138),
      new NationFeatures.PlaceInfo("Columbia", "199", 52),
      new NationFeatures.PlaceInfo("Korea, Republic of", "132", 114),
      new NationFeatures.PlaceInfo("Japan", "138", 108),
      new NationFeatures.PlaceInfo("New Zealand", "092", 153),
      new NationFeatures.PlaceInfo("Ukraine", "023", 216)
    };
    private static NationFeatures.PlaceInfo[] EuropeanNations = new NationFeatures.PlaceInfo[33]
    {
      new NationFeatures.PlaceInfo("Austria", "229", 22),
      new NationFeatures.PlaceInfo("Belgium", "222", 29),
      new NationFeatures.PlaceInfo("Bulgaria", "212", 40),
      new NationFeatures.PlaceInfo("Croatia", "192", 59),
      new NationFeatures.PlaceInfo("Cyprus", "190", 61),
      new NationFeatures.PlaceInfo("Czech Republic", "189", 62),
      new NationFeatures.PlaceInfo("Denmark", "188", 63),
      new NationFeatures.PlaceInfo("Estonia", "178", 73),
      new NationFeatures.PlaceInfo("Finland", "173", 77),
      new NationFeatures.PlaceInfo("France", "172", 78),
      new NationFeatures.PlaceInfo("Germany", "165", 84),
      new NationFeatures.PlaceInfo("Greece", "162", 86),
      new NationFeatures.PlaceInfo("Hungary", "148", 98),
      new NationFeatures.PlaceInfo("Iceland", "147", 99),
      new NationFeatures.PlaceInfo("Ireland", "142", 104),
      new NationFeatures.PlaceInfo("Italy", "140", 106),
      new NationFeatures.PlaceInfo("Latvia", "128", 118),
      new NationFeatures.PlaceInfo("Lithuania", "122", 124),
      new NationFeatures.PlaceInfo("Luxembourg", "121", 125),
      new NationFeatures.PlaceInfo("Macedonia", "119", (int) sbyte.MaxValue),
      new NationFeatures.PlaceInfo("Malta", "113", 133),
      new NationFeatures.PlaceInfo("Netherlands", "095", 150),
      new NationFeatures.PlaceInfo("Norway", "085", 160),
      new NationFeatures.PlaceInfo("Poland", "074", 171),
      new NationFeatures.PlaceInfo("Portugal", "073", 172),
      new NationFeatures.PlaceInfo("Republic of Montenegro", "265", 65435),
      new NationFeatures.PlaceInfo("Romania", "069", 175),
      new NationFeatures.PlaceInfo("Slovakia", "053", 189),
      new NationFeatures.PlaceInfo("Slovenia", "052", 190),
      new NationFeatures.PlaceInfo("Spain", "047", 194),
      new NationFeatures.PlaceInfo("Sweden", "040", 199),
      new NationFeatures.PlaceInfo("Switzerland", "039", 200),
      new NationFeatures.PlaceInfo("Turkey", "028", 211)
    };
    private static NationFeatures.PlaceInfo Liechtenstein = new NationFeatures.PlaceInfo(nameof (Liechtenstein), "123", 123);

    public static bool isUsingBenMAPRegression(int NationLocId, int StateLocId)
    {
      if (NationLocId == NationFeatures.USA.LocId)
      {
        for (int index = 0; index < NationFeatures.US_NonContinentalStates.Length; ++index)
        {
          if (StateLocId == NationFeatures.US_NonContinentalStates[index].LocId)
            return true;
        }
        return false;
      }
      for (int index = 0; index < NationFeatures.FourNations.Length; ++index)
      {
        if (NationFeatures.FourNations[index].LocId == NationLocId)
          return true;
      }
      for (int index = 0; index < NationFeatures.EuropeanNations.Length; ++index)
      {
        if (NationFeatures.EuropeanNations[index].LocId == NationLocId)
          return true;
      }
      return false;
    }

    public static bool isUsingBenMAPRegression(string NationCode, string StateCode)
    {
      if (NationCode == NationFeatures.USA.Code)
      {
        for (int index = 0; index < NationFeatures.US_NonContinentalStates.Length; ++index)
        {
          if (StateCode == NationFeatures.US_NonContinentalStates[index].Code)
            return true;
        }
        return false;
      }
      for (int index = 0; index < NationFeatures.FourNations.Length; ++index)
      {
        if (NationFeatures.FourNations[index].Code == NationCode)
          return true;
      }
      for (int index = 0; index < NationFeatures.EuropeanNations.Length; ++index)
      {
        if (NationFeatures.EuropeanNations[index].Code == NationCode)
          return true;
      }
      return false;
    }

    public static bool isUsingBenMAPresults(int NationLocId)
    {
      if (NationLocId == NationFeatures.USA.LocId)
        return true;
      for (int index = 0; index < NationFeatures.FourNations.Length; ++index)
      {
        if (NationLocId == NationFeatures.FourNations[index].LocId)
          return true;
      }
      for (int index = 0; index < NationFeatures.EuropeanNations.Length; ++index)
      {
        if (NationLocId == NationFeatures.EuropeanNations[index].LocId)
          return true;
      }
      return false;
    }

    public static bool isUsingBenMAPresults(string NationCode)
    {
      if (NationCode == NationFeatures.USA.Code)
        return true;
      for (int index = 0; index < NationFeatures.FourNations.Length; ++index)
      {
        if (NationCode == NationFeatures.FourNations[index].Code)
          return true;
      }
      for (int index = 0; index < NationFeatures.EuropeanNations.Length; ++index)
      {
        if (NationCode == NationFeatures.EuropeanNations[index].Code)
          return true;
      }
      return false;
    }

    public static bool isUsingActualBenMAP(int NationLocId, int StateLocId)
    {
      if (NationLocId != NationFeatures.USA.LocId)
        return false;
      for (int index = 0; index < NationFeatures.US_NonContinentalStates.Length; ++index)
      {
        if (StateLocId == NationFeatures.US_NonContinentalStates[index].LocId)
          return false;
      }
      return true;
    }

    public static bool isUsingActualBenMAP(string NationCode, string StateCode)
    {
      if (!(NationCode == NationFeatures.USA.Code))
        return false;
      for (int index = 0; index < NationFeatures.US_NonContinentalStates.Length; ++index)
      {
        if (StateCode == NationFeatures.US_NonContinentalStates[index].Code)
          return false;
      }
      return true;
    }

    public static bool IsUSlikeNation(string NationCode)
    {
      for (int index = 0; index < NationFeatures.FourNations.Length; ++index)
      {
        if (NationFeatures.FourNations[index].Code == NationCode)
          return true;
      }
      for (int index = 0; index < NationFeatures.EuropeanNations.Length; ++index)
      {
        if (NationFeatures.EuropeanNations[index].Code == NationCode)
          return true;
      }
      return false;
    }

    public static bool IsUSlikeNation(int NationLocId)
    {
      for (int index = 0; index < NationFeatures.FourNations.Length; ++index)
      {
        if (NationFeatures.FourNations[index].LocId == NationLocId)
          return true;
      }
      for (int index = 0; index < NationFeatures.EuropeanNations.Length; ++index)
      {
        if (NationFeatures.EuropeanNations[index].LocId == NationLocId)
          return true;
      }
      return false;
    }

    public static bool IsUSorUSlikeNation(string NationCode)
    {
      if (NationCode == NationFeatures.USA.Code)
        return true;
      for (int index = 0; index < NationFeatures.FourNations.Length; ++index)
      {
        if (NationFeatures.FourNations[index].Code == NationCode)
          return true;
      }
      for (int index = 0; index < NationFeatures.EuropeanNations.Length; ++index)
      {
        if (NationFeatures.EuropeanNations[index].Code == NationCode)
          return true;
      }
      return false;
    }

    public static bool IsUSorUSlikeNation(int NationLocId)
    {
      if (NationLocId == NationFeatures.USA.LocId)
        return true;
      for (int index = 0; index < NationFeatures.FourNations.Length; ++index)
      {
        if (NationFeatures.FourNations[index].LocId == NationLocId)
          return true;
      }
      for (int index = 0; index < NationFeatures.EuropeanNations.Length; ++index)
      {
        if (NationFeatures.EuropeanNations[index].LocId == NationLocId)
          return true;
      }
      return false;
    }

    public static bool isGrassModelAvailable(int NationLocId) => NationLocId == NationFeatures.USA.LocId;

    public static bool isGrassModelAvailable(string NationCode) => NationCode == NationFeatures.USA.Code;

    public static bool isUVavailable(string NationCode)
    {
      if (NationCode == NationFeatures.USA.Code)
        return true;
      for (int index = 0; index < NationFeatures.FourNations.Length; ++index)
      {
        if (NationFeatures.FourNations[index].Code == NationCode)
          return true;
      }
      for (int index = 0; index < NationFeatures.EuropeanNations.Length; ++index)
      {
        if (NationFeatures.EuropeanNations[index].Code == NationCode)
          return true;
      }
      return NationFeatures.Liechtenstein.Code == NationCode;
    }

    public static bool isAustralia(string NationCode) => NationCode == "230";

    public static bool isUSA(string NationCode) => NationCode == "001";

    public class PlaceInfo
    {
      public string Name = "";
      public string Code = "";
      public int LocId;

      public PlaceInfo(string aName, string aCode, int anId)
      {
        this.Name = aName;
        this.Code = aCode;
        this.LocId = anId;
      }
    }
  }
}
