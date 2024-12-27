using System;

namespace System.Configuration
{
	// Token: 0x0200070D RID: 1805
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class SettingsSerializeAsAttribute : Attribute
	{
		// Token: 0x0600374F RID: 14159 RVA: 0x000EAFF3 File Offset: 0x000E9FF3
		public SettingsSerializeAsAttribute(SettingsSerializeAs serializeAs)
		{
			this._serializeAs = serializeAs;
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06003750 RID: 14160 RVA: 0x000EB002 File Offset: 0x000EA002
		public SettingsSerializeAs SerializeAs
		{
			get
			{
				return this._serializeAs;
			}
		}

		// Token: 0x040031B8 RID: 12728
		private readonly SettingsSerializeAs _serializeAs;
	}
}
