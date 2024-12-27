using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006B9 RID: 1721
	[Serializable]
	internal class CrossAppDomainChannel : IChannelSender, IChannelReceiver, IChannel
	{
		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06003E85 RID: 16005 RVA: 0x000D732F File Offset: 0x000D632F
		// (set) Token: 0x06003E86 RID: 16006 RVA: 0x000D7345 File Offset: 0x000D6345
		private static CrossAppDomainChannel gAppDomainChannel
		{
			get
			{
				return Thread.GetDomain().RemotingData.ChannelServicesData.xadmessageSink;
			}
			set
			{
				Thread.GetDomain().RemotingData.ChannelServicesData.xadmessageSink = value;
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06003E87 RID: 16007 RVA: 0x000D735C File Offset: 0x000D635C
		internal static CrossAppDomainChannel AppDomainChannel
		{
			get
			{
				if (CrossAppDomainChannel.gAppDomainChannel == null)
				{
					CrossAppDomainChannel crossAppDomainChannel = new CrossAppDomainChannel();
					lock (CrossAppDomainChannel.staticSyncObject)
					{
						if (CrossAppDomainChannel.gAppDomainChannel == null)
						{
							CrossAppDomainChannel.gAppDomainChannel = crossAppDomainChannel;
						}
					}
				}
				return CrossAppDomainChannel.gAppDomainChannel;
			}
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x000D73B0 File Offset: 0x000D63B0
		internal static void RegisterChannel()
		{
			CrossAppDomainChannel appDomainChannel = CrossAppDomainChannel.AppDomainChannel;
			ChannelServices.RegisterChannelInternal(appDomainChannel, false);
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06003E89 RID: 16009 RVA: 0x000D73CA File Offset: 0x000D63CA
		public virtual string ChannelName
		{
			get
			{
				return "XAPPDMN";
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06003E8A RID: 16010 RVA: 0x000D73D1 File Offset: 0x000D63D1
		public virtual string ChannelURI
		{
			get
			{
				return "XAPPDMN_URI";
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06003E8B RID: 16011 RVA: 0x000D73D8 File Offset: 0x000D63D8
		public virtual int ChannelPriority
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x000D73DC File Offset: 0x000D63DC
		public string Parse(string url, out string objectURI)
		{
			objectURI = url;
			return null;
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06003E8D RID: 16013 RVA: 0x000D73E2 File Offset: 0x000D63E2
		public virtual object ChannelData
		{
			get
			{
				return new CrossAppDomainData(Context.DefaultContext.InternalContextID, Thread.GetDomain().GetId(), Identity.ProcessGuid);
			}
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x000D7404 File Offset: 0x000D6404
		public virtual IMessageSink CreateMessageSink(string url, object data, out string objectURI)
		{
			objectURI = null;
			IMessageSink messageSink = null;
			if (url != null && data == null)
			{
				if (url.StartsWith("XAPPDMN", StringComparison.Ordinal))
				{
					throw new RemotingException(Environment.GetResourceString("Remoting_AppDomains_NYI"));
				}
			}
			else
			{
				CrossAppDomainData crossAppDomainData = data as CrossAppDomainData;
				if (crossAppDomainData != null && crossAppDomainData.ProcessGuid.Equals(Identity.ProcessGuid))
				{
					messageSink = CrossAppDomainSink.FindOrCreateSink(crossAppDomainData);
				}
			}
			return messageSink;
		}

		// Token: 0x06003E8F RID: 16015 RVA: 0x000D745E File Offset: 0x000D645E
		public virtual string[] GetUrlsForUri(string objectURI)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x06003E90 RID: 16016 RVA: 0x000D746F File Offset: 0x000D646F
		public virtual void StartListening(object data)
		{
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x000D7471 File Offset: 0x000D6471
		public virtual void StopListening(object data)
		{
		}

		// Token: 0x04001FA2 RID: 8098
		private const string _channelName = "XAPPDMN";

		// Token: 0x04001FA3 RID: 8099
		private const string _channelURI = "XAPPDMN_URI";

		// Token: 0x04001FA4 RID: 8100
		private static object staticSyncObject = new object();

		// Token: 0x04001FA5 RID: 8101
		private static PermissionSet s_fullTrust = new PermissionSet(PermissionState.Unrestricted);
	}
}
