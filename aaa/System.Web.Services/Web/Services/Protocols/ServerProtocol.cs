using System;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Web.Caching;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000030 RID: 48
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class ServerProtocol
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00004804 File Offset: 0x00003804
		internal static object InternalSyncObject
		{
			get
			{
				if (ServerProtocol.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref ServerProtocol.s_InternalSyncObject, obj, null);
				}
				return ServerProtocol.s_InternalSyncObject;
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004830 File Offset: 0x00003830
		internal void SetContext(Type type, HttpContext context, HttpRequest request, HttpResponse response)
		{
			this.type = type;
			this.context = context;
			this.request = request;
			this.response = response;
			this.Initialize();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004858 File Offset: 0x00003858
		internal virtual void CreateServerInstance()
		{
			this.target = Activator.CreateInstance(this.ServerType.Type);
			WebService webService = this.target as WebService;
			if (webService != null)
			{
				webService.SetContext(this.context);
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004898 File Offset: 0x00003898
		internal virtual void DisposeServerInstance()
		{
			if (this.target == null)
			{
				return;
			}
			IDisposable disposable = this.target as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			this.target = null;
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000048CA File Offset: 0x000038CA
		protected internal HttpContext Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000102 RID: 258 RVA: 0x000048D2 File Offset: 0x000038D2
		protected internal HttpRequest Request
		{
			get
			{
				return this.request;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000048DA File Offset: 0x000038DA
		protected internal HttpResponse Response
		{
			get
			{
				return this.response;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000048E2 File Offset: 0x000038E2
		internal Type Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000048EA File Offset: 0x000038EA
		protected internal virtual object Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000048F2 File Offset: 0x000038F2
		internal virtual bool WriteException(Exception e, Stream outputStream)
		{
			return false;
		}

		// Token: 0x06000107 RID: 263
		internal abstract bool Initialize();

		// Token: 0x06000108 RID: 264
		internal abstract object[] ReadParameters();

		// Token: 0x06000109 RID: 265
		internal abstract void WriteReturns(object[] returns, Stream outputStream);

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600010A RID: 266
		internal abstract LogicalMethodInfo MethodInfo { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600010B RID: 267
		internal abstract ServerType ServerType { get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600010C RID: 268
		internal abstract bool IsOneWay { get; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000048F5 File Offset: 0x000038F5
		internal virtual Exception OnewayInitException
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000048F8 File Offset: 0x000038F8
		internal WebMethodAttribute MethodAttribute
		{
			get
			{
				if (this.methodAttr == null)
				{
					this.methodAttr = this.MethodInfo.MethodAttribute;
				}
				return this.methodAttr;
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004919 File Offset: 0x00003919
		internal string GenerateFaultString(Exception e)
		{
			return this.GenerateFaultString(e, false);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004923 File Offset: 0x00003923
		internal static void SetHttpResponseStatusCode(HttpResponse httpResponse, int statusCode)
		{
			httpResponse.TrySkipIisCustomErrors = true;
			httpResponse.StatusCode = statusCode;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004934 File Offset: 0x00003934
		internal string GenerateFaultString(Exception e, bool htmlEscapeMessage)
		{
			bool flag = this.Context != null && !this.Context.IsCustomErrorEnabled;
			if (flag && !htmlEscapeMessage)
			{
				return e.ToString();
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (flag)
			{
				ServerProtocol.GenerateFaultString(e, stringBuilder);
			}
			else
			{
				for (Exception ex = e; ex != null; ex = ex.InnerException)
				{
					string text = (htmlEscapeMessage ? HttpUtility.HtmlEncode(ex.Message) : ex.Message);
					if (text.Length == 0)
					{
						text = e.GetType().Name;
					}
					stringBuilder.Append(text);
					if (ex.InnerException != null)
					{
						stringBuilder.Append(" ---> ");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000049D8 File Offset: 0x000039D8
		private static void GenerateFaultString(Exception e, StringBuilder builder)
		{
			builder.Append(e.GetType().FullName);
			if (e.Message != null && e.Message.Length > 0)
			{
				builder.Append(": ");
				builder.Append(HttpUtility.HtmlEncode(e.Message));
			}
			if (e.InnerException != null)
			{
				builder.Append(" ---> ");
				ServerProtocol.GenerateFaultString(e.InnerException, builder);
				builder.Append(Environment.NewLine);
				builder.Append("   ");
				builder.Append(Res.GetString("StackTraceEnd"));
			}
			if (e.StackTrace != null)
			{
				builder.Append(Environment.NewLine);
				builder.Append(e.StackTrace);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004A95 File Offset: 0x00003A95
		internal void WriteOneWayResponse()
		{
			this.context.Response.ContentType = null;
			this.Response.StatusCode = 202;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004AB8 File Offset: 0x00003AB8
		private string CreateKey(Type protocolType, Type serverType)
		{
			string fullName = protocolType.FullName;
			string fullName2 = serverType.FullName;
			string text = serverType.TypeHandle.Value.ToString();
			string urlKeyComponent = this.GetUrlKeyComponent();
			int num = fullName.Length + urlKeyComponent.Length + fullName2.Length + text.Length + 2;
			StringBuilder stringBuilder = new StringBuilder(num);
			stringBuilder.Append(fullName);
			stringBuilder.Append('[');
			stringBuilder.Append(urlKeyComponent);
			stringBuilder.Append(']');
			stringBuilder.Append(fullName2);
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004B60 File Offset: 0x00003B60
		internal virtual string GetUrlKeyComponent()
		{
			return this.Request.Url.GetLeftPart(UriPartial.Path);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004B73 File Offset: 0x00003B73
		protected void AddToCache(Type protocolType, Type serverType, object value)
		{
			HttpRuntime.Cache.Insert(this.CreateKey(protocolType, serverType), value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004B95 File Offset: 0x00003B95
		protected object GetFromCache(Type protocolType, Type serverType)
		{
			return HttpRuntime.Cache.Get(this.CreateKey(protocolType, serverType));
		}

		// Token: 0x0400026B RID: 619
		private Type type;

		// Token: 0x0400026C RID: 620
		private HttpRequest request;

		// Token: 0x0400026D RID: 621
		private HttpResponse response;

		// Token: 0x0400026E RID: 622
		private HttpContext context;

		// Token: 0x0400026F RID: 623
		private object target;

		// Token: 0x04000270 RID: 624
		private WebMethodAttribute methodAttr;

		// Token: 0x04000271 RID: 625
		private static object s_InternalSyncObject;
	}
}
