using System;
using System.ComponentModel;

namespace System.Deployment.Application
{
	// Token: 0x0200000D RID: 13
	public class DownloadFileGroupCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000070 RID: 112 RVA: 0x00004D53 File Offset: 0x00003D53
		internal DownloadFileGroupCompletedEventArgs(Exception error, bool cancelled, object userState, string groupName)
			: base(error, cancelled, userState)
		{
			this._groupName = groupName;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00004D66 File Offset: 0x00003D66
		public string Group
		{
			get
			{
				return this._groupName;
			}
		}

		// Token: 0x04000047 RID: 71
		private readonly string _groupName;
	}
}
