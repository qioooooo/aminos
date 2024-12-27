using System;
using System.ComponentModel;

namespace System.Web
{
	// Token: 0x020000EE RID: 238
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Event)]
	internal sealed class WebSysDisplayNameAttribute : DisplayNameAttribute
	{
		// Token: 0x06000B16 RID: 2838 RVA: 0x0002C4E5 File Offset: 0x0002B4E5
		internal WebSysDisplayNameAttribute(string DisplayName)
			: base(DisplayName)
		{
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0002C4EE File Offset: 0x0002B4EE
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

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x0002C516 File Offset: 0x0002B516
		public override object TypeId
		{
			get
			{
				return typeof(DisplayNameAttribute);
			}
		}

		// Token: 0x04001341 RID: 4929
		private bool replaced;
	}
}
