using System;

namespace System.Runtime.Remoting
{
	// Token: 0x0200074C RID: 1868
	internal class RemoteAppEntry
	{
		// Token: 0x060042ED RID: 17133 RVA: 0x000E57E4 File Offset: 0x000E47E4
		internal RemoteAppEntry(string appName, string appURI)
		{
			this._remoteAppName = appName;
			this._remoteAppURI = appURI;
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x000E57FA File Offset: 0x000E47FA
		internal string GetAppURI()
		{
			return this._remoteAppURI;
		}

		// Token: 0x0400217E RID: 8574
		private string _remoteAppName;

		// Token: 0x0400217F RID: 8575
		private string _remoteAppURI;
	}
}
