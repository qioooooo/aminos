using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020001B6 RID: 438
	internal class AssertSection : ConfigurationElement
	{
		// Token: 0x06000D60 RID: 3424 RVA: 0x0002AD80 File Offset: 0x00029D80
		static AssertSection()
		{
			AssertSection._properties.Add(AssertSection._propAssertUIEnabled);
			AssertSection._properties.Add(AssertSection._propLogFile);
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000D61 RID: 3425 RVA: 0x0002ADF4 File Offset: 0x00029DF4
		[ConfigurationProperty("assertuienabled", DefaultValue = true)]
		public bool AssertUIEnabled
		{
			get
			{
				return (bool)base[AssertSection._propAssertUIEnabled];
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x0002AE06 File Offset: 0x00029E06
		[ConfigurationProperty("logfilename", DefaultValue = "")]
		public string LogFileName
		{
			get
			{
				return (string)base[AssertSection._propLogFile];
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000D63 RID: 3427 RVA: 0x0002AE18 File Offset: 0x00029E18
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return AssertSection._properties;
			}
		}

		// Token: 0x04000EBD RID: 3773
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04000EBE RID: 3774
		private static readonly ConfigurationProperty _propAssertUIEnabled = new ConfigurationProperty("assertuienabled", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04000EBF RID: 3775
		private static readonly ConfigurationProperty _propLogFile = new ConfigurationProperty("logfilename", typeof(string), string.Empty, ConfigurationPropertyOptions.None);
	}
}
