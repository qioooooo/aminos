using System;
using System.ComponentModel;

namespace System.DirectoryServices
{
	// Token: 0x0200001D RID: 29
	[AttributeUsage(AttributeTargets.All)]
	public class DSDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00002FE4 File Offset: 0x00001FE4
		public DSDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002FED File Offset: 0x00001FED
		public override string Description
		{
			get
			{
				if (!this.replaced)
				{
					this.replaced = true;
					base.DescriptionValue = Res.GetString(base.Description);
				}
				return base.Description;
			}
		}

		// Token: 0x04000168 RID: 360
		private bool replaced;
	}
}
