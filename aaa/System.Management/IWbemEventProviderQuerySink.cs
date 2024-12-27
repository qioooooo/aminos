using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200009B RID: 155
	[TypeLibType(512)]
	[Guid("580ACAF8-FA1C-11D0-AD72-00C04FD8FDFF")]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemEventProviderQuerySink
	{
		// Token: 0x06000473 RID: 1139
		[PreserveSig]
		int NewQuery_([In] uint dwId, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszQueryLanguage, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszQuery);

		// Token: 0x06000474 RID: 1140
		[PreserveSig]
		int CancelQuery_([In] uint dwId);
	}
}
