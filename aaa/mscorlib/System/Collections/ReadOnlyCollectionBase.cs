using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x02000263 RID: 611
	[ComVisible(true)]
	[Serializable]
	public abstract class ReadOnlyCollectionBase : ICollection, IEnumerable
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001877 RID: 6263 RVA: 0x00040214 File Offset: 0x0003F214
		protected ArrayList InnerList
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

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001878 RID: 6264 RVA: 0x0004022F File Offset: 0x0003F22F
		public virtual int Count
		{
			get
			{
				return this.InnerList.Count;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001879 RID: 6265 RVA: 0x0004023C File Offset: 0x0003F23C
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InnerList.IsSynchronized;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x0600187A RID: 6266 RVA: 0x00040249 File Offset: 0x0003F249
		object ICollection.SyncRoot
		{
			get
			{
				return this.InnerList.SyncRoot;
			}
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00040256 File Offset: 0x0003F256
		void ICollection.CopyTo(Array array, int index)
		{
			this.InnerList.CopyTo(array, index);
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x00040265 File Offset: 0x0003F265
		public virtual IEnumerator GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		// Token: 0x0400098C RID: 2444
		private ArrayList list;
	}
}
