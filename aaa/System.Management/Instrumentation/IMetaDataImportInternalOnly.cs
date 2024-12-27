using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Management.Instrumentation
{
	// Token: 0x020000B4 RID: 180
	[Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibType(TypeLibTypeFlags.FRestricted)]
	[ComImport]
	internal interface IMetaDataImportInternalOnly
	{
		// Token: 0x06000559 RID: 1369
		void f1();

		// Token: 0x0600055A RID: 1370
		void f2();

		// Token: 0x0600055B RID: 1371
		void f3();

		// Token: 0x0600055C RID: 1372
		void f4();

		// Token: 0x0600055D RID: 1373
		void f5();

		// Token: 0x0600055E RID: 1374
		void f6();

		// Token: 0x0600055F RID: 1375
		void f7();

		// Token: 0x06000560 RID: 1376
		void GetScopeProps([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder szName, [In] uint cchName, out uint pchName, out Guid pmvid);
	}
}
