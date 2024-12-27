using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x0200003C RID: 60
	public class TcpChannel : IChannelReceiver, IChannelSender, IChannel, ISecurableChannel
	{
		// Token: 0x060001F0 RID: 496 RVA: 0x00009E1C File Offset: 0x00008E1C
		public TcpChannel()
		{
			this._clientChannel = new TcpClientChannel();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00009E41 File Offset: 0x00008E41
		public TcpChannel(int port)
			: this()
		{
			this._serverChannel = new TcpServerChannel(port);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00009E58 File Offset: 0x00008E58
		public TcpChannel(IDictionary properties, IClientChannelSinkProvider clientSinkProvider, IServerChannelSinkProvider serverSinkProvider)
		{
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			bool flag = false;
			if (properties != null)
			{
				foreach (object obj in properties)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text;
					if ((text = (string)dictionaryEntry.Key) != null)
					{
						if (text == "name")
						{
							this._channelName = (string)dictionaryEntry.Value;
							continue;
						}
						if (text == "priority")
						{
							this._channelPriority = Convert.ToInt32((string)dictionaryEntry.Value, CultureInfo.InvariantCulture);
							continue;
						}
						if (text == "port")
						{
							hashtable2["port"] = dictionaryEntry.Value;
							flag = true;
							continue;
						}
					}
					hashtable[dictionaryEntry.Key] = dictionaryEntry.Value;
					hashtable2[dictionaryEntry.Key] = dictionaryEntry.Value;
				}
			}
			this._clientChannel = new TcpClientChannel(hashtable, clientSinkProvider);
			if (flag)
			{
				this._serverChannel = new TcpServerChannel(hashtable2, serverSinkProvider);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00009FA8 File Offset: 0x00008FA8
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x00009FD4 File Offset: 0x00008FD4
		public bool IsSecured
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				if (this._clientChannel != null)
				{
					return this._clientChannel.IsSecured;
				}
				return this._serverChannel != null && this._serverChannel.IsSecured;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			set
			{
				if (ChannelServices.RegisteredChannels.Contains(this))
				{
					throw new InvalidOperationException(CoreChannel.GetResourceString("Remoting_InvalidOperation_IsSecuredCannotBeChangedOnRegisteredChannels"));
				}
				if (this._clientChannel != null)
				{
					this._clientChannel.IsSecured = value;
				}
				if (this._serverChannel != null)
				{
					this._serverChannel.IsSecured = value;
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000A026 File Offset: 0x00009026
		public int ChannelPriority
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelPriority;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000A02E File Offset: 0x0000902E
		public string ChannelName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelName;
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000A036 File Offset: 0x00009036
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public string Parse(string url, out string objectURI)
		{
			return TcpChannelHelper.ParseURL(url, out objectURI);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000A03F File Offset: 0x0000903F
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public IMessageSink CreateMessageSink(string url, object remoteChannelData, out string objectURI)
		{
			return this._clientChannel.CreateMessageSink(url, remoteChannelData, out objectURI);
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000A04F File Offset: 0x0000904F
		public object ChannelData
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				if (this._serverChannel != null)
				{
					return this._serverChannel.ChannelData;
				}
				return null;
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000A066 File Offset: 0x00009066
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public string[] GetUrlsForUri(string objectURI)
		{
			if (this._serverChannel != null)
			{
				return this._serverChannel.GetUrlsForUri(objectURI);
			}
			return null;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000A07E File Offset: 0x0000907E
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void StartListening(object data)
		{
			if (this._serverChannel != null)
			{
				this._serverChannel.StartListening(data);
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000A094 File Offset: 0x00009094
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void StopListening(object data)
		{
			if (this._serverChannel != null)
			{
				this._serverChannel.StopListening(data);
			}
		}

		// Token: 0x04000160 RID: 352
		private TcpClientChannel _clientChannel;

		// Token: 0x04000161 RID: 353
		private TcpServerChannel _serverChannel;

		// Token: 0x04000162 RID: 354
		private int _channelPriority = 1;

		// Token: 0x04000163 RID: 355
		private string _channelName = "tcp";
	}
}
