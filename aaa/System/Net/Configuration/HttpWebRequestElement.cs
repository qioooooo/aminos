using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200064F RID: 1615
	public sealed class HttpWebRequestElement : ConfigurationElement
	{
		// Token: 0x060031F8 RID: 12792 RVA: 0x000D557C File Offset: 0x000D457C
		public HttpWebRequestElement()
		{
			this.properties.Add(this.maximumResponseHeadersLength);
			this.properties.Add(this.maximumErrorResponseLength);
			this.properties.Add(this.maximumUnauthorizedUploadLength);
			this.properties.Add(this.useUnsafeHeaderParsing);
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x000D5664 File Offset: 0x000D4664
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			PropertyInformation[] array = new PropertyInformation[]
			{
				base.ElementInformation.Properties["maximumResponseHeadersLength"],
				base.ElementInformation.Properties["maximumErrorResponseLength"]
			};
			foreach (PropertyInformation propertyInformation in array)
			{
				if (propertyInformation.ValueOrigin == PropertyValueOrigin.SetHere)
				{
					try
					{
						ExceptionHelper.WebPermissionUnrestricted.Demand();
					}
					catch (Exception ex)
					{
						throw new ConfigurationErrorsException(SR.GetString("net_config_property_permission", new object[] { propertyInformation.Name }), ex);
					}
				}
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x060031FA RID: 12794 RVA: 0x000D5720 File Offset: 0x000D4720
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x060031FB RID: 12795 RVA: 0x000D5728 File Offset: 0x000D4728
		// (set) Token: 0x060031FC RID: 12796 RVA: 0x000D573B File Offset: 0x000D473B
		[ConfigurationProperty("maximumUnauthorizedUploadLength", DefaultValue = -1)]
		public int MaximumUnauthorizedUploadLength
		{
			get
			{
				return (int)base[this.maximumUnauthorizedUploadLength];
			}
			set
			{
				base[this.maximumUnauthorizedUploadLength] = value;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x060031FD RID: 12797 RVA: 0x000D574F File Offset: 0x000D474F
		// (set) Token: 0x060031FE RID: 12798 RVA: 0x000D5762 File Offset: 0x000D4762
		[ConfigurationProperty("maximumErrorResponseLength", DefaultValue = 64)]
		public int MaximumErrorResponseLength
		{
			get
			{
				return (int)base[this.maximumErrorResponseLength];
			}
			set
			{
				base[this.maximumErrorResponseLength] = value;
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x060031FF RID: 12799 RVA: 0x000D5776 File Offset: 0x000D4776
		// (set) Token: 0x06003200 RID: 12800 RVA: 0x000D5789 File Offset: 0x000D4789
		[ConfigurationProperty("maximumResponseHeadersLength", DefaultValue = 64)]
		public int MaximumResponseHeadersLength
		{
			get
			{
				return (int)base[this.maximumResponseHeadersLength];
			}
			set
			{
				base[this.maximumResponseHeadersLength] = value;
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06003201 RID: 12801 RVA: 0x000D579D File Offset: 0x000D479D
		// (set) Token: 0x06003202 RID: 12802 RVA: 0x000D57B0 File Offset: 0x000D47B0
		[ConfigurationProperty("useUnsafeHeaderParsing", DefaultValue = false)]
		public bool UseUnsafeHeaderParsing
		{
			get
			{
				return (bool)base[this.useUnsafeHeaderParsing];
			}
			set
			{
				base[this.useUnsafeHeaderParsing] = value;
			}
		}

		// Token: 0x04002EF3 RID: 12019
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002EF4 RID: 12020
		private readonly ConfigurationProperty maximumResponseHeadersLength = new ConfigurationProperty("maximumResponseHeadersLength", typeof(int), 64, ConfigurationPropertyOptions.None);

		// Token: 0x04002EF5 RID: 12021
		private readonly ConfigurationProperty maximumErrorResponseLength = new ConfigurationProperty("maximumErrorResponseLength", typeof(int), 64, ConfigurationPropertyOptions.None);

		// Token: 0x04002EF6 RID: 12022
		private readonly ConfigurationProperty maximumUnauthorizedUploadLength = new ConfigurationProperty("maximumUnauthorizedUploadLength", typeof(int), -1, ConfigurationPropertyOptions.None);

		// Token: 0x04002EF7 RID: 12023
		private readonly ConfigurationProperty useUnsafeHeaderParsing = new ConfigurationProperty("useUnsafeHeaderParsing", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}
