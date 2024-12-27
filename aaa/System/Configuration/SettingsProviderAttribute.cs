using System;

namespace System.Configuration
{
	// Token: 0x0200070C RID: 1804
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class SettingsProviderAttribute : Attribute
	{
		// Token: 0x0600374C RID: 14156 RVA: 0x000EAFC5 File Offset: 0x000E9FC5
		public SettingsProviderAttribute(string providerTypeName)
		{
			this._providerTypeName = providerTypeName;
		}

		// Token: 0x0600374D RID: 14157 RVA: 0x000EAFD4 File Offset: 0x000E9FD4
		public SettingsProviderAttribute(Type providerType)
		{
			if (providerType != null)
			{
				this._providerTypeName = providerType.AssemblyQualifiedName;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x0600374E RID: 14158 RVA: 0x000EAFEB File Offset: 0x000E9FEB
		public string ProviderTypeName
		{
			get
			{
				return this._providerTypeName;
			}
		}

		// Token: 0x040031B7 RID: 12727
		private readonly string _providerTypeName;
	}
}
