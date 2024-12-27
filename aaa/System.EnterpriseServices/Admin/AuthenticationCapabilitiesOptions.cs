using System;

namespace System.EnterpriseServices.Admin
{
	// Token: 0x02000052 RID: 82
	[Serializable]
	internal enum AuthenticationCapabilitiesOptions
	{
		// Token: 0x040000A7 RID: 167
		None,
		// Token: 0x040000A8 RID: 168
		StaticCloaking = 32,
		// Token: 0x040000A9 RID: 169
		DynamicCloaking = 64,
		// Token: 0x040000AA RID: 170
		SecureReference = 2
	}
}
