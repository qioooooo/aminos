using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net.Cache;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000484 RID: 1156
	[ComVisible(true)]
	public class WebClient : Component
	{
		// Token: 0x06002307 RID: 8967 RVA: 0x00089388 File Offset: 0x00088388
		private void InitWebClientAsync()
		{
			if (!this.m_InitWebClientAsync)
			{
				this.openReadOperationCompleted = new SendOrPostCallback(this.OpenReadOperationCompleted);
				this.openWriteOperationCompleted = new SendOrPostCallback(this.OpenWriteOperationCompleted);
				this.downloadStringOperationCompleted = new SendOrPostCallback(this.DownloadStringOperationCompleted);
				this.downloadDataOperationCompleted = new SendOrPostCallback(this.DownloadDataOperationCompleted);
				this.downloadFileOperationCompleted = new SendOrPostCallback(this.DownloadFileOperationCompleted);
				this.uploadStringOperationCompleted = new SendOrPostCallback(this.UploadStringOperationCompleted);
				this.uploadDataOperationCompleted = new SendOrPostCallback(this.UploadDataOperationCompleted);
				this.uploadFileOperationCompleted = new SendOrPostCallback(this.UploadFileOperationCompleted);
				this.uploadValuesOperationCompleted = new SendOrPostCallback(this.UploadValuesOperationCompleted);
				this.reportDownloadProgressChanged = new SendOrPostCallback(this.ReportDownloadProgressChanged);
				this.reportUploadProgressChanged = new SendOrPostCallback(this.ReportUploadProgressChanged);
				this.m_Progress = new WebClient.ProgressData();
				this.m_InitWebClientAsync = true;
			}
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x00089478 File Offset: 0x00088478
		private void ClearWebClientState()
		{
			if (this.AnotherCallInProgress(Interlocked.Increment(ref this.m_CallNesting)))
			{
				this.CompleteWebClientState();
				throw new NotSupportedException(SR.GetString("net_webclient_no_concurrent_io_allowed"));
			}
			this.m_ContentLength = -1L;
			this.m_WebResponse = null;
			this.m_WebRequest = null;
			this.m_Method = null;
			this.m_Cancelled = false;
			if (this.m_Progress != null)
			{
				this.m_Progress.Reset();
			}
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x000894E5 File Offset: 0x000884E5
		private void CompleteWebClientState()
		{
			Interlocked.Decrement(ref this.m_CallNesting);
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x0600230A RID: 8970 RVA: 0x000894F3 File Offset: 0x000884F3
		// (set) Token: 0x0600230B RID: 8971 RVA: 0x000894FB File Offset: 0x000884FB
		public Encoding Encoding
		{
			get
			{
				return this.m_Encoding;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Encoding");
				}
				this.m_Encoding = value;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x0600230C RID: 8972 RVA: 0x00089512 File Offset: 0x00088512
		// (set) Token: 0x0600230D RID: 8973 RVA: 0x00089534 File Offset: 0x00088534
		public string BaseAddress
		{
			get
			{
				if (!(this.m_baseAddress == null))
				{
					return this.m_baseAddress.ToString();
				}
				return string.Empty;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					this.m_baseAddress = null;
					return;
				}
				try
				{
					this.m_baseAddress = new Uri(value);
				}
				catch (UriFormatException ex)
				{
					throw new ArgumentException(SR.GetString("net_webclient_invalid_baseaddress"), "value", ex);
				}
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x0600230E RID: 8974 RVA: 0x0008958C File Offset: 0x0008858C
		// (set) Token: 0x0600230F RID: 8975 RVA: 0x00089594 File Offset: 0x00088594
		public ICredentials Credentials
		{
			get
			{
				return this.m_credentials;
			}
			set
			{
				this.m_credentials = value;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06002310 RID: 8976 RVA: 0x0008959D File Offset: 0x0008859D
		// (set) Token: 0x06002311 RID: 8977 RVA: 0x000895AF File Offset: 0x000885AF
		public bool UseDefaultCredentials
		{
			get
			{
				return this.m_credentials is SystemNetworkCredential;
			}
			set
			{
				this.m_credentials = (value ? CredentialCache.DefaultCredentials : null);
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x000895C2 File Offset: 0x000885C2
		// (set) Token: 0x06002313 RID: 8979 RVA: 0x000895DE File Offset: 0x000885DE
		public WebHeaderCollection Headers
		{
			get
			{
				if (this.m_headers == null)
				{
					this.m_headers = new WebHeaderCollection(WebHeaderCollectionType.WebRequest);
				}
				return this.m_headers;
			}
			set
			{
				this.m_headers = value;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x000895E7 File Offset: 0x000885E7
		// (set) Token: 0x06002315 RID: 8981 RVA: 0x00089602 File Offset: 0x00088602
		public NameValueCollection QueryString
		{
			get
			{
				if (this.m_requestParameters == null)
				{
					this.m_requestParameters = new NameValueCollection();
				}
				return this.m_requestParameters;
			}
			set
			{
				this.m_requestParameters = value;
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002316 RID: 8982 RVA: 0x0008960B File Offset: 0x0008860B
		public WebHeaderCollection ResponseHeaders
		{
			get
			{
				if (this.m_WebResponse != null)
				{
					return this.m_WebResponse.Headers;
				}
				return null;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002317 RID: 8983 RVA: 0x00089622 File Offset: 0x00088622
		// (set) Token: 0x06002318 RID: 8984 RVA: 0x00089642 File Offset: 0x00088642
		public IWebProxy Proxy
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (!this.m_ProxySet)
				{
					return WebRequest.InternalDefaultWebProxy;
				}
				return this.m_Proxy;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				this.m_Proxy = value;
				this.m_ProxySet = true;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002319 RID: 8985 RVA: 0x0008965C File Offset: 0x0008865C
		// (set) Token: 0x0600231A RID: 8986 RVA: 0x00089664 File Offset: 0x00088664
		public RequestCachePolicy CachePolicy
		{
			get
			{
				return this.m_CachePolicy;
			}
			set
			{
				this.m_CachePolicy = value;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x0600231B RID: 8987 RVA: 0x0008966D File Offset: 0x0008866D
		public bool IsBusy
		{
			get
			{
				return this.m_AsyncOp != null;
			}
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0008967C File Offset: 0x0008867C
		protected virtual WebRequest GetWebRequest(Uri address)
		{
			WebRequest webRequest = WebRequest.Create(address);
			this.CopyHeadersTo(webRequest);
			if (this.Credentials != null)
			{
				webRequest.Credentials = this.Credentials;
			}
			if (this.m_Method != null)
			{
				webRequest.Method = this.m_Method;
			}
			if (this.m_ContentLength != -1L)
			{
				webRequest.ContentLength = this.m_ContentLength;
			}
			if (this.m_ProxySet)
			{
				webRequest.Proxy = this.m_Proxy;
			}
			if (this.m_CachePolicy != null)
			{
				webRequest.CachePolicy = this.m_CachePolicy;
			}
			return webRequest;
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x00089700 File Offset: 0x00088700
		protected virtual WebResponse GetWebResponse(WebRequest request)
		{
			WebResponse response = request.GetResponse();
			this.m_WebResponse = response;
			return response;
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x0008971C File Offset: 0x0008871C
		protected virtual WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			WebResponse webResponse = request.EndGetResponse(result);
			this.m_WebResponse = webResponse;
			return webResponse;
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x00089739 File Offset: 0x00088739
		public byte[] DownloadData(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.DownloadData(this.GetUri(address));
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x00089758 File Offset: 0x00088758
		public byte[] DownloadData(Uri address)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadData", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.ClearWebClientState();
			byte[] array2;
			try
			{
				WebRequest webRequest;
				byte[] array = this.DownloadDataInternal(address, out webRequest);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "DownloadData", array);
				}
				array2 = array;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return array2;
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x000897D8 File Offset: 0x000887D8
		private byte[] DownloadDataInternal(Uri address, out WebRequest request)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadData", address);
			}
			request = null;
			byte[] array2;
			try
			{
				request = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				byte[] array = this.DownloadBits(request, null, null, null);
				array2 = array;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(request);
				throw ex;
			}
			catch
			{
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				WebClient.AbortRequest(request);
				throw ex2;
			}
			return array2;
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x000898B8 File Offset: 0x000888B8
		public void DownloadFile(string address, string fileName)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.DownloadFile(this.GetUri(address), fileName);
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x000898D8 File Offset: 0x000888D8
		public void DownloadFile(Uri address, string fileName)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadFile", address + ", " + fileName);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			WebRequest webRequest = null;
			FileStream fileStream = null;
			bool flag = false;
			this.ClearWebClientState();
			try
			{
				fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
				webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.DownloadBits(webRequest, fileStream, null, null);
				flag = true;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(webRequest);
				throw ex;
			}
			catch
			{
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				WebClient.AbortRequest(webRequest);
				throw ex2;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
					if (!flag)
					{
						File.Delete(fileName);
					}
					fileStream = null;
				}
				this.CompleteWebClientState();
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "DownloadFile", "");
			}
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x00089A38 File Offset: 0x00088A38
		public Stream OpenRead(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.OpenRead(this.GetUri(address));
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x00089A58 File Offset: 0x00088A58
		public Stream OpenRead(Uri address)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "OpenRead", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			WebRequest webRequest = null;
			this.ClearWebClientState();
			Stream stream;
			try
			{
				webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				WebResponse webResponse = (this.m_WebResponse = this.GetWebResponse(webRequest));
				Stream responseStream = webResponse.GetResponseStream();
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "OpenRead", responseStream);
				}
				stream = responseStream;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(webRequest);
				throw ex;
			}
			catch
			{
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				WebClient.AbortRequest(webRequest);
				throw ex2;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return stream;
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x00089B88 File Offset: 0x00088B88
		public Stream OpenWrite(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.OpenWrite(this.GetUri(address), null);
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x00089BA6 File Offset: 0x00088BA6
		public Stream OpenWrite(Uri address)
		{
			return this.OpenWrite(address, null);
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x00089BB0 File Offset: 0x00088BB0
		public Stream OpenWrite(string address, string method)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.OpenWrite(this.GetUri(address), method);
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x00089BD0 File Offset: 0x00088BD0
		public Stream OpenWrite(Uri address, string method)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "OpenWrite", address + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			WebRequest webRequest = null;
			this.ClearWebClientState();
			Stream stream;
			try
			{
				this.m_Method = method;
				webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				WebClient.WebClientWriteStream webClientWriteStream = new WebClient.WebClientWriteStream(webRequest.GetRequestStream(), webRequest, this);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "OpenWrite", webClientWriteStream);
				}
				stream = webClientWriteStream;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(webRequest);
				throw ex;
			}
			catch
			{
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				WebClient.AbortRequest(webRequest);
				throw ex2;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return stream;
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x00089D10 File Offset: 0x00088D10
		public byte[] UploadData(string address, byte[] data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadData(this.GetUri(address), null, data);
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x00089D2F File Offset: 0x00088D2F
		public byte[] UploadData(Uri address, byte[] data)
		{
			return this.UploadData(address, null, data);
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x00089D3A File Offset: 0x00088D3A
		public byte[] UploadData(string address, string method, byte[] data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadData(this.GetUri(address), method, data);
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x00089D5C File Offset: 0x00088D5C
		public byte[] UploadData(Uri address, string method, byte[] data)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadData", address + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.ClearWebClientState();
			byte[] array2;
			try
			{
				WebRequest webRequest;
				byte[] array = this.UploadDataInternal(address, method, data, out webRequest);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "UploadData", array);
				}
				array2 = array;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return array2;
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x00089E00 File Offset: 0x00088E00
		private byte[] UploadDataInternal(Uri address, string method, byte[] data, out WebRequest request)
		{
			request = null;
			byte[] array2;
			try
			{
				this.m_Method = method;
				this.m_ContentLength = (long)data.Length;
				request = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.UploadBits(request, null, data, null, null, null, null);
				byte[] array = this.DownloadBits(request, null, null, null);
				array2 = array;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(request);
				throw ex;
			}
			catch
			{
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				WebClient.AbortRequest(request);
				throw ex2;
			}
			return array2;
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x00089EEC File Offset: 0x00088EEC
		private void OpenFileInternal(bool needsHeaderAndBoundary, string fileName, ref FileStream fs, ref byte[] buffer, ref byte[] formHeaderBytes, ref byte[] boundaryBytes)
		{
			fileName = Path.GetFullPath(fileName);
			if (this.m_headers == null)
			{
				this.m_headers = new WebHeaderCollection(WebHeaderCollectionType.WebRequest);
			}
			string text = this.m_headers["Content-Type"];
			if (text != null)
			{
				if (text.ToLower(CultureInfo.InvariantCulture).StartsWith("multipart/"))
				{
					throw new WebException(SR.GetString("net_webclient_Multipart"));
				}
			}
			else
			{
				text = "application/octet-stream";
			}
			fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			int num = 8192;
			this.m_ContentLength = -1L;
			if (this.m_Method.ToUpper(CultureInfo.InvariantCulture) == "POST")
			{
				if (needsHeaderAndBoundary)
				{
					string text2 = "---------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
					this.m_headers["Content-Type"] = "multipart/form-data; boundary=" + text2;
					string text3 = string.Concat(new string[]
					{
						"--",
						text2,
						"\r\nContent-Disposition: form-data; name=\"file\"; filename=\"",
						Path.GetFileName(fileName),
						"\"\r\nContent-Type: ",
						text,
						"\r\n\r\n"
					});
					formHeaderBytes = Encoding.UTF8.GetBytes(text3);
					boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + text2 + "--\r\n");
				}
				else
				{
					formHeaderBytes = new byte[0];
					boundaryBytes = new byte[0];
				}
				if (fs.CanSeek)
				{
					this.m_ContentLength = fs.Length + (long)formHeaderBytes.Length + (long)boundaryBytes.Length;
					num = (int)Math.Min(8192L, fs.Length);
				}
			}
			else
			{
				this.m_headers["Content-Type"] = text;
				formHeaderBytes = null;
				boundaryBytes = null;
				if (fs.CanSeek)
				{
					this.m_ContentLength = fs.Length;
					num = (int)Math.Min(8192L, fs.Length);
				}
			}
			buffer = new byte[num];
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x0008A0E7 File Offset: 0x000890E7
		public byte[] UploadFile(string address, string fileName)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadFile(this.GetUri(address), fileName);
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x0008A105 File Offset: 0x00089105
		public byte[] UploadFile(Uri address, string fileName)
		{
			return this.UploadFile(address, null, fileName);
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x0008A110 File Offset: 0x00089110
		public byte[] UploadFile(string address, string method, string fileName)
		{
			return this.UploadFile(this.GetUri(address), method, fileName);
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0008A124 File Offset: 0x00089124
		public byte[] UploadFile(Uri address, string method, string fileName)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadFile", address + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			FileStream fileStream = null;
			WebRequest webRequest = null;
			this.ClearWebClientState();
			byte[] array5;
			try
			{
				this.m_Method = method;
				byte[] array = null;
				byte[] array2 = null;
				byte[] array3 = null;
				Uri uri = this.GetUri(address);
				bool flag = uri.Scheme != Uri.UriSchemeFile;
				this.OpenFileInternal(flag, fileName, ref fileStream, ref array3, ref array, ref array2);
				webRequest = (this.m_WebRequest = this.GetWebRequest(uri));
				this.UploadBits(webRequest, fileStream, array3, array, array2, null, null);
				byte[] array4 = this.DownloadBits(webRequest, null, null, null);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "UploadFile", array4);
				}
				array5 = array4;
			}
			catch (Exception ex)
			{
				if (fileStream != null)
				{
					fileStream.Close();
					fileStream = null;
				}
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(webRequest);
				throw ex;
			}
			catch
			{
				if (fileStream != null)
				{
					fileStream.Close();
					fileStream = null;
				}
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				WebClient.AbortRequest(webRequest);
				throw ex2;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return array5;
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0008A2FC File Offset: 0x000892FC
		private byte[] UploadValuesInternal(NameValueCollection data)
		{
			if (this.m_headers == null)
			{
				this.m_headers = new WebHeaderCollection(WebHeaderCollectionType.WebRequest);
			}
			string text = this.m_headers["Content-Type"];
			if (text != null && string.Compare(text, "application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new WebException(SR.GetString("net_webclient_ContentType"));
			}
			this.m_headers["Content-Type"] = "application/x-www-form-urlencoded";
			string text2 = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text3 in data.AllKeys)
			{
				stringBuilder.Append(text2);
				stringBuilder.Append(WebClient.UrlEncode(text3));
				stringBuilder.Append("=");
				stringBuilder.Append(WebClient.UrlEncode(data[text3]));
				text2 = "&";
			}
			byte[] bytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());
			this.m_ContentLength = (long)bytes.Length;
			return bytes;
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x0008A3EA File Offset: 0x000893EA
		public byte[] UploadValues(string address, NameValueCollection data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadValues(this.GetUri(address), null, data);
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x0008A409 File Offset: 0x00089409
		public byte[] UploadValues(Uri address, NameValueCollection data)
		{
			return this.UploadValues(address, null, data);
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0008A414 File Offset: 0x00089414
		public byte[] UploadValues(string address, string method, NameValueCollection data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadValues(this.GetUri(address), method, data);
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x0008A434 File Offset: 0x00089434
		public byte[] UploadValues(Uri address, string method, NameValueCollection data)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadValues", address + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			WebRequest webRequest = null;
			this.ClearWebClientState();
			byte[] array3;
			try
			{
				byte[] array = this.UploadValuesInternal(data);
				this.m_Method = method;
				webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.UploadBits(webRequest, null, array, null, null, null, null);
				byte[] array2 = this.DownloadBits(webRequest, null, null, null);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "UploadValues", address + ", " + method);
				}
				array3 = array2;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(webRequest);
				throw ex;
			}
			catch
			{
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				WebClient.AbortRequest(webRequest);
				throw ex2;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return array3;
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x0008A5A4 File Offset: 0x000895A4
		public string UploadString(string address, string data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadString(this.GetUri(address), null, data);
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x0008A5C3 File Offset: 0x000895C3
		public string UploadString(Uri address, string data)
		{
			return this.UploadString(address, null, data);
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x0008A5CE File Offset: 0x000895CE
		public string UploadString(string address, string method, string data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadString(this.GetUri(address), method, data);
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x0008A5F0 File Offset: 0x000895F0
		public string UploadString(Uri address, string method, string data)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadString", address + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.ClearWebClientState();
			string text;
			try
			{
				byte[] bytes = this.Encoding.GetBytes(data);
				WebRequest webRequest;
				byte[] array = this.UploadDataInternal(address, method, bytes, out webRequest);
				string @string = this.GuessDownloadEncoding(webRequest).GetString(array);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "UploadString", @string);
				}
				text = @string;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return text;
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x0008A6B4 File Offset: 0x000896B4
		public string DownloadString(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.DownloadString(this.GetUri(address));
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x0008A6D4 File Offset: 0x000896D4
		public string DownloadString(Uri address)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadString", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.ClearWebClientState();
			string text;
			try
			{
				WebRequest webRequest;
				byte[] array = this.DownloadDataInternal(address, out webRequest);
				string @string = this.GuessDownloadEncoding(webRequest).GetString(array);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "DownloadString", @string);
				}
				text = @string;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return text;
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x0008A760 File Offset: 0x00089760
		private static void AbortRequest(WebRequest request)
		{
			try
			{
				if (request != null)
				{
					request.Abort();
				}
			}
			catch (Exception ex)
			{
				if (ex is OutOfMemoryException || ex is StackOverflowException || ex is ThreadAbortException)
				{
					throw;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x0008A7B4 File Offset: 0x000897B4
		private void CopyHeadersTo(WebRequest request)
		{
			if (this.m_headers != null && request is HttpWebRequest)
			{
				string text = this.m_headers["Accept"];
				string text2 = this.m_headers["Connection"];
				string text3 = this.m_headers["Content-Type"];
				string text4 = this.m_headers["Expect"];
				string text5 = this.m_headers["Referer"];
				string text6 = this.m_headers["User-Agent"];
				this.m_headers.RemoveInternal("Accept");
				this.m_headers.RemoveInternal("Connection");
				this.m_headers.RemoveInternal("Content-Type");
				this.m_headers.RemoveInternal("Expect");
				this.m_headers.RemoveInternal("Referer");
				this.m_headers.RemoveInternal("User-Agent");
				request.Headers = this.m_headers;
				if (text != null && text.Length > 0)
				{
					((HttpWebRequest)request).Accept = text;
				}
				if (text2 != null && text2.Length > 0)
				{
					((HttpWebRequest)request).Connection = text2;
				}
				if (text3 != null && text3.Length > 0)
				{
					((HttpWebRequest)request).ContentType = text3;
				}
				if (text4 != null && text4.Length > 0)
				{
					((HttpWebRequest)request).Expect = text4;
				}
				if (text5 != null && text5.Length > 0)
				{
					((HttpWebRequest)request).Referer = text5;
				}
				if (text6 != null && text6.Length > 0)
				{
					((HttpWebRequest)request).UserAgent = text6;
				}
			}
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x0008A944 File Offset: 0x00089944
		private Uri GetUri(string path)
		{
			Uri uri;
			if (this.m_baseAddress != null)
			{
				if (!Uri.TryCreate(this.m_baseAddress, path, out uri))
				{
					return new Uri(Path.GetFullPath(path));
				}
			}
			else if (!Uri.TryCreate(path, UriKind.Absolute, out uri))
			{
				return new Uri(Path.GetFullPath(path));
			}
			return this.GetUri(uri);
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x0008A99C File Offset: 0x0008999C
		private Uri GetUri(Uri address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			Uri uri = address;
			if (!address.IsAbsoluteUri && this.m_baseAddress != null && !Uri.TryCreate(this.m_baseAddress, address, out uri))
			{
				return address;
			}
			if ((uri.Query == null || uri.Query == string.Empty) && this.m_requestParameters != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string text = string.Empty;
				for (int i = 0; i < this.m_requestParameters.Count; i++)
				{
					stringBuilder.Append(text + this.m_requestParameters.AllKeys[i] + "=" + this.m_requestParameters[i]);
					text = "&";
				}
				uri = new UriBuilder(uri)
				{
					Query = stringBuilder.ToString()
				}.Uri;
			}
			return uri;
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x0008AA7C File Offset: 0x00089A7C
		private static void DownloadBitsResponseCallback(IAsyncResult result)
		{
			WebClient.DownloadBitsState downloadBitsState = (WebClient.DownloadBitsState)result.AsyncState;
			WebRequest request = downloadBitsState.Request;
			Exception ex = null;
			try
			{
				WebResponse webResponse = downloadBitsState.WebClient.GetWebResponse(request, result);
				downloadBitsState.WebClient.m_WebResponse = webResponse;
				downloadBitsState.SetResponse(webResponse);
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex2;
				if (!(ex2 is WebException) && !(ex2 is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex2);
				}
				WebClient.AbortRequest(request);
				if (downloadBitsState != null && downloadBitsState.WriteStream != null)
				{
					downloadBitsState.WriteStream.Close();
				}
			}
			finally
			{
				if (ex != null)
				{
					downloadBitsState.CompletionDelegate(null, ex, downloadBitsState.AsyncOp);
				}
			}
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x0008AB5C File Offset: 0x00089B5C
		private static void DownloadBitsReadCallback(IAsyncResult result)
		{
			WebClient.DownloadBitsState downloadBitsState = (WebClient.DownloadBitsState)result.AsyncState;
			WebClient.DownloadBitsReadCallbackState(downloadBitsState, result);
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x0008AB7C File Offset: 0x00089B7C
		private static void DownloadBitsReadCallbackState(WebClient.DownloadBitsState state, IAsyncResult result)
		{
			Stream readStream = state.ReadStream;
			Exception ex = null;
			bool flag = false;
			try
			{
				int num = 0;
				if (readStream != null && readStream != Stream.Null)
				{
					num = readStream.EndRead(result);
				}
				flag = state.RetrieveBytes(ref num);
			}
			catch (Exception ex2)
			{
				flag = true;
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex2;
				state.InnerBuffer = null;
				if (!(ex2 is WebException) && !(ex2 is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex2);
				}
				WebClient.AbortRequest(state.Request);
				if (state != null && state.WriteStream != null)
				{
					state.WriteStream.Close();
				}
			}
			finally
			{
				if (flag)
				{
					if (ex == null)
					{
						state.Close();
					}
					state.CompletionDelegate(state.InnerBuffer, ex, state.AsyncOp);
				}
			}
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x0008AC6C File Offset: 0x00089C6C
		private byte[] DownloadBits(WebRequest request, Stream writeStream, CompletionDelegate completionDelegate, AsyncOperation asyncOp)
		{
			WebClient.DownloadBitsState downloadBitsState = new WebClient.DownloadBitsState(request, writeStream, completionDelegate, asyncOp, this.m_Progress, this);
			if (downloadBitsState.Async)
			{
				request.BeginGetResponse(new AsyncCallback(WebClient.DownloadBitsResponseCallback), downloadBitsState);
				return null;
			}
			WebResponse webResponse = (this.m_WebResponse = this.GetWebResponse(request));
			int num = downloadBitsState.SetResponse(webResponse);
			bool flag;
			do
			{
				flag = downloadBitsState.RetrieveBytes(ref num);
			}
			while (!flag);
			downloadBitsState.Close();
			return downloadBitsState.InnerBuffer;
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x0008ACE0 File Offset: 0x00089CE0
		private static void UploadBitsRequestCallback(IAsyncResult result)
		{
			WebClient.UploadBitsState uploadBitsState = (WebClient.UploadBitsState)result.AsyncState;
			WebRequest request = uploadBitsState.Request;
			Exception ex = null;
			try
			{
				Stream stream = request.EndGetRequestStream(result);
				uploadBitsState.SetRequestStream(stream);
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex2;
				if (!(ex2 is WebException) && !(ex2 is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex2);
				}
				WebClient.AbortRequest(request);
				if (uploadBitsState != null && uploadBitsState.ReadStream != null)
				{
					uploadBitsState.ReadStream.Close();
				}
			}
			finally
			{
				if (ex != null)
				{
					uploadBitsState.CompletionDelegate(null, ex, uploadBitsState.AsyncOp);
				}
			}
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x0008ADAC File Offset: 0x00089DAC
		private static void UploadBitsWriteCallback(IAsyncResult result)
		{
			WebClient.UploadBitsState uploadBitsState = (WebClient.UploadBitsState)result.AsyncState;
			Stream writeStream = uploadBitsState.WriteStream;
			Exception ex = null;
			bool flag = false;
			try
			{
				writeStream.EndWrite(result);
				flag = uploadBitsState.WriteBytes();
			}
			catch (Exception ex2)
			{
				flag = true;
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex2;
				if (!(ex2 is WebException) && !(ex2 is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex2);
				}
				WebClient.AbortRequest(uploadBitsState.Request);
				if (uploadBitsState != null && uploadBitsState.ReadStream != null)
				{
					uploadBitsState.ReadStream.Close();
				}
			}
			finally
			{
				if (flag)
				{
					if (ex == null)
					{
						uploadBitsState.Close();
					}
					uploadBitsState.CompletionDelegate(null, ex, uploadBitsState.AsyncOp);
				}
			}
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x0008AE8C File Offset: 0x00089E8C
		private void UploadBits(WebRequest request, Stream readStream, byte[] buffer, byte[] header, byte[] footer, CompletionDelegate completionDelegate, AsyncOperation asyncOp)
		{
			if (request.RequestUri.Scheme == Uri.UriSchemeFile)
			{
				footer = (header = null);
			}
			WebClient.UploadBitsState uploadBitsState = new WebClient.UploadBitsState(request, readStream, buffer, header, footer, completionDelegate, asyncOp, this.m_Progress, this);
			if (uploadBitsState.Async)
			{
				request.BeginGetRequestStream(new AsyncCallback(WebClient.UploadBitsRequestCallback), uploadBitsState);
				return;
			}
			Stream requestStream = request.GetRequestStream();
			uploadBitsState.SetRequestStream(requestStream);
			while (!uploadBitsState.WriteBytes())
			{
			}
			uploadBitsState.Close();
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x0008AF10 File Offset: 0x00089F10
		private Encoding GuessDownloadEncoding(WebRequest request)
		{
			try
			{
				string text;
				if ((text = request.ContentType) == null)
				{
					return this.Encoding;
				}
				text = text.ToLower(CultureInfo.InvariantCulture);
				string[] array = text.Split(new char[] { ';', '=', ' ' });
				bool flag = false;
				foreach (string text2 in array)
				{
					if (text2 == "charset")
					{
						flag = true;
					}
					else if (flag)
					{
						return Encoding.GetEncoding(text2);
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
			}
			catch
			{
			}
			return this.Encoding;
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x0008AFE0 File Offset: 0x00089FE0
		private string MapToDefaultMethod(Uri address)
		{
			Uri uri;
			if (!address.IsAbsoluteUri && this.m_baseAddress != null)
			{
				uri = new Uri(this.m_baseAddress, address);
			}
			else
			{
				uri = address;
			}
			if (uri.Scheme.ToLower(CultureInfo.InvariantCulture) == "ftp")
			{
				return "STOR";
			}
			return "POST";
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x0008B03B File Offset: 0x0008A03B
		private static string UrlEncode(string str)
		{
			if (str == null)
			{
				return null;
			}
			return WebClient.UrlEncode(str, Encoding.UTF8);
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x0008B04D File Offset: 0x0008A04D
		private static string UrlEncode(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			return Encoding.ASCII.GetString(WebClient.UrlEncodeToBytes(str, e));
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x0008B068 File Offset: 0x0008A068
		private static byte[] UrlEncodeToBytes(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			byte[] bytes = e.GetBytes(str);
			return WebClient.UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false);
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x0008B090 File Offset: 0x0008A090
		private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				char c = (char)bytes[offset + i];
				if (c == ' ')
				{
					num++;
				}
				else if (!WebClient.IsSafe(c))
				{
					num2++;
				}
			}
			if (!alwaysCreateReturnValue && num == 0 && num2 == 0)
			{
				return bytes;
			}
			byte[] array = new byte[count + num2 * 2];
			int num3 = 0;
			for (int j = 0; j < count; j++)
			{
				byte b = bytes[offset + j];
				char c2 = (char)b;
				if (WebClient.IsSafe(c2))
				{
					array[num3++] = b;
				}
				else if (c2 == ' ')
				{
					array[num3++] = 43;
				}
				else
				{
					array[num3++] = 37;
					array[num3++] = (byte)WebClient.IntToHex((b >> 4) & 15);
					array[num3++] = (byte)WebClient.IntToHex((int)(b & 15));
				}
			}
			return array;
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x0008B165 File Offset: 0x0008A165
		private static char IntToHex(int n)
		{
			if (n <= 9)
			{
				return (char)(n + 48);
			}
			return (char)(n - 10 + 97);
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x0008B17C File Offset: 0x0008A17C
		private static bool IsSafe(char ch)
		{
			if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
			{
				return true;
			}
			if (ch != '!')
			{
				switch (ch)
				{
				case '\'':
				case '(':
				case ')':
				case '*':
				case '-':
				case '.':
					return true;
				case '+':
				case ',':
					break;
				default:
					if (ch == '_')
					{
						return true;
					}
					break;
				}
				return false;
			}
			return true;
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x0008B1E1 File Offset: 0x0008A1E1
		private void InvokeOperationCompleted(AsyncOperation asyncOp, SendOrPostCallback callback, AsyncCompletedEventArgs eventArgs)
		{
			if (Interlocked.CompareExchange<AsyncOperation>(ref this.m_AsyncOp, null, asyncOp) == asyncOp)
			{
				this.CompleteWebClientState();
				asyncOp.PostOperationCompleted(callback, eventArgs);
			}
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x0008B201 File Offset: 0x0008A201
		private bool AnotherCallInProgress(int callNesting)
		{
			return callNesting > 1;
		}

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06002354 RID: 9044 RVA: 0x0008B207 File Offset: 0x0008A207
		// (remove) Token: 0x06002355 RID: 9045 RVA: 0x0008B220 File Offset: 0x0008A220
		public event OpenReadCompletedEventHandler OpenReadCompleted;

		// Token: 0x06002356 RID: 9046 RVA: 0x0008B239 File Offset: 0x0008A239
		protected virtual void OnOpenReadCompleted(OpenReadCompletedEventArgs e)
		{
			if (this.OpenReadCompleted != null)
			{
				this.OpenReadCompleted(this, e);
			}
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x0008B250 File Offset: 0x0008A250
		private void OpenReadOperationCompleted(object arg)
		{
			this.OnOpenReadCompleted((OpenReadCompletedEventArgs)arg);
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x0008B260 File Offset: 0x0008A260
		private void OpenReadAsyncCallback(IAsyncResult result)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)result;
			AsyncOperation asyncOperation = (AsyncOperation)lazyAsyncResult.AsyncState;
			WebRequest webRequest = (WebRequest)lazyAsyncResult.AsyncObject;
			Stream stream = null;
			Exception ex = null;
			try
			{
				WebResponse webResponse = (this.m_WebResponse = this.GetWebResponse(webRequest, result));
				stream = webResponse.GetResponseStream();
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex2;
				if (!(ex2 is WebException) && !(ex2 is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex2);
				}
			}
			catch
			{
				ex = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			OpenReadCompletedEventArgs openReadCompletedEventArgs = new OpenReadCompletedEventArgs(stream, ex, this.m_Cancelled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this.openReadOperationCompleted, openReadCompletedEventArgs);
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x0008B360 File Offset: 0x0008A360
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void OpenReadAsync(Uri address)
		{
			this.OpenReadAsync(address, null);
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x0008B36C File Offset: 0x0008A36C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void OpenReadAsync(Uri address, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "OpenReadAsync", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				webRequest.BeginGetResponse(new AsyncCallback(this.OpenReadAsyncCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				OpenReadCompletedEventArgs openReadCompletedEventArgs = new OpenReadCompletedEventArgs(null, ex, this.m_Cancelled, asyncOperation.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOperation, this.openReadOperationCompleted, openReadCompletedEventArgs);
			}
			catch
			{
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				OpenReadCompletedEventArgs openReadCompletedEventArgs2 = new OpenReadCompletedEventArgs(null, ex2, this.m_Cancelled, asyncOperation.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOperation, this.openReadOperationCompleted, openReadCompletedEventArgs2);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "OpenReadAsync", null);
			}
		}

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x0600235B RID: 9051 RVA: 0x0008B4D0 File Offset: 0x0008A4D0
		// (remove) Token: 0x0600235C RID: 9052 RVA: 0x0008B4E9 File Offset: 0x0008A4E9
		public event OpenWriteCompletedEventHandler OpenWriteCompleted;

		// Token: 0x0600235D RID: 9053 RVA: 0x0008B502 File Offset: 0x0008A502
		protected virtual void OnOpenWriteCompleted(OpenWriteCompletedEventArgs e)
		{
			if (this.OpenWriteCompleted != null)
			{
				this.OpenWriteCompleted(this, e);
			}
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x0008B519 File Offset: 0x0008A519
		private void OpenWriteOperationCompleted(object arg)
		{
			this.OnOpenWriteCompleted((OpenWriteCompletedEventArgs)arg);
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x0008B528 File Offset: 0x0008A528
		private void OpenWriteAsyncCallback(IAsyncResult result)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)result;
			AsyncOperation asyncOperation = (AsyncOperation)lazyAsyncResult.AsyncState;
			WebRequest webRequest = (WebRequest)lazyAsyncResult.AsyncObject;
			WebClient.WebClientWriteStream webClientWriteStream = null;
			Exception ex = null;
			try
			{
				webClientWriteStream = new WebClient.WebClientWriteStream(webRequest.EndGetRequestStream(result), webRequest, this);
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex2;
				if (!(ex2 is WebException) && !(ex2 is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex2);
				}
			}
			catch
			{
				ex = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			OpenWriteCompletedEventArgs openWriteCompletedEventArgs = new OpenWriteCompletedEventArgs(webClientWriteStream, ex, this.m_Cancelled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this.openWriteOperationCompleted, openWriteCompletedEventArgs);
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0008B618 File Offset: 0x0008A618
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void OpenWriteAsync(Uri address)
		{
			this.OpenWriteAsync(address, null, null);
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0008B623 File Offset: 0x0008A623
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void OpenWriteAsync(Uri address, string method)
		{
			this.OpenWriteAsync(address, method, null);
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0008B630 File Offset: 0x0008A630
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void OpenWriteAsync(Uri address, string method, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "OpenWriteAsync", address + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				this.m_Method = method;
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				webRequest.BeginGetRequestStream(new AsyncCallback(this.OpenWriteAsyncCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				OpenWriteCompletedEventArgs openWriteCompletedEventArgs = new OpenWriteCompletedEventArgs(null, ex, this.m_Cancelled, asyncOperation.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOperation, this.openWriteOperationCompleted, openWriteCompletedEventArgs);
			}
			catch
			{
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				OpenWriteCompletedEventArgs openWriteCompletedEventArgs2 = new OpenWriteCompletedEventArgs(null, ex2, this.m_Cancelled, asyncOperation.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOperation, this.openWriteOperationCompleted, openWriteCompletedEventArgs2);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "OpenWriteAsync", null);
			}
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06002363 RID: 9059 RVA: 0x0008B7B0 File Offset: 0x0008A7B0
		// (remove) Token: 0x06002364 RID: 9060 RVA: 0x0008B7C9 File Offset: 0x0008A7C9
		public event DownloadStringCompletedEventHandler DownloadStringCompleted;

		// Token: 0x06002365 RID: 9061 RVA: 0x0008B7E2 File Offset: 0x0008A7E2
		protected virtual void OnDownloadStringCompleted(DownloadStringCompletedEventArgs e)
		{
			if (this.DownloadStringCompleted != null)
			{
				this.DownloadStringCompleted(this, e);
			}
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x0008B7F9 File Offset: 0x0008A7F9
		private void DownloadStringOperationCompleted(object arg)
		{
			this.OnDownloadStringCompleted((DownloadStringCompletedEventArgs)arg);
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x0008B808 File Offset: 0x0008A808
		private void DownloadStringAsyncCallback(byte[] returnBytes, Exception exception, AsyncOperation asyncOp)
		{
			string text = null;
			try
			{
				if (returnBytes != null)
				{
					text = this.GuessDownloadEncoding(this.m_WebRequest).GetString(returnBytes);
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				exception = ex;
			}
			catch
			{
				exception = new Exception(SR.GetString("net_nonClsCompliantException"));
			}
			DownloadStringCompletedEventArgs downloadStringCompletedEventArgs = new DownloadStringCompletedEventArgs(text, exception, this.m_Cancelled, asyncOp.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOp, this.downloadStringOperationCompleted, downloadStringCompletedEventArgs);
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x0008B8A4 File Offset: 0x0008A8A4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void DownloadStringAsync(Uri address)
		{
			this.DownloadStringAsync(address, null);
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x0008B8B0 File Offset: 0x0008A8B0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void DownloadStringAsync(Uri address, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadStringAsync", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.DownloadBits(webRequest, null, new CompletionDelegate(this.DownloadStringAsyncCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				this.DownloadStringAsyncCallback(null, ex, asyncOperation);
			}
			catch
			{
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				this.DownloadStringAsyncCallback(null, ex2, asyncOperation);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "DownloadStringAsync", "");
			}
		}

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x0600236A RID: 9066 RVA: 0x0008B9E0 File Offset: 0x0008A9E0
		// (remove) Token: 0x0600236B RID: 9067 RVA: 0x0008B9F9 File Offset: 0x0008A9F9
		public event DownloadDataCompletedEventHandler DownloadDataCompleted;

		// Token: 0x0600236C RID: 9068 RVA: 0x0008BA12 File Offset: 0x0008AA12
		protected virtual void OnDownloadDataCompleted(DownloadDataCompletedEventArgs e)
		{
			if (this.DownloadDataCompleted != null)
			{
				this.DownloadDataCompleted(this, e);
			}
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x0008BA29 File Offset: 0x0008AA29
		private void DownloadDataOperationCompleted(object arg)
		{
			this.OnDownloadDataCompleted((DownloadDataCompletedEventArgs)arg);
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x0008BA38 File Offset: 0x0008AA38
		private void DownloadDataAsyncCallback(byte[] returnBytes, Exception exception, AsyncOperation asyncOp)
		{
			DownloadDataCompletedEventArgs downloadDataCompletedEventArgs = new DownloadDataCompletedEventArgs(returnBytes, exception, this.m_Cancelled, asyncOp.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOp, this.downloadDataOperationCompleted, downloadDataCompletedEventArgs);
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x0008BA67 File Offset: 0x0008AA67
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void DownloadDataAsync(Uri address)
		{
			this.DownloadDataAsync(address, null);
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x0008BA74 File Offset: 0x0008AA74
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void DownloadDataAsync(Uri address, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadDataAsync", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.DownloadBits(webRequest, null, new CompletionDelegate(this.DownloadDataAsyncCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				this.DownloadDataAsyncCallback(null, ex, asyncOperation);
			}
			catch
			{
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				this.DownloadDataAsyncCallback(null, ex2, asyncOperation);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "DownloadDataAsync", null);
			}
		}

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06002371 RID: 9073 RVA: 0x0008BBA0 File Offset: 0x0008ABA0
		// (remove) Token: 0x06002372 RID: 9074 RVA: 0x0008BBB9 File Offset: 0x0008ABB9
		public event AsyncCompletedEventHandler DownloadFileCompleted;

		// Token: 0x06002373 RID: 9075 RVA: 0x0008BBD2 File Offset: 0x0008ABD2
		protected virtual void OnDownloadFileCompleted(AsyncCompletedEventArgs e)
		{
			if (this.DownloadFileCompleted != null)
			{
				this.DownloadFileCompleted(this, e);
			}
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x0008BBE9 File Offset: 0x0008ABE9
		private void DownloadFileOperationCompleted(object arg)
		{
			this.OnDownloadFileCompleted((AsyncCompletedEventArgs)arg);
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x0008BBF8 File Offset: 0x0008ABF8
		private void DownloadFileAsyncCallback(byte[] returnBytes, Exception exception, AsyncOperation asyncOp)
		{
			AsyncCompletedEventArgs asyncCompletedEventArgs = new AsyncCompletedEventArgs(exception, this.m_Cancelled, asyncOp.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOp, this.downloadFileOperationCompleted, asyncCompletedEventArgs);
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x0008BC26 File Offset: 0x0008AC26
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void DownloadFileAsync(Uri address, string fileName)
		{
			this.DownloadFileAsync(address, fileName, null);
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x0008BC34 File Offset: 0x0008AC34
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void DownloadFileAsync(Uri address, string fileName, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadFileAsync", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			FileStream fileStream = null;
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.DownloadBits(webRequest, fileStream, new CompletionDelegate(this.DownloadFileAsyncCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (fileStream != null)
				{
					fileStream.Close();
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				this.DownloadFileAsyncCallback(null, ex, asyncOperation);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "DownloadFileAsync", null);
			}
		}

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06002378 RID: 9080 RVA: 0x0008BD4C File Offset: 0x0008AD4C
		// (remove) Token: 0x06002379 RID: 9081 RVA: 0x0008BD65 File Offset: 0x0008AD65
		public event UploadStringCompletedEventHandler UploadStringCompleted;

		// Token: 0x0600237A RID: 9082 RVA: 0x0008BD7E File Offset: 0x0008AD7E
		protected virtual void OnUploadStringCompleted(UploadStringCompletedEventArgs e)
		{
			if (this.UploadStringCompleted != null)
			{
				this.UploadStringCompleted(this, e);
			}
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x0008BD95 File Offset: 0x0008AD95
		private void UploadStringOperationCompleted(object arg)
		{
			this.OnUploadStringCompleted((UploadStringCompletedEventArgs)arg);
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x0008BDA4 File Offset: 0x0008ADA4
		private void UploadStringAsyncWriteCallback(byte[] returnBytes, Exception exception, AsyncOperation asyncOp)
		{
			if (exception != null)
			{
				UploadStringCompletedEventArgs uploadStringCompletedEventArgs = new UploadStringCompletedEventArgs(null, exception, this.m_Cancelled, asyncOp.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOp, this.uploadStringOperationCompleted, uploadStringCompletedEventArgs);
			}
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x0008BDD8 File Offset: 0x0008ADD8
		private void UploadStringAsyncReadCallback(byte[] returnBytes, Exception exception, AsyncOperation asyncOp)
		{
			string text = null;
			try
			{
				if (returnBytes != null)
				{
					text = this.GuessDownloadEncoding(this.m_WebRequest).GetString(returnBytes);
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				exception = ex;
			}
			catch
			{
				exception = new Exception(SR.GetString("net_nonClsCompliantException"));
			}
			UploadStringCompletedEventArgs uploadStringCompletedEventArgs = new UploadStringCompletedEventArgs(text, exception, this.m_Cancelled, asyncOp.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOp, this.uploadStringOperationCompleted, uploadStringCompletedEventArgs);
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x0008BE74 File Offset: 0x0008AE74
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadStringAsync(Uri address, string data)
		{
			this.UploadStringAsync(address, null, data, null);
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x0008BE80 File Offset: 0x0008AE80
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadStringAsync(Uri address, string method, string data)
		{
			this.UploadStringAsync(address, method, data, null);
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x0008BE8C File Offset: 0x0008AE8C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadStringAsync(Uri address, string method, string data, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadStringAsync", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				byte[] bytes = this.Encoding.GetBytes(data);
				this.m_Method = method;
				this.m_ContentLength = (long)bytes.Length;
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.UploadBits(webRequest, null, bytes, null, null, new CompletionDelegate(this.UploadStringAsyncWriteCallback), asyncOperation);
				this.DownloadBits(webRequest, null, new CompletionDelegate(this.UploadStringAsyncReadCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				this.UploadStringAsyncWriteCallback(null, ex, asyncOperation);
			}
			catch
			{
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				this.UploadStringAsyncWriteCallback(null, ex2, asyncOperation);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "UploadStringAsync", null);
			}
		}

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06002381 RID: 9089 RVA: 0x0008C00C File Offset: 0x0008B00C
		// (remove) Token: 0x06002382 RID: 9090 RVA: 0x0008C025 File Offset: 0x0008B025
		public event UploadDataCompletedEventHandler UploadDataCompleted;

		// Token: 0x06002383 RID: 9091 RVA: 0x0008C03E File Offset: 0x0008B03E
		protected virtual void OnUploadDataCompleted(UploadDataCompletedEventArgs e)
		{
			if (this.UploadDataCompleted != null)
			{
				this.UploadDataCompleted(this, e);
			}
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x0008C055 File Offset: 0x0008B055
		private void UploadDataOperationCompleted(object arg)
		{
			this.OnUploadDataCompleted((UploadDataCompletedEventArgs)arg);
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x0008C064 File Offset: 0x0008B064
		private void UploadDataAsyncWriteCallback(byte[] returnBytes, Exception exception, AsyncOperation asyncOp)
		{
			if (exception != null)
			{
				UploadDataCompletedEventArgs uploadDataCompletedEventArgs = new UploadDataCompletedEventArgs(returnBytes, exception, this.m_Cancelled, asyncOp.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOp, this.uploadDataOperationCompleted, uploadDataCompletedEventArgs);
			}
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x0008C098 File Offset: 0x0008B098
		private void UploadDataAsyncReadCallback(byte[] returnBytes, Exception exception, AsyncOperation asyncOp)
		{
			UploadDataCompletedEventArgs uploadDataCompletedEventArgs = new UploadDataCompletedEventArgs(returnBytes, exception, this.m_Cancelled, asyncOp.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOp, this.uploadDataOperationCompleted, uploadDataCompletedEventArgs);
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x0008C0C7 File Offset: 0x0008B0C7
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadDataAsync(Uri address, byte[] data)
		{
			this.UploadDataAsync(address, null, data, null);
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x0008C0D3 File Offset: 0x0008B0D3
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadDataAsync(Uri address, string method, byte[] data)
		{
			this.UploadDataAsync(address, method, data, null);
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x0008C0E0 File Offset: 0x0008B0E0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadDataAsync(Uri address, string method, byte[] data, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadDataAsync", address + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				this.m_Method = method;
				this.m_ContentLength = (long)data.Length;
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.UploadBits(webRequest, null, data, null, null, new CompletionDelegate(this.UploadDataAsyncWriteCallback), asyncOperation);
				this.DownloadBits(webRequest, null, new CompletionDelegate(this.UploadDataAsyncReadCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				this.UploadDataAsyncWriteCallback(null, ex, asyncOperation);
			}
			catch
			{
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				this.UploadDataAsyncWriteCallback(null, ex2, asyncOperation);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "UploadDataAsync", null);
			}
		}

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x0600238A RID: 9098 RVA: 0x0008C25C File Offset: 0x0008B25C
		// (remove) Token: 0x0600238B RID: 9099 RVA: 0x0008C275 File Offset: 0x0008B275
		public event UploadFileCompletedEventHandler UploadFileCompleted;

		// Token: 0x0600238C RID: 9100 RVA: 0x0008C28E File Offset: 0x0008B28E
		protected virtual void OnUploadFileCompleted(UploadFileCompletedEventArgs e)
		{
			if (this.UploadFileCompleted != null)
			{
				this.UploadFileCompleted(this, e);
			}
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x0008C2A5 File Offset: 0x0008B2A5
		private void UploadFileOperationCompleted(object arg)
		{
			this.OnUploadFileCompleted((UploadFileCompletedEventArgs)arg);
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x0008C2B4 File Offset: 0x0008B2B4
		private void UploadFileAsyncWriteCallback(byte[] returnBytes, Exception exception, AsyncOperation asyncOp)
		{
			if (exception != null)
			{
				UploadFileCompletedEventArgs uploadFileCompletedEventArgs = new UploadFileCompletedEventArgs(returnBytes, exception, this.m_Cancelled, asyncOp.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOp, this.uploadFileOperationCompleted, uploadFileCompletedEventArgs);
			}
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x0008C2E8 File Offset: 0x0008B2E8
		private void UploadFileAsyncReadCallback(byte[] returnBytes, Exception exception, AsyncOperation asyncOp)
		{
			UploadFileCompletedEventArgs uploadFileCompletedEventArgs = new UploadFileCompletedEventArgs(returnBytes, exception, this.m_Cancelled, asyncOp.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOp, this.uploadFileOperationCompleted, uploadFileCompletedEventArgs);
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x0008C317 File Offset: 0x0008B317
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadFileAsync(Uri address, string fileName)
		{
			this.UploadFileAsync(address, null, fileName, null);
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x0008C323 File Offset: 0x0008B323
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadFileAsync(Uri address, string method, string fileName)
		{
			this.UploadFileAsync(address, method, fileName, null);
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x0008C330 File Offset: 0x0008B330
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadFileAsync(Uri address, string method, string fileName, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadFileAsync", address + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			FileStream fileStream = null;
			try
			{
				this.m_Method = method;
				byte[] array = null;
				byte[] array2 = null;
				byte[] array3 = null;
				Uri uri = this.GetUri(address);
				bool flag = uri.Scheme != Uri.UriSchemeFile;
				this.OpenFileInternal(flag, fileName, ref fileStream, ref array3, ref array, ref array2);
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(uri));
				this.UploadBits(webRequest, fileStream, array3, array, array2, new CompletionDelegate(this.UploadFileAsyncWriteCallback), asyncOperation);
				this.DownloadBits(webRequest, null, new CompletionDelegate(this.UploadFileAsyncReadCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (fileStream != null)
				{
					fileStream.Close();
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				this.UploadFileAsyncWriteCallback(null, ex, asyncOperation);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "UploadFileAsync", null);
			}
		}

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06002393 RID: 9107 RVA: 0x0008C4B0 File Offset: 0x0008B4B0
		// (remove) Token: 0x06002394 RID: 9108 RVA: 0x0008C4C9 File Offset: 0x0008B4C9
		public event UploadValuesCompletedEventHandler UploadValuesCompleted;

		// Token: 0x06002395 RID: 9109 RVA: 0x0008C4E2 File Offset: 0x0008B4E2
		protected virtual void OnUploadValuesCompleted(UploadValuesCompletedEventArgs e)
		{
			if (this.UploadValuesCompleted != null)
			{
				this.UploadValuesCompleted(this, e);
			}
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x0008C4F9 File Offset: 0x0008B4F9
		private void UploadValuesOperationCompleted(object arg)
		{
			this.OnUploadValuesCompleted((UploadValuesCompletedEventArgs)arg);
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x0008C508 File Offset: 0x0008B508
		private void UploadValuesAsyncWriteCallback(byte[] returnBytes, Exception exception, AsyncOperation asyncOp)
		{
			if (exception != null)
			{
				UploadValuesCompletedEventArgs uploadValuesCompletedEventArgs = new UploadValuesCompletedEventArgs(returnBytes, exception, this.m_Cancelled, asyncOp.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOp, this.uploadValuesOperationCompleted, uploadValuesCompletedEventArgs);
			}
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x0008C53C File Offset: 0x0008B53C
		private void UploadValuesAsyncReadCallback(byte[] returnBytes, Exception exception, AsyncOperation asyncOp)
		{
			UploadValuesCompletedEventArgs uploadValuesCompletedEventArgs = new UploadValuesCompletedEventArgs(returnBytes, exception, this.m_Cancelled, asyncOp.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOp, this.uploadValuesOperationCompleted, uploadValuesCompletedEventArgs);
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x0008C56B File Offset: 0x0008B56B
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadValuesAsync(Uri address, NameValueCollection data)
		{
			this.UploadValuesAsync(address, null, data, null);
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0008C577 File Offset: 0x0008B577
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadValuesAsync(Uri address, string method, NameValueCollection data)
		{
			this.UploadValuesAsync(address, method, data, null);
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0008C584 File Offset: 0x0008B584
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadValuesAsync(Uri address, string method, NameValueCollection data, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadValuesAsync", address + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				byte[] array = this.UploadValuesInternal(data);
				this.m_Method = method;
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.UploadBits(webRequest, null, array, null, null, new CompletionDelegate(this.UploadValuesAsyncWriteCallback), asyncOperation);
				this.DownloadBits(webRequest, null, new CompletionDelegate(this.UploadValuesAsyncReadCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				this.UploadValuesAsyncWriteCallback(null, ex, asyncOperation);
			}
			catch
			{
				Exception ex2 = new WebException(SR.GetString("net_webclient"), new Exception(SR.GetString("net_nonClsCompliantException")));
				this.UploadValuesAsyncWriteCallback(null, ex2, asyncOperation);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "UploadValuesAsync", null);
			}
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x0008C700 File Offset: 0x0008B700
		public void CancelAsync()
		{
			WebRequest webRequest = this.m_WebRequest;
			this.m_Cancelled = true;
			WebClient.AbortRequest(webRequest);
		}

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x0600239D RID: 9117 RVA: 0x0008C721 File Offset: 0x0008B721
		// (remove) Token: 0x0600239E RID: 9118 RVA: 0x0008C73A File Offset: 0x0008B73A
		public event DownloadProgressChangedEventHandler DownloadProgressChanged;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x0600239F RID: 9119 RVA: 0x0008C753 File Offset: 0x0008B753
		// (remove) Token: 0x060023A0 RID: 9120 RVA: 0x0008C76C File Offset: 0x0008B76C
		public event UploadProgressChangedEventHandler UploadProgressChanged;

		// Token: 0x060023A1 RID: 9121 RVA: 0x0008C785 File Offset: 0x0008B785
		protected virtual void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
		{
			if (this.DownloadProgressChanged != null)
			{
				this.DownloadProgressChanged(this, e);
			}
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x0008C79C File Offset: 0x0008B79C
		protected virtual void OnUploadProgressChanged(UploadProgressChangedEventArgs e)
		{
			if (this.UploadProgressChanged != null)
			{
				this.UploadProgressChanged(this, e);
			}
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x0008C7B3 File Offset: 0x0008B7B3
		private void ReportDownloadProgressChanged(object arg)
		{
			this.OnDownloadProgressChanged((DownloadProgressChangedEventArgs)arg);
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x0008C7C1 File Offset: 0x0008B7C1
		private void ReportUploadProgressChanged(object arg)
		{
			this.OnUploadProgressChanged((UploadProgressChangedEventArgs)arg);
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x0008C7D0 File Offset: 0x0008B7D0
		private void PostProgressChanged(AsyncOperation asyncOp, WebClient.ProgressData progress)
		{
			if (asyncOp != null && progress.BytesSent + progress.BytesReceived > 0L)
			{
				int num;
				if (progress.HasUploadPhase)
				{
					if (progress.TotalBytesToReceive < 0L && progress.BytesReceived == 0L)
					{
						num = ((progress.TotalBytesToSend < 0L) ? 0 : ((progress.TotalBytesToSend == 0L) ? 50 : ((int)(50L * progress.BytesSent / progress.TotalBytesToSend))));
					}
					else
					{
						num = ((progress.TotalBytesToSend < 0L) ? 50 : ((progress.TotalBytesToReceive == 0L) ? 100 : ((int)(50L * progress.BytesReceived / progress.TotalBytesToReceive + 50L))));
					}
					asyncOp.Post(this.reportUploadProgressChanged, new UploadProgressChangedEventArgs(num, asyncOp.UserSuppliedState, progress.BytesSent, progress.TotalBytesToSend, progress.BytesReceived, progress.TotalBytesToReceive));
					return;
				}
				num = ((progress.TotalBytesToReceive < 0L) ? 0 : ((progress.TotalBytesToReceive == 0L) ? 100 : ((int)(100L * progress.BytesReceived / progress.TotalBytesToReceive))));
				asyncOp.Post(this.reportDownloadProgressChanged, new DownloadProgressChangedEventArgs(num, asyncOp.UserSuppliedState, progress.BytesReceived, progress.TotalBytesToReceive));
			}
		}

		// Token: 0x0400240D RID: 9229
		private const int DefaultCopyBufferLength = 8192;

		// Token: 0x0400240E RID: 9230
		private const int DefaultDownloadBufferLength = 65536;

		// Token: 0x0400240F RID: 9231
		private const string DefaultUploadFileContentType = "application/octet-stream";

		// Token: 0x04002410 RID: 9232
		private const string UploadFileContentType = "multipart/form-data";

		// Token: 0x04002411 RID: 9233
		private const string UploadValuesContentType = "application/x-www-form-urlencoded";

		// Token: 0x04002412 RID: 9234
		private Uri m_baseAddress;

		// Token: 0x04002413 RID: 9235
		private ICredentials m_credentials;

		// Token: 0x04002414 RID: 9236
		private WebHeaderCollection m_headers;

		// Token: 0x04002415 RID: 9237
		private NameValueCollection m_requestParameters;

		// Token: 0x04002416 RID: 9238
		private WebResponse m_WebResponse;

		// Token: 0x04002417 RID: 9239
		private WebRequest m_WebRequest;

		// Token: 0x04002418 RID: 9240
		private Encoding m_Encoding = Encoding.Default;

		// Token: 0x04002419 RID: 9241
		private string m_Method;

		// Token: 0x0400241A RID: 9242
		private long m_ContentLength = -1L;

		// Token: 0x0400241B RID: 9243
		private bool m_InitWebClientAsync;

		// Token: 0x0400241C RID: 9244
		private bool m_Cancelled;

		// Token: 0x0400241D RID: 9245
		private WebClient.ProgressData m_Progress;

		// Token: 0x0400241E RID: 9246
		private IWebProxy m_Proxy;

		// Token: 0x0400241F RID: 9247
		private bool m_ProxySet;

		// Token: 0x04002420 RID: 9248
		private RequestCachePolicy m_CachePolicy;

		// Token: 0x04002421 RID: 9249
		private int m_CallNesting;

		// Token: 0x04002422 RID: 9250
		private AsyncOperation m_AsyncOp;

		// Token: 0x04002424 RID: 9252
		private SendOrPostCallback openReadOperationCompleted;

		// Token: 0x04002426 RID: 9254
		private SendOrPostCallback openWriteOperationCompleted;

		// Token: 0x04002428 RID: 9256
		private SendOrPostCallback downloadStringOperationCompleted;

		// Token: 0x0400242A RID: 9258
		private SendOrPostCallback downloadDataOperationCompleted;

		// Token: 0x0400242C RID: 9260
		private SendOrPostCallback downloadFileOperationCompleted;

		// Token: 0x0400242E RID: 9262
		private SendOrPostCallback uploadStringOperationCompleted;

		// Token: 0x04002430 RID: 9264
		private SendOrPostCallback uploadDataOperationCompleted;

		// Token: 0x04002432 RID: 9266
		private SendOrPostCallback uploadFileOperationCompleted;

		// Token: 0x04002434 RID: 9268
		private SendOrPostCallback uploadValuesOperationCompleted;

		// Token: 0x04002437 RID: 9271
		private SendOrPostCallback reportDownloadProgressChanged;

		// Token: 0x04002438 RID: 9272
		private SendOrPostCallback reportUploadProgressChanged;

		// Token: 0x02000485 RID: 1157
		private class ProgressData
		{
			// Token: 0x060023A6 RID: 9126 RVA: 0x0008C8FC File Offset: 0x0008B8FC
			internal void Reset()
			{
				this.BytesSent = 0L;
				this.TotalBytesToSend = -1L;
				this.BytesReceived = 0L;
				this.TotalBytesToReceive = -1L;
				this.HasUploadPhase = false;
			}

			// Token: 0x04002439 RID: 9273
			internal long BytesSent;

			// Token: 0x0400243A RID: 9274
			internal long TotalBytesToSend = -1L;

			// Token: 0x0400243B RID: 9275
			internal long BytesReceived;

			// Token: 0x0400243C RID: 9276
			internal long TotalBytesToReceive = -1L;

			// Token: 0x0400243D RID: 9277
			internal bool HasUploadPhase;
		}

		// Token: 0x02000486 RID: 1158
		private class DownloadBitsState
		{
			// Token: 0x060023A8 RID: 9128 RVA: 0x0008C93D File Offset: 0x0008B93D
			internal DownloadBitsState(WebRequest request, Stream writeStream, CompletionDelegate completionDelegate, AsyncOperation asyncOp, WebClient.ProgressData progress, WebClient webClient)
			{
				this.WriteStream = writeStream;
				this.Request = request;
				this.AsyncOp = asyncOp;
				this.CompletionDelegate = completionDelegate;
				this.WebClient = webClient;
				this.Progress = progress;
			}

			// Token: 0x1700075A RID: 1882
			// (get) Token: 0x060023A9 RID: 9129 RVA: 0x0008C972 File Offset: 0x0008B972
			internal bool Async
			{
				get
				{
					return this.AsyncOp != null;
				}
			}

			// Token: 0x060023AA RID: 9130 RVA: 0x0008C980 File Offset: 0x0008B980
			internal int SetResponse(WebResponse response)
			{
				this.ContentLength = response.ContentLength;
				if (this.ContentLength == -1L || this.ContentLength > 65536L)
				{
					this.Length = 65536L;
				}
				else
				{
					this.Length = this.ContentLength;
				}
				if (this.WriteStream == null)
				{
					if (this.ContentLength > 2147483647L)
					{
						throw new WebException(SR.GetString("net_webstatus_MessageLengthLimitExceeded"), WebExceptionStatus.MessageLengthLimitExceeded);
					}
					this.SgBuffers = new ScatterGatherBuffers(this.Length);
				}
				this.InnerBuffer = new byte[(int)this.Length];
				this.ReadStream = response.GetResponseStream();
				if (this.Async && response.ContentLength >= 0L)
				{
					this.Progress.TotalBytesToReceive = response.ContentLength;
				}
				if (this.Async)
				{
					if (this.ReadStream == null || this.ReadStream == Stream.Null)
					{
						WebClient.DownloadBitsReadCallbackState(this, null);
					}
					else
					{
						this.ReadStream.BeginRead(this.InnerBuffer, this.Offset, (int)this.Length - this.Offset, new AsyncCallback(WebClient.DownloadBitsReadCallback), this);
					}
					return -1;
				}
				if (this.ReadStream == null || this.ReadStream == Stream.Null)
				{
					return 0;
				}
				return this.ReadStream.Read(this.InnerBuffer, this.Offset, (int)this.Length - this.Offset);
			}

			// Token: 0x060023AB RID: 9131 RVA: 0x0008CAE0 File Offset: 0x0008BAE0
			internal bool RetrieveBytes(ref int bytesRetrieved)
			{
				if (bytesRetrieved > 0)
				{
					if (this.WriteStream != null)
					{
						this.WriteStream.Write(this.InnerBuffer, 0, bytesRetrieved);
					}
					else
					{
						this.SgBuffers.Write(this.InnerBuffer, 0, bytesRetrieved);
					}
					if (this.Async)
					{
						this.Progress.BytesReceived += (long)bytesRetrieved;
					}
					if ((long)this.Offset != this.ContentLength)
					{
						if (this.Async)
						{
							this.WebClient.PostProgressChanged(this.AsyncOp, this.Progress);
							this.ReadStream.BeginRead(this.InnerBuffer, this.Offset, (int)this.Length - this.Offset, new AsyncCallback(WebClient.DownloadBitsReadCallback), this);
						}
						else
						{
							bytesRetrieved = this.ReadStream.Read(this.InnerBuffer, this.Offset, (int)this.Length - this.Offset);
						}
						return false;
					}
				}
				if (this.Async)
				{
					if (this.Progress.TotalBytesToReceive < 0L)
					{
						this.Progress.TotalBytesToReceive = this.Progress.BytesReceived;
					}
					this.WebClient.PostProgressChanged(this.AsyncOp, this.Progress);
				}
				if (this.ReadStream != null)
				{
					this.ReadStream.Close();
				}
				if (this.WriteStream != null)
				{
					this.WriteStream.Close();
				}
				else if (this.WriteStream == null)
				{
					byte[] array = new byte[this.SgBuffers.Length];
					if (this.SgBuffers.Length > 0)
					{
						BufferOffsetSize[] buffers = this.SgBuffers.GetBuffers();
						int num = 0;
						foreach (BufferOffsetSize bufferOffsetSize in buffers)
						{
							Buffer.BlockCopy(bufferOffsetSize.Buffer, 0, array, num, bufferOffsetSize.Size);
							num += bufferOffsetSize.Size;
						}
					}
					this.InnerBuffer = array;
				}
				return true;
			}

			// Token: 0x060023AC RID: 9132 RVA: 0x0008CCAC File Offset: 0x0008BCAC
			internal void Close()
			{
				if (this.WriteStream != null)
				{
					this.WriteStream.Close();
				}
				if (this.ReadStream != null)
				{
					this.ReadStream.Close();
				}
			}

			// Token: 0x0400243E RID: 9278
			internal WebClient WebClient;

			// Token: 0x0400243F RID: 9279
			internal Stream WriteStream;

			// Token: 0x04002440 RID: 9280
			internal byte[] InnerBuffer;

			// Token: 0x04002441 RID: 9281
			internal AsyncOperation AsyncOp;

			// Token: 0x04002442 RID: 9282
			internal WebRequest Request;

			// Token: 0x04002443 RID: 9283
			internal CompletionDelegate CompletionDelegate;

			// Token: 0x04002444 RID: 9284
			internal Stream ReadStream;

			// Token: 0x04002445 RID: 9285
			internal ScatterGatherBuffers SgBuffers;

			// Token: 0x04002446 RID: 9286
			internal long ContentLength;

			// Token: 0x04002447 RID: 9287
			internal long Length;

			// Token: 0x04002448 RID: 9288
			internal int Offset;

			// Token: 0x04002449 RID: 9289
			internal WebClient.ProgressData Progress;
		}

		// Token: 0x02000487 RID: 1159
		private class UploadBitsState
		{
			// Token: 0x060023AD RID: 9133 RVA: 0x0008CCD4 File Offset: 0x0008BCD4
			internal UploadBitsState(WebRequest request, Stream readStream, byte[] buffer, byte[] header, byte[] footer, CompletionDelegate completionDelegate, AsyncOperation asyncOp, WebClient.ProgressData progress, WebClient webClient)
			{
				this.InnerBuffer = buffer;
				this.Header = header;
				this.Footer = footer;
				this.ReadStream = readStream;
				this.Request = request;
				this.AsyncOp = asyncOp;
				this.CompletionDelegate = completionDelegate;
				if (this.AsyncOp != null)
				{
					this.Progress = progress;
					this.Progress.HasUploadPhase = true;
					this.Progress.TotalBytesToSend = ((request.ContentLength < 0L) ? (-1L) : request.ContentLength);
				}
				this.WebClient = webClient;
			}

			// Token: 0x1700075B RID: 1883
			// (get) Token: 0x060023AE RID: 9134 RVA: 0x0008CD5F File Offset: 0x0008BD5F
			internal bool FileUpload
			{
				get
				{
					return this.ReadStream != null;
				}
			}

			// Token: 0x1700075C RID: 1884
			// (get) Token: 0x060023AF RID: 9135 RVA: 0x0008CD6D File Offset: 0x0008BD6D
			internal bool Async
			{
				get
				{
					return this.AsyncOp != null;
				}
			}

			// Token: 0x060023B0 RID: 9136 RVA: 0x0008CD7C File Offset: 0x0008BD7C
			internal void SetRequestStream(Stream writeStream)
			{
				this.WriteStream = writeStream;
				byte[] array;
				if (this.Header != null)
				{
					array = this.Header;
					this.Header = null;
				}
				else
				{
					array = new byte[0];
				}
				if (this.Async)
				{
					this.Progress.BytesSent += (long)array.Length;
					this.WriteStream.BeginWrite(array, 0, array.Length, new AsyncCallback(WebClient.UploadBitsWriteCallback), this);
					return;
				}
				this.WriteStream.Write(array, 0, array.Length);
			}

			// Token: 0x060023B1 RID: 9137 RVA: 0x0008CE00 File Offset: 0x0008BE00
			internal bool WriteBytes()
			{
				if (this.Async)
				{
					this.WebClient.PostProgressChanged(this.AsyncOp, this.Progress);
				}
				int num2;
				byte[] array;
				if (this.FileUpload)
				{
					int num = 0;
					if (this.InnerBuffer != null)
					{
						num = this.ReadStream.Read(this.InnerBuffer, 0, this.InnerBuffer.Length);
						if (num <= 0)
						{
							this.ReadStream.Close();
							this.InnerBuffer = null;
						}
					}
					if (this.InnerBuffer != null)
					{
						num2 = num;
						array = this.InnerBuffer;
					}
					else
					{
						if (this.Footer == null)
						{
							return true;
						}
						num2 = this.Footer.Length;
						array = this.Footer;
						this.Footer = null;
					}
				}
				else
				{
					if (this.InnerBuffer == null)
					{
						return true;
					}
					num2 = this.InnerBuffer.Length;
					array = this.InnerBuffer;
					this.InnerBuffer = null;
				}
				if (this.Async)
				{
					this.Progress.BytesSent += (long)num2;
					this.WriteStream.BeginWrite(array, 0, num2, new AsyncCallback(WebClient.UploadBitsWriteCallback), this);
				}
				else
				{
					this.WriteStream.Write(array, 0, num2);
				}
				return false;
			}

			// Token: 0x060023B2 RID: 9138 RVA: 0x0008CF15 File Offset: 0x0008BF15
			internal void Close()
			{
				if (this.WriteStream != null)
				{
					this.WriteStream.Close();
				}
				if (this.ReadStream != null)
				{
					this.ReadStream.Close();
				}
			}

			// Token: 0x0400244A RID: 9290
			internal WebClient WebClient;

			// Token: 0x0400244B RID: 9291
			internal Stream WriteStream;

			// Token: 0x0400244C RID: 9292
			internal byte[] InnerBuffer;

			// Token: 0x0400244D RID: 9293
			internal byte[] Header;

			// Token: 0x0400244E RID: 9294
			internal byte[] Footer;

			// Token: 0x0400244F RID: 9295
			internal AsyncOperation AsyncOp;

			// Token: 0x04002450 RID: 9296
			internal WebRequest Request;

			// Token: 0x04002451 RID: 9297
			internal CompletionDelegate CompletionDelegate;

			// Token: 0x04002452 RID: 9298
			internal Stream ReadStream;

			// Token: 0x04002453 RID: 9299
			internal long Length;

			// Token: 0x04002454 RID: 9300
			internal int Offset;

			// Token: 0x04002455 RID: 9301
			internal WebClient.ProgressData Progress;
		}

		// Token: 0x02000488 RID: 1160
		private class WebClientWriteStream : Stream
		{
			// Token: 0x060023B3 RID: 9139 RVA: 0x0008CF3D File Offset: 0x0008BF3D
			public WebClientWriteStream(Stream stream, WebRequest request, WebClient webClient)
			{
				this.m_request = request;
				this.m_stream = stream;
				this.m_WebClient = webClient;
			}

			// Token: 0x1700075D RID: 1885
			// (get) Token: 0x060023B4 RID: 9140 RVA: 0x0008CF5A File Offset: 0x0008BF5A
			public override bool CanRead
			{
				get
				{
					return this.m_stream.CanRead;
				}
			}

			// Token: 0x1700075E RID: 1886
			// (get) Token: 0x060023B5 RID: 9141 RVA: 0x0008CF67 File Offset: 0x0008BF67
			public override bool CanSeek
			{
				get
				{
					return this.m_stream.CanSeek;
				}
			}

			// Token: 0x1700075F RID: 1887
			// (get) Token: 0x060023B6 RID: 9142 RVA: 0x0008CF74 File Offset: 0x0008BF74
			public override bool CanWrite
			{
				get
				{
					return this.m_stream.CanWrite;
				}
			}

			// Token: 0x17000760 RID: 1888
			// (get) Token: 0x060023B7 RID: 9143 RVA: 0x0008CF81 File Offset: 0x0008BF81
			public override bool CanTimeout
			{
				get
				{
					return this.m_stream.CanTimeout;
				}
			}

			// Token: 0x17000761 RID: 1889
			// (get) Token: 0x060023B8 RID: 9144 RVA: 0x0008CF8E File Offset: 0x0008BF8E
			// (set) Token: 0x060023B9 RID: 9145 RVA: 0x0008CF9B File Offset: 0x0008BF9B
			public override int ReadTimeout
			{
				get
				{
					return this.m_stream.ReadTimeout;
				}
				set
				{
					this.m_stream.ReadTimeout = value;
				}
			}

			// Token: 0x17000762 RID: 1890
			// (get) Token: 0x060023BA RID: 9146 RVA: 0x0008CFA9 File Offset: 0x0008BFA9
			// (set) Token: 0x060023BB RID: 9147 RVA: 0x0008CFB6 File Offset: 0x0008BFB6
			public override int WriteTimeout
			{
				get
				{
					return this.m_stream.WriteTimeout;
				}
				set
				{
					this.m_stream.WriteTimeout = value;
				}
			}

			// Token: 0x17000763 RID: 1891
			// (get) Token: 0x060023BC RID: 9148 RVA: 0x0008CFC4 File Offset: 0x0008BFC4
			public override long Length
			{
				get
				{
					return this.m_stream.Length;
				}
			}

			// Token: 0x17000764 RID: 1892
			// (get) Token: 0x060023BD RID: 9149 RVA: 0x0008CFD1 File Offset: 0x0008BFD1
			// (set) Token: 0x060023BE RID: 9150 RVA: 0x0008CFDE File Offset: 0x0008BFDE
			public override long Position
			{
				get
				{
					return this.m_stream.Position;
				}
				set
				{
					this.m_stream.Position = value;
				}
			}

			// Token: 0x060023BF RID: 9151 RVA: 0x0008CFEC File Offset: 0x0008BFEC
			[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
			{
				return this.m_stream.BeginRead(buffer, offset, size, callback, state);
			}

			// Token: 0x060023C0 RID: 9152 RVA: 0x0008D000 File Offset: 0x0008C000
			[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
			{
				return this.m_stream.BeginWrite(buffer, offset, size, callback, state);
			}

			// Token: 0x060023C1 RID: 9153 RVA: 0x0008D014 File Offset: 0x0008C014
			protected override void Dispose(bool disposing)
			{
				try
				{
					if (disposing)
					{
						this.m_stream.Close();
						this.m_WebClient.GetWebResponse(this.m_request).Close();
					}
				}
				finally
				{
					base.Dispose(disposing);
				}
			}

			// Token: 0x060023C2 RID: 9154 RVA: 0x0008D060 File Offset: 0x0008C060
			public override int EndRead(IAsyncResult result)
			{
				return this.m_stream.EndRead(result);
			}

			// Token: 0x060023C3 RID: 9155 RVA: 0x0008D06E File Offset: 0x0008C06E
			public override void EndWrite(IAsyncResult result)
			{
				this.m_stream.EndWrite(result);
			}

			// Token: 0x060023C4 RID: 9156 RVA: 0x0008D07C File Offset: 0x0008C07C
			public override void Flush()
			{
				this.m_stream.Flush();
			}

			// Token: 0x060023C5 RID: 9157 RVA: 0x0008D089 File Offset: 0x0008C089
			public override int Read(byte[] buffer, int offset, int count)
			{
				return this.m_stream.Read(buffer, offset, count);
			}

			// Token: 0x060023C6 RID: 9158 RVA: 0x0008D099 File Offset: 0x0008C099
			public override long Seek(long offset, SeekOrigin origin)
			{
				return this.m_stream.Seek(offset, origin);
			}

			// Token: 0x060023C7 RID: 9159 RVA: 0x0008D0A8 File Offset: 0x0008C0A8
			public override void SetLength(long value)
			{
				this.m_stream.SetLength(value);
			}

			// Token: 0x060023C8 RID: 9160 RVA: 0x0008D0B6 File Offset: 0x0008C0B6
			public override void Write(byte[] buffer, int offset, int count)
			{
				this.m_stream.Write(buffer, offset, count);
			}

			// Token: 0x04002456 RID: 9302
			private WebRequest m_request;

			// Token: 0x04002457 RID: 9303
			private Stream m_stream;

			// Token: 0x04002458 RID: 9304
			private WebClient m_WebClient;
		}
	}
}
