using System;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000018 RID: 24
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DefaultHttpHandler : IHttpAsyncHandler, IHttpHandler
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002F78 File Offset: 0x00001F78
		protected HttpContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002F80 File Offset: 0x00001F80
		protected NameValueCollection ExecuteUrlHeaders
		{
			get
			{
				if (this._executeUrlHeaders == null && this._context != null)
				{
					this._executeUrlHeaders = new NameValueCollection(this._context.Request.Headers);
				}
				return this._executeUrlHeaders;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002FB3 File Offset: 0x00001FB3
		public virtual void OnExecuteUrlPreconditionFailure()
		{
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002FB5 File Offset: 0x00001FB5
		public virtual string OverrideExecuteUrlPath()
		{
			return null;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002FB8 File Offset: 0x00001FB8
		internal static bool IsClassicAspRequest(string filePath)
		{
			return StringUtil.StringEndsWithIgnoreCase(filePath, ".asp");
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002FC8 File Offset: 0x00001FC8
		public virtual IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback callback, object state)
		{
			if (HttpRuntime.UseIntegratedPipeline)
			{
				throw new PlatformNotSupportedException(SR.GetString("Method_Not_Supported_By_Iis_Integrated_Mode", new object[] { "DefaultHttpHandler.BeginProcessRequest" }));
			}
			this._context = context;
			HttpResponse response = this._context.Response;
			if (response.CanExecuteUrlForEntireResponse)
			{
				string text = this.OverrideExecuteUrlPath();
				if (text != null && !HttpRuntime.IsFullTrust && !base.GetType().Assembly.GlobalAssemblyCache)
				{
					HttpRuntime.CheckFilePermission(context.Request.MapPath(text));
				}
				return response.BeginExecuteUrlForEntireResponse(text, this._executeUrlHeaders, callback, state);
			}
			this.OnExecuteUrlPreconditionFailure();
			this._context = null;
			HttpRequest request = context.Request;
			if (request.HttpVerb == HttpVerb.POST)
			{
				throw new HttpException(405, SR.GetString("Method_not_allowed", new object[] { request.HttpMethod, request.Path }));
			}
			if (DefaultHttpHandler.IsClassicAspRequest(request.FilePath))
			{
				throw new HttpException(403, SR.GetString("Path_forbidden", new object[] { request.Path }));
			}
			StaticFileHandler.ProcessRequestInternal(context);
			return new HttpAsyncResult(callback, state, true, null, null);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000030F4 File Offset: 0x000020F4
		public virtual void EndProcessRequest(IAsyncResult result)
		{
			if (this._context != null)
			{
				HttpResponse response = this._context.Response;
				this._context = null;
				response.EndExecuteUrlForEntireResponse(result);
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003123 File Offset: 0x00002123
		public virtual void ProcessRequest(HttpContext context)
		{
			throw new InvalidOperationException(SR.GetString("Cannot_call_defaulthttphandler_sync"));
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003134 File Offset: 0x00002134
		public virtual bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000D10 RID: 3344
		private HttpContext _context;

		// Token: 0x04000D11 RID: 3345
		private NameValueCollection _executeUrlHeaders;
	}
}
