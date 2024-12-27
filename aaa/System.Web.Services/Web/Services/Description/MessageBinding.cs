using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000E4 RID: 228
	public abstract class MessageBinding : NamedItem
	{
		// Token: 0x06000629 RID: 1577 RVA: 0x0001D306 File Offset: 0x0001C306
		internal void SetParent(OperationBinding parent)
		{
			this.parent = parent;
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x0001D30F File Offset: 0x0001C30F
		public OperationBinding OperationBinding
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x0400045B RID: 1115
		private OperationBinding parent;
	}
}
