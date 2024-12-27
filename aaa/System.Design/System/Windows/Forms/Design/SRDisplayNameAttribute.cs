using System;
using System.ComponentModel;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000291 RID: 657
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
	internal sealed class SRDisplayNameAttribute : DisplayNameAttribute
	{
		// Token: 0x06001874 RID: 6260 RVA: 0x000806DA File Offset: 0x0007F6DA
		public SRDisplayNameAttribute(string displayName)
			: base(displayName)
		{
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001875 RID: 6261 RVA: 0x000806E3 File Offset: 0x0007F6E3
		public override string DisplayName
		{
			get
			{
				if (!this.replaced)
				{
					this.replaced = true;
					base.DisplayNameValue = SR.GetString(base.DisplayName);
				}
				return base.DisplayName;
			}
		}

		// Token: 0x04001428 RID: 5160
		private bool replaced;
	}
}
