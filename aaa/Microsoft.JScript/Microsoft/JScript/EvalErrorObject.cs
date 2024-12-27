using System;

namespace Microsoft.JScript
{
	// Token: 0x0200007B RID: 123
	public sealed class EvalErrorObject : ErrorObject
	{
		// Token: 0x060005A6 RID: 1446 RVA: 0x000277DE File Offset: 0x000267DE
		internal EvalErrorObject(ErrorPrototype parent, object[] args)
			: base(parent, args)
		{
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x000277E8 File Offset: 0x000267E8
		internal EvalErrorObject(ErrorPrototype parent, object e)
			: base(parent, e)
		{
		}
	}
}
