using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web.Services.Protocols;
using System.Web.SessionState;

namespace System.Web.Services
{
	// Token: 0x02000014 RID: 20
	public class WebService : MarshalByValueComponent
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000029A2 File Offset: 0x000019A2
		[Description("The ASP.NET application object for the current request.")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HttpApplicationState Application
		{
			[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
			get
			{
				return this.Context.Application;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000044 RID: 68 RVA: 0x000029AF File Offset: 0x000019AF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebServicesDescription("WebServiceContext")]
		[Browsable(false)]
		public HttpContext Context
		{
			[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
			get
			{
				if (this.context == null)
				{
					this.context = HttpContext.Current;
				}
				if (this.context == null)
				{
					throw new InvalidOperationException(Res.GetString("WebMissingHelpContext"));
				}
				return this.context;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000029E2 File Offset: 0x000019E2
		[WebServicesDescription("WebServiceSession")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HttpSessionState Session
		{
			[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
			get
			{
				return this.Context.Session;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000029EF File Offset: 0x000019EF
		[Browsable(false)]
		[WebServicesDescription("WebServiceServer")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HttpServerUtility Server
		{
			[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
			get
			{
				return this.Context.Server;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000029FC File Offset: 0x000019FC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebServicesDescription("WebServiceUser")]
		public IPrincipal User
		{
			[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
			get
			{
				return this.Context.User;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002A0C File Offset: 0x00001A0C
		[ComVisible(false)]
		[WebServicesDescription("WebServiceSoapVersion")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SoapProtocolVersion SoapVersion
		{
			[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
			get
			{
				object obj = this.Context.Items[WebService.SoapVersionContextSlot];
				if (obj != null && obj is SoapProtocolVersion)
				{
					return (SoapProtocolVersion)obj;
				}
				return SoapProtocolVersion.Default;
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002A42 File Offset: 0x00001A42
		internal void SetContext(HttpContext context)
		{
			this.context = context;
		}

		// Token: 0x04000217 RID: 535
		private HttpContext context;

		// Token: 0x04000218 RID: 536
		internal static readonly string SoapVersionContextSlot = "WebServiceSoapVersion";
	}
}
