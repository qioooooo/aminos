using System;
using System.Configuration;

namespace System.Web.Services.Configuration
{
	// Token: 0x02000142 RID: 322
	public sealed class WsiProfilesElement : ConfigurationElement
	{
		// Token: 0x06000A12 RID: 2578 RVA: 0x000478EC File Offset: 0x000468EC
		public WsiProfilesElement()
		{
			this.properties.Add(this.name);
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0004793C File Offset: 0x0004693C
		public WsiProfilesElement(WsiProfiles name)
			: this()
		{
			this.Name = name;
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0004794B File Offset: 0x0004694B
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x0004795E File Offset: 0x0004695E
		[ConfigurationProperty("name", IsKey = true, DefaultValue = WsiProfiles.None)]
		public WsiProfiles Name
		{
			get
			{
				return (WsiProfiles)base[this.name];
			}
			set
			{
				if (!this.IsValidWsiProfilesValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				base[this.name] = value;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x00047986 File Offset: 0x00046986
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0004798E File Offset: 0x0004698E
		private bool IsValidWsiProfilesValue(WsiProfiles value)
		{
			return Enum.IsDefined(typeof(WsiProfiles), value);
		}

		// Token: 0x04000642 RID: 1602
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04000643 RID: 1603
		private readonly ConfigurationProperty name = new ConfigurationProperty("name", typeof(WsiProfiles), WsiProfiles.None, ConfigurationPropertyOptions.IsKey);
	}
}
