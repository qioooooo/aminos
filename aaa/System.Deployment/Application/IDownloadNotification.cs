using System;

namespace System.Deployment.Application
{
	// Token: 0x02000039 RID: 57
	internal interface IDownloadNotification
	{
		// Token: 0x060001CD RID: 461
		void DownloadModified(object sender, DownloadEventArgs e);

		// Token: 0x060001CE RID: 462
		void DownloadCompleted(object sender, DownloadEventArgs e);
	}
}
