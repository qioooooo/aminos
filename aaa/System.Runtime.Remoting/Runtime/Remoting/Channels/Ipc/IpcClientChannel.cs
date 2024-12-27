using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x02000053 RID: 83
	public class IpcClientChannel : IChannelSender, IChannel, ISecurableChannel
	{
		// Token: 0x0600029F RID: 671 RVA: 0x0000D15A File Offset: 0x0000C15A
		public IpcClientChannel()
		{
			this.SetupChannel();
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000D17A File Offset: 0x0000C17A
		public IpcClientChannel(string name, IClientChannelSinkProvider sinkProvider)
		{
			this._channelName = name;
			this._sinkProvider = sinkProvider;
			this.SetupChannel();
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000D1A8 File Offset: 0x0000C1A8
		public IpcClientChannel(IDictionary properties, IClientChannelSinkProvider sinkProvider)
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

		// Token: 0x060002A2 RID: 674 RVA: 0x0000D2AC File Offset: 0x0000C2AC
		private void SetupChannel()
		{
			if (this._sinkProvider != null)
			{
				CoreChannel.AppendProviderToClientProviderChain(this._sinkProvider, new IpcClientTransportSinkProvider(this._prop));
				return;
			}
			this._sinkProvider = this.CreateDefaultClientProviderChain();
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000D2D9 File Offset: 0x0000C2D9
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x0000D2E1 File Offset: 0x0000C2E1
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

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000D2EA File Offset: 0x0000C2EA
		public int ChannelPriority
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelPriority;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000D2F2 File Offset: 0x0000C2F2
		public string ChannelName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelName;
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000D2FA File Offset: 0x0000C2FA
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public string Parse(string url, out string objectURI)
		{
			return IpcChannelHelper.ParseURL(url, out objectURI);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000D304 File Offset: 0x0000C304
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

		// Token: 0x060002A9 RID: 681 RVA: 0x0000D390 File Offset: 0x0000C390
		private IClientChannelSinkProvider CreateDefaultClientProviderChain()
		{
			IClientChannelSinkProvider clientChannelSinkProvider = new BinaryClientFormatterSinkProvider();
			IClientChannelSinkProvider clientChannelSinkProvider2 = clientChannelSinkProvider;
			clientChannelSinkProvider2.Next = new IpcClientTransportSinkProvider(this._prop);
			return clientChannelSinkProvider;
		}

		// Token: 0x040001E5 RID: 485
		private int _channelPriority = 1;

		// Token: 0x040001E6 RID: 486
		private string _channelName = "ipc client";

		// Token: 0x040001E7 RID: 487
		private bool _secure;

		// Token: 0x040001E8 RID: 488
		private IDictionary _prop;

		// Token: 0x040001E9 RID: 489
		private IClientChannelSinkProvider _sinkProvider;
	}
}
