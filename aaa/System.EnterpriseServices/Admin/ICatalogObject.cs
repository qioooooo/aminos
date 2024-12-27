using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.EnterpriseServices.Admin
{
	// Token: 0x0200005C RID: 92
	[Guid("6EB22871-8A19-11D0-81B6-00A0C9231C29")]
	[SuppressUnmanagedCodeSecurity]
	[ComImport]
	internal interface ICatalogObject
	{
		// Token: 0x060001FF RID: 511
		[DispId(1)]
		object GetValue([MarshalAs(UnmanagedType.BStr)] [In] string propName);

		// Token: 0x06000200 RID: 512
		[DispId(1)]
		void SetValue([MarshalAs(UnmanagedType.BStr)] [In] string propName, [In] object value);

		// Token: 0x06000201 RID: 513
		[DispId(2)]
		object Key();

		// Token: 0x06000202 RID: 514
		[DispId(3)]
		object Name();

		// Token: 0x06000203 RID: 515
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.VariantBool)]
		bool IsPropertyReadOnly([MarshalAs(UnmanagedType.BStr)] [In] string bstrPropName);

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000204 RID: 516
		bool Valid
		{
			[DispId(5)]
			[return: MarshalAs(UnmanagedType.VariantBool)]
			get;
		}

		// Token: 0x06000205 RID: 517
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.VariantBool)]
		bool IsPropertyWriteOnly([MarshalAs(UnmanagedType.BStr)] [In] string bstrPropName);
	}
}
