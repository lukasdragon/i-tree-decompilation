// Decompiled with JetBrains decompiler
// Type: Eco.Util.DataAnalyzer
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Eco.Util
{
  public class DataAnalyzer
  {
    private RecordsetView _rv;
    private ImportSpec _spec;
    private List<FieldMap> _fmList;
    private Dictionary<string, PropertyDescriptor> _props;
    private Dictionary<FieldMap, Dictionary<object, object>> _validValueMap;
    private Dictionary<FieldMap, Dictionary<object, string>> _invalidValueMap;
    private int[] _validRowIndexes;
    private int[] _invalidRowIndexes;
    private int _valid;
    private int _invalid;
    private ImportedDataList _importedData;
    private RejectedDataList _rejectedData;

    public DataAnalyzer(RecordsetView rv, ImportSpec spec, List<FieldMap> fmList)
    {
      this._rv = rv;
      this._spec = spec;
      this._fmList = fmList != null ? fmList.Where<FieldMap>((Func<FieldMap, bool>) (fm => !string.IsNullOrEmpty(fm.SourceColumn) || fm.DefaultValue != null)).ToList<FieldMap>() : (List<FieldMap>) null;
      this._validRowIndexes = new int[this._rv.Count];
      this._invalidRowIndexes = new int[this._rv.Count];
      this._valid = -1;
      this._invalid = -1;
      this._validValueMap = new Dictionary<FieldMap, Dictionary<object, object>>();
      this._invalidValueMap = new Dictionary<FieldMap, Dictionary<object, string>>();
      this._props = new Dictionary<string, PropertyDescriptor>();
    }

    public void Analyze(IProgress<ProgressEventArgs> progress, CancellationToken ct)
    {
      this._valid = 0;
      this._invalid = 0;
      foreach (FieldMap fm in this._fmList)
      {
        Dictionary<object, object> dictionary1 = new Dictionary<object, object>();
        Dictionary<object, string> dictionary2 = new Dictionary<object, string>();
        PropertyDescriptor propertyDescriptor;
        if (!this._props.TryGetValue(fm.FieldSpec.Field, out propertyDescriptor))
        {
          propertyDescriptor = this._spec.Properties.Find(fm.FieldSpec.Field, true);
          if (propertyDescriptor != null)
            this._props[fm.FieldSpec.Field] = propertyDescriptor;
        }
        if (propertyDescriptor != null)
        {
          foreach (KeyValuePair<object, object> keyValuePair in fm.Map)
          {
            object key = keyValuePair.Key;
            object component = keyValuePair.Value;
            string str = (string) null;
            if (component != null && fm.ValueMember != null)
              component = TypeDescriptor.GetProperties(component.GetType()).Find(fm.ValueMember, true)?.GetValue(component);
            if (component != null)
            {
              object dest;
              if (!TypeHelper.TryConvert(component, propertyDescriptor.PropertyType, out dest))
              {
                str = string.Format(Strings.ErrConvert, component, (object) DataAnalyzer.GetTypeDesc(propertyDescriptor.PropertyType));
                component = (object) null;
              }
              else
                component = dest;
            }
            else if (fm.Data == null && !propertyDescriptor.PropertyType.IsEnum && !fm.NeedsMapped)
              str = string.Format(Strings.ErrConvert, key, (object) DataAnalyzer.GetTypeDesc(propertyDescriptor.PropertyType));
            if (component == null && fm.FieldSpec.NullValue != null && !fm.FieldSpec.Required)
              component = fm.FieldSpec.NullValue;
            else if (component != null && fm.FieldSpec.Constraints.Count > 0)
            {
              foreach (FieldConstraint constraint in fm.FieldSpec.Constraints)
              {
                if (!constraint.IsValid(component))
                {
                  str = constraint.FormatError((object) string.Format("'{0}'", component));
                  component = (object) null;
                  break;
                }
              }
            }
            if (component != null)
              dictionary1[key] = component;
            else if (str != null)
              dictionary2[key] = str;
          }
        }
        this._validValueMap[fm] = dictionary1;
        this._invalidValueMap[fm] = dictionary2;
      }
      int count = this._rv.Count;
      int num1 = 0;
      int num2 = Math.Max(count / 100, 1);
      foreach (RecordsetRow rr in this._rv)
      {
        ct.ThrowIfCancellationRequested();
        if (num1 % num2 == 0)
          progress.Report(new ProgressEventArgs(Strings.MsgAnalyzingData, count / num2, num1 / num2));
        if (this.Process(rr) != null)
          this._validRowIndexes[this._valid++] = num1++;
        else
          this._invalidRowIndexes[this._invalid++] = num1++;
      }
      this._importedData = new ImportedDataList(this, this._validRowIndexes, this._valid);
      this._rejectedData = new RejectedDataList(this, this._invalidRowIndexes, this._invalid);
      progress.Report(new ProgressEventArgs(Strings.MsgAnalyzingData, count / num2, num1 / num2));
    }

    public IList ImportedData => (IList) this._importedData;

    public IList RejectedData => (IList) this._rejectedData;

    internal object Process(int row) => this.Process(this._rv[row] as RecordsetRow);

    internal object Process(RecordsetRow rr)
    {
      object instance = Activator.CreateInstance(this._spec.ListType);
      bool flag = true;
      foreach (FieldMap fm in this._fmList)
      {
        PropertyDescriptor propertyDescriptor;
        if (!this._props.TryGetValue(fm.FieldSpec.Field, out propertyDescriptor))
        {
          propertyDescriptor = this._spec.Properties.Find(fm.FieldSpec.Field, true);
          if (propertyDescriptor != null)
            this._props[fm.FieldSpec.Field] = propertyDescriptor;
        }
        if (propertyDescriptor != null)
        {
          object defaultValue;
          if (string.IsNullOrEmpty(fm.SourceColumn))
            defaultValue = fm.DefaultValue;
          else
            this._validValueMap[fm].TryGetValue(rr[fm.SourceColumn], out defaultValue);
          if (defaultValue != null)
            propertyDescriptor.SetValue(instance, defaultValue);
          else if (fm.FieldSpec.Required)
          {
            flag = false;
            break;
          }
        }
        else if (fm.FieldSpec.Required)
        {
          flag = false;
          break;
        }
      }
      return flag ? instance : (object) null;
    }

    internal string GetError(int row) => this.GetError(this._rv[row] as RecordsetRow);

    internal string GetError(RecordsetRow rr)
    {
      string error = (string) null;
      foreach (FieldMap fm in this._fmList)
      {
        if (!string.IsNullOrEmpty(fm.SourceColumn))
        {
          Dictionary<object, object> validValue = this._validValueMap[fm];
          Dictionary<object, string> invalidValue = this._invalidValueMap[fm];
          object key1 = rr[fm.SourceColumn];
          object key2 = key1;
          object obj;
          ref object local = ref obj;
          validValue.TryGetValue(key2, out local);
          if (obj == null)
          {
            if (!fm.NeedsMapped && string.IsNullOrEmpty(key1.ToString()))
              error = Strings.ErrNoValue;
            else if (!invalidValue.TryGetValue(key1, out error))
              error = string.Format(Strings.ErrUnmapped, key1);
            if (fm.FieldSpec.Required)
            {
              error = string.Format(Strings.ErrRequired, (object) fm.FieldSpec.Description, (object) error);
              break;
            }
            break;
          }
        }
      }
      return error;
    }

    private static string GetTypeDesc(Type t)
    {
      if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof (Nullable<>)))
        t = t.GetGenericArguments()[0];
      string typeDesc = t.Name;
      switch (Type.GetTypeCode(t))
      {
        case TypeCode.Int16:
        case TypeCode.Int32:
        case TypeCode.Int64:
          typeDesc = Strings.TypeInteger;
          break;
        case TypeCode.UInt16:
        case TypeCode.UInt32:
        case TypeCode.UInt64:
          typeDesc = Strings.TypePositiveInteger;
          break;
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          typeDesc = Strings.TypeDecimal;
          break;
        case TypeCode.DateTime:
          typeDesc = Strings.TypeDateTime;
          break;
        case TypeCode.String:
          typeDesc = Strings.TypeString;
          break;
      }
      return typeDesc;
    }

    internal PropertyDescriptorCollection GetItemProperties(
      PropertyDescriptor[] listAccessors)
    {
      return this._spec.Properties;
    }

    internal string GetListName(PropertyDescriptor[] listAccessors) => this._rv.GetListName(listAccessors);

    internal object SyncRoot => this._rv.SyncRoot;
  }
}
