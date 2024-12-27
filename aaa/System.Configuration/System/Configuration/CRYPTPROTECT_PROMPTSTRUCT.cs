using System;

namespace System.Configuration
{
	// Token: 0x02000055 RID: 85
	internal struct CRYPTPROTECT_PROMPTSTRUCT : IDisposable
	{
		// Token: 0x06000372 RID: 882 RVA: 0x0001276A File Offset: 0x0001176A
		void IDisposable.Dispose()
		{
			this.hwndApp = IntPtr.Zero;
		}

		// Token: 0x040002D4 RID: 724
		public int cbSize;

		// Token: 0x040002D5 RID: 725
		public int dwPromptFlags;

		// Token: 0x040002D6 RID: 726
		public IntPtr hwndApp;

		// Token: 0x040002D7 RID: 727
		public string szPrompt;
	}
}
