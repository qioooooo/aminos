using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000085 RID: 133
	// (Invoke) Token: 0x060002C3 RID: 707
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate bool VERIFYSERVERCERT(IntPtr Connection, IntPtr pServerCert);
}
