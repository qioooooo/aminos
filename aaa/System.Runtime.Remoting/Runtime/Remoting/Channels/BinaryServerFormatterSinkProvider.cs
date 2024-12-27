using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000063 RID: 99
	public class BinaryServerFormatterSinkProvider : IServerFormatterSinkProvider, IServerChannelSinkProvider
	{
		// Token: 0x06000324 RID: 804 RVA: 0x0000EB7C File Offset: 0x0000DB7C
		public BinaryServerFormatterSinkProvider()
		{
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000EB94 File Offset: 0x0000DB94
		public BinaryServerFormatterSinkProvider(IDictionary properties, ICollection providerData)
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

		// Token: 0x06000326 RID: 806 RVA: 0x0000ECAC File Offset: 0x0000DCAC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void GetChannelData(IChannelDataStore channelData)
		{
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000ECB0 File Offset: 0x0000DCB0
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
			BinaryServerFormatterSink.Protocol protocol = BinaryServerFormatterSink.Protocol.Other;
			string text = channel.GetUrlsForUri("")[0];
			if (string.Compare("http", 0, text, 0, 4, StringComparison.OrdinalIgnoreCase) == 0)
			{
				protocol = BinaryServerFormatterSink.Protocol.Http;
			}
			return new BinaryServerFormatterSink(protocol, serverChannelSink, channel)
			{
				TypeFilterLevel = this._formatterSecurityLevel,
				IncludeVersioning = this._includeVersioning,
				StrictBinding = this._strictBinding
			};
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000ED33 File Offset: 0x0000DD33
		// (set) Token: 0x06000329 RID: 809 RVA: 0x0000ED3B File Offset: 0x0000DD3B
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

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000ED44 File Offset: 0x0000DD44
		// (set) Token: 0x0600032B RID: 811 RVA: 0x0000ED4C File Offset: 0x0000DD4C
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

		// Token: 0x04000251 RID: 593
		private IServerChannelSinkProvider _next;

		// Token: 0x04000252 RID: 594
		private bool _includeVersioning = true;

		// Token: 0x04000253 RID: 595
		private bool _strictBinding;

		// Token: 0x04000254 RID: 596
		private TypeFilterLevel _formatterSecurityLevel = TypeFilterLevel.Low;
	}
}
