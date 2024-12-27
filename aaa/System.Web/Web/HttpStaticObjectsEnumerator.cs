using System;
using System.Collections;

namespace System.Web
{
	// Token: 0x02000094 RID: 148
	internal class HttpStaticObjectsEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x060007BD RID: 1981 RVA: 0x00022C40 File Offset: 0x00021C40
		internal HttpStaticObjectsEnumerator(IDictionaryEnumerator e)
		{
			this._enum = e;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00022C4F File Offset: 0x00021C4F
		public void Reset()
		{
			this._enum.Reset();
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00022C5C File Offset: 0x00021C5C
		public bool MoveNext()
		{
			return this._enum.MoveNext();
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00022C69 File Offset: 0x00021C69
		public object Key
		{
			get
			{
				return this._enum.Key;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x00022C78 File Offset: 0x00021C78
		public object Value
		{
			get
			{
				HttpStaticObjectsEntry httpStaticObjectsEntry = (HttpStaticObjectsEntry)this._enum.Value;
				if (httpStaticObjectsEntry == null)
				{
					return null;
				}
				return httpStaticObjectsEntry.Instance;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00022CA1 File Offset: 0x00021CA1
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00022CAE File Offset: 0x00021CAE
		public DictionaryEntry Entry
		{
			get
			{
				return new DictionaryEntry(this._enum.Key, this.Value);
			}
		}

		// Token: 0x04001165 RID: 4453
		private IDictionaryEnumerator _enum;
	}
}
