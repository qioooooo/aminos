using System;

namespace System.Security.Policy
{
	// Token: 0x02000490 RID: 1168
	internal interface IDelayEvaluatedEvidence
	{
		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002EE3 RID: 12003
		bool IsVerified { get; }

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002EE4 RID: 12004
		bool WasUsed { get; }

		// Token: 0x06002EE5 RID: 12005
		void MarkUsed();
	}
}
