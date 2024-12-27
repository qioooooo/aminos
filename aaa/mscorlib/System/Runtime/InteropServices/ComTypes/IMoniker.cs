using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000562 RID: 1378
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000000f-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IMoniker
	{
		// Token: 0x060033A5 RID: 13221
		void GetClassID(out Guid pClassID);

		// Token: 0x060033A6 RID: 13222
		[PreserveSig]
		int IsDirty();

		// Token: 0x060033A7 RID: 13223
		void Load(IStream pStm);

		// Token: 0x060033A8 RID: 13224
		void Save(IStream pStm, [MarshalAs(UnmanagedType.Bool)] bool fClearDirty);

		// Token: 0x060033A9 RID: 13225
		void GetSizeMax(out long pcbSize);

		// Token: 0x060033AA RID: 13226
		void BindToObject(IBindCtx pbc, IMoniker pmkToLeft, [In] ref Guid riidResult, [MarshalAs(UnmanagedType.Interface)] out object ppvResult);

		// Token: 0x060033AB RID: 13227
		void BindToStorage(IBindCtx pbc, IMoniker pmkToLeft, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObj);

		// Token: 0x060033AC RID: 13228
		void Reduce(IBindCtx pbc, int dwReduceHowFar, ref IMoniker ppmkToLeft, out IMoniker ppmkReduced);

		// Token: 0x060033AD RID: 13229
		void ComposeWith(IMoniker pmkRight, [MarshalAs(UnmanagedType.Bool)] bool fOnlyIfNotGeneric, out IMoniker ppmkComposite);

		// Token: 0x060033AE RID: 13230
		void Enum([MarshalAs(UnmanagedType.Bool)] bool fForward, out IEnumMoniker ppenumMoniker);

		// Token: 0x060033AF RID: 13231
		[PreserveSig]
		int IsEqual(IMoniker pmkOtherMoniker);

		// Token: 0x060033B0 RID: 13232
		void Hash(out int pdwHash);

		// Token: 0x060033B1 RID: 13233
		[PreserveSig]
		int IsRunning(IBindCtx pbc, IMoniker pmkToLeft, IMoniker pmkNewlyRunning);

		// Token: 0x060033B2 RID: 13234
		void GetTimeOfLastChange(IBindCtx pbc, IMoniker pmkToLeft, out FILETIME pFileTime);

		// Token: 0x060033B3 RID: 13235
		void Inverse(out IMoniker ppmk);

		// Token: 0x060033B4 RID: 13236
		void CommonPrefixWith(IMoniker pmkOther, out IMoniker ppmkPrefix);

		// Token: 0x060033B5 RID: 13237
		void RelativePathTo(IMoniker pmkOther, out IMoniker ppmkRelPath);

		// Token: 0x060033B6 RID: 13238
		void GetDisplayName(IBindCtx pbc, IMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplayName);

		// Token: 0x060033B7 RID: 13239
		void ParseDisplayName(IBindCtx pbc, IMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, out int pchEaten, out IMoniker ppmkOut);

		// Token: 0x060033B8 RID: 13240
		[PreserveSig]
		int IsSystemMoniker(out int pdwMksys);
	}
}
