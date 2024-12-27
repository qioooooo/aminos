using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000CD RID: 205
	[InterfaceType(1)]
	[Guid("1BE41571-91DD-11D1-AEB2-00C04FB68820")]
	[ComImport]
	internal interface IWbemProviderInitSink
	{
		// Token: 0x0600061C RID: 1564
		[PreserveSig]
		int SetStatus_([In] int lStatus, [In] int lFlags);
	}
}
