using System;
using System.ComponentModel;
using System.Configuration;

namespace System.Xml.Serialization.Configuration
{
	// Token: 0x0200034D RID: 845
	public sealed class DateTimeSerializationSection : ConfigurationSection
	{
		// Token: 0x06002914 RID: 10516 RVA: 0x000D3224 File Offset: 0x000D2224
		public DateTimeSerializationSection()
		{
			this.properties.Add(this.mode);
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06002915 RID: 10517 RVA: 0x000D3284 File Offset: 0x000D2284
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06002916 RID: 10518 RVA: 0x000D328C File Offset: 0x000D228C
		// (set) Token: 0x06002917 RID: 10519 RVA: 0x000D329F File Offset: 0x000D229F
		[ConfigurationProperty("mode", DefaultValue = DateTimeSerializationSection.DateTimeSerializationMode.Roundtrip)]
		public DateTimeSerializationSection.DateTimeSerializationMode Mode
		{
			get
			{
				return (DateTimeSerializationSection.DateTimeSerializationMode)base[this.mode];
			}
			set
			{
				base[this.mode] = value;
			}
		}

		// Token: 0x040016D9 RID: 5849
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x040016DA RID: 5850
		private readonly ConfigurationProperty mode = new ConfigurationProperty("mode", typeof(DateTimeSerializationSection.DateTimeSerializationMode), DateTimeSerializationSection.DateTimeSerializationMode.Roundtrip, new EnumConverter(typeof(DateTimeSerializationSection.DateTimeSerializationMode)), null, ConfigurationPropertyOptions.None);

		// Token: 0x0200034E RID: 846
		public enum DateTimeSerializationMode
		{
			// Token: 0x040016DC RID: 5852
			Default,
			// Token: 0x040016DD RID: 5853
			Roundtrip,
			// Token: 0x040016DE RID: 5854
			Local
		}
	}
}
