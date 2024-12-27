using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000072 RID: 114
	// (Invoke) Token: 0x06000248 RID: 584
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate bool QUERYCLIENTCERT(IntPtr Connection, IntPtr trusted_CAs, ref IntPtr certificateHandle);
}
