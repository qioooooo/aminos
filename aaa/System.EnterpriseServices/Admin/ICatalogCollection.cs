using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;

namespace System.EnterpriseServices.Admin
{
	// Token: 0x0200005D RID: 93
	[Guid("6EB22872-8A19-11D0-81B6-00A0C9231C29")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[SuppressUnmanagedCodeSecurity]
	[ComImport]
	internal interface ICatalogCollection
	{
		// Token: 0x06000206 RID: 518
		[DispId(-4)]
		void GetEnumerator(out IEnumerator pEnum);

		// Token: 0x06000207 RID: 519
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object Item([In] int lIndex);

		// Token: 0x06000208 RID: 520
		[DispId(1610743810)]
		int Count();

		// Token: 0x06000209 RID: 521
		[DispId(1610743811)]
		void Remove([In] int lIndex);

		// Token: 0x0600020A RID: 522
		[DispId(1610743812)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object Add();

		// Token: 0x0600020B RID: 523
		[DispId(2)]
		void Populate();

		// Token: 0x0600020C RID: 524
		[DispId(3)]
		int SaveChanges();

		// Token: 0x0600020D RID: 525
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetCollection([MarshalAs(UnmanagedType.BStr)] [In] string bstrCollName, [In] object varObjectKey);

		// Token: 0x0600020E RID: 526
		[DispId(6)]
		object Name();

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600020F RID: 527
		bool IsAddEnabled
		{
			[DispId(7)]
			[return: MarshalAs(UnmanagedType.VariantBool)]
			get;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000210 RID: 528
		bool IsRemoveEnabled
		{
			[DispId(8)]
			[return: MarshalAs(UnmanagedType.VariantBool)]
			get;
		}

		// Token: 0x06000211 RID: 529
		[DispId(9)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetUtilInterface();

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000212 RID: 530
		int DataStoreMajorVersion
		{
			[DispId(10)]
			get;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000213 RID: 531
		int DataStoreMinorVersion
		{
			[DispId(11)]
			get;
		}

		// Token: 0x06000214 RID: 532
		void PopulateByKey([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)] [In] object[] aKeys);

		// Token: 0x06000215 RID: 533
		[DispId(13)]
		void PopulateByQuery([MarshalAs(UnmanagedType.BStr)] [In] string bstrQueryString, [In] int lQueryType);
	}
}
