using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000666 RID: 1638
	public sealed class SmtpSpecifiedPickupDirectoryElement : ConfigurationElement
	{
		// Token: 0x060032B3 RID: 12979 RVA: 0x000D71E5 File Offset: 0x000D61E5
		public SmtpSpecifiedPickupDirectoryElement()
		{
			this.properties.Add(this.pickupDirectoryLocation);
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x060032B4 RID: 12980 RVA: 0x000D7225 File Offset: 0x000D6225
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x060032B5 RID: 12981 RVA: 0x000D722D File Offset: 0x000D622D
		// (set) Token: 0x060032B6 RID: 12982 RVA: 0x000D7240 File Offset: 0x000D6240
		[ConfigurationProperty("pickupDirectoryLocation")]
		public string PickupDirectoryLocation
		{
			get
			{
				return (string)base[this.pickupDirectoryLocation];
			}
			set
			{
				base[this.pickupDirectoryLocation] = value;
			}
		}

		// Token: 0x04002F61 RID: 12129
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F62 RID: 12130
		private readonly ConfigurationProperty pickupDirectoryLocation = new ConfigurationProperty("pickupDirectoryLocation", typeof(string), null, ConfigurationPropertyOptions.None);
	}
}
