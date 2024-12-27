using System;
using System.ComponentModel;
using System.Data;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200050B RID: 1291
	[DefaultProperty("CookieName")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CookieParameter : Parameter
	{
		// Token: 0x06003EFC RID: 16124 RVA: 0x00105DCB File Offset: 0x00104DCB
		public CookieParameter()
		{
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x00105DD3 File Offset: 0x00104DD3
		public CookieParameter(string name, string cookieName)
			: base(name)
		{
			this.CookieName = cookieName;
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x00105DE3 File Offset: 0x00104DE3
		public CookieParameter(string name, DbType dbType, string cookieName)
			: base(name, dbType)
		{
			this.CookieName = cookieName;
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x00105DF4 File Offset: 0x00104DF4
		public CookieParameter(string name, TypeCode type, string cookieName)
			: base(name, type)
		{
			this.CookieName = cookieName;
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x00105E05 File Offset: 0x00104E05
		protected CookieParameter(CookieParameter original)
			: base(original)
		{
			this.CookieName = original.CookieName;
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06003F01 RID: 16129 RVA: 0x00105E1C File Offset: 0x00104E1C
		// (set) Token: 0x06003F02 RID: 16130 RVA: 0x00105E49 File Offset: 0x00104E49
		[DefaultValue("")]
		[WebSysDescription("CookieParameter_CookieName")]
		[WebCategory("Parameter")]
		public string CookieName
		{
			get
			{
				object obj = base.ViewState["CookieName"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				if (this.CookieName != value)
				{
					base.ViewState["CookieName"] = value;
					base.OnParameterChanged();
				}
			}
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x00105E70 File Offset: 0x00104E70
		protected override Parameter Clone()
		{
			return new CookieParameter(this);
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x00105E78 File Offset: 0x00104E78
		protected override object Evaluate(HttpContext context, Control control)
		{
			if (context == null || context.Request == null)
			{
				return null;
			}
			HttpCookie httpCookie = context.Request.Cookies[this.CookieName];
			if (httpCookie == null)
			{
				return null;
			}
			return httpCookie.Value;
		}
	}
}
