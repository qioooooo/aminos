using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000719 RID: 1817
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebPartConnectionsEventArgs : EventArgs
	{
		// Token: 0x0600584E RID: 22606 RVA: 0x00163735 File Offset: 0x00162735
		public WebPartConnectionsEventArgs(WebPart provider, ProviderConnectionPoint providerConnectionPoint, WebPart consumer, ConsumerConnectionPoint consumerConnectionPoint)
		{
			this._provider = provider;
			this._providerConnectionPoint = providerConnectionPoint;
			this._consumer = consumer;
			this._consumerConnectionPoint = consumerConnectionPoint;
		}

		// Token: 0x0600584F RID: 22607 RVA: 0x0016375A File Offset: 0x0016275A
		public WebPartConnectionsEventArgs(WebPart provider, ProviderConnectionPoint providerConnectionPoint, WebPart consumer, ConsumerConnectionPoint consumerConnectionPoint, WebPartConnection connection)
			: this(provider, providerConnectionPoint, consumer, consumerConnectionPoint)
		{
			this._connection = connection;
		}

		// Token: 0x170016D6 RID: 5846
		// (get) Token: 0x06005850 RID: 22608 RVA: 0x0016376F File Offset: 0x0016276F
		public WebPartConnection Connection
		{
			get
			{
				return this._connection;
			}
		}

		// Token: 0x170016D7 RID: 5847
		// (get) Token: 0x06005851 RID: 22609 RVA: 0x00163777 File Offset: 0x00162777
		public WebPart Consumer
		{
			get
			{
				return this._consumer;
			}
		}

		// Token: 0x170016D8 RID: 5848
		// (get) Token: 0x06005852 RID: 22610 RVA: 0x0016377F File Offset: 0x0016277F
		public ConsumerConnectionPoint ConsumerConnectionPoint
		{
			get
			{
				return this._consumerConnectionPoint;
			}
		}

		// Token: 0x170016D9 RID: 5849
		// (get) Token: 0x06005853 RID: 22611 RVA: 0x00163787 File Offset: 0x00162787
		public WebPart Provider
		{
			get
			{
				return this._provider;
			}
		}

		// Token: 0x170016DA RID: 5850
		// (get) Token: 0x06005854 RID: 22612 RVA: 0x0016378F File Offset: 0x0016278F
		public ProviderConnectionPoint ProviderConnectionPoint
		{
			get
			{
				return this._providerConnectionPoint;
			}
		}

		// Token: 0x04002FD8 RID: 12248
		private WebPart _provider;

		// Token: 0x04002FD9 RID: 12249
		private ProviderConnectionPoint _providerConnectionPoint;

		// Token: 0x04002FDA RID: 12250
		private WebPart _consumer;

		// Token: 0x04002FDB RID: 12251
		private ConsumerConnectionPoint _consumerConnectionPoint;

		// Token: 0x04002FDC RID: 12252
		private WebPartConnection _connection;
	}
}
