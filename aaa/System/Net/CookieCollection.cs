using System;
using System.Collections;
using System.Runtime.Serialization;

namespace System.Net
{
	// Token: 0x02000391 RID: 913
	[Serializable]
	public class CookieCollection : ICollection, IEnumerable
	{
		// Token: 0x06001C7C RID: 7292 RVA: 0x0006BFCC File Offset: 0x0006AFCC
		public CookieCollection()
		{
			this.m_IsReadOnly = true;
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x0006BFF1 File Offset: 0x0006AFF1
		internal CookieCollection(bool IsReadOnly)
		{
			this.m_IsReadOnly = IsReadOnly;
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001C7E RID: 7294 RVA: 0x0006C016 File Offset: 0x0006B016
		public bool IsReadOnly
		{
			get
			{
				return this.m_IsReadOnly;
			}
		}

		// Token: 0x1700058A RID: 1418
		public Cookie this[int index]
		{
			get
			{
				if (index < 0 || index >= this.m_list.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return (Cookie)this.m_list[index];
			}
		}

		// Token: 0x1700058B RID: 1419
		public Cookie this[string name]
		{
			get
			{
				foreach (object obj in this.m_list)
				{
					Cookie cookie = (Cookie)obj;
					if (string.Compare(cookie.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return cookie;
					}
				}
				return null;
			}
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x0006C0B8 File Offset: 0x0006B0B8
		public void Add(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			this.m_version++;
			int num = this.IndexOf(cookie);
			if (num == -1)
			{
				this.m_list.Add(cookie);
				return;
			}
			this.m_list[num] = cookie;
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x0006C108 File Offset: 0x0006B108
		public void Add(CookieCollection cookies)
		{
			if (cookies == null)
			{
				throw new ArgumentNullException("cookies");
			}
			foreach (object obj in cookies)
			{
				Cookie cookie = (Cookie)obj;
				this.Add(cookie);
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001C83 RID: 7299 RVA: 0x0006C16C File Offset: 0x0006B16C
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001C84 RID: 7300 RVA: 0x0006C179 File Offset: 0x0006B179
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001C85 RID: 7301 RVA: 0x0006C17C File Offset: 0x0006B17C
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x0006C17F File Offset: 0x0006B17F
		public void CopyTo(Array array, int index)
		{
			this.m_list.CopyTo(array, index);
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x0006C18E File Offset: 0x0006B18E
		public void CopyTo(Cookie[] array, int index)
		{
			this.m_list.CopyTo(array, index);
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x0006C1A0 File Offset: 0x0006B1A0
		internal DateTime TimeStamp(CookieCollection.Stamp how)
		{
			switch (how)
			{
			case CookieCollection.Stamp.Set:
				this.m_TimeStamp = DateTime.Now;
				break;
			case CookieCollection.Stamp.SetToUnused:
				this.m_TimeStamp = DateTime.MinValue;
				break;
			case CookieCollection.Stamp.SetToMaxUsed:
				this.m_TimeStamp = DateTime.MaxValue;
				break;
			}
			return this.m_TimeStamp;
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001C89 RID: 7305 RVA: 0x0006C1F2 File Offset: 0x0006B1F2
		internal bool IsOtherVersionSeen
		{
			get
			{
				return this.m_has_other_versions;
			}
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x0006C1FC File Offset: 0x0006B1FC
		internal int InternalAdd(Cookie cookie, bool isStrict)
		{
			int num = 1;
			if (isStrict)
			{
				IComparer comparer = Cookie.GetComparer();
				int num2 = 0;
				foreach (object obj in this.m_list)
				{
					Cookie cookie2 = (Cookie)obj;
					if (comparer.Compare(cookie, cookie2) == 0)
					{
						num = 0;
						if (cookie2.Variant <= cookie.Variant)
						{
							this.m_list[num2] = cookie;
							break;
						}
						break;
					}
					else
					{
						num2++;
					}
				}
				if (num2 == this.m_list.Count)
				{
					this.m_list.Add(cookie);
				}
			}
			else
			{
				this.m_list.Add(cookie);
			}
			if (cookie.Version != 1)
			{
				this.m_has_other_versions = true;
			}
			return num;
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x0006C2D0 File Offset: 0x0006B2D0
		internal int IndexOf(Cookie cookie)
		{
			IComparer comparer = Cookie.GetComparer();
			int num = 0;
			foreach (object obj in this.m_list)
			{
				Cookie cookie2 = (Cookie)obj;
				if (comparer.Compare(cookie, cookie2) == 0)
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x0006C348 File Offset: 0x0006B348
		internal void RemoveAt(int idx)
		{
			this.m_list.RemoveAt(idx);
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x0006C356 File Offset: 0x0006B356
		public IEnumerator GetEnumerator()
		{
			return new CookieCollection.CookieCollectionEnumerator(this);
		}

		// Token: 0x04001D1C RID: 7452
		internal int m_version;

		// Token: 0x04001D1D RID: 7453
		private ArrayList m_list = new ArrayList();

		// Token: 0x04001D1E RID: 7454
		private DateTime m_TimeStamp = DateTime.MinValue;

		// Token: 0x04001D1F RID: 7455
		private bool m_has_other_versions;

		// Token: 0x04001D20 RID: 7456
		[OptionalField]
		private bool m_IsReadOnly;

		// Token: 0x02000392 RID: 914
		internal enum Stamp
		{
			// Token: 0x04001D22 RID: 7458
			Check,
			// Token: 0x04001D23 RID: 7459
			Set,
			// Token: 0x04001D24 RID: 7460
			SetToUnused,
			// Token: 0x04001D25 RID: 7461
			SetToMaxUsed
		}

		// Token: 0x02000393 RID: 915
		private class CookieCollectionEnumerator : IEnumerator
		{
			// Token: 0x06001C8E RID: 7310 RVA: 0x0006C35E File Offset: 0x0006B35E
			internal CookieCollectionEnumerator(CookieCollection cookies)
			{
				this.m_cookies = cookies;
				this.m_count = cookies.Count;
				this.m_version = cookies.m_version;
			}

			// Token: 0x17000590 RID: 1424
			// (get) Token: 0x06001C8F RID: 7311 RVA: 0x0006C38C File Offset: 0x0006B38C
			object IEnumerator.Current
			{
				get
				{
					if (this.m_index < 0 || this.m_index >= this.m_count)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					if (this.m_version != this.m_cookies.m_version)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
					}
					return this.m_cookies[this.m_index];
				}
			}

			// Token: 0x06001C90 RID: 7312 RVA: 0x0006C3F4 File Offset: 0x0006B3F4
			bool IEnumerator.MoveNext()
			{
				if (this.m_version != this.m_cookies.m_version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				if (++this.m_index < this.m_count)
				{
					return true;
				}
				this.m_index = this.m_count;
				return false;
			}

			// Token: 0x06001C91 RID: 7313 RVA: 0x0006C44C File Offset: 0x0006B44C
			void IEnumerator.Reset()
			{
				this.m_index = -1;
			}

			// Token: 0x04001D26 RID: 7462
			private CookieCollection m_cookies;

			// Token: 0x04001D27 RID: 7463
			private int m_count;

			// Token: 0x04001D28 RID: 7464
			private int m_index = -1;

			// Token: 0x04001D29 RID: 7465
			private int m_version;
		}
	}
}
