using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Web.Management;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x020000EF RID: 239
	[ComVisible(false)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class HttpWorkerRequest
	{
		// Token: 0x06000B19 RID: 2841 RVA: 0x0002C522 File Offset: 0x0002B522
		protected HttpWorkerRequest()
		{
			this._startTime = DateTime.UtcNow;
		}

		// Token: 0x06000B1A RID: 2842
		public abstract string GetUriPath();

		// Token: 0x06000B1B RID: 2843
		public abstract string GetQueryString();

		// Token: 0x06000B1C RID: 2844
		public abstract string GetRawUrl();

		// Token: 0x06000B1D RID: 2845
		public abstract string GetHttpVerbName();

		// Token: 0x06000B1E RID: 2846
		public abstract string GetHttpVersion();

		// Token: 0x06000B1F RID: 2847
		public abstract string GetRemoteAddress();

		// Token: 0x06000B20 RID: 2848
		public abstract int GetRemotePort();

		// Token: 0x06000B21 RID: 2849
		public abstract string GetLocalAddress();

		// Token: 0x06000B22 RID: 2850
		public abstract int GetLocalPort();

		// Token: 0x06000B23 RID: 2851 RVA: 0x0002C538 File Offset: 0x0002B538
		internal virtual string GetLocalPortAsString()
		{
			return this.GetLocalPort().ToString(NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x0002C558 File Offset: 0x0002B558
		// (set) Token: 0x06000B25 RID: 2853 RVA: 0x0002C562 File Offset: 0x0002B562
		internal bool IsInReadEntitySync
		{
			get
			{
				return this._isInReadEntitySync;
			}
			set
			{
				this._isInReadEntitySync = value;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x0002C56D File Offset: 0x0002B56D
		internal virtual bool IsRewriteModuleEnabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0002C570 File Offset: 0x0002B570
		internal virtual void SetRawUrl(string path)
		{
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0002C572 File Offset: 0x0002B572
		public virtual byte[] GetQueryStringRawBytes()
		{
			return null;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0002C575 File Offset: 0x0002B575
		public virtual string GetRemoteName()
		{
			return this.GetRemoteAddress();
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0002C57D File Offset: 0x0002B57D
		public virtual string GetServerName()
		{
			return this.GetLocalAddress();
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0002C585 File Offset: 0x0002B585
		public virtual long GetConnectionID()
		{
			return 0L;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0002C589 File Offset: 0x0002B589
		public virtual long GetUrlContextID()
		{
			return 0L;
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0002C58D File Offset: 0x0002B58D
		public virtual string GetAppPoolID()
		{
			return null;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0002C590 File Offset: 0x0002B590
		public virtual int GetRequestReason()
		{
			return 0;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0002C593 File Offset: 0x0002B593
		public virtual IntPtr GetUserToken()
		{
			return IntPtr.Zero;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0002C59A File Offset: 0x0002B59A
		public virtual IntPtr GetVirtualPathToken()
		{
			return IntPtr.Zero;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0002C5A1 File Offset: 0x0002B5A1
		public virtual bool IsSecure()
		{
			return false;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0002C5A4 File Offset: 0x0002B5A4
		public virtual string GetProtocol()
		{
			if (!this.IsSecure())
			{
				return "http";
			}
			return "https";
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0002C5B9 File Offset: 0x0002B5B9
		public virtual string GetFilePath()
		{
			return this.GetUriPath();
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0002C5C1 File Offset: 0x0002B5C1
		internal VirtualPath GetFilePathObject()
		{
			return VirtualPath.Create(this.GetFilePath(), VirtualPathOptions.AllowNull | VirtualPathOptions.AllowAbsolutePath);
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0002C5CF File Offset: 0x0002B5CF
		public virtual string GetFilePathTranslated()
		{
			return null;
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0002C5D2 File Offset: 0x0002B5D2
		public virtual string GetPathInfo()
		{
			return string.Empty;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0002C5D9 File Offset: 0x0002B5D9
		public virtual string GetAppPath()
		{
			return null;
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0002C5DC File Offset: 0x0002B5DC
		public virtual string GetAppPathTranslated()
		{
			return null;
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0002C5E0 File Offset: 0x0002B5E0
		public virtual int GetPreloadedEntityBodyLength()
		{
			byte[] preloadedEntityBody = this.GetPreloadedEntityBody();
			if (preloadedEntityBody == null)
			{
				return 0;
			}
			return preloadedEntityBody.Length;
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0002C5FC File Offset: 0x0002B5FC
		public virtual int GetPreloadedEntityBody(byte[] buffer, int offset)
		{
			int num = 0;
			byte[] preloadedEntityBody = this.GetPreloadedEntityBody();
			if (preloadedEntityBody != null)
			{
				num = preloadedEntityBody.Length;
				Buffer.BlockCopy(preloadedEntityBody, 0, buffer, offset, num);
			}
			return num;
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0002C624 File Offset: 0x0002B624
		public virtual byte[] GetPreloadedEntityBody()
		{
			return null;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0002C627 File Offset: 0x0002B627
		public virtual bool IsEntireEntityBodyIsPreloaded()
		{
			return false;
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0002C62C File Offset: 0x0002B62C
		public virtual int GetTotalEntityBodyLength()
		{
			int num = 0;
			string knownRequestHeader = this.GetKnownRequestHeader(11);
			if (knownRequestHeader != null)
			{
				try
				{
					num = int.Parse(knownRequestHeader, CultureInfo.InvariantCulture);
				}
				catch
				{
				}
			}
			return num;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0002C66C File Offset: 0x0002B66C
		public virtual int ReadEntityBody(byte[] buffer, int size)
		{
			return 0;
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0002C670 File Offset: 0x0002B670
		public virtual int ReadEntityBody(byte[] buffer, int offset, int size)
		{
			byte[] array = new byte[size];
			int num = this.ReadEntityBody(array, size);
			if (num > 0)
			{
				if (offset < 0 || buffer.Length - offset < size)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				Buffer.BlockCopy(array, 0, buffer, offset, num);
			}
			return num;
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0002C6B3 File Offset: 0x0002B6B3
		public virtual string GetKnownRequestHeader(int index)
		{
			return null;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0002C6B6 File Offset: 0x0002B6B6
		public virtual string GetUnknownRequestHeader(string name)
		{
			return null;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0002C6B9 File Offset: 0x0002B6B9
		[CLSCompliant(false)]
		public virtual string[][] GetUnknownRequestHeaders()
		{
			return null;
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0002C6BC File Offset: 0x0002B6BC
		public virtual string GetServerVariable(string name)
		{
			return null;
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0002C6BF File Offset: 0x0002B6BF
		public virtual long GetBytesRead()
		{
			return 0L;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0002C6C3 File Offset: 0x0002B6C3
		internal virtual DateTime GetStartTime()
		{
			return this._startTime;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0002C6CB File Offset: 0x0002B6CB
		internal virtual void ResetStartTime()
		{
			this._startTime = DateTime.UtcNow;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0002C6D8 File Offset: 0x0002B6D8
		public virtual string MapPath(string virtualPath)
		{
			return null;
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000B48 RID: 2888 RVA: 0x0002C6DB File Offset: 0x0002B6DB
		public virtual string MachineConfigPath
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x0002C6DE File Offset: 0x0002B6DE
		public virtual string RootWebConfigPath
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x0002C6E1 File Offset: 0x0002B6E1
		public virtual string MachineInstallDirectory
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0002C6E4 File Offset: 0x0002B6E4
		internal virtual void RaiseTraceEvent(IntegratedTraceType traceType, string eventData)
		{
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0002C6E6 File Offset: 0x0002B6E6
		internal virtual void RaiseTraceEvent(WebBaseEvent webEvent)
		{
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x0002C6E8 File Offset: 0x0002B6E8
		public virtual Guid RequestTraceIdentifier
		{
			get
			{
				return this._traceId;
			}
		}

		// Token: 0x06000B4E RID: 2894
		public abstract void SendStatus(int statusCode, string statusDescription);

		// Token: 0x06000B4F RID: 2895 RVA: 0x0002C6F0 File Offset: 0x0002B6F0
		internal virtual void SendStatus(int statusCode, int subStatusCode, string statusDescription)
		{
			this.SendStatus(statusCode, statusDescription);
		}

		// Token: 0x06000B50 RID: 2896
		public abstract void SendKnownResponseHeader(int index, string value);

		// Token: 0x06000B51 RID: 2897
		public abstract void SendUnknownResponseHeader(string name, string value);

		// Token: 0x06000B52 RID: 2898 RVA: 0x0002C6FA File Offset: 0x0002B6FA
		internal virtual void SetHeaderEncoding(Encoding encoding)
		{
		}

		// Token: 0x06000B53 RID: 2899
		public abstract void SendResponseFromMemory(byte[] data, int length);

		// Token: 0x06000B54 RID: 2900 RVA: 0x0002C6FC File Offset: 0x0002B6FC
		public virtual void SendResponseFromMemory(IntPtr data, int length)
		{
			if (length > 0)
			{
				InternalSecurityPermissions.UnmanagedCode.Demand();
				byte[] array = new byte[length];
				Misc.CopyMemory(data, 0, array, 0, length);
				this.SendResponseFromMemory(array, length);
			}
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0002C730 File Offset: 0x0002B730
		internal virtual void SendResponseFromMemory(IntPtr data, int length, bool isBufferFromUnmanagedPool)
		{
			this.SendResponseFromMemory(data, length);
		}

		// Token: 0x06000B56 RID: 2902
		public abstract void SendResponseFromFile(string filename, long offset, long length);

		// Token: 0x06000B57 RID: 2903
		public abstract void SendResponseFromFile(IntPtr handle, long offset, long length);

		// Token: 0x06000B58 RID: 2904 RVA: 0x0002C73A File Offset: 0x0002B73A
		internal virtual void TransmitFile(string filename, long length, bool isImpersonating)
		{
			this.TransmitFile(filename, 0L, length, isImpersonating);
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0002C747 File Offset: 0x0002B747
		internal virtual void TransmitFile(string filename, long offset, long length, bool isImpersonating)
		{
			this.SendResponseFromFile(filename, offset, length);
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000B5A RID: 2906 RVA: 0x0002C752 File Offset: 0x0002B752
		internal virtual bool SupportsLongTransmitFile
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0002C755 File Offset: 0x0002B755
		internal virtual string SetupKernelCaching(int secondsToLive, string originalCacheUrl, bool enableKernelCacheForVaryByStar)
		{
			return null;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0002C758 File Offset: 0x0002B758
		internal virtual void DisableKernelCache()
		{
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x0002C75A File Offset: 0x0002B75A
		// (set) Token: 0x06000B5E RID: 2910 RVA: 0x0002C75D File Offset: 0x0002B75D
		internal virtual bool TrySkipIisCustomErrors
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x0002C75F File Offset: 0x0002B75F
		internal virtual bool SupportsExecuteUrl
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0002C762 File Offset: 0x0002B762
		internal virtual IAsyncResult BeginExecuteUrl(string url, string method, string headers, bool sendHeaders, bool addUserIndo, IntPtr token, string name, string authType, byte[] entity, AsyncCallback cb, object state)
		{
			throw new NotSupportedException(SR.GetString("ExecuteUrl_not_supported"));
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0002C773 File Offset: 0x0002B773
		internal virtual void EndExecuteUrl(IAsyncResult result)
		{
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0002C775 File Offset: 0x0002B775
		internal virtual void UpdateResponseCounters(bool finalFlush, int bytesOut)
		{
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0002C777 File Offset: 0x0002B777
		internal virtual void UpdateRequestCounters(int bytesIn)
		{
		}

		// Token: 0x06000B64 RID: 2916
		public abstract void FlushResponse(bool finalFlush);

		// Token: 0x06000B65 RID: 2917
		public abstract void EndOfRequest();

		// Token: 0x06000B66 RID: 2918 RVA: 0x0002C779 File Offset: 0x0002B779
		public virtual void SetEndOfSendNotification(HttpWorkerRequest.EndOfSendNotification callback, object extraData)
		{
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0002C77B File Offset: 0x0002B77B
		public virtual void SendCalculatedContentLength(int contentLength)
		{
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0002C77D File Offset: 0x0002B77D
		public virtual void SendCalculatedContentLength(long contentLength)
		{
			this.SendCalculatedContentLength(Convert.ToInt32(contentLength));
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0002C78B File Offset: 0x0002B78B
		public virtual bool HeadersSent()
		{
			return true;
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0002C78E File Offset: 0x0002B78E
		public virtual bool IsClientConnected()
		{
			return true;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0002C791 File Offset: 0x0002B791
		public virtual void CloseConnection()
		{
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0002C793 File Offset: 0x0002B793
		public virtual byte[] GetClientCertificate()
		{
			return new byte[0];
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0002C79B File Offset: 0x0002B79B
		public virtual DateTime GetClientCertificateValidFrom()
		{
			return DateTime.Now;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0002C7A2 File Offset: 0x0002B7A2
		public virtual DateTime GetClientCertificateValidUntil()
		{
			return DateTime.Now;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0002C7A9 File Offset: 0x0002B7A9
		public virtual byte[] GetClientCertificateBinaryIssuer()
		{
			return new byte[0];
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0002C7B1 File Offset: 0x0002B7B1
		public virtual int GetClientCertificateEncoding()
		{
			return 0;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0002C7B4 File Offset: 0x0002B7B4
		public virtual byte[] GetClientCertificatePublicKey()
		{
			return new byte[0];
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0002C7BC File Offset: 0x0002B7BC
		public bool HasEntityBody()
		{
			string knownRequestHeader = this.GetKnownRequestHeader(11);
			return (knownRequestHeader != null && !knownRequestHeader.Equals("0")) || this.GetKnownRequestHeader(6) != null || this.GetPreloadedEntityBody() != null || (this.IsEntireEntityBodyIsPreloaded() && false);
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0002C804 File Offset: 0x0002B804
		public static string GetStatusDescription(int code)
		{
			if (code >= 100 && code < 600)
			{
				int num = code / 100;
				int num2 = code % 100;
				if (num2 < HttpWorkerRequest.s_HTTPStatusDescriptions[num].Length)
				{
					return HttpWorkerRequest.s_HTTPStatusDescriptions[num][num2];
				}
			}
			return string.Empty;
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0002C844 File Offset: 0x0002B844
		public static int GetKnownRequestHeaderIndex(string header)
		{
			object obj = HttpWorkerRequest.s_requestHeadersLoookupTable[header];
			if (obj != null)
			{
				return (int)obj;
			}
			return -1;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0002C868 File Offset: 0x0002B868
		public static string GetKnownRequestHeaderName(int index)
		{
			return HttpWorkerRequest.s_requestHeaderNames[index];
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0002C871 File Offset: 0x0002B871
		internal static string GetServerVariableNameFromKnownRequestHeaderIndex(int index)
		{
			return HttpWorkerRequest.s_serverVarFromRequestHeaderNames[index];
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0002C87C File Offset: 0x0002B87C
		public static int GetKnownResponseHeaderIndex(string header)
		{
			object obj = HttpWorkerRequest.s_responseHeadersLoookupTable[header];
			if (obj != null)
			{
				return (int)obj;
			}
			return -1;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0002C8A0 File Offset: 0x0002B8A0
		public static string GetKnownResponseHeaderName(int index)
		{
			return HttpWorkerRequest.s_responseHeaderNames[index];
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0002C8AC File Offset: 0x0002B8AC
		private static void DefineHeader(bool isRequest, bool isResponse, int index, string headerName, string serverVarName)
		{
			if (isRequest)
			{
				HttpWorkerRequest.s_serverVarFromRequestHeaderNames[index] = serverVarName;
				HttpWorkerRequest.s_requestHeaderNames[index] = headerName;
				HttpWorkerRequest.s_requestHeadersLoookupTable.Add(headerName, index);
			}
			if (isResponse)
			{
				HttpWorkerRequest.s_responseHeaderNames[index] = headerName;
				HttpWorkerRequest.s_responseHeadersLoookupTable.Add(headerName, index);
			}
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0002C900 File Offset: 0x0002B900
		static HttpWorkerRequest()
		{
			HttpWorkerRequest.DefineHeader(true, true, 0, "Cache-Control", "HTTP_CACHE_CONTROL");
			HttpWorkerRequest.DefineHeader(true, true, 1, "Connection", "HTTP_CONNECTION");
			HttpWorkerRequest.DefineHeader(true, true, 2, "Date", "HTTP_DATE");
			HttpWorkerRequest.DefineHeader(true, true, 3, "Keep-Alive", "HTTP_KEEP_ALIVE");
			HttpWorkerRequest.DefineHeader(true, true, 4, "Pragma", "HTTP_PRAGMA");
			HttpWorkerRequest.DefineHeader(true, true, 5, "Trailer", "HTTP_TRAILER");
			HttpWorkerRequest.DefineHeader(true, true, 6, "Transfer-Encoding", "HTTP_TRANSFER_ENCODING");
			HttpWorkerRequest.DefineHeader(true, true, 7, "Upgrade", "HTTP_UPGRADE");
			HttpWorkerRequest.DefineHeader(true, true, 8, "Via", "HTTP_VIA");
			HttpWorkerRequest.DefineHeader(true, true, 9, "Warning", "HTTP_WARNING");
			HttpWorkerRequest.DefineHeader(true, true, 10, "Allow", "HTTP_ALLOW");
			HttpWorkerRequest.DefineHeader(true, true, 11, "Content-Length", "HTTP_CONTENT_LENGTH");
			HttpWorkerRequest.DefineHeader(true, true, 12, "Content-Type", "HTTP_CONTENT_TYPE");
			HttpWorkerRequest.DefineHeader(true, true, 13, "Content-Encoding", "HTTP_CONTENT_ENCODING");
			HttpWorkerRequest.DefineHeader(true, true, 14, "Content-Language", "HTTP_CONTENT_LANGUAGE");
			HttpWorkerRequest.DefineHeader(true, true, 15, "Content-Location", "HTTP_CONTENT_LOCATION");
			HttpWorkerRequest.DefineHeader(true, true, 16, "Content-MD5", "HTTP_CONTENT_MD5");
			HttpWorkerRequest.DefineHeader(true, true, 17, "Content-Range", "HTTP_CONTENT_RANGE");
			HttpWorkerRequest.DefineHeader(true, true, 18, "Expires", "HTTP_EXPIRES");
			HttpWorkerRequest.DefineHeader(true, true, 19, "Last-Modified", "HTTP_LAST_MODIFIED");
			HttpWorkerRequest.DefineHeader(true, false, 20, "Accept", "HTTP_ACCEPT");
			HttpWorkerRequest.DefineHeader(true, false, 21, "Accept-Charset", "HTTP_ACCEPT_CHARSET");
			HttpWorkerRequest.DefineHeader(true, false, 22, "Accept-Encoding", "HTTP_ACCEPT_ENCODING");
			HttpWorkerRequest.DefineHeader(true, false, 23, "Accept-Language", "HTTP_ACCEPT_LANGUAGE");
			HttpWorkerRequest.DefineHeader(true, false, 24, "Authorization", "HTTP_AUTHORIZATION");
			HttpWorkerRequest.DefineHeader(true, false, 25, "Cookie", "HTTP_COOKIE");
			HttpWorkerRequest.DefineHeader(true, false, 26, "Expect", "HTTP_EXPECT");
			HttpWorkerRequest.DefineHeader(true, false, 27, "From", "HTTP_FROM");
			HttpWorkerRequest.DefineHeader(true, false, 28, "Host", "HTTP_HOST");
			HttpWorkerRequest.DefineHeader(true, false, 29, "If-Match", "HTTP_IF_MATCH");
			HttpWorkerRequest.DefineHeader(true, false, 30, "If-Modified-Since", "HTTP_IF_MODIFIED_SINCE");
			HttpWorkerRequest.DefineHeader(true, false, 31, "If-None-Match", "HTTP_IF_NONE_MATCH");
			HttpWorkerRequest.DefineHeader(true, false, 32, "If-Range", "HTTP_IF_RANGE");
			HttpWorkerRequest.DefineHeader(true, false, 33, "If-Unmodified-Since", "HTTP_IF_UNMODIFIED_SINCE");
			HttpWorkerRequest.DefineHeader(true, false, 34, "Max-Forwards", "HTTP_MAX_FORWARDS");
			HttpWorkerRequest.DefineHeader(true, false, 35, "Proxy-Authorization", "HTTP_PROXY_AUTHORIZATION");
			HttpWorkerRequest.DefineHeader(true, false, 36, "Referer", "HTTP_REFERER");
			HttpWorkerRequest.DefineHeader(true, false, 37, "Range", "HTTP_RANGE");
			HttpWorkerRequest.DefineHeader(true, false, 38, "TE", "HTTP_TE");
			HttpWorkerRequest.DefineHeader(true, false, 39, "User-Agent", "HTTP_USER_AGENT");
			HttpWorkerRequest.DefineHeader(false, true, 20, "Accept-Ranges", null);
			HttpWorkerRequest.DefineHeader(false, true, 21, "Age", null);
			HttpWorkerRequest.DefineHeader(false, true, 22, "ETag", null);
			HttpWorkerRequest.DefineHeader(false, true, 23, "Location", null);
			HttpWorkerRequest.DefineHeader(false, true, 24, "Proxy-Authenticate", null);
			HttpWorkerRequest.DefineHeader(false, true, 25, "Retry-After", null);
			HttpWorkerRequest.DefineHeader(false, true, 26, "Server", null);
			HttpWorkerRequest.DefineHeader(false, true, 27, "Set-Cookie", null);
			HttpWorkerRequest.DefineHeader(false, true, 28, "Vary", null);
			HttpWorkerRequest.DefineHeader(false, true, 29, "WWW-Authenticate", null);
		}

		// Token: 0x04001342 RID: 4930
		public const int HeaderCacheControl = 0;

		// Token: 0x04001343 RID: 4931
		public const int HeaderConnection = 1;

		// Token: 0x04001344 RID: 4932
		public const int HeaderDate = 2;

		// Token: 0x04001345 RID: 4933
		public const int HeaderKeepAlive = 3;

		// Token: 0x04001346 RID: 4934
		public const int HeaderPragma = 4;

		// Token: 0x04001347 RID: 4935
		public const int HeaderTrailer = 5;

		// Token: 0x04001348 RID: 4936
		public const int HeaderTransferEncoding = 6;

		// Token: 0x04001349 RID: 4937
		public const int HeaderUpgrade = 7;

		// Token: 0x0400134A RID: 4938
		public const int HeaderVia = 8;

		// Token: 0x0400134B RID: 4939
		public const int HeaderWarning = 9;

		// Token: 0x0400134C RID: 4940
		public const int HeaderAllow = 10;

		// Token: 0x0400134D RID: 4941
		public const int HeaderContentLength = 11;

		// Token: 0x0400134E RID: 4942
		public const int HeaderContentType = 12;

		// Token: 0x0400134F RID: 4943
		public const int HeaderContentEncoding = 13;

		// Token: 0x04001350 RID: 4944
		public const int HeaderContentLanguage = 14;

		// Token: 0x04001351 RID: 4945
		public const int HeaderContentLocation = 15;

		// Token: 0x04001352 RID: 4946
		public const int HeaderContentMd5 = 16;

		// Token: 0x04001353 RID: 4947
		public const int HeaderContentRange = 17;

		// Token: 0x04001354 RID: 4948
		public const int HeaderExpires = 18;

		// Token: 0x04001355 RID: 4949
		public const int HeaderLastModified = 19;

		// Token: 0x04001356 RID: 4950
		public const int HeaderAccept = 20;

		// Token: 0x04001357 RID: 4951
		public const int HeaderAcceptCharset = 21;

		// Token: 0x04001358 RID: 4952
		public const int HeaderAcceptEncoding = 22;

		// Token: 0x04001359 RID: 4953
		public const int HeaderAcceptLanguage = 23;

		// Token: 0x0400135A RID: 4954
		public const int HeaderAuthorization = 24;

		// Token: 0x0400135B RID: 4955
		public const int HeaderCookie = 25;

		// Token: 0x0400135C RID: 4956
		public const int HeaderExpect = 26;

		// Token: 0x0400135D RID: 4957
		public const int HeaderFrom = 27;

		// Token: 0x0400135E RID: 4958
		public const int HeaderHost = 28;

		// Token: 0x0400135F RID: 4959
		public const int HeaderIfMatch = 29;

		// Token: 0x04001360 RID: 4960
		public const int HeaderIfModifiedSince = 30;

		// Token: 0x04001361 RID: 4961
		public const int HeaderIfNoneMatch = 31;

		// Token: 0x04001362 RID: 4962
		public const int HeaderIfRange = 32;

		// Token: 0x04001363 RID: 4963
		public const int HeaderIfUnmodifiedSince = 33;

		// Token: 0x04001364 RID: 4964
		public const int HeaderMaxForwards = 34;

		// Token: 0x04001365 RID: 4965
		public const int HeaderProxyAuthorization = 35;

		// Token: 0x04001366 RID: 4966
		public const int HeaderReferer = 36;

		// Token: 0x04001367 RID: 4967
		public const int HeaderRange = 37;

		// Token: 0x04001368 RID: 4968
		public const int HeaderTe = 38;

		// Token: 0x04001369 RID: 4969
		public const int HeaderUserAgent = 39;

		// Token: 0x0400136A RID: 4970
		public const int RequestHeaderMaximum = 40;

		// Token: 0x0400136B RID: 4971
		public const int HeaderAcceptRanges = 20;

		// Token: 0x0400136C RID: 4972
		public const int HeaderAge = 21;

		// Token: 0x0400136D RID: 4973
		public const int HeaderEtag = 22;

		// Token: 0x0400136E RID: 4974
		public const int HeaderLocation = 23;

		// Token: 0x0400136F RID: 4975
		public const int HeaderProxyAuthenticate = 24;

		// Token: 0x04001370 RID: 4976
		public const int HeaderRetryAfter = 25;

		// Token: 0x04001371 RID: 4977
		public const int HeaderServer = 26;

		// Token: 0x04001372 RID: 4978
		public const int HeaderSetCookie = 27;

		// Token: 0x04001373 RID: 4979
		public const int HeaderVary = 28;

		// Token: 0x04001374 RID: 4980
		public const int HeaderWwwAuthenticate = 29;

		// Token: 0x04001375 RID: 4981
		public const int ResponseHeaderMaximum = 30;

		// Token: 0x04001376 RID: 4982
		public const int ReasonResponseCacheMiss = 0;

		// Token: 0x04001377 RID: 4983
		public const int ReasonFileHandleCacheMiss = 1;

		// Token: 0x04001378 RID: 4984
		public const int ReasonCachePolicy = 2;

		// Token: 0x04001379 RID: 4985
		public const int ReasonCacheSecurity = 3;

		// Token: 0x0400137A RID: 4986
		public const int ReasonClientDisconnect = 4;

		// Token: 0x0400137B RID: 4987
		public const int ReasonDefault = 0;

		// Token: 0x0400137C RID: 4988
		private DateTime _startTime;

		// Token: 0x0400137D RID: 4989
		private volatile bool _isInReadEntitySync;

		// Token: 0x0400137E RID: 4990
		private Guid _traceId;

		// Token: 0x0400137F RID: 4991
		private static readonly string[][] s_HTTPStatusDescriptions = new string[][]
		{
			null,
			new string[] { "Continue", "Switching Protocols", "Processing" },
			new string[] { "OK", "Created", "Accepted", "Non-Authoritative Information", "No Content", "Reset Content", "Partial Content", "Multi-Status" },
			new string[]
			{
				"Multiple Choices",
				"Moved Permanently",
				"Found",
				"See Other",
				"Not Modified",
				"Use Proxy",
				string.Empty,
				"Temporary Redirect"
			},
			new string[]
			{
				"Bad Request",
				"Unauthorized",
				"Payment Required",
				"Forbidden",
				"Not Found",
				"Method Not Allowed",
				"Not Acceptable",
				"Proxy Authentication Required",
				"Request Timeout",
				"Conflict",
				"Gone",
				"Length Required",
				"Precondition Failed",
				"Request Entity Too Large",
				"Request-Uri Too Long",
				"Unsupported Media Type",
				"Requested Range Not Satisfiable",
				"Expectation Failed",
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				"Unprocessable Entity",
				"Locked",
				"Failed Dependency"
			},
			new string[]
			{
				"Internal Server Error",
				"Not Implemented",
				"Bad Gateway",
				"Service Unavailable",
				"Gateway Timeout",
				"Http Version Not Supported",
				string.Empty,
				"Insufficient Storage"
			}
		};

		// Token: 0x04001380 RID: 4992
		private static string[] s_serverVarFromRequestHeaderNames = new string[40];

		// Token: 0x04001381 RID: 4993
		private static string[] s_requestHeaderNames = new string[40];

		// Token: 0x04001382 RID: 4994
		private static string[] s_responseHeaderNames = new string[30];

		// Token: 0x04001383 RID: 4995
		private static Hashtable s_requestHeadersLoookupTable = new Hashtable(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04001384 RID: 4996
		private static Hashtable s_responseHeadersLoookupTable = new Hashtable(StringComparer.OrdinalIgnoreCase);

		// Token: 0x020000F0 RID: 240
		// (Invoke) Token: 0x06000B7C RID: 2940
		public delegate void EndOfSendNotification(HttpWorkerRequest wr, object extraData);
	}
}
