using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200054F RID: 1359
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DataListItemCollection : ICollection, IEnumerable
	{
		// Token: 0x0600431C RID: 17180 RVA: 0x001154EB File Offset: 0x001144EB
		public DataListItemCollection(ArrayList items)
		{
			this.items = items;
		}

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x0600431D RID: 17181 RVA: 0x001154FA File Offset: 0x001144FA
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x0600431E RID: 17182 RVA: 0x00115507 File Offset: 0x00114507
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x0600431F RID: 17183 RVA: 0x0011550A File Offset: 0x0011450A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x06004320 RID: 17184 RVA: 0x0011550D File Offset: 0x0011450D
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001057 RID: 4183
		public DataListItem this[int index]
		{
			get
			{
				return (DataListItem)this.items[index];
			}
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x00115524 File Offset: 0x00114524
		public void CopyTo(Array array, int index)
		{
			foreach (object obj in this)
			{
				array.SetValue(obj, index++);
			}
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x00115554 File Offset: 0x00114554
		public IEnumerator GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x04002947 RID: 10567
		private ArrayList items;
	}
}
