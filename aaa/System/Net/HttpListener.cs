using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003CA RID: 970
	public sealed class HttpListener : IDisposable
	{
		// Token: 0x06001E7C RID: 7804 RVA: 0x0007491C File Offset: 0x0007391C
		public HttpListener()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "HttpListener", "");
			}
			if (!UnsafeNclNativeMethods.HttpApi.Supported)
			{
				throw new PlatformNotSupportedException();
			}
			this.m_State = HttpListener.State.Stopped;
			this.m_InternalLock = new object();
			this.m_DefaultServiceNames = new ServiceNameStore();
			this.m_ExtendedProtectionPolicy = new ExtendedProtectionPolicy(PolicyEnforcement.Never);
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "HttpListener", "");
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001E7D RID: 7805 RVA: 0x000749B3 File Offset: 0x000739B3
		internal SafeCloseHandle RequestQueueHandle
		{
			get
			{
				return this.m_RequestQueueHandle;
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001E7E RID: 7806 RVA: 0x000749BC File Offset: 0x000739BC
		// (set) Token: 0x06001E7F RID: 7807 RVA: 0x000749DC File Offset: 0x000739DC
		public AuthenticationSchemeSelector AuthenticationSchemeSelectorDelegate
		{
			get
			{
				HttpListener.AuthenticationSelectorInfo authenticationDelegate = this.m_AuthenticationDelegate;
				if (authenticationDelegate != null)
				{
					return authenticationDelegate.Delegate;
				}
				return null;
			}
			set
			{
				this.CheckDisposed();
				try
				{
					new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Demand();
					this.m_AuthenticationDelegate = new HttpListener.AuthenticationSelectorInfo(value, true);
				}
				catch (SecurityException ex)
				{
					this.m_SecurityException = ex;
					this.m_AuthenticationDelegate = new HttpListener.AuthenticationSelectorInfo(value, false);
				}
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001E80 RID: 7808 RVA: 0x00074A34 File Offset: 0x00073A34
		// (set) Token: 0x06001E81 RID: 7809 RVA: 0x00074A3C File Offset: 0x00073A3C
		public HttpListener.ExtendedProtectionSelector ExtendedProtectionSelectorDelegate
		{
			get
			{
				return this.m_ExtendedProtectionSelectorDelegate;
			}
			set
			{
				this.CheckDisposed();
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (!AuthenticationManager.OSSupportsExtendedProtection)
				{
					throw new PlatformNotSupportedException(SR.GetString("security_ExtendedProtection_NoOSSupport"));
				}
				this.m_ExtendedProtectionSelectorDelegate = value;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001E82 RID: 7810 RVA: 0x00074A6B File Offset: 0x00073A6B
		// (set) Token: 0x06001E83 RID: 7811 RVA: 0x00074A73 File Offset: 0x00073A73
		public AuthenticationSchemes AuthenticationSchemes
		{
			get
			{
				return this.m_AuthenticationScheme;
			}
			set
			{
				this.CheckDisposed();
				if ((value & (AuthenticationSchemes.Digest | AuthenticationSchemes.Negotiate | AuthenticationSchemes.Ntlm)) != AuthenticationSchemes.None)
				{
					new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Demand();
				}
				this.m_AuthenticationScheme = value;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001E84 RID: 7812 RVA: 0x00074A96 File Offset: 0x00073A96
		// (set) Token: 0x06001E85 RID: 7813 RVA: 0x00074AA0 File Offset: 0x00073AA0
		public ExtendedProtectionPolicy ExtendedProtectionPolicy
		{
			get
			{
				return this.m_ExtendedProtectionPolicy;
			}
			set
			{
				this.CheckDisposed();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!AuthenticationManager.OSSupportsExtendedProtection && value.PolicyEnforcement == PolicyEnforcement.Always)
				{
					throw new PlatformNotSupportedException(SR.GetString("security_ExtendedProtection_NoOSSupport"));
				}
				if (value.CustomChannelBinding != null)
				{
					throw new ArgumentException(SR.GetString("net_listener_cannot_set_custom_cbt"), "CustomChannelBinding");
				}
				this.m_ExtendedProtectionPolicy = value;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001E86 RID: 7814 RVA: 0x00074B05 File Offset: 0x00073B05
		public ServiceNameCollection DefaultServiceNames
		{
			get
			{
				return this.m_DefaultServiceNames.ServiceNames;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001E87 RID: 7815 RVA: 0x00074B12 File Offset: 0x00073B12
		// (set) Token: 0x06001E88 RID: 7816 RVA: 0x00074B1A File Offset: 0x00073B1A
		public string Realm
		{
			get
			{
				return this.m_Realm;
			}
			set
			{
				this.CheckDisposed();
				this.m_Realm = value;
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001E89 RID: 7817 RVA: 0x00074B29 File Offset: 0x00073B29
		public static bool IsSupported
		{
			get
			{
				return UnsafeNclNativeMethods.HttpApi.Supported;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x00074B30 File Offset: 0x00073B30
		public bool IsListening
		{
			get
			{
				return this.m_State == HttpListener.State.Started;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x00074B3B File Offset: 0x00073B3B
		// (set) Token: 0x06001E8C RID: 7820 RVA: 0x00074B43 File Offset: 0x00073B43
		public bool IgnoreWriteExceptions
		{
			get
			{
				return this.m_IgnoreWriteExceptions;
			}
			set
			{
				this.CheckDisposed();
				this.m_IgnoreWriteExceptions = value;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x00074B52 File Offset: 0x00073B52
		// (set) Token: 0x06001E8E RID: 7822 RVA: 0x00074B5C File Offset: 0x00073B5C
		public bool UnsafeConnectionNtlmAuthentication
		{
			get
			{
				return this.m_UnsafeConnectionNtlmAuthentication;
			}
			set
			{
				this.CheckDisposed();
				if (this.m_UnsafeConnectionNtlmAuthentication == value)
				{
					return;
				}
				lock (this.DisconnectResults.SyncRoot)
				{
					if (this.m_UnsafeConnectionNtlmAuthentication != value)
					{
						this.m_UnsafeConnectionNtlmAuthentication = value;
						if (!value)
						{
							foreach (object obj in this.DisconnectResults.Values)
							{
								HttpListener.DisconnectAsyncResult disconnectAsyncResult = (HttpListener.DisconnectAsyncResult)obj;
								disconnectAsyncResult.AuthenticatedConnection = null;
							}
						}
					}
				}
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x00074C08 File Offset: 0x00073C08
		private Hashtable DisconnectResults
		{
			get
			{
				if (this.m_DisconnectResults == null)
				{
					lock (this.m_InternalLock)
					{
						if (this.m_DisconnectResults == null)
						{
							this.m_DisconnectResults = Hashtable.Synchronized(new Hashtable());
						}
					}
				}
				return this.m_DisconnectResults;
			}
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x00074C64 File Offset: 0x00073C64
		internal unsafe void AddPrefix(string uriPrefix)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "AddPrefix", "uriPrefix:" + uriPrefix);
			}
			string text = null;
			try
			{
				if (uriPrefix == null)
				{
					throw new ArgumentNullException("uriPrefix");
				}
				new WebPermission(NetworkAccess.Accept, uriPrefix).Demand();
				this.CheckDisposed();
				int num;
				if (string.Compare(uriPrefix, 0, "http://", 0, 7, StringComparison.OrdinalIgnoreCase) == 0)
				{
					num = 7;
				}
				else
				{
					if (string.Compare(uriPrefix, 0, "https://", 0, 8, StringComparison.OrdinalIgnoreCase) != 0)
					{
						throw new ArgumentException(SR.GetString("net_listener_scheme"), "uriPrefix");
					}
					num = 8;
				}
				bool flag = false;
				int num2 = num;
				while (num2 < uriPrefix.Length && uriPrefix[num2] != '/' && (uriPrefix[num2] != ':' || flag))
				{
					if (uriPrefix[num2] == '[')
					{
						if (flag)
						{
							num2 = num;
							break;
						}
						flag = true;
					}
					if (flag && uriPrefix[num2] == ']')
					{
						flag = false;
					}
					num2++;
				}
				if (num == num2)
				{
					throw new ArgumentException(SR.GetString("net_listener_host"), "uriPrefix");
				}
				if (uriPrefix[uriPrefix.Length - 1] != '/')
				{
					throw new ArgumentException(SR.GetString("net_listener_slash"), "uriPrefix");
				}
				text = ((uriPrefix[num2] == ':') ? string.Copy(uriPrefix) : (uriPrefix.Substring(0, num2) + ((num == 7) ? ":80" : ":443") + uriPrefix.Substring(num2)));
				try
				{
					fixed (char* ptr = text)
					{
						num = 0;
						while (ptr[num] != ':')
						{
							ptr[num] = (char)CaseInsensitiveAscii.AsciiToLower[(int)((byte)ptr[num])];
							num++;
						}
					}
				}
				finally
				{
					string text2 = null;
				}
				if (this.m_State == HttpListener.State.Started)
				{
					uint num3 = this.InternalAddPrefix(text);
					if (num3 != 0U)
					{
						if (num3 == 183U)
						{
							throw new HttpListenerException((int)num3, SR.GetString("net_listener_already", new object[] { text }));
						}
						throw new HttpListenerException((int)num3);
					}
				}
				this.m_UriPrefixes[uriPrefix] = text;
				this.m_DefaultServiceNames.Add(uriPrefix);
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "AddPrefix", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "AddPrefix", "prefix:" + text);
				}
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001E91 RID: 7825 RVA: 0x00074EF4 File Offset: 0x00073EF4
		public HttpListenerPrefixCollection Prefixes
		{
			get
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.HttpListener, this, "Prefixes_get", "");
				}
				this.CheckDisposed();
				if (this.m_Prefixes == null)
				{
					this.m_Prefixes = new HttpListenerPrefixCollection(this);
				}
				return this.m_Prefixes;
			}
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x00074F34 File Offset: 0x00073F34
		internal bool RemovePrefix(string uriPrefix)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "RemovePrefix", "uriPrefix:" + uriPrefix);
			}
			try
			{
				this.CheckDisposed();
				if (uriPrefix == null)
				{
					throw new ArgumentNullException("uriPrefix");
				}
				if (!this.m_UriPrefixes.Contains(uriPrefix))
				{
					return false;
				}
				if (this.m_State == HttpListener.State.Started)
				{
					this.InternalRemovePrefix((string)this.m_UriPrefixes[uriPrefix]);
				}
				this.m_UriPrefixes.Remove(uriPrefix);
				this.m_DefaultServiceNames.Remove(uriPrefix);
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "RemovePrefix", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "RemovePrefix", "uriPrefix:" + uriPrefix);
				}
			}
			return true;
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x00075024 File Offset: 0x00074024
		internal void RemoveAll(bool clear)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "RemoveAll", "");
			}
			try
			{
				this.CheckDisposed();
				if (this.m_UriPrefixes.Count > 0)
				{
					if (this.m_State == HttpListener.State.Started)
					{
						foreach (object obj in this.m_UriPrefixes.Values)
						{
							string text = (string)obj;
							this.InternalRemovePrefix(text);
						}
					}
					if (clear)
					{
						this.m_UriPrefixes.Clear();
						this.m_DefaultServiceNames.Clear();
					}
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "RemoveAll", "");
				}
			}
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x00075100 File Offset: 0x00074100
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal void EnsureBoundHandle()
		{
			if (!this.m_RequestHandleBound)
			{
				lock (this.m_InternalLock)
				{
					if (!this.m_RequestHandleBound)
					{
						ThreadPool.BindHandle(this.m_RequestQueueHandle.DangerousGetHandle());
						this.m_RequestHandleBound = true;
					}
				}
			}
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x0007515C File Offset: 0x0007415C
		public void Start()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Start", "");
			}
			try
			{
				this.CheckDisposed();
				if (this.m_State != HttpListener.State.Started)
				{
					this.m_RequestQueueHandle = SafeCloseHandle.CreateRequestQueueHandle();
					this.AddAll();
					this.m_State = HttpListener.State.Started;
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "Start", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Start", "");
				}
			}
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x00075208 File Offset: 0x00074208
		public void Stop()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Stop", "");
			}
			try
			{
				this.CheckDisposed();
				if (this.m_State != HttpListener.State.Stopped)
				{
					this.RemoveAll(false);
					this.m_RequestQueueHandle.Close();
					this.m_RequestHandleBound = false;
					this.m_State = HttpListener.State.Stopped;
					this.ClearDigestCache();
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "Stop", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Stop", "");
				}
			}
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x000752C0 File Offset: 0x000742C0
		public void Abort()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Abort", "");
			}
			try
			{
				if (this.m_RequestQueueHandle != null)
				{
					this.m_RequestQueueHandle.Abort();
				}
				this.m_RequestHandleBound = false;
				this.m_State = HttpListener.State.Closed;
				this.ClearDigestCache();
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "Abort", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Abort", "");
				}
			}
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x00075368 File Offset: 0x00074368
		public void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Close", "");
			}
			try
			{
				((IDisposable)this).Dispose();
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "Close", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Close", "");
				}
			}
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x000753F0 File Offset: 0x000743F0
		private void Dispose(bool disposing)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Dispose", "");
			}
			try
			{
				if (this.m_State != HttpListener.State.Closed)
				{
					this.Stop();
					this.m_RequestHandleBound = false;
					this.m_State = HttpListener.State.Closed;
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "Dispose", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Dispose", "");
				}
			}
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x00075490 File Offset: 0x00074490
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0007549C File Offset: 0x0007449C
		private unsafe uint InternalAddPrefix(string uriPrefix)
		{
			uint num;
			fixed (char* ptr = uriPrefix)
			{
				num = UnsafeNclNativeMethods.HttpApi.HttpAddUrl(this.m_RequestQueueHandle, (ushort*)ptr, null);
			}
			return num;
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x000754CC File Offset: 0x000744CC
		private unsafe bool InternalRemovePrefix(string uriPrefix)
		{
			uint num;
			fixed (char* ptr = uriPrefix)
			{
				num = UnsafeNclNativeMethods.HttpApi.HttpRemoveUrl(this.m_RequestQueueHandle, (ushort*)ptr);
			}
			return num != 1168U;
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x00075504 File Offset: 0x00074504
		private void AddAll()
		{
			if (this.m_UriPrefixes.Count > 0)
			{
				foreach (object obj in this.m_UriPrefixes.Values)
				{
					string text = (string)obj;
					uint num = this.InternalAddPrefix(text);
					if (num != 0U)
					{
						this.Abort();
						if (num == 183U)
						{
							throw new HttpListenerException((int)num, SR.GetString("net_listener_already", new object[] { text }));
						}
						throw new HttpListenerException((int)num);
					}
				}
			}
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x000755AC File Offset: 0x000745AC
		public unsafe HttpListenerContext GetContext()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "GetContext", "");
			}
			SyncRequestContext syncRequestContext = null;
			HttpListenerContext httpListenerContext = null;
			bool flag = false;
			checked
			{
				HttpListenerContext httpListenerContext2;
				try
				{
					this.CheckDisposed();
					if (this.m_State == HttpListener.State.Stopped)
					{
						throw new InvalidOperationException(SR.GetString("net_listener_mustcall", new object[] { "Start()" }));
					}
					if (this.m_UriPrefixes.Count == 0)
					{
						throw new InvalidOperationException(SR.GetString("net_listener_mustcall", new object[] { "AddPrefix()" }));
					}
					uint num = 4096U;
					ulong num2 = 0UL;
					syncRequestContext = new SyncRequestContext((int)num);
					uint num4;
					for (;;)
					{
						uint num3 = 0U;
						num4 = UnsafeNclNativeMethods.HttpApi.HttpReceiveHttpRequest(this.m_RequestQueueHandle, num2, 1U, syncRequestContext.RequestBlob, num, &num3, null);
						if (num4 == 87U && num2 != 0UL)
						{
							num2 = 0UL;
						}
						else if (num4 == 234U)
						{
							num = num3;
							num2 = syncRequestContext.RequestBlob->RequestId;
							syncRequestContext.Reset((int)num);
						}
						else
						{
							if (num4 != 0U)
							{
								break;
							}
							httpListenerContext = this.HandleAuthentication(syncRequestContext, out flag);
							if (flag)
							{
								syncRequestContext = null;
								flag = false;
							}
							if (httpListenerContext != null)
							{
								goto Block_12;
							}
							if (syncRequestContext == null)
							{
								syncRequestContext = new SyncRequestContext((int)num);
							}
							num2 = 0UL;
						}
					}
					throw new HttpListenerException((int)num4);
					Block_12:
					httpListenerContext2 = httpListenerContext;
				}
				catch (Exception ex)
				{
					if (Logging.On)
					{
						Logging.Exception(Logging.HttpListener, this, "GetContext", ex);
					}
					throw;
				}
				finally
				{
					if (syncRequestContext != null && !flag)
					{
						syncRequestContext.ReleasePins();
						syncRequestContext.Close();
					}
					if (Logging.On)
					{
						Logging.Exit(Logging.HttpListener, this, "GetContext", string.Concat(new object[]
						{
							"HttpListenerContext#",
							ValidationHelper.HashString(httpListenerContext),
							" RequestTraceIdentifier#",
							httpListenerContext.Request.RequestTraceIdentifier
						}));
					}
				}
				return httpListenerContext2;
			}
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x0007579C File Offset: 0x0007479C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginGetContext(AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "BeginGetContext", "");
			}
			ListenerAsyncResult listenerAsyncResult = null;
			try
			{
				this.CheckDisposed();
				if (this.m_State == HttpListener.State.Stopped)
				{
					throw new InvalidOperationException(SR.GetString("net_listener_mustcall", new object[] { "Start()" }));
				}
				listenerAsyncResult = new ListenerAsyncResult(this, state, callback);
				uint num = listenerAsyncResult.QueueBeginGetContext();
				if (num != 0U && num != 997U)
				{
					throw new HttpListenerException((int)num);
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "BeginGetContext", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.HttpListener, this, "BeginGetContext", "IAsyncResult#" + ValidationHelper.HashString(listenerAsyncResult));
				}
			}
			return listenerAsyncResult;
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x0007587C File Offset: 0x0007487C
		public HttpListenerContext EndGetContext(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "EndGetContext", "IAsyncResult#" + ValidationHelper.HashString(asyncResult));
			}
			HttpListenerContext httpListenerContext = null;
			try
			{
				this.CheckDisposed();
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				ListenerAsyncResult listenerAsyncResult = asyncResult as ListenerAsyncResult;
				if (listenerAsyncResult == null || listenerAsyncResult.AsyncObject != this)
				{
					throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
				}
				if (listenerAsyncResult.EndCalled)
				{
					throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndGetContext" }));
				}
				listenerAsyncResult.EndCalled = true;
				httpListenerContext = listenerAsyncResult.InternalWaitForCompletion() as HttpListenerContext;
				if (httpListenerContext == null)
				{
					throw listenerAsyncResult.Result as Exception;
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "EndGetContext", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "EndGetContext", (httpListenerContext == null) ? "<no context>" : string.Concat(new object[]
					{
						"HttpListenerContext#",
						ValidationHelper.HashString(httpListenerContext),
						" RequestTraceIdentifier#",
						httpListenerContext.Request.RequestTraceIdentifier
					}));
				}
			}
			return httpListenerContext;
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000759D8 File Offset: 0x000749D8
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private WindowsIdentity CreateWindowsIdentity(IntPtr userToken, string type, WindowsAccountType acctType, bool isAuthenticated)
		{
			return new WindowsIdentity(userToken, type, acctType, isAuthenticated);
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000759E4 File Offset: 0x000749E4
		internal unsafe HttpListenerContext HandleAuthentication(RequestContextBase memoryBlob, out bool stoleBlob)
		{
			string text = null;
			stoleBlob = false;
			string verb = UnsafeNclNativeMethods.HttpApi.GetVerb(memoryBlob.RequestBlob);
			string knownHeader = UnsafeNclNativeMethods.HttpApi.GetKnownHeader(memoryBlob.RequestBlob, 24);
			ulong connectionId = memoryBlob.RequestBlob->ConnectionId;
			ulong requestId = memoryBlob.RequestBlob->RequestId;
			bool flag = memoryBlob.RequestBlob->pSslInfo != null;
			HttpListener.DisconnectAsyncResult disconnectAsyncResult = (HttpListener.DisconnectAsyncResult)this.DisconnectResults[connectionId];
			if (this.UnsafeConnectionNtlmAuthentication)
			{
				if (knownHeader == null)
				{
					WindowsPrincipal windowsPrincipal = ((disconnectAsyncResult == null) ? null : disconnectAsyncResult.AuthenticatedConnection);
					if (windowsPrincipal != null)
					{
						stoleBlob = true;
						HttpListenerContext httpListenerContext = new HttpListenerContext(this, memoryBlob);
						httpListenerContext.SetIdentity(windowsPrincipal, null);
						httpListenerContext.Request.ReleasePins();
						return httpListenerContext;
					}
				}
				else if (disconnectAsyncResult != null)
				{
					disconnectAsyncResult.AuthenticatedConnection = null;
				}
			}
			stoleBlob = true;
			HttpListenerContext httpListenerContext2 = null;
			NTAuthentication ntauthentication = null;
			NTAuthentication ntauthentication2 = null;
			NTAuthentication ntauthentication3 = null;
			AuthenticationSchemes authenticationSchemes = AuthenticationSchemes.None;
			AuthenticationSchemes authenticationSchemes2 = this.AuthenticationSchemes;
			ExtendedProtectionPolicy extendedProtectionPolicy = this.m_ExtendedProtectionPolicy;
			HttpListenerContext httpListenerContext3;
			try
			{
				if (disconnectAsyncResult != null && !disconnectAsyncResult.StartOwningDisconnectHandling())
				{
					disconnectAsyncResult = null;
				}
				if (disconnectAsyncResult != null)
				{
					ntauthentication = disconnectAsyncResult.Session;
				}
				httpListenerContext2 = new HttpListenerContext(this, memoryBlob);
				HttpListener.AuthenticationSelectorInfo authenticationDelegate = this.m_AuthenticationDelegate;
				if (authenticationDelegate != null)
				{
					try
					{
						httpListenerContext2.Request.ReleasePins();
						authenticationSchemes2 = authenticationDelegate.Delegate(httpListenerContext2.Request);
						if (!authenticationDelegate.AdvancedAuth && (authenticationSchemes2 & (AuthenticationSchemes.Digest | AuthenticationSchemes.Negotiate | AuthenticationSchemes.Ntlm)) != AuthenticationSchemes.None)
						{
							throw this.m_SecurityException;
						}
						goto IL_01A2;
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						if (Logging.On)
						{
							Logging.PrintError(Logging.HttpListener, this, "HandleAuthentication", SR.GetString("net_log_listener_delegate_exception", new object[] { ex }));
						}
						this.SendError(requestId, HttpStatusCode.InternalServerError, null);
						httpListenerContext2.Close();
						return null;
					}
				}
				stoleBlob = false;
				IL_01A2:
				HttpListener.ExtendedProtectionSelector extendedProtectionSelectorDelegate = this.m_ExtendedProtectionSelectorDelegate;
				if (extendedProtectionSelectorDelegate != null)
				{
					extendedProtectionPolicy = extendedProtectionSelectorDelegate(httpListenerContext2.Request);
					if (extendedProtectionPolicy == null)
					{
						extendedProtectionPolicy = new ExtendedProtectionPolicy(PolicyEnforcement.Never);
					}
				}
				int num = -1;
				if (knownHeader != null && (authenticationSchemes2 & ~AuthenticationSchemes.Anonymous) != AuthenticationSchemes.None)
				{
					num = 0;
					while (num < knownHeader.Length && knownHeader[num] != ' ' && knownHeader[num] != '\t' && knownHeader[num] != '\r' && knownHeader[num] != '\n')
					{
						num++;
					}
					if (num < knownHeader.Length)
					{
						if ((authenticationSchemes2 & AuthenticationSchemes.Negotiate) != AuthenticationSchemes.None && string.Compare(knownHeader, 0, "Negotiate", 0, num, StringComparison.OrdinalIgnoreCase) == 0)
						{
							authenticationSchemes = AuthenticationSchemes.Negotiate;
						}
						else if ((authenticationSchemes2 & AuthenticationSchemes.Ntlm) != AuthenticationSchemes.None && string.Compare(knownHeader, 0, "NTLM", 0, num, StringComparison.OrdinalIgnoreCase) == 0)
						{
							authenticationSchemes = AuthenticationSchemes.Ntlm;
						}
						else if ((authenticationSchemes2 & AuthenticationSchemes.Digest) != AuthenticationSchemes.None && string.Compare(knownHeader, 0, "Digest", 0, num, StringComparison.OrdinalIgnoreCase) == 0)
						{
							authenticationSchemes = AuthenticationSchemes.Digest;
						}
						else if ((authenticationSchemes2 & AuthenticationSchemes.Basic) != AuthenticationSchemes.None && string.Compare(knownHeader, 0, "Basic", 0, num, StringComparison.OrdinalIgnoreCase) == 0)
						{
							authenticationSchemes = AuthenticationSchemes.Basic;
						}
						else if (Logging.On)
						{
							Logging.PrintWarning(Logging.HttpListener, this, "HandleAuthentication", SR.GetString("net_log_listener_unsupported_authentication_scheme", new object[] { knownHeader, authenticationSchemes2 }));
						}
					}
				}
				HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
				bool flag2 = false;
				if (authenticationSchemes == AuthenticationSchemes.None)
				{
					if (Logging.On)
					{
						Logging.PrintWarning(Logging.HttpListener, this, "HandleAuthentication", SR.GetString("net_log_listener_unmatched_authentication_scheme", new object[]
						{
							ValidationHelper.ToString(authenticationSchemes2),
							(knownHeader == null) ? "<null>" : knownHeader
						}));
					}
					if ((authenticationSchemes2 & AuthenticationSchemes.Anonymous) != AuthenticationSchemes.None)
					{
						if (!stoleBlob)
						{
							stoleBlob = true;
							httpListenerContext2.Request.ReleasePins();
						}
						return httpListenerContext2;
					}
					httpStatusCode = HttpStatusCode.Unauthorized;
					httpListenerContext2.Request.DetachBlob(memoryBlob);
					httpListenerContext2.Close();
					httpListenerContext2 = null;
				}
				else
				{
					byte[] array = null;
					byte[] array2 = null;
					string text2 = null;
					num++;
					while (num < knownHeader.Length && (knownHeader[num] == ' ' || knownHeader[num] == '\t' || knownHeader[num] == '\r' || knownHeader[num] == '\n'))
					{
						num++;
					}
					string text3 = ((num < knownHeader.Length) ? knownHeader.Substring(num) : "");
					IPrincipal principal = null;
					bool flag3 = false;
					AuthenticationSchemes authenticationSchemes3 = authenticationSchemes;
					switch (authenticationSchemes3)
					{
					case AuthenticationSchemes.Digest:
					{
						ChannelBinding channelBinding = this.GetChannelBinding(connectionId, flag, extendedProtectionPolicy, out flag3);
						if (!flag3)
						{
							ntauthentication3 = new NTAuthentication(true, "WDigest", null, this.GetContextFlags(extendedProtectionPolicy, flag), channelBinding);
							SecurityStatus securityStatus;
							text2 = ntauthentication3.GetOutgoingDigestBlob(text3, verb, null, this.Realm, false, false, out securityStatus);
							if (securityStatus == SecurityStatus.OK)
							{
								text2 = null;
							}
							if (ntauthentication3.IsValidContext)
							{
								SafeCloseHandle safeCloseHandle = null;
								try
								{
									if (!this.CheckSpn(ntauthentication3, flag, extendedProtectionPolicy))
									{
										httpStatusCode = HttpStatusCode.Unauthorized;
									}
									else
									{
										httpListenerContext2.Request.ServiceName = ntauthentication3.ClientSpecifiedSpn;
										safeCloseHandle = ntauthentication3.GetContextToken(out securityStatus);
										if (securityStatus != SecurityStatus.OK)
										{
											httpStatusCode = this.HttpStatusFromSecurityStatus(securityStatus);
										}
										else if (safeCloseHandle == null)
										{
											httpStatusCode = HttpStatusCode.Unauthorized;
										}
										else
										{
											principal = new WindowsPrincipal(this.CreateWindowsIdentity(safeCloseHandle.DangerousGetHandle(), "Digest", WindowsAccountType.Normal, true));
										}
									}
								}
								finally
								{
									if (safeCloseHandle != null)
									{
										safeCloseHandle.Close();
									}
								}
								ntauthentication2 = ntauthentication3;
								if (text2 != null)
								{
									text = "Digest " + text2;
								}
							}
							else
							{
								httpStatusCode = this.HttpStatusFromSecurityStatus(securityStatus);
							}
						}
						else
						{
							httpStatusCode = HttpStatusCode.Unauthorized;
						}
						break;
					}
					case AuthenticationSchemes.Negotiate:
					case AuthenticationSchemes.Ntlm:
					{
						string text4 = ((authenticationSchemes == AuthenticationSchemes.Ntlm) ? "NTLM" : "Negotiate");
						if (ntauthentication != null && ntauthentication.Package == text4)
						{
							ntauthentication3 = ntauthentication;
						}
						else
						{
							ChannelBinding channelBinding = this.GetChannelBinding(connectionId, flag, extendedProtectionPolicy, out flag3);
							if (!flag3)
							{
								ntauthentication3 = new NTAuthentication(true, text4, null, this.GetContextFlags(extendedProtectionPolicy, flag), channelBinding);
							}
						}
						if (!flag3)
						{
							try
							{
								array = Convert.FromBase64String(text3);
							}
							catch (FormatException)
							{
								httpStatusCode = HttpStatusCode.BadRequest;
								flag2 = true;
							}
							if (!flag2)
							{
								SecurityStatus securityStatus;
								array2 = ntauthentication3.GetOutgoingBlob(array, false, out securityStatus);
								flag2 = !ntauthentication3.IsValidContext;
								if (flag2)
								{
									if (securityStatus == SecurityStatus.InvalidHandle && ntauthentication == null && array != null && array.Length > 0)
									{
										securityStatus = SecurityStatus.InvalidToken;
									}
									httpStatusCode = this.HttpStatusFromSecurityStatus(securityStatus);
								}
							}
							if (array2 != null)
							{
								text2 = Convert.ToBase64String(array2);
							}
							if (!flag2)
							{
								if (ntauthentication3.IsCompleted)
								{
									SafeCloseHandle safeCloseHandle2 = null;
									try
									{
										if (!this.CheckSpn(ntauthentication3, flag, extendedProtectionPolicy))
										{
											httpStatusCode = HttpStatusCode.Unauthorized;
										}
										else
										{
											httpListenerContext2.Request.ServiceName = ntauthentication3.ClientSpecifiedSpn;
											SecurityStatus securityStatus;
											safeCloseHandle2 = ntauthentication3.GetContextToken(out securityStatus);
											if (securityStatus != SecurityStatus.OK)
											{
												httpStatusCode = this.HttpStatusFromSecurityStatus(securityStatus);
											}
											else
											{
												WindowsPrincipal windowsPrincipal2 = new WindowsPrincipal(this.CreateWindowsIdentity(safeCloseHandle2.DangerousGetHandle(), ntauthentication3.ProtocolName, WindowsAccountType.Normal, true));
												principal = windowsPrincipal2;
												if (this.UnsafeConnectionNtlmAuthentication && ntauthentication3.ProtocolName == "NTLM")
												{
													if (disconnectAsyncResult == null)
													{
														this.RegisterForDisconnectNotification(connectionId, ref disconnectAsyncResult);
													}
													if (disconnectAsyncResult != null)
													{
														lock (this.DisconnectResults.SyncRoot)
														{
															if (this.UnsafeConnectionNtlmAuthentication)
															{
																disconnectAsyncResult.AuthenticatedConnection = windowsPrincipal2;
															}
														}
													}
												}
											}
										}
										break;
									}
									finally
									{
										if (safeCloseHandle2 != null)
										{
											safeCloseHandle2.Close();
										}
									}
								}
								ntauthentication2 = ntauthentication3;
								text = ((authenticationSchemes == AuthenticationSchemes.Ntlm) ? "NTLM" : "Negotiate");
								if (!string.IsNullOrEmpty(text2))
								{
									text = text + " " + text2;
								}
							}
						}
						else
						{
							httpStatusCode = HttpStatusCode.Unauthorized;
						}
						break;
					}
					case AuthenticationSchemes.Digest | AuthenticationSchemes.Negotiate:
						break;
					default:
						if (authenticationSchemes3 == AuthenticationSchemes.Basic)
						{
							try
							{
								array = Convert.FromBase64String(text3);
								text3 = WebHeaderCollection.HeaderEncoding.GetString(array, 0, array.Length);
								num = text3.IndexOf(':');
								if (num != -1)
								{
									string text5 = text3.Substring(0, num);
									string text6 = text3.Substring(num + 1);
									principal = new GenericPrincipal(new HttpListenerBasicIdentity(text5, text6), null);
								}
								else
								{
									httpStatusCode = HttpStatusCode.BadRequest;
								}
							}
							catch (FormatException)
							{
							}
						}
						break;
					}
					if (principal != null)
					{
						httpListenerContext2.SetIdentity(principal, text2);
					}
					else
					{
						if (Logging.On)
						{
							Logging.PrintWarning(Logging.HttpListener, this, "HandleAuthentication", SR.GetString("net_log_listener_create_valid_identity_failed"));
						}
						httpListenerContext2.Request.DetachBlob(memoryBlob);
						httpListenerContext2.Close();
						httpListenerContext2 = null;
					}
				}
				ArrayList arrayList = null;
				if (httpListenerContext2 == null)
				{
					if (text != null)
					{
						HttpListener.AddChallenge(ref arrayList, text);
					}
					else
					{
						if (ntauthentication2 != null)
						{
							if (ntauthentication2 == ntauthentication3)
							{
								ntauthentication3 = null;
							}
							if (ntauthentication2 != ntauthentication)
							{
								NTAuthentication ntauthentication4 = ntauthentication2;
								ntauthentication2 = null;
								ntauthentication4.CloseContext();
							}
							else
							{
								ntauthentication2 = null;
							}
						}
						if (httpStatusCode != HttpStatusCode.Unauthorized)
						{
							this.SendError(requestId, httpStatusCode, null);
							return null;
						}
						arrayList = this.BuildChallenge(authenticationSchemes2, connectionId, out ntauthentication2, extendedProtectionPolicy, flag);
					}
				}
				if (disconnectAsyncResult == null && ntauthentication2 != null)
				{
					this.RegisterForDisconnectNotification(connectionId, ref disconnectAsyncResult);
					if (disconnectAsyncResult == null)
					{
						if (ntauthentication2 != null)
						{
							if (ntauthentication2 == ntauthentication3)
							{
								ntauthentication3 = null;
							}
							if (ntauthentication2 != ntauthentication)
							{
								NTAuthentication ntauthentication5 = ntauthentication2;
								ntauthentication2 = null;
								ntauthentication5.CloseContext();
							}
							else
							{
								ntauthentication2 = null;
							}
						}
						this.SendError(requestId, HttpStatusCode.InternalServerError, null);
						httpListenerContext2.Request.DetachBlob(memoryBlob);
						httpListenerContext2.Close();
						return null;
					}
				}
				if (ntauthentication != ntauthentication2)
				{
					if (ntauthentication == ntauthentication3)
					{
						ntauthentication3 = null;
					}
					NTAuthentication ntauthentication6 = ntauthentication;
					ntauthentication = ntauthentication2;
					disconnectAsyncResult.Session = ntauthentication2;
					if (ntauthentication6 != null)
					{
						if ((authenticationSchemes2 & AuthenticationSchemes.Digest) != AuthenticationSchemes.None)
						{
							this.SaveDigestContext(ntauthentication6);
						}
						else
						{
							ntauthentication6.CloseContext();
						}
					}
				}
				if (httpListenerContext2 == null)
				{
					this.SendError(requestId, (arrayList != null && arrayList.Count > 0) ? HttpStatusCode.Unauthorized : HttpStatusCode.Forbidden, arrayList);
					httpListenerContext3 = null;
				}
				else
				{
					if (!stoleBlob)
					{
						stoleBlob = true;
						httpListenerContext2.Request.ReleasePins();
					}
					httpListenerContext3 = httpListenerContext2;
				}
			}
			catch
			{
				if (httpListenerContext2 != null)
				{
					httpListenerContext2.Request.DetachBlob(memoryBlob);
					httpListenerContext2.Close();
				}
				if (ntauthentication2 != null)
				{
					if (ntauthentication2 == ntauthentication3)
					{
						ntauthentication3 = null;
					}
					if (ntauthentication2 != ntauthentication)
					{
						NTAuthentication ntauthentication7 = ntauthentication2;
						ntauthentication2 = null;
						ntauthentication7.CloseContext();
					}
					else
					{
						ntauthentication2 = null;
					}
				}
				throw;
			}
			finally
			{
				try
				{
					if (ntauthentication != null && ntauthentication != ntauthentication2)
					{
						if (ntauthentication2 == null && disconnectAsyncResult != null)
						{
							disconnectAsyncResult.Session = null;
						}
						if ((authenticationSchemes2 & AuthenticationSchemes.Digest) != AuthenticationSchemes.None)
						{
							this.SaveDigestContext(ntauthentication);
						}
						else
						{
							ntauthentication.CloseContext();
						}
					}
					if (ntauthentication3 != null && ntauthentication != ntauthentication3 && ntauthentication2 != ntauthentication3)
					{
						ntauthentication3.CloseContext();
					}
				}
				finally
				{
					if (disconnectAsyncResult != null)
					{
						disconnectAsyncResult.FinishOwningDisconnectHandling();
					}
				}
			}
			return httpListenerContext3;
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x00076488 File Offset: 0x00075488
		private static bool ScenarioChecksChannelBinding(bool isSecureConnection, ProtectionScenario scenario)
		{
			return isSecureConnection && scenario == ProtectionScenario.TransportSelected;
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x00076494 File Offset: 0x00075494
		private ChannelBinding GetChannelBinding(ulong connectionId, bool isSecureConnection, ExtendedProtectionPolicy policy, out bool extendedProtectionFailure)
		{
			extendedProtectionFailure = false;
			if (policy.PolicyEnforcement == PolicyEnforcement.Never)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_cbt_disabled"));
				}
				return null;
			}
			if (!isSecureConnection)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_cbt_http"));
				}
				return null;
			}
			if (!AuthenticationManager.OSSupportsExtendedProtection)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_cbt_platform"));
				}
				return null;
			}
			if (policy.ProtectionScenario == ProtectionScenario.TrustedProxy)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_cbt_trustedproxy"));
				}
				return null;
			}
			ChannelBinding channelBindingFromTls = this.GetChannelBindingFromTls(connectionId);
			if (channelBindingFromTls == null)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_cbt"));
				}
				extendedProtectionFailure = true;
			}
			return channelBindingFromTls;
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x00076564 File Offset: 0x00075564
		private bool CheckSpn(NTAuthentication context, bool isSecureConnection, ExtendedProtectionPolicy policy)
		{
			if (context.IsKerberos)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_spn_kerberos"));
				}
				return true;
			}
			if (policy.PolicyEnforcement == PolicyEnforcement.Never || HttpListener.ScenarioChecksChannelBinding(isSecureConnection, policy.ProtectionScenario))
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_spn_disabled"));
				}
				return true;
			}
			if (HttpListener.ScenarioChecksChannelBinding(isSecureConnection, policy.ProtectionScenario))
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_spn_cbt"));
				}
				return true;
			}
			if (!AuthenticationManager.OSSupportsExtendedProtection)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_spn_platform"));
				}
				return true;
			}
			string clientSpecifiedSpn = context.ClientSpecifiedSpn;
			if (string.IsNullOrEmpty(clientSpecifiedSpn))
			{
				bool flag;
				if (policy.PolicyEnforcement == PolicyEnforcement.WhenSupported)
				{
					flag = true;
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_spn_whensupported"));
					}
				}
				else
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_spn_failed_always"));
					}
					flag = false;
				}
				return flag;
			}
			if (string.Compare(clientSpecifiedSpn, "http/localhost", StringComparison.OrdinalIgnoreCase) == 0)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_spn_loopback"));
				}
				return true;
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_spn", new object[] { clientSpecifiedSpn }));
			}
			ServiceNameCollection serviceNames = this.GetServiceNames(policy);
			bool flag2 = false;
			foreach (object obj in serviceNames)
			{
				string text = (string)obj;
				if (string.Compare(clientSpecifiedSpn, text, StringComparison.OrdinalIgnoreCase) == 0)
				{
					flag2 = true;
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_spn_passed"));
						break;
					}
					break;
				}
			}
			if (Logging.On && !flag2)
			{
				Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_spn_failed"));
				if (serviceNames.Count == 0)
				{
					Logging.PrintWarning(Logging.HttpListener, this, "CheckSpn", SR.GetString("net_log_listener_spn_failed_empty"));
				}
				else
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_spn_failed_dump"));
					foreach (object obj2 in serviceNames)
					{
						string text2 = (string)obj2;
						Logging.PrintInfo(Logging.HttpListener, this, "\t" + text2);
					}
				}
			}
			return flag2;
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x00076804 File Offset: 0x00075804
		private ServiceNameCollection GetServiceNames(ExtendedProtectionPolicy policy)
		{
			ServiceNameCollection serviceNameCollection;
			if (policy.CustomServiceNames == null)
			{
				if (this.m_DefaultServiceNames.ServiceNames.Count == 0)
				{
					throw new InvalidOperationException(SR.GetString("net_listener_no_spns"));
				}
				serviceNameCollection = this.m_DefaultServiceNames.ServiceNames;
			}
			else
			{
				serviceNameCollection = policy.CustomServiceNames;
			}
			return serviceNameCollection;
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x00076854 File Offset: 0x00075854
		private ContextFlags GetContextFlags(ExtendedProtectionPolicy policy, bool isSecureConnection)
		{
			ContextFlags contextFlags = ContextFlags.Connection;
			if (policy.PolicyEnforcement != PolicyEnforcement.Never)
			{
				if (policy.PolicyEnforcement == PolicyEnforcement.WhenSupported)
				{
					contextFlags |= ContextFlags.AllowMissingBindings;
				}
				if (policy.ProtectionScenario == ProtectionScenario.TrustedProxy)
				{
					contextFlags |= ContextFlags.ProxyBindings;
				}
			}
			return contextFlags;
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x00076892 File Offset: 0x00075892
		private static void AddChallenge(ref ArrayList challenges, string challenge)
		{
			if (challenge != null)
			{
				challenge = challenge.Trim();
				if (challenge.Length > 0)
				{
					if (challenges == null)
					{
						challenges = new ArrayList(4);
					}
					challenges.Add(challenge);
				}
			}
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x000768C0 File Offset: 0x000758C0
		private ArrayList BuildChallenge(AuthenticationSchemes authenticationScheme, ulong connectionId, out NTAuthentication newContext, ExtendedProtectionPolicy policy, bool isSecureConnection)
		{
			ArrayList arrayList = null;
			newContext = null;
			if ((authenticationScheme & AuthenticationSchemes.Negotiate) != AuthenticationSchemes.None)
			{
				HttpListener.AddChallenge(ref arrayList, "Negotiate");
			}
			if ((authenticationScheme & AuthenticationSchemes.Ntlm) != AuthenticationSchemes.None)
			{
				HttpListener.AddChallenge(ref arrayList, "NTLM");
			}
			if ((authenticationScheme & AuthenticationSchemes.Digest) != AuthenticationSchemes.None)
			{
				NTAuthentication ntauthentication = null;
				try
				{
					bool flag;
					ChannelBinding channelBinding = this.GetChannelBinding(connectionId, isSecureConnection, policy, out flag);
					if (!flag)
					{
						ntauthentication = new NTAuthentication(true, "WDigest", null, this.GetContextFlags(policy, isSecureConnection), channelBinding);
						SecurityStatus securityStatus;
						string outgoingDigestBlob = ntauthentication.GetOutgoingDigestBlob(null, null, null, this.Realm, false, false, out securityStatus);
						if (ntauthentication.IsValidContext)
						{
							newContext = ntauthentication;
						}
						HttpListener.AddChallenge(ref arrayList, "Digest" + (string.IsNullOrEmpty(outgoingDigestBlob) ? "" : (" " + outgoingDigestBlob)));
					}
				}
				finally
				{
					if (ntauthentication != null && newContext != ntauthentication)
					{
						ntauthentication.CloseContext();
					}
				}
			}
			if ((authenticationScheme & AuthenticationSchemes.Basic) != AuthenticationSchemes.None)
			{
				HttpListener.AddChallenge(ref arrayList, "Basic realm=\"" + this.Realm + "\"");
			}
			return arrayList;
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x000769B8 File Offset: 0x000759B8
		private void RegisterForDisconnectNotification(ulong connectionId, ref HttpListener.DisconnectAsyncResult disconnectResult)
		{
			try
			{
				HttpListener.DisconnectAsyncResult disconnectAsyncResult = new HttpListener.DisconnectAsyncResult(this, connectionId);
				this.EnsureBoundHandle();
				uint num = UnsafeNclNativeMethods.HttpApi.HttpWaitForDisconnect(this.m_RequestQueueHandle, connectionId, disconnectAsyncResult.NativeOverlapped);
				if (num == 0U || num == 997U)
				{
					disconnectResult = disconnectAsyncResult;
					this.DisconnectResults[connectionId] = disconnectResult;
				}
			}
			catch (Win32Exception ex)
			{
				int nativeErrorCode = ex.NativeErrorCode;
			}
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x00076A24 File Offset: 0x00075A24
		private unsafe void SendError(ulong requestId, HttpStatusCode httpStatusCode, ArrayList challenges)
		{
			UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE http_RESPONSE = default(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE);
			http_RESPONSE.Version = default(UnsafeNclNativeMethods.HttpApi.HTTP_VERSION);
			http_RESPONSE.Version.MajorVersion = 1;
			http_RESPONSE.Version.MinorVersion = 1;
			http_RESPONSE.StatusCode = (ushort)httpStatusCode;
			string statusDescription = HttpListenerResponse.GetStatusDescription((int)httpStatusCode);
			uint num = 0U;
			byte[] bytes = Encoding.Default.GetBytes(statusDescription);
			uint num2;
			fixed (byte* ptr = bytes)
			{
				http_RESPONSE.pReason = (sbyte*)ptr;
				http_RESPONSE.ReasonLength = (ushort)bytes.Length;
				byte[] bytes2 = Encoding.Default.GetBytes("0");
				fixed (byte* ptr2 = bytes2)
				{
					(&http_RESPONSE.Headers.KnownHeaders)[11].pRawValue = (sbyte*)ptr2;
					(&http_RESPONSE.Headers.KnownHeaders)[11].RawValueLength = (ushort)bytes2.Length;
					http_RESPONSE.Headers.UnknownHeaderCount = checked((ushort)((challenges == null) ? 0 : challenges.Count));
					GCHandle[] array = null;
					UnsafeNclNativeMethods.HttpApi.HTTP_UNKNOWN_HEADER[] array2 = null;
					GCHandle gchandle = default(GCHandle);
					GCHandle gchandle2 = default(GCHandle);
					if (http_RESPONSE.Headers.UnknownHeaderCount > 0)
					{
						array = new GCHandle[(int)http_RESPONSE.Headers.UnknownHeaderCount];
						array2 = new UnsafeNclNativeMethods.HttpApi.HTTP_UNKNOWN_HEADER[(int)http_RESPONSE.Headers.UnknownHeaderCount];
					}
					try
					{
						if (http_RESPONSE.Headers.UnknownHeaderCount > 0)
						{
							gchandle = GCHandle.Alloc(array2, GCHandleType.Pinned);
							http_RESPONSE.Headers.pUnknownHeaders = (UnsafeNclNativeMethods.HttpApi.HTTP_UNKNOWN_HEADER*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(array2, 0);
							gchandle2 = GCHandle.Alloc(HttpListener.s_WwwAuthenticateBytes, GCHandleType.Pinned);
							sbyte* ptr3 = (sbyte*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(HttpListener.s_WwwAuthenticateBytes, 0);
							for (int i = 0; i < array.Length; i++)
							{
								byte[] bytes3 = Encoding.Default.GetBytes((string)challenges[i]);
								array[i] = GCHandle.Alloc(bytes3, GCHandleType.Pinned);
								array2[i].pName = ptr3;
								array2[i].NameLength = (ushort)HttpListener.s_WwwAuthenticateBytes.Length;
								array2[i].pRawValue = (sbyte*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(bytes3, 0);
								array2[i].RawValueLength = checked((ushort)bytes3.Length);
							}
						}
						num2 = UnsafeNclNativeMethods.HttpApi.HttpSendHttpResponse(this.m_RequestQueueHandle, requestId, 0U, &http_RESPONSE, null, &num, SafeLocalFree.Zero, 0U, null, null);
					}
					finally
					{
						if (gchandle.IsAllocated)
						{
							gchandle.Free();
						}
						if (gchandle2.IsAllocated)
						{
							gchandle2.Free();
						}
						if (array != null)
						{
							for (int j = 0; j < array.Length; j++)
							{
								if (array[j].IsAllocated)
								{
									array[j].Free();
								}
							}
						}
					}
				}
			}
			if (num2 != 0U)
			{
				HttpListenerContext.CancelRequest(this.m_RequestQueueHandle, requestId);
			}
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x00076D24 File Offset: 0x00075D24
		private static int GetTokenOffsetFromBlob(IntPtr blob)
		{
			IntPtr intPtr = Marshal.ReadIntPtr(blob, (int)Marshal.OffsetOf(HttpListener.ChannelBindingStatusType, "ChannelToken"));
			return (int)IntPtrHelper.Subtract(intPtr, blob);
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x00076D54 File Offset: 0x00075D54
		private static int GetTokenSizeFromBlob(IntPtr blob)
		{
			return Marshal.ReadInt32(blob, (int)Marshal.OffsetOf(HttpListener.ChannelBindingStatusType, "ChannelTokenSize"));
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x00076D70 File Offset: 0x00075D70
		internal unsafe ChannelBinding GetChannelBindingFromTls(ulong connectionId)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, "HttpListener#" + ValidationHelper.HashString(this) + "::GetChannelBindingFromTls() connectionId: " + connectionId.ToString());
			}
			int num = HttpListener.RequestChannelBindStatusSize + 128;
			SafeLocalFreeChannelBinding safeLocalFreeChannelBinding = null;
			uint num2 = 0U;
			uint num3;
			for (;;)
			{
				byte[] array = new byte[num];
				fixed (byte* ptr = array)
				{
					num3 = UnsafeNclNativeMethods.HttpApi.HttpReceiveClientCertificate(this.RequestQueueHandle, connectionId, 1U, ptr, (uint)num, &num2, null);
					if (num3 == 0U)
					{
						int tokenOffsetFromBlob = HttpListener.GetTokenOffsetFromBlob((IntPtr)((void*)ptr));
						int tokenSizeFromBlob = HttpListener.GetTokenSizeFromBlob((IntPtr)((void*)ptr));
						safeLocalFreeChannelBinding = SafeLocalFreeChannelBinding.LocalAlloc(tokenSizeFromBlob);
						if (safeLocalFreeChannelBinding.IsInvalid)
						{
							break;
						}
						Marshal.Copy(array, tokenOffsetFromBlob, safeLocalFreeChannelBinding.DangerousGetHandle(), tokenSizeFromBlob);
					}
					else
					{
						if (num3 != 234U)
						{
							goto IL_00E7;
						}
						int tokenSizeFromBlob2 = HttpListener.GetTokenSizeFromBlob((IntPtr)((void*)ptr));
						num = HttpListener.RequestChannelBindStatusSize + tokenSizeFromBlob2;
					}
				}
				if (num3 == 0U)
				{
					return safeLocalFreeChannelBinding;
				}
			}
			throw new OutOfMemoryException();
			IL_00E7:
			if (num3 == 87U)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.HttpListener, "HttpListener#" + ValidationHelper.HashString(this) + "::GetChannelBindingFromTls() Can't retrieve CBT from TLS: ERROR_INVALID_PARAMETER");
				}
				return null;
			}
			throw new HttpListenerException((int)num3);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x00076EAC File Offset: 0x00075EAC
		internal void CheckDisposed()
		{
			if (this.m_State == HttpListener.State.Closed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x00076EC8 File Offset: 0x00075EC8
		private HttpStatusCode HttpStatusFromSecurityStatus(SecurityStatus status)
		{
			if (NclUtilities.IsCredentialFailure(status))
			{
				return HttpStatusCode.Unauthorized;
			}
			if (NclUtilities.IsClientFault(status))
			{
				return HttpStatusCode.BadRequest;
			}
			return HttpStatusCode.InternalServerError;
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x00076EEC File Offset: 0x00075EEC
		private void SaveDigestContext(NTAuthentication digestContext)
		{
			if (this.m_SavedDigests == null)
			{
				Interlocked.CompareExchange<HttpListener.DigestContext[]>(ref this.m_SavedDigests, new HttpListener.DigestContext[1024], null);
			}
			NTAuthentication ntauthentication = null;
			ArrayList arrayList = null;
			lock (this.m_SavedDigests)
			{
				if (!this.IsListening)
				{
					digestContext.CloseContext();
					return;
				}
				int num = (((num = Environment.TickCount) == 0) ? 1 : num);
				this.m_NewestContext = (this.m_NewestContext + 1) & 1023;
				int timestamp = this.m_SavedDigests[this.m_NewestContext].timestamp;
				ntauthentication = this.m_SavedDigests[this.m_NewestContext].context;
				this.m_SavedDigests[this.m_NewestContext].timestamp = num;
				this.m_SavedDigests[this.m_NewestContext].context = digestContext;
				if (this.m_OldestContext == this.m_NewestContext)
				{
					this.m_OldestContext = (this.m_NewestContext + 1) & 1023;
				}
				while (num - this.m_SavedDigests[this.m_OldestContext].timestamp >= 300 && this.m_SavedDigests[this.m_OldestContext].context != null)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(this.m_SavedDigests[this.m_OldestContext].context);
					this.m_SavedDigests[this.m_OldestContext].context = null;
					this.m_OldestContext = (this.m_OldestContext + 1) & 1023;
				}
				if (ntauthentication != null && num - timestamp <= 10000)
				{
					if (this.m_ExtraSavedDigests == null || num - this.m_ExtraSavedDigestsTimestamp > 10000)
					{
						arrayList = this.m_ExtraSavedDigestsBaking;
						this.m_ExtraSavedDigestsBaking = this.m_ExtraSavedDigests;
						this.m_ExtraSavedDigestsTimestamp = num;
						this.m_ExtraSavedDigests = new ArrayList();
					}
					this.m_ExtraSavedDigests.Add(ntauthentication);
					ntauthentication = null;
				}
			}
			if (ntauthentication != null)
			{
				ntauthentication.CloseContext();
			}
			if (arrayList != null)
			{
				for (int i = 0; i < arrayList.Count; i++)
				{
					((NTAuthentication)arrayList[i]).CloseContext();
				}
			}
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x0007711C File Offset: 0x0007611C
		private void ClearDigestCache()
		{
			if (this.m_SavedDigests == null)
			{
				return;
			}
			ArrayList[] array = new ArrayList[3];
			lock (this.m_SavedDigests)
			{
				array[0] = this.m_ExtraSavedDigestsBaking;
				this.m_ExtraSavedDigestsBaking = null;
				array[1] = this.m_ExtraSavedDigests;
				this.m_ExtraSavedDigests = null;
				this.m_NewestContext = 0;
				this.m_OldestContext = 0;
				array[2] = new ArrayList();
				for (int i = 0; i < 1024; i++)
				{
					if (this.m_SavedDigests[i].context != null)
					{
						array[2].Add(this.m_SavedDigests[i].context);
						this.m_SavedDigests[i].context = null;
					}
					this.m_SavedDigests[i].timestamp = 0;
				}
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != null)
				{
					for (int k = 0; k < array[j].Count; k++)
					{
						((NTAuthentication)array[j][k]).CloseContext();
					}
				}
			}
		}

		// Token: 0x04001E4C RID: 7756
		private const int DigestLifetimeSeconds = 300;

		// Token: 0x04001E4D RID: 7757
		private const int MaximumDigests = 1024;

		// Token: 0x04001E4E RID: 7758
		private const int MinimumDigestLifetimeSeconds = 10;

		// Token: 0x04001E4F RID: 7759
		private static readonly Type ChannelBindingStatusType = typeof(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_CHANNEL_BIND_STATUS);

		// Token: 0x04001E50 RID: 7760
		private static readonly int RequestChannelBindStatusSize = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_CHANNEL_BIND_STATUS));

		// Token: 0x04001E51 RID: 7761
		private static byte[] s_WwwAuthenticateBytes = new byte[]
		{
			87, 87, 87, 45, 65, 117, 116, 104, 101, 110,
			116, 105, 99, 97, 116, 101
		};

		// Token: 0x04001E52 RID: 7762
		private HttpListener.AuthenticationSelectorInfo m_AuthenticationDelegate;

		// Token: 0x04001E53 RID: 7763
		private AuthenticationSchemes m_AuthenticationScheme = AuthenticationSchemes.Anonymous;

		// Token: 0x04001E54 RID: 7764
		private SecurityException m_SecurityException;

		// Token: 0x04001E55 RID: 7765
		private string m_Realm;

		// Token: 0x04001E56 RID: 7766
		private SafeCloseHandle m_RequestQueueHandle;

		// Token: 0x04001E57 RID: 7767
		private bool m_RequestHandleBound;

		// Token: 0x04001E58 RID: 7768
		private HttpListener.State m_State;

		// Token: 0x04001E59 RID: 7769
		private HttpListenerPrefixCollection m_Prefixes;

		// Token: 0x04001E5A RID: 7770
		private bool m_IgnoreWriteExceptions;

		// Token: 0x04001E5B RID: 7771
		private bool m_UnsafeConnectionNtlmAuthentication;

		// Token: 0x04001E5C RID: 7772
		private HttpListener.ExtendedProtectionSelector m_ExtendedProtectionSelectorDelegate;

		// Token: 0x04001E5D RID: 7773
		private ExtendedProtectionPolicy m_ExtendedProtectionPolicy;

		// Token: 0x04001E5E RID: 7774
		private ServiceNameStore m_DefaultServiceNames;

		// Token: 0x04001E5F RID: 7775
		private Hashtable m_DisconnectResults;

		// Token: 0x04001E60 RID: 7776
		private object m_InternalLock;

		// Token: 0x04001E61 RID: 7777
		internal Hashtable m_UriPrefixes = new Hashtable();

		// Token: 0x04001E62 RID: 7778
		private HttpListener.DigestContext[] m_SavedDigests;

		// Token: 0x04001E63 RID: 7779
		private ArrayList m_ExtraSavedDigests;

		// Token: 0x04001E64 RID: 7780
		private ArrayList m_ExtraSavedDigestsBaking;

		// Token: 0x04001E65 RID: 7781
		private int m_ExtraSavedDigestsTimestamp;

		// Token: 0x04001E66 RID: 7782
		private int m_NewestContext;

		// Token: 0x04001E67 RID: 7783
		private int m_OldestContext;

		// Token: 0x020003CB RID: 971
		private class AuthenticationSelectorInfo
		{
			// Token: 0x06001EB4 RID: 7860 RVA: 0x00077284 File Offset: 0x00076284
			internal AuthenticationSelectorInfo(AuthenticationSchemeSelector selectorDelegate, bool canUseAdvancedAuth)
			{
				this.m_SelectorDelegate = selectorDelegate;
				this.m_CanUseAdvancedAuth = canUseAdvancedAuth;
			}

			// Token: 0x17000620 RID: 1568
			// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x0007729A File Offset: 0x0007629A
			internal AuthenticationSchemeSelector Delegate
			{
				get
				{
					return this.m_SelectorDelegate;
				}
			}

			// Token: 0x17000621 RID: 1569
			// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x000772A2 File Offset: 0x000762A2
			internal bool AdvancedAuth
			{
				get
				{
					return this.m_CanUseAdvancedAuth;
				}
			}

			// Token: 0x04001E68 RID: 7784
			private AuthenticationSchemeSelector m_SelectorDelegate;

			// Token: 0x04001E69 RID: 7785
			private bool m_CanUseAdvancedAuth;
		}

		// Token: 0x020003CC RID: 972
		// (Invoke) Token: 0x06001EB8 RID: 7864
		public delegate ExtendedProtectionPolicy ExtendedProtectionSelector(HttpListenerRequest request);

		// Token: 0x020003CD RID: 973
		private enum State
		{
			// Token: 0x04001E6B RID: 7787
			Stopped,
			// Token: 0x04001E6C RID: 7788
			Started,
			// Token: 0x04001E6D RID: 7789
			Closed
		}

		// Token: 0x020003CE RID: 974
		private struct DigestContext
		{
			// Token: 0x04001E6E RID: 7790
			internal NTAuthentication context;

			// Token: 0x04001E6F RID: 7791
			internal int timestamp;
		}

		// Token: 0x020003CF RID: 975
		private class DisconnectAsyncResult : IAsyncResult
		{
			// Token: 0x17000622 RID: 1570
			// (get) Token: 0x06001EBB RID: 7867 RVA: 0x000772AA File Offset: 0x000762AA
			internal unsafe NativeOverlapped* NativeOverlapped
			{
				get
				{
					return this.m_NativeOverlapped;
				}
			}

			// Token: 0x17000623 RID: 1571
			// (get) Token: 0x06001EBC RID: 7868 RVA: 0x000772B2 File Offset: 0x000762B2
			public object AsyncState
			{
				get
				{
					throw ExceptionHelper.PropertyNotImplementedException;
				}
			}

			// Token: 0x17000624 RID: 1572
			// (get) Token: 0x06001EBD RID: 7869 RVA: 0x000772B9 File Offset: 0x000762B9
			public WaitHandle AsyncWaitHandle
			{
				get
				{
					throw ExceptionHelper.PropertyNotImplementedException;
				}
			}

			// Token: 0x17000625 RID: 1573
			// (get) Token: 0x06001EBE RID: 7870 RVA: 0x000772C0 File Offset: 0x000762C0
			public bool CompletedSynchronously
			{
				get
				{
					throw ExceptionHelper.PropertyNotImplementedException;
				}
			}

			// Token: 0x17000626 RID: 1574
			// (get) Token: 0x06001EBF RID: 7871 RVA: 0x000772C7 File Offset: 0x000762C7
			public bool IsCompleted
			{
				get
				{
					throw ExceptionHelper.PropertyNotImplementedException;
				}
			}

			// Token: 0x06001EC0 RID: 7872 RVA: 0x000772D0 File Offset: 0x000762D0
			internal DisconnectAsyncResult(HttpListener httpListener, ulong connectionId)
			{
				this.m_OwnershipState = 1;
				this.m_HttpListener = httpListener;
				this.m_ConnectionId = connectionId;
				this.m_NativeOverlapped = new Overlapped
				{
					AsyncResult = this
				}.UnsafePack(HttpListener.DisconnectAsyncResult.s_IOCallback, null);
			}

			// Token: 0x06001EC1 RID: 7873 RVA: 0x00077318 File Offset: 0x00076318
			internal bool StartOwningDisconnectHandling()
			{
				int num;
				while ((num = Interlocked.CompareExchange(ref this.m_OwnershipState, 1, 0)) == 2)
				{
					Thread.SpinWait(1);
				}
				return num < 2;
			}

			// Token: 0x06001EC2 RID: 7874 RVA: 0x00077343 File Offset: 0x00076343
			internal void FinishOwningDisconnectHandling()
			{
				if (Interlocked.CompareExchange(ref this.m_OwnershipState, 0, 1) == 2)
				{
					this.HandleDisconnect();
				}
			}

			// Token: 0x06001EC3 RID: 7875 RVA: 0x0007735C File Offset: 0x0007635C
			private unsafe static void WaitCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
			{
				Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
				HttpListener.DisconnectAsyncResult disconnectAsyncResult = (HttpListener.DisconnectAsyncResult)overlapped.AsyncResult;
				Overlapped.Free(nativeOverlapped);
				if (Interlocked.Exchange(ref disconnectAsyncResult.m_OwnershipState, 2) == 0)
				{
					disconnectAsyncResult.HandleDisconnect();
				}
			}

			// Token: 0x06001EC4 RID: 7876 RVA: 0x00077398 File Offset: 0x00076398
			private void HandleDisconnect()
			{
				this.m_HttpListener.DisconnectResults.Remove(this.m_ConnectionId);
				if (this.m_Session != null)
				{
					if (this.m_Session.Package == "WDigest")
					{
						this.m_HttpListener.SaveDigestContext(this.m_Session);
					}
					else
					{
						this.m_Session.CloseContext();
					}
				}
				IDisposable disposable = ((this.m_AuthenticatedConnection == null) ? null : (this.m_AuthenticatedConnection.Identity as IDisposable));
				if (disposable != null && this.m_AuthenticatedConnection.Identity.AuthenticationType == "NTLM" && this.m_HttpListener.UnsafeConnectionNtlmAuthentication)
				{
					disposable.Dispose();
				}
				Interlocked.Exchange(ref this.m_OwnershipState, 3);
			}

			// Token: 0x17000627 RID: 1575
			// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x00077458 File Offset: 0x00076458
			// (set) Token: 0x06001EC6 RID: 7878 RVA: 0x00077460 File Offset: 0x00076460
			internal WindowsPrincipal AuthenticatedConnection
			{
				get
				{
					return this.m_AuthenticatedConnection;
				}
				set
				{
					this.m_AuthenticatedConnection = value;
				}
			}

			// Token: 0x17000628 RID: 1576
			// (get) Token: 0x06001EC7 RID: 7879 RVA: 0x00077469 File Offset: 0x00076469
			// (set) Token: 0x06001EC8 RID: 7880 RVA: 0x00077471 File Offset: 0x00076471
			internal NTAuthentication Session
			{
				get
				{
					return this.m_Session;
				}
				set
				{
					this.m_Session = value;
				}
			}

			// Token: 0x04001E70 RID: 7792
			internal const string NTLM = "NTLM";

			// Token: 0x04001E71 RID: 7793
			private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(HttpListener.DisconnectAsyncResult.WaitCallback);

			// Token: 0x04001E72 RID: 7794
			private ulong m_ConnectionId;

			// Token: 0x04001E73 RID: 7795
			private HttpListener m_HttpListener;

			// Token: 0x04001E74 RID: 7796
			private unsafe NativeOverlapped* m_NativeOverlapped;

			// Token: 0x04001E75 RID: 7797
			private int m_OwnershipState;

			// Token: 0x04001E76 RID: 7798
			private WindowsPrincipal m_AuthenticatedConnection;

			// Token: 0x04001E77 RID: 7799
			private NTAuthentication m_Session;
		}
	}
}
