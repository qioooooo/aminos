using System;
using System.Data.Common;

namespace System.Data.SqlClient
{
	// Token: 0x020002CE RID: 718
	internal sealed class SqlDebugContext : IDisposable
	{
		// Token: 0x060024F2 RID: 9458 RVA: 0x00279EA0 File Offset: 0x002792A0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x00279EBC File Offset: 0x002792BC
		private void Dispose(bool disposing)
		{
			if (this.pMemMap != IntPtr.Zero)
			{
				NativeMethods.UnmapViewOfFile(this.pMemMap);
				this.pMemMap = IntPtr.Zero;
			}
			if (this.hMemMap != IntPtr.Zero)
			{
				NativeMethods.CloseHandle(this.hMemMap);
				this.hMemMap = IntPtr.Zero;
			}
			this.active = false;
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x00279F24 File Offset: 0x00279324
		~SqlDebugContext()
		{
			this.Dispose(false);
		}

		// Token: 0x04001770 RID: 6000
		internal uint pid;

		// Token: 0x04001771 RID: 6001
		internal uint tid;

		// Token: 0x04001772 RID: 6002
		internal bool active;

		// Token: 0x04001773 RID: 6003
		internal IntPtr pMemMap = ADP.PtrZero;

		// Token: 0x04001774 RID: 6004
		internal IntPtr hMemMap = ADP.PtrZero;

		// Token: 0x04001775 RID: 6005
		internal uint dbgpid;

		// Token: 0x04001776 RID: 6006
		internal bool fOption;

		// Token: 0x04001777 RID: 6007
		internal string machineName;

		// Token: 0x04001778 RID: 6008
		internal string sdiDllName;

		// Token: 0x04001779 RID: 6009
		internal byte[] data;
	}
}
