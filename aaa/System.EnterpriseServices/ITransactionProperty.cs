using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000044 RID: 68
	[Guid("788ea814-87b1-11d1-bba6-00c04fc2fa5f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ITransactionProperty
	{
		// Token: 0x0600013E RID: 318
		[PreserveSig]
		void SetConsistent(bool fConsistent);

		// Token: 0x0600013F RID: 319
		void GetTransaction(out ITransaction ptx);

		// Token: 0x06000140 RID: 320
		[PreserveSig]
		void GetTxStream(out ITxStreamInternal ptsi);

		// Token: 0x06000141 RID: 321
		[PreserveSig]
		Guid GetTxStreamGuid();

		// Token: 0x06000142 RID: 322
		[PreserveSig]
		int GetTxStreamMarshalSize();

		// Token: 0x06000143 RID: 323
		[PreserveSig]
		int GetTxStreamMarshalBuffer();

		// Token: 0x06000144 RID: 324
		[PreserveSig]
		short GetUnmarshalVariant();

		// Token: 0x06000145 RID: 325
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool NeedEnvoy();

		// Token: 0x06000146 RID: 326
		[PreserveSig]
		short GetRootDtcCapabilities();

		// Token: 0x06000147 RID: 327
		[PreserveSig]
		int GetTransactionResourcePool(out ITransactionResourcePool pool);

		// Token: 0x06000148 RID: 328
		void GetTransactionId(ref Guid guid);

		// Token: 0x06000149 RID: 329
		object GetClassInfo();

		// Token: 0x0600014A RID: 330
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsRoot();
	}
}
