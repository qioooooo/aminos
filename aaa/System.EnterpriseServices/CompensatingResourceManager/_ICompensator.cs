using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000A7 RID: 167
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("BBC01830-8D3B-11D1-82EC-00A0C91EEDE9")]
	[ComImport]
	internal interface _ICompensator
	{
		// Token: 0x060003EE RID: 1006
		void _SetLogControl(IntPtr logControl);

		// Token: 0x060003EF RID: 1007
		void _BeginPrepare();

		// Token: 0x060003F0 RID: 1008
		[return: MarshalAs(UnmanagedType.Bool)]
		bool _PrepareRecord(_LogRecord record);

		// Token: 0x060003F1 RID: 1009
		[return: MarshalAs(UnmanagedType.Bool)]
		bool _EndPrepare();

		// Token: 0x060003F2 RID: 1010
		void _BeginCommit(bool fRecovery);

		// Token: 0x060003F3 RID: 1011
		[return: MarshalAs(UnmanagedType.Bool)]
		bool _CommitRecord(_LogRecord record);

		// Token: 0x060003F4 RID: 1012
		void _EndCommit();

		// Token: 0x060003F5 RID: 1013
		void _BeginAbort(bool fRecovery);

		// Token: 0x060003F6 RID: 1014
		[return: MarshalAs(UnmanagedType.Bool)]
		bool _AbortRecord(_LogRecord record);

		// Token: 0x060003F7 RID: 1015
		void _EndAbort();
	}
}
