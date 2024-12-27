using System;
using System.Collections;

namespace System.Data.Design
{
	// Token: 0x02000075 RID: 117
	internal interface INamedObjectCollection : ICollection, IEnumerable
	{
		// Token: 0x060004FE RID: 1278
		INameService GetNameService();
	}
}
