using System;

namespace System.Configuration
{
	// Token: 0x02000706 RID: 1798
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class DefaultSettingValueAttribute : Attribute
	{
		// Token: 0x06003741 RID: 14145 RVA: 0x000EAF4A File Offset: 0x000E9F4A
		public DefaultSettingValueAttribute(string value)
		{
			this._value = value;
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06003742 RID: 14146 RVA: 0x000EAF59 File Offset: 0x000E9F59
		public string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x040031B2 RID: 12722
		private readonly string _value;
	}
}
