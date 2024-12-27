using System;
using System.Collections;

namespace System.Web.Caching
{
	// Token: 0x02000102 RID: 258
	internal class AggregateEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x06000C0E RID: 3086 RVA: 0x0002FF49 File Offset: 0x0002EF49
		internal AggregateEnumerator(IDictionaryEnumerator[] enumerators)
		{
			this._enumerators = enumerators;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0002FF58 File Offset: 0x0002EF58
		public bool MoveNext()
		{
			bool flag;
			for (;;)
			{
				flag = this._enumerators[this._iCurrent].MoveNext();
				if (flag || this._iCurrent == this._enumerators.Length - 1)
				{
					break;
				}
				this._iCurrent++;
			}
			return flag;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0002FFA0 File Offset: 0x0002EFA0
		public void Reset()
		{
			for (int i = 0; i <= this._iCurrent; i++)
			{
				this._enumerators[i].Reset();
			}
			this._iCurrent = 0;
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x0002FFD2 File Offset: 0x0002EFD2
		public object Current
		{
			get
			{
				return this._enumerators[this._iCurrent].Current;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x0002FFE6 File Offset: 0x0002EFE6
		public object Key
		{
			get
			{
				return this._enumerators[this._iCurrent].Key;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x0002FFFA File Offset: 0x0002EFFA
		public object Value
		{
			get
			{
				return this._enumerators[this._iCurrent].Value;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x0003000E File Offset: 0x0002F00E
		public DictionaryEntry Entry
		{
			get
			{
				return this._enumerators[this._iCurrent].Entry;
			}
		}

		// Token: 0x040013F5 RID: 5109
		private IDictionaryEnumerator[] _enumerators;

		// Token: 0x040013F6 RID: 5110
		private int _iCurrent;
	}
}
