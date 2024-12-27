using System;
using System.Collections;
using System.Threading;

namespace System.EnterpriseServices
{
	// Token: 0x020000DF RID: 223
	internal sealed class RWHashTable
	{
		// Token: 0x0600051E RID: 1310 RVA: 0x00011DC4 File Offset: 0x00010DC4
		public RWHashTable()
		{
			this._hashtable = new Hashtable();
			this._rwlock = new ReaderWriterLock();
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00011DE4 File Offset: 0x00010DE4
		public object Get(object o)
		{
			object obj;
			try
			{
				this._rwlock.AcquireReaderLock(-1);
				obj = this._hashtable[o];
			}
			finally
			{
				this._rwlock.ReleaseReaderLock();
			}
			return obj;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00011E2C File Offset: 0x00010E2C
		public void Put(object key, object val)
		{
			try
			{
				this._rwlock.AcquireWriterLock(-1);
				this._hashtable[key] = val;
			}
			finally
			{
				this._rwlock.ReleaseWriterLock();
			}
		}

		// Token: 0x0400020A RID: 522
		private Hashtable _hashtable;

		// Token: 0x0400020B RID: 523
		private ReaderWriterLock _rwlock;
	}
}
