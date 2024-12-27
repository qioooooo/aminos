using System;

namespace System.Web.UI.Design
{
	// Token: 0x02000355 RID: 853
	public interface IDataSourceDesigner
	{
		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001FF1 RID: 8177
		bool CanConfigure { get; }

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001FF2 RID: 8178
		bool CanRefreshSchema { get; }

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06001FF3 RID: 8179
		// (remove) Token: 0x06001FF4 RID: 8180
		event EventHandler DataSourceChanged;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06001FF5 RID: 8181
		// (remove) Token: 0x06001FF6 RID: 8182
		event EventHandler SchemaRefreshed;

		// Token: 0x06001FF7 RID: 8183
		void Configure();

		// Token: 0x06001FF8 RID: 8184
		DesignerDataSourceView GetView(string viewName);

		// Token: 0x06001FF9 RID: 8185
		string[] GetViewNames();

		// Token: 0x06001FFA RID: 8186
		void RefreshSchema(bool preferSilent);

		// Token: 0x06001FFB RID: 8187
		void ResumeDataSourceEvents();

		// Token: 0x06001FFC RID: 8188
		void SuppressDataSourceEvents();
	}
}
