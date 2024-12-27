using System;
using System.Security.Permissions;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x0200016D RID: 365
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ExpressionBuilderContext
	{
		// Token: 0x06001058 RID: 4184 RVA: 0x00048E58 File Offset: 0x00047E58
		internal ExpressionBuilderContext(VirtualPath virtualPath)
		{
			this._virtualPath = virtualPath;
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00048E67 File Offset: 0x00047E67
		public ExpressionBuilderContext(string virtualPath)
		{
			this._virtualPath = global::System.Web.VirtualPath.Create(virtualPath);
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00048E7B File Offset: 0x00047E7B
		public ExpressionBuilderContext(TemplateControl templateControl)
		{
			this._templateControl = templateControl;
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x00048E8A File Offset: 0x00047E8A
		public TemplateControl TemplateControl
		{
			get
			{
				return this._templateControl;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x00048E92 File Offset: 0x00047E92
		public string VirtualPath
		{
			get
			{
				if (this._virtualPath == null && this._templateControl != null)
				{
					return this._templateControl.AppRelativeVirtualPath;
				}
				return global::System.Web.VirtualPath.GetVirtualPathString(this._virtualPath);
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x00048EC1 File Offset: 0x00047EC1
		internal VirtualPath VirtualPathObject
		{
			get
			{
				if (this._virtualPath == null && this._templateControl != null)
				{
					return this._templateControl.VirtualPath;
				}
				return this._virtualPath;
			}
		}

		// Token: 0x0400164E RID: 5710
		private TemplateControl _templateControl;

		// Token: 0x0400164F RID: 5711
		private VirtualPath _virtualPath;
	}
}
