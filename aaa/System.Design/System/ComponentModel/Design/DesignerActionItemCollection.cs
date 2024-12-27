using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x02000210 RID: 528
	public class DesignerActionItemCollection : CollectionBase
	{
		// Token: 0x17000337 RID: 823
		public DesignerActionItem this[int index]
		{
			get
			{
				return (DesignerActionItem)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x000651C0 File Offset: 0x000641C0
		public int Add(DesignerActionItem value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x000651DB File Offset: 0x000641DB
		public bool Contains(DesignerActionItem value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x000651E9 File Offset: 0x000641E9
		public void CopyTo(DesignerActionItem[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x000651F8 File Offset: 0x000641F8
		public int IndexOf(DesignerActionItem value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x00065206 File Offset: 0x00064206
		public void Insert(int index, DesignerActionItem value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00065215 File Offset: 0x00064215
		public void Remove(DesignerActionItem value)
		{
			base.List.Remove(value);
		}
	}
}
