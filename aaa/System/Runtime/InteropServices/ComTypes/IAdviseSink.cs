using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200026F RID: 623
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000010F-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IAdviseSink
	{
		// Token: 0x06001588 RID: 5512
		[PreserveSig]
		void OnDataChange([In] ref FORMATETC format, [In] ref STGMEDIUM stgmedium);

		// Token: 0x06001589 RID: 5513
		[PreserveSig]
		void OnViewChange(int aspect, int index);

		// Token: 0x0600158A RID: 5514
		[PreserveSig]
		void OnRename(IMoniker moniker);

		// Token: 0x0600158B RID: 5515
		[PreserveSig]
		void OnSave();

		// Token: 0x0600158C RID: 5516
		[PreserveSig]
		void OnClose();
	}
}
