using System;
using System.Collections;

namespace System.Data.Design
{
	// Token: 0x02000093 RID: 147
	internal interface IDesignConnectionCollection : INamedObjectCollection, ICollection, IEnumerable
	{
		// Token: 0x0600062F RID: 1583
		IDesignConnection Get(string name);

		// Token: 0x06000630 RID: 1584
		void Set(IDesignConnection connection);

		// Token: 0x06000631 RID: 1585
		void Remove(string name);

		// Token: 0x06000632 RID: 1586
		void Clear();
	}
}
