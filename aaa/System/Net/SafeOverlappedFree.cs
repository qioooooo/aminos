using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000519 RID: 1305
	[ComVisible(false)]
	internal sealed class SafeOverlappedFree : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002837 RID: 10295 RVA: 0x000A5C09 File Offset: 0x000A4C09
		private SafeOverlappedFree()
			: base(true)
		{
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x000A5C12 File Offset: 0x000A4C12
		private SafeOverlappedFree(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x000A5C1C File Offset: 0x000A4C1C
		public static SafeOverlappedFree Alloc()
		{
			SafeOverlappedFree safeOverlappedFree = UnsafeNclNativeMethods.SafeNetHandlesSafeOverlappedFree.LocalAlloc(64, (UIntPtr)((ulong)((long)Win32.OverlappedSize)));
			if (safeOverlappedFree.IsInvalid)
			{
				safeOverlappedFree.SetHandleAsInvalid();
				throw new OutOfMemoryException();
			}
			return safeOverlappedFree;
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x000A5C54 File Offset: 0x000A4C54
		public static SafeOverlappedFree Alloc(SafeCloseSocket socketHandle)
		{
			SafeOverlappedFree safeOverlappedFree = SafeOverlappedFree.Alloc();
			safeOverlappedFree._socketHandle = socketHandle;
			return safeOverlappedFree;
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x000A5C70 File Offset: 0x000A4C70
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Close(bool resetOwner)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (resetOwner)
				{
					this._socketHandle = null;
				}
				base.Close();
			}
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x000A5CA8 File Offset: 0x000A4CA8
		protected override bool ReleaseHandle()
		{
			SafeCloseSocket socketHandle = this._socketHandle;
			if (socketHandle != null && !socketHandle.IsInvalid)
			{
				socketHandle.Dispose();
			}
			return UnsafeNclNativeMethods.SafeNetHandles.LocalFree(this.handle) == IntPtr.Zero;
		}

		// Token: 0x0400276D RID: 10093
		private const int LPTR = 64;

		// Token: 0x0400276E RID: 10094
		internal static readonly SafeOverlappedFree Zero = new SafeOverlappedFree(false);

		// Token: 0x0400276F RID: 10095
		private SafeCloseSocket _socketHandle;
	}
}
