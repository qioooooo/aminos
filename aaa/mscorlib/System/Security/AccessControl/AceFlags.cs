using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008E3 RID: 2275
	[Flags]
	public enum AceFlags : byte
	{
		// Token: 0x04002AD6 RID: 10966
		None = 0,
		// Token: 0x04002AD7 RID: 10967
		ObjectInherit = 1,
		// Token: 0x04002AD8 RID: 10968
		ContainerInherit = 2,
		// Token: 0x04002AD9 RID: 10969
		NoPropagateInherit = 4,
		// Token: 0x04002ADA RID: 10970
		InheritOnly = 8,
		// Token: 0x04002ADB RID: 10971
		Inherited = 16,
		// Token: 0x04002ADC RID: 10972
		SuccessfulAccess = 64,
		// Token: 0x04002ADD RID: 10973
		FailedAccess = 128,
		// Token: 0x04002ADE RID: 10974
		InheritanceFlags = 15,
		// Token: 0x04002ADF RID: 10975
		AuditFlags = 192
	}
}
