using System;
using System.ComponentModel;

namespace System.IO
{
	// Token: 0x0200072E RID: 1838
	[AttributeUsage(AttributeTargets.All)]
	public class IODescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06003823 RID: 14371 RVA: 0x000ED156 File Offset: 0x000EC156
		public IODescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06003824 RID: 14372 RVA: 0x000ED15F File Offset: 0x000EC15F
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

		// Token: 0x04003218 RID: 12824
		private bool replaced;
	}
}
