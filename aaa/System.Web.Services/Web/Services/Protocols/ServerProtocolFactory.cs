using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Threading;
using System.Web.Services.Diagnostics;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200002E RID: 46
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class ServerProtocolFactory
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x00004670 File Offset: 0x00003670
		internal ServerProtocol Create(Type type, HttpContext context, HttpRequest request, HttpResponse response, out bool abortProcessing)
		{
			ServerProtocol serverProtocol = null;
			abortProcessing = false;
			serverProtocol = this.CreateIfRequestCompatible(request);
			ServerProtocol serverProtocol2;
			try
			{
				if (serverProtocol != null)
				{
					serverProtocol.SetContext(type, context, request, response);
				}
				serverProtocol2 = serverProtocol;
			}
			catch (Exception ex)
			{
				abortProcessing = true;
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, this, "Create", ex);
				}
				if (serverProtocol != null && !serverProtocol.WriteException(ex, serverProtocol.Response.OutputStream))
				{
					throw new InvalidOperationException(Res.GetString("UnableToHandleRequest0"), ex);
				}
				serverProtocol2 = null;
			}
			catch
			{
				abortProcessing = true;
				if (serverProtocol != null && !serverProtocol.WriteException(new Exception(Res.GetString("NonClsCompliantException")), serverProtocol.Response.OutputStream))
				{
					throw new InvalidOperationException(Res.GetString("UnableToHandleRequest0"), null);
				}
				serverProtocol2 = null;
			}
			return serverProtocol2;
		}

		// Token: 0x060000F9 RID: 249
		protected abstract ServerProtocol CreateIfRequestCompatible(HttpRequest request);
	}
}
