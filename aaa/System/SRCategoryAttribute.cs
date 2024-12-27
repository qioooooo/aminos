using System;
using System.ComponentModel;

namespace System
{
	// Token: 0x02000008 RID: 8
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class SRCategoryAttribute : CategoryAttribute
	{
		// Token: 0x06000021 RID: 33 RVA: 0x000023E0 File Offset: 0x000013E0
		public SRCategoryAttribute(string category)
			: base(category)
		{
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000023E9 File Offset: 0x000013E9
		protected override string GetLocalizedString(string value)
		{
			return SR.GetString(value);
		}
	}
}
