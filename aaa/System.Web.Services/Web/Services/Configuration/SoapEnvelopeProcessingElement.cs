using System;
using System.ComponentModel;
using System.Configuration;

namespace System.Web.Services.Configuration
{
	// Token: 0x02000135 RID: 309
	public sealed class SoapEnvelopeProcessingElement : ConfigurationElement
	{
		// Token: 0x06000986 RID: 2438 RVA: 0x00045964 File Offset: 0x00044964
		public SoapEnvelopeProcessingElement()
		{
			this.properties.Add(this.readTimeout);
			this.properties.Add(this.strict);
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x000459EF File Offset: 0x000449EF
		public SoapEnvelopeProcessingElement(int readTimeout)
			: this()
		{
			this.ReadTimeout = readTimeout;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x000459FE File Offset: 0x000449FE
		public SoapEnvelopeProcessingElement(int readTimeout, bool strict)
			: this()
		{
			this.ReadTimeout = readTimeout;
			this.IsStrict = strict;
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x00045A14 File Offset: 0x00044A14
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x00045A27 File Offset: 0x00044A27
		[ConfigurationProperty("readTimeout", DefaultValue = 2147483647)]
		[TypeConverter(typeof(InfiniteIntConverter))]
		public int ReadTimeout
		{
			get
			{
				return (int)base[this.readTimeout];
			}
			set
			{
				base[this.readTimeout] = value;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x00045A3B File Offset: 0x00044A3B
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x00045A4E File Offset: 0x00044A4E
		[ConfigurationProperty("strict", DefaultValue = false)]
		public bool IsStrict
		{
			get
			{
				return (bool)base[this.strict];
			}
			set
			{
				base[this.strict] = value;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x00045A62 File Offset: 0x00044A62
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x0400060D RID: 1549
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x0400060E RID: 1550
		private readonly ConfigurationProperty readTimeout = new ConfigurationProperty("readTimeout", typeof(int), int.MaxValue, new InfiniteIntConverter(), null, ConfigurationPropertyOptions.None);

		// Token: 0x0400060F RID: 1551
		private readonly ConfigurationProperty strict = new ConfigurationProperty("strict", typeof(bool), false);
	}
}
