using System;
using System.Collections;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x0200013F RID: 319
	public class DataSourceDescriptorCollection : CollectionBase
	{
		// Token: 0x06000C5D RID: 3165 RVA: 0x00030724 File Offset: 0x0002F724
		public int Add(DataSourceDescriptor value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x00030732 File Offset: 0x0002F732
		public int IndexOf(DataSourceDescriptor value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00030740 File Offset: 0x0002F740
		public void Insert(int index, DataSourceDescriptor value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0003074F File Offset: 0x0002F74F
		public bool Contains(DataSourceDescriptor value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0003075D File Offset: 0x0002F75D
		public void CopyTo(DataSourceDescriptor[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0003076C File Offset: 0x0002F76C
		public void Remove(DataSourceDescriptor value)
		{
			base.List.Remove(value);
		}

		// Token: 0x170001C6 RID: 454
		public DataSourceDescriptor this[int index]
		{
			get
			{
				return (DataSourceDescriptor)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}
	}
}
