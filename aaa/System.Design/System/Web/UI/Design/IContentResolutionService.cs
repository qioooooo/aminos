using System;
using System.Collections;

namespace System.Web.UI.Design
{
	// Token: 0x02000373 RID: 883
	public interface IContentResolutionService
	{
		// Token: 0x06002105 RID: 8453
		ContentDesignerState GetContentDesignerState(string identifier);

		// Token: 0x06002106 RID: 8454
		void SetContentDesignerState(string identifier, ContentDesignerState state);

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06002107 RID: 8455
		IDictionary ContentDefinitions { get; }
	}
}
