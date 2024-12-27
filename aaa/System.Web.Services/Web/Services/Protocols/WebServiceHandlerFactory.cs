using System;
using System.Security.Permissions;
using System.Threading;
using System.Web.Services.Configuration;
using System.Web.Services.Diagnostics;
using System.Web.UI;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200008F RID: 143
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class WebServiceHandlerFactory : IHttpHandlerFactory
	{
		// Token: 0x060003BA RID: 954 RVA: 0x00012B80 File Offset: 0x00011B80
		public IHttpHandler GetHandler(HttpContext context, string verb, string url, string filePath)
		{
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "GetHandler", new object[0]) : null);
			if (Tracing.On)
			{
				Tracing.Enter("IHttpHandlerFactory.GetHandler", traceMethod, Tracing.Details(context.Request));
			}
			new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal).Demand();
			Type compiledType = WebServiceParser.GetCompiledType(url, context);
			IHttpHandler httpHandler = this.CoreGetHandler(compiledType, context, context.Request, context.Response);
			if (Tracing.On)
			{
				Tracing.Exit("IHttpHandlerFactory.GetHandler", traceMethod);
			}
			return httpHandler;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00012C08 File Offset: 0x00011C08
		internal IHttpHandler CoreGetHandler(Type type, HttpContext context, HttpRequest request, HttpResponse response)
		{
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "CoreGetHandler", new object[0]) : null);
			ServerProtocolFactory[] serverProtocolFactories = WebServicesSection.Current.ServerProtocolFactories;
			ServerProtocol serverProtocol = null;
			bool flag = false;
			for (int i = 0; i < serverProtocolFactories.Length; i++)
			{
				try
				{
					serverProtocol = serverProtocolFactories[i].Create(type, context, request, response, out flag);
					if ((serverProtocol != null && serverProtocol.GetType() != typeof(UnsupportedRequestProtocol)) || flag)
					{
						break;
					}
				}
				catch (Exception ex)
				{
					if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
					{
						throw;
					}
					throw Tracing.ExceptionThrow(traceMethod, new InvalidOperationException(Res.GetString("FailedToHandleRequest0"), ex));
				}
				catch
				{
					throw Tracing.ExceptionThrow(traceMethod, new InvalidOperationException(Res.GetString("FailedToHandleRequest0"), null));
				}
			}
			if (flag)
			{
				return new NopHandler();
			}
			if (serverProtocol == null)
			{
				if (request.PathInfo != null && request.PathInfo.Length != 0)
				{
					throw Tracing.ExceptionThrow(traceMethod, new InvalidOperationException(Res.GetString("WebUnrecognizedRequestFormatUrl", new object[] { request.PathInfo })));
				}
				throw Tracing.ExceptionThrow(traceMethod, new InvalidOperationException(Res.GetString("WebUnrecognizedRequestFormat")));
			}
			else
			{
				if (serverProtocol is UnsupportedRequestProtocol)
				{
					throw Tracing.ExceptionThrow(traceMethod, new HttpException(((UnsupportedRequestProtocol)serverProtocol).HttpCode, Res.GetString("WebUnrecognizedRequestFormat")));
				}
				bool isAsync = serverProtocol.MethodInfo.IsAsync;
				bool enableSession = serverProtocol.MethodAttribute.EnableSession;
				if (isAsync)
				{
					if (enableSession)
					{
						return new AsyncSessionHandler(serverProtocol);
					}
					return new AsyncSessionlessHandler(serverProtocol);
				}
				else
				{
					if (enableSession)
					{
						return new SyncSessionHandler(serverProtocol);
					}
					return new SyncSessionlessHandler(serverProtocol);
				}
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00012DBC File Offset: 0x00011DBC
		public void ReleaseHandler(IHttpHandler handler)
		{
		}
	}
}
