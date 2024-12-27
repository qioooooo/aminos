using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006DE RID: 1758
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class PersonalizationDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06005647 RID: 22087 RVA: 0x0015C806 File Offset: 0x0015B806
		public PersonalizationDictionary()
		{
			this._dictionary = new HybridDictionary(true);
		}

		// Token: 0x06005648 RID: 22088 RVA: 0x0015C81A File Offset: 0x0015B81A
		public PersonalizationDictionary(int initialSize)
		{
			this._dictionary = new HybridDictionary(initialSize, true);
		}

		// Token: 0x17001644 RID: 5700
		// (get) Token: 0x06005649 RID: 22089 RVA: 0x0015C82F File Offset: 0x0015B82F
		public virtual int Count
		{
			get
			{
				return this._dictionary.Count;
			}
		}

		// Token: 0x17001645 RID: 5701
		// (get) Token: 0x0600564A RID: 22090 RVA: 0x0015C83C File Offset: 0x0015B83C
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001646 RID: 5702
		// (get) Token: 0x0600564B RID: 22091 RVA: 0x0015C83F File Offset: 0x0015B83F
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001647 RID: 5703
		// (get) Token: 0x0600564C RID: 22092 RVA: 0x0015C842 File Offset: 0x0015B842
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001648 RID: 5704
		// (get) Token: 0x0600564D RID: 22093 RVA: 0x0015C845 File Offset: 0x0015B845
		public virtual ICollection Keys
		{
			get
			{
				return this._dictionary.Keys;
			}
		}

		// Token: 0x17001649 RID: 5705
		// (get) Token: 0x0600564E RID: 22094 RVA: 0x0015C852 File Offset: 0x0015B852
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700164A RID: 5706
		// (get) Token: 0x0600564F RID: 22095 RVA: 0x0015C855 File Offset: 0x0015B855
		public virtual ICollection Values
		{
			get
			{
				return this._dictionary.Values;
			}
		}

		// Token: 0x1700164B RID: 5707
		public virtual PersonalizationEntry this[string key]
		{
			get
			{
				key = StringUtil.CheckAndTrimString(key, "key");
				return (PersonalizationEntry)this._dictionary[key];
			}
			set
			{
				key = StringUtil.CheckAndTrimString(key, "key");
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._dictionary[key] = value;
			}
		}

		// Token: 0x06005652 RID: 22098 RVA: 0x0015C8AC File Offset: 0x0015B8AC
		public virtual void Add(string key, PersonalizationEntry value)
		{
			key = StringUtil.CheckAndTrimString(key, "key");
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this._dictionary.Add(key, value);
		}

		// Token: 0x06005653 RID: 22099 RVA: 0x0015C8D6 File Offset: 0x0015B8D6
		public virtual void Clear()
		{
			this._dictionary.Clear();
		}

		// Token: 0x06005654 RID: 22100 RVA: 0x0015C8E3 File Offset: 0x0015B8E3
		public virtual bool Contains(string key)
		{
			key = StringUtil.CheckAndTrimString(key, "key");
			return this._dictionary.Contains(key);
		}

		// Token: 0x06005655 RID: 22101 RVA: 0x0015C8FE File Offset: 0x0015B8FE
		public virtual void CopyTo(DictionaryEntry[] array, int index)
		{
			this._dictionary.CopyTo(array, index);
		}

		// Token: 0x06005656 RID: 22102 RVA: 0x0015C90D File Offset: 0x0015B90D
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return this._dictionary.GetEnumerator();
		}

		// Token: 0x06005657 RID: 22103 RVA: 0x0015C91A File Offset: 0x0015B91A
		public virtual void Remove(string key)
		{
			key = StringUtil.CheckAndTrimString(key, "key");
			this._dictionary.Remove(key);
		}

		// Token: 0x06005658 RID: 22104 RVA: 0x0015C938 File Offset: 0x0015B938
		internal void RemoveSharedProperties()
		{
			DictionaryEntry[] array = new DictionaryEntry[this.Count];
			this.CopyTo(array, 0);
			foreach (DictionaryEntry dictionaryEntry in array)
			{
				if (((PersonalizationEntry)dictionaryEntry.Value).Scope == PersonalizationScope.Shared)
				{
					this.Remove((string)dictionaryEntry.Key);
				}
			}
		}

		// Token: 0x1700164C RID: 5708
		object IDictionary.this[object key]
		{
			get
			{
				if (!(key is string))
				{
					throw new ArgumentException(SR.GetString("PersonalizationDictionary_MustBeTypeString"), "key");
				}
				return this[(string)key];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!(key is string))
				{
					throw new ArgumentException(SR.GetString("PersonalizationDictionary_MustBeTypeString"), "key");
				}
				if (!(value is PersonalizationEntry))
				{
					throw new ArgumentException(SR.GetString("PersonalizationDictionary_MustBeTypePersonalizationEntry"), "value");
				}
				this[(string)key] = (PersonalizationEntry)value;
			}
		}

		// Token: 0x0600565B RID: 22107 RVA: 0x0015CA30 File Offset: 0x0015BA30
		void IDictionary.Add(object key, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(key is string))
			{
				throw new ArgumentException(SR.GetString("PersonalizationDictionary_MustBeTypeString"), "key");
			}
			if (!(value is PersonalizationEntry))
			{
				throw new ArgumentException(SR.GetString("PersonalizationDictionary_MustBeTypePersonalizationEntry"), "value");
			}
			this.Add((string)key, (PersonalizationEntry)value);
		}

		// Token: 0x0600565C RID: 22108 RVA: 0x0015CA97 File Offset: 0x0015BA97
		bool IDictionary.Contains(object key)
		{
			if (!(key is string))
			{
				throw new ArgumentException(SR.GetString("PersonalizationDictionary_MustBeTypeString"), "key");
			}
			return this.Contains((string)key);
		}

		// Token: 0x0600565D RID: 22109 RVA: 0x0015CAC2 File Offset: 0x0015BAC2
		void IDictionary.Remove(object key)
		{
			if (!(key is string))
			{
				throw new ArgumentException(SR.GetString("PersonalizationDictionary_MustBeTypeString"), "key");
			}
			this.Remove((string)key);
		}

		// Token: 0x0600565E RID: 22110 RVA: 0x0015CAED File Offset: 0x0015BAED
		void ICollection.CopyTo(Array array, int index)
		{
			if (!(array is DictionaryEntry[]))
			{
				throw new ArgumentException(SR.GetString("PersonalizationDictionary_MustBeTypeDictionaryEntryArray"), "array");
			}
			this.CopyTo((DictionaryEntry[])array, index);
		}

		// Token: 0x0600565F RID: 22111 RVA: 0x0015CB19 File Offset: 0x0015BB19
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04002F56 RID: 12118
		private HybridDictionary _dictionary;
	}
}
