// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.SAS.LineValues
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System;
using System.Globalization;
using System.Linq;

namespace i_Tree_Eco_v6.SAS
{
  internal class LineValues
  {
    private string[] m_values;
    public char separater = ',';
    private int m_Number;
    private CultureInfo ciUsed = CultureInfo.InvariantCulture;

    public void SetLine(string aLine)
    {
      if (!aLine.Contains<char>('"'))
      {
        this.m_values = aLine.Split(this.separater);
        this.m_Number = this.m_values.GetLength(0);
      }
      else
      {
        this.m_values = aLine.Split(this.separater);
        int length = this.m_values.GetLength(0);
        bool flag = false;
        int index1 = 0;
        for (int index2 = 0; index2 < length; ++index2)
        {
          if (flag)
          {
            if (this.m_values[index2].Contains<char>('"'))
            {
              string[] strArray1 = this.m_values[index2].Trim().Trim().Split('"');
              if (strArray1.GetLength(0) == 2)
              {
                if (strArray1[1] == "")
                {
                  string[] strArray2 = this.m_values[index2].Split('"');
                  ref string local = ref this.m_values[index1];
                  local = local + "," + strArray2[0];
                  ++index1;
                  flag = false;
                }
                else
                {
                  ref string local = ref this.m_values[index1];
                  local = local + "," + this.m_values[index2];
                }
              }
              else
              {
                ref string local = ref this.m_values[index1];
                local = local + "," + this.m_values[index2];
              }
            }
            else
            {
              ref string local = ref this.m_values[index1];
              local = local + "," + this.m_values[index2];
            }
          }
          else if (this.m_values[index2].Contains<char>('"'))
          {
            string str = this.m_values[index2].Trim();
            string[] strArray3 = str.Trim().Split('"');
            if (strArray3.GetLength(0) == 2)
            {
              if (strArray3[0] == "")
              {
                string[] strArray4 = this.m_values[index2].Split('"');
                this.m_values[index1] = strArray4[1];
                flag = true;
              }
              else
              {
                if (index1 != index2)
                  this.m_values[index1] = this.m_values[index2];
                ++index1;
              }
            }
            else if (strArray3.GetLength(0) == 3)
            {
              if (strArray3[0] == "" && strArray3[2] == "")
              {
                if (strArray3[1] == "")
                {
                  this.m_values[index1] = "";
                  ++index1;
                }
                else
                {
                  this.m_values[index1] = str.Substring(0, str.Length - 1).Substring(1);
                  ++index1;
                }
              }
              else
              {
                if (index1 != index2)
                  this.m_values[index1] = this.m_values[index2];
                ++index1;
              }
            }
            else
            {
              if (index1 != index2)
                this.m_values[index1] = this.m_values[index2];
              ++index1;
            }
          }
          else
          {
            if (index1 != index2)
              this.m_values[index1] = this.m_values[index2];
            ++index1;
          }
        }
        if (flag)
          this.m_Number = index1 + 1;
        else
          this.m_Number = index1;
      }
    }

    public string ElementAt(int index) => index >= 1 && index <= this.m_Number ? this.m_values[index - 1] : throw new Exception("subscript out of bound");

    public int Count => this.m_Number;
  }
}
