using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.MetadataServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x0200002C RID: 44
	public class HttpServerChannel : BaseChannelWithProperties, IChannelReceiver, IChannel, IChannelReceiverHook
	{
		// Token: 0x06000157 RID: 343 RVA: 0x000076C8 File Offset: 0x000066C8
		public HttpServerChannel()
		{
			this.SetupMachineName();
			this.SetupChannel();
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000773C File Offset: 0x0000673C
		public HttpServerChannel(int port)
		{
			this._port = port;
			this.SetupMachineName();
			this.SetupChannel();
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000077B8 File Offset: 0x000067B8
		public HttpServerChannel(string name, int port)
		{
			this._channelName = name;
			this._port = port;
			this.SetupMachineName();
			this.SetupChannel();
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007838 File Offset: 0x00006838
		public HttpServerChannel(string name, int port, IServerChannelSinkProvider sinkProvider)
		{
			this._channelName = name;
			this._port = port;
			this._sinkProvider = sinkProvider;
			this.SetupMachineName();
			this.SetupChannel();
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000078C0 File Offset: 0x000068C0
		public HttpServerChannel(IDictionary properties, IServerChannelSinkProvider sinkProvider)
		{
			if (properties != null)
			{
				foreach (object obj in properties)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text;
					switch (text = (string)dictionaryEntry.Key)
					{
					case "name":
						this._channelName = (string)dictionaryEntry.Value;
						break;
					case "bindTo":
						this._bindToAddr = IPAddress.Parse((string)dictionaryEntry.Value);
						break;
					case "listen":
						this._wantsToListen = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "machineName":
						this._forcedMachineName = (string)dictionaryEntry.Value;
						break;
					case "port":
						this._port = Convert.ToInt32(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "priority":
						this._channelPriority = Convert.ToInt32(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "suppressChannelData":
						this._bSuppressChannelData = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "useIpAddress":
						this._bUseIpAddress = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "exclusiveAddressUse":
						this._bExclusiveAddressUse = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					}
				}
			}
			this._sinkProvider = sinkProvider;
			this.SetupMachineName();
			this.SetupChannel();
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00007B4C File Offset: 0x00006B4C
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00007B4F File Offset: 0x00006B4F
		internal bool IsSecured
		{
			get
			{
				return false;
			}
			set
			{
				if (this._port >= 0 && value)
				{
					throw new RemotingException(CoreChannel.GetResourceString("Remoting_Http_UseIISToSecureHttpServer"));
				}
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007B70 File Offset: 0x00006B70
		private void SetupMachineName()
		{
			if (this._forcedMachineName != null)
			{
				this._machineName = CoreChannel.DecodeMachineName(this._forcedMachineName);
				return;
			}
			if (!this._bUseIpAddress)
			{
				this._machineName = CoreChannel.GetMachineName();
				return;
			}
			if (this._bindToAddr == IPAddress.Any || this._bindToAddr == IPAddress.IPv6Any)
			{
				this._machineName = CoreChannel.GetMachineIp();
			}
			else
			{
				this._machineName = this._bindToAddr.ToString();
			}
			if (this._bindToAddr.AddressFamily == AddressFamily.InterNetworkV6)
			{
				this._machineName = "[" + this._machineName + "]";
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007C10 File Offset: 0x00006C10
		private void SetupChannel()
		{
			this._channelData = new ChannelDataStore(null);
			if (this._port > 0)
			{
				string channelUri = this.GetChannelUri();
				this._channelData.ChannelUris = new string[1];
				this._channelData.ChannelUris[0] = channelUri;
				this._wantsToListen = false;
			}
			if (this._sinkProvider == null)
			{
				this._sinkProvider = this.CreateDefaultServerProviderChain();
			}
			CoreChannel.CollectChannelDataFromServerSinkProviders(this._channelData, this._sinkProvider);
			this._sinkChain = ChannelServices.CreateServerChannelSinkChain(this._sinkProvider, this);
			this._transportSink = new HttpServerTransportSink(this._sinkChain);
			this.SinksWithProperties = this._sinkChain;
			if (this._port >= 0)
			{
				this._tcpListener = new ExclusiveTcpListener(this._bindToAddr, this._port);
				ThreadStart threadStart = new ThreadStart(this.Listen);
				this._listenerThread = new Thread(threadStart);
				this._listenerThread.IsBackground = true;
				this.StartListening(null);
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00007D00 File Offset: 0x00006D00
		private IServerChannelSinkProvider CreateDefaultServerProviderChain()
		{
			IServerChannelSinkProvider serverChannelSinkProvider = new SdlChannelSinkProvider();
			IServerChannelSinkProvider serverChannelSinkProvider2 = serverChannelSinkProvider;
			serverChannelSinkProvider2.Next = new SoapServerFormatterSinkProvider();
			serverChannelSinkProvider2 = serverChannelSinkProvider2.Next;
			serverChannelSinkProvider2.Next = new BinaryServerFormatterSinkProvider();
			return serverChannelSinkProvider;
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00007D33 File Offset: 0x00006D33
		public int ChannelPriority
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelPriority;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00007D3B File Offset: 0x00006D3B
		public string ChannelName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelName;
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007D43 File Offset: 0x00006D43
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public string Parse(string url, out string objectURI)
		{
			return HttpChannelHelper.ParseURL(url, out objectURI);
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00007D4C File Offset: 0x00006D4C
		public object ChannelData
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				if (!this._bSuppressChannelData && (this._bListening || this._bHooked))
				{
					return this._channelData;
				}
				return null;
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00007D70 File Offset: 0x00006D70
		public string GetChannelUri()
		{
			if (this._channelData != null && this._channelData.ChannelUris != null)
			{
				return this._channelData.ChannelUris[0];
			}
			return string.Concat(new object[] { "http://", this._machineName, ":", this._port });
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007DD4 File Offset: 0x00006DD4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public virtual string[] GetUrlsForUri(string objectUri)
		{
			string[] array = new string[1];
			if (!objectUri.StartsWith("/", StringComparison.Ordinal))
			{
				objectUri = "/" + objectUri;
			}
			array[0] = this.GetChannelUri() + objectUri;
			return array;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007E14 File Offset: 0x00006E14
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void StartListening(object data)
		{
			if (this._port >= 0 && !this._listenerThread.IsAlive)
			{
				this._listenerThread.Start();
				this._waitForStartListening.WaitOne();
				if (this._startListeningException != null)
				{
					Exception startListeningException = this._startListeningException;
					this._startListeningException = null;
					throw startListeningException;
				}
				this._bListening = true;
				if (this._port == 0)
				{
					this._port = ((IPEndPoint)this._tcpListener.LocalEndpoint).Port;
					if (this._channelData != null)
					{
						string channelUri = this.GetChannelUri();
						this._channelData.ChannelUris = new string[1];
						this._channelData.ChannelUris[0] = channelUri;
					}
				}
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007EC4 File Offset: 0x00006EC4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void StopListening(object data)
		{
			if (this._port > 0)
			{
				this._bListening = false;
				if (this._tcpListener != null)
				{
					this._tcpListener.Stop();
				}
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00007EE9 File Offset: 0x00006EE9
		public string ChannelScheme
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return "http";
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00007EF0 File Offset: 0x00006EF0
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00007EF8 File Offset: 0x00006EF8
		public bool WantsToListen
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._wantsToListen;
			}
			set
			{
				this._wantsToListen = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00007F01 File Offset: 0x00006F01
		public IServerChannelSink ChannelSinkChain
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return this._sinkChain;
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007F0C File Offset: 0x00006F0C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void AddHookChannelUri(string channelUri)
		{
			if (this._channelData.ChannelUris != null)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Http_LimitListenerOfOne"));
			}
			if (this._forcedMachineName != null)
			{
				channelUri = HttpChannelHelper.ReplaceMachineNameWithThisString(channelUri, this._forcedMachineName);
			}
			else if (this._bUseIpAddress)
			{
				channelUri = HttpChannelHelper.ReplaceMachineNameWithThisString(channelUri, CoreChannel.GetMachineIp());
			}
			this._channelData.ChannelUris = new string[] { channelUri };
			this._wantsToListen = false;
			this._bHooked = true;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007F88 File Offset: 0x00006F88
		private void Listen()
		{
			bool flag = false;
			try
			{
				this._tcpListener.Start(this._bExclusiveAddressUse);
				flag = true;
			}
			catch (Exception ex)
			{
				this._startListeningException = ex;
			}
			catch
			{
				this._startListeningException = new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException"));
			}
			this._waitForStartListening.Set();
			while (flag)
			{
				try
				{
					Socket socket = this._tcpListener.AcceptSocket();
					if (socket == null)
					{
						throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Socket_Accept"), new object[] { Marshal.GetLastWin32Error().ToString(CultureInfo.InvariantCulture) }));
					}
					socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, 1);
					socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
					LingerOption lingerOption = new LingerOption(true, 3);
					socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, lingerOption);
					Stream stream = new SocketStream(socket);
					new HttpServerSocketHandler(socket, CoreChannel.RequestQueue, stream)
					{
						DataArrivedCallback = new WaitCallback(this._transportSink.ServiceRequest)
					}.BeginReadMessage();
				}
				catch (Exception ex2)
				{
					if (!this._bListening)
					{
						flag = false;
					}
					else
					{
						SocketException ex3 = ex2 as SocketException;
					}
				}
				catch
				{
					if (!this._bListening)
					{
						flag = false;
					}
				}
			}
		}

		// Token: 0x17000050 RID: 80
		public override object this[object key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000171 RID: 369 RVA: 0x000080F9 File Offset: 0x000070F9
		public override ICollection Keys
		{
			get
			{
				return new ArrayList();
			}
		}

		// Token: 0x04000118 RID: 280
		private int _channelPriority = 1;

		// Token: 0x04000119 RID: 281
		private string _channelName = "http server";

		// Token: 0x0400011A RID: 282
		private string _machineName;

		// Token: 0x0400011B RID: 283
		private int _port = -1;

		// Token: 0x0400011C RID: 284
		private ChannelDataStore _channelData;

		// Token: 0x0400011D RID: 285
		private string _forcedMachineName;

		// Token: 0x0400011E RID: 286
		private bool _bUseIpAddress = true;

		// Token: 0x0400011F RID: 287
		private IPAddress _bindToAddr = (Socket.SupportsIPv4 ? IPAddress.Any : IPAddress.IPv6Any);

		// Token: 0x04000120 RID: 288
		private bool _bSuppressChannelData;

		// Token: 0x04000121 RID: 289
		private IServerChannelSinkProvider _sinkProvider;

		// Token: 0x04000122 RID: 290
		private HttpServerTransportSink _transportSink;

		// Token: 0x04000123 RID: 291
		private IServerChannelSink _sinkChain;

		// Token: 0x04000124 RID: 292
		private bool _wantsToListen = true;

		// Token: 0x04000125 RID: 293
		private bool _bHooked;

		// Token: 0x04000126 RID: 294
		private ExclusiveTcpListener _tcpListener;

		// Token: 0x04000127 RID: 295
		private bool _bExclusiveAddressUse = true;

		// Token: 0x04000128 RID: 296
		private Thread _listenerThread;

		// Token: 0x04000129 RID: 297
		private bool _bListening;

		// Token: 0x0400012A RID: 298
		private Exception _startListeningException;

		// Token: 0x0400012B RID: 299
		private AutoResetEvent _waitForStartListening = new AutoResetEvent(false);
	}
}
