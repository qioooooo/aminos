using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x0200000A RID: 10
	[ComVisible(true)]
	public interface IList : ICollection, IEnumerable
	{
		// Token: 0x17000004 RID: 4
		object this[int index] { get; set; }

		// Token: 0x06000018 RID: 24
		int Add(object value);

		// Token: 0x06000019 RID: 25
		bool Contains(object value);

		// Token: 0x0600001A RID: 26
		void Clear();

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001B RID: 27
		bool IsReadOnly { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001C RID: 28
		bool IsFixedSize { get; }

		// Token: 0x0600001D RID: 29
		int IndexOf(object value);

		// Token: 0x0600001E RID: 30
		void Insert(int index, object value);

		// Token: 0x0600001F RID: 31
		void Remove(object value);

		// Token: 0x06000020 RID: 32
		void RemoveAt(int index);
	}
}
