using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000023 RID: 35
	public class HttpChannel : BaseChannelWithProperties, IChannelReceiver, IChannelSender, IChannel, IChannelReceiverHook, ISecurableChannel
	{
		// Token: 0x060000ED RID: 237 RVA: 0x000055BA File Offset: 0x000045BA
		public HttpChannel()
		{
			this._clientChannel = new HttpClientChannel();
			this._serverChannel = new HttpServerChannel();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000055EA File Offset: 0x000045EA
		public HttpChannel(int port)
		{
			this._clientChannel = new HttpClientChannel();
			this._serverChannel = new HttpServerChannel(port);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000561C File Offset: 0x0000461C
		public HttpChannel(IDictionary properties, IClientChannelSinkProvider clientSinkProvider, IServerChannelSinkProvider serverSinkProvider)
		{
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
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
						if (text == "secure")
						{
							this._secure = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
							hashtable["secure"] = dictionaryEntry.Value;
							hashtable2["secure"] = dictionaryEntry.Value;
							continue;
						}
					}
					hashtable[dictionaryEntry.Key] = dictionaryEntry.Value;
					hashtable2[dictionaryEntry.Key] = dictionaryEntry.Value;
				}
			}
			this._clientChannel = new HttpClientChannel(hashtable, clientSinkProvider);
			this._serverChannel = new HttpServerChannel(hashtable2, serverSinkProvider);
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00005790 File Offset: 0x00004790
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x000057BC File Offset: 0x000047BC
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x0000580E File Offset: 0x0000480E
		public int ChannelPriority
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelPriority;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00005816 File Offset: 0x00004816
		public string ChannelName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelName;
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000581E File Offset: 0x0000481E
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public string Parse(string url, out string objectURI)
		{
			return HttpChannelHelper.ParseURL(url, out objectURI);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005827 File Offset: 0x00004827
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public IMessageSink CreateMessageSink(string url, object remoteChannelData, out string objectURI)
		{
			return this._clientChannel.CreateMessageSink(url, remoteChannelData, out objectURI);
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00005837 File Offset: 0x00004837
		public object ChannelData
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._serverChannel.ChannelData;
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005844 File Offset: 0x00004844
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public string[] GetUrlsForUri(string objectURI)
		{
			return this._serverChannel.GetUrlsForUri(objectURI);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005852 File Offset: 0x00004852
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void StartListening(object data)
		{
			this._serverChannel.StartListening(data);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005860 File Offset: 0x00004860
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void StopListening(object data)
		{
			this._serverChannel.StopListening(data);
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000586E File Offset: 0x0000486E
		public string ChannelScheme
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return "http";
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00005875 File Offset: 0x00004875
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00005882 File Offset: 0x00004882
		public bool WantsToListen
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._serverChannel.WantsToListen;
			}
			set
			{
				this._serverChannel.WantsToListen = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00005890 File Offset: 0x00004890
		public IServerChannelSink ChannelSinkChain
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._serverChannel.ChannelSinkChain;
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000589D File Offset: 0x0000489D
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void AddHookChannelUri(string channelUri)
		{
			this._serverChannel.AddHookChannelUri(channelUri);
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000058AC File Offset: 0x000048AC
		public override IDictionary Properties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return new AggregateDictionary(new ArrayList(2)
				{
					this._clientChannel.Properties,
					this._serverChannel.Properties
				});
			}
		}

		// Token: 0x1700002E RID: 46
		public override object this[object key]
		{
			get
			{
				if (this._clientChannel.Contains(key))
				{
					return this._clientChannel[key];
				}
				if (this._serverChannel.Contains(key))
				{
					return this._serverChannel[key];
				}
				return null;
			}
			set
			{
				if (this._clientChannel.Contains(key))
				{
					this._clientChannel[key] = value;
					return;
				}
				if (this._serverChannel.Contains(key))
				{
					this._serverChannel[key] = value;
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000595C File Offset: 0x0000495C
		public override ICollection Keys
		{
			get
			{
				if (HttpChannel.s_keySet == null)
				{
					ICollection keys = this._clientChannel.Keys;
					ICollection keys2 = this._serverChannel.Keys;
					int num = keys.Count + keys2.Count;
					ArrayList arrayList = new ArrayList(num);
					foreach (object obj in keys)
					{
						arrayList.Add(obj);
					}
					foreach (object obj2 in keys2)
					{
						arrayList.Add(obj2);
					}
					HttpChannel.s_keySet = arrayList;
				}
				return HttpChannel.s_keySet;
			}
		}

		// Token: 0x040000CE RID: 206
		private static ICollection s_keySet;

		// Token: 0x040000CF RID: 207
		private HttpClientChannel _clientChannel;

		// Token: 0x040000D0 RID: 208
		private HttpServerChannel _serverChannel;

		// Token: 0x040000D1 RID: 209
		private int _channelPriority = 1;

		// Token: 0x040000D2 RID: 210
		private string _channelName = "http";

		// Token: 0x040000D3 RID: 211
		private bool _secure;
	}
}
