using System;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x02000027 RID: 39
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class ResCategoryAttribute : CategoryAttribute
	{
		// Token: 0x0600009A RID: 154 RVA: 0x001C85C0 File Offset: 0x001C79C0
		public ResCategoryAttribute(string category)
			: base(category)
		{
		}

		// Token: 0x0600009B RID: 155 RVA: 0x001C85D4 File Offset: 0x001C79D4
		protected override string GetLocalizedString(string value)
		{
			return Res.GetString(value);
		}
	}
}
