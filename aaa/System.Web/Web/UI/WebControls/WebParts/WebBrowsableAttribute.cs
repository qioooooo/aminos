using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000701 RID: 1793
	[AttributeUsage(AttributeTargets.Property)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebBrowsableAttribute : Attribute
	{
		// Token: 0x06005774 RID: 22388 RVA: 0x00160F43 File Offset: 0x0015FF43
		public WebBrowsableAttribute()
			: this(true)
		{
		}

		// Token: 0x06005775 RID: 22389 RVA: 0x00160F4C File Offset: 0x0015FF4C
		public WebBrowsableAttribute(bool browsable)
		{
			this._browsable = browsable;
		}

		// Token: 0x1700168E RID: 5774
		// (get) Token: 0x06005776 RID: 22390 RVA: 0x00160F5B File Offset: 0x0015FF5B
		public bool Browsable
		{
			get
			{
				return this._browsable;
			}
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x00160F64 File Offset: 0x0015FF64
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			WebBrowsableAttribute webBrowsableAttribute = obj as WebBrowsableAttribute;
			return webBrowsableAttribute != null && webBrowsableAttribute.Browsable == this.Browsable;
		}

		// Token: 0x06005778 RID: 22392 RVA: 0x00160F91 File Offset: 0x0015FF91
		public override int GetHashCode()
		{
			return this._browsable.GetHashCode();
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x00160F9E File Offset: 0x0015FF9E
		public override bool IsDefaultAttribute()
		{
			return this.Equals(WebBrowsableAttribute.Default);
		}

		// Token: 0x04002F9F RID: 12191
		public static readonly WebBrowsableAttribute Yes = new WebBrowsableAttribute(true);

		// Token: 0x04002FA0 RID: 12192
		public static readonly WebBrowsableAttribute No = new WebBrowsableAttribute(false);

		// Token: 0x04002FA1 RID: 12193
		public static readonly WebBrowsableAttribute Default = WebBrowsableAttribute.No;

		// Token: 0x04002FA2 RID: 12194
		private bool _browsable;
	}
}
