// Decompiled with JetBrains decompiler
// Type: IPED.Domain.Properties.Resources
// Assembly: IPED.Domain, Version=1.1.6145.0, Culture=neutral, PublicKeyToken=null
// MVID: A1138CF7-F031-4F0B-8D41-1DE13D446B52
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\IPED.Domain.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace IPED.Domain.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (IPED.Domain.Properties.Resources.resourceMan == null)
          IPED.Domain.Properties.Resources.resourceMan = new ResourceManager("IPED.Domain.Properties.Resources", typeof (IPED.Domain.Properties.Resources).Assembly);
        return IPED.Domain.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => IPED.Domain.Properties.Resources.resourceCulture;
      set => IPED.Domain.Properties.Resources.resourceCulture = value;
    }

    internal static string Lookup_Code => IPED.Domain.Properties.Resources.ResourceManager.GetString(nameof (Lookup_Code), IPED.Domain.Properties.Resources.resourceCulture);

    internal static string Lookup_Description => IPED.Domain.Properties.Resources.ResourceManager.GetString(nameof (Lookup_Description), IPED.Domain.Properties.Resources.resourceCulture);

    internal static string Lookup_Id => IPED.Domain.Properties.Resources.ResourceManager.GetString(nameof (Lookup_Id), IPED.Domain.Properties.Resources.resourceCulture);

    internal static string Pest_CommonName => IPED.Domain.Properties.Resources.ResourceManager.GetString(nameof (Pest_CommonName), IPED.Domain.Properties.Resources.resourceCulture);

    internal static string Pest_Id => IPED.Domain.Properties.Resources.ResourceManager.GetString(nameof (Pest_Id), IPED.Domain.Properties.Resources.resourceCulture);

    internal static string Pest_ScientificName => IPED.Domain.Properties.Resources.ResourceManager.GetString(nameof (Pest_ScientificName), IPED.Domain.Properties.Resources.resourceCulture);
  }
}
