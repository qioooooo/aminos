using System;
using System.Collections;

namespace System.DirectoryServices
{
	// Token: 0x02000038 RID: 56
	public class ResultPropertyValueCollection : ReadOnlyCollectionBase
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00006E45 File Offset: 0x00005E45
		internal ResultPropertyValueCollection(object[] values)
		{
			if (values == null)
			{
				values = new object[0];
			}
			base.InnerList.AddRange(values);
		}

		// Token: 0x17000069 RID: 105
		public object this[int index]
		{
			get
			{
				object obj = base.InnerList[index];
				if (obj is Exception)
				{
					throw (Exception)obj;
				}
				return obj;
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006E8E File Offset: 0x00005E8E
		public bool Contains(object value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00006E9C File Offset: 0x00005E9C
		public int IndexOf(object value)
		{
			return base.InnerList.IndexOf(value);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00006EAA File Offset: 0x00005EAA
		public void CopyTo(object[] values, int index)
		{
			base.InnerList.CopyTo(values, index);
		}
	}
}
