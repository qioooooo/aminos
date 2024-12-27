using System;
using System.Configuration;

namespace System.Data
{
	// Token: 0x0200033F RID: 831
	internal sealed class LocalDBConfigurationSection : ConfigurationSection
	{
		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002B48 RID: 11080 RVA: 0x002A1A58 File Offset: 0x002A0E58
		[ConfigurationProperty("localdbinstances", IsRequired = true)]
		public LocalDBInstancesCollection LocalDbInstances
		{
			get
			{
				return ((LocalDBInstancesCollection)base["localdbinstances"]) ?? new LocalDBInstancesCollection();
			}
		}
	}
}
