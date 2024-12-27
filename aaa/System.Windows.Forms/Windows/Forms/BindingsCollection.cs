using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200024E RID: 590
	[DefaultEvent("CollectionChanged")]
	public class BindingsCollection : BaseCollection
	{
		// Token: 0x06001E7E RID: 7806 RVA: 0x0003F61B File Offset: 0x0003E61B
		internal BindingsCollection()
		{
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001E7F RID: 7807 RVA: 0x0003F623 File Offset: 0x0003E623
		public override int Count
		{
			get
			{
				if (this.list == null)
				{
					return 0;
				}
				return base.Count;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001E80 RID: 7808 RVA: 0x0003F635 File Offset: 0x0003E635
		protected override ArrayList List
		{
			get
			{
				if (this.list == null)
				{
					this.list = new ArrayList();
				}
				return this.list;
			}
		}

		// Token: 0x1700043A RID: 1082
		public Binding this[int index]
		{
			get
			{
				return (Binding)this.List[index];
			}
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x0003F664 File Offset: 0x0003E664
		protected internal void Add(Binding binding)
		{
			CollectionChangeEventArgs collectionChangeEventArgs = new CollectionChangeEventArgs(CollectionChangeAction.Add, binding);
			this.OnCollectionChanging(collectionChangeEventArgs);
			this.AddCore(binding);
			this.OnCollectionChanged(collectionChangeEventArgs);
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x0003F68E File Offset: 0x0003E68E
		protected virtual void AddCore(Binding dataBinding)
		{
			if (dataBinding == null)
			{
				throw new ArgumentNullException("dataBinding");
			}
			this.List.Add(dataBinding);
		}

		// Token: 0x140000AC RID: 172
		// (add) Token: 0x06001E84 RID: 7812 RVA: 0x0003F6AB File Offset: 0x0003E6AB
		// (remove) Token: 0x06001E85 RID: 7813 RVA: 0x0003F6C4 File Offset: 0x0003E6C4
		[SRDescription("collectionChangingEventDescr")]
		public event CollectionChangeEventHandler CollectionChanging
		{
			add
			{
				this.onCollectionChanging = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChanging, value);
			}
			remove
			{
				this.onCollectionChanging = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChanging, value);
			}
		}

		// Token: 0x140000AD RID: 173
		// (add) Token: 0x06001E86 RID: 7814 RVA: 0x0003F6DD File Offset: 0x0003E6DD
		// (remove) Token: 0x06001E87 RID: 7815 RVA: 0x0003F6F6 File Offset: 0x0003E6F6
		[SRDescription("collectionChangedEventDescr")]
		public event CollectionChangeEventHandler CollectionChanged
		{
			add
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChanged, value);
			}
			remove
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChanged, value);
			}
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x0003F710 File Offset: 0x0003E710
		protected internal void Clear()
		{
			CollectionChangeEventArgs collectionChangeEventArgs = new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null);
			this.OnCollectionChanging(collectionChangeEventArgs);
			this.ClearCore();
			this.OnCollectionChanged(collectionChangeEventArgs);
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x0003F739 File Offset: 0x0003E739
		protected virtual void ClearCore()
		{
			this.List.Clear();
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x0003F746 File Offset: 0x0003E746
		protected virtual void OnCollectionChanging(CollectionChangeEventArgs e)
		{
			if (this.onCollectionChanging != null)
			{
				this.onCollectionChanging(this, e);
			}
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x0003F75D File Offset: 0x0003E75D
		protected virtual void OnCollectionChanged(CollectionChangeEventArgs ccevent)
		{
			if (this.onCollectionChanged != null)
			{
				this.onCollectionChanged(this, ccevent);
			}
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x0003F774 File Offset: 0x0003E774
		protected internal void Remove(Binding binding)
		{
			CollectionChangeEventArgs collectionChangeEventArgs = new CollectionChangeEventArgs(CollectionChangeAction.Remove, binding);
			this.OnCollectionChanging(collectionChangeEventArgs);
			this.RemoveCore(binding);
			this.OnCollectionChanged(collectionChangeEventArgs);
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x0003F79E File Offset: 0x0003E79E
		protected internal void RemoveAt(int index)
		{
			this.Remove(this[index]);
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x0003F7AD File Offset: 0x0003E7AD
		protected virtual void RemoveCore(Binding dataBinding)
		{
			this.List.Remove(dataBinding);
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x0003F7BB File Offset: 0x0003E7BB
		protected internal bool ShouldSerializeMyAll()
		{
			return this.Count > 0;
		}

		// Token: 0x040013E9 RID: 5097
		private ArrayList list;

		// Token: 0x040013EA RID: 5098
		private CollectionChangeEventHandler onCollectionChanging;

		// Token: 0x040013EB RID: 5099
		private CollectionChangeEventHandler onCollectionChanged;
	}
}
