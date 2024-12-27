using System;
using System.ComponentModel;

namespace System.Timers
{
	// Token: 0x02000738 RID: 1848
	[AttributeUsage(AttributeTargets.All)]
	public class TimersDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06003855 RID: 14421 RVA: 0x000ED9F8 File Offset: 0x000EC9F8
		public TimersDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06003856 RID: 14422 RVA: 0x000EDA01 File Offset: 0x000ECA01
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

		// Token: 0x04003238 RID: 12856
		private bool replaced;
	}
}
