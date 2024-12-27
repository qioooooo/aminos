using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004D2 RID: 1234
	internal class CallbackClosure
	{
		// Token: 0x06002666 RID: 9830 RVA: 0x0009C2A0 File Offset: 0x0009B2A0
		internal CallbackClosure(ExecutionContext context, AsyncCallback callback)
		{
			if (callback != null)
			{
				this.savedCallback = callback;
				this.savedContext = context;
			}
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x0009C2B9 File Offset: 0x0009B2B9
		internal bool IsCompatible(AsyncCallback callback)
		{
			return callback != null && this.savedCallback != null && object.Equals(this.savedCallback, callback);
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002668 RID: 9832 RVA: 0x0009C2D9 File Offset: 0x0009B2D9
		internal AsyncCallback AsyncCallback
		{
			get
			{
				return this.savedCallback;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002669 RID: 9833 RVA: 0x0009C2E1 File Offset: 0x0009B2E1
		internal ExecutionContext Context
		{
			get
			{
				return this.savedContext;
			}
		}

		// Token: 0x040025E9 RID: 9705
		private AsyncCallback savedCallback;

		// Token: 0x040025EA RID: 9706
		private ExecutionContext savedContext;
	}
}
