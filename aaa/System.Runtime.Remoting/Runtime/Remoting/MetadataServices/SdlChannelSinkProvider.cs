using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting.Channels;
using System.Security.Permissions;

namespace System.Runtime.Remoting.MetadataServices
{
	// Token: 0x0200006E RID: 110
	public class SdlChannelSinkProvider : IServerChannelSinkProvider
	{
		// Token: 0x06000375 RID: 885 RVA: 0x000107D3 File Offset: 0x0000F7D3
		public SdlChannelSinkProvider()
		{
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000107E4 File Offset: 0x0000F7E4
		public SdlChannelSinkProvider(IDictionary properties, ICollection providerData)
		{
			if (properties != null)
			{
				foreach (object obj in properties)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text;
					if ((text = (string)dictionaryEntry.Key) != null)
					{
						if (text == "remoteApplicationMetadataEnabled")
						{
							this._bRemoteApplicationMetadataEnabled = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
							continue;
						}
						if (text == "metadataEnabled")
						{
							this._bMetadataEnabled = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
							continue;
						}
					}
					CoreChannel.ReportUnknownProviderConfigProperty(base.GetType().Name, (string)dictionaryEntry.Key);
				}
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000108C4 File Offset: 0x0000F8C4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void GetChannelData(IChannelDataStore localChannelData)
		{
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000108C8 File Offset: 0x0000F8C8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public IServerChannelSink CreateSink(IChannelReceiver channel)
		{
			IServerChannelSink serverChannelSink = null;
			if (this._next != null)
			{
				serverChannelSink = this._next.CreateSink(channel);
			}
			return new SdlChannelSink(channel, serverChannelSink)
			{
				RemoteApplicationMetadataEnabled = this._bRemoteApplicationMetadataEnabled,
				MetadataEnabled = this._bMetadataEnabled
			};
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000379 RID: 889 RVA: 0x0001090D File Offset: 0x0000F90D
		// (set) Token: 0x0600037A RID: 890 RVA: 0x00010915 File Offset: 0x0000F915
		public IServerChannelSinkProvider Next
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._next;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			set
			{
				this._next = value;
			}
		}

		// Token: 0x0400027E RID: 638
		private IServerChannelSinkProvider _next;

		// Token: 0x0400027F RID: 639
		private bool _bRemoteApplicationMetadataEnabled;

		// Token: 0x04000280 RID: 640
		private bool _bMetadataEnabled = true;
	}
}
