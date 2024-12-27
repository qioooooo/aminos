using System;

namespace System.Configuration
{
	// Token: 0x0200002E RID: 46
	public class ConfigurationLocation
	{
		// Token: 0x06000254 RID: 596 RVA: 0x0000F619 File Offset: 0x0000E619
		internal ConfigurationLocation(Configuration config, string locationSubPath)
		{
			this._config = config;
			this._locationSubPath = locationSubPath;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000F62F File Offset: 0x0000E62F
		public string Path
		{
			get
			{
				return this._locationSubPath;
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000F637 File Offset: 0x0000E637
		public Configuration OpenConfiguration()
		{
			return this._config.OpenLocationConfiguration(this._locationSubPath);
		}

		// Token: 0x0400025B RID: 603
		private Configuration _config;

		// Token: 0x0400025C RID: 604
		private string _locationSubPath;
	}
}
