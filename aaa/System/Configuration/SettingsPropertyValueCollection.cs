using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x02000718 RID: 1816
	public class SettingsPropertyValueCollection : ICloneable, ICollection, IEnumerable
	{
		// Token: 0x06003795 RID: 14229 RVA: 0x000EBA40 File Offset: 0x000EAA40
		public SettingsPropertyValueCollection()
		{
			this._Indices = new Hashtable(10, CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
			this._Values = new ArrayList();
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x000EBA6C File Offset: 0x000EAA6C
		public void Add(SettingsPropertyValue property)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			int num = this._Values.Add(property);
			try
			{
				this._Indices.Add(property.Name, num);
			}
			catch (Exception)
			{
				this._Values.RemoveAt(num);
				throw;
			}
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x000EBACC File Offset: 0x000EAACC
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

		// Token: 0x17000CE3 RID: 3299
		public SettingsPropertyValue this[string name]
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
				return (SettingsPropertyValue)this._Values[num];
			}
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x000EBC57 File Offset: 0x000EAC57
		public IEnumerator GetEnumerator()
		{
			return this._Values.GetEnumerator();
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x000EBC64 File Offset: 0x000EAC64
		public object Clone()
		{
			return new SettingsPropertyValueCollection(this._Indices, this._Values);
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x000EBC77 File Offset: 0x000EAC77
		public void SetReadOnly()
		{
			if (this._ReadOnly)
			{
				return;
			}
			this._ReadOnly = true;
			this._Values = ArrayList.ReadOnly(this._Values);
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x000EBC9A File Offset: 0x000EAC9A
		public void Clear()
		{
			this._Values.Clear();
			this._Indices.Clear();
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x0600379D RID: 14237 RVA: 0x000EBCB2 File Offset: 0x000EACB2
		public int Count
		{
			get
			{
				return this._Values.Count;
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x0600379E RID: 14238 RVA: 0x000EBCBF File Offset: 0x000EACBF
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x0600379F RID: 14239 RVA: 0x000EBCC2 File Offset: 0x000EACC2
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x000EBCC5 File Offset: 0x000EACC5
		public void CopyTo(Array array, int index)
		{
			this._Values.CopyTo(array, index);
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x000EBCD4 File Offset: 0x000EACD4
		private SettingsPropertyValueCollection(Hashtable indices, ArrayList values)
		{
			this._Indices = (Hashtable)indices.Clone();
			this._Values = (ArrayList)values.Clone();
		}

		// Token: 0x040031D1 RID: 12753
		private Hashtable _Indices;

		// Token: 0x040031D2 RID: 12754
		private ArrayList _Values;

		// Token: 0x040031D3 RID: 12755
		private bool _ReadOnly;
	}
}
