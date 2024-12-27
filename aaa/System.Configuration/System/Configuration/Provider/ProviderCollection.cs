using System;
using System.Collections;

namespace System.Configuration.Provider
{
	// Token: 0x02000088 RID: 136
	public class ProviderCollection : ICollection, IEnumerable
	{
		// Token: 0x060004FD RID: 1277 RVA: 0x00019534 File Offset: 0x00018534
		public ProviderCollection()
		{
			this._Hashtable = new Hashtable(10, StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00019550 File Offset: 0x00018550
		public virtual void Add(ProviderBase provider)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (provider.Name == null || provider.Name.Length < 1)
			{
				throw new ArgumentException(SR.GetString("Config_provider_name_null_or_empty"));
			}
			this._Hashtable.Add(provider.Name, provider);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x000195BB File Offset: 0x000185BB
		public void Remove(string name)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			this._Hashtable.Remove(name);
		}

		// Token: 0x1700016D RID: 365
		public ProviderBase this[string name]
		{
			get
			{
				return this._Hashtable[name] as ProviderBase;
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x000195F4 File Offset: 0x000185F4
		public IEnumerator GetEnumerator()
		{
			return this._Hashtable.Values.GetEnumerator();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00019606 File Offset: 0x00018606
		public void SetReadOnly()
		{
			if (this._ReadOnly)
			{
				return;
			}
			this._ReadOnly = true;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00019618 File Offset: 0x00018618
		public void Clear()
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			this._Hashtable.Clear();
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x0001963D File Offset: 0x0001863D
		public int Count
		{
			get
			{
				return this._Hashtable.Count;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x0001964A File Offset: 0x0001864A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x0001964D File Offset: 0x0001864D
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00019650 File Offset: 0x00018650
		public void CopyTo(ProviderBase[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001965A File Offset: 0x0001865A
		void ICollection.CopyTo(Array array, int index)
		{
			this._Hashtable.Values.CopyTo(array, index);
		}

		// Token: 0x0400036C RID: 876
		private Hashtable _Hashtable;

		// Token: 0x0400036D RID: 877
		private bool _ReadOnly;
	}
}
