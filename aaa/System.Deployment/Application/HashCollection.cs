using System;
using System.Collections;
using System.Deployment.Internal.Isolation.Manifest;

namespace System.Deployment.Application
{
	// Token: 0x02000031 RID: 49
	internal class HashCollection : IEnumerable
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x0000BA24 File Offset: 0x0000AA24
		public void AddHash(byte[] digestValue, CMS_HASH_DIGESTMETHOD digestMethod, CMS_HASH_TRANSFORM transform)
		{
			Hash hash = new Hash(digestValue, digestMethod, transform);
			this._hashes.Add(hash);
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000BA47 File Offset: 0x0000AA47
		public int Count
		{
			get
			{
				return this._hashes.Count;
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000BA54 File Offset: 0x0000AA54
		public HashCollection.HashEnumerator GetEnumerator()
		{
			return new HashCollection.HashEnumerator(this);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000BA5C File Offset: 0x0000AA5C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400010F RID: 271
		protected ArrayList _hashes = new ArrayList();

		// Token: 0x02000032 RID: 50
		public class HashEnumerator : IEnumerator
		{
			// Token: 0x060001AD RID: 429 RVA: 0x0000BA64 File Offset: 0x0000AA64
			public HashEnumerator(HashCollection hashCollection)
			{
				this._hashCollection = hashCollection;
				this._index = -1;
			}

			// Token: 0x060001AE RID: 430 RVA: 0x0000BA7A File Offset: 0x0000AA7A
			public void Reset()
			{
				this._index = -1;
			}

			// Token: 0x060001AF RID: 431 RVA: 0x0000BA83 File Offset: 0x0000AA83
			public bool MoveNext()
			{
				this._index++;
				return this._index < this._hashCollection._hashes.Count;
			}

			// Token: 0x17000094 RID: 148
			// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000BAAB File Offset: 0x0000AAAB
			public Hash Current
			{
				get
				{
					return (Hash)this._hashCollection._hashes[this._index];
				}
			}

			// Token: 0x17000095 RID: 149
			// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000BAC8 File Offset: 0x0000AAC8
			object IEnumerator.Current
			{
				get
				{
					return this._hashCollection._hashes[this._index];
				}
			}

			// Token: 0x04000110 RID: 272
			private int _index;

			// Token: 0x04000111 RID: 273
			private HashCollection _hashCollection;
		}
	}
}
