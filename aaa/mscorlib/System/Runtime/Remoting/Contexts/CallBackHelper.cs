using System;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006B0 RID: 1712
	[Serializable]
	internal class CallBackHelper
	{
		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06003E56 RID: 15958 RVA: 0x000D6B39 File Offset: 0x000D5B39
		// (set) Token: 0x06003E57 RID: 15959 RVA: 0x000D6B46 File Offset: 0x000D5B46
		internal bool IsEERequested
		{
			get
			{
				return (this._flags & 1) == 1;
			}
			set
			{
				if (value)
				{
					this._flags |= 1;
				}
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (set) Token: 0x06003E58 RID: 15960 RVA: 0x000D6B59 File Offset: 0x000D5B59
		internal bool IsCrossDomain
		{
			set
			{
				if (value)
				{
					this._flags |= 256;
				}
			}
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x000D6B70 File Offset: 0x000D5B70
		internal CallBackHelper(IntPtr privateData, bool bFromEE, int targetDomainID)
		{
			this.IsEERequested = bFromEE;
			this.IsCrossDomain = targetDomainID != 0;
			this._privateData = privateData;
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x000D6B93 File Offset: 0x000D5B93
		internal void Func()
		{
			if (this.IsEERequested)
			{
				Context.ExecuteCallBackInEE(this._privateData);
			}
		}

		// Token: 0x04001F92 RID: 8082
		internal const int RequestedFromEE = 1;

		// Token: 0x04001F93 RID: 8083
		internal const int XDomainTransition = 256;

		// Token: 0x04001F94 RID: 8084
		private int _flags;

		// Token: 0x04001F95 RID: 8085
		private IntPtr _privateData;
	}
}
