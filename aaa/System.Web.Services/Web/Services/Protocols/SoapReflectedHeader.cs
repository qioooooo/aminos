using System;
using System.Reflection;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000076 RID: 118
	internal class SoapReflectedHeader
	{
		// Token: 0x04000345 RID: 837
		internal Type headerType;

		// Token: 0x04000346 RID: 838
		internal MemberInfo memberInfo;

		// Token: 0x04000347 RID: 839
		internal SoapHeaderDirection direction;

		// Token: 0x04000348 RID: 840
		internal bool repeats;

		// Token: 0x04000349 RID: 841
		internal bool custom;
	}
}
