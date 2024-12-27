using System;

namespace Microsoft.JScript
{
	// Token: 0x020000FE RID: 254
	public sealed class RangeErrorObject : ErrorObject
	{
		// Token: 0x06000AEB RID: 2795 RVA: 0x0005471C File Offset: 0x0005371C
		internal RangeErrorObject(ErrorPrototype parent, object[] args)
			: base(parent, args)
		{
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00054726 File Offset: 0x00053726
		internal RangeErrorObject(ErrorPrototype parent, object e)
			: base(parent, e)
		{
		}
	}
}
