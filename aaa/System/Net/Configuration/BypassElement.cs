using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000645 RID: 1605
	public sealed class BypassElement : ConfigurationElement
	{
		// Token: 0x060031AD RID: 12717 RVA: 0x000D49A0 File Offset: 0x000D39A0
		public BypassElement()
		{
			this.properties.Add(this.address);
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x000D49E0 File Offset: 0x000D39E0
		public BypassElement(string address)
			: this()
		{
			this.Address = address;
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x060031AF RID: 12719 RVA: 0x000D49EF File Offset: 0x000D39EF
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x060031B0 RID: 12720 RVA: 0x000D49F7 File Offset: 0x000D39F7
		// (set) Token: 0x060031B1 RID: 12721 RVA: 0x000D4A0A File Offset: 0x000D3A0A
		[ConfigurationProperty("address", IsRequired = true, IsKey = true)]
		public string Address
		{
			get
			{
				return (string)base[this.address];
			}
			set
			{
				base[this.address] = value;
			}
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x060031B2 RID: 12722 RVA: 0x000D4A19 File Offset: 0x000D3A19
		internal string Key
		{
			get
			{
				return this.Address;
			}
		}

		// Token: 0x04002E98 RID: 11928
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002E99 RID: 11929
		private readonly ConfigurationProperty address = new ConfigurationProperty("address", typeof(string), null, ConfigurationPropertyOptions.IsKey);
	}
}
