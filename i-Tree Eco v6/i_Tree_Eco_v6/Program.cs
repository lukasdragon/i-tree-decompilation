// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Program
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using CefSharp;
using CefSharp.WinForms;
using DaveyTree.Controls;
using i_Tree_Eco_v6.Forms;
using i_Tree_Eco_v6.Resources;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Forms;

namespace i_Tree_Eco_v6
{
  internal static class Program
  {
    private static DataGridViewCellStyle _dgActiveColumnHeaderStyle;
    private static DataGridViewCellStyle _dgActiveRowHeaderStyle;
    private static DataGridViewCellStyle _dgInactiveColumnHeaderStyle;
    private static DataGridViewCellStyle _dgInactiveRowHeaderStyle;
    private static DataGridViewCellStyle _dgActiveCellStyle;
    private static DataGridViewCellStyle _dgInActiveCellStyle;

    [STAThread]
    public static void Main(string[] args)
    {
      RegistryView registryView = RegistryView.Registry64;
      try
      {
        new SystemComponents(registryView).ThrowIfInvalid();
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        if (!(ex is VersionException versionException))
          return;
        string component = versionException.Component;
        int num = (int) RTFMessageBox.Show(component == "OLEDB" || component == "DAO" ? Strings.ACE_VersionError : Strings.Component_VersionError.Replace("[[Component]]", versionException.Component).Replace("[[Version]]", versionException.VersionRequired.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      Program.EnableTLS();
      Version version = Program.FrameworkVersion();
      if (version == (Version) null || version < new Version(4, 6, 1))
      {
        int num1 = (int) MessageBox.Show(Strings.MsgSystemUnsupported, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      if (!Program.InitCefSharp())
        return;
      Application.Run((Form) new MainRibbonForm(args));
    }

    public static bool LaunchUpdater(Control owner = null)
    {
      bool flag = false;
      try
      {
        RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("Software\\i-Tree\\v3\\", false);
        if (registryKey == null)
        {
          int num = (int) MessageBox.Show(Strings.ErrNoVersionFound, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          string fileName = Path.Combine(Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Application.ExecutablePath)), "UpdateTool"), "iTreeUpdater.exe");
          flag = new Process()
          {
            StartInfo = new ProcessStartInfo(fileName, registryKey.GetValue("Version", (object) string.Empty).ToString())
          }.Start();
        }
      }
      catch (SecurityException ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) owner, Strings.ErrInsufficientPrivileges, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) owner, string.Format(Strings.ErrUnexpected, (object) ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return flag;
    }

    private static void EnableTLS()
    {
      SecurityProtocolType result;
      if (Enum.TryParse<SecurityProtocolType>("Tls12", out result))
        ServicePointManager.SecurityProtocol |= result;
      if (!Enum.TryParse<SecurityProtocolType>("Tls13", out result))
        return;
      ServicePointManager.SecurityProtocol |= result;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool InitCefSharp()
    {
      string path1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "i-Tree", "Eco");
      string str1 = Path.Combine(path1, "Temporary Internet Files");
      string str2 = Path.Combine(path1, "User Data");
      CefSettings settings = new CefSettings();
      settings.CachePath = str1;
      settings.UserDataPath = str2;
      settings.LogSeverity = LogSeverity.Disable;
      return Cef.Initialize((CefSettingsBase) settings);
    }

    private static Version FrameworkVersion()
    {
      string name = "Software\\Microsoft\\Net Framework Setup\\NDP\\v4\\Full";
      Version version = (Version) null;
      using (RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
      {
        using (RegistryKey registryKey2 = registryKey1.OpenSubKey(name, false))
        {
          if (registryKey2 != null)
          {
            object obj = registryKey2.GetValue("Version", (object) null);
            if (obj != null)
              version = new Version(obj.ToString());
          }
        }
      }
      return version;
    }

    public static ProgramSession Session => ProgramSession.GetInstance();

    public static DataGridViewCellStyle ActiveGridDefaultCellStyle
    {
      get
      {
        if (Program._dgActiveCellStyle == null)
          Program._dgActiveCellStyle = new DataGridViewCellStyle()
          {
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            BackColor = SystemColors.Window,
            Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0),
            ForeColor = SystemColors.ControlText,
            SelectionBackColor = SystemColors.Highlight,
            SelectionForeColor = SystemColors.HighlightText,
            WrapMode = DataGridViewTriState.False
          };
        return Program._dgActiveCellStyle;
      }
    }

    public static DataGridViewCellStyle InActiveGridDefaultCellStyle
    {
      get
      {
        if (Program._dgInActiveCellStyle == null)
          Program._dgInActiveCellStyle = new DataGridViewCellStyle()
          {
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            BackColor = System.Drawing.Color.WhiteSmoke,
            Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0),
            ForeColor = SystemColors.ControlText,
            SelectionBackColor = System.Drawing.Color.Gray,
            SelectionForeColor = System.Drawing.Color.Black,
            WrapMode = DataGridViewTriState.False
          };
        return Program._dgInActiveCellStyle;
      }
    }

    public static DataGridViewCellStyle ActiveGridColumnHeaderStyle
    {
      get
      {
        if (Program._dgActiveColumnHeaderStyle == null)
          Program._dgActiveColumnHeaderStyle = new DataGridViewCellStyle()
          {
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            BackColor = System.Drawing.Color.FromArgb(250, 250, 185),
            Font = new Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0),
            ForeColor = SystemColors.WindowText,
            SelectionBackColor = SystemColors.Highlight,
            SelectionForeColor = SystemColors.HighlightText,
            WrapMode = DataGridViewTriState.True,
            Padding = new Padding(5, 3, 5, 3)
          };
        return Program._dgActiveColumnHeaderStyle;
      }
    }

    public static DataGridViewCellStyle ActiveGridRowHeaderStyle
    {
      get
      {
        if (Program._dgActiveRowHeaderStyle == null)
          Program._dgActiveRowHeaderStyle = new DataGridViewCellStyle()
          {
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            BackColor = System.Drawing.Color.FromArgb(250, 250, 185),
            Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0),
            ForeColor = SystemColors.WindowText,
            SelectionBackColor = SystemColors.Highlight,
            SelectionForeColor = SystemColors.HighlightText,
            WrapMode = DataGridViewTriState.True
          };
        return Program._dgActiveRowHeaderStyle;
      }
    }

    public static DataGridViewCellStyle InActiveGridColumnHeaderStyle
    {
      get
      {
        if (Program._dgInactiveColumnHeaderStyle == null)
          Program._dgInactiveColumnHeaderStyle = new DataGridViewCellStyle()
          {
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            BackColor = System.Drawing.Color.LightGray,
            Font = new Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0),
            ForeColor = SystemColors.WindowText,
            SelectionBackColor = SystemColors.Highlight,
            SelectionForeColor = SystemColors.HighlightText,
            WrapMode = DataGridViewTriState.True,
            Padding = new Padding(5, 3, 5, 3)
          };
        return Program._dgInactiveColumnHeaderStyle;
      }
    }

    public static DataGridViewCellStyle InActiveGridRowHeaderStyle
    {
      get
      {
        if (Program._dgInactiveRowHeaderStyle == null)
          Program._dgInactiveRowHeaderStyle = new DataGridViewCellStyle()
          {
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            BackColor = System.Drawing.Color.LightGray,
            Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0),
            ForeColor = SystemColors.WindowText,
            SelectionBackColor = System.Drawing.Color.Gray,
            SelectionForeColor = System.Drawing.Color.Black,
            WrapMode = DataGridViewTriState.True
          };
        return Program._dgInactiveRowHeaderStyle;
      }
    }
  }
}
