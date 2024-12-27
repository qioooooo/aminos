using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200063A RID: 1594
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum SecurityPermissionFlag
	{
		// Token: 0x04001DE4 RID: 7652
		NoFlags = 0,
		// Token: 0x04001DE5 RID: 7653
		Assertion = 1,
		// Token: 0x04001DE6 RID: 7654
		UnmanagedCode = 2,
		// Token: 0x04001DE7 RID: 7655
		SkipVerification = 4,
		// Token: 0x04001DE8 RID: 7656
		Execution = 8,
		// Token: 0x04001DE9 RID: 7657
		ControlThread = 16,
		// Token: 0x04001DEA RID: 7658
		ControlEvidence = 32,
		// Token: 0x04001DEB RID: 7659
		ControlPolicy = 64,
		// Token: 0x04001DEC RID: 7660
		SerializationFormatter = 128,
		// Token: 0x04001DED RID: 7661
		ControlDomainPolicy = 256,
		// Token: 0x04001DEE RID: 7662
		ControlPrincipal = 512,
		// Token: 0x04001DEF RID: 7663
		ControlAppDomain = 1024,
		// Token: 0x04001DF0 RID: 7664
		RemotingConfiguration = 2048,
		// Token: 0x04001DF1 RID: 7665
		Infrastructure = 4096,
		// Token: 0x04001DF2 RID: 7666
		BindingRedirects = 8192,
		// Token: 0x04001DF3 RID: 7667
		AllFlags = 16383
	}
}
