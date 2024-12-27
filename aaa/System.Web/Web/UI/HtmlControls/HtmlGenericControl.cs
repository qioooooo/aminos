using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x02000494 RID: 1172
	[ConstructorNeedsTag(true)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlGenericControl : HtmlContainerControl
	{
		// Token: 0x060036DF RID: 14047 RVA: 0x000ECACD File Offset: 0x000EBACD
		public HtmlGenericControl()
			: this("span")
		{
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x000ECADA File Offset: 0x000EBADA
		public HtmlGenericControl(string tag)
		{
			if (tag == null)
			{
				tag = string.Empty;
			}
			this._tagName = tag;
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x060036E1 RID: 14049 RVA: 0x000ECAF3 File Offset: 0x000EBAF3
		// (set) Token: 0x060036E2 RID: 14050 RVA: 0x000ECAFB File Offset: 0x000EBAFB
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		public new string TagName
		{
			get
			{
				return this._tagName;
			}
			set
			{
				this._tagName = value;
			}
		}
	}
}
