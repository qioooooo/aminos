using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x02000674 RID: 1652
	public sealed class IdnElement : ConfigurationElement
	{
		// Token: 0x060032FC RID: 13052 RVA: 0x000D7C70 File Offset: 0x000D6C70
		public IdnElement()
		{
			this.properties.Add(this.enabled);
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x060032FD RID: 13053 RVA: 0x000D7CC6 File Offset: 0x000D6CC6
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x060032FE RID: 13054 RVA: 0x000D7CCE File Offset: 0x000D6CCE
		// (set) Token: 0x060032FF RID: 13055 RVA: 0x000D7CE1 File Offset: 0x000D6CE1
		[ConfigurationProperty("enabled", DefaultValue = UriIdnScope.None)]
		public UriIdnScope Enabled
		{
			get
			{
				return (UriIdnScope)base[this.enabled];
			}
			set
			{
				base[this.enabled] = value;
			}
		}

		// Token: 0x04002F7B RID: 12155
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F7C RID: 12156
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(UriIdnScope), UriIdnScope.None, new IdnElement.UriIdnScopeTypeConverter(), null, ConfigurationPropertyOptions.None);

		// Token: 0x02000675 RID: 1653
		private class UriIdnScopeTypeConverter : TypeConverter
		{
			// Token: 0x06003300 RID: 13056 RVA: 0x000D7CF5 File Offset: 0x000D6CF5
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x06003301 RID: 13057 RVA: 0x000D7D10 File Offset: 0x000D6D10
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string text = value as string;
				if (text != null)
				{
					text = text.ToLower(CultureInfo.InvariantCulture);
					string text2;
					if ((text2 = text) != null)
					{
						if (text2 == "all")
						{
							return UriIdnScope.All;
						}
						if (text2 == "none")
						{
							return UriIdnScope.None;
						}
						if (text2 == "allexceptintranet")
						{
							return UriIdnScope.AllExceptIntranet;
						}
					}
				}
				return base.ConvertFrom(context, culture, value);
			}
		}
	}
}
