using System;
using System.ComponentModel;

namespace System.Web.Services
{
	// Token: 0x02000019 RID: 25
	[AttributeUsage(AttributeTargets.All)]
	internal class WebServicesDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00002DC9 File Offset: 0x00001DC9
		internal WebServicesDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002DD2 File Offset: 0x00001DD2
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

		// Token: 0x04000222 RID: 546
		private bool replaced;
	}
}
