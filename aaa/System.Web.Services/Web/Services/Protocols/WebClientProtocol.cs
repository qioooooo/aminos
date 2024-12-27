using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Web.Services.Diagnostics;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000026 RID: 38
	[ComVisible(true)]
	public abstract class WebClientProtocol : Component
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000098 RID: 152 RVA: 0x0000323C File Offset: 0x0000223C
		internal static object InternalSyncObject
		{
			get
			{
				if (WebClientProtocol.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref WebClientProtocol.s_InternalSyncObject, obj, null);
				}
				return WebClientProtocol.s_InternalSyncObject;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003274 File Offset: 0x00002274
		protected WebClientProtocol()
		{
			this.timeout = 100000;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000032A4 File Offset: 0x000022A4
		internal WebClientProtocol(WebClientProtocol protocol)
		{
			this.credentials = protocol.credentials;
			this.uri = protocol.uri;
			this.timeout = protocol.timeout;
			this.connectionGroupName = protocol.connectionGroupName;
			this.requestEncoding = protocol.requestEncoding;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600009C RID: 156 RVA: 0x0000330E File Offset: 0x0000230E
		internal static RequestCachePolicy BypassCache
		{
			get
			{
				if (WebClientProtocol.bypassCache == null)
				{
					WebClientProtocol.bypassCache = new RequestCachePolicy(RequestCacheLevel.BypassCache);
				}
				return WebClientProtocol.bypassCache;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00003327 File Offset: 0x00002327
		// (set) Token: 0x0600009E RID: 158 RVA: 0x0000332F File Offset: 0x0000232F
		public ICredentials Credentials
		{
			get
			{
				return this.credentials;
			}
			set
			{
				this.credentials = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00003338 File Offset: 0x00002338
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x0000334A File Offset: 0x0000234A
		public bool UseDefaultCredentials
		{
			get
			{
				return this.credentials == CredentialCache.DefaultCredentials;
			}
			set
			{
				this.credentials = (value ? CredentialCache.DefaultCredentials : null);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x0000335D File Offset: 0x0000235D
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003373 File Offset: 0x00002373
		[DefaultValue("")]
		public string ConnectionGroupName
		{
			get
			{
				if (this.connectionGroupName != null)
				{
					return this.connectionGroupName;
				}
				return string.Empty;
			}
			set
			{
				this.connectionGroupName = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000337C File Offset: 0x0000237C
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003384 File Offset: 0x00002384
		internal WebRequest PendingSyncRequest
		{
			get
			{
				return this.pendingSyncRequest;
			}
			set
			{
				this.pendingSyncRequest = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x0000338D File Offset: 0x0000238D
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003395 File Offset: 0x00002395
		[WebServicesDescription("ClientProtocolPreAuthenticate")]
		[DefaultValue(false)]
		public bool PreAuthenticate
		{
			get
			{
				return this.preAuthenticate;
			}
			set
			{
				this.preAuthenticate = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000339E File Offset: 0x0000239E
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x000033BF File Offset: 0x000023BF
		[DefaultValue("")]
		[RecommendedAsConfigurable(true)]
		[WebServicesDescription("ClientProtocolUrl")]
		public string Url
		{
			get
			{
				if (!(this.uri == null))
				{
					return this.uri.ToString();
				}
				return string.Empty;
			}
			set
			{
				this.uri = new Uri(value);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000033CD File Offset: 0x000023CD
		internal Hashtable AsyncInvokes
		{
			get
			{
				return this.asyncInvokes;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000033D5 File Offset: 0x000023D5
		internal object NullToken
		{
			get
			{
				return this.nullToken;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000033DD File Offset: 0x000023DD
		// (set) Token: 0x060000AC RID: 172 RVA: 0x000033E5 File Offset: 0x000023E5
		internal Uri Uri
		{
			get
			{
				return this.uri;
			}
			set
			{
				this.uri = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000033EE File Offset: 0x000023EE
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000033F6 File Offset: 0x000023F6
		[WebServicesDescription("ClientProtocolEncoding")]
		[RecommendedAsConfigurable(true)]
		[DefaultValue(null)]
		public Encoding RequestEncoding
		{
			get
			{
				return this.requestEncoding;
			}
			set
			{
				this.requestEncoding = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000033FF File Offset: 0x000023FF
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00003407 File Offset: 0x00002407
		[DefaultValue(100000)]
		[WebServicesDescription("ClientProtocolTimeout")]
		[RecommendedAsConfigurable(true)]
		public int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				this.timeout = ((value < -1) ? (-1) : value);
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003418 File Offset: 0x00002418
		public virtual void Abort()
		{
			WebRequest webRequest = this.PendingSyncRequest;
			if (webRequest != null)
			{
				webRequest.Abort();
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003438 File Offset: 0x00002438
		internal IAsyncResult BeginSend(Uri requestUri, WebClientAsyncResult asyncResult, bool callWriteAsyncRequest)
		{
			if (WebClientProtocol.readResponseAsyncCallback == null)
			{
				lock (WebClientProtocol.InternalSyncObject)
				{
					if (WebClientProtocol.readResponseAsyncCallback == null)
					{
						WebClientProtocol.getRequestStreamAsyncCallback = new AsyncCallback(WebClientProtocol.GetRequestStreamAsyncCallback);
						WebClientProtocol.getResponseAsyncCallback = new AsyncCallback(WebClientProtocol.GetResponseAsyncCallback);
						WebClientProtocol.readResponseAsyncCallback = new AsyncCallback(WebClientProtocol.ReadResponseAsyncCallback);
					}
				}
			}
			WebRequest webRequest = this.GetWebRequest(requestUri);
			asyncResult.Request = webRequest;
			this.InitializeAsyncRequest(webRequest, asyncResult.InternalAsyncState);
			if (callWriteAsyncRequest)
			{
				webRequest.BeginGetRequestStream(WebClientProtocol.getRequestStreamAsyncCallback, asyncResult);
			}
			else
			{
				webRequest.BeginGetResponse(WebClientProtocol.getResponseAsyncCallback, asyncResult);
			}
			if (!asyncResult.IsCompleted)
			{
				asyncResult.CombineCompletedSynchronously(false);
			}
			return asyncResult;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000034F8 File Offset: 0x000024F8
		private static void ProcessAsyncException(WebClientAsyncResult client, Exception e, string method)
		{
			if (Tracing.On)
			{
				Tracing.ExceptionCatch(TraceEventType.Error, typeof(WebClientProtocol), method, e);
			}
			WebException ex = e as WebException;
			if (ex != null && ex.Response != null)
			{
				client.Response = ex.Response;
				return;
			}
			if (client.IsCompleted)
			{
				throw new InvalidOperationException(Res.GetString("ThereWasAnErrorDuringAsyncProcessing"), e);
			}
			client.Complete(e);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003560 File Offset: 0x00002560
		private static void GetRequestStreamAsyncCallback(IAsyncResult asyncResult)
		{
			WebClientAsyncResult webClientAsyncResult = (WebClientAsyncResult)asyncResult.AsyncState;
			webClientAsyncResult.CombineCompletedSynchronously(asyncResult.CompletedSynchronously);
			bool flag = true;
			try
			{
				Stream stream = webClientAsyncResult.Request.EndGetRequestStream(asyncResult);
				flag = false;
				try
				{
					webClientAsyncResult.ClientProtocol.AsyncBufferedSerialize(webClientAsyncResult.Request, stream, webClientAsyncResult.InternalAsyncState);
				}
				finally
				{
					stream.Close();
				}
				webClientAsyncResult.Request.BeginGetResponse(WebClientProtocol.getResponseAsyncCallback, webClientAsyncResult);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				WebClientProtocol.ProcessAsyncException(webClientAsyncResult, ex, "GetRequestStreamAsyncCallback");
				if (flag)
				{
					WebException ex2 = ex as WebException;
					if (ex2 != null && ex2.Response != null)
					{
						webClientAsyncResult.Complete(ex);
					}
				}
			}
			catch
			{
				WebClientProtocol.ProcessAsyncException(webClientAsyncResult, new Exception(Res.GetString("NonClsCompliantException")), "GetRequestStreamAsyncCallback");
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003658 File Offset: 0x00002658
		private static void GetResponseAsyncCallback(IAsyncResult asyncResult)
		{
			WebClientAsyncResult webClientAsyncResult = (WebClientAsyncResult)asyncResult.AsyncState;
			webClientAsyncResult.CombineCompletedSynchronously(asyncResult.CompletedSynchronously);
			try
			{
				webClientAsyncResult.Response = webClientAsyncResult.ClientProtocol.GetWebResponse(webClientAsyncResult.Request, asyncResult);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				WebClientProtocol.ProcessAsyncException(webClientAsyncResult, ex, "GetResponseAsyncCallback");
				if (webClientAsyncResult.Response == null)
				{
					return;
				}
			}
			catch
			{
				WebClientProtocol.ProcessAsyncException(webClientAsyncResult, new Exception(Res.GetString("NonClsCompliantException")), "GetResponseAsyncCallback");
				if (webClientAsyncResult.Response == null)
				{
					return;
				}
			}
			WebClientProtocol.ReadAsyncResponse(webClientAsyncResult);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003714 File Offset: 0x00002714
		private static void ReadAsyncResponse(WebClientAsyncResult client)
		{
			if (client.Response.ContentLength == 0L)
			{
				client.Complete();
				return;
			}
			try
			{
				client.ResponseStream = client.Response.GetResponseStream();
				WebClientProtocol.ReadAsyncResponseStream(client);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				WebClientProtocol.ProcessAsyncException(client, ex, "ReadAsyncResponse");
			}
			catch
			{
				WebClientProtocol.ProcessAsyncException(client, new Exception(Res.GetString("NonClsCompliantException")), "ReadAsyncResponse");
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000037B4 File Offset: 0x000027B4
		private static void ReadAsyncResponseStream(WebClientAsyncResult client)
		{
			for (;;)
			{
				byte[] array = client.Buffer;
				long contentLength = client.Response.ContentLength;
				if (array == null)
				{
					array = (client.Buffer = new byte[(contentLength == -1L) ? 1024L : contentLength]);
				}
				else if (contentLength != -1L && contentLength > (long)array.Length)
				{
					array = (client.Buffer = new byte[contentLength]);
				}
				IAsyncResult asyncResult = client.ResponseStream.BeginRead(array, 0, array.Length, WebClientProtocol.readResponseAsyncCallback, client);
				if (!asyncResult.CompletedSynchronously)
				{
					break;
				}
				if (WebClientProtocol.ProcessAsyncResponseStreamResult(client, asyncResult))
				{
					return;
				}
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003840 File Offset: 0x00002840
		private static bool ProcessAsyncResponseStreamResult(WebClientAsyncResult client, IAsyncResult asyncResult)
		{
			int num = client.ResponseStream.EndRead(asyncResult);
			long contentLength = client.Response.ContentLength;
			bool flag;
			if (contentLength > 0L && (long)num == contentLength)
			{
				client.ResponseBufferedStream = new MemoryStream(client.Buffer);
				flag = true;
			}
			else if (num > 0)
			{
				if (client.ResponseBufferedStream == null)
				{
					int num2 = (int)((contentLength == -1L) ? ((long)client.Buffer.Length) : contentLength);
					client.ResponseBufferedStream = new MemoryStream(num2);
				}
				client.ResponseBufferedStream.Write(client.Buffer, 0, num);
				flag = false;
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				client.Complete();
			}
			return flag;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000038D4 File Offset: 0x000028D4
		private static void ReadResponseAsyncCallback(IAsyncResult asyncResult)
		{
			WebClientAsyncResult webClientAsyncResult = (WebClientAsyncResult)asyncResult.AsyncState;
			webClientAsyncResult.CombineCompletedSynchronously(asyncResult.CompletedSynchronously);
			if (asyncResult.CompletedSynchronously)
			{
				return;
			}
			try
			{
				if (!WebClientProtocol.ProcessAsyncResponseStreamResult(webClientAsyncResult, asyncResult))
				{
					WebClientProtocol.ReadAsyncResponseStream(webClientAsyncResult);
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				WebClientProtocol.ProcessAsyncException(webClientAsyncResult, ex, "ReadResponseAsyncCallback");
			}
			catch
			{
				WebClientProtocol.ProcessAsyncException(webClientAsyncResult, new Exception(Res.GetString("NonClsCompliantException")), "ReadResponseAsyncCallback");
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003978 File Offset: 0x00002978
		internal void NotifyClientCallOut(WebRequest request)
		{
			if (RemoteDebugger.IsClientCallOutEnabled())
			{
				this.debugger = new RemoteDebugger();
				this.debugger.NotifyClientCallOut(request);
				return;
			}
			this.debugger = null;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000039A0 File Offset: 0x000029A0
		protected virtual WebRequest GetWebRequest(Uri uri)
		{
			if (uri == null)
			{
				throw new InvalidOperationException(Res.GetString("WebMissingPath"));
			}
			WebRequest webRequest = WebRequest.Create(uri);
			this.PendingSyncRequest = webRequest;
			webRequest.Timeout = this.timeout;
			webRequest.ConnectionGroupName = this.connectionGroupName;
			webRequest.Credentials = this.Credentials;
			webRequest.PreAuthenticate = this.PreAuthenticate;
			webRequest.CachePolicy = WebClientProtocol.BypassCache;
			return webRequest;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003A10 File Offset: 0x00002A10
		protected virtual WebResponse GetWebResponse(WebRequest request)
		{
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "GetWebResponse", new object[0]) : null);
			WebResponse webResponse = null;
			try
			{
				if (Tracing.On)
				{
					Tracing.Enter("WebRequest.GetResponse", traceMethod, new TraceMethod(request, "GetResponse", new object[0]));
				}
				webResponse = request.GetResponse();
				if (Tracing.On)
				{
					Tracing.Exit("WebRequest.GetResponse", traceMethod);
				}
			}
			catch (WebException ex)
			{
				if (ex.Response == null)
				{
					throw ex;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Error, this, "GetWebResponse", ex);
				}
				webResponse = ex.Response;
			}
			finally
			{
				if (this.debugger != null)
				{
					this.debugger.NotifyClientCallReturn(webResponse);
				}
			}
			return webResponse;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003AD8 File Offset: 0x00002AD8
		protected virtual WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			WebResponse webResponse = request.EndGetResponse(result);
			if (webResponse != null && this.debugger != null)
			{
				this.debugger.NotifyClientCallReturn(webResponse);
			}
			return webResponse;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003B05 File Offset: 0x00002B05
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		internal virtual void InitializeAsyncRequest(WebRequest request, object internalAsyncState)
		{
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003B07 File Offset: 0x00002B07
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		internal virtual void AsyncBufferedSerialize(WebRequest request, Stream requestStream, object internalAsyncState)
		{
			throw new NotSupportedException(Res.GetString("ProtocolDoesNotAsyncSerialize"));
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003B18 File Offset: 0x00002B18
		internal WebResponse EndSend(IAsyncResult asyncResult, ref object internalAsyncState, ref Stream responseStream)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException(Res.GetString("WebNullAsyncResultInEnd"));
			}
			WebClientAsyncResult webClientAsyncResult = (WebClientAsyncResult)asyncResult;
			if (webClientAsyncResult.EndSendCalled)
			{
				throw new InvalidOperationException(Res.GetString("CanTCallTheEndMethodOfAnAsyncCallMoreThan"));
			}
			webClientAsyncResult.EndSendCalled = true;
			WebResponse webResponse = webClientAsyncResult.WaitForResponse();
			internalAsyncState = webClientAsyncResult.InternalAsyncState;
			responseStream = webClientAsyncResult.ResponseBufferedStream;
			return webResponse;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003B76 File Offset: 0x00002B76
		protected static object GetFromCache(Type type)
		{
			return WebClientProtocol.cache[type];
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003B83 File Offset: 0x00002B83
		protected static void AddToCache(Type type, object value)
		{
			WebClientProtocol.cache.Add(type, value);
		}

		// Token: 0x0400023C RID: 572
		private static AsyncCallback getRequestStreamAsyncCallback;

		// Token: 0x0400023D RID: 573
		private static AsyncCallback getResponseAsyncCallback;

		// Token: 0x0400023E RID: 574
		private static AsyncCallback readResponseAsyncCallback;

		// Token: 0x0400023F RID: 575
		private static ClientTypeCache cache = new ClientTypeCache();

		// Token: 0x04000240 RID: 576
		private static RequestCachePolicy bypassCache;

		// Token: 0x04000241 RID: 577
		private ICredentials credentials;

		// Token: 0x04000242 RID: 578
		private bool preAuthenticate;

		// Token: 0x04000243 RID: 579
		private Uri uri;

		// Token: 0x04000244 RID: 580
		private int timeout;

		// Token: 0x04000245 RID: 581
		private string connectionGroupName;

		// Token: 0x04000246 RID: 582
		private Encoding requestEncoding;

		// Token: 0x04000247 RID: 583
		private RemoteDebugger debugger;

		// Token: 0x04000248 RID: 584
		private WebRequest pendingSyncRequest;

		// Token: 0x04000249 RID: 585
		private object nullToken = new object();

		// Token: 0x0400024A RID: 586
		private Hashtable asyncInvokes = Hashtable.Synchronized(new Hashtable());

		// Token: 0x0400024B RID: 587
		private static object s_InternalSyncObject;
	}
}
