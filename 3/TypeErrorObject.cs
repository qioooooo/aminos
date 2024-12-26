using System;

namespace Microsoft.JScript
{
	// Token: 0x02000123 RID: 291
	public sealed class TypeErrorObject : ErrorObject
	{
		// Token: 0x06000BEA RID: 3050 RVA: 0x0005AA76 File Offset: 0x00059A76
		internal TypeErrorObject(ErrorPrototype parent, object[] args)
			: base(parent, args)
		{
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0005AA80 File Offset: 0x00059A80
		internal TypeErrorObject(ErrorPrototype parent, object e)
			: base(parent, e)
		{
		}
	}
}
