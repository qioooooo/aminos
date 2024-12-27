using System;
using System.Security.Permissions;
using System.Web.Compilation;

namespace System.Web.UI
{
	// Token: 0x02000441 RID: 1089
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	public class PageHandlerFactory : IHttpHandlerFactory2, IHttpHandlerFactory
	{
		// Token: 0x060033FB RID: 13307 RVA: 0x000E1D1F File Offset: 0x000E0D1F
		protected internal PageHandlerFactory()
		{
			this._isInheritedInstance = base.GetType() != typeof(PageHandlerFactory);
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x000E1D42 File Offset: 0x000E0D42
		public virtual IHttpHandler GetHandler(HttpContext context, string requestType, string virtualPath, string path)
		{
			return this.GetHandlerHelper(context, requestType, VirtualPath.CreateNonRelative(virtualPath), path);
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x000E1D54 File Offset: 0x000E0D54
		IHttpHandler IHttpHandlerFactory2.GetHandler(HttpContext context, string requestType, VirtualPath virtualPath, string physicalPath)
		{
			if (this._isInheritedInstance)
			{
				return this.GetHandler(context, requestType, virtualPath.VirtualPathString, physicalPath);
			}
			return this.GetHandlerHelper(context, requestType, virtualPath, physicalPath);
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x000E1D7A File Offset: 0x000E0D7A
		public virtual void ReleaseHandler(IHttpHandler handler)
		{
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x000E1D7C File Offset: 0x000E0D7C
		private IHttpHandler GetHandlerHelper(HttpContext context, string requestType, VirtualPath virtualPath, string physicalPath)
		{
			Page page = BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(Page), context, true, true) as Page;
			if (page == null)
			{
				return null;
			}
			page.TemplateControlVirtualPath = virtualPath;
			return page;
		}

		// Token: 0x04002480 RID: 9344
		private bool _isInheritedInstance;
	}
}
