using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Install
{
	// Token: 0x0200000F RID: 15
	[Guid("1E233FE7-C16D-4512-8C3B-2E9988F08D38")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IManagedInstaller
	{
		// Token: 0x06000062 RID: 98
		[return: MarshalAs(UnmanagedType.I4)]
		int ManagedInstall([MarshalAs(UnmanagedType.BStr)] [In] string commandLine, [MarshalAs(UnmanagedType.I4)] [In] int hInstall);
	}
}
