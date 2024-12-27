using System;

namespace System.Security
{
	// Token: 0x020005FB RID: 1531
	internal interface ISecurityElementFactory
	{
		// Token: 0x060037CD RID: 14285
		SecurityElement CreateSecurityElement();

		// Token: 0x060037CE RID: 14286
		object Copy();

		// Token: 0x060037CF RID: 14287
		string GetTag();

		// Token: 0x060037D0 RID: 14288
		string Attribute(string attributeName);
	}
}
