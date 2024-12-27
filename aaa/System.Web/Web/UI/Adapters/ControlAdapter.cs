using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.Adapters
{
	// Token: 0x020003BF RID: 959
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class ControlAdapter
	{
		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06002EFA RID: 12026 RVA: 0x000D2265 File Offset: 0x000D1265
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected Control Control
		{
			get
			{
				return this._control;
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06002EFB RID: 12027 RVA: 0x000D226D File Offset: 0x000D126D
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected Page Page
		{
			get
			{
				if (this.Control != null)
				{
					return this.Control.Page;
				}
				return null;
			}
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06002EFC RID: 12028 RVA: 0x000D2284 File Offset: 0x000D1284
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected PageAdapter PageAdapter
		{
			get
			{
				if (this.Control != null && this.Control.Page != null)
				{
					return this.Control.Page.PageAdapter;
				}
				return null;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06002EFD RID: 12029 RVA: 0x000D22B0 File Offset: 0x000D12B0
		protected HttpBrowserCapabilities Browser
		{
			get
			{
				if (this._browser == null)
				{
					if (this.Page.RequestInternal != null)
					{
						this._browser = this.Page.RequestInternal.Browser;
					}
					else
					{
						HttpContext httpContext = HttpContext.Current;
						if (httpContext != null && httpContext.Request != null)
						{
							this._browser = httpContext.Request.Browser;
						}
					}
				}
				return this._browser;
			}
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x000D2312 File Offset: 0x000D1312
		protected internal virtual void OnInit(EventArgs e)
		{
			this.Control.OnInit(e);
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x000D2320 File Offset: 0x000D1320
		protected internal virtual void OnLoad(EventArgs e)
		{
			this.Control.OnLoad(e);
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x000D232E File Offset: 0x000D132E
		protected internal virtual void OnPreRender(EventArgs e)
		{
			this.Control.OnPreRender(e);
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x000D233C File Offset: 0x000D133C
		protected internal virtual void Render(HtmlTextWriter writer)
		{
			if (this._control != null)
			{
				this._control.Render(writer);
			}
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x000D2352 File Offset: 0x000D1352
		protected virtual void RenderChildren(HtmlTextWriter writer)
		{
			if (this._control != null)
			{
				this._control.RenderChildren(writer);
			}
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x000D2368 File Offset: 0x000D1368
		protected internal virtual void OnUnload(EventArgs e)
		{
			this.Control.OnUnload(e);
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x000D2376 File Offset: 0x000D1376
		protected internal virtual void BeginRender(HtmlTextWriter writer)
		{
			writer.BeginRender();
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x000D237E File Offset: 0x000D137E
		protected internal virtual void CreateChildControls()
		{
			this.Control.CreateChildControls();
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x000D238B File Offset: 0x000D138B
		protected internal virtual void EndRender(HtmlTextWriter writer)
		{
			writer.EndRender();
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x000D2393 File Offset: 0x000D1393
		protected internal virtual void LoadAdapterControlState(object state)
		{
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x000D2395 File Offset: 0x000D1395
		protected internal virtual void LoadAdapterViewState(object state)
		{
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000D2397 File Offset: 0x000D1397
		protected internal virtual object SaveAdapterControlState()
		{
			return null;
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x000D239A File Offset: 0x000D139A
		protected internal virtual object SaveAdapterViewState()
		{
			return null;
		}

		// Token: 0x040021BE RID: 8638
		private HttpBrowserCapabilities _browser;

		// Token: 0x040021BF RID: 8639
		internal Control _control;
	}
}
