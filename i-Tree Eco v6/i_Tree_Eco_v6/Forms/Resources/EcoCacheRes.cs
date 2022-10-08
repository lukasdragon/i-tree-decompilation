// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.Resources.EcoCacheRes
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace i_Tree_Eco_v6.Forms.Resources
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class EcoCacheRes
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal EcoCacheRes()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (EcoCacheRes.resourceMan == null)
          EcoCacheRes.resourceMan = new ResourceManager("i_Tree_Eco_v6.Forms.Resources.EcoCacheRes", typeof (EcoCacheRes).Assembly);
        return EcoCacheRes.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => EcoCacheRes.resourceCulture;
      set => EcoCacheRes.resourceCulture = value;
    }

    internal static string CacheEula => EcoCacheRes.ResourceManager.GetString(nameof (CacheEula), EcoCacheRes.resourceCulture);

    internal static string CacheServerError => EcoCacheRes.ResourceManager.GetString(nameof (CacheServerError), EcoCacheRes.resourceCulture);

    internal static string CacheTitle => EcoCacheRes.ResourceManager.GetString(nameof (CacheTitle), EcoCacheRes.resourceCulture);
  }
}
