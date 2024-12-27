using System;

namespace System.Web.Configuration
{
	// Token: 0x020001EA RID: 490
	internal class HandlerWithFactory
	{
		// Token: 0x06001B0E RID: 6926 RVA: 0x0007D1FE File Offset: 0x0007C1FE
		internal HandlerWithFactory(IHttpHandler handler, IHttpHandlerFactory factory)
		{
			this._handler = handler;
			this._factory = factory;
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x0007D214 File Offset: 0x0007C214
		internal void Recycle()
		{
			this._factory.ReleaseHandler(this._handler);
		}

		// Token: 0x04001817 RID: 6167
		private IHttpHandler _handler;

		// Token: 0x04001818 RID: 6168
		private IHttpHandlerFactory _factory;
	}
}
