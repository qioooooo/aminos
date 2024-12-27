using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004EA RID: 1258
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	public sealed class AutomationProxyAttribute : Attribute
	{
		// Token: 0x06003142 RID: 12610 RVA: 0x000A9445 File Offset: 0x000A8445
		public AutomationProxyAttribute(bool val)
		{
			this._val = val;
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06003143 RID: 12611 RVA: 0x000A9454 File Offset: 0x000A8454
		public bool Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001951 RID: 6481
		internal bool _val;
	}
}
