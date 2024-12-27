using System;
using System.Collections;

namespace System.Web.UI.Design
{
	// Token: 0x02000377 RID: 887
	public interface IDataSourceProvider
	{
		// Token: 0x0600211D RID: 8477
		object GetSelectedDataSource();

		// Token: 0x0600211E RID: 8478
		IEnumerable GetResolvedSelectedDataSource();
	}
}
