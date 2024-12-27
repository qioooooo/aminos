using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000468 RID: 1128
	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	public sealed class SafeWaitHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002D62 RID: 11618 RVA: 0x00098B97 File Offset: 0x00097B97
		private SafeWaitHandle()
			: base(true)
		{
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x00098BA0 File Offset: 0x00097BA0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public SafeWaitHandle(IntPtr existingHandle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(existingHandle);
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x00098BB0 File Offset: 0x00097BB0
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}
	}
}
