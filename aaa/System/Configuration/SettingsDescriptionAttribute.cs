using System;

namespace System.Configuration
{
	// Token: 0x02000708 RID: 1800
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class SettingsDescriptionAttribute : Attribute
	{
		// Token: 0x06003744 RID: 14148 RVA: 0x000EAF69 File Offset: 0x000E9F69
		public SettingsDescriptionAttribute(string description)
		{
			this._desc = description;
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x06003745 RID: 14149 RVA: 0x000EAF78 File Offset: 0x000E9F78
		public string Description
		{
			get
			{
				return this._desc;
			}
		}

		// Token: 0x040031B3 RID: 12723
		private readonly string _desc;
	}
}
