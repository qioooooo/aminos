using System;

namespace System.Configuration
{
	// Token: 0x02000709 RID: 1801
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SettingsGroupDescriptionAttribute : Attribute
	{
		// Token: 0x06003746 RID: 14150 RVA: 0x000EAF80 File Offset: 0x000E9F80
		public SettingsGroupDescriptionAttribute(string description)
		{
			this._desc = description;
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x06003747 RID: 14151 RVA: 0x000EAF8F File Offset: 0x000E9F8F
		public string Description
		{
			get
			{
				return this._desc;
			}
		}

		// Token: 0x040031B4 RID: 12724
		private readonly string _desc;
	}
}
