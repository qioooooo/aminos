using System;
using System.Threading;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200002A RID: 42
	internal class UserToken
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x00003E41 File Offset: 0x00002E41
		internal UserToken(SendOrPostCallback callback, object userState)
		{
			this.callback = callback;
			this.userState = userState;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00003E57 File Offset: 0x00002E57
		internal SendOrPostCallback Callback
		{
			get
			{
				return this.callback;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00003E5F File Offset: 0x00002E5F
		internal object UserState
		{
			get
			{
				return this.userState;
			}
		}

		// Token: 0x0400025B RID: 603
		private SendOrPostCallback callback;

		// Token: 0x0400025C RID: 604
		private object userState;
	}
}
