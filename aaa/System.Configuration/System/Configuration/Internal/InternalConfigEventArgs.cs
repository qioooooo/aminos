using System;

namespace System.Configuration.Internal
{
	// Token: 0x020000C0 RID: 192
	public sealed class InternalConfigEventArgs : EventArgs
	{
		// Token: 0x0600071E RID: 1822 RVA: 0x0001F8F4 File Offset: 0x0001E8F4
		public InternalConfigEventArgs(string configPath)
		{
			this._configPath = configPath;
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0001F903 File Offset: 0x0001E903
		// (set) Token: 0x06000720 RID: 1824 RVA: 0x0001F90B File Offset: 0x0001E90B
		public string ConfigPath
		{
			get
			{
				return this._configPath;
			}
			set
			{
				this._configPath = value;
			}
		}

		// Token: 0x04000420 RID: 1056
		private string _configPath;
	}
}
