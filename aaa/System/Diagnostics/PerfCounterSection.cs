using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020001C8 RID: 456
	internal class PerfCounterSection : ConfigurationElement
	{
		// Token: 0x06000E42 RID: 3650 RVA: 0x0002D8B8 File Offset: 0x0002C8B8
		static PerfCounterSection()
		{
			PerfCounterSection._properties.Add(PerfCounterSection._propFileMappingSize);
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x0002D8F7 File Offset: 0x0002C8F7
		[ConfigurationProperty("filemappingsize", DefaultValue = 524288)]
		public int FileMappingSize
		{
			get
			{
				return (int)base[PerfCounterSection._propFileMappingSize];
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x0002D909 File Offset: 0x0002C909
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return PerfCounterSection._properties;
			}
		}

		// Token: 0x04000EEF RID: 3823
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04000EF0 RID: 3824
		private static readonly ConfigurationProperty _propFileMappingSize = new ConfigurationProperty("filemappingsize", typeof(int), 524288, ConfigurationPropertyOptions.None);
	}
}
