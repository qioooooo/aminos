using System;
using System.Configuration;
using System.Web.Compilation;

namespace System.Web.Configuration
{
	// Token: 0x020001E7 RID: 487
	internal class HandlerFactoryCache
	{
		// Token: 0x06001B02 RID: 6914 RVA: 0x0007CFBC File Offset: 0x0007BFBC
		internal HandlerFactoryCache(string type)
		{
			object obj = this.Create(type);
			if (obj is IHttpHandler)
			{
				this._factory = new HandlerFactoryWrapper((IHttpHandler)obj, this.GetHandlerType(type));
				return;
			}
			if (obj is IHttpHandlerFactory)
			{
				this._factory = (IHttpHandlerFactory)obj;
				return;
			}
			throw new HttpException(SR.GetString("Type_not_factory_or_handler", new object[] { obj.GetType().FullName }));
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x0007D034 File Offset: 0x0007C034
		internal HandlerFactoryCache(HttpHandlerAction mapping)
		{
			object obj = mapping.Create();
			if (obj is IHttpHandler)
			{
				this._factory = new HandlerFactoryWrapper((IHttpHandler)obj, this.GetHandlerType(mapping));
				return;
			}
			if (obj is IHttpHandlerFactory)
			{
				this._factory = (IHttpHandlerFactory)obj;
				return;
			}
			throw new HttpException(SR.GetString("Type_not_factory_or_handler", new object[] { obj.GetType().FullName }));
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001B04 RID: 6916 RVA: 0x0007D0A9 File Offset: 0x0007C0A9
		internal IHttpHandlerFactory Factory
		{
			get
			{
				return this._factory;
			}
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x0007D0B4 File Offset: 0x0007C0B4
		internal Type GetHandlerType(HttpHandlerAction handlerAction)
		{
			Type type = BuildManager.GetType(handlerAction.Type, true, false);
			if (!ConfigUtil.IsTypeHandlerOrFactory(type))
			{
				throw new ConfigurationErrorsException(SR.GetString("Type_not_factory_or_handler", new object[] { handlerAction.Type }), handlerAction.ElementInformation.Source, handlerAction.ElementInformation.LineNumber);
			}
			return type;
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x0007D110 File Offset: 0x0007C110
		internal Type GetHandlerType(string type)
		{
			Type type2 = BuildManager.GetType(type, true, false);
			HttpRuntime.FailIfNoAPTCABit(type2, null, null);
			if (!ConfigUtil.IsTypeHandlerOrFactory(type2))
			{
				throw new ConfigurationErrorsException(SR.GetString("Type_not_factory_or_handler", new object[] { type }));
			}
			return type2;
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x0007D153 File Offset: 0x0007C153
		internal object Create(string type)
		{
			return HttpRuntime.CreateNonPublicInstance(this.GetHandlerType(type));
		}

		// Token: 0x04001811 RID: 6161
		private IHttpHandlerFactory _factory;
	}
}
