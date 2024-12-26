using System;

namespace Microsoft.JScript
{
	// Token: 0x020000EC RID: 236
	internal sealed class NullLiteral : ConstantWrapper
	{
		// Token: 0x06000A6A RID: 2666 RVA: 0x0004F301 File Offset: 0x0004E301
		internal NullLiteral(Context context)
			: base(DBNull.Value, context)
		{
		}
	}
}
