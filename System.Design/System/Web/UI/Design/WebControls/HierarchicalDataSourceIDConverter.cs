using System;
using System.ComponentModel;

namespace System.Web.UI.Design.WebControls
{
	public class HierarchicalDataSourceIDConverter : DataSourceIDConverter
	{
		protected override bool IsValidDataSource(IComponent component)
		{
			return component is IHierarchicalDataSource;
		}
	}
}
