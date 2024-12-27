using System;

namespace System.Security.Permissions
{
	// Token: 0x0200061F RID: 1567
	[Serializable]
	internal enum BuiltInPermissionFlag
	{
		// Token: 0x04001D76 RID: 7542
		EnvironmentPermission = 1,
		// Token: 0x04001D77 RID: 7543
		FileDialogPermission,
		// Token: 0x04001D78 RID: 7544
		FileIOPermission = 4,
		// Token: 0x04001D79 RID: 7545
		IsolatedStorageFilePermission = 8,
		// Token: 0x04001D7A RID: 7546
		ReflectionPermission = 16,
		// Token: 0x04001D7B RID: 7547
		RegistryPermission = 32,
		// Token: 0x04001D7C RID: 7548
		SecurityPermission = 64,
		// Token: 0x04001D7D RID: 7549
		UIPermission = 128,
		// Token: 0x04001D7E RID: 7550
		PrincipalPermission = 256,
		// Token: 0x04001D7F RID: 7551
		PublisherIdentityPermission = 512,
		// Token: 0x04001D80 RID: 7552
		SiteIdentityPermission = 1024,
		// Token: 0x04001D81 RID: 7553
		StrongNameIdentityPermission = 2048,
		// Token: 0x04001D82 RID: 7554
		UrlIdentityPermission = 4096,
		// Token: 0x04001D83 RID: 7555
		ZoneIdentityPermission = 8192,
		// Token: 0x04001D84 RID: 7556
		KeyContainerPermission = 16384
	}
}
