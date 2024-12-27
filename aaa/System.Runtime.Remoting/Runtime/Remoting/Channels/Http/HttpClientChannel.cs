using System;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000028 RID: 40
	public class HttpClientChannel : BaseChannelWithProperties, IChannelSender, IChannel, ISecurableChannel
	{
		// Token: 0x06000128 RID: 296 RVA: 0x00006200 File Offset: 0x00005200
		public HttpClientChannel()
		{
			this.SetupChannel();
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006238 File Offset: 0x00005238
		public HttpClientChannel(string name, IClientChannelSinkProvider sinkProvider)
		{
			this._channelName = name;
			this._sinkProvider = sinkProvider;
			this.SetupChannel();
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006288 File Offset: 0x00005288
		public HttpClientChannel(IDictionary properties, IClientChannelSinkProvider sinkProvider)
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
					case "priority":
						this._channelPriority = Convert.ToInt32(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "proxyName":
						this["proxyName"] = dictionaryEntry.Value;
						break;
					case "proxyPort":
						this["proxyPort"] = dictionaryEntry.Value;
						break;
					case "timeout":
						this._timeout = Convert.ToInt32(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "clientConnectionLimit":
						this._clientConnectionLimit = Convert.ToInt32(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "useDefaultCredentials":
						this._bUseDefaultCredentials = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "useAuthenticatedConnectionSharing":
						this._bAuthenticatedConnectionSharing = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					}
				}
			}
			this._sinkProvider = sinkProvider;
			this.SetupChannel();
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000064B0 File Offset: 0x000054B0
		private void SetupChannel()
		{
			if (this._sinkProvider != null)
			{
				CoreChannel.AppendProviderToClientProviderChain(this._sinkProvider, new HttpClientTransportSinkProvider(this._timeout));
				return;
			}
			this._sinkProvider = this.CreateDefaultClientProviderChain();
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000064DD File Offset: 0x000054DD
		// (set) Token: 0x0600012D RID: 301 RVA: 0x000064E5 File Offset: 0x000054E5
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
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600012E RID: 302 RVA: 0x000064EE File Offset: 0x000054EE
		public int ChannelPriority
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelPriority;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600012F RID: 303 RVA: 0x000064F6 File Offset: 0x000054F6
		public string ChannelName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelName;
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000064FE File Offset: 0x000054FE
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public string Parse(string url, out string objectURI)
		{
			return HttpChannelHelper.ParseURL(url, out objectURI);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006508 File Offset: 0x00005508
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public virtual IMessageSink CreateMessageSink(string url, object remoteChannelData, out string objectURI)
		{
			objectURI = null;
			string text = null;
			if (url != null)
			{
				text = this.Parse(url, out objectURI);
			}
			else if (remoteChannelData != null && remoteChannelData is IChannelDataStore)
			{
				IChannelDataStore channelDataStore = (IChannelDataStore)remoteChannelData;
				string text2 = this.Parse(channelDataStore.ChannelUris[0], out objectURI);
				if (text2 != null)
				{
					text = channelDataStore.ChannelUris[0];
				}
			}
			if (text == null)
			{
				return null;
			}
			if (url == null)
			{
				url = text;
			}
			if (this._clientConnectionLimit > 0)
			{
				ServicePoint servicePoint = ServicePointManager.FindServicePoint(new Uri(text));
				if (servicePoint.ConnectionLimit < this._clientConnectionLimit)
				{
					servicePoint.ConnectionLimit = this._clientConnectionLimit;
				}
			}
			IClientChannelSink clientChannelSink = this._sinkProvider.CreateSink(this, url, remoteChannelData);
			IMessageSink messageSink = clientChannelSink as IMessageSink;
			if (clientChannelSink != null && messageSink == null)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Channels_ChannelSinkNotMsgSink"));
			}
			return messageSink;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000065C4 File Offset: 0x000055C4
		private IClientChannelSinkProvider CreateDefaultClientProviderChain()
		{
			IClientChannelSinkProvider clientChannelSinkProvider = new SoapClientFormatterSinkProvider();
			IClientChannelSinkProvider clientChannelSinkProvider2 = clientChannelSinkProvider;
			clientChannelSinkProvider2.Next = new HttpClientTransportSinkProvider(this._timeout);
			return clientChannelSinkProvider;
		}

		// Token: 0x1700003F RID: 63
		public override object this[object key]
		{
			get
			{
				string text = key as string;
				if (text == null)
				{
					return null;
				}
				string text2;
				if ((text2 = text.ToLower(CultureInfo.InvariantCulture)) != null)
				{
					if (text2 == "proxyname")
					{
						return this._proxyName;
					}
					if (text2 == "proxyport")
					{
						return this._proxyPort;
					}
				}
				return null;
			}
			set
			{
				string text = key as string;
				if (text == null)
				{
					return;
				}
				string text2;
				if ((text2 = text.ToLower(CultureInfo.InvariantCulture)) != null)
				{
					if (text2 == "proxyname")
					{
						this._proxyName = (string)value;
						this.UpdateProxy();
						return;
					}
					if (!(text2 == "proxyport"))
					{
						return;
					}
					this._proxyPort = Convert.ToInt32(value, CultureInfo.InvariantCulture);
					this.UpdateProxy();
				}
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000135 RID: 309 RVA: 0x000066B0 File Offset: 0x000056B0
		public override ICollection Keys
		{
			get
			{
				if (HttpClientChannel.s_keySet == null)
				{
					HttpClientChannel.s_keySet = new ArrayList(2) { "proxyname", "proxyport" };
				}
				return HttpClientChannel.s_keySet;
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000066F0 File Offset: 0x000056F0
		private void UpdateProxy()
		{
			if (this._proxyName != null && this._proxyName.Length > 0 && this._proxyPort > 0)
			{
				WebProxy webProxy = new WebProxy(this._proxyName, this._proxyPort);
				webProxy.BypassProxyOnLocal = true;
				string[] array = new string[] { CoreChannel.GetMachineIp() };
				webProxy.BypassList = array;
				this._proxyObject = webProxy;
				return;
			}
			this._proxyObject = new WebProxy();
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00006760 File Offset: 0x00005760
		internal IWebProxy ProxyObject
		{
			get
			{
				return this._proxyObject;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00006768 File Offset: 0x00005768
		internal bool UseDefaultCredentials
		{
			get
			{
				return this._secure || this._bUseDefaultCredentials;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0000677A File Offset: 0x0000577A
		internal bool UseAuthenticatedConnectionSharing
		{
			get
			{
				return this._bAuthenticatedConnectionSharing;
			}
		}

		// Token: 0x040000DA RID: 218
		private const string ProxyNameKey = "proxyname";

		// Token: 0x040000DB RID: 219
		private const string ProxyPortKey = "proxyport";

		// Token: 0x040000DC RID: 220
		private static ICollection s_keySet;

		// Token: 0x040000DD RID: 221
		private int _channelPriority = 1;

		// Token: 0x040000DE RID: 222
		private string _channelName = "http client";

		// Token: 0x040000DF RID: 223
		private IWebProxy _proxyObject;

		// Token: 0x040000E0 RID: 224
		private string _proxyName;

		// Token: 0x040000E1 RID: 225
		private int _proxyPort = -1;

		// Token: 0x040000E2 RID: 226
		private int _timeout = -1;

		// Token: 0x040000E3 RID: 227
		private int _clientConnectionLimit;

		// Token: 0x040000E4 RID: 228
		private bool _bUseDefaultCredentials;

		// Token: 0x040000E5 RID: 229
		private bool _bAuthenticatedConnectionSharing = true;

		// Token: 0x040000E6 RID: 230
		private bool _secure;

		// Token: 0x040000E7 RID: 231
		private IClientChannelSinkProvider _sinkProvider;
	}
}
