// Decompiled with JetBrains decompiler
// Type: Eco.Util.Cache.EcoCache
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using C1.C1Zip;
using Eco.Domain.v6;
using mdblib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;

namespace Eco.Util.Cache
{
  public class EcoCache
  {
    public int CacheStatus;
    public Project CurrentProject;
    public string[] CacheDirs = new string[11]
    {
      "iTree",
      "iTree\\EcoCached",
      "iTree\\EcoCached\\PollutionMasterDBs",
      "iTree\\EcoCached\\PollutionMasterDBs\\USA",
      "iTree\\EcoCached\\PollutionMasterDBs\\Canada",
      "iTree\\EcoCached\\PollutionMasterDBs\\Australia",
      "iTree\\EcoCached\\PollutionMasterDBs\\UK",
      "iTree\\EcoCached\\RadioSonde",
      "iTree\\EcoCached\\Weather",
      "iTree\\EcoCached\\ProjectXML",
      "iTree\\EcoCached\\temp"
    };
    public Dictionary<string, bool> ToBeDownloaded = new Dictionary<string, bool>()
    {
      {
        "weather",
        false
      },
      {
        "radiosondefile",
        false
      },
      {
        "pollution",
        false
      }
    };
    private string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private CacheDownloadForm progress_form;
    private Dictionary<string, string> db_paths = new Dictionary<string, string>()
    {
      {
        "CO",
        (string) null
      },
      {
        "NO2",
        (string) null
      },
      {
        "O3",
        (string) null
      },
      {
        "PM10",
        (string) null
      },
      {
        "PM25",
        (string) null
      },
      {
        "SO2",
        (string) null
      }
    };

    public string ServerURL { get; set; }

    public string UploadURL { get; set; }

    public string RequestURL { get; set; }

    public YearLocationData CurrentYear { get; set; }

    public string PollutionCOdbName
    {
      get => this.db_paths["CO"];
      set => this.db_paths["CO"] = value;
    }

    public string PollutionNO2dbName
    {
      get => this.db_paths["NO2"];
      set => this.db_paths["NO2"] = value;
    }

    public string PollutionO3dbName
    {
      get => this.db_paths["O3"];
      set => this.db_paths["O3"] = value;
    }

    public string PollutionPM10dbName
    {
      get => this.db_paths["PM10"];
      set => this.db_paths["PM10"] = value;
    }

    public string PollutionPM25dbName
    {
      get => this.db_paths["PM25"];
      set => this.db_paths["PM25"] = value;
    }

    public string PollutionSO2dbName
    {
      get => this.db_paths["SO2"];
      set => this.db_paths["SO2"] = value;
    }

    public string RadioSondeFileName { get; set; }

    public string WeatherFileName { get; set; }

    public XmlNode WeatherXml { get; set; }

    public XmlNode RadioSondeXml { get; set; }

    public XmlNode PollutionXml { get; set; }

    public Dictionary<string, AccessDatabase> DBs { get; set; }

    private Timer downloadTimer { get; set; }

    private DateTime currentTime { get; set; }

    private XmlDocument ResponseXml { get; set; }

    private int downloaded_files { get; set; }

    private int total_files { get; set; }

    private List<string> pollution_tables_to_be_downloaded { get; set; }

    public EcoCache(Project project, YearLocationData year, CacheDownloadForm form = null, string url = "https://www.itreetools.org/ecocache")
    {
      this.CurrentProject = project;
      this.CurrentYear = year;
      this.ServerURL = url;
      this.UploadURL = this.ServerURL + "/uploadfile.cfm";
      this.DBs = new Dictionary<string, AccessDatabase>()
      {
        {
          "CO",
          (AccessDatabase) null
        },
        {
          "NO2",
          (AccessDatabase) null
        },
        {
          "O3",
          (AccessDatabase) null
        },
        {
          "PM10",
          (AccessDatabase) null
        },
        {
          "PM25",
          (AccessDatabase) null
        },
        {
          "SO2",
          (AccessDatabase) null
        }
      };
      this.pollution_tables_to_be_downloaded = new List<string>();
      this.downloaded_files = 0;
      this.total_files = 0;
      this.progress_form = form != null ? form : new CacheDownloadForm();
      this.downloadTimer = new Timer(15000.0);
      this.PrepareFolders();
    }

    public async Task Cache()
    {
      EcoCache ecoCache = this;
      ecoCache.DBs = new Dictionary<string, AccessDatabase>()
      {
        {
          "CO",
          (AccessDatabase) null
        },
        {
          "NO2",
          (AccessDatabase) null
        },
        {
          "O3",
          (AccessDatabase) null
        },
        {
          "PM10",
          (AccessDatabase) null
        },
        {
          "PM25",
          (AccessDatabase) null
        },
        {
          "SO2",
          (AccessDatabase) null
        }
      };
      ecoCache.pollution_tables_to_be_downloaded = new List<string>();
      ecoCache.downloaded_files = 0;
      ecoCache.total_files = 0;
      ecoCache.BuildRequestURL();
      if (ecoCache.GetData() != -1)
        ecoCache.ParseData();
      if (ecoCache.ToBeDownloaded["weather"] && ecoCache.CacheStatus == 1)
        await ecoCache.DownloadWeather();
      if (ecoCache.ToBeDownloaded["radiosondefile"] && ecoCache.CacheStatus == 1)
        await ecoCache.DownloadRadioSonde();
      if (ecoCache.ToBeDownloaded["pollution"] && ecoCache.CacheStatus == 1)
        await ecoCache.DownloadPollution();
      ecoCache.ClearTemp();
    }

    public void UploadCache(
      string uploadfile,
      string fileFormName = "uploadedfile",
      string contenttype = "application/octet-stream",
      string userAgent = "iTreeEco_fjksdla*hUI*(hIoHio78asufhdsa&*Yhhioafdsaapjm")
    {
      Uri requestUri = new Uri(this.UploadURL);
      string zipInTemp = this.CreateZipInTemp(uploadfile);
      if (zipInTemp == "exists")
        return;
      string str = "----------" + DateTime.Now.Ticks.ToString("x");
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUri);
      httpWebRequest.UserAgent = userAgent;
      httpWebRequest.KeepAlive = true;
      httpWebRequest.ContentType = "multipart/form-data; boundary=" + str;
      httpWebRequest.Method = "POST";
      httpWebRequest.Timeout = 300000;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("--");
      stringBuilder.Append(str);
      stringBuilder.Append("\r\n");
      stringBuilder.Append("Content-Disposition: form-data; name=\"");
      stringBuilder.Append(fileFormName);
      stringBuilder.Append("\"; filename=\"");
      stringBuilder.Append(Path.GetFileName(zipInTemp));
      stringBuilder.Append("\"");
      stringBuilder.Append("\r\n");
      stringBuilder.Append("Content-Type: ");
      stringBuilder.Append(contenttype);
      stringBuilder.Append("\r\n");
      stringBuilder.Append("\r\n");
      byte[] bytes1 = Encoding.UTF8.GetBytes(stringBuilder.ToString());
      byte[] bytes2 = Encoding.ASCII.GetBytes("\r\n--" + str + "\r\n");
      FileStream fileStream = new FileStream(zipInTemp, FileMode.Open, FileAccess.Read);
      long num = (long) bytes1.Length + fileStream.Length + (long) bytes2.Length;
      httpWebRequest.ContentLength = num;
      try
      {
        Stream requestStream = httpWebRequest.GetRequestStream();
        requestStream.Write(bytes1, 0, bytes1.Length);
        byte[] buffer = new byte[(int) checked ((uint) Math.Min(4096, (int) fileStream.Length))];
        int count;
        while ((count = fileStream.Read(buffer, 0, buffer.Length)) != 0)
          requestStream.Write(buffer, 0, count);
        requestStream.Write(bytes2, 0, bytes2.Length);
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        StreamReader streamReader = new StreamReader(responseStream);
        streamReader.ReadToEnd();
        streamReader.Close();
        responseStream.Close();
        fileStream.Close();
      }
      catch (WebException ex)
      {
        Console.WriteLine("WebException: {0}", (object) ex.ToString());
        fileStream.Close();
      }
      catch (IOException ex)
      {
        Console.WriteLine("IOException: {0}", (object) ex.ToString());
        fileStream.Close();
      }
      System.IO.File.Delete(zipInTemp);
    }

    public void PrepareFolders()
    {
      foreach (string cacheDir in this.CacheDirs)
        EcoCache.CreateDir(Path.Combine(this.AppData, cacheDir));
    }

    public void PrepareMDBs(
      string year,
      string country_code,
      string version_number,
      bool delete_all = false)
    {
      string str1 = Path.Combine(this.AppData, this.CacheDirs[2], EcoCache.GetCountryCode(country_code), year);
      Directory.CreateDirectory(str1);
      List<string> stringList = new List<string>((IEnumerable<string>) this.DBs.Keys);
      string format = "\r\n            CREATE TABLE [DatabaseRegistry] (\r\n                [RegistryId] LONG NOT NULL DEFAULT 0,\r\n                [Description] VARCHAR(25) NOT NULL,\r\n                [Value] VARCHAR(15) NOT NULL\r\n            );\r\n            INSERT INTO [DatabaseRegistry] ([RegistryId], [Description], [Value]) VALUES (6, 'Pollution', '{0}');";
      foreach (string key in stringList)
      {
        string str2 = Path.Combine(str1, key + ".mdb");
        if (delete_all || !System.IO.File.Exists(str2))
        {
          this.ToBeDownloaded["pollution"] = true;
          this.DBs[key] = AccessDatabase.Create(str2, "4.0", 1033);
          this.db_paths[key] = str2;
          MemoryStream input = new MemoryStream(Encoding.UTF8.GetBytes(string.Format(format, (object) version_number)));
          new AccessSchemaReader(this.DBs[key], (Stream) input).Parse();
        }
        else
        {
          this.DBs[key] = AccessDatabase.Open(str2, false);
          this.ToBeDownloaded["pollution"] = false;
        }
        if (!(key == "CO"))
        {
          if (!(key == "NO2"))
          {
            if (!(key == "O3"))
            {
              if (!(key == "PM10"))
              {
                if (!(key == "PM25"))
                {
                  if (key == "SO2")
                    this.PollutionSO2dbName = str2;
                }
                else
                  this.PollutionPM25dbName = str2;
              }
              else
                this.PollutionPM10dbName = str2;
            }
            else
              this.PollutionO3dbName = str2;
          }
          else
            this.PollutionNO2dbName = str2;
        }
        else
          this.PollutionCOdbName = str2;
      }
    }

    public int GetData()
    {
      Stream inStream = EcoCache.DownloadFile(this.RequestURL);
      string str = Path.Combine(this.AppData, this.CacheDirs[9], this.CurrentYear.Guid.ToString() + "-" + this.CurrentYear.Revision.ToString() + ".xml");
      if (inStream == null)
      {
        if (!System.IO.File.Exists(str))
          return this.CacheStatus = -1;
        this.ResponseXml = new XmlDocument();
        this.ResponseXml.Load(str);
        return this.CacheStatus = 0;
      }
      this.ResponseXml = new XmlDocument();
      this.ResponseXml.Load(inStream);
      this.ResponseXml.Save(str);
      return this.CacheStatus = 1;
    }

    public void ParseData()
    {
      XmlNode xmlNode = this.ResponseXml.DocumentElement.SelectSingleNode("/EcoDataInfo/Results");
      this.WeatherXml = xmlNode.SelectSingleNode("WeatherFile");
      this.RadioSondeXml = xmlNode.SelectSingleNode("RadioSondeFile");
      this.PollutionXml = xmlNode.SelectSingleNode("PollutionData");
      if (this.WeatherXml.HasChildNodes)
      {
        string path = Path.Combine(this.AppData, this.CacheDirs[8], this.WeatherXml.SelectSingleNode("Year").InnerText, this.WeatherXml.SelectSingleNode("FileName").InnerText);
        DateTime dateTime = EcoCache.ConvertToDateTime(this.WeatherXml.SelectSingleNode("CreatedTimeUTC").InnerText);
        if (!System.IO.File.Exists(path) || System.IO.File.GetLastWriteTime(path) != dateTime)
        {
          if (this.CacheStatus == 0)
          {
            this.CacheStatus = -1;
          }
          else
          {
            this.ToBeDownloaded["weather"] = true;
            ++this.total_files;
          }
        }
        else
          this.WeatherFileName = path;
      }
      if (this.RadioSondeXml.HasChildNodes)
      {
        string path = Path.Combine(this.AppData, this.CacheDirs[7], this.RadioSondeXml.SelectSingleNode("Year").InnerText, this.RadioSondeXml.SelectSingleNode("Name").InnerText);
        DateTime dateTime = EcoCache.ConvertToDateTime(this.RadioSondeXml.SelectSingleNode("CreatedTimeUTC").InnerText);
        if (!System.IO.File.Exists(path) || System.IO.File.GetLastWriteTime(path) != dateTime)
        {
          if (this.CacheStatus == 0)
          {
            this.CacheStatus = -1;
          }
          else
          {
            this.ToBeDownloaded["radiosondefile"] = true;
            ++this.total_files;
          }
        }
        else
          this.RadioSondeFileName = path;
      }
      if (this.PollutionXml.HasChildNodes)
      {
        string innerText1 = this.PollutionXml.SelectSingleNode("Version").InnerText;
        EcoCache.GetCountryCode(this.PollutionXml.SelectSingleNode("NationId").InnerText);
        string innerText2 = this.PollutionXml.SelectSingleNode("Year").InnerText;
        bool flag = false;
        this.PrepareMDBs(innerText2, this.PollutionXml.SelectSingleNode("NationId").InnerText, innerText1);
        string format = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};";
        foreach (string key in this.db_paths.Keys)
        {
          this.DBs[key].Close();
          using (OleDbConnection connection = new OleDbConnection(string.Format(format, (object) this.db_paths[key])))
          {
            connection.Open();
            using (OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT DISTINCT TOP 1 * FROM DatabaseRegistry ", connection).ExecuteReader())
            {
              oleDbDataReader.Read();
              if (!this.ToBeDownloaded["pollution"])
              {
                if (!(innerText1 != (string) oleDbDataReader["Value"]))
                  continue;
              }
              if (this.CacheStatus == 0)
              {
                this.CacheStatus = -1;
              }
              else
              {
                flag = true;
                break;
              }
            }
          }
        }
        if (flag)
        {
          if (!this.ToBeDownloaded["pollution"])
          {
            this.ToBeDownloaded["pollution"] = true;
            this.PrepareMDBs(innerText2, this.PollutionXml.SelectSingleNode("NationId").InnerText, innerText1, true);
          }
          this.total_files += this.PollutionXml.SelectSingleNode("CO_Tables").ChildNodes.Count;
          this.total_files += this.PollutionXml.SelectSingleNode("NO2_Tables").ChildNodes.Count;
          this.total_files += this.PollutionXml.SelectSingleNode("O3_Tables").ChildNodes.Count;
          this.total_files += this.PollutionXml.SelectSingleNode("PM10_Tables").ChildNodes.Count;
          this.total_files += this.PollutionXml.SelectSingleNode("PM25_Tables").ChildNodes.Count;
          this.total_files += this.PollutionXml.SelectSingleNode("SO2_Tables").ChildNodes.Count;
        }
        else
        {
          int tablesToBeDownloaded = this.GetPollutionTablesToBeDownloaded();
          this.total_files += tablesToBeDownloaded;
          if (this.CacheStatus == 0 && tablesToBeDownloaded > 0)
            this.CacheStatus = -1;
        }
      }
      if (this.total_files == 0 || this.CacheStatus != 1)
        return;
      this.progress_form.SetFileCount(this.total_files);
      this.progress_form.Show();
    }

    public async Task DownloadWeather()
    {
      EcoCache ecoCache = this;
      string innerText1 = ecoCache.WeatherXml.SelectSingleNode("Year").InnerText;
      string innerText2 = ecoCache.WeatherXml.SelectSingleNode("FileName").InnerText;
      string str = Path.Combine(ecoCache.AppData, ecoCache.CacheDirs[8], innerText1);
      string file_path = Path.Combine(str, innerText2);
      DateTime creation_time = EcoCache.ConvertToDateTime(ecoCache.WeatherXml.SelectSingleNode("CreatedTimeUTC").InnerText);
      EcoCache.CreateDir(str);
      EcoCache.CacheWebClient client = new EcoCache.CacheWebClient();
      Uri url = new Uri(ecoCache.BuildDownloadURL("weather", innerText1, innerText2));
      client.DownloadFileCompleted += (AsyncCompletedEventHandler) ((sender, e) => this.SetDownloadedFileTimeStamp(sender, e, file_path, creation_time));
      client.DownloadFileCompleted += new AsyncCompletedEventHandler(ecoCache.UpdateTotalFileProgressCallBack);
      client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ecoCache.UpdateCurrentFileProgress);
      ecoCache.downloadTimer.Elapsed += (ElapsedEventHandler) ((sender, e) => this.UpdateDownloadTimer(sender, e, client));
      try
      {
        ecoCache.currentTime = DateTime.Now;
        ecoCache.downloadTimer.Start();
        await client.DownloadFileTaskAsync(url, file_path);
      }
      catch (WebException ex)
      {
        Console.WriteLine("WebException [{0}]: {1}", (object) url.ToString(), (object) ex.ToString());
        client.CancelAsync();
        ecoCache.downloadTimer.Stop();
        ecoCache.progress_form.Close();
        ecoCache.CacheStatus = -1;
      }
      catch (TaskCanceledException ex)
      {
        Console.WriteLine("TaskCanceledException [{0}]: {1}", (object) url.ToString(), (object) ex.ToString());
        ecoCache.downloadTimer.Stop();
        ecoCache.progress_form.Close();
        ecoCache.CacheStatus = -1;
      }
      ecoCache.WeatherFileName = file_path;
      url = (Uri) null;
    }

    public async Task DownloadRadioSonde()
    {
      EcoCache ecoCache = this;
      string innerText1 = ecoCache.RadioSondeXml.SelectSingleNode("Year").InnerText;
      string innerText2 = ecoCache.RadioSondeXml.SelectSingleNode("Name").InnerText;
      string str = Path.Combine(ecoCache.AppData, ecoCache.CacheDirs[7], innerText1);
      string file_path = Path.Combine(str, innerText2);
      DateTime creation_time = EcoCache.ConvertToDateTime(ecoCache.RadioSondeXml.SelectSingleNode("CreatedTimeUTC").InnerText);
      EcoCache.CreateDir(str);
      EcoCache.CacheWebClient client = new EcoCache.CacheWebClient();
      Uri url = new Uri(ecoCache.BuildDownloadURL("radiosonde", innerText1, innerText2));
      client.DownloadFileCompleted += (AsyncCompletedEventHandler) ((sender, e) => this.SetDownloadedFileTimeStamp(sender, e, file_path, creation_time));
      client.DownloadFileCompleted += new AsyncCompletedEventHandler(ecoCache.UpdateTotalFileProgressCallBack);
      client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ecoCache.UpdateCurrentFileProgress);
      ecoCache.downloadTimer.Elapsed += (ElapsedEventHandler) ((sender, e) => this.UpdateDownloadTimer(sender, e, client));
      try
      {
        ecoCache.currentTime = DateTime.Now;
        ecoCache.downloadTimer.Start();
        await client.DownloadFileTaskAsync(url, file_path);
      }
      catch (WebException ex)
      {
        Console.WriteLine("WebException [{0}]: {1}", (object) url.ToString(), (object) ex.ToString());
        client.CancelAsync();
        ecoCache.downloadTimer.Stop();
        ecoCache.progress_form.Close();
        ecoCache.CacheStatus = -1;
      }
      catch (TaskCanceledException ex)
      {
        Console.WriteLine("TaskCanceledException [{0}]: {1}", (object) url.ToString(), (object) ex.ToString());
        ecoCache.progress_form.Close();
        ecoCache.downloadTimer.Stop();
        ecoCache.CacheStatus = -1;
      }
      ecoCache.RadioSondeFileName = file_path;
      url = (Uri) null;
    }

    private void SetDownloadedFileTimeStamp(
      object sender,
      AsyncCompletedEventArgs e,
      string file_path,
      DateTime creation_time)
    {
      this.downloadTimer.Stop();
      if (e.Error != null)
        return;
      System.IO.File.SetCreationTime(file_path, creation_time);
      System.IO.File.SetLastWriteTime(file_path, creation_time);
    }

    public async Task DownloadPollution()
    {
      EcoCache ecoCache1 = this;
      string year = ecoCache1.PollutionXml.SelectSingleNode("Year").InnerText;
      string nation = ecoCache1.PollutionXml.SelectSingleNode("NationId").InnerText;
      string innerText = ecoCache1.PollutionXml.SelectSingleNode("Version").InnerText;
      string path = Path.Combine(ecoCache1.AppData, ecoCache1.CacheDirs[2], EcoCache.GetCountryCode(nation), year);
      string temp_path = Path.Combine(ecoCache1.AppData, ecoCache1.CacheDirs[10]);
      Dictionary<string, XmlNode> dictionary = new Dictionary<string, XmlNode>()
      {
        {
          "CO",
          ecoCache1.PollutionXml.SelectSingleNode("CO_Tables")
        },
        {
          "NO2",
          ecoCache1.PollutionXml.SelectSingleNode("NO2_Tables")
        },
        {
          "O3",
          ecoCache1.PollutionXml.SelectSingleNode("O3_Tables")
        },
        {
          "PM10",
          ecoCache1.PollutionXml.SelectSingleNode("PM10_Tables")
        },
        {
          "PM25",
          ecoCache1.PollutionXml.SelectSingleNode("PM25_Tables")
        },
        {
          "SO2",
          ecoCache1.PollutionXml.SelectSingleNode("SO2_Tables")
        }
      };
      EcoCache.CreateDir(path);
      foreach (KeyValuePair<string, XmlNode> keyValuePair in dictionary)
      {
        EcoCache ecoCache = ecoCache1;
        KeyValuePair<string, XmlNode> pollutant = keyValuePair;
        foreach (XmlNode xmlNode in pollutant.Value)
        {
          XmlNode table = xmlNode;
          if (ecoCache1.pollution_tables_to_be_downloaded.Count == 0 || ecoCache1.pollution_tables_to_be_downloaded.Exists((Predicate<string>) (t => t == table.InnerText)))
          {
            string sql_path = Path.Combine(temp_path, table.InnerText + ".sql");
            Uri url = new Uri(ecoCache1.BuildPollutionDownloadURL(year, nation, table.InnerText, pollutant.Key));
            EcoCache.CacheWebClient client = new EcoCache.CacheWebClient();
            client.DownloadFileCompleted += (AsyncCompletedEventHandler) ((sender, e) => closure_5.AppendToDatabase(sender, e, closure_5.DBs[pollutant.Key], sql_path));
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(ecoCache1.UpdateTotalFileProgressCallBack);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ecoCache1.UpdateCurrentFileProgress);
            ecoCache1.downloadTimer.Elapsed += (ElapsedEventHandler) ((sender, e) => closure_5.UpdateDownloadTimer(sender, e, client));
            try
            {
              ecoCache1.currentTime = DateTime.Now;
              ecoCache1.downloadTimer.Start();
              await client.DownloadFileTaskAsync(url, sql_path);
            }
            catch (WebException ex)
            {
              Console.WriteLine("WebException [{0}]: {1}", (object) url.ToString(), (object) ex.ToString());
              client.CancelAsync();
              ecoCache1.downloadTimer.Stop();
              ecoCache1.progress_form.Close();
              ecoCache1.CacheStatus = -1;
            }
            catch (TaskCanceledException ex)
            {
              Console.WriteLine("TaskCanceledException [{0}]: {1}", (object) url.ToString(), (object) ex.ToString());
              ecoCache1.progress_form.Close();
              ecoCache1.downloadTimer.Stop();
              ecoCache1.CacheStatus = -1;
            }
            url = (Uri) null;
          }
        }
        ecoCache1.DBs[pollutant.Key].Close();
        if (ecoCache1.CacheStatus != -1)
          ;
        else
          break;
      }
      year = (string) null;
      nation = (string) null;
      temp_path = (string) null;
    }

    private void UpdateDownloadTimer(
      object sender,
      ElapsedEventArgs e,
      EcoCache.CacheWebClient client)
    {
      DateTime now = DateTime.Now;
    }

    private void AppendToDatabase(
      object sender,
      AsyncCompletedEventArgs e,
      AccessDatabase db,
      string sql_path)
    {
      this.downloadTimer.Stop();
      if (e.Error != null)
        return;
      using (FileStream input = new FileStream(sql_path, FileMode.Open))
        new AccessSchemaReader(db, (Stream) input).Parse();
    }

    private void UpdateCurrentFileProgress(object sender, DownloadProgressChangedEventArgs e)
    {
      this.currentTime = DateTime.Now;
      this.progress_form.UpdateCurrentFileProgress(e.ProgressPercentage);
    }

    private void UpdateTotalFileProgressCallBack(object sender, AsyncCompletedEventArgs e)
    {
      ++this.downloaded_files;
      this.progress_form.UpdateTotalFileProgress(this.downloaded_files);
      if (this.total_files != this.downloaded_files)
        return;
      this.progress_form.Close();
    }

    private void ClearTemp()
    {
      foreach (FileInfo file in new DirectoryInfo(Path.Combine(this.AppData, this.CacheDirs[10])).GetFiles())
      {
        if (Path.GetExtension(file.ToString()) != ".zip")
          file.Delete();
      }
    }

    public void BuildRequestURL() => this.RequestURL = string.Format("{0}{1}?wStationId={2}&wyear={3}&pyear={4}&locationid={5}&NationId={6}", (object) this.ServerURL, (object) "/getdatainfo.cfm", (object) this.CurrentYear.WeatherStationId, (object) this.CurrentYear.WeatherYear, (object) this.CurrentYear.PollutionYear, (object) this.CurrentProject.LocationId, (object) this.CurrentProject.NationCode);

    private string BuildDownloadURL(string datatype, string year, string file_name) => string.Format("{0}{1}?Datatype={2}&year={3}&FileName={4}", (object) this.ServerURL, (object) "/download.cfm", (object) datatype, (object) year, (object) file_name);

    private string BuildPollutionDownloadURL(
      string year,
      string nation,
      string table_name,
      string pollutant)
    {
      return string.Format("{0}{1}?Datatype={2}&year={3}&NationId={4}&TableName={5}&Pollutant={6}", (object) this.ServerURL, (object) "/download.cfm", (object) "pollution", (object) year, (object) nation, (object) table_name, (object) pollutant);
    }

    private static string GetCountryCode(string code)
    {
      if (code == "001")
        return "USA";
      if (code == "002")
        return "Canada";
      if (code == "230")
        return "Australia";
      return code == "021" ? "UK" : "NA";
    }

    private static Stream DownloadFile(string url)
    {
      try
      {
        return WebRequest.Create(url).GetResponse().GetResponseStream();
      }
      catch (WebException ex)
      {
        Console.WriteLine("Download Error [{0}]: {1}", (object) url, (object) ex.ToString());
        return (Stream) null;
      }
    }

    private static void CreateDir(string path)
    {
      try
      {
        Directory.CreateDirectory(path);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Couldn't create the dir `{0}`\nError: {1}", (object) path, (object) ex.ToString());
      }
    }

    private static DateTime ConvertToDateTime(string utc_value)
    {
      DateTime dateTime;
      try
      {
        dateTime = DateTime.SpecifyKind(DateTime.Parse(utc_value), DateTimeKind.Utc);
      }
      catch (FormatException ex)
      {
        Console.WriteLine("Date Error: {0}", (object) ex);
        return DateTime.Now;
      }
      return dateTime.ToLocalTime();
    }

    private string CreateZipInTemp(string filePath)
    {
      string zipInTemp = Path.Combine(this.AppData, this.CacheDirs[10], string.Format("{0}_{1}.zip", (object) this.CurrentYear.Guid, (object) Path.GetFileName(filePath)));
      if (System.IO.File.Exists(zipInTemp))
        return "exists";
      using (C1ZipFile c1ZipFile = new C1ZipFile())
      {
        c1ZipFile.Create(zipInTemp);
        c1ZipFile.Entries.Add(filePath, Path.GetFileName(filePath));
      }
      return zipInTemp;
    }

    private int GetPollutionTablesToBeDownloaded()
    {
      int tablesToBeDownloaded = 0;
      string format = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};";
      foreach (KeyValuePair<string, XmlNode> keyValuePair in new Dictionary<string, XmlNode>()
      {
        {
          "CO",
          this.PollutionXml.SelectSingleNode("CO_Tables")
        },
        {
          "NO2",
          this.PollutionXml.SelectSingleNode("NO2_Tables")
        },
        {
          "O3",
          this.PollutionXml.SelectSingleNode("O3_Tables")
        },
        {
          "PM10",
          this.PollutionXml.SelectSingleNode("PM10_Tables")
        },
        {
          "PM25",
          this.PollutionXml.SelectSingleNode("PM25_Tables")
        },
        {
          "SO2",
          this.PollutionXml.SelectSingleNode("SO2_Tables")
        }
      })
      {
        this.DBs[keyValuePair.Key].Close();
        string dbPath = this.db_paths[keyValuePair.Key];
        List<string> stringList1 = new List<string>();
        string connectionString = string.Format(format, (object) dbPath);
        foreach (XmlNode childNode in keyValuePair.Value.ChildNodes)
          stringList1.Add(childNode.InnerText);
        using (OleDbConnection oleDbConnection = new OleDbConnection(connectionString))
        {
          oleDbConnection.Open();
          DataTable schema = oleDbConnection.GetSchema("Tables");
          List<string> stringList2 = new List<string>();
          foreach (DataRow row in (InternalDataCollectionBase) schema.Rows)
          {
            string str = row.ItemArray[2].ToString();
            stringList2.Add(str);
          }
          foreach (string str in stringList1)
          {
            string x = str;
            if (!stringList2.Exists((Predicate<string>) (t => t == x)))
            {
              ++tablesToBeDownloaded;
              this.pollution_tables_to_be_downloaded.Add(x);
            }
          }
        }
      }
      if (tablesToBeDownloaded > 0)
        this.ToBeDownloaded["pollution"] = true;
      return tablesToBeDownloaded;
    }

    public class CacheWebClient : WebClient
    {
      private int timeout;

      public int Timeout
      {
        get => this.timeout;
        set => this.timeout = value;
      }

      public CacheWebClient() => this.timeout = 300000;

      protected override WebRequest GetWebRequest(Uri address)
      {
        WebRequest webRequest = base.GetWebRequest(address);
        webRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
        webRequest.Timeout = this.timeout;
        return webRequest;
      }
    }
  }
}
