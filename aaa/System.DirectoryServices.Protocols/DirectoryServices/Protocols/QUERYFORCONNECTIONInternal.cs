using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000082 RID: 130
	// (Invoke) Token: 0x060002B7 RID: 695
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int QUERYFORCONNECTIONInternal(IntPtr Connection, IntPtr ReferralFromConnection, IntPtr NewDNPtr, string HostName, int PortNumber, SEC_WINNT_AUTH_IDENTITY_EX SecAuthIdentity, Luid CurrentUserToken, ref IntPtr ConnectionToUse);
}
