using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000270 RID: 624
	[Guid("0000010E-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IDataObject
	{
		// Token: 0x0600158D RID: 5517
		void GetData([In] ref FORMATETC format, out STGMEDIUM medium);

		// Token: 0x0600158E RID: 5518
		void GetDataHere([In] ref FORMATETC format, ref STGMEDIUM medium);

		// Token: 0x0600158F RID: 5519
		[PreserveSig]
		int QueryGetData([In] ref FORMATETC format);

		// Token: 0x06001590 RID: 5520
		[PreserveSig]
		int GetCanonicalFormatEtc([In] ref FORMATETC formatIn, out FORMATETC formatOut);

		// Token: 0x06001591 RID: 5521
		void SetData([In] ref FORMATETC formatIn, [In] ref STGMEDIUM medium, [MarshalAs(UnmanagedType.Bool)] bool release);

		// Token: 0x06001592 RID: 5522
		IEnumFORMATETC EnumFormatEtc(DATADIR direction);

		// Token: 0x06001593 RID: 5523
		[PreserveSig]
		int DAdvise([In] ref FORMATETC pFormatetc, ADVF advf, IAdviseSink adviseSink, out int connection);

		// Token: 0x06001594 RID: 5524
		void DUnadvise(int connection);

		// Token: 0x06001595 RID: 5525
		[PreserveSig]
		int EnumDAdvise(out IEnumSTATDATA enumAdvise);
	}
}
