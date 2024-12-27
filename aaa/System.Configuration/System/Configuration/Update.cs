using System;

namespace System.Configuration
{
	// Token: 0x02000058 RID: 88
	internal abstract class Update
	{
		// Token: 0x06000384 RID: 900 RVA: 0x000127C3 File Offset: 0x000117C3
		internal Update(string configKey, bool moved, string updatedXml)
		{
			this._configKey = configKey;
			this._moved = moved;
			this._updatedXml = updatedXml;
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000385 RID: 901 RVA: 0x000127E0 File Offset: 0x000117E0
		internal string ConfigKey
		{
			get
			{
				return this._configKey;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000386 RID: 902 RVA: 0x000127E8 File Offset: 0x000117E8
		internal bool Moved
		{
			get
			{
				return this._moved;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000387 RID: 903 RVA: 0x000127F0 File Offset: 0x000117F0
		internal string UpdatedXml
		{
			get
			{
				return this._updatedXml;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000388 RID: 904 RVA: 0x000127F8 File Offset: 0x000117F8
		// (set) Token: 0x06000389 RID: 905 RVA: 0x00012800 File Offset: 0x00011800
		internal bool Retrieved
		{
			get
			{
				return this._retrieved;
			}
			set
			{
				this._retrieved = value;
			}
		}

		// Token: 0x040002DF RID: 735
		private bool _moved;

		// Token: 0x040002E0 RID: 736
		private bool _retrieved;

		// Token: 0x040002E1 RID: 737
		private string _configKey;

		// Token: 0x040002E2 RID: 738
		private string _updatedXml;
	}
}
