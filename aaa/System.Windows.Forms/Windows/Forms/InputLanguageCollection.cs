using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x0200044D RID: 1101
	public class InputLanguageCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06004195 RID: 16789 RVA: 0x000EB128 File Offset: 0x000EA128
		internal InputLanguageCollection(InputLanguage[] value)
		{
			base.InnerList.AddRange(value);
		}

		// Token: 0x17000CB9 RID: 3257
		public InputLanguage this[int index]
		{
			get
			{
				return (InputLanguage)base.InnerList[index];
			}
		}

		// Token: 0x06004197 RID: 16791 RVA: 0x000EB14F File Offset: 0x000EA14F
		public bool Contains(InputLanguage value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x06004198 RID: 16792 RVA: 0x000EB15D File Offset: 0x000EA15D
		public void CopyTo(InputLanguage[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x000EB16C File Offset: 0x000EA16C
		public int IndexOf(InputLanguage value)
		{
			return base.InnerList.IndexOf(value);
		}
	}
}
