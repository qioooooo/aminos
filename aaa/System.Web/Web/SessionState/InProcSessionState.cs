using System;
using System.Web.Util;

namespace System.Web.SessionState
{
	// Token: 0x0200035E RID: 862
	internal sealed class InProcSessionState
	{
		// Token: 0x060029FE RID: 10750 RVA: 0x000BB4FF File Offset: 0x000BA4FF
		internal InProcSessionState(ISessionStateItemCollection sessionItems, HttpStaticObjectsCollection staticObjects, int timeout, bool locked, DateTime utcLockDate, int lockCookie, int flags)
		{
			this.Copy(sessionItems, staticObjects, timeout, locked, utcLockDate, lockCookie, flags);
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x000BB518 File Offset: 0x000BA518
		internal void Copy(ISessionStateItemCollection sessionItems, HttpStaticObjectsCollection staticObjects, int timeout, bool locked, DateTime utcLockDate, int lockCookie, int flags)
		{
			this._sessionItems = sessionItems;
			this._staticObjects = staticObjects;
			this._timeout = timeout;
			this._locked = locked;
			this._utcLockDate = utcLockDate;
			this._lockCookie = lockCookie;
			this._flags = flags;
		}

		// Token: 0x04001F28 RID: 7976
		internal ISessionStateItemCollection _sessionItems;

		// Token: 0x04001F29 RID: 7977
		internal HttpStaticObjectsCollection _staticObjects;

		// Token: 0x04001F2A RID: 7978
		internal int _timeout;

		// Token: 0x04001F2B RID: 7979
		internal bool _locked;

		// Token: 0x04001F2C RID: 7980
		internal DateTime _utcLockDate;

		// Token: 0x04001F2D RID: 7981
		internal int _lockCookie;

		// Token: 0x04001F2E RID: 7982
		internal ReadWriteSpinLock _spinLock;

		// Token: 0x04001F2F RID: 7983
		internal int _flags;
	}
}
