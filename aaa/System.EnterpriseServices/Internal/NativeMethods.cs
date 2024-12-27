using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000DC RID: 220
	internal sealed class NativeMethods
	{
		// Token: 0x06000513 RID: 1299 RVA: 0x00011B30 File Offset: 0x00010B30
		private NativeMethods()
		{
		}

		// Token: 0x06000514 RID: 1300
		[DllImport("KERNEL32", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CloseHandle(IntPtr Handle);

		// Token: 0x06000515 RID: 1301
		[DllImport("ADVAPI32", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool OpenThreadToken(IntPtr ThreadHandle, uint DesiredAccess, bool OpenAsSelf, ref IntPtr TokenHandle);

		// Token: 0x06000516 RID: 1302
		[DllImport("ADVAPI32", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetThreadToken(IntPtr Thread, IntPtr Token);

		// Token: 0x06000517 RID: 1303
		[DllImport("Kernel32", CharSet = CharSet.Auto)]
		internal static extern IntPtr GetCurrentThread();
	}
}
