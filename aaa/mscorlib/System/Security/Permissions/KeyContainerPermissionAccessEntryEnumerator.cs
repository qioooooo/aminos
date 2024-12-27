using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200064A RID: 1610
	[ComVisible(true)]
	[Serializable]
	public sealed class KeyContainerPermissionAccessEntryEnumerator : IEnumerator
	{
		// Token: 0x06003A91 RID: 14993 RVA: 0x000C68AA File Offset: 0x000C58AA
		private KeyContainerPermissionAccessEntryEnumerator()
		{
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x000C68B2 File Offset: 0x000C58B2
		internal KeyContainerPermissionAccessEntryEnumerator(KeyContainerPermissionAccessEntryCollection entries)
		{
			this.m_entries = entries;
			this.m_current = -1;
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06003A93 RID: 14995 RVA: 0x000C68C8 File Offset: 0x000C58C8
		public KeyContainerPermissionAccessEntry Current
		{
			get
			{
				return this.m_entries[this.m_current];
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06003A94 RID: 14996 RVA: 0x000C68DB File Offset: 0x000C58DB
		object IEnumerator.Current
		{
			get
			{
				return this.m_entries[this.m_current];
			}
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x000C68EE File Offset: 0x000C58EE
		public bool MoveNext()
		{
			if (this.m_current == this.m_entries.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x000C6916 File Offset: 0x000C5916
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04001E31 RID: 7729
		private KeyContainerPermissionAccessEntryCollection m_entries;

		// Token: 0x04001E32 RID: 7730
		private int m_current;
	}
}
