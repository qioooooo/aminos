using System;
using System.ComponentModel;

namespace System.Data.OracleClient
{
	// Token: 0x02000003 RID: 3
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class ResCategoryAttribute : CategoryAttribute
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000541D4 File Offset: 0x000535D4
		public ResCategoryAttribute(string category)
			: base(category)
		{
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000541E8 File Offset: 0x000535E8
		protected override string GetLocalizedString(string value)
		{
			return Res.GetString(value);
		}
	}
}
