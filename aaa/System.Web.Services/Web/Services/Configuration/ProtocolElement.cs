using System;
using System.Configuration;

namespace System.Web.Services.Configuration
{
	// Token: 0x0200013F RID: 319
	public sealed class ProtocolElement : ConfigurationElement
	{
		// Token: 0x060009FC RID: 2556 RVA: 0x000475FC File Offset: 0x000465FC
		public ProtocolElement()
		{
			this.properties.Add(this.name);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0004764C File Offset: 0x0004664C
		public ProtocolElement(WebServiceProtocols protocol)
			: this()
		{
			this.Name = protocol;
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x0004765B File Offset: 0x0004665B
		// (set) Token: 0x060009FF RID: 2559 RVA: 0x0004766E File Offset: 0x0004666E
		[ConfigurationProperty("name", IsKey = true, DefaultValue = WebServiceProtocols.Unknown)]
		public WebServiceProtocols Name
		{
			get
			{
				return (WebServiceProtocols)base[this.name];
			}
			set
			{
				if (!this.IsValidProtocolsValue(value))
				{
					value = WebServiceProtocols.Unknown;
				}
				base[this.name] = value;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0004768E File Offset: 0x0004668E
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00047696 File Offset: 0x00046696
		private bool IsValidProtocolsValue(WebServiceProtocols value)
		{
			return Enum.IsDefined(typeof(WebServiceProtocols), value);
		}

		// Token: 0x04000637 RID: 1591
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04000638 RID: 1592
		private readonly ConfigurationProperty name = new ConfigurationProperty("name", typeof(WebServiceProtocols), WebServiceProtocols.Unknown, ConfigurationPropertyOptions.IsKey);
	}
}
