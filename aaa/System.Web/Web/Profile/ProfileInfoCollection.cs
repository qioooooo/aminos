using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.Profile
{
	// Token: 0x0200030A RID: 778
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class ProfileInfoCollection : ICollection, IEnumerable
	{
		// Token: 0x0600265E RID: 9822 RVA: 0x000A4ACB File Offset: 0x000A3ACB
		public ProfileInfoCollection()
		{
			this._Hashtable = new Hashtable(10, StringComparer.CurrentCultureIgnoreCase);
			this._ArrayList = new ArrayList();
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x000A4AF0 File Offset: 0x000A3AF0
		public void Add(ProfileInfo profileInfo)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			if (profileInfo == null || profileInfo.UserName == null)
			{
				throw new ArgumentNullException("profileInfo");
			}
			this._Hashtable.Add(profileInfo.UserName, this._CurPos);
			this._ArrayList.Add(profileInfo);
			this._CurPos++;
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x000A4B58 File Offset: 0x000A3B58
		public void Remove(string name)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			object obj = this._Hashtable[name];
			if (obj == null)
			{
				return;
			}
			this._Hashtable.Remove(name);
			this._ArrayList[(int)obj] = null;
			this._NumBlanks++;
		}

		// Token: 0x17000804 RID: 2052
		public ProfileInfo this[string name]
		{
			get
			{
				object obj = this._Hashtable[name];
				if (obj == null)
				{
					return null;
				}
				return this._ArrayList[(int)obj] as ProfileInfo;
			}
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x000A4BE5 File Offset: 0x000A3BE5
		public IEnumerator GetEnumerator()
		{
			this.DoCompact();
			return this._ArrayList.GetEnumerator();
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x000A4BF8 File Offset: 0x000A3BF8
		public void SetReadOnly()
		{
			if (this._ReadOnly)
			{
				return;
			}
			this._ReadOnly = true;
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x000A4C0A File Offset: 0x000A3C0A
		public void Clear()
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			this._Hashtable.Clear();
			this._ArrayList.Clear();
			this._CurPos = 0;
			this._NumBlanks = 0;
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002665 RID: 9829 RVA: 0x000A4C3E File Offset: 0x000A3C3E
		public int Count
		{
			get
			{
				return this._Hashtable.Count;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002666 RID: 9830 RVA: 0x000A4C4B File Offset: 0x000A3C4B
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002667 RID: 9831 RVA: 0x000A4C4E File Offset: 0x000A3C4E
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x000A4C51 File Offset: 0x000A3C51
		public void CopyTo(Array array, int index)
		{
			this.DoCompact();
			this._ArrayList.CopyTo(array, index);
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x000A4C66 File Offset: 0x000A3C66
		public void CopyTo(ProfileInfo[] array, int index)
		{
			this.DoCompact();
			this._ArrayList.CopyTo(array, index);
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x000A4C7C File Offset: 0x000A3C7C
		private void DoCompact()
		{
			if (this._NumBlanks < 1)
			{
				return;
			}
			ArrayList arrayList = new ArrayList(this._CurPos - this._NumBlanks);
			int num = -1;
			for (int i = 0; i < this._CurPos; i++)
			{
				if (this._ArrayList[i] != null)
				{
					arrayList.Add(this._ArrayList[i]);
				}
				else if (num == -1)
				{
					num = i;
				}
			}
			this._NumBlanks = 0;
			this._ArrayList = arrayList;
			this._CurPos = this._ArrayList.Count;
			for (int j = num; j < this._CurPos; j++)
			{
				ProfileInfo profileInfo = this._ArrayList[j] as ProfileInfo;
				this._Hashtable[profileInfo.UserName] = j;
			}
		}

		// Token: 0x04001DBD RID: 7613
		private Hashtable _Hashtable;

		// Token: 0x04001DBE RID: 7614
		private ArrayList _ArrayList;

		// Token: 0x04001DBF RID: 7615
		private bool _ReadOnly;

		// Token: 0x04001DC0 RID: 7616
		private int _CurPos;

		// Token: 0x04001DC1 RID: 7617
		private int _NumBlanks;
	}
}
