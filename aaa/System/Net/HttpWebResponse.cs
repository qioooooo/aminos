using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020003E4 RID: 996
	[Serializable]
	public class HttpWebResponse : WebResponse, ISerializable
	{
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x0600203E RID: 8254 RVA: 0x0007EF0E File Offset: 0x0007DF0E
		// (set) Token: 0x0600203F RID: 8255 RVA: 0x0007EF16 File Offset: 0x0007DF16
		internal Stream ResponseStream
		{
			get
			{
				return this.m_ConnectStream;
			}
			set
			{
				this.m_ConnectStream = value;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002040 RID: 8256 RVA: 0x0007EF1F File Offset: 0x0007DF1F
		public override bool IsMutuallyAuthenticated
		{
			get
			{
				return this.m_IsMutuallyAuthenticated;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (set) Token: 0x06002041 RID: 8257 RVA: 0x0007EF27 File Offset: 0x0007DF27
		internal bool InternalSetIsMutuallyAuthenticated
		{
			set
			{
				this.m_IsMutuallyAuthenticated = value;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002042 RID: 8258 RVA: 0x0007EF30 File Offset: 0x0007DF30
		// (set) Token: 0x06002043 RID: 8259 RVA: 0x0007EF51 File Offset: 0x0007DF51
		public CookieCollection Cookies
		{
			get
			{
				this.CheckDisposed();
				if (this.m_cookies == null)
				{
					this.m_cookies = new CookieCollection();
				}
				return this.m_cookies;
			}
			set
			{
				this.CheckDisposed();
				this.m_cookies = value;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002044 RID: 8260 RVA: 0x0007EF60 File Offset: 0x0007DF60
		public override WebHeaderCollection Headers
		{
			get
			{
				return this.m_HttpResponseHeaders;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002045 RID: 8261 RVA: 0x0007EF68 File Offset: 0x0007DF68
		public override long ContentLength
		{
			get
			{
				return this.m_ContentLength;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002046 RID: 8262 RVA: 0x0007EF70 File Offset: 0x0007DF70
		public string ContentEncoding
		{
			get
			{
				this.CheckDisposed();
				string text = this.m_HttpResponseHeaders["Content-Encoding"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002047 RID: 8263 RVA: 0x0007EFA0 File Offset: 0x0007DFA0
		public override string ContentType
		{
			get
			{
				this.CheckDisposed();
				string contentType = this.m_HttpResponseHeaders.ContentType;
				if (contentType != null)
				{
					return contentType;
				}
				return string.Empty;
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002048 RID: 8264 RVA: 0x0007EFCC File Offset: 0x0007DFCC
		public string CharacterSet
		{
			get
			{
				this.CheckDisposed();
				string contentType = this.m_HttpResponseHeaders.ContentType;
				if (this.m_CharacterSet == null && !ValidationHelper.IsBlankString(contentType))
				{
					this.m_CharacterSet = string.Empty;
					string text = contentType.ToLower(CultureInfo.InvariantCulture);
					if (text.Trim().StartsWith("text/"))
					{
						this.m_CharacterSet = "ISO-8859-1";
					}
					int num = text.IndexOf(";");
					if (num > 0)
					{
						while ((num = text.IndexOf("charset", num)) >= 0)
						{
							num += 7;
							if (text[num - 8] != ';')
							{
								if (text[num - 8] != ' ')
								{
									continue;
								}
							}
							while (num < text.Length && text[num] == ' ')
							{
								num++;
							}
							if (num < text.Length - 1 && text[num] == '=')
							{
								num++;
								int num2 = text.IndexOf(';', num);
								if (num2 > num)
								{
									this.m_CharacterSet = contentType.Substring(num, num2 - num).Trim();
									break;
								}
								this.m_CharacterSet = contentType.Substring(num).Trim();
								break;
							}
						}
					}
				}
				return this.m_CharacterSet;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002049 RID: 8265 RVA: 0x0007F0F4 File Offset: 0x0007E0F4
		public string Server
		{
			get
			{
				this.CheckDisposed();
				string server = this.m_HttpResponseHeaders.Server;
				if (server != null)
				{
					return server;
				}
				return string.Empty;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x0600204A RID: 8266 RVA: 0x0007F120 File Offset: 0x0007E120
		public DateTime LastModified
		{
			get
			{
				this.CheckDisposed();
				string lastModified = this.m_HttpResponseHeaders.LastModified;
				if (lastModified == null)
				{
					return DateTime.Now;
				}
				return HttpProtocolUtils.string2date(lastModified);
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x0600204B RID: 8267 RVA: 0x0007F14E File Offset: 0x0007E14E
		public HttpStatusCode StatusCode
		{
			get
			{
				return this.m_StatusCode;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x0600204C RID: 8268 RVA: 0x0007F156 File Offset: 0x0007E156
		public string StatusDescription
		{
			get
			{
				this.CheckDisposed();
				return this.m_StatusDescription;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x0600204D RID: 8269 RVA: 0x0007F164 File Offset: 0x0007E164
		public Version ProtocolVersion
		{
			get
			{
				this.CheckDisposed();
				if (!this.m_IsVersionHttp11)
				{
					return HttpVersion.Version10;
				}
				return HttpVersion.Version11;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x0600204E RID: 8270 RVA: 0x0007F180 File Offset: 0x0007E180
		internal bool KeepAlive
		{
			get
			{
				if (this.m_UsesProxySemantics)
				{
					string text = this.Headers["Proxy-Connection"];
					if (text != null)
					{
						return text.ToLower(CultureInfo.InvariantCulture).IndexOf("close") < 0 || text.ToLower(CultureInfo.InvariantCulture).IndexOf("keep-alive") >= 0;
					}
				}
				if (this.ProtocolVersion == HttpVersion.Version10)
				{
					string text2 = this.Headers["Keep-Alive"];
					return text2 != null;
				}
				if (this.ProtocolVersion >= HttpVersion.Version11)
				{
					string text3 = this.Headers["Connection"];
					return text3 == null || text3.ToLower(CultureInfo.InvariantCulture).IndexOf("close") < 0 || text3.ToLower(CultureInfo.InvariantCulture).IndexOf("keep-alive") >= 0;
				}
				return false;
			}
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x0007F268 File Offset: 0x0007E268
		public override Stream GetResponseStream()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "GetResponseStream", "");
			}
			this.CheckDisposed();
			if (!this.CanGetResponseStream())
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "GetResponseStream", Stream.Null);
				}
				return Stream.Null;
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, "ContentLength=" + this.m_ContentLength);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "GetResponseStream", this.m_ConnectStream);
			}
			return this.m_ConnectStream;
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x0007F30C File Offset: 0x0007E30C
		public override void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Close", "");
			}
			if (!this.m_disposed)
			{
				this.m_disposed = true;
				Stream connectStream = this.m_ConnectStream;
				ICloseEx closeEx = connectStream as ICloseEx;
				if (closeEx != null)
				{
					closeEx.CloseEx(CloseExState.Normal);
				}
				else if (connectStream != null)
				{
					connectStream.Close();
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "Close", "");
			}
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x0007F384 File Offset: 0x0007E384
		internal void Abort()
		{
			Stream connectStream = this.m_ConnectStream;
			ICloseEx closeEx = connectStream as ICloseEx;
			try
			{
				if (closeEx != null)
				{
					closeEx.CloseEx(CloseExState.Abort);
				}
				else if (connectStream != null)
				{
					connectStream.Close();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x0007F3CC File Offset: 0x0007E3CC
		internal override void OnDispose()
		{
			this.m_propertiesDisposed = true;
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x0007F3D5 File Offset: 0x0007E3D5
		internal bool CanGetResponseStream()
		{
			return !this.m_Verb.ExpectNoContentResponse;
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x0007F3E8 File Offset: 0x0007E3E8
		internal HttpWebResponse(Uri responseUri, KnownHttpVerb verb, CoreResponseData coreData, string mediaType, bool usesProxySemantics, DecompressionMethods decompressionMethod)
		{
			this.m_Uri = responseUri;
			this.m_Verb = verb;
			this.m_MediaType = mediaType;
			this.m_UsesProxySemantics = usesProxySemantics;
			this.m_ConnectStream = coreData.m_ConnectStream;
			this.m_HttpResponseHeaders = coreData.m_ResponseHeaders;
			this.m_ContentLength = coreData.m_ContentLength;
			this.m_StatusCode = coreData.m_StatusCode;
			this.m_StatusDescription = coreData.m_StatusDescription;
			this.m_IsVersionHttp11 = coreData.m_IsVersionHttp11;
			if (this.m_ContentLength == 0L && this.m_ConnectStream is ConnectStream)
			{
				((ConnectStream)this.m_ConnectStream).CallDone();
			}
			string text = this.m_HttpResponseHeaders["Content-Location"];
			if (text != null)
			{
				try
				{
					this.m_Uri = new Uri(this.m_Uri, text);
				}
				catch (UriFormatException)
				{
				}
			}
			if (decompressionMethod != DecompressionMethods.None)
			{
				string text2 = this.m_HttpResponseHeaders["Content-Encoding"];
				if (text2 != null)
				{
					if ((decompressionMethod & DecompressionMethods.GZip) != DecompressionMethods.None && text2.IndexOf("gzip") != -1)
					{
						this.m_ConnectStream = new GZipWrapperStream(this.m_ConnectStream, CompressionMode.Decompress);
						this.m_ContentLength = -1L;
						this.m_HttpResponseHeaders["Content-Encoding"] = null;
						return;
					}
					if ((decompressionMethod & DecompressionMethods.Deflate) != DecompressionMethods.None && text2.IndexOf("deflate") != -1)
					{
						this.m_ConnectStream = new DeflateWrapperStream(this.m_ConnectStream, CompressionMode.Decompress);
						this.m_ContentLength = -1L;
						this.m_HttpResponseHeaders["Content-Encoding"] = null;
					}
				}
			}
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x0007F560 File Offset: 0x0007E560
		[Obsolete("Serialization is obsoleted for this type.  http://go.microsoft.com/fwlink/?linkid=14202")]
		protected HttpWebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			this.m_HttpResponseHeaders = (WebHeaderCollection)serializationInfo.GetValue("m_HttpResponseHeaders", typeof(WebHeaderCollection));
			this.m_Uri = (Uri)serializationInfo.GetValue("m_Uri", typeof(Uri));
			this.m_Certificate = (X509Certificate)serializationInfo.GetValue("m_Certificate", typeof(X509Certificate));
			Version version = (Version)serializationInfo.GetValue("m_Version", typeof(Version));
			this.m_IsVersionHttp11 = version.Equals(HttpVersion.Version11);
			this.m_StatusCode = (HttpStatusCode)serializationInfo.GetInt32("m_StatusCode");
			this.m_ContentLength = serializationInfo.GetInt64("m_ContentLength");
			this.m_Verb = KnownHttpVerb.Parse(serializationInfo.GetString("m_Verb"));
			this.m_StatusDescription = serializationInfo.GetString("m_StatusDescription");
			this.m_MediaType = serializationInfo.GetString("m_MediaType");
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x0007F65B File Offset: 0x0007E65B
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x0007F668 File Offset: 0x0007E668
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("m_HttpResponseHeaders", this.m_HttpResponseHeaders, typeof(WebHeaderCollection));
			serializationInfo.AddValue("m_Uri", this.m_Uri, typeof(Uri));
			serializationInfo.AddValue("m_Certificate", this.m_Certificate, typeof(X509Certificate));
			serializationInfo.AddValue("m_Version", this.ProtocolVersion, typeof(Version));
			serializationInfo.AddValue("m_StatusCode", this.m_StatusCode);
			serializationInfo.AddValue("m_ContentLength", this.m_ContentLength);
			serializationInfo.AddValue("m_Verb", this.m_Verb.Name);
			serializationInfo.AddValue("m_StatusDescription", this.m_StatusDescription);
			serializationInfo.AddValue("m_MediaType", this.m_MediaType);
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x0007F748 File Offset: 0x0007E748
		public string GetResponseHeader(string headerName)
		{
			this.CheckDisposed();
			string text = this.m_HttpResponseHeaders[headerName];
			if (text != null)
			{
				return text;
			}
			return string.Empty;
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06002059 RID: 8281 RVA: 0x0007F772 File Offset: 0x0007E772
		public override Uri ResponseUri
		{
			get
			{
				this.CheckDisposed();
				return this.m_Uri;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x0600205A RID: 8282 RVA: 0x0007F780 File Offset: 0x0007E780
		public string Method
		{
			get
			{
				this.CheckDisposed();
				return this.m_Verb.Name;
			}
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x0007F793 File Offset: 0x0007E793
		private void CheckDisposed()
		{
			if (this.m_propertiesDisposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x04001F97 RID: 8087
		private Uri m_Uri;

		// Token: 0x04001F98 RID: 8088
		private KnownHttpVerb m_Verb;

		// Token: 0x04001F99 RID: 8089
		private HttpStatusCode m_StatusCode;

		// Token: 0x04001F9A RID: 8090
		private string m_StatusDescription;

		// Token: 0x04001F9B RID: 8091
		private Stream m_ConnectStream;

		// Token: 0x04001F9C RID: 8092
		private WebHeaderCollection m_HttpResponseHeaders;

		// Token: 0x04001F9D RID: 8093
		private long m_ContentLength;

		// Token: 0x04001F9E RID: 8094
		private string m_MediaType;

		// Token: 0x04001F9F RID: 8095
		private string m_CharacterSet;

		// Token: 0x04001FA0 RID: 8096
		private bool m_IsVersionHttp11;

		// Token: 0x04001FA1 RID: 8097
		internal X509Certificate m_Certificate;

		// Token: 0x04001FA2 RID: 8098
		private CookieCollection m_cookies;

		// Token: 0x04001FA3 RID: 8099
		private bool m_disposed;

		// Token: 0x04001FA4 RID: 8100
		private bool m_propertiesDisposed;

		// Token: 0x04001FA5 RID: 8101
		private bool m_UsesProxySemantics;

		// Token: 0x04001FA6 RID: 8102
		private bool m_IsMutuallyAuthenticated;
	}
}
