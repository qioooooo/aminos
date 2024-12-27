using System;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x02000026 RID: 38
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class ResDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06000098 RID: 152 RVA: 0x001C8578 File Offset: 0x001C7978
		public ResDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000099 RID: 153 RVA: 0x001C858C File Offset: 0x001C798C
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

		// Token: 0x04000073 RID: 115
		private bool replaced;
	}
}
