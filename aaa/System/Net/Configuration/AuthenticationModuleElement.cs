using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000641 RID: 1601
	public sealed class AuthenticationModuleElement : ConfigurationElement
	{
		// Token: 0x06003191 RID: 12689 RVA: 0x000D450C File Offset: 0x000D350C
		public AuthenticationModuleElement()
		{
			this.properties.Add(this.type);
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x000D454C File Offset: 0x000D354C
		public AuthenticationModuleElement(string typeName)
			: this()
		{
			if (typeName != (string)this.type.DefaultValue)
			{
				this.Type = typeName;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06003193 RID: 12691 RVA: 0x000D4573 File Offset: 0x000D3573
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06003194 RID: 12692 RVA: 0x000D457B File Offset: 0x000D357B
		// (set) Token: 0x06003195 RID: 12693 RVA: 0x000D458E File Offset: 0x000D358E
		[ConfigurationProperty("type", IsRequired = true, IsKey = true)]
		public string Type
		{
			get
			{
				return (string)base[this.type];
			}
			set
			{
				base[this.type] = value;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06003196 RID: 12694 RVA: 0x000D459D File Offset: 0x000D359D
		internal string Key
		{
			get
			{
				return this.Type;
			}
		}

		// Token: 0x04002E92 RID: 11922
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002E93 RID: 11923
		private readonly ConfigurationProperty type = new ConfigurationProperty("type", typeof(string), null, ConfigurationPropertyOptions.IsKey);
	}
}
