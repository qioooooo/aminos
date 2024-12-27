using System;
using System.Collections;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000024 RID: 36
	internal class DictionaryEnumeratorByKeys : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x06000103 RID: 259 RVA: 0x00005A40 File Offset: 0x00004A40
		public DictionaryEnumeratorByKeys(IDictionary properties)
		{
			this._properties = properties;
			this._keyEnum = properties.Keys.GetEnumerator();
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005A60 File Offset: 0x00004A60
		public bool MoveNext()
		{
			return this._keyEnum.MoveNext();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005A6D File Offset: 0x00004A6D
		public void Reset()
		{
			this._keyEnum.Reset();
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00005A7A File Offset: 0x00004A7A
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00005A87 File Offset: 0x00004A87
		public DictionaryEntry Entry
		{
			get
			{
				return new DictionaryEntry(this.Key, this.Value);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00005A9A File Offset: 0x00004A9A
		public object Key
		{
			get
			{
				return this._keyEnum.Current;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00005AA7 File Offset: 0x00004AA7
		public object Value
		{
			get
			{
				return this._properties[this.Key];
			}
		}

		// Token: 0x040000D4 RID: 212
		private IDictionary _properties;

		// Token: 0x040000D5 RID: 213
		private IEnumerator _keyEnum;
	}
}
