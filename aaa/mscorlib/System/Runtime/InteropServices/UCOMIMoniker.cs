using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200052F RID: 1327
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IMoniker instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("0000000f-0000-0000-C000-000000000046")]
	[ComImport]
	public interface UCOMIMoniker
	{
		// Token: 0x0600331D RID: 13085
		void GetClassID(out Guid pClassID);

		// Token: 0x0600331E RID: 13086
		[PreserveSig]
		int IsDirty();

		// Token: 0x0600331F RID: 13087
		void Load(UCOMIStream pStm);

		// Token: 0x06003320 RID: 13088
		void Save(UCOMIStream pStm, [MarshalAs(UnmanagedType.Bool)] bool fClearDirty);

		// Token: 0x06003321 RID: 13089
		void GetSizeMax(out long pcbSize);

		// Token: 0x06003322 RID: 13090
		void BindToObject(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, [In] ref Guid riidResult, [MarshalAs(UnmanagedType.Interface)] out object ppvResult);

		// Token: 0x06003323 RID: 13091
		void BindToStorage(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObj);

		// Token: 0x06003324 RID: 13092
		void Reduce(UCOMIBindCtx pbc, int dwReduceHowFar, ref UCOMIMoniker ppmkToLeft, out UCOMIMoniker ppmkReduced);

		// Token: 0x06003325 RID: 13093
		void ComposeWith(UCOMIMoniker pmkRight, [MarshalAs(UnmanagedType.Bool)] bool fOnlyIfNotGeneric, out UCOMIMoniker ppmkComposite);

		// Token: 0x06003326 RID: 13094
		void Enum([MarshalAs(UnmanagedType.Bool)] bool fForward, out UCOMIEnumMoniker ppenumMoniker);

		// Token: 0x06003327 RID: 13095
		void IsEqual(UCOMIMoniker pmkOtherMoniker);

		// Token: 0x06003328 RID: 13096
		void Hash(out int pdwHash);

		// Token: 0x06003329 RID: 13097
		void IsRunning(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, UCOMIMoniker pmkNewlyRunning);

		// Token: 0x0600332A RID: 13098
		void GetTimeOfLastChange(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, out FILETIME pFileTime);

		// Token: 0x0600332B RID: 13099
		void Inverse(out UCOMIMoniker ppmk);

		// Token: 0x0600332C RID: 13100
		void CommonPrefixWith(UCOMIMoniker pmkOther, out UCOMIMoniker ppmkPrefix);

		// Token: 0x0600332D RID: 13101
		void RelativePathTo(UCOMIMoniker pmkOther, out UCOMIMoniker ppmkRelPath);

		// Token: 0x0600332E RID: 13102
		void GetDisplayName(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplayName);

		// Token: 0x0600332F RID: 13103
		void ParseDisplayName(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, out int pchEaten, out UCOMIMoniker ppmkOut);

		// Token: 0x06003330 RID: 13104
		void IsSystemMoniker(out int pdwMksys);
	}
}
