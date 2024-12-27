using System;
using System.Collections;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x02000141 RID: 321
	public class DataSourceGroupCollection : CollectionBase
	{
		// Token: 0x06000C6B RID: 3179 RVA: 0x000307AC File Offset: 0x0002F7AC
		public int Add(DataSourceGroup value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x000307BA File Offset: 0x0002F7BA
		public int IndexOf(DataSourceGroup value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x000307C8 File Offset: 0x0002F7C8
		public void Insert(int index, DataSourceGroup value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x000307D7 File Offset: 0x0002F7D7
		public bool Contains(DataSourceGroup value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x000307E5 File Offset: 0x0002F7E5
		public void CopyTo(DataSourceGroup[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x000307F4 File Offset: 0x0002F7F4
		public void Remove(DataSourceGroup value)
		{
			base.List.Remove(value);
		}

		// Token: 0x170001CB RID: 459
		public DataSourceGroup this[int index]
		{
			get
			{
				return (DataSourceGroup)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}
	}
}
