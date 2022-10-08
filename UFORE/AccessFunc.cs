// Decompiled with JetBrains decompiler
// Type: UFORE.AccessFunc
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using ADOX;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace UFORE
{
  public class AccessFunc
  {
    public static void CreateDB(string DB)
    {
      Console.Write(Path.GetDirectoryName(DB));
      if (!Directory.Exists(Path.GetDirectoryName(DB)))
        throw new DirectoryNotFoundException(DB);
      // ISSUE: variable of a compiler-generated type
      Catalog instance = (Catalog) Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00000602-0000-0010-8000-00AA006D2EA4")));
      if (File.Exists(DB))
        File.Delete(DB);
      string ConnectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DB + ";Jet OLEDB:Engine Type=5";
      // ISSUE: reference to a compiler-generated method
      instance.Create(ConnectString);
    }

    public static void AppendField(OleDbConnection conn, string tbl, string field, string type)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = conn;
        oleDbCommand.CommandText = "ALTER TABLE [" + tbl + "] ADD [" + field + "] " + type + ";";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void AppendField(string DB, string tbl, string field, string type)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + DB))
      {
        conn.Open();
        AccessFunc.AppendField(conn, tbl, field, type);
      }
    }

    public static void RemoveTable(OleDbConnection conn, string tbl)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        bool flag = false;
        oleDbCommand.Connection = conn;
        DataTable oleDbSchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[4]
        {
          null,
          null,
          null,
          (object) "TABLE"
        });
        for (int index = 0; index < oleDbSchemaTable.Rows.Count; ++index)
        {
          if (oleDbSchemaTable.Rows[index].ItemArray[2].ToString() == tbl)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return;
        oleDbCommand.CommandText = "DROP TABLE " + tbl + ";";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void RemoveTable(string DB, string tbl)
    {
      using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + DB))
      {
        conn.Open();
        AccessFunc.RemoveTable(conn, tbl);
      }
    }

    public static void CopyTable(
      string srcDB,
      string srcTbl,
      OleDbConnection cnDstDB,
      string dstTbl)
    {
      using (OleDbCommand oleDbCommand = new OleDbCommand())
      {
        oleDbCommand.Connection = cnDstDB;
        oleDbCommand.CommandText = "SELECT * INTO " + dstTbl + " FROM " + srcTbl + " IN \"" + srcDB + "\";";
        oleDbCommand.ExecuteNonQuery();
      }
    }

    [Obsolete]
    public static void CopyTable(string srcDB, string srcTbl, string dstDB, string dstTbl)
    {
      using (OleDbConnection cnDstDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + dstDB))
      {
        cnDstDB.Open();
        AccessFunc.CopyTable(srcDB, srcTbl, cnDstDB, dstTbl);
      }
    }
  }
}
