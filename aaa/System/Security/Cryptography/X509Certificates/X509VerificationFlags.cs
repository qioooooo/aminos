using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000338 RID: 824
	[Flags]
	public enum X509VerificationFlags
	{
		// Token: 0x04001AF1 RID: 6897
		NoFlag = 0,
		// Token: 0x04001AF2 RID: 6898
		IgnoreNotTimeValid = 1,
		// Token: 0x04001AF3 RID: 6899
		IgnoreCtlNotTimeValid = 2,
		// Token: 0x04001AF4 RID: 6900
		IgnoreNotTimeNested = 4,
		// Token: 0x04001AF5 RID: 6901
		IgnoreInvalidBasicConstraints = 8,
		// Token: 0x04001AF6 RID: 6902
		AllowUnknownCertificateAuthority = 16,
		// Token: 0x04001AF7 RID: 6903
		IgnoreWrongUsage = 32,
		// Token: 0x04001AF8 RID: 6904
		IgnoreInvalidName = 64,
		// Token: 0x04001AF9 RID: 6905
		IgnoreInvalidPolicy = 128,
		// Token: 0x04001AFA RID: 6906
		IgnoreEndRevocationUnknown = 256,
		// Token: 0x04001AFB RID: 6907
		IgnoreCtlSignerRevocationUnknown = 512,
		// Token: 0x04001AFC RID: 6908
		IgnoreCertificateAuthorityRevocationUnknown = 1024,
		// Token: 0x04001AFD RID: 6909
		IgnoreRootRevocationUnknown = 2048,
		// Token: 0x04001AFE RID: 6910
		AllFlags = 4095
	}
}
