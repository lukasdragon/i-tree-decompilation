// Decompiled with JetBrains decompiler
// Type: Eco.Util.DataGridViewManager
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.Controls.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Eco.Util
{
  public class DataGridViewManager
  {
    private Stack<DataGridViewManager.Action> m_undoStack;
    private Stack<DataGridViewManager.Action> m_redoStack;
    private Dictionary<int, DataGridViewManager.TrackObject> m_trackObjects;
    private DataGridView m_grid;
    private IDataBindingList m_dataSource;
    private bool m_bindingComplete;
    private bool m_inUndoRedo;
    private object m_previousCellValue;
    private bool m_cellDirty;
    private bool m_rowDirty;
    private Dictionary<string, object> m_oldValues;

    public DataGridViewManager(DataGridView grid)
    {
      this.m_undoStack = new Stack<DataGridViewManager.Action>();
      this.m_redoStack = new Stack<DataGridViewManager.Action>();
      this.m_trackObjects = new Dictionary<int, DataGridViewManager.TrackObject>();
      this.m_oldValues = new Dictionary<string, object>();
      this.m_grid = grid;
      this.m_dataSource = grid.DataSource as IDataBindingList;
      this.m_bindingComplete = this.m_dataSource != null;
      if (this.m_bindingComplete)
      {
        this.m_dataSource.ListChanged += new ListChangedEventHandler(this.DataSource_ListChanged);
        this.m_dataSource.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.DataSource_BeforeRemove);
      }
      this.m_grid.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.Grid_CellBeginEdit);
      this.m_grid.CellEndEdit += new DataGridViewCellEventHandler(this.Grid_CellEndEdit);
      this.m_grid.DataSourceChanged += new EventHandler(this.Grid_DataSourceChanged);
      this.m_grid.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.Grid_DataBindingComplete);
    }

    private void Grid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) => this.m_bindingComplete = true;

    private void Grid_DataSourceChanged(object sender, EventArgs e)
    {
      if (this.m_dataSource != null)
      {
        this.m_dataSource.ListChanged -= new ListChangedEventHandler(this.DataSource_ListChanged);
        this.m_dataSource.BeforeRemove -= new EventHandler<ListChangedEventArgs>(this.DataSource_BeforeRemove);
      }
      this.m_dataSource = this.m_grid.DataSource as IDataBindingList;
      if (this.m_dataSource != null)
      {
        this.m_dataSource.ListChanged += new ListChangedEventHandler(this.DataSource_ListChanged);
        this.m_dataSource.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.DataSource_BeforeRemove);
      }
      this.m_bindingComplete = false;
      this.Clear();
    }

    public bool CanUndo => this.m_undoStack.Count > 0 || this.IsCurrentCellDirty;

    public bool CanRedo => !this.m_grid.IsCurrentCellDirty && this.m_redoStack.Count > 0;

    public void Clear()
    {
      this.m_undoStack.Clear();
      this.m_redoStack.Clear();
      this.m_trackObjects.Clear();
    }

    private void Push(object o, DataGridViewManager.Action action)
    {
      if (this.m_trackObjects.ContainsKey(action.HashKey))
      {
        DataGridViewManager.TrackObject trackObject = this.m_trackObjects[action.HashKey];
        trackObject.Value = o;
        ++trackObject.RefCount;
      }
      else
      {
        DataGridViewManager.TrackObject trackObject = new DataGridViewManager.TrackObject(o);
        ++trackObject.RefCount;
        this.m_trackObjects[action.HashKey] = trackObject;
      }
      this.m_undoStack.Push(action);
      foreach (DataGridViewManager.Action redo in this.m_redoStack)
      {
        DataGridViewManager.TrackObject trackObject = this.m_trackObjects[redo.HashKey];
        --trackObject.RefCount;
        if (trackObject.RefCount == 0)
          this.m_trackObjects.Remove(redo.HashKey);
      }
      this.m_redoStack.Clear();
    }

    private bool IsCurrentCellDirty
    {
      get
      {
        bool currentCellDirty = false;
        if (this.m_grid.CurrentCell != null)
        {
          object v2 = this.m_grid.CurrentCell.Value;
          currentCellDirty = this.m_grid.IsCurrentCellDirty || this.m_grid.IsCurrentCellInEditMode && !this.ValuesEqual(this.m_previousCellValue, v2);
        }
        return currentCellDirty;
      }
    }

    public void Undo()
    {
      if (this.IsCurrentCellDirty)
        this.m_grid.EndEdit();
      if (this.m_undoStack.Count == 0)
        return;
      DataGridViewManager.Action action = this.m_undoStack.Pop();
      this.m_redoStack.Push(action);
      switch (action.Change)
      {
        case DataGridViewManager.Action.ChangeMode.Add:
          this.RemoveRow(action);
          break;
        case DataGridViewManager.Action.ChangeMode.Delete:
          this.InsertRow(action);
          break;
        case DataGridViewManager.Action.ChangeMode.Modify:
          this.UpdateCell(action);
          break;
        default:
          throw new InvalidOperationException("Unknown undo action change: " + action.Change.ToString());
      }
    }

    public void Redo()
    {
      if (this.m_redoStack.Count == 0)
        return;
      DataGridViewManager.Action action = this.m_redoStack.Pop();
      this.m_undoStack.Push(action);
      switch (action.Change)
      {
        case DataGridViewManager.Action.ChangeMode.Add:
          this.InsertRow(action);
          break;
        case DataGridViewManager.Action.ChangeMode.Delete:
          this.RemoveRow(action);
          break;
        case DataGridViewManager.Action.ChangeMode.Modify:
          this.UpdateCell(action);
          break;
        default:
          throw new InvalidOperationException("Unknown redo action change: " + action.Change.ToString());
      }
    }

    private void UpdateCell(DataGridViewManager.Action action)
    {
      Dictionary<string, object> dictionary1 = action.Arguments[0] as Dictionary<string, object>;
      this.m_inUndoRedo = true;
      DataGridViewCell currentCell = this.m_grid.CurrentCell;
      IList<string> stringList = (IList<string>) new List<string>((IEnumerable<string>) dictionary1.Keys);
      Dictionary<string, object> dictionary2 = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      object component = this.m_trackObjects[action.HashKey].Value;
      for (int index = stringList.Count - 1; index >= 0; --index)
      {
        string str = stringList[index];
        PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component).Find(str, true);
        if (propertyDescriptor != null)
        {
          object obj = propertyDescriptor.GetValue(component);
          propertyDescriptor.SetValue(component, dictionary1[str]);
          dictionary2[str] = obj;
        }
      }
      action.Arguments[0] = (object) dictionary2;
      string key = action.Arguments[1] as string;
      bool dirty = (bool) action.Arguments[2];
      bool rowDirty = (bool) action.Arguments[3];
      int rowIndex = this.m_dataSource.IndexOf(component);
      int columnIndex = -1;
      for (int index = 0; index < this.m_grid.Columns.Count; ++index)
      {
        DataGridViewColumn column = this.m_grid.Columns[index];
        if (key.Equals(column.DataPropertyName, StringComparison.OrdinalIgnoreCase))
        {
          columnIndex = index;
          break;
        }
      }
      if (currentCell == null || columnIndex != -1 && currentCell.ColumnIndex != columnIndex || currentCell.RowIndex != rowIndex)
        this.m_grid.CurrentCell = this.m_grid[columnIndex, rowIndex];
      else if (currentCell.IsInEditMode)
      {
        this.m_previousCellValue = dictionary1[key];
        this.m_grid.RefreshEdit();
      }
      if (this.m_grid.CurrentCell != null && this.m_grid.IsCurrentCellDirty != dirty)
      {
        this.m_grid.NotifyCurrentCellDirty(dirty);
        action.Arguments[2] = (object) !dirty;
      }
      if (this.m_grid.CurrentRow != null && this.m_grid.IsCurrentRowDirty != rowDirty)
      {
        this.m_grid.NotifyCurrentRowDirty(rowDirty);
        action.Arguments[3] = (object) !rowDirty;
      }
      this.m_inUndoRedo = false;
    }

    private void InsertRow(DataGridViewManager.Action action)
    {
      if (this.m_dataSource == null)
        return;
      this.m_inUndoRedo = true;
      bool flag = (bool) action.Arguments[0];
      ICloneable cloneable = this.m_trackObjects[action.HashKey].Value as ICloneable;
      object obj = cloneable.Clone();
      foreach (KeyValuePair<int, DataGridViewManager.TrackObject> trackObject1 in this.m_trackObjects)
      {
        DataGridViewManager.TrackObject trackObject2 = trackObject1.Value;
        if (trackObject2.Value == cloneable)
          trackObject2.Value = obj;
      }
      this.m_dataSource.Add(obj);
      if (!flag)
      {
        DataGridViewColumn sortedColumn = this.m_grid.SortedColumn;
        if (sortedColumn != null)
        {
          switch (this.m_grid.SortOrder)
          {
            case SortOrder.Ascending:
              this.m_grid.Sort(sortedColumn, ListSortDirection.Ascending);
              break;
            case SortOrder.Descending:
              this.m_grid.Sort(sortedColumn, ListSortDirection.Descending);
              break;
          }
        }
      }
      this.m_grid.BindingContext[(object) this.m_dataSource].Position = this.m_dataSource.IndexOf(obj);
      if (flag)
        this.m_grid.NotifyCurrentRowDirty(true);
      this.m_inUndoRedo = false;
    }

    private void RemoveRow(DataGridViewManager.Action action)
    {
      object obj = this.m_trackObjects[action.HashKey].Value;
      if (this.m_dataSource == null || !this.m_dataSource.Contains(obj))
        return;
      this.m_inUndoRedo = true;
      this.m_dataSource.Remove(obj);
      CurrencyManager currencyManager = this.m_grid.BindingContext[(object) this.m_dataSource] as CurrencyManager;
      if (currencyManager.Position != -1)
      {
        this.m_grid.Rows[currencyManager.Position].Selected = true;
      }
      else
      {
        foreach (DataGridViewBand selectedRow in (BaseCollection) this.m_grid.SelectedRows)
          selectedRow.Selected = false;
        this.m_grid.CurrentCell = (DataGridViewCell) null;
      }
      this.m_inUndoRedo = false;
    }

    private void Grid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      this.m_previousCellValue = this.m_grid[e.ColumnIndex, e.RowIndex].Value;
      this.m_cellDirty = this.m_grid.IsCurrentCellDirty;
      this.m_rowDirty = this.m_grid.IsCurrentRowDirty;
      if (this.m_dataSource == null || e.RowIndex >= this.m_dataSource.Count || !(this.m_dataSource[e.RowIndex] is INotifyPropertyChanging propertyChanging))
        return;
      this.m_oldValues = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      propertyChanging.PropertyChanging += new PropertyChangingEventHandler(this.TrackObject_PropertyChanging);
    }

    private void TrackObject_PropertyChanging(object sender, PropertyChangingEventArgs e)
    {
      if (this.m_inUndoRedo)
        return;
      PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(sender).Find(e.PropertyName, true);
      if (propertyDescriptor == null)
        return;
      this.m_oldValues[e.PropertyName] = propertyDescriptor.GetValue(sender);
    }

    private void Grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (this.m_inUndoRedo || e.RowIndex < 0 || this.m_dataSource == null || e.RowIndex >= this.m_dataSource.Count)
        return;
      object o = this.m_dataSource[e.RowIndex];
      if (!(o is INotifyPropertyChanging propertyChanging))
        return;
      propertyChanging.PropertyChanging -= new PropertyChangingEventHandler(this.TrackObject_PropertyChanging);
      if (this.m_oldValues.Count <= 0)
        return;
      DataGridViewCell dataGridViewCell = this.m_grid[e.ColumnIndex, e.RowIndex];
      int hashCode = o.GetHashCode();
      this.Push(o, new DataGridViewManager.Action(hashCode, DataGridViewManager.Action.ChangeMode.Modify, new object[4]
      {
        (object) this.m_oldValues,
        (object) dataGridViewCell.OwningColumn.DataPropertyName,
        (object) this.m_cellDirty,
        (object) this.m_rowDirty
      }));
    }

    private bool ValuesEqual(object v1, object v2) => v1 == null ? v2 == null : v1.Equals(v2);

    private void DataSource_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType != ListChangedType.ItemAdded || this.m_inUndoRedo)
        return;
      object o = this.m_dataSource[e.NewIndex];
      int hashCode = o.GetHashCode();
      this.Push(o, new DataGridViewManager.Action(hashCode, DataGridViewManager.Action.ChangeMode.Add, new object[1]
      {
        (object) (e.NewIndex == this.m_grid.NewRowIndex)
      }));
    }

    private void DataSource_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      if (this.m_inUndoRedo || this.m_dataSource == null || !this.m_bindingComplete)
        return;
      ICloneable cloneable = this.m_dataSource[e.NewIndex] as ICloneable;
      this.m_inUndoRedo = true;
      object o = cloneable.Clone();
      int hashCode = o.GetHashCode();
      foreach (KeyValuePair<int, DataGridViewManager.TrackObject> trackObject1 in this.m_trackObjects)
      {
        DataGridViewManager.TrackObject trackObject2 = trackObject1.Value;
        if (trackObject2.Value == cloneable)
          trackObject2.Value = o;
      }
      this.Push(o, new DataGridViewManager.Action(hashCode, DataGridViewManager.Action.ChangeMode.Delete, new object[1]
      {
        (object) (e.NewIndex == this.m_grid.NewRowIndex)
      }));
      this.m_inUndoRedo = false;
    }

    private class TrackObject
    {
      public object Value;
      public int RefCount;

      public TrackObject(object obj) => this.Value = obj;
    }

    private class Action
    {
      public Action(
        int hashKey,
        DataGridViewManager.Action.ChangeMode change,
        params object[] arguments)
      {
        this.HashKey = hashKey;
        this.Change = change;
        this.Arguments = arguments;
      }

      public int HashKey { get; private set; }

      public DataGridViewManager.Action.ChangeMode Change { get; private set; }

      public object[] Arguments { get; private set; }

      public enum ChangeMode
      {
        Add,
        Delete,
        Modify,
      }
    }
  }
}
