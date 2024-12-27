using System;
using System.Threading;
using Microsoft.Win32;

namespace System.EnterpriseServices
{
	// Token: 0x0200008C RID: 140
	internal class CatalogSync
	{
		// Token: 0x06000351 RID: 849 RVA: 0x0000AAA4 File Offset: 0x00009AA4
		internal CatalogSync()
		{
			this._set = false;
			this._version = 0;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000AABC File Offset: 0x00009ABC
		internal void Set()
		{
			try
			{
				if (!this._set && ContextUtil.IsInTransaction)
				{
					this._set = true;
					RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Classes\\CLSID");
					this._version = (int)registryKey.GetValue("CLBVersion", 0);
				}
			}
			catch
			{
				this._set = false;
				this._version = 0;
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000AB30 File Offset: 0x00009B30
		internal void Wait()
		{
			if (this._set)
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Classes\\CLSID");
				for (;;)
				{
					int num = (int)registryKey.GetValue("CLBVersion", 0);
					if (num != this._version)
					{
						break;
					}
					Thread.Sleep(0);
				}
				this._set = false;
			}
		}

		// Token: 0x04000149 RID: 329
		private bool _set;

		// Token: 0x0400014A RID: 330
		private int _version;
	}
}
