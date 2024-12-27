using System;
using System.Runtime.InteropServices;
using System.Web.Configuration;

namespace System.Web
{
	// Token: 0x020000AF RID: 175
	[ComVisible(false)]
	internal sealed class NativeMethods
	{
		// Token: 0x0600088A RID: 2186 RVA: 0x000264B3 File Offset: 0x000254B3
		private NativeMethods()
		{
		}

		// Token: 0x0600088B RID: 2187
		[DllImport("Fusion.dll", CharSet = CharSet.Auto)]
		internal static extern int CreateAssemblyCache(out IAssemblyCache ppAsmCache, uint dwReserved);
	}
}
