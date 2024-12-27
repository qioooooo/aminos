using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000712 RID: 1810
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebPartConnectionsCancelEventArgs : CancelEventArgs
	{
		// Token: 0x0600582A RID: 22570 RVA: 0x001633DB File Offset: 0x001623DB
		public WebPartConnectionsCancelEventArgs(WebPart provider, ProviderConnectionPoint providerConnectionPoint, WebPart consumer, ConsumerConnectionPoint consumerConnectionPoint)
		{
			this._provider = provider;
			this._providerConnectionPoint = providerConnectionPoint;
			this._consumer = consumer;
			this._consumerConnectionPoint = consumerConnectionPoint;
		}

		// Token: 0x0600582B RID: 22571 RVA: 0x00163400 File Offset: 0x00162400
		public WebPartConnectionsCancelEventArgs(WebPart provider, ProviderConnectionPoint providerConnectionPoint, WebPart consumer, ConsumerConnectionPoint consumerConnectionPoint, WebPartConnection connection)
			: this(provider, providerConnectionPoint, consumer, consumerConnectionPoint)
		{
			this._connection = connection;
		}

		// Token: 0x170016C7 RID: 5831
		// (get) Token: 0x0600582C RID: 22572 RVA: 0x00163415 File Offset: 0x00162415
		public WebPartConnection Connection
		{
			get
			{
				return this._connection;
			}
		}

		// Token: 0x170016C8 RID: 5832
		// (get) Token: 0x0600582D RID: 22573 RVA: 0x0016341D File Offset: 0x0016241D
		public WebPart Consumer
		{
			get
			{
				return this._consumer;
			}
		}

		// Token: 0x170016C9 RID: 5833
		// (get) Token: 0x0600582E RID: 22574 RVA: 0x00163425 File Offset: 0x00162425
		public ConsumerConnectionPoint ConsumerConnectionPoint
		{
			get
			{
				return this._consumerConnectionPoint;
			}
		}

		// Token: 0x170016CA RID: 5834
		// (get) Token: 0x0600582F RID: 22575 RVA: 0x0016342D File Offset: 0x0016242D
		public WebPart Provider
		{
			get
			{
				return this._provider;
			}
		}

		// Token: 0x170016CB RID: 5835
		// (get) Token: 0x06005830 RID: 22576 RVA: 0x00163435 File Offset: 0x00162435
		public ProviderConnectionPoint ProviderConnectionPoint
		{
			get
			{
				return this._providerConnectionPoint;
			}
		}

		// Token: 0x04002FD3 RID: 12243
		private WebPart _provider;

		// Token: 0x04002FD4 RID: 12244
		private ProviderConnectionPoint _providerConnectionPoint;

		// Token: 0x04002FD5 RID: 12245
		private WebPart _consumer;

		// Token: 0x04002FD6 RID: 12246
		private ConsumerConnectionPoint _consumerConnectionPoint;

		// Token: 0x04002FD7 RID: 12247
		private WebPartConnection _connection;
	}
}
