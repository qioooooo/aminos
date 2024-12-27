using System;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Net
{
	// Token: 0x020003D0 RID: 976
	public sealed class HttpListenerContext
	{
		// Token: 0x06001ECA RID: 7882 RVA: 0x00077490 File Offset: 0x00076490
		internal unsafe HttpListenerContext(HttpListener httpListener, RequestContextBase memoryBlob)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, ".ctor", "httpListener#" + ValidationHelper.HashString(httpListener) + " requestBlob=" + ValidationHelper.HashString((IntPtr)((void*)memoryBlob.RequestBlob)));
			}
			this.m_Listener = httpListener;
			this.m_Request = new HttpListenerRequest(this, memoryBlob);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000774F8 File Offset: 0x000764F8
		internal void SetIdentity(IPrincipal principal, string mutualAuthentication)
		{
			this.m_MutualAuthentication = mutualAuthentication;
			this.m_User = principal;
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001ECC RID: 7884 RVA: 0x00077508 File Offset: 0x00076508
		public HttpListenerRequest Request
		{
			get
			{
				return this.m_Request;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001ECD RID: 7885 RVA: 0x00077510 File Offset: 0x00076510
		public HttpListenerResponse Response
		{
			get
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.HttpListener, this, "Response", "");
				}
				if (this.m_Response == null)
				{
					this.m_Response = new HttpListenerResponse(this);
				}
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Response", "");
				}
				return this.m_Response;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001ECE RID: 7886 RVA: 0x0007756F File Offset: 0x0007656F
		public IPrincipal User
		{
			get
			{
				if (!(this.m_User is WindowsPrincipal))
				{
					return this.m_User;
				}
				new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Demand();
				return this.m_User;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001ECF RID: 7887 RVA: 0x0007759A File Offset: 0x0007659A
		internal bool PromoteCookiesToRfc2965
		{
			get
			{
				return this.m_PromoteCookiesToRfc2965;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001ED0 RID: 7888 RVA: 0x000775A2 File Offset: 0x000765A2
		internal string MutualAuthentication
		{
			get
			{
				return this.m_MutualAuthentication;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001ED1 RID: 7889 RVA: 0x000775AA File Offset: 0x000765AA
		internal HttpListener Listener
		{
			get
			{
				return this.m_Listener;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001ED2 RID: 7890 RVA: 0x000775B2 File Offset: 0x000765B2
		internal SafeCloseHandle RequestQueueHandle
		{
			get
			{
				return this.m_Listener.RequestQueueHandle;
			}
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000775BF File Offset: 0x000765BF
		internal void EnsureBoundHandle()
		{
			this.m_Listener.EnsureBoundHandle();
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001ED4 RID: 7892 RVA: 0x000775CC File Offset: 0x000765CC
		internal ulong RequestId
		{
			get
			{
				return this.Request.RequestId;
			}
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000775DC File Offset: 0x000765DC
		internal void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Close()", "");
			}
			try
			{
				if (this.m_Response != null)
				{
					this.m_Response.Close();
				}
			}
			finally
			{
				try
				{
					this.m_Request.Close();
				}
				finally
				{
					IDisposable disposable = ((this.m_User == null) ? null : (this.m_User.Identity as IDisposable));
					if (disposable != null && this.m_User.Identity.AuthenticationType != "NTLM" && !this.m_Listener.UnsafeConnectionNtlmAuthentication)
					{
						disposable.Dispose();
					}
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Close", "");
			}
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000776B0 File Offset: 0x000766B0
		internal void Abort()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Abort", "");
			}
			HttpListenerContext.CancelRequest(this.RequestQueueHandle, this.m_Request.RequestId);
			try
			{
				this.m_Request.Close();
			}
			finally
			{
				IDisposable disposable = ((this.m_User == null) ? null : (this.m_User.Identity as IDisposable));
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Abort", "");
			}
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x00077750 File Offset: 0x00076750
		internal UnsafeNclNativeMethods.HttpApi.HTTP_VERB GetKnownMethod()
		{
			return UnsafeNclNativeMethods.HttpApi.GetKnownVerb(this.Request.RequestBuffer, this.Request.OriginalBlobAddress);
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x00077770 File Offset: 0x00076770
		internal unsafe static void CancelRequest(SafeCloseHandle requestQueueHandle, ulong requestId)
		{
			UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK http_DATA_CHUNK = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
			http_DATA_CHUNK.DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
			http_DATA_CHUNK.pBuffer = (byte*)(&http_DATA_CHUNK);
			UnsafeNclNativeMethods.HttpApi.HttpSendResponseEntityBody(requestQueueHandle, requestId, 1U, 1, &http_DATA_CHUNK, null, SafeLocalFree.Zero, 0U, null, null);
		}

		// Token: 0x04001E78 RID: 7800
		internal const string NTLM = "NTLM";

		// Token: 0x04001E79 RID: 7801
		private HttpListener m_Listener;

		// Token: 0x04001E7A RID: 7802
		private HttpListenerRequest m_Request;

		// Token: 0x04001E7B RID: 7803
		private HttpListenerResponse m_Response;

		// Token: 0x04001E7C RID: 7804
		private IPrincipal m_User;

		// Token: 0x04001E7D RID: 7805
		private string m_MutualAuthentication;

		// Token: 0x04001E7E RID: 7806
		private bool m_PromoteCookiesToRfc2965;
	}
}
