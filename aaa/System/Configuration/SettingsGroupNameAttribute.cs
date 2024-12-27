using System;

namespace System.Configuration
{
	// Token: 0x0200070A RID: 1802
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SettingsGroupNameAttribute : Attribute
	{
		// Token: 0x06003748 RID: 14152 RVA: 0x000EAF97 File Offset: 0x000E9F97
		public SettingsGroupNameAttribute(string groupName)
		{
			this._groupName = groupName;
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06003749 RID: 14153 RVA: 0x000EAFA6 File Offset: 0x000E9FA6
		public string GroupName
		{
			get
			{
				return this._groupName;
			}
		}

		// Token: 0x040031B5 RID: 12725
		private readonly string _groupName;
	}
}
