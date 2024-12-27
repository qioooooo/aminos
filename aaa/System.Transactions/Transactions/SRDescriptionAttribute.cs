using System;
using System.ComponentModel;

namespace System.Transactions
{
	// Token: 0x02000002 RID: 2
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class SRDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06000001 RID: 1 RVA: 0x0002740C File Offset: 0x0002680C
		public SRDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00027420 File Offset: 0x00026820
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

		// Token: 0x04000001 RID: 1
		private bool replaced;
	}
}
