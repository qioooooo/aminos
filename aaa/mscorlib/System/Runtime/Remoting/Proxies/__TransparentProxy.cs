using System;

namespace System.Runtime.Remoting.Proxies
{
	// Token: 0x02000789 RID: 1929
	internal sealed class __TransparentProxy
	{
		// Token: 0x06004534 RID: 17716 RVA: 0x000EC4A8 File Offset: 0x000EB4A8
		private __TransparentProxy()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Constructor"));
		}

		// Token: 0x04002240 RID: 8768
		private RealProxy _rp;

		// Token: 0x04002241 RID: 8769
		private object _stubData;

		// Token: 0x04002242 RID: 8770
		private IntPtr _pMT;

		// Token: 0x04002243 RID: 8771
		private IntPtr _pInterfaceMT;

		// Token: 0x04002244 RID: 8772
		private IntPtr _stub;
	}
}
