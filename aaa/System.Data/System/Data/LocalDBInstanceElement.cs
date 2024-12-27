using System;
using System.Configuration;

namespace System.Data
{
	// Token: 0x0200033C RID: 828
	internal sealed class LocalDBInstanceElement : ConfigurationElement
	{
		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002B3F RID: 11071 RVA: 0x002A1954 File Offset: 0x002A0D54
		[ConfigurationProperty("name", IsRequired = true)]
		public string Name
		{
			get
			{
				return base["name"] as string;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06002B40 RID: 11072 RVA: 0x002A1974 File Offset: 0x002A0D74
		[ConfigurationProperty("version", IsRequired = true)]
		public string Version
		{
			get
			{
				return base["version"] as string;
			}
		}
	}
}
