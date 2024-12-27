using System;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200019B RID: 411
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public class AdapterDictionary : OrderedDictionary
	{
		// Token: 0x1700042F RID: 1071
		public string this[string key]
		{
			get
			{
				return (string)base[key];
			}
			set
			{
				base[key] = value;
			}
		}
	}
}
