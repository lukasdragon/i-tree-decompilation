// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.ProjectFile
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Eco.Domain.v5
{
  public class ProjectFile
  {
    private string m_file;
    private Dictionary<string, object> m_properties;

    public ProjectFile() => this.m_properties = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    private ProjectFile(string file)
      : this()
    {
      this.m_file = file;
    }

    public static ProjectFile Open(string file)
    {
      ProjectFile projectFile = new ProjectFile(file);
      StreamReader streamReader = new StreamReader((Stream) new FileStream(file, FileMode.Open, FileAccess.Read));
      Regex regex1 = new Regex("^\\s*'");
      Regex regex2 = new Regex("^\\s*([^=]+)=(.*?)\\s*$");
      Regex regex3 = new Regex("^(([^@]*)@)+$");
      for (string input1 = streamReader.ReadLine(); input1 != null; input1 = streamReader.ReadLine())
      {
        if (!regex1.IsMatch(input1) && regex2.IsMatch(input1))
        {
          foreach (Match match in regex2.Matches(input1))
          {
            string prop = match.Groups[1].Value;
            string input2 = match.Groups[2].Value;
            MatchCollection matchCollection = regex3.Matches(input2);
            if (matchCollection.Count == 0)
            {
              projectFile[prop] = (object) input2;
            }
            else
            {
              Group group = matchCollection[0].Groups[2];
              string[] strArray = new string[group.Captures.Count - 1];
              for (int i = 1; i < group.Captures.Count; ++i)
                strArray[i - 1] = group.Captures[i].Value;
              projectFile[prop] = (object) strArray;
            }
          }
        }
      }
      return projectFile;
    }

    public virtual object this[string prop]
    {
      get => this.m_properties.ContainsKey(prop) ? this.m_properties[prop] : (object) null;
      set
      {
        if (value != null)
        {
          this.m_properties[prop] = value;
        }
        else
        {
          if (!this.m_properties.ContainsKey(prop))
            return;
          this.m_properties.Remove(prop);
        }
      }
    }
  }
}
