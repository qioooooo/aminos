using System;
using System.Collections;

namespace System.Data
{
	// Token: 0x020000BB RID: 187
	public interface IDataParameterCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x170001D1 RID: 465
		object this[string parameterName] { get; set; }

		// Token: 0x06000C81 RID: 3201
		bool Contains(string parameterName);

		// Token: 0x06000C82 RID: 3202
		int IndexOf(string parameterName);

		// Token: 0x06000C83 RID: 3203
		void RemoveAt(string parameterName);
	}
}
