using System;

namespace System.Security.Permissions
{
	// Token: 0x020002C4 RID: 708
	[Flags]
	[Serializable]
	public enum StorePermissionFlags
	{
		// Token: 0x04001624 RID: 5668
		NoFlags = 0,
		// Token: 0x04001625 RID: 5669
		CreateStore = 1,
		// Token: 0x04001626 RID: 5670
		DeleteStore = 2,
		// Token: 0x04001627 RID: 5671
		EnumerateStores = 4,
		// Token: 0x04001628 RID: 5672
		OpenStore = 16,
		// Token: 0x04001629 RID: 5673
		AddToStore = 32,
		// Token: 0x0400162A RID: 5674
		RemoveFromStore = 64,
		// Token: 0x0400162B RID: 5675
		EnumerateCertificates = 128,
		// Token: 0x0400162C RID: 5676
		AllFlags = 247
	}
}
