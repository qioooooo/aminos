using System;
using System.Collections;
using System.Threading;

namespace System.EnterpriseServices
{
	// Token: 0x020000E0 RID: 224
	internal sealed class RWHashTableEx
	{
		// Token: 0x06000521 RID: 1313 RVA: 0x00011E70 File Offset: 0x00010E70
		public RWHashTableEx()
		{
			this._hashtable = new Hashtable();
			this._rwlock = new ReaderWriterLock();
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00011E90 File Offset: 0x00010E90
		public object Get(object o, out bool bFound)
		{
			bFound = false;
			object obj2;
			try
			{
				this._rwlock.AcquireReaderLock(-1);
				object obj = this._hashtable[o];
				if (obj != null)
				{
					bFound = true;
					obj2 = ((RWHashTableEx.RWTableEntry)obj)._realObject;
				}
				else
				{
					obj2 = null;
				}
			}
			finally
			{
				this._rwlock.ReleaseReaderLock();
			}
			return obj2;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00011EF0 File Offset: 0x00010EF0
		public void Put(object key, object val)
		{
			RWHashTableEx.RWTableEntry rwtableEntry = new RWHashTableEx.RWTableEntry(val);
			try
			{
				this._rwlock.AcquireWriterLock(-1);
				this._hashtable[key] = rwtableEntry;
			}
			finally
			{
				this._rwlock.ReleaseWriterLock();
			}
		}

		// Token: 0x0400020C RID: 524
		private Hashtable _hashtable;

		// Token: 0x0400020D RID: 525
		private ReaderWriterLock _rwlock;

		// Token: 0x020000E1 RID: 225
		internal class RWTableEntry
		{
			// Token: 0x06000524 RID: 1316 RVA: 0x00011F3C File Offset: 0x00010F3C
			public RWTableEntry(object o)
			{
				this._realObject = o;
			}

			// Token: 0x0400020E RID: 526
			internal object _realObject;
		}
	}
}
