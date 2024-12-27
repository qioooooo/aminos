using System;

namespace System.Web.UI.Design
{
	// Token: 0x0200036E RID: 878
	public interface IHierarchicalDataSourceDesigner
	{
		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x060020DE RID: 8414
		bool CanConfigure { get; }

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x060020DF RID: 8415
		bool CanRefreshSchema { get; }

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x060020E0 RID: 8416
		// (remove) Token: 0x060020E1 RID: 8417
		event EventHandler DataSourceChanged;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060020E2 RID: 8418
		// (remove) Token: 0x060020E3 RID: 8419
		event EventHandler SchemaRefreshed;

		// Token: 0x060020E4 RID: 8420
		void Configure();

		// Token: 0x060020E5 RID: 8421
		DesignerHierarchicalDataSourceView GetView(string viewPath);

		// Token: 0x060020E6 RID: 8422
		void RefreshSchema(bool preferSilent);

		// Token: 0x060020E7 RID: 8423
		void ResumeDataSourceEvents();

		// Token: 0x060020E8 RID: 8424
		void SuppressDataSourceEvents();
	}
}
