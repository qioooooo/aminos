using System;

namespace System.Web.UI.Design
{
	public interface IHierarchicalDataSourceDesigner
	{
		bool CanConfigure { get; }

		bool CanRefreshSchema { get; }

		event EventHandler DataSourceChanged;

		event EventHandler SchemaRefreshed;

		void Configure();

		DesignerHierarchicalDataSourceView GetView(string viewPath);

		void RefreshSchema(bool preferSilent);

		void ResumeDataSourceEvents();

		void SuppressDataSourceEvents();
	}
}
