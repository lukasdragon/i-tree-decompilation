// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Tasks.UpdateV5DatabaseTask
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Properties;
using mdblib;
using System.IO;

namespace i_Tree_Eco_v6.Tasks
{
  public class UpdateV5DatabaseTask : UpdateDatabaseTask
  {
    protected string m_dbfile;

    public UpdateV5DatabaseTask(string db) => this.m_dbfile = db;

    protected override void Work()
    {
      AccessDatabase aDB = AccessDatabase.Open(this.m_dbfile, false);
      new AccessSchemaReader(aDB, this.GetUpdate(), this.OutputStream, this.OutputStream).Parse();
      aDB.Close();
    }

    private Stream GetUpdate()
    {
      MemoryStream update = new MemoryStream();
      StreamWriter streamWriter = new StreamWriter((Stream) update);
      streamWriter.Write(Resources.Eco_v5);
      streamWriter.Flush();
      update.Position = 0L;
      return (Stream) update;
    }
  }
}
