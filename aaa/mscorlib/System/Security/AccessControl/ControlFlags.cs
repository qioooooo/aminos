using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000924 RID: 2340
	[Flags]
	public enum ControlFlags
	{
		// Token: 0x04002BEE RID: 11246
		None = 0,
		// Token: 0x04002BEF RID: 11247
		OwnerDefaulted = 1,
		// Token: 0x04002BF0 RID: 11248
		GroupDefaulted = 2,
		// Token: 0x04002BF1 RID: 11249
		DiscretionaryAclPresent = 4,
		// Token: 0x04002BF2 RID: 11250
		DiscretionaryAclDefaulted = 8,
		// Token: 0x04002BF3 RID: 11251
		SystemAclPresent = 16,
		// Token: 0x04002BF4 RID: 11252
		SystemAclDefaulted = 32,
		// Token: 0x04002BF5 RID: 11253
		DiscretionaryAclUntrusted = 64,
		// Token: 0x04002BF6 RID: 11254
		ServerSecurity = 128,
		// Token: 0x04002BF7 RID: 11255
		DiscretionaryAclAutoInheritRequired = 256,
		// Token: 0x04002BF8 RID: 11256
		SystemAclAutoInheritRequired = 512,
		// Token: 0x04002BF9 RID: 11257
		DiscretionaryAclAutoInherited = 1024,
		// Token: 0x04002BFA RID: 11258
		SystemAclAutoInherited = 2048,
		// Token: 0x04002BFB RID: 11259
		DiscretionaryAclProtected = 4096,
		// Token: 0x04002BFC RID: 11260
		SystemAclProtected = 8192,
		// Token: 0x04002BFD RID: 11261
		RMControlValid = 16384,
		// Token: 0x04002BFE RID: 11262
		SelfRelative = 32768
	}
}
