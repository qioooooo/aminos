using System;

namespace System.Net
{
	// Token: 0x020003FB RID: 1019
	internal enum IgnoreCertProblem
	{
		// Token: 0x04002040 RID: 8256
		not_time_valid = 1,
		// Token: 0x04002041 RID: 8257
		ctl_not_time_valid,
		// Token: 0x04002042 RID: 8258
		not_time_nested = 4,
		// Token: 0x04002043 RID: 8259
		invalid_basic_constraints = 8,
		// Token: 0x04002044 RID: 8260
		all_not_time_valid = 7,
		// Token: 0x04002045 RID: 8261
		allow_unknown_ca = 16,
		// Token: 0x04002046 RID: 8262
		wrong_usage = 32,
		// Token: 0x04002047 RID: 8263
		invalid_name = 64,
		// Token: 0x04002048 RID: 8264
		invalid_policy = 128,
		// Token: 0x04002049 RID: 8265
		end_rev_unknown = 256,
		// Token: 0x0400204A RID: 8266
		ctl_signer_rev_unknown = 512,
		// Token: 0x0400204B RID: 8267
		ca_rev_unknown = 1024,
		// Token: 0x0400204C RID: 8268
		root_rev_unknown = 2048,
		// Token: 0x0400204D RID: 8269
		all_rev_unknown = 3840,
		// Token: 0x0400204E RID: 8270
		none = 4095
	}
}
