using System;
using System.ComponentModel;

namespace System.Web
{
	// Token: 0x020000ED RID: 237
	[AttributeUsage(AttributeTargets.All)]
	internal class WebSysDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06000B13 RID: 2835 RVA: 0x0002C4A8 File Offset: 0x0002B4A8
		internal WebSysDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x0002C4B1 File Offset: 0x0002B4B1
		public override string Description
		{
			get
			{
				if (!this.replaced)
				{
					this.replaced = true;
					base.DescriptionValue = SR.GetString(base.Description);
				}
				return base.Description;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0002C4D9 File Offset: 0x0002B4D9
		public override object TypeId
		{
			get
			{
				return typeof(DescriptionAttribute);
			}
		}

		// Token: 0x04001340 RID: 4928
		private bool replaced;
	}
}
