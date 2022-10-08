// Decompiled with JetBrains decompiler
// Type: Eco.Util.ImportSource
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using ADODB;
using ADOX;
using Eco.Util.Properties;
using System;
using System.Collections.Generic;
using System.IO;

namespace Eco.Util
{
  public class ImportSource : IDisposable
  {
    private static Dictionary<string, FileFormat> m_dExtFormat = new Dictionary<string, FileFormat>();
    private Connection m_con;
    private bool m_hasHeader;
    private object m_lock;
    private bool _disposed;

    static ImportSource()
    {
      foreach (string str in Settings.Default.ExtExcel)
        ImportSource.m_dExtFormat.Add(str.TrimStart('*'), FileFormat.Excel);
      foreach (string str in Settings.Default.ExtCSV)
        ImportSource.m_dExtFormat.Add(str.TrimStart('*'), FileFormat.CSV);
      foreach (string str in Settings.Default.ExtAccess)
        ImportSource.m_dExtFormat.Add(str.TrimStart('*'), FileFormat.Access);
    }

    public ImportSource(string file)
    {
      this.m_lock = new object();
      this.FileName = file;
      this.SetFormat();
    }

    public string FileName { get; private set; }

    public FileFormat FileFormat { get; private set; }

    public bool HasHeader
    {
      get => this.m_hasHeader;
      set
      {
        this.m_hasHeader = value;
        lock (this.m_lock)
        {
          if (this.m_con == null || ((_Connection) this.m_con).State == 0)
            return;
          ((_Connection) this.m_con).Close();
        }
      }
    }

    private void SetFormat()
    {
      if (FileSignature.IsAccessDatabase(this.FileName))
      {
        this.FileFormat = FileFormat.Access;
      }
      else
      {
        string lower = Path.GetExtension(this.FileName).ToLower();
        if (ImportSource.m_dExtFormat.ContainsKey(lower))
          this.FileFormat = ImportSource.m_dExtFormat[lower];
        else
          this.FileFormat = FileFormat.Unknown;
      }
    }

    private string ConnectionString
    {
      get
      {
        List<string> stringList1 = new List<string>();
        List<string> stringList2 = new List<string>();
        stringList1.Add("Provider=Microsoft.ACE.OLEDB.12.0;");
        switch (this.FileFormat)
        {
          case FileFormat.CSV:
            stringList1.Add("Data Source=" + Path.GetDirectoryName(this.FileName) + ";");
            stringList2.Add("Text;");
            if (this.m_hasHeader)
              stringList2.Add("HDR=YES;");
            else
              stringList2.Add("HDR=NO;");
            stringList2.Add("IMEX=1;");
            stringList2.Add("FMT=Delimited;");
            break;
          case FileFormat.Excel:
            stringList1.Add("Data Source=" + this.FileName + ";");
            string lower = Path.GetExtension(this.FileName).ToLower();
            if (!(lower == ".xls") && !(lower == ".xlsb"))
            {
              if (!(lower == ".xlsx"))
              {
                if (lower == ".xlsm")
                  stringList2.Add("Excel 12.0 Macro;");
              }
              else
                stringList2.Add("Excel 12.0 Xml;");
            }
            else
              stringList2.Add("Excel 12.0;");
            if (this.m_hasHeader)
              stringList2.Add("HDR=YES;");
            else
              stringList2.Add("HDR=NO;");
            stringList2.Add("IMEX=1;");
            break;
          case FileFormat.Access:
            stringList1.Add("Data Source=" + this.FileName + ";");
            break;
        }
        if (stringList2.Count > 0)
          stringList1.Add("Extended Properties=\"" + string.Concat(stringList2.ToArray()) + "\";");
        return string.Concat(stringList1.ToArray());
      }
    }

    public RecordsetView GetData(string source)
    {
      RecordsetView data = (RecordsetView) null;
      if (source != null)
      {
        lock (this.m_lock)
        {
          if (this.m_con != null && ((_Connection) this.m_con).State != 0)
            ((_Connection) this.m_con).Close();
          RecordsetClass rs = new RecordsetClass();
          ((_Recordset) rs).ActiveConnection = (object) this.Connection;
          ((_Recordset) rs).CursorLocation = CursorLocationEnum.adUseServer;
          ((_Recordset) rs).Properties[(object) "Append-Only Rowset"].Value = (object) true;
          ((_Recordset) rs).Properties[(object) "Updatability"].Value = (object) 0;
          ((_Recordset) rs).Properties[(object) "Jet OLEDB:Bulk Transactions"].Value = (object) 1;
          ((_Recordset) rs).Properties[(object) "Jet OLEDB:Inconsistent"].Value = (object) true;
          ((_Recordset) rs).Properties[(object) "Jet OLEDB:Locking Granularity"].Value = (object) 1;
          ((_Recordset) rs).Open((object) string.Format("[{0}]", (object) source), (object) this.Connection, CursorTypeEnum.adOpenKeyset, LockTypeEnum.adLockReadOnly, 2);
          data = new RecordsetView((Recordset) rs);
        }
      }
      return data;
    }

    public List<DataSource> DataSources()
    {
      List<DataSource> dataSourceList = (List<DataSource>) null;
      if (this.FileFormat != FileFormat.CSV)
      {
        dataSourceList = new List<DataSource>();
        lock (this.m_lock)
        {
          Catalog catalog = (Catalog) new CatalogClass();
          catalog.ActiveConnection = (object) this.Connection;
          for (int index = 0; index < catalog.Tables.Count; ++index)
          {
            Table table = catalog.Tables[(object) index];
            string name = table.Name;
            SourceType sourceType = SourceType.Table;
            if (this.FileFormat == FileFormat.Excel)
              sourceType = !name.Trim('\'').EndsWith("$") ? SourceType.Range : SourceType.Worksheet;
            else if (!table.Type.Equals("TABLE"))
              continue;
            dataSourceList.Add(new DataSource()
            {
              Name = name,
              SourceType = sourceType
            });
          }
          if (this.FileFormat != FileFormat.Excel)
          {
            for (int index = 0; index < catalog.Views.Count; ++index)
              dataSourceList.Add(new DataSource()
              {
                Name = catalog.Views[(object) index].Name,
                SourceType = SourceType.View
              });
          }
        }
      }
      return dataSourceList;
    }

    public void Dispose() => this.Dispose(true);

    protected virtual void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (disposing && this.m_con != null && ((_Connection) this.m_con).State != 0)
      {
        ((_Connection) this.m_con).Close();
        this.m_con = (Connection) null;
      }
      this._disposed = true;
    }

    private Connection Connection
    {
      get
      {
        lock (this.m_lock)
        {
          if (this.m_con != null)
          {
            if (((_Connection) this.m_con).State != 0)
              goto label_7;
          }
          this.m_con = (Connection) new ConnectionClass();
          ((_Connection) this.m_con).Open(this.ConnectionString, string.Empty, string.Empty, 0);
        }
label_7:
        return this.m_con;
      }
    }
  }
}
