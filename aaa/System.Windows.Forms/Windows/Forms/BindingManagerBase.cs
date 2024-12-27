using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x02000240 RID: 576
	public abstract class BindingManagerBase
	{
		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x000357C0 File Offset: 0x000347C0
		public BindingsCollection Bindings
		{
			get
			{
				if (this.bindings == null)
				{
					this.bindings = new ListManagerBindingsCollection(this);
					this.bindings.CollectionChanging += this.OnBindingsCollectionChanging;
					this.bindings.CollectionChanged += this.OnBindingsCollectionChanged;
				}
				return this.bindings;
			}
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x00035815 File Offset: 0x00034815
		protected internal void OnBindingComplete(BindingCompleteEventArgs args)
		{
			if (this.onBindingCompleteHandler != null)
			{
				this.onBindingCompleteHandler(this, args);
			}
		}

		// Token: 0x06001B7A RID: 7034
		protected internal abstract void OnCurrentChanged(EventArgs e);

		// Token: 0x06001B7B RID: 7035
		protected internal abstract void OnCurrentItemChanged(EventArgs e);

		// Token: 0x06001B7C RID: 7036 RVA: 0x0003582C File Offset: 0x0003482C
		protected internal void OnDataError(Exception e)
		{
			if (this.onDataErrorHandler != null)
			{
				this.onDataErrorHandler(this, new BindingManagerDataErrorEventArgs(e));
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06001B7D RID: 7037
		public abstract object Current { get; }

		// Token: 0x06001B7E RID: 7038
		internal abstract void SetDataSource(object dataSource);

		// Token: 0x06001B7F RID: 7039 RVA: 0x00035848 File Offset: 0x00034848
		public BindingManagerBase()
		{
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x00035850 File Offset: 0x00034850
		internal BindingManagerBase(object dataSource)
		{
			this.SetDataSource(dataSource);
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06001B81 RID: 7041
		internal abstract Type BindType { get; }

		// Token: 0x06001B82 RID: 7042
		internal abstract PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors);

		// Token: 0x06001B83 RID: 7043 RVA: 0x0003585F File Offset: 0x0003485F
		public virtual PropertyDescriptorCollection GetItemProperties()
		{
			return this.GetItemProperties(null);
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x00035868 File Offset: 0x00034868
		protected internal virtual PropertyDescriptorCollection GetItemProperties(ArrayList dataSources, ArrayList listAccessors)
		{
			IList list = null;
			if (this is CurrencyManager)
			{
				list = ((CurrencyManager)this).List;
			}
			if (list is ITypedList)
			{
				PropertyDescriptor[] array = new PropertyDescriptor[listAccessors.Count];
				listAccessors.CopyTo(array, 0);
				return ((ITypedList)list).GetItemProperties(array);
			}
			return this.GetItemProperties(this.BindType, 0, dataSources, listAccessors);
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x000358C4 File Offset: 0x000348C4
		protected virtual PropertyDescriptorCollection GetItemProperties(Type listType, int offset, ArrayList dataSources, ArrayList listAccessors)
		{
			if (listAccessors.Count < offset)
			{
				return null;
			}
			if (listAccessors.Count != offset)
			{
				PropertyInfo[] properties = listType.GetProperties();
				if (typeof(IList).IsAssignableFrom(listType))
				{
					PropertyDescriptorCollection propertyDescriptorCollection = null;
					for (int i = 0; i < properties.Length; i++)
					{
						if ("Item".Equals(properties[i].Name) && properties[i].PropertyType != typeof(object))
						{
							propertyDescriptorCollection = TypeDescriptor.GetProperties(properties[i].PropertyType, new Attribute[]
							{
								new BrowsableAttribute(true)
							});
						}
					}
					if (propertyDescriptorCollection == null)
					{
						IList list;
						if (offset == 0)
						{
							list = this.DataSource as IList;
						}
						else
						{
							list = dataSources[offset - 1] as IList;
						}
						if (list != null && list.Count > 0)
						{
							propertyDescriptorCollection = TypeDescriptor.GetProperties(list[0]);
						}
					}
					if (propertyDescriptorCollection != null)
					{
						for (int j = 0; j < propertyDescriptorCollection.Count; j++)
						{
							if (propertyDescriptorCollection[j].Equals(listAccessors[offset]))
							{
								return this.GetItemProperties(propertyDescriptorCollection[j].PropertyType, offset + 1, dataSources, listAccessors);
							}
						}
					}
				}
				else
				{
					for (int k = 0; k < properties.Length; k++)
					{
						if (properties[k].Name.Equals(((PropertyDescriptor)listAccessors[offset]).Name))
						{
							return this.GetItemProperties(properties[k].PropertyType, offset + 1, dataSources, listAccessors);
						}
					}
				}
				return null;
			}
			if (!typeof(IList).IsAssignableFrom(listType))
			{
				return TypeDescriptor.GetProperties(listType);
			}
			PropertyInfo[] properties2 = listType.GetProperties();
			for (int l = 0; l < properties2.Length; l++)
			{
				if ("Item".Equals(properties2[l].Name) && properties2[l].PropertyType != typeof(object))
				{
					return TypeDescriptor.GetProperties(properties2[l].PropertyType, new Attribute[]
					{
						new BrowsableAttribute(true)
					});
				}
			}
			IList list2 = dataSources[offset - 1] as IList;
			if (list2 != null && list2.Count > 0)
			{
				return TypeDescriptor.GetProperties(list2[0]);
			}
			return null;
		}

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06001B86 RID: 7046 RVA: 0x00035AEE File Offset: 0x00034AEE
		// (remove) Token: 0x06001B87 RID: 7047 RVA: 0x00035B07 File Offset: 0x00034B07
		public event BindingCompleteEventHandler BindingComplete
		{
			add
			{
				this.onBindingCompleteHandler = (BindingCompleteEventHandler)Delegate.Combine(this.onBindingCompleteHandler, value);
			}
			remove
			{
				this.onBindingCompleteHandler = (BindingCompleteEventHandler)Delegate.Remove(this.onBindingCompleteHandler, value);
			}
		}

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06001B88 RID: 7048 RVA: 0x00035B20 File Offset: 0x00034B20
		// (remove) Token: 0x06001B89 RID: 7049 RVA: 0x00035B39 File Offset: 0x00034B39
		public event EventHandler CurrentChanged
		{
			add
			{
				this.onCurrentChangedHandler = (EventHandler)Delegate.Combine(this.onCurrentChangedHandler, value);
			}
			remove
			{
				this.onCurrentChangedHandler = (EventHandler)Delegate.Remove(this.onCurrentChangedHandler, value);
			}
		}

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x06001B8A RID: 7050 RVA: 0x00035B52 File Offset: 0x00034B52
		// (remove) Token: 0x06001B8B RID: 7051 RVA: 0x00035B6B File Offset: 0x00034B6B
		public event EventHandler CurrentItemChanged
		{
			add
			{
				this.onCurrentItemChangedHandler = (EventHandler)Delegate.Combine(this.onCurrentItemChangedHandler, value);
			}
			remove
			{
				this.onCurrentItemChangedHandler = (EventHandler)Delegate.Remove(this.onCurrentItemChangedHandler, value);
			}
		}

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06001B8C RID: 7052 RVA: 0x00035B84 File Offset: 0x00034B84
		// (remove) Token: 0x06001B8D RID: 7053 RVA: 0x00035B9D File Offset: 0x00034B9D
		public event BindingManagerDataErrorEventHandler DataError
		{
			add
			{
				this.onDataErrorHandler = (BindingManagerDataErrorEventHandler)Delegate.Combine(this.onDataErrorHandler, value);
			}
			remove
			{
				this.onDataErrorHandler = (BindingManagerDataErrorEventHandler)Delegate.Remove(this.onDataErrorHandler, value);
			}
		}

		// Token: 0x06001B8E RID: 7054
		internal abstract string GetListName();

		// Token: 0x06001B8F RID: 7055
		public abstract void CancelCurrentEdit();

		// Token: 0x06001B90 RID: 7056
		public abstract void EndCurrentEdit();

		// Token: 0x06001B91 RID: 7057
		public abstract void AddNew();

		// Token: 0x06001B92 RID: 7058
		public abstract void RemoveAt(int index);

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06001B93 RID: 7059
		// (set) Token: 0x06001B94 RID: 7060
		public abstract int Position { get; set; }

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06001B95 RID: 7061 RVA: 0x00035BB6 File Offset: 0x00034BB6
		// (remove) Token: 0x06001B96 RID: 7062 RVA: 0x00035BCF File Offset: 0x00034BCF
		public event EventHandler PositionChanged
		{
			add
			{
				this.onPositionChangedHandler = (EventHandler)Delegate.Combine(this.onPositionChangedHandler, value);
			}
			remove
			{
				this.onPositionChangedHandler = (EventHandler)Delegate.Remove(this.onPositionChangedHandler, value);
			}
		}

		// Token: 0x06001B97 RID: 7063
		protected abstract void UpdateIsBinding();

		// Token: 0x06001B98 RID: 7064
		protected internal abstract string GetListName(ArrayList listAccessors);

		// Token: 0x06001B99 RID: 7065
		public abstract void SuspendBinding();

		// Token: 0x06001B9A RID: 7066
		public abstract void ResumeBinding();

		// Token: 0x06001B9B RID: 7067 RVA: 0x00035BE8 File Offset: 0x00034BE8
		protected void PullData()
		{
			bool flag;
			this.PullData(out flag);
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x00035C00 File Offset: 0x00034C00
		internal void PullData(out bool success)
		{
			success = true;
			this.pullingData = true;
			try
			{
				this.UpdateIsBinding();
				int count = this.Bindings.Count;
				for (int i = 0; i < count; i++)
				{
					if (this.Bindings[i].PullData())
					{
						success = false;
					}
				}
			}
			finally
			{
				this.pullingData = false;
			}
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x00035C68 File Offset: 0x00034C68
		protected void PushData()
		{
			bool flag;
			this.PushData(out flag);
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x00035C80 File Offset: 0x00034C80
		internal void PushData(out bool success)
		{
			success = true;
			if (this.pullingData)
			{
				return;
			}
			this.UpdateIsBinding();
			int count = this.Bindings.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.Bindings[i].PushData())
				{
					success = false;
				}
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001B9F RID: 7071
		internal abstract object DataSource { get; }

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001BA0 RID: 7072
		internal abstract bool IsBinding { get; }

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001BA1 RID: 7073 RVA: 0x00035CCD File Offset: 0x00034CCD
		public bool IsBindingSuspended
		{
			get
			{
				return !this.IsBinding;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06001BA2 RID: 7074
		public abstract int Count { get; }

		// Token: 0x06001BA3 RID: 7075 RVA: 0x00035CD8 File Offset: 0x00034CD8
		private void OnBindingsCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			Binding binding = e.Element as Binding;
			switch (e.Action)
			{
			case CollectionChangeAction.Add:
				binding.BindingComplete += this.Binding_BindingComplete;
				return;
			case CollectionChangeAction.Remove:
				binding.BindingComplete -= this.Binding_BindingComplete;
				return;
			case CollectionChangeAction.Refresh:
				foreach (object obj in this.bindings)
				{
					Binding binding2 = (Binding)obj;
					binding2.BindingComplete += this.Binding_BindingComplete;
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x00035D90 File Offset: 0x00034D90
		private void OnBindingsCollectionChanging(object sender, CollectionChangeEventArgs e)
		{
			if (e.Action == CollectionChangeAction.Refresh)
			{
				foreach (object obj in this.bindings)
				{
					Binding binding = (Binding)obj;
					binding.BindingComplete -= this.Binding_BindingComplete;
				}
			}
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x00035E00 File Offset: 0x00034E00
		internal void Binding_BindingComplete(object sender, BindingCompleteEventArgs args)
		{
			this.OnBindingComplete(args);
		}

		// Token: 0x0400131F RID: 4895
		private BindingsCollection bindings;

		// Token: 0x04001320 RID: 4896
		private bool pullingData;

		// Token: 0x04001321 RID: 4897
		protected EventHandler onCurrentChangedHandler;

		// Token: 0x04001322 RID: 4898
		protected EventHandler onPositionChangedHandler;

		// Token: 0x04001323 RID: 4899
		private BindingCompleteEventHandler onBindingCompleteHandler;

		// Token: 0x04001324 RID: 4900
		internal EventHandler onCurrentItemChangedHandler;

		// Token: 0x04001325 RID: 4901
		internal BindingManagerDataErrorEventHandler onDataErrorHandler;
	}
}
