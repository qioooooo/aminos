using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x0200024A RID: 586
	[ComVisible(true)]
	public interface IDictionary : ICollection, IEnumerable
	{
		// Token: 0x17000345 RID: 837
		object this[object key] { get; set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06001773 RID: 6003
		ICollection Keys { get; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06001774 RID: 6004
		ICollection Values { get; }

		// Token: 0x06001775 RID: 6005
		bool Contains(object key);

		// Token: 0x06001776 RID: 6006
		void Add(object key, object value);

		// Token: 0x06001777 RID: 6007
		void Clear();

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06001778 RID: 6008
		bool IsReadOnly { get; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06001779 RID: 6009
		bool IsFixedSize { get; }

		// Token: 0x0600177A RID: 6010
		IDictionaryEnumerator GetEnumerator();

		// Token: 0x0600177B RID: 6011
		void Remove(object key);
	}
}
