using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003B6 RID: 950
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IParserAccessor
	{
		// Token: 0x06002E45 RID: 11845
		void AddParsedSubObject(object obj);
	}
}
