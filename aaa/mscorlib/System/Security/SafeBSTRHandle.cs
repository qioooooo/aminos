using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Security
{
	// Token: 0x02000674 RID: 1652
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeBSTRHandle : SafePointer
	{
		// Token: 0x06003C2C RID: 15404 RVA: 0x000CEAF8 File Offset: 0x000CDAF8
		internal SafeBSTRHandle()
			: base(true)
		{
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x000CEB04 File Offset: 0x000CDB04
		internal static SafeBSTRHandle Allocate(string src, uint len)
		{
			SafeBSTRHandle safeBSTRHandle = SafeBSTRHandle.SysAllocStringLen(src, len);
			safeBSTRHandle.Initialize((ulong)(len * 2U));
			return safeBSTRHandle;
		}

		// Token: 0x06003C2E RID: 15406
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode)]
		private static extern SafeBSTRHandle SysAllocStringLen(string src, uint len);

		// Token: 0x06003C2F RID: 15407 RVA: 0x000CEB24 File Offset: 0x000CDB24
		protected override bool ReleaseHandle()
		{
			Win32Native.ZeroMemory(this.handle, (uint)(Win32Native.SysStringLen(this.handle) * 2));
			Win32Native.SysFreeString(this.handle);
			return true;
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x000CEB4C File Offset: 0x000CDB4C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal unsafe void ClearBuffer()
		{
			byte* ptr = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.AcquirePointer(ref ptr);
				Win32Native.ZeroMemory((IntPtr)((void*)ptr), (uint)(Win32Native.SysStringLen((IntPtr)((void*)ptr)) * 2));
			}
			finally
			{
				if (ptr != null)
				{
					base.ReleasePointer();
				}
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x06003C31 RID: 15409 RVA: 0x000CEBA0 File Offset: 0x000CDBA0
		internal unsafe int Length
		{
			get
			{
				byte* ptr = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				int num;
				try
				{
					base.AcquirePointer(ref ptr);
					num = Win32Native.SysStringLen((IntPtr)((void*)ptr));
				}
				finally
				{
					if (ptr != null)
					{
						base.ReleasePointer();
					}
				}
				return num;
			}
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x000CEBE8 File Offset: 0x000CDBE8
		internal unsafe static void Copy(SafeBSTRHandle source, SafeBSTRHandle target)
		{
			byte* ptr = null;
			byte* ptr2 = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				source.AcquirePointer(ref ptr);
				target.AcquirePointer(ref ptr2);
				Buffer.memcpyimpl(ptr, ptr2, Win32Native.SysStringLen((IntPtr)((void*)ptr)) * 2);
			}
			finally
			{
				if (ptr != null)
				{
					source.ReleasePointer();
				}
				if (ptr2 != null)
				{
					target.ReleasePointer();
				}
			}
		}
	}
}
