using System;
using System.Runtime.Remoting.Messaging;

namespace System.EnterpriseServices.Thunk
{
	// Token: 0x02000053 RID: 83
	internal interface IProxyInvoke
	{
		// Token: 0x060000A4 RID: 164
		IMessage LocalInvoke(IMessage msg);

		// Token: 0x060000A5 RID: 165
		IntPtr GetOuterIUnknown();
	}
}
