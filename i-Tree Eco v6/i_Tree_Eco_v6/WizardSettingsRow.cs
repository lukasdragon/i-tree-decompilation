// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.WizardSettingsRow
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using System;

namespace i_Tree_Eco_v6
{
  internal class WizardSettingsRow
  {
    private WizardPage editPage;
    private string labelText;
    private Type settingDataType;
    private object value;
    private string headerText;

    public WizardPage EditPage
    {
      get => this.editPage;
      set => this.editPage = value;
    }

    public string LabelText
    {
      get => this.labelText;
      set => this.labelText = value;
    }

    public Type SettingDataType
    {
      get => this.settingDataType;
      set => this.settingDataType = value;
    }

    public object Value
    {
      get => this.value;
      set => this.value = value;
    }

    public string HeaderText
    {
      get => this.headerText;
      set => this.headerText = value;
    }
  }
}
