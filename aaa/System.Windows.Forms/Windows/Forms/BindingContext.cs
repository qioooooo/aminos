using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x0200023E RID: 574
	[DefaultEvent("CollectionChanged")]
	public class BindingContext : ICollection, IEnumerable
	{
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06001B5C RID: 7004 RVA: 0x000352A8 File Offset: 0x000342A8
		int ICollection.Count
		{
			get
			{
				this.ScrubWeakRefs();
				return this.listManagers.Count;
			}
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x000352BB File Offset: 0x000342BB
		void ICollection.CopyTo(Array ar, int index)
		{
			IntSecurity.UnmanagedCode.Demand();
			this.ScrubWeakRefs();
			this.listManagers.CopyTo(ar, index);
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x000352DA File Offset: 0x000342DA
		IEnumerator IEnumerable.GetEnumerator()
		{
			IntSecurity.UnmanagedCode.Demand();
			this.ScrubWeakRefs();
			return this.listManagers.GetEnumerator();
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06001B5F RID: 7007 RVA: 0x000352F7 File Offset: 0x000342F7
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06001B60 RID: 7008 RVA: 0x000352FA File Offset: 0x000342FA
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001B61 RID: 7009 RVA: 0x000352FD File Offset: 0x000342FD
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x00035300 File Offset: 0x00034300
		public BindingContext()
		{
			this.listManagers = new Hashtable();
		}

		// Token: 0x17000361 RID: 865
		public BindingManagerBase this[object dataSource]
		{
			get
			{
				return this[dataSource, ""];
			}
		}

		// Token: 0x17000362 RID: 866
		public BindingManagerBase this[object dataSource, string dataMember]
		{
			get
			{
				return this.EnsureListManager(dataSource, dataMember);
			}
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x0003532B File Offset: 0x0003432B
		protected internal void Add(object dataSource, BindingManagerBase listManager)
		{
			this.AddCore(dataSource, listManager);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataSource));
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x00035342 File Offset: 0x00034342
		protected virtual void AddCore(object dataSource, BindingManagerBase listManager)
		{
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			if (listManager == null)
			{
				throw new ArgumentNullException("listManager");
			}
			this.listManagers[this.GetKey(dataSource, "")] = new WeakReference(listManager, false);
		}

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06001B67 RID: 7015 RVA: 0x0003537E File Offset: 0x0003437E
		// (remove) Token: 0x06001B68 RID: 7016 RVA: 0x00035385 File Offset: 0x00034385
		[SRDescription("collectionChangedEventDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public event CollectionChangeEventHandler CollectionChanged
		{
			add
			{
				throw new NotImplementedException();
			}
			remove
			{
			}
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x00035387 File Offset: 0x00034387
		protected internal void Clear()
		{
			this.ClearCore();
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x0003539C File Offset: 0x0003439C
		protected virtual void ClearCore()
		{
			this.listManagers.Clear();
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x000353A9 File Offset: 0x000343A9
		public bool Contains(object dataSource)
		{
			return this.Contains(dataSource, "");
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x000353B7 File Offset: 0x000343B7
		public bool Contains(object dataSource, string dataMember)
		{
			return this.listManagers.ContainsKey(this.GetKey(dataSource, dataMember));
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x000353CC File Offset: 0x000343CC
		internal BindingContext.HashKey GetKey(object dataSource, string dataMember)
		{
			return new BindingContext.HashKey(dataSource, dataMember);
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x000353D5 File Offset: 0x000343D5
		protected virtual void OnCollectionChanged(CollectionChangeEventArgs ccevent)
		{
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x000353D7 File Offset: 0x000343D7
		protected internal void Remove(object dataSource)
		{
			this.RemoveCore(dataSource);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, dataSource));
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x000353ED File Offset: 0x000343ED
		protected virtual void RemoveCore(object dataSource)
		{
			this.listManagers.Remove(this.GetKey(dataSource, ""));
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x00035408 File Offset: 0x00034408
		internal BindingManagerBase EnsureListManager(object dataSource, string dataMember)
		{
			BindingManagerBase bindingManagerBase = null;
			if (dataMember == null)
			{
				dataMember = "";
			}
			if (dataSource is ICurrencyManagerProvider)
			{
				bindingManagerBase = (dataSource as ICurrencyManagerProvider).GetRelatedCurrencyManager(dataMember);
				if (bindingManagerBase != null)
				{
					return bindingManagerBase;
				}
			}
			BindingContext.HashKey key = this.GetKey(dataSource, dataMember);
			WeakReference weakReference = this.listManagers[key] as WeakReference;
			if (weakReference != null)
			{
				bindingManagerBase = (BindingManagerBase)weakReference.Target;
			}
			if (bindingManagerBase != null)
			{
				return bindingManagerBase;
			}
			if (dataMember.Length == 0)
			{
				if (dataSource is IList || dataSource is IListSource)
				{
					bindingManagerBase = new CurrencyManager(dataSource);
				}
				else
				{
					bindingManagerBase = new PropertyManager(dataSource);
				}
			}
			else
			{
				int num = dataMember.LastIndexOf(".");
				string text = ((num == -1) ? "" : dataMember.Substring(0, num));
				string text2 = dataMember.Substring(num + 1);
				BindingManagerBase bindingManagerBase2 = this.EnsureListManager(dataSource, text);
				PropertyDescriptor propertyDescriptor = bindingManagerBase2.GetItemProperties().Find(text2, true);
				if (propertyDescriptor == null)
				{
					throw new ArgumentException(SR.GetString("RelatedListManagerChild", new object[] { text2 }));
				}
				if (typeof(IList).IsAssignableFrom(propertyDescriptor.PropertyType))
				{
					bindingManagerBase = new RelatedCurrencyManager(bindingManagerBase2, text2);
				}
				else
				{
					bindingManagerBase = new RelatedPropertyManager(bindingManagerBase2, text2);
				}
			}
			if (weakReference == null)
			{
				this.listManagers.Add(key, new WeakReference(bindingManagerBase, false));
			}
			else
			{
				weakReference.Target = bindingManagerBase;
			}
			return bindingManagerBase;
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x00035554 File Offset: 0x00034554
		private static void CheckPropertyBindingCycles(BindingContext newBindingContext, Binding propBinding)
		{
			if (newBindingContext == null || propBinding == null)
			{
				return;
			}
			if (newBindingContext.Contains(propBinding.BindableComponent, ""))
			{
				BindingManagerBase bindingManagerBase = newBindingContext.EnsureListManager(propBinding.BindableComponent, "");
				for (int i = 0; i < bindingManagerBase.Bindings.Count; i++)
				{
					Binding binding = bindingManagerBase.Bindings[i];
					if (binding.DataSource == propBinding.BindableComponent)
					{
						if (propBinding.BindToObject.BindingMemberInfo.BindingMember.Equals(binding.PropertyName))
						{
							throw new ArgumentException(SR.GetString("DataBindingCycle", new object[] { binding.PropertyName }), "propBinding");
						}
					}
					else if (propBinding.BindToObject.BindingManagerBase is PropertyManager)
					{
						BindingContext.CheckPropertyBindingCycles(newBindingContext, binding);
					}
				}
			}
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x0003562C File Offset: 0x0003462C
		private void ScrubWeakRefs()
		{
			object[] array = new object[this.listManagers.Count];
			this.listManagers.CopyTo(array, 0);
			foreach (DictionaryEntry dictionaryEntry in array)
			{
				WeakReference weakReference = (WeakReference)dictionaryEntry.Value;
				if (weakReference.Target == null)
				{
					this.listManagers.Remove(dictionaryEntry.Key);
				}
			}
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x00035698 File Offset: 0x00034698
		public static void UpdateBinding(BindingContext newBindingContext, Binding binding)
		{
			BindingManagerBase bindingManagerBase = binding.BindingManagerBase;
			if (bindingManagerBase != null)
			{
				bindingManagerBase.Bindings.Remove(binding);
			}
			if (newBindingContext != null)
			{
				if (binding.BindToObject.BindingManagerBase is PropertyManager)
				{
					BindingContext.CheckPropertyBindingCycles(newBindingContext, binding);
				}
				BindToObject bindToObject = binding.BindToObject;
				BindingManagerBase bindingManagerBase2 = newBindingContext.EnsureListManager(bindToObject.DataSource, bindToObject.BindingMemberInfo.BindingPath);
				bindingManagerBase2.Bindings.Add(binding);
			}
		}

		// Token: 0x0400131B RID: 4891
		private Hashtable listManagers;

		// Token: 0x0200023F RID: 575
		internal class HashKey
		{
			// Token: 0x06001B75 RID: 7029 RVA: 0x00035708 File Offset: 0x00034708
			internal HashKey(object dataSource, string dataMember)
			{
				if (dataSource == null)
				{
					throw new ArgumentNullException("dataSource");
				}
				if (dataMember == null)
				{
					dataMember = "";
				}
				this.wRef = new WeakReference(dataSource, false);
				this.dataSourceHashCode = dataSource.GetHashCode();
				this.dataMember = dataMember.ToLower(CultureInfo.InvariantCulture);
			}

			// Token: 0x06001B76 RID: 7030 RVA: 0x0003575D File Offset: 0x0003475D
			public override int GetHashCode()
			{
				return this.dataSourceHashCode * this.dataMember.GetHashCode();
			}

			// Token: 0x06001B77 RID: 7031 RVA: 0x00035774 File Offset: 0x00034774
			public override bool Equals(object target)
			{
				if (target is BindingContext.HashKey)
				{
					BindingContext.HashKey hashKey = (BindingContext.HashKey)target;
					return this.wRef.Target == hashKey.wRef.Target && this.dataMember == hashKey.dataMember;
				}
				return false;
			}

			// Token: 0x0400131C RID: 4892
			private WeakReference wRef;

			// Token: 0x0400131D RID: 4893
			private int dataSourceHashCode;

			// Token: 0x0400131E RID: 4894
			private string dataMember;
		}
	}
}
