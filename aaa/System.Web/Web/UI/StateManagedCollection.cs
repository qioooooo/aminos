using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000468 RID: 1128
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class StateManagedCollection : IList, ICollection, IEnumerable, IStateManager
	{
		// Token: 0x0600355D RID: 13661 RVA: 0x000E6978 File Offset: 0x000E5978
		protected StateManagedCollection()
		{
			this._collectionItems = new ArrayList();
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x0600355E RID: 13662 RVA: 0x000E698B File Offset: 0x000E598B
		public int Count
		{
			get
			{
				return this._collectionItems.Count;
			}
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x000E6998 File Offset: 0x000E5998
		public void Clear()
		{
			this.OnClear();
			this._collectionItems.Clear();
			this.OnClearComplete();
			if (this._tracking)
			{
				this._saveAll = true;
			}
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x000E69C0 File Offset: 0x000E59C0
		public void CopyTo(Array array, int index)
		{
			this._collectionItems.CopyTo(array, index);
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x000E69CF File Offset: 0x000E59CF
		protected virtual object CreateKnownType(int index)
		{
			throw new InvalidOperationException(SR.GetString("StateManagedCollection_NoKnownTypes"));
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x000E69E0 File Offset: 0x000E59E0
		public IEnumerator GetEnumerator()
		{
			return this._collectionItems.GetEnumerator();
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x000E69ED File Offset: 0x000E59ED
		protected virtual Type[] GetKnownTypes()
		{
			return null;
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x000E69F0 File Offset: 0x000E59F0
		private int GetKnownTypeCount()
		{
			Type[] knownTypes = this.GetKnownTypes();
			if (knownTypes == null)
			{
				return 0;
			}
			return knownTypes.Length;
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x000E6A0C File Offset: 0x000E5A0C
		private void InsertInternal(int index, object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			if (((IStateManager)this).IsTrackingViewState)
			{
				((IStateManager)o).TrackViewState();
				this.SetDirtyObject(o);
			}
			this.OnInsert(index, o);
			int num;
			if (index == -1)
			{
				num = this._collectionItems.Add(o);
			}
			else
			{
				num = index;
				this._collectionItems.Insert(index, o);
			}
			try
			{
				this.OnInsertComplete(index, o);
			}
			catch
			{
				this._collectionItems.RemoveAt(num);
				throw;
			}
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x000E6A94 File Offset: 0x000E5A94
		private void LoadAllItemsFromViewState(object savedState)
		{
			Pair pair = (Pair)savedState;
			if (pair.Second is Pair)
			{
				Pair pair2 = (Pair)pair.Second;
				object[] array = (object[])pair.First;
				int[] array2 = (int[])pair2.First;
				ArrayList arrayList = (ArrayList)pair2.Second;
				this.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					object obj;
					if (array2 == null)
					{
						obj = this.CreateKnownType(0);
					}
					else
					{
						int num = array2[i];
						if (num < this.GetKnownTypeCount())
						{
							obj = this.CreateKnownType(num);
						}
						else
						{
							string text = (string)arrayList[num - this.GetKnownTypeCount()];
							Type type = Type.GetType(text);
							obj = Activator.CreateInstance(type);
						}
					}
					((IStateManager)obj).TrackViewState();
					((IStateManager)obj).LoadViewState(array[i]);
					((IList)this).Add(obj);
				}
				return;
			}
			object[] array3 = (object[])pair.First;
			int[] array4 = (int[])pair.Second;
			this.Clear();
			for (int j = 0; j < array3.Length; j++)
			{
				int num2 = 0;
				if (array4 != null)
				{
					num2 = array4[j];
				}
				object obj2 = this.CreateKnownType(num2);
				((IStateManager)obj2).TrackViewState();
				((IStateManager)obj2).LoadViewState(array3[j]);
				((IList)this).Add(obj2);
			}
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x000E6BF0 File Offset: 0x000E5BF0
		private void LoadChangedItemsFromViewState(object savedState)
		{
			Triplet triplet = (Triplet)savedState;
			if (triplet.Third is Pair)
			{
				Pair pair = (Pair)triplet.Third;
				ArrayList arrayList = (ArrayList)triplet.First;
				ArrayList arrayList2 = (ArrayList)triplet.Second;
				ArrayList arrayList3 = (ArrayList)pair.First;
				ArrayList arrayList4 = (ArrayList)pair.Second;
				for (int i = 0; i < arrayList.Count; i++)
				{
					int num = (int)arrayList[i];
					if (num < this.Count)
					{
						((IStateManager)((IList)this)[num]).LoadViewState(arrayList2[i]);
					}
					else
					{
						object obj;
						if (arrayList3 == null)
						{
							obj = this.CreateKnownType(0);
						}
						else
						{
							int num2 = (int)arrayList3[i];
							if (num2 < this.GetKnownTypeCount())
							{
								obj = this.CreateKnownType(num2);
							}
							else
							{
								string text = (string)arrayList4[num2 - this.GetKnownTypeCount()];
								Type type = Type.GetType(text);
								obj = Activator.CreateInstance(type);
							}
						}
						((IStateManager)obj).TrackViewState();
						((IStateManager)obj).LoadViewState(arrayList2[i]);
						((IList)this).Add(obj);
					}
				}
				return;
			}
			ArrayList arrayList5 = (ArrayList)triplet.First;
			ArrayList arrayList6 = (ArrayList)triplet.Second;
			ArrayList arrayList7 = (ArrayList)triplet.Third;
			for (int j = 0; j < arrayList5.Count; j++)
			{
				int num3 = (int)arrayList5[j];
				if (num3 < this.Count)
				{
					((IStateManager)((IList)this)[num3]).LoadViewState(arrayList6[j]);
				}
				else
				{
					int num4 = 0;
					if (arrayList7 != null)
					{
						num4 = (int)arrayList7[j];
					}
					object obj2 = this.CreateKnownType(num4);
					((IStateManager)obj2).TrackViewState();
					((IStateManager)obj2).LoadViewState(arrayList6[j]);
					((IList)this).Add(obj2);
				}
			}
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x000E6DF2 File Offset: 0x000E5DF2
		protected virtual void OnClear()
		{
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x000E6DF4 File Offset: 0x000E5DF4
		protected virtual void OnClearComplete()
		{
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x000E6DF6 File Offset: 0x000E5DF6
		protected virtual void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x000E6E06 File Offset: 0x000E5E06
		protected virtual void OnInsert(int index, object value)
		{
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x000E6E08 File Offset: 0x000E5E08
		protected virtual void OnInsertComplete(int index, object value)
		{
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x000E6E0A File Offset: 0x000E5E0A
		protected virtual void OnRemove(int index, object value)
		{
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x000E6E0C File Offset: 0x000E5E0C
		protected virtual void OnRemoveComplete(int index, object value)
		{
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x000E6E10 File Offset: 0x000E5E10
		private object SaveAllItemsToViewState()
		{
			bool flag = false;
			int count = this._collectionItems.Count;
			int[] array = new int[count];
			object[] array2 = new object[count];
			ArrayList arrayList = null;
			IDictionary dictionary = null;
			int knownTypeCount = this.GetKnownTypeCount();
			for (int i = 0; i < count; i++)
			{
				object obj = this._collectionItems[i];
				this.SetDirtyObject(obj);
				array2[i] = ((IStateManager)obj).SaveViewState();
				if (array2[i] != null)
				{
					flag = true;
				}
				Type type = obj.GetType();
				int num = -1;
				if (knownTypeCount != 0)
				{
					num = ((IList)this.GetKnownTypes()).IndexOf(type);
				}
				if (num != -1)
				{
					array[i] = num;
				}
				else
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
						dictionary = new HybridDictionary();
					}
					object obj2 = dictionary[type];
					if (obj2 == null)
					{
						arrayList.Add(type.AssemblyQualifiedName);
						obj2 = arrayList.Count + knownTypeCount - 1;
						dictionary[type] = obj2;
					}
					array[i] = (int)obj2;
				}
			}
			if (!flag)
			{
				return null;
			}
			if (arrayList == null)
			{
				if (knownTypeCount == 1)
				{
					array = null;
				}
				return new Pair(array2, array);
			}
			return new Pair(array2, new Pair(array, arrayList));
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x000E6F38 File Offset: 0x000E5F38
		private object SaveChangedItemsToViewState()
		{
			bool flag = false;
			int count = this._collectionItems.Count;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			ArrayList arrayList4 = null;
			IDictionary dictionary = null;
			int knownTypeCount = this.GetKnownTypeCount();
			for (int i = 0; i < count; i++)
			{
				object obj = this._collectionItems[i];
				object obj2 = ((IStateManager)obj).SaveViewState();
				if (obj2 != null)
				{
					flag = true;
					arrayList.Add(i);
					arrayList2.Add(obj2);
					Type type = obj.GetType();
					int num = -1;
					if (knownTypeCount != 0)
					{
						num = ((IList)this.GetKnownTypes()).IndexOf(type);
					}
					if (num != -1)
					{
						arrayList3.Add(num);
					}
					else
					{
						if (arrayList4 == null)
						{
							arrayList4 = new ArrayList();
							dictionary = new HybridDictionary();
						}
						object obj3 = dictionary[type];
						if (obj3 == null)
						{
							arrayList4.Add(type.AssemblyQualifiedName);
							obj3 = arrayList4.Count + knownTypeCount - 1;
							dictionary[type] = obj3;
						}
						arrayList3.Add(obj3);
					}
				}
			}
			if (!flag)
			{
				return null;
			}
			if (arrayList4 == null)
			{
				if (knownTypeCount == 1)
				{
					arrayList3 = null;
				}
				return new Triplet(arrayList, arrayList2, arrayList3);
			}
			return new Triplet(arrayList, arrayList2, new Pair(arrayList3, arrayList4));
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000E707F File Offset: 0x000E607F
		public void SetDirty()
		{
			this._saveAll = true;
		}

		// Token: 0x06003572 RID: 13682
		protected abstract void SetDirtyObject(object o);

		// Token: 0x06003573 RID: 13683 RVA: 0x000E7088 File Offset: 0x000E6088
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06003574 RID: 13684 RVA: 0x000E7090 File Offset: 0x000E6090
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06003575 RID: 13685 RVA: 0x000E7098 File Offset: 0x000E6098
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06003576 RID: 13686 RVA: 0x000E709B File Offset: 0x000E609B
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06003577 RID: 13687 RVA: 0x000E709E File Offset: 0x000E609E
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06003578 RID: 13688 RVA: 0x000E70A1 File Offset: 0x000E60A1
		bool IList.IsReadOnly
		{
			get
			{
				return this._collectionItems.IsReadOnly;
			}
		}

		// Token: 0x17000BFA RID: 3066
		object IList.this[int index]
		{
			get
			{
				return this._collectionItems[index];
			}
			set
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("StateManagedCollection_InvalidIndex"));
				}
				((IList)this).RemoveAt(index);
				((IList)this).Insert(index, value);
			}
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x000E70EF File Offset: 0x000E60EF
		int IList.Add(object value)
		{
			this.OnValidate(value);
			this.InsertInternal(-1, value);
			return this._collectionItems.Count - 1;
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x000E710D File Offset: 0x000E610D
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x000E7115 File Offset: 0x000E6115
		bool IList.Contains(object value)
		{
			if (value == null)
			{
				return false;
			}
			this.OnValidate(value);
			return this._collectionItems.Contains(value);
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x000E712F File Offset: 0x000E612F
		int IList.IndexOf(object value)
		{
			if (value == null)
			{
				return -1;
			}
			this.OnValidate(value);
			return this._collectionItems.IndexOf(value);
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x000E714C File Offset: 0x000E614C
		void IList.Insert(int index, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (index < 0 || index > this.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("StateManagedCollection_InvalidIndex"));
			}
			this.OnValidate(value);
			this.InsertInternal(index, value);
			if (this._tracking)
			{
				this._saveAll = true;
			}
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x000E71A7 File Offset: 0x000E61A7
		void IList.Remove(object value)
		{
			if (value == null)
			{
				return;
			}
			this.OnValidate(value);
			((IList)this).RemoveAt(((IList)this).IndexOf(value));
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x000E71C4 File Offset: 0x000E61C4
		void IList.RemoveAt(int index)
		{
			object obj = this._collectionItems[index];
			this.OnRemove(index, obj);
			this._collectionItems.RemoveAt(index);
			try
			{
				this.OnRemoveComplete(index, obj);
			}
			catch
			{
				this._collectionItems.Insert(index, obj);
				throw;
			}
			if (this._tracking)
			{
				this._saveAll = true;
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06003582 RID: 13698 RVA: 0x000E722C File Offset: 0x000E622C
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return this._tracking;
			}
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x000E7234 File Offset: 0x000E6234
		void IStateManager.LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				if (savedState is Triplet)
				{
					this.LoadChangedItemsFromViewState(savedState);
					return;
				}
				this.LoadAllItemsFromViewState(savedState);
			}
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x000E7250 File Offset: 0x000E6250
		object IStateManager.SaveViewState()
		{
			if (this._saveAll)
			{
				return this.SaveAllItemsToViewState();
			}
			return this.SaveChangedItemsToViewState();
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x000E7268 File Offset: 0x000E6268
		void IStateManager.TrackViewState()
		{
			this._tracking = true;
			foreach (object obj in this._collectionItems)
			{
				IStateManager stateManager = (IStateManager)obj;
				stateManager.TrackViewState();
			}
		}

		// Token: 0x0400252F RID: 9519
		private ArrayList _collectionItems;

		// Token: 0x04002530 RID: 9520
		private bool _tracking;

		// Token: 0x04002531 RID: 9521
		private bool _saveAll;
	}
}
