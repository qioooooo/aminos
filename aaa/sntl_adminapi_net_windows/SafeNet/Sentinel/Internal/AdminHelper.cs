using System;
using System.Runtime.InteropServices;

namespace SafeNet.Sentinel.Internal
{
	// Token: 0x02000006 RID: 6
	internal class AdminHelper
	{
		// Token: 0x06000025 RID: 37
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetDllDirectory(string pathName);
	}
}
