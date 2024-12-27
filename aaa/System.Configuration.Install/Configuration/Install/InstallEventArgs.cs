using System;
using System.Collections;

namespace System.Configuration.Install
{
	// Token: 0x02000013 RID: 19
	public class InstallEventArgs : EventArgs
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00004226 File Offset: 0x00003226
		public InstallEventArgs()
		{
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000422E File Offset: 0x0000322E
		public InstallEventArgs(IDictionary savedState)
		{
			this.savedState = savedState;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000423D File Offset: 0x0000323D
		public IDictionary SavedState
		{
			get
			{
				return this.savedState;
			}
		}

		// Token: 0x040000F3 RID: 243
		private IDictionary savedState;
	}
}
