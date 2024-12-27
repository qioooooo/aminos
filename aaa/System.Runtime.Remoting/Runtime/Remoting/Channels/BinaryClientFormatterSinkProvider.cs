using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000061 RID: 97
	public class BinaryClientFormatterSinkProvider : IClientFormatterSinkProvider, IClientChannelSinkProvider
	{
		// Token: 0x06000310 RID: 784 RVA: 0x0000E7DD File Offset: 0x0000D7DD
		public BinaryClientFormatterSinkProvider()
		{
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000E7EC File Offset: 0x0000D7EC
		public BinaryClientFormatterSinkProvider(IDictionary properties, ICollection providerData)
		{
			if (properties != null)
			{
				foreach (object obj in properties)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text = dictionaryEntry.Key.ToString();
					string text2;
					if ((text2 = text) != null)
					{
						if (!(text2 == "includeVersions"))
						{
							if (text2 == "strictBinding")
							{
								this._strictBinding = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
							}
						}
						else
						{
							this._includeVersioning = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						}
					}
				}
			}
			CoreChannel.VerifyNoProviderData(base.GetType().Name, providerData);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000E8C0 File Offset: 0x0000D8C0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
		{
			IClientChannelSink clientChannelSink = null;
			if (this._next != null)
			{
				clientChannelSink = this._next.CreateSink(channel, url, remoteChannelData);
				if (clientChannelSink == null)
				{
					return null;
				}
			}
			SinkChannelProtocol sinkChannelProtocol = CoreChannel.DetermineChannelProtocol(channel);
			return new BinaryClientFormatterSink(clientChannelSink)
			{
				IncludeVersioning = this._includeVersioning,
				StrictBinding = this._strictBinding,
				ChannelProtocol = sinkChannelProtocol
			};
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000E919 File Offset: 0x0000D919
		// (set) Token: 0x06000314 RID: 788 RVA: 0x0000E921 File Offset: 0x0000D921
		public IClientChannelSinkProvider Next
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

		// Token: 0x0400024A RID: 586
		private IClientChannelSinkProvider _next;

		// Token: 0x0400024B RID: 587
		private bool _includeVersioning = true;

		// Token: 0x0400024C RID: 588
		private bool _strictBinding;
	}
}
