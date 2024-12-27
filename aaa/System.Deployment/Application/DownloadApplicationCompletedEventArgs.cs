using System;
using System.ComponentModel;

namespace System.Deployment.Application
{
	// Token: 0x02000061 RID: 97
	public class DownloadApplicationCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060002FE RID: 766 RVA: 0x00011458 File Offset: 0x00010458
		internal DownloadApplicationCompletedEventArgs(AsyncCompletedEventArgs e, string logFilePath, string shortcutAppId)
			: base(e.Error, e.Cancelled, e.UserState)
		{
			this._logFilePath = logFilePath;
			this._shortcutAppId = shortcutAppId;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00011480 File Offset: 0x00010480
		public string LogFilePath
		{
			get
			{
				return this._logFilePath;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000300 RID: 768 RVA: 0x00011488 File Offset: 0x00010488
		public string ShortcutAppId
		{
			get
			{
				return this._shortcutAppId;
			}
		}

		// Token: 0x0400024B RID: 587
		private string _logFilePath;

		// Token: 0x0400024C RID: 588
		private string _shortcutAppId;
	}
}
