using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000066 RID: 102
	public class SoapClientFormatterSinkProvider : IClientFormatterSinkProvider, IClientChannelSinkProvider
	{
		// Token: 0x06000337 RID: 823 RVA: 0x0000F342 File Offset: 0x0000E342
		public SoapClientFormatterSinkProvider()
		{
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000F354 File Offset: 0x0000E354
		public SoapClientFormatterSinkProvider(IDictionary properties, ICollection providerData)
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

		// Token: 0x06000339 RID: 825 RVA: 0x0000F428 File Offset: 0x0000E428
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
			return new SoapClientFormatterSink(clientChannelSink)
			{
				IncludeVersioning = this._includeVersioning,
				StrictBinding = this._strictBinding,
				ChannelProtocol = sinkChannelProtocol
			};
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000F481 File Offset: 0x0000E481
		// (set) Token: 0x0600033B RID: 827 RVA: 0x0000F489 File Offset: 0x0000E489
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

		// Token: 0x0400025F RID: 607
		private IClientChannelSinkProvider _next;

		// Token: 0x04000260 RID: 608
		private bool _includeVersioning = true;

		// Token: 0x04000261 RID: 609
		private bool _strictBinding;
	}
}
