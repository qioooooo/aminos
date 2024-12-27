using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Net.Mail;

namespace System.Net.Configuration
{
	// Token: 0x02000661 RID: 1633
	public sealed class SmtpSection : ConfigurationSection
	{
		// Token: 0x0600328A RID: 12938 RVA: 0x000D6B68 File Offset: 0x000D5B68
		public SmtpSection()
		{
			this.properties.Add(this.deliveryMethod);
			this.properties.Add(this.from);
			this.properties.Add(this.network);
			this.properties.Add(this.specifiedPickupDirectory);
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x0600328B RID: 12939 RVA: 0x000D6C45 File Offset: 0x000D5C45
		// (set) Token: 0x0600328C RID: 12940 RVA: 0x000D6C58 File Offset: 0x000D5C58
		[ConfigurationProperty("deliveryMethod", DefaultValue = SmtpDeliveryMethod.Network)]
		public SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				return (SmtpDeliveryMethod)base[this.deliveryMethod];
			}
			set
			{
				base[this.deliveryMethod] = value;
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x0600328D RID: 12941 RVA: 0x000D6C6C File Offset: 0x000D5C6C
		// (set) Token: 0x0600328E RID: 12942 RVA: 0x000D6C7F File Offset: 0x000D5C7F
		[ConfigurationProperty("from")]
		public string From
		{
			get
			{
				return (string)base[this.from];
			}
			set
			{
				base[this.from] = value;
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x0600328F RID: 12943 RVA: 0x000D6C8E File Offset: 0x000D5C8E
		[ConfigurationProperty("network")]
		public SmtpNetworkElement Network
		{
			get
			{
				return (SmtpNetworkElement)base[this.network];
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06003290 RID: 12944 RVA: 0x000D6CA1 File Offset: 0x000D5CA1
		[ConfigurationProperty("specifiedPickupDirectory")]
		public SmtpSpecifiedPickupDirectoryElement SpecifiedPickupDirectory
		{
			get
			{
				return (SmtpSpecifiedPickupDirectoryElement)base[this.specifiedPickupDirectory];
			}
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06003291 RID: 12945 RVA: 0x000D6CB4 File Offset: 0x000D5CB4
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F4A RID: 12106
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F4B RID: 12107
		private readonly ConfigurationProperty from = new ConfigurationProperty("from", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F4C RID: 12108
		private readonly ConfigurationProperty network = new ConfigurationProperty("network", typeof(SmtpNetworkElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F4D RID: 12109
		private readonly ConfigurationProperty specifiedPickupDirectory = new ConfigurationProperty("specifiedPickupDirectory", typeof(SmtpSpecifiedPickupDirectoryElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F4E RID: 12110
		private readonly ConfigurationProperty deliveryMethod = new ConfigurationProperty("deliveryMethod", typeof(SmtpDeliveryMethod), SmtpDeliveryMethod.Network, new SmtpSection.SmtpDeliveryMethodTypeConverter(), null, ConfigurationPropertyOptions.None);

		// Token: 0x02000662 RID: 1634
		private class SmtpDeliveryMethodTypeConverter : TypeConverter
		{
			// Token: 0x06003292 RID: 12946 RVA: 0x000D6CBC File Offset: 0x000D5CBC
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x06003293 RID: 12947 RVA: 0x000D6CD8 File Offset: 0x000D5CD8
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string text = value as string;
				if (text != null)
				{
					text = text.ToLower(CultureInfo.InvariantCulture);
					string text2;
					if ((text2 = text) != null)
					{
						if (text2 == "network")
						{
							return SmtpDeliveryMethod.Network;
						}
						if (text2 == "specifiedpickupdirectory")
						{
							return SmtpDeliveryMethod.SpecifiedPickupDirectory;
						}
						if (text2 == "pickupdirectoryfromiis")
						{
							return SmtpDeliveryMethod.PickupDirectoryFromIis;
						}
					}
				}
				return base.ConvertFrom(context, culture, value);
			}
		}
	}
}
