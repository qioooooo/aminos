using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x020004A4 RID: 1188
	[SupportsEventValidation]
	[DefaultEvent("")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlInputReset : HtmlInputButton
	{
		// Token: 0x06003787 RID: 14215 RVA: 0x000EE243 File Offset: 0x000ED243
		public HtmlInputReset()
			: base("reset")
		{
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x000EE250 File Offset: 0x000ED250
		public HtmlInputReset(string type)
			: base(type)
		{
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x06003789 RID: 14217 RVA: 0x000EE259 File Offset: 0x000ED259
		// (set) Token: 0x0600378A RID: 14218 RVA: 0x000EE261 File Offset: 0x000ED261
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override bool CausesValidation
		{
			get
			{
				return base.CausesValidation;
			}
			set
			{
				base.CausesValidation = value;
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x0600378B RID: 14219 RVA: 0x000EE26A File Offset: 0x000ED26A
		// (set) Token: 0x0600378C RID: 14220 RVA: 0x000EE272 File Offset: 0x000ED272
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string ValidationGroup
		{
			get
			{
				return base.ValidationGroup;
			}
			set
			{
				base.ValidationGroup = value;
			}
		}

		// Token: 0x1400004D RID: 77
		// (add) Token: 0x0600378D RID: 14221 RVA: 0x000EE27B File Offset: 0x000ED27B
		// (remove) Token: 0x0600378E RID: 14222 RVA: 0x000EE284 File Offset: 0x000ED284
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler ServerClick
		{
			add
			{
				base.ServerClick += value;
			}
			remove
			{
				base.ServerClick -= value;
			}
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x000EE28D File Offset: 0x000ED28D
		internal override void RenderAttributesInternal(HtmlTextWriter writer)
		{
		}
	}
}
