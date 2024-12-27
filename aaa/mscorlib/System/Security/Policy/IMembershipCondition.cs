using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x0200047A RID: 1146
	[ComVisible(true)]
	public interface IMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06002E11 RID: 11793
		bool Check(Evidence evidence);

		// Token: 0x06002E12 RID: 11794
		IMembershipCondition Copy();

		// Token: 0x06002E13 RID: 11795
		string ToString();

		// Token: 0x06002E14 RID: 11796
		bool Equals(object obj);
	}
}
