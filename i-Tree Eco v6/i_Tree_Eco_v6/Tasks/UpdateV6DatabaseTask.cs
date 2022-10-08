// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Tasks.UpdateV6DatabaseTask
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Util;
using mdblib;
using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Tasks
{
  public class UpdateV6DatabaseTask : UpdateDatabaseTask
  {
    private InputSession m_is;

    public UpdateV6DatabaseTask(InputSession inputSession) => this.m_is = inputSession;

    protected override void Work()
    {
      try
      {
        bool flag1 = FileSignature.IsAccessDatabase(this.m_is.InputDb);
        bool flag2 = FileSignature.IsSqliteDatabase(this.m_is.InputDb);
        while (this.m_is.UpdateRequired())
        {
          string path3 = flag2 ? "SQLite" : "Access";
          string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Updates", path3, this.m_is.Version?.ToString() + ".sql");
          if (!File.Exists(path))
            throw new UpdateDatabaseException("Project update required, but no update found.");
          using (FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read))
          {
            if (flag1)
            {
              AccessDatabase aDB = AccessDatabase.Open(this.m_is.InputDb, false);
              new AccessSchemaReader(aDB, (Stream) input, this.OutputStream, this.OutputStream).Parse();
              aDB.Close();
            }
            else if (flag2)
            {
              try
              {
                using (StreamReader streamReader = new StreamReader((Stream) input))
                {
                  string end = streamReader.ReadToEnd();
                  using (SQLiteConnection connection = new SQLiteConnection(this.m_is.ConnectionString))
                  {
                    connection.Open();
                    using (SQLiteCommand sqLiteCommand = new SQLiteCommand(connection))
                    {
                      sqLiteCommand.CommandText = end;
                      sqLiteCommand.ExecuteNonQuery();
                    }
                    connection.Close();
                  }
                }
              }
              catch (SQLiteException ex)
              {
                using (StreamWriter streamWriter = new StreamWriter(this.OutputStream))
                  streamWriter.WriteLine(ex.Message);
              }
            }
          }
        }
      }
      catch (UpdateDatabaseException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new UpdateDatabaseException("An unexpected error occured", ex);
      }
    }
  }
}
