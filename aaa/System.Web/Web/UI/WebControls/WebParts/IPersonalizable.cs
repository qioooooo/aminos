using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006CB RID: 1739
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IPersonalizable
	{
		// Token: 0x17001607 RID: 5639
		// (get) Token: 0x0600559B RID: 21915
		bool IsDirty { get; }

		// Token: 0x0600559C RID: 21916
		void Load(PersonalizationDictionary state);

		// Token: 0x0600559D RID: 21917
		void Save(PersonalizationDictionary state);
	}
}
