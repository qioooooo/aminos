using System;
using System.Collections;

namespace System.Web.Util
{
	// Token: 0x02000782 RID: 1922
	internal class SimpleRecyclingCache
	{
		// Token: 0x06005CC2 RID: 23746 RVA: 0x00173EAC File Offset: 0x00172EAC
		internal SimpleRecyclingCache()
		{
			this.CreateHashtable();
		}

		// Token: 0x06005CC3 RID: 23747 RVA: 0x00173EBA File Offset: 0x00172EBA
		private void CreateHashtable()
		{
			SimpleRecyclingCache._hashtable = new Hashtable(100, StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x170017CE RID: 6094
		internal object this[object key]
		{
			get
			{
				return SimpleRecyclingCache._hashtable[key];
			}
			set
			{
				lock (this)
				{
					if (SimpleRecyclingCache._hashtable.Count >= 100)
					{
						SimpleRecyclingCache._hashtable.Clear();
					}
					SimpleRecyclingCache._hashtable[key] = value;
				}
			}
		}

		// Token: 0x04003190 RID: 12688
		private const int MAX_SIZE = 100;

		// Token: 0x04003191 RID: 12689
		private static Hashtable _hashtable;
	}
}
