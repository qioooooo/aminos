using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000043 RID: 67
	[Guid("4E31107F-8E81-11d1-9DCE-00C04FC2FBA2")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ITxStreamInternal
	{
		// Token: 0x0600013B RID: 315
		void GetTransaction(out ITransaction ptx);

		// Token: 0x0600013C RID: 316
		[PreserveSig]
		Guid GetGuid();

		// Token: 0x0600013D RID: 317
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool TxIsDoomed();
	}
}
