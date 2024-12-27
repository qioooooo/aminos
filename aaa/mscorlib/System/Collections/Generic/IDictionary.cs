using System;

namespace System.Collections.Generic
{
	// Token: 0x02000275 RID: 629
	public interface IDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		// Token: 0x170003D1 RID: 977
		TValue this[TKey key] { get; set; }

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x0600193A RID: 6458
		ICollection<TKey> Keys { get; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x0600193B RID: 6459
		ICollection<TValue> Values { get; }

		// Token: 0x0600193C RID: 6460
		bool ContainsKey(TKey key);

		// Token: 0x0600193D RID: 6461
		void Add(TKey key, TValue value);

		// Token: 0x0600193E RID: 6462
		bool Remove(TKey key);

		// Token: 0x0600193F RID: 6463
		bool TryGetValue(TKey key, out TValue value);
	}
}
