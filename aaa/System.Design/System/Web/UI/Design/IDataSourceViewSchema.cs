using System;

namespace System.Web.UI.Design
{
	// Token: 0x0200031F RID: 799
	public interface IDataSourceViewSchema
	{
		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001E1F RID: 7711
		string Name { get; }

		// Token: 0x06001E20 RID: 7712
		IDataSourceViewSchema[] GetChildren();

		// Token: 0x06001E21 RID: 7713
		IDataSourceFieldSchema[] GetFields();
	}
}
