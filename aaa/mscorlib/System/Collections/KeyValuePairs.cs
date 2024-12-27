using System;
using System.Diagnostics;

namespace System.Collections
{
	// Token: 0x02000258 RID: 600
	[DebuggerDisplay("{value}", Name = "[{key}]", Type = "")]
	internal class KeyValuePairs
	{
		// Token: 0x06001822 RID: 6178 RVA: 0x0003EE41 File Offset: 0x0003DE41
		public KeyValuePairs(object key, object value)
		{
			this.value = value;
			this.key = key;
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06001823 RID: 6179 RVA: 0x0003EE57 File Offset: 0x0003DE57
		public object Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001824 RID: 6180 RVA: 0x0003EE5F File Offset: 0x0003DE5F
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x04000966 RID: 2406
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object key;

		// Token: 0x04000967 RID: 2407
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object value;
	}
}
