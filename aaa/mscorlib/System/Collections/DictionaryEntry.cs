using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x0200024C RID: 588
	[ComVisible(true)]
	[Serializable]
	public struct DictionaryEntry
	{
		// Token: 0x06001799 RID: 6041 RVA: 0x0003CE73 File Offset: 0x0003BE73
		public DictionaryEntry(object key, object value)
		{
			this._key = key;
			this._value = value;
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x0600179A RID: 6042 RVA: 0x0003CE83 File Offset: 0x0003BE83
		// (set) Token: 0x0600179B RID: 6043 RVA: 0x0003CE8B File Offset: 0x0003BE8B
		public object Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x0003CE94 File Offset: 0x0003BE94
		// (set) Token: 0x0600179D RID: 6045 RVA: 0x0003CE9C File Offset: 0x0003BE9C
		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x0400093E RID: 2366
		private object _key;

		// Token: 0x0400093F RID: 2367
		private object _value;
	}
}
