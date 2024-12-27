using System;

namespace System.Collections.Specialized
{
	// Token: 0x02000250 RID: 592
	public interface IOrderedDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x1700042D RID: 1069
		object this[int index] { get; set; }

		// Token: 0x06001458 RID: 5208
		IDictionaryEnumerator GetEnumerator();

		// Token: 0x06001459 RID: 5209
		void Insert(int index, object key, object value);

		// Token: 0x0600145A RID: 5210
		void RemoveAt(int index);
	}
}
