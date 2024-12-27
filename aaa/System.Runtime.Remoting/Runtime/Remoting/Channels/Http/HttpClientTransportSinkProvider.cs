using System;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000029 RID: 41
	internal class HttpClientTransportSinkProvider : IClientChannelSinkProvider
	{
		// Token: 0x0600013A RID: 314 RVA: 0x00006782 File Offset: 0x00005782
		internal HttpClientTransportSinkProvider(int timeout)
		{
			this._timeout = timeout;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006794 File Offset: 0x00005794
		public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
		{
			HttpClientTransportSink httpClientTransportSink = new HttpClientTransportSink((HttpClientChannel)channel, url);
			httpClientTransportSink["timeout"] = this._timeout;
			return httpClientTransportSink;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600013C RID: 316 RVA: 0x000067C5 File Offset: 0x000057C5
		// (set) Token: 0x0600013D RID: 317 RVA: 0x000067C8 File Offset: 0x000057C8
		public IClientChannelSinkProvider Next
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x040000E8 RID: 232
		private int _timeout;
	}
}
