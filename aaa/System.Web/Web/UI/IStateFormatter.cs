using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000415 RID: 1045
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IStateFormatter
	{
		// Token: 0x060032C1 RID: 12993
		object Deserialize(string serializedState);

		// Token: 0x060032C2 RID: 12994
		string Serialize(object state);
	}
}
