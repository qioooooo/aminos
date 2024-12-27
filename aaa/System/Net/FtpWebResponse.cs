using System;
using System.IO;

namespace System.Net
{
	// Token: 0x020003C3 RID: 963
	public class FtpWebResponse : WebResponse, IDisposable
	{
		// Token: 0x06001E4A RID: 7754 RVA: 0x000742E0 File Offset: 0x000732E0
		internal FtpWebResponse(Stream responseStream, long contentLength, Uri responseUri, FtpStatusCode statusCode, string statusLine, DateTime lastModified, string bannerMessage, string welcomeMessage, string exitMessage)
		{
			this.m_ResponseStream = responseStream;
			if (responseStream == null && contentLength < 0L)
			{
				contentLength = 0L;
			}
			this.m_ContentLength = contentLength;
			this.m_ResponseUri = responseUri;
			this.m_StatusCode = statusCode;
			this.m_StatusLine = statusLine;
			this.m_LastModified = lastModified;
			this.m_BannerMessage = bannerMessage;
			this.m_WelcomeMessage = welcomeMessage;
			this.m_ExitMessage = exitMessage;
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x00074344 File Offset: 0x00073344
		internal FtpWebResponse(HttpWebResponse httpWebResponse)
		{
			this.m_HttpWebResponse = httpWebResponse;
			base.InternalSetFromCache = this.m_HttpWebResponse.IsFromCache;
			base.InternalSetIsCacheFresh = this.m_HttpWebResponse.IsCacheFresh;
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x00074375 File Offset: 0x00073375
		internal void UpdateStatus(FtpStatusCode statusCode, string statusLine, string exitMessage)
		{
			this.m_StatusCode = statusCode;
			this.m_StatusLine = statusLine;
			this.m_ExitMessage = exitMessage;
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x0007438C File Offset: 0x0007338C
		public override Stream GetResponseStream()
		{
			Stream stream;
			if (this.HttpProxyMode)
			{
				stream = this.m_HttpWebResponse.GetResponseStream();
			}
			else if (this.m_ResponseStream != null)
			{
				stream = this.m_ResponseStream;
			}
			else
			{
				stream = (this.m_ResponseStream = new FtpWebResponse.EmptyStream());
			}
			return stream;
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x000743D2 File Offset: 0x000733D2
		internal void SetResponseStream(Stream stream)
		{
			if (stream == null || stream == Stream.Null || stream is FtpWebResponse.EmptyStream)
			{
				return;
			}
			this.m_ResponseStream = stream;
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x000743F0 File Offset: 0x000733F0
		public override void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Close", "");
			}
			if (this.HttpProxyMode)
			{
				this.m_HttpWebResponse.Close();
			}
			else
			{
				Stream responseStream = this.m_ResponseStream;
				if (responseStream != null)
				{
					responseStream.Close();
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "Close", "");
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001E50 RID: 7760 RVA: 0x0007445A File Offset: 0x0007345A
		public override long ContentLength
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.ContentLength;
				}
				return this.m_ContentLength;
			}
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x00074476 File Offset: 0x00073476
		internal void SetContentLength(long value)
		{
			if (this.HttpProxyMode)
			{
				return;
			}
			this.m_ContentLength = value;
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001E52 RID: 7762 RVA: 0x00074488 File Offset: 0x00073488
		public override WebHeaderCollection Headers
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.Headers;
				}
				if (this.m_FtpRequestHeaders == null)
				{
					lock (this)
					{
						if (this.m_FtpRequestHeaders == null)
						{
							this.m_FtpRequestHeaders = new WebHeaderCollection(WebHeaderCollectionType.FtpWebResponse);
						}
					}
				}
				return this.m_FtpRequestHeaders;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001E53 RID: 7763 RVA: 0x000744EC File Offset: 0x000734EC
		public override Uri ResponseUri
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.ResponseUri;
				}
				return this.m_ResponseUri;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001E54 RID: 7764 RVA: 0x00074508 File Offset: 0x00073508
		public FtpStatusCode StatusCode
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return (FtpStatusCode)this.m_HttpWebResponse.StatusCode;
				}
				return this.m_StatusCode;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001E55 RID: 7765 RVA: 0x00074524 File Offset: 0x00073524
		public string StatusDescription
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.StatusDescription;
				}
				return this.m_StatusLine;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001E56 RID: 7766 RVA: 0x00074540 File Offset: 0x00073540
		public DateTime LastModified
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.LastModified;
				}
				return this.m_LastModified;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001E57 RID: 7767 RVA: 0x0007455C File Offset: 0x0007355C
		public string BannerMessage
		{
			get
			{
				return this.m_BannerMessage;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001E58 RID: 7768 RVA: 0x00074564 File Offset: 0x00073564
		public string WelcomeMessage
		{
			get
			{
				return this.m_WelcomeMessage;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001E59 RID: 7769 RVA: 0x0007456C File Offset: 0x0007356C
		public string ExitMessage
		{
			get
			{
				return this.m_ExitMessage;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001E5A RID: 7770 RVA: 0x00074574 File Offset: 0x00073574
		private bool HttpProxyMode
		{
			get
			{
				return this.m_HttpWebResponse != null;
			}
		}

		// Token: 0x04001E3A RID: 7738
		internal Stream m_ResponseStream;

		// Token: 0x04001E3B RID: 7739
		private long m_ContentLength;

		// Token: 0x04001E3C RID: 7740
		private Uri m_ResponseUri;

		// Token: 0x04001E3D RID: 7741
		private FtpStatusCode m_StatusCode;

		// Token: 0x04001E3E RID: 7742
		private string m_StatusLine;

		// Token: 0x04001E3F RID: 7743
		private WebHeaderCollection m_FtpRequestHeaders;

		// Token: 0x04001E40 RID: 7744
		private HttpWebResponse m_HttpWebResponse;

		// Token: 0x04001E41 RID: 7745
		private DateTime m_LastModified;

		// Token: 0x04001E42 RID: 7746
		private string m_BannerMessage;

		// Token: 0x04001E43 RID: 7747
		private string m_WelcomeMessage;

		// Token: 0x04001E44 RID: 7748
		private string m_ExitMessage;

		// Token: 0x020003C4 RID: 964
		internal class EmptyStream : MemoryStream
		{
			// Token: 0x06001E5B RID: 7771 RVA: 0x00074582 File Offset: 0x00073582
			internal EmptyStream()
				: base(new byte[0], false)
			{
			}
		}
	}
}
