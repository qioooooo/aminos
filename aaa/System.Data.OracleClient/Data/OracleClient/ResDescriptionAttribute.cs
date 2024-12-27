using System;
using System.ComponentModel;

namespace System.Data.OracleClient
{
	// Token: 0x02000002 RID: 2
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class ResDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06000001 RID: 1 RVA: 0x0005418C File Offset: 0x0005358C
		public ResDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x000541A0 File Offset: 0x000535A0
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

		// Token: 0x04000001 RID: 1
		private bool replaced;
	}
}
