using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008F7 RID: 2295
	[Flags]
	public enum CryptoKeyRights
	{
		// Token: 0x04002B23 RID: 11043
		ReadData = 1,
		// Token: 0x04002B24 RID: 11044
		WriteData = 2,
		// Token: 0x04002B25 RID: 11045
		ReadExtendedAttributes = 8,
		// Token: 0x04002B26 RID: 11046
		WriteExtendedAttributes = 16,
		// Token: 0x04002B27 RID: 11047
		ReadAttributes = 128,
		// Token: 0x04002B28 RID: 11048
		WriteAttributes = 256,
		// Token: 0x04002B29 RID: 11049
		Delete = 65536,
		// Token: 0x04002B2A RID: 11050
		ReadPermissions = 131072,
		// Token: 0x04002B2B RID: 11051
		ChangePermissions = 262144,
		// Token: 0x04002B2C RID: 11052
		TakeOwnership = 524288,
		// Token: 0x04002B2D RID: 11053
		Synchronize = 1048576,
		// Token: 0x04002B2E RID: 11054
		FullControl = 2032027,
		// Token: 0x04002B2F RID: 11055
		GenericAll = 268435456,
		// Token: 0x04002B30 RID: 11056
		GenericExecute = 536870912,
		// Token: 0x04002B31 RID: 11057
		GenericWrite = 1073741824,
		// Token: 0x04002B32 RID: 11058
		GenericRead = -2147483648
	}
}
