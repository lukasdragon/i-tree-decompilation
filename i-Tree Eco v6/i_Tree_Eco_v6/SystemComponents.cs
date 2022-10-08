// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.SystemComponents
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace i_Tree_Eco_v6
{
  public class SystemComponents
  {
    private const string ACE_DAO_CLSID = "{CD7791B9-43FD-42C5-AE42-8DD2811F0419}";
    private const string ACE_OLEDB_CLSID = "{3BE786A0-0366-4F5C-9434-25CF162E475E}";
    private static readonly Version REQ_ACE_VERSION = new Version("14.0.7010.1000");
    private static readonly Version REQ_MDAC_VERSION = new Version("2.8");

    public SystemComponents(RegistryView registryView)
    {
      using (RegistryKey rkHKCR = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, registryView))
      {
        this.Dao = SystemComponents.GetComponentVersion(rkHKCR, "{CD7791B9-43FD-42C5-AE42-8DD2811F0419}");
        this.OleDb = SystemComponents.GetComponentVersion(rkHKCR, "{3BE786A0-0366-4F5C-9434-25CF162E475E}");
      }
      using (RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
      {
        using (RegistryKey registryKey2 = registryKey1.OpenSubKey("Software\\Microsoft\\DataAccess", false))
        {
          if (registryKey2 == null)
            return;
          object obj = registryKey2.GetValue("FullInstallVer");
          if (obj == null)
            return;
          this.Mdac = new Version(obj.ToString());
        }
      }
    }

    public Version OleDb { get; private set; }

    public Version Dao { get; private set; }

    public Version Mdac { get; private set; }

    public bool IsValid => this.OleDb != (Version) null && this.OleDb >= SystemComponents.REQ_ACE_VERSION && this.Dao != (Version) null && this.Dao >= SystemComponents.REQ_ACE_VERSION && this.Mdac != (Version) null && this.Mdac >= SystemComponents.REQ_MDAC_VERSION;

    public void ThrowIfInvalid()
    {
      if (this.OleDb == (Version) null || this.OleDb < SystemComponents.REQ_ACE_VERSION)
        throw new VersionException("OLEDB", SystemComponents.REQ_ACE_VERSION, this.OleDb);
      if (this.Dao == (Version) null || this.Dao < SystemComponents.REQ_ACE_VERSION)
        throw new VersionException("DAO", SystemComponents.REQ_ACE_VERSION, this.Dao);
      if (this.Mdac == (Version) null || this.Mdac < SystemComponents.REQ_MDAC_VERSION)
        throw new VersionException("MDAC", SystemComponents.REQ_MDAC_VERSION, this.Mdac);
    }

    private static Version GetComponentVersion(RegistryKey rkHKCR, string clsid)
    {
      Version componentVersion = (Version) null;
      string name = string.Format("CLSID\\{0}\\InprocServer32", (object) clsid);
      using (RegistryKey registryKey = rkHKCR.OpenSubKey(name, false))
      {
        if (registryKey != null)
        {
          object obj = registryKey.GetValue((string) null);
          if (obj != null)
          {
            string str = obj.ToString();
            if (!string.IsNullOrEmpty(str))
            {
              if (File.Exists(str))
              {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(str);
                componentVersion = new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart);
              }
            }
          }
        }
      }
      return componentVersion;
    }
  }
}
