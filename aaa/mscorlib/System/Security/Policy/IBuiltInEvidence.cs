using System;

namespace System.Security.Policy
{
	// Token: 0x0200047E RID: 1150
	internal interface IBuiltInEvidence
	{
		// Token: 0x06002E21 RID: 11809
		int OutputToBuffer(char[] buffer, int position, bool verbose);

		// Token: 0x06002E22 RID: 11810
		int InitFromBuffer(char[] buffer, int position);

		// Token: 0x06002E23 RID: 11811
		int GetRequiredSize(bool verbose);
	}
}
