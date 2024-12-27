using System;

namespace Microsoft.JScript
{
	// Token: 0x02000102 RID: 258
	public sealed class ReferenceErrorObject : ErrorObject
	{
		// Token: 0x06000AFC RID: 2812 RVA: 0x000547ED File Offset: 0x000537ED
		internal ReferenceErrorObject(ErrorPrototype parent, object[] args)
			: base(parent, args)
		{
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x000547F7 File Offset: 0x000537F7
		internal ReferenceErrorObject(ErrorPrototype parent, object e)
			: base(parent, e)
		{
		}
	}
}
