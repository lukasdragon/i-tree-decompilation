// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.ProgramSession
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.Win.C1Ribbon;
using DaveyTree.Threading.Tasks.Schedulers;
using Eco.Util;
using Eco.Util.Cache;
using Eco.Util.Services;
using Eco.Util.Views;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using SqlWeb.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6
{
  public class ProgramSession : IDisposable
  {
    private static InputSession _inputSession;
    private static ProgramSession _instance;
    private int _initialized;
    private ISessionFactory m_sfLocSp;
    private ISessionFactory m_sfPest;
    private SpeciesService _speciesService;
    private Dictionary<string, SpeciesView> _species;
    private Dictionary<string, SpeciesView> _invalidSpecies;
    private IPEDData _iped;
    private object _syncobj;
    public EcoCache m_cache;
    private CultureInfo m_spCulture;
    private Version m_spVersion;
    private Version m_locVersion;
    private bool m_disposed;

    public event EventHandler InputSessionChanged;

    private ProgramSession() => this._syncobj = new object();

    private void Initialize()
    {
      if (Interlocked.CompareExchange(ref this._initialized, 1, 0) != 0)
        return;
      lock (this._syncobj)
      {
        this.InitLocSpDb();
        this.InitPestDb();
        this.InitSpecies();
      }
    }

    private string GetConfigFile(Assembly lib)
    {
      FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(typeof (Configuration).Assembly.Location);
      return Path.Combine(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "i-Tree", "Eco"), string.Format("{0}.{1}.cfg", (object) lib.GetName().Name, (object) versionInfo.FileVersion));
    }

    private bool ConfigValid(string cfgFile, Assembly lib)
    {
      if (!File.Exists(cfgFile))
        return false;
      FileInfo fileInfo = new FileInfo(cfgFile);
      return new FileInfo(lib.Location).LastWriteTime == fileInfo.LastWriteTime;
    }

    private Configuration GetNHConfig(Assembly lib)
    {
      string configFile = this.GetConfigFile(lib);
      Configuration nhConfig = (Configuration) null;
      if (this.ConfigValid(configFile, lib))
      {
        try
        {
          using (FileStream serializationStream = new FileStream(configFile, FileMode.Open))
            nhConfig = new BinaryFormatter().Deserialize((Stream) serializationStream) as Configuration;
        }
        catch (SerializationException ex)
        {
          nhConfig = (Configuration) null;
        }
      }
      return nhConfig;
    }

    private void UpdateNHConfig(Assembly lib, Configuration config)
    {
      string configFile = this.GetConfigFile(lib);
      string directoryName = Path.GetDirectoryName(configFile);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      using (FileStream serializationStream = new FileStream(configFile, FileMode.Create))
        new BinaryFormatter().Serialize((Stream) serializationStream, (object) config);
      FileInfo fileInfo = new FileInfo(lib.Location);
      File.SetLastWriteTime(configFile, fileInfo.LastWriteTime);
    }

    private void InitLocSpDb()
    {
      Assembly assembly = typeof (Location).Assembly;
      Configuration config = this.GetNHConfig(assembly);
      if (config == null)
      {
        config = new Configuration().Configure(Assembly.GetExecutingAssembly(), "i_Tree_Eco_v6.LocationSpecies.cfg.xml");
        config.Properties["connection.connection_string"] = i_Tree_Eco_v6.Properties.Settings.Default.LocSpDb;
        this.UpdateNHConfig(assembly, config);
      }
      this.VerifyLocationSpeciesVersion(config, assembly);
      this.m_sfLocSp = config.BuildSessionFactory();
      this.ValidateLocSpCache(config);
    }

    private void InitPestDb()
    {
      Assembly assembly = typeof (IPED.Domain.Pest).Assembly;
      Configuration config = this.GetNHConfig(assembly);
      if (config == null)
      {
        config = new Configuration().Configure(Assembly.GetExecutingAssembly(), "i_Tree_Eco_v6.Pest.cfg.xml");
        config.Properties["connection.connection_string"] = i_Tree_Eco_v6.Properties.Settings.Default.PestDb;
        this.UpdateNHConfig(assembly, config);
      }
      this.m_sfPest = config.BuildSessionFactory();
    }

    private void ValidateLocSpCache(Configuration config)
    {
      Version locSpCacheVersion = this.GetLocSpCacheVersion();
      if (!(this.GetLocationSpeciesDbVersion(config) != locSpCacheVersion))
        return;
      this.ClearCache(this.m_sfLocSp);
    }

    private Version GetLocSpCacheVersion() => RetryExecutionHandler.Execute<Version>((Func<Version>) (() =>
    {
      using (ISession session = this.m_sfLocSp.OpenSession())
      {
        using (session.BeginTransaction())
        {
          RegistryEntry registryEntry = session.Get<RegistryEntry>((object) 4);
          return !string.IsNullOrEmpty(registryEntry.Value) ? new Version(registryEntry.Value) : (Version) null;
        }
      }
    }));

    private void ClearCache(ISessionFactory sessionFactory)
    {
      foreach (string key in (IEnumerable<string>) sessionFactory.GetAllClassMetadata().Keys)
        sessionFactory.EvictEntity(key);
      foreach (string key in (IEnumerable<string>) this.m_sfLocSp.GetAllCollectionMetadata().Keys)
        sessionFactory.EvictCollection(key);
      sessionFactory.EvictQueries();
    }

    private void VerifyLocationSpeciesVersion(Configuration config, Assembly assembly)
    {
      Version speciesDbVersion = this.GetLocationSpeciesDbVersion(config);
      Version assemblyProductVersion = this.GetAssemblyProductVersion(assembly);
      if (speciesDbVersion.Major != assemblyProductVersion.Major)
        throw new VersionException("i-Tree Database", assemblyProductVersion, speciesDbVersion);
    }

    private Version GetLocationSpeciesDbVersion(Configuration config)
    {
      string connectionString = config.Properties["connection.connection_string"];
      return RetryExecutionHandler.Execute<Version>((Func<Version>) (() =>
      {
        using (DbConnection dbConnection = (DbConnection) new SqlWebConnection(connectionString))
        {
          using (DbCommand dbCommand = (DbCommand) new SqlWebCommand())
          {
            dbConnection.Open();
            dbCommand.Connection = dbConnection;
            dbCommand.CommandText = "Select r.Value from _DatabaseRegistry r Where RegistryId = 4";
            object obj = dbCommand.ExecuteScalar();
            if (obj != null)
            {
              if (obj != DBNull.Value)
                return new Version(Convert.ToString(obj));
            }
          }
        }
        return (Version) null;
      }));
    }

    private Version GetAssemblyProductVersion(Assembly assembly) => new Version(FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion);

    public Dictionary<string, SpeciesView> Species
    {
      get
      {
        lock (this._syncobj)
          return this._species;
      }
    }

    public Dictionary<string, SpeciesView> InvalidSpecies
    {
      get
      {
        lock (this._syncobj)
          return this._invalidSpecies;
      }
    }

    private void InitSpecies()
    {
      this._speciesService = new SpeciesService(this.m_sfLocSp);
      this._species = this._speciesService.GetValidSpecies().ToDictionary<LocationSpecies.Domain.Species, string, SpeciesView>((Func<LocationSpecies.Domain.Species, string>) (sp => sp.Code), (Func<LocationSpecies.Domain.Species, SpeciesView>) (sp => new SpeciesView(sp)));
      this._invalidSpecies = this._speciesService.GetInvalidSpecies().ToDictionary<LocationSpecies.Domain.Species, string, SpeciesView>((Func<LocationSpecies.Domain.Species, string>) (sp => sp.Code), (Func<LocationSpecies.Domain.Species, SpeciesView>) (sp => new SpeciesView(sp)));
    }

    public TaskScheduler Scheduler => StaTaskSchedulerEx.Instance();

    public static ProgramSession GetInstance()
    {
      if (ProgramSession._instance == null)
      {
        ProgramSession programSession = new ProgramSession();
        if (Interlocked.CompareExchange<ProgramSession>(ref ProgramSession._instance, programSession, (ProgramSession) null) == null)
          ProgramSession._instance.Initialize();
      }
      return ProgramSession._instance;
    }

    public ISessionFactory LocSp
    {
      get
      {
        lock (this._syncobj)
          return this.m_sfLocSp;
      }
    }

    public ISessionFactory IPEDSessionFactory
    {
      get
      {
        lock (this._syncobj)
          return this.m_sfPest;
      }
    }

    public IPEDData IPEDData
    {
      get
      {
        lock (this._syncobj)
        {
          if (this._iped == null)
            this._iped = new IPEDData(this.m_sfPest);
        }
        return this._iped;
      }
    }

    public virtual InputSession InputSession
    {
      get => ProgramSession._inputSession;
      set
      {
        ProgramSession._inputSession = value;
        this.OnInputSessionChanged();
      }
    }

    protected virtual void OnInputSessionChanged()
    {
      if (this.InputSessionChanged == null)
        return;
      foreach (Delegate invocation in this.InputSessionChanged.GetInvocationList())
      {
        Control target = invocation.Target as Control;
        object[] objArray = new object[2]
        {
          (object) this,
          (object) EventArgs.Empty
        };
        if (target != null && target.InvokeRequired)
          target.BeginInvoke(invocation, objArray);
        else
          invocation.DynamicInvoke(objArray);
      }
    }

    public bool UseEnglishUnits
    {
      get => i_Tree_Eco_v6.Properties.Settings.Default.UseEnglishUnits;
      set => i_Tree_Eco_v6.Properties.Settings.Default.UseEnglishUnits = value;
    }

    public string CacheUrl => i_Tree_Eco_v6.Properties.Settings.Default.CacheUrl;

    public SpeciesDisplayEnum SpeciesDisplayName
    {
      get
      {
        SpeciesDisplayEnum result = SpeciesDisplayEnum.CommonName;
        Enum.TryParse<SpeciesDisplayEnum>(i_Tree_Eco_v6.Properties.Settings.Default.SpeciesName, true, out result);
        return result;
      }
      set => i_Tree_Eco_v6.Properties.Settings.Default.SpeciesName = Enum.IsDefined(typeof (SpeciesDisplayEnum), (object) value) ? value.ToString() : throw new InvalidCastException();
    }

    public bool ShowGPS { get; set; }

    public bool ShowComments { get; set; }

    public bool ShowUID { get; set; }

    public bool HideZeros { get; set; }

    public VisualStyle VisualStyle
    {
      get => i_Tree_Eco_v6.Properties.Settings.Default.VisualStyle;
      set => i_Tree_Eco_v6.Properties.Settings.Default.VisualStyle = value;
    }

    public bool AutoCheckUpdates
    {
      get => i_Tree_Eco_v6.Properties.Settings.Default.AutoCheckUpdates;
      set => i_Tree_Eco_v6.Properties.Settings.Default.AutoCheckUpdates = value;
    }

    public string[] MRUList
    {
      get => i_Tree_Eco_v6.Properties.Settings.Default.MRUList;
      set => i_Tree_Eco_v6.Properties.Settings.Default.MRUList = value;
    }

    public bool MinimizeHelpPanel
    {
      get => i_Tree_Eco_v6.Properties.Settings.Default.MinimizeHelpPanel;
      set => i_Tree_Eco_v6.Properties.Settings.Default.MinimizeHelpPanel = value;
    }

    public void Save() => i_Tree_Eco_v6.Properties.Settings.Default.Save();

    public CultureInfo SpeciesCulture
    {
      get
      {
        if (this.m_spCulture == null)
        {
          CultureInfo ci = CultureInfo.CurrentCulture;
          using (ISession session = this.m_sfLocSp.OpenSession())
          {
            using (session.BeginTransaction())
            {
              Language language;
              do
              {
                language = session.CreateCriteria<Language>().Add(Restrictions.Where<Language>((System.Linq.Expressions.Expression<Func<Language, bool>>) (l => l.Culture == ci))).SetCacheable(true).UniqueResult<Language>();
                ci = ci.Parent;
              }
              while (language == null && ci != CultureInfo.InvariantCulture);
              this.m_spCulture = language == null ? CultureInfo.GetCultureInfo("en") : language.Culture;
            }
          }
        }
        return this.m_spCulture;
      }
    }

    public Version SpeciesVersion
    {
      get
      {
        if (this.m_spVersion == (Version) null)
        {
          using (ISession session = this.m_sfLocSp.OpenSession())
          {
            using (session.BeginTransaction())
            {
              RegistryEntry registryEntry = session.Get<RegistryEntry>((object) 3);
              if (registryEntry != null)
                this.m_spVersion = new Version(registryEntry.Value);
            }
          }
        }
        return this.m_spVersion;
      }
    }

    public Version LocationVersion
    {
      get
      {
        if (this.m_locVersion == (Version) null)
        {
          using (ISession session = this.m_sfLocSp.OpenSession())
          {
            using (session.BeginTransaction())
            {
              RegistryEntry registryEntry = session.Get<RegistryEntry>((object) 4);
              if (registryEntry != null)
                this.m_locVersion = new Version(registryEntry.Value);
            }
          }
        }
        return this.m_locVersion;
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (this.m_disposed)
        return;
      if (disposing)
      {
        if (this.m_sfLocSp != null && !this.m_sfLocSp.IsClosed)
        {
          this.m_sfLocSp.Dispose();
          this.m_sfLocSp = (ISessionFactory) null;
        }
        if (this.m_sfPest != null && !this.m_sfPest.IsClosed)
        {
          this.m_sfPest.Dispose();
          this.m_sfPest = (ISessionFactory) null;
        }
      }
      Interlocked.CompareExchange<ProgramSession>(ref ProgramSession._instance, (ProgramSession) null, this);
      this.m_disposed = true;
    }
  }
}
