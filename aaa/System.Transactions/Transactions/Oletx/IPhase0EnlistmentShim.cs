using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Transactions.Oletx
{
	// Token: 0x02000081 RID: 129
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("55FF6514-948A-4307-A692-73B84E2AF53E")]
	[SuppressUnmanagedCodeSecurity]
	[ComImport]
	internal interface IPhase0EnlistmentShim
	{
		// Token: 0x06000351 RID: 849
		void Unenlist();

		// Token: 0x06000352 RID: 850
		void Phase0Done([MarshalAs(UnmanagedType.Bool)] bool voteYes);
	}
}
