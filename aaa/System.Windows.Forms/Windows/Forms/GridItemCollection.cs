using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x02000419 RID: 1049
	public class GridItemCollection : ICollection, IEnumerable
	{
		// Token: 0x06003E6B RID: 15979 RVA: 0x000E3337 File Offset: 0x000E2337
		internal GridItemCollection(GridItem[] entries)
		{
			if (entries == null)
			{
				this.entries = new GridItem[0];
				return;
			}
			this.entries = entries;
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06003E6C RID: 15980 RVA: 0x000E3356 File Offset: 0x000E2356
		public int Count
		{
			get
			{
				return this.entries.Length;
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06003E6D RID: 15981 RVA: 0x000E3360 File Offset: 0x000E2360
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06003E6E RID: 15982 RVA: 0x000E3363 File Offset: 0x000E2363
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BFB RID: 3067
		public GridItem this[int index]
		{
			get
			{
				return this.entries[index];
			}
		}

		// Token: 0x17000BFC RID: 3068
		public GridItem this[string label]
		{
			get
			{
				foreach (GridItem gridItem in this.entries)
				{
					if (gridItem.Label == label)
					{
						return gridItem;
					}
				}
				return null;
			}
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x000E33AB File Offset: 0x000E23AB
		void ICollection.CopyTo(Array dest, int index)
		{
			if (this.entries.Length > 0)
			{
				Array.Copy(this.entries, 0, dest, index, this.entries.Length);
			}
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x000E33CE File Offset: 0x000E23CE
		public IEnumerator GetEnumerator()
		{
			return this.entries.GetEnumerator();
		}

		// Token: 0x04001EC3 RID: 7875
		public static GridItemCollection Empty = new GridItemCollection(new GridItem[0]);

		// Token: 0x04001EC4 RID: 7876
		internal GridItem[] entries;
	}
}
