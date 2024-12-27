using System;

namespace System.Web.UI.Design
{
	// Token: 0x02000376 RID: 886
	public interface IDataBindingSchemaProvider
	{
		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x0600211A RID: 8474
		bool CanRefreshSchema { get; }

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x0600211B RID: 8475
		IDataSourceViewSchema Schema { get; }

		// Token: 0x0600211C RID: 8476
		void RefreshSchema(bool preferSilent);
	}
}
