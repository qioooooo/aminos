using System;
using System.Collections;
using System.Globalization;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x02000050 RID: 80
	public class IpcServerChannel : IChannelReceiver, IChannel, ISecurableChannel
	{
		// Token: 0x06000280 RID: 640 RVA: 0x0000C860 File Offset: 0x0000B860
		public IpcServerChannel(string portName)
		{
			if (portName == null)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Ipc_NoPortNameSpecified"));
			}
			this._portName = portName;
			this.SetupChannel();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000C8BC File Offset: 0x0000B8BC
		public IpcServerChannel(string name, string portName)
		{
			if (portName == null)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Ipc_NoPortNameSpecified"));
			}
			this._channelName = name;
			this._portName = portName;
			this.SetupChannel();
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000C91C File Offset: 0x0000B91C
		public IpcServerChannel(string name, string portName, IServerChannelSinkProvider sinkProvider)
		{
			if (portName == null)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Ipc_NoPortNameSpecified"));
			}
			this._channelName = name;
			this._portName = portName;
			this._sinkProvider = sinkProvider;
			this.SetupChannel();
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000C983 File Offset: 0x0000B983
		public IpcServerChannel(IDictionary properties, IServerChannelSinkProvider sinkProvider)
			: this(properties, sinkProvider, null)
		{
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000C990 File Offset: 0x0000B990
		public IpcServerChannel(IDictionary properties, IServerChannelSinkProvider sinkProvider, CommonSecurityDescriptor securityDescriptor)
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
					case "portName":
						this._portName = (string)dictionaryEntry.Value;
						break;
					case "priority":
						this._channelPriority = Convert.ToInt32(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "secure":
						this._secure = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "impersonate":
						this._impersonate = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						this.authSet = true;
						break;
					case "suppressChannelData":
						this._bSuppressChannelData = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "authorizedGroup":
						this._authorizedGroup = (string)dictionaryEntry.Value;
						break;
					case "exclusiveAddressUse":
						this._bExclusiveAddressUse = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					}
				}
			}
			if (this._portName == null)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Ipc_NoPortNameSpecified"));
			}
			this._sinkProvider = sinkProvider;
			this._securityDescriptor = securityDescriptor;
			this.SetupChannel();
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000CBDC File Offset: 0x0000BBDC
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0000CBE4 File Offset: 0x0000BBE4
		public bool IsSecured
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._secure;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			set
			{
				this._secure = value;
				if (this._transportSink != null)
				{
					this._transportSink.IsSecured = value;
				}
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000CC04 File Offset: 0x0000BC04
		private void SetupChannel()
		{
			if (this.authSet && !this._secure)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Ipc_AuthenticationConfig"));
			}
			this._channelData = new ChannelDataStore(null);
			this._channelData.ChannelUris = new string[1];
			this._channelData.ChannelUris[0] = this.GetChannelUri();
			if (this._sinkProvider == null)
			{
				this._sinkProvider = this.CreateDefaultServerProviderChain();
			}
			CoreChannel.CollectChannelDataFromServerSinkProviders(this._channelData, this._sinkProvider);
			IServerChannelSink serverChannelSink = ChannelServices.CreateServerChannelSinkChain(this._sinkProvider, this);
			this._transportSink = new IpcServerTransportSink(serverChannelSink, this._secure, this._impersonate);
			ThreadStart threadStart = new ThreadStart(this.Listen);
			this._listenerThread = new Thread(threadStart);
			this._listenerThread.IsBackground = true;
			this.StartListening(null);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000CCD8 File Offset: 0x0000BCD8
		private IServerChannelSinkProvider CreateDefaultServerProviderChain()
		{
			IServerChannelSinkProvider serverChannelSinkProvider = new BinaryServerFormatterSinkProvider();
			IServerChannelSinkProvider serverChannelSinkProvider2 = serverChannelSinkProvider;
			serverChannelSinkProvider2.Next = new SoapServerFormatterSinkProvider();
			return serverChannelSinkProvider;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000CCF9 File Offset: 0x0000BCF9
		public int ChannelPriority
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelPriority;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000CD01 File Offset: 0x0000BD01
		public string ChannelName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelName;
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000CD09 File Offset: 0x0000BD09
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public string Parse(string url, out string objectURI)
		{
			return IpcChannelHelper.ParseURL(url, out objectURI);
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000CD12 File Offset: 0x0000BD12
		public object ChannelData
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				if (this._bSuppressChannelData || !this._bListening)
				{
					return null;
				}
				return this._channelData;
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000CD2C File Offset: 0x0000BD2C
		public string GetChannelUri()
		{
			return "ipc://" + this._portName;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000CD40 File Offset: 0x0000BD40
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public virtual string[] GetUrlsForUri(string objectUri)
		{
			if (objectUri == null)
			{
				throw new ArgumentNullException("objectUri");
			}
			string[] array = new string[1];
			if (!objectUri.StartsWith("/", StringComparison.Ordinal))
			{
				objectUri = "/" + objectUri;
			}
			array[0] = this.GetChannelUri() + objectUri;
			return array;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000CD90 File Offset: 0x0000BD90
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void StartListening(object data)
		{
			if (!this._listenerThread.IsAlive)
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
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000CDE0 File Offset: 0x0000BDE0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void StopListening(object data)
		{
			this._bListening = false;
			this._port.Dispose();
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000CDF4 File Offset: 0x0000BDF4
		private void Listen()
		{
			bool flag = true;
			IntPtr zero = IntPtr.Zero;
			bool flag2 = false;
			CommonSecurityDescriptor commonSecurityDescriptor = this._securityDescriptor;
			if (flag)
			{
				try
				{
					if (commonSecurityDescriptor == null && this._authorizedGroup != null)
					{
						NTAccount ntaccount = new NTAccount(this._authorizedGroup);
						commonSecurityDescriptor = IpcPort.CreateSecurityDescriptor((SecurityIdentifier)ntaccount.Translate(typeof(SecurityIdentifier)));
					}
					this._port = IpcPort.Create(this._portName, commonSecurityDescriptor, this._bExclusiveAddressUse);
				}
				catch (Exception ex)
				{
					this._startListeningException = ex;
				}
				catch
				{
					this._startListeningException = new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException"));
				}
				finally
				{
					this._waitForStartListening.Set();
				}
				if (this._port != null)
				{
					flag2 = this._port.WaitForConnect();
					flag = this._bListening;
				}
			}
			while (flag && this._startListeningException == null)
			{
				IpcPort ipcPort = IpcPort.Create(this._portName, commonSecurityDescriptor, false);
				if (flag2)
				{
					new IpcServerHandler(this._port, CoreChannel.RequestQueue, new PipeStream(this._port))
					{
						DataArrivedCallback = new WaitCallback(this._transportSink.ServiceRequest)
					}.BeginReadMessage();
				}
				this._port = ipcPort;
				flag2 = this._port.WaitForConnect();
				flag = this._bListening;
			}
		}

		// Token: 0x040001CF RID: 463
		private int _channelPriority = 20;

		// Token: 0x040001D0 RID: 464
		private string _channelName = "ipc server";

		// Token: 0x040001D1 RID: 465
		private string _portName;

		// Token: 0x040001D2 RID: 466
		private ChannelDataStore _channelData;

		// Token: 0x040001D3 RID: 467
		private IpcPort _port;

		// Token: 0x040001D4 RID: 468
		private bool _bSuppressChannelData;

		// Token: 0x040001D5 RID: 469
		private bool _secure;

		// Token: 0x040001D6 RID: 470
		private bool _impersonate;

		// Token: 0x040001D7 RID: 471
		private string _authorizedGroup;

		// Token: 0x040001D8 RID: 472
		private CommonSecurityDescriptor _securityDescriptor;

		// Token: 0x040001D9 RID: 473
		private bool authSet;

		// Token: 0x040001DA RID: 474
		private bool _bExclusiveAddressUse = true;

		// Token: 0x040001DB RID: 475
		private IServerChannelSinkProvider _sinkProvider;

		// Token: 0x040001DC RID: 476
		private IpcServerTransportSink _transportSink;

		// Token: 0x040001DD RID: 477
		private Thread _listenerThread;

		// Token: 0x040001DE RID: 478
		private bool _bListening;

		// Token: 0x040001DF RID: 479
		private Exception _startListeningException;

		// Token: 0x040001E0 RID: 480
		private AutoResetEvent _waitForStartListening = new AutoResetEvent(false);
	}
}
