using System;
using Microsoft.Win32.SafeHandles;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x0200005E RID: 94
	internal class PipeHandle : CriticalHandleMinusOneIsInvalid
	{
		// Token: 0x060002FF RID: 767 RVA: 0x0000E78C File Offset: 0x0000D78C
		internal PipeHandle()
		{
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000E794 File Offset: 0x0000D794
		internal PipeHandle(IntPtr handle)
		{
			base.SetHandle(handle);
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000E7A3 File Offset: 0x0000D7A3
		public IntPtr Handle
		{
			get
			{
				return this.handle;
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000E7AB File Offset: 0x0000D7AB
		protected override bool ReleaseHandle()
		{
			return NativePipe.CloseHandle(this.handle) != 0;
		}
	}
}
