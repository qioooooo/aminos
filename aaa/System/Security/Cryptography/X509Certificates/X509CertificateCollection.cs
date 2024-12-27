using System;
using System.Collections;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000329 RID: 809
	[Serializable]
	public class X509CertificateCollection : CollectionBase
	{
		// Token: 0x0600197E RID: 6526 RVA: 0x00057E84 File Offset: 0x00056E84
		public X509CertificateCollection()
		{
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x00057E8C File Offset: 0x00056E8C
		public X509CertificateCollection(X509CertificateCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x00057E9B File Offset: 0x00056E9B
		public X509CertificateCollection(X509Certificate[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x170004E3 RID: 1251
		public X509Certificate this[int index]
		{
			get
			{
				return (X509Certificate)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x00057ECC File Offset: 0x00056ECC
		public int Add(X509Certificate value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x00057EDC File Offset: 0x00056EDC
		public void AddRange(X509Certificate[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x00057F10 File Offset: 0x00056F10
		public void AddRange(X509CertificateCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x00057F4C File Offset: 0x00056F4C
		public bool Contains(X509Certificate value)
		{
			foreach (object obj in base.List)
			{
				X509Certificate x509Certificate = (X509Certificate)obj;
				if (x509Certificate.Equals(value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x00057FB0 File Offset: 0x00056FB0
		public void CopyTo(X509Certificate[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x00057FBF File Offset: 0x00056FBF
		public int IndexOf(X509Certificate value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x00057FCD File Offset: 0x00056FCD
		public void Insert(int index, X509Certificate value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x00057FDC File Offset: 0x00056FDC
		public new X509CertificateCollection.X509CertificateEnumerator GetEnumerator()
		{
			return new X509CertificateCollection.X509CertificateEnumerator(this);
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x00057FE4 File Offset: 0x00056FE4
		public void Remove(X509Certificate value)
		{
			base.List.Remove(value);
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x00057FF4 File Offset: 0x00056FF4
		public override int GetHashCode()
		{
			int num = 0;
			foreach (X509Certificate x509Certificate in this)
			{
				num += x509Certificate.GetHashCode();
			}
			return num;
		}

		// Token: 0x0200032A RID: 810
		public class X509CertificateEnumerator : IEnumerator
		{
			// Token: 0x0600198D RID: 6541 RVA: 0x00058048 File Offset: 0x00057048
			public X509CertificateEnumerator(X509CertificateCollection mappings)
			{
				this.temp = mappings;
				this.baseEnumerator = this.temp.GetEnumerator();
			}

			// Token: 0x170004E4 RID: 1252
			// (get) Token: 0x0600198E RID: 6542 RVA: 0x00058068 File Offset: 0x00057068
			public X509Certificate Current
			{
				get
				{
					return (X509Certificate)this.baseEnumerator.Current;
				}
			}

			// Token: 0x170004E5 RID: 1253
			// (get) Token: 0x0600198F RID: 6543 RVA: 0x0005807A File Offset: 0x0005707A
			object IEnumerator.Current
			{
				get
				{
					return this.baseEnumerator.Current;
				}
			}

			// Token: 0x06001990 RID: 6544 RVA: 0x00058087 File Offset: 0x00057087
			public bool MoveNext()
			{
				return this.baseEnumerator.MoveNext();
			}

			// Token: 0x06001991 RID: 6545 RVA: 0x00058094 File Offset: 0x00057094
			bool IEnumerator.MoveNext()
			{
				return this.baseEnumerator.MoveNext();
			}

			// Token: 0x06001992 RID: 6546 RVA: 0x000580A1 File Offset: 0x000570A1
			public void Reset()
			{
				this.baseEnumerator.Reset();
			}

			// Token: 0x06001993 RID: 6547 RVA: 0x000580AE File Offset: 0x000570AE
			void IEnumerator.Reset()
			{
				this.baseEnumerator.Reset();
			}

			// Token: 0x04001ABA RID: 6842
			private IEnumerator baseEnumerator;

			// Token: 0x04001ABB RID: 6843
			private IEnumerable temp;
		}
	}
}
