using System;

namespace Microsoft.JScript
{
	// Token: 0x020000EB RID: 235
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
	public class NotRecommended : Attribute
	{
		// Token: 0x06000A67 RID: 2663 RVA: 0x0004F2E1 File Offset: 0x0004E2E1
		public NotRecommended(string message)
		{
			this.message = message;
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x0004F2F0 File Offset: 0x0004E2F0
		public bool IsError
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0004F2F3 File Offset: 0x0004E2F3
		public string Message
		{
			get
			{
				return JScriptException.Localize(this.message, null);
			}
		}

		// Token: 0x04000670 RID: 1648
		private string message;
	}
}
