using System;

namespace System.Web.Configuration
{
	// Token: 0x020001E8 RID: 488
	internal class HandlerFactoryWrapper : IHttpHandlerFactory
	{
		// Token: 0x06001B08 RID: 6920 RVA: 0x0007D161 File Offset: 0x0007C161
		internal HandlerFactoryWrapper(IHttpHandler handler, Type handlerType)
		{
			this._handler = handler;
			this._handlerType = handlerType;
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0007D177 File Offset: 0x0007C177
		public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
		{
			if (this._handler == null)
			{
				this._handler = (IHttpHandler)HttpRuntime.CreateNonPublicInstance(this._handlerType);
			}
			return this._handler;
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x0007D19D File Offset: 0x0007C19D
		public void ReleaseHandler(IHttpHandler handler)
		{
			if (this._handler != null && !this._handler.IsReusable)
			{
				this._handler = null;
			}
		}

		// Token: 0x04001812 RID: 6162
		private IHttpHandler _handler;

		// Token: 0x04001813 RID: 6163
		private Type _handlerType;
	}
}
