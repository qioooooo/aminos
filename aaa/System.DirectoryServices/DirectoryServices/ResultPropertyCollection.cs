using System;
using System.Collections;
using System.Globalization;

namespace System.DirectoryServices
{
	// Token: 0x02000037 RID: 55
	public class ResultPropertyCollection : DictionaryBase
	{
		// Token: 0x06000183 RID: 387 RVA: 0x00006D8B File Offset: 0x00005D8B
		internal ResultPropertyCollection()
		{
		}

		// Token: 0x17000066 RID: 102
		public ResultPropertyValueCollection this[string name]
		{
			get
			{
				object obj = name.ToLower(CultureInfo.InvariantCulture);
				if (this.Contains((string)obj))
				{
					return (ResultPropertyValueCollection)base.InnerHashtable[obj];
				}
				return new ResultPropertyValueCollection(new object[0]);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00006DD8 File Offset: 0x00005DD8
		public ICollection PropertyNames
		{
			get
			{
				return base.Dictionary.Keys;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00006DE5 File Offset: 0x00005DE5
		public ICollection Values
		{
			get
			{
				return base.Dictionary.Values;
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006DF2 File Offset: 0x00005DF2
		internal void Add(string name, ResultPropertyValueCollection value)
		{
			base.Dictionary.Add(name.ToLower(CultureInfo.InvariantCulture), value);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00006E0C File Offset: 0x00005E0C
		public bool Contains(string propertyName)
		{
			object obj = propertyName.ToLower(CultureInfo.InvariantCulture);
			return base.Dictionary.Contains(obj);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00006E31 File Offset: 0x00005E31
		public void CopyTo(ResultPropertyValueCollection[] array, int index)
		{
			base.Dictionary.Values.CopyTo(array, index);
		}
	}
}
