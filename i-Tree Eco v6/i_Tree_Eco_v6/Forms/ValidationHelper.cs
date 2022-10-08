// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ValidationHelper
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System;

namespace i_Tree_Eco_v6.Forms
{
  internal class ValidationHelper
  {
    public static bool ValidateCoordinates(object x, object y)
    {
      if (x != DBNull.Value)
      {
        if (y != DBNull.Value)
        {
          try
          {
            double num1 = Convert.ToDouble(x);
            double num2 = Convert.ToDouble(y);
            if (num2 >= -90.0 && num2 <= 90.0 && num1 >= -180.0)
            {
              if (num1 <= 180.0)
                goto label_7;
            }
            return false;
          }
          catch
          {
            return false;
          }
label_7:
          return true;
        }
      }
      return false;
    }
  }
}
