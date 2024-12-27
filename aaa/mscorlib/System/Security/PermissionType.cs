using System;

namespace System.Security
{
	// Token: 0x02000658 RID: 1624
	[Serializable]
	internal enum PermissionType
	{
		// Token: 0x04001E48 RID: 7752
		SecurityUnmngdCodeAccess,
		// Token: 0x04001E49 RID: 7753
		SecuritySkipVerification,
		// Token: 0x04001E4A RID: 7754
		ReflectionTypeInfo,
		// Token: 0x04001E4B RID: 7755
		SecurityAssert,
		// Token: 0x04001E4C RID: 7756
		ReflectionMemberAccess,
		// Token: 0x04001E4D RID: 7757
		SecuritySerialization,
		// Token: 0x04001E4E RID: 7758
		ReflectionRestrictedMemberAccess,
		// Token: 0x04001E4F RID: 7759
		FullTrust,
		// Token: 0x04001E50 RID: 7760
		SecurityBindingRedirects,
		// Token: 0x04001E51 RID: 7761
		UIPermission,
		// Token: 0x04001E52 RID: 7762
		EnvironmentPermission,
		// Token: 0x04001E53 RID: 7763
		FileDialogPermission,
		// Token: 0x04001E54 RID: 7764
		FileIOPermission,
		// Token: 0x04001E55 RID: 7765
		ReflectionPermission,
		// Token: 0x04001E56 RID: 7766
		SecurityPermission,
		// Token: 0x04001E57 RID: 7767
		SecurityControlEvidence = 16,
		// Token: 0x04001E58 RID: 7768
		SecurityControlPrincipal
	}
}
