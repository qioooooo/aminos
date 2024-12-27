using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000084 RID: 132
	// (Invoke) Token: 0x060002BF RID: 703
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int DEREFERENCECONNECTIONInternal(IntPtr Connection, IntPtr ConnectionToDereference);
}
