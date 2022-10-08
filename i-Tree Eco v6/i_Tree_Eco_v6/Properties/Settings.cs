// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Properties.Settings
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.Win.C1Ribbon;
using Eco.Util;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace i_Tree_Eco_v6.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default => Settings.defaultInstance;

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool UseEnglishUnits
    {
      get => (bool) this[nameof (UseEnglishUnits)];
      set => this[nameof (UseEnglishUnits)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("CommonName")]
    public string SpeciesName
    {
      get => (string) this[nameof (SpeciesName)];
      set => this[nameof (SpeciesName)] = (object) value;
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("Data Source=https://data.itreetools.org/sql;Database=Pest;")]
    public string PestDb => (string) this[nameof (PestDb)];

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("Windows7")]
    public VisualStyle VisualStyle
    {
      get => (VisualStyle) this[nameof (VisualStyle)];
      set => this[nameof (VisualStyle)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("Azure")]
    public ThemeColor ThemeColor
    {
      get => (ThemeColor) this[nameof (ThemeColor)];
      set => this[nameof (ThemeColor)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("DarkGray")]
    public ThemeLightness ThemeLightness
    {
      get => (ThemeLightness) this[nameof (ThemeLightness)];
      set => this[nameof (ThemeLightness)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool AutoCheckUpdates
    {
      get => (bool) this[nameof (AutoCheckUpdates)];
      set => this[nameof (AutoCheckUpdates)] = (object) value;
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("https://www.itreetools.org/ecocache")]
    public string CacheUrl => (string) this[nameof (CacheUrl)];

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool ShowWhatsNewOnLaunch
    {
      get => (bool) this[nameof (ShowWhatsNewOnLaunch)];
      set => this[nameof (ShowWhatsNewOnLaunch)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool SetInitialOpenDirectory
    {
      get => (bool) this[nameof (SetInitialOpenDirectory)];
      set => this[nameof (SetInitialOpenDirectory)] = (object) value;
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("ftp://cicada.davey.com/fromuserssasv6")]
    public string FromUsersFtpUrl => (string) this[nameof (FromUsersFtpUrl)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("/eco/mobile_v6/")]
    public string MobileAppUrl => (string) this[nameof (MobileAppUrl)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("/smartphonecollection/dataconfigure.php")]
    public string MobileUploadUrl => (string) this[nameof (MobileUploadUrl)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("/smartphonecollection/datafetch.php")]
    public string MobileFetchUrl => (string) this[nameof (MobileFetchUrl)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("/smartphonecollection/dataretrieved.php")]
    public string MobileRetrievedUrl => (string) this[nameof (MobileRetrievedUrl)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("/smartphonecollection/resetpassword.php")]
    public string MobileResetPasswordUrl => (string) this[nameof (MobileResetPasswordUrl)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("i-Tree")]
    public string UserAgent => (string) this[nameof (UserAgent)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    public string Referer => (string) this[nameof (Referer)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("www.itreetools.org")]
    public string Host => (string) this[nameof (Host)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("https://")]
    public string Https => (string) this[nameof (Https)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("ftp://cicada.davey.com/fromuserssasv6international")]
    public string FromUsersFtpInternationalUrl => (string) this[nameof (FromUsersFtpInternationalUrl)];

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool MinimizeHelpPanel
    {
      get => (bool) this[nameof (MinimizeHelpPanel)];
      set => this[nameof (MinimizeHelpPanel)] = (object) value;
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("ftp://cicada.davey.com/tousersv6")]
    public string ToUsersFtpUrl => (string) this[nameof (ToUsersFtpUrl)];

    [UserScopedSetting]
    [DebuggerNonUserCode]
    public string[] MRUList
    {
      get => (string[]) this[nameof (MRUList)];
      set => this[nameof (MRUList)] = (object) value;
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("/xr.php")]
    public string ExchangeRateUrl => (string) this[nameof (ExchangeRateUrl)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0.1")]
    public double PlotSize => (double) this[nameof (PlotSize)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("http://www.itreetools.org/ecosampling/index.html?ecoversion=6&unit={0}&embeded={1}")]
    public string PlotSamplingUrl => (string) this[nameof (PlotSamplingUrl)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("http://")]
    public string Http => (string) this[nameof (Http)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>*.txt</string>\r\n</ArrayOfString>")]
    public string[] ExtText => (string[]) this[nameof (ExtText)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>*.prj</string>\r\n</ArrayOfString>")]
    public string[] ExtGISProj => (string[]) this[nameof (ExtGISProj)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>*.*</string>\r\n</ArrayOfString>")]
    public string[] ExtAllFiles => (string[]) this[nameof (ExtAllFiles)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>*.ieco</string>\r\n</ArrayOfString>")]
    public string[] ExtEcoProj => (string[]) this[nameof (ExtEcoProj)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("*.csv")]
    public string ExtCSV => (string) this[nameof (ExtCSV)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>*.kml</string>\r\n</ArrayOfString>")]
    public string[] ExtKML => (string[]) this[nameof (ExtKML)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>*.zip</string>\r\n</ArrayOfString>")]
    public string[] ExtZip => (string[]) this[nameof (ExtZip)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("*.png")]
    public string ExtPNG => (string) this[nameof (ExtPNG)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("*.jpg")]
    public string ExtJPG => (string) this[nameof (ExtJPG)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("*.xls")]
    public string ExtExcel => (string) this[nameof (ExtExcel)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("*.mdb")]
    public string ExtAccess => (string) this[nameof (ExtAccess)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("Data Source=https://data.itreetools.org/sql;Database=LocationSpecies;")]
    public string LocSpDb => (string) this[nameof (LocSpDb)];

    [UserScopedSetting]
    [DebuggerNonUserCode]
    public Submitter Submitter
    {
      get => (Submitter) this[nameof (Submitter)];
      set => this[nameof (Submitter)] = (object) value;
    }

    private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
    {
    }

    private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
    {
    }
  }
}
