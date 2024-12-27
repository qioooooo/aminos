using System;
using System.Web.Util;

namespace System.Web.SessionState
{
	// Token: 0x0200037F RID: 895
	internal sealed class CachedContent
	{
		// Token: 0x06002B5B RID: 11099 RVA: 0x000C1096 File Offset: 0x000C0096
		internal CachedContent(byte[] content, IntPtr stateItem, bool locked, DateTime utcLockDate, int lockCookie, int extraFlags)
		{
			this._content = content;
			this._stateItem = stateItem;
			this._locked = locked;
			this._utcLockDate = utcLockDate;
			this._lockCookie = lockCookie;
			this._extraFlags = extraFlags;
		}

		// Token: 0x04002008 RID: 8200
		internal byte[] _content;

		// Token: 0x04002009 RID: 8201
		internal IntPtr _stateItem;

		// Token: 0x0400200A RID: 8202
		internal bool _locked;

		// Token: 0x0400200B RID: 8203
		internal DateTime _utcLockDate;

		// Token: 0x0400200C RID: 8204
		internal int _lockCookie;

		// Token: 0x0400200D RID: 8205
		internal int _extraFlags;

		// Token: 0x0400200E RID: 8206
		internal ReadWriteSpinLock _spinLock;
	}
}
