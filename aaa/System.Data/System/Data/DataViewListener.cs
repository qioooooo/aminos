using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x020000AA RID: 170
	internal sealed class DataViewListener
	{
		// Token: 0x06000BAA RID: 2986 RVA: 0x001F8640 File Offset: 0x001F7A40
		internal DataViewListener(DataView dv)
		{
			this.ObjectID = dv.ObjectID;
			this._dvWeak = new WeakReference(dv);
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x001F866C File Offset: 0x001F7A6C
		private void ChildRelationCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			DataView dataView = (DataView)this._dvWeak.Target;
			if (dataView != null)
			{
				dataView.ChildRelationCollectionChanged(sender, e);
				return;
			}
			this.CleanUp(true);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x001F86A0 File Offset: 0x001F7AA0
		private void ParentRelationCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			DataView dataView = (DataView)this._dvWeak.Target;
			if (dataView != null)
			{
				dataView.ParentRelationCollectionChanged(sender, e);
				return;
			}
			this.CleanUp(true);
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x001F86D4 File Offset: 0x001F7AD4
		private void ColumnCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			DataView dataView = (DataView)this._dvWeak.Target;
			if (dataView != null)
			{
				dataView.ColumnCollectionChangedInternal(sender, e);
				return;
			}
			this.CleanUp(true);
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x001F8708 File Offset: 0x001F7B08
		internal void MaintainDataView(ListChangedType changedType, DataRow row, bool trackAddRemove)
		{
			DataView dataView = (DataView)this._dvWeak.Target;
			if (dataView != null)
			{
				dataView.MaintainDataView(changedType, row, trackAddRemove);
				return;
			}
			this.CleanUp(true);
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x001F873C File Offset: 0x001F7B3C
		internal void IndexListChanged(ListChangedEventArgs e)
		{
			DataView dataView = (DataView)this._dvWeak.Target;
			if (dataView != null)
			{
				dataView.IndexListChangedInternal(e);
				return;
			}
			this.CleanUp(true);
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x001F876C File Offset: 0x001F7B6C
		internal void RegisterMetaDataEvents(DataTable table)
		{
			this._table = table;
			if (table != null)
			{
				this.RegisterListener(table);
				CollectionChangeEventHandler collectionChangeEventHandler = new CollectionChangeEventHandler(this.ColumnCollectionChanged);
				table.Columns.ColumnPropertyChanged += collectionChangeEventHandler;
				table.Columns.CollectionChanged += collectionChangeEventHandler;
				CollectionChangeEventHandler collectionChangeEventHandler2 = new CollectionChangeEventHandler(this.ChildRelationCollectionChanged);
				((DataRelationCollection.DataTableRelationCollection)table.ChildRelations).RelationPropertyChanged += collectionChangeEventHandler2;
				table.ChildRelations.CollectionChanged += collectionChangeEventHandler2;
				CollectionChangeEventHandler collectionChangeEventHandler3 = new CollectionChangeEventHandler(this.ParentRelationCollectionChanged);
				((DataRelationCollection.DataTableRelationCollection)table.ParentRelations).RelationPropertyChanged += collectionChangeEventHandler3;
				table.ParentRelations.CollectionChanged += collectionChangeEventHandler3;
			}
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x001F8808 File Offset: 0x001F7C08
		internal void UnregisterMetaDataEvents()
		{
			this.UnregisterMetaDataEvents(true);
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x001F881C File Offset: 0x001F7C1C
		private void UnregisterMetaDataEvents(bool updateListeners)
		{
			DataTable table = this._table;
			this._table = null;
			if (table != null)
			{
				CollectionChangeEventHandler collectionChangeEventHandler = new CollectionChangeEventHandler(this.ColumnCollectionChanged);
				table.Columns.ColumnPropertyChanged -= collectionChangeEventHandler;
				table.Columns.CollectionChanged -= collectionChangeEventHandler;
				CollectionChangeEventHandler collectionChangeEventHandler2 = new CollectionChangeEventHandler(this.ChildRelationCollectionChanged);
				((DataRelationCollection.DataTableRelationCollection)table.ChildRelations).RelationPropertyChanged -= collectionChangeEventHandler2;
				table.ChildRelations.CollectionChanged -= collectionChangeEventHandler2;
				CollectionChangeEventHandler collectionChangeEventHandler3 = new CollectionChangeEventHandler(this.ParentRelationCollectionChanged);
				((DataRelationCollection.DataTableRelationCollection)table.ParentRelations).RelationPropertyChanged -= collectionChangeEventHandler3;
				table.ParentRelations.CollectionChanged -= collectionChangeEventHandler3;
				if (updateListeners)
				{
					List<DataViewListener> listeners = table.GetListeners();
					lock (listeners)
					{
						listeners.Remove(this);
					}
				}
			}
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x001F88FC File Offset: 0x001F7CFC
		internal void RegisterListChangedEvent(Index index)
		{
			this._index = index;
			if (index != null)
			{
				lock (index)
				{
					index.AddRef();
					index.ListChangedAdd(this);
				}
			}
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x001F8950 File Offset: 0x001F7D50
		internal void UnregisterListChangedEvent()
		{
			Index index = this._index;
			this._index = null;
			if (index != null)
			{
				lock (index)
				{
					index.ListChangedRemove(this);
					if (index.RemoveRef() <= 1)
					{
						index.RemoveRef();
					}
				}
			}
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x001F89B4 File Offset: 0x001F7DB4
		private void CleanUp(bool updateListeners)
		{
			this.UnregisterMetaDataEvents(updateListeners);
			this.UnregisterListChangedEvent();
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x001F89D0 File Offset: 0x001F7DD0
		private void RegisterListener(DataTable table)
		{
			List<DataViewListener> listeners = table.GetListeners();
			lock (listeners)
			{
				int num = listeners.Count - 1;
				while (0 <= num)
				{
					DataViewListener dataViewListener = listeners[num];
					if (!dataViewListener._dvWeak.IsAlive)
					{
						listeners.RemoveAt(num);
						dataViewListener.CleanUp(false);
					}
					num--;
				}
				listeners.Add(this);
			}
		}

		// Token: 0x0400084B RID: 2123
		private readonly WeakReference _dvWeak;

		// Token: 0x0400084C RID: 2124
		private DataTable _table;

		// Token: 0x0400084D RID: 2125
		private Index _index;

		// Token: 0x0400084E RID: 2126
		internal readonly int ObjectID;
	}
}
