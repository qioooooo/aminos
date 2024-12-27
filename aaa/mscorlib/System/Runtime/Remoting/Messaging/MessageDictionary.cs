using System;
using System.Collections;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006FE RID: 1790
	internal abstract class MessageDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06004061 RID: 16481 RVA: 0x000DBD91 File Offset: 0x000DAD91
		internal MessageDictionary(string[] keys, IDictionary idict)
		{
			this._keys = keys;
			this._dict = idict;
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x000DBDA7 File Offset: 0x000DADA7
		internal bool HasUserData()
		{
			return this._dict != null && this._dict.Count > 0;
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06004063 RID: 16483 RVA: 0x000DBDC2 File Offset: 0x000DADC2
		internal IDictionary InternalDictionary
		{
			get
			{
				return this._dict;
			}
		}

		// Token: 0x06004064 RID: 16484
		internal abstract object GetMessageValue(int i);

		// Token: 0x06004065 RID: 16485
		internal abstract void SetSpecialKey(int keyNum, object value);

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06004066 RID: 16486 RVA: 0x000DBDCA File Offset: 0x000DADCA
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06004067 RID: 16487 RVA: 0x000DBDCD File Offset: 0x000DADCD
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06004068 RID: 16488 RVA: 0x000DBDD0 File Offset: 0x000DADD0
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06004069 RID: 16489 RVA: 0x000DBDD3 File Offset: 0x000DADD3
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x000DBDD6 File Offset: 0x000DADD6
		public virtual bool Contains(object key)
		{
			return this.ContainsSpecialKey(key) || (this._dict != null && this._dict.Contains(key));
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x000DBDFC File Offset: 0x000DADFC
		protected virtual bool ContainsSpecialKey(object key)
		{
			if (!(key is string))
			{
				return false;
			}
			string text = (string)key;
			for (int i = 0; i < this._keys.Length; i++)
			{
				if (text.Equals(this._keys[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x000DBE40 File Offset: 0x000DAE40
		public virtual void CopyTo(Array array, int index)
		{
			for (int i = 0; i < this._keys.Length; i++)
			{
				array.SetValue(this.GetMessageValue(i), index + i);
			}
			if (this._dict != null)
			{
				this._dict.CopyTo(array, index + this._keys.Length);
			}
		}

		// Token: 0x17000B1E RID: 2846
		public virtual object this[object key]
		{
			get
			{
				string text = key as string;
				if (text != null)
				{
					for (int i = 0; i < this._keys.Length; i++)
					{
						if (text.Equals(this._keys[i]))
						{
							return this.GetMessageValue(i);
						}
					}
					if (this._dict != null)
					{
						return this._dict[key];
					}
				}
				return null;
			}
			set
			{
				if (!this.ContainsSpecialKey(key))
				{
					if (this._dict == null)
					{
						this._dict = new Hashtable();
					}
					this._dict[key] = value;
					return;
				}
				if (key.Equals(Message.UriKey))
				{
					this.SetSpecialKey(0, value);
					return;
				}
				if (key.Equals(Message.CallContextKey))
				{
					this.SetSpecialKey(1, value);
					return;
				}
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidKey"));
			}
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x000DBF5A File Offset: 0x000DAF5A
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new MessageDictionaryEnumerator(this, this._dict);
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x000DBF68 File Offset: 0x000DAF68
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x000DBF6F File Offset: 0x000DAF6F
		public virtual void Add(object key, object value)
		{
			if (this.ContainsSpecialKey(key))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidKey"));
			}
			if (this._dict == null)
			{
				this._dict = new Hashtable();
			}
			this._dict.Add(key, value);
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x000DBFAA File Offset: 0x000DAFAA
		public virtual void Clear()
		{
			if (this._dict != null)
			{
				this._dict.Clear();
			}
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x000DBFBF File Offset: 0x000DAFBF
		public virtual void Remove(object key)
		{
			if (this.ContainsSpecialKey(key) || this._dict == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidKey"));
			}
			this._dict.Remove(key);
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06004074 RID: 16500 RVA: 0x000DBFF0 File Offset: 0x000DAFF0
		public virtual ICollection Keys
		{
			get
			{
				int num = this._keys.Length;
				ICollection collection = ((this._dict != null) ? this._dict.Keys : null);
				if (collection != null)
				{
					num += collection.Count;
				}
				ArrayList arrayList = new ArrayList(num);
				for (int i = 0; i < this._keys.Length; i++)
				{
					arrayList.Add(this._keys[i]);
				}
				if (collection != null)
				{
					arrayList.AddRange(collection);
				}
				return arrayList;
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06004075 RID: 16501 RVA: 0x000DC060 File Offset: 0x000DB060
		public virtual ICollection Values
		{
			get
			{
				int num = this._keys.Length;
				ICollection collection = ((this._dict != null) ? this._dict.Keys : null);
				if (collection != null)
				{
					num += collection.Count;
				}
				ArrayList arrayList = new ArrayList(num);
				for (int i = 0; i < this._keys.Length; i++)
				{
					arrayList.Add(this.GetMessageValue(i));
				}
				if (collection != null)
				{
					arrayList.AddRange(collection);
				}
				return arrayList;
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06004076 RID: 16502 RVA: 0x000DC0CC File Offset: 0x000DB0CC
		public virtual int Count
		{
			get
			{
				if (this._dict != null)
				{
					return this._dict.Count + this._keys.Length;
				}
				return this._keys.Length;
			}
		}

		// Token: 0x04002057 RID: 8279
		internal string[] _keys;

		// Token: 0x04002058 RID: 8280
		internal IDictionary _dict;
	}
}
