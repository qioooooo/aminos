using System;
using System.Collections;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006DE RID: 1758
	internal class DictionaryEnumeratorByKeys : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x06003F2A RID: 16170 RVA: 0x000D852D File Offset: 0x000D752D
		public DictionaryEnumeratorByKeys(IDictionary properties)
		{
			this._properties = properties;
			this._keyEnum = properties.Keys.GetEnumerator();
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x000D854D File Offset: 0x000D754D
		public bool MoveNext()
		{
			return this._keyEnum.MoveNext();
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x000D855A File Offset: 0x000D755A
		public void Reset()
		{
			this._keyEnum.Reset();
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06003F2D RID: 16173 RVA: 0x000D8567 File Offset: 0x000D7567
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06003F2E RID: 16174 RVA: 0x000D8574 File Offset: 0x000D7574
		public DictionaryEntry Entry
		{
			get
			{
				return new DictionaryEntry(this.Key, this.Value);
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06003F2F RID: 16175 RVA: 0x000D8587 File Offset: 0x000D7587
		public object Key
		{
			get
			{
				return this._keyEnum.Current;
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06003F30 RID: 16176 RVA: 0x000D8594 File Offset: 0x000D7594
		public object Value
		{
			get
			{
				return this._properties[this.Key];
			}
		}

		// Token: 0x04001FCF RID: 8143
		private IDictionary _properties;

		// Token: 0x04001FD0 RID: 8144
		private IEnumerator _keyEnum;
	}
}
