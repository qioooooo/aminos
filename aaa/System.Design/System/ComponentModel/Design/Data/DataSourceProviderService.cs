using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x02000142 RID: 322
	[Guid("ABE5C1F0-C96E-40c4-A22D-4A5CEC899BDC")]
	public abstract class DataSourceProviderService
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000C73 RID: 3187
		public abstract bool SupportsAddNewDataSource { get; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000C74 RID: 3188
		public abstract bool SupportsConfigureDataSource { get; }

		// Token: 0x06000C75 RID: 3189
		public abstract DataSourceGroupCollection GetDataSources();

		// Token: 0x06000C76 RID: 3190
		public abstract DataSourceGroup InvokeAddNewDataSource(IWin32Window parentWindow, FormStartPosition startPosition);

		// Token: 0x06000C77 RID: 3191
		public abstract bool InvokeConfigureDataSource(IWin32Window parentWindow, FormStartPosition startPosition, DataSourceDescriptor dataSourceDescriptor);

		// Token: 0x06000C78 RID: 3192
		public abstract object AddDataSourceInstance(IDesignerHost host, DataSourceDescriptor dataSourceDescriptor);

		// Token: 0x06000C79 RID: 3193
		public abstract void NotifyDataSourceComponentAdded(object dsc);
	}
}
