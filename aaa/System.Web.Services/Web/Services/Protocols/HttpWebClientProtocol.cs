using System;
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200002B RID: 43
	[ComVisible(true)]
	public abstract class HttpWebClientProtocol : WebClientProtocol
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x00003E67 File Offset: 0x00002E67
		protected HttpWebClientProtocol()
		{
			this.allowAutoRedirect = false;
			this.userAgent = HttpWebClientProtocol.UserAgentDefault;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003E84 File Offset: 0x00002E84
		internal HttpWebClientProtocol(HttpWebClientProtocol protocol)
			: base(protocol)
		{
			this.allowAutoRedirect = protocol.allowAutoRedirect;
			this.enableDecompression = protocol.enableDecompression;
			this.cookieJar = protocol.cookieJar;
			this.clientCertificates = protocol.clientCertificates;
			this.proxy = protocol.proxy;
			this.userAgent = protocol.userAgent;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00003EE0 File Offset: 0x00002EE0
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00003EE8 File Offset: 0x00002EE8
		[WebServicesDescription("ClientProtocolAllowAutoRedirect")]
		[DefaultValue(false)]
		public bool AllowAutoRedirect
		{
			get
			{
				return this.allowAutoRedirect;
			}
			set
			{
				this.allowAutoRedirect = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00003EF1 File Offset: 0x00002EF1
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00003EF9 File Offset: 0x00002EF9
		[WebServicesDescription("ClientProtocolCookieContainer")]
		[DefaultValue(null)]
		public CookieContainer CookieContainer
		{
			get
			{
				return this.cookieJar;
			}
			set
			{
				this.cookieJar = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00003F02 File Offset: 0x00002F02
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebServicesDescription("ClientProtocolClientCertificates")]
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				if (this.clientCertificates == null)
				{
					this.clientCertificates = new X509CertificateCollection();
				}
				return this.clientCertificates;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00003F1D File Offset: 0x00002F1D
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00003F25 File Offset: 0x00002F25
		[DefaultValue(false)]
		[WebServicesDescription("ClientProtocolEnableDecompression")]
		public bool EnableDecompression
		{
			get
			{
				return this.enableDecompression;
			}
			set
			{
				this.enableDecompression = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00003F2E File Offset: 0x00002F2E
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00003F44 File Offset: 0x00002F44
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebServicesDescription("ClientProtocolUserAgent")]
		public string UserAgent
		{
			get
			{
				if (this.userAgent != null)
				{
					return this.userAgent;
				}
				return string.Empty;
			}
			set
			{
				this.userAgent = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00003F4D File Offset: 0x00002F4D
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00003F55 File Offset: 0x00002F55
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public IWebProxy Proxy
		{
			get
			{
				return this.proxy;
			}
			set
			{
				this.proxy = value;
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00003F60 File Offset: 0x00002F60
		protected override WebRequest GetWebRequest(Uri uri)
		{
			WebRequest webRequest = base.GetWebRequest(uri);
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest != null)
			{
				httpWebRequest.UserAgent = this.UserAgent;
				httpWebRequest.AllowAutoRedirect = this.allowAutoRedirect;
				httpWebRequest.AutomaticDecompression = (this.enableDecompression ? DecompressionMethods.GZip : DecompressionMethods.None);
				httpWebRequest.AllowWriteStreamBuffering = true;
				httpWebRequest.SendChunked = false;
				if (this.unsafeAuthenticatedConnectionSharing != httpWebRequest.UnsafeAuthenticatedConnectionSharing)
				{
					httpWebRequest.UnsafeAuthenticatedConnectionSharing = this.unsafeAuthenticatedConnectionSharing;
				}
				if (this.proxy != null)
				{
					httpWebRequest.Proxy = this.proxy;
				}
				if (this.clientCertificates != null && this.clientCertificates.Count > 0)
				{
					httpWebRequest.ClientCertificates.AddRange(this.clientCertificates);
				}
				httpWebRequest.CookieContainer = this.cookieJar;
			}
			return webRequest;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000401C File Offset: 0x0000301C
		protected override WebResponse GetWebResponse(WebRequest request)
		{
			return base.GetWebResponse(request);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004034 File Offset: 0x00003034
		protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			return base.GetWebResponse(request, result);
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000404B File Offset: 0x0000304B
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00004053 File Offset: 0x00003053
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool UnsafeAuthenticatedConnectionSharing
		{
			get
			{
				return this.unsafeAuthenticatedConnectionSharing;
			}
			set
			{
				this.unsafeAuthenticatedConnectionSharing = value;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000405C File Offset: 0x0000305C
		protected void CancelAsync(object userState)
		{
			if (userState == null)
			{
				userState = base.NullToken;
			}
			object obj = userState;
			object[] array = new object[1];
			WebClientAsyncResult webClientAsyncResult = this.OperationCompleted(obj, array, null, true);
			if (webClientAsyncResult != null)
			{
				webClientAsyncResult.Abort();
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004090 File Offset: 0x00003090
		internal WebClientAsyncResult OperationCompleted(object userState, object[] parameters, Exception e, bool canceled)
		{
			WebClientAsyncResult webClientAsyncResult = (WebClientAsyncResult)base.AsyncInvokes[userState];
			if (webClientAsyncResult != null)
			{
				AsyncOperation asyncOperation = (AsyncOperation)webClientAsyncResult.AsyncState;
				UserToken userToken = (UserToken)asyncOperation.UserSuppliedState;
				InvokeCompletedEventArgs invokeCompletedEventArgs = new InvokeCompletedEventArgs(parameters, e, canceled, userState);
				base.AsyncInvokes.Remove(userState);
				asyncOperation.PostOperationCompleted(userToken.Callback, invokeCompletedEventArgs);
			}
			return webClientAsyncResult;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000040F0 File Offset: 0x000030F0
		public static bool GenerateXmlMappings(Type type, ArrayList mappings)
		{
			if (!typeof(SoapHttpClientProtocol).IsAssignableFrom(type))
			{
				return false;
			}
			WebServiceBindingAttribute attribute = WebServiceBindingReflector.GetAttribute(type);
			if (attribute == null)
			{
				throw new InvalidOperationException(Res.GetString("WebClientBindingAttributeRequired"));
			}
			string @namespace = attribute.Namespace;
			bool flag = SoapReflector.ServiceDefaultIsEncoded(type);
			ArrayList arrayList = new ArrayList();
			SoapClientType.GenerateXmlMappings(type, arrayList, @namespace, flag, mappings);
			return true;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000414C File Offset: 0x0000314C
		public static Hashtable GenerateXmlMappings(Type[] types, ArrayList mappings)
		{
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			Hashtable hashtable = new Hashtable();
			foreach (Type type in types)
			{
				ArrayList arrayList = new ArrayList();
				if (HttpWebClientProtocol.GenerateXmlMappings(type, mappings))
				{
					hashtable.Add(type, arrayList);
					mappings.Add(arrayList);
				}
			}
			return hashtable;
		}

		// Token: 0x0400025D RID: 605
		private bool allowAutoRedirect;

		// Token: 0x0400025E RID: 606
		private bool enableDecompression;

		// Token: 0x0400025F RID: 607
		private CookieContainer cookieJar;

		// Token: 0x04000260 RID: 608
		private X509CertificateCollection clientCertificates;

		// Token: 0x04000261 RID: 609
		private IWebProxy proxy;

		// Token: 0x04000262 RID: 610
		private static string UserAgentDefault = "Mozilla/4.0 (compatible; MSIE 6.0; MS Web Services Client Protocol " + Environment.Version.ToString() + ")";

		// Token: 0x04000263 RID: 611
		private string userAgent;

		// Token: 0x04000264 RID: 612
		private bool unsafeAuthenticatedConnectionSharing;
	}
}
