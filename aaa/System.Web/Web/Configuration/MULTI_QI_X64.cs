using System;
using System.Runtime.InteropServices;

namespace System.Web.Configuration
{
	// Token: 0x02000214 RID: 532
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct MULTI_QI_X64 : IDisposable
	{
		// Token: 0x06001C9A RID: 7322 RVA: 0x0008311A File Offset: 0x0008211A
		internal MULTI_QI_X64(IntPtr pid)
		{
			this.piid = pid;
			this.pItf = IntPtr.Zero;
			this.hr = 0;
			this.padding = 0;
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x0008313C File Offset: 0x0008213C
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

		// Token: 0x040018F6 RID: 6390
		internal IntPtr piid;

		// Token: 0x040018F7 RID: 6391
		internal IntPtr pItf;

		// Token: 0x040018F8 RID: 6392
		internal int hr;

		// Token: 0x040018F9 RID: 6393
		internal int padding;
	}
}
