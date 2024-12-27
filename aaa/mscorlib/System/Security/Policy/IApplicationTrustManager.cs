using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x02000491 RID: 1169
	[ComVisible(true)]
	public interface IApplicationTrustManager : ISecurityEncodable
	{
		// Token: 0x06002EE6 RID: 12006
		ApplicationTrust DetermineApplicationTrust(ActivationContext activationContext, TrustManagerContext context);
	}
}
