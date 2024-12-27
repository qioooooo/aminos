using System;
using System.Runtime.InteropServices;

namespace System.Web.Configuration
{
	// Token: 0x02000213 RID: 531
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct MULTI_QI : IDisposable
	{
		// Token: 0x06001C98 RID: 7320 RVA: 0x00083091 File Offset: 0x00082091
		internal MULTI_QI(IntPtr pid)
		{
			this.piid = pid;
			this.pItf = IntPtr.Zero;
			this.hr = 0;
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x000830AC File Offset: 0x000820AC
		void IDisposable.Dispose()
		{
			if (this.pItf != IntPtr.Zero)
			{
				Marshal.Release(this.pItf);
				this.pItf = IntPtr.Zero;
			}
			if (this.piid != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.piid);
				this.piid = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x040018F3 RID: 6387
		internal IntPtr piid;

		// Token: 0x040018F4 RID: 6388
		internal IntPtr pItf;

		// Token: 0x040018F5 RID: 6389
		internal int hr;
	}
}
