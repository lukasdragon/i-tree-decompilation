// Decompiled with JetBrains decompiler
// Type: Eco.Util.Strings
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Eco.Util
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Strings
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Strings()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Strings.resourceMan == null)
          Strings.resourceMan = new ResourceManager("Eco.Util.Strings", typeof (Strings).Assembly);
        return Strings.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Strings.resourceCulture;
      set => Strings.resourceCulture = value;
    }

    internal static string ErrConvert => Strings.ResourceManager.GetString(nameof (ErrConvert), Strings.resourceCulture);

    internal static string ErrNoValue => Strings.ResourceManager.GetString(nameof (ErrNoValue), Strings.resourceCulture);

    internal static string ErrRequired => Strings.ResourceManager.GetString(nameof (ErrRequired), Strings.resourceCulture);

    internal static string ErrUnmapped => Strings.ResourceManager.GetString(nameof (ErrUnmapped), Strings.resourceCulture);

    internal static string HealthClassLookupErrorMessage => Strings.ResourceManager.GetString(nameof (HealthClassLookupErrorMessage), Strings.resourceCulture);

    internal static string InternetConnectionIssue => Strings.ResourceManager.GetString(nameof (InternetConnectionIssue), Strings.resourceCulture);

    internal static string MsgAnalyzingData => Strings.ResourceManager.GetString(nameof (MsgAnalyzingData), Strings.resourceCulture);

    internal static string MsgArgumentNotDefined => Strings.ResourceManager.GetString(nameof (MsgArgumentNotDefined), Strings.resourceCulture);

    internal static string MsgInvalidParameters => Strings.ResourceManager.GetString(nameof (MsgInvalidParameters), Strings.resourceCulture);

    internal static string TypeDateTime => Strings.ResourceManager.GetString(nameof (TypeDateTime), Strings.resourceCulture);

    internal static string TypeDecimal => Strings.ResourceManager.GetString(nameof (TypeDecimal), Strings.resourceCulture);

    internal static string TypeInteger => Strings.ResourceManager.GetString(nameof (TypeInteger), Strings.resourceCulture);

    internal static string TypePositiveInteger => Strings.ResourceManager.GetString(nameof (TypePositiveInteger), Strings.resourceCulture);

    internal static string TypeString => Strings.ResourceManager.GetString(nameof (TypeString), Strings.resourceCulture);
  }
}
