using System;
using System.Collections;
using System.EnterpriseServices.Admin;

namespace System.EnterpriseServices
{
	// Token: 0x020000A0 RID: 160
	internal interface IConfigCallback
	{
		// Token: 0x060003CD RID: 973
		object FindObject(ICatalogCollection coll, object key);

		// Token: 0x060003CE RID: 974
		void ConfigureDefaults(object a, object key);

		// Token: 0x060003CF RID: 975
		bool Configure(object a, object key);

		// Token: 0x060003D0 RID: 976
		bool AfterSaveChanges(object a, object key);

		// Token: 0x060003D1 RID: 977
		void ConfigureSubCollections(ICatalogCollection coll);

		// Token: 0x060003D2 RID: 978
		IEnumerator GetEnumerator();
	}
}
