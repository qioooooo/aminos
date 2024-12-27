using System;
using System.ComponentModel;

namespace System.Configuration
{
	// Token: 0x02000006 RID: 6
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class SRCategoryAttribute : CategoryAttribute
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002101 File Offset: 0x00001101
		public SRCategoryAttribute(string category)
			: base(category)
		{
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000210A File Offset: 0x0000110A
		protected override string GetLocalizedString(string value)
		{
			return SR.GetString(value);
		}
	}
}
