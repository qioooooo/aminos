using System;

namespace Microsoft.JScript
{
	// Token: 0x0200011E RID: 286
	public sealed class SyntaxErrorObject : ErrorObject
	{
		// Token: 0x06000BBA RID: 3002 RVA: 0x0005938C File Offset: 0x0005838C
		internal SyntaxErrorObject(ErrorPrototype parent, object[] args)
			: base(parent, args)
		{
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00059396 File Offset: 0x00058396
		internal SyntaxErrorObject(ErrorPrototype parent, object e)
			: base(parent, e)
		{
		}
	}
}
