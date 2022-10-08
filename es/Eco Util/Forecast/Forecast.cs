// Decompiled with JetBrains decompiler
// Type: Eco.Util.Forecast.Forecast
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Eco.Util.Forecast
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Forecast
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Forecast()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Eco.Util.Forecast.Forecast.resourceMan == null)
          Eco.Util.Forecast.Forecast.resourceMan = new ResourceManager("Eco.Util.Forecast.Forecast", typeof (Eco.Util.Forecast.Forecast).Assembly);
        return Eco.Util.Forecast.Forecast.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Eco.Util.Forecast.Forecast.resourceCulture;
      set => Eco.Util.Forecast.Forecast.resourceCulture = value;
    }

    internal static string Class1Hurricane => Eco.Util.Forecast.Forecast.ResourceManager.GetString(nameof (Class1Hurricane), Eco.Util.Forecast.Forecast.resourceCulture);

    internal static string Class2Hurricane => Eco.Util.Forecast.Forecast.ResourceManager.GetString(nameof (Class2Hurricane), Eco.Util.Forecast.Forecast.resourceCulture);

    internal static string Class3Hurricane => Eco.Util.Forecast.Forecast.ResourceManager.GetString(nameof (Class3Hurricane), Eco.Util.Forecast.Forecast.resourceCulture);

    internal static string Class4Hurricane => Eco.Util.Forecast.Forecast.ResourceManager.GetString(nameof (Class4Hurricane), Eco.Util.Forecast.Forecast.resourceCulture);

    internal static string Class5Hurricane => Eco.Util.Forecast.Forecast.ResourceManager.GetString(nameof (Class5Hurricane), Eco.Util.Forecast.Forecast.resourceCulture);

    internal static string TropicalDepression => Eco.Util.Forecast.Forecast.ResourceManager.GetString(nameof (TropicalDepression), Eco.Util.Forecast.Forecast.resourceCulture);

    internal static string TropicalStorm => Eco.Util.Forecast.Forecast.ResourceManager.GetString(nameof (TropicalStorm), Eco.Util.Forecast.Forecast.resourceCulture);

    internal static string WindStorm => Eco.Util.Forecast.Forecast.ResourceManager.GetString(nameof (WindStorm), Eco.Util.Forecast.Forecast.resourceCulture);
  }
}
