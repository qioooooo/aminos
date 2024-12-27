using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000C0 RID: 192
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class NegotiateCallerNameResponse
	{
		// Token: 0x040004E3 RID: 1251
		public int messageType;

		// Token: 0x040004E4 RID: 1252
		public string callerName;
	}
}
