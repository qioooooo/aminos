using System;

namespace System.Security.Policy
{
	// Token: 0x0200047C RID: 1148
	internal interface IReportMatchMembershipCondition : IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06002E15 RID: 11797
		bool Check(Evidence evidence, out object usedEvidence);
	}
}
