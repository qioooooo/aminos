using System;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace System.Security
{
	// Token: 0x02000479 RID: 1145
	[ComVisible(true)]
	public interface ISecurityPolicyEncodable
	{
		// Token: 0x06002E0F RID: 11791
		SecurityElement ToXml(PolicyLevel level);

		// Token: 0x06002E10 RID: 11792
		void FromXml(SecurityElement e, PolicyLevel level);
	}
}
