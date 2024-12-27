using System;
using System.Collections;
using System.Threading;

namespace System.EnterpriseServices
{
	// Token: 0x0200003F RID: 63
	internal class Cachetable
	{
		// Token: 0x0600012F RID: 303 RVA: 0x000056A0 File Offset: 0x000046A0
		public Cachetable()
		{
			this._cache = new Hashtable();
			this._rwlock = new ReaderWriterLock();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000056C0 File Offset: 0x000046C0
		public object Get(object key)
		{
			this._rwlock.AcquireReaderLock(-1);
			object obj;
			try
			{
				obj = this._cache[key];
			}
			finally
			{
				this._rwlock.ReleaseReaderLock();
			}
			return obj;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005708 File Offset: 0x00004708
		public object Set(object key, object nv)
		{
			this._rwlock.AcquireWriterLock(-1);
			object obj2;
			try
			{
				object obj = this._cache[key];
				if (obj == null)
				{
					this._cache[key] = nv;
					obj2 = nv;
				}
				else
				{
					obj2 = obj;
				}
			}
			finally
			{
				this._rwlock.ReleaseWriterLock();
			}
			return obj2;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00005764 File Offset: 0x00004764
		public void Reset(object key, object nv)
		{
			this._rwlock.AcquireWriterLock(-1);
			try
			{
				this._cache[key] = nv;
			}
			finally
			{
				this._rwlock.ReleaseWriterLock();
			}
		}

		// Token: 0x0400008A RID: 138
		private Hashtable _cache;

		// Token: 0x0400008B RID: 139
		private ReaderWriterLock _rwlock;
	}
}
