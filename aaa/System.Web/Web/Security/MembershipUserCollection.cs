using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.Security
{
	// Token: 0x02000343 RID: 835
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class MembershipUserCollection : ICollection, IEnumerable
	{
		// Token: 0x060028A2 RID: 10402 RVA: 0x000B273D File Offset: 0x000B173D
		public MembershipUserCollection()
		{
			this._Indices = new Hashtable(10, StringComparer.CurrentCultureIgnoreCase);
			this._Values = new ArrayList();
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x000B2762 File Offset: 0x000B1762
		private MembershipUserCollection(Hashtable indices, ArrayList values)
		{
			this._Indices = (Hashtable)indices.Clone();
			this._Values = (ArrayList)values.Clone();
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000B278C File Offset: 0x000B178C
		public void Add(MembershipUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			int num = this._Values.Add(user);
			try
			{
				this._Indices.Add(user.UserName, num);
			}
			catch
			{
				this._Values.RemoveAt(num);
				throw;
			}
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000B27FC File Offset: 0x000B17FC
		public void Remove(string name)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			object obj = this._Indices[name];
			if (obj == null || !(obj is int))
			{
				return;
			}
			int num = (int)obj;
			if (num >= this._Values.Count)
			{
				return;
			}
			this._Values.RemoveAt(num);
			this._Indices.Remove(name);
			ArrayList arrayList = new ArrayList();
			foreach (object obj2 in this._Indices)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
				if ((int)dictionaryEntry.Value > num)
				{
					arrayList.Add(dictionaryEntry.Key);
				}
			}
			foreach (object obj3 in arrayList)
			{
				string text = (string)obj3;
				this._Indices[text] = (int)this._Indices[text] - 1;
			}
		}

		// Token: 0x17000899 RID: 2201
		public MembershipUser this[string name]
		{
			get
			{
				object obj = this._Indices[name];
				if (obj == null || !(obj is int))
				{
					return null;
				}
				int num = (int)obj;
				if (num >= this._Values.Count)
				{
					return null;
				}
				return (MembershipUser)this._Values[num];
			}
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x000B2987 File Offset: 0x000B1987
		public IEnumerator GetEnumerator()
		{
			return this._Values.GetEnumerator();
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000B2994 File Offset: 0x000B1994
		public void SetReadOnly()
		{
			if (this._ReadOnly)
			{
				return;
			}
			this._ReadOnly = true;
			this._Values = ArrayList.ReadOnly(this._Values);
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000B29B7 File Offset: 0x000B19B7
		public void Clear()
		{
			this._Values.Clear();
			this._Indices.Clear();
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060028AA RID: 10410 RVA: 0x000B29CF File Offset: 0x000B19CF
		public int Count
		{
			get
			{
				return this._Values.Count;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060028AB RID: 10411 RVA: 0x000B29DC File Offset: 0x000B19DC
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x000B29DF File Offset: 0x000B19DF
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000B29E2 File Offset: 0x000B19E2
		void ICollection.CopyTo(Array array, int index)
		{
			this._Values.CopyTo(array, index);
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000B29F1 File Offset: 0x000B19F1
		public void CopyTo(MembershipUser[] array, int index)
		{
			this._Values.CopyTo(array, index);
		}

		// Token: 0x04001ECC RID: 7884
		private Hashtable _Indices;

		// Token: 0x04001ECD RID: 7885
		private ArrayList _Values;

		// Token: 0x04001ECE RID: 7886
		private bool _ReadOnly;
	}
}
