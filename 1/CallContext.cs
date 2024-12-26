using System;

namespace Microsoft.JScript
{
	// Token: 0x02000037 RID: 55
	internal class CallContext
	{
		// Token: 0x0600022E RID: 558 RVA: 0x00011176 File Offset: 0x00010176
		internal CallContext(Context sourceContext, LateBinding callee, object[] actual_parameters)
		{
			this.sourceContext = sourceContext;
			this.callee = callee;
			this.actual_parameters = actual_parameters;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00011193 File Offset: 0x00010193
		internal string FunctionName()
		{
			if (this.callee == null)
			{
				return "eval";
			}
			return this.callee.ToString();
		}

		// Token: 0x0400014C RID: 332
		internal readonly Context sourceContext;

		// Token: 0x0400014D RID: 333
		private readonly LateBinding callee;

		// Token: 0x0400014E RID: 334
		private readonly object[] actual_parameters;
	}
}
