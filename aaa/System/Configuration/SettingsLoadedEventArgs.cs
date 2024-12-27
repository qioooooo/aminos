using System;

namespace System.Configuration
{
	// Token: 0x020006E6 RID: 1766
	public class SettingsLoadedEventArgs : EventArgs
	{
		// Token: 0x06003687 RID: 13959 RVA: 0x000E8C3B File Offset: 0x000E7C3B
		public SettingsLoadedEventArgs(SettingsProvider provider)
		{
			this._provider = provider;
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06003688 RID: 13960 RVA: 0x000E8C4A File Offset: 0x000E7C4A
		public SettingsProvider Provider
		{
			get
			{
				return this._provider;
			}
		}

		// Token: 0x04003187 RID: 12679
		private SettingsProvider _provider;
	}
}
