// Decompiled with JetBrains decompiler
// Type: Eco.Util.InputSession
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.v6;
using Eco.Util.Queries;
using Eco.Util.Queries.Interfaces;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Eco.Util
{
  public class InputSession : IDisposable
  {
    private ISessionFactory m_inputSF;
    private Guid? m_year;
    private Guid? m_forecast;
    private bool m_temporary;
    private bool m_disposed;
    private string m_cacheDir;
    private bool m_isAccess;

    public event EventHandler YearChanged;

    public event EventHandler ForecastChanged;

    public InputSession(string input_db)
      : this(input_db, false)
    {
    }

    public InputSession(string input_db, bool temporary)
    {
      this.InputDb = input_db;
      this.m_isAccess = FileSignature.IsAccessDatabase(input_db);
      this.DbProviderFactory = !this.m_isAccess ? (DbProviderFactory) SQLiteFactory.Instance : (DbProviderFactory) OleDbFactory.Instance;
      this.m_temporary = temporary;
      this.m_cacheDir = Path.GetTempFileName();
      File.Delete(this.m_cacheDir);
    }

    ~InputSession() => this.Dispose(false);

    public void Close()
    {
      if (this.m_inputSF == null || this.m_inputSF.IsClosed)
        return;
      this.m_inputSF.Close();
    }

    public ISession CreateSession() => this.SessionFactory.OpenSession();

    public IStatelessSession CreateStatelessSession() => this.SessionFactory.OpenStatelessSession();

    public string InputDb { get; }

    public ISessionFactory SessionFactory
    {
      get
      {
        if (this.m_inputSF == null || this.m_inputSF.IsClosed)
        {
          Assembly assembly1 = typeof (Configuration).Assembly;
          Assembly assembly2 = typeof (Project).Assembly;
          FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly1.Location);
          string path = string.Format("{0}\\i-Tree\\", (object) System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
          string str = string.Format("{0}\\{1}.{2}.{3}.cfg", (object) path, (object) typeof (Project).Assembly.GetName().Name, (object) versionInfo.FileVersion, this.m_isAccess ? (object) "mdb" : (object) "sqlite");
          BinaryFormatter binaryFormatter = new BinaryFormatter();
          Configuration configuration = (Configuration) null;
          Directory.CreateDirectory(path);
          if (this.ConfigValid(str, assembly2))
          {
            try
            {
              using (FileStream serializationStream = new FileStream(str, FileMode.Open))
                configuration = binaryFormatter.Deserialize((Stream) serializationStream) as Configuration;
            }
            catch (SerializationException ex)
            {
              configuration = (Configuration) null;
            }
          }
          if (configuration == null)
          {
            configuration = !this.m_isAccess ? new Configuration().SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider").SetProperty("dialect", "NHibernate.Dialect.SQLiteDialect").SetProperty("connection.driver_class", "NHibernate.Driver.SQLite20Driver").SetProperty("query.substitutions", "true=1;false=0").SetProperty("adonet.batch_size", "50") : new Configuration().SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider").SetProperty("dialect", "NHibernate.JetDriver.JetDialect, NHibernate.JetDriver").SetProperty("connection.driver_class", "NHibernate.JetDriver.JetDriver, NHibernate.JetDriver").SetProperty("adonet.batch_size", "50");
            configuration.AddAssembly(assembly2);
            this.AddQueries(configuration);
            using (FileStream serializationStream = new FileStream(str, FileMode.Create))
              binaryFormatter.Serialize((Stream) serializationStream, (object) configuration);
            FileInfo fileInfo = new FileInfo(assembly2.Location);
            File.SetLastWriteTime(str, fileInfo.LastWriteTime);
          }
          configuration.SetProperty("connection.connection_string", this.ConnectionString);
          this.m_inputSF = configuration.BuildSessionFactory();
        }
        return this.m_inputSF;
      }
    }

    private void AddQueries(Configuration config)
    {
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
      string str1 = "Eco.Util.Queries." + (this.m_isAccess ? "Access" : "SQLite");
      string str2 = ".hbm.xml";
      foreach (string name in manifestResourceNames)
      {
        if (name.StartsWith(str1, StringComparison.OrdinalIgnoreCase) && name.EndsWith(str2))
        {
          Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(name);
          config.AddInputStream(manifestResourceStream);
        }
      }
    }

    private bool ConfigValid(string cfgFile, Assembly assembly)
    {
      if (!File.Exists(cfgFile))
        return false;
      FileInfo fileInfo = new FileInfo(cfgFile);
      return new FileInfo(assembly.Location).LastWriteTime == fileInfo.LastWriteTime;
    }

    public Guid? YearKey
    {
      get => this.m_year;
      set
      {
        this.m_year = value;
        this.OnYearChanged();
      }
    }

    public Guid? ForecastKey
    {
      get => this.m_forecast;
      set
      {
        this.m_forecast = value;
        this.OnForecastChanged();
      }
    }

    protected virtual void OnYearChanged()
    {
      if (this.YearChanged == null)
        return;
      foreach (Delegate invocation in this.YearChanged.GetInvocationList())
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

    protected virtual void OnForecastChanged()
    {
      if (this.ForecastChanged == null)
        return;
      foreach (Delegate invocation in this.ForecastChanged.GetInvocationList())
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

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.m_disposed)
        return;
      if (disposing && this.m_inputSF != null)
        this.m_inputSF.Dispose();
      if (this.m_temporary)
      {
        if (File.Exists(this.InputDb))
        {
          try
          {
            File.Delete(this.InputDb);
          }
          catch (Exception ex)
          {
          }
        }
      }
      if (Directory.Exists(this.m_cacheDir))
      {
        try
        {
          Directory.Delete(this.m_cacheDir, true);
        }
        catch (Exception ex)
        {
        }
      }
      this.m_disposed = true;
    }

    public string ConnectionString
    {
      get
      {
        DbConnectionStringBuilder connectionStringBuilder = this.DbProviderFactory.CreateConnectionStringBuilder();
        if (this.m_isAccess)
          connectionStringBuilder.Add("Provider", (object) "Microsoft.ACE.OLEDB.12.0");
        connectionStringBuilder.Add("Data Source", (object) this.InputDb);
        return connectionStringBuilder.ConnectionString;
      }
    }

    public DbProviderFactory DbProviderFactory { get; }

    public Version Version
    {
      get
      {
        Version version = (Version) null;
        try
        {
          using (DbConnection connection = this.DbProviderFactory.CreateConnection())
          {
            using (DbCommand command = connection.CreateCommand())
            {
              connection.ConnectionString = this.ConnectionString;
              connection.Open();
              command.CommandText = "SELECT [Value] FROM [DatabaseRegistryTable] WHERE [RegistryID] = 2";
              version = new Version(command.ExecuteScalar().ToString());
            }
          }
        }
        catch (ArgumentException ex)
        {
        }
        catch (DbException ex)
        {
        }
        catch (InvalidOperationException ex)
        {
        }
        return version;
      }
    }

    private Version ModelVersion
    {
      get
      {
        Version version = new Version(FileVersionInfo.GetVersionInfo(typeof (Project).Assembly.Location).ProductVersion);
        return new Version(version.Major, version.Minor, version.Build);
      }
    }

    public bool UpdateRequired()
    {
      bool flag = false;
      Version version = this.Version;
      if (version != (Version) null)
        flag = version < this.ModelVersion;
      return flag;
    }

    public bool CompatibleWithModel() => this.ModelVersion.Equals(this.Version);

    public IQuerySupplier GetQuerySupplier(ISession session) => !this.m_isAccess ? (IQuerySupplier) new SQLiteQuerySupplier(session) : (IQuerySupplier) new AccessQuerySupplier(session);

    public SASIQuerySupplier GetSASQuerySupplier(ISession session) => !this.m_isAccess ? (SASIQuerySupplier) new SASSQLiteQuerySupplier(session) : (SASIQuerySupplier) new SASAccessQuerySupplier(session);
  }
}
