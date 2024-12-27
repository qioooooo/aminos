using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Collections.Specialized
{
	// Token: 0x02000261 RID: 609
	[DesignerSerializer("System.Diagnostics.Design.StringDictionaryCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Serializable]
	public class StringDictionary : IEnumerable
	{
		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x00045CD8 File Offset: 0x00044CD8
		public virtual int Count
		{
			get
			{
				return this.contents.Count;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001526 RID: 5414 RVA: 0x00045CE5 File Offset: 0x00044CE5
		public virtual bool IsSynchronized
		{
			get
			{
				return this.contents.IsSynchronized;
			}
		}

		// Token: 0x1700046D RID: 1133
		public virtual string this[string key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				return (string)this.contents[key.ToLower(CultureInfo.InvariantCulture)];
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				this.contents[key.ToLower(CultureInfo.InvariantCulture)] = value;
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x00045D44 File Offset: 0x00044D44
		public virtual ICollection Keys
		{
			get
			{
				return this.contents.Keys;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x00045D51 File Offset: 0x00044D51
		public virtual object SyncRoot
		{
			get
			{
				return this.contents.SyncRoot;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600152B RID: 5419 RVA: 0x00045D5E File Offset: 0x00044D5E
		public virtual ICollection Values
		{
			get
			{
				return this.contents.Values;
			}
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x00045D6B File Offset: 0x00044D6B
		public virtual void Add(string key, string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.contents.Add(key.ToLower(CultureInfo.InvariantCulture), value);
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x00045D92 File Offset: 0x00044D92
		public virtual void Clear()
		{
			this.contents.Clear();
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x00045D9F File Offset: 0x00044D9F
		public virtual bool ContainsKey(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this.contents.ContainsKey(key.ToLower(CultureInfo.InvariantCulture));
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x00045DC5 File Offset: 0x00044DC5
		public virtual bool ContainsValue(string value)
		{
			return this.contents.ContainsValue(value);
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x00045DD3 File Offset: 0x00044DD3
		public virtual void CopyTo(Array array, int index)
		{
			this.contents.CopyTo(array, index);
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x00045DE2 File Offset: 0x00044DE2
		public virtual IEnumerator GetEnumerator()
		{
			return this.contents.GetEnumerator();
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x00045DEF File Offset: 0x00044DEF
		public virtual void Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.contents.Remove(key.ToLower(CultureInfo.InvariantCulture));
		}

		// Token: 0x040011BB RID: 4539
		internal Hashtable contents = new Hashtable();
	}
}
