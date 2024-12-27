using System;
using System.Data.Common;

namespace System.Data.SqlClient
{
	// Token: 0x02000313 RID: 787
	internal sealed class InOutOfProcHelper
	{
		// Token: 0x06002919 RID: 10521 RVA: 0x0029256C File Offset: 0x0029196C
		private InOutOfProcHelper()
		{
			IntPtr moduleHandle = SafeNativeMethods.GetModuleHandle(null);
			if (IntPtr.Zero != moduleHandle)
			{
				if (IntPtr.Zero != SafeNativeMethods.GetProcAddress(moduleHandle, "_______SQL______Process______Available@0"))
				{
					this._inProc = true;
					return;
				}
				if (IntPtr.Zero != SafeNativeMethods.GetProcAddress(moduleHandle, "______SQL______Process______Available"))
				{
					this._inProc = true;
				}
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x0600291A RID: 10522 RVA: 0x002925D0 File Offset: 0x002919D0
		internal static bool InProc
		{
			get
			{
				return InOutOfProcHelper.SingletonInstance._inProc;
			}
		}

		// Token: 0x040019A7 RID: 6567
		private static readonly InOutOfProcHelper SingletonInstance = new InOutOfProcHelper();

		// Token: 0x040019A8 RID: 6568
		private bool _inProc;
	}
}
