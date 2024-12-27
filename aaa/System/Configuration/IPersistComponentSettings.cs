using System;

namespace System.Configuration
{
	// Token: 0x020006FB RID: 1787
	public interface IPersistComponentSettings
	{
		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x0600370F RID: 14095
		// (set) Token: 0x06003710 RID: 14096
		bool SaveSettings { get; set; }

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06003711 RID: 14097
		// (set) Token: 0x06003712 RID: 14098
		string SettingsKey { get; set; }

		// Token: 0x06003713 RID: 14099
		void LoadComponentSettings();

		// Token: 0x06003714 RID: 14100
		void SaveComponentSettings();

		// Token: 0x06003715 RID: 14101
		void ResetComponentSettings();
	}
}
