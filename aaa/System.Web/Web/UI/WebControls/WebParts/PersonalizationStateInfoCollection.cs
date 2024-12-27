using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006E5 RID: 1765
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class PersonalizationStateInfoCollection : ICollection, IEnumerable
	{
		// Token: 0x0600568A RID: 22154 RVA: 0x0015D4AF File Offset: 0x0015C4AF
		public PersonalizationStateInfoCollection()
		{
			this._indices = new Dictionary<PersonalizationStateInfoCollection.Key, int>(PersonalizationStateInfoCollection.KeyComparer.Default);
			this._values = new ArrayList();
		}

		// Token: 0x17001655 RID: 5717
		// (get) Token: 0x0600568B RID: 22155 RVA: 0x0015D4D2 File Offset: 0x0015C4D2
		public int Count
		{
			get
			{
				return this._values.Count;
			}
		}

		// Token: 0x17001656 RID: 5718
		public PersonalizationStateInfo this[string path, string username]
		{
			get
			{
				if (path == null)
				{
					throw new ArgumentNullException("path");
				}
				PersonalizationStateInfoCollection.Key key = new PersonalizationStateInfoCollection.Key(path, username);
				int num;
				if (!this._indices.TryGetValue(key, out num))
				{
					return null;
				}
				return (PersonalizationStateInfo)this._values[num];
			}
		}

		// Token: 0x17001657 RID: 5719
		public PersonalizationStateInfo this[int index]
		{
			get
			{
				return (PersonalizationStateInfo)this._values[index];
			}
		}

		// Token: 0x0600568E RID: 22158 RVA: 0x0015D53C File Offset: 0x0015C53C
		public void Add(PersonalizationStateInfo data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			UserPersonalizationStateInfo userPersonalizationStateInfo = data as UserPersonalizationStateInfo;
			PersonalizationStateInfoCollection.Key key;
			if (userPersonalizationStateInfo != null)
			{
				key = new PersonalizationStateInfoCollection.Key(userPersonalizationStateInfo.Path, userPersonalizationStateInfo.Username);
			}
			else
			{
				key = new PersonalizationStateInfoCollection.Key(data.Path, null);
			}
			if (!this._indices.ContainsKey(key))
			{
				int num = this._values.Add(data);
				try
				{
					this._indices.Add(key, num);
				}
				catch
				{
					this._values.RemoveAt(num);
					throw;
				}
				return;
			}
			if (userPersonalizationStateInfo != null)
			{
				throw new ArgumentException(SR.GetString("PersonalizationStateInfoCollection_CouldNotAddUserStateInfo", new object[] { key.Path, key.Username }));
			}
			throw new ArgumentException(SR.GetString("PersonalizationStateInfoCollection_CouldNotAddSharedStateInfo", new object[] { key.Path }));
		}

		// Token: 0x0600568F RID: 22159 RVA: 0x0015D61C File Offset: 0x0015C61C
		public void Clear()
		{
			this._values.Clear();
			this._indices.Clear();
		}

		// Token: 0x06005690 RID: 22160 RVA: 0x0015D634 File Offset: 0x0015C634
		public void CopyTo(PersonalizationStateInfo[] array, int index)
		{
			this._values.CopyTo(array, index);
		}

		// Token: 0x06005691 RID: 22161 RVA: 0x0015D643 File Offset: 0x0015C643
		public IEnumerator GetEnumerator()
		{
			return this._values.GetEnumerator();
		}

		// Token: 0x17001658 RID: 5720
		// (get) Token: 0x06005692 RID: 22162 RVA: 0x0015D650 File Offset: 0x0015C650
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005693 RID: 22163 RVA: 0x0015D654 File Offset: 0x0015C654
		public void Remove(string path, string username)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			PersonalizationStateInfoCollection.Key key = new PersonalizationStateInfoCollection.Key(path, username);
			int num;
			if (!this._indices.TryGetValue(key, out num))
			{
				return;
			}
			this._indices.Remove(key);
			try
			{
				this._values.RemoveAt(num);
			}
			catch
			{
				this._indices.Add(key, num);
				throw;
			}
			ArrayList arrayList = new ArrayList();
			foreach (KeyValuePair<PersonalizationStateInfoCollection.Key, int> keyValuePair in this._indices)
			{
				if (keyValuePair.Value > num)
				{
					arrayList.Add(keyValuePair.Key);
				}
			}
			foreach (object obj in arrayList)
			{
				PersonalizationStateInfoCollection.Key key2 = (PersonalizationStateInfoCollection.Key)obj;
				this._indices[key2] = this._indices[key2] - 1;
			}
		}

		// Token: 0x06005694 RID: 22164 RVA: 0x0015D780 File Offset: 0x0015C780
		public void SetReadOnly()
		{
			if (this._readOnly)
			{
				return;
			}
			this._readOnly = true;
			this._values = ArrayList.ReadOnly(this._values);
		}

		// Token: 0x17001659 RID: 5721
		// (get) Token: 0x06005695 RID: 22165 RVA: 0x0015D7A3 File Offset: 0x0015C7A3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06005696 RID: 22166 RVA: 0x0015D7A6 File Offset: 0x0015C7A6
		void ICollection.CopyTo(Array array, int index)
		{
			this._values.CopyTo(array, index);
		}

		// Token: 0x04002F64 RID: 12132
		private Dictionary<PersonalizationStateInfoCollection.Key, int> _indices;

		// Token: 0x04002F65 RID: 12133
		private bool _readOnly;

		// Token: 0x04002F66 RID: 12134
		private ArrayList _values;

		// Token: 0x020006E6 RID: 1766
		[Serializable]
		private sealed class Key
		{
			// Token: 0x06005697 RID: 22167 RVA: 0x0015D7B5 File Offset: 0x0015C7B5
			internal Key(string path, string username)
			{
				this.Path = path;
				this.Username = username;
			}

			// Token: 0x04002F67 RID: 12135
			public string Path;

			// Token: 0x04002F68 RID: 12136
			public string Username;
		}

		// Token: 0x020006E7 RID: 1767
		[Serializable]
		private sealed class KeyComparer : IEqualityComparer<PersonalizationStateInfoCollection.Key>
		{
			// Token: 0x06005698 RID: 22168 RVA: 0x0015D7CB File Offset: 0x0015C7CB
			bool IEqualityComparer<PersonalizationStateInfoCollection.Key>.Equals(PersonalizationStateInfoCollection.Key x, PersonalizationStateInfoCollection.Key y)
			{
				return this.Compare(x, y) == 0;
			}

			// Token: 0x06005699 RID: 22169 RVA: 0x0015D7D8 File Offset: 0x0015C7D8
			int IEqualityComparer<PersonalizationStateInfoCollection.Key>.GetHashCode(PersonalizationStateInfoCollection.Key key)
			{
				if (key == null)
				{
					return 0;
				}
				int hashCode = key.Path.ToLowerInvariant().GetHashCode();
				int num = 0;
				if (key.Username != null)
				{
					num = key.Username.ToLowerInvariant().GetHashCode();
				}
				return HashCodeCombiner.CombineHashCodes(hashCode, num);
			}

			// Token: 0x0600569A RID: 22170 RVA: 0x0015D820 File Offset: 0x0015C820
			private int Compare(PersonalizationStateInfoCollection.Key x, PersonalizationStateInfoCollection.Key y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					return -1;
				}
				if (y == null)
				{
					return 1;
				}
				int num = string.Compare(x.Path, y.Path, StringComparison.OrdinalIgnoreCase);
				if (num != 0)
				{
					return num;
				}
				return string.Compare(x.Username, y.Username, StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x04002F69 RID: 12137
			internal static readonly IEqualityComparer<PersonalizationStateInfoCollection.Key> Default = new PersonalizationStateInfoCollection.KeyComparer();
		}
	}
}
