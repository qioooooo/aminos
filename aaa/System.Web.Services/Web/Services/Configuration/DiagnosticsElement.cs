using System;
using System.Configuration;

namespace System.Web.Services.Configuration
{
	// Token: 0x02000134 RID: 308
	public sealed class DiagnosticsElement : ConfigurationElement
	{
		// Token: 0x06000982 RID: 2434 RVA: 0x000458E4 File Offset: 0x000448E4
		public DiagnosticsElement()
		{
			this.properties.Add(this.suppressReturningExceptions);
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x00045933 File Offset: 0x00044933
		// (set) Token: 0x06000984 RID: 2436 RVA: 0x00045946 File Offset: 0x00044946
		[ConfigurationProperty("suppressReturningExceptions", DefaultValue = false)]
		public bool SuppressReturningExceptions
		{
			get
			{
				return (bool)base[this.suppressReturningExceptions];
			}
			set
			{
				base[this.suppressReturningExceptions] = value;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0004595A File Offset: 0x0004495A
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x0400060B RID: 1547
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x0400060C RID: 1548
		private readonly ConfigurationProperty suppressReturningExceptions = new ConfigurationProperty("suppressReturningExceptions", typeof(bool), false);
	}
}
