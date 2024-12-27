using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x0200002F RID: 47
	public class ConfigurationLocationCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000257 RID: 599 RVA: 0x0000F64A File Offset: 0x0000E64A
		internal ConfigurationLocationCollection(ICollection col)
		{
			base.InnerList.AddRange(col);
		}

		// Token: 0x1700008D RID: 141
		public ConfigurationLocation this[int index]
		{
			get
			{
				return (ConfigurationLocation)base.InnerList[index];
			}
		}
	}
}
