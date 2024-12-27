using System;

namespace System.DirectoryServices
{
	// Token: 0x0200000A RID: 10
	[Flags]
	public enum ActiveDirectoryRights
	{
		// Token: 0x04000131 RID: 305
		Delete = 65536,
		// Token: 0x04000132 RID: 306
		ReadControl = 131072,
		// Token: 0x04000133 RID: 307
		WriteDacl = 262144,
		// Token: 0x04000134 RID: 308
		WriteOwner = 524288,
		// Token: 0x04000135 RID: 309
		Synchronize = 1048576,
		// Token: 0x04000136 RID: 310
		AccessSystemSecurity = 16777216,
		// Token: 0x04000137 RID: 311
		GenericRead = 131220,
		// Token: 0x04000138 RID: 312
		GenericWrite = 131112,
		// Token: 0x04000139 RID: 313
		GenericExecute = 131076,
		// Token: 0x0400013A RID: 314
		GenericAll = 983551,
		// Token: 0x0400013B RID: 315
		CreateChild = 1,
		// Token: 0x0400013C RID: 316
		DeleteChild = 2,
		// Token: 0x0400013D RID: 317
		ListChildren = 4,
		// Token: 0x0400013E RID: 318
		Self = 8,
		// Token: 0x0400013F RID: 319
		ReadProperty = 16,
		// Token: 0x04000140 RID: 320
		WriteProperty = 32,
		// Token: 0x04000141 RID: 321
		DeleteTree = 64,
		// Token: 0x04000142 RID: 322
		ListObject = 128,
		// Token: 0x04000143 RID: 323
		ExtendedRight = 256
	}
}
