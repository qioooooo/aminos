using System;
using System.ComponentModel;

namespace System.ServiceProcess
{
	// Token: 0x0200002F RID: 47
	[AttributeUsage(AttributeTargets.All)]
	public class ServiceProcessDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x060000EB RID: 235 RVA: 0x00005989 File Offset: 0x00004989
		public ServiceProcessDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00005992 File Offset: 0x00004992
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

		// Token: 0x04000220 RID: 544
		private bool replaced;
	}
}
