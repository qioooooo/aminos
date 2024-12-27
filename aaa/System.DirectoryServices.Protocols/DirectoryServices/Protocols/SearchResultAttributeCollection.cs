using System;
using System.Collections;
using System.Globalization;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200000C RID: 12
	public class SearchResultAttributeCollection : DictionaryBase
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00003695 File Offset: 0x00002695
		internal SearchResultAttributeCollection()
		{
		}

		// Token: 0x17000008 RID: 8
		public DirectoryAttribute this[string attributeName]
		{
			get
			{
				if (attributeName == null)
				{
					throw new ArgumentNullException("attributeName");
				}
				object obj = attributeName.ToLower(CultureInfo.InvariantCulture);
				return (DirectoryAttribute)base.InnerHashtable[obj];
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000036D8 File Offset: 0x000026D8
		public ICollection AttributeNames
		{
			get
			{
				return base.Dictionary.Keys;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000036E5 File Offset: 0x000026E5
		public ICollection Values
		{
			get
			{
				return base.Dictionary.Values;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000036F2 File Offset: 0x000026F2
		internal void Add(string name, DirectoryAttribute value)
		{
			base.Dictionary.Add(name.ToLower(CultureInfo.InvariantCulture), value);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000370C File Offset: 0x0000270C
		public bool Contains(string attributeName)
		{
			if (attributeName == null)
			{
				throw new ArgumentNullException("attributeName");
			}
			object obj = attributeName.ToLower(CultureInfo.InvariantCulture);
			return base.Dictionary.Contains(obj);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000373F File Offset: 0x0000273F
		public void CopyTo(DirectoryAttribute[] array, int index)
		{
			base.Dictionary.Values.CopyTo(array, index);
		}
	}
}
