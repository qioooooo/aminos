using System;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000744 RID: 1860
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebPartUserCapability
	{
		// Token: 0x06005A42 RID: 23106 RVA: 0x0016C418 File Offset: 0x0016B418
		public WebPartUserCapability(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("name");
			}
			this._name = name;
		}

		// Token: 0x17001751 RID: 5969
		// (get) Token: 0x06005A43 RID: 23107 RVA: 0x0016C43A File Offset: 0x0016B43A
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x06005A44 RID: 23108 RVA: 0x0016C444 File Offset: 0x0016B444
		public override bool Equals(object o)
		{
			if (o == this)
			{
				return true;
			}
			WebPartUserCapability webPartUserCapability = o as WebPartUserCapability;
			return webPartUserCapability != null && webPartUserCapability.Name == this.Name;
		}

		// Token: 0x06005A45 RID: 23109 RVA: 0x0016C474 File Offset: 0x0016B474
		public override int GetHashCode()
		{
			return this._name.GetHashCode();
		}

		// Token: 0x04003084 RID: 12420
		private string _name;
	}
}
