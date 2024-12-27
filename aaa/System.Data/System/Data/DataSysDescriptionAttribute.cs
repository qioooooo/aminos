using System;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x02000099 RID: 153
	[AttributeUsage(AttributeTargets.All)]
	[Obsolete("DataSysDescriptionAttribute has been deprecated.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
	public class DataSysDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06000928 RID: 2344 RVA: 0x001E9508 File Offset: 0x001E8908
		[Obsolete("DataSysDescriptionAttribute has been deprecated.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
		public DataSysDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x001E951C File Offset: 0x001E891C
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

		// Token: 0x040007B7 RID: 1975
		private bool replaced;
	}
}
