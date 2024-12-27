using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000A8 RID: 168
	[Guid("70C8E441-C7ED-11D1-82FB-00A0C91EEDE9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface _IMonitorLogRecords
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060003F8 RID: 1016
		int Count { get; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060003F9 RID: 1017
		TransactionState TransactionState { get; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060003FA RID: 1018
		bool StructuredRecords
		{
			[return: MarshalAs(UnmanagedType.VariantBool)]
			get;
		}

		// Token: 0x060003FB RID: 1019
		void GetLogRecord([In] int dwIndex, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] ref _LogRecord pRecord);

		// Token: 0x060003FC RID: 1020
		object GetLogRecordVariants([In] object IndexNumber);
	}
}
