using System;
using System.Runtime.InteropServices;

namespace System.Net.Mail
{
	// Token: 0x02000690 RID: 1680
	[StructLayout(LayoutKind.Sequential)]
	internal class _METADATA_HANDLE_INFO
	{
		// Token: 0x060033DE RID: 13278 RVA: 0x000DB421 File Offset: 0x000DA421
		private _METADATA_HANDLE_INFO()
		{
			this.dwMDPermissions = 0;
			this.dwMDSystemChangeNumber = 0;
		}

		// Token: 0x04002FE3 RID: 12259
		internal int dwMDPermissions;

		// Token: 0x04002FE4 RID: 12260
		internal int dwMDSystemChangeNumber;
	}
}
