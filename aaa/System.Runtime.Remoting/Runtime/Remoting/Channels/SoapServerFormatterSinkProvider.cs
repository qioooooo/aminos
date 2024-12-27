using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000068 RID: 104
	public class SoapServerFormatterSinkProvider : IServerFormatterSinkProvider, IServerChannelSinkProvider
	{
		// Token: 0x0600034B RID: 843 RVA: 0x0000F7E8 File Offset: 0x0000E7E8
		public SoapServerFormatterSinkProvider()
		{
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000F800 File Offset: 0x0000E800
		public SoapServerFormatterSinkProvider(IDictionary properties, ICollection providerData)
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
							if (!(text2 == "strictBinding"))
							{
								if (text2 == "typeFilterLevel")
								{
									this._formatterSecurityLevel = (TypeFilterLevel)Enum.Parse(typeof(TypeFilterLevel), (string)dictionaryEntry.Value);
								}
							}
							else
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

		// Token: 0x0600034D RID: 845 RVA: 0x0000F918 File Offset: 0x0000E918
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void GetChannelData(IChannelDataStore channelData)
		{
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000F91C File Offset: 0x0000E91C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public IServerChannelSink CreateSink(IChannelReceiver channel)
		{
			if (channel == null)
			{
				throw new ArgumentNullException("channel");
			}
			IServerChannelSink serverChannelSink = null;
			if (this._next != null)
			{
				serverChannelSink = this._next.CreateSink(channel);
			}
			SoapServerFormatterSink.Protocol protocol = SoapServerFormatterSink.Protocol.Other;
			string text = channel.GetUrlsForUri("")[0];
			if (string.Compare("http", 0, text, 0, 4, StringComparison.OrdinalIgnoreCase) == 0)
			{
				protocol = SoapServerFormatterSink.Protocol.Http;
			}
			return new SoapServerFormatterSink(protocol, serverChannelSink, channel)
			{
				IncludeVersioning = this._includeVersioning,
				StrictBinding = this._strictBinding,
				TypeFilterLevel = this._formatterSecurityLevel
			};
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0000F99F File Offset: 0x0000E99F
		// (set) Token: 0x06000350 RID: 848 RVA: 0x0000F9A7 File Offset: 0x0000E9A7
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

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000F9B0 File Offset: 0x0000E9B0
		// (set) Token: 0x06000352 RID: 850 RVA: 0x0000F9B8 File Offset: 0x0000E9B8
		[ComVisible(false)]
		public TypeFilterLevel TypeFilterLevel
		{
			get
			{
				return this._formatterSecurityLevel;
			}
			set
			{
				this._formatterSecurityLevel = value;
			}
		}

		// Token: 0x04000266 RID: 614
		private IServerChannelSinkProvider _next;

		// Token: 0x04000267 RID: 615
		private bool _includeVersioning = true;

		// Token: 0x04000268 RID: 616
		private bool _strictBinding;

		// Token: 0x04000269 RID: 617
		private TypeFilterLevel _formatterSecurityLevel = TypeFilterLevel.Low;
	}
}
