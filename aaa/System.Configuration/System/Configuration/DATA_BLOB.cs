using System;
using System.Runtime.InteropServices;

namespace System.Configuration
{
	// Token: 0x02000056 RID: 86
	internal struct DATA_BLOB : IDisposable
	{
		// Token: 0x06000373 RID: 883 RVA: 0x00012777 File Offset: 0x00011777
		void IDisposable.Dispose()
		{
			if (this.pbData != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.pbData);
				this.pbData = IntPtr.Zero;
			}
		}

		// Token: 0x040002D8 RID: 728
		public int cbData;

		// Token: 0x040002D9 RID: 729
		public IntPtr pbData;
	}
}
