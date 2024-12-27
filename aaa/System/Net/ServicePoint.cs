using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	// Token: 0x02000433 RID: 1075
	public class ServicePoint
	{
		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06002172 RID: 8562 RVA: 0x000842BD File Offset: 0x000832BD
		internal string LookupString
		{
			get
			{
				return this.m_LookupString;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06002173 RID: 8563 RVA: 0x000842C5 File Offset: 0x000832C5
		internal string Hostname
		{
			get
			{
				return this.m_HostName;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06002174 RID: 8564 RVA: 0x000842CD File Offset: 0x000832CD
		// (set) Token: 0x06002175 RID: 8565 RVA: 0x000842D5 File Offset: 0x000832D5
		public BindIPEndPoint BindIPEndPointDelegate
		{
			get
			{
				return this.m_BindIPEndPointDelegate;
			}
			set
			{
				ExceptionHelper.InfrastructurePermission.Demand();
				this.m_BindIPEndPointDelegate = value;
			}
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x000842E8 File Offset: 0x000832E8
		internal ServicePoint(Uri address, TimerThread.Queue defaultIdlingQueue, int defaultConnectionLimit, string lookupString, bool userChangedLimit, bool proxyServicePoint)
		{
			this.m_ProxyServicePoint = proxyServicePoint;
			this.m_Address = address;
			this.m_ConnectionName = address.Scheme;
			this.m_Host = address.DnsSafeHost;
			this.m_Port = address.Port;
			this.m_IdlingQueue = defaultIdlingQueue;
			this.m_ConnectionLimit = defaultConnectionLimit;
			this.m_HostLoopbackGuess = TriState.Unspecified;
			this.m_LookupString = lookupString;
			this.m_UserChangedLimit = userChangedLimit;
			this.m_UseNagleAlgorithm = ServicePointManager.UseNagleAlgorithm;
			this.m_Expect100Continue = ServicePointManager.Expect100Continue;
			this.m_ConnectionGroupList = new Hashtable(10);
			this.m_ConnectionLeaseTimeout = -1;
			this.m_ReceiveBufferSize = -1;
			this.m_UseTcpKeepAlive = ServicePointManager.s_UseTcpKeepAlive;
			this.m_TcpKeepAliveTime = ServicePointManager.s_TcpKeepAliveTime;
			this.m_TcpKeepAliveInterval = ServicePointManager.s_TcpKeepAliveInterval;
			this.m_Understands100Continue = true;
			this.m_HttpBehaviour = HttpBehaviour.Unknown;
			this.m_IdleSince = DateTime.Now;
			this.m_ExpiringTimer = this.m_IdlingQueue.CreateTimer(ServicePointManager.IdleServicePointTimeoutDelegate, this);
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x000843E0 File Offset: 0x000833E0
		internal ServicePoint(string host, int port, TimerThread.Queue defaultIdlingQueue, int defaultConnectionLimit, string lookupString, bool userChangedLimit, bool proxyServicePoint)
		{
			this.m_ProxyServicePoint = proxyServicePoint;
			this.m_ConnectionName = "ByHost:" + host + ":" + port.ToString(CultureInfo.InvariantCulture);
			this.m_IdlingQueue = defaultIdlingQueue;
			this.m_ConnectionLimit = defaultConnectionLimit;
			this.m_HostLoopbackGuess = TriState.Unspecified;
			this.m_LookupString = lookupString;
			this.m_UserChangedLimit = userChangedLimit;
			this.m_ConnectionGroupList = new Hashtable(10);
			this.m_ConnectionLeaseTimeout = -1;
			this.m_ReceiveBufferSize = -1;
			this.m_Host = host;
			this.m_Port = port;
			this.m_HostMode = true;
			this.m_IdleSince = DateTime.Now;
			this.m_ExpiringTimer = this.m_IdlingQueue.CreateTimer(ServicePointManager.IdleServicePointTimeoutDelegate, this);
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x000844A0 File Offset: 0x000834A0
		internal object CachedChannelBinding
		{
			get
			{
				return this.m_CachedChannelBinding;
			}
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x000844A8 File Offset: 0x000834A8
		internal void SetCachedChannelBinding(Uri uri, ChannelBinding binding)
		{
			if (uri.Scheme == Uri.UriSchemeHttps)
			{
				this.m_CachedChannelBinding = ((binding != null) ? binding : DBNull.Value);
			}
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x000844D0 File Offset: 0x000834D0
		private ConnectionGroup FindConnectionGroup(string connName, bool dontCreate)
		{
			string text = ConnectionGroup.MakeQueryStr(connName);
			ConnectionGroup connectionGroup = this.m_ConnectionGroupList[text] as ConnectionGroup;
			if (connectionGroup == null && !dontCreate)
			{
				connectionGroup = new ConnectionGroup(this, connName);
				this.m_ConnectionGroupList[text] = connectionGroup;
			}
			return connectionGroup;
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x00084514 File Offset: 0x00083514
		internal Socket GetConnection(PooledStream PooledStream, object owner, bool async, out IPAddress address, ref Socket abortSocket, ref Socket abortSocket6, int timeout)
		{
			Socket socket = null;
			Socket socket2 = null;
			Socket socket3 = null;
			Exception ex = null;
			address = null;
			if (Socket.SupportsIPv4)
			{
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			}
			if (Socket.OSSupportsIPv6)
			{
				socket2 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
			}
			abortSocket = socket;
			abortSocket6 = socket2;
			ServicePoint.ConnectSocketState connectSocketState = null;
			if (async)
			{
				connectSocketState = new ServicePoint.ConnectSocketState(this, PooledStream, owner, socket, socket2);
			}
			WebExceptionStatus webExceptionStatus = this.ConnectSocket(socket, socket2, ref socket3, ref address, connectSocketState, timeout, out ex);
			if (webExceptionStatus == WebExceptionStatus.Pending)
			{
				return null;
			}
			if (webExceptionStatus != WebExceptionStatus.Success)
			{
				throw new WebException(NetRes.GetWebStatusString(webExceptionStatus), (webExceptionStatus == WebExceptionStatus.ProxyNameResolutionFailure || webExceptionStatus == WebExceptionStatus.NameResolutionFailure) ? this.Host : null, ex, webExceptionStatus, null, WebExceptionInternalStatus.ServicePointFatal);
			}
			if (socket3 == null)
			{
				throw new IOException(SR.GetString("net_io_transportfailure"));
			}
			this.CompleteGetConnection(socket, socket2, socket3, address);
			return socket3;
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000845D4 File Offset: 0x000835D4
		private void CompleteGetConnection(Socket socket, Socket socket6, Socket finalSocket, IPAddress address)
		{
			if (finalSocket.AddressFamily == AddressFamily.InterNetwork)
			{
				if (socket6 != null)
				{
					socket6.Close();
					socket6 = null;
				}
			}
			else if (socket != null)
			{
				socket.Close();
				socket = null;
			}
			if (!this.UseNagleAlgorithm)
			{
				finalSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, 1);
			}
			if (this.ReceiveBufferSize != -1)
			{
				finalSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, this.ReceiveBufferSize);
			}
			if (this.m_UseTcpKeepAlive)
			{
				byte[] array = new byte[]
				{
					1,
					0,
					0,
					0,
					(byte)(this.m_TcpKeepAliveTime & 255),
					(byte)((this.m_TcpKeepAliveTime >> 8) & 255),
					(byte)((this.m_TcpKeepAliveTime >> 16) & 255),
					(byte)((this.m_TcpKeepAliveTime >> 24) & 255),
					(byte)(this.m_TcpKeepAliveInterval & 255),
					(byte)((this.m_TcpKeepAliveInterval >> 8) & 255),
					(byte)((this.m_TcpKeepAliveInterval >> 16) & 255),
					(byte)((this.m_TcpKeepAliveInterval >> 24) & 255)
				};
				finalSocket.IOControl((IOControlCode)((ulong)(-1744830460)), array, null);
			}
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x000846ED File Offset: 0x000836ED
		internal virtual void SubmitRequest(HttpWebRequest request)
		{
			this.SubmitRequest(request, null);
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x000846F8 File Offset: 0x000836F8
		internal void SubmitRequest(HttpWebRequest request, string connName)
		{
			ConnectionGroup connectionGroup;
			lock (this)
			{
				connectionGroup = this.FindConnectionGroup(connName, false);
			}
			for (;;)
			{
				Connection connection = connectionGroup.FindConnection(request, connName);
				if (connection == null)
				{
					break;
				}
				if (connection.SubmitRequest(request))
				{
					return;
				}
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x0600217F RID: 8575 RVA: 0x00084748 File Offset: 0x00083748
		// (set) Token: 0x06002180 RID: 8576 RVA: 0x00084750 File Offset: 0x00083750
		public int ConnectionLeaseTimeout
		{
			get
			{
				return this.m_ConnectionLeaseTimeout;
			}
			set
			{
				if (!ValidationHelper.ValidateRange(value, -1, 2147483647))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value != this.m_ConnectionLeaseTimeout)
				{
					this.m_ConnectionLeaseTimeout = value;
					this.m_ConnectionLeaseTimerQueue = null;
				}
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06002181 RID: 8577 RVA: 0x00084784 File Offset: 0x00083784
		internal TimerThread.Queue ConnectionLeaseTimerQueue
		{
			get
			{
				if (this.m_ConnectionLeaseTimerQueue == null)
				{
					TimerThread.Queue orCreateQueue = TimerThread.GetOrCreateQueue(this.ConnectionLeaseTimeout);
					this.m_ConnectionLeaseTimerQueue = orCreateQueue;
				}
				return this.m_ConnectionLeaseTimerQueue;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06002182 RID: 8578 RVA: 0x000847B4 File Offset: 0x000837B4
		public Uri Address
		{
			get
			{
				if (this.m_HostMode)
				{
					throw new NotSupportedException(SR.GetString("net_servicePointAddressNotSupportedInHostMode"));
				}
				if (this.m_ProxyServicePoint)
				{
					ExceptionHelper.WebPermissionUnrestricted.Demand();
				}
				return this.m_Address;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06002183 RID: 8579 RVA: 0x000847E6 File Offset: 0x000837E6
		internal Uri InternalAddress
		{
			get
			{
				return this.m_Address;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x000847EE File Offset: 0x000837EE
		internal string Host
		{
			get
			{
				if (this.m_HostMode)
				{
					return this.m_Host;
				}
				return this.m_Address.Host;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002185 RID: 8581 RVA: 0x0008480A File Offset: 0x0008380A
		internal int Port
		{
			get
			{
				return this.m_Port;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x00084812 File Offset: 0x00083812
		// (set) Token: 0x06002187 RID: 8583 RVA: 0x00084820 File Offset: 0x00083820
		public int MaxIdleTime
		{
			get
			{
				return this.m_IdlingQueue.Duration;
			}
			set
			{
				if (!ValidationHelper.ValidateRange(value, -1, 2147483647))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value == this.m_IdlingQueue.Duration)
				{
					return;
				}
				lock (this)
				{
					if (this.m_ExpiringTimer == null || this.m_ExpiringTimer.Cancel())
					{
						this.m_IdlingQueue = TimerThread.GetOrCreateQueue(value);
						if (this.m_ExpiringTimer != null)
						{
							double totalMilliseconds = (DateTime.Now - this.m_IdleSince).TotalMilliseconds;
							int num = ((totalMilliseconds >= 2147483647.0) ? int.MaxValue : ((int)totalMilliseconds));
							int num2 = ((value == -1) ? (-1) : ((num >= value) ? 0 : (value - num)));
							this.m_ExpiringTimer = TimerThread.CreateQueue(num2).CreateTimer(ServicePointManager.IdleServicePointTimeoutDelegate, this);
						}
					}
				}
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x000848F8 File Offset: 0x000838F8
		// (set) Token: 0x06002189 RID: 8585 RVA: 0x00084900 File Offset: 0x00083900
		public bool UseNagleAlgorithm
		{
			get
			{
				return this.m_UseNagleAlgorithm;
			}
			set
			{
				this.m_UseNagleAlgorithm = value;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x00084909 File Offset: 0x00083909
		// (set) Token: 0x0600218B RID: 8587 RVA: 0x00084911 File Offset: 0x00083911
		public int ReceiveBufferSize
		{
			get
			{
				return this.m_ReceiveBufferSize;
			}
			set
			{
				if (!ValidationHelper.ValidateRange(value, -1, 2147483647))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_ReceiveBufferSize = value;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x0600218D RID: 8589 RVA: 0x0008493C File Offset: 0x0008393C
		// (set) Token: 0x0600218C RID: 8588 RVA: 0x00084933 File Offset: 0x00083933
		public bool Expect100Continue
		{
			get
			{
				return this.m_Expect100Continue;
			}
			set
			{
				this.m_Expect100Continue = value;
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x0600218E RID: 8590 RVA: 0x00084944 File Offset: 0x00083944
		public DateTime IdleSince
		{
			get
			{
				return this.m_IdleSince;
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x0600218F RID: 8591 RVA: 0x0008494C File Offset: 0x0008394C
		public virtual Version ProtocolVersion
		{
			get
			{
				if (this.m_HttpBehaviour <= HttpBehaviour.HTTP10 && this.m_HttpBehaviour != HttpBehaviour.Unknown)
				{
					return HttpVersion.Version10;
				}
				return HttpVersion.Version11;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x0008496A File Offset: 0x0008396A
		// (set) Token: 0x06002191 RID: 8593 RVA: 0x00084972 File Offset: 0x00083972
		internal HttpBehaviour HttpBehaviour
		{
			get
			{
				return this.m_HttpBehaviour;
			}
			set
			{
				this.m_HttpBehaviour = value;
				this.m_Understands100Continue = this.m_Understands100Continue && (this.m_HttpBehaviour > HttpBehaviour.HTTP10 || this.m_HttpBehaviour == HttpBehaviour.Unknown);
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x000849A1 File Offset: 0x000839A1
		public string ConnectionName
		{
			get
			{
				return this.m_ConnectionName;
			}
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x000849A9 File Offset: 0x000839A9
		public bool CloseConnectionGroup(string connectionGroupName)
		{
			return this.ReleaseConnectionGroup(HttpWebRequest.GenerateConnectionGroup(connectionGroupName, false, false).ToString()) || this.ReleaseConnectionGroup(HttpWebRequest.GenerateConnectionGroup(connectionGroupName, true, false).ToString()) || ConnectionPoolManager.RemoveConnectionPool(this, connectionGroupName);
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x000849E4 File Offset: 0x000839E4
		// (set) Token: 0x06002195 RID: 8597 RVA: 0x00084AC4 File Offset: 0x00083AC4
		public int ConnectionLimit
		{
			get
			{
				if (!this.m_UserChangedLimit && this.m_IPAddressInfoList == null && this.m_HostLoopbackGuess == TriState.Unspecified)
				{
					lock (this)
					{
						if (!this.m_UserChangedLimit && this.m_IPAddressInfoList == null && this.m_HostLoopbackGuess == TriState.Unspecified)
						{
							IPAddress ipaddress = null;
							if (IPAddress.TryParse(this.m_Host, out ipaddress))
							{
								this.m_HostLoopbackGuess = (ServicePoint.IsAddressListLoopback(new IPAddress[] { ipaddress }) ? TriState.True : TriState.False);
							}
							else
							{
								this.m_HostLoopbackGuess = (NclUtilities.GuessWhetherHostIsLoopback(this.m_Host) ? TriState.True : TriState.False);
							}
						}
					}
				}
				if (!this.m_UserChangedLimit && !((this.m_IPAddressInfoList == null) ? (this.m_HostLoopbackGuess != TriState.True) : (!this.m_IPAddressesAreLoopback)))
				{
					return int.MaxValue;
				}
				return this.m_ConnectionLimit;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (!this.m_UserChangedLimit || this.m_ConnectionLimit != value)
				{
					lock (this)
					{
						if (!this.m_UserChangedLimit || this.m_ConnectionLimit != value)
						{
							this.m_ConnectionLimit = value;
							this.m_UserChangedLimit = true;
							this.ResolveConnectionLimit();
						}
					}
				}
			}
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x00084B38 File Offset: 0x00083B38
		private void ResolveConnectionLimit()
		{
			int connectionLimit = this.ConnectionLimit;
			foreach (object obj in this.m_ConnectionGroupList.Values)
			{
				ConnectionGroup connectionGroup = (ConnectionGroup)obj;
				connectionGroup.ConnectionLimit = connectionLimit;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06002197 RID: 8599 RVA: 0x00084BA0 File Offset: 0x00083BA0
		public int CurrentConnections
		{
			get
			{
				int num = 0;
				lock (this)
				{
					foreach (object obj in this.m_ConnectionGroupList.Values)
					{
						ConnectionGroup connectionGroup = (ConnectionGroup)obj;
						num += connectionGroup.CurrentConnections;
					}
				}
				return num;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06002198 RID: 8600 RVA: 0x00084C24 File Offset: 0x00083C24
		public X509Certificate Certificate
		{
			get
			{
				object serverCertificateOrBytes = this.m_ServerCertificateOrBytes;
				if (serverCertificateOrBytes != null && serverCertificateOrBytes.GetType() == typeof(byte[]))
				{
					return (X509Certificate)(this.m_ServerCertificateOrBytes = new X509Certificate((byte[])serverCertificateOrBytes));
				}
				return serverCertificateOrBytes as X509Certificate;
			}
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x00084C6D File Offset: 0x00083C6D
		internal void UpdateServerCertificate(X509Certificate certificate)
		{
			if (certificate != null)
			{
				this.m_ServerCertificateOrBytes = certificate.GetRawCertData();
				return;
			}
			this.m_ServerCertificateOrBytes = null;
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x0600219A RID: 8602 RVA: 0x00084C88 File Offset: 0x00083C88
		public X509Certificate ClientCertificate
		{
			get
			{
				object clientCertificateOrBytes = this.m_ClientCertificateOrBytes;
				if (clientCertificateOrBytes != null && clientCertificateOrBytes.GetType() == typeof(byte[]))
				{
					return (X509Certificate)(this.m_ClientCertificateOrBytes = new X509Certificate((byte[])clientCertificateOrBytes));
				}
				return clientCertificateOrBytes as X509Certificate;
			}
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x00084CD1 File Offset: 0x00083CD1
		internal void UpdateClientCertificate(X509Certificate certificate)
		{
			if (certificate != null)
			{
				this.m_ClientCertificateOrBytes = certificate.GetRawCertData();
				return;
			}
			this.m_ClientCertificateOrBytes = null;
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x0600219C RID: 8604 RVA: 0x00084CEA File Offset: 0x00083CEA
		public bool SupportsPipelining
		{
			get
			{
				return this.m_HttpBehaviour > HttpBehaviour.HTTP10 || this.m_HttpBehaviour == HttpBehaviour.Unknown;
			}
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x00084D00 File Offset: 0x00083D00
		public void SetTcpKeepAlive(bool enabled, int keepAliveTime, int keepAliveInterval)
		{
			if (!enabled)
			{
				this.m_UseTcpKeepAlive = false;
				this.m_TcpKeepAliveTime = 0;
				this.m_TcpKeepAliveInterval = 0;
				return;
			}
			this.m_UseTcpKeepAlive = true;
			if (keepAliveTime <= 0)
			{
				throw new ArgumentOutOfRangeException("keepAliveTime");
			}
			if (keepAliveInterval <= 0)
			{
				throw new ArgumentOutOfRangeException("keepAliveInterval");
			}
			this.m_TcpKeepAliveTime = keepAliveTime;
			this.m_TcpKeepAliveInterval = keepAliveInterval;
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x0600219F RID: 8607 RVA: 0x00084D62 File Offset: 0x00083D62
		// (set) Token: 0x0600219E RID: 8606 RVA: 0x00084D59 File Offset: 0x00083D59
		internal bool Understands100Continue
		{
			get
			{
				return this.m_Understands100Continue;
			}
			set
			{
				this.m_Understands100Continue = value;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x060021A0 RID: 8608 RVA: 0x00084D6A File Offset: 0x00083D6A
		internal bool InternalProxyServicePoint
		{
			get
			{
				return this.m_ProxyServicePoint;
			}
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x00084D74 File Offset: 0x00083D74
		internal void IncrementConnection()
		{
			lock (this)
			{
				this.m_CurrentConnections++;
				if (this.m_CurrentConnections == 1)
				{
					this.m_ExpiringTimer.Cancel();
					this.m_ExpiringTimer = null;
				}
			}
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x00084DCC File Offset: 0x00083DCC
		internal void DecrementConnection()
		{
			lock (this)
			{
				this.m_CurrentConnections--;
				if (this.m_CurrentConnections == 0)
				{
					this.m_IdleSince = DateTime.Now;
					this.m_ExpiringTimer = this.m_IdlingQueue.CreateTimer(ServicePointManager.IdleServicePointTimeoutDelegate, this);
				}
				else if (this.m_CurrentConnections < 0)
				{
					this.m_CurrentConnections = 0;
				}
			}
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x00084E44 File Offset: 0x00083E44
		internal RemoteCertValidationCallback SetupHandshakeDoneProcedure(TlsStream secureStream, object request)
		{
			return ServicePoint.HandshakeDoneProcedure.CreateAdapter(this, secureStream, request);
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x00084E50 File Offset: 0x00083E50
		internal bool ReleaseConnectionGroup(string connName)
		{
			lock (this)
			{
				ConnectionGroup connectionGroup = this.FindConnectionGroup(connName, true);
				if (connectionGroup == null)
				{
					return false;
				}
				connectionGroup.DisableKeepAliveOnConnections();
				this.m_ConnectionGroupList.Remove(connName);
			}
			return true;
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x00084EA4 File Offset: 0x00083EA4
		internal void ReleaseAllConnectionGroups()
		{
			ArrayList arrayList = new ArrayList(this.m_ConnectionGroupList.Count);
			lock (this)
			{
				foreach (object obj in this.m_ConnectionGroupList.Values)
				{
					ConnectionGroup connectionGroup = (ConnectionGroup)obj;
					arrayList.Add(connectionGroup);
				}
				this.m_ConnectionGroupList.Clear();
			}
			foreach (object obj2 in arrayList)
			{
				ConnectionGroup connectionGroup2 = (ConnectionGroup)obj2;
				connectionGroup2.DisableKeepAliveOnConnections();
			}
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x00084F8C File Offset: 0x00083F8C
		private static void ConnectSocketCallback(IAsyncResult asyncResult)
		{
			ServicePoint.ConnectSocketState connectSocketState = (ServicePoint.ConnectSocketState)asyncResult.AsyncState;
			Socket socket = null;
			IPAddress ipaddress = null;
			Exception ex = null;
			Exception ex2 = null;
			WebExceptionStatus webExceptionStatus = WebExceptionStatus.ConnectFailure;
			try
			{
				webExceptionStatus = connectSocketState.servicePoint.ConnectSocketInternal(connectSocketState.connectFailure, connectSocketState.s4, connectSocketState.s6, ref socket, ref ipaddress, connectSocketState, asyncResult, -1, out ex);
			}
			catch (SocketException ex3)
			{
				ex2 = ex3;
			}
			catch (ObjectDisposedException ex4)
			{
				ex2 = ex4;
			}
			if (webExceptionStatus == WebExceptionStatus.Pending)
			{
				return;
			}
			if (webExceptionStatus == WebExceptionStatus.Success)
			{
				try
				{
					connectSocketState.servicePoint.CompleteGetConnection(connectSocketState.s4, connectSocketState.s6, socket, ipaddress);
					goto IL_00B4;
				}
				catch (SocketException ex5)
				{
					ex2 = ex5;
					goto IL_00B4;
				}
				catch (ObjectDisposedException ex6)
				{
					ex2 = ex6;
					goto IL_00B4;
				}
			}
			ex2 = new WebException(NetRes.GetWebStatusString(webExceptionStatus), (webExceptionStatus == WebExceptionStatus.ProxyNameResolutionFailure || webExceptionStatus == WebExceptionStatus.NameResolutionFailure) ? connectSocketState.servicePoint.Host : null, ex, webExceptionStatus, null, WebExceptionInternalStatus.ServicePointFatal);
			try
			{
				IL_00B4:
				connectSocketState.pooledStream.ConnectionCallback(connectSocketState.owner, ex2, socket, ipaddress);
			}
			catch
			{
				if (socket == null || !socket.CleanedUp)
				{
					throw;
				}
			}
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x000850B4 File Offset: 0x000840B4
		private void BindUsingDelegate(Socket socket, IPEndPoint remoteIPEndPoint)
		{
			IPEndPoint ipendPoint = new IPEndPoint(remoteIPEndPoint.Address, remoteIPEndPoint.Port);
			int i;
			for (i = 0; i < 2147483647; i++)
			{
				IPEndPoint ipendPoint2 = this.BindIPEndPointDelegate(this, ipendPoint, i);
				if (ipendPoint2 == null)
				{
					break;
				}
				try
				{
					socket.InternalBind(ipendPoint2);
					break;
				}
				catch
				{
				}
			}
			if (i == 2147483647)
			{
				throw new OverflowException("Reached maximum number of BindIPEndPointDelegate retries");
			}
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x00085124 File Offset: 0x00084124
		private WebExceptionStatus ConnectSocketInternal(bool connectFailure, Socket s4, Socket s6, ref Socket socket, ref IPAddress address, ServicePoint.ConnectSocketState state, IAsyncResult asyncResult, int timeout, out Exception exception)
		{
			exception = null;
			bool flag = false;
			IPAddress[] array = null;
			for (int i = 0; i < 2; i++)
			{
				int j = 0;
				int num;
				if (asyncResult == null)
				{
					array = this.GetIPAddressInfoList(out num, array, timeout, out flag);
					if (array == null || array.Length == 0)
					{
						break;
					}
					if (flag)
					{
						break;
					}
				}
				else
				{
					array = state.addresses;
					num = state.currentIndex;
					j = state.i;
					i = state.unsuccessfulAttempts;
				}
				while (j < array.Length)
				{
					IPAddress ipaddress = array[num];
					try
					{
						IPEndPoint ipendPoint = new IPEndPoint(ipaddress, this.m_Port);
						Socket socket2;
						if (ipendPoint.Address.AddressFamily == AddressFamily.InterNetwork)
						{
							socket2 = s4;
						}
						else
						{
							socket2 = s6;
						}
						if (state != null)
						{
							if (asyncResult == null)
							{
								state.addresses = array;
								state.currentIndex = num;
								state.i = j;
								state.unsuccessfulAttempts = i;
								state.connectFailure = connectFailure;
								if (this.BindIPEndPointDelegate != null && !socket2.IsBound)
								{
									this.BindUsingDelegate(socket2, ipendPoint);
								}
								socket2.UnsafeBeginConnect(ipendPoint, ServicePoint.m_ConnectCallbackDelegate, state);
								return WebExceptionStatus.Pending;
							}
							IAsyncResult asyncResult2 = asyncResult;
							asyncResult = null;
							socket2.EndConnect(asyncResult2);
						}
						else
						{
							if (this.BindIPEndPointDelegate != null && !socket2.IsBound)
							{
								this.BindUsingDelegate(socket2, ipendPoint);
							}
							socket2.InternalConnect(ipendPoint);
						}
						socket = socket2;
						address = ipaddress;
						exception = null;
						this.UpdateCurrentIndex(array, num);
						return WebExceptionStatus.Success;
					}
					catch (ObjectDisposedException)
					{
						return WebExceptionStatus.RequestCanceled;
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						exception = ex;
						connectFailure = true;
					}
					num++;
					if (num >= array.Length)
					{
						num = 0;
					}
					j++;
				}
			}
			this.Failed(array);
			if (connectFailure)
			{
				return WebExceptionStatus.ConnectFailure;
			}
			if (flag)
			{
				return WebExceptionStatus.Timeout;
			}
			if (!this.InternalProxyServicePoint)
			{
				return WebExceptionStatus.NameResolutionFailure;
			}
			return WebExceptionStatus.ProxyNameResolutionFailure;
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x000852FC File Offset: 0x000842FC
		private WebExceptionStatus ConnectSocket(Socket s4, Socket s6, ref Socket socket, ref IPAddress address, ServicePoint.ConnectSocketState state, int timeout, out Exception exception)
		{
			return this.ConnectSocketInternal(false, s4, s6, ref socket, ref address, state, null, timeout, out exception);
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x0008531C File Offset: 0x0008431C
		[Conditional("DEBUG")]
		internal void Debug(int requestHash)
		{
			foreach (object obj in this.m_ConnectionGroupList.Values)
			{
				ConnectionGroup connectionGroup = (ConnectionGroup)obj;
				if (connectionGroup != null)
				{
				}
			}
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x00085378 File Offset: 0x00084378
		private void Failed(IPAddress[] addresses)
		{
			if (addresses == this.m_IPAddressInfoList)
			{
				lock (this)
				{
					if (addresses == this.m_IPAddressInfoList)
					{
						this.m_AddressListFailed = true;
					}
				}
			}
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x000853C0 File Offset: 0x000843C0
		private void UpdateCurrentIndex(IPAddress[] addresses, int currentIndex)
		{
			if (addresses == this.m_IPAddressInfoList && (this.m_CurrentAddressInfoIndex != currentIndex || !this.m_ConnectedSinceDns))
			{
				lock (this)
				{
					if (addresses == this.m_IPAddressInfoList)
					{
						if (!ServicePointManager.EnableDnsRoundRobin)
						{
							this.m_CurrentAddressInfoIndex = currentIndex;
						}
						this.m_ConnectedSinceDns = true;
					}
				}
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x00085428 File Offset: 0x00084428
		private bool HasTimedOut
		{
			get
			{
				int dnsRefreshTimeout = ServicePointManager.DnsRefreshTimeout;
				return dnsRefreshTimeout != -1 && this.m_LastDnsResolve + new TimeSpan(0, 0, 0, 0, dnsRefreshTimeout) < DateTime.UtcNow;
			}
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x00085460 File Offset: 0x00084460
		private IPAddress[] GetIPAddressInfoList(out int currentIndex, IPAddress[] addresses, int timeout, out bool timedOut)
		{
			IPHostEntry iphostEntry = null;
			currentIndex = 0;
			bool flag = false;
			bool flag2 = false;
			timedOut = false;
			lock (this)
			{
				if (addresses != null && !this.m_ConnectedSinceDns && !this.m_AddressListFailed && addresses == this.m_IPAddressInfoList)
				{
					return null;
				}
				if (this.m_IPAddressInfoList == null || this.m_AddressListFailed || addresses == this.m_IPAddressInfoList || this.HasTimedOut)
				{
					this.m_CurrentAddressInfoIndex = 0;
					this.m_ConnectedSinceDns = false;
					this.m_AddressListFailed = false;
					this.m_LastDnsResolve = DateTime.UtcNow;
					flag = true;
				}
			}
			if (flag)
			{
				try
				{
					iphostEntry = Dns.InternalResolveFast(this.m_Host, timeout, out timedOut);
					if (timedOut)
					{
						flag2 = true;
					}
				}
				catch (Exception ex)
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					flag2 = true;
				}
			}
			lock (this)
			{
				if (flag)
				{
					this.m_IPAddressInfoList = null;
					if (!flag2 && iphostEntry != null && iphostEntry.AddressList != null && iphostEntry.AddressList.Length > 0)
					{
						this.SetAddressList(iphostEntry);
					}
				}
				if (this.m_IPAddressInfoList != null && this.m_IPAddressInfoList.Length > 0)
				{
					currentIndex = this.m_CurrentAddressInfoIndex;
					if (ServicePointManager.EnableDnsRoundRobin)
					{
						this.m_CurrentAddressInfoIndex++;
						if (this.m_CurrentAddressInfoIndex >= this.m_IPAddressInfoList.Length)
						{
							this.m_CurrentAddressInfoIndex = 0;
						}
					}
					return this.m_IPAddressInfoList;
				}
			}
			return null;
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x000855D8 File Offset: 0x000845D8
		private void SetAddressList(IPHostEntry ipHostEntry)
		{
			bool ipaddressesAreLoopback = this.m_IPAddressesAreLoopback;
			bool flag = this.m_IPAddressInfoList == null;
			this.m_IPAddressesAreLoopback = ServicePoint.IsAddressListLoopback(ipHostEntry.AddressList);
			this.m_IPAddressInfoList = ipHostEntry.AddressList;
			this.m_HostName = ipHostEntry.HostName;
			if (flag || ipaddressesAreLoopback != this.m_IPAddressesAreLoopback)
			{
				this.ResolveConnectionLimit();
			}
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x00085634 File Offset: 0x00084634
		private static bool IsAddressListLoopback(IPAddress[] addressList)
		{
			IPAddress[] array = null;
			try
			{
				array = NclUtilities.LocalAddresses;
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.Web, SR.GetString("net_log_retrieving_localhost_exception", new object[] { ex }));
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_resolved_servicepoint_may_not_be_remote_server"));
				}
			}
			int i;
			for (i = 0; i < addressList.Length; i++)
			{
				if (!IPAddress.IsLoopback(addressList[i]))
				{
					if (array == null)
					{
						break;
					}
					int num = 0;
					while (num < array.Length && !addressList[i].Equals(array[num]))
					{
						num++;
					}
					if (num >= array.Length)
					{
						break;
					}
				}
			}
			return i == addressList.Length;
		}

		// Token: 0x04002189 RID: 8585
		internal const int LoopbackConnectionLimit = 2147483647;

		// Token: 0x0400218A RID: 8586
		private int m_ConnectionLeaseTimeout;

		// Token: 0x0400218B RID: 8587
		private TimerThread.Queue m_ConnectionLeaseTimerQueue;

		// Token: 0x0400218C RID: 8588
		private bool m_ProxyServicePoint;

		// Token: 0x0400218D RID: 8589
		private bool m_UserChangedLimit;

		// Token: 0x0400218E RID: 8590
		private bool m_UseNagleAlgorithm;

		// Token: 0x0400218F RID: 8591
		private TriState m_HostLoopbackGuess;

		// Token: 0x04002190 RID: 8592
		private int m_ReceiveBufferSize;

		// Token: 0x04002191 RID: 8593
		private bool m_Expect100Continue;

		// Token: 0x04002192 RID: 8594
		private bool m_Understands100Continue;

		// Token: 0x04002193 RID: 8595
		private HttpBehaviour m_HttpBehaviour;

		// Token: 0x04002194 RID: 8596
		private string m_LookupString;

		// Token: 0x04002195 RID: 8597
		private int m_ConnectionLimit;

		// Token: 0x04002196 RID: 8598
		private Hashtable m_ConnectionGroupList;

		// Token: 0x04002197 RID: 8599
		private Uri m_Address;

		// Token: 0x04002198 RID: 8600
		private string m_Host;

		// Token: 0x04002199 RID: 8601
		private int m_Port;

		// Token: 0x0400219A RID: 8602
		private TimerThread.Queue m_IdlingQueue;

		// Token: 0x0400219B RID: 8603
		private TimerThread.Timer m_ExpiringTimer;

		// Token: 0x0400219C RID: 8604
		private DateTime m_IdleSince;

		// Token: 0x0400219D RID: 8605
		private string m_ConnectionName;

		// Token: 0x0400219E RID: 8606
		private int m_CurrentConnections;

		// Token: 0x0400219F RID: 8607
		private bool m_HostMode;

		// Token: 0x040021A0 RID: 8608
		private BindIPEndPoint m_BindIPEndPointDelegate;

		// Token: 0x040021A1 RID: 8609
		private object m_CachedChannelBinding;

		// Token: 0x040021A2 RID: 8610
		private static readonly AsyncCallback m_ConnectCallbackDelegate = new AsyncCallback(ServicePoint.ConnectSocketCallback);

		// Token: 0x040021A3 RID: 8611
		private object m_ServerCertificateOrBytes;

		// Token: 0x040021A4 RID: 8612
		private object m_ClientCertificateOrBytes;

		// Token: 0x040021A5 RID: 8613
		private bool m_UseTcpKeepAlive;

		// Token: 0x040021A6 RID: 8614
		private int m_TcpKeepAliveTime;

		// Token: 0x040021A7 RID: 8615
		private int m_TcpKeepAliveInterval;

		// Token: 0x040021A8 RID: 8616
		private string m_HostName = string.Empty;

		// Token: 0x040021A9 RID: 8617
		private IPAddress[] m_IPAddressInfoList;

		// Token: 0x040021AA RID: 8618
		private int m_CurrentAddressInfoIndex;

		// Token: 0x040021AB RID: 8619
		private bool m_ConnectedSinceDns;

		// Token: 0x040021AC RID: 8620
		private bool m_AddressListFailed;

		// Token: 0x040021AD RID: 8621
		private DateTime m_LastDnsResolve;

		// Token: 0x040021AE RID: 8622
		private bool m_IPAddressesAreLoopback;

		// Token: 0x02000434 RID: 1076
		private class HandshakeDoneProcedure
		{
			// Token: 0x060021B2 RID: 8626 RVA: 0x000856FC File Offset: 0x000846FC
			internal static RemoteCertValidationCallback CreateAdapter(ServicePoint serviePoint, TlsStream secureStream, object request)
			{
				ServicePoint.HandshakeDoneProcedure handshakeDoneProcedure = new ServicePoint.HandshakeDoneProcedure(serviePoint, secureStream, request);
				return new RemoteCertValidationCallback(handshakeDoneProcedure.CertValidationCallback);
			}

			// Token: 0x060021B3 RID: 8627 RVA: 0x0008571E File Offset: 0x0008471E
			private HandshakeDoneProcedure(ServicePoint serviePoint, TlsStream secureStream, object request)
			{
				this.m_ServicePoint = serviePoint;
				this.m_SecureStream = secureStream;
				this.m_Request = request;
			}

			// Token: 0x060021B4 RID: 8628 RVA: 0x0008573C File Offset: 0x0008473C
			private bool CertValidationCallback(string hostName, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
			{
				this.m_ServicePoint.UpdateServerCertificate(certificate);
				this.m_ServicePoint.UpdateClientCertificate(this.m_SecureStream.ClientCertificate);
				bool flag = true;
				if (ServicePointManager.GetLegacyCertificatePolicy() != null && this.m_Request is WebRequest)
				{
					flag = false;
					bool flag2 = ServicePointManager.CertPolicyValidationCallback.Invoke(hostName, this.m_ServicePoint, certificate, (WebRequest)this.m_Request, chain, sslPolicyErrors);
					if (!flag2 && (!ServicePointManager.CertPolicyValidationCallback.UsesDefault || ServicePointManager.ServerCertificateValidationCallback == null))
					{
						return flag2;
					}
				}
				if (ServicePointManager.ServerCertificateValidationCallback != null)
				{
					return ServicePointManager.ServerCertValidationCallback.Invoke(this.m_Request, certificate, chain, sslPolicyErrors);
				}
				return !flag || sslPolicyErrors == SslPolicyErrors.None;
			}

			// Token: 0x040021AF RID: 8623
			private TlsStream m_SecureStream;

			// Token: 0x040021B0 RID: 8624
			private object m_Request;

			// Token: 0x040021B1 RID: 8625
			private ServicePoint m_ServicePoint;
		}

		// Token: 0x02000435 RID: 1077
		private class ConnectSocketState
		{
			// Token: 0x060021B5 RID: 8629 RVA: 0x000857E4 File Offset: 0x000847E4
			internal ConnectSocketState(ServicePoint servicePoint, PooledStream pooledStream, object owner, Socket s4, Socket s6)
			{
				this.servicePoint = servicePoint;
				this.pooledStream = pooledStream;
				this.owner = owner;
				this.s4 = s4;
				this.s6 = s6;
			}

			// Token: 0x040021B2 RID: 8626
			internal ServicePoint servicePoint;

			// Token: 0x040021B3 RID: 8627
			internal Socket s4;

			// Token: 0x040021B4 RID: 8628
			internal Socket s6;

			// Token: 0x040021B5 RID: 8629
			internal object owner;

			// Token: 0x040021B6 RID: 8630
			internal IPAddress[] addresses;

			// Token: 0x040021B7 RID: 8631
			internal int currentIndex;

			// Token: 0x040021B8 RID: 8632
			internal int i;

			// Token: 0x040021B9 RID: 8633
			internal int unsuccessfulAttempts;

			// Token: 0x040021BA RID: 8634
			internal bool connectFailure;

			// Token: 0x040021BB RID: 8635
			internal PooledStream pooledStream;
		}
	}
}
