using System;

namespace System.Security.Principal
{
	// Token: 0x020004B8 RID: 1208
	[Serializable]
	internal enum KerbLogonSubmitType
	{
		// Token: 0x0400185A RID: 6234
		KerbInteractiveLogon = 2,
		// Token: 0x0400185B RID: 6235
		KerbSmartCardLogon = 6,
		// Token: 0x0400185C RID: 6236
		KerbWorkstationUnlockLogon,
		// Token: 0x0400185D RID: 6237
		KerbSmartCardUnlockLogon,
		// Token: 0x0400185E RID: 6238
		KerbProxyLogon,
		// Token: 0x0400185F RID: 6239
		KerbTicketLogon,
		// Token: 0x04001860 RID: 6240
		KerbTicketUnlockLogon,
		// Token: 0x04001861 RID: 6241
		KerbS4ULogon
	}
}
