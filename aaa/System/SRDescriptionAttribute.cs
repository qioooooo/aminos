using System;
using System.ComponentModel;

namespace System
{
	// Token: 0x02000006 RID: 6
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class SRDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x0600000A RID: 10 RVA: 0x0000215E File Offset: 0x0000115E
		public SRDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002167 File Offset: 0x00001167
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

		// Token: 0x04000037 RID: 55
		private bool replaced;
	}
}
