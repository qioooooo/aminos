using System;
using System.ComponentModel;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000458 RID: 1112
	public class HierarchicalDataSourceIDConverter : DataSourceIDConverter
	{
		// Token: 0x060028A6 RID: 10406 RVA: 0x000DF4A7 File Offset: 0x000DE4A7
		protected override bool IsValidDataSource(IComponent component)
		{
			return component is IHierarchicalDataSource;
		}
	}
}
