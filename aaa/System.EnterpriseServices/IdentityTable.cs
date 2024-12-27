using System;
using System.Collections;

namespace System.EnterpriseServices
{
	// Token: 0x0200003E RID: 62
	internal static class IdentityTable
	{
		// Token: 0x0600012C RID: 300 RVA: 0x0000555C File Offset: 0x0000455C
		public static void RemoveObject(IntPtr key, object val)
		{
			lock (IdentityTable._table)
			{
				WeakReference weakReference = IdentityTable._table[key] as WeakReference;
				if (weakReference != null && (weakReference.Target == val || weakReference.Target == null))
				{
					IdentityTable._table.Remove(key);
					weakReference.Target = null;
				}
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000055D0 File Offset: 0x000045D0
		public static object FindObject(IntPtr key)
		{
			object obj = null;
			lock (IdentityTable._table)
			{
				WeakReference weakReference = IdentityTable._table[key] as WeakReference;
				if (weakReference != null)
				{
					obj = weakReference.Target;
				}
			}
			return obj;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005628 File Offset: 0x00004628
		public static void AddObject(IntPtr key, object val)
		{
			lock (IdentityTable._table)
			{
				WeakReference weakReference = IdentityTable._table[key] as WeakReference;
				if (weakReference == null)
				{
					weakReference = new WeakReference(val, false);
					IdentityTable._table.Add(key, weakReference);
				}
				else if (weakReference.Target == null)
				{
					weakReference.Target = val;
				}
			}
		}

		// Token: 0x04000089 RID: 137
		private static Hashtable _table = new Hashtable();
	}
}
