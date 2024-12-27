using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x0200004F RID: 79
	public class IpcChannel : IChannelReceiver, IChannelSender, IChannel, ISecurableChannel
	{
		// Token: 0x06000273 RID: 627 RVA: 0x0000C5CB File Offset: 0x0000B5CB
		public IpcChannel()
		{
			this._clientChannel = new IpcClientChannel();
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000C5F1 File Offset: 0x0000B5F1
		public IpcChannel(string portName)
			: this()
		{
			this._serverChannel = new IpcServerChannel(portName);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000C608 File Offset: 0x0000B608
		public IpcChannel(IDictionary properties, IClientChannelSinkProvider clientSinkProvider, IServerChannelSinkProvider serverSinkProvider)
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
						if (text == "portName")
						{
							hashtable2["portName"] = dictionaryEntry.Value;
							flag = true;
							continue;
						}
					}
					hashtable[dictionaryEntry.Key] = dictionaryEntry.Value;
					hashtable2[dictionaryEntry.Key] = dictionaryEntry.Value;
				}
			}
			this._clientChannel = new IpcClientChannel(hashtable, clientSinkProvider);
			if (flag)
			{
				this._serverChannel = new IpcServerChannel(hashtable2, serverSinkProvider, null);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000C75C File Offset: 0x0000B75C
		// (set) Token: 0x06000277 RID: 631 RVA: 0x0000C788 File Offset: 0x0000B788
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

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000C7DA File Offset: 0x0000B7DA
		public int ChannelPriority
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelPriority;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000C7E2 File Offset: 0x0000B7E2
		public string ChannelName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelName;
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000C7EA File Offset: 0x0000B7EA
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public string Parse(string url, out string objectURI)
		{
			return IpcChannelHelper.ParseURL(url, out objectURI);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000C7F3 File Offset: 0x0000B7F3
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public IMessageSink CreateMessageSink(string url, object remoteChannelData, out string objectURI)
		{
			return this._clientChannel.CreateMessageSink(url, remoteChannelData, out objectURI);
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000C803 File Offset: 0x0000B803
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

		// Token: 0x0600027D RID: 637 RVA: 0x0000C81A File Offset: 0x0000B81A
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public string[] GetUrlsForUri(string objectURI)
		{
			if (this._serverChannel != null)
			{
				return this._serverChannel.GetUrlsForUri(objectURI);
			}
			return null;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000C832 File Offset: 0x0000B832
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void StartListening(object data)
		{
			if (this._serverChannel != null)
			{
				this._serverChannel.StartListening(data);
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000C848 File Offset: 0x0000B848
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void StopListening(object data)
		{
			if (this._serverChannel != null)
			{
				this._serverChannel.StopListening(data);
			}
		}

		// Token: 0x040001CB RID: 459
		private IpcClientChannel _clientChannel;

		// Token: 0x040001CC RID: 460
		private IpcServerChannel _serverChannel;

		// Token: 0x040001CD RID: 461
		private int _channelPriority = 20;

		// Token: 0x040001CE RID: 462
		private string _channelName = "ipc";
	}
}
