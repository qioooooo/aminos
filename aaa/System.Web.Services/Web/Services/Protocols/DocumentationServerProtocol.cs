using System;
using System.IO;
using System.Threading;
using System.Web.Services.Configuration;
using System.Web.Services.Diagnostics;
using System.Web.UI;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000034 RID: 52
	internal sealed class DocumentationServerProtocol : ServerProtocol
	{
		// Token: 0x0600012A RID: 298 RVA: 0x00004FAC File Offset: 0x00003FAC
		internal override bool Initialize()
		{
			this.serverType = (DocumentationServerType)base.GetFromCache(typeof(DocumentationServerProtocol), base.Type);
			if (this.serverType == null)
			{
				lock (ServerProtocol.InternalSyncObject)
				{
					this.serverType = (DocumentationServerType)base.GetFromCache(typeof(DocumentationServerProtocol), base.Type);
					if (this.serverType == null)
					{
						string text = base.Request.Url.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped).Replace("#", "%23");
						this.serverType = new DocumentationServerType(base.Type, text);
						base.AddToCache(typeof(DocumentationServerProtocol), base.Type, this.serverType);
					}
				}
			}
			WebServicesSection webServicesSection = WebServicesSection.Current;
			if (webServicesSection.WsdlHelpGenerator.Href != null && webServicesSection.WsdlHelpGenerator.Href.Length > 0)
			{
				TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "Initialize", new object[0]) : null);
				if (Tracing.On)
				{
					Tracing.Enter("ASP.NET", traceMethod, new TraceMethod(typeof(PageParser), "GetCompiledPageInstance", new object[]
					{
						webServicesSection.WsdlHelpGenerator.HelpGeneratorVirtualPath,
						webServicesSection.WsdlHelpGenerator.HelpGeneratorPath,
						base.Context
					}));
				}
				this.handler = PageParser.GetCompiledPageInstance(webServicesSection.WsdlHelpGenerator.HelpGeneratorVirtualPath, webServicesSection.WsdlHelpGenerator.HelpGeneratorPath, base.Context);
				if (Tracing.On)
				{
					Tracing.Exit("ASP.NET", traceMethod);
				}
			}
			return true;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00005160 File Offset: 0x00004160
		internal override ServerType ServerType
		{
			get
			{
				return this.serverType;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00005168 File Offset: 0x00004168
		internal override bool IsOneWay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600012D RID: 301 RVA: 0x0000516B File Offset: 0x0000416B
		internal override LogicalMethodInfo MethodInfo
		{
			get
			{
				return this.serverType.MethodInfo;
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005178 File Offset: 0x00004178
		internal override object[] ReadParameters()
		{
			return new object[0];
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005180 File Offset: 0x00004180
		internal override void WriteReturns(object[] returnValues, Stream outputStream)
		{
			try
			{
				if (this.handler != null)
				{
					base.Context.Items.Add("wsdls", this.serverType.ServiceDescriptions);
					base.Context.Items.Add("schemas", this.serverType.Schemas);
					string text = base.Context.Request.ServerVariables["LOCAL_ADDR"];
					string text2 = base.Context.Request.ServerVariables["REMOTE_ADDR"];
					if (base.Context.Request.Url.IsLoopback || (text != null && text2 != null && text == text2))
					{
						base.Context.Items.Add("wsdlsWithPost", this.serverType.ServiceDescriptionsWithPost);
						base.Context.Items.Add("schemasWithPost", this.serverType.SchemasWithPost);
					}
					base.Context.Items.Add("conformanceWarnings", WebServicesSection.Current.EnabledConformanceWarnings);
					base.Response.ContentType = "text/html";
					this.handler.ProcessRequest(base.Context);
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new InvalidOperationException(Res.GetString("HelpGeneratorInternalError"), ex);
			}
			catch
			{
				throw new InvalidOperationException(Res.GetString("HelpGeneratorInternalError"), null);
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005330 File Offset: 0x00004330
		internal override bool WriteException(Exception e, Stream outputStream)
		{
			return false;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005333 File Offset: 0x00004333
		internal void Documentation()
		{
		}

		// Token: 0x04000278 RID: 632
		private const int MAX_PATH_SIZE = 1024;

		// Token: 0x04000279 RID: 633
		private DocumentationServerType serverType;

		// Token: 0x0400027A RID: 634
		private IHttpHandler handler;
	}
}
