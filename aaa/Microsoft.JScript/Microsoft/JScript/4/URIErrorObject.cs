using System;

namespace Microsoft.JScript
{
	// Token: 0x0200012B RID: 299
	public sealed class URIErrorObject : ErrorObject
	{
		// Token: 0x06000DD4 RID: 3540 RVA: 0x0005DF32 File Offset: 0x0005CF32
		internal URIErrorObject(ErrorPrototype parent, object[] args)
			: base(parent, args)
		{
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0005DF3C File Offset: 0x0005CF3C
		internal URIErrorObject(ErrorPrototype parent, object e)
			: base(parent, e)
		{
		}
	}
}
