using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008E2 RID: 2274
	public enum AceType : byte
	{
		// Token: 0x04002AC3 RID: 10947
		AccessAllowed,
		// Token: 0x04002AC4 RID: 10948
		AccessDenied,
		// Token: 0x04002AC5 RID: 10949
		SystemAudit,
		// Token: 0x04002AC6 RID: 10950
		SystemAlarm,
		// Token: 0x04002AC7 RID: 10951
		AccessAllowedCompound,
		// Token: 0x04002AC8 RID: 10952
		AccessAllowedObject,
		// Token: 0x04002AC9 RID: 10953
		AccessDeniedObject,
		// Token: 0x04002ACA RID: 10954
		SystemAuditObject,
		// Token: 0x04002ACB RID: 10955
		SystemAlarmObject,
		// Token: 0x04002ACC RID: 10956
		AccessAllowedCallback,
		// Token: 0x04002ACD RID: 10957
		AccessDeniedCallback,
		// Token: 0x04002ACE RID: 10958
		AccessAllowedCallbackObject,
		// Token: 0x04002ACF RID: 10959
		AccessDeniedCallbackObject,
		// Token: 0x04002AD0 RID: 10960
		SystemAuditCallback,
		// Token: 0x04002AD1 RID: 10961
		SystemAlarmCallback,
		// Token: 0x04002AD2 RID: 10962
		SystemAuditCallbackObject,
		// Token: 0x04002AD3 RID: 10963
		SystemAlarmCallbackObject,
		// Token: 0x04002AD4 RID: 10964
		MaxDefinedAceType = 16
	}
}
