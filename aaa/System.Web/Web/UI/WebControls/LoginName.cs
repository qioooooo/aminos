using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005D7 RID: 1495
	[DefaultProperty("FormatString")]
	[Designer("System.Web.UI.Design.WebControls.LoginNameDesigner,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Bindable(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class LoginName : WebControl
	{
		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x06004932 RID: 18738 RVA: 0x0012A714 File Offset: 0x00129714
		// (set) Token: 0x06004933 RID: 18739 RVA: 0x0012A741 File Offset: 0x00129741
		[WebSysDescription("LoginName_FormatString")]
		[WebCategory("Appearance")]
		[DefaultValue("{0}")]
		[Localizable(true)]
		public virtual string FormatString
		{
			get
			{
				object obj = this.ViewState["FormatString"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "{0}";
			}
			set
			{
				this.ViewState["FormatString"] = value;
			}
		}

		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x06004934 RID: 18740 RVA: 0x0012A754 File Offset: 0x00129754
		internal string UserName
		{
			get
			{
				if (base.DesignMode)
				{
					return SR.GetString("LoginName_DesignModeUserName");
				}
				return LoginUtil.GetUserName(this);
			}
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x0012A76F File Offset: 0x0012976F
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.UserName))
			{
				base.Render(writer);
			}
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x0012A785 File Offset: 0x00129785
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.UserName))
			{
				base.RenderBeginTag(writer);
			}
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x0012A79B File Offset: 0x0012979B
		public override void RenderEndTag(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.UserName))
			{
				base.RenderEndTag(writer);
			}
		}

		// Token: 0x06004938 RID: 18744 RVA: 0x0012A7B4 File Offset: 0x001297B4
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			string text = this.UserName;
			if (!string.IsNullOrEmpty(text))
			{
				text = HttpUtility.HtmlEncode(text);
				string formatString = this.FormatString;
				if (formatString.Length == 0)
				{
					writer.Write(text);
					return;
				}
				try
				{
					writer.Write(string.Format(CultureInfo.CurrentCulture, formatString, new object[] { text }));
				}
				catch (FormatException ex)
				{
					throw new FormatException(SR.GetString("LoginName_InvalidFormatString"), ex);
				}
			}
		}

		// Token: 0x04002B1D RID: 11037
		private const string _defaultFormatString = "{0}";
	}
}
