using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x0200003E RID: 62
	public class TcpClientChannel : IChannelSender, IChannel, ISecurableChannel
	{
		// Token: 0x060001FE RID: 510 RVA: 0x0000A0FB File Offset: 0x000090FB
		public TcpClientChannel()
		{
			this.SetupChannel();
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A11B File Offset: 0x0000911B
		public TcpClientChannel(string name, IClientChannelSinkProvider sinkProvider)
		{
			this._channelName = name;
			this._sinkProvider = sinkProvider;
			this.SetupChannel();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A14C File Offset: 0x0000914C
		public TcpClientChannel(IDictionary properties, IClientChannelSinkProvider sinkProvider)
		{
			if (properties != null)
			{
				this._prop = properties;
				foreach (object obj in properties)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text;
					if ((text = (string)dictionaryEntry.Key) != null)
					{
						if (!(text == "name"))
						{
							if (!(text == "priority"))
							{
								if (text == "secure")
								{
									this._secure = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
								}
							}
							else
							{
								this._channelPriority = Convert.ToInt32(dictionaryEntry.Value, CultureInfo.InvariantCulture);
							}
						}
						else
						{
							this._channelName = (string)dictionaryEntry.Value;
						}
					}
				}
			}
			this._sinkProvider = sinkProvider;
			this.SetupChannel();
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A250 File Offset: 0x00009250
		private void SetupChannel()
		{
			if (this._sinkProvider != null)
			{
				CoreChannel.AppendProviderToClientProviderChain(this._sinkProvider, new TcpClientTransportSinkProvider(this._prop));
				return;
			}
			this._sinkProvider = this.CreateDefaultClientProviderChain();
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000A27D File Offset: 0x0000927D
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000A285 File Offset: 0x00009285
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

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000A28E File Offset: 0x0000928E
		public int ChannelPriority
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelPriority;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000A296 File Offset: 0x00009296
		public string ChannelName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelName;
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000A29E File Offset: 0x0000929E
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public string Parse(string url, out string objectURI)
		{
			return TcpChannelHelper.ParseURL(url, out objectURI);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000A2A8 File Offset: 0x000092A8
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
			IClientChannelSink clientChannelSink = this._sinkProvider.CreateSink(this, url, remoteChannelData);
			IMessageSink messageSink = clientChannelSink as IMessageSink;
			if (clientChannelSink != null && messageSink == null)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Channels_ChannelSinkNotMsgSink"));
			}
			return messageSink;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000A334 File Offset: 0x00009334
		private IClientChannelSinkProvider CreateDefaultClientProviderChain()
		{
			IClientChannelSinkProvider clientChannelSinkProvider = new BinaryClientFormatterSinkProvider();
			IClientChannelSinkProvider clientChannelSinkProvider2 = clientChannelSinkProvider;
			clientChannelSinkProvider2.Next = new TcpClientTransportSinkProvider(this._prop);
			return clientChannelSinkProvider;
		}

		// Token: 0x04000165 RID: 357
		private int _channelPriority = 1;

		// Token: 0x04000166 RID: 358
		private string _channelName = "tcp";

		// Token: 0x04000167 RID: 359
		private bool _secure;

		// Token: 0x04000168 RID: 360
		private IDictionary _prop;

		// Token: 0x04000169 RID: 361
		private IClientChannelSinkProvider _sinkProvider;
	}
}
