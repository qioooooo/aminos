using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Data
{
	// Token: 0x0200005A RID: 90
	public class InternalDataCollectionBase : ICollection, IEnumerable
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x001D490C File Offset: 0x001D3D0C
		[Browsable(false)]
		public virtual int Count
		{
			get
			{
				return this.List.Count;
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x001D4924 File Offset: 0x001D3D24
		public virtual void CopyTo(Array ar, int index)
		{
			this.List.CopyTo(ar, index);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x001D4940 File Offset: 0x001D3D40
		public virtual IEnumerator GetEnumerator()
		{
			return this.List.GetEnumerator();
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x001D4958 File Offset: 0x001D3D58
		[Browsable(false)]
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x001D4968 File Offset: 0x001D3D68
		[Browsable(false)]
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x001D4978 File Offset: 0x001D3D78
		internal int NamesEqual(string s1, string s2, bool fCaseSensitive, CultureInfo locale)
		{
			if (fCaseSensitive)
			{
				if (string.Compare(s1, s2, false, locale) == 0)
				{
					return 1;
				}
				return 0;
			}
			else
			{
				if (locale.CompareInfo.Compare(s1, s2, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
				{
					return 0;
				}
				if (string.Compare(s1, s2, false, locale) == 0)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x001D49BC File Offset: 0x001D3DBC
		[Browsable(false)]
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x001D49CC File Offset: 0x001D3DCC
		protected virtual ArrayList List
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040006A6 RID: 1702
		internal static CollectionChangeEventArgs RefreshEventArgs = new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null);
	}
}
