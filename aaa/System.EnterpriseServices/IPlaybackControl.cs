using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000010 RID: 16
	[Guid("51372AFD-CAE7-11CF-BE81-00AA00A2FA25")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IPlaybackControl
	{
		// Token: 0x0600003B RID: 59
		void FinalClientRetry();

		// Token: 0x0600003C RID: 60
		void FinalServerRetry();
	}
}
