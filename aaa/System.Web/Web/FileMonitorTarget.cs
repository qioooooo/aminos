using System;

namespace System.Web
{
	// Token: 0x0200002F RID: 47
	internal sealed class FileMonitorTarget
	{
		// Token: 0x060000EE RID: 238 RVA: 0x00005819 File Offset: 0x00004819
		internal FileMonitorTarget(FileChangeEventHandler callback, string alias)
		{
			this.Callback = callback;
			this.Alias = alias;
			this.UtcStartMonitoring = DateTime.UtcNow;
			this._refs = 1;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005841 File Offset: 0x00004841
		internal int AddRef()
		{
			this._refs++;
			return this._refs;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005857 File Offset: 0x00004857
		internal int Release()
		{
			this._refs--;
			return this._refs;
		}

		// Token: 0x04000DA9 RID: 3497
		internal readonly FileChangeEventHandler Callback;

		// Token: 0x04000DAA RID: 3498
		internal readonly string Alias;

		// Token: 0x04000DAB RID: 3499
		internal readonly DateTime UtcStartMonitoring;

		// Token: 0x04000DAC RID: 3500
		private int _refs;
	}
}
