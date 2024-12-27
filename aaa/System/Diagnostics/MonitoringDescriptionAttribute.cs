using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x02000761 RID: 1889
	[AttributeUsage(AttributeTargets.All)]
	public class MonitoringDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x060039FE RID: 14846 RVA: 0x000F553F File Offset: 0x000F453F
		public MonitoringDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x060039FF RID: 14847 RVA: 0x000F5548 File Offset: 0x000F4548
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

		// Token: 0x040032E7 RID: 13031
		private bool replaced;
	}
}
