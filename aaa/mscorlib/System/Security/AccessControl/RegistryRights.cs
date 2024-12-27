using System;

namespace System.Security.AccessControl
{
	// Token: 0x0200091C RID: 2332
	[Flags]
	public enum RegistryRights
	{
		// Token: 0x04002BD6 RID: 11222
		QueryValues = 1,
		// Token: 0x04002BD7 RID: 11223
		SetValue = 2,
		// Token: 0x04002BD8 RID: 11224
		CreateSubKey = 4,
		// Token: 0x04002BD9 RID: 11225
		EnumerateSubKeys = 8,
		// Token: 0x04002BDA RID: 11226
		Notify = 16,
		// Token: 0x04002BDB RID: 11227
		CreateLink = 32,
		// Token: 0x04002BDC RID: 11228
		ExecuteKey = 131097,
		// Token: 0x04002BDD RID: 11229
		ReadKey = 131097,
		// Token: 0x04002BDE RID: 11230
		WriteKey = 131078,
		// Token: 0x04002BDF RID: 11231
		Delete = 65536,
		// Token: 0x04002BE0 RID: 11232
		ReadPermissions = 131072,
		// Token: 0x04002BE1 RID: 11233
		ChangePermissions = 262144,
		// Token: 0x04002BE2 RID: 11234
		TakeOwnership = 524288,
		// Token: 0x04002BE3 RID: 11235
		FullControl = 983103
	}
}
