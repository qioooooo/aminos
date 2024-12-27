using System;
using System.Security.Permissions;

namespace System.Web.Profile
{
	// Token: 0x02000305 RID: 773
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ProfileGroupBase
	{
		// Token: 0x170007FC RID: 2044
		public object this[string propertyName]
		{
			get
			{
				return this._Parent[this._MyName + propertyName];
			}
			set
			{
				this._Parent[this._MyName + propertyName] = value;
			}
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000A4995 File Offset: 0x000A3995
		public object GetPropertyValue(string propertyName)
		{
			return this._Parent[this._MyName + propertyName];
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x000A49AE File Offset: 0x000A39AE
		public void SetPropertyValue(string propertyName, object propertyValue)
		{
			this._Parent[this._MyName + propertyName] = propertyValue;
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x000A49C8 File Offset: 0x000A39C8
		public ProfileGroupBase()
		{
			this._Parent = null;
			this._MyName = null;
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x000A49DE File Offset: 0x000A39DE
		public void Init(ProfileBase parent, string myName)
		{
			if (this._Parent == null)
			{
				this._Parent = parent;
				this._MyName = myName + ".";
			}
		}

		// Token: 0x04001DB0 RID: 7600
		private string _MyName;

		// Token: 0x04001DB1 RID: 7601
		private ProfileBase _Parent;
	}
}
