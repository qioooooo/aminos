using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000D7 RID: 215
	public class ReadOnlyStringCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060006A7 RID: 1703 RVA: 0x00023295 File Offset: 0x00022295
		internal ReadOnlyStringCollection()
		{
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0002329D File Offset: 0x0002229D
		internal ReadOnlyStringCollection(ArrayList values)
		{
			if (values == null)
			{
				values = new ArrayList();
			}
			base.InnerList.AddRange(values);
		}

		// Token: 0x1700018D RID: 397
		public string this[int index]
		{
			get
			{
				object obj = base.InnerList[index];
				if (obj is Exception)
				{
					throw (Exception)obj;
				}
				return (string)obj;
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000232EC File Offset: 0x000222EC
		public bool Contains(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				string text = (string)base.InnerList[i];
				if (Utils.Compare(text, value) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0002333C File Offset: 0x0002233C
		public int IndexOf(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				string text = (string)base.InnerList[i];
				if (Utils.Compare(text, value) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0002338B File Offset: 0x0002238B
		public void CopyTo(string[] values, int index)
		{
			base.InnerList.CopyTo(values, index);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002339A File Offset: 0x0002239A
		internal void Add(string value)
		{
			base.InnerList.Add(value);
		}
	}
}
