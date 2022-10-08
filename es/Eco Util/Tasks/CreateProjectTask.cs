// Decompiled with JetBrains decompiler
// Type: Eco.Util.Tasks.CreateProjectTask
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System.IO;
using System.Reflection;

namespace Eco.Util.Tasks
{
  public class CreateProjectTask : ATask<bool>
  {
    private string m_dbFile;

    public CreateProjectTask(string dbFile) => this.m_dbFile = dbFile;

    protected override bool Work()
    {
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      bool flag = true;
      using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream("Eco.Util.TemplateDB.sqlite3"))
      {
        using (FileStream destination = new FileStream(this.m_dbFile, FileMode.Create))
          manifestResourceStream.CopyTo((Stream) destination);
      }
      return flag;
    }
  }
}
